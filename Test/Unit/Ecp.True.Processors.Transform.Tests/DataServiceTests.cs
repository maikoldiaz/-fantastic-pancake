// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataServiceTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Tests
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Ownership.Services.Interfaces;
    using Ecp.True.Processors.Transform.Services;
    using Ecp.True.Processors.Transform.Services.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The DataServiceTests.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Transform.Tests.HomologatorTestBase" />
    [TestClass]
    public class DataServiceTests : HomologatorTestBase
    {
        /// <summary>
        /// The BLOB client.
        /// </summary>
        private readonly Mock<IBlobGenerator> blobGeneratorMock = new Mock<IBlobGenerator>();

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly Mock<ITrueLogger<DataService>> mockLogger = new Mock<ITrueLogger<DataService>>();

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The file registration service mock.
        /// </summary>
        private readonly Mock<IFileRegistrationTransactionService> fileRegistrationSericeMock = new Mock<IFileRegistrationTransactionService>();

        /// <summary>
        /// The file registration service mock.
        /// </summary>
        private readonly Mock<ILogisticsService> logisticMovementServiceMock = new Mock<ILogisticsService>();

        /// <summary>
        /// The blobGenerator.
        /// </summary>
        private DataService dataService;

        /// <summary>
        /// The mock service bus queue client.
        /// </summary>
        private Mock<IServiceBusQueueClient> mockServiceBusQueueClient;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockServiceBusQueueClient = new Mock<IServiceBusQueueClient>();

            this.dataService = new DataService(
                this.mockLogger.Object,
                this.blobGeneratorMock.Object,
                this.fileRegistrationSericeMock.Object,
                this.mockAzureClientFactory.Object,
                this.logisticMovementServiceMock.Object);
        }

        /// <summary>
        /// Transforms the processor should save retry message when save retry message invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task TransformProcessor_ShouldSaveRetryMessage_WhenSaveRetryMessageInvokedAsync()
        {
            var trueMessage = new TrueMessage
            {
                Message = MessageType.Inventory,
                SourceSystem = SystemType.SINOPER,
                TargetSystem = SystemType.TRUE,
                MessageId = "someId",
            };

            var jobject = this.GetMovementJArray();
            this.blobGeneratorMock.Setup(m => m.GenerateBlobsArrayAsync(It.IsAny<JArray>(), It.IsAny<TrueMessage>())).Callback<JArray, TrueMessage>((array, message) =>
            {
                message.FileRegistration.FileRegistrationTransactions.Add(new FileRegistrationTransaction
                {
                    FileRegistrationId = 1,
                    BlobPath = "xml/movements",
                });
            });

            this.mockAzureClientFactory.Setup(m => m.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);

            await this.dataService.SaveAsync(jobject, trueMessage).ConfigureAwait(false);

            this.blobGeneratorMock.Verify(m => m.GenerateBlobsArrayAsync(It.IsAny<JArray>(), It.IsAny<TrueMessage>()), Times.Once);
            this.mockAzureClientFactory.Verify(m => m.GetQueueClient(It.IsAny<string>()), Times.Exactly(1));
        }

        /// <summary>
        /// Transforms the processor should not queue message when save retry message invoked without file registration transaction asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task TransformProcessor_ShouldNotQueueMessage_WhenSaveRetryMessage_InvokedWithoutFileRegistrationTransactionAsync()
        {
            var trueMessage = new TrueMessage
            {
                Message = MessageType.Inventory,
                SourceSystem = SystemType.SINOPER,
                TargetSystem = SystemType.TRUE,
                MessageId = "someId",
            };

            var jobject = this.GetMovementJArray();
            this.blobGeneratorMock.Setup(m => m.GenerateBlobsArrayAsync(It.IsAny<JArray>(), It.IsAny<TrueMessage>()));

            this.mockAzureClientFactory.Setup(m => m.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);

            await this.dataService.SaveAsync(jobject, trueMessage).ConfigureAwait(false);

            this.blobGeneratorMock.Verify(m => m.GenerateBlobsArrayAsync(It.IsAny<JArray>(), It.IsAny<TrueMessage>()), Times.Once);
            this.mockAzureClientFactory.Verify(m => m.GetQueueClient(It.IsAny<string>()), Times.Never);
        }

        /// <summary>
        /// Transforms the processor should return j object when transform movement invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task TransformProcessor_ShouldReturnJObject_WhenTransformMovementInvokedAsync()
        {
            var trueMessage = new TrueMessage
            {
                Message = MessageType.Inventory,
                SourceSystem = SystemType.SINOPER,
                TargetSystem = SystemType.TRUE,
                MessageId = "someId",
            };

            var jobject = this.GetMovementJArray();
            this.blobGeneratorMock.Setup(m => m.GenerateBlobsArrayAsync(jobject, trueMessage)).Callback<JArray, TrueMessage>((array, message) =>
            {
                message.FileRegistration.FileRegistrationTransactions.Add(new FileRegistrationTransaction
                {
                    FileRegistrationId = 1,
                    BlobPath = "xml/movements",
                });
            });
            this.mockAzureClientFactory.Setup(m => m.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);
            await this.dataService.SaveAsync(jobject, trueMessage).ConfigureAwait(false);
            this.blobGeneratorMock.Verify(m => m.GenerateBlobsArrayAsync(It.IsAny<JArray>(), It.IsAny<TrueMessage>()), Times.Once);
            this.mockAzureClientFactory.Verify(m => m.GetQueueClient(It.IsAny<string>()), Times.Exactly(1));
        }

        /// <summary>
        /// Transforms the processor should return j object when transform inventory invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task TransformProcessor_ShouldReturnJObject_WhenTransformInventoryInvokedAsync()
        {
            var trueMessage = new TrueMessage
            {
                Message = MessageType.Inventory,
                SourceSystem = SystemType.SINOPER,
                TargetSystem = SystemType.TRUE,
                MessageId = "someId",
            };

            var jobject = this.GetInventoryJArray();
            this.blobGeneratorMock.Setup(m => m.GenerateBlobsArrayAsync(jobject, trueMessage)).Callback<JArray, TrueMessage>((array, message) =>
            {
                message.FileRegistration.FileRegistrationTransactions.Add(new FileRegistrationTransaction
                {
                    FileRegistrationId = 1,
                    BlobPath = "xml/inventory",
                });
            });
            this.mockAzureClientFactory.Setup(m => m.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);
            await this.dataService.SaveAsync(jobject, trueMessage).ConfigureAwait(false);
            this.blobGeneratorMock.Verify(m => m.GenerateBlobsArrayAsync(It.IsAny<JArray>(), It.IsAny<TrueMessage>()), Times.Once);
            this.mockAzureClientFactory.Verify(m => m.GetQueueClient(It.IsAny<string>()), Times.Exactly(1));
        }

        /// <summary>
        /// Transforms the processor should return for status type identifier failed when transform inventory invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task TransformProcessor_ShouldReturn_For_StatusTypeId_FAILED_WhenTransformInventoryInvokedAsync()
        {
            var trueMessage = new TrueMessage
            {
                Message = MessageType.Inventory,
                SourceSystem = SystemType.SINOPER,
                TargetSystem = SystemType.TRUE,
                MessageId = "someId",
            };

            var jobject = this.GetInventoryJArray();
            this.blobGeneratorMock.Setup(m => m.GenerateBlobsArrayAsync(jobject, trueMessage)).Callback<JArray, TrueMessage>((array, message) =>
            {
                message.FileRegistration.FileRegistrationTransactions.Add(new FileRegistrationTransaction
                {
                    FileRegistrationId = 1,
                    BlobPath = "xml/inventory",
                    StatusTypeId = StatusType.FAILED,
                });
            });
            this.mockAzureClientFactory.Setup(m => m.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);

            await this.dataService.SaveAsync(jobject, trueMessage).ConfigureAwait(false);

            this.blobGeneratorMock.Verify(m => m.GenerateBlobsArrayAsync(It.IsAny<JArray>(), It.IsAny<TrueMessage>()), Times.Once);
            this.mockAzureClientFactory.Verify(m => m.GetQueueClient(It.IsAny<string>()), Times.Never);
        }

        /// <summary>
        /// Transforms the processor should populate pending transactions for exceptions when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task TransformProcessor_ShouldPopulatePendingTransactions_For_Exceptions_WhenInvokedAsync()
        {
            var trueMessage = new TrueMessage
            {
                Message = MessageType.Inventory,
                SourceSystem = SystemType.SINOPER,
                TargetSystem = SystemType.TRUE,
                MessageId = "someId",
            };

            var jobject = this.GetInventoryJArray();
            this.blobGeneratorMock.Setup(m => m.GenerateBlobsArrayAsync(jobject, trueMessage)).Callback<JArray, TrueMessage>((array, message) =>
            {
                message.FileRegistration.FileRegistrationTransactions.Add(new FileRegistrationTransaction
                {
                    FileRegistrationId = 1,
                    BlobPath = "xml/inventory",
                });
            });
            this.mockAzureClientFactory.Setup(m => m.GetQueueClient(It.IsAny<string>())).Returns(() => null);

            await this.dataService.SaveAsync(jobject, trueMessage).ConfigureAwait(false);

            this.blobGeneratorMock.Verify(m => m.GenerateBlobsArrayAsync(It.IsAny<JArray>(), It.IsAny<TrueMessage>()), Times.Once);
            this.mockAzureClientFactory.Verify(m => m.GetQueueClient(It.IsAny<string>()), Times.Exactly(1));
        }

        /// <summary>
        /// Saves the excel asynchronous save excel entity when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveExcelAsync_SaveExcelEntity_WhenInvokedAsync()
        {
            // Arrange
            this.blobGeneratorMock.Setup(m => m.GenerateBlobsArrayAsync(It.IsAny<JArray>(), It.IsAny<TrueMessage>()));
            this.fileRegistrationSericeMock.Setup(m => m.UpdateFileRegistrationAsync(It.IsAny<FileRegistration>()));
            this.fileRegistrationSericeMock.Setup(m => m.InsertPendingTransactionsAsync(It.IsAny<ConcurrentBag<PendingTransaction>>()));
            var message = new TrueMessage() { FileRegistration = new FileRegistration(), PendingTransactions = new ConcurrentBag<PendingTransaction>() };

            // Act
            await this.dataService.SaveExcelAsync(new JArray(), new JArray(), new JArray(), new JArray(), message).ConfigureAwait(false);

            // Assert
            this.blobGeneratorMock.Verify(m => m.GenerateBlobsArrayAsync(It.IsAny<JArray>(), It.IsAny<TrueMessage>()), Times.Exactly(4));
            this.fileRegistrationSericeMock.Verify(m => m.UpdateFileRegistrationAsync(It.IsAny<FileRegistration>()), Times.Exactly(1));
            this.fileRegistrationSericeMock.Verify(m => m.InsertPendingTransactionsAsync(It.IsAny<ConcurrentBag<PendingTransaction>>()), Times.Exactly(1));
        }

        /// <summary>
        /// Saves the excel asynchronous should throw argument exception when message is null asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SaveExcelAsync_ShouldThrowArgumentException_WhenMessageIsNullAsync()
        {
            await this.dataService.SaveExcelAsync(new JArray(), new JArray(), new JArray(), new JArray(), null).ConfigureAwait(false);
        }

        /// <summary>
        /// Saves the external source entity array asynchronous should save entity when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveExternalSourceArrayAync_ShouldSaveEntity_WhenInvokedAsync()
        {
            // Arrange
            this.blobGeneratorMock.Setup(m => m.GenerateBlobsArrayAsync(It.IsAny<JArray>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            this.fileRegistrationSericeMock.Setup(m => m.InsertFileRegistrationAsync(It.IsAny<FileRegistration>()));
            this.mockAzureClientFactory.Setup(m => m.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);
            this.mockServiceBusQueueClient.Setup(m => m.QueueMessageAsync(It.IsAny<JsonQueueMessage>()));
            var message = new TrueMessage() { InputBlobPath = "InputBlobPath", MessageId = "UTMessageId", Message = MessageType.Movement, ShouldHomologate = false };
            var arr = JArray.Parse("[{\"SourceSystem\": \"SINOPER\"}]");

            // Act
            var result = await this.dataService.SaveExternalSourceEntityArrayAsync(arr, message).ConfigureAwait(false);

            // Assert
            this.blobGeneratorMock.Verify(m => m.GenerateBlobsArrayAsync(It.IsAny<JArray>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
            this.fileRegistrationSericeMock.Verify(m => m.InsertFileRegistrationAsync(It.IsAny<FileRegistration>()), Times.Exactly(1));
            this.mockAzureClientFactory.Verify(m => m.GetQueueClient(It.IsAny<string>()), Times.Exactly(1));
            this.mockServiceBusQueueClient.Verify(m => m.QueueMessageAsync(It.IsAny<JsonQueueMessage>()), Times.Exactly(1));
            Assert.AreEqual("UTMessageId", result);
        }

        /// <summary>
        /// Saves the external source entity asynchronous should save entity when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveExternalSourceEntityAsync_ShouldSaveEntity_WhenInvokedAsync()
        {
            // Arrange
            this.blobGeneratorMock.Setup(m => m.GenerateBlobsAsync(It.IsAny<JObject>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            this.fileRegistrationSericeMock.Setup(m => m.InsertFileRegistrationAsync(It.IsAny<FileRegistration>()));
            this.logisticMovementServiceMock.Setup(m => m.ProcessLogisticMovementAsync(It.IsAny<LogisticMovementResponse>())).Returns(Task.CompletedTask);
            this.mockAzureClientFactory.Setup(m => m.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);
            var message = new TrueMessage() { InputBlobPath = "InputBlobPath", MessageId = "UTMessageId", Message = MessageType.Logistic, ShouldHomologate = false };
            var obj = JObject.Parse("{\"SourceSystem\": \"SAP\"}");

            // Act
            var result = await this.dataService.SaveExternalSourceEntityAsync(obj, message).ConfigureAwait(false);

            // Assert
            this.blobGeneratorMock.Verify(m => m.GenerateBlobsAsync(It.IsAny<JObject>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
            this.fileRegistrationSericeMock.Verify(m => m.InsertFileRegistrationAsync(It.IsAny<FileRegistration>()), Times.Exactly(1));
            Assert.AreEqual("UTMessageId", result);
        }

        /// <summary>
        /// Saves the external source entity asynchronous should save entity when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveSapLogisticEntityAsync_ShouldSaveEntity_WhenInvokedAsync()
        {
            // Arrange
            this.blobGeneratorMock.Setup(m => m.GenerateBlobsAsync(It.IsAny<JObject>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            this.fileRegistrationSericeMock.Setup(m => m.InsertFileRegistrationAsync(It.IsAny<FileRegistration>()));
            this.logisticMovementServiceMock.Setup(m => m.ProcessLogisticMovementAsync(It.IsAny<LogisticMovementResponse>())).Returns(Task.CompletedTask);
            this.mockAzureClientFactory.Setup(m => m.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);
            var message = new TrueMessage() { InputBlobPath = "InputBlobPath", MessageId = "UTMessageId", Message = MessageType.Logistic, ShouldHomologate = false };
            var obj = JObject.Parse("{\"SourceSystem\": \"SAP\"}");

            // Act
            var result = await this.dataService.SaveLogisticEntityAsync(obj, message).ConfigureAwait(false);

            // Assert
            this.blobGeneratorMock.Verify(m => m.GenerateBlobsAsync(It.IsAny<JObject>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
            this.fileRegistrationSericeMock.Verify(m => m.InsertFileRegistrationAsync(It.IsAny<FileRegistration>()), Times.Exactly(1));
            Assert.AreEqual("UTMessageId", result);
        }

        /// <summary>
        /// Saves the external source array asynchronous should throw argument exception when entity is null asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SaveExternalSourceArrayAsync_ShouldThrowArgumentException_WhenEntityIsNullAsync()
        {
            await this.dataService.SaveExternalSourceEntityArrayAsync(null, It.IsAny<TrueMessage>()).ConfigureAwait(false);
        }

        /// <summary>
        /// Saves the external source array asynchronous should throw argument exception when message is null asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SaveExternalSourceArrayAsync_ShouldThrowArgumentException_WhenMessageIsNullAsync()
        {
            await this.dataService.SaveExternalSourceEntityArrayAsync(new JArray(), null).ConfigureAwait(false);
        }

        /// <summary>
        /// Saves the external source entity asynchronous should throw argument exception when entity is null asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SaveExternalSourceEntityAsync_ShouldThrowArgumentException_WhenEntityIsNullAsync()
        {
            await this.dataService.SaveExternalSourceEntityAsync(null, It.IsAny<TrueMessage>()).ConfigureAwait(false);
        }

        /// <summary>
        /// Saves the external source entity asynchronous should throw argument exception when message is null asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SaveExternalSourceEntityAsync_ShouldThrowArgumentException_WhenMessageIsNullAsync()
        {
            await this.dataService.SaveExternalSourceEntityAsync(new JObject(), null).ConfigureAwait(false);
        }

        /// <summary>
        /// Transforms the processor should return j object when transform movement invoked from sap asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task TransformProcessor_ShouldReturnJObject_WhenTransformMovementInvokedFromSapAsync()
        {
            // Arrange
            var trueMessage = new TrueMessage
            {
                Message = MessageType.Inventory,
                SourceSystem = SystemType.SAP,
                TargetSystem = SystemType.TRUE,
                MessageId = "someId",
            };

            var jobject = this.GetMovementJArray();
            this.blobGeneratorMock.Setup(m => m.GenerateBlobsArrayAsync(jobject, trueMessage)).Callback<JArray, TrueMessage>((array, message) =>
            {
                message.FileRegistration.FileRegistrationTransactions.Add(new FileRegistrationTransaction
                {
                    FileRegistrationId = 1,
                    BlobPath = "xml/movements",
                });
            });
            this.mockAzureClientFactory.Setup(m => m.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);
            this.mockServiceBusQueueClient.Setup(m => m.QueueScheduleMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));

            // Act
            await this.dataService.SaveAsync(jobject, trueMessage).ConfigureAwait(false);

            // Assert
            this.blobGeneratorMock.Verify(m => m.GenerateBlobsArrayAsync(It.IsAny<JArray>(), It.IsAny<TrueMessage>()), Times.Once);
            this.mockAzureClientFactory.Verify(m => m.GetQueueClient(It.IsAny<string>()), Times.Exactly(2));
        }
    }
}
