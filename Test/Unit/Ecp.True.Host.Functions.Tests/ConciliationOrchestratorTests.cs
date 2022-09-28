// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConciliationOrchestratorTests.cs" company="Microsoft">
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
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Host.Functions.Ownership;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Conciliation.Entities;
    using Ecp.True.Processors.Conciliation.Interfaces;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The Conciliation cut off orchestrator tests.
    /// </summary>
    [TestClass]
    public class ConciliationOrchestratorTests
    {
        /// <summary>
        /// The logger mock.
        /// </summary>
        private readonly Mock<ITrueLogger<ConciliationOrchestrator>> loggerMock = new Mock<ITrueLogger<ConciliationOrchestrator>>();

        /// <summary>
        /// The service provider mock.
        /// </summary>`
        private readonly Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();

        /// <summary>
        /// The ownership rule processor.
        /// </summary>
        private readonly Mock<IConciliationProcessor> ownershipRuleProcessor = new Mock<IConciliationProcessor>();

        /// <summary>
        /// The durable orchestration client.
        /// </summary>
        private readonly Mock<IDurableOrchestrationClient> orchestrationClientMock = new Mock<IDurableOrchestrationClient>();

        /// <summary>
        /// The durable activity context.
        /// </summary>
        private readonly Mock<IDurableOrchestrationContext> orchestrationContextMock = new Mock<IDurableOrchestrationContext>();

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
        private ConciliationOrchestrator orchestrator;

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
        /// The ownership rule processor.
        /// </summary>
        private Mock<IOwnershipRuleProcessor> ownershipProcessor = new Mock<IOwnershipRuleProcessor>();

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
            this.serviceProviderMock.Setup(s => s.GetService(typeof(ITrueLogger<ConciliationOrchestrator>))).Returns(this.loggerMock.Object);
            this.mockFunctionBaseLogger = new Mock<ITrueLogger<FunctionBase>>();
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
            this.orchestrator = new ConciliationOrchestrator(
                this.loggerMock.Object,
                this.telemetryMock.Object,
                this.configurationHandlerMock.Object,
                this.mockAzureClientFactory.Object,
                this.serviceProviderMock.Object,
                this.ownershipProcessor.Object,
                this.ownershipRuleProcessor.Object,
                this.failureHandlerFactory.Object,
                this.mockUnitOfWorkFactory.Object);
        }

        /// <summary>
        /// Calculate Conciliation  asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ManualConciliationAsync_ShouldProccesingTicketSuccesfullyAsync()
        {
            // Arrange
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ConciliationRuleData>())).ReturnsAsync("ticket");

            // Act
            await this.orchestrator.ConciliationAsync(new ConciliationNodesResquest(), null, null, 1, this.orchestrationClientMock.Object, new ExecutionContext()).ConfigureAwait(false);

            // Assert
            this.orchestrationClientMock.Verify(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ConciliationRuleData>()), Times.Once);
        }

        /// <summary>
        /// Calculate Conciliation  asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task ManualConciliationAsync_ShouldThrownExceptionAsync()
        {
            // Arrange
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ConciliationRuleData>())).ThrowsAsync(new Exception());

            // Act
            await this.orchestrator.ConciliationAsync(new ConciliationNodesResquest(), null, null, 1, this.orchestrationClientMock.Object, new ExecutionContext()).ConfigureAwait(false);
        }

        /// <summary>
        /// Calculate Conciliation  asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ManualConciliationAsync_ShouldHandleExceptionAsync()
        {
            // Arrange
            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ConciliationRuleData>())).ThrowsAsync(new Exception());

            // Act
            await this.orchestrator.ConciliationAsync(new ConciliationNodesResquest(), null, null, 10, this.orchestrationClientMock.Object, new ExecutionContext()).ConfigureAwait(false);

            // Assert
            this.failureHandlerFactory.Verify(m => m.GetFailureHandler(It.IsAny<TicketType>()), Times.Once);
        }

        [TestMethod]
        public async Task CalculateOwnershipConciliationAsync_WhenTriggeredAsync()
        {
            // Arrange
            var conciliationRuleData = new ConciliationRuleData
            {
                ConciliationNodes = new ConciliationNodesResquest { TicketId = 1 },
                ChaosValue = "N/A",
                Activity = "Activity",
                Caller = "Caller",
                Orchestrator = "Orchestrator",
                ReplyTo = "Ana@ecopetrol.com",
            };
            var mockDurableActivityContext = new Mock<IDurableActivityContext>();
            mockDurableActivityContext.Setup(m => m.GetInput<ConciliationRuleData>()).Returns(conciliationRuleData);
            this.serviceProviderMock.Setup(s => s.GetService(typeof(ITrueLogger<FunctionBase>))).Returns(this.mockFunctionBaseLogger.Object);
            await this.orchestrator.CalculateOwnershipAsync(mockDurableActivityContext.Object).ConfigureAwait(false);
            this.ownershipProcessor.Verify(m => m.ProcessAsync(It.IsAny<OwnershipRuleData>(), It.IsAny<ChainType>()), Times.Once);
        }

        /// <summary>
        /// Conciliation s the orchestrator asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ManualConciliationOrchestratorAsync_ShouldProccesingTicketValidateConciliationNodeStatesIsFalseAsync()
        {
            // Arrange
            var conciliationRuleData = new ConciliationRuleData
            {
                ConciliationNodes = new ConciliationNodesResquest { TicketId = 1 },
                ChaosValue = "N/A",
                Activity = "Activity",
                Caller = "Caller",
                Orchestrator = "Orchestrator",
                ReplyTo = "Ana@ecopetrol.com",
            };

            this.orchestrationContextMock.Setup(m => m.GetInput<ConciliationRuleData>()).Returns(conciliationRuleData);
            this.serviceProviderMock.Setup(s => s.GetService(typeof(ITrueLogger<FunctionBase>))).Returns(this.mockFunctionBaseLogger.Object);
            this.orchestrationContextMock.Setup(x => x.CallActivityAsync<ConciliationRuleData>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(conciliationRuleData);

            // Act
            await this.orchestrator.ConciliationOrchestratorAsync(this.orchestrationContextMock.Object).ConfigureAwait(false);

            // Assert
            this.orchestrationContextMock.Verify(x => x.CallActivityAsync<ConciliationRuleData>(It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(1));
        }

        /// <summary>
        /// Conciliation s the orchestrator asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ManualConciliationOrchestratorAsync_ShouldProccesingTicketValidateConciliationNodeStatesIsTrueAsync()
        {
            // Arrange
            var conciliationRuleData = new ConciliationRuleData
            {
                ConciliationNodes = new ConciliationNodesResquest { TicketId = 1 },
                ChaosValue = "N/A",
                Activity = "Activity",
                Caller = "Caller",
                Orchestrator = "Orchestrator",
                ReplyTo = "Ana@ecopetrol.com",
                ValidateConciliationNodeStates = true,
            };

            this.orchestrationContextMock.Setup(m => m.GetInput<ConciliationRuleData>()).Returns(conciliationRuleData);
            this.serviceProviderMock.Setup(s => s.GetService(typeof(ITrueLogger<FunctionBase>))).Returns(this.mockFunctionBaseLogger.Object);
            this.orchestrationContextMock.Setup(x => x.CallActivityAsync<ConciliationRuleData>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(conciliationRuleData);

            // Act
            await this.orchestrator.ConciliationOrchestratorAsync(this.orchestrationContextMock.Object).ConfigureAwait(false);

            // Assert
            this.orchestrationContextMock.Verify(x => x.CallActivityAsync<ConciliationRuleData>(It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(4));
        }

        /// <summary>
        /// Conciliation s the orchestrator asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ManualConciliationOrchestratorAsync_ShouldThrownExceptionAsync()
        {
            // Arrange
            var conciliationRuleData = new ConciliationRuleData
            {
                ConciliationNodes = new ConciliationNodesResquest { TicketId = 1 },
                ChaosValue = "N/A",
                Activity = "Activity",
                Caller = "Caller",
                Orchestrator = "Orchestrator",
                ReplyTo = "Ana@ecopetrol.com",
                ValidateConciliationNodeStates = true,
            };

            this.orchestrationContextMock.Setup(m => m.GetInput<ConciliationRuleData>()).Returns(conciliationRuleData);
            this.serviceProviderMock.Setup(s => s.GetService(typeof(ITrueLogger<FunctionBase>))).Returns(this.mockFunctionBaseLogger.Object);
            this.orchestrationContextMock.Setup(x => x.CallActivityAsync<ConciliationRuleData>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(conciliationRuleData);
            this.orchestrationContextMock.Setup(x => x.CallActivityAsync<ConciliationRuleData>(It.IsAny<string>(), It.IsAny<object>())).ThrowsAsync(new Exception());

            // Act
            await this.orchestrator.ConciliationOrchestratorAsync(this.orchestrationContextMock.Object).ConfigureAwait(false);

            // Assert
            this.orchestrationContextMock.Verify(x => x.CallActivityAsync<ConciliationRuleData>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
            this.orchestrationContextMock.Verify(x => x.CallActivityAsync(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
        }

        /// <summary>
        /// Conciliation s the orchestrator asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ManualConciliationOrchestratorAsync_ShouldThrownExceptionWhenValidateConciliationNodeStatesIsTrueAsync()
        {
            // Arrange
            var conciliationRuleData = new ConciliationRuleData
            {
                ConciliationNodes = new ConciliationNodesResquest { TicketId = 1 },
                ChaosValue = "N/A",
                Activity = "Activity",
                Caller = "Caller",
                Orchestrator = "Orchestrator",
                ReplyTo = "Ana@ecopetrol.com",
                ValidateConciliationNodeStates = true,
            };

            this.orchestrationContextMock.Setup(m => m.GetInput<ConciliationRuleData>()).Returns(conciliationRuleData);
            this.serviceProviderMock.Setup(s => s.GetService(typeof(ITrueLogger<FunctionBase>))).Returns(this.mockFunctionBaseLogger.Object);
            this.orchestrationContextMock.Setup(x => x.CallActivityAsync<ConciliationRuleData>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(conciliationRuleData);
            this.orchestrationContextMock.Setup(x => x.CallActivityAsync(It.IsAny<string>(), It.IsAny<ConciliationRuleData>())).ThrowsAsync(new Exception());

            // Act
            await this.orchestrator.ConciliationOrchestratorAsync(this.orchestrationContextMock.Object).ConfigureAwait(false);

            // Assert
            this.orchestrationContextMock.Verify(x => x.CallActivityAsync<ConciliationRuleData>(It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(4));
            this.orchestrationContextMock.Verify(x => x.CallActivityAsync(It.IsAny<string>(), It.IsAny<ConciliationRuleData>()), Times.Once);
        }

        /// <summary>
        /// Clears the data asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ConciliationHandleFailureAsync_ShouldExecutedSuccesfullyAsync()
        {
            // Arrange
            var mockDurableActivityContext = new Mock<IDurableActivityContext>();
            var conciliationRuleData = new ConciliationRuleData
            {
                ConciliationNodes = new ConciliationNodesResquest { TicketId = 1 },
                ChaosValue = "N/A",
                Activity = "Activity",
                Caller = "Caller",
                Orchestrator = "Orchestrator",
                ReplyTo = "Ana@ecopetrol.com",
                ValidateConciliationNodeStates = true,
            };

            var objectTuple = new Tuple<ConciliationRuleData, string>(conciliationRuleData, "Rule Data");
            mockDurableActivityContext.Setup(m => m.GetInput<Tuple<ConciliationRuleData, string>>()).Returns(objectTuple);

            // Act
            await this.orchestrator.ConciliationHandleFailureAsync(mockDurableActivityContext.Object).ConfigureAwait(false);

            // Assert
            this.failureHandlerFactory.Verify(m => m.GetFailureHandler(It.IsAny<TicketType>()), Times.Once);
        }

        /// <summary>
        /// Conciliation.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ConciliationAsync_ShouldExecutedSuccesfullyAsync()
        {
            // Arrange
            var mockDurableActivityContext = new Mock<IDurableActivityContext>();
            var conciliationRuleData = new ConciliationRuleData
            {
                ConciliationNodes = new ConciliationNodesResquest { TicketId = 1 },
                ChaosValue = "N/A",
                Activity = "Activity",
                Caller = "Caller",
                Orchestrator = "Orchestrator",
                ReplyTo = "Ana@ecopetrol.com",
                ValidateConciliationNodeStates = true,
            };

            mockDurableActivityContext.Setup(m => m.GetInput<ConciliationRuleData>()).Returns(conciliationRuleData);
            this.serviceProviderMock.Setup(s => s.GetService(typeof(ITrueLogger<FunctionBase>))).Returns(this.mockFunctionBaseLogger.Object);

            // Act
            await this.orchestrator.DoConciliationAsync(mockDurableActivityContext.Object).ConfigureAwait(false);

            // Assert
            this.ownershipRuleProcessor.Verify(m => m.DoConciliationAsync(It.IsAny<ConciliationNodesResquest>()), Times.Once);
        }

        /// <summary>
        /// Validate if exist Nodes in state APPROVED or SUBMITFORAPPROVAL.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ValidateConciliationNodeStateAsync_ShouldExecutedSuccesfullyAsync()
        {
            // Arrange
            var mockDurableActivityContext = new Mock<IDurableActivityContext>();
            var conciliationRuleData = new ConciliationRuleData
            {
                ConciliationNodes = new ConciliationNodesResquest { TicketId = 9 , NodeId = 9 },
                ChaosValue = "N/A",
                Activity = "Activity",
                Caller = "Caller",
                Orchestrator = "Orchestrator",
                ReplyTo = "Ana@ecopetrol.com",
                ValidateConciliationNodeStates = true,
            };

            mockDurableActivityContext.Setup(m => m.GetInput<ConciliationRuleData>()).Returns(conciliationRuleData);
            this.ownershipRuleProcessor.Setup(s => s.GetConciliationNodesAsync(It.IsAny<int>(), It.IsAny<int?>())).ReturnsAsync(new List<OwnershipNodeData> { new OwnershipNodeData() { NodeId = 9, OwnershipStatusId = 9 } });
            this.serviceProviderMock.Setup(s => s.GetService(typeof(ITrueLogger<FunctionBase>))).Returns(this.mockFunctionBaseLogger.Object);

            // Act
            var taskConciliationRuleData = await this.orchestrator.ValidateConciliationNodeStateAsync(mockDurableActivityContext.Object).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(taskConciliationRuleData);
            this.ownershipRuleProcessor.Verify(m => m.GetConciliationNodesAsync(It.IsAny<int>(), It.IsAny<int?>()), Times.Once);
        }

        /// <summary>
        /// Finalize the ownership.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task FinalizeConciliationpAsync_ShouldExecutedSuccesfullyAsync()
        {
            // Arrange
            var mockDurableActivityContext = new Mock<IDurableActivityContext>();
            var conciliationRuleData = new ConciliationRuleData
            {
                ConciliationNodes = new ConciliationNodesResquest { TicketId = 1 },
                ChaosValue = "N/A",
                Activity = "Activity",
                Caller = "Caller",
                Orchestrator = "Orchestrator",
                ReplyTo = "Ana@ecopetrol.com",
                ValidateConciliationNodeStates = true,
            };

            mockDurableActivityContext.Setup(m => m.GetInput<ConciliationRuleData>()).Returns(conciliationRuleData);
            this.serviceProviderMock.Setup(s => s.GetService(typeof(ITrueLogger<FunctionBase>))).Returns(this.mockFunctionBaseLogger.Object);

            // Act
            await this.orchestrator.FinalizeConciliationAsync(mockDurableActivityContext.Object).ConfigureAwait(false);

            // Assert
            this.ownershipRuleProcessor.Verify(m => m.FinalizeProcessAsync(It.IsAny<ConciliationRuleData>()), Times.Once);
        }

        /// <summary>
        /// Finalize the ownership.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task FinalizeConciliationpAsync_ShouldThrownExceptionAsync()
        {
            // Arrange
            var mockDurableActivityContext = new Mock<IDurableActivityContext>();
            var conciliationRuleData = new ConciliationRuleData
            {
                ConciliationNodes = new ConciliationNodesResquest { TicketId = 1 },
                ChaosValue = "N/A",
                Activity = "Activity",
                Caller = "Caller",
                Orchestrator = "Orchestrator",
                ReplyTo = "Ana@ecopetrol.com",
                ValidateConciliationNodeStates = true,
            };
            mockDurableActivityContext.Setup(m => m.GetInput<ConciliationRuleData>()).Returns(conciliationRuleData);
            this.serviceProviderMock.Setup(s => s.GetService(typeof(ITrueLogger<FunctionBase>))).Returns(this.mockFunctionBaseLogger.Object);

            this.serviceProviderMock.Setup(x => x.GetService(typeof(ITrueLogger<FunctionBase>))).Throws(new Exception());

            // Act
            await this.orchestrator.FinalizeConciliationAsync(mockDurableActivityContext.Object).ConfigureAwait(false);

            // Assert
            this.telemetryMock.Verify(m => m.TrackEvent(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, string>>(), It.IsAny<IDictionary<string, double>>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Delete Status Nodes.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task DeteleConciliationMovementsAsync_ShouldExecutedSuccesfullyAsync()
        {
            // Arrange
            var mockDurableActivityContext = new Mock<IDurableActivityContext>();
            var conciliationRuleData = new ConciliationRuleData
            {
                ConciliationNodes = new ConciliationNodesResquest { TicketId = 1 },
                ChaosValue = "N/A",
                Activity = "Activity",
                Caller = "Caller",
                Orchestrator = "Orchestrator",
                ReplyTo = "Ana@ecopetrol.com",
                ValidateConciliationNodeStates = true,
            };
            mockDurableActivityContext.Setup(m => m.GetInput<ConciliationRuleData>()).Returns(conciliationRuleData);
            this.serviceProviderMock.Setup(s => s.GetService(typeof(ITrueLogger<FunctionBase>))).Returns(this.mockFunctionBaseLogger.Object);

            // Act
            await this.orchestrator.DeleteConciliationMovementsAsync(mockDurableActivityContext.Object).ConfigureAwait(false);

            // Assert
            this.ownershipRuleProcessor.Verify(m => m.RegisterNegativeMovementsAsync(It.IsAny<IEnumerable<Movement>>()), Times.Once);
            this.ownershipRuleProcessor.Verify(m => m.DeleteConciliationMovementsAsync(It.IsAny<int>(), It.IsAny<int?>()), Times.Once);
            this.ownershipRuleProcessor.Verify(m => m.DeleteRelationshipOtherSegmentMovementsAsync(It.IsAny<int>(), It.IsAny<int?>()), Times.Once);
        }

        /// <summary>
        /// Update Conciliation Movements.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateConciliationNodesAsync_ShouldExecutedSuccesfullyAsync()
        {
            // Arrange
            var mockDurableActivityContext = new Mock<IDurableActivityContext>();
            var conciliationRuleData = new ConciliationRuleData
            {
                ConciliationNodes = new ConciliationNodesResquest { TicketId = 1 },
                ChaosValue = "N/A",
                Activity = "Activity",
                Caller = "Caller",
                Orchestrator = "Orchestrator",
                ReplyTo = "Ana@ecopetrol.com",
                ValidateConciliationNodeStates = true,
            };
            mockDurableActivityContext.Setup(m => m.GetInput<ConciliationRuleData>()).Returns(conciliationRuleData);
            this.serviceProviderMock.Setup(s => s.GetService(typeof(ITrueLogger<FunctionBase>))).Returns(this.mockFunctionBaseLogger.Object);

            // Act
            await this.orchestrator.UpdateConciliationNodesAsync(mockDurableActivityContext.Object).ConfigureAwait(false);

            // Assert
            this.ownershipRuleProcessor.Verify(m => m.UpdateOwnershipNodeAsync(It.IsAny<int>(), It.IsAny<StatusType>(), It.IsAny<OwnershipNodeStatusType>(), It.IsAny<int?>()), Times.Once);
        }
    }
}