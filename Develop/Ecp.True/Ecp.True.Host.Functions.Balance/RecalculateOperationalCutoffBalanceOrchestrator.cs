// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecalculateOperationalCutoffBalanceOrchestrator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Balance.Orchestration
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Balance.Calculation.Input;
    using Ecp.True.Processors.Balance.Interfaces;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;

    /// <summary>
    /// The operational cutoff orchestrator.
    /// </summary>
    public class RecalculateOperationalCutoffBalanceOrchestrator : OrchestratorBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<RecalculateOperationalCutoffBalanceOrchestrator> logger;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// The balance processor.
        /// </summary>
        private readonly IBalanceProcessor balanceProcessor;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecalculateOperationalCutoffBalanceOrchestrator"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="balanceProcessor">The balance processor.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="failureHandlerFactory">The failureHandlerFactory.</param>
        /// <param name="unitOfWorkFactory">The unit of work.</param>
        public RecalculateOperationalCutoffBalanceOrchestrator(
            ITrueLogger<RecalculateOperationalCutoffBalanceOrchestrator> logger,
            ITelemetry telemetry,
            IServiceProvider serviceProvider,
            IBalanceProcessor balanceProcessor,
            IConfigurationHandler configurationHandler,
            IAzureClientFactory azureClientFactory,
            IFailureHandlerFactory failureHandlerFactory,
            IUnitOfWorkFactory unitOfWorkFactory)
            : base(configurationHandler, azureClientFactory, serviceProvider, logger)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            ArgumentValidators.ThrowIfNull(failureHandlerFactory, nameof(failureHandlerFactory));

            this.logger = logger;
            this.telemetry = telemetry;
            this.balanceProcessor = balanceProcessor;
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        }

        /// <summary>
        /// Calculate the balance asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="durableOrchestrationClient">The starter.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("RecalculateOperationalCutoffBalance")]
        public async Task RecalculateOperationalCutoffBalanceAsync(
            [ServiceBusTrigger("%RecalculateOperationalCutoffBalanceQueue%", Connection = "IntegrationServiceBusConnectionString", IsSessionsEnabled = true)] int ticketId,
            string label,
            string replyTo,
            [DurableClient] IDurableOrchestrationClient durableOrchestrationClient,
            Microsoft.Azure.WebJobs.ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(ticketId, nameof(ticketId));
            ArgumentValidators.ThrowIfNull(durableOrchestrationClient, nameof(durableOrchestrationClient));

            this.logger.LogInformation($"Recalculate operational cutoff processing is requested for ticket {ticketId}", $"{ticketId}");

            try
            {
                await this.InitializeAsync().ConfigureAwait(false);
                this.ProcessMetadata(label, FunctionNames.RecalculateOperationalCutOff, replyTo);
                var info = await this.GetOperationalCutOffInformationAsync(ticketId, label, replyTo).ConfigureAwait(false);

                // Calling the Balance Orchestrator
                await this.TryStartAsync(
                    durableOrchestrationClient,
                    OrchestratorNames.RecalculateOperationalCutoffBalanceOrchestrator,
                    ticketId.ToString(CultureInfo.InvariantCulture),
                    info).ConfigureAwait(false);

                this.logger.LogInformation($"Recalculate operational cutoff processing triggered for ticket: {ticketId}", $"{ticketId}");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, Constants.RecalculateCutOffOrchestratorFailed, $"{ticketId}");
            }
        }

        /// <summary>
        /// The balance orchestrator asynchronous.
        /// </summary>
        /// <param name="orchestrationContext">The orchestrationContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("RecalculateOperationalCutoffBalanceOrchestrator")]
        public async Task RecalculateOperationalCutoffBalanceOrchestratorAsync(
        [OrchestrationTrigger] IDurableOrchestrationContext orchestrationContext)
        {
            ArgumentValidators.ThrowIfNull(orchestrationContext, nameof(orchestrationContext));
            var operationalCutOffInfo = orchestrationContext.GetInput<OperationalCutOffInfo>();
            this.logger.LogInformation($"Recalculate operational cutoff orchestration started for ticket {operationalCutOffInfo.Ticket.TicketId}", $"{operationalCutOffInfo.Ticket.TicketId}");

            try
            {
                this.ProcessMetadata(
                    operationalCutOffInfo.ChaosValue,
                    string.Join(".", operationalCutOffInfo.Caller, operationalCutOffInfo.Orchestrator),
                    operationalCutOffInfo.ReplyTo);

                await this.CallActivityAsync(ActivityNames.DeleteSegmentBalance, orchestrationContext, operationalCutOffInfo).ConfigureAwait(true);
                await this.CallActivityAsync(ActivityNames.DeleteSystemBalance, orchestrationContext, operationalCutOffInfo).ConfigureAwait(true);

                var segmentInfo = await this.CallActivityAsync(ActivityNames.RecalculateCalculateSegment, orchestrationContext, operationalCutOffInfo).ConfigureAwait(true);
                var systemInfo = await this.CallActivityAsync(ActivityNames.RecalculateCalculateSystem, orchestrationContext, segmentInfo).ConfigureAwait(true);
                await this.ExecuteActivityAsync(ActivityNames.RecalculateComplete, orchestrationContext, systemInfo).ConfigureAwait(true);
                try
                {
                    await this.ExecuteActivityAsync(ActivityNames.RecalculateFinalizeCutOff, orchestrationContext, operationalCutOffInfo).ConfigureAwait(true);
                }
                catch (Exception ex)
                {
                    var error = ex.InnerException ?? ex;
                    this.logger.LogError(
                        error,
                        $"Failed to finalize the recalculate operational cut off for ticketId : {operationalCutOffInfo.Ticket.TicketId}",
                        $"{operationalCutOffInfo.Ticket.TicketId}");
                }
            }
            catch (Exception ex)
            {
                var error = ex.InnerException ?? ex;
                this.logger.LogError(error, $"Exception occurred in recalculate operational cutoff orchestration. {operationalCutOffInfo.Ticket}");
            }

            this.logger.LogInformation($"Recalculate operational cutoff orchestration finished for ticket {operationalCutOffInfo.Ticket.TicketId}", $"{operationalCutOffInfo.Ticket.TicketId}");
        }

        /// <summary>
        /// Gets the ticket asynchronous.
        /// </summary>
        /// <param name="activityContext">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.RecalculateCalculateSegment)]
        public async Task<OperationalCutOffInfo> ProcessSegmentAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));

            var cutOffInfo = activityContext.GetInput<OperationalCutOffInfo>();

            cutOffInfo.Activity = ActivityNames.RecalculateCalculateSegment;
            await this.InitializeAsync(cutOffInfo).ConfigureAwait(false);

            this.logger.LogInformation($"The segment processing is triggered for ticket: {cutOffInfo.Ticket.TicketId}", $"{cutOffInfo.Ticket.TicketId}");
            cutOffInfo.SegmentUnbalances = await this.balanceProcessor.ProcessSegmentAsync(cutOffInfo.Ticket.TicketId).ConfigureAwait(false);

            return cutOffInfo;
        }

        /// <summary>
        /// Gets the ticket asynchronous.
        /// </summary>
        /// <param name="activityContext">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.RecalculateCalculateSystem)]
        public async Task<OperationalCutOffInfo> RecalculateProcessSystemAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));

            var cutOffInfo = activityContext.GetInput<OperationalCutOffInfo>();

            cutOffInfo.Activity = ActivityNames.RecalculateCalculateSystem;
            await this.InitializeAsync(cutOffInfo).ConfigureAwait(false);

            this.logger.LogInformation($"The recalculate system processing is triggered for ticket: {cutOffInfo.Ticket.TicketId}", $"{cutOffInfo.Ticket.TicketId}");
            cutOffInfo.SystemUnbalances = await this.balanceProcessor.ProcessSystemAsync(cutOffInfo.Ticket.TicketId).ConfigureAwait(false);

            return cutOffInfo;
        }

        /// <summary>
        /// Gets the ticket asynchronous.
        /// </summary>
        /// <param name="activityContext">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.DeleteSystemBalance)]
        public async Task DeleteSystemBalanceAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));

            var cutOffInfo = activityContext.GetInput<OperationalCutOffInfo>();

            cutOffInfo.Activity = ActivityNames.DeleteSystemBalance;
            await this.InitializeAsync(cutOffInfo).ConfigureAwait(false);

            this.logger.LogInformation($"The de delete system processing is triggered for ticket: {cutOffInfo.Ticket.TicketId}", $"{cutOffInfo.Ticket.TicketId}");
            await this.balanceProcessor.DeleteBalanceAsync<SystemUnbalance>(cutOffInfo.Ticket.TicketId).ConfigureAwait(false);
        }

        /// <summary>
        /// Post calculation tasks.
        /// </summary>
        /// <param name="activityContext">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.RecalculateComplete)]
        public async Task RecalculateCompleteCalculationAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));

            var cutOffInfo = activityContext.GetInput<OperationalCutOffInfo>();

            cutOffInfo.Activity = ActivityNames.RecalculateComplete;
            await this.InitializeAsync(cutOffInfo).ConfigureAwait(false);

            this.logger.LogInformation($"The complete recalculate calculation is triggered for ticket: {cutOffInfo.Ticket.TicketId}", $"{cutOffInfo.Ticket.TicketId}");
            await this.balanceProcessor.CompleteAsync(cutOffInfo).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the ticket asynchronous.
        /// </summary>
        /// <param name="activityContext">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.DeleteSegmentBalance)]
        public async Task DeleteSegmentBalanceAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));

            var cutOffInfo = activityContext.GetInput<OperationalCutOffInfo>();

            cutOffInfo.Activity = ActivityNames.DeleteSegmentBalance;
            await this.InitializeAsync(cutOffInfo).ConfigureAwait(false);

            this.logger.LogInformation($"The delete segment processing is triggered for ticket: {cutOffInfo.Ticket.TicketId}", $"{cutOffInfo.Ticket.TicketId}");
            await this.balanceProcessor.DeleteBalanceAsync<SegmentUnbalance>(cutOffInfo.Ticket.TicketId).ConfigureAwait(false);
        }

        /// <summary>
        /// Finalizes the cut off asynchronous.
        /// </summary>
        /// <param name="activityContext">The activity context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.RecalculateFinalizeCutOff)]
        public async Task RecalculateFinalizeCutOffAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var cutOffInfo = activityContext.GetInput<OperationalCutOffInfo>();
            try
            {
                this.logger.LogInformation($"Recalculate cutoff processing finalizer started for ticket Id {cutOffInfo.Ticket.TicketId}", $"{cutOffInfo.Ticket.TicketId}");

                cutOffInfo.Activity = ActivityNames.RecalculateFinalizeCutOff;
                await this.InitializeAsync(cutOffInfo).ConfigureAwait(false);

                await this.balanceProcessor.FinalizeProcessAsync(cutOffInfo.Ticket.TicketId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var error = ex.InnerException ?? ex;
                this.logger.LogError(error, $"Failed to finalize the recalculate operational cut off for ticket : {cutOffInfo.Ticket.TicketId}", error);
                this.telemetry.TrackEvent(Constants.Critical, EventName.CutoffFailureEvent.ToString("G"));
            }
        }

        /// <summary>
        /// Get operational cut off info.
        /// </summary>
        /// <param name="ticketId">The ticket.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The replyTo.</param>
        /// <returns>Get operational cutOff information.</returns>
        private async Task<OperationalCutOffInfo> GetOperationalCutOffInformationAsync(int ticketId, string label, string replyTo)
        {
            var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();
            var ticket = await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            return new OperationalCutOffInfo
            {
                Ticket = ticket,
                Step = MovementCalculationStep.Interface,
                BalanceInput = await this.balanceProcessor.GetBalanceInputAsync(ticket).ConfigureAwait(false),
                Caller = FunctionNames.RecalculateOperationalCutOff,
                ChaosValue = label,
                Orchestrator = OrchestratorNames.RecalculateOperationalCutoffBalanceOrchestrator,
                ReplyTo = replyTo,
            };
        }
    }
}