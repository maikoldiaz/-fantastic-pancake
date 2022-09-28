// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadStatusContractTest.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Sap.Interfaces;
    using Ecp.True.Processors.Sap.Services;
    using Ecp.True.Processors.Sap.Services.FactoryUpload;
    using Ecp.True.Proxies.Sap.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class UploadStatusContractTest
    {
        /// <summary>
        /// The upload status contract.
        /// </summary>
        private UploadStatusContract uploadStatusContract;

        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private Mock<IUnitOfWork> unitOfWorkMock;

        /// <summary>
        /// The sap proxy.
        /// </summary>
        private Mock<ISapProxy> sapProxyMock;

        /// <summary>
        /// The sap tracking.
        /// </summary>
        private Mock<ISapTrackingProcessor> sapTrackingMock;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private Mock<ITelemetry> telemetryMock;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<SapStatusProcessor>> loggerMock;

        /// <summary>
        /// The sap repository.
        /// </summary>
        private Mock<IRepository<SapUploadContract>> sapRepository;

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
            this.telemetryMock = new Mock<ITelemetry>();
            this.loggerMock = new Mock<ITrueLogger<SapStatusProcessor>>();
            this.sapRepository = new Mock<IRepository<SapUploadContract>>();
            this.fileRegistrationTransactionService = new Mock<IFileRegistrationTransactionService>();
            this.uploadStatusContract = new UploadStatusContract(
                this.unitOfWorkMock.Object,
                this.sapProxyMock.Object,
                this.sapTrackingMock.Object,
                this.telemetryMock.Object,
                this.loggerMock.Object,
                new FileRegistration { UploadId = Guid.NewGuid().ToString() },
                this.fileRegistrationTransactionService.Object);
        }

        /// <summary>
        /// Try to send to sap asynchronous with the information from a status upload.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task UploadStatusContractTest_SendUploadProcessorSapAsync_ShouldSendToSapAsync()
        {
            this.unitOfWorkMock.Setup(x => x.CreateRepository<SapUploadContract>()).Returns(this.sapRepository.Object);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<SapUploadContract>().ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).Returns(this.GetSapContractResponseAsync());
            this.loggerMock.Setup(x => x.LogInformation(It.IsAny<string>(), It.IsAny<string>()));
            this.sapTrackingMock.Setup(x => x.CreateBlobAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(Guid.NewGuid().ToString);
            this.sapProxyMock.Setup(x => x.UpdateUploadStatusContractAsync(It.IsAny<SapUploadStatusContract>()));
            this.fileRegistrationTransactionService.Setup(x =>
                x.InsertFileRegistrationAsync(It.IsAny<FileRegistration>()));

            await this.uploadStatusContract.SendUploadProcessorSapAsync().ConfigureAwait(false);

            this.unitOfWorkMock.Verify(x => x.CreateRepository<SapUploadContract>(), Times.Once);
            this.sapTrackingMock.Verify(x => x.CreateBlobAsync(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
            this.sapProxyMock.Verify(x => x.UpdateUploadStatusContractAsync(It.IsAny<SapUploadStatusContract>()), Times.Once);
            this.sapTrackingMock.Verify(x => x.InsertSapTrackingAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            this.fileRegistrationTransactionService.Verify(x => x.InsertFileRegistrationAsync(It.IsAny<FileRegistration>()), Times.Once);
        }

        private async Task<IEnumerable<SapUploadContract>> GetSapContractResponseAsync()
        {
            return await Task.FromResult(new List<SapUploadContract>()
            {
                new SapUploadContract
                {
                    FileRegistrationId = 33,
                },
            }).ConfigureAwait(false);
        }
    }
}
