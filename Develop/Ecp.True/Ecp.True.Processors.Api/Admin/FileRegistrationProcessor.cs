// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileRegistrationProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Azure.Storage.Sas;

    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Core;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// The register file processor.
    /// </summary>
    public class FileRegistrationProcessor : ProcessorBase, IFileRegistrationProcessor
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<FileRegistrationProcessor> logger;

        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        /// <summary>
        /// The azure Client Factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileRegistrationProcessor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        public FileRegistrationProcessor(
            ITrueLogger<FileRegistrationProcessor> logger,
            IRepositoryFactory factory,
            IUnitOfWorkFactory unitOfWorkFactory,
            IAzureClientFactory azureClientFactory,
            IConfigurationHandler configurationHandler)
            : base(factory)
        {
            this.logger = logger;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.azureClientFactory = azureClientFactory;
            this.configurationHandler = configurationHandler;
        }

        /// <summary>
        /// Gets the register files.
        /// </summary>
        /// <param name="fileUploadIds">The file upload ids.</param>
        /// <returns>
        /// Returns the registered files.
        /// </returns>
        public Task<IEnumerable<FileRegistration>> GetFileRegistrationStatusAsync(IEnumerable<Guid> fileUploadIds)
        {
            var repository = this.CreateRepository<FileRegistration>();
            var uploadIds = fileUploadIds.Select(f => f.ToString("D", CultureInfo.InvariantCulture));
            return repository.GetAllAsync(x => uploadIds.Contains(x.UploadId));
        }

        /// <summary>
        /// Registers the file information.
        /// </summary>
        /// <param name="fileRegistration">The register file.</param>
        /// <returns>
        /// Return the task.
        /// </returns>
        public async Task RegisterAsync(FileRegistration fileRegistration)
        {
            ArgumentValidators.ThrowIfNull(fileRegistration, nameof(fileRegistration));

            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var repository = unitOfWork.CreateRepository<FileRegistration>();

                fileRegistration.FileUploadStatus = FileUploadStatus.PROCESSING;
                fileRegistration.MessageDate = DateTime.UtcNow.ToTrue();
                fileRegistration.SourceTypeId = HomologateSourceType(fileRegistration.SystemTypeId);

                repository.Insert(fileRegistration);

                await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
                await this.PushMessageToHomologationQueueAsync(fileRegistration).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task<FileAccessInfo> GetFileRegistrationAccessInfoAsync(string blobFileName, SystemType systemType)
        {
            var info = await this.azureClientFactory.GetFileAccessInfoAsync(
                   ContainerName.True,
                   GetBlobFullName(blobFileName, systemType),
                   30,
                   BlobSasPermissions.Write).ConfigureAwait(false);

            info.BlobName = blobFileName;
            return info;
        }

        /// <inheritdoc/>
        public Task<FileAccessInfo> GetFileRegistrationAccessInfoAsync()
        {
            return this.azureClientFactory.GetFileAccessInfoAsync(ContainerName.True, null, 30, BlobSasPermissions.Read);
        }

        /// <inheritdoc/>
        public Task<FileAccessInfo> GetFileRegistrationAccessInfoByContainerAsync(string containerName)
        {
            return this.azureClientFactory.GetFileAccessInfoAsync(containerName, null, 30, BlobSasPermissions.Read);
        }

        /// <summary>
        /// Updates the file registration transaction asynchronous.
        /// </summary>
        /// <param name="fileRegistrationTransactionIds">The file registration transaction ids.</param>
        /// <returns>Returns a task.</returns>
        public async Task RetryAsync(int[] fileRegistrationTransactionIds)
        {
            ArgumentValidators.ThrowIfNull(fileRegistrationTransactionIds, nameof(fileRegistrationTransactionIds));

            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var pendingTransactionErrorrepository = unitOfWork.CreateRepository<PendingTransactionError>();
                var repository = this.CreateRepository<FileRegistrationTransaction>();

                foreach (var fileRegistrationTransactionId in fileRegistrationTransactionIds)
                {
                    var fileRegistrationTransaction = await repository.GetByIdAsync(fileRegistrationTransactionId).ConfigureAwait(false);

                    var pendingTransactionErrors = await pendingTransactionErrorrepository.GetAllAsync(
                    e => e.RecordId == fileRegistrationTransaction.RecordId).ConfigureAwait(false);

                    foreach (var pendingTransactionError in pendingTransactionErrors)
                    {
                        pendingTransactionError.IsRetrying = true;
                        pendingTransactionError.Comment = Constants.Retried;
                        pendingTransactionErrorrepository.Update(pendingTransactionError);
                    }
                }

                await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }

            var tasks = new List<Task>();

            fileRegistrationTransactionIds.ForEach(e => tasks.Add(this.PushMessageToMessageRetryQueueAsync(e)));
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the full name of the BLOB.
        /// </summary>
        /// <param name="blobName">Name of the BLOB.</param>
        /// <param name="systemType">Specifies the file type.</param>
        /// <returns>The full name of blob file.</returns>
        private static string GetBlobFullName(string blobName, SystemType systemType)
        {
            return $"{systemType.ToString().ToLowerCase()}/{blobName}";
        }

        /// <summary>
        /// Homologate SystemType Excel when SystemType is CONTRACT OR EVENT.
        /// </summary>
        /// <param name="systemTypeId">system type from register file. </param>
        /// <returns>Homologate SystemType.</returns>
        private static SystemType HomologateSourceType(SystemType systemTypeId) => systemTypeId == SystemType.CONTRACT ? SystemType.EXCEL : systemTypeId;

        /// <summary>
        /// Pushes the message to message retry queue asynchronous.
        /// </summary>
        /// <param name="fileRegistrationTransactionId">The file registration transaction identifier.</param>
        /// <returns>Returns the Task.</returns>
        private Task PushMessageToMessageRetryQueueAsync(int fileRegistrationTransactionId)
        {
            // updated pending transaction error
            var client = this.azureClientFactory.GetQueueClient(QueueConstants.RetryJsonQueue);
            var message = new JsonQueueMessage { FileRegistrationTransactionId = fileRegistrationTransactionId };
            return client.QueueMessageAsync(message);
        }

        /// <summary>
        /// Pushes the message to homologation queue.
        /// </summary>
        /// <param name="fileRegistration">The file registration.</param>
        private async Task PushMessageToHomologationQueueAsync(FileRegistration fileRegistration)
        {
            try
            {
                ArgumentValidators.ThrowIfNull(fileRegistration, nameof(fileRegistration));

                var config = await this.configurationHandler.GetConfigurationAsync<ServiceBusSettings>(ConfigurationConstants.ServiceBusSettings).ConfigureAwait(false);

                string queueName;
                switch (fileRegistration.SystemTypeId)
                {
                    case SystemType.EVENTS:
                        queueName = QueueConstants.ExcelEventQueue;
                        break;
                    case SystemType.CONTRACT:
                        queueName = QueueConstants.ExcelContractQueue;
                        break;
                    default:
                        queueName = config.ExcelQueueName;
                        break;
                }

                var client = this.azureClientFactory.GetQueueClient(queueName);

                await client.QueueMessageAsync(fileRegistration).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                await this.HandleFileRegistrationFailureAsync(fileRegistration).ConfigureAwait(false);
            }
        }

        private async Task HandleFileRegistrationFailureAsync(FileRegistration fileRegistration)
        {
            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var fileRegistrationRepository = unitOfWork.CreateRepository<FileRegistration>();
                var fileRegistrationRecord = await fileRegistrationRepository.GetByIdAsync(fileRegistration.FileRegistrationId).ConfigureAwait(false);
                fileRegistrationRecord.IsParsed = false;
                fileRegistrationRepository.Update(fileRegistrationRecord);

                var pendingTransactionRepository = unitOfWork.CreateRepository<PendingTransaction>();
                var pendingTransaction = fileRegistrationRecord.ToPendingTransaction(new[] { Constants.TechnicalExceptionErrorMessage });
                pendingTransactionRepository.Insert(pendingTransaction);
                await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }
    }
}
