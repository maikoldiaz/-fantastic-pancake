// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryProductBlockchainService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Blockchain.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Core.Dto;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Azure.Entities;

    /// <summary>
    /// The Blockchain Inventory Service.
    /// </summary>
    public class InventoryProductBlockchainService : BlockchainService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<InventoryProductBlockchainService> logger;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryProductBlockchainService" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="azureclientFactory">The azureclient factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="telemetry">The telemetry.</param>
        public InventoryProductBlockchainService(
            ITrueLogger<InventoryProductBlockchainService> logger,
            IAzureClientFactory azureclientFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            ITelemetry telemetry)
            : base(azureclientFactory, logger, telemetry)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));

            this.logger = logger;
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        }

        /// <inheritdoc/>
        public override ServiceType Type => ServiceType.InventoryProduct;

        /// <inheritdoc/>
        public override async Task RegisterAsync(int entityId)
        {
            var repository = this.unitOfWork.CreateRepository<InventoryProduct>();
            var inventoryProduct = await repository.SingleOrDefaultAsync(
                x => x.InventoryProductId == entityId &&
                x.BlockchainStatus == StatusType.PROCESSING, "Owners").ConfigureAwait(false);
            if (inventoryProduct == null || !Enum.TryParse(inventoryProduct.EventType, true, out EventType eventType))
            {
                this.logger.LogInformation($"No inventory product record found or event type is invalid");
                return;
            }

            var status = await ValidateAsync(inventoryProduct, repository).ConfigureAwait(false);
            if (!status)
            {
                this.logger.LogInformation($"Validation failed for inventory product {entityId}.", $"{entityId}");
                return;
            }

            var existingInventoryProduct = await this.unitOfWork.InventoryProductRepository.GetLatestBlockchainInventoryProductAsync(inventoryProduct.InventoryProductUniqueId).ConfigureAwait(false);

            var result = await this.RegisterInventoryProductAsync(inventoryProduct, existingInventoryProduct).ConfigureAwait(false);
            if (result)
            {
                await this.RegisterOwnerAsync(
                        inventoryProduct.Owners,
                        inventoryProduct.InventoryProductUniqueId,
                        inventoryProduct.BlockchainInventoryProductTransactionId,
                        eventType.ToString(),
                        existingInventoryProduct).ConfigureAwait(false);
            }
        }

        private static async Task<bool> ValidateAsync(InventoryProduct inventoryProduct, IRepository<InventoryProduct> inventoryProductRepository)
        {
            // If there is earlier inventory product which is not registered yet
            var count = await inventoryProductRepository.GetCountAsync(
                x => x.InventoryProductUniqueId == inventoryProduct.InventoryProductUniqueId &&
                x.InventoryProductId < inventoryProduct.InventoryProductId &&
                x.BlockchainStatus != StatusType.PROCESSED &&
                (x.EventType != EventType.Update.ToString("G") || !x.IsDeleted)).ConfigureAwait(false);

            return count == 0;
        }

        private async Task<bool> RegisterInventoryProductAsync(InventoryProduct inventoryProduct, InventoryProduct existingInventoryProduct)
        {
            var result = await this.WriteToInventoryProductBlockchainAsync(inventoryProduct, existingInventoryProduct).ConfigureAwait(false);
            this.logger.LogInformation($"Contract call finished for inventory product: {inventoryProduct.InventoryProductId}, sending message to offchain queue.");
            await this.QueueAsync(result, inventoryProduct.InventoryProductId, ServiceType.InventoryProduct).ConfigureAwait(false);
            return true;
        }

        private async Task RegisterOwnerAsync(
            IEnumerable<Owner> owners,
            string inventoryProductUniqueId,
            Guid? blockchainInventoryProductTransactionId,
            string eventType,
            InventoryProduct existingInventoryProduct)
        {
            var tasks = new List<Task>();
            foreach (var owner in owners)
            {
                tasks.Add(Task.Run(async () =>
                {
                    this.logger.LogInformation($"Registering Owner: {owner.OwnerId} with identifier {owner.Id} for inventory {blockchainInventoryProductTransactionId}");
                    var result = await this.WriteToInventoryProductOwnersBlockchainAsync(owner, inventoryProductUniqueId, blockchainInventoryProductTransactionId, eventType, existingInventoryProduct)
                                 .ConfigureAwait(false);
                    this.logger.LogInformation($"Contract call finished for Owner: {owner.OwnerId} with identifier {owner.Id} for inventory {blockchainInventoryProductTransactionId}");
                    await this.QueueAsync(result, owner.Id, ServiceType.Owner).ConfigureAwait(false);
                }));
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task<OffchainMessage> WriteToInventoryProductBlockchainAsync(InventoryProduct inventoryProduct, InventoryProduct existingInventoryProduct)
        {
            ArgumentValidators.ThrowIfNull(inventoryProduct, nameof(inventoryProduct));
            var eventType = Enum.Parse<EventType>(inventoryProduct.EventType, true);

            this.logger.LogInformation($"Started building inventory parameters {inventoryProduct.InventoryProductId}");
            var parameters = new Dictionary<string, object>
                {
                    { "inventoryProductId", inventoryProduct.InventoryProductUniqueId },
                    { "transactionId", inventoryProduct.BlockchainInventoryProductTransactionId.ToString() },
                    { "inventoryDate", inventoryProduct.InventoryDate.Value.Ticks },
                    { "metadata", inventoryProduct.GetMetadata() },
                };

            if (eventType != EventType.Delete)
            {
                parameters.Add("productVolume", inventoryProduct.ProductVolume.ToBlockChainNumber());
                parameters.Add("measurementUnit", inventoryProduct.MeasurementUnit.HasValue ? inventoryProduct.MeasurementUnit.ToString() : null);
            }

            var isBridge = (existingInventoryProduct != null && existingInventoryProduct.BlockNumber != null
                           && existingInventoryProduct.BlockNumber.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                           || (inventoryProduct.BlockNumber != null && inventoryProduct.BlockNumber.StartsWith("0x", StringComparison.OrdinalIgnoreCase));

            this.logger.LogInformation($"Finished building inventory parameters for {inventoryProduct.InventoryProductId}, writing to contract");

            return await this.WriteAsync(eventType.ToString(), parameters, isBridge ? $"{ContractNames.InventoryProductsFactory}Bridge" : ContractNames.InventoryProductsFactory)
                   .ConfigureAwait(false);
        }
    }
}