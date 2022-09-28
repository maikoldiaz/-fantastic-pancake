// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConciliationOrchestrator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Ownership
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Conciliation.Entities;
    using Ecp.True.Processors.Conciliation.Interfaces;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.HandleFailure;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;

    /// <summary>
    /// The Generator.
    /// </summary>
    public class ConciliationOrchestrator : OrchestratorBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<ConciliationOrchestrator> logger;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// The  processor.
        /// </summary>
        private readonly IConciliationProcessor conciliationProcessor;

        /// <summary>
        /// The ownership rule processor.
        /// </summary>
        private readonly IOwnershipRuleProcessor ownershipRuleProcessor;

        /// <summary>
        /// The  failure handler.
        /// </summary>
        private readonly IFailureHandler failureHandler;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConciliationOrchestrator"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="ownershipRuleProcessor">The ownership rule processor.</param>
        /// <param name="conciliationProcessor">The  conciliation processor.</param>
        /// <param name="failureHandlerFactory">The failureHandlerFactory.</param>
        /// <param name="unitOfWorkFactory">The unit of work.</param>
        public ConciliationOrchestrator(
            ITrueLogger<ConciliationOrchestrator> logger,
            ITelemetry telemetry,
            IConfigurationHandler configurationHandler,
            IAzureClientFactory azureClientFactory,
            IServiceProvider serviceProvider,
            IOwnershipRuleProcessor ownershipRuleProcessor,
            IConciliationProcessor conciliationProcessor,
            IFailureHandlerFactory failureHandlerFactory,
            IUnitOfWorkFactory unitOfWorkFactory)
            : base(configurationHandler, azureClientFactory, serviceProvider, logger)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            ArgumentValidators.ThrowIfNull(failureHandlerFactory, nameof(failureHandlerFactory));

            this.logger = logger;
            this.telemetry = telemetry;
            this.conciliationProcessor = conciliationProcessor;
            this.ownershipRuleProcessor = ownershipRuleProcessor;
            this.failureHandler = failureHandlerFactory.GetFailureHandler(TicketType.Conciliation);
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        }

        /// <summary>
        /// Calculate Conciliation  asynchronous.
        /// </summary>
        /// <param name="conciliationNodes">conciliationNodes.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="deliveryCount">The deliveryCount.</param>
        /// <param name="durableOrchestrationClient">The starter.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("Conciliation")]
        public async Task ConciliationAsync(
            [ServiceBusTrigger("%Conciliationqueue%", Connection = "IntegrationServiceBusConnectionString", IsSessionsEnabled = true)] ConciliationNodesResquest conciliationNodes,
            string label,
            string replyTo,
            int deliveryCount,
            [DurableClient] IDurableOrchestrationClient durableOrchestrationClient,
            ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(conciliationNodes, nameof(conciliationNodes));

            this.logger.LogInformation($"Conciliation processing is requested for ticket {conciliationNodes.TicketId}", $"{conciliationNodes.TicketId}");

            try
            {
                await this.InitializeAsync().ConfigureAwait(false);

                this.ProcessMetadata(label, FunctionNames.Conciliation, replyTo);

                this.logger.LogInformation(
                    $"Conciliation  orchestrator is triggered for ticket {conciliationNodes.TicketId} with instance id " +
                    $"{context.InvocationId}", $"{conciliationNodes.TicketId}");

                var conciliationRuleData = new ConciliationRuleData
                {
                    ConciliationNodes = conciliationNodes,
                    ChaosValue = label,
                    Caller = FunctionNames.Conciliation,
                    Orchestrator = OrchestratorNames.ConciliationOrchestrator,
                    ReplyTo = replyTo,
                };

                await this.TryStartAsync(
                    durableOrchestrationClient,
                    OrchestratorNames.ConciliationOrchestrator,
                    conciliationNodes.TicketId.ToString(CultureInfo.InvariantCulture),
                    conciliationRuleData).ConfigureAwait(false);

                this.logger.LogInformation($"The Conciliation  processing is triggered for ticket: {conciliationNodes.TicketId}", $"{conciliationNodes.TicketId}");
            }
            catch (Exception ex)
            {
                this.telemetry.TrackEvent(Constants.Critical, EventName.ConciliationFailureEvent.ToString("G"));
                this.logger.LogError(ex, Constants.ConciliationOrchestratorFailed, $"{conciliationNodes.TicketId}");
                if (deliveryCount < 10)
                {
                    throw;
                }

                await this.failureHandler.HandleFailureAsync(
                    this.unitOfWork,
                    new FailureInfo(conciliationNodes.TicketId, conciliationNodes.NodeId, Constants.ConciliationFailureMessage)).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Conciliation s the orchestrator asynchronous.
        /// </summary>
        /// <param name="orchestrationContext">The orchestrationContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("ConciliationOrchestrator")]
        public async Task ConciliationOrchestratorAsync(
        [OrchestrationTrigger] IDurableOrchestrationContext orchestrationContext)
        {
            ArgumentValidators.ThrowIfNull(orchestrationContext, nameof(orchestrationContext));
            var conciliationRuleData = orchestrationContext.GetInput<ConciliationRuleData>();
            ArgumentValidators.ThrowIfNull(conciliationRuleData.ConciliationNodes, nameof(conciliationRuleData.ConciliationNodes));
            try
            {
                this.logger.LogInformation(
                    $"Conciliation  orchestration started for ticket {conciliationRuleData.ConciliationNodes.TicketId}",
                    $"{conciliationRuleData.ConciliationNodes.TicketId}");

                this.ProcessMetadata(
                    conciliationRuleData.ChaosValue,
                    string.Join(".", conciliationRuleData.Caller, conciliationRuleData.Orchestrator),
                    conciliationRuleData.ReplyTo);

                conciliationRuleData = await this.CallActivityAsync(ActivityNames.ValidateConciliationNodeStates, orchestrationContext, conciliationRuleData).ConfigureAwait(true);
                if (conciliationRuleData.ValidateConciliationNodeStates)
                {
                    await this.CallActivityAsync(ActivityNames.DeleteConciliationMovements, orchestrationContext, conciliationRuleData).ConfigureAwait(true);
                    await this.CallActivityAsync(ActivityNames.UpdateConciliationNodes, orchestrationContext, conciliationRuleData).ConfigureAwait(true);
                    await this.CallActivityAsync(ActivityNames.DoConciliation, orchestrationContext, conciliationRuleData).ConfigureAwait(true);
                    await this.ExecuteActivityAsync(ActivityNames.CalculateOwnershipConciliationData, orchestrationContext, conciliationRuleData).ConfigureAwait(true);
                    await this.ExecuteActivityAsync(ActivityNames.FinalizeConciliation, orchestrationContext, conciliationRuleData).ConfigureAwait(true);
                }
                else
                {
                    this.logger.LogInformation(
                    $"Conciliation process cannot be run with nodes in Approved or Submitted for approval status for ticket {conciliationRuleData.ConciliationNodes.TicketId}",
                    $"{conciliationRuleData.ConciliationNodes.TicketId}");
                }

                this.logger.LogInformation(
                    $"Conciliation orchestration finished for ticket {conciliationRuleData.ConciliationNodes.TicketId}",
                    $"{conciliationRuleData.ConciliationNodes.TicketId}");
            }
            catch (Exception ex)
            {
                var error = ex.InnerException ?? ex;
                this.telemetry.TrackEvent(Constants.Critical, EventName.ConciliationFailureEvent.ToString("G"));
                this.logger.LogError(error, $"Exception occurred in conciliation  orchestration for ticket {conciliationRuleData.ConciliationNodes.TicketId}", error);
                orchestrationContext.SetCustomStatus(Constants.CustomFailureStatus);
                this.logger.LogInformation(
                    $"Calling HandleFailure activity from conciliation  orchestrator for {conciliationRuleData.ConciliationNodes.TicketId}.",
                    $"{conciliationRuleData.ConciliationNodes.TicketId}");
                await orchestrationContext.CallActivityAsync(ActivityNames.ConciliationHandleFailure, Tuple.Create(conciliationRuleData, Constants.ConciliationFailureMessage)).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Clears the data asynchronous.
        /// </summary>
        /// <param name="activityContext">The activityContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.ConciliationHandleFailure)]
        public async Task ConciliationHandleFailureAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var result = activityContext.GetInput<Tuple<ConciliationRuleData, string>>();
            this.logger.LogInformation($"Clear conciliation  data is requested for ticket: {result.Item1.ConciliationNodes.TicketId}", $"{result.Item1.ConciliationNodes.TicketId}");
            await this.InitializeAsync().ConfigureAwait(false);
            await this.failureHandler.HandleFailureAsync(
                this.unitOfWork,
                new FailureInfo(result.Item1.ConciliationNodes.TicketId, result.Item1.ConciliationNodes.NodeId, result.Item2)).ConfigureAwait(false);
        }

        /// <summary>
        /// Conciliation.
        /// </summary>
        /// <param name="activityContext">The activityContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.DoConciliation)]
        public async Task DoConciliationAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ConciliationRuleData conciliationRuleData = null;
            try
            {
                ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
                conciliationRuleData = activityContext.GetInput<ConciliationRuleData>();
                this.logger.LogInformation(
                    $"Consolidation is requested for ticket: {conciliationRuleData.ConciliationNodes.TicketId}",
                    $"{conciliationRuleData.ConciliationNodes.TicketId}");

                conciliationRuleData.Activity = ActivityNames.DoConciliation;
                await this.InitializeAsync(conciliationRuleData).ConfigureAwait(false);
                await this.conciliationProcessor.DoConciliationAsync(conciliationRuleData.ConciliationNodes).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var error = ex.InnerException ?? ex;
                this.logger.LogError(error, $"Failed to finalize the conciliation for ticket {conciliationRuleData.ConciliationNodes.TicketId}", error);
                await this.failureHandler.HandleFailureAsync(
                    this.unitOfWork,
                    new FailureInfo(conciliationRuleData.ConciliationNodes.TicketId, conciliationRuleData.ConciliationNodes.NodeId, Constants.ConciliationFailureMessage)).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Validate if exist Nodes in state APPROVED or SUBMITFORAPPROVAL.
        /// </summary>
        /// <param name="activityContext">The activityContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.ValidateConciliationNodeStates)]
        public async Task<ConciliationRuleData> ValidateConciliationNodeStateAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var conciliationRuleData = activityContext.GetInput<ConciliationRuleData>();
            this.logger.LogInformation(
                $"Validate Conciliation Nodes in state APPROVED or SUBMITFORAPPROVAL requested for ticket: {conciliationRuleData.ConciliationNodes.TicketId}",
                $"{conciliationRuleData.ConciliationNodes.TicketId}");

            conciliationRuleData.Activity = ActivityNames.ValidateConciliationNodeStates;
            await this.InitializeAsync(conciliationRuleData).ConfigureAwait(false);

            var ownershipNodeData = await this.conciliationProcessor.GetConciliationNodesAsync(
                conciliationRuleData.ConciliationNodes.TicketId,
                conciliationRuleData.ConciliationNodes.NodeId).ConfigureAwait(false);

            var conciliationNodes = ownershipNodeData.Where(x =>
                x.OwnershipStatusId == (int)OwnershipNodeStatusType.APPROVED || x.OwnershipStatusId == (int)OwnershipNodeStatusType.SUBMITFORAPPROVAL).ToList();

            conciliationRuleData.ValidateConciliationNodeStates = !conciliationNodes.Any();
            return conciliationRuleData;
        }

        /// <summary>
        /// Finalize the ownership.
        /// </summary>
        /// <param name="activityContext">The activityContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.FinalizeConciliation)]
        public async Task FinalizeConciliationAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ConciliationRuleData conciliationRuleData = null;
            try
            {
                ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
                conciliationRuleData = activityContext.GetInput<ConciliationRuleData>();
                this.logger.LogInformation(
                    $"Conciliation processing finalizer started for ticket Id {conciliationRuleData.ConciliationNodes.TicketId}",
                    $"{conciliationRuleData.ConciliationNodes.TicketId}");

                conciliationRuleData.Activity = ActivityNames.FinalizeConciliation;
                await this.InitializeAsync(conciliationRuleData).ConfigureAwait(false);

                await this.conciliationProcessor.FinalizeProcessAsync(conciliationRuleData).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var error = ex.InnerException ?? ex;
                this.logger.LogError(error, $"Failed to finalize the conciliation for ticket {conciliationRuleData?.ConciliationNodes.TicketId}", error);
                this.telemetry.TrackEvent(Constants.Critical, EventName.ConciliationFailureEvent.ToString("G"));
            }
        }

        /// <summary>
        /// Delete Status Nodes.
        /// </summary>
        /// <param name="activityContext">The activityContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.DeleteConciliationMovements)]
        public async Task DeleteConciliationMovementsAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var conciliationRuleData = activityContext.GetInput<ConciliationRuleData>();
            this.logger.LogInformation(
                $"Delete Conciliation Movements requested for ticket: {conciliationRuleData.ConciliationNodes.TicketId}",
                $"{conciliationRuleData.ConciliationNodes.TicketId}");

            conciliationRuleData.Activity = ActivityNames.DeleteConciliationMovements;
            await this.InitializeAsync(conciliationRuleData).ConfigureAwait(false);

            IEnumerable<Movement> movements = await this.conciliationProcessor.GetConciliationMovementsAsync(
                conciliationRuleData.ConciliationNodes.TicketId,
                conciliationRuleData.ConciliationNodes.NodeId).ConfigureAwait(false);

            await this.conciliationProcessor.RegisterNegativeMovementsAsync(movements).ConfigureAwait(false);
            await this.conciliationProcessor.DeleteConciliationMovementsAsync(conciliationRuleData.ConciliationNodes.TicketId, conciliationRuleData.ConciliationNodes.NodeId).ConfigureAwait(false);
            await this.conciliationProcessor.DeleteRelationshipOtherSegmentMovementsAsync(
                conciliationRuleData.ConciliationNodes.TicketId,
                conciliationRuleData.ConciliationNodes.NodeId).ConfigureAwait(false);
        }

        /// <summary>
        /// Update Conciliation Movements.
        /// </summary>
        /// <param name="activityContext">The activityContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.UpdateConciliationNodes)]
        public async Task UpdateConciliationNodesAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var conciliationRuleData = activityContext.GetInput<ConciliationRuleData>();
            this.logger.LogInformation(
                $"Update Conciliation Movements requested for ticket: {conciliationRuleData.ConciliationNodes.TicketId}",
                $"{conciliationRuleData.ConciliationNodes.TicketId}");

            conciliationRuleData.Activity = ActivityNames.UpdateConciliationNodes;
            await this.InitializeAsync(conciliationRuleData).ConfigureAwait(false);

            await this.conciliationProcessor.UpdateOwnershipNodeAsync(
                conciliationRuleData.ConciliationNodes.TicketId,
                StatusType.PROCESSED,
                OwnershipNodeStatusType.OWNERSHIP,
                conciliationRuleData.ConciliationNodes.NodeId).ConfigureAwait(false);
        }

        /// <summary>
        /// Calculates the ownership.
        /// </summary>
        /// <param name="activityContext">The activityContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.CalculateOwnershipConciliationData)]
        public async Task CalculateOwnershipAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var conciliationRuleData = activityContext.GetInput<ConciliationRuleData>();
            this.logger.LogInformation(
                $"Calculate ownership balance is requested for ticket: {conciliationRuleData.ConciliationNodes.TicketId}",
                $"{conciliationRuleData.ConciliationNodes.TicketId}");

            OwnershipRuleData ownershipRuleData = new OwnershipRuleData
            {
                Activity = ActivityNames.CalculateOwnershipConciliationData,
                TicketId = conciliationRuleData.ConciliationNodes.TicketId,
                Errors = new List<ErrorInfo>(),
                Caller = FunctionNames.Conciliation,
                Orchestrator = OrchestratorNames.ConciliationOrchestrator,
            };
            await this.InitializeAsync(conciliationRuleData).ConfigureAwait(false);

            await this.ownershipRuleProcessor.ProcessAsync(ownershipRuleData, ChainType.CalculateOwnershipData).ConfigureAwait(false);
        }
    }
}