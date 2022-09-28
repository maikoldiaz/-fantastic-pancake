// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResultValidationExecutorTests.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
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
    /// The ResultValidationExecutorTests.
    /// </summary>
    [TestClass]
    public class ResultValidationExecutorTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<ResultValidationExecutor>> mockLogger;

        /// <summary>
        /// The mock ownership calculation service.
        /// </summary>
        private Mock<IOwnershipValidator> mockOwnershipValidator;

        /// <summary>
        /// The result validation executor.
        /// </summary>
        private IExecutor resultValidationExecutor;

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
            this.mockLogger = new Mock<ITrueLogger<ResultValidationExecutor>>();
            this.mockOwnershipValidator = new Mock<IOwnershipValidator>();
            this.resultValidationExecutor = new ResultValidationExecutor(this.mockLogger.Object);
            this.ownershipRuleData = new OwnershipRuleData() { TicketId = 25281 };
            this.ownershipRuleRequest = new OwnershipRuleRequest();
            this.ownershipRuleResponse = new OwnershipRuleResponse();
        }

        /// <summary>
        /// Executes the asynchronous should return errors when result validation fails asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExecuteAsync_ShouldReturnErrors_WhenResultValidationFailsAsync()
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

            this.ownershipRuleRequest.MovementsOperationalData = new List<MovementOperationalData> { movementOperationalData };
            this.ownershipRuleRequest.InventoryOperationalData = new List<InventoryOperationalData> { inventoryOperationalData };
            this.ownershipRuleData.OwnershipRuleRequest = this.ownershipRuleRequest;
            this.ownershipRuleData.OwnershipRuleResponse = new OwnershipRuleResponse
            {
                InventoryResults = new List<OwnershipResultInventory>(),
                MovementResults = new List<OwnershipResultMovement>(),
            };

            await this.resultValidationExecutor.ExecuteAsync(this.ownershipRuleData).ConfigureAwait(false);

            Assert.IsNotNull(this.ownershipRuleData.Errors);
        }

        /// <summary>
        /// Executes the asynchronous should return no inventory and movement errors when no data asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExecuteAsync_ShouldReturnNoInventoryAndMovementErrors_WhenNoDataAsync()
        {
            this.ownershipRuleData.OwnershipRuleResponse = new OwnershipRuleResponse
            {
                InventoryResults = new List<OwnershipResultInventory>(),
                MovementResults = new List<OwnershipResultMovement>(),
            };

            await this.resultValidationExecutor.ExecuteAsync(this.ownershipRuleData).ConfigureAwait(false);

            Assert.IsNotNull(this.ownershipRuleData.Errors);
            Assert.IsTrue(this.ownershipRuleData.Errors.ToArray()[0].Message == Constants.ValidationNoInventoryAndMovementResultFailureMessage);
        }

        /// <summary>
        /// Results the validation executor should return the process type when invoked.
        /// </summary>
        [TestMethod]
        public void ResultValidationExecutor_ShouldReturnTheProcessType_WhenInvoked()
        {
            Assert.AreEqual(ProcessType.Ownership, this.resultValidationExecutor.ProcessType);
        }

        /// <summary>
        /// Results the validation executor should return the execution order when invoked.
        /// </summary>
        [TestMethod]
        public void ResultValidationExecutor_ShouldReturnTheExecutionOrder_WhenInvoked()
        {
            Assert.AreEqual(6, this.resultValidationExecutor.Order);
        }
    }
}
