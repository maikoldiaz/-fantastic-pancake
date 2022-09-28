// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapStatusProcessorTests.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Sap.Interfaces;
    using Ecp.True.Processors.Sap.Services;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Sap.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The Sap Status Processor Tests.
    /// </summary>
    [TestClass]
    public class SapStatusProcessorTests
    {
        /// <summary>
        /// The mockblob client.
        /// </summary>
        private Mock<IBlobStorageSasClient> mockBlobSasClient;

        /// <summary>
        /// The unit of work mock.
        /// </summary>
        private Mock<IUnitOfWorkFactory> unitOfWorkFactoryMock;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<IUnitOfWork> unitOfWorkMock;

        /// <summary>
        /// The mock sap proxy.
        /// </summary>
        private Mock<ISapProxy> mockSapProxy;

        /// <summary>
        /// The mock configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigurationHandler;

        /// <summary>
        /// The system repository mock.
        /// </summary>
        private Mock<IMovementRepository> mockMovementRepository;

        /// <summary>
        /// The system repository mock.
        /// </summary>
        private Mock<IRepository<SapTracking>> mockSapTrackingRepository;

        /// <summary>
        /// The mock file registration repository.
        /// </summary>
        private Mock<IRepository<FileRegistration>> mockFileRegistrationRepository;

        /// <summary>
        /// The mock sap upload repository.
        /// </summary>
        private Mock<IRepository<SapUpload>> mockSapUploadRepository;

        /// <summary>
        /// The sap tracking repository.
        /// </summary>
        private Mock<IRepository<SapTracking>> sapTrackingRepository;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<SapStatusProcessor>> mockLogger;

        /// <summary>
        /// The sap repository.
        /// </summary>
        private Mock<IRepository<SapMapping>> sapRepository;

        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>
        /// The system repository mock.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureClientFactory;

        /// <summary>
        /// The mock service bus queue client.
        /// </summary>
        private Mock<IServiceBusQueueClient> mockServiceBusQueueClient;

        /// <summary>
        /// The mock service bus queue client.
        /// </summary>
        private Mock<IRepository<FileRegistrationTransaction>> mockFileRegistrationTransactionRepository;

        /// <summary>
        /// The mock service bus queue client.
        /// </summary>
        private Mock<IRepository<SapUploadContract>> mockSapUploadContractRepository;

        /// <summary>
        /// The sap status processor.
        /// </summary>
        private SapStatusProcessor sapStatusProcessor;

        /// <summary>
        /// The BLOB client mock.
        /// </summary>
        private Mock<IBlobStorageClient> blobClientMock;

        /// <summary>
        /// The sap trakingMock mock.
        /// </summary>
        private Mock<ISapTrackingProcessor> sapTrakingMock;

        /// <summary>
        /// The telemetry mock.
        /// </summary>
        private Mock<ITelemetry> telemetryMock;

        /// <summary>
        /// The file registration transaction service.
        /// </summary>
        private Mock<IFileRegistrationTransactionService> fileRegistrationTransactionService;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.unitOfWorkMock = new Mock<IUnitOfWork>();
            this.unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
            this.unitOfWorkFactoryMock.Setup(x => x.GetUnitOfWork()).Returns(this.unitOfWorkMock.Object);
            this.mockSapProxy = new Mock<ISapProxy>();
            this.mockConfigurationHandler = new Mock<IConfigurationHandler>();
            this.mockMovementRepository = new Mock<IMovementRepository>();
            this.sapRepository = new Mock<IRepository<SapMapping>>();
            this.mockLogger = new Mock<ITrueLogger<SapStatusProcessor>>();
            this.mockSapTrackingRepository = new Mock<IRepository<SapTracking>>();
            this.unitOfWorkMock.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.mockFileRegistrationRepository = new Mock<IRepository<FileRegistration>>();
            this.mockSapUploadRepository = new Mock<IRepository<SapUpload>>();
            this.sapTrackingRepository = new Mock<IRepository<SapTracking>>();
            this.mockAzureClientFactory = new Mock<IAzureClientFactory>();
            this.mockServiceBusQueueClient = new Mock<IServiceBusQueueClient>();
            this.mockFileRegistrationTransactionRepository = new Mock<IRepository<FileRegistrationTransaction>>();
            this.mockSapUploadContractRepository = new Mock<IRepository<SapUploadContract>>();
            this.blobClientMock = new Mock<IBlobStorageClient>();
            this.sapTrakingMock = new Mock<ISapTrackingProcessor>();
            this.telemetryMock = new Mock<ITelemetry>();
            this.fileRegistrationTransactionService = new Mock<IFileRegistrationTransactionService>();
            this.mockBlobSasClient = new Mock<IBlobStorageSasClient>();
            this.sapStatusProcessor = new SapStatusProcessor(
                this.mockLogger.Object,
                this.unitOfWorkFactoryMock.Object,
                this.mockSapProxy.Object,
                this.mockAzureClientFactory.Object,
                this.sapTrakingMock.Object,
                this.telemetryMock.Object,
                this.fileRegistrationTransactionService.Object);
        }

        /// <summary>
        /// the processor update failure sap tracking asynchronous asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        /// <summary>
        /// Updates the upload status should be successfull asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task SapStatusProcessor_TryUploadStatusAsync_ShouldScheduleMessageAsync()
        {
            var fileRegistration = new FileRegistration
            {
                UploadId = "test",
                FileUploadStatus = FileUploadStatus.PROCESSING,
                SourceTypeId = SystemType.PURCHASE,
            };
            this.mockLogger.Setup(x => x.LogInformation(It.IsAny<string>())).Verifiable();
            this.unitOfWorkMock.Setup(x => x.CreateRepository<FileRegistration>()).Returns(this.mockFileRegistrationRepository.Object);
            this.mockFileRegistrationRepository.Setup(s => s.FirstOrDefaultAsync(It.IsAny<Expression<Func<FileRegistration, bool>>>())).ReturnsAsync(fileRegistration);
            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<ServiceBusSettings>(It.IsAny<string>())).ReturnsAsync(new ServiceBusSettings() { ConnectionString = string.Empty });
            this.mockServiceBusQueueClient.Setup(x => x.QueueScheduleMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            this.mockAzureClientFactory.Setup(x => x.GetBlobStorageSaSClient(It.IsAny<string>(), It.IsAny<string>())).Returns(this.mockBlobSasClient.Object);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<FileRegistrationTransaction>()).Returns(this.mockFileRegistrationTransactionRepository.Object);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<SapUploadContract>()).Returns(this.mockSapUploadContractRepository.Object);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<SapUploadContract>().ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).Returns(this.GetSapContractResponseAsync());
            this.fileRegistrationTransactionService.Setup(x => x.InsertFileRegistrationAsync(It.IsAny<FileRegistration>()));

            await this.sapStatusProcessor.TryUploadStatusAsync(fileRegistration.UploadId).ConfigureAwait(false);

            this.unitOfWorkMock.Verify(x => x.CreateRepository<FileRegistration>(), Times.Once);
        }

        /// <summary>
        /// the status processor try upload status asynchronous should update status asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task SapStatusProcessor_TryUploadStatusAsync__ShouldUpdateStatusAsync()
        {
            var fileRegistration = new FileRegistration()
            {
                UploadId = Guid.NewGuid().ToString(),
                FileUploadStatus = FileUploadStatus.FINALIZED,
                SourceTypeId = SystemType.MOVEMENTS,
            };
            var sapUploads = new List<SapUpload>();
            sapUploads.Add(this.GetSapUpload(fileRegistration.UploadId));
            this.mockLogger.Setup(x => x.LogInformation(It.IsAny<string>())).Verifiable();
            this.unitOfWorkMock.Setup(x => x.CreateRepository<FileRegistration>()).Returns(this.mockFileRegistrationRepository.Object);
            this.mockFileRegistrationRepository.Setup(s => s.FirstOrDefaultAsync(It.IsAny<Expression<Func<FileRegistration, bool>>>())).ReturnsAsync(fileRegistration);
            this.mockSapProxy.Setup(x => x.UpdateUploadStatusAsync(It.IsAny<SapUploadStatus>())).ReturnsAsync(true);
            this.mockSapProxy.Setup(x => x.UpdateUploadStatusContractAsync(It.IsAny<SapUploadStatusContract>())).ReturnsAsync(true);
            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<ServiceBusSettings>(It.IsAny<string>())).ReturnsAsync(new ServiceBusSettings() { ConnectionString = string.Empty });
            this.mockServiceBusQueueClient.Setup(x => x.QueueScheduleMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            this.mockAzureClientFactory.Setup(x => x.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<SapUpload>()).Returns(this.mockSapUploadRepository.Object);
            this.mockSapUploadRepository.Setup(x => x.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(sapUploads);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<SapTracking>()).Returns(this.sapTrackingRepository.Object);
            this.sapTrackingRepository.Setup(x => x.Insert(It.IsAny<SapTracking>()));
            this.unitOfWorkMock.Setup(x => x.CreateRepository<FileRegistrationTransaction>()).Returns(this.mockFileRegistrationTransactionRepository.Object);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<SapUploadContract>()).Returns(this.mockSapUploadContractRepository.Object);
            this.mockAzureClientFactory.Setup(x => x.GetBlobStorageSaSClient(It.IsAny<string>(), It.IsAny<string>())).Returns(this.mockBlobSasClient.Object);
            this.mockFileRegistrationTransactionRepository.Setup(x => x.GetCountAsync(It.IsAny<Expression<Func<FileRegistrationTransaction, bool>>>())).ReturnsAsync(0);
            this.fileRegistrationTransactionService.Setup(x => x.InsertFileRegistrationAsync(It.IsAny<FileRegistration>()));

            await this.sapStatusProcessor.TryUploadStatusAsync(fileRegistration.UploadId).ConfigureAwait(false);

            this.unitOfWorkMock.Verify(x => x.CreateRepository<FileRegistration>(), Times.Once);
            this.mockSapUploadRepository.Verify(x => x.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockSapProxy.Verify(x => x.UpdateUploadStatusAsync(It.IsAny<SapUploadStatus>()), Times.Once);
        }

        /// <summary>
        /// Saps the status processor try upload status asynchronous should update error status asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task SapStatusProcessor_TryUploadStatusAsync_ShouldUpdateErrorStatusAsync()
        {
            var fileRegistration = new FileRegistration()
            {
                UploadId = Guid.NewGuid().ToString(),
                FileUploadStatus = FileUploadStatus.FINALIZED,
                SourceTypeId = SystemType.MOVEMENTS,
            };
            var sapUploads = new List<SapUpload>();
            sapUploads.Add(this.GetSapUpload(fileRegistration.UploadId));

            this.mockLogger.Setup(x => x.LogInformation(It.IsAny<string>())).Verifiable();
            this.unitOfWorkMock.Setup(x => x.CreateRepository<FileRegistration>()).Returns(this.mockFileRegistrationRepository.Object);
            this.mockFileRegistrationRepository.Setup(s => s.FirstOrDefaultAsync(It.IsAny<Expression<Func<FileRegistration, bool>>>())).ReturnsAsync(fileRegistration);
            this.mockSapProxy.Setup(x => x.UpdateUploadStatusAsync(It.IsAny<SapUploadStatus>())).Throws(new Exception());
            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<ServiceBusSettings>(It.IsAny<string>())).ReturnsAsync(new ServiceBusSettings() { ConnectionString = string.Empty });
            this.mockServiceBusQueueClient.Setup(x => x.QueueScheduleMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            this.mockAzureClientFactory.Setup(x => x.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<SapUpload>()).Returns(this.mockSapUploadRepository.Object);
            this.mockSapUploadRepository.Setup(x => x.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(sapUploads);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<SapTracking>()).Returns(this.sapTrackingRepository.Object);
            this.sapTrackingRepository.Setup(x => x.Insert(It.IsAny<SapTracking>()));
            this.unitOfWorkMock.Setup(x => x.CreateRepository<FileRegistrationTransaction>()).Returns(this.mockFileRegistrationTransactionRepository.Object);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<SapUploadContract>()).Returns(this.mockSapUploadContractRepository.Object);
            this.mockAzureClientFactory.Setup(x => x.GetBlobClient(It.IsAny<string>())).Returns(this.blobClientMock.Object);
            this.fileRegistrationTransactionService.Setup(x => x.InsertFileRegistrationAsync(It.IsAny<FileRegistration>()));

            await this.sapStatusProcessor.TryUploadStatusAsync(fileRegistration.UploadId).ConfigureAwait(false);

            this.unitOfWorkMock.Verify(x => x.CreateRepository<FileRegistration>(), Times.Once);
            this.mockSapUploadRepository.Verify(x => x.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockSapProxy.Verify(x => x.UpdateUploadStatusAsync(It.IsAny<SapUploadStatus>()), Times.Once);
        }

        private SapUpload GetSapUpload(string uploadId)
        {
            var sapUpload = new SapUpload();
            sapUpload.ProcessId = uploadId;
            sapUpload.FileRegistrationId = 1;
            sapUpload.DocumentId = "1";
            sapUpload.TransactionId = 1;
            sapUpload.ErrorCode = "000";
            sapUpload.ErrorMessage = "Transacción exitosa";
            return sapUpload;
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
