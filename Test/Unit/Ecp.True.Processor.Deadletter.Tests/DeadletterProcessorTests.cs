// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeadletterProcessorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Deadletter.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Core.Dto;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Deadletter;
    using Ecp.True.Processors.Deadletter.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The deadletter processor tests.
    /// </summary>
    [TestClass]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Test Class")]
    public class DeadletterProcessorTests
    {
        /// <summary>
        /// The BLOB client.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureclientFactory;

        /// <summary>
        /// The mockblob client.
        /// </summary>
        private Mock<IBlobStorageClient> mockBlobClient;

        /// <summary>
        /// The repository factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock telemetry.
        /// </summary>
        private Mock<ITelemetry> mockTelemetry;

        /// <summary>
        /// The deadlettered Message Repository Mock.
        /// </summary>
        private Mock<IRepository<DeadletteredMessage>> deadletteredMessageRepositoryMock;

        /// <summary>
        /// The deadletter processor.
        /// </summary>
        private DeadletterProcessor deadletterProcessor;

        /// <summary>
        /// The repository factory.
        /// </summary>
        private Mock<IRepositoryFactory> repositoryFactory;

        /// <summary>
        /// The mock service bus queue client.
        /// </summary>
        private Mock<IServiceBusQueueClient> mockServiceBusQueueClient;

        /// <summary>
        /// The mock service bus queue client.
        /// </summary>
        private Mock<IReconcileService> mockReconcileService;

        /// <summary>
        /// The mockblob client.
        /// </summary>
        private Mock<IBlobStorageSasClient> mockBlobSasClient;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.repositoryFactory = new Mock<IRepositoryFactory>();
            this.unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockServiceBusQueueClient = new Mock<IServiceBusQueueClient>();
            this.mockReconcileService = new Mock<IReconcileService>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockTelemetry = new Mock<ITelemetry>();

            this.mockAzureclientFactory = new Mock<IAzureClientFactory>();
            this.mockBlobClient = new Mock<IBlobStorageClient>();
            this.mockBlobSasClient = new Mock<IBlobStorageSasClient>();
            this.mockAzureclientFactory.Setup(a => a.GetBlobClient(It.IsAny<string>())).Returns(this.mockBlobClient.Object);
            this.mockAzureclientFactory.Setup(a => a.GetBlobStorageSaSClient(It.IsAny<string>(), It.IsAny<string>())).Returns(this.mockBlobSasClient.Object);
            this.mockAzureclientFactory.Setup(a => a.GetBlobClient(It.IsAny<string>())).Returns(this.mockBlobClient.Object);
            this.mockAzureclientFactory.Setup(a => a.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);

            this.deadletteredMessageRepositoryMock = new Mock<IRepository<DeadletteredMessage>>();
            this.deadletteredMessageRepositoryMock.Setup(m => m.Insert(It.IsAny<DeadletteredMessage>()));
            this.deadletteredMessageRepositoryMock.Setup(m => m.Update(It.IsAny<DeadletteredMessage>()));

            this.repositoryFactory.Setup(x => x.CreateRepository<DeadletteredMessage>()).Returns(this.deadletteredMessageRepositoryMock.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<DeadletteredMessage>()).Returns(this.deadletteredMessageRepositoryMock.Object);
            this.mockUnitOfWork.Setup(m => m.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            this.unitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);

            this.deadletterProcessor = new DeadletterProcessor(
                this.mockAzureclientFactory.Object,
                this.unitOfWorkFactory.Object,
                this.mockReconcileService.Object,
                this.repositoryFactory.Object,
                this.mockTelemetry.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task DeadletterProcessor_ThrowErrorAsync()
        {
            await this.deadletterProcessor.ProcessAsync(null).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task DeadletterProcessor_ShouldCreateBlob_WithSuccessAsync()
        {
            var deadletteredMessage = new DeadletteredMessage
            {
                DeadletteredMessageId = 1,
                BlobPath = string.Empty,
                QueueName = "Movement",
                ProcessName = "Movement",
                Content = new JObject(new JProperty("Movement", "Movement")),
                Status = false,
            };
            this.mockBlobSasClient.Setup(b => b.CreateBlobAsync(It.IsAny<Stream>())).Returns(Task.CompletedTask);
            this.mockBlobSasClient.Setup(b => b.CreateBlobAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            await this.deadletterProcessor.ProcessAsync(deadletteredMessage).ConfigureAwait(false);

            this.mockBlobSasClient.Verify(b => b.CreateBlobAsync(It.IsAny<string>()), Times.Once);
            Assert.IsNotNull(deadletteredMessage.BlobPath);
            Assert.AreEqual(true, deadletteredMessage.Status);
        }

        [TestMethod]
        public async Task DeadletterProcessor_ShouldRetrigger_WithSuccessAsync()
        {
            var deadletteredMessage = new DeadletteredMessage
            {
                DeadletteredMessageId = 1,
                BlobPath = string.Empty,
                QueueName = "Movement",
                ProcessName = "Movement",
                Content = new JObject(new JProperty("Movement", "Movement")),
                Status = false,
            };
            IEnumerable<int> deadlettredMessageIds = new List<int>() { 1 };

            // Creating the blob.
            this.deadletteredMessageRepositoryMock.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<DeadletteredMessage, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<DeadletteredMessage> { deadletteredMessage });
            this.mockBlobSasClient.Setup(b => b.CreateBlobAsync(It.IsAny<Stream>())).Returns(Task.CompletedTask);
            this.mockBlobSasClient.Setup(b => b.CreateBlobAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            await this.deadletterProcessor.ProcessAsync(deadletteredMessage).ConfigureAwait(false);
            this.mockAzureclientFactory.Setup(a => a.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);
            this.mockServiceBusQueueClient.Setup(a => a.QueueMessageAsync(It.IsAny<object>()));

            var stream = this.GenerateStreamFromString(JsonConvert.SerializeObject(new List<DeadletteredMessage> { deadletteredMessage }));
            this.mockBlobSasClient.Setup(b => b.GetCloudBlobStreamAsync()).ReturnsAsync(stream);
            this.mockBlobSasClient.Setup(b => b.DeleteBlobAsync()).Returns(Task.CompletedTask);

            ////Retriggering the deadlettred message to the queue
            await this.deadletterProcessor.RetriggerAsync(deadlettredMessageIds).ConfigureAwait(false);

            this.mockAzureclientFactory.Verify(a => a.GetQueueClient(It.IsAny<string>()), Times.Once);
            this.mockBlobSasClient.Verify(b => b.GetCloudBlobStreamAsync(), Times.Once);
            this.mockBlobSasClient.Verify(b => b.DeleteBlobAsync(), Times.Once);

            Assert.AreEqual(false, deadletteredMessage.Status);
        }

        [TestMethod]
        public async Task DeadletterProcessor_ShouldResetCriticalRecordsAsync_WithSuccessAsync()
        {
            this.mockReconcileService.Setup(x => x.ResetAsync(It.IsAny<BlockchainFailures>())).Returns(Task.CompletedTask);
            await this.deadletterProcessor.ResetAsync(new BlockchainFailures()).ConfigureAwait(false);
            this.mockReconcileService.Verify(b => b.ResetAsync(It.IsAny<BlockchainFailures>()), Times.Once);
        }

        [TestMethod]
        public async Task DeadletterProcessor_GetReconciliationInfoAsync_ShouldReturnInfoAsync()
        {
            var info = new List<FailedRecord>
            {
                new FailedRecord { Name = nameof(Movement), RecordId = 1 },
                new FailedRecord { Name = nameof(InventoryProduct), RecordId = 2 },
                new FailedRecord { Name = nameof(Unbalance), RecordId = 3 },
                new FailedRecord { Name = nameof(Ownership), RecordId = 4 },
                new FailedRecord { Name = nameof(OffchainNode), RecordId = 5 },
                new FailedRecord { Name = nameof(OffchainNodeConnection), RecordId = 6 },
            };

            this.mockReconcileService.Setup(x => x.GetFailuresAsync(It.IsAny<bool>(), null)).ReturnsAsync(info);
            var parameter = new BlockchainFailuresRequest();
            parameter.IsCritical = true;

            var failures = await this.deadletterProcessor.GetFailuresAsync(parameter).ConfigureAwait(false);

            this.mockReconcileService.Verify(b => b.GetFailuresAsync(true, null), Times.Once);
            Assert.AreEqual(1, failures.Movements.Count());
            Assert.AreEqual(1, failures.Movements.First());

            Assert.AreEqual(1, failures.InventoryProducts.Count());
            Assert.AreEqual(2, failures.InventoryProducts.First());

            Assert.AreEqual(1, failures.Unbalances.Count());
            Assert.AreEqual(3, failures.Unbalances.First());

            Assert.AreEqual(1, failures.Ownerships.Count());
            Assert.AreEqual(4, failures.Ownerships.First());

            Assert.AreEqual(1, failures.Nodes.Count());
            Assert.AreEqual(5, failures.Nodes.First());

            Assert.AreEqual(1, failures.Connections.Count());
            Assert.AreEqual(6, failures.Connections.First());
        }

        /// <summary>
        /// Deads the letter processor handle registration failure asynchronous should log pending transaction when is retry true invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task DeadLetterProcessor_HandleRegistrationFailureAsync_ShouldLogPendingTransaction_When_IsRetry_True_InvokedAsync()
        {
            var entityId = "1";

            var fileRegistrationTransaction = new FileRegistrationTransaction
            {
                FileRegistrationTransactionId = 1,
                FileRegistration = new FileRegistration
                {
                    UploadId = "123",
                },
            };

            var mockFileRegistrationTransactionRepository = new Mock<IRepository<FileRegistrationTransaction>>();
            mockFileRegistrationTransactionRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<FileRegistrationTransaction, bool>>>(), It.IsAny<string>())).ReturnsAsync(fileRegistrationTransaction);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<FileRegistrationTransaction>()).Returns(mockFileRegistrationTransactionRepository.Object);

            var mockPendingTransactionRepository = new Mock<IRepository<PendingTransaction>>();
            mockPendingTransactionRepository.Setup(a => a.Insert(It.IsAny<PendingTransaction>()));
            this.mockUnitOfWork.Setup(m => m.CreateRepository<PendingTransaction>()).Returns(mockPendingTransactionRepository.Object);

            await this.deadletterProcessor.HandleRegistrationRetryFailureAsync(entityId).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(m => m.CreateRepository<FileRegistrationTransaction>(), Times.Once);
            mockFileRegistrationTransactionRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<FileRegistrationTransaction, bool>>>(), It.IsAny<string>()), Times.Once);
            mockPendingTransactionRepository.Verify(a => a.Insert(It.IsAny<PendingTransaction>()), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<PendingTransaction>(), Times.Once);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Deads the letter processor handle registration failure asynchronous should update file registration is parsed when is retry false invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task DeadLetterProcessor_HandleRegistrationFailureAsync_ShouldUpdateFileRegistrationIsParsed_When_IsRetry_False_InvokedAsync()
        {
            var entityId = "123";

            var fileRegistration = new FileRegistration
            {
                UploadId = "123",
            };

            var mockfileRegistrationRepository = new Mock<IRepository<FileRegistration>>();
            mockfileRegistrationRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<FileRegistration, bool>>>())).ReturnsAsync(fileRegistration);
            mockfileRegistrationRepository.Setup(r => r.Update(It.IsAny<FileRegistration>()));
            this.mockUnitOfWork.Setup(m => m.CreateRepository<FileRegistration>()).Returns(mockfileRegistrationRepository.Object);

            var mockPendingTransactionRepository = new Mock<IRepository<PendingTransaction>>();
            mockPendingTransactionRepository.Setup(a => a.Insert(It.IsAny<PendingTransaction>()));
            this.mockUnitOfWork.Setup(m => m.CreateRepository<PendingTransaction>()).Returns(mockPendingTransactionRepository.Object);

            await this.deadletterProcessor.HandleRegistrationFailureAsync(entityId).ConfigureAwait(false);

            mockfileRegistrationRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<FileRegistration, bool>>>()), Times.Once);
            mockfileRegistrationRepository.Verify(r => r.Update(It.IsAny<FileRegistration>()), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<FileRegistration>(), Times.Once);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockPendingTransactionRepository.Verify(a => a.Insert(It.IsAny<PendingTransaction>()), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<PendingTransaction>(), Times.Once);
        }

        /// <summary>
        /// Deads the letter processor handle sap failure asynchronous should update failed status asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DeadLetterProcessor_HandleSapFailureAsync_ShouldUpdateFailedStatusAsync()
        {
            var sapQueueMessage = new SapQueueMessage(SapRequestType.Upload, "test");

            var fileRegistration = new FileRegistration
            {
                UploadId = "test",
            };
            var mockfileRegistrationRepository = new Mock<IRepository<FileRegistration>>();
            mockfileRegistrationRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<FileRegistration, bool>>>())).ReturnsAsync(fileRegistration);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<FileRegistration>()).Returns(mockfileRegistrationRepository.Object);

            var mockSapTrackingRepository = new Mock<IRepository<SapTracking>>();
            mockSapTrackingRepository.Setup(a => a.Insert(It.IsAny<SapTracking>()));
            this.mockUnitOfWork.Setup(m => m.CreateRepository<SapTracking>()).Returns(mockSapTrackingRepository.Object);

            await this.deadletterProcessor.HandleSapFailureAsync(sapQueueMessage).ConfigureAwait(false);

            mockfileRegistrationRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<FileRegistration, bool>>>()), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<FileRegistration>(), Times.Once);
            mockSapTrackingRepository.Verify(a => a.Insert(It.IsAny<SapTracking>()), Times.Once);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<SapTracking>(), Times.Once);
        }

        /// <summary>
        /// Deads the letter processor handle sap failure asynchronous should update movement status asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DeadLetterProcessor_HandleSapFailureAsync_ShouldUpdateMovementStatusAsync()
        {
            var sapQueueMessage = new SapQueueMessage
            {
                MessageId = 1,
                PreviousMovementTransactionId = 2,
                RequestType = SapRequestType.Movement,
            };

            var sapTracking = new SapTracking { MovementTransactionId = 1 };

            var mockSapTrackingRepository = new Mock<IRepository<SapTracking>>();
            mockSapTrackingRepository.Setup(a => a.FirstOrDefaultAsync(It.IsAny<Expression<Func<SapTracking, bool>>>())).ReturnsAsync(sapTracking);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<SapTracking>()).Returns(mockSapTrackingRepository.Object);

            await this.deadletterProcessor.HandleSapFailureAsync(sapQueueMessage).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(m => m.CreateRepository<SapTracking>(), Times.Once);
            mockSapTrackingRepository.Verify(a => a.Update(It.IsAny<SapTracking>()), Times.Exactly(2));
            this.mockUnitOfWork.Verify(m => m.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Deadletters the processor should log critical event when processing offchain message.
        /// </summary>
        [TestMethod]
        public void DeadletterProcessor_ShouldLogCriticalEvent_WhenProcessingOffchainMessage()
        {
            var message = new OffchainMessage();
            this.mockTelemetry.Setup(m => m.TrackEvent(Constants.Critical, EventName.OffchainSyncFailed.ToString("G"), It.IsAny<IDictionary<string, string>>(), null, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()));

            this.deadletterProcessor.HandleOffchainFailure(message);
            this.mockTelemetry.Verify(m => m.TrackEvent(Constants.Critical, EventName.OffchainSyncFailed.ToString("G"), It.IsAny<IDictionary<string, string>>(), null, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Deads the letter processor handle sap failure asynchronous should update movement status asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DeadLetterProcessor_HandleReportFailure_ShouldUpdateFailedStatusAsync()
        {
            var entity = new ReportExecution { ExecutionId = 1 };

            var mockRepository = new Mock<IRepository<ReportExecution>>();
            mockRepository.Setup(a => a.GetByIdAsync(entity.ExecutionId)).ReturnsAsync(entity);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<ReportExecution>()).Returns(mockRepository.Object);

            await this.deadletterProcessor.HandleReportFailureAsync(1, ReportType.BeforeCutOff).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(m => m.CreateRepository<ReportExecution>(), Times.Once);
            mockRepository.Verify(a => a.Update(It.Is<ReportExecution>(e => e.StatusTypeId == StatusType.FAILED)), Times.Once);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Generates the stream from string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>The stream.</returns>
        private Stream GenerateStreamFromString(string s)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(s));
        }
    }
}
