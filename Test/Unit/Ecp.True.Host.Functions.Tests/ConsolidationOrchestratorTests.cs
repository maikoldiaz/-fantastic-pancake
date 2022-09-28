// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidationOrchestratorTests.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Chaos.Interfaces;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Delta;
    using Ecp.True.Logging;
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
    /// The ConsolidationOrchestratorTests.
    /// </summary>
    [TestClass]
    public class ConsolidationOrchestratorTests
    {
        /// <summary>
        /// The logger mock.
        /// </summary>
        private readonly Mock<ITrueLogger<ConsolidationOrchestrator>> loggerMock = new Mock<ITrueLogger<ConsolidationOrchestrator>>();

        /// <summary>
        /// The durable orchestration client.
        /// </summary>
        private readonly Mock<IDurableOrchestrationClient> orchestrationClientMock = new Mock<IDurableOrchestrationClient>();

        /// <summary>
        /// The durable activity context.
        /// </summary>
        private readonly Mock<IDurableActivityContext> activityContextMock = new Mock<IDurableActivityContext>();

        /// <summary>
        /// The service provider mock.
        /// </summary>
        private readonly Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();

        /// <summary>
        /// The mock delta failure handler.
        /// </summary>
        private readonly Mock<IFailureHandler> mockDeltaFailureHandler = new Mock<IFailureHandler>();

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
        private readonly Mock<IConsolidationProcessor> consolidationProcessor = new Mock<IConsolidationProcessor>();

        /// <summary>
        /// The durable orchestration context mock.
        /// </summary>
        private readonly Mock<IDurableOrchestrationContext> durableOrchestrationContextMock = new Mock<IDurableOrchestrationContext>();

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
        private ConsolidationOrchestrator orchestrator;

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

            this.orchestrator = new ConsolidationOrchestrator(
                this.loggerMock.Object,
                this.configurationHandlerMock.Object,
                this.mockAzureClientFactory.Object,
                this.serviceProviderMock.Object,
                this.failureHandlerFactory.Object,
                this.mockUnitOfWorkFactory.Object,
                this.consolidationProcessor.Object);
        }

        /// <summary>
        /// Consolidates the official delta asynchronous should process when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ConsolidateOfficialDeltaAsync_ShouldProcess_WhenTriggeredAsync()
        {
            var ticket = new Ticket { TicketId = 1 };
            var consolidationBatches = new List<ConsolidationBatch>();
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DeltaOrchestrator>())).ReturnsAsync("ticket");
            this.consolidationProcessor.Setup(m => m.ValidateTicketAsync(It.IsAny<int>())).ReturnsAsync((true, ticket, string.Empty));
            this.consolidationProcessor.Setup(m => m.GetConsolidationBatchesAsync(It.IsAny<Ticket>())).ReturnsAsync(consolidationBatches);

            await this.orchestrator.ConsolidateOfficialDeltaAsync(1, null, null, 1, this.orchestrationClientMock.Object, new ExecutionContext()).ConfigureAwait(false);

            this.consolidationProcessor.Verify(m => m.ValidateTicketAsync(It.IsAny<int>()), Times.Once);
            this.consolidationProcessor.Verify(m => m.GetConsolidationBatchesAsync(It.IsAny<Ticket>()), Times.Once);
        }

        /// <summary>
        /// Consolidates the official delta asynchronous should return for ticket not valid when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ConsolidateOfficialDeltaAsync_ShouldReturn_ForTicketNotValid_WhenTriggeredAsync()
        {
            this.mockDeltaFailureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.failureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockDeltaFailureHandler.Object);
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ConsolidationOrchestrator>())).ReturnsAsync("ticket");
            this.consolidationProcessor.Setup(m => m.ValidateTicketAsync(It.IsAny<int>())).ReturnsAsync((false, null, "Invalid ticket"));

            await this.orchestrator.ConsolidateOfficialDeltaAsync(1, null, null, 1, this.orchestrationClientMock.Object, new ExecutionContext()).ConfigureAwait(false);

            this.consolidationProcessor.Verify(m => m.ValidateTicketAsync(It.IsAny<int>()), Times.Once);
            this.mockDeltaFailureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Never);
        }

        /// <summary>
        /// Consolidates the official delta asynchronous should handle failure for ticket not valid when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ConsolidateOfficialDeltaAsync_ShouldHandleFailure_ForTicketNotValid_WhenTriggeredAsync()
        {
            var ticket = new Ticket { TicketId = 1 };
            this.mockDeltaFailureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.failureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockDeltaFailureHandler.Object);
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ConsolidationOrchestrator>())).ReturnsAsync("ticket");
            this.consolidationProcessor.Setup(m => m.ValidateTicketAsync(It.IsAny<int>())).ReturnsAsync((false, ticket, "Invalid ticket"));

            await this.orchestrator.ConsolidateOfficialDeltaAsync(1, null, null, 1, this.orchestrationClientMock.Object, new ExecutionContext()).ConfigureAwait(false);

            this.consolidationProcessor.Verify(m => m.ValidateTicketAsync(It.IsAny<int>()), Times.Once);
            this.mockDeltaFailureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Once);
        }

        /// <summary>
        /// Consolidates the official delta asynchronous should handle failure for exception when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ConsolidateOfficialDeltaAsync_ShouldHandleFailure_ForException_WhenTriggeredAsync()
        {
            this.mockDeltaFailureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.failureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockDeltaFailureHandler.Object);
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DeltaOrchestrator>())).ReturnsAsync("ticket");
            this.consolidationProcessor.Setup(m => m.ValidateTicketAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            await this.orchestrator.ConsolidateOfficialDeltaAsync(1, null, null, 10, this.orchestrationClientMock.Object, new ExecutionContext()).ConfigureAwait(false);

            this.consolidationProcessor.Verify(m => m.ValidateTicketAsync(It.IsAny<int>()), Times.Once);
            this.mockDeltaFailureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Once);
        }

        /// <summary>
        /// Consolidates the official delta asynchronous should throw exception for exception delivery count less than10 when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task ConsolidateOfficialDeltaAsync_ShouldThrowException_ForExceptionDeliveryCountLessThan10_WhenTriggeredAsync()
        {
            this.mockDeltaFailureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.failureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockDeltaFailureHandler.Object);
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DeltaOrchestrator>())).ReturnsAsync("ticket");
            this.consolidationProcessor.Setup(m => m.ValidateTicketAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            await this.orchestrator.ConsolidateOfficialDeltaAsync(1, null, null, 1, this.orchestrationClientMock.Object, new ExecutionContext()).ConfigureAwait(false);
        }

        /// <summary>
        /// Consolidations the data orchestrator asynchronous should call consolidate activity when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ConsolidationDataOrchestratorAsync_ShouldCallConsolidateActivity_WhenTriggeredAsync()
        {
            var ticket = new Ticket { TicketId = 1 };
            var consolidationData = new ConsolidationData
            {
                Ticket = ticket,
                Batches = new List<ConsolidationBatch> { new ConsolidationBatch { Ticket = ticket } },
            };

            this.durableOrchestrationContextMock.Setup(m => m.GetInput<ConsolidationData>()).Returns(consolidationData);
            this.durableOrchestrationContextMock.Setup(x => x.CallActivityAsync<ConsolidationData>(It.IsAny<string>(), It.IsAny<object>()));

            await this.orchestrator.ConsolidationDataOrchestratorAsync(this.durableOrchestrationContextMock.Object).ConfigureAwait(false);

            this.durableOrchestrationContextMock.Verify(x => x.CallActivityAsync<ConsolidationData>(It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(1));
        }

        /// <summary>
        /// Consolidations the data orchestrator asynchronous should handle failure for any exception when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ConsolidationDataOrchestratorAsync_ShouldHandleFailure_ForAnyException_WhenTriggeredAsync()
        {
            var ticket = new Ticket { TicketId = 1 };
            var consolidationData = new ConsolidationData
            {
                Ticket = ticket,
                Batches = new List<ConsolidationBatch> { new ConsolidationBatch { Ticket = ticket } },
            };

            this.durableOrchestrationContextMock.Setup(m => m.GetInput<ConsolidationData>()).Returns(consolidationData);
            this.durableOrchestrationContextMock.Setup(x => x.CallActivityAsync<ConsolidationData>(It.IsAny<string>(), It.IsAny<object>())).ThrowsAsync(new Exception());

            await this.orchestrator.ConsolidationDataOrchestratorAsync(this.durableOrchestrationContextMock.Object).ConfigureAwait(false);

            this.durableOrchestrationContextMock.Verify(x => x.CallActivityAsync<ConsolidationData>(It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(1));
        }

        /// <summary>
        /// Consolidates the asynchronous should call consolidate processor when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ConsolidateAsync_ShouldCallConsolidateProcessor_WhenTriggeredAsync()
        {
            var ticket = new Ticket { TicketId = 1 };
            var consolidationData = new ConsolidationData
            {
                Ticket = ticket,
                Batches = new List<ConsolidationBatch> { new ConsolidationBatch { Ticket = ticket } },
            };

            Tuple<ConsolidationBatch, ConsolidationData> tuple = new Tuple<ConsolidationBatch, ConsolidationData>(consolidationData.Batches.First(), consolidationData);

            this.activityContextMock.Setup(m => m.GetInput<Tuple<ConsolidationBatch, ConsolidationData>>()).Returns(tuple);
            this.consolidationProcessor.Setup(x => x.ConsolidateAsync(It.IsAny<ConsolidationBatch>()));

            await this.orchestrator.ConsolidateAsync(this.activityContextMock.Object).ConfigureAwait(false);

            this.consolidationProcessor.Verify(x => x.ConsolidateAsync(It.IsAny<ConsolidationBatch>()), Times.Once);
        }

        /// <summary>
        /// Completes the consolidation asynchronous should complete consolidation when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CompleteConsolidationAsync_ShouldCompleteConsolidation_WhenTriggeredAsync()
        {
            var ticket = new Ticket { TicketId = 1 };
            var consolidationData = new ConsolidationData
            {
                Ticket = ticket,
                Batches = new List<ConsolidationBatch> { new ConsolidationBatch { Ticket = ticket } },
            };

            this.activityContextMock.Setup(m => m.GetInput<ConsolidationData>()).Returns(consolidationData);
            this.consolidationProcessor.Setup(x => x.CompleteConsolidationAsync(It.IsAny<int>(), It.IsAny<int>()));

            await this.orchestrator.CompleteConsolidationAsync(this.activityContextMock.Object).ConfigureAwait(false);

            this.consolidationProcessor.Verify(x => x.CompleteConsolidationAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        /// <summary>
        /// Handles the failure asynchronous when triggered asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task HandleFailureAsync_WhenTriggeredAsync()
        {
            var ticket = new Ticket { TicketId = 1 };
            var consolidationData = new ConsolidationData
            {
                Ticket = ticket,
                Batches = new List<ConsolidationBatch> { new ConsolidationBatch { Ticket = ticket } },
            };

            Tuple<ConsolidationData, string> tuple = new Tuple<ConsolidationData, string>(consolidationData, "Some error");

            this.mockDeltaFailureHandler.Setup(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()));
            this.failureHandlerFactory.Setup(a => a.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.mockDeltaFailureHandler.Object);
            this.activityContextMock.Setup(m => m.GetInput<Tuple<ConsolidationData, string>>()).Returns(tuple);

            await this.orchestrator.HandleConsolidationFailureAsync(this.activityContextMock.Object).ConfigureAwait(false);

            this.mockDeltaFailureHandler.Verify(a => a.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Once);
        }

        /// <summary>
        /// Purges the official delta history asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task PurgeConsolidatedDataHistoryAsync()
        {
            var ticket = new Ticket { TicketId = 1 };
            var consolidationData = new ConsolidationData
            {
                Ticket = ticket,
                Batches = new List<ConsolidationBatch> { new ConsolidationBatch { Ticket = ticket } },
            };
            var timerInfo = new TimerInfo(null, new ScheduleStatus(), false);
            this.activityContextMock.Setup(m => m.GetInput<ConsolidationData>()).Returns(consolidationData);

            await this.orchestrator.PurgeConsolidatedDataHistoryAsync(timerInfo, this.orchestrationClientMock.Object, new ExecutionContext()).ConfigureAwait(false);
            this.consolidationProcessor.Verify(m => m.ConsolidateAsync(It.IsAny<ConsolidationBatch>()), Times.Never);
        }
    }
}
