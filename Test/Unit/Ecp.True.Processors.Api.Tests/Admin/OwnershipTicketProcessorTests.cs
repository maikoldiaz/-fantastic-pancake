// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipTicketProcessorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Tests.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Api.Admin;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The ownership ticket processor tests.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Api.Tests.ProcessorTestBase" />
    [TestClass]
    public class OwnershipTicketProcessorTests : ProcessorTestBase
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private OwnershipTicketProcessor processor;

        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>
        /// The mock queue client.
        /// </summary>
        private Mock<IServiceBusQueueClient> mockQueueClient;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.mockQueueClient = new Mock<IServiceBusQueueClient>();
            this.mockQueueClient.Setup(c => c.QueueMessageAsync(It.IsAny<int>()));
            this.mockFactory.Setup(m => m.TicketInfoRepository.GetLastTicketIdAsync()).ReturnsAsync(10);

            this.processor = new OwnershipTicketProcessor(this.mockFactory.Object);
        }

        /// <summary>
        /// Gets the ownership last performed date by segment asynchronous should return date time when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The Task.
        /// </returns>
        [TestMethod]
        public async Task GetOwnershipLastPerformedDateBySegmentAsync_ShouldReturnDateTime_WhenInvokedAsync()
        {
            var segmentId = 2;
            var ticket = new Ticket { TicketId = 23678, CategoryElementId = 2, StartDate = new DateTime(2019, 02, 12), EndDate = new DateTime(2019, 02, 16), Status = 0, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null };
            var mockTicketRepository = new Mock<IRepository<Ticket>>();
            mockTicketRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(new List<Ticket> { ticket });
            this.mockFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(mockTicketRepository.Object);

            var result = await this.processor.GetOwnershipLastPerformedDateBySegmentAsync(segmentId).ConfigureAwait(false);

            Assert.AreEqual(result, ticket.EndDate);
            mockTicketRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Ticket, bool>>>()), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<Ticket>(), Times.Once);
        }

        /// <summary>
        /// Validates the existing ticket when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Validate_Existing_Ticket_WhenInvokedAsync()
        {
            var ticket = new Ticket { CategoryElementId = 1, StartDate = DateTime.UtcNow.AddDays(-2), EndDate = DateTime.UtcNow };
            var repoMock = new Mock<IRepository<Ticket>>();
            repoMock.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(1);
            this.mockFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(repoMock.Object);
            var result = await this.processor.ValidateExistingTicketAsync(ticket).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<Ticket>(), Times.Once);
            repoMock.Verify(m => m.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Gets the ownership by segment asynchronous when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetOwnershipBySegmentAsync_WhenInvokedAsync()
        {
            Dictionary<TicketType, DateTime?> ownershipDates = new Dictionary<TicketType, DateTime?>();
            this.mockFactory.Setup(m => m.TicketInfoRepository.GetOwnershipBySegmentAsync(1)).ReturnsAsync(ownershipDates);
            var result = await this.processor.GetOwnershipBySegmentAsync(1).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockFactory.Verify(m => m.TicketInfoRepository.GetOwnershipBySegmentAsync(It.IsAny<int>()), Times.Once);
        }

        /// <summary>
        /// Gets the delta inventories when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetDeltaInventories_ShouldGetInventories_WhenInvokedAsync()
        {
            var ticket = new Ticket
            {
                CategoryElementId = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10),
            };
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var parameters = new Dictionary<string, object>
            {
                { "@Segmentid", ticket.CategoryElementId },
                { "@StartDate", ticket.StartDate },
                { "@EndDate", ticket.EndDate },
                { "@IsOriginal", false },
            };

            IEnumerable<OperationalDeltaInventory> inventoryList = new List<OperationalDeltaInventory>() { new OperationalDeltaInventory { InventoryId = "121" } };

            this.mockFactory.Setup(m => m.CreateRepository<OperationalDeltaInventory>().ExecuteQueryAsync(
                Repositories.Constants.OriginalOrUpdatedInventoriesProcedureName, parameters)).Returns(Task.FromResult(inventoryList));
            var result = await this.processor.GetDeltaInventoriesAsync(ticket).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<OperationalDeltaInventory>().ExecuteQueryAsync(Repositories.Constants.OriginalOrUpdatedInventoriesProcedureName, parameters), Times.Once);
            Assert.AreEqual(inventoryList.FirstOrDefault().InventoryId, result.FirstOrDefault().InventoryId);
        }

        /// <summary>
        /// Gets the delta movements when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetDeltaInventories_ShouldGetMovements_WhenInvokedAsync()
        {
            var ticket = new Ticket
            {
                CategoryElementId = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10),
            };
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var parameters = new Dictionary<string, object>
            {
                { "@Segmentid", ticket.CategoryElementId },
                { "@StartDate", ticket.StartDate },
                { "@EndDate", ticket.EndDate },
                { "@IsOriginal", false },
            };

            IEnumerable<OperationalDeltaMovement> inventoryList = new List<OperationalDeltaMovement>() { new OperationalDeltaMovement { MovementId = "121" } };

            this.mockFactory.Setup(m => m.CreateRepository<OperationalDeltaMovement>().ExecuteQueryAsync(
                Repositories.Constants.OriginalOrUpdatedMovementsProcedureName, parameters)).Returns(Task.FromResult(inventoryList));
            var result = await this.processor.GetDeltaMovementsAsync(ticket).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockFactory.Verify(m => m.CreateRepository<OperationalDeltaMovement>().ExecuteQueryAsync(Repositories.Constants.OriginalOrUpdatedMovementsProcedureName, parameters), Times.Once);
            Assert.AreEqual(inventoryList.FirstOrDefault().MovementId, result.FirstOrDefault().MovementId);
        }

        [TestMethod]
        public async Task GetTicketProcessingStatusAsync_ShouldNotReturnOwnershipProcessingStatus_IfTicketsUnderProcessingStateExistAsync()
        {
            var ticket = new Ticket
            {
                CategoryElementId = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10),
                Status = StatusType.PROCESSING,
                TicketTypeId = TicketType.Ownership,
            };

            IEnumerable<Ticket> tickets = new List<Ticket> { ticket };
            var repoMock = new Mock<IRepository<Ticket>>();
            this.mockFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(repoMock.Object);
            repoMock.Setup(r => r.GetAllAsync(a =>
            a.CategoryElementId == 1 &&
            a.Status == StatusType.PROCESSING)).ReturnsAsync(tickets);
            this.mockFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(repoMock.Object);
            var result = await this.processor.GetTicketProcessingStatusAsync(1, false).ConfigureAwait(false);

            // Assert
            Assert.AreNotEqual(TicketType.Ownership.ToString(), result);
        }

        [TestMethod]
        public async Task GetTicketProcessingStatusAsync_ShouldReturnCutOffProcessingStatus_IfTicketsUnderProcessingStateExistAsync()
        {
            var ticket = new Ticket
            {
                CategoryElementId = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10),
                Status = StatusType.PROCESSING,
                TicketTypeId = TicketType.Cutoff,
            };

            IEnumerable<Ticket> tickets = new List<Ticket> { ticket };
            var repoMock = new Mock<IRepository<Ticket>>();
            repoMock.Setup(r => r.GetAllAsync(a =>
            a.CategoryElementId == 1 &&
            a.Status == StatusType.PROCESSING &&
            (a.TicketTypeId == TicketType.Cutoff || a.TicketTypeId == TicketType.Delta))).Returns(Task.FromResult(tickets));
            this.mockFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(repoMock.Object);
            var result = await this.processor.GetTicketProcessingStatusAsync(1, false).ConfigureAwait(false);

            // Assert
            Assert.AreNotEqual(TicketType.Cutoff.ToString(), result);
        }

        [TestMethod]
        public async Task GetTicketProcessingStatusAsync_ShouldNotReturnProcessingStatus_IfTicketsUnderProcessingStateDontExistAsync()
        {
            var ticket = new Ticket();

            IEnumerable<Ticket> tickets = new List<Ticket> { ticket };
            var repoMock = new Mock<IRepository<Ticket>>();
            repoMock.Setup(r => r.GetAllAsync(a =>
            a.CategoryElementId == 1 &&
            a.Status == StatusType.PROCESSING &&
            (a.TicketTypeId == TicketType.Cutoff || a.TicketTypeId == TicketType.Delta))).Returns(Task.FromResult(tickets));
            this.mockFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(repoMock.Object);
            var result = await this.processor.GetTicketProcessingStatusAsync(1, false).ConfigureAwait(false);

            // Assert
            Assert.AreNotEqual(TicketType.Ownership.ToString(), result);
        }

        [TestMethod]
        [Ignore("Will fix later")]
        public async Task GetTicketProcessingStatusAsync_ShouldReturnCutoffProcessingStatus_IfTicketsUnderProcessingForBothOwnershipAndCutoffExistAsync()
        {
            IEnumerable<Ticket> tickets = new List<Ticket>
            {
                new Ticket
                {
                CategoryElementId = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10),
                Status = StatusType.PROCESSING,
                TicketTypeId = TicketType.Cutoff,
                },
            };
            var repoMock = new Mock<IRepository<Ticket>>();
            repoMock.Setup(r => r.GetAllAsync(a =>
            a.CategoryElementId == tickets.FirstOrDefault().CategoryElementId &&
            a.Status == StatusType.PROCESSING)).ReturnsAsync(tickets);
            this.mockFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(repoMock.Object);
            var result = await this.processor.GetTicketProcessingStatusAsync(tickets.FirstOrDefault().CategoryElementId, false).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(TicketType.Cutoff.ToString(), result);
        }

        [TestMethod]
        [Ignore("Will fix later")]
        public async Task GetTicketProcessingStatusAsync_ShouldReturnOwnershipProcessingStatus_IfOwnershipTicketTypeIsPassedAsync()
        {
            IEnumerable<Ticket> tickets = new List<Ticket>
            {
                new Ticket
                {
                CategoryElementId = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10),
                Status = StatusType.PROCESSING,
                TicketTypeId = TicketType.Cutoff,
                },
            };
            var repoMock = new Mock<IRepository<Ticket>>();
            repoMock.Setup(r => r.GetAllAsync(a =>
            a.CategoryElementId == tickets.FirstOrDefault().CategoryElementId &&
            a.Status == StatusType.PROCESSING)).ReturnsAsync(tickets);
            this.mockFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(repoMock.Object);
            var result = await this.processor.GetTicketProcessingStatusAsync(tickets.FirstOrDefault().CategoryElementId, true).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(TicketType.Cutoff.ToString(), result);
        }
    }
}
