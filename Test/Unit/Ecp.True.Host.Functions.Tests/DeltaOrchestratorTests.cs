// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaOrchestratorTests.cs" company="Microsoft">
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
    using Ecp.True.Host.Functions.Delta;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.HandleFailure;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Azure.WebJobs.Extensions.Timers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The DeltaOrchestratorTests.
    /// </summary>
    [TestClass]
    public class DeltaOrchestratorTests
    {
        /// <summary>
        /// The logger mock.
        /// </summary>
        private readonly Mock<ITrueLogger<DeltaOrchestrator>> loggerMock = new Mock<ITrueLogger<DeltaOrchestrator>>();

        /// <summary>
        /// The service provider mock.
        /// </summary>
        private readonly Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();

        /// <summary>
        /// The durable orchestration client.
        /// </summary>
        private readonly Mock<IDurableOrchestrationClient> orchestrationClientMock = new Mock<IDurableOrchestrationClient>();

        /// <summary>
        /// The durable activity context.
        /// </summary>
        private readonly Mock<IDurableActivityContext> activityContextMock = new Mock<IDurableActivityContext>();

        /// <summary>
        /// The durable orchestration context mock.
        /// </summary>
        private readonly Mock<IDurableOrchestrationContext> durableOrchestrationContextMock = new Mock<IDurableOrchestrationContext>();

        /// <summary>
        /// Gets the mock SQL token provider.
        /// </summary>
        /// <value>
        /// The mock SQL token provider.
        /// </value>
        private readonly Mock<ISqlTokenProvider> mockSqlTokenProvider = new Mock<ISqlTokenProvider>();

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The configuration handler mock.
        /// </summary>
        private readonly Mock<IConfigurationHandler> configurationHandlerMock = new Mock<IConfigurationHandler>();

        /// <summary>
        /// The delta processor.
        /// </summary>
        private readonly Mock<IDeltaProcessor> deltaProcessor = new Mock<IDeltaProcessor>();

        /// <summary>
        /// The mock delta failure handler.
        /// </summary>
        private readonly Mock<IFailureHandler> mockDeltaFailureHandler = new Mock<IFailureHandler>();

        /// <summary>
        /// The failure handler mock.
        /// </summary>
        private readonly Mock<ITelemetry> telemetryMock = new Mock<ITelemetry>();

        /// <summary>
        /// The failure handler factory.
        /// </summary>
        private Mock<IFailureHandlerFactory> failureHandlerFactory;

        /// <summary>
        /// The ownership orchestrator.
        /// </summary>
        private DeltaOrchestrator orchestrator;

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
        /// Initialize the required set up.
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

            this.orchestrator = new DeltaOrchestrator(
                this.loggerMock.Object,
                this.telemetryMock.Object,
                this.configurationHandlerMock.Object,
                this.mockAzureClientFactory.Object,
                this.serviceProviderMock.Object,
                this.failureHandlerFactory.Object,
                this.mockUnitOfWorkFactory.Object,
                this.deltaProcessor.Object);
        }

        /// <summary>
        /// Calculates the delta asynchronous cut off or delta ticket exists when triggered asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task CalculateDeltaAsync_CutOffOrDeltaTicketExists_WhenTriggeredAsync()
        {
            var ticket = new Entities.TransportBalance.Ticket { TicketId = 1 };
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DeltaOrchestrator>())).ReturnsAsync("ticket");
            this.deltaProcessor.Setup(m => m.ValidateTicketAsync(It.IsAny<int>())).ReturnsAsync((true, ticket));

            await this.orchestrator.CalculateDeltaAsync(1, null, null, 1, this.orchestrationClientMock.Object, new Microsoft.Azure.WebJobs.ExecutionContext()).ConfigureAwait(false);

            this.deltaProcessor.Verify(m => m.ValidateTicketAsync(It.IsAny<int>()), Times.Once);
        }

        /// <summary>
        /// Calculates the delta asynchronous should handle failure for when triggered asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task CalculateDeltaAsync_ShouldReturn_ForTicketNotValid_WhenTriggeredAsync()
        {
            this.mockDeltaFailureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.failureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockDeltaFailureHandler.Object);
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DeltaOrchestrator>())).ReturnsAsync("ticket");
            this.deltaProcessor.Setup(m => m.ValidateTicketAsync(It.IsAny<int>())).ReturnsAsync((false, null));
            await this.orchestrator.CalculateDeltaAsync(1, null, null, 1, this.orchestrationClientMock.Object, new ExecutionContext()).ConfigureAwait(false);
            this.deltaProcessor.Verify(m => m.ValidateTicketAsync(It.IsAny<int>()), Times.Once);
            this.mockDeltaFailureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Never);
        }

        /// <summary>
        /// Calculates the delta asynchronous should handle failure for ticket not valid when triggered asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task CalculateDeltaAsync_ShouldHandleFailure_ForTicketNotValid_WhenTriggeredAsync()
        {
            var ticket = new Entities.TransportBalance.Ticket { TicketId = 1 };
            this.mockDeltaFailureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.failureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockDeltaFailureHandler.Object);
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DeltaOrchestrator>())).ReturnsAsync("ticket");
            this.deltaProcessor.Setup(m => m.ValidateTicketAsync(It.IsAny<int>())).ReturnsAsync((false, ticket));
            await this.orchestrator.CalculateDeltaAsync(1, null, null, 1, this.orchestrationClientMock.Object, new Microsoft.Azure.WebJobs.ExecutionContext()).ConfigureAwait(false);
            this.deltaProcessor.Verify(m => m.ValidateTicketAsync(It.IsAny<int>()), Times.Once);
            this.mockDeltaFailureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Once);
        }

        /// <summary>
        /// Calculates the delta asynchronous should handle failure for exception when triggered asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task CalculateDeltaAsync_ShouldHandleFailure_ForException_WhenTriggeredAsync()
        {
            this.mockDeltaFailureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.failureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockDeltaFailureHandler.Object);
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DeltaOrchestrator>())).ReturnsAsync("ticket");
            this.deltaProcessor.Setup(m => m.ValidateTicketAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            await this.orchestrator.CalculateDeltaAsync(1, null, null, 10, this.orchestrationClientMock.Object, new Microsoft.Azure.WebJobs.ExecutionContext()).ConfigureAwait(false);

            this.deltaProcessor.Verify(m => m.ValidateTicketAsync(It.IsAny<int>()), Times.Once);
            this.mockDeltaFailureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Once);
        }

        /// <summary>
        /// Calculates the delta asynchronous should throw exception for exception delivery count less than10 when triggered asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task CalculateDeltaAsync_ShouldThrowException_ForExceptionDeliveryCountLessThan10_WhenTriggeredAsync()
        {
            this.mockDeltaFailureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.failureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockDeltaFailureHandler.Object);
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DeltaOrchestrator>())).ReturnsAsync("ticket");
            this.deltaProcessor.Setup(m => m.ValidateTicketAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            await this.orchestrator.CalculateDeltaAsync(1, null, null, 1, this.orchestrationClientMock.Object, new Microsoft.Azure.WebJobs.ExecutionContext()).ConfigureAwait(false);
        }

        /// <summary>
        /// Deltas the orchestrator asynchronous when triggered asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DeltaOrchestratorAsync_WhenTriggeredAsync()
        {
            var deltaData = new DeltaData
            {
                Ticket = new Entities.TransportBalance.Ticket { TicketId = 1 },
            };

            this.durableOrchestrationContextMock.Setup(m => m.GetInput<DeltaData>()).Returns(deltaData);
            this.durableOrchestrationContextMock.Setup(x => x.CallActivityAsync<DeltaData>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(deltaData);

            await this.orchestrator.DeltaOrchestratorAsync(this.durableOrchestrationContextMock.Object).ConfigureAwait(false);

            this.durableOrchestrationContextMock.Verify(x => x.CallActivityAsync<DeltaData>(It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(4));
        }

        /// <summary>
        /// Deltas the orchestrator asynchronous should throw exception when triggered asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DeltaOrchestratorAsync_ShouldThrowException_WhenTriggeredAsync()
        {
            var deltaData = new DeltaData
            {
                Ticket = new Entities.TransportBalance.Ticket { TicketId = 1 },
            };

            this.durableOrchestrationContextMock.Setup(m => m.GetInput<DeltaData>()).Returns(deltaData);
            this.durableOrchestrationContextMock.Setup(x => x.CallActivityAsync<DeltaData>(It.IsAny<string>(), It.IsAny<object>())).Throws(new Exception());

            await this.orchestrator.DeltaOrchestratorAsync(this.durableOrchestrationContextMock.Object).ConfigureAwait(false);

            this.durableOrchestrationContextMock.Verify(x => x.CallActivityAsync<DeltaData>(It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(1));
        }

        /// <summary>
        /// Gets the delta asynchronous when triggered asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task GetDeltaAsync_WhenTriggeredAsync()
        {
            var deltaData = new DeltaData
            {
                Ticket = new Entities.TransportBalance.Ticket { TicketId = 1 },
            };

            this.activityContextMock.Setup(m => m.GetInput<DeltaData>()).Returns(deltaData);

            await this.orchestrator.GetDeltaAsync(this.activityContextMock.Object).ConfigureAwait(false);

            this.deltaProcessor.Verify(m => m.ProcessAsync(deltaData, ChainType.GetDelta), Times.Once);
        }

        /// <summary>
        /// Request the delta asynchronous when triggered asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task RequestDeltaAsync_WhenTriggeredAsync()
        {
            var deltaData = new DeltaData
            {
                Ticket = new Entities.TransportBalance.Ticket { TicketId = 1 },
            };

            this.activityContextMock.Setup(m => m.GetInput<DeltaData>()).Returns(deltaData);

            await this.orchestrator.RequestDeltaAsync(this.activityContextMock.Object).ConfigureAwait(false);

            this.deltaProcessor.Verify(m => m.ProcessAsync(deltaData, ChainType.RequestDelta), Times.Once);
        }

        /// <summary>
        /// Process the delta asynchronous when triggered asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ProcessDeltaAsync_WhenTriggeredAsync()
        {
            var deltaData = new DeltaData
            {
                Ticket = new Entities.TransportBalance.Ticket { TicketId = 1 },
            };

            this.activityContextMock.Setup(m => m.GetInput<DeltaData>()).Returns(deltaData);

            await this.orchestrator.ProcessDeltaAsync(this.activityContextMock.Object).ConfigureAwait(false);

            this.deltaProcessor.Verify(m => m.ProcessAsync(deltaData, ChainType.ProcessDelta), Times.Once);
        }

        /// <summary>
        /// Completes the delta asynchronous when triggered asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task CompleteDeltaAsync_WhenTriggeredAsync()
        {
            var deltaData = new DeltaData
            {
                Ticket = new Entities.TransportBalance.Ticket { TicketId = 1 },
            };

            this.activityContextMock.Setup(m => m.GetInput<DeltaData>()).Returns(deltaData);

            await this.orchestrator.CompleteDeltaAsync(this.activityContextMock.Object).ConfigureAwait(false);

            this.deltaProcessor.Verify(m => m.ProcessAsync(deltaData, ChainType.CompleteDelta), Times.Once);
        }

        [TestMethod]
        public async Task FinalizeDeltaAsync_ShouldFinalizeOwnershipForOwnershipAsync_forTicketAsync()
        {
            var deltaData = new DeltaData
            {
                Ticket = new Entities.TransportBalance.Ticket { TicketId = 1 },
            };

            this.activityContextMock.Setup(m => m.GetInput<DeltaData>()).Returns(deltaData);
            await this.orchestrator.FinalizeDeltaAsync(this.activityContextMock.Object).ConfigureAwait(false);
            this.deltaProcessor.Verify(m => m.FinalizeProcessAsync(deltaData.Ticket.TicketId), Times.Once);
        }

        /// <summary>
        /// Handles the failure asynchronous when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task HandleFailureAsync_WhenTriggeredAsync()
        {
            var deltaData = new DeltaData
            {
                Ticket = new Entities.TransportBalance.Ticket { TicketId = 1 },
            };

            Tuple<DeltaData, string> tuple = new Tuple<DeltaData, string>(deltaData, "Some error");

            this.mockDeltaFailureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.failureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockDeltaFailureHandler.Object);
            this.activityContextMock.Setup(m => m.GetInput<Tuple<DeltaData, string>>()).Returns(tuple);

            await this.orchestrator.HandleFailureAsync(this.activityContextMock.Object).ConfigureAwait(false);

            this.mockDeltaFailureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Once);
        }

        /// <summary>
        /// Completes the delta history when triggered asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task PurgeDeltaHistoryAsync()
        {
            var deltaData = new DeltaData
            {
                Ticket = new Entities.TransportBalance.Ticket { TicketId = 1 },
            };
            var timerInfo = new TimerInfo(null, new ScheduleStatus(), false);
            this.activityContextMock.Setup(m => m.GetInput<DeltaData>()).Returns(deltaData);

            await this.orchestrator.PurgeDeltaHistoryAsync(timerInfo, this.orchestrationClientMock.Object, new ExecutionContext()).ConfigureAwait(false);
            this.deltaProcessor.Verify(m => m.ProcessAsync(deltaData, It.IsAny<ChainType>()), Times.Never);
        }
    }
}
