// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapStatusProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Sap.Services
{
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Sap.Interfaces;
    using Ecp.True.Processors.Sap.Services.FactoryUpload;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Sap.Interfaces;

    /// <summary>
    /// The Sap Status Processor.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Sap.Interfaces.ISapStatusProcessor" />
    public class SapStatusProcessor : ISapStatusProcessor
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<SapStatusProcessor> logger;

        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The sap proxy.
        /// </summary>
        private readonly ISapProxy sapProxy;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The sap tracking.
        /// </summary>
        private readonly ISapTrackingProcessor sapTracking;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// The file registration transaction service.
        /// </summary>
        private readonly IFileRegistrationTransactionService fileRegistrationTransactionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SapStatusProcessor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="sapProxy">The sap proxy.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="sapTracking">The sapTracking.</param>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="fileRegistrationTransactionService">The file registration transaction service.</param>
        public SapStatusProcessor(
            ITrueLogger<SapStatusProcessor> logger,
            IUnitOfWorkFactory unitOfWorkFactory,
            ISapProxy sapProxy,
            IAzureClientFactory azureClientFactory,
            ISapTrackingProcessor sapTracking,
            ITelemetry telemetry,
            IFileRegistrationTransactionService fileRegistrationTransactionService)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            this.logger = logger;
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.sapProxy = sapProxy;
            this.azureClientFactory = azureClientFactory;
            this.sapTracking = sapTracking;
            this.telemetry = telemetry;
            this.fileRegistrationTransactionService = fileRegistrationTransactionService;
        }

        /// <summary>
        /// Tries the upload status asynchronous.
        /// </summary>
        /// <param name="uploadId">The upload identifier.</param>
        /// <returns>The Task.</returns>
        public async Task TryUploadStatusAsync(string uploadId)
        {
            this.logger.LogInformation($"Update Upload status is requested for upload Id {uploadId}", $"{uploadId}");
            var existProcessing = await this.ExistFileRegistrationTransactionProcessingAsync(uploadId).ConfigureAwait(false);
            if (existProcessing)
            {
                // send message to queue again with same schedule
                var sapRequest = new SapQueueMessage(SapRequestType.Upload, uploadId);
                var client = this.azureClientFactory.GetQueueClient(QueueConstants.SapQueue);
                await client.QueueScheduleMessageAsync(sapRequest, Constants.SapUploadStatus, Constants.SapQueueIntervalInSecs).ConfigureAwait(false);
                return;
            }

            var fileRegistration = await this.GetFileRegistrationAsync(uploadId).ConfigureAwait(false);

            await this.DoUpdateStatusAsync(fileRegistration).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the file registration asynchronous.
        /// </summary>
        /// <param name="uploadId">The upload identifier.</param>
        /// <returns>The file registration.</returns>
        private async Task<FileRegistration> GetFileRegistrationAsync(string uploadId)
        {
            var fileRegistrationRepository = this.unitOfWork.CreateRepository<FileRegistration>();
            return await fileRegistrationRepository.FirstOrDefaultAsync(x => x.UploadId == uploadId).ConfigureAwait(false);
        }

        /// <summary>
        /// Exist File Registration Transaction Processing asynchronous.
        /// </summary>
        /// <param name="uploadId">The upload identifier.</param>
        /// <returns> The Boolean. </returns>
        private async Task<bool> ExistFileRegistrationTransactionProcessingAsync(string uploadId)
        {
            var fileRegistrationTransactionRepository = this.unitOfWork.CreateRepository<FileRegistrationTransaction>();
            var processingRecords = await fileRegistrationTransactionRepository.GetCountAsync(x => x.UploadId == uploadId && x.StatusTypeId == StatusType.PROCESSING).ConfigureAwait(false);
            return processingRecords > 0;
        }

        /// <summary>
        /// Does the update status asynchronous.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns>The Task.</returns>
        private async Task DoUpdateStatusAsync(FileRegistration registration)
        {
            var uploadStatusFactory = new UploadStatusFactory(
                this.unitOfWork,
                this.sapProxy,
                this.sapTracking,
                this.logger,
                this.telemetry,
                registration,
                this.fileRegistrationTransactionService);

            var uploadStatus = uploadStatusFactory.UploadStatus();
            await uploadStatus.SendUploadProcessorSapAsync().ConfigureAwait(false);
        }
    }
}
