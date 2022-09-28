// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaOrchestrator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// ---------

namespace Ecp.True.Host.Functions.Delta
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
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
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;

    /// <summary>
    /// The Official Delta Orchestrator.
    /// </summary>
    public class OfficialDeltaOrchestrator : OrchestratorBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<OfficialDeltaOrchestrator> logger;

        /// <summary>
        /// The failure handler.
        /// </summary>
        private readonly IFailureHandler failureHandler;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The official delta processor.
        /// </summary>
        private readonly IOfficialDeltaProcessor officialDeltaProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="OfficialDeltaOrchestrator"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="failureHandlerFactory">The failure handler factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="officialDeltaProcessor">The official delta processor.</param>
        public OfficialDeltaOrchestrator(
            ITrueLogger<OfficialDeltaOrchestrator> logger,
            IConfigurationHandler configurationHandler,
            IAzureClientFactory azureClientFactory,
            IServiceProvider serviceProvider,
            IFailureHandlerFactory failureHandlerFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            IOfficialDeltaProcessor officialDeltaProcessor)
            : base(configurationHandler, azureClientFactory, serviceProvider, logger)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            ArgumentValidators.ThrowIfNull(failureHandlerFactory, nameof(failureHandlerFactory));

            this.logger = logger;
            this.failureHandler = failureHandlerFactory.GetFailureHandler(TicketType.OfficialDelta);
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.officialDeltaProcessor = officialDeltaProcessor;
        }

        /// <summary>
        ///  official delta processor.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="deliveryCount">The delivery count.</param>
        /// <param name="durableOrchestrationClient">The durable orchestration client.</param>
        /// <param name="context">The context.</param>
        /// <returns>The task.</returns>
        [FunctionName("OfficialDelta")]
        public async Task OfficialDeltaAsync(
           [ServiceBusTrigger("%OfficialDelta%", Connection = "IntegrationServiceBusConnectionString", IsSessionsEnabled = true)] int ticketId,
           string label,
           string replyTo,
           int deliveryCount,
           [DurableClient] IDurableOrchestrationClient durableOrchestrationClient,
           ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(ticketId, nameof(ticketId));

            this.logger.LogInformation($"Official delta  processing is requested for ticket {ticketId}", $"{ticketId}");

            try
            {
                var officialDeltaData = new OfficialDeltaData
                {
                    ChaosValue = label,
                    Caller = FunctionNames.OfficialDelta,
                    ReplyTo = replyTo,
                    Orchestrator = OrchestratorNames.OfficialDeltaDataOrchestrator,
                };

                await this.InitializeAsync().ConfigureAwait(false);
                this.ProcessMetadata(label, FunctionNames.OfficialDelta, replyTo);

                (bool isValid, Ticket ticket, string errorMessage) = await this.officialDeltaProcessor.ValidateTicketAsync(ticketId).ConfigureAwait(false);
                if (!isValid)
                {
                    this.logger.LogInformation($"Official delta  processing validation failed for {ticketId}", $"{ticketId}");
                    if (ticket != null)
                    {
                        await this.failureHandler.HandleFailureAsync(
                       this.unitOfWork,
                       new FailureInfo(ticketId, errorMessage)).ConfigureAwait(false);
                    }

                    return;
                }

                this.logger.LogInformation($"Official delta orchestrator is triggered for ticket {ticketId}", $"{ticketId}");

                officialDeltaData.Ticket = ticket;

                await this.TryStartAsync(
                     durableOrchestrationClient,
                     OrchestratorNames.OfficialDeltaDataOrchestrator,
                     $"{ticketId.ToString(CultureInfo.InvariantCulture)}_officialdelta",
                     officialDeltaData).ConfigureAwait(false);

                this.logger.LogInformation($"official delta  processing is triggered for ticket: {ticketId}", $"{ticketId}");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Exception occurred in official delta processing. {ex.Message}", $"{ticketId}");
                if (deliveryCount < 10)
                {
                    throw;
                }

                // Get handler and handle failure.
                await this.failureHandler.HandleFailureAsync(this.unitOfWork, new FailureInfo(ticketId, Constants.OfficialDeltaFailureMessage)).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// the delta data Orchestrator asynchronous.
        /// </summary>
        /// <param name="orchestrationContext">The orchestration context.</param>
        /// <returns>The task.</returns>
        [FunctionName("OfficialDeltaDataOrchestrator")]
        public async Task OfficialDeltaDataOrchestratorAsync(
        [OrchestrationTrigger] IDurableOrchestrationContext orchestrationContext)
        {
            ArgumentValidators.ThrowIfNull(orchestrationContext, nameof(orchestrationContext));
            var officialDelta = orchestrationContext.GetInput<OfficialDeltaData>();

            this.logger.LogInformation($"Official Delta orchestration Started for ticket {officialDelta.Ticket.TicketId}", $"{officialDelta.Ticket.TicketId}");
            try
            {
                var excludeData = officialDelta;
                var excludedDataWithMovementsToDelete = officialDelta;
                try
                {
                    this.ProcessMetadata(officialDelta.ChaosValue, string.Join(".", officialDelta.Caller, officialDelta.Orchestrator), officialDelta.ReplyTo);

                    var officialBuildData = await this.CallActivityAsync(ActivityNames.BuildOfficialData, orchestrationContext, officialDelta).ConfigureAwait(true);
                    excludeData = await this.CallActivityAsync(ActivityNames.ExcludeData, orchestrationContext, officialBuildData).ConfigureAwait(true);
                    if (excludeData.PendingOfficialInventories.Any() || excludeData.PendingOfficialMovements.Any())
                    {
                        excludedDataWithMovementsToDelete = await this.CallActivityAsync(ActivityNames.RegisterNodeActivity, orchestrationContext, excludeData).ConfigureAwait(true);
                    }
                }
                catch (Exception ex)
                {
                    await this.RegisterFailureAsync(orchestrationContext, officialDelta, ex, Constants.OfficialDeltaMovInvIdentificationFailureMessage).ConfigureAwait(false);
                }

                if (excludeData.PendingOfficialInventories.Any() || excludeData.PendingOfficialMovements.Any())
                {
                    var requestedDeltaData = await this.CallActivityAsync(ActivityNames.RequestOfficialDelta, orchestrationContext, excludedDataWithMovementsToDelete).ConfigureAwait(true);
                    var processedDeltaData = await this.CallActivityAsync(ActivityNames.ProcessOfficialDelta, orchestrationContext, requestedDeltaData).ConfigureAwait(true);

                    if (processedDeltaData is { HasProcessingErrors: false })
                    {
                        var registeredDeltaData = await this.CallActivityAsync(ActivityNames.RegisterMovementsOfficialDelta, orchestrationContext, processedDeltaData).ConfigureAwait(true);
                        try
                        {
                            await this.ExecuteActivityAsync(ActivityNames.CalculateOfficialDelta, orchestrationContext, registeredDeltaData).ConfigureAwait(true);
                            await this.ExecuteActivityAsync(ActivityNames.CompleteOfficialDelta, orchestrationContext, officialDelta).ConfigureAwait(true);
                        }
                        catch (Exception ex)
                        {
                            await this.RegisterFailureAsync(orchestrationContext, officialDelta, ex, Constants.OfficialDeltaCalculationFailureMessage).ConfigureAwait(false);
                        }
                    }

                    this.logger.LogInformation($"Official Delta orchestration finished for ticket {officialDelta.Ticket.TicketId}", $"{officialDelta.Ticket.TicketId}");
                }
                else
                {
                    await orchestrationContext.CallActivityAsync(ActivityNames.HandleOfficialDeltaFailure, Tuple.Create(officialDelta, Constants.OfficialDeltaFailedMessage)).ConfigureAwait(true);
                }
            }
            catch (Exception ex)
            {
                await this.RegisterFailureAsync(orchestrationContext, officialDelta, ex, Constants.OfficialDeltaFailureMessage).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// BuildOfficialData asynchronous.
        /// </summary>
        /// <param name="activityContext">The activity context.</param>
        /// <returns>The task.</returns>
        [FunctionName(ActivityNames.BuildOfficialData)]
        public async Task<OfficialDeltaData> BuildOfficialDataAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var officialDeltaData = activityContext.GetInput<OfficialDeltaData>();

            this.logger.LogInformation($"Excluding official delta data for Ticket: {officialDeltaData.Ticket.TicketId}", $"{officialDeltaData.Ticket.TicketId}");

            officialDeltaData.Activity = ActivityNames.BuildOfficialData;
            await this.InitializeAsync(officialDeltaData).ConfigureAwait(false);

            return await this.officialDeltaProcessor.BuildOfficialDataAsync(officialDeltaData).ConfigureAwait(false);
        }

        /// <summary>
        /// Exclude data asynchronous.
        /// </summary>
        /// <param name="activityContext">The activity context.</param>
        /// <returns>The task.</returns>
        [FunctionName(ActivityNames.ExcludeData)]
        public async Task<OfficialDeltaData> ExcludeDataAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var officialDeltaData = activityContext.GetInput<OfficialDeltaData>();

            this.logger.LogInformation($"Requesting delta for ticket: {officialDeltaData.Ticket.TicketId}", $"{officialDeltaData.Ticket.TicketId}");

            officialDeltaData.Activity = ActivityNames.ExcludeData;
            await this.InitializeAsync(officialDeltaData).ConfigureAwait(false);

            return await this.officialDeltaProcessor.ExcludeDataAsync(officialDeltaData).ConfigureAwait(false);
        }

        /// <summary>
        /// RegisterNodeActivity asynchronous.
        /// </summary>
        /// <param name="activityContext">The activity context.</param>
        /// <returns>The task.</returns>
        [FunctionName(ActivityNames.RegisterNodeActivity)]
        public async Task<OfficialDeltaData> RegisterNodeActivityAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var officialDeltaData = activityContext.GetInput<OfficialDeltaData>();

            this.logger.LogInformation($"Registering nodes for official delta ticket: {officialDeltaData.Ticket.TicketId}", $"{officialDeltaData.Ticket.TicketId}");

            officialDeltaData.Activity = ActivityNames.RegisterNodeActivity;
            await this.InitializeAsync(officialDeltaData).ConfigureAwait(false);

            await this.officialDeltaProcessor.BuildOfficialDeltaDataAsync(officialDeltaData).ConfigureAwait(false);
            return await this.officialDeltaProcessor.RegisterAsync(officialDeltaData).ConfigureAwait(false);
        }

        /// <summary>
        /// Request delta asynchronous.
        /// </summary>
        /// <param name="activityContext">The activity context.</param>
        /// <returns>The task.</returns>
        [FunctionName(ActivityNames.RequestOfficialDelta)]
        public async Task<OfficialDeltaData> RequestOfficialDeltaAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var deltaData = activityContext.GetInput<OfficialDeltaData>();

            this.logger.LogInformation($"Requesting official delta for ticket: {deltaData.Ticket.TicketId}", $"{deltaData.Ticket.TicketId}");

            deltaData.Activity = ActivityNames.RequestOfficialDelta;
            await this.InitializeAsync(deltaData).ConfigureAwait(false);

            return await this.officialDeltaProcessor.ProcessAsync(deltaData, ChainType.RequestOfficialDelta).ConfigureAwait(false);
        }

        /// <summary>
        /// Process delta asynchronous.
        /// </summary>
        /// <param name="activityContext">The activity context.</param>
        /// <returns>The task.</returns>
        [FunctionName(ActivityNames.ProcessOfficialDelta)]
        public async Task<OfficialDeltaData> ProcessOfficialDeltaAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var deltaData = activityContext.GetInput<OfficialDeltaData>();

            this.logger.LogInformation($"Processing official delta for ticket: {deltaData.Ticket.TicketId}", $"{deltaData.Ticket.TicketId}");

            deltaData.Activity = ActivityNames.ProcessOfficialDelta;
            await this.InitializeAsync(deltaData).ConfigureAwait(false);

            return await this.officialDeltaProcessor.ProcessAsync(deltaData, ChainType.ProcessOfficialDelta).ConfigureAwait(false);
        }

        /// <summary>
        /// Register movements.
        /// </summary>
        /// <param name="activityContext">The activityContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.RegisterMovementsOfficialDelta)]
        public async Task<OfficialDeltaData> RegisterMovementsOfficialDeltaAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var deltaData = activityContext.GetInput<OfficialDeltaData>();

            this.logger.LogInformation($"Registering movements official delta for ticket: {deltaData.Ticket.TicketId}", $"{deltaData.Ticket.TicketId}");

            deltaData.Activity = ActivityNames.RegisterMovementsOfficialDelta;
            await this.InitializeAsync(deltaData).ConfigureAwait(false);

            return await this.officialDeltaProcessor.ProcessAsync(deltaData, ChainType.RegisterMovementsOfficialDelta).ConfigureAwait(false);
        }

        /// <summary>
        /// Complete delta.
        /// </summary>
        /// <param name="activityContext">The activityContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.CalculateOfficialDelta)]
        public async Task CalculateOfficialDeltaAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var deltaData = activityContext.GetInput<OfficialDeltaData>();

            this.logger.LogInformation($"Performing pre-calculation for official delta ticket: {deltaData.Ticket.TicketId}", $"{deltaData.Ticket.TicketId}");

            deltaData.Activity = ActivityNames.CalculateOfficialDelta;
            await this.InitializeAsync(deltaData).ConfigureAwait(false);

            await this.officialDeltaProcessor.ProcessAsync(deltaData, ChainType.CalculateOfficialDelta).ConfigureAwait(false);
        }

        /// <summary>
        /// Complete delta.
        /// </summary>
        /// <param name="activityContext">The activityContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.CompleteOfficialDelta)]
        public async Task CompleteOfficialDeltaAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var deltaData = activityContext.GetInput<OfficialDeltaData>();

            this.logger.LogInformation($"The complete process is triggered for official delta ticket: {deltaData.Ticket.TicketId}", $"{deltaData.Ticket.TicketId}");

            deltaData.Activity = ActivityNames.CompleteOfficialDelta;
            await this.InitializeAsync(deltaData).ConfigureAwait(false);

            await this.officialDeltaProcessor.FinalizeProcessAsync(deltaData.Ticket.TicketId).ConfigureAwait(false);
            this.logger.LogInformation($"Success official delta ticket: {deltaData.Ticket.TicketId}", $"{deltaData.Ticket.TicketId}");
        }

        /// <summary>
        /// Handles the OfficialDelta failure asynchronous.
        /// </summary>
        /// <param name="activityContext">The activity context.</param>
        /// <returns>The task.</returns>
        [FunctionName(ActivityNames.HandleOfficialDeltaFailure)]
        public async Task HandleOfficialDeltaFailureAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var result = activityContext.GetInput<Tuple<OfficialDeltaData, string>>();

            this.logger.LogInformation(
               $" official delta processing failure for ticket: {result.Item1.Ticket.TicketId}",
               $"{result.Item1.Ticket.TicketId}");

            await this.InitializeAsync().ConfigureAwait(false);
            await this.failureHandler.HandleFailureAsync(
                this.unitOfWork,
                new FailureInfo(result.Item1.Ticket.TicketId, result.Item2, result.Item1.GeneratedMovements.Select(x => x.MovementTransactionId))).ConfigureAwait(false);
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
        [FunctionName("PurgeOfficialDeltaHistory")]
        public async Task PurgeOfficialDeltaHistoryAsync(
            [TimerTrigger("%OfficialDeltaPurgeInterval%")] TimerInfo timer,
            [DurableClient] IDurableOrchestrationClient durableOrchestrationClient,
            ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(timer, nameof(timer));
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(durableOrchestrationClient, nameof(durableOrchestrationClient));

            this.logger.LogInformation($"Purge delta history function triggered with schedule: {timer.Schedule}", Constants.PurgeOfficialDeltaHistoryKey);
            try
            {
                await this.InitializeAsync().ConfigureAwait(false);
                this.logger.LogInformation($"The purge delta history job has started with schedule: {timer.Schedule}", Constants.PurgeOfficialDeltaHistoryKey);
                var tasks = new List<Task>
                {
                    this.PurgeOrchestrationDataAsync(durableOrchestrationClient, Constants.PurgeOfficialDeltaHistoryKey, Constants.PurgingOfficialDeltaMessage),
                    this.DoHandleFailureAsync(durableOrchestrationClient, this.unitOfWork, this.failureHandler, Constants.OfficialDeltaFailureMessage),
                };
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message, Constants.PurgeDeltaHistoryKey);
            }

            this.logger.LogInformation($"The purge delta history job has finished at: {DateTime.UtcNow.ToTrue()}", Constants.PurgeOfficialDeltaHistoryKey);
        }

        private async Task RegisterFailureAsync(IDurableOrchestrationContext orchestrationContext, OfficialDeltaData officialDelta, Exception ex, string errorMessage)
        {
            var error = ex.InnerException ?? ex;
            this.logger.LogError(ex, $"Exception occurred in official delta  orchestration for ticket {officialDelta.Ticket.TicketId}.", error);
            orchestrationContext.SetCustomStatus(Constants.CustomFailureStatus);
            this.logger.LogInformation($"Calling HandleFailure activity from Official Delta orchestrator for {officialDelta.Ticket.TicketId}.", $"{officialDelta.Ticket.TicketId}");
            await orchestrationContext.CallActivityAsync(
                "HandleOfficialDeltaFailure", Tuple.Create(officialDelta, errorMessage)).ConfigureAwait(true);
        }
    }
}
