// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeadletterProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Deadletter
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Configuration.Entities;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.HandleFailure;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Deadletter.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using QueueMessage = Ecp.True.Host.Functions.Core.Entities.QueueMessage;

    /// <summary>
    /// The Deadlettering.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Functions.Core.FunctionBase" />
    public class DeadletterProcessor : FunctionBase
    {
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        private readonly ITrueLogger<DeadletterProcessor> logger;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// The Deadletter processor.
        /// </summary>
        private readonly IDeadletterProcessor deadletterProcessor;

        /// <summary>
        /// The failure handler factory.
        /// </summary>
        private readonly IFailureHandlerFactory failureHandlerFactory;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeadletterProcessor" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="deadletterProcessor">The deadletter processor.</param>
        /// <param name="failureHandlerFactory">The failureHandlerFactory.</param>
        /// <param name="unitOfWorkFactory">The unit of work.</param>
        public DeadletterProcessor(
            IServiceProvider serviceProvider,
            ITrueLogger<DeadletterProcessor> logger,
            ITelemetry telemetry,
            IDeadletterProcessor deadletterProcessor,
            IFailureHandlerFactory failureHandlerFactory,
            IUnitOfWorkFactory unitOfWorkFactory)
            : base(serviceProvider)
        {
            ArgumentValidators.ThrowIfNull(failureHandlerFactory, nameof(failureHandlerFactory));
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));

            this.telemetry = telemetry;
            this.logger = logger;
            this.deadletterProcessor = deadletterProcessor;
            this.failureHandlerFactory = failureHandlerFactory;
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        }

        /// <summary>
        /// Transforms the movement asynchronous.
        /// </summary>
        /// <param name="deadletteredMessage">The message.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("ProcessDeadletter")]
        public async Task GetDeadletteredMessageAsync(
            [ServiceBusTrigger("%DeadLetter%", Connection = "IntegrationServiceBusConnectionString")] DeadletteredMessage deadletteredMessage,
            string label,
            string replyTo,
            ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(deadletteredMessage, nameof(deadletteredMessage));

            this.logger.LogInformation($"Deadletter processing triggered for message {JsonConvert.SerializeObject(deadletteredMessage)}", Constants.Deadletter);
            if (string.IsNullOrWhiteSpace(deadletteredMessage.ProcessName) || string.IsNullOrWhiteSpace(deadletteredMessage.QueueName))
            {
                this.logger.LogInformation($"Deadletter message is malformed. Message {JsonConvert.SerializeObject(deadletteredMessage)}", Constants.Deadletter);
            }

            await this.TryInitializeAsync().ConfigureAwait(false);
            this.ProcessMetadata(label, FunctionNames.Deadletter, replyTo);

            this.logger.LogInformation($"Deadletter processing started for process {deadletteredMessage.ProcessName}", Constants.Deadletter);

            await this.deadletterProcessor.ProcessAsync(deadletteredMessage).ConfigureAwait(false);
            this.logger.LogInformation($"Deadletter processing finished for process {deadletteredMessage.ProcessName}", Constants.Deadletter);
        }

        /// <summary>
        /// Processes the file registration retry deadlettering asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The task.</returns>
        [FunctionName("RetryJsonDeadlettering")]
        public async Task ProcessFileRegistrationRetryDeadletteringAsync(
            [ServiceBusTrigger("%RetryJsonQueue%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] string message)
        {
            await this.ProcessQueueMessageAsync(message, "RetryJsonDeadlettering", true).ConfigureAwait(false);
        }

        /// <summary>
        /// Processes the excel movement and inventory deadlettering asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The task.</returns>
        [FunctionName("ExcelMovementAndInventoryDeadlettering")]
        public async Task ProcessExcelMovementAndInventoryDeadletteringAsync(
          [ServiceBusTrigger("%ExcelQueue%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] string message)
        {
            await this.ProcessQueueMessageAsync(message, "ExcelMovementAndInventoryDeadlettering").ConfigureAwait(false);
        }

        /// <summary>
        /// Processes the excel event deadlettering asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The task.</returns>
        [FunctionName("ExcelEventDeadlettering")]
        public async Task ProcessExcelEventDeadletteringAsync(
         [ServiceBusTrigger("%ExcelEventQueue%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] string message)
        {
            await this.ProcessQueueMessageAsync(message, "ExcelEventDeadlettering").ConfigureAwait(false);
        }

        /// <summary>
        /// Processes the excel contract deadlettering asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The task.</returns>
        [FunctionName("ExcelContractDeadlettering")]
        public async Task ProcessExcelContractDeadletteringAsync(
          [ServiceBusTrigger("%ExcelContractQueue%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] string message)
        {
            await this.ProcessQueueMessageAsync(message, "ExcelContractDeadlettering").ConfigureAwait(false);
        }

        /// <summary>
        /// Processes the finalizer deadlettering.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        [FunctionName("FinalizerDeadlettering")]
        public void ProcessFinalizerDeadlettering(
          [ServiceBusTrigger("%Finalizer%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] int ticketId)
        {
            this.telemetry.TrackEvent(Constants.Critical, $"Failed to finalize the process for ticket {ticketId}");
        }

        /// <summary>
        /// Processes the ownership rules deadlettering.
        /// </summary>
        /// <param name="source">The source.</param>
        [FunctionName("OwnershipRulesDeadlettering")]
        public void ProcessOwnershipRulesDeadlettering(
          [ServiceBusTrigger("%OwnershipRulesQueue%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] string source)
        {
            this.telemetry.TrackEvent(Constants.Critical, $"Failed to sync ownership rule for source {source}");
        }

        /// <summary>
        /// Processes the re calculate ownership deadlettering asynchronous.
        /// </summary>
        /// <param name="recalculateOwnershipMessage">The ownership node ID.</param>
        /// <returns>The task.</returns>
        [FunctionName("ReCalculateOwnershipDeadlettering")]
        public async Task ProcessReCalculateOwnershipDeadletteringAsync(
         [ServiceBusTrigger("%CalculateOwnershipQueue%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] RecalculateOwnershipMessage recalculateOwnershipMessage)
        {
            this.logger.LogInformation($"Deadlettering message {JsonConvert.SerializeObject(recalculateOwnershipMessage)}", "ReCalculateOwnershipDeadlettering");
            ArgumentValidators.ThrowIfNull(recalculateOwnershipMessage, nameof(recalculateOwnershipMessage));

            var entity = new JObject();
            entity.Add(new JProperty("OwnershipNodeId", recalculateOwnershipMessage.OwnershipNodeId));
            entity.Add(new JProperty("HasDeletedMovementOwnerships", recalculateOwnershipMessage.HasDeletedMovementOwnerships));

            await this.ProcessAsync(
                entity,
                "ReCalculateOwnership",
                QueueConstants.CalculateOwnershipQueue,
                ticketId: recalculateOwnershipMessage.OwnershipNodeId).ConfigureAwait(false);
        }

        /// <summary>
        /// Processes the logistics deadlettering asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The task.</returns>
        [FunctionName("LogisticsDeadlettering")]
        public async Task ProcessLogisticsDeadletteringAsync(
         [ServiceBusTrigger("%LogisticsQueue%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] string message)
        {
            try
            {
                var queueMessage = JsonConvert.DeserializeObject<Entities.Core.QueueMessage>(message);
                await this.TryInitializeAsync().ConfigureAwait(false);
                this.logger.LogInformation($"Handling Deadletterred logistics ticket {queueMessage.TicketId}");
                var logisticsFailureHandler = this.failureHandlerFactory.GetFailureHandler(TicketType.Logistics);
                await logisticsFailureHandler.HandleFailureAsync(this.unitOfWork, new FailureInfo(queueMessage.TicketId, Constants.LogisticsFailureMessage)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.telemetry.TrackEvent(Constants.Critical, $"Handling deadlettered logistics ticket failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Processes the operational cutoff deadlettering asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket ID.</param>
        /// <returns>The task.</returns>
        [FunctionName("OperationalCutoffDeadlettering")]
        public async Task ProcessOperationalCutoffDeadletteringAsync(
         [ServiceBusTrigger("%OperationalCutoffQueue%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] int ticketId)
        {
            try
            {
                await this.TryInitializeAsync().ConfigureAwait(false);
                this.logger.LogInformation($"Deadlettering message {JsonConvert.SerializeObject(ticketId)}");
                var cutoffFailureHandler = this.failureHandlerFactory.GetFailureHandler(TicketType.Cutoff);
                await cutoffFailureHandler.HandleFailureAsync(this.unitOfWork, new FailureInfo(ticketId, Constants.OperationalCutOffFailureMessage)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Deadlettering failed for the cutoff process for the {ticketId}");
                this.telemetry.TrackEvent(Constants.Critical, $"Deadlettering failed for the cutoff process for the {ticketId}");
            }
        }

        /// <summary>
        /// Processes the ownership deadlettering asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket ID.</param>
        /// <returns>The task.</returns>
        [FunctionName("OwnershipDeadlettering")]
        public async Task ProcessOwnershipDeadletteringAsync(
        [ServiceBusTrigger("%OwnershipQueue%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] int ticketId)
        {
            try
            {
                await this.TryInitializeAsync().ConfigureAwait(false);
                this.logger.LogInformation($"Deadlettering message {JsonConvert.SerializeObject(ticketId)}");
                var ownershipFailureHandler = this.failureHandlerFactory.GetFailureHandler(TicketType.Ownership);
                await ownershipFailureHandler.HandleFailureAsync(this.unitOfWork, new FailureInfo(ticketId, Constants.OwnershipFailureMessage)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Deadlettering failed for the ownership process for the {ticketId}");
                this.telemetry.TrackEvent(Constants.Critical, $"Deadlettering failed for the ownership process for the {ticketId}");
            }
        }

        /// <summary>
        /// Processes the delta deadlettering asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>The task.</returns>
        [FunctionName("DeltaDeadlettering")]
        public async Task ProcessDeltaDeadletteringAsync(
        [ServiceBusTrigger("%DeltaQueue%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] int ticketId)
        {
            try
            {
                await this.TryInitializeAsync().ConfigureAwait(false);
                this.logger.LogInformation($"Deadlettering message {JsonConvert.SerializeObject(ticketId)}");
                var deltaFailureHandler = this.failureHandlerFactory.GetFailureHandler(TicketType.Delta);
                await deltaFailureHandler.HandleFailureAsync(this.unitOfWork, new FailureInfo(ticketId, Constants.DeltaFailureMessage)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.telemetry.TrackEvent(Constants.Critical, $"Deadlettering failed for the delta process : {ex.Message}");
            }
        }

        /// <summary>
        /// Processes the sap deadlettering asynchronous.
        /// </summary>
        /// <param name="sapQueueMessage">The sap queue message.</param>
        /// <returns>The Task.</returns>
        [FunctionName("SapDeadlettering")]
        public Task ProcessSapDeadletteringAsync(
        [ServiceBusTrigger("%Sap%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] SapQueueMessage sapQueueMessage)
        {
            return this.ProcessSapQueueMessageAsync(sapQueueMessage, "ProcessSapDeadlettering");
        }

        /// <summary>
        /// Processes the Consolidated Data deadlettering asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>The task.</returns>
        [FunctionName("ConsolidatedDataDeadlettering")]
        public async Task ProcessConsolidatedDataDeadletteringAsync(
        [ServiceBusTrigger("%ConsolidatedDataQueue%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] int ticketId)
        {
            try
            {
                await this.TryInitializeAsync().ConfigureAwait(false);
                this.logger.LogInformation($"Handling deadlettered official delta ticket {ticketId}");
                var officialDeltaFailureHandler = this.failureHandlerFactory.GetFailureHandler(TicketType.OfficialDelta);
                await officialDeltaFailureHandler.HandleFailureAsync(this.unitOfWork, new FailureInfo(ticketId, Constants.ConsolidationFailureMessage)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.telemetry.TrackEvent(Constants.Critical, $"Failed to handle the deadlettered official delta ticket : {ex.Message}");
            }
        }

        /// <summary>
        /// Processes the Official Delta deadlettering asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>The task.</returns>
        [FunctionName("OfficialDeltaDeadlettering")]
        public async Task ProcessOfficialDeltaDataDeadletteringAsync(
        [ServiceBusTrigger("%OfficialDeltaQueue%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] int ticketId)
        {
            try
            {
                await this.TryInitializeAsync().ConfigureAwait(false);
                this.logger.LogInformation($"Handling deadlettered official delta ticket {ticketId}");
                var officialDeltaFailureHandler = this.failureHandlerFactory.GetFailureHandler(TicketType.OfficialDelta);
                await officialDeltaFailureHandler.HandleFailureAsync(this.unitOfWork, new FailureInfo(ticketId, Constants.OfficialDeltaFailureMessage)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.telemetry.TrackEvent(Constants.Critical, $"Failed to handle the deadlettered official delta ticket : {ex.Message}");
            }
        }

        /// <summary>
        /// Processes the before cutoff deadlettering asynchronous.
        /// </summary>
        /// <param name="executionId">The execution identifier.</param>
        /// <returns>The task.</returns>
        [FunctionName("BeforeCutoffDeadlettering")]
        public async Task ProcessBeforeCutoffDeadletteringAsync(
        [ServiceBusTrigger("%BeforeCutoffReportQueue%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] int executionId)
        {
            try
            {
                await this.TryInitializeAsync().ConfigureAwait(false);
                this.logger.LogInformation($"Handling deadlettered before cutoff report {executionId}");

                await this.deadletterProcessor.HandleReportFailureAsync(executionId, ReportType.BeforeCutOff).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.telemetry.TrackEvent(Constants.Critical, $"Failed to handle the deadlettered official delta ticket : {ex.Message}");
            }
        }

        /// <summary>
        /// Processes the before cutoff deadlettering asynchronous.
        /// </summary>
        /// <param name="executionId">The execution identifier.</param>
        /// <returns>The task.</returns>
        [FunctionName("OfficialInitialBalanceDeadlettering")]
        public async Task ProcessOfficialInitialBalanceDeadletteringAsync(
        [ServiceBusTrigger("%OfficialInitialBalanceReportQueue%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] int executionId)
        {
            try
            {
                await this.TryInitializeAsync().ConfigureAwait(false);
                this.logger.LogInformation($"Handling deadlettered before cutoff report {executionId}");

                await this.deadletterProcessor.HandleReportFailureAsync(executionId, ReportType.OfficialInitialBalance).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.telemetry.TrackEvent(Constants.Critical, $"Failed to handle the deadlettered official delta ticket : {ex.Message}");
            }
        }

        /// <summary>
        /// Processes the before cutoff deadlettering asynchronous.
        /// </summary>
        /// <param name="executionId">The execution identifier.</param>
        /// <returns>The task.</returns>
        [FunctionName("OperativeBalanceDeadlettering")]
        public async Task ProcessOperativeBalanceDeadletteringAsync(
        [ServiceBusTrigger("%OperativeBalanceReportQueue%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] int executionId)
        {
            try
            {
                await this.TryInitializeAsync().ConfigureAwait(false);
                this.logger.LogInformation($"Handling deadlettered before cutoff report {executionId}");

                await this.deadletterProcessor.HandleReportFailureAsync(executionId, ReportType.OperativeBalance).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.telemetry.TrackEvent(Constants.Critical, $"Failed to handle the deadlettered official delta ticket : {ex.Message}");
            }
        }

        /// <summary>
        /// Does the initialize asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        protected override async Task DoInitializeAsync()
        {
            var configurationHandler = this.Resolve<IConfigurationHandler>();
            var azureClientFactory = this.Resolve<IAzureClientFactory>();
            if (azureClientFactory.IsReady)
            {
                return;
            }

            var storageSettings = await configurationHandler.GetConfigurationAsync<StorageSettings>(ConfigurationConstants.StorageSettings).ConfigureAwait(false);
            var serviceBusSettings = await configurationHandler.GetConfigurationAsync<ServiceBusSettings>(ConfigurationConstants.ServiceBusSettings).ConfigureAwait(false);

            azureClientFactory.Initialize(new AzureConfiguration(storageSettings, serviceBusSettings));
        }

        /// <summary>
        /// Processes the sap queue message asynchronous.
        /// </summary>
        /// <param name="sapRequest">The sap request.</param>
        /// <param name="process">The process.</param>
        /// <returns>The Task.</returns>
        private async Task ProcessSapQueueMessageAsync(SapQueueMessage sapRequest, string process)
        {
            try
            {
                ArgumentValidators.ThrowIfNull(sapRequest, nameof(sapRequest));

                this.logger.LogInformation($"Deadlettering message {JsonConvert.SerializeObject(sapRequest)}", process);

                await this.TryInitializeAsync().ConfigureAwait(false);
                await this.deadletterProcessor.HandleSapFailureAsync(sapRequest).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Exceptions in deadletter sap message. {ex.Message}");
            }
        }

        /// <summary>
        /// Processes the queue message asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="process">The process.</param>
        /// <param name="isJsonMessage">if set to <c>true</c> [is retry].</param>
        private async Task ProcessQueueMessageAsync(string message, string process, bool isJsonMessage = false)
        {
            try
            {
                ArgumentValidators.ThrowIfNullOrEmpty(message, nameof(message));

                this.logger.LogInformation($"Deadlettering message {JsonConvert.SerializeObject(message)}", process);

                var entityId = string.Empty;
                var isRetry = false;
                await this.TryInitializeAsync().ConfigureAwait(false);

                if (isJsonMessage)
                {
                    var queueMessage = JsonConvert.DeserializeObject<JsonQueueMessage>(message);
                    isRetry = queueMessage.IsRetry;
                    entityId = queueMessage.IsRetry ? queueMessage.FileRegistrationTransactionId.ToString(CultureInfo.InvariantCulture) : queueMessage.UploadId;
                }
                else
                {
                    var queueMessage = JsonConvert.DeserializeObject<QueueMessage>(message);
                    entityId = queueMessage.UploadId ?? queueMessage.UploadFileId;
                }

                if (isRetry)
                {
                    await this.deadletterProcessor.HandleRegistrationRetryFailureAsync(entityId).ConfigureAwait(false);
                }
                else
                {
                    await this.deadletterProcessor.HandleRegistrationFailureAsync(entityId).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Exceptions in deadletter registration message. {ex.Message}");
            }
        }

        /// <summary>
        /// Processes the asynchronous.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="processName">Name of the process.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="isSessionEnabled">if set to <c>true</c> [is session enabled].</param>
        /// <param name="ticketId">The ticket identifier.</param>
        private async Task ProcessAsync(
            JObject content,
            string processName,
            string queueName,
            bool isSessionEnabled = false,
            int? ticketId = null)
        {
            try
            {
                await this.TryInitializeAsync().ConfigureAwait(false);

                var message = new DeadletteredMessage
                {
                    Content = content,
                    ProcessName = processName,
                    QueueName = queueName,
                    IsSessionEnabled = isSessionEnabled,
                    ErrorMessage = $"{queueName} queue dead lettered message.",
                    TicketId = ticketId,
                };

                await this.deadletterProcessor.ProcessAsync(message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.telemetry.TrackEvent(Constants.Critical, $"Deadlettering failed for the {processName} process : {ex.Message}");
            }
        }
    }
}
