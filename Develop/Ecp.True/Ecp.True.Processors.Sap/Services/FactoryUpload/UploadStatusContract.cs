// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadStatusContract.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Sap.Services.FactoryUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Sap.Interfaces;
    using Ecp.True.Processors.Sap.Interfaces.IFactoryUpload;
    using Ecp.True.Proxies.Sap.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Class UploadStatusContract.
    /// </summary>
    public class UploadStatusContract : IUploadStatus
    {
        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The sap proxy.
        /// </summary>
        private readonly ISapProxy sapProxy;

        /// <summary>
        /// The sap tracking.
        /// </summary>
        private readonly ISapTrackingProcessor sapTracking;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<SapStatusProcessor> logger;

        /// <summary>
        /// The registration.
        /// </summary>
        private readonly FileRegistration registration;

        /// <summary>
        /// The file registration transaction service.
        /// </summary>
        private readonly IFileRegistrationTransactionService fileRegistrationTransactionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadStatusContract"/> class.
        /// Constructor UploadStatusInventory.
        /// </summary>
        /// <param name="unitOfWork">unitOfWork.</param>
        /// <param name="sapProxy">sapProxy.</param>
        /// <param name="sapTracking">sapTracking.</param>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="logger">logger.</param>
        /// <param name="registration">registration.</param>
        /// <param name="fileRegistrationTransactionService">The file registration transaction service.</param>
        public UploadStatusContract(
            IUnitOfWork unitOfWork,
            ISapProxy sapProxy,
            ISapTrackingProcessor sapTracking,
            ITelemetry telemetry,
            ITrueLogger<SapStatusProcessor> logger,
            FileRegistration registration,
            IFileRegistrationTransactionService fileRegistrationTransactionService)
        {
            this.unitOfWork = unitOfWork;
            this.sapProxy = sapProxy;
            this.sapTracking = sapTracking;
            this.telemetry = telemetry;
            this.logger = logger;
            this.registration = registration;
            this.fileRegistrationTransactionService = fileRegistrationTransactionService;
        }

        /// <summary>
        /// Method SendUploadProcessorSapAsync.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task SendUploadProcessorSapAsync()
        {
            bool result = false;
            string errorMessage = string.Empty;
            string blobName = string.Empty;
            var eventName = this.registration.SourceTypeId == SystemType.SELL
                    ? EventName.SapUploadSellFailureEvent
                    : EventName.SapUploadPurchaseFailureEvent;

            // step 1 - create and Upload the json to blob
            var payloads = await this.BuildSapPayloadAsync().ConfigureAwait(false);
            var payload = payloads.OrderBy(x => x.StatusMessage).First();
            try
            {
                var requestPayload = new SapUploadStatusContract(payload, this.registration.SourceSystem);
                var requestUploadId = this.registration.UploadId;

                // step 2 - create and Upload the json to blob
                var responseUploadId = Guid.NewGuid().ToString();
                string json = JsonConvert.SerializeObject(new SapUploadStatusContractOutput { SapUploadStatusContract = requestPayload }, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver(),
                    Formatting = Formatting.Indented,
                });

                blobName = await this.sapTracking.CreateBlobAsync(requestUploadId, JsonConvert.DeserializeObject(json)).ConfigureAwait(false);

                // step 3 - Save response File Registration
                await this.SaveResponseFileRegistrationAsync(requestUploadId, responseUploadId, blobName).ConfigureAwait(false);

                // step 4 - send the request to SAP PO
                result = await this.sapProxy.UpdateUploadStatusContractAsync(requestPayload).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                this.logger.LogError(ex, $"Error occurred in Update Upload status processing for upload Id: {this.registration.UploadId}. Error: {ex.Message}");
            }

            await this.sapTracking.InsertSapTrackingAsync(payload.FileRegistrationId, result, errorMessage, blobName).ConfigureAwait(false);

            // step 4 - assign technical error in case of any response other than 200
            if (!result)
            {
                errorMessage = Constants.SapTechnicalErrorContract;
                this.logger.LogError(new Exception(errorMessage), errorMessage);
                this.telemetry.TrackEvent(Constants.Critical, eventName.ToString("G"));
            }
        }

        /// <summary>
        /// Saves the response file registration asynchronous.
        /// The response to be sent to SAP is saved.
        /// </summary>
        /// <param name="processId">The process identifier.</param>
        /// <param name="blobName">Name of the BLOB.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private Task SaveResponseFileRegistrationAsync(string requestUploadId, string processId, string blobName)
        {
            var fileRegistrationResponse = new FileRegistration
            {
                UploadId = processId,
                MessageDate = DateTime.Now,
                Name = $"{requestUploadId}",
                ActionType = this.registration.ActionType,
                FileUploadStatus = this.registration.FileUploadStatus,
                SystemTypeId = this.registration.SystemTypeId,
                SegmentId = this.registration.SegmentId,
                BlobPath = blobName,
                HomologationInventoryBlobPath = this.registration.HomologationInventoryBlobPath,
                HomologationMovementBlobPath = this.registration.HomologationMovementBlobPath,
                PreviousUploadId = Guid.Parse(this.registration.UploadId),
                IsParsed = this.registration.IsParsed,
                SourceSystem = this.registration.SourceSystem,
                SourceTypeId = this.registration.SourceTypeId,
                IntegrationType = IntegrationType.RESPONSE,
            };

            return this.fileRegistrationTransactionService.InsertFileRegistrationAsync(fileRegistrationResponse);
        }

        /// <summary>
        /// Builds the sap payload asynchronous.
        /// </summary>
        /// <returns>The Sap Upload Status.</returns>
        private async Task<IEnumerable<SapUploadContract>> BuildSapPayloadAsync()
        {
            var parameters = new Dictionary<string, object>
            {
                { "@UploadId", this.registration.UploadId },
            };
            return await this.unitOfWork.CreateRepository<SapUploadContract>().ExecuteQueryAsync(Repositories.Constants.GetUploadContractDetailsSap, parameters).ConfigureAwait(false);
        }
    }
}
