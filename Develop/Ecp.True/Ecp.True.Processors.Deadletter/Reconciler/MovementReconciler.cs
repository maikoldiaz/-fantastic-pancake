// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementReconciler.cs" company="Microsoft">
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
    public class MovementReconciler : ReconcilerBase<Movement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovementReconciler" /> class.
        /// </summary>
        /// <param name="factory">The unit of work factory.</param>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        public MovementReconciler(IUnitOfWorkFactory factory, ITelemetry telemetry, IAzureClientFactory azureClientFactory, IConfigurationHandler configurationHandler)
            : base(factory, telemetry, azureClientFactory, configurationHandler, x => x.Movements, y => y.MovementTransactionId, z => z.MovementId)
        {
        }

        /// <inheritdoc />
        public override ServiceType Type => ServiceType.Movement;

        /// <inheritdoc/>
        protected override EventName EventName => EventName.MovementReconcileFailed;

        /// <inheritdoc/>
        protected override string QueueName => QueueConstants.BlockchainMovementQueue;

        /// <inheritdoc/>
        protected override async Task DoReconcileAsync(Movement entity, OffchainMessage message)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            if (!entity.PreviousBlockchainMovementTransactionId.HasValue)
            {
                return;
            }

            var repository = this.UnitOfWork.CreateRepository<Movement>();
            var previous = await repository.SingleOrDefaultAsync(x => x.BlockchainMovementTransactionId == entity.PreviousBlockchainMovementTransactionId).ConfigureAwait(false);

            previous.BlockchainStatus = message.Status;
            previous.TransactionHash = message.TransactionHash;
            previous.BlockNumber = message.BlockNumber;

            repository.Update(previous);
        }

        /// <inheritdoc/>
        protected override Task<IEnumerable<Movement>> GetEntitiesAsync(ReconciliationSettings settings, bool isCritical, int? takeRecords)
        {
            ArgumentValidators.ThrowIfNull(settings, nameof(settings));
            if (isCritical)
            {
                return this.GetCriticalMovementsAsync(settings, takeRecords);
            }

            return this.UnitOfWork.CreateRepository<Movement>().OrderByAsync(
                                x =>
                                x.BlockchainStatus == StatusType.PROCESSING && x.CreatedDate < settings.MaxDateTime && x.RetryCount <= settings.DefaultRetries &&
                                !(x.EventType == "Update" && !x.PreviousBlockchainMovementTransactionId.HasValue) && x.PendingApproval != true,
                                y => y.MovementTransactionId,
                                GetTake(settings, takeRecords));
        }

        /// <inheritdoc/>
        protected async override Task<IEvent> GetEventAsync(Movement entity)
        {
            var events = await this.AzureClientFactory.EthereumClient.GetEventsAsync<Blockchain.Events.MovementEvent>(entity?.BlockchainMovementTransactionId.ToString()).ConfigureAwait(false);
            return events.FirstOrDefault();
        }

        /// <inheritdoc/>
        protected override Task<IEnumerable<Movement>> GetFailedEntitiesAsync(ReconciliationSettings settings)
        {
            ArgumentValidators.ThrowIfNull(settings, nameof(settings));
            return this.UnitOfWork.CreateRepository<Movement>().OrderByAsync(
                                x => x.BlockchainStatus == StatusType.FAILED && x.RetryCount <= (settings.DefaultRetries + 1) &&
                                !(x.EventType == "Update" && !x.PreviousBlockchainMovementTransactionId.HasValue),
                                y => y.MovementTransactionId,
                                ReconciliationSettings.FailureBatch);
        }

        /// <inheritdoc/>
        protected override OffchainMessage GetOffchainMessage(Movement entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var offchainMessage = new OffchainMessage();
            offchainMessage.EntityId = entity.MovementTransactionId;

            return offchainMessage;
        }

        private Task<IEnumerable<Movement>> GetCriticalMovementsAsync(ReconciliationSettings settings, int? takeRecords)
        {
            return this.UnitOfWork.CreateRepository<Movement>().OrderByAsync(
                            x =>
                            x.BlockchainStatus == StatusType.PROCESSING && x.CreatedDate < settings.MaxDateTime && x.RetryCount > settings.DefaultRetries &&
                            !(x.EventType == "Update" && !x.PreviousBlockchainMovementTransactionId.HasValue) && x.PendingApproval != true,
                            y => y.MovementTransactionId,
                            takeRecords.HasValue ? takeRecords : settings.DefaultBatch);
        }
    }
}
