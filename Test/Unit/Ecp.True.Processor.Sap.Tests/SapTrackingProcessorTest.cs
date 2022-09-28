// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapTrackingProcessorTest.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Processor.Sap.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Sap.Services;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class SapTrackingProcessorTest
    {
        /// <summary>
        /// The unit of work mock.
        /// </summary>
        private Mock<IUnitOfWorkFactory> unitOfWorkFactoryMock;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<IUnitOfWork> unitOfWorkMock;

        /// <summary>
        /// The system repository mock.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureClientFactory;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<SapStatusProcessor>> mockLogger;

        /// <summary>
        /// The BLOB client mock.
        /// </summary>
        private Mock<IBlobStorageSasClient> blobClientMock;

        /// <summary>
        /// The sap tracking repository.
        /// </summary>
        private Mock<IRepository<SapTracking>> sapTrackingRepository;

        /// <summary>
        /// The Sap tracking processor.
        /// </summary>
        private SapTrackingProcessor sapTrackingProcessor;

        [TestInitialize]
        public void Initilize()
        {
            this.unitOfWorkMock = new Mock<IUnitOfWork>();
            this.mockAzureClientFactory = new Mock<IAzureClientFactory>();
            this.mockLogger = new Mock<ITrueLogger<SapStatusProcessor>>();
            this.blobClientMock = new Mock<IBlobStorageSasClient>();
            this.unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
            this.unitOfWorkFactoryMock.Setup(x => x.GetUnitOfWork()).Returns(this.unitOfWorkMock.Object);
            this.sapTrackingRepository = new Mock<IRepository<SapTracking>>();
            this.sapTrackingProcessor = new SapTrackingProcessor(
                this.unitOfWorkFactoryMock.Object,
                this.mockAzureClientFactory.Object,
                this.mockLogger.Object);
        }

        /// <summary>
        /// Try to create an asynchronous Blog with the information from a contract.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task SapTrackingProcessorTest_CreateBlobAsync_ShuoldCreateBlobAsync()
        {
            var sapUploadContract = new SapUploadContract()
            {
                CreatedBy = "testShouldCreateBlob",
                CreatedDate = DateTime.Now,
                FileRegistrationId = 1,
                Information = "Test",
                LastModifiedBy = "testShouldCreateBlob",
                LastModifiedDate = DateTime.Now,
                OrderId = 1,
                ProcessingTime = "ProcessingTime",
                SourceTypeId = "purchases",
                StatusMessage = "some",
                TransactionId = 123,
            };

            var payload = new SapUploadStatusContract(sapUploadContract, "test");
            this.mockAzureClientFactory.Setup(x => x.GetBlobStorageSaSClient(It.IsAny<string>(), It.IsAny<string>())).Returns(this.blobClientMock.Object);
            this.mockLogger.Setup(x => x.LogInformation(It.IsAny<string>(), It.IsAny<string>()));

            var result = await this.sapTrackingProcessor.CreateBlobAsync($"{sapUploadContract.FileRegistrationId}-{1}", payload).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockAzureClientFactory.Verify(x => x.GetBlobStorageSaSClient(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            this.mockLogger.Verify(x => x.LogInformation(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Try to insert an asynchronous sap tracking.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task SapTrackingProcessorTest_InsertSapTrackingAsync_InsertSapTrackingAsync()
        {
            this.mockAzureClientFactory.Setup(x => x.GetBlobStorageSaSClient(It.IsAny<string>(), It.IsAny<string>())).Returns(this.blobClientMock.Object);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<SapTracking>()).Returns(this.sapTrackingRepository.Object);
            this.sapTrackingRepository.Setup(x => x.Insert(It.IsAny<SapTracking>()));

            await this.sapTrackingProcessor.InsertSapTrackingAsync(245983, true, string.Empty, string.Empty).ConfigureAwait(false);

            this.unitOfWorkMock.Verify(x => x.CreateRepository<SapTracking>(), Times.Once);
            this.unitOfWorkMock.Verify(x => x.SaveAsync(CancellationToken.None), Times.Once);
        }
    }
}
