// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SendLogisticMovement.cs" company="Microsoft">
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
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Sap.Interfaces;
    using Ecp.True.Proxies.Sap.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Class SendLogisticMovement.
    /// </summary>
    public class SendLogisticMovement
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
        private readonly ITrueLogger<SapProcessor> logger;

        /// <summary>
        /// The file registration transaction service.
        /// </summary>
        private readonly IFileRegistrationTransactionService fileRegistrationTransactionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendLogisticMovement"/> class.
        /// Constructor UploadStatusInventory.
        /// </summary>
        /// <param name="unitOfWork">unitOfWork.</param>
        /// <param name="sapProxy">sapProxy.</param>
        /// <param name="sapTracking">sapTracking.</param>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="logger">logger.</param>
        /// <param name="fileRegistrationTransactionService">The file registration transaction service.</param>
        public SendLogisticMovement(
            IUnitOfWork unitOfWork,
            ISapProxy sapProxy,
            ISapTrackingProcessor sapTracking,
            ITelemetry telemetry,
            ITrueLogger<SapProcessor> logger,
            IFileRegistrationTransactionService fileRegistrationTransactionService)
        {
            this.unitOfWork = unitOfWork;
            this.sapProxy = sapProxy;
            this.sapTracking = sapTracking;
            this.telemetry = telemetry;
            this.logger = logger;
            this.fileRegistrationTransactionService = fileRegistrationTransactionService;
        }

        /// <summary>
        ///  Send Data to SAP.
        /// </summary>
        /// <param name="sapLogisticRequest">The sapLogisticRequest.</param>
        /// <param name="logisticMovement">The logisticMovement.</param>
        /// <returns>Process.</returns>
        public async Task<string> SendUploadProcessorSapAsync(SapLogisticRequest sapLogisticRequest, LogisticMovement logisticMovement)
        {
            ArgumentValidators.ThrowIfNull(sapLogisticRequest, nameof(sapLogisticRequest));

            ArgumentValidators.ThrowIfNull(logisticMovement, nameof(logisticMovement));

            string json = JsonConvert.SerializeObject(sapLogisticRequest, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(),
                Formatting = Formatting.Indented,
            });

            var result = false;

            // step 1 - create and Upload the json to blob
            var responseUploadId = Guid.NewGuid().ToString();
            var blobName = await this.sapTracking.CreateBlobAsync(responseUploadId, JsonConvert.DeserializeObject(json)).ConfigureAwait(false);

            try
            {
                // step 2 - send the request to SAP PO
                result = await this.sapProxy.SendLogisticMovementAsync(JsonConvert.DeserializeObject(json)).ConfigureAwait(false);

                // step 3 - Save response File Registration
                await this.SaveResponseFileRegistrationAsync(responseUploadId, blobName).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error occurred in process logistic movement for upload Id: {logisticMovement.LogisticMovementId}. Error: {ex.Message}");
                this.telemetry.TrackEvent(Constants.Critical, EventName.SapUploadLogisticMovementFailureEvent.ToString("G"));
            }

            // step 4 - assign technical error in case of any response other than 200
            if (!result)
            {
                var errorMessage = Constants.SapTechnicalErrorSendLogisticMovement;
                this.logger.LogError(new Exception(errorMessage), errorMessage);
                await this.UpdateLogisticMovementStatusAsync(logisticMovement, StatusType.FAILED, errorMessage).ConfigureAwait(false);
                await this.UpdateTicketStatusAsync(logisticMovement.TicketId, StatusType.FAILED).ConfigureAwait(false);
            }
            else
            {
                await this.UpdateLogisticMovementStatusAsync(logisticMovement, StatusType.SENT, string.Empty).ConfigureAwait(false);
            }

            return blobName;
        }

        /// <summary>
        /// Update logistic movement status after sent to SAP.
        /// </summary>
        /// <param name="logisticMovement">The logisticMovement.</param>
        /// <param name="status">The new status.</param>
        /// <param name="errorMessage">The error message if failed.</param>
        /// <returns>The Task.</returns>
        public async Task UpdateLogisticMovementStatusAsync(LogisticMovement logisticMovement, StatusType status, string errorMessage)
        {
            ArgumentValidators.ThrowIfNull(logisticMovement, nameof(logisticMovement));

            var repository = this.unitOfWork.CreateRepository<LogisticMovement>();

            logisticMovement.StatusProcessId = status;
            logisticMovement.MessageProcess = errorMessage;
            logisticMovement.SapSentDate = DateTime.Now;
            repository.Update(logisticMovement);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Update logistic movement status after sent to SAP.
        /// </summary>
        /// <param name="ticketId">The Ticket Id.</param>
        /// <param name="status">The new status.</param>
        /// <returns>The Task.</returns>
        public async Task UpdateTicketStatusAsync(int ticketId, StatusType status)
        {
            var repository = this.unitOfWork.CreateRepository<Ticket>();
            var ticket = await repository.GetByIdAsync(ticketId).ConfigureAwait(false);
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));

            ticket.Status = status;
            ticket.LastModifiedDate = DateTime.Now;
            repository.Update(ticket);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Saves the response file registration asynchronous.
        /// The response to be sent to SAP is saved.
        /// </summary>
        /// <param name="processId">The process identifier.</param>
        /// <param name="blobName">Name of the BLOB.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private Task SaveResponseFileRegistrationAsync(string processId, string blobName)
        {
            var fileRegistrationResponse = new FileRegistration
            {
                UploadId = processId,
                MessageDate = DateTime.Now,
                Name = $"{processId}",
                ActionType = FileRegistrationActionType.Insert,
                FileUploadStatus = FileUploadStatus.FINALIZED,
                SystemTypeId = SystemType.SAP,
                SegmentId = null,
                BlobPath = blobName,
                HomologationInventoryBlobPath = null,
                HomologationMovementBlobPath = null,
                PreviousUploadId = null,
                IsParsed = false,
                SourceSystem = SystemType.SAP.ToString(),
                SourceTypeId = SystemType.LOGISTIC,
                IntegrationType = IntegrationType.REQUEST,
            };

            return this.fileRegistrationTransactionService.InsertFileRegistrationAsync(fileRegistrationResponse);
        }
    }
}
