// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryProductReconciler.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Deadletter.Reconciler
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Core.Dto;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Azure.Interfaces;

    /// <summary>
    /// The movement reconciler.
    /// </summary>
    public class InventoryProductReconciler : ReconcilerBase<InventoryProduct>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryProductReconciler" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        public InventoryProductReconciler(IUnitOfWorkFactory factory, ITelemetry telemetry, IAzureClientFactory azureClientFactory, IConfigurationHandler configurationHandler)
            : base(factory, telemetry, azureClientFactory, configurationHandler, x => x.InventoryProducts, y => y.InventoryProductId, z => z.InventoryProductUniqueId)
        {
        }

        /// <inheritdoc />
        public override ServiceType Type => ServiceType.InventoryProduct;

        /// <inheritdoc/>
        protected override EventName EventName => EventName.InventoryReconcileFailed;

        /// <inheritdoc/>
        protected override string QueueName => QueueConstants.BlockchainInventoryProductQueue;

        /// <inheritdoc/>
        protected override async Task DoReconcileAsync(InventoryProduct entity, OffchainMessage message)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            if (!entity.PreviousBlockchainInventoryProductTransactionId.HasValue)
            {
                return;
            }

            var repository = this.UnitOfWork.CreateRepository<InventoryProduct>();
            var previous = await repository.SingleOrDefaultAsync(x => x.BlockchainInventoryProductTransactionId == entity.PreviousBlockchainInventoryProductTransactionId).ConfigureAwait(false);

            previous.BlockchainStatus = message.Status;
            previous.TransactionHash = message.TransactionHash;
            previous.BlockNumber = message.BlockNumber;

            repository.Update(previous);
        }

        /// <inheritdoc/>
        protected override Task<IEnumerable<InventoryProduct>> GetEntitiesAsync(ReconciliationSettings settings, bool isCritical, int? takeRecords)
        {
            ArgumentValidators.ThrowIfNull(settings, nameof(settings));
            int? take = GetTake(settings, takeRecords);
            if (isCritical)
            {
                return this.UnitOfWork.CreateRepository<InventoryProduct>().OrderByAsync(
                                x => x.BlockchainStatus == StatusType.PROCESSING && x.CreatedDate < settings.MaxDateTime &&
                                x.RetryCount > settings.DefaultRetries && !(x.EventType == "Update" && !x.PreviousBlockchainInventoryProductTransactionId.HasValue),
                                y => y.InventoryProductId,
                                take);
            }

            return this.UnitOfWork.CreateRepository<InventoryProduct>().OrderByAsync(
                                x => x.BlockchainStatus == StatusType.PROCESSING && x.CreatedDate < settings.MaxDateTime &&
                                x.RetryCount <= settings.DefaultRetries && !(x.EventType == "Update" && !x.PreviousBlockchainInventoryProductTransactionId.HasValue),
                                y => y.InventoryProductId,
                                take);
        }

        /// <inheritdoc/>
        protected async override Task<IEvent> GetEventAsync(InventoryProduct entity)
        {
            var events = await this.AzureClientFactory.EthereumClient
                .GetEventsAsync<Blockchain.Events.InventoryProductEvent>(entity?.BlockchainInventoryProductTransactionId.ToString()).ConfigureAwait(false);
            return events.FirstOrDefault();
        }

        /// <inheritdoc/>
        protected override Task<IEnumerable<InventoryProduct>> GetFailedEntitiesAsync(ReconciliationSettings settings)
        {
            ArgumentValidators.ThrowIfNull(settings, nameof(settings));
            return this.UnitOfWork.CreateRepository<InventoryProduct>().OrderByAsync(
                                x => x.BlockchainStatus == StatusType.FAILED && x.RetryCount <= (settings.DefaultRetries + 1) &&
                                !(x.EventType == "Update" && !x.PreviousBlockchainInventoryProductTransactionId.HasValue),
                                y => y.InventoryProductId,
                                ReconciliationSettings.FailureBatch);
        }

        /// <inheritdoc/>
        protected override OffchainMessage GetOffchainMessage(InventoryProduct entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var offchainMessage = new OffchainMessage();
            offchainMessage.EntityId = entity.InventoryProductId;

            return offchainMessage;
        }
    }
}
