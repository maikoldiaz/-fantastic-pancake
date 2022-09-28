// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecutionManagerTests.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core.Entities;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Executors;
    using Ecp.True.Processors.Ownership.Services.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ExecutionManagerTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<ExecutionManager>> mockLogger;

        /// <summary>
        /// The mock ownership service.
        /// </summary>
        private Mock<IOwnershipService> mockOwnershipService;

        private OwnershipRuleData ownershipRuleData;

        private IExecutionManager executionManager;

        private Mock<IExecutor> mockExecutor;

        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<ExecutionManager>>();
            this.mockOwnershipService = new Mock<IOwnershipService>();
            this.ownershipRuleData = new OwnershipRuleData() { TicketId = 25281, Errors = new List<ErrorInfo>(), };
            this.mockExecutor = new Mock<IExecutor>();
            this.executionManager = new ExecutionManager(this.mockOwnershipService.Object, this.mockLogger.Object);
            this.executionManager.Initialize(this.mockExecutor.Object);
        }

        [TestMethod]
        public async Task ExecuteAsync_BuildExecutorAsync()
        {
            this.mockExecutor.Setup(x => x.ExecuteAsync(It.IsAny<object>()));

            await this.executionManager.ExecuteChainAsync(this.ownershipRuleData).ConfigureAwait(false);

            this.mockExecutor.Verify(x => x.ExecuteAsync(It.IsAny<object>()), Times.Once);
        }

        [TestMethod]
        public async Task ExecuteAsync_BuildExecutorErrorAsync()
        {
            this.ownershipRuleData.OwnershipRuleResponse = new OwnershipRuleResponse
            {
                MovementErrors = new List<OwnershipErrorMovement>() { new OwnershipErrorMovement() { Ticket = "123" } },
                InventoryErrors = new List<OwnershipErrorInventory>() { new OwnershipErrorInventory() { Ticket = "123" } },
            };

            this.ownershipRuleData.HasProcessingErrors = true;

            this.mockExecutor.Setup(x => x.ExecuteAsync(It.IsAny<object>()));

            this.ownershipRuleData.Errors = new List<ErrorInfo>() { new ErrorInfo("Test Error") };
            this.mockOwnershipService.Setup(x => x.HandleFailureAsync(
                It.IsAny<int>(),
                It.IsAny<List<ErrorInfo>>(),
                It.IsAny<List<OwnershipErrorMovement>>(),
                It.IsAny<List<OwnershipErrorInventory>>(),
                It.IsAny<bool>()));
            await this.executionManager.ExecuteChainAsync(this.ownershipRuleData).ConfigureAwait(false);

            this.mockExecutor.Verify(x => x.ExecuteAsync(It.IsAny<object>()), Times.Once);
            this.mockOwnershipService.Verify(x => x.HandleFailureAsync(It.IsAny<int>(), It.IsAny<List<ErrorInfo>>(), It.IsAny<List<OwnershipErrorMovement>>(), It.IsAny<List<OwnershipErrorInventory>>(), It.IsAny<bool>()), Times.Once);
        }
    }
}
