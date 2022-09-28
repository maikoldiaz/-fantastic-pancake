// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipBlockchainService.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Core.Dto;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Azure.Entities;

    /// <summary>
    /// The Blockchain Movement Ownership Service.
    /// </summary>
    public class OwnershipBlockchainService : BlockchainService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<OwnershipBlockchainService> logger;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipBlockchainService" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="azureclientFactory">The azureclient factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="telemetry">The telemetry.</param>
        public OwnershipBlockchainService(
               ITrueLogger<OwnershipBlockchainService> logger,
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
        public override ServiceType Type => ServiceType.Ownership;

        /// <summary>
        ///  Registers the asynchronous.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>The task.</returns>
        /// <inheritdoc />
        public override async Task RegisterAsync(int entityId)
        {
            var repository = this.unitOfWork.CreateRepository<Ownership>();
            var ownership = await repository.SingleOrDefaultAsync(
                x => x.OwnershipId == entityId && x.BlockchainStatus == StatusType.PROCESSING, "MovementTransaction", "InventoryProduct").ConfigureAwait(false);

            // Return if record does not exist or already registered or invalid event type is passed
            if (ownership == null || !Enum.TryParse(ownership.EventType, true, out EventType eventType))
            {
                this.logger.LogInformation($"No ownership record found or event type is invalid");
                return;
            }

            var validationStatus = await ValidateAsync(ownership, repository).ConfigureAwait(false);
            if (!validationStatus)
            {
                this.logger.LogInformation($"Validation failed for ownership {entityId}.", $"{entityId}");
                return;
            }

            var existingOwnership = await this.GetExistingOwnershipAsync(ownership).ConfigureAwait(false);

            var result = await this.WriteToBlockchainAsync(ownership, eventType, existingOwnership).ConfigureAwait(false);
            this.logger.LogInformation($"Contract call finished for ownership: {entityId}, sending message to offchain queue.");
            await this.QueueAsync(result, entityId, this.Type).ConfigureAwait(false);
        }

        private static async Task<bool> ValidateAsync(Ownership ownership, IRepository<Ownership> ownershipRepository)
        {
            var movementTransactionId = ownership.MovementTransactionId.GetValueOrDefault();
            var inventoryProductId = ownership.InventoryProductId.GetValueOrDefault();

            // If there is earlier movement or inventory which is not registered yet
            var count = await ownershipRepository.GetCountAsync(x =>
                                                        x.OwnerId == ownership.OwnerId &&
                                                        (x.MovementTransactionId == movementTransactionId || x.InventoryProductId == inventoryProductId) &&
                                                        x.BlockchainStatus != StatusType.PROCESSED &&
                                                        x.OwnershipId < ownership.OwnershipId &&
                                                        (x.EventType != EventType.Update.ToString("G") || !x.IsDeleted)).ConfigureAwait(false);
            return count == 0;
        }

        private async Task<Ownership> GetExistingOwnershipAsync(Ownership ownership)
        {
            var ownershipRepository = this.unitOfWork.CreateRepository<Ownership>();
            var existingOwnerships = ownership.MessageTypeId == MessageType.MovementOwnership ?
                         await ownershipRepository.OrderByAsync(
                         x => x.MovementTransactionId == ownership.MovementTransactionId
                         && x.OwnerId == ownership.OwnerId && x.BlockchainStatus == StatusType.PROCESSED
                         && x.EventType == EventType.Insert.ToString("G"),
                         y => y.OwnershipId,
                         null).ConfigureAwait(false)
                         : await ownershipRepository.OrderByAsync(
                         x => x.InventoryProductId == ownership.InventoryProductId
                         && x.OwnerId == ownership.OwnerId && x.BlockchainStatus == StatusType.PROCESSED
                         && x.EventType == EventType.Insert.ToString("G"),
                         y => y.OwnershipId,
                         null).ConfigureAwait(false);
            return existingOwnerships.FirstOrDefault();
        }

        private Task<OffchainMessage> WriteToBlockchainAsync(Ownership ownership, EventType eventType, Ownership existingOwnership)
        {
            var parameters = new Dictionary<string, object>();

            this.logger.LogInformation($"Started building ownership parameters {ownership.OwnershipId}");
            if (ownership.MessageTypeId == MessageType.MovementOwnership)
            {
                parameters.Add("movementId", ownership.MovementTransaction.MovementId);
                parameters.Add("movementOwnershipId", $"{ownership.OwnerId}-{ownership.MovementTransaction.BlockchainMovementTransactionId.GetValueOrDefault()}");
            }
            else
            {
                parameters.Add("inventoryProductId", ownership.InventoryProduct.InventoryProductUniqueId);
                parameters.Add("inventoryProductOwnershipId", $"{ownership.OwnerId}-{ownership.InventoryProduct.BlockchainInventoryProductTransactionId.GetValueOrDefault()}");
            }

            parameters.Add("ticketId", ownership.TicketId);

            if (eventType != EventType.Delete)
            {
                parameters.Add("ownershipVolume", ownership.OwnershipVolume.ToBlockChainNumber());
                parameters.Add("ownershipPercentage", ownership.OwnershipPercentage.ToBlockChainNumber());
                parameters.Add("metadata", $"{ownership.OwnerId}|{ownership.AppliedRule}|{ownership.RuleVersion}");
            }

            var isBridge = (existingOwnership != null && existingOwnership.BlockNumber != null && existingOwnership.BlockNumber.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                           || (ownership.BlockNumber != null && ownership.BlockNumber.StartsWith("0x", StringComparison.OrdinalIgnoreCase));

            var contractName = ownership.MessageTypeId == MessageType.MovementOwnership ? ContractNames.MovementOwnershipsFactory : ContractNames.InventoryProductOwnershipsFactory;
            var modifiedContractName = isBridge ? $"{contractName}Bridge" : contractName;

            this.logger.LogInformation($"Finished building ownership parameters for {ownership.OwnershipId}, writing to contract");
            return this.WriteAsync(eventType.ToString(), parameters, modifiedContractName);
        }
    }
}