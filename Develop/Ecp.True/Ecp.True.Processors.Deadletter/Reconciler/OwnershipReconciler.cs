// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipReconciler.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Core.Dto;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Blockchain;
    using Ecp.True.Processors.Blockchain.Events;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Azure.Interfaces;

    /// <summary>
    /// The movement reconciler.
    /// </summary>
    public class OwnershipReconciler : ReconcilerBase<Ownership>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipReconciler" /> class.
        /// </summary>
        /// <param name="factory">The unit of work factory.</param>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        public OwnershipReconciler(IUnitOfWorkFactory factory, ITelemetry telemetry, IAzureClientFactory azureClientFactory, IConfigurationHandler configurationHandler)
            : base(factory, telemetry, azureClientFactory, configurationHandler, x => x.Ownerships, y => y.OwnershipId, null)
        {
        }

        /// <inheritdoc />
        public override ServiceType Type => ServiceType.Ownership;

        /// <inheritdoc/>
        protected override EventName EventName => EventName.OwnershipReconcileFailed;

        /// <inheritdoc/>
        protected override string QueueName => QueueConstants.BlockchainOwnershipQueue;

        /// <inheritdoc/>
        public override async Task ReconcileAsync(OffchainMessage message)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));

            var repository = this.UnitOfWork.CreateRepository<Ownership>();
            var entity = await repository.SingleOrDefaultAsync(a => a.OwnershipId == message.EntityId, "MovementTransaction", "InventoryProduct").ConfigureAwait(false);

            if (entity == null || entity.BlockchainStatus == StatusType.PROCESSED)
            {
                return;
            }

            // To-Do: Update transaction IDs are part of strategies or remove these columns and update queries.
            // Once done, remove this override.
            entity.BlockchainMovementTransactionId = entity.MessageTypeId == MessageType.MovementOwnership ? entity.MovementTransaction.BlockchainMovementTransactionId : null;
            entity.BlockchainInventoryProductTransactionId = entity.MessageTypeId == MessageType.InventoryOwnership ? entity.InventoryProduct.BlockchainInventoryProductTransactionId : null;

            entity.BlockchainStatus = message.Status;
            entity.TransactionHash = message.TransactionHash;
            entity.BlockNumber = message.BlockNumber;

            repository.Update(entity);

            await this.DoReconcileAsync(entity, message).ConfigureAwait(false);
            await this.UnitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Does the reconcile asynchronous.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The task.</returns>
        protected override async Task DoReconcileAsync(ReconciliationSettings settings)
        {
            var ownerships = await this.GetEntitiesAsync(settings, false, null).ConfigureAwait(false);

            var grouped = ownerships.Where(x => x.MovementTransactionId.HasValue).GroupBy(x => x.MovementTransactionId.Value, x => x.OwnershipId);
            var allGrouped = grouped.Concat(ownerships.Where(x => x.InventoryProductId.HasValue).GroupBy(x => x.InventoryProductId.Value, x => x.OwnershipId));

            var tasks = allGrouped.Select(x => this.QueueAsync(x.ToList(), x.Key.ToString(CultureInfo.InvariantCulture), this.QueueName));
            await Task.WhenAll(tasks).ConfigureAwait(false);

            ownerships.ForEach(x => x.RetryCount++);
            this.UnitOfWork.CreateRepository<Ownership>().UpdateAll(ownerships);
        }

        /// <inheritdoc/>
        protected override async Task DoReconcileAsync(Ownership entity, OffchainMessage message)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            if (!entity.PreviousBlockchainOwnershipId.HasValue)
            {
                return;
            }

            var repository = this.UnitOfWork.CreateRepository<Ownership>();
            var previous = await repository.SingleOrDefaultAsync(x => x.BlockchainOwnershipId == entity.PreviousBlockchainOwnershipId).ConfigureAwait(false);

            previous.BlockchainStatus = message.Status;
            previous.TransactionHash = message.TransactionHash;
            previous.BlockNumber = message.BlockNumber;

            repository.Update(previous);
        }

        /// <inheritdoc/>
        protected override Task<IEnumerable<Ownership>> GetEntitiesAsync(ReconciliationSettings settings, bool isCritical, int? takeRecords)
        {
            ArgumentValidators.ThrowIfNull(settings, nameof(settings));
            if (isCritical)
            {
                return this.UnitOfWork.CreateRepository<Ownership>().OrderByAsync(
                                x =>
                                x.BlockchainStatus == StatusType.PROCESSING && x.CreatedDate < settings.MaxDateTime && x.RetryCount > settings.DefaultRetries &&
                                !(x.EventType == "Update" && !x.PreviousBlockchainOwnershipId.HasValue),
                                y => y.OwnershipId,
                                GetTake(settings, takeRecords));
            }

            return this.UnitOfWork.CreateRepository<Ownership>().OrderByAsync(
                                x =>
                                x.BlockchainStatus == StatusType.PROCESSING && x.CreatedDate < settings.MaxDateTime && x.RetryCount <= settings.DefaultRetries &&
                                !(x.EventType == "Update" && !x.PreviousBlockchainOwnershipId.HasValue),
                                y => y.OwnershipId,
                                GetTake(settings, takeRecords));
        }

        /// <inheritdoc/>
        protected async override Task<IEvent> GetEventAsync(Ownership entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            IEnumerable<OwnershipEvent> events;
            if (entity.BlockchainMovementTransactionId.HasValue)
            {
                var id = $"{entity.OwnerId}-{entity.BlockchainMovementTransactionId.GetValueOrDefault()}";
                events = await this.AzureClientFactory.EthereumClient.GetEventsAsync<Blockchain.Events.MovementOwnershipEvent>(id).ConfigureAwait(false);
            }
            else
            {
                var id = $"{entity.OwnerId}-{entity.BlockchainInventoryProductTransactionId.GetValueOrDefault()}";
                events = await this.AzureClientFactory.EthereumClient.GetEventsAsync<Blockchain.Events.InventoryProductOwnershipEvent>(id).ConfigureAwait(false);
            }

            return FilterEvent(events, entity);
        }

        /// <inheritdoc/>
        protected override Task<IEnumerable<Ownership>> GetFailedEntitiesAsync(ReconciliationSettings settings)
        {
            ArgumentValidators.ThrowIfNull(settings, nameof(settings));
            return this.UnitOfWork.CreateRepository<Ownership>().OrderByAsync(
                                x => x.BlockchainStatus == StatusType.FAILED && x.RetryCount <= (settings.DefaultRetries + 1) &&
                                !(x.EventType == "Update" && !x.PreviousBlockchainOwnershipId.HasValue),
                                y => y.OwnershipId,
                                ReconciliationSettings.FailureBatch);
        }

        /// <inheritdoc/>
        protected override OffchainMessage GetOffchainMessage(Ownership entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var offchainMessage = new OffchainMessage();
            offchainMessage.EntityId = entity.OwnershipId;

            return offchainMessage;
        }

        private static IEvent FilterEvent(IEnumerable<OwnershipEvent> events, Ownership entity)
        {
            if (!Enum.TryParse(entity.EventType, true, out EventType eventType))
            {
                return null;
            }

            if (eventType == EventType.Insert)
            {
                return events.FirstOrDefault(x => x.ActionType == 0 && x.Volume.ToDecimal() == entity.OwnershipVolume);
            }
            else if (eventType == EventType.Update)
            {
                return events.FirstOrDefault(x => x.ActionType == 1 && x.Volume.ToDecimal() == entity.OwnershipVolume);
            }
            else
            {
                return events.FirstOrDefault(x => x.ActionType == 2 && x.Volume == 0);
            }
        }
    }
}
