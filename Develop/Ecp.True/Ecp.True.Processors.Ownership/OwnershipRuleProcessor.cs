// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipRuleProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.OwnershipRules.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.Response;

    /// <summary>
    /// The Ownership Rule Processor.
    /// </summary>
    public class OwnershipRuleProcessor : ProcessorBase, IOwnershipRuleProcessor
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<OwnershipRuleProcessor> logger;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// The ownership rule service.
        /// </summary>
        private readonly IOwnershipRuleProxy ownershipRuleService;

        /// <summary>
        /// The ownership rule service.
        /// </summary>
        private readonly IExecutionChainBuilder executionChainBuilder;

        /// <summary>
        /// The execution manager.
        /// </summary>
        private readonly IExecutionManager executionManager;

        /// <summary>
        /// The finalizer.
        /// </summary>
        private readonly IFinalizer finalizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipRuleProcessor"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="finalizerFactory">The finalizer factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="ownershipRuleService">The ownership rule service.</param>
        /// <param name="executionChainBuilder">The execution chain builder.</param>
        /// <param name="executionManagerFactory">The execution manager.</param>
        public OwnershipRuleProcessor(
            IRepositoryFactory factory,
            ITrueLogger<OwnershipRuleProcessor> logger,
            IUnitOfWorkFactory unitOfWorkFactory,
            IAzureClientFactory azureClientFactory,
            IFinalizerFactory finalizerFactory,
            IConfigurationHandler configurationHandler,
            IOwnershipRuleProxy ownershipRuleService,
            IExecutionChainBuilder executionChainBuilder,
            IExecutionManagerFactory executionManagerFactory)
            : base(factory)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            ArgumentValidators.ThrowIfNull(executionManagerFactory, nameof(executionManagerFactory));
            ArgumentValidators.ThrowIfNull(finalizerFactory, nameof(finalizerFactory));
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.azureClientFactory = azureClientFactory;
            this.configurationHandler = configurationHandler;
            this.logger = logger;
            this.ownershipRuleService = ownershipRuleService;
            this.executionChainBuilder = executionChainBuilder;
            this.executionManager = executionManagerFactory.GetExecutionManager(True.Entities.Enumeration.TicketType.Ownership);
            this.finalizer = finalizerFactory.GetFinalizer(True.Entities.Enumeration.FinalizerType.Ownership);
        }

        /// <inheritdoc/>
        public async Task<OwnershipRuleData> ProcessAsync(OwnershipRuleData ownershipRuleData, ChainType chainType)
        {
            ArgumentValidators.ThrowIfNull(ownershipRuleData, nameof(OwnershipRuleData));
            this.logger.LogInformation($"Ownership processing for chain {chainType} started.", $"{ownershipRuleData.TicketId}");

            var executor = this.executionChainBuilder.Build(ProcessType.Ownership, chainType);
            this.logger.LogInformation($"Ownership rule processing will start from {executor.GetType().Name}", $"{ownershipRuleData.TicketId}");

            this.executionManager.Initialize(executor);
            var result = await this.executionManager.ExecuteChainAsync(ownershipRuleData).ConfigureAwait(false);

            this.logger.LogInformation($"Ownership processing for chain {chainType} finished.", $"{ownershipRuleData.TicketId}");

            return (OwnershipRuleData)result;
        }

        /// <inheritdoc/>
        public async Task CleanOwnershipDataAsync(int ticketId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@TicketId", ticketId },
            };

            var repository = this.unitOfWork.CreateRepository<Ownership>();
            await repository.ExecuteAsync(Repositories.Constants.OperationalCutOffAndOwnershipCleanupProcedureName, parameters).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task QueueSyncOwnershipRuleAsync(string source)
        {
            var configuration = await this.configurationHandler.GetConfigurationAsync<OwnershipRuleSettings>(ConfigurationConstants.OwnershipRuleSettings).ConfigureAwait(false);
            var repository = this.unitOfWork.CreateRepository<OwnershipRuleRefreshHistory>();
            var lastRecordQuery = await repository.QueryAllAsync(null).ConfigureAwait(false);
            var lastUpdated = lastRecordQuery.OrderByDescending(e => e.CreatedDate).FirstOrDefault();

            if (lastUpdated == null)
            {
                this.logger.LogInformation($"No last record found at {DateTime.UtcNow.ToTrue()}.", Constants.OwnershipRulesSync);
                await this.DoSyncAsync(source).ConfigureAwait(false);
                return;
            }

            var elapsedTime = (DateTime.UtcNow.ToTrue() - lastUpdated.CreatedDate.GetValueOrDefault().AddMinutes(-5)).TotalHours;
            this.logger.LogInformation($"Time elapsed since last sync in hours: {elapsedTime}.", Constants.OwnershipRulesSync);

            if (configuration.RefreshIntervalInHours < elapsedTime)
            {
                this.logger.LogInformation($"Starting ownership rules sync at {DateTime.UtcNow.ToTrue()}.", Constants.OwnershipRulesSync);
                await this.DoSyncAsync(source).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task SyncOwnershipRulesAsync(string source)
        {
            var repository = this.unitOfWork.CreateRepository<OwnershipRuleRefreshHistory>();
            var ownershipRuleRefreshHistory = new OwnershipRuleRefreshHistory();
            var ownershipRuleSettings = await this.configurationHandler.GetConfigurationAsync<OwnershipRuleSettings>(ConfigurationConstants.OwnershipRuleSettings).ConfigureAwait(false);
            this.ownershipRuleService.Initialize(ownershipRuleSettings);

            try
            {
                ownershipRuleRefreshHistory.Status = true;
                ownershipRuleRefreshHistory.RequestedBy = source;

                repository.Insert(ownershipRuleRefreshHistory);
                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);

                var response = await this.ownershipRuleService.GetActiveRulesAsync().ConfigureAwait(false);
                await this.MergeOwnershipRulesAsync(response).ConfigureAwait(false);
                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Exception while syncing ownership rules. {ex.Message}", Constants.OwnershipRulesSync);
            }
            finally
            {
                ownershipRuleRefreshHistory.Status = false;
                repository.Update(ownershipRuleRefreshHistory);
                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task<int> GetOwnershipTicketByOwnershipNodeIdAsync(int ownershipNodeId)
        {
            var ownershipNodeRepository = this.unitOfWork.CreateRepository<OwnershipNode>();
            var ownershipNode = await ownershipNodeRepository.GetByIdAsync(ownershipNodeId).ConfigureAwait(false);
            return ownershipNode.TicketId;
        }

        /// <inheritdoc />
        public Task FinalizeProcessAsync(OwnershipRuleData ownershipRuleData)
        {
            return this.finalizer.ProcessAsync(ownershipRuleData);
        }

        private static void MergeAndUpdate<TEntity>(IEnumerable<TEntity> sourceEntities, IEnumerable<TEntity> inputEntities, IRepository<TEntity> repository)
        where TEntity : RuleEntity
        {
            // update all existing entities by finding intersection
            sourceEntities.ForEach(x =>
            {
                var updated = inputEntities.FirstOrDefault(y => y.RuleId == x.RuleId);
                if (updated != null)
                {
                    x.CopyFrom(updated);
                    repository.Update(x);
                }
            });

            // add new rules
            var newRules = new List<TEntity>();
            newRules.AddRange(inputEntities.Where(x => !sourceEntities.Any(y => y.RuleId == x.RuleId)));
            if (newRules.Any())
            {
                repository.InsertAll(newRules);
            }

            // deactivate stale rules
            var missing = new List<TEntity>();
            missing.AddRange(sourceEntities.Where(a => !inputEntities.Any(r => r.RuleId == a.RuleId)));

            if (missing.Any())
            {
                missing.ForEach(e => e.IsActive = false);
                repository.UpdateAll(missing);
            }
        }

        private async Task MergeOwnershipRulesAsync(OwnershipRuleResponse response)
        {
            ArgumentValidators.ThrowIfNull(response, nameof(response));

            var nodeConnectionRuleRepo = this.unitOfWork.CreateRepository<NodeConnectionProductRule>();
            var nodeOwnershipRuleRepo = this.unitOfWork.CreateRepository<NodeOwnershipRule>();
            var nopeProductRuleRepo = this.unitOfWork.CreateRepository<NodeProductRule>();

            var activeNodeConnectionRules = await nodeConnectionRuleRepo.GetAllAsync(null).ConfigureAwait(false);
            var activeNodeOwnershipRules = await nodeOwnershipRuleRepo.GetAllAsync(null).ConfigureAwait(false);
            var activeNodeProductRules = await nopeProductRuleRepo.GetAllAsync(null).ConfigureAwait(false);

            var sourceNodeConnectionRules = response.OwnershipRuleConnections?.Select(
                s => new NodeConnectionProductRule
                {
                    IsActive = true,
                    RuleId = s.RuleId,
                    RuleName = s.Name,
                });

            MergeAndUpdate(
                activeNodeConnectionRules,
                sourceNodeConnectionRules,
                nodeConnectionRuleRepo);

            var sourceNodeOwnershipRules = response.NodeOwnershipRules?.Select(
                s => new NodeOwnershipRule
                {
                    IsActive = true,
                    RuleId = s.RuleId,
                    RuleName = s.Name,
                });
            MergeAndUpdate(
                activeNodeOwnershipRules,
                sourceNodeOwnershipRules,
                nodeOwnershipRuleRepo);

            var sourceNodeProductRules = response.NodeProductOwnershipRules?.Select(
                s => new NodeProductRule
                {
                    IsActive = true,
                    RuleId = s.RuleId,
                    RuleName = s.Name,
                });
            MergeAndUpdate(
                activeNodeProductRules,
                sourceNodeProductRules,
                nopeProductRuleRepo);
        }

        private async Task DoSyncAsync(string source)
        {
            var ownershipRuleQueueClient = this.azureClientFactory.GetQueueClient(QueueConstants.OwnershipRuleQueue);
            await ownershipRuleQueueClient.QueueSessionMessageAsync(source, Constants.OwnershipRulesSync).ConfigureAwait(false);
        }
    }
}
