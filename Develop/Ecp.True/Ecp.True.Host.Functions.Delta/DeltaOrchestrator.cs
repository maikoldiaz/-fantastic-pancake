// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaOrchestrator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Delta
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.HandleFailure;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;

    /// <summary>
    /// The DeltaOrchestrator.
    /// </summary>
    public class DeltaOrchestrator : OrchestratorBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<DeltaOrchestrator> logger;

        /// <summary>
        /// The failure handler.
        /// </summary>
        private readonly IFailureHandler failureHandler;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The delta processor.
        /// </summary>
        private readonly IDeltaProcessor deltaProcessor;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaOrchestrator" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="failureHandlerFactory">The failureHandlerFactory.</param>
        /// <param name="unitOfWorkFactory">The unit of work.</param>
        /// <param name="deltaProcessor">The delta processor.</param>
        /// <param name="failureHandler">The failure Handler.</param>
        public DeltaOrchestrator(
            ITrueLogger<DeltaOrchestrator> logger,
            ITelemetry telemetry,
            IConfigurationHandler configurationHandler,
            IAzureClientFactory azureClientFactory,
            IServiceProvider serviceProvider,
            IFailureHandlerFactory failureHandlerFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            IDeltaProcessor deltaProcessor)
            : base(configurationHandler, azureClientFactory, serviceProvider, logger)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            ArgumentValidators.ThrowIfNull(failureHandlerFactory, nameof(failureHandlerFactory));

            this.logger = logger;
            this.telemetry = telemetry;
            this.failureHandler = failureHandlerFactory.GetFailureHandler(TicketType.Delta);
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.deltaProcessor = deltaProcessor;
        }

        /// <summary>
        /// Calculates the delta asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="deliveryCount">The delivery count.</param>
        /// <param name="durableOrchestrationClient">The durable orchestration client.</param>
        /// <param name="context">The context.</param>
        /// <returns>The task.</returns>
        [FunctionName("CalculateDelta")]
        public async Task CalculateDeltaAsync(
            [ServiceBusTrigger("%Delta%", Connection = "IntegrationServiceBusConnectionString", IsSessionsEnabled = true)] int ticketId,
            string label,
            string replyTo,
            int deliveryCount,
            [DurableClient] IDurableOrchestrationClient durableOrchestrationClient,
            ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(ticketId, nameof(ticketId));

            this.logger.LogInformation($"Delta processing is requested for ticket {ticketId}", $"{ticketId}");

            try
            {
                var deltaData = new DeltaData
                {
                    ChaosValue = label,
                    Caller = FunctionNames.Delta,
                    ReplyTo = replyTo,
                    Orchestrator = OrchestratorNames.DeltaOrchestrator,
                };

                await this.InitializeAsync().ConfigureAwait(false);
                this.ProcessMetadata(label, FunctionNames.Delta, replyTo);

                (bool isValid, Ticket ticket) = await this.deltaProcessor.ValidateTicketAsync(ticketId).ConfigureAwait(false);
                if (!isValid)
                {
                    if (ticket != null)
                    {
                        await this.failureHandler.HandleFailureAsync(
                       this.unitOfWork,
                       new FailureInfo(ticketId, "Cutoff/Ownership tickets for the selected segments are already processing.")).ConfigureAwait(false);
                    }

                    return;
                }

                this.logger.LogInformation($"Delta orchestrator is triggered for ticket {ticketId} with instance id {context.InvocationId}", $"{ticketId}");

                deltaData.Ticket = ticket;

                await this.TryStartAsync(durableOrchestrationClient, "DeltaOrchestrator", ticketId.ToString(CultureInfo.InvariantCulture), deltaData).ConfigureAwait(false);
                this.logger.LogInformation($"The Delta processing is triggered for ticket: {ticketId}", $"{ticketId}");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, Constants.DeltaFailureMessage, $"{ticketId}");
                if (deliveryCount < 10)
                {
                    throw;
                }

                // Get handler and handle failure.
                await this.failureHandler.HandleFailureAsync(this.unitOfWork, new FailureInfo(ticketId, Constants.DeltaFailureMessage)).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Ownerships the orchestrator asynchronous.
        /// </summary>
        /// <param name="orchestrationContext">The orchestrationContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("DeltaOrchestrator")]
        public async Task DeltaOrchestratorAsync(
        [OrchestrationTrigger] IDurableOrchestrationContext orchestrationContext)
        {
            ArgumentValidators.ThrowIfNull(orchestrationContext, nameof(orchestrationContext));
            var deltaData = orchestrationContext.GetInput<DeltaData>();
            try
            {
                this.logger.LogInformation($"Delta orchestration started for ticket {deltaData.Ticket.TicketId}", $"{deltaData.Ticket.TicketId}");

                this.ProcessMetadata(deltaData.ChaosValue, string.Join(".", deltaData.Caller, deltaData.Orchestrator), deltaData.ReplyTo);

                var initialDeltaData = await this.CallActivityAsync(ActivityNames.GetDelta, orchestrationContext, deltaData).ConfigureAwait(true);
                var requestedDeltaData = await this.CallActivityAsync(ActivityNames.RequestDelta, orchestrationContext, initialDeltaData).ConfigureAwait(true);
                var processedDeltaData = await this.CallActivityAsync(ActivityNames.ProcessDelta, orchestrationContext, requestedDeltaData).ConfigureAwait(true);
                var completedDeltaData = await this.CallActivityAsync(ActivityNames.CompleteDelta, orchestrationContext, processedDeltaData).ConfigureAwait(true);

                if (completedDeltaData != null && !completedDeltaData.HasProcessingErrors)
                {
                    await this.ExecuteActivityAsync(ActivityNames.FinalizeDelta, orchestrationContext, deltaData).ConfigureAwait(true);
                }

                this.logger.LogInformation($"Delta orchestration finished for ticket {deltaData.Ticket.TicketId}", $"{deltaData.Ticket.TicketId}");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, Constants.DeltaFailureMessage, deltaData.Ticket.TicketId);
                orchestrationContext.SetCustomStatus(Constants.CustomFailureStatus);
                this.logger.LogInformation($"Calling HandleFailure activity from delta orchestrator for {deltaData.Ticket.TicketId}.", $"{deltaData.Ticket.TicketId}");
                await orchestrationContext.CallActivityAsync(ActivityNames.HandleFailure, Tuple.Create(deltaData, Constants.DeltaFailureMessage)).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Get the delta.
        /// </summary>
        /// <param name="activityContext">The activityContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.GetDelta)]
        public async Task<DeltaData> GetDeltaAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var deltaData = activityContext.GetInput<DeltaData>();

            this.logger.LogInformation($"Getting Delta data for ticket: {deltaData.Ticket.TicketId}", $"{deltaData.Ticket.TicketId}");

            deltaData.Activity = ActivityNames.GetDelta;
            await this.InitializeAsync(deltaData).ConfigureAwait(false);

            return await this.deltaProcessor.ProcessAsync(deltaData, ChainType.GetDelta).ConfigureAwait(false);
        }

        /// <summary>
        /// Request delta asynchronous.
        /// </summary>
        /// <param name="activityContext">The activity context.</param>
        /// <returns>The task.</returns>
        [FunctionName(ActivityNames.RequestDelta)]
        public async Task<DeltaData> RequestDeltaAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var deltaData = activityContext.GetInput<DeltaData>();

            this.logger.LogInformation($"Requesting delta for ticket: {deltaData.Ticket.TicketId}", $"{deltaData.Ticket.TicketId}");

            deltaData.Activity = ActivityNames.RequestDelta;
            await this.InitializeAsync(deltaData).ConfigureAwait(false);

            return await this.deltaProcessor.ProcessAsync(deltaData, ChainType.RequestDelta).ConfigureAwait(false);
        }

        /// <summary>
        /// Process delta asynchronous.
        /// </summary>
        /// <param name="activityContext">The activity context.</param>
        /// <returns>The task.</returns>
        [FunctionName(ActivityNames.ProcessDelta)]
        public async Task<DeltaData> ProcessDeltaAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var deltaData = activityContext.GetInput<DeltaData>();

            this.logger.LogInformation($"Processing delta for ticket: {deltaData.Ticket.TicketId}", $"{deltaData.Ticket.TicketId}");

            deltaData.Activity = ActivityNames.ProcessDelta;
            await this.InitializeAsync(deltaData).ConfigureAwait(false);

            return await this.deltaProcessor.ProcessAsync(deltaData, ChainType.ProcessDelta).ConfigureAwait(false);
        }

        /// <summary>
        /// Complete delta.
        /// </summary>
        /// <param name="activityContext">The activityContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.CompleteDelta)]
        public async Task CompleteDeltaAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var deltaData = activityContext.GetInput<DeltaData>();

            this.logger.LogInformation($"Completing delta for ticket: {deltaData.Ticket.TicketId}", $"{deltaData.Ticket.TicketId}");

            deltaData.Activity = ActivityNames.CompleteDelta;
            await this.InitializeAsync(deltaData).ConfigureAwait(false);

            await this.deltaProcessor.ProcessAsync(deltaData, ChainType.CompleteDelta).ConfigureAwait(false);
        }

        /// <summary>
        /// Finalizes the cut off asynchronous.
        /// </summary>
        /// <param name="activityContext">The activity context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.FinalizeDelta)]
        public async Task FinalizeDeltaAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var deltaData = activityContext.GetInput<DeltaData>();
            try
            {
                this.logger.LogInformation($"Delta processing finalizer started for ticket Id {deltaData.Ticket.TicketId}", $"{deltaData.Ticket.TicketId}");

                deltaData.Activity = ActivityNames.FinalizeDelta;
                await this.InitializeAsync(deltaData).ConfigureAwait(false);

                await this.deltaProcessor.FinalizeProcessAsync(deltaData.Ticket.TicketId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var error = ex.InnerException ?? ex;
                this.logger.LogError(error, $"Failed to finalize the delta. {error.Message}", $"{deltaData.Ticket.TicketId}");
                this.telemetry.TrackEvent(Constants.Critical, EventName.OperativeDeltaFailureEvent.ToString("G"));
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
            var result = activityContext.GetInput<Tuple<DeltaData, string>>();

            this.logger.LogInformation($"Handling delta processing failure for ticket: {result.Item1.Ticket.TicketId}", $"{result.Item1.Ticket.TicketId}");
            await this.InitializeAsync().ConfigureAwait(false);
            await this.failureHandler.HandleFailureAsync(this.unitOfWork, new FailureInfo(result.Item1.Ticket.TicketId, result.Item2)).ConfigureAwait(false);
        }

        /// <summary>
        /// Purges delta history asynchronous.
        /// </summary>
        /// <param name="timer">The timer.</param>
        /// <param name="durableOrchestrationClient">The durable orchestration client.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("PurgeDeltaHistory")]
        public async Task PurgeDeltaHistoryAsync(
            [TimerTrigger("%DeltaPurgeInterval%")] TimerInfo timer,
            [DurableClient] IDurableOrchestrationClient durableOrchestrationClient,
            ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(timer, nameof(timer));
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(durableOrchestrationClient, nameof(durableOrchestrationClient));

            this.logger.LogInformation($"Purge delta history function triggered with schedule: {timer.Schedule}", Constants.PurgeDeltaHistoryKey);
            try
            {
                await this.InitializeAsync().ConfigureAwait(false);
                this.logger.LogInformation($"The purge delta history job has started with schedule: {timer.Schedule}", Constants.PurgeDeltaHistoryKey);
                var tasks = new List<Task>
                {
                    this.PurgeOrchestrationDataAsync(durableOrchestrationClient, Constants.PurgeDeltaHistoryKey, Constants.PurgingDeltaMessage),
                    this.DoHandleFailureAsync(durableOrchestrationClient, this.unitOfWork, this.failureHandler, Constants.DeltaFailureMessage),
                };
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message, Constants.PurgeDeltaHistoryKey);
            }

            this.logger.LogInformation($"The purge delta history job has finished at: {DateTime.UtcNow.ToTrue()}", Constants.PurgeDeltaHistoryKey);
        }
    }
}