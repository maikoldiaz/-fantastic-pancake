// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipOrchestratorTests.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Chaos.Interfaces;
    using Ecp.True.Configuration;
    using Ecp.True.Core.Entities;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Host.Functions.Ownership;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Conciliation.Interfaces;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.HandleFailure;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Azure.WebJobs.Extensions.Timers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The operational cut off orchestrator tests.
    /// </summary>
    [TestClass]
    public class OwnershipOrchestratorTests
    {
        /// <summary>
        /// The logger mock.
        /// </summary>
        private readonly Mock<ITrueLogger<OwnershipOrchestrator>> loggerMock = new Mock<ITrueLogger<OwnershipOrchestrator>>();

        /// <summary>
        /// The service provider mock.
        /// </summary>
        private readonly Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();

        /// <summary>
        /// The ownership rule processor.
        /// </summary>
        private readonly Mock<IOwnershipRuleProcessor> ownershipRuleProcessor = new Mock<IOwnershipRuleProcessor>();

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
        /// The failure handler mock.
        /// </summary>
        private readonly Mock<IFailureHandler> ownershipFailureHandlerMock = new Mock<IFailureHandler>();

        /// <summary>
        /// conciliation processor mock.
        /// </summary>
        private readonly Mock<IConciliationProcessor> conciliationProcessorMock = new Mock<IConciliationProcessor>();

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
        private OwnershipOrchestrator orchestrator;

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
        /// The mock of FunctionBase.
        /// </summary>
        private Mock<ITrueLogger<FunctionBase>> mockFunctionBaseLogger;

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
            this.mockFunctionBaseLogger = new Mock<ITrueLogger<FunctionBase>>();
            this.failureHandlerFactory = new Mock<IFailureHandlerFactory>();
            this.serviceProviderMock.Setup(m => m.GetService(typeof(IConnectionFactory))).Returns(connectionFactoryMock.Object);
            this.serviceProviderMock.Setup(m => m.GetService(typeof(IAzureClientFactory))).Returns(azureClientFactory.Object);
            this.serviceProviderMock.Setup(m => m.GetService(typeof(ISqlTokenProvider))).Returns(this.mockSqlTokenProvider.Object);
            this.serviceProviderMock.Setup(r => r.GetService(typeof(IChaosManager))).Returns(this.mockChaosManager.Object);
            this.serviceProviderMock.Setup(s => s.GetService(typeof(ITrueLogger<OwnershipOrchestrator>))).Returns(this.loggerMock.Object);
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockUnitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            var analysisSettings = new AnalysisSettings
            {
                ClientId = "ClientID",
                ClientSecret = "ClientSecret",
                ModelName = "ModelName",
                Region = "Region",
                ServerName = "ServerName",
                TenantId = "TenantID",
            };

            this.configurationHandlerMock.Setup(m => m.GetConfigurationAsync<AnalysisSettings>(ConfigurationConstants.AnalysisSettings))
                .ReturnsAsync(analysisSettings);

            this.failureHandlerFactory.Setup(m => m.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.ownershipFailureHandlerMock.Object);
            this.orchestrator = new OwnershipOrchestrator(
                this.loggerMock.Object,
                this.telemetryMock.Object,
                this.configurationHandlerMock.Object,
                this.mockAzureClientFactory.Object,
                this.serviceProviderMock.Object,
                this.ownershipRuleProcessor.Object,
                this.conciliationProcessorMock.Object,
                this.failureHandlerFactory.Object,
                this.mockUnitOfWorkFactory.Object);
        }

        /// <summary>
        /// The calculate balance should get the ticket by ID when triggered asynchronously.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CalculateOwnershipsAsync_WhenTriggeredAsync()
        {
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<OwnershipOrchestrator>())).ReturnsAsync("ticket");
            this.ownershipRuleProcessor.Setup(m => m.CleanOwnershipDataAsync(It.IsAny<int>()));
            await this.orchestrator.CalculateOwnershipsAsync(1, null, null, 1, this.orchestrationClientMock.Object, new Microsoft.Azure.WebJobs.ExecutionContext()).ConfigureAwait(false);
            this.ownershipRuleProcessor.Verify(m => m.CleanOwnershipDataAsync(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task OwnershipOrchestratorAsync_WhenTriggeredAsync()
        {
            var ownershipRuleData = new OwnershipRuleData
            {
                TicketId = 1,
                Errors = new List<ErrorInfo>(),
            };

            this.durableOrchestrationContextMock.Setup(m => m.GetInput<OwnershipRuleData>()).Returns(ownershipRuleData);
            this.durableOrchestrationContextMock.Setup(x => x.CallActivityAsync<OwnershipRuleData>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(ownershipRuleData);
            await this.orchestrator.OwnershipOrchestratorAsync(this.durableOrchestrationContextMock.Object).ConfigureAwait(false);
            this.durableOrchestrationContextMock.Verify(x => x.CallActivityAsync<OwnershipRuleData>(It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(3));
        }

        [TestMethod]
        public async Task OwnershipOrchestratorAsync_ThrowsExceptionAsync()
        {
            var ownershipRuleData = new OwnershipRuleData
            {
                TicketId = 1,
                Errors = new List<ErrorInfo>(),
            };

            this.durableOrchestrationContextMock.Setup(m => m.GetInput<OwnershipRuleData>()).Returns(ownershipRuleData);
            this.durableOrchestrationContextMock.Setup(x => x.CallActivityAsync<OwnershipRuleData>(It.IsAny<string>(), It.IsAny<object>())).ThrowsAsync(new Exception());
            this.orchestrationClientMock.Setup(m => m.GetStatusAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new DurableOrchestrationStatus { RuntimeStatus = OrchestrationRuntimeStatus.Completed });
            await this.orchestrator.OwnershipOrchestratorAsync(this.durableOrchestrationContextMock.Object).ConfigureAwait(false);
            this.durableOrchestrationContextMock.Verify(x => x.CallActivityAsync<OwnershipRuleData>(It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(1));
        }

        [TestMethod]
        public async Task ProcessAnalyticsAsync_WhenTriggeredAsync()
        {
            var ownershipRuleData = new OwnershipRuleData
            {
                TicketId = 1,
            };

            this.activityContextMock.Setup(m => m.GetInput<OwnershipRuleData>()).Returns(ownershipRuleData);
            await this.orchestrator.ProcessAnalyticsAsync(this.activityContextMock.Object).ConfigureAwait(false);
            this.ownershipRuleProcessor.Verify(m => m.ProcessAsync(ownershipRuleData, ChainType.ProcessAnalytics), Times.Once);
        }

        [TestMethod]
        public async Task ProcessOwnershipAsync_WhenTriggeredAsync()
        {
            var ownershipRuleData = new OwnershipRuleData
            {
                TicketId = 1,
            };

            this.activityContextMock.Setup(m => m.GetInput<OwnershipRuleData>()).Returns(ownershipRuleData);
            await this.orchestrator.ProcessOwnershipAsync(this.activityContextMock.Object).ConfigureAwait(false);
            this.ownershipRuleProcessor.Verify(m => m.ProcessAsync(ownershipRuleData, ChainType.RequestOwnershipData), Times.Once);
        }

        [TestMethod]
        public async Task BuildOwnershipAsync_WhenTriggeredAsync()
        {
            var ownershipRuleData = new OwnershipRuleData
            {
                TicketId = 1,
            };

            this.activityContextMock.Setup(m => m.GetInput<OwnershipRuleData>()).Returns(ownershipRuleData);
            await this.orchestrator.BuildOwnershipAsync(this.activityContextMock.Object).ConfigureAwait(false);
            this.ownershipRuleProcessor.Verify(m => m.ProcessAsync(ownershipRuleData, ChainType.Register), Times.Once);
        }

        [TestMethod]
        public async Task CalculateOwnershipAsync_WhenTriggeredAsync()
        {
            var ownershipRuleData = new OwnershipRuleData
            {
                TicketId = 1,
            };

            this.activityContextMock.Setup(m => m.GetInput<OwnershipRuleData>()).Returns(ownershipRuleData);
            await this.orchestrator.CalculateOwnershipAsync(this.activityContextMock.Object).ConfigureAwait(false);
            this.ownershipRuleProcessor.Verify(m => m.ProcessAsync(ownershipRuleData, ChainType.CalculateOwnershipData), Times.Once);
        }

        [TestMethod]
        public async Task FinalizeOwnershipAsync_ShouldFinalizeOwnershipForOwnershipAsync_forTicketAsync()
        {
            var ownershipRuleData = new OwnershipRuleData
            {
                TicketId = 1,
            };

            this.activityContextMock.Setup(m => m.GetInput<OwnershipRuleData>()).Returns(ownershipRuleData);
            await this.orchestrator.FinalizeOwnershipAsync(this.activityContextMock.Object).ConfigureAwait(false);
            this.ownershipRuleProcessor.Verify(m => m.FinalizeProcessAsync(ownershipRuleData), Times.Once);
        }

        [TestMethod]
        public async Task HandleFailureAsync_ShouldHandleFailureForOwnershipAsync_forTicketAsync()
        {
            this.ownershipFailureHandlerMock.SetupGet(x => x.TicketType).Returns(TicketType.Ownership);
            this.activityContextMock.Setup(m => m.GetInput<Tuple<OwnershipRuleData, string>>()).Returns(Tuple.Create(new OwnershipRuleData { TicketId = 1 }, "ErrorMessage"));
            await this.orchestrator.HandleFailureAsync(this.activityContextMock.Object).ConfigureAwait(false);
            this.ownershipFailureHandlerMock.Verify(m => m.HandleFailureAsync(It.IsAny<IUnitOfWork>(), It.IsAny<FailureInfo>()), Times.Once);
        }

        /// <summary>
        /// Completes the Ownership history when triggered asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task PurgeOwnershipHistoryAsync()
        {
            var ownershipRuleData = new OwnershipRuleData
            {
                TicketId = 1,
            };

            var timerInfo = new TimerInfo(null, new ScheduleStatus(), false);
            this.activityContextMock.Setup(m => m.GetInput<OwnershipRuleData>()).Returns(ownershipRuleData);

            await this.orchestrator.PurgeOwnershipHistoryAsync(timerInfo, this.orchestrationClientMock.Object, new Microsoft.Azure.WebJobs.ExecutionContext()).ConfigureAwait(false);
            this.ownershipRuleProcessor.Verify(m => m.ProcessAsync(ownershipRuleData, It.IsAny<ChainType>()), Times.Never);
        }

        /// <summary>
        /// Throw an exception when an error ocurred.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task CalculateOwnerships_ShouldReturnAnExceptionAsync()
        {
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<OwnershipRuleData>())).ThrowsAsync(new Exception());
            this.orchestrationClientMock.Setup(m => m.GetStatusAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new DurableOrchestrationStatus { RuntimeStatus = OrchestrationRuntimeStatus.Completed });

            await this.orchestrator.CalculateOwnershipsAsync(1, null, null, 1, this.orchestrationClientMock.Object, new Microsoft.Azure.WebJobs.ExecutionContext()).ConfigureAwait(false);
        }

        /// <summary>
        /// Should register a critical event on telemetry.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task FinalizeOwnership_ShouldRegisterInTelemtryAsync()
        {
            this.ownershipRuleProcessor.Setup(o => o.FinalizeProcessAsync(It.IsAny<OwnershipRuleData>())).ThrowsAsync(new Exception());
            var ownershipRuleData = new OwnershipRuleData
            {
                TicketId = 1,
            };

            this.activityContextMock.Setup(m => m.GetInput<OwnershipRuleData>()).Returns(ownershipRuleData);
            await this.orchestrator.FinalizeOwnershipAsync(this.activityContextMock.Object).ConfigureAwait(false);
            this.telemetryMock.Verify(
                t => t.TrackEvent(
                Ecp.True.Core.Constants.Critical, It.IsAny<string>(), It.IsAny<IDictionary<string, string>>(), It.IsAny<IDictionary<string, double>>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()),
                Times.Once);
        }

        /// <summary>
        /// Execute DoConciliationAsync of conciliationProcessor.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ConciliationOwnership_ShouldturnAnExceptionAsync()
        {
            var ownershipRuleData = new OwnershipRuleData
            {
                TicketId = 1,
            };

            this.activityContextMock.Setup(m => m.GetInput<OwnershipRuleData>()).Returns(ownershipRuleData);
            await this.orchestrator.ConciliationOwnershipAsync(this.activityContextMock.Object).ConfigureAwait(false);
            this.conciliationProcessorMock.Verify(c => c.DoConciliationAsync(It.IsAny<ConciliationNodesResquest>()), Times.Once);
        }

        /// <summary>
        /// Delete Status Nodes.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task DeteleConciliationMovementsOwnershipAsync_ShouldExecutedSuccesfullyAsync()
        {
            // Arrange
            var mockDurableActivityContext = new Mock<IDurableActivityContext>();
            var conciliationRuleData = new OwnershipRuleData
            {
                ChaosValue = "N/A",
                Activity = "Activity",
                Caller = "Caller",
                Orchestrator = "Orchestrator",
                ReplyTo = "Ana@ecopetrol.com",
            };
            mockDurableActivityContext.Setup(m => m.GetInput<OwnershipRuleData>()).Returns(conciliationRuleData);
            this.serviceProviderMock.Setup(s => s.GetService(typeof(ITrueLogger<FunctionBase>))).Returns(this.mockFunctionBaseLogger.Object);

            // Act
            await this.orchestrator.DeleteConciliationMovementsAsync(mockDurableActivityContext.Object).ConfigureAwait(false);

            // Assert
            this.conciliationProcessorMock.Verify(m => m.RegisterNegativeMovementsAsync(It.IsAny<IEnumerable<Movement>>()), Times.Once);
            this.conciliationProcessorMock.Verify(m => m.DeleteConciliationMovementsAsync(It.IsAny<int>(), It.IsAny<int?>()), Times.Once);
            this.conciliationProcessorMock.Verify(m => m.DeleteRelationshipOtherSegmentMovementsAsync(It.IsAny<int>(), It.IsAny<int?>()), Times.Once);
        }
    }
}
