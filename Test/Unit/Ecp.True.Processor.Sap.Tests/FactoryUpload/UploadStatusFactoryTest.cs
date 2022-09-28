// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadStatusFactoryTest.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Sap.Tests.FactoryUpload
{
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Sap.Interfaces;
    using Ecp.True.Processors.Sap.Interfaces.IFactoryUpload;
    using Ecp.True.Processors.Sap.Services;
    using Ecp.True.Processors.Sap.Services.FactoryUpload;
    using Ecp.True.Proxies.Sap.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class UploadStatusFactoryTest
    {
        /// <summary>
        /// The upload status factory.
        /// </summary>
        private UploadStatusFactory uploadStatusFactory;

        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private Mock<IUnitOfWork> unitOfWorkMock;

        /// <summary>
        /// The sap proxy.
        /// </summary>
        private Mock<ISapProxy> sapProxyMock;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private Mock<ITelemetry> telemetryMock;

        /// <summary>
        /// The sap tracking.
        /// </summary>
        private Mock<ISapTrackingProcessor> sapTrackingMock;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<SapStatusProcessor>> loggerMock;

        /// <summary>
        /// The file registration transaction service.
        /// </summary>
        private Mock<IFileRegistrationTransactionService> fileRegistrationTransactionService;

        [TestInitialize]
        public void Initialize()
        {
            this.unitOfWorkMock = new Mock<IUnitOfWork>();
            this.sapProxyMock = new Mock<ISapProxy>();
            this.sapTrackingMock = new Mock<ISapTrackingProcessor>();
            this.loggerMock = new Mock<ITrueLogger<SapStatusProcessor>>();
            this.telemetryMock = new Mock<ITelemetry>();
            this.fileRegistrationTransactionService = new Mock<IFileRegistrationTransactionService>();
        }

        /// <summary>
        /// Try to create an upload status contract.
        /// </summary>
        [TestMethod]
        public void UploadStatusFactoryTest_GetUploadStatus_Sell_ShouldCreateAContract()
        {
            this.uploadStatusFactory = this.GetObject(SystemType.SELL);

            IUploadStatus result = this.uploadStatusFactory.UploadStatus();

            Assert.AreEqual(result.GetType(), typeof(UploadStatusContract));
        }

        /// <summary>
        /// Try to create an upload status contract.
        /// </summary>
        [TestMethod]
        public void UploadStatusFactoryTest_GetUploadStatus_Purchase_ShouldCreateAContract()
        {
            this.uploadStatusFactory = this.GetObject(SystemType.PURCHASE);

            IUploadStatus result = this.uploadStatusFactory.UploadStatus();

            Assert.AreEqual(result.GetType(), typeof(UploadStatusContract));
        }

        /// <summary>
        /// Try to create an upload status inventory.
        /// </summary>
        [TestMethod]
        public void UploadStatusFactoryTest_GetUploadStatus_Inventory_ShouldCreateAInventory()
        {
            this.uploadStatusFactory = this.GetObject(SystemType.INVENTORY);

            IUploadStatus result = this.uploadStatusFactory.UploadStatus();

            Assert.AreEqual(result.GetType(), typeof(UploadStatusMovementOrInventory));
        }

        /// <summary>
        /// Try to create an upload status movement.
        /// </summary>
        [TestMethod]
        public void UploadStatusFactoryTest_GetUploadStatus_Movement_ShouldCreateAInventory()
        {
            this.uploadStatusFactory = this.GetObject(SystemType.MOVEMENTS);

            IUploadStatus result = this.uploadStatusFactory.UploadStatus();

            Assert.AreEqual(result.GetType(), typeof(UploadStatusMovementOrInventory));
        }

        private UploadStatusFactory GetObject(SystemType sourceTypeId)
        {
            return new UploadStatusFactory(
            this.unitOfWorkMock.Object,
            this.sapProxyMock.Object,
            this.sapTrackingMock.Object,
            this.loggerMock.Object,
            this.telemetryMock.Object,
            new FileRegistration
            {
                SourceTypeId = sourceTypeId,
            },
            this.fileRegistrationTransactionService.Object);
        }
    }
}
