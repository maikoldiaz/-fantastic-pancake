// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogisticsOrchestrator.cs" company="Microsoft">
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
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using EntityConstants = Ecp.True.Entities.Constants;
    using SqlConstants = Ecp.True.DataAccess.Sql.Constants;

    /// <summary>
    /// Logistics Orchestrator.
    /// </summary>
    /// <seealso cref="FunctionBase" />
    public class LogisticsOrchestrator : OrchestratorBase
    {
        /// <summary>
        /// The ownership calculation service.
        /// </summary>
        private readonly IOwnershipCalculationService ownershipCalculationService;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<LogisticsOrchestrator> logger;

        /// <summary>
        /// The logistics processor.
        /// </summary>
        private readonly ILogisticsProcessor logisticsProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogisticsOrchestrator" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="logisticsProcessor">The logistics processor.</param>
        /// <param name="ownershipCalculationService">The ownership calculation service.</param>
        public LogisticsOrchestrator(
            ITrueLogger<LogisticsOrchestrator> logger,
            IConfigurationHandler configurationHandler,
            IAzureClientFactory azureClientFactory,
            IServiceProvider serviceProvider,
            ILogisticsProcessor logisticsProcessor,
            IOwnershipCalculationService ownershipCalculationService)
            : base(configurationHandler, azureClientFactory, serviceProvider, logger)
        {
            this.logger = logger;
            this.logisticsProcessor = logisticsProcessor;
            this.ownershipCalculationService = ownershipCalculationService;
        }

        /// <summary>
        /// Calculate the official logistics asynchronous.
        /// </summary>
        /// <param name="message">The ticket Id.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="deliveryCount">The deliveryCount.</param>
        /// <param name="durableOrchestrationClient">The starter.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("ProcessOfficialLogistics")]
        public async Task CalculateLogisticAsync(
            [ServiceBusTrigger("%OfficialLogistics%", Connection = "IntegrationServiceBusConnectionString", IsSessionsEnabled = true)] QueueMessage message,
            string label,
            string replyTo,
            int deliveryCount,
            [DurableClient] IDurableOrchestrationClient durableOrchestrationClient,
            ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(message, nameof(message));

            this.logger.LogInformation($"Logistics processing is requested for ticket {message.TicketId}", $"{message.TicketId}");
            try
            {
                await this.InitializeAsync().ConfigureAwait(false);
                this.ProcessMetadata(label, FunctionNames.OfficialLogistics, replyTo);

                this.logger.LogInformation($"Official logistics orchestrator is triggered for ticket {message.TicketId} with instance id {context.InvocationId}", $"{message.TicketId}");
                var ticket = await this.logisticsProcessor.GetTicketAsync(message.TicketId).ConfigureAwait(false);
                var logisticsOrchestratorData = new LogisticsOrchestratorData
                {
                    ChaosValue = label,
                    Caller = FunctionNames.OfficialLogistics,
                    Orchestrator = OrchestratorNames.OfficialLogisticsOrchestrator,
                    ReplyTo = replyTo,
                    Ticket = ticket,
                    SystemType = message.SystemTypeId,
                };

                await this.TryStartAsync(
                    durableOrchestrationClient,
                    "OfficialLogisticsOrchestrator",
                    message.TicketId.ToString(CultureInfo.InvariantCulture),
                    logisticsOrchestratorData).ConfigureAwait(false);
                this.logger.LogInformation($"The Official logistics processing is triggered for ticket: {message.TicketId}", $"{message.TicketId}");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, Constants.OfficialLogisticsFailureMessage, $"{message.TicketId}");
                if (deliveryCount < 10)
                {
                    throw;
                }

                await this.HandleLogisticsErrorsAsync(ex, message.TicketId).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Officials the logistics orchestrator asynchronous.
        /// </summary>
        /// <param name="orchestrationContext">The orchestration context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("OfficialLogisticsOrchestrator")]
        public async Task OfficialLogisticsOrchestratorAsync(
        [OrchestrationTrigger] IDurableOrchestrationContext orchestrationContext)
        {
            ArgumentValidators.ThrowIfNull(orchestrationContext, nameof(orchestrationContext));
            var logisticsOrchestratorData = orchestrationContext.GetInput<LogisticsOrchestratorData>();
            try
            {
                this.logger.LogInformation($"Official logistics orchestration started for ticket {logisticsOrchestratorData.Ticket.TicketId}", $"{logisticsOrchestratorData.Ticket.TicketId}");
                this.ProcessMetadata(
                    logisticsOrchestratorData.ChaosValue,
                    string.Join(".", logisticsOrchestratorData.Caller, logisticsOrchestratorData.Orchestrator),
                    logisticsOrchestratorData.ReplyTo);
                await this.ExecuteActivityAsync(ActivityNames.ProcessOfficialLogisticsAnalytics, orchestrationContext, logisticsOrchestratorData).ConfigureAwait(true);
                this.logger.LogInformation($"Official logistics orchestration finished for ticket {logisticsOrchestratorData.Ticket.TicketId}", $"{logisticsOrchestratorData.Ticket.TicketId}");
            }
            catch (Exception ex)
            {
                var error = ex.InnerException ?? ex;
                this.logger.LogError(error, Constants.OfficialLogisticsFailureMessage, error);
                orchestrationContext.SetCustomStatus(Constants.CustomFailureStatus);
                this.logger.LogInformation($"Calling HandleFailure activity from official logistics orchestrator for {logisticsOrchestratorData.Ticket.TicketId}.", $"{logisticsOrchestratorData}");
                await this.ExecuteActivityAsync(
                    ActivityNames.OfficialLogisticsHandleFailure,
                    orchestrationContext,
                    Tuple.Create(logisticsOrchestratorData, Constants.OfficialLogisticsFailureMessage)).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Processes the analytics asynchronous.
        /// </summary>
        /// <param name="activityContext">The activity context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.ProcessOfficialLogisticsAnalytics)]
        public async Task GenerateLogisticsAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var logisticsOrchestratorData = activityContext.GetInput<LogisticsOrchestratorData>();
            this.logger.LogInformation($"Process analytics is requested for ticket: {logisticsOrchestratorData.Ticket.TicketId} ", $"{logisticsOrchestratorData.Ticket.TicketId} ");
            logisticsOrchestratorData.Activity = ActivityNames.ProcessOfficialLogisticsAnalytics;
            await this.InitializeAsync(logisticsOrchestratorData).ConfigureAwait(false);
            await this.logisticsProcessor.GenerateOfficialLogisticsAsync(logisticsOrchestratorData.Ticket, logisticsOrchestratorData.SystemType.GetValueOrDefault()).ConfigureAwait(false);
        }

        /// <summary>
        /// Clears the data asynchronous.
        /// </summary>
        /// <param name="activityContext">The activityContext.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName(ActivityNames.OfficialLogisticsHandleFailure)]
        public async Task HandleFailureAsync(
            [ActivityTrigger] IDurableActivityContext activityContext)
        {
            ArgumentValidators.ThrowIfNull(activityContext, nameof(activityContext));
            var result = activityContext.GetInput<Tuple<LogisticsOrchestratorData, string>>();
            this.logger.LogInformation($"Clear official  logistics data is requested for ticket: {result.Item1.Ticket.TicketId}", $"{result.Item1.Ticket.TicketId}");
            await this.InitializeAsync().ConfigureAwait(false);
            await this.HandleLogisticsErrorsAsync(new Exception(result.Item2), result.Item1.Ticket.TicketId).ConfigureAwait(false);
        }

        /// <summary>
        /// Handles the logistics database errors asynchronous.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>The task.</returns>
        private async Task HandleLogisticsErrorsAsync(Exception exception, int ticketId)
        {
            try
            {
                var errorMessage = string.Empty;
                var sqlException = exception as SqlException;
                if (sqlException != null)
                {
                    if (exception.Message.EqualsIgnoreCase(SqlConstants.NoSapHomologationForMovementType))
                    {
                        this.logger.LogError(sqlException, $"{EntityConstants.NoSapHomologationFoundForMovementType} Ticket Id: {ticketId}");
                        errorMessage = EntityConstants.NoSapHomologationFoundForMovementType;
                    }
                    else if (exception.Message.EqualsIgnoreCase(SqlConstants.InvalidCombinationToSivMovement))
                    {
                        this.logger.LogError(sqlException, $"{EntityConstants.InvalidCombinationToSivMovement} Ticket Id: {ticketId}");
                        errorMessage = EntityConstants.InvalidCombinationToSivMovement;
                    }
                    else
                    {
                        this.logger.LogError(sqlException, $"Error occurred for ticket Id: {ticketId}. Error: {exception.Message}");
                        errorMessage = exception.Message;
                    }
                }
                else
                {
                    this.logger.LogError(exception, $"Exception while processing ticket Id: {ticketId} for logistics generation.");
                    errorMessage = exception.Message;
                }

                await this.ownershipCalculationService.UpdateTicketErrorsAsync(ticketId, errorMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
            }
        }
    }
}
