// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidationOrchestrator.cs" company="Microsoft">
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
    using Ecp.True.Processors.Core.HandleFailure;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;

    /// <summary>
    /// The ConsolidationOrchestrator.
    /// </summary>
    public class ConsolidationOrchestrator : OrchestratorBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<ConsolidationOrchestrator> logger;

        /// <summary>
        /// The failure handler.
        /// </summary>
        private readonly IFailureHandler failureHandler;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The consolidation processor.
        /// </summary>
        private readonly IConsolidationProcessor consolidationProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsolidationOrchestrator"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="failureHandlerFactory">The failure handler factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="consolidationProcessor">The consolidation processor.</param>
        public ConsolidationOrchestrator(
            ITrueLogger<ConsolidationOrchestrator> logger,
            IConfigurationHandler configurationHandler,
            IAzureClientFactory azureClientFactory,
            IServiceProvider serviceProvider,
            IFailureHandlerFactory failureHandlerFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            IConsolidationProcessor consolidationProcessor)
            : base(configurationHandler, azureClientFactory, serviceProvider, logger)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            ArgumentValidators.ThrowIfNull(failureHandlerFactory, nameof(failureHandlerFactory));

            this.logger = logger;
            this.failureHandler = failureHandlerFactory.GetFailureHandler(TicketType.OfficialDelta);
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.consolidationProcessor = consolidationProcessor;
        }

        /// <summary>
        /// Consolidates the official delta asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="deliveryCount">The delivery count.</param>
        /// <param name="durableOrchestrationClient">The durable orchestration client.</param>
        /// <param name="context">The context.</param>
        /// <returns>The task.</returns>
        [FunctionName("ConsolidateOfficialDelta")]
        public async Task ConsolidateOfficialDeltaAsync(
           [ServiceBusTrigger("%DeltaConsolidation%", Connection = "IntegrationServiceBusConnectionString", IsSessionsEnabled = true)] int ticketId,
           string label,
           string replyTo,
           int deliveryCount,
           [DurableClient] IDurableOrchestrationClient durableOrchestrationClient,
           ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(ticketId, nameof(ticketId));

            this.logger.LogInformation($"Consolidation processing is requested for ticket {ticketId}", $"{ticketId}");

            try
            {
                await this.InitializeAsync().ConfigureAwait(false);
                this.ProcessMetadata(label, FunctionNames.OfficialDelta, replyTo);

                var consolidationData = new ConsolidationData
                {
                    ChaosValue = label,
                    Caller = FunctionNames.OfficialDelta,
                    Orchestrator = OrchestratorNames.ConsolidationDataOrchestrator,
                    ReplyTo = replyTo,
                };

                (bool isValid, Ticket ticket, string errorMessage) = await this.consolidationProcessor.ValidateTicketAsync(ticketId).ConfigureAwait(false);
                if (!isValid)
                {
                    if (ticket != null)
                    {
                        await this.failureHandler.HandleFailureAsync(
                       this.unitOfWork,
                       new FailureInfo(ticketId, errorMessage)).ConfigureAwait(false);
                    }

                    return;
                }

                this.logger.LogInformation($"Consolidation orchestrator is triggered for ticket {ticketId}", $"{ticketId}");

                consolidationData.Batches = await this.consolidationProcessor.GetConsolidationBatchesAsync(ticket).ConfigureAwait(false);
                consolidationData.Ticket = ticket;

                await this.TryStartAsync(
                     durableOrchestrationClient,
                     OrchestratorNames.ConsolidationDataOrchestrator,
                     ticketId.ToString(CultureInfo.InvariantCulture),
                     consolidationData).ConfigureAwait(false);

                this.logger.LogInformation($"Consolidation processing is triggered for ticket: {ticketId}", $"{ticketId}");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, Constants.ConsolidationFailureMessage, $"{ticketId}");
                if (deliveryCount < 10)
                {
                    throw;
                }

                await this.failureHandler.HandleFailureAsync(this.unitOfWork, new FailureInfo(ticketId, Constants.ConsolidationFailureMessage)).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Consolidation data Orchestrator asynchronous.
        /// </summary>
        /// <param name="orchestrationContext">The orchestration context.</param>
        /// <returns>The task.</returns>
        [FunctionName("ConsolidationDataOrchestrator")]
        public async Task ConsolidationDataOrchestratorAsync(
        [OrchestrationTrigger] IDurableOrchestrationContext orchestrationContext)
        {
            ArgumentValidators.ThrowIfNull(orchestrationContext, nameof(orchestrationContext));
            var consolidationData = orchestrationContext.GetInput<ConsolidationData>();
            try
            {
                this.ProcessMetadata(consolidationData.ChaosValue, string.Join(".", consolidationData.Caller, consolidationData.Orchestrator), consolidationData.ReplyTo);

                foreach (var batch in consolidationData.Batches)
                {
                    this.logger.LogInformation($"Consolidation started for ticket {batch.Ticket.TicketId}.", batch);
                    await this.ExecuteActivityAsync(ActivityNames.Consolidate, orchestrationContext, Tuple.Create(batch, consolidationData)).ConfigureAwait(true);
                    this.logger.LogInformation($"Consolidation finished for ticket {batch.Ticket.TicketId}.", batch);
                }

                await this.CallActivityAsync(ActivityNames.CompleteConsolidation, orchestrationContext, consolidationData).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                var error = ex.InnerException ?? ex;
                this.logger.LogError(error, Constants.ConsolidationFailureMessage, consolidationData.Ticket.TicketId);
                orchestrationContext.SetCustomStatus(Constants.CustomFailureStatus);
                this.logger.LogInformation(
                    $"Calling HandleFailure activity from consolidation orchestrator for {consolidationData.Ticket.TicketId}.",
                    $"{consolidationData.Ticket.TicketId}");
                await orchestrationContext.CallActivityAsync(ActivityNames.HandleConsolidationFailure, Tuple.Create(consolidationData, Constants.ConsolidationFailureMessage)).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Consolidates the asynchronous.
        /// </summary>
        /// <param name="activityContext">The activity context.</param>
        /// <returns>The task.</returns>
        [FunctionName(ActivityNames.Consolidate)]
        public async Task ConsolidateAsync(
           [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var (batch, consolidationData) = activityContext.GetInput<Tuple<ConsolidationBatch, ConsolidationData>>();

            this.logger.LogInformation($"Consolidating official delta data for ticket: {batch.Ticket.TicketId}.", batch);

            consolidationData.Activity = ActivityNames.Consolidate;
            await this.InitializeAsync(consolidationData).ConfigureAwait(false);

            await this.consolidationProcessor.ConsolidateAsync(batch).ConfigureAwait(false);
        }

        /// <summary>
        /// Complets the consolidation asynchronous.
        /// </summary>
        /// <param name="activityContext">The activity context.</param>
        /// <returns>The task.</returns>
        [FunctionName(ActivityNames.CompleteConsolidation)]
        public async Task CompleteConsolidationAsync(
           [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var consolidationData = activityContext.GetInput<ConsolidationData>();

            this.logger.LogInformation($"Completing Consolidation for ticket: {consolidationData.Ticket.TicketId}", $"{consolidationData.Ticket.TicketId}");

            consolidationData.Activity = ActivityNames.CompleteConsolidation;
            await this.InitializeAsync(consolidationData).ConfigureAwait(false);

            await this.consolidationProcessor.CompleteConsolidationAsync(consolidationData.Ticket.TicketId, consolidationData.Ticket.CategoryElementId).ConfigureAwait(false);
        }

        /// <summary>
        /// Handles the consolidation failure asynchronous.
        /// </summary>
        /// <param name="activityContext">The activity context.</param>
        /// <returns>The task.</returns>
        [FunctionName(ActivityNames.HandleConsolidationFailure)]
        public async Task HandleConsolidationFailureAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var result = activityContext.GetInput<Tuple<ConsolidationData, string>>();

            this.logger.LogInformation($"Consolidating official delta processing failure for ticket: {result.Item1.Ticket.TicketId}");

            await this.InitializeAsync().ConfigureAwait(false);
            await this.failureHandler.HandleFailureAsync(this.unitOfWork, new FailureInfo(result.Item1.Ticket.TicketId, result.Item2)).ConfigureAwait(false);
        }

        /// <summary>
        /// Purges the official delta history asynchronous.
        /// </summary>
        /// <param name="timer">The timer.</param>
        /// <param name="durableOrchestrationClient">The durable orchestration client.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("PurgeConsolidatedDataHistory")]
        public async Task PurgeConsolidatedDataHistoryAsync(
          [TimerTrigger("%ConsolidatedDataPurgeInterval%")] TimerInfo timer,
          [DurableClient] IDurableOrchestrationClient durableOrchestrationClient,
          ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(timer, nameof(timer));
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(durableOrchestrationClient, nameof(durableOrchestrationClient));

            this.logger.LogInformation($"Purge consolidated data history function triggered with schedule: {timer.Schedule}", Constants.PurgeConsolidationHistoryKey);
            try
            {
                await this.InitializeAsync().ConfigureAwait(false);
                this.logger.LogInformation($"The purge consolidated data history job has started with schedule: {timer.Schedule}", Constants.PurgeConsolidationHistoryKey);
                var tasks = new List<Task>
                {
                    this.PurgeOrchestrationDataAsync(durableOrchestrationClient, Constants.PurgeConsolidationHistoryKey, Constants.PurgingConsolidationMessage),
                    this.DoHandleFailureAsync(durableOrchestrationClient, this.unitOfWork, this.failureHandler, Constants.ConsolidationFailureMessage),
                };
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message, Constants.PurgeConsolidationHistoryKey);
            }

            this.logger.LogInformation($"The purge consolidated data history job has finished at: {DateTime.UtcNow.ToTrue()}", Constants.PurgeConsolidationHistoryKey);
        }

        /// <summary>
        /// Does the initialize asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        protected override async Task DoInitializeAsync()
        {
            await base.DoInitializeAsync().ConfigureAwait(false);
        }
    }
}
