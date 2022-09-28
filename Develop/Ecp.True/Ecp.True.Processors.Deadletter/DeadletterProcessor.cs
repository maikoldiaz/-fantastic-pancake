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

namespace Ecp.True.Processors.Deadletter
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Core.Dto;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Deadletter.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The Deadletter Processor.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Api.ProcessorBase" />
    /// <seealso cref="Ecp.True.Processors.Deadletter.Interfaces.IDeadletterProcessor" />
    public class DeadletterProcessor : ProcessorBase, IDeadletterProcessor
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The reconcile service .
        /// </summary>
        private readonly IReconcileService reconciler;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeadletterProcessor" /> class.
        /// </summary>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="reconciler">The reconciler.</param>
        /// <param name="repositoryFactory">The repository service.</param>
        /// <param name="telemetry">The telemetry.</param>
        public DeadletterProcessor(
            IAzureClientFactory azureClientFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            IReconcileService reconciler,
            IRepositoryFactory repositoryFactory,
            ITelemetry telemetry)
            : base(repositoryFactory)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            ArgumentValidators.ThrowIfNull(azureClientFactory, nameof(azureClientFactory));
            this.azureClientFactory = azureClientFactory;
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.reconciler = reconciler;
            this.telemetry = telemetry;
        }

        /// <inheritdoc/>
        public async Task ProcessAsync(DeadletteredMessage message)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            message.BlobPath = $"{Constants.DeadletteredMessageBlobPath}{Guid.NewGuid()}";
            await this.azureClientFactory.GetBlobStorageSaSClient(ContainerName.True, message.BlobPath).CreateBlobAsync(message.Content.ToString()).ConfigureAwait(false);

            var deadletteredMessageRepository = this.unitOfWork.CreateRepository<DeadletteredMessage>();
            message.Status = true;
            deadletteredMessageRepository.Insert(message);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<bool> RetriggerAsync(IEnumerable<int> messageIds)
        {
            ArgumentValidators.ThrowIfNull(messageIds, nameof(messageIds));

            var deadletteredMessageRepository = this.unitOfWork.CreateRepository<DeadletteredMessage>();
            var deadletteredMessages = await deadletteredMessageRepository.GetAllAsync(x => x.Status && messageIds.Contains(x.DeadletteredMessageId)).ConfigureAwait(false);
            if (messageIds.Any(x => !deadletteredMessages.Any(y => y.DeadletteredMessageId == x)))
            {
                return false;
            }

            foreach (var deadletteredMessage in deadletteredMessages)
            {
                var queueClient = this.azureClientFactory.GetQueueClient(deadletteredMessage.QueueName);
                if (deadletteredMessage.IsSessionEnabled && deadletteredMessage.TicketId.HasValue)
                {
                    await queueClient.QueueSessionMessageAsync(deadletteredMessage.TicketId, deadletteredMessage.TicketId.ToString()).ConfigureAwait(false);
                }
                else
                {
                    var content = await this.GetContentAsync(deadletteredMessage.BlobPath).ConfigureAwait(false);
                    await queueClient.QueueMessageAsync(content).ConfigureAwait(false);
                }

                deadletteredMessage.Status = false;
                deadletteredMessageRepository.Update(deadletteredMessage);

                await this.azureClientFactory.GetBlobStorageSaSClient(ContainerName.True, deadletteredMessage.BlobPath).DeleteBlobAsync().ConfigureAwait(true);
            }

            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            return true;
        }

        /// <inheritdoc/>
        public async Task<BlockchainFailures> GetFailuresAsync(BlockchainFailuresRequest failuresRequest)
        {
            ArgumentValidators.ThrowIfNull(failuresRequest, nameof(failuresRequest));

            var records = await this.reconciler.GetFailuresAsync(failuresRequest.IsCritical, failuresRequest.TakeRecords).ConfigureAwait(false);
            return new BlockchainFailures
            {
                Movements = records.Where(r => r.Name.EqualsIgnoreCase(nameof(Movement))).Select(r => r.RecordId),
                InventoryProducts = records.Where(r => r.Name.EqualsIgnoreCase(nameof(InventoryProduct))).Select(r => r.RecordId),
                Unbalances = records.Where(r => r.Name.EqualsIgnoreCase(nameof(Unbalance))).Select(r => r.RecordId),
                Ownerships = records.Where(r => r.Name.EqualsIgnoreCase(nameof(Ownership))).Select(r => r.RecordId),
                Nodes = records.Where(r => r.Name.EqualsIgnoreCase(nameof(OffchainNode))).Select(r => r.RecordId),
                Connections = records.Where(r => r.Name.EqualsIgnoreCase(nameof(OffchainNodeConnection))).Select(r => r.RecordId),
            };
        }

        /// <inheritdoc/>
        public Task ResetAsync(BlockchainFailures failures)
        {
            return this.reconciler.ResetAsync(failures);
        }

        /// <inheritdoc/>
        public async Task HandleRegistrationRetryFailureAsync(string transactionId)
        {
            var fileRegistrationTransactionRepository = this.unitOfWork.CreateRepository<FileRegistrationTransaction>();
            var fileRegistrationTransaction = await fileRegistrationTransactionRepository.SingleOrDefaultAsync(
                a => a.FileRegistrationTransactionId == Convert.ToInt32(transactionId, CultureInfo.InvariantCulture), "FileRegistration").ConfigureAwait(false);

            var pendingTransactionRepository = this.unitOfWork.CreateRepository<PendingTransaction>();

            var pendingTransaction = new PendingTransaction
            {
                BlobName = fileRegistrationTransaction.FileRegistration.BlobPath,
                MessageId = fileRegistrationTransaction.FileRegistration.UploadId,
                ErrorJson = JsonConvert.SerializeObject(new[] { Constants.RegistrationErrorMessage }),
            };

            pendingTransaction.Errors.Add(new PendingTransactionError
            {
                ErrorMessage = $"{Constants.RegistrationErrorMessage}",
                RecordId = fileRegistrationTransaction.RecordId,
            });

            pendingTransactionRepository.Insert(pendingTransaction);

            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task HandleRegistrationFailureAsync(string transactionId)
        {
            var fileRegistrationRepository = this.unitOfWork.CreateRepository<FileRegistration>();
            var fileRegistration = await fileRegistrationRepository.SingleOrDefaultAsync(x => x.UploadId == transactionId).ConfigureAwait(false);
            fileRegistration.IsParsed = false;
            fileRegistrationRepository.Update(fileRegistration);

            var pendingTransactionRepository = this.unitOfWork.CreateRepository<PendingTransaction>();
            var pendingTransaction = fileRegistration.ToPendingTransaction(new[] { Constants.RegistrationErrorMessage });
            pendingTransactionRepository.Insert(pendingTransaction);

            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task HandleSapFailureAsync(SapQueueMessage sapRequest)
        {
            ArgumentValidators.ThrowIfNull(sapRequest, nameof(sapRequest));
            if (sapRequest.RequestType == SapRequestType.Upload)
            {
                var fileRegistrationRepository = this.unitOfWork.CreateRepository<FileRegistration>();
                var fileRegistration = await fileRegistrationRepository.SingleOrDefaultAsync(x => x.UploadId == sapRequest.UploadId).ConfigureAwait(false);
                var sapTracking = new SapTracking
                {
                    FileRegistrationId = fileRegistration.FileRegistrationId,
                    StatusTypeId = StatusType.FAILED,
                    OperationalDate = DateTime.UtcNow.ToTrue(),
                    Comment = string.Empty,
                    ErrorMessage = $"Failed SAP request {sapRequest.UploadId} in Dead Lettering.",
                };
                var sapTrackingRepository = this.unitOfWork.CreateRepository<SapTracking>();
                sapTrackingRepository.Insert(sapTracking);
                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
            else
            {
                var sapTrackingRepository = this.unitOfWork.CreateRepository<SapTracking>();
                await UpdateFailedSapTrackingStatusAsync(sapRequest.MessageId, sapTrackingRepository).ConfigureAwait(false);
                if (sapRequest.PreviousMovementTransactionId.HasValue)
                {
                    await UpdateFailedSapTrackingStatusAsync(sapRequest.PreviousMovementTransactionId.Value, sapTrackingRepository).ConfigureAwait(false);
                }

                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public void HandleOffchainFailure(OffchainMessage message)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            var properties = new Dictionary<string, string>
            {
                { "TransactionHash", message.TransactionHash },
                { "BlockNumber", message.BlockNumber },
                { "EntityId", message.EntityId.ToString(CultureInfo.InvariantCulture) },
                { "Type", message.Type.ToString("G") },
            };
            this.telemetry.TrackEvent(Constants.Critical, EventName.OffchainSyncFailed.ToString("G"), properties);
        }

        /// <inheritdoc/>
        public async Task HandleReportFailureAsync(int executionId, ReportType type)
        {
            try
            {
                var repo = this.unitOfWork.CreateRepository<ReportExecution>();
                var entity = await repo.GetByIdAsync(executionId).ConfigureAwait(false);

                entity.StatusTypeId = StatusType.FAILED;
                repo.Update(entity);

                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
            catch (SqlException ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "ReportType", type.ToString("G") },
                    { "ExecutionId", executionId.ToString(CultureInfo.InvariantCulture) },
                    { "ErrorMessage", ex.Message },
                };
                this.telemetry.TrackEvent(Constants.Critical, EventName.ReportExecutionFailed.ToString("G"), properties);
            }
        }

        /// <summary>
        /// Updates the sap tracking status asynchronous.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <param name="repository">The sap tracking repository.</param>
        /// <returns>The Task.</returns>
        private static async Task UpdateFailedSapTrackingStatusAsync(int transactionId, IRepository<SapTracking> repository)
        {
            var existing = await repository.FirstOrDefaultAsync(x => x.MovementTransactionId == transactionId).ConfigureAwait(false);
            if (existing == null)
            {
                return;
            }

            existing.StatusTypeId = StatusType.FAILED;
            existing.OperationalDate = DateTime.UtcNow.ToTrue();
            existing.ErrorMessage = $"Failed SAP request {transactionId} in Dead Lettering.";
            repository.Update(existing);
        }

        /// <summary>
        /// Gets the content asynchronous.
        /// </summary>
        /// <param name="blobPath">The BLOB path.</param>
        /// <returns>The JToken.</returns>
        private async Task<JToken> GetContentAsync(string blobPath)
        {
            var stream = await this.azureClientFactory.GetBlobStorageSaSClient(ContainerName.True, blobPath).GetCloudBlobStreamAsync().ConfigureAwait(false);
            stream.Position = 0;
            return stream.DeserializeStream<JToken>();
        }
    }
}