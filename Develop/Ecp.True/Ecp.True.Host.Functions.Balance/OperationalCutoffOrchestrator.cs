// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationalCutoffOrchestrator.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Balance.Calculation.Input;
    using Ecp.True.Processors.Balance.Interfaces;
    using Ecp.True.Processors.Core.HandleFailure;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using EntitiesConstants = Ecp.True.Entities.Constants;

    /// <summary>
    /// The operational cutoff orchestrator.
    /// </summary>
    public class OperationalCutoffOrchestrator : OrchestratorBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<OperationalCutoffOrchestrator> logger;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// The balance processor.
        /// </summary>
        private readonly IBalanceProcessor balanceProcessor;

        /// <summary>
        /// The cutoff failure handler.
        /// </summary>
        private readonly IFailureHandler cutoffFailureHandler;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationalCutoffOrchestrator"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="balanceProcessor">The balance processor.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="failureHandlerFactory">The failureHandlerFactory.</param>
        /// <param name="unitOfWorkFactory">The unit of work.</param>
        public OperationalCutoffOrchestrator(
            ITrueLogger<OperationalCutoffOrchestrator> logger,
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
            this.cutoffFailureHandler = failureHandlerFactory.GetFailureHandler(TicketType.Cutoff);
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        }

        /// <summary>
        /// Calculate the balance asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="deliveryCount">The deliveryCount.</param>
        /// <param name="durableOrchestrationClient">The starter.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("ProcessOperationalCutoff")]
        public async Task CalculateBalanceAsync(
            [ServiceBusTrigger("%OperationalCutoff%", Connection = "IntegrationServiceBusConnectionString", IsSessionsEnabled = true)] int ticketId,
            string label,
            string replyTo,
            int deliveryCount,
            [DurableClient] IDurableOrchestrationClient durableOrchestrationClient,
            Microsoft.Azure.WebJobs.ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(durableOrchestrationClient, nameof(durableOrchestrationClient));

            this.logger.LogInformation($"Operational cutoff processing is requested for ticket {ticketId}", $"{ticketId}");

            try
            {
                await this.InitializeAsync().ConfigureAwait(false);
                this.ProcessMetadata(label, FunctionNames.OperationalCutOff, replyTo);

                var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();
                var ticket = await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);

                var ticketValidationStatus = await this.ValidateTicketAsync(ticketRepository, ticket, ticketId).ConfigureAwait(false);

                if (!ticketValidationStatus)
                {
                    return;
                }

                ////this.logger.LogInformation($"Operational cut off clean up is triggered for ticket {ticketId} with instance id {context.InvocationId}", $"{ticketId}");
                ////await this.balanceProcessor.CleanOperationalCutOffDataAsync(ticketId).ConfigureAwait(false);

                var info = new OperationalCutOffInfo
                {
                    Ticket = ticket,
                    Step = MovementCalculationStep.Interface,
                    BalanceInput = await this.balanceProcessor.GetBalanceInputAsync(ticket).ConfigureAwait(false),
                    Caller = FunctionNames.OperationalCutOff,
                    ChaosValue = label,
                    Orchestrator = OrchestratorNames.OperationalCutoffOrchestrator,
                    ReplyTo = replyTo,
                };

                // Calling the Balance Orchestrator
                await this.TryStartAsync(durableOrchestrationClient, OrchestratorNames.OperationalCutoffOrchestrator, ticketId.ToString(CultureInfo.InvariantCulture), info).ConfigureAwait(false);
                this.logger.LogInformation($"Operational cutoff processing triggered for ticket: {ticketId}", $"{ticketId}");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, Constants.CutOffOrchestratorFailed, $"{ticketId}");
                if (deliveryCount < 10)
                {
                    throw;
                }

                await this.cutoffFailureHandler.HandleFailureAsync(this.unitOfWork, new FailureInfo(ticketId, ex.Message)).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// The balance orchestrator asynchronous.
        /// </summary>
        /// <param name="orchestrationContext">The orchestrationContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("OperationalCutoffOrchestrator")]
        public async Task OperationalCutoffOrchestratorAsync(
        [OrchestrationTrigger] IDurableOrchestrationContext orchestrationContext)
        {
            ArgumentValidators.ThrowIfNull(orchestrationContext, nameof(orchestrationContext));
            var operationalCutOffInfo = orchestrationContext.GetInput<OperationalCutOffInfo>();
            this.logger.LogInformation($"Operational cutoff orchestration started for ticket {operationalCutOffInfo.Ticket.TicketId}", $"{operationalCutOffInfo.Ticket.TicketId}");

            try
            {
                this.ProcessMetadata(
                    operationalCutOffInfo.ChaosValue,
                    string.Join(".", operationalCutOffInfo.Caller, operationalCutOffInfo.Orchestrator),
                    operationalCutOffInfo.ReplyTo);

                while (operationalCutOffInfo.Step <= MovementCalculationStep.Unbalance)
                {
                    operationalCutOffInfo = await orchestrationContext.CallActivityAsync<OperationalCutOffInfo>(ActivityNames.Calculate, operationalCutOffInfo).ConfigureAwait(true);
                    operationalCutOffInfo.Step += 1;
                }

                await this.ExecuteActivityAsync(ActivityNames.Register, orchestrationContext, operationalCutOffInfo).ConfigureAwait(true);
                var segmentInfo = await this.CallActivityAsync(ActivityNames.CalculateSegment, orchestrationContext, operationalCutOffInfo).ConfigureAwait(true);
                var systemInfo = await this.CallActivityAsync(ActivityNames.CalculateSystem, orchestrationContext, segmentInfo).ConfigureAwait(true);
                await this.ExecuteActivityAsync(ActivityNames.Complete, orchestrationContext, systemInfo).ConfigureAwait(true);
                try
                {
                    await this.ExecuteActivityAsync(ActivityNames.FinalizeCutOff, orchestrationContext, operationalCutOffInfo).ConfigureAwait(true);
                }
                catch (Exception ex)
                {
                    var error = ex.InnerException ?? ex;
                    this.logger.LogError(error, $"Failed to finalize the operational cut off for ticketId : {operationalCutOffInfo.Ticket.TicketId}", $"{operationalCutOffInfo.Ticket.TicketId}");
                    this.telemetry.TrackEvent(Constants.Critical, EventName.CutoffFailureEvent.ToString("G"));
                }
            }
            catch (Exception ex)
            {
                var error = ex.InnerException ?? ex;
                this.logger.LogError(error, $"Exception occurred in operational cutoff orchestration. {operationalCutOffInfo.Ticket}");
                orchestrationContext.SetCustomStatus(Constants.CustomFailureStatus);
                operationalCutOffInfo.Ticket.ErrorMessage = error.Message;
                await this.ExecuteActivityAsync(ActivityNames.HandleFailure, orchestrationContext, operationalCutOffInfo.Ticket).ConfigureAwait(true);
            }

            this.logger.LogInformation($"Operational cutoff orchestration finished for ticket {operationalCutOffInfo.Ticket.TicketId}", $"{operationalCutOffInfo.Ticket.TicketId}");
        }

        /// <summary>
        /// Gets the ticket asynchronous.
        /// </summary>
        /// <param name="activityContext">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.Calculate)]
        public async Task<OperationalCutOffInfo> ProcessCalculationOutputAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));

            var operationalCutOffInfo = activityContext.GetInput<OperationalCutOffInfo>();

            operationalCutOffInfo.Activity = ActivityNames.Calculate;
            await this.InitializeAsync(operationalCutOffInfo).ConfigureAwait(false);

            this.logger.LogInformation($"The balance calculation is triggered for ticket: {operationalCutOffInfo.Ticket.TicketId}", $"{operationalCutOffInfo.Ticket.TicketId}");
            var operationalCutOffOutput = await this.balanceProcessor.ProcessCalculationAsync(operationalCutOffInfo).ConfigureAwait(false);

            var movements = this.TransformToCalculationMovements(operationalCutOffOutput.Movements);
            operationalCutOffInfo.BalanceInput.Movements = operationalCutOffInfo.BalanceInput.Movements.AddRange(movements);

            operationalCutOffInfo.Movements = operationalCutOffInfo.Movements.AddRange(operationalCutOffOutput.Movements);
            operationalCutOffInfo.Unbalances.Add(operationalCutOffInfo.Step, operationalCutOffOutput.UnbalanceList);

            return operationalCutOffInfo;
        }

        /// <summary>
        /// Register the calculation output.
        /// </summary>
        /// <param name="activityContext">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.Register)]
        public async Task RegisterAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));

            var cutOffInfo = activityContext.GetInput<OperationalCutOffInfo>();

            cutOffInfo.Activity = ActivityNames.Register;
            await this.InitializeAsync(cutOffInfo).ConfigureAwait(false);

            this.logger.LogInformation($"The calculation registration started for ticket: {cutOffInfo.Ticket.TicketId}", $"{cutOffInfo.Ticket.TicketId}");
            await this.balanceProcessor.RegisterAsync(cutOffInfo).ConfigureAwait(false);

            this.logger.LogInformation($"The calculation registration finished for ticket: {cutOffInfo.Ticket.TicketId}", $"{cutOffInfo.Ticket.TicketId}");
        }

        /// <summary>
        /// Gets the ticket asynchronous.
        /// </summary>
        /// <param name="activityContext">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.CalculateSegment)]
        public async Task<OperationalCutOffInfo> ProcessSegmentAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));

            var cutOffInfo = activityContext.GetInput<OperationalCutOffInfo>();

            cutOffInfo.Activity = ActivityNames.CalculateSegment;
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
        [FunctionName(ActivityNames.CalculateSystem)]
        public async Task<OperationalCutOffInfo> ProcessSystemAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));

            var cutOffInfo = activityContext.GetInput<OperationalCutOffInfo>();

            cutOffInfo.Activity = ActivityNames.CalculateSystem;
            await this.InitializeAsync(cutOffInfo).ConfigureAwait(false);

            this.logger.LogInformation($"The system processing is triggered for ticket: {cutOffInfo.Ticket.TicketId}", $"{cutOffInfo.Ticket.TicketId}");
            cutOffInfo.SystemUnbalances = await this.balanceProcessor.ProcessSystemAsync(cutOffInfo.Ticket.TicketId).ConfigureAwait(false);

            return cutOffInfo;
        }

        /// <summary>
        /// Post calculation tasks.
        /// </summary>
        /// <param name="activityContext">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.Complete)]
        public async Task CompleteCalculationAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));

            var cutOffInfo = activityContext.GetInput<OperationalCutOffInfo>();

            cutOffInfo.Activity = ActivityNames.Complete;
            await this.InitializeAsync(cutOffInfo).ConfigureAwait(false);

            this.logger.LogInformation($"The complete calculation is triggered for ticket: {cutOffInfo.Ticket.TicketId}", $"{cutOffInfo.Ticket.TicketId}");
            await this.balanceProcessor.CompleteAsync(cutOffInfo).ConfigureAwait(false);
        }

        /// <summary>
        /// Finalizes the cut off asynchronous.
        /// </summary>
        /// <param name="activityContext">The activity context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.FinalizeCutOff)]
        public async Task FinalizeCutOffAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var cutOffInfo = activityContext.GetInput<OperationalCutOffInfo>();
            try
            {
                this.logger.LogInformation($"Cutoff processing finalizer started for ticket Id {cutOffInfo.Ticket.TicketId}", $"{cutOffInfo.Ticket.TicketId}");

                cutOffInfo.Activity = ActivityNames.FinalizeCutOff;
                await this.InitializeAsync(cutOffInfo).ConfigureAwait(false);

                await this.balanceProcessor.FinalizeProcessAsync(cutOffInfo.Ticket.TicketId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var error = ex.InnerException ?? ex;
                this.logger.LogError(error, $"Failed to finalize the operational cut off for ticket : {cutOffInfo.Ticket.TicketId}", error);
                this.telemetry.TrackEvent(Constants.Critical, EventName.CutoffFailureEvent.ToString("G"));
            }
        }

        /// <summary>
        /// Post calculation tasks.
        /// </summary>
        /// <param name="activityContext">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.HandleFailure)]
        public async Task HandleFailureAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));

            var ticket = activityContext.GetInput<Ticket>();
            await this.InitializeAsync().ConfigureAwait(false);

            this.logger.LogInformation($"Handle failure is triggered for ticket: {ticket.TicketId}");
            await this.cutoffFailureHandler.HandleFailureAsync(this.unitOfWork, new FailureInfo(ticket.TicketId, ticket.ErrorMessage)).ConfigureAwait(false);
        }

        /// <summary>
        /// Purges the operational cut off history.
        /// </summary>
        /// <param name="timer">The timer.</param>
        /// <param name="durableOrchestrationClient">The durable orchestration client.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("PurgeOperationalCutOffHistory")]
        public async Task PurgeOperationalCutOffHistoryAsync(
            [TimerTrigger("%OperationalCutOffPurgeInterval%")] TimerInfo timer,
            [DurableClient] IDurableOrchestrationClient durableOrchestrationClient,
            Microsoft.Azure.WebJobs.ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(timer, nameof(timer));
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            this.logger.LogInformation($"Purge operational cut off history function triggered with schedule: {timer.Schedule}", Constants.PurgeOperationalCutOffHistoryKey);
            try
            {
                await this.InitializeAsync().ConfigureAwait(false);
                this.logger.LogInformation($"The purge operational cut off history job has started with schedule: {timer.Schedule}", Constants.PurgeOperationalCutOffHistoryKey);
                var tasks = new List<Task>
                {
                    this.PurgeOrchestrationDataAsync(durableOrchestrationClient, Constants.PurgeOperationalCutOffHistoryKey, Constants.PurgingCutOffMessage),
                    this.DoHandleFailureAsync(durableOrchestrationClient, this.unitOfWork, this.cutoffFailureHandler, Constants.OperationalCutOffFailureMessage),
                };
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message, Constants.PurgeOperationalCutOffHistoryKey);
            }

            this.logger.LogInformation($"The purge operational cut off history job has finished at: {DateTime.UtcNow.ToTrue()}", Constants.PurgeOperationalCutOffHistoryKey);
        }

        /// <summary>
        /// Checks if the delta ticket exists asynchronous.
        /// </summary>
        /// <param name="segmentId">The segment identifier.</param>
        /// <param name="ticketRepository">The ticket repository.</param>
        /// <returns>return true/false.</returns>
        private static async Task<bool> ExistsDeltaTicketAsync(int segmentId, IRepository<Ticket> ticketRepository)
        {
            var ticket = await ticketRepository.FirstOrDefaultAsync(
                a => a.CategoryElementId == segmentId && a.TicketTypeId == TicketType.Delta && a.Status == StatusType.PROCESSING)
                              .ConfigureAwait(false);
            return ticket != null;
        }

        /// <summary>
        /// Checks if another cutoff ticket exists asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="segmentId">The segment identifier.</param>
        /// <param name="ticketRepository">The ticket repository.</param>
        /// <returns>returns true/ false.</returns>
        private static async Task<bool> ExistsAnotherCutoffTicketAsync(int ticketId, int segmentId, IRepository<Ticket> ticketRepository)
        {
            var ticket = await ticketRepository.FirstOrDefaultAsync(
                a => a.TicketId < ticketId && a.CategoryElementId == segmentId &&
                a.TicketTypeId == TicketType.Cutoff &&
                a.Status == StatusType.PROCESSING).ConfigureAwait(false);

            return ticket != null;
        }

        /// <summary>
        /// Validates the ticket asynchronous.
        /// </summary>
        /// <param name="ticketRepository">The ticket repository.</param>
        /// <param name="ticket">The ticket.</param>
        /// <returns>returns true/ false.</returns>
        private async Task<bool> ValidateTicketAsync(IRepository<Ticket> ticketRepository, Ticket ticket, int ticketId)
        {
            if (ticket == null || ticket.Status != StatusType.PROCESSING)
            {
                this.logger.LogInformation($"Ticket {ticketId} does not exists or is already processed.", $"{ticketId}");
                return false;
            }

            var previousCutOffTicket = await ExistsAnotherCutoffTicketAsync(ticket.TicketId, ticket.CategoryElementId, ticketRepository).ConfigureAwait(false);

            if (previousCutOffTicket)
            {
                await this.FailTicketAsync(ticket, ticketRepository, EntitiesConstants.CutoffAlreadyRunning).ConfigureAwait(false);
                return false;
            }

            var deltaTicket = await ExistsDeltaTicketAsync(ticket.CategoryElementId, ticketRepository).ConfigureAwait(false);
            if (deltaTicket)
            {
                await this.FailTicketAsync(ticket, ticketRepository, EntitiesConstants.DeltaAlreadyRunning).ConfigureAwait(false);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Fails the ticket asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketRepository">The ticket repository.</param>
        /// <param name="errorMessage">The error message.</param>
        private async Task FailTicketAsync(Ticket ticket, IRepository<Ticket> ticketRepository, string errorMessage)
        {
            ticket.Status = StatusType.FAILED;
            ticket.ErrorMessage = errorMessage;
            ticketRepository.Update(ticket);

            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Transforms to calculation movements.
        /// </summary>
        /// <param name="movements">The movements.</param>
        /// <returns>list of movement calculation input.</returns>
        private IEnumerable<MovementCalculationInput> TransformToCalculationMovements(IEnumerable<Movement> movements)
        {
            return movements.Select(x => new MovementCalculationInput
            {
                MovementId = x.MovementId,
                SourceNodeId = x.MovementSource?.SourceNodeId,
                DestinationNodeId = x.MovementDestination?.DestinationNodeId,
                SourceProductId = x.MovementSource?.SourceProductId,
                DestinationProductId = x.MovementDestination?.DestinationProductId,
                OperationalDate = x.OperationalDate,
                NetStandardVolume = x.NetStandardVolume,
                MessageTypeId = x.MessageTypeId,
                UncertaintyPercentage = x.UncertaintyPercentage,
                MeasurementUnit = x.MeasurementUnit.HasValue ? x.MeasurementUnit.ToString() : null,
            });
        }
    }
}