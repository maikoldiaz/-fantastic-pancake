// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Blockchain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Core.Dto;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Blockchain.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Azure.Entities;
    using Nethereum.Hex.HexTypes;
    using Newtonsoft.Json;

    /// <summary>
    /// The Blockchain Base.
    /// </summary>
    public abstract class BlockchainService : IBlockchainService
    {
        /// <summary>
        /// The azureclient factory.
        /// </summary>
        private readonly IAzureClientFactory azureclientFactory;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger logger;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// The contract address.
        /// </summary>
        private string contractFactoryAddress;

        /// <summary>
        /// The contract abi.
        /// </summary>
        private string contractFactoryAbi;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockchainService" /> class.
        /// </summary>
        /// <param name="azureclientFactory">The azureclient factory.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="telemetry">The telemetry.</param>
        protected BlockchainService(IAzureClientFactory azureclientFactory, ITrueLogger logger, ITelemetry telemetry)
        {
            this.azureclientFactory = azureclientFactory;
            this.logger = logger;
            this.telemetry = telemetry;
        }

        /// <summary>
        /// Gets the service type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public abstract ServiceType Type { get; }

        /// <summary>
        /// Gets the event identifier.
        /// </summary>
        /// <returns>The event id.</returns>
        protected static string EventId => Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

        /// <inheritdoc/>
        public virtual void Initialize(BlockchainSettings blockchainConfiguration)
        {
            ArgumentValidators.ThrowIfNull(blockchainConfiguration, nameof(blockchainConfiguration));
            ArgumentValidators.ThrowIfNullOrEmpty(blockchainConfiguration.ContractFactoryAbi, nameof(blockchainConfiguration.ContractFactoryAbi));
            ArgumentValidators.ThrowIfNullOrEmpty(blockchainConfiguration.ContractFactoryContractAddress, nameof(blockchainConfiguration.ContractFactoryContractAddress));

            this.contractFactoryAddress = blockchainConfiguration.ContractFactoryContractAddress;
            this.contractFactoryAbi = blockchainConfiguration.ContractFactoryAbi;
        }

        /// <inheritdoc/>
        public abstract Task RegisterAsync(int entityId);

        /// <summary>
        /// Registers the failure asynchronous.
        /// </summary>
        /// <param name="pendingTransaction">The pending transaction.</param>
        /// <param name="fileRegistrationTransaction">The file registration transaction.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        protected static void RegisterFailure(PendingTransaction pendingTransaction, FileRegistrationTransaction fileRegistrationTransaction, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            ArgumentValidators.ThrowIfNull(fileRegistrationTransaction, nameof(fileRegistrationTransaction));
            unitOfWork.CreateRepository<PendingTransaction>().Insert(pendingTransaction);
            fileRegistrationTransaction.StatusTypeId = StatusType.FAILED;
        }

        /// <summary>
        /// Do call master contract async.
        /// </summary>
        /// <param name="contractName">Type of the contractName.</param>
        /// <returns>Returns a task.</returns>
        protected Task<ContractMetadataStruct> GetLatestContractMetadataAsync(string contractName)
        {
            var parameters = new Dictionary<string, object>
                {
                    { "contractName", contractName },
                };

            return this.azureclientFactory.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(
                 this.contractFactoryAbi,
                 this.contractFactoryAddress,
                 "GetLatest",
                 parameters);
        }

        /// <summary>
        /// Writes the asynchronous.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="contractName">Name of the contract.</param>
        /// <returns>The task.</returns>
        protected async Task<OffchainMessage> WriteAsync(string methodName, IDictionary<string, object> parameters, string contractName)
        {
            ArgumentValidators.ThrowIfNull(parameters, nameof(parameters));
            ArgumentValidators.ThrowIfNullOrEmpty(methodName, nameof(methodName));
            ArgumentValidators.ThrowIfNullOrEmpty(contractName, nameof(contractName));
            var offchainMessage = new OffchainMessage();
            try
            {
                // Get latest contract and setting the latest contract abi and address
                var contractMetadata = await this.GetLatestContractMetadataAsync(contractName).ConfigureAwait(false);
                this.logger.LogInformation($"Latest contract is:{contractMetadata}");

                var receipt = await this.azureclientFactory.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(
                   contractMetadata.ContractAbi,
                   contractMetadata.ContractAddress,
                   methodName,
                   parameters).ConfigureAwait(false);

                this.logger.LogInformation(
                    $"Gas used for contract {contractName} and method {methodName} {receipt.GasUsed.Value} with cumulative gas used {receipt.CumulativeGasUsed.Value}");

                string blockNumber = null;
                if (receipt.BlockNumber.HexValue != null)
                {
                    blockNumber = contractName.Contains("Bridge", StringComparison.OrdinalIgnoreCase)
                        ? receipt.BlockNumber.HexValue
                        : receipt.BlockNumber.ToUlong().ToString(CultureInfo.InvariantCulture);
                }

                offchainMessage.TransactionHash = receipt.TransactionHash;
                offchainMessage.BlockNumber = blockNumber;

                if (string.IsNullOrWhiteSpace(blockNumber) || string.IsNullOrWhiteSpace(receipt.TransactionHash))
                {
                    throw new EthereumRequireException("Transaction hash or block number is empty.");
                }

                offchainMessage.Status = StatusType.PROCESSED;
            }
            catch (EthereumRequireException ex)
            {
                this.logger.LogError(ex, $"Error occurred in contract {contractName} and method {methodName}. Error Message : {ex.Message}");
                offchainMessage.Status = StatusType.FAILED;
            }

            return offchainMessage;
        }

        /// <summary>
        /// Gets the movement contract metadata asynchronous.
        /// </summary>
        /// <typeparam name="T">Entity.</typeparam>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="contractName">Name of the contract.</param>
        /// <returns>The task.</returns>
        protected async Task<T> GetBlockchainDataAsync<T>(string methodName, IDictionary<string, object> parameters, string contractName)
        {
            // Get latest contract and setting the latest contract abi and address
            var contractMetadata = await this.GetLatestContractMetadataAsync(contractName).ConfigureAwait(false);
            return await this.azureclientFactory.EthereumClient.CallMethodAsync<T>(
                 contractMetadata.ContractAbi,
                 contractMetadata.ContractAddress,
                 methodName,
                 parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Queues the asynchronous.
        /// </summary>
        /// <param name="offchainMessage">The offchainMessage.</param>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="type">The type.</param>
        /// <returns>The task.</returns>
        protected async Task QueueAsync(OffchainMessage offchainMessage, int entityId, ServiceType type)
        {
            ArgumentValidators.ThrowIfNull(offchainMessage, nameof(offchainMessage));
            try
            {
                var queueClient = this.azureclientFactory.GetQueueClient(QueueConstants.OffchainQueue);
                offchainMessage.EntityId = entityId;
                offchainMessage.Type = type;

                await queueClient.QueueMessageAsync(offchainMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "TransactionHash", offchainMessage.TransactionHash },
                    { "BlockNumber", offchainMessage.BlockNumber },
                    { "EntityId", entityId.ToString(CultureInfo.InvariantCulture) },
                    { "Type", this.Type.ToString("G") },
                };

                this.logger.LogInformation($"Offchain Sync failed for entityId {entityId} {JsonConvert.SerializeObject(properties)}");
                this.logger.LogError(ex, ex.Message);
                this.telemetry.TrackEvent(Constants.Critical, EventName.OffchainSyncFailed.ToString("G"), properties: properties);
            }
        }

        /// <summary>
        /// Sends to blockchain asynchronous.
        /// </summary>
        /// <param name="identifier">The data.</param>
        /// <param name="queueName">The queue name.</param>
        /// <returns>The task.</returns>
        protected async Task SendToBlockchainAsync(int identifier, string queueName)
        {
            ArgumentValidators.ThrowIfNull(identifier, nameof(identifier));
            var queueClient = this.azureclientFactory.GetQueueClient(queueName);
            await queueClient.QueueSessionMessageAsync(identifier, identifier.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
        }

        /// <summary>
        /// Writes to movement owners blockchain asynchronous.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="movementId">The movement identifier.</param>
        /// <param name="blockchainMovementTransactionId">The blockchain movement transaction identifier.</param>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="existingMovement">The existing movement.</param>
        /// <returns>The task.</returns>
        protected async Task<OffchainMessage> WriteToMovementOwnersBlockchainAsync(Owner owner, string movementId, Guid? blockchainMovementTransactionId, string eventType, Movement existingMovement)
        {
            ArgumentValidators.ThrowIfNull(owner, nameof(owner));

            bool isBridge = IsMovementOwnerBridge(owner, existingMovement);

            var movementOwnerId = $"{movementId}-{owner.OwnerId}";
            var parameters = new Dictionary<string, object>
                {
                    { "movementOwnerId", movementOwnerId },
                    { "movementId", movementId },
                    { "ownerId", owner.OwnerId.ToString(CultureInfo.InvariantCulture) },
                };

            var hasOwnersParameters = new Dictionary<string, object>
                {
                    { "movementOwnerId", movementOwnerId },
                };

            var hasOwner = isBridge || await this.HasOwnerAsync(ContractNames.MovementOwnersFactory, hasOwnersParameters).ConfigureAwait(false);

            if (hasOwner && eventType == EventType.Delete.ToString("G"))
            {
                return await this.WriteAsync("Delete", parameters, isBridge ? $"{ContractNames.MovementOwnersFactory}Bridge" : ContractNames.MovementOwnersFactory).ConfigureAwait(false);
            }

            parameters.Add("transactionId", blockchainMovementTransactionId.ToString());
            parameters.Add("ownershipValue", owner.OwnershipValue.ToBlockChainNumber());
            parameters.Add("ownershipValueUnit", owner.OwnershipValueUnit);

            var methodName = hasOwner ? EventType.Update.ToString("G") : EventType.Insert.ToString("G");
            return await this.WriteAsync(methodName, parameters, isBridge ? $"{ContractNames.MovementOwnersFactory}Bridge" : ContractNames.MovementOwnersFactory).ConfigureAwait(false);
        }

        /// <summary>
        /// Writes to inventory product owners blockchain asynchronous.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="inventoryProductUniqueId">The inventory product unique identifier.</param>
        /// <param name="blockchainInventoryProductTransactionId">The blockchain inventory product transaction identifier.</param>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="existingInventoryProduct">The existing inventory product.</param>
        /// <returns>The task.</returns>
        protected async Task<OffchainMessage> WriteToInventoryProductOwnersBlockchainAsync(
            Owner owner, string inventoryProductUniqueId, Guid? blockchainInventoryProductTransactionId, string eventType, InventoryProduct existingInventoryProduct)
        {
            ArgumentValidators.ThrowIfNull(owner, nameof(owner));

            bool isBridge = IsInventoryProductOwnerBridge(owner, existingInventoryProduct);

            var inventoryProductOwnerId = $"{inventoryProductUniqueId}-{owner.OwnerId}";
            var parameters = new Dictionary<string, object>
            {
                { "inventoryProductOwnerId", inventoryProductOwnerId },
                { "inventoryProductId", inventoryProductUniqueId },
                { "ownerId", owner.OwnerId.ToString(CultureInfo.InvariantCulture) },
            };
            var hasOwnersParameters = new Dictionary<string, object>
            {
                { "inventoryProductOwnerId", inventoryProductOwnerId },
            };
            var hasOwner = isBridge || await this.HasOwnerAsync(ContractNames.InventoryProductOwnersFactory, hasOwnersParameters).ConfigureAwait(false);

            if (hasOwner && eventType == EventType.Delete.ToString("G"))
            {
                return await this.WriteAsync("Delete", parameters, isBridge ? $"{ContractNames.InventoryProductOwnersFactory}Bridge" : ContractNames.InventoryProductOwnersFactory)
                       .ConfigureAwait(false);
            }

            parameters.Add("transactionId", blockchainInventoryProductTransactionId.ToString());
            parameters.Add("ownershipValue", owner.OwnershipValue.ToBlockChainNumber());
            parameters.Add("ownershipValueUnit", owner.OwnershipValueUnit);

            var methodName = hasOwner ? EventType.Update.ToString("G") : EventType.Insert.ToString("G");

            return await this.WriteAsync(methodName, parameters, isBridge ? $"{ContractNames.InventoryProductOwnersFactory}Bridge" : ContractNames.InventoryProductOwnersFactory).ConfigureAwait(false);
        }

        private static bool IsMovementOwnerBridge(Owner owner, Movement existingMovement)
        {
            return (existingMovement != null && existingMovement.BlockNumber != null && existingMovement.BlockNumber.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                           || (owner.BlockNumber != null && owner.BlockNumber.StartsWith("0x", StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsInventoryProductOwnerBridge(Owner owner, InventoryProduct existingInventoryProduct)
        {
            return (existingInventoryProduct != null && existingInventoryProduct.BlockNumber != null
                           && existingInventoryProduct.BlockNumber.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                           || (owner.BlockNumber != null && owner.BlockNumber.StartsWith("0x", StringComparison.OrdinalIgnoreCase));
        }

        private Task<bool> HasOwnerAsync(string contractName, Dictionary<string, object> parameters)
        {
            return this.GetBlockchainDataAsync<bool>("HasOwner", parameters, contractName);
        }
    }
}
