// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeadLetterListenerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Tests
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Chaos.Interfaces;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Host.Functions.Deadletter;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.HandleFailure;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Deadletter.Interfaces;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The deadletter listner test.
    /// </summary>
    [TestClass]
    public class DeadLetterListenerTests
    {
        /// <summary>
        /// The failure handler mock.
        /// </summary>
        private readonly Mock<IFailureHandler> cutoffFailureHandlerMock = new Mock<IFailureHandler>();

        /// <summary>
        /// The failure handler mock.
        /// </summary>
        private readonly Mock<IFailureHandler> ownershipFailureHandlerMock = new Mock<IFailureHandler>();

        /// <summary>
        /// The failure handler mock.
        /// </summary>
        private readonly Mock<IFailureHandler> logisticsFailureHandlerMock = new Mock<IFailureHandler>();

        /// <summary>
        /// The official delta failure handler mock.
        /// </summary>
        private readonly Mock<IFailureHandler> officialDeltaFailureHandlerMock = new Mock<IFailureHandler>();

        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly Mock<ITelemetry> mockTelemetry = new Mock<ITelemetry>();

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        private readonly Mock<ITrueLogger<DeadletterProcessor>> mockLogger = new Mock<ITrueLogger<DeadletterProcessor>>();

        /// <summary>
        /// The Deadletter processor.
        /// </summary>
        private readonly Mock<IDeadletterProcessor> mockDeadletterProcessor = new Mock<IDeadletterProcessor>();

        /// <summary>
        /// The mock service provider.
        /// </summary>
        private readonly Mock<IServiceProvider> mockServiceProvider = new Mock<IServiceProvider>();

        /// <summary>
        /// The mock data generator service.
        /// </summary>
        private readonly Mock<IDataGeneratorService> mockDataGeneratorService = new Mock<IDataGeneratorService>();

        /// <summary>
        /// The mock ownership calculation service.
        /// </summary>
        private readonly Mock<IOwnershipCalculationService> mockOwnershipCalculationService = new Mock<IOwnershipCalculationService>();

        /// <summary>
        /// The failure handler mock.
        /// </summary>
        private readonly Mock<IFailureHandler> deltaFailureHandlerMock = new Mock<IFailureHandler>();

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The mock connection factory.
        /// </summary>
        private Mock<IConnectionFactory> mockConnectionFactory = new Mock<IConnectionFactory>();

        /// <summary>
        /// The mock configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigHandler = new Mock<IConfigurationHandler>();

        /// <summary>
        /// Gets the mock SQL token provider.
        /// </summary>
        /// <value>
        /// The mock SQL token provider.
        /// </value>
        private Mock<ISqlTokenProvider> mockSqlTokenProvider = new Mock<ISqlTokenProvider>();

        /// <summary>
        /// The mock chaos manager.
        /// </summary>
        private Mock<IChaosManager> mockChaosManager = new Mock<IChaosManager>();

        /// <summary>
        /// The deadletter listener.
        /// </summary>
        private DeadletterProcessor processor;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The failure handler factory.
        /// </summary>
        private Mock<IFailureHandlerFactory> failureHandlerFactory;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockUnitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.failureHandlerFactory = new Mock<IFailureHandlerFactory>();

            this.processor = new DeadletterProcessor(this.mockServiceProvider.Object, this.mockLogger.Object, this.mockTelemetry.Object, this.mockDeadletterProcessor.Object, this.failureHandlerFactory.Object, this.mockUnitOfWorkFactory.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetDeadletterMessage_NullContext_ShouldThrowExceptionAsync()
        {
            this.SetupMocks(true);
            await this.processor.GetDeadletteredMessageAsync(null, "label", null, null).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task GetDeadletterMessage_NullProcessName_ShouldFailAsync()
        {
            this.SetupMocks(true);

            var deadletter = new DeadletteredMessage
            {
                ProcessName = "Movement",
                QueueName = string.Empty,
                DeadletteredMessageId = 1,
            };
            await this.processor.GetDeadletteredMessageAsync(deadletter, "label", null, new ExecutionContext()).ConfigureAwait(false);
            this.mockDeadletterProcessor.Verify(a => a.ProcessAsync(deadletter), Times.Once);
        }

        [TestMethod]
        public async Task GetDeadletterMessage_ShouldBeSuccessfullPAsync()
        {
            this.SetupMocks(true);

            var deadletter = new DeadletteredMessage
            {
                ProcessName = "Movement",
                QueueName = "Movement",
                DeadletteredMessageId = 1,
            };
            await this.processor.GetDeadletteredMessageAsync(deadletter, "label", null, new ExecutionContext()).ConfigureAwait(false);
            this.mockDeadletterProcessor.Verify(a => a.ProcessAsync(deadletter), Times.Once);
        }

        /// <summary>
        /// Processes the file registration retry deadlettering asynchronous should process message when invoked asynchronous.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ProcessFileRegistrationRetryDeadletteringAsync_ShouldProcessMessage_WhenIsRetry_True_WhenInvokedAsync()
        {
            // Arrange
            this.SetupMocks(true);
            this.mockDeadletterProcessor.Setup(m => m.HandleRegistrationRetryFailureAsync(It.IsAny<string>()));
            var message = new JObject
            {
                { "FileRegistrationTransactionId", 1 },
                { "IsRetry", true },
            };

            // Act
            await this.processor.ProcessFileRegistrationRetryDeadletteringAsync(message.ToString()).ConfigureAwait(false);

            // Assert
            this.mockDeadletterProcessor.Verify(m => m.HandleRegistrationRetryFailureAsync(It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Processes the file registration retry deadlettering asynchronous should process message when is retry false when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ProcessFileRegistrationRetryDeadletteringAsync_ShouldProcessMessage_WhenIsRetry_False__WhenInvokedAsync()
        {
            // Arrange
            this.SetupMocks(true);
            this.mockDeadletterProcessor.Setup(m => m.HandleRegistrationFailureAsync(It.IsAny<string>()));
            var message = new JObject
            {
                { "UploadId", "123" },
            };

            // Act
            await this.processor.ProcessFileRegistrationRetryDeadletteringAsync(message.ToString()).ConfigureAwait(false);

            // Assert
            this.mockDeadletterProcessor.Verify(m => m.HandleRegistrationFailureAsync(It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Processes the file registration retry deadlettering asynchronous should json serialization exception when context is null asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ProcessFileRegistrationRetryDeadletteringAsync_ShouldJsonSerializationException_WhenContextIsNullAsync()
        {
            this.SetupMocks(true);
            var invalidData = 123;
            await this.processor.ProcessFileRegistrationRetryDeadletteringAsync(invalidData.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            this.mockLogger.Verify(a => a.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        /// <summary>
        /// Processes the excel movement and inventory deadlettering asynchronous should process message when invoked asynchronous.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ProcessExcelMovementAndInventoryDeadletteringAsync_ShouldProcessMessage_WhenInvokedAsync()
        {
            // Arrange
            this.SetupMocks(true);
            this.mockDeadletterProcessor.Setup(m => m.HandleRegistrationFailureAsync(It.IsAny<string>()));
            var message = new JObject
            {
                { "UploadId", 1 },
                { "UploadFileId", 1 },
                { "ActionType", 1 },
            };

            // Act
            await this.processor.ProcessExcelMovementAndInventoryDeadletteringAsync(message.ToString()).ConfigureAwait(false);

            // Assert
            this.mockDeadletterProcessor.Verify(m => m.HandleRegistrationFailureAsync(It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Processes the excel movement and inventory deadlettering asynchronous should json serialization exception when context is null asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ProcessExcelMovementAndInventoryDeadletteringAsync_ShouldJsonSerializationException_WhenContextIsNullAsync()
        {
            this.SetupMocks(true);
            var invalidData = 123;
            await this.processor.ProcessExcelMovementAndInventoryDeadletteringAsync(invalidData.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            this.mockLogger.Verify(a => a.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        /// <summary>
        /// Processes the excel event deadlettering asynchronous should process message when invoked asynchronous.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ProcessExcelEventDeadletteringAsync_ShouldProcessMessage_WhenInvokedAsync()
        {
            // Arrange
            this.SetupMocks(true);
            this.mockDeadletterProcessor.Setup(m => m.HandleRegistrationFailureAsync(It.IsAny<string>()));
            var message = new JObject
            {
                { "UploadId", 1 },
                { "UploadFileId", 1 },
                { "ActionType", 1 },
            };

            // Act
            await this.processor.ProcessExcelEventDeadletteringAsync(message.ToString()).ConfigureAwait(false);

            // Assert
            this.mockDeadletterProcessor.Verify(m => m.HandleRegistrationFailureAsync(It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Processes the excel event deadlettering asynchronous should json serialization exception when context is null asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ProcessExcelEventDeadletteringAsync_ShouldJsonSerializationException_WhenContextIsNullAsync()
        {
            this.SetupMocks(true);
            var invalidData = 123;
            await this.processor.ProcessExcelEventDeadletteringAsync(invalidData.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            this.mockLogger.Verify(a => a.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        /// <summary>
        /// Processes the excel contract deadlettering asynchronous should process message when invoked asynchronous.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ProcessExcelContractDeadletteringAsync_ShouldProcessMessage_WhenInvokedAsync()
        {
            // Arrange
            this.SetupMocks(true);
            this.mockDeadletterProcessor.Setup(m => m.HandleRegistrationFailureAsync(It.IsAny<string>()));
            var message = new JObject
            {
                { "UploadId", 1 },
                { "UploadFileId", 1 },
                { "ActionType", 1 },
            };

            // Act
            await this.processor.ProcessExcelContractDeadletteringAsync(message.ToString()).ConfigureAwait(false);

            // Assert
            this.mockDeadletterProcessor.Verify(m => m.HandleRegistrationFailureAsync(It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Processes the excel contract deadlettering asynchronous should json serialization exception when context is null asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ProcessExcelContractDeadletteringAsync_ShouldJsonSerializationException_WhenContextIsNullAsync()
        {
            this.SetupMocks(true);
            var invalidData = 123;
            await this.processor.ProcessExcelContractDeadletteringAsync(invalidData.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            this.mockLogger.Verify(a => a.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        /// <summary>
        /// Processes the re calculate ownership deadlettering asynchronous should process message when invoked asynchronous.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ProcessReCalculateOwnershipDeadletteringAsync_ShouldProcessMessage_WhenInvokedAsync()
        {
            // Arrange
            this.SetupMocks(true);
            this.mockDeadletterProcessor.Setup(m => m.ProcessAsync(It.IsAny<DeadletteredMessage>()));

            // Act
            await this.processor.ProcessReCalculateOwnershipDeadletteringAsync(new Entities.Dto.RecalculateOwnershipMessage(1, false)).ConfigureAwait(false);

            // Assert
            this.mockDeadletterProcessor.Verify(m => m.ProcessAsync(It.IsAny<DeadletteredMessage>()), Times.Once);
        }

        /// <summary>
        /// Processes the logistics deadlettering asynchronous should process message when invoked asynchronous.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ProcessLogisticsDeadletteringAsync_ShouldProcessMessage_WhenInvokedAsync()
        {
            // Arrange
            this.SetupMocks(true);
            this.logisticsFailureHandlerMock.SetupGet(x => x.TicketType).Returns(TicketType.Logistics);
            this.failureHandlerFactory.Setup(m => m.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.logisticsFailureHandlerMock.Object);

            // Assert
            var queueMessage = new Entities.Core.QueueMessage
            {
                TicketId = 12345,
            };

            // Act
            await this.processor.ProcessLogisticsDeadletteringAsync(JsonConvert.SerializeObject(queueMessage)).ConfigureAwait(false);

            // Assert
            this.failureHandlerFactory.Verify(m => m.GetFailureHandler(It.IsAny<TicketType>()), Times.Once);
            this.logisticsFailureHandlerMock.Verify(x => x.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Once);
        }

        /// <summary>
        /// Processes the operational cutoff handle failure asynchronous should process message when invoked asynchronous.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ProcessOperationalCutoffHandleFailureAsync_ShouldProcessMessage_WhenInvokedAsync()
        {
            // Arrange
            this.SetupMocks(true);
            this.cutoffFailureHandlerMock.SetupGet(x => x.TicketType).Returns(TicketType.Cutoff);
            this.failureHandlerFactory.Setup(m => m.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.cutoffFailureHandlerMock.Object);

            // Act
            await this.processor.ProcessOperationalCutoffDeadletteringAsync(1).ConfigureAwait(false);

            // Assert
            this.failureHandlerFactory.Verify(m => m.GetFailureHandler(It.IsAny<TicketType>()), Times.Once);
        }

        /// <summary>
        /// Processes the ownership handle failure asynchronous should process message when invoked asynchronous.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ProcessOwnershipHandleFailureAsync_ShouldProcessMessage_WhenInvokedAsync()
        {
            // Arrange
            this.SetupMocks(true);
            this.ownershipFailureHandlerMock.SetupGet(x => x.TicketType).Returns(TicketType.Ownership);
            this.failureHandlerFactory.Setup(m => m.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.ownershipFailureHandlerMock.Object);

            // Act
            await this.processor.ProcessOwnershipDeadletteringAsync(1).ConfigureAwait(false);

            // Assert
            this.failureHandlerFactory.Verify(m => m.GetFailureHandler(It.IsAny<TicketType>()), Times.Once);
        }

        /// <summary>
        /// Processes the ownership handle failure asynchronous should process message when invoked with false asynchronous.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ProcessOwnershipHandleFailureAsync_ShouldProcessMessage_WhenInvokedWithFalseAsync()
        {
            // Arrange
            this.SetupMocks(false);
            this.ownershipFailureHandlerMock.SetupGet(x => x.TicketType).Returns(TicketType.Ownership);
            this.failureHandlerFactory.Setup(m => m.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.ownershipFailureHandlerMock.Object);

            // Act
            await this.processor.ProcessOwnershipDeadletteringAsync(1).ConfigureAwait(false);

            // Assert
            this.failureHandlerFactory.Verify(m => m.GetFailureHandler(It.IsAny<TicketType>()), Times.Once);
        }

        /// <summary>
        /// Processes the delta handle failure asynchronous should process message when invoked asynchronous.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ProcessDeltaHandleFailureAsync_ShouldProcessMessage_WhenInvokedAsync()
        {
            // Arrange
            this.SetupMocks(true);
            this.deltaFailureHandlerMock.SetupGet(x => x.TicketType).Returns(TicketType.Delta);
            this.failureHandlerFactory.Setup(m => m.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.deltaFailureHandlerMock.Object);

            // Act
            await this.processor.ProcessDeltaDeadletteringAsync(1).ConfigureAwait(false);

            // Assert
            this.failureHandlerFactory.Verify(m => m.GetFailureHandler(It.IsAny<TicketType>()), Times.Once);
        }

        /// <summary>
        /// Processes the sap deadlettering asynchronous should process message when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProcessSapDeadletteringAsync_ShouldProcessMessage_WhenInvokedAsync()
        {
            // Arrange
            this.SetupMocks(true);
            var sapQueueMessage = new SapQueueMessage(SapRequestType.Upload, "test");
            this.mockDeadletterProcessor.Setup(m => m.HandleSapFailureAsync(sapQueueMessage));

            // Act
            await this.processor.ProcessSapDeadletteringAsync(sapQueueMessage).ConfigureAwait(false);

            // Assert
            this.mockDeadletterProcessor.Verify(m => m.HandleSapFailureAsync(sapQueueMessage), Times.Once);
        }

        [TestMethod]
        public async Task ProcessSapDeadletteringAsync_WhenErrorAsync()
        {
            // Arrange
            this.SetupMocks(true);
            var sapQueueMessage = new SapQueueMessage(SapRequestType.Upload, "test");
            this.mockDeadletterProcessor.Setup(m => m.HandleSapFailureAsync(sapQueueMessage)).Throws(new Exception());

            // Act
            await this.processor.ProcessSapDeadletteringAsync(sapQueueMessage).ConfigureAwait(false);

            // Assert
            this.mockDeadletterProcessor.Verify(m => m.HandleSapFailureAsync(sapQueueMessage), Times.Once);
        }

        /// <summary>
        /// Processes the consolidated data deadlettering asynchronous should process message when invoked with false asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProcessConsolidatedDataDeadletteringAsync()
        {
            // Arrange
            this.SetupMocks(false);
            this.officialDeltaFailureHandlerMock.SetupGet(x => x.TicketType).Returns(TicketType.OfficialDelta);
            this.failureHandlerFactory.Setup(m => m.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.officialDeltaFailureHandlerMock.Object);

            // Act
            await this.processor.ProcessConsolidatedDataDeadletteringAsync(1).ConfigureAwait(false);

            // Assert
            this.failureHandlerFactory.Verify(m => m.GetFailureHandler(It.IsAny<TicketType>()), Times.Once);
        }

        /// <summary>
        /// Processes the consolidated data deadlettering asynchronous should process message when invoked with false asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProcessConsolidatedDataDeadlettering_WhenErrorAsync()
        {
            // Arrange
            this.SetupMocks(false);
            this.officialDeltaFailureHandlerMock.SetupGet(x => x.TicketType).Returns(TicketType.OfficialDelta);
            this.failureHandlerFactory.Setup(m => m.GetFailureHandler(It.IsAny<TicketType>())).Throws(new Exception());

            // Act
            await this.processor.ProcessConsolidatedDataDeadletteringAsync(0).ConfigureAwait(false);

            // Assert
            this.failureHandlerFactory.Verify(m => m.GetFailureHandler(It.IsAny<TicketType>()), Times.Once);
        }

        /// <summary>
        /// Processes the excel event deadlettering asynchronous should process message when invoked asynchronous.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ProcessBeforeCutoffDeadletteringAsync_ShouldProcessMessage_WhenInvokedAsync()
        {
            // Arrange
            this.SetupMocks(true);
            this.mockDeadletterProcessor.Setup(m => m.HandleReportFailureAsync(1, ReportType.BeforeCutOff));

            // Act
            await this.processor.ProcessBeforeCutoffDeadletteringAsync(1).ConfigureAwait(false);

            // Assert
            this.mockDeadletterProcessor.Verify(m => m.HandleReportFailureAsync(1, ReportType.BeforeCutOff), Times.Once);
        }

        /// <summary>
        /// Processes the excel event deadlettering asynchronous should process message when invoked asynchronous.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ProcessOfficialInitialBalanceDeadletteringAsync_ShouldProcessMessage_WhenInvokedAsync()
        {
            // Arrange
            this.SetupMocks(true);
            this.mockDeadletterProcessor.Setup(m => m.HandleReportFailureAsync(1, ReportType.OfficialInitialBalance));

            // Act
            await this.processor.ProcessOfficialInitialBalanceDeadletteringAsync(1).ConfigureAwait(false);

            // Assert
            this.mockDeadletterProcessor.Verify(m => m.HandleReportFailureAsync(1, ReportType.OfficialInitialBalance), Times.Once);
        }

        /// <summary>
        /// Processes the excel event deadlettering asynchronous should process message when invoked asynchronous.
        /// </summary>
        /// <returns>Task representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ProcessOperativeBalanceDeadletteringAsync_ShouldProcessMessage_WhenInvokedAsync()
        {
            // Arrange
            this.SetupMocks(true);
            this.mockDeadletterProcessor.Setup(m => m.HandleReportFailureAsync(1, ReportType.OperativeBalance));

            // Act
            await this.processor.ProcessOperativeBalanceDeadletteringAsync(1).ConfigureAwait(false);

            // Assert
            this.mockDeadletterProcessor.Verify(m => m.HandleReportFailureAsync(1, ReportType.OperativeBalance), Times.Once);
        }

        /// <summary>
        /// Setups the mocks.
        /// </summary>
        /// <param name="isReady">if set to <c>true</c> [is ready].</param>
        private void SetupMocks(bool isReady)
        {
            this.mockConnectionFactory = new Mock<IConnectionFactory>();
            this.mockConfigHandler = new Mock<IConfigurationHandler>();
            this.mockAzureClientFactory = new Mock<IAzureClientFactory>();
            this.mockSqlTokenProvider = new Mock<ISqlTokenProvider>();
            this.mockChaosManager = new Mock<IChaosManager>();

            this.mockConnectionFactory.Setup(m => m.SetupStorageConnection(It.IsAny<string>()));
            this.mockConnectionFactory.SetupGet(m => m.IsReady).Returns(isReady);

            this.mockAzureClientFactory.SetupGet(m => m.IsReady).Returns(isReady);
            this.mockAzureClientFactory.Setup(m => m.Initialize(It.IsAny<AzureConfiguration>()));

            this.mockConfigHandler.Setup(m => m.GetConfigurationAsync<SqlConnectionSettings>(ConfigurationConstants.SqlConnectionSettings)).ReturnsAsync(new SqlConnectionSettings());

            this.mockServiceProvider.Setup(s => s.GetService(typeof(IConnectionFactory))).Returns(this.mockConnectionFactory.Object);

            this.mockServiceProvider.Setup(s => s.GetService(typeof(IConnectionFactory))).Returns(this.mockConnectionFactory.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IConfigurationHandler))).Returns(this.mockConfigHandler.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IAzureClientFactory))).Returns(this.mockAzureClientFactory.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IDataGeneratorService))).Returns(this.mockDataGeneratorService.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IOwnershipCalculationService))).Returns(this.mockOwnershipCalculationService.Object);
            this.mockServiceProvider.Setup(r => r.GetService(typeof(ISqlTokenProvider))).Returns(this.mockSqlTokenProvider.Object);
            this.mockServiceProvider.Setup(r => r.GetService(typeof(IChaosManager))).Returns(this.mockChaosManager.Object);
        }
    }
}