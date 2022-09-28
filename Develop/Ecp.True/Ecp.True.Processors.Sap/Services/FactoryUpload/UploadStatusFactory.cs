// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadStatusFactory.cs" company="Microsoft">
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
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Sap.Interfaces;
    using Ecp.True.Processors.Sap.Interfaces.IFactoryUpload;
    using Ecp.True.Proxies.Sap.Interfaces;

    /// <summary>
    /// Class UploadStatusFactory.
    /// </summary>
    public class UploadStatusFactory : IUploadStatusFactory
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
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<SapStatusProcessor> logger;

        /// <summary>
        /// The sap tracking.
        /// </summary>
        private readonly ISapTrackingProcessor sapTracking;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// The registration.
        /// </summary>
        private readonly FileRegistration registration;

        /// <summary>
        /// The file registration transaction service.
        /// </summary>
        private readonly IFileRegistrationTransactionService fileRegistrationTransactionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadStatusFactory"/> class.
        /// Constructor UploadStatusInventory.
        /// </summary>
        /// <param name="unitOfWork">unitOfWork.</param>
        /// <param name="sapProxy">sapProxy.</param>
        /// <param name="sapTracking">sapTracking.</param>
        /// <param name="logger">logger.</param>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="registration">registration.</param>
        /// <param name="fileRegistrationTransactionService">The file registration transaction service.</param>
        public UploadStatusFactory(
            IUnitOfWork unitOfWork,
            ISapProxy sapProxy,
            ISapTrackingProcessor sapTracking,
            ITrueLogger<SapStatusProcessor> logger,
            ITelemetry telemetry,
            FileRegistration registration,
            IFileRegistrationTransactionService fileRegistrationTransactionService)
        {
            this.unitOfWork = unitOfWork;
            this.sapProxy = sapProxy;
            this.sapTracking = sapTracking;
            this.logger = logger;
            this.telemetry = telemetry;
            this.registration = registration;
            this.fileRegistrationTransactionService = fileRegistrationTransactionService;
        }

        /// <summary>
        /// Method GetUploadStatus.
        /// </summary>
        /// <returns>IUploadStatus.</returns>
        public IUploadStatus UploadStatus()
        {
            switch (this.registration.SourceTypeId)
            {
                case SystemType.PURCHASE:
                case SystemType.SELL:
                    return new UploadStatusContract(
                     this.unitOfWork,
                     this.sapProxy,
                     this.sapTracking,
                     this.telemetry,
                     this.logger,
                     this.registration,
                     this.fileRegistrationTransactionService);
                default:
                    return new UploadStatusMovementOrInventory(
                     this.unitOfWork,
                     this.sapProxy,
                     this.sapTracking,
                     this.telemetry,
                     this.logger,
                     this.registration,
                     this.fileRegistrationTransactionService);
            }
        }
    }
}
