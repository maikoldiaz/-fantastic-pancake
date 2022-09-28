// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TicketControllerTests.cs" company="Microsoft">
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
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The ticket controller test class.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Api.Tests.ControllerTestBase" />
    [TestClass]
    public class TicketControllerTests : ControllerTestBase
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private TicketController controller;

        /// <summary>
        /// The ticket processor.
        /// </summary>
        private Mock<ITicketProcessor> ticketProcessor;

        /// <summary>
        /// The ownership ticket processor.
        /// </summary>
        private Mock<IOwnershipTicketProcessor> ownershipTicketProcessor;

        [TestInitialize]
        public void Initialize()
        {
            this.ticketProcessor = new Mock<ITicketProcessor>();
            this.ownershipTicketProcessor = new Mock<IOwnershipTicketProcessor>();
            this.controller = new TicketController(this.ticketProcessor.Object, this.ownershipTicketProcessor.Object);
        }

        /// <summary>
        /// Queries the tickets returns tickets when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task QueryTickets_ReturnsTickets_WhenInvokedAsync()
        {
            var tickets = new[] { new Ticket() }.AsQueryable();
            this.ticketProcessor.Setup(m => m.QueryAllAsync<Ticket>(null)).ReturnsAsync(tickets);

            var result = await this.controller.QueryTicketsAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, tickets);

            this.ticketProcessor.Verify(c => c.QueryAllAsync<Ticket>(null), Times.Once());
        }

        /// <summary>
        /// Gets the ticket info by identifier asynchronous should invoke processor to return ticket info asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetTicketInfoAsync_ShouldInvokeProcessor_ToReturnTicketInfoAsync()
        {
            this.ticketProcessor.Setup(m => m.GetTicketInfoAsync(23680)).ReturnsAsync(It.IsAny<TicketInfo>());
            var result = await this.controller.GetTicketInfoAsync(23680).ConfigureAwait(false);
            var entityResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(entityResult, typeof(EntityResult));
            Assert.IsNotNull(entityResult);
            this.ticketProcessor.Verify(c => c.GetTicketInfoAsync(23680), Times.Once());
        }

        /// <summary>
        /// Gets the ticket info by identifier asynchronous should invoke processor to return null for non existing tickets asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetTicketInfoAsync_ShouldInvokeProcessor_ToReturnNullAsync()
        {
            this.ticketProcessor.Setup(m => m.GetTicketInfoAsync(0000)).ReturnsAsync(It.IsAny<TicketInfo>());
            var result = await this.controller.GetTicketInfoAsync(0000).ConfigureAwait(false);
            var entityResult = result as EntityResult;

            // Assert
            Assert.IsNull(entityResult.Value);
            this.ticketProcessor.Verify(c => c.GetTicketInfoAsync(0000), Times.Once());
        }

        /// <summary>
        /// Gets unbalances for a ticket when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Validate_Cutoff_WhenInvokedAsync()
        {
            var ticket = new Ticket { TicketId = 1, CategoryElementId = 2, StartDate = DateTime.Now.AddDays(-2), EndDate = DateTime.Now, Status = StatusType.PROCESSED, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null };
            this.ticketProcessor.Setup(m => m.ValidateCutOffAsync(It.IsAny<Ticket>())).Returns(Task.FromResult(true));

            var result = await this.controller.ValidateCutOffAsync(ticket).ConfigureAwait(false);

            // Assert or verify
            this.ticketProcessor.Verify(c => c.ValidateCutOffAsync(It.IsAny<Ticket>()), Times.Once());
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }

        /// <summary>
        /// Validates the existing ticket should invoke processor to return validation result asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ValidateExistingTicket_ShouldInvokeProcessor_ToReturnValidationResultAsync()
        {
            var ticket = new Ticket();
            this.ownershipTicketProcessor.Setup(m => m.ValidateExistingTicketAsync(It.IsAny<Ticket>())).Returns(Task.FromResult(true));
            var result = await this.controller.ValidateExistingTicketAsync(ticket).ConfigureAwait(false);

            this.ownershipTicketProcessor.Verify(c => c.ValidateExistingTicketAsync(It.IsAny<Ticket>()), Times.Once());
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }

        /// <summary>
        /// Gets the ownership by segment should invoke processor to return ownership dates asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task GetOwnershipBySegment_ShouldInvokeProcessor_ToReturnOwnershipDatesAsync()
        {
            Dictionary<TicketType, DateTime?> ownershipDates = new Dictionary<TicketType, DateTime?>();
            this.ownershipTicketProcessor.Setup(m => m.GetOwnershipBySegmentAsync(It.IsAny<int>())).Returns(Task.FromResult(ownershipDates));
            var result = await this.controller.GetOwnershipBySegmentAsync(1).ConfigureAwait(false);

            this.ownershipTicketProcessor.Verify(c => c.GetOwnershipBySegmentAsync(It.IsAny<int>()), Times.Once());
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }

        /// <summary>
        /// Gets the ownership last performed date by segment asynchronous returns date when invokes asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetOwnershipLastPerformedDateBySegmentAsync_ReturnsDate_WhenInvokesAsync()
        {
            var segmentId = 10;
            var endDate = DateTime.UtcNow.AddDays(1).Date;
            this.ownershipTicketProcessor.Setup(m => m.GetOwnershipLastPerformedDateBySegmentAsync(It.IsAny<int>())).ReturnsAsync(endDate);

            var result = await this.controller.GetOwnershipLastPerformedDateBySegmentAsync(segmentId).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));

            this.ownershipTicketProcessor.Verify(c => c.GetOwnershipLastPerformedDateBySegmentAsync(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task GetTicketsView_ReturnsTickets_WhenInvokedAsync()
        {
            var tickets = new List<TicketEntity>() { new TicketEntity() }.AsQueryable();
            this.ownershipTicketProcessor.Setup(m => m.QueryViewAsync<TicketEntity>()).ReturnsAsync(tickets);
            var result = await this.controller.QueryTicketEntitiesAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(result, tickets);

            this.ownershipTicketProcessor.Verify(c => c.QueryViewAsync<TicketEntity>(), Times.Once());
        }

        [TestMethod]
        public async Task GetTicketsView_ReturnsNull_WhenInvokedAsync()
        {
            var tickets = new List<TicketEntity>() { new TicketEntity() }.AsQueryable();
            this.ownershipTicketProcessor.Setup(m => m.QueryViewAsync<TicketEntity>()).ReturnsAsync(tickets);
            var result = await this.controller.QueryTicketEntitiesAsync().ConfigureAwait(false);
            var entityResult = result as EntityResult;

            // Assert
            Assert.IsNull(entityResult, "Result should not be null");

            this.ownershipTicketProcessor.Verify(c => c.QueryViewAsync<TicketEntity>(), Times.Once());
        }

        /// <summary>
        /// Validates the cutoff already running asynchronous returns true when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task Validate_CutoffAlreadyRunningAsync_ReturnsTrue_WhenInvokedAsync()
        {
            this.ticketProcessor.Setup(m => m.ExistsTicketAsync(It.IsAny<Ticket>())).ReturnsAsync(true);
            var result = await this.controller.ExistsTicketAsync(new Ticket()).ConfigureAwait(false);
            var actionResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(true, actionResult.Value);
        }

        /// <summary>
        /// Validates the cutoff already running asynchronous returns false when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task Validate_CutoffAlreadyRunningAsync_ReturnsFalse_WhenInvokedAsync()
        {
            this.ticketProcessor.Setup(m => m.ExistsTicketAsync(It.IsAny<Ticket>())).ReturnsAsync(false);
            var result = await this.controller.ExistsTicketAsync(new Ticket()).ConfigureAwait(false);
            var actionResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(false, actionResult.Value);
        }

        /// <summary>
        /// Validates the delta already running asynchronous returns true when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task Validate_DeltaAlreadyRunningAsync_ReturnsTrue_WhenInvokedAsync()
        {
            this.ticketProcessor.Setup(m => m.ExistsDeltaTicketAsync(It.IsAny<int>())).ReturnsAsync(true);
            var result = await this.controller.ExistsDeltaTicketAsync(1).ConfigureAwait(false);
            var actionResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(true, actionResult.Value);
        }

        /// <summary>
        /// Validates the delta already running asynchronous returns false when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task Validate_DeltaAlreadyRunningAsync_ReturnsFalse_WhenInvokedAsync()
        {
            this.ticketProcessor.Setup(m => m.ExistsDeltaTicketAsync(It.IsAny<int>())).ReturnsAsync(false);
            var result = await this.controller.ExistsDeltaTicketAsync(1).ConfigureAwait(false);
            var actionResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(EntityResult));
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(false, actionResult.Value);
        }

        /// <summary>
        /// Creates the node asynchronous should invoke processor to return200 success asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task SaveOperationalCutOffAsync_ShouldInvokeProcessor_ToReturn200SuccessAsync()
        {
            var operationalCutOff = new OperationalCutOff();
            this.ticketProcessor.Setup(m => m.SaveTicketAsync(operationalCutOff));

            var result = await this.controller.SaveOperationalCutOffAsync(operationalCutOff).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.ticketProcessor.Verify(c => c.SaveTicketAsync(operationalCutOff), Times.Once());
        }

        [TestMethod]
        public async Task GetDeltaInventories_ShouldReturnDeltaInventories_IfTheyExistAsync()
        {
            var ticket = new Ticket
            {
                CategoryElementId = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10),
            };
            var inventories = new List<OperationalDeltaInventory>() { new OperationalDeltaInventory { InventoryId = "121" } }.AsQueryable();
            this.ownershipTicketProcessor.Setup(m => m.GetDeltaInventoriesAsync(ticket)).ReturnsAsync(inventories);
            var result = await this.controller.ExistsDeltaInventoriesAsync(ticket).ConfigureAwait(false);
            var entityResult = result as EntityResult;
            var inventoryResult = (IEnumerable<OperationalDeltaInventory>)entityResult.Value;

            // Assert
            this.ownershipTicketProcessor.Verify(c => c.GetDeltaInventoriesAsync(ticket), Times.Once());
            Assert.AreEqual(inventories.FirstOrDefault().InventoryId, inventoryResult.FirstOrDefault().InventoryId);
        }

        [TestMethod]
        public async Task GetDeltaInventories_ShouldReturnDeltaMovements_IfTheyExistAsync()
        {
            var ticket = new Ticket
            {
                CategoryElementId = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10),
            };
            var inventories = new List<OperationalDeltaMovement>() { new OperationalDeltaMovement { MovementId = "121" } }.AsQueryable();
            this.ownershipTicketProcessor.Setup(m => m.GetDeltaMovementsAsync(ticket)).ReturnsAsync(inventories);
            var result = await this.controller.ExistsDeltaMovementsAsync(ticket).ConfigureAwait(false);
            var entityResult = result as EntityResult;
            var inventoryResult = (IEnumerable<OperationalDeltaMovement>)entityResult.Value;

            // Assert
            this.ownershipTicketProcessor.Verify(c => c.GetDeltaMovementsAsync(ticket), Times.Once());
            Assert.AreEqual(inventories.FirstOrDefault().MovementId, inventoryResult.FirstOrDefault().MovementId);
        }

        [TestMethod]
        public async Task GetTicketProcessingStatusAsync_ShouldReturnTicketProcessingStatus_IfItExistsAsync()
        {
            var ticket = new Ticket
            {
                CategoryElementId = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10),
            };
            this.ownershipTicketProcessor.Setup(m => m.GetTicketProcessingStatusAsync(ticket.CategoryElementId, false)).ReturnsAsync("Cutoff");
            var result = await this.controller.GetTicketProcessingStatusAsync(ticket.CategoryElementId, false).ConfigureAwait(false);
            var entityResult = result as EntityResult;

            // Assert
            this.ownershipTicketProcessor.Verify(c => c.GetTicketProcessingStatusAsync(ticket.CategoryElementId, false), Times.Once());
            Assert.AreEqual(TicketType.Cutoff.ToString(), entityResult.Messagekey);
        }

        [TestMethod]
        public async Task UpdateCutOffComment_ShouldReturnUpdateCount_IfUpdateIsSuccessfulAsync()
        {
            var batch = new OperationalCutOffBatch();
            this.ticketProcessor.Setup(m => m.UpdateCommentAsync(batch));
            await this.controller.UpdateCommentAsync(batch).ConfigureAwait(false);
            this.ticketProcessor.Verify(c => c.UpdateCommentAsync(batch), Times.Once());
        }

        /// <summary>
        /// Gets the DeltaExceptions info by Ticket identifier asynchronous should invoke processor to return Exception info asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetDeltaExceptionsDetailsAsync_ShouldInvokeProcessor_ToReturnExceptionDetailsAsync()
        {
            var test = new List<DeltaExceptions> { new DeltaExceptions { Identifier = "2", Unit = "Data", Type = "Movement" }, new DeltaExceptions { Identifier = "1", Unit = "Data", Type = "Inventory" } };
            this.ticketProcessor.Setup(m => m.GetDeltaExceptionsDetailsAsync(23680, TicketType.Delta)).ReturnsAsync(test);
            var result = await this.controller.GetDeltaExceptionsDetailsAsync(23680, TicketType.Delta).ConfigureAwait(false);
            var entityResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(entityResult, typeof(EntityResult));
            Assert.IsNotNull(entityResult);
            this.ticketProcessor.Verify(c => c.GetDeltaExceptionsDetailsAsync(23680, TicketType.Delta), Times.Once());
        }

        /// <summary>
        /// Gets the DeltaExceptions info by Ticket identifier asynchronous should invoke processor to return Exception info asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ApproveOfficialNodeDeltaAsync_ShouldInvokeProcessor_ToReturnSuccessAsync()
        {
            var response = new DeltaNodeApprovalResponse() { IsApproverExist = true, IsValidOfficialDeltaNode = true };
            this.ticketProcessor.Setup(m => m.SendDeltaNodeForApprovalAsync(It.IsAny<DeltaNodeStatusRequest>())).ReturnsAsync(response);
            var result = await this.controller.SendDeltaNodeForApprovalAsync(new DeltaNodeStatusRequest() { NodeId = 1, SegmentId = 1 }).ConfigureAwait(false);
            var entityResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(entityResult, typeof(EntityResult));
            Assert.IsNotNull(entityResult);
            this.ticketProcessor.Verify(c => c.SendDeltaNodeForApprovalAsync(It.IsAny<DeltaNodeStatusRequest>()), Times.Once());
        }

        /// <summary>
        /// Gets the DeltaExceptions info by Ticket identifier asynchronous should invoke processor to return Exception info asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetDeltaNodesForReopenAsync_ShouldInvokeProcessor_ToReturnSuccessAsync()
        {
            var response = new List<DeltaNodeReopenResponse>() { new DeltaNodeReopenResponse() { Node = "Node", Segment = "Segment", DeltaNode = 1 } };
            this.ticketProcessor.Setup(m => m.GetDeltaNodesForReopenAsync(It.IsAny<int>())).ReturnsAsync(response);
            var result = await this.controller.GetDeltaNodesForReopenAsync(1).ConfigureAwait(false);
            var entityResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(entityResult, typeof(EntityResult));
            Assert.IsNotNull(entityResult);
            this.ticketProcessor.Verify(c => c.GetDeltaNodesForReopenAsync(It.IsAny<int>()), Times.Once());
        }

        /// <summary>
        /// Gets the DeltaExceptions info by Ticket identifier asynchronous should invoke processor to return Exception info asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ReopenDeltaNodeAsync_ShouldInvokeProcessor_ToReturnSuccessAsync()
        {
            this.ticketProcessor.Setup(m => m.ReopenDeltaNodesAsync(It.IsAny<DeltaNodeReopenRequest>())).Verifiable();
            var result = await this.controller.ReopenDeltaNodesAsync(new DeltaNodeReopenRequest() { DeltaNodeId = new List<int>() { 1 }, Comment = "Test" }).ConfigureAwait(false);
            var entityResult = result as EntityResult;

            // Assert
            Assert.IsInstanceOfType(entityResult, typeof(EntityResult));
            Assert.IsNotNull(entityResult);
            this.ticketProcessor.Verify(c => c.ReopenDeltaNodesAsync(It.IsAny<DeltaNodeReopenRequest>()), Times.Once());
        }
    }
}
