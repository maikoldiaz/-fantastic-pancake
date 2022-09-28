// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReconcilerBase.cs" company="Microsoft">
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
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Core.Dto;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Deadletter.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Azure.Interfaces;

    /// <summary>
    /// The reconciler base.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="Ecp.True.Processors.Deadletter.Interfaces.IReconciler" />
    public abstract class ReconcilerBase<TEntity> : IReconciler
        where TEntity : class, IBlockchainEntity
    {
        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// The predicate.
        /// </summary>
        private readonly Func<BlockchainFailures, IEnumerable<int>> predicate;

        /// <summary>
        /// The identifier resolver.
        /// </summary>
        private readonly Func<TEntity, int> identifierResolver;

        /// <summary>
        /// The group identifier resolver.
        /// </summary>
        private readonly Func<TEntity, string> groupIdResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReconcilerBase{TEntity}" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="identifierResolver">The identifier resolver.</param>
        /// <param name="groupIdResolver">The group identifier resolver.</param>
        protected ReconcilerBase(
            IUnitOfWorkFactory factory,
            ITelemetry telemetry,
            IAzureClientFactory azureClientFactory,
            IConfigurationHandler configurationHandler,
            Func<BlockchainFailures, IEnumerable<int>> predicate,
            Func<TEntity, int> identifierResolver,
            Func<TEntity, string> groupIdResolver)
        {
            ArgumentValidators.ThrowIfNull(factory, nameof(factory));
            this.telemetry = telemetry;
            this.AzureClientFactory = azureClientFactory;
            this.configurationHandler = configurationHandler;
            this.predicate = predicate;
            this.identifierResolver = identifierResolver;
            this.groupIdResolver = groupIdResolver;
            this.UnitOfWork = factory.GetUnitOfWork();
        }

        /// <inheritdoc/>
        public abstract ServiceType Type { get; }

        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        /// <value>
        /// The unit of work.
        /// </value>
        protected IUnitOfWork UnitOfWork { get; private set; }

        /// <summary>
        /// Gets the azure client factory.
        /// </summary>
        /// <value>
        /// The azure client factory.
        /// </value>
        protected IAzureClientFactory AzureClientFactory { get; private set; }

        /// <summary>
        /// Gets the name of the event.
        /// </summary>
        /// <value>
        /// The name of the event.
        /// </value>
        protected abstract EventName EventName { get; }

        /// <summary>
        /// Gets the name of the queue.
        /// </summary>
        /// <value>
        /// The name of the queue.
        /// </value>
        protected abstract string QueueName { get; }

        /// <inheritdoc/>
        public async Task<IEnumerable<FailedRecord>> GetFailuresAsync(bool isCritical, int? takeRecords)
        {
            var settings = await this.configurationHandler.GetConfigurationAsync<ReconciliationSettings>(ConfigurationConstants.ReconciliationSettings).ConfigureAwait(false);
            var name = typeof(TEntity).Name;
            var expired = await this.GetEntitiesAsync(settings, true, takeRecords).ConfigureAwait(false);
            if (isCritical)
            {
                return expired.Select(x => new FailedRecord { Name = name, RecordId = this.identifierResolver(x) });
            }

            var pending = await this.GetEntitiesAsync(settings, false, takeRecords).ConfigureAwait(false);
            return expired.Concat(pending).Select(x => new FailedRecord { Name = name, RecordId = this.identifierResolver(x) });
        }

        /// <inheritdoc/>
        public async Task ReconcileAsync()
        {
            var settings = await this.configurationHandler.GetConfigurationAsync<ReconciliationSettings>(ConfigurationConstants.ReconciliationSettings).ConfigureAwait(false);
            var entities = await this.GetEntitiesAsync(settings, true, null).ConfigureAwait(false);

            if (entities.Any())
            {
                var metrics = new Dictionary<string, double> { { "Count", entities.Count() }, };
                this.telemetry.TrackEvent(Constants.Critical, this.EventName.ToString("G"), metrics: metrics);
            }

            await this.DoReconcileAsync(settings).ConfigureAwait(false);
            await this.UnitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async virtual Task ReconcileAsync(OffchainMessage message)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));

            var repository = this.UnitOfWork.CreateRepository<TEntity>();
            var entity = await repository.GetByIdAsync(message.EntityId).ConfigureAwait(false);

            if (entity == null || entity.BlockchainStatus == StatusType.PROCESSED)
            {
                return;
            }

            entity.BlockchainStatus = message.Status;
            entity.TransactionHash = message.TransactionHash;
            entity.BlockNumber = message.BlockNumber;

            repository.Update(entity);

            await this.DoReconcileAsync(entity, message).ConfigureAwait(false);
            await this.UnitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task ReconcileFailureAsync()
        {
            var settings = await this.configurationHandler.GetConfigurationAsync<ReconciliationSettings>(ConfigurationConstants.ReconciliationSettings).ConfigureAwait(false);
            var repository = this.UnitOfWork.CreateRepository<TEntity>();
            var failedEntities = await this.GetFailedEntitiesAsync(settings).ConfigureAwait(false);

            foreach (var failedEntity in failedEntities)
            {
                var evt = await this.GetEventAsync(failedEntity).ConfigureAwait(false);
                if (evt != null)
                {
                    var offchainMessage = this.GetOffchainMessage(failedEntity);
                    offchainMessage.BlockNumber = evt.BlockNumber.ToString(CultureInfo.InvariantCulture);
                    offchainMessage.TransactionHash = evt.TransactionHash;
                    offchainMessage.Status = StatusType.PROCESSED;
                    offchainMessage.Type = this.Type;

                    var queueClient = this.AzureClientFactory.GetQueueClient(QueueConstants.OffchainQueue);
                    await queueClient.QueueMessageAsync(offchainMessage).ConfigureAwait(false);
                }
                else
                {
                    failedEntity.BlockchainStatus = failedEntity.RetryCount <= settings.DefaultRetries ? StatusType.PROCESSING : failedEntity.BlockchainStatus;
                    failedEntity.RetryCount++;
                    repository.Update(failedEntity);
                    await this.UnitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
                }
            }
        }

        /// <inheritdoc/>
        public async Task ResetAsync(BlockchainFailures info)
        {
            var repository = this.UnitOfWork.CreateRepository<TEntity>();
            var entities = await repository.GetAllAsync(x => this.predicate(info).Contains(this.identifierResolver(x))).ConfigureAwait(false);

            entities.ForEach(x => x.RetryCount = 0);
            await this.UnitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the number of records to get.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="takeRecords">the records to get.</param>
        /// <returns>the records number.</returns>
        protected static int GetTake(ReconciliationSettings settings, int? takeRecords)
        {
            ArgumentValidators.ThrowIfNull(settings, nameof(settings));
            return takeRecords.HasValue ? (int)takeRecords : settings.DefaultBatch;
        }

        /// <summary>
        /// Does the reconcile asynchronous.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The task.</returns>
        protected virtual async Task DoReconcileAsync(ReconciliationSettings settings)
        {
            var entities = await this.GetEntitiesAsync(settings, false, null).ConfigureAwait(false);

            var grouped = entities.GroupBy(x => this.groupIdResolver(x), x => this.identifierResolver(x));
            await Task.WhenAll(grouped.Select(x => this.QueueAsync(x.ToList(), x.Key, this.QueueName))).ConfigureAwait(false);

            entities.ForEach(x => x.RetryCount++);
            this.UnitOfWork.CreateRepository<TEntity>().UpdateAll(entities);
        }

        /// <summary>
        /// Does the reconcile asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        /// <returns>The task.</returns>
        protected virtual Task DoReconcileAsync(TEntity entity, OffchainMessage message)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets the event asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The event.</returns>
        protected virtual Task<IEvent> GetEventAsync(TEntity entity)
        {
            return Task.FromResult((IEvent)null);
        }

        /// <summary>
        /// Gets the entities asynchronous.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The task.</returns>
        protected virtual Task<IEnumerable<TEntity>> GetFailedEntitiesAsync(ReconciliationSettings settings)
        {
            ArgumentValidators.ThrowIfNull(settings, nameof(settings));
            return this.UnitOfWork.CreateRepository<TEntity>().OrderByAsync(
                                x => x.BlockchainStatus == StatusType.FAILED && x.RetryCount <= (settings.DefaultRetries + 1),
                                y => this.identifierResolver(y),
                                ReconciliationSettings.FailureBatch);
        }

        /// <summary>
        /// Gets the entities asynchronous.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="isCritical">if set to <c>true</c> [is critical].</param>
        /// <param name="takeRecords">the records to get.</param>
        /// <returns>The task.</returns>
        protected virtual Task<IEnumerable<TEntity>> GetEntitiesAsync(ReconciliationSettings settings, bool isCritical, int? takeRecords)
        {
            ArgumentValidators.ThrowIfNull(settings, nameof(settings));
            int take = GetTake(settings, takeRecords);
            if (isCritical)
            {
                return this.UnitOfWork.CreateRepository<TEntity>().OrderByAsync(
                                x =>
                                x.BlockchainStatus == StatusType.PROCESSING && x.CreatedDate < settings.MaxDateTime && x.RetryCount > settings.DefaultRetries,
                                y => this.identifierResolver(y),
                                take);
            }

            return this.UnitOfWork.CreateRepository<TEntity>().OrderByAsync(
                                x =>
                                x.BlockchainStatus == StatusType.PROCESSING && x.CreatedDate < settings.MaxDateTime && x.RetryCount <= settings.DefaultRetries,
                                y => this.identifierResolver(y),
                                take);
        }

        /// <summary>
        /// Queues the asynchronous.
        /// </summary>
        /// <param name="entityIds">The entity ids.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns>The task.</returns>
        protected async Task QueueAsync(IEnumerable<int> entityIds, string sessionId, string queueName)
        {
            ArgumentValidators.ThrowIfNull(entityIds, nameof(entityIds));
            var queueClient = this.AzureClientFactory.GetQueueClient(queueName);
            foreach (var id in entityIds)
            {
                await queueClient.QueueSessionMessageAsync(id, sessionId).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the event asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The event.</returns>
        protected virtual OffchainMessage GetOffchainMessage(TEntity entity)
        {
            return null;
        }
    }
}
