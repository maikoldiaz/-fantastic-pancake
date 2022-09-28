// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipOrchestrator.cs" company="Microsoft">
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
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Logging;
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
    /// The OwnershipGenerator.
    /// </summary>
    public class OwnershipOrchestrator : OrchestratorBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<OwnershipOrchestrator> logger;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// The ownership rule processor.
        /// </summary>
        private readonly IOwnershipRuleProcessor ownershipRuleProcessor;

        /// <summary>
        /// The  processor.
        /// </summary>
        private readonly IConciliationProcessor conciliationProcessor;

        /// <summary>
        /// The ownership failure handler.
        /// </summary>
        private readonly IFailureHandler ownershipFailureHandler;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipOrchestrator"/> class.
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
        public OwnershipOrchestrator(
            ITrueLogger<OwnershipOrchestrator> logger,
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
            this.ownershipRuleProcessor = ownershipRuleProcessor;
            this.conciliationProcessor = conciliationProcessor;
            this.ownershipFailureHandler = failureHandlerFactory.GetFailureHandler(TicketType.Ownership);
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        }

        /// <summary>
        /// Calculate the ownership asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket Id.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="deliveryCount">The deliveryCount.</param>
        /// <param name="durableOrchestrationClient">The starter.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("ProcessOwnership")]
        public async Task CalculateOwnershipsAsync(
            [ServiceBusTrigger("%Ownership%", Connection = "IntegrationServiceBusConnectionString", IsSessionsEnabled = true)] int ticketId,
            string label,
            string replyTo,
            int deliveryCount,
            [DurableClient] IDurableOrchestrationClient durableOrchestrationClient,
            ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(ticketId, nameof(ticketId));

            this.logger.LogInformation($"Ownership processing is requested for ticket {ticketId}", $"{ticketId}");

            try
            {
                await this.InitializeAsync().ConfigureAwait(false);

                this.ProcessMetadata(label, FunctionNames.Ownership, replyTo);

                ////// Cleaning the ownership data
                ////this.logger.LogInformation($"Ownership clean up is triggered for ticket {ticketId} with instance id {context.InvocationId}", $"{ticketId}");
                ////await this.ownershipRuleProcessor.CleanOwnershipDataAsync(ticketId).ConfigureAwait(false);

                this.logger.LogInformation($"Ownership orchestrator is triggered for ticket {ticketId} with instance id {context.InvocationId}", $"{ticketId}");
                var ownershipRuleData = new OwnershipRuleData
                {
                    TicketId = ticketId,
                    Errors = new List<ErrorInfo>(),
                    ChaosValue = label,
                    Caller = FunctionNames.Ownership,
                    Orchestrator = OrchestratorNames.OwnershipOrchestrator,
                    ReplyTo = replyTo,
                };

                await this.TryStartAsync(durableOrchestrationClient, OrchestratorNames.OwnershipOrchestrator, ticketId.ToString(CultureInfo.InvariantCulture), ownershipRuleData).ConfigureAwait(false);
                this.logger.LogInformation($"The Ownership processing is triggered for ticket: {ticketId}", $"{ticketId}");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, Constants.OwnershipOrchestratorFailed, $"{ticketId}");
                if (deliveryCount < 10)
                {
                    throw;
                }

                await this.ownershipFailureHandler.HandleFailureAsync(this.unitOfWork, new FailureInfo(ticketId, Constants.OwnershipFailureMessage)).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Ownerships the orchestrator asynchronous.
        /// </summary>
        /// <param name="orchestrationContext">The orchestrationContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("OwnershipOrchestrator")]
        public async Task OwnershipOrchestratorAsync(
        [OrchestrationTrigger] IDurableOrchestrationContext orchestrationContext)
        {
            ArgumentValidators.ThrowIfNull(orchestrationContext, nameof(orchestrationContext));
            var ownershipRuleData = orchestrationContext.GetInput<OwnershipRuleData>();
            try
            {
                this.logger.LogInformation($"Ownership orchestration started for ticket {ownershipRuleData.TicketId}", $"{ownershipRuleData.TicketId}");

                this.ProcessMetadata(ownershipRuleData.ChaosValue, string.Join(".", ownershipRuleData.Caller, ownershipRuleData.Orchestrator), ownershipRuleData.ReplyTo);

                var analyticsOwnershipRuleData = await this.CallActivityAsync(ActivityNames.ProcessAnalytics, orchestrationContext, ownershipRuleData).ConfigureAwait(true);
                var requestOwnershipRuleData = await this.CallActivityAsync(ActivityNames.RequestOwnershipData, orchestrationContext, analyticsOwnershipRuleData)
                                               .ConfigureAwait(true);
                if (!requestOwnershipRuleData.Errors.Any() && !requestOwnershipRuleData.HasProcessingErrors)
                {
                    var registeredOwnershipRuleData = await this.CallActivityAsync(ActivityNames.Register, orchestrationContext, requestOwnershipRuleData).ConfigureAwait(true);

                    try
                    {
                        await this.ExecuteActivityAsync(ActivityNames.ConciliationOwnership, orchestrationContext, ownershipRuleData).ConfigureAwait(true);
                    }
                    catch (Exception ex)
                    {
                        var error = ex.InnerException ?? ex;
                        this.logger.LogError(error, $"Failed to finalize the ownership for ticketId. {ownershipRuleData.TicketId}", $"{ownershipRuleData.TicketId}");
                        this.telemetry.TrackEvent(Constants.Critical, EventName.OwnershipFailureEvent.ToString("G"));

                        try
                        {
                            await this.ExecuteActivityAsync(ActivityNames.DeleteConciliationMovementsOwnership, orchestrationContext, ownershipRuleData).ConfigureAwait(true);
                            await this.ExecuteActivityAsync(ActivityNames.UpdateConciliationOwnershipStatus, orchestrationContext, ownershipRuleData).ConfigureAwait(true);
                        }
                        catch (Exception exe)
                        {
                            var errorDelete = exe.InnerException ?? exe;
                            this.logger.LogError(errorDelete, $"Failed to finalize delete Conciliation Movements for ticket {ownershipRuleData.TicketId}", errorDelete);
                        }
                    }

                    await this.ExecuteActivityAsync(ActivityNames.CalculateOwnershipData, orchestrationContext, registeredOwnershipRuleData).ConfigureAwait(true);
                    try
                    {
                        await this.ExecuteActivityAsync(ActivityNames.FinalizeOwnership, orchestrationContext, ownershipRuleData).ConfigureAwait(true);
                    }
                    catch (Exception ex)
                    {
                        var error = ex.InnerException ?? ex;
                        this.logger.LogError(error, $"Failed to finalize the ownership for ticketId. {ownershipRuleData.TicketId}", $"{ownershipRuleData.TicketId}");
                        this.telemetry.TrackEvent(Constants.Critical, EventName.OwnershipFailureEvent.ToString("G"));
                    }
                }

                this.logger.LogInformation($"Ownership orchestration finished for ticket {ownershipRuleData.TicketId}", $"{ownershipRuleData.TicketId}");
            }
            catch (Exception ex)
            {
                var error = ex.InnerException ?? ex;
                this.logger.LogError(error, $"Exception occurred in ownership orchestration for ticket {ownershipRuleData.TicketId}", error);
                orchestrationContext.SetCustomStatus(Constants.CustomFailureStatus);
                this.logger.LogInformation($"Calling HandleFailure activity from ownership orchestrator for {ownershipRuleData.TicketId}.", $"{ownershipRuleData.TicketId}");
                await orchestrationContext.CallActivityAsync(ActivityNames.HandleFailure, Tuple.Create(ownershipRuleData, Constants.OwnershipFailureMessage)).ConfigureAwait(true);
                try
                {
                    await this.ExecuteActivityAsync(ActivityNames.DeleteConciliationMovementsOwnership, orchestrationContext, ownershipRuleData).ConfigureAwait(true);
                }
                catch (Exception exe)
                {
                    var errorDelete = exe.InnerException ?? exe;
                    this.logger.LogError(errorDelete, $"Failed delete Conciliation Movements for ticket {ownershipRuleData.TicketId}", errorDelete);
                }
            }
        }

        /// <summary>
        /// Processes the analytics.
        /// </summary>
        /// <param name="activityContext">The activityContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.ProcessAnalytics)]
        public async Task<OwnershipRuleData> ProcessAnalyticsAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var ownershipRuleData = activityContext.GetInput<OwnershipRuleData>();
            this.logger.LogInformation($"Process analytics is requested for ticket: {ownershipRuleData.TicketId}", $"{ownershipRuleData.TicketId}");

            ownershipRuleData.Activity = ActivityNames.ProcessAnalytics;
            await this.InitializeAsync(ownershipRuleData).ConfigureAwait(false);

            return await this.ownershipRuleProcessor.ProcessAsync(ownershipRuleData, ChainType.ProcessAnalytics).ConfigureAwait(false);
        }

        /// <summary>
        /// Processes the ownership.
        /// </summary>
        /// <param name="activityContext">The activityContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.RequestOwnershipData)]
        public async Task<OwnershipRuleData> ProcessOwnershipAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var ownershipRuleData = activityContext.GetInput<OwnershipRuleData>();
            this.logger.LogInformation($"Process ownership is requested for ticket: {ownershipRuleData.TicketId}", $"{ownershipRuleData.TicketId}");

            ownershipRuleData.Activity = ActivityNames.RequestOwnershipData;
            await this.InitializeAsync(ownershipRuleData).ConfigureAwait(false);

            return await this.ownershipRuleProcessor.ProcessAsync(ownershipRuleData, ChainType.RequestOwnershipData).ConfigureAwait(false);
        }

        /// <summary>
        /// Register ownership.
        /// </summary>
        /// <param name="activityContext">The activityContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.Register)]
        public async Task<OwnershipRuleData> BuildOwnershipAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var ownershipRuleData = activityContext.GetInput<OwnershipRuleData>();
            this.logger.LogInformation($"Register ownership is requested for ticket: {ownershipRuleData.TicketId}", $"{ownershipRuleData.TicketId}");

            ownershipRuleData.Activity = ActivityNames.Register;
            await this.InitializeAsync(ownershipRuleData).ConfigureAwait(false);

            return await this.ownershipRuleProcessor.ProcessAsync(ownershipRuleData, ChainType.Register).ConfigureAwait(false);
        }

        /// <summary>
        /// Calculates the ownership.
        /// </summary>
        /// <param name="activityContext">The activityContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.CalculateOwnershipData)]
        public async Task CalculateOwnershipAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var ownershipRuleData = activityContext.GetInput<OwnershipRuleData>();
            this.logger.LogInformation($"Calculate ownership is requested for ticket: {ownershipRuleData.TicketId}", $"{ownershipRuleData.TicketId}");

            ownershipRuleData.Activity = ActivityNames.CalculateOwnershipData;
            await this.InitializeAsync(ownershipRuleData).ConfigureAwait(false);

            await this.ownershipRuleProcessor.ProcessAsync(ownershipRuleData, ChainType.CalculateOwnershipData).ConfigureAwait(false);
        }

        /// <summary>
        /// Finalize the ownership.
        /// </summary>
        /// <param name="activityContext">The activityContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.FinalizeOwnership)]
        public async Task FinalizeOwnershipAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            OwnershipRuleData ownershipRuleData = activityContext.GetInput<OwnershipRuleData>();

            try
            {
                this.logger.LogInformation($"Ownership processing finalizer started for ticket Id {ownershipRuleData.TicketId}", $"{ownershipRuleData.TicketId}");

                ownershipRuleData.Activity = ActivityNames.FinalizeOwnership;
                await this.InitializeAsync(ownershipRuleData).ConfigureAwait(false);

                await this.ownershipRuleProcessor.FinalizeProcessAsync(ownershipRuleData).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var error = ex.InnerException ?? ex;
                this.logger.LogError(error, $"Failed to finalize the ownership for ticket {ownershipRuleData.TicketId}", error);
                this.telemetry.TrackEvent(Constants.Critical, EventName.OwnershipFailureEvent.ToString("G"));
            }
        }

        /// <summary>
        /// Clears the data asynchronous.
        /// </summary>
        /// <param name="activityContext">The activityContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.HandleFailure)]
        public async Task HandleFailureAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var result = activityContext.GetInput<Tuple<OwnershipRuleData, string>>();
            this.logger.LogInformation($"Clear ownership data is requested for ticket: {result.Item1.TicketId}", $"{result.Item1.TicketId}");
            await this.InitializeAsync().ConfigureAwait(false);
            await this.ownershipFailureHandler.HandleFailureAsync(this.unitOfWork, new FailureInfo(result.Item1.TicketId, result.Item2)).ConfigureAwait(false);
        }

        /// <summary>
        /// Purges ownership history asynchronous.
        /// </summary>
        /// <param name="timer">The timer.</param>
        /// <param name="durableOrchestrationClient">The durable orchestration client.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("PurgeOwnershipHistory")]
        public async Task PurgeOwnershipHistoryAsync(
            [TimerTrigger("%OwnershipPurgeInterval%")] TimerInfo timer,
            [DurableClient] IDurableOrchestrationClient durableOrchestrationClient,
            ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(timer, nameof(timer));
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(durableOrchestrationClient, nameof(durableOrchestrationClient));

            this.logger.LogInformation($"Purge ownership history function triggered with schedule: {timer.Schedule}", Constants.PurgeOwnershipHistoryKey);
            try
            {
                await this.InitializeAsync().ConfigureAwait(false);
                this.logger.LogInformation($"The purge ownership history job has started with schedule: {timer.Schedule}", Constants.PurgeOwnershipHistoryKey);
                var tasks = new List<Task>
                {
                    this.PurgeOrchestrationDataAsync(durableOrchestrationClient, Constants.PurgeOwnershipHistoryKey, Constants.PurgingOwnershipMessage),
                    this.DoHandleFailureAsync(durableOrchestrationClient, this.unitOfWork, this.ownershipFailureHandler, Constants.OwnershipFailureMessage),
                };
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message, Constants.PurgeOwnershipHistoryKey);
            }

            this.logger.LogInformation($"The purge ownership history job has finished at: {DateTime.UtcNow.ToTrue()}", Constants.PurgeOwnershipHistoryKey);
        }

        /// <summary>
        /// Conciliation.
        /// </summary>
        /// <param name="activityContext">The activityContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.ConciliationOwnership)]
        public async Task ConciliationOwnershipAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            OwnershipRuleData ownershipRuleData = null;
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            ownershipRuleData = activityContext.GetInput<OwnershipRuleData>();
            this.logger.LogInformation(
                $"Conciliation is requested for ticket: {ownershipRuleData.TicketId}",
                $"{ownershipRuleData.TicketId}");

            ownershipRuleData.Activity = ActivityNames.ConciliationOwnership;
            await this.InitializeAsync(ownershipRuleData).ConfigureAwait(false);
            await this.conciliationProcessor.DoConciliationAsync(new Entities.Dto.ConciliationNodesResquest { TicketId = ownershipRuleData.TicketId }).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete Status Nodes.
        /// </summary>
        /// <param name="activityContext">The activityContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.DeleteConciliationMovementsOwnership)]
        public async Task DeleteConciliationMovementsAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            OwnershipRuleData ownershipRuleData = null;
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            ownershipRuleData = activityContext.GetInput<OwnershipRuleData>();

            this.logger.LogInformation($"Delete Conciliation Movements requested for ticket: {ownershipRuleData.TicketId}", $"{ownershipRuleData.TicketId}");
            ownershipRuleData.Activity = ActivityNames.DeleteConciliationMovements;
            await this.InitializeAsync(ownershipRuleData).ConfigureAwait(false);
            IEnumerable<Movement> movements = await this.conciliationProcessor.GetConciliationMovementsAsync(ownershipRuleData.TicketId, null).ConfigureAwait(false);
            await this.conciliationProcessor.RegisterNegativeMovementsAsync(movements).ConfigureAwait(false);
            await this.conciliationProcessor.DeleteRelationshipOtherSegmentMovementsAsync(ownershipRuleData.TicketId, null).ConfigureAwait(false);
            await this.conciliationProcessor.DeleteConciliationMovementsAsync(ownershipRuleData.TicketId, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete Status Nodes.
        /// </summary>
        /// <param name="activityContext">The activityContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.UpdateConciliationOwnershipStatus)]
        public async Task UpdateConciliationOwnershipStatusAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            OwnershipRuleData ownershipRuleData = null;
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            ownershipRuleData = activityContext.GetInput<OwnershipRuleData>();

            this.logger.LogInformation($"Update Conciliation Ownership Status requested for ticket: {ownershipRuleData.TicketId}", $"{ownershipRuleData.TicketId}");
            ownershipRuleData.Activity = ActivityNames.UpdateConciliationOwnershipStatus;
            await this.InitializeAsync(ownershipRuleData).ConfigureAwait(false);
            await this.conciliationProcessor.UpdateStatusTicketAsync(
                ownershipRuleData.TicketId,
                Entities.Core.StatusType.CONCILIATIONFAILED,
                Constants.ConciliationFailureMessage).ConfigureAwait(false);
            await this.conciliationProcessor.UpdateOwnershipNodeAsync(
                ownershipRuleData.TicketId,
                Entities.Core.StatusType.CONCILIATIONFAILED,
                OwnershipNodeStatusType.CONCILIATIONFAILED,
                null).ConfigureAwait(false);
        }
    }
}