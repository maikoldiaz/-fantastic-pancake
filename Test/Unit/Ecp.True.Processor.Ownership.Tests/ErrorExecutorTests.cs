// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorExecutorTests.cs" company="Microsoft">
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
    /// The ErrorExecutorTests.
    /// </summary>
    [TestClass]
    public class ErrorExecutorTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<ErrorExecutor>> mockLogger;

        /// <summary>
        /// The mock ownership calculation service.
        /// </summary>
        private Mock<IOwnershipService> mockOwnershipService;

        /// <summary>
        /// The error executor.
        /// </summary>
        private IExecutor errorExecutor;

        /// <summary>
        /// The ownership rule data.
        /// </summary>
        private OwnershipRuleData ownershipRuleData;

        /// <summary>
        /// The ownership rule request.
        /// </summary>
        private OwnershipRuleRequest ownershipRuleRequest;

        /// <summary>
        /// The ownership rule response.
        /// </summary>
        private OwnershipRuleResponse ownershipRuleResponse;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<ErrorExecutor>>();
            this.mockOwnershipService = new Mock<IOwnershipService>();
            this.errorExecutor = new ErrorExecutor(this.mockLogger.Object);
            this.ownershipRuleData = new OwnershipRuleData() { TicketId = 25281 };
            this.ownershipRuleRequest = new OwnershipRuleRequest();
            this.ownershipRuleResponse = new OwnershipRuleResponse();
        }

        /// <summary>
        /// Executes the asynchronous should process errors when inventory error is not empty asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ExecuteAsync_ShouldProcessErrors_WhenInventoryErrorIsNotEmptyAsync()
        {
            var movementOperationalData = new MovementOperationalData()
            {
                Ticket = 25281,
                MovementTransactionId = 5452,
                OwnershipUnit = "%",
                MovementTypeId = "43",
            };

            var inventoryOperationalData = new InventoryOperationalData()
            {
                Ticket = 25281,
                InventoryId = 5452,
                OwnershipUnit = "%",
            };

            var inventoryErrors = new OwnershipErrorInventory()
            {
                ResponseInventoryId = "5452",
                ResponseNodeId = "5451",
                ErrorDescription = "Error occurred",
            };

            this.ownershipRuleRequest.MovementsOperationalData = new List<MovementOperationalData> { movementOperationalData };
            this.ownershipRuleRequest.InventoryOperationalData = new List<InventoryOperationalData> { inventoryOperationalData };
            this.ownershipRuleResponse.InventoryErrors = new List<OwnershipErrorInventory> { inventoryErrors };
            this.ownershipRuleResponse.MovementErrors = new List<OwnershipErrorMovement>();
            this.ownershipRuleData.OwnershipRuleRequest = this.ownershipRuleRequest;
            this.ownershipRuleData.OwnershipRuleResponse = this.ownershipRuleResponse;

            await this.errorExecutor.ExecuteAsync(this.ownershipRuleData).ConfigureAwait(false);

            Assert.IsTrue(this.ownershipRuleData.HasProcessingErrors);
        }

        /// <summary>
        /// Executes the asynchronous should process errors when movement error is not empty asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExecuteAsync_ShouldProcessErrors_WhenMovementErrorIsNotEmptyAsync()
        {
            var movementOperationalData = new MovementOperationalData()
            {
                Ticket = 25281,
                MovementTransactionId = 5452,
                OwnershipUnit = "%",
                MovementTypeId = "43",
            };

            var inventoryOperationalData = new InventoryOperationalData()
            {
                Ticket = 25281,
                InventoryId = 5452,
                OwnershipUnit = "%",
            };

            var movementErrors = new OwnershipErrorMovement()
            {
                ResponseMovementId = "5452",
                ResponseSourceNodeId = "5453",
                ErrorDescription = "Error occurred",
            };

            this.ownershipRuleRequest.MovementsOperationalData = new List<MovementOperationalData> { movementOperationalData };
            this.ownershipRuleRequest.InventoryOperationalData = new List<InventoryOperationalData> { inventoryOperationalData };
            this.ownershipRuleResponse.InventoryErrors = new List<OwnershipErrorInventory>();
            this.ownershipRuleResponse.MovementErrors = new List<OwnershipErrorMovement> { movementErrors };
            this.ownershipRuleData.OwnershipRuleRequest = this.ownershipRuleRequest;
            this.ownershipRuleData.OwnershipRuleResponse = this.ownershipRuleResponse;

            await this.errorExecutor.ExecuteAsync(this.ownershipRuleData).ConfigureAwait(false);

            Assert.IsTrue(this.ownershipRuleData.HasProcessingErrors);
        }

        /// <summary>
        /// Errors the executor should return the process type when invoked.
        /// </summary>
        [TestMethod]
        public void ErrorExecutor_ShouldReturnTheProcessType_WhenInvoked()
        {
            Assert.AreEqual(ProcessType.Ownership, this.errorExecutor.ProcessType);
        }

        /// <summary>
        /// Errors the executor should return the execution order when invoked.
        /// </summary>
        [TestMethod]
        public void ErrorExecutor_ShouldReturnTheExecutionOrder_WhenInvoked()
        {
            Assert.AreEqual(5, this.errorExecutor.Order);
        }
    }
}
