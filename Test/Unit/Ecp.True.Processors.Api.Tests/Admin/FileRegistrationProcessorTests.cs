// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileRegistrationProcessorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Tests.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
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
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Api.Admin;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// The node processor tests.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Api.Tests.ProcessorTestBase" />
    [TestClass]
    public class FileRegistrationProcessorTests : ProcessorTestBase
    {
        /// <summary>
        /// The mockblob client.
        /// </summary>
        private Mock<IBlobStorageSasClient> mockBlobSasClient;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<FileRegistrationProcessor>> mockLogger;

        /// <summary>
        /// The processor.
        /// </summary>
        private FileRegistrationProcessor processor;

        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigHandler;

        /// <summary>
        /// The mock business context.
        /// </summary>
        private Mock<IAzureClientFactory> azureClientFactory;

        /// <summary>
        /// The mockblob client.
        /// </summary>
        private Mock<IBlobStorageClient> mockBlobClient;

        /// <summary>
        /// The mock queue client.
        /// </summary>
        private Mock<IServiceBusQueueClient> mockQueueClient;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.azureClientFactory = new Mock<IAzureClientFactory>();
            this.mockBlobClient = new Mock<IBlobStorageClient>();
            this.mockQueueClient = new Mock<IServiceBusQueueClient>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockConfigHandler = new Mock<IConfigurationHandler>();
            this.mockLogger = new Mock<ITrueLogger<FileRegistrationProcessor>>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockBlobSasClient = new Mock<IBlobStorageSasClient>();

            this.mockUnitOfWork.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));
            this.mockUnitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.azureClientFactory.Setup(a => a.GetBlobStorageSaSClient(It.IsAny<string>(), It.IsAny<string>())).Returns(this.mockBlobSasClient.Object);
            this.azureClientFactory.Setup(a => a.GetQueueClient(It.IsAny<string>())).Returns(this.mockQueueClient.Object);
            this.mockQueueClient.Setup(c => c.QueueMessageAsync(It.IsAny<FileRegistration>()));
            this.mockQueueClient.Setup(c => c.QueueMessageAsync(It.IsAny<int>()));
            this.mockQueueClient.Setup(c => c.QueueMessageAsync(It.IsAny<JsonQueueMessage>()));

            this.processor = new FileRegistrationProcessor(this.mockLogger.Object, this.mockFactory.Object, this.mockUnitOfWorkFactory.Object, this.azureClientFactory.Object, this.mockConfigHandler.Object);
        }

        /// <summary>
        /// Gets the Read Sas Token for container.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetReadFile_RegistrationAccessInfoAsync()
        {
            var fileAccessInfo = new FileAccessInfo { AccountName = "storageAccount" };
            this.azureClientFactory.Setup(m => m.GetFileAccessInfoAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<BlobSasPermissions>())).ReturnsAsync(fileAccessInfo);
            var result = await this.processor.GetFileRegistrationAccessInfoAsync().ConfigureAwait(false);
            this.azureClientFactory.Verify(a => a.GetFileAccessInfoAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<BlobSasPermissions>()), Times.Once);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(fileAccessInfo, result);
        }

        /// <summary>
        /// Gets the Read Sas Token for container.
        /// </summary>
        /// <param name="containerName">The container name.</param>
        /// <returns>The task.</returns>
        [TestMethod]
        [DataRow(ContainerName.True)]
        [DataRow(ContainerName.Delta)]
        [DataRow(ContainerName.Ownership)]
        public async Task GetReadFile_RegistrationAccessInfoByContainerAsync(string containerName)
        {
            var fileAccessInfo = new FileAccessInfo { AccountName = "storageAccount", ContainerName = containerName };
            this.azureClientFactory.Setup(m => m.GetFileAccessInfoAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<BlobSasPermissions>())).ReturnsAsync(fileAccessInfo);

            var result = await this.processor.GetFileRegistrationAccessInfoByContainerAsync(containerName).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(fileAccessInfo, result);

            this.azureClientFactory.Verify(a => a.GetFileAccessInfoAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<BlobSasPermissions>()), Times.Once);
        }

        /// <summary>
        /// Gets the Write Sas Token for container.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetUploadFile_RegistrationAccessInfoAsync()
        {
            var fileAccessInfo = new FileAccessInfo { AccountName = "storageAccount" };
            string fileName = "GUID FILE NAME";
            this.azureClientFactory.Setup(m => m.GetFileAccessInfoAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<BlobSasPermissions>())).ReturnsAsync(fileAccessInfo);
            var result = await this.processor.GetFileRegistrationAccessInfoAsync(fileName, It.IsAny<SystemType>()).ConfigureAwait(false);
            this.azureClientFactory.Verify(a => a.GetFileAccessInfoAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<BlobSasPermissions>()), Times.Once);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(fileAccessInfo, result);
        }

        /// <summary>
        /// Gets the register files asynchronous should returns existing files asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetRegisterFilesAsync_ShouldReturnsExistingFilesAsync()
        {
            var fileIds = new[] { Guid.NewGuid(), Guid.NewGuid() };
            var registerFiles = new[] { new FileRegistration() { UploadId = Guid.NewGuid().ToString() } };
            var repoMock = new Mock<IRepository<FileRegistration>>();
            repoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<FileRegistration, bool>>>())).ReturnsAsync(registerFiles);

            this.mockFactory.Setup(m => m.CreateRepository<FileRegistration>()).Returns(repoMock.Object);

            var result = await this.processor.GetFileRegistrationStatusAsync(fileIds).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(result, registerFiles);

            this.mockFactory.Verify(m => m.CreateRepository<FileRegistration>(), Times.Once);
            repoMock.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<FileRegistration, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Registers the file asynchronous should add new files to database asynchronous.
        /// </summary>
        /// <returns>The tasks.</returns>
        [TestMethod]
        public async Task RegisterFileAsync_ShouldAddNewFilesToDatabaseAsync()
        {
            var token = new CancellationToken(false);
            var repoMock = new Mock<IRepository<FileRegistration>>();
            repoMock.Setup(r => r.Insert(It.IsAny<FileRegistration>()));
            this.mockUnitOfWork.Setup(a => a.CreateRepository<FileRegistration>()).Returns(repoMock.Object);

            this.mockConfigHandler.Setup(c => c.GetConfigurationAsync<ServiceBusSettings>(ConfigurationConstants.ServiceBusSettings)).ReturnsAsync(new ServiceBusSettings());
            var registerFile = new FileRegistration { UploadId = Guid.NewGuid().ToString(), SystemTypeId = Entities.Dto.SystemType.EXCEL, SourceTypeId = SystemType.EXCEL };

            await this.processor.RegisterAsync(registerFile).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(m => m.CreateRepository<FileRegistration>(), Times.Once);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(token), Times.Once);
            repoMock.Verify(r => r.Insert(It.IsAny<FileRegistration>()), Times.Once);

            this.azureClientFactory.Verify(a => a.GetQueueClient(It.IsAny<string>()), Times.Once);
            this.mockQueueClient.Verify(c => c.QueueMessageAsync(It.IsAny<FileRegistration>()), Times.Once);
        }

        /// <summary>
        /// Registers the file asynchronous should fail file registration if push message to service bus fails asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task RegisterFileAsync_ShouldFailFileRegistration_If_PushMessageToServiceBusFailsAsync()
        {
            var token = new CancellationToken(false);
            var fileRegistration = new FileRegistration { FileRegistrationId = 123, SourceTypeId = SystemType.EXCEL };

            var mockPendingTransactionRepository = new Mock<IRepository<PendingTransaction>>();
            mockPendingTransactionRepository.Setup(r => r.Insert(It.IsAny<PendingTransaction>()));

            var mockFileRegistrationRepository = new Mock<IRepository<FileRegistration>>();
            mockFileRegistrationRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(fileRegistration);
            mockFileRegistrationRepository.Setup(r => r.Insert(It.IsAny<FileRegistration>()));
            mockFileRegistrationRepository.Setup(r => r.Update(It.IsAny<FileRegistration>()));

            this.mockQueueClient.Setup(c => c.QueueMessageAsync(It.IsAny<FileRegistration>())).ThrowsAsync(new Exception());
            this.mockUnitOfWork.Setup(a => a.CreateRepository<FileRegistration>()).Returns(mockFileRegistrationRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<PendingTransaction>()).Returns(mockPendingTransactionRepository.Object);
            this.mockConfigHandler.Setup(c => c.GetConfigurationAsync<ServiceBusSettings>(ConfigurationConstants.ServiceBusSettings)).ReturnsAsync(new ServiceBusSettings());
            var registerFile = new FileRegistration { UploadId = Guid.NewGuid().ToString(), SystemTypeId = Entities.Dto.SystemType.EXCEL, SourceTypeId = SystemType.EXCEL };

            await this.processor.RegisterAsync(registerFile).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(m => m.CreateRepository<FileRegistration>(), Times.Exactly(2));
            this.mockUnitOfWork.Verify(m => m.SaveAsync(token), Times.Exactly(2));
            mockFileRegistrationRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
            mockFileRegistrationRepository.Verify(r => r.Insert(It.IsAny<FileRegistration>()), Times.Once);
            mockFileRegistrationRepository.Verify(r => r.Update(It.IsAny<FileRegistration>()), Times.Once);

            this.azureClientFactory.Verify(a => a.GetQueueClient(It.IsAny<string>()), Times.Once);
            this.mockQueueClient.Verify(c => c.QueueMessageAsync(It.IsAny<FileRegistration>()), Times.Once);

            mockPendingTransactionRepository.Verify(r => r.Insert(It.IsAny<PendingTransaction>()), Times.Once);
        }

        /// <summary>
        /// Registers the file asynchronous should add new files to database asynchronous.
        /// </summary>
        /// <returns>The tasks.</returns>
        [TestMethod]
        public async Task RetryAsync_ShouldAddNewMessagesToServiceBusQueueAsync()
        {
            var token = new CancellationToken(false);
            var fileRegistrationTransactionRepoMock = new Mock<IRepository<FileRegistrationTransaction>>();
            var pendingTransactionErrorRepoMock = new Mock<IRepository<PendingTransactionError>>();
            this.mockFactory.Setup(s => s.CreateRepository<FileRegistrationTransaction>()).Returns(fileRegistrationTransactionRepoMock.Object);
            fileRegistrationTransactionRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(new FileRegistrationTransaction());
            pendingTransactionErrorRepoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<PendingTransactionError, bool>>>())).ReturnsAsync(new List<PendingTransactionError>() { new PendingTransactionError() });

            pendingTransactionErrorRepoMock.Setup(r => r.Update(It.IsAny<PendingTransactionError>()));
            this.mockUnitOfWork.Setup(a => a.CreateRepository<PendingTransactionError>()).Returns(pendingTransactionErrorRepoMock.Object);

            this.mockUnitOfWork.Setup(m => m.SaveAsync(token));

            this.mockConfigHandler.Setup(c => c.GetConfigurationAsync<ServiceBusSettings>(ConfigurationConstants.ServiceBusSettings)).ReturnsAsync(new ServiceBusSettings());

            await this.processor.RetryAsync(new int[] { 1 }).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(m => m.CreateRepository<PendingTransactionError>(), Times.Once);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(token), Times.Once);
            pendingTransactionErrorRepoMock.Verify(r => r.Update(It.IsAny<PendingTransactionError>()), Times.Once);

            this.azureClientFactory.Verify(a => a.GetQueueClient(It.IsAny<string>()), Times.Once);
            this.mockQueueClient.Verify(c => c.QueueMessageAsync(It.IsAny<JsonQueueMessage>()), Times.Once);
        }
    }
}
