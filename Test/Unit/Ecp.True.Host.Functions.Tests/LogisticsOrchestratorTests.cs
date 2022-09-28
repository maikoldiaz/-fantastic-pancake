// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogisticsOrchestratorTests.cs" company="Microsoft">
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
    using System.Data.SqlClient;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;
    using Ecp.True.Chaos.Interfaces;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Host.Functions.Ownership;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SqlConstants = Ecp.True.DataAccess.Sql.Constants;

    [TestClass]
    public class LogisticsOrchestratorTests
    {
        /// <summary>
        /// The logger mock.
        /// </summary>
        private readonly Mock<ITrueLogger<LogisticsOrchestrator>> loggerMock = new Mock<ITrueLogger<LogisticsOrchestrator>>();

        /// <summary>
        /// The service provider mock.
        /// </summary>
        private readonly Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The configuration handler mock.
        /// </summary>
        private readonly Mock<IConfigurationHandler> configurationHandlerMock = new Mock<IConfigurationHandler>();

        /// <summary>
        /// The failure handler mock.
        /// </summary>
        private readonly Mock<IFailureHandler> logisticsFailureHandlerMock = new Mock<IFailureHandler>();

        /// <summary>
        /// Gets the mock SQL token provider.
        /// </summary>
        /// <value>
        /// The mock SQL token provider.
        /// </value>
        private readonly Mock<ISqlTokenProvider> mockSqlTokenProvider = new Mock<ISqlTokenProvider>();

        /// <summary>
        /// The durable orchestration context mock.
        /// </summary>
        private readonly Mock<IDurableOrchestrationContext> durableOrchestrationContextMock = new Mock<IDurableOrchestrationContext>();

        /// <summary>
        /// The durable orchestration context mock.
        /// </summary>
        private readonly Mock<IDurableOrchestrationClient> orchestrationClientMock = new Mock<IDurableOrchestrationClient>();

        /// <summary>
        /// The durable activity context.
        /// </summary>
        private readonly Mock<IDurableActivityContext> activityContextMock = new Mock<IDurableActivityContext>();

        /// <summary>
        /// The logistics processor.
        /// </summary>
        private readonly Mock<ILogisticsProcessor> logisticsProcessor = new Mock<ILogisticsProcessor>();

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The failure handler factory.
        /// </summary>
        private Mock<IFailureHandlerFactory> failureHandlerFactory;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The orchestrator.
        /// </summary>
        private LogisticsOrchestrator orchestrator;

        /// <summary>
        /// The mock chaos manager.
        /// </summary>
        private Mock<IChaosManager> mockChaosManager;

        /// <summary>
        /// The mock chaos manager.
        /// </summary>
        private Mock<IOwnershipCalculationService> ownershipCalculationService;

        [TestInitialize]
        public void Initialize()
        {
            var connectionFactoryMock = new Mock<IConnectionFactory>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            connectionFactoryMock.Setup(m => m.IsReady).Returns(true);
            var azureClientFactory = new Mock<IAzureClientFactory>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockChaosManager = new Mock<IChaosManager>();
            this.ownershipCalculationService = new Mock<IOwnershipCalculationService>();
            var analysisSettings = new AnalysisSettings
            {
                ClientId = "ClientID",
                ClientSecret = "ClientSecret",
                ModelName = "ModelName",
                Region = "Region",
                ServerName = "ServerName",
                TenantId = "TenantID",
            };
            this.failureHandlerFactory = new Mock<IFailureHandlerFactory>();
            this.serviceProviderMock.Setup(m => m.GetService(typeof(IConnectionFactory))).Returns(connectionFactoryMock.Object);
            this.serviceProviderMock.Setup(m => m.GetService(typeof(IAzureClientFactory))).Returns(azureClientFactory.Object);
            this.serviceProviderMock.Setup(m => m.GetService(typeof(ISqlTokenProvider))).Returns(this.mockSqlTokenProvider.Object);
            this.serviceProviderMock.Setup(r => r.GetService(typeof(IChaosManager))).Returns(this.mockChaosManager.Object);
            this.serviceProviderMock.Setup(r => r.GetService(typeof(IOwnershipCalculationService))).Returns(this.ownershipCalculationService.Object);
            this.configurationHandlerMock.Setup(m => m.GetConfigurationAsync<AnalysisSettings>(ConfigurationConstants.AnalysisSettings))
              .ReturnsAsync(analysisSettings);
            this.failureHandlerFactory.Setup(m => m.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.logisticsFailureHandlerMock.Object);

            this.mockUnitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.orchestrator = new LogisticsOrchestrator(
                this.loggerMock.Object,
                this.configurationHandlerMock.Object,
                this.mockAzureClientFactory.Object,
                this.serviceProviderMock.Object,
                this.logisticsProcessor.Object,
                this.ownershipCalculationService.Object);
        }

        /// <summary>
        /// The calculate balance should get the ticket by ID when triggered asynchronously.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialLogisticsOrchestratorAsync_WhenTriggeredAsync()
        {
            var logisticsOrchestratorData = new LogisticsOrchestratorData { Ticket = new Ticket { TicketId = 1, }, SystemType = 6, };
            logisticsOrchestratorData.Ticket = new Entities.TransportBalance.Ticket();
            logisticsOrchestratorData.Ticket.TicketId = 1;
            this.durableOrchestrationContextMock.Setup(m => m.GetInput<LogisticsOrchestratorData>()).Returns(logisticsOrchestratorData);
            await this.orchestrator.OfficialLogisticsOrchestratorAsync(this.durableOrchestrationContextMock.Object).ConfigureAwait(false);
            this.durableOrchestrationContextMock.Verify(m => m.GetInput<LogisticsOrchestratorData>(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task OfficialLogisticsOrchestratorAsync_ReturnException_WhenTriggeredAsync()
        {
            var logisticsOrchestratorData = new LogisticsOrchestratorData();
            this.durableOrchestrationContextMock.Setup(m => m.GetInput<LogisticsOrchestratorData>()).Returns(logisticsOrchestratorData);
            await this.orchestrator.OfficialLogisticsOrchestratorAsync(this.durableOrchestrationContextMock.Object).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task GenerateLogisticsAsync_WhenTriggeredAsync()
        {
            var logisticsOrchestratorData = new LogisticsOrchestratorData
            {
                Ticket = new Ticket { TicketId = 1 },
                SystemType = (int?)SystemType.SIV,
            };
            this.logisticsProcessor.Setup(l => l.GenerateOfficialLogisticsAsync(It.IsAny<Ticket>(), It.IsAny<int>()));
            this.activityContextMock.Setup(m => m.GetInput<LogisticsOrchestratorData>()).Returns(logisticsOrchestratorData);
            await this.orchestrator.GenerateLogisticsAsync(this.activityContextMock.Object).ConfigureAwait(false);
            this.activityContextMock.Verify(m => m.GetInput<LogisticsOrchestratorData>(), Times.Once);
            this.logisticsProcessor
                .Verify(l => l.GenerateOfficialLogisticsAsync(logisticsOrchestratorData.Ticket, logisticsOrchestratorData.SystemType.GetValueOrDefault()), Times.Once);
        }

        [TestMethod]
        public async Task HandleFailureAsync_ShouldHandleFailureForOwnershipAsync_forTicketAsync()
        {
            this.logisticsFailureHandlerMock.SetupGet(x => x.TicketType).Returns(TicketType.OfficialLogistics);
            this.activityContextMock.Setup(m => m.GetInput<Tuple<LogisticsOrchestratorData, string>>()).Returns(Tuple.Create(new LogisticsOrchestratorData() { Ticket = new Ticket { TicketId = 1 } }, "ErrorMessage"));
            await this.orchestrator.HandleFailureAsync(this.activityContextMock.Object).ConfigureAwait(false);
        }

        /// <summary>
        /// Generates the logistics asynchronous should generate logistics excel asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CalculateLogisticAsyncc_ShouldGenerateLogisticsExcelAsync()
        {
            var queueMessage = new QueueMessage { TicketId = 1, CorrelationId = "2345678g", SystemTypeId = 1 };
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<LogisticsOrchestrator>())).ReturnsAsync("ticket");
            await this.orchestrator.CalculateLogisticAsync(queueMessage, null, null, 1, this.orchestrationClientMock.Object, new Microsoft.Azure.WebJobs.ExecutionContext()).ConfigureAwait(false);
        }

        /// <summary>
        /// Generates the logistics asynchronous should generate logistics excel asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CalculateLogisticAsyncc_ShouldGenerateLogisticsExceptionAsync()
        {
            var queueMessage = new QueueMessage { TicketId = 1, CorrelationId = "2345678g", SystemTypeId = 1 };
            this.logisticsProcessor.Setup(l => l.GetTicketAsync(It.IsAny<int>())).Throws(new Exception());
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<LogisticsOrchestrator>())).Throws(new Exception());
            await this.orchestrator.CalculateLogisticAsync(queueMessage, null, null, 11, this.orchestrationClientMock.Object, new Microsoft.Azure.WebJobs.ExecutionContext()).ConfigureAwait(false);
            this.logisticsProcessor
            .Verify(l => l.GetTicketAsync(1), Times.Once);
        }

        /// <summary>
        /// Generates the logistics asynchronous should generate logistics Sql Exeption asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CalculateLogisticAsyncc_ShouldNoSapHomologationForMovementTypeAsync()
        {
            var sqlException = FormatterServices.GetUninitializedObject(typeof(SqlException)) as SqlException;
            var messageField = typeof(SqlException).GetField("_message", BindingFlags.NonPublic | BindingFlags.Instance);
            messageField.SetValue(sqlException, SqlConstants.NoSapHomologationForMovementType);
            var queueMessage = new QueueMessage { TicketId = 1, CorrelationId = "2345678g", SystemTypeId = 1 };
            this.logisticsProcessor.Setup(l => l.GetTicketAsync(It.IsAny<int>())).Throws(sqlException);
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<LogisticsOrchestrator>())).Throws(new Exception());
            await this.orchestrator.CalculateLogisticAsync(queueMessage, null, null, 11, this.orchestrationClientMock.Object, new Microsoft.Azure.WebJobs.ExecutionContext()).ConfigureAwait(false);
            this.logisticsProcessor
            .Verify(l => l.GetTicketAsync(1), Times.Once);
        }

        /// <summary>
        /// Calculate the logistics asynchronous sInvalidCombinationToSivMovement asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CalculateLogisticAsyncc_ShouldInvalidCombinationToSivMovementnAsync()
        {
            var sqlException = FormatterServices.GetUninitializedObject(typeof(SqlException)) as SqlException;
            var messageField = typeof(SqlException).GetField("_message", BindingFlags.NonPublic | BindingFlags.Instance);
            messageField.SetValue(sqlException, SqlConstants.InvalidCombinationToSivMovement);
            var queueMessage = new QueueMessage { TicketId = 1, CorrelationId = "2345678g", SystemTypeId = 1 };
            this.logisticsProcessor.Setup(l => l.GetTicketAsync(It.IsAny<int>())).Throws(sqlException);
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<LogisticsOrchestrator>())).Throws(new Exception());
            await this.orchestrator.CalculateLogisticAsync(queueMessage, null, null, 11, this.orchestrationClientMock.Object, new Microsoft.Azure.WebJobs.ExecutionContext()).ConfigureAwait(false);
            this.logisticsProcessor
            .Verify(l => l.GetTicketAsync(1), Times.Once);
        }

        /// <summary>
        /// Calculate the logistics asynchronous SqlException asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CalculateLogisticAsyncc_ShouldSqlExceptionAsync()
        {
            var sqlException = FormatterServices.GetUninitializedObject(typeof(SqlException)) as SqlException;
            var queueMessage = new QueueMessage { TicketId = 1, CorrelationId = "2345678g", SystemTypeId = 1 };
            this.logisticsProcessor.Setup(l => l.GetTicketAsync(It.IsAny<int>())).Throws(sqlException);
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<LogisticsOrchestrator>())).Throws(new Exception());
            await this.orchestrator.CalculateLogisticAsync(queueMessage, null, null, 11, this.orchestrationClientMock.Object, new Microsoft.Azure.WebJobs.ExecutionContext()).ConfigureAwait(false);
            this.logisticsProcessor
            .Verify(l => l.GetTicketAsync(1), Times.Once);
        }
    }
}
