// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnerBlockchainService.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// The OwnerBlockchainService.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Blockchain.BlockchainService" />
    public class OwnerBlockchainService : BlockchainService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<OwnerBlockchainService> logger;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnerBlockchainService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="azureclientFactory">The azureclient factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="telemetry">The telemetry.</param>
        public OwnerBlockchainService(
          ITrueLogger<OwnerBlockchainService> logger,
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
        public override ServiceType Type => ServiceType.Owner;

        /// <summary>
        /// Registers the asynchronous.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns>Returns the completed task.</returns>
        public override async Task RegisterAsync(int entityId)
        {
            var repository = this.unitOfWork.CreateRepository<Owner>();
            var owner = await repository.SingleOrDefaultAsync(
                                                x => x.Id == entityId && x.BlockchainStatus == StatusType.PROCESSING)
                                                .ConfigureAwait(false);

            if (owner == null)
            {
                this.logger.LogInformation($"No owner record found.");
                return;
            }

            await this.DoRegisterAsync(owner).ConfigureAwait(false);
        }

        private async Task DoRegisterAsync(Owner owner)
        {
            if (owner.MovementTransactionId.HasValue)
            {
                this.logger.LogInformation($"Movement owner registration {owner.Id}.", $"{owner.Id}");
                var movementRepository = this.unitOfWork.CreateRepository<Movement>();
                var movement = await movementRepository.SingleOrDefaultAsync(
                                                    x => x.MovementTransactionId == owner.MovementTransactionId && x.BlockchainStatus == StatusType.PROCESSED).ConfigureAwait(false);
                if (movement != null && Enum.TryParse(movement.EventType, true, out EventType eventType))
                {
                    this.logger.LogInformation($"Registering Owner: {owner.OwnerId} with identifier {owner.Id} for movement {movement.MovementTransactionId}");
                    var result = await this.WriteToMovementOwnersBlockchainAsync(
                                        owner, movement.MovementId, movement.BlockchainMovementTransactionId, eventType.ToString(), movement).ConfigureAwait(false);
                    this.logger.LogInformation($"Contract call finished for Owner: {owner.OwnerId} with identifier {owner.Id} for movement {movement.MovementTransactionId}");
                    await this.QueueAsync(result, owner.Id, ServiceType.Owner).ConfigureAwait(false);
                }
            }

            if (owner.InventoryProductId.HasValue)
            {
                this.logger.LogInformation($"InventoryProduct owner registration {owner.Id}.", $"{owner.Id}");
                var inventoryProductRepository = this.unitOfWork.CreateRepository<InventoryProduct>();
                var inventoryProduct = await inventoryProductRepository.SingleOrDefaultAsync(
                    x => x.InventoryProductId == owner.InventoryProductId && x.BlockchainStatus == StatusType.PROCESSED).ConfigureAwait(false);
                if (inventoryProduct != null && Enum.TryParse(inventoryProduct.EventType, true, out EventType eventType))
                {
                    this.logger.LogInformation($"Registering Owner: {owner.OwnerId} with identifier {owner.Id} for inventory {inventoryProduct.InventoryProductId}");
                    var result = await this.WriteToInventoryProductOwnersBlockchainAsync(
                    owner, inventoryProduct.InventoryProductUniqueId, inventoryProduct.BlockchainInventoryProductTransactionId, eventType.ToString(), inventoryProduct).ConfigureAwait(false);
                    this.logger.LogInformation($"Contract call finished for Owner: {owner.OwnerId} with identifier {owner.Id} for inventory {inventoryProduct.InventoryProductId}");
                    await this.QueueAsync(result, owner.Id, ServiceType.Owner).ConfigureAwait(false);
                }
            }
        }
    }
}
