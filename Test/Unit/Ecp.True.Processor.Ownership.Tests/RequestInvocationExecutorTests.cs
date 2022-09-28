// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestInvocationExecutorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Ownership.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core.Entities;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Executors;
    using Ecp.True.Processors.Ownership.Integration;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.Request;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The RequestInvocationExecutorTests.
    /// </summary>
    [TestClass]
    public class RequestInvocationExecutorTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<RequestInvocationExecutor>> mockLogger;

        /// <summary>
        /// The mock ownership calculation service.
        /// </summary>
        private Mock<IOwnershipRuleProxy> mockOwnershipRuleService;

        /// <summary>
        /// The mock configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigurationHandler;

        /// <summary>
        /// The mock ownership calculation service.
        /// </summary>
        private Mock<IOwnershipCalculationService> mockOwnershipCalculationService;

        /// <summary>
        /// The request invocation execution.
        /// </summary>
        private IExecutor requestInvocationExecution;

        /// <summary>
        /// The ownership rule data.
        /// </summary>
        private OwnershipRuleData ownershipRuleData;

        /// <summary>
        /// The ownership rule request.
        /// </summary>
        private OwnershipRuleRequest ownershipRuleRequest;

        /// <summary>
        /// The Save Integration File mock.
        /// </summary>
        private Mock<ISaveIntegrationOwnershipFile> mockSaveIntegrationFile;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<RequestInvocationExecutor>>();
            this.mockOwnershipCalculationService = new Mock<IOwnershipCalculationService>();
            this.mockOwnershipRuleService = new Mock<IOwnershipRuleProxy>();
            this.mockConfigurationHandler = new Mock<IConfigurationHandler>();
            this.mockSaveIntegrationFile = new Mock<ISaveIntegrationOwnershipFile>();
            this.requestInvocationExecution = new RequestInvocationExecutor(
                this.mockLogger.Object,
                this.mockOwnershipRuleService.Object,
                this.mockOwnershipCalculationService.Object,
                this.mockConfigurationHandler.Object,
                this.mockSaveIntegrationFile.Object);
            this.ownershipRuleData = new OwnershipRuleData() { TicketId = 25281 };
            this.ownershipRuleRequest = new OwnershipRuleRequest();
        }

        /// <summary>
        /// Requests the invocation executor should return the process type when invoked.
        /// </summary>
        [TestMethod]
        public void RequestInvocationExecutor_ShouldReturnTheProcessType_WhenInvoked()
        {
            Assert.AreEqual(ProcessType.Ownership, this.requestInvocationExecution.ProcessType);
        }

        /// <summary>
        /// Requests the invocation executor should return the execution order when invoked.
        /// </summary>
        [TestMethod]
        public void RequestInvocationExecutor_ShouldReturnTheExecutionOrder_WhenInvoked()
        {
            Assert.AreEqual(3, this.requestInvocationExecution.Order);
        }

        /// <summary>
        /// Executes the asynchronous should process data when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExecuteAsync_ShouldProcessData_WhenInvokedAsync()
        {
            var ownershipData = new OwnershipRuleData
            {
                TicketId = 123,
                OwnershipRuleRequest = new OwnershipRuleRequest
                {
                    InventoryOperationalData = new List<InventoryOperationalData>(),
                    PreviousInventoryOperationalData = new List<PreviousInventoryOperationalData>(),
                    MovementsOperationalData = new List<MovementOperationalData>(),
                    PreviousMovementsOperationalData = new List<PreviousMovementOperationalData>(),
                    NodeConfigurations = new List<NodeConfiguration>(),
                    NodeConnections = new List<NodeConnection>(),
                    Events = new List<Event>(),
                    Contracts = new List<Contract>(),
                },
                Errors = new List<ErrorInfo>(),
            };

            var ownershipRuleSettings = new OwnershipRuleSettings
            {
                ShouldStoreResponse = true,
            };

            var ownershipRuleResponse = new OwnershipRuleResponse();

            this.mockConfigurationHandler.Setup(a => a.GetConfigurationAsync<OwnershipRuleSettings>(It.IsAny<string>())).ReturnsAsync(ownershipRuleSettings);
            this.mockOwnershipRuleService.Setup(a => a.Initialize(It.IsAny<OwnershipRuleSettings>()));
            this.mockOwnershipRuleService.Setup(a => a.ProcessOwnershipAsync(It.IsAny<OwnershipRuleRequest>(), It.IsAny<int>())).ReturnsAsync(ownershipRuleResponse);
            this.mockOwnershipCalculationService.Setup(a => a.AddOwnershipNodesAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<int>()));
            this.mockSaveIntegrationFile.Setup(x => x.RegisterIntegrationAsync(It.IsAny<IntegrationData>())).ReturnsAsync(Guid.NewGuid().ToString);

            await this.requestInvocationExecution.ExecuteAsync(ownershipData).ConfigureAwait(false);

            this.mockConfigurationHandler.Verify(a => a.GetConfigurationAsync<OwnershipRuleSettings>(It.IsAny<string>()), Times.Once);
            this.mockOwnershipRuleService.Verify(a => a.Initialize(It.IsAny<OwnershipRuleSettings>()), Times.Once);
            this.mockOwnershipRuleService.Verify(a => a.ProcessOwnershipAsync(It.IsAny<OwnershipRuleRequest>(), It.IsAny<int>()), Times.Once);
            this.mockOwnershipCalculationService.Verify(a => a.AddOwnershipNodesAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<int>()), Times.Once);
            this.mockSaveIntegrationFile.Verify(a => a.RegisterIntegrationAsync(It.IsAny<IntegrationData>()), Times.Exactly(2));
        }

        /// <summary>
        /// Executes the asynchronous should log exception when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExecuteAsync_ShouldLogException_WhenInvokedAsync()
        {
            var ownershipData = new OwnershipRuleData
            {
                TicketId = 123,
                OwnershipRuleRequest = new OwnershipRuleRequest
                {
                    InventoryOperationalData = new List<InventoryOperationalData>(),
                    PreviousInventoryOperationalData = new List<PreviousInventoryOperationalData>(),
                    MovementsOperationalData = new List<MovementOperationalData>(),
                    PreviousMovementsOperationalData = new List<PreviousMovementOperationalData>(),
                    NodeConfigurations = new List<NodeConfiguration>(),
                    NodeConnections = new List<NodeConnection>(),
                    Events = new List<Event>(),
                    Contracts = new List<Contract>(),
                },
                Errors = new List<ErrorInfo>(),
            };

            var ownershipRuleSettings = new OwnershipRuleSettings
            {
                ShouldStoreResponse = true,
            };

            this.mockConfigurationHandler.Setup(a => a.GetConfigurationAsync<OwnershipRuleSettings>(It.IsAny<string>())).ReturnsAsync(ownershipRuleSettings);
            this.mockOwnershipRuleService.Setup(a => a.Initialize(It.IsAny<OwnershipRuleSettings>()));
            this.mockOwnershipRuleService.Setup(a => a.ProcessOwnershipAsync(It.IsAny<OwnershipRuleRequest>(), It.IsAny<int>())).ThrowsAsync(new Exception());
            this.mockOwnershipCalculationService.Setup(a => a.AddOwnershipNodesAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<int>()));
            this.mockSaveIntegrationFile.Setup(x => x.RegisterIntegrationAsync(It.IsAny<IntegrationData>())).ReturnsAsync(Guid.NewGuid().ToString);

            await this.requestInvocationExecution.ExecuteAsync(ownershipData).ConfigureAwait(false);

            this.mockConfigurationHandler.Verify(a => a.GetConfigurationAsync<OwnershipRuleSettings>(It.IsAny<string>()), Times.Once);
            this.mockOwnershipRuleService.Verify(a => a.Initialize(It.IsAny<OwnershipRuleSettings>()), Times.Once);
            this.mockOwnershipRuleService.Verify(a => a.ProcessOwnershipAsync(It.IsAny<OwnershipRuleRequest>(), It.IsAny<int>()), Times.Once);
            this.mockOwnershipCalculationService.Verify(a => a.AddOwnershipNodesAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<int>()), Times.Never);
            this.mockSaveIntegrationFile.Verify(a => a.RegisterIntegrationAsync(It.IsAny<IntegrationData>()), Times.Exactly(1));
        }
    }
}
