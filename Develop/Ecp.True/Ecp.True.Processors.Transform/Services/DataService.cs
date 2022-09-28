// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Entities.Sap.Purchases;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Ownership.Services.Interfaces;
    using Ecp.True.Processors.Transform.Services.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The data service.
    /// </summary>
    public class DataService : IDataService
    {
        /// <summary>
        /// Gets or sets the service bus queue service.
        /// </summary>
        /// <value>
        /// The service bus queue service.
        /// </value>
        private static readonly IDictionary<string, string> MessageTypeQueueMapping = new Dictionary<string, string>();

        /// <summary>
        /// The messages sent successfully to queue.
        /// </summary>
        private readonly string queueMessagesSent = "Messages sent to queue.";

        /// <summary>
        /// Sending messages to queue.
        /// </summary>
        private readonly string sendingQueueMessages = "Sending the messages to queue.";

        /// <summary>
        /// The messages sent successfully to queue.
        /// </summary>
        private readonly string queueMessageNotSent = "Messages not sent to queue.";

        /// <summary>
        /// The blob created message.
        /// </summary>
        private readonly string blobCreatedMessage = "Blob created successfully";

        /// <summary>
        /// The file registration record inserted message.
        /// </summary>
        private readonly string fileRegistrationRecordInsertedMessage = "FileRegistration record insertion successful";

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<DataService> logger;

        /// <summary>
        /// The blob generator.
        /// </summary>
        private readonly IBlobGenerator blobGenerator;

        /// <summary>
        /// The file registration transaction service.
        /// </summary>
        private readonly IFileRegistrationTransactionService fileRegistrationTransactionService;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The file registration transaction service.
        /// </summary>
        private readonly ILogisticsService logisticsService;

        /// <summary>
        /// Initializes static members of the <see cref="DataService"/> class.Initializes the <see cref="DataService" /> class.</summary>
        static DataService()
        {
            InitializeMessageTypeQueueMapping();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataService" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="blobGenerator">The blob generator.</param>
        /// <param name="fileRegistrationTransactionService">The file registration transaction service.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="logisticsMovementService">The logistic movement service.</param>
        public DataService(
            ITrueLogger<DataService> logger,
            IBlobGenerator blobGenerator,
            IFileRegistrationTransactionService fileRegistrationTransactionService,
            IAzureClientFactory azureClientFactory,
            ILogisticsService logisticsMovementService)
        {
            this.logger = logger;
            this.blobGenerator = blobGenerator;
            this.fileRegistrationTransactionService = fileRegistrationTransactionService;
            this.azureClientFactory = azureClientFactory;
            this.logisticsService = logisticsMovementService;
        }

        /// <inheritdoc/>
        public async Task SaveAsync(JArray homologatedArray, TrueMessage trueMessage)
        {
            // Try catch here?
            ArgumentValidators.ThrowIfNull(trueMessage, nameof(trueMessage));

            // Generate Blob
            await this.blobGenerator.GenerateBlobsArrayAsync(homologatedArray, trueMessage).ConfigureAwait(false);

            this.logger.LogInformation($"Blobs generated for homologation for {trueMessage.MessageId}", trueMessage.MessageId);

            await this.DoSaveAsync(trueMessage).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task SaveExcelAsync(JArray inventoryArray, JArray movementArray, JArray eventArray, JArray contractArray, TrueMessage trueMessage)
        {
            // try catch here?
            ArgumentValidators.ThrowIfNull(trueMessage, nameof(trueMessage));

            // Generate blobs
            await this.DoSaveExcelAsync(inventoryArray, trueMessage).ConfigureAwait(false);
            await this.DoSaveExcelAsync(movementArray, trueMessage).ConfigureAwait(false);
            await this.DoSaveExcelAsync(eventArray, trueMessage).ConfigureAwait(false);
            await this.DoSaveExcelAsync(contractArray, trueMessage).ConfigureAwait(false);

            this.logger.LogInformation($"Blobs generated for homologation: {trueMessage.MessageId}", trueMessage.MessageId);

            await this.DoSaveAsync(trueMessage).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<string> SaveExternalSourceEntityArrayAsync(JArray entity, TrueMessage message)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            string sourceSystem = GetSourceSystem(entity, message);
            var fileRegistration = this.CreateFileRegistration(message, sourceSystem, GetEventType(message.Message, entity));
            message.FileRegistration = fileRegistration;

            // Save the Json to the blob
            await this.blobGenerator.GenerateBlobsArrayAsync(entity, message.InputBlobPath, SystemType.TRUE.ToString().ToLowerCase()).ConfigureAwait(false);
            this.logger.LogInformation(this.blobCreatedMessage, fileRegistration.UploadId);

            // Save FileRegistration
            await this.fileRegistrationTransactionService.InsertFileRegistrationAsync(fileRegistration).ConfigureAwait(false);
            this.logger.LogInformation(this.fileRegistrationRecordInsertedMessage, fileRegistration.UploadId);

            // Send messages to queue.
            await this.PushMessageToQueueAsync("json", message).ConfigureAwait(false);
            this.logger.LogInformation(this.queueMessagesSent, fileRegistration.UploadId);
            return fileRegistration.UploadId;
        }

        /// <inheritdoc/>
        public async Task<string> SaveExternalSourceEntityAsync(JObject entity, TrueMessage message)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            string sourceSystem = message.SourceSystem.ToString();
            var fileRegistration = this.CreateFileRegistration(message, sourceSystem, GetEventType(message.Message, entity));
            message.FileRegistration = fileRegistration;

            // Save the Json to the blob
            await this.blobGenerator.GenerateBlobsAsync(entity, message.InputBlobPath, SystemType.TRUE.ToString().ToLowerCase()).ConfigureAwait(false);
            this.logger.LogInformation(this.blobCreatedMessage, fileRegistration.UploadId);

            // Save FileRegistration
            await this.fileRegistrationTransactionService.InsertFileRegistrationAsync(fileRegistration).ConfigureAwait(false);
            this.logger.LogInformation(this.fileRegistrationRecordInsertedMessage, fileRegistration.UploadId);

            // Send messages to queue.
            await this.PushMessageToQueueAsync("json", message).ConfigureAwait(false);
            this.logger.LogInformation(this.queueMessagesSent, fileRegistration.UploadId);
            return fileRegistration.UploadId;
        }

        /// <inheritdoc/>
        public async Task<string> SaveLogisticEntityAsync(JObject entity, TrueMessage message)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            ArgumentValidators.ThrowIfNull(message, nameof(message));

            string sourceSystem = message.SourceSystem.ToString();
            var fileRegistration = this.CreateFileRegistration(message, sourceSystem, GetEventType(message.Message, entity));
            message.FileRegistration = fileRegistration;

            // Save the Json to the blob
            await this.blobGenerator.GenerateBlobsAsync(entity, message.InputBlobPath, SystemType.TRUE.ToString().ToLowerCase()).ConfigureAwait(false);
            this.logger.LogInformation(this.blobCreatedMessage, fileRegistration.UploadId);

            // Save FileRegistration
            await this.fileRegistrationTransactionService.InsertFileRegistrationAsync(fileRegistration).ConfigureAwait(false);
            this.logger.LogInformation(this.fileRegistrationRecordInsertedMessage, fileRegistration.UploadId);

            // Actualizar Base de datos.
            await this.logisticsService.ProcessLogisticMovementAsync(
                JsonConvert.DeserializeObject<LogisticMovementResponse>(entity.ToString())).ConfigureAwait(false);
            return fileRegistration.UploadId;
        }

        private static string GetSourceSystem(JArray entity, TrueMessage message)
        {
            switch (message.Message)
            {
                case MessageType.Purchase:
                case MessageType.Sale:
                    return message.SourceSystem.ToString();
                default:
                    return entity.First["SourceSystem"].ToString();
            }
        }

        /// <summary>
        /// Initializes the message type queue mapping.
        /// </summary>
        private static void InitializeMessageTypeQueueMapping()
        {
            MessageTypeQueueMapping.Add(MessageType.Movement.ToString(), QueueConstants.HomologatedMovementsQueue);
            MessageTypeQueueMapping.Add(MessageType.Inventory.ToString(), QueueConstants.HomologatedInventoryQueue);
            MessageTypeQueueMapping.Add(MessageType.Events.ToString(), QueueConstants.HomologatedEventsQueue);
            MessageTypeQueueMapping.Add(MessageType.Contract.ToString(), QueueConstants.HomologatedContractsQueue);
            MessageTypeQueueMapping.Add(MessageType.Sale.ToString(), QueueConstants.HomologatedContractsQueue);
            MessageTypeQueueMapping.Add(MessageType.Purchase.ToString(), QueueConstants.HomologatedContractsQueue);
        }

        private static SystemType CreateSourceType(MessageType message)
        {
            switch (message)
            {
                case MessageType.Purchase:
                    return SystemType.PURCHASE;
                case MessageType.Sale:
                    return SystemType.SELL;
                case MessageType.Movement:
                    return SystemType.MOVEMENTS;
                case MessageType.Inventory:
                    return SystemType.INVENTORY;
                case MessageType.Logistic:
                    return SystemType.LOGISTIC;
                default:
                    return SystemType.EVENTS;
            }
        }

        private static string GetEventType(MessageType message, JArray entity)
        {
            if (message.Equals(MessageType.Purchase))
            {
                return JsonConvert.DeserializeObject<SapPurchase>(entity.First().ToString()).PurchaseOrder.Event;
            }
            else if (message.Equals(MessageType.Sale))
            {
                return JsonConvert.DeserializeObject<Sale>(entity.First().ToString()).OrderSale.ControlData.EventSapPo;
            }
            else
            {
                return Constants.EventInsert;
            }
        }

        private static string GetEventType(MessageType message, JObject entity)
        {
            if (message.Equals(MessageType.Purchase))
            {
                return JsonConvert.DeserializeObject<SapPurchase>(entity.ToString()).PurchaseOrder.Event;
            }
            else if (message.Equals(MessageType.Sale))
            {
                return JsonConvert.DeserializeObject<Sale>(entity.ToString()).OrderSale.ControlData.EventSapPo;
            }
            else if (message.Equals(MessageType.Logistic))
            {
                return Constants.EventSapCreate;
            }
            else
            {
                return Constants.EventInsert;
            }
        }

        private static FileRegistrationActionType GetFileRegistrationActionType(string movement)
        {
            return movement.EqualsIgnoreCase(Constants.EventSapCreate) ? FileRegistrationActionType.Insert : FileRegistrationActionType.Update;
        }

        private async Task DoSaveAsync(TrueMessage trueMessage)
        {
            ArgumentValidators.ThrowIfNull(trueMessage, nameof(trueMessage));

            // Generate file records in DB for non retry case
            if (!trueMessage.IsRetry)
            {
                await this.fileRegistrationTransactionService.UpdateFileRegistrationAsync(trueMessage.FileRegistration).ConfigureAwait(false);
            }

            this.logger.LogInformation(trueMessage.MessageId, this.sendingQueueMessages);

            // Send messages to queue.
            await this.SendMessagesToQueueAsync(trueMessage).ConfigureAwait(false);

            // Log errors.
            await this.fileRegistrationTransactionService.InsertPendingTransactionsAsync(trueMessage.PendingTransactions).ConfigureAwait(false);

            this.logger.LogInformation(this.queueMessagesSent, trueMessage.MessageId);
        }

        /// <summary>
        /// Pushes the message to queue.
        /// </summary>
        /// <param name="queueName">The queue name.</param>
        /// <param name="trueMessage">The trueMessage.</param>
        private async Task PushMessageToQueueAsync(string queueName, TrueMessage trueMessage)
        {
            var client = this.azureClientFactory.GetQueueClient(queueName);
            await client.QueueMessageAsync(
                new JsonQueueMessage { UploadId = trueMessage.MessageId, MessageTypeId = trueMessage.Message, IsOfficialSapMovement = trueMessage.IsOfficialSapMovement }).ConfigureAwait(false);
        }

        private async Task SendMessagesToQueueAsync(TrueMessage trueMessage)
        {
            if (trueMessage.FileRegistration.FileRegistrationTransactions.Count == 0)
            {
                return;
            }

            if (trueMessage.SourceSystem == SystemType.SAP)
            {
                var sapRequest = new SapQueueMessage(SapRequestType.Upload, trueMessage.MessageId);
                var client = this.azureClientFactory.GetQueueClient(QueueConstants.SapQueue);
                await client.QueueScheduleMessageAsync(sapRequest, Constants.SapUploadStatus, Constants.SapQueueIntervalInSecs).ConfigureAwait(false);
            }

            var groupedTransactions = trueMessage.FileRegistration.FileRegistrationTransactions.GroupBy(x => x.SessionId);
            var tasks = groupedTransactions.Select(x => this.QueueMessageAsync(trueMessage, x.ToList()));
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task QueueMessageAsync(TrueMessage trueMessage, IEnumerable<FileRegistrationTransaction> fileRegistrationTransactions)
        {
            foreach (var fileRegistrationTransaction in fileRegistrationTransactions)
            {
                await this.DoQueueMessageAsync(trueMessage, fileRegistrationTransaction).ConfigureAwait(false);
            }
        }

        private async Task DoQueueMessageAsync(TrueMessage trueMessage, FileRegistrationTransaction message)
        {
            if (message.StatusTypeId == StatusType.FAILED)
            {
                return;
            }

            var queueName = default(string);

            try
            {
                queueName = MessageTypeQueueMapping.Single(x => message.BlobPath.Contains(x.Key, StringComparison.InvariantCultureIgnoreCase)).Value;
                var queueClient = this.azureClientFactory.GetQueueClient(queueName);
                await queueClient.QueueSessionMessageAsync(message, message.SessionId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, this.queueMessageNotSent, message.BlobPath);
                message.StatusTypeId = StatusType.FAILED;
                trueMessage.PopulatePendingTransactions(ex.Message, queueName, Constants.TechnicalExceptionParsingErrorMessage);
            }
        }

        private async Task DoSaveExcelAsync(JArray objectJArray, TrueMessage trueMessage)
        {
            if (objectJArray != null)
            {
                // Generate blobs
                await this.blobGenerator.GenerateBlobsArrayAsync(objectJArray, trueMessage).ConfigureAwait(false);
            }
        }

        private FileRegistration CreateFileRegistration(TrueMessage message, string sourceSystem, string movement)
        {
            return new FileRegistration
            {
                BlobPath = message.InputBlobPath,
                Name = $"{message.MessageId}",
                SystemTypeId = SystemType.SAP,
                UploadId = message.MessageId,
                ActionType = GetFileRegistrationActionType(movement),
                MessageDate = DateTime.UtcNow.ToTrue(),
                SourceSystem = sourceSystem,
                SourceTypeId = CreateSourceType(message.Message),
                IntegrationType = message.IntegrationType,
            };
        }
    }
}