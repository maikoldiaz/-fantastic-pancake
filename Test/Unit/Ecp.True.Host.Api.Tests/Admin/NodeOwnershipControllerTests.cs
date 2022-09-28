// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeOwnershipControllerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Tests.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Approval.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Contract = Ecp.True.Entities.Registration.Contract;

    /// <summary>
    /// The Node Ownership test class.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Api.Tests.ControllerTestBase" />
    [TestClass]
    public class NodeOwnershipControllerTests : ControllerTestBase
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private NodeOwnershipController controller;

        /// <summary>
        /// The Node Ownership processor.
        /// </summary>
        private Mock<INodeOwnershipProcessor> mockProcessor;

        /// <summary>
        /// The Approval Processor processor.
        /// </summary>
        private Mock<IApprovalProcessor> mockApprovalProcessor;

        [TestInitialize]
        public void Initialize()
        {
            this.mockProcessor = new Mock<INodeOwnershipProcessor>();
            this.mockApprovalProcessor = new Mock<IApprovalProcessor>();
            this.controller = new NodeOwnershipController(this.mockProcessor.Object, this.mockApprovalProcessor.Object);
        }

        /// <summary>
        /// Reopens the Ticket asynchronous should invoke processor to return200 success asynchronous.
        /// </summary>
        /// <returns>The Reopen Ticket result.</returns>
        [TestMethod]
        public async Task ReopenTicketAsync_ShouldInvokeProcessor_ToReturn200SuccessAsync()
        {
            var reopenTicket = new ReopenTicket();
            this.mockProcessor.Setup(m => m.ReopenOwnershipNodeAsync(reopenTicket));

            var result = await this.controller.ReopenOwnershipNodeAsync(reopenTicket).ConfigureAwait(false);

            // Assert
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.ReopenOwnershipNodeAsync(reopenTicket), Times.Once());
        }

        /// <summary>
        /// Movement owners data is returned.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetOwnersForMovementAsync_ShouldInvokeProcessor_GetOwnersResultAsync()
        {
            var movementOwners = new List<NodeConnectionProductOwner>();
            movementOwners.Add(new NodeConnectionProductOwner { NodeConnectionProductOwnerId = 100, NodeConnectionProductId = 10, IsDeleted = false });
            this.mockProcessor.Setup(o => o.GetOwnersForMovementAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(movementOwners);

            var result = await this.controller.GetOwnersForMovementAsync(10, 20, "testProductId").ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            this.mockProcessor.Verify(c => c.GetOwnersForMovementAsync(10, 20, "testProductId"), Times.Once());
        }

        /// <summary>
        /// Movement owners data is returned.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetOwnersForInventoryAsync_ShouldInvokeProcessor_GetOwnersResultAsync()
        {
            var inventoryOwners = new List<StorageLocationProductOwner>();
            inventoryOwners.Add(new StorageLocationProductOwner { StorageLocationProductOwnerId = 100, OwnershipPercentage = 100, StorageLocationProductId = 10, IsDeleted = false });
            this.mockProcessor.Setup(o => o.GetOwnersForInventoryAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(inventoryOwners);

            var result = await this.controller.GetOwnersForInventoryAsync(10, "testProductId").ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            this.mockProcessor.Verify(c => c.GetOwnersForInventoryAsync(10, "testProductId"), Times.Once());
        }

        /// <summary>
        /// Gets the conditional ownership node by identifier asynchronous should return entity result asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetConditionalOwnershipNodeByIdAsync_ShouldReturnEntityResultAsync()
        {
            var ownershipNode = new OwnershipNode
            {
                OwnershipNodeId = 12,
                TicketId = 123,
                OwnershipStatus = OwnershipNodeStatusType.LOCKED,
            };
            this.mockProcessor.Setup(o => o.GetConditionalOwnershipNodeByIdAsync(It.IsAny<int>())).ReturnsAsync(ownershipNode);

            var result = await this.controller.GetConditionalOwnershipNodeByIdAsync(ownershipNode.OwnershipNodeId).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            this.mockProcessor.Verify(c => c.GetConditionalOwnershipNodeByIdAsync(It.IsAny<int>()), Times.Once());
        }

        /// <summary>
        /// Gets the conditional ownership node by identifier asynchronous should forbidden asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public async Task GetConditionalOwnershipNodeByIdAsync_ShouldForbiddenAsync()
        {
            var ownershipNode = new OwnershipNode();
            this.mockProcessor.Setup(o => o.GetConditionalOwnershipNodeByIdAsync(It.IsAny<int>())).ThrowsAsync(new UnauthorizedAccessException());
            await this.controller.GetConditionalOwnershipNodeByIdAsync(ownershipNode.OwnershipNodeId).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the conditional ownership node by identifier asynchronous should not found asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetConditionalOwnershipNodeByIdAsync_ShouldNotFoundAsync()
        {
            this.mockProcessor.Setup(o => o.GetConditionalOwnershipNodeByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            var result = await this.controller.GetConditionalOwnershipNodeByIdAsync(12).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            this.mockProcessor.Verify(c => c.GetConditionalOwnershipNodeByIdAsync(It.IsAny<int>()), Times.Once());
        }

        /// <summary>
        /// Movement owners data is returned.
        /// </summary>
        /// <returns>
        /// The status.
        /// </returns>
        [TestMethod]
        public async Task SendOwnershipNodeForApprovalAsync_ShouldInvokeApprovalProcessor_GetOkResultAsync()
        {
            int ownershipNodeId = 1;
            this.mockApprovalProcessor.Setup(m => m.SendOwnershipNodeIdForApprovalAsync(It.IsAny<int>()));
            var result = await this.controller.SendOwnershipNodeForApprovalAsync(ownershipNodeId).ConfigureAwait(false);

            this.mockApprovalProcessor.Verify(c => c.SendOwnershipNodeIdForApprovalAsync(It.IsAny<int>()), Times.Once());
            Assert.IsNotNull(result, "OwnershipNodeForApprovalSentSuccessfully");
        }

        /// <summary>
        /// Gets the rules synchronize progress asynchronous should return progress status asynchronous.
        /// </summary>
        /// /// <returns>
        /// The status.
        /// </returns>
        [TestMethod]
        public async Task GetRulesSyncProgressAsync_shouldReturnProgressStatusAsync()
        {
            this.mockProcessor.Setup(o => o.IsSyncInProgressAsync());
            var result = await this.controller.GetRulesSyncProgressAsync().ConfigureAwait(false);
            Assert.IsNotNull(result);
            this.mockProcessor.Verify(c => c.IsSyncInProgressAsync(), Times.Once());
        }

        /// <summary>
        /// Gets the rules synchronize progress asynchronous should return progress status asynchronous.
        /// </summary>
        /// /// <returns>
        /// The status.
        /// </returns>
        [TestMethod]
        public async Task UpdateRuleSyncAsync_shouldreturnProgressStatusAsync()
        {
            this.mockProcessor.Setup(o => o.TryRefreshRulesAsync());
            var result = await this.controller.UpdateRuleSyncAsync().ConfigureAwait(false);
            Assert.IsNotNull(result);
            this.mockProcessor.Verify(c => c.TryRefreshRulesAsync(), Times.Once());
        }

        /// <summary>
        /// Gets the nodes asynchronous should return active nodes.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetContractsAsync_ShouldInvokeProcessor_ToReturnContractsAsync()
        {
            var contracts = new[] { new Contract() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryAllAsync<Contract>(null)).ReturnsAsync(contracts);

            var result = await this.controller.QueryContractsAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, contracts);

            this.mockProcessor.Verify(c => c.QueryAllAsync<Contract>(null), Times.Once());
        }

        /// <summary>
        /// Validates the ownership nodes asynchronous should invoke processor to return validation result.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ValidateOwnershipNodes_ShouldInvokeProcessor_ToReturnValidationResultAsync()
        {
            var ticket = new Ticket();
            IEnumerable<OwnershipValidationResult> ownershipResult = new List<OwnershipValidationResult>();
            this.mockProcessor.Setup(m => m.ValidateOwnershipNodesAsync(It.IsAny<Ticket>())).Returns(Task.FromResult(ownershipResult));
            var result = await this.controller.ValidateOwnershipNodesAsync(ticket).ConfigureAwait(false);

            this.mockProcessor.Verify(c => c.ValidateOwnershipNodesAsync(It.IsAny<Ticket>()), Times.Once());
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }

        /// <summary>
        /// Queries the ownershipNodes returns ownershipNodes when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task QueryTickets_ReturnsOwnershipNodes_WhenInvokedAsync()
        {
            var ownershipNodes = new[] { new OwnershipNode() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryAllAsync<OwnershipNode>(null)).ReturnsAsync(ownershipNodes);

            var result = await this.controller.QueryOwnershipNodeAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, ownershipNodes);

            this.mockProcessor.Verify(c => c.QueryAllAsync<OwnershipNode>(null), Times.Once());
        }

        [TestMethod]
        public async Task GetOwnershipNodeView_ReturnsOwnershipNode_WhenInvokedAsync()
        {
            var ownershipNodes = new List<OwnershipNodeData>() { new OwnershipNodeData() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryViewAsync<OwnershipNodeData>()).ReturnsAsync(ownershipNodes);
            var result = await this.controller.QueryOwnershipNodeViewAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(result, ownershipNodes);

            this.mockProcessor.Verify(c => c.QueryViewAsync<OwnershipNodeData>(), Times.Once());
        }

        [TestMethod]
        public async Task GetOwnershipNodeView_ReturnsNull_WhenInvokedAsync()
        {
            var ownershipNodes = new List<OwnershipNodeData>() { new OwnershipNodeData() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryViewAsync<OwnershipNodeData>()).ReturnsAsync(ownershipNodes);
            var result = await this.controller.QueryOwnershipNodeViewAsync().ConfigureAwait(false);
            var entityResult = result as EntityResult;

            // Assert
            Assert.IsNull(entityResult, "Result should be null");

            this.mockProcessor.Verify(c => c.QueryViewAsync<OwnershipNodeData>(), Times.Once());
        }

        /// <summary>
        /// Gets the ownership calculation errors should invoke processor to return errors asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetOwnershipCalculationErrors_ReturnsErrors_WhenInvokedAsync()
        {
            var errors = new List<OwnershipError>() { new OwnershipError() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryViewAsync<OwnershipError>()).ReturnsAsync(errors);
            var result = await this.controller.QueryOwnershipNodeErrorsAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(result, errors);

            this.mockProcessor.Verify(c => c.QueryViewAsync<OwnershipError>(), Times.Once());
        }

        /// <summary>
        /// Gets the ownership calculation errors should invoke processor to return null asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetOwnershipCalculationErrors_ReturnsNull_WhenInvokedAsync()
        {
            var errors = new List<OwnershipError>() { new OwnershipError() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryViewAsync<OwnershipError>()).ReturnsAsync(errors);
            var result = await this.controller.QueryOwnershipNodeErrorsAsync().ConfigureAwait(false);
            var entityResult = result as EntityResult;

            // Assert
            Assert.IsNull(entityResult, "Result should not be null");

            this.mockProcessor.Verify(c => c.QueryViewAsync<OwnershipError>(), Times.Once());
        }

        [TestMethod]
        public async Task GetOwnershipNodeBalanceSummary_ReturnsOwnershipNodeBalanceSummary_WhenInvokedAsync()
        {
            int ownershipNodeId = 1;
            IEnumerable<OwnershipNodeBalanceSummary> ownershipNodeBalanceSummary = new List<OwnershipNodeBalanceSummary>();
            this.mockProcessor.Setup(m => m.GetOwnershipNodeBalanceSummaryAsync(It.IsAny<int>())).Returns(Task.FromResult(ownershipNodeBalanceSummary));
            var result = await this.controller.GetOwnershipNodeBalanceSummaryAsync(ownershipNodeId).ConfigureAwait(false);

            this.mockProcessor.Verify(c => c.GetOwnershipNodeBalanceSummaryAsync(It.IsAny<int>()), Times.Once());
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }

        [TestMethod]
        public async Task GetOwnershipNodeMovementInventoryDetails_ReturnsOwnershipNodeMovementInventoryDetails_WhenInvokedAsync()
        {
            int ownershipNodeId = 1;
            IEnumerable<OwnershipNodeMovementInventoryDetails> ownershipNodeMovementInventoryDetails = new List<OwnershipNodeMovementInventoryDetails>();
            this.mockProcessor.Setup(m => m.GetOwnerShipNodeMovementInventoryDetailsAsync(It.IsAny<int>())).Returns(Task.FromResult(ownershipNodeMovementInventoryDetails));
            var result = await this.controller.GetOwnershipNodeMovementInventoryDetailsAsync(ownershipNodeId).ConfigureAwait(false);

            this.mockProcessor.Verify(c => c.GetOwnerShipNodeMovementInventoryDetailsAsync(It.IsAny<int>()), Times.Once());
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }
    }
}