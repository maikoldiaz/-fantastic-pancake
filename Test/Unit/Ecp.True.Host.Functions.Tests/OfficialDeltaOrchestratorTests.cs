// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaOrchestratorTests.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Chaos.Interfaces;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Delta;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.HandleFailure;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Azure.WebJobs.Extensions.Timers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The ConsolidationOrchestratorTests.
    /// </summary>
    [TestClass]
    public class OfficialDeltaOrchestratorTests
    {
        /// <summary>
        /// The logger mock.
        /// </summary>
        private readonly Mock<ITrueLogger<OfficialDeltaOrchestrator>> loggerMock = new Mock<ITrueLogger<OfficialDeltaOrchestrator>>();

        /// <summary>
        /// The durable orchestration client.
        /// </summary>
        private readonly Mock<IDurableOrchestrationClient> orchestrationClientMock = new Mock<IDurableOrchestrationClient>();

        /// <summary>
        /// The durable orchestration context mock.
        /// </summary>
        private readonly Mock<IDurableOrchestrationContext> durableOrchestrationContextMock = new Mock<IDurableOrchestrationContext>();

        /// <summary>
        /// The service provider mock.
        /// </summary>
        private readonly Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();

        /// <summary>
        /// The mock delta failure handler.
        /// </summary>
        private readonly Mock<IFailureHandler> mockDeltaFailureHandler = new Mock<IFailureHandler>();

        /// <summary>
        /// The durable activity context.
        /// </summary>
        private readonly Mock<IDurableActivityContext> activityContextMock = new Mock<IDurableActivityContext>();

        /// <summary>
        /// Gets the mock SQL token provider.
        /// </summary>
        /// <value>
        /// The mock SQL token provider.
        /// </value>
        private readonly Mock<ISqlTokenProvider> mockSqlTokenProvider = new Mock<ISqlTokenProvider>();

        /// <summary>
        /// The configuration handler mock.
        /// </summary>
        private readonly Mock<IConfigurationHandler> configurationHandlerMock = new Mock<IConfigurationHandler>();

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The consolidation processor.
        /// </summary>
        private readonly Mock<IOfficialDeltaProcessor> mockDeltaProcessor = new Mock<IOfficialDeltaProcessor>();

        /// <summary>
        /// The failure handler factory.
        /// </summary>
        private Mock<IFailureHandlerFactory> failureHandlerFactory;

        /// <summary>
        /// The mock chaos manager.
        /// </summary>
        private Mock<IChaosManager> mockChaosManager;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The consolidation orchestrator.
        /// </summary>
        private OfficialDeltaOrchestrator orchestrator;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            var connectionFactoryMock = new Mock<IConnectionFactory>();
            var azureClientFactory = new Mock<IAzureClientFactory>();
            connectionFactoryMock.Setup(m => m.IsReady).Returns(true);
            azureClientFactory.Setup(m => m.IsReady).Returns(true);
            this.mockChaosManager = new Mock<IChaosManager>();
            this.failureHandlerFactory = new Mock<IFailureHandlerFactory>();
            this.serviceProviderMock.Setup(m => m.GetService(typeof(IConnectionFactory))).Returns(connectionFactoryMock.Object);
            this.serviceProviderMock.Setup(m => m.GetService(typeof(IAzureClientFactory))).Returns(azureClientFactory.Object);
            this.serviceProviderMock.Setup(m => m.GetService(typeof(ISqlTokenProvider))).Returns(this.mockSqlTokenProvider.Object);
            this.serviceProviderMock.Setup(r => r.GetService(typeof(IChaosManager))).Returns(this.mockChaosManager.Object);
            this.serviceProviderMock.Setup(s => s.GetService(typeof(ITrueLogger<DeltaOrchestrator>))).Returns(this.loggerMock.Object);
            this.failureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockDeltaFailureHandler.Object);
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockUnitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);

            this.orchestrator = new OfficialDeltaOrchestrator(
                this.loggerMock.Object,
                this.configurationHandlerMock.Object,
                this.mockAzureClientFactory.Object,
                this.serviceProviderMock.Object,
                this.failureHandlerFactory.Object,
                this.mockUnitOfWorkFactory.Object,
                this.mockDeltaProcessor.Object);
        }

        /// <summary>
        /// Official delta asynchronous should process when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialDeltaAsync_ShouldProcess_WhenTriggeredAsync()
        {
            var ticket = new Ticket { TicketId = 1 };
            var officialDeltaData = new OfficialDeltaData();
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DeltaOrchestrator>())).ReturnsAsync("ticket");
            this.mockDeltaProcessor.Setup(m => m.ValidateTicketAsync(It.IsAny<int>())).ReturnsAsync((true, ticket, string.Empty));
            this.mockDeltaProcessor.Setup(m => m.BuildOfficialDataAsync(It.IsAny<OfficialDeltaData>())).ReturnsAsync(officialDeltaData);

            await this.orchestrator.OfficialDeltaAsync(1, null, null, 1, this.orchestrationClientMock.Object, new ExecutionContext()).ConfigureAwait(false);

            this.mockDeltaProcessor.Verify(m => m.ValidateTicketAsync(It.IsAny<int>()), Times.Once);
        }

        /// <summary>
        ///  official delta asynchronous should return for ticket not valid when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialDeltaAsync_ShouldReturn_ForTicketNotValid_WhenTriggeredAsync()
        {
            this.mockDeltaFailureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.failureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockDeltaFailureHandler.Object);
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<OfficialDeltaOrchestrator>())).ReturnsAsync("ticket");
            this.mockDeltaProcessor.Setup(m => m.ValidateTicketAsync(It.IsAny<int>())).ReturnsAsync((false, null, "Invalid ticket"));

            await this.orchestrator.OfficialDeltaAsync(1, null, null, 1, this.orchestrationClientMock.Object, new ExecutionContext()).ConfigureAwait(false);

            this.mockDeltaProcessor.Verify(m => m.ValidateTicketAsync(It.IsAny<int>()), Times.Once);
            this.mockDeltaFailureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Never);
        }

        /// <summary>
        /// Official delta  asynchronous should handle failure for ticket not valid when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialDeltaAsync_ShouldHandleFailure_ForTicketNotValid_WhenTriggeredAsync()
        {
            var ticket = new Ticket { TicketId = 1 };
            this.mockDeltaFailureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.failureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockDeltaFailureHandler.Object);
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<OfficialDeltaOrchestrator>())).ReturnsAsync("ticket");
            this.mockDeltaProcessor.Setup(m => m.ValidateTicketAsync(It.IsAny<int>())).ReturnsAsync((false, ticket, "Invalid ticket"));

            await this.orchestrator.OfficialDeltaAsync(1, null, null, 1, this.orchestrationClientMock.Object, new ExecutionContext()).ConfigureAwait(false);

            this.mockDeltaProcessor.Verify(m => m.ValidateTicketAsync(It.IsAny<int>()), Times.Once);
            this.mockDeltaFailureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Once);
        }

        /// <summary>
        /// Official delta asynchronous should handle failure for exception when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialDeltaAsync_ShouldHandleFailure_ForException_WhenTriggeredAsync()
        {
            this.mockDeltaFailureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.failureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockDeltaFailureHandler.Object);
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DeltaOrchestrator>())).ReturnsAsync("ticket");
            this.mockDeltaProcessor.Setup(m => m.ValidateTicketAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            await this.orchestrator.OfficialDeltaAsync(1, null, null, 10, this.orchestrationClientMock.Object, new ExecutionContext()).ConfigureAwait(false);

            this.mockDeltaProcessor.Verify(m => m.ValidateTicketAsync(It.IsAny<int>()), Times.Once);
            this.mockDeltaFailureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Once);
        }

        /// <summary>
        /// Official delta asynchronous should throw exception for exception delivery count less than10 when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task ConsolidateOfficialDeltaAsync_ShouldThrowException_ForExceptionDeliveryCountLessThan10_WhenTriggeredAsync()
        {
            this.mockDeltaFailureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.failureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockDeltaFailureHandler.Object);
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DeltaOrchestrator>())).ReturnsAsync("ticket");
            this.mockDeltaProcessor.Setup(m => m.ValidateTicketAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            await this.orchestrator.OfficialDeltaAsync(1, null, null, 1, this.orchestrationClientMock.Object, new ExecutionContext()).ConfigureAwait(false);
        }

        /// <summary>
        /// The data orchestrator asynchronous should call buildData activity when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task DataOrchestratorAsync_ShouldCallBuildActivity_WhenTriggeredAsync()
        {
            var ticket = new Ticket { TicketId = 1 };
            var officialDeltaData = new OfficialDeltaData
            {
                Ticket = ticket,
            };

            this.durableOrchestrationContextMock.Setup(m => m.GetInput<OfficialDeltaData>()).Returns(officialDeltaData);

            await this.orchestrator.OfficialDeltaDataOrchestratorAsync(this.durableOrchestrationContextMock.Object).ConfigureAwait(false);

            this.durableOrchestrationContextMock.Verify(x => x.CallActivityAsync<OfficialDeltaData>(It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(1));
        }

        /// <summary>
        /// Handles the failure asynchronous when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task HandleFailureAsync_WhenTriggeredAsync()
        {
            var ticket = new Ticket { TicketId = 1 };
            var officialDeltaData = new OfficialDeltaData
            {
                Ticket = ticket,
            };

            Tuple<OfficialDeltaData, string> tuple = new Tuple<OfficialDeltaData, string>(officialDeltaData, "Some error");

            this.mockDeltaFailureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.failureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockDeltaFailureHandler.Object);
            this.activityContextMock.Setup(m => m.GetInput<Tuple<OfficialDeltaData, string>>()).Returns(tuple);

            await this.orchestrator.HandleOfficialDeltaFailureAsync(this.activityContextMock.Object).ConfigureAwait(false);

            this.mockDeltaFailureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Once);
        }

        /// <summary>
        /// Build Data  when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BuildDataAsync_WhenTriggeredAsync()
        {
            var ticket = new Ticket { TicketId = 1 };
            var officialDeltaData = new OfficialDeltaData
            {
                Ticket = ticket,
            };

            this.activityContextMock.Setup(m => m.GetInput<OfficialDeltaData>()).Returns(officialDeltaData);

            await this.orchestrator.BuildOfficialDataAsync(this.activityContextMock.Object).ConfigureAwait(false);

            this.mockDeltaProcessor.Verify(a => a.BuildOfficialDataAsync(It.IsAny<OfficialDeltaData>()), Times.Once);
        }

        /// <summary>
        /// Exclude Data  when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExcludeDataAsync_WhenTriggeredAsync()
        {
            var ticket = new Ticket { TicketId = 1 };
            var officialDeltaData = new OfficialDeltaData
            {
                Ticket = ticket,
            };

            this.activityContextMock.Setup(m => m.GetInput<OfficialDeltaData>()).Returns(officialDeltaData);

            await this.orchestrator.ExcludeDataAsync(this.activityContextMock.Object).ConfigureAwait(false);

            this.mockDeltaProcessor.Verify(a => a.ExcludeDataAsync(It.IsAny<OfficialDeltaData>()), Times.Once);
        }

        /// <summary>
        /// Exclude Data  when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task RequestOfficialDeltaAsync_WhenTriggeredAsync()
        {
            var ticket = new Ticket { TicketId = 1 };
            var officialDeltaData = new OfficialDeltaData
            {
                Ticket = ticket,
            };

            this.activityContextMock.Setup(m => m.GetInput<OfficialDeltaData>()).Returns(officialDeltaData);

            await this.orchestrator.RequestOfficialDeltaAsync(this.activityContextMock.Object).ConfigureAwait(false);

            this.mockDeltaProcessor.Verify(a => a.ProcessAsync(It.IsAny<OfficialDeltaData>(), It.IsAny<ChainType>()), Times.Once);
        }

        /// <summary>
        /// Exclude Data  when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ProcessOfficialDeltaAsync_WhenTriggeredAsync()
        {
            var ticket = new Ticket { TicketId = 1 };
            var officialDeltaData = new OfficialDeltaData
            {
                Ticket = ticket,
            };

            this.activityContextMock.Setup(m => m.GetInput<OfficialDeltaData>()).Returns(officialDeltaData);

            await this.orchestrator.ProcessOfficialDeltaAsync(this.activityContextMock.Object).ConfigureAwait(false);

            this.mockDeltaProcessor.Verify(a => a.ProcessAsync(It.IsAny<OfficialDeltaData>(), It.IsAny<ChainType>()), Times.Once);
        }

        /// <summary>
        /// Exclude Data  when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task RegisterMovementsOfficialDeltaAsync_WhenTriggeredAsync()
        {
            var ticket = new Ticket { TicketId = 1 };
            var officialDeltaData = new OfficialDeltaData
            {
                Ticket = ticket,
            };

            this.activityContextMock.Setup(m => m.GetInput<OfficialDeltaData>()).Returns(officialDeltaData);

            await this.orchestrator.RegisterMovementsOfficialDeltaAsync(this.activityContextMock.Object).ConfigureAwait(false);

            this.mockDeltaProcessor.Verify(a => a.ProcessAsync(It.IsAny<OfficialDeltaData>(), It.IsAny<ChainType>()), Times.Once);
        }

        /// <summary>
        /// Exclude Data  when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CalculateOfficialDeltaAsync_WhenTriggeredAsync()
        {
            var ticket = new Ticket { TicketId = 1 };
            var officialDeltaData = new OfficialDeltaData
            {
                Ticket = ticket,
            };

            this.activityContextMock.Setup(m => m.GetInput<OfficialDeltaData>()).Returns(officialDeltaData);

            await this.orchestrator.CalculateOfficialDeltaAsync(this.activityContextMock.Object).ConfigureAwait(false);

            this.mockDeltaProcessor.Verify(a => a.ProcessAsync(It.IsAny<OfficialDeltaData>(), It.IsAny<ChainType>()), Times.Once);
        }

        /// <summary>
        /// Exclude Data  when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CompleteOfficialDeltaAsync_WhenTriggeredAsync()
        {
            var ticket = new Ticket { TicketId = 1 };
            var officialDeltaData = new OfficialDeltaData
            {
                Ticket = ticket,
            };

            this.activityContextMock.Setup(m => m.GetInput<OfficialDeltaData>()).Returns(officialDeltaData);

            await this.orchestrator.CompleteOfficialDeltaAsync(this.activityContextMock.Object).ConfigureAwait(false);

            this.mockDeltaProcessor.Verify(a => a.FinalizeProcessAsync(It.IsAny<int>()), Times.Once);
        }

        /// <summary>
        /// Completes the Ownership history when triggered asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task PurgeOfficialDeltaHistoryAsync_WhenTriggeredAsync()
        {
            var ticket = new Ticket { TicketId = 1 };
            var officialDeltaData = new OfficialDeltaData
            {
                Ticket = ticket,
            };

            var timerInfo = new TimerInfo(null, new ScheduleStatus(), false);
            this.activityContextMock.Setup(m => m.GetInput<OfficialDeltaData>()).Returns(officialDeltaData);

            await this.orchestrator.PurgeOfficialDeltaHistoryAsync(timerInfo, this.orchestrationClientMock.Object, new Microsoft.Azure.WebJobs.ExecutionContext()).ConfigureAwait(false);
            this.mockDeltaProcessor.Verify(m => m.ProcessAsync(officialDeltaData, It.IsAny<ChainType>()), Times.Never);
        }

        /// <summary>
        /// Register Node  when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task RegisterDataAsync_WhenTriggeredAsync()
        {
            var ticket = new Ticket { TicketId = 1 };
            var officialDeltaData = new OfficialDeltaData
            {
                Ticket = ticket,
            };

            this.activityContextMock.Setup(m => m.GetInput<OfficialDeltaData>()).Returns(officialDeltaData);

            await this.orchestrator.RegisterNodeActivityAsync(this.activityContextMock.Object).ConfigureAwait(false);

            this.mockDeltaProcessor.Verify(a => a.RegisterAsync(It.IsAny<OfficialDeltaData>()), Times.Once);
        }
    }
}
