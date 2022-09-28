// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Transformer.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Transform
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Host.Functions.Transform.Orchestrator;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using CoreConstants = Ecp.True.Core.Constants;
    using QueueMessage = Ecp.True.Host.Functions.Core.Entities.QueueMessage;

    /// <summary>
    /// The azure function service for canonical transformation and homologation.
    /// </summary>
    public class Transformer : OrchestratorBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<Transformer> logger;

        /// <summary>
        /// The should homologate.
        /// </summary>
        private bool shouldHomologate;

        /// <summary>
        /// The should homologate Purchase.
        /// </summary>
        private bool shouldHomologatePurchase;

        /// <summary>
        /// The should homologate Sale.
        /// </summary>
        private bool shouldHomologateSale;

        /// <summary>
        /// Initializes a new instance of the <see cref="Transformer" /> class.
        /// </summary>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="telemetry">The telemetry.</param>
        public Transformer(
            IConfigurationHandler configurationHandler,
            IAzureClientFactory azureClientFactory,
            IServiceProvider serviceProvider,
            ITrueLogger<Transformer> logger)
            : base(configurationHandler, azureClientFactory, serviceProvider, logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Retries the message asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="starter">The starter.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("JsonTransformer")]
        public Task TransformJsonAsync(
            [ServiceBusTrigger("%RetryMessageQueue%", Connection = "IntegrationServiceBusConnectionString")] JsonQueueMessage message,
            string label,
            string replyTo,
            [DurableClient] IDurableOrchestrationClient starter,
            ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            return this.ProcessAsync(context, message, starter, label, replyTo);
        }

        /// <summary>
        /// Transforms the excel asynchronous.
        /// </summary>
        /// <param name="queueMessage">The queue message.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="starter">The starter.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("ExcelTransformer")]
        public Task TransformExcelAsync(
            [ServiceBusTrigger("%ExcelQueue%", Connection = "IntegrationServiceBusConnectionString")] QueueMessage queueMessage,
            string label,
            string replyTo,
            [DurableClient] IDurableOrchestrationClient starter,
            ExecutionContext context)
        {
            return this.ProcessAsync(context, queueMessage, MessageType.MovementAndInventory, starter, nameof(ExcelRegistrationOrchestrator), label, replyTo);
        }

        /// <summary>
        /// Transforms event excel asynchronous.
        /// </summary>
        /// <param name="queueMessage">The queue message.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="starter">The starter.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("ExcelEventTransformer")]
        public Task TransformEventExcelAsync(
            [ServiceBusTrigger("%ExcelEventQueue%", Connection = "IntegrationServiceBusConnectionString")] QueueMessage queueMessage,
            string label,
            string replyTo,
            [DurableClient] IDurableOrchestrationClient starter,
            ExecutionContext context)
        {
            return this.ProcessAsync(context, queueMessage, MessageType.Events, starter, nameof(ExcelRegistrationOrchestrator), label, replyTo);
        }

        /// <summary>
        /// Transforms event excel asynchronous.
        /// </summary>
        /// <param name="queueMessage">The queue message.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="starter">The starter.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("ExcelContractTransformer")]
        public Task TransformContractExcelAsync(
            [ServiceBusTrigger("%ExcelContractQueue%", Connection = "IntegrationServiceBusConnectionString")] QueueMessage queueMessage,
            string label,
            string replyTo,
            [DurableClient] IDurableOrchestrationClient starter,
            ExecutionContext context)
        {
            return this.ProcessAsync(context, queueMessage, MessageType.Contract, starter, nameof(ExcelRegistrationOrchestrator), label, replyTo);
        }

        /// <summary>
        /// Purges the transform history asynchronous.
        /// </summary>
        /// <param name="timer">The timer.</param>
        /// <param name="durableOrchestrationClient">The durable orchestration client.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("PurgeTransformHistory")]
        public async Task PurgeTransformHistoryAsync(
            [TimerTrigger("%TransformPurgeInterval%")] TimerInfo timer,
            [DurableClient] IDurableOrchestrationClient durableOrchestrationClient,
            ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(timer, nameof(timer));
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(durableOrchestrationClient, nameof(durableOrchestrationClient));

            this.logger.LogInformation($"Purge transform history function triggered with schedule: {timer.Schedule}", CoreConstants.PurgeTransformHistory);
            try
            {
                await this.InitializeAsync().ConfigureAwait(false);
                this.logger.LogInformation($"The purge transform history job has started with schedule: {timer.Schedule}", CoreConstants.PurgeTransformHistory);
                var filter = new OrchestrationStatusQueryCondition
                {
                    CreatedTimeTo = DateTime.UtcNow.AddMinutes(-30),
                };

                var queryResult = await durableOrchestrationClient.ListInstancesAsync(filter, System.Threading.CancellationToken.None).ConfigureAwait(false);
                foreach (var instance in queryResult.DurableOrchestrationState)
                {
                    if (instance.LastUpdatedTime < DateTime.UtcNow.AddDays(-7))
                    {
                        this.logger.LogInformation($"Purging transform history of instance id : {instance.InstanceId} and name : {instance.Name}", CoreConstants.PurgeTransformHistory);
                        await durableOrchestrationClient.PurgeInstanceHistoryAsync(instance.InstanceId).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message, CoreConstants.PurgeTransformHistory);
            }

            this.logger.LogInformation($"The purge transform history job has finished at: {DateTime.UtcNow.ToTrue()}", CoreConstants.PurgeTransformHistory);
        }

        /// <summary>
        /// Does the initialize asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        protected override async Task DoInitializeAsync()
        {
            var configurationHandler = this.Resolve<IConfigurationHandler>();
            this.shouldHomologate = await configurationHandler.GetConfigurationAsync<bool>(ConfigurationConstants.ShouldHomologate).ConfigureAwait(false);
            this.shouldHomologatePurchase = await configurationHandler.GetConfigurationAsync<bool>(ConfigurationConstants.ShouldHomologatePurchase).ConfigureAwait(false);
            this.shouldHomologateSale = await configurationHandler.GetConfigurationAsync<bool>(ConfigurationConstants.ShouldHomologateSale).ConfigureAwait(false);

            await base.DoInitializeAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Builds the true message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="context">The context.</param>
        /// <returns>The true message.</returns>
        private static TrueMessage BuildTrueMessage(QueueMessage message, MessageType messageType, ExecutionContext context)
        {
            var blobPath = $"{message.SystemTypeId.ToString().ToLowerCase()}/xml/{Pluralize(messageType)}/{message.UploadId ?? message.UploadFileId}";

            if (messageType == MessageType.MovementAndInventory || messageType == MessageType.Events || messageType == MessageType.Contract)
            {
                blobPath = $"{message.SystemTypeId.ToString().ToLowerCase()}/{message.UploadId ?? message.UploadFileId}";
            }

            var trueMessage = new TrueMessage(message.SystemTypeId, messageType, message.UploadId ?? message.UploadFileId, blobPath, context.InvocationId);
            if (trueMessage.SourceSystem == SystemType.EXCEL)
            {
                trueMessage.ActionType = message.ActionType == FileRegistrationActionType.ReInject ? FileRegistrationActionType.Insert.ToString("G") : message.ActionType.ToString("G");
            }

            return trueMessage;
        }

        /// <summary>
        /// Pluralizes the specified message type.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <returns>The pluralized message type.</returns>
        private static string Pluralize(MessageType messageType)
        {
            if (messageType == MessageType.Inventory)
            {
                return messageType.ToString().ToLowerCase();
            }

            if (messageType == MessageType.Loss)
            {
                messageType.Pluralize("es").ToLowerCase();
            }

            return messageType.Pluralize().ToLowerCase();
        }

        /// <summary>
        /// Processes the input message.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="queueMessage">The queue message.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="starter">The starter.</param>
        /// <param name="orchestratorName">the orchestrator name.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        private async Task ProcessAsync(
            ExecutionContext context,
            QueueMessage queueMessage,
            MessageType messageType,
            IDurableOrchestrationClient starter,
            string orchestratorName,
            string label,
            string replyTo)
        {
            ArgumentValidators.ThrowIfNull(queueMessage, nameof(queueMessage));
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            await this.InitializeAsync().ConfigureAwait(false);
            this.ProcessMetadata(label, FunctionNames.Transform, replyTo);

            this.logger.LogInformation($"{queueMessage.SystemTypeId} {messageType} received.", queueMessage.UploadId ?? queueMessage.UploadFileId);

            var message = BuildTrueMessage(queueMessage, messageType, context);
            message.ShouldHomologate = this.shouldHomologate;

            var requestMessage = new RegistrationData
            {
                InvocationId = context.InvocationId,
                TrueMessage = message,
                ChaosValue = label,
                ReplyTo = replyTo,
                Caller = FunctionNames.Transform,
                Orchestrator = orchestratorName,
            };

            await this.TryStartAsync(starter, orchestratorName, message.XmlBlobName, requestMessage).ConfigureAwait(true);
            this.logger.LogInformation($"{queueMessage.SystemTypeId} {messageType} processed successfully.", queueMessage.UploadId ?? queueMessage.UploadFileId);
        }

        private bool ShouldHomologateMessage(JsonQueueMessage message)
        {
            switch (message.MessageTypeId)
            {
                case MessageType.Purchase:
                    return this.shouldHomologatePurchase;
                case MessageType.Sale:
                    return this.shouldHomologateSale;
                case MessageType.Sap:
                    return true;
                default:
                    return !message.IsOfficialSapMovement && this.shouldHomologate;
            }
        }

        private async Task ProcessAsync(ExecutionContext context, JsonQueueMessage message, IDurableOrchestrationClient starter, string label, string replyTo)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            var processingMessage = $"Retry message {message.FileRegistrationTransactionId} received.";
            var processedMessage = $"Retry message {message.FileRegistrationTransactionId} processed successfully.";
            var tag = message.FileRegistrationTransactionId.ToString(CultureInfo.InvariantCulture);

            if (!message.IsRetry)
            {
                processingMessage = $"SAP PO File Processing message {message.UploadId} received.";
                processedMessage = $"SAP PO File Processing message {message.UploadId} processed successfully.";

                tag = message.UploadId.ToString(CultureInfo.InvariantCulture);
            }

            await this.InitializeAsync().ConfigureAwait(false);
            this.ProcessMetadata(label, FunctionNames.Transform, replyTo);

            this.logger.LogInformation(processingMessage, tag);

            var trueMessage = new TrueMessage
            {
                SourceSystem = SystemType.SAP,
                TargetSystem = SystemType.TRUE,
                IsRetry = message.IsRetry,
                FileRegistrationTransactionId = message.FileRegistrationTransactionId,
                MessageId = message.UploadId,
                Message = message.MessageTypeId,
                ShouldHomologate = this.ShouldHomologateMessage(message),
                InputBlobPath = $"{SystemType.SAP.ToString().ToLowerCase()}/{message.MessageTypeId}/{message.UploadId}",
            };

            var requestMessage = new RegistrationData
            {
                InvocationId = context.InvocationId,
                TrueMessage = trueMessage,
                ChaosValue = label,
                ReplyTo = replyTo,
                Caller = FunctionNames.Transform,
                Orchestrator = OrchestratorNames.JsonRegistrationOrchestrator,
            };

            await this.TryStartAsync(starter, nameof(JsonRegistrationOrchestrator), tag, requestMessage).ConfigureAwait(true);
            this.logger.LogInformation(processedMessage, tag);
        }
    }
}
