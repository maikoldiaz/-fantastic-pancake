// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildExecutorTests.cs" company="Microsoft">
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
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Query;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Executors;
    using Ecp.True.Processors.Ownership.Services.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.Request;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The BuildExecutorTests.
    /// </summary>
    [TestClass]
    public class BuildExecutorTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<BuildExecutor>> mockLogger;

        /// <summary>
        /// The mock ownership service.
        /// </summary>
        private Mock<IOwnershipService> mockOwnershipService;

        /// <summary>
        /// The ownership rule data.
        /// </summary>
        private OwnershipRuleData ownershipRuleData;

        /// <summary>
        /// The build executor.
        /// </summary>
        private IExecutor buildExecutor;

        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<BuildExecutor>>();
            this.mockOwnershipService = new Mock<IOwnershipService>();
            this.ownershipRuleData = new OwnershipRuleData() { TicketId = 25281 };
            this.buildExecutor = new BuildExecutor(this.mockLogger.Object, this.mockOwnershipService.Object);
        }

        /// <summary>
        /// Executes the asynchronous build executor asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExecuteAsync_BuildExecutorAsync()
        {
            var ownership = new Ownership
            {
                TicketId = 25281,
            };

            var previousMovementOperationalData = new PreviousMovementOperationalData
            {
                MovementId = 100,
            };

            this.ownershipRuleData.OwnershipRuleRequest = new OwnershipRuleRequest
            {
                PreviousMovementsOperationalData = new List<PreviousMovementOperationalData> { previousMovementOperationalData },
            };

            this.ownershipRuleData.OwnershipRuleResponse = new OwnershipRuleResponse
            {
                InventoryResults = new List<OwnershipResultInventory>(),
                MovementResults = new List<OwnershipResultMovement>(),
            };

            this.mockOwnershipService.Setup(x => x.BuildOwnershipResults(It.IsAny<List<OwnershipResultInventory>>(), It.IsAny<List<OwnershipResultMovement>>())).Returns(new List<Ownership> { ownership });
            this.mockOwnershipService.Setup(x => x.ConsolidateInventoryResults(It.IsAny<List<OwnershipResultInventory>>(), It.IsAny<List<PreviousInventoryOperationalData>>(), 25281)).Returns(new List<OwnershipResultInventory>());
            this.mockOwnershipService.Setup(x => x.ConsolidateMovementResults(It.IsAny<List<OwnershipResultMovement>>(), It.IsAny<List<PreviousMovementOperationalData>>(), 25281)).Returns(new List<OwnershipResultMovement>());

            await this.buildExecutor.ExecuteAsync(this.ownershipRuleData).ConfigureAwait(false);

            Assert.IsNotNull(this.ownershipRuleData.Ownerships);
            this.mockOwnershipService.Verify(x => x.BuildOwnershipResults(It.IsAny<List<OwnershipResultInventory>>(), It.IsAny<List<OwnershipResultMovement>>()), Times.Once);
            this.mockOwnershipService.Verify(x => x.ConsolidateInventoryResults(It.IsAny<List<OwnershipResultInventory>>(), It.IsAny<List<PreviousInventoryOperationalData>>(), 25281), Times.Once);
            this.mockOwnershipService.Verify(x => x.ConsolidateMovementResults(It.IsAny<List<OwnershipResultMovement>>(), It.IsAny<List<PreviousMovementOperationalData>>(), 25281), Times.Once);
        }

        /// <summary>
        /// Builds the executor should return the process type when invoked.
        /// </summary>
        [TestMethod]
        public void BuildExecutor_ShouldReturnTheProcessType_WhenInvoked()
        {
            Assert.AreEqual(ProcessType.Ownership, this.buildExecutor.ProcessType);
        }

        /// <summary>
        /// Builds the executor should return the execution order when invoked.
        /// </summary>
        [TestMethod]
        public void BuildExecutor_ShouldReturnTheExecutionOrder_WhenInvoked()
        {
            Assert.AreEqual(7, this.buildExecutor.Order);
        }
    }
}
