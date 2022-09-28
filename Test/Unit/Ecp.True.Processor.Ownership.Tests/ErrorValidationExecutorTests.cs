// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorValidationExecutorTests.cs" company="Microsoft">
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
    /// The ErrorValidationExecutorTests.
    /// </summary>
    [TestClass]
    public class ErrorValidationExecutorTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<ErrorValidationExecutor>> mockLogger;

        /// <summary>
        /// The mock ownership calculation service.
        /// </summary>
        private Mock<IOwnershipValidator> mockOwnershipValidator;

        /// <summary>
        /// The error validation executor.
        /// </summary>
        private IExecutor errorValidationExecutor;

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
            this.mockLogger = new Mock<ITrueLogger<ErrorValidationExecutor>>();
            this.mockOwnershipValidator = new Mock<IOwnershipValidator>();
            this.errorValidationExecutor = new ErrorValidationExecutor(this.mockLogger.Object, this.mockOwnershipValidator.Object);
            this.ownershipRuleData = new OwnershipRuleData() { TicketId = 25281 };
            this.ownershipRuleRequest = new OwnershipRuleRequest();
            this.ownershipRuleResponse = new OwnershipRuleResponse();
        }

        /// <summary>
        /// Executes the asynchronous should return errors when error validation fails asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExecuteAsync_ShouldReturnErrors_WhenErrorValidationFailsAsync()
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

            var movementErrors = new OwnershipErrorMovement()
            {
                ResponseMovementId = "5452",
                ResponseSourceNodeId = "5453",
                ErrorDescription = "Error occurred",
            };

            this.ownershipRuleRequest.MovementsOperationalData = new List<MovementOperationalData> { movementOperationalData };
            this.ownershipRuleRequest.InventoryOperationalData = new List<InventoryOperationalData> { inventoryOperationalData };
            this.ownershipRuleResponse.InventoryErrors = new List<OwnershipErrorInventory> { inventoryErrors };
            this.ownershipRuleResponse.MovementErrors = new List<OwnershipErrorMovement> { movementErrors };
            this.ownershipRuleData.OwnershipRuleRequest = this.ownershipRuleRequest;
            this.ownershipRuleData.OwnershipRuleResponse = this.ownershipRuleResponse;

            var errors = new List<ErrorInfo> { new ErrorInfo("Invalid movements or inventories") };

            this.mockOwnershipValidator.Setup(x => x.ValidateOwnershipRuleErrorAsync(this.ownershipRuleResponse.InventoryErrors, this.ownershipRuleResponse.MovementErrors)).ReturnsAsync(errors);

            await this.errorValidationExecutor.ExecuteAsync(this.ownershipRuleData).ConfigureAwait(false);

            this.mockOwnershipValidator.Verify(x => x.ValidateOwnershipRuleErrorAsync(this.ownershipRuleResponse.InventoryErrors, this.ownershipRuleResponse.MovementErrors), Times.Once);
        }

        /// <summary>
        /// Errors the validation executor should return the process type when invoked.
        /// </summary>
        [TestMethod]
        public void ErrorValidationExecutor_ShouldReturnTheProcessType_WhenInvoked()
        {
            Assert.AreEqual(ProcessType.Ownership, this.errorValidationExecutor.ProcessType);
        }

        /// <summary>
        /// Errors the validation executor should return the execution order when invoked.
        /// </summary>
        [TestMethod]
        public void ErrorValidationExecutor_ShouldReturnTheExecutionOrder_WhenInvoked()
        {
            Assert.AreEqual(4, this.errorValidationExecutor.Order);
        }
    }
}
