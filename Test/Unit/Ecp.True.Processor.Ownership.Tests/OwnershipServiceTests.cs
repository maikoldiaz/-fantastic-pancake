// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipServiceTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Ownership.Calculation.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core.Entities;
    using Ecp.True.DataAccess;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Processors.Ownership.Services;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The OwnershipServiceTests.
    /// </summary>
    [TestClass]
    public class OwnershipServiceTests
    {
        /// <summary>
        /// The mock registration strategy factory.
        /// </summary>
        private readonly Mock<IRegistrationStrategyFactory> mockRegistrationStrategyFactory = new Mock<IRegistrationStrategyFactory>();

        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The ownership service.
        /// </summary>
        private OwnershipService ownershipService;

        /// <summary>
        /// The mock ticket repository.
        /// </summary>
        private Mock<IRepository<OwnershipNode>> mockOwnershipNodeRepository;

        /// <summary>
        /// The mock ticket repository.
        /// </summary>
        private Mock<IRepository<Ticket>> mockTicketRepository;

        private Mock<ITrueLogger<OwnershipService>> mockLogger;

        /// <summary>
        /// The mock inventory ownership service.
        /// </summary>
        private Mock<IInventoryOwnershipService> mockInventoryOwnershipService = new Mock<IInventoryOwnershipService>();

        /// <summary>
        /// The mock movement ownership service.
        /// </summary>
        private Mock<IMovementOwnershipService> mockMovementOwnershipService = new Mock<IMovementOwnershipService>();

        /// <summary>
        /// The mock movement ownership service.
        /// </summary>
        private Mock<IOwnershipResultService> mockMovementService = new Mock<IOwnershipResultService>();

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockOwnershipNodeRepository = new Mock<IRepository<OwnershipNode>>();
            this.mockTicketRepository = new Mock<IRepository<Ticket>>();

            this.mockInventoryOwnershipService = new Mock<IInventoryOwnershipService>();
            this.mockMovementOwnershipService = new Mock<IMovementOwnershipService>();
            this.mockUnitOfWork.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));
            this.mockMovementService = new Mock<IOwnershipResultService>();

            this.unitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<OwnershipNode>()).Returns(this.mockOwnershipNodeRepository.Object);
            this.unitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);
            this.mockLogger = new Mock<ITrueLogger<OwnershipService>>();

            this.ownershipService = new OwnershipService(
                this.mockLogger.Object,
                this.unitOfWorkFactory.Object,
                this.mockInventoryOwnershipService.Object,
                this.mockMovementOwnershipService.Object,
                this.mockRegistrationStrategyFactory.Object,
                this.mockMovementService.Object);
        }

        /// <summary>
        /// Processes the ownership results asynchronous should process ownership results asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ProcessOwnershipResultsAsync_ShouldProcessOwnershipResultsAsync()
        {
            var ticketId = 12375;

            var onwrshipNodeOne = new OwnershipNode()
            {
                OwnershipNodeId = 12,
                TicketId = ticketId,
                NodeId = 12,
                Status = StatusType.PROCESSED,
            };

            var ownershipRuleData = new OwnershipRuleData()
            {
                OwnershipNodeId = 12,
                TicketId = ticketId,
            };

            this.mockOwnershipNodeRepository.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<OwnershipNode> { onwrshipNodeOne });
            this.mockOwnershipNodeRepository.Setup(m => m.UpdateAll(It.IsAny<IEnumerable<OwnershipNode>>()));
            this.mockTicketRepository.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Ticket());
            this.mockTicketRepository.Setup(m => m.Update(It.IsAny<Ticket>()));
            this.mockRegistrationStrategyFactory.Setup(m => m.OwnershipRegistrationStrategy.RegisterAsync(It.IsAny<object>(), It.IsAny<UnitOfWork>()));

            // Act
            await this.ownershipService.RegisterResultsAsync(ownershipRuleData).ConfigureAwait(false);

            // Assert
            this.unitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<OwnershipNode>(), Times.Once);
        }

        /// <summary>
        /// Consolidates the movement results should should add movements with owner identifier and applied rule.
        /// </summary>
        [TestMethod]
        public void ConsolidateMovementResults_ShouldShouldAddMovementsWithOwnerIdAndAppliedRule()
        {
            var movementResultList = new List<OwnershipResultMovement>()
            {
                new OwnershipResultMovement
                {
                    OwnerId = 12,
                    OwnershipPercentage = 100,
                    ResponseMovementId = "Test",
                },
            };

            var previousMovementOperationalData = new List<PreviousMovementOperationalData>()
            {
                new PreviousMovementOperationalData
                {
                    OwnershipPercentage = 100,
                    OwnerId = 13,
                    AppliedRule = 1,
                    MovementId = 1234,
                    OwnershipVolume = 1999,
                },
            };

            var result = this.ownershipService.ConsolidateMovementResults(movementResultList, previousMovementOperationalData, 1000);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        /// <summary>
        /// Consolidates the movement results should should add movements with owner identifier and applied rule.
        /// </summary>
        [TestMethod]
        public void ConsolidateMovementResults_ShouldShouldNotAddMovementsWithoutOwnerId()
        {
            var movementResultList = new List<OwnershipResultMovement>()
            {
                new OwnershipResultMovement
                {
                    OwnerId = 12,
                    OwnershipPercentage = 100,
                    ResponseMovementId = "Test",
                },
            };

            var previousMovementOperationalData = new List<PreviousMovementOperationalData>()
            {
                new PreviousMovementOperationalData
                {
                    OwnershipPercentage = 100,
                    AppliedRule = 1,
                    MovementId = 1234,
                    OwnershipVolume = 1999,
                },
            };

            var result = this.ownershipService.ConsolidateMovementResults(movementResultList, previousMovementOperationalData, 1000);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        /// <summary>
        /// Consolidates the inventory results should should not add inventory without owner identifier.
        /// </summary>
        [TestMethod]
        public void ConsolidateInventoryResults_ShouldShouldNotAddInventoryWithoutOwnerId()
        {
            var inventoryResultList = new List<OwnershipResultInventory>()
            {
                new OwnershipResultInventory
                {
                    OwnerId = 12,
                    OwnershipPercentage = 100,
                    ResponseInventoryId = "Test",
                },
            };

            var previousInventories = new List<PreviousInventoryOperationalData>()
            {
                new PreviousInventoryOperationalData
                {
                    OwnershipPercentage = 100,
                    OwnershipVolume = 1999,
                },
            };

            var result = this.ownershipService.ConsolidateInventoryResults(inventoryResultList, previousInventories, 1000);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void ConsolidateInventoryResults_ShouldShouldAddInventoryWithOwnerIdAndAppliedRule()
        {
            var inventoryResultList = new List<OwnershipResultInventory>()
            {
                new OwnershipResultInventory
                {
                    OwnerId = 12,
                    OwnershipPercentage = 100,
                    ResponseInventoryId = "Test",
                },
            };

            var previousInventories = new List<PreviousInventoryOperationalData>()
            {
                new PreviousInventoryOperationalData
                {
                    OwnershipPercentage = 100,
                    OwnershipVolume = 1999,
                    OwnerId = 12,
                    IsOwnershipCalculated = false,
                    InventoryId = 1234,
                    NodeId = 12,
                    ProductId = "Test",
                },
            };

            var result = this.ownershipService.ConsolidateInventoryResults(inventoryResultList, previousInventories, 1000);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        /// <summary>
        /// Updates the ticket status to failed should handle validation failur asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateTicketStatusToFailed_ShouldHandleValidationFailurAsync()
        {
            var ticketId = 12375;
            var errors = new List<ErrorInfo>();
            errors.Add(new ErrorInfo("Test"));
            var onwrshipNodeOne = new OwnershipNode()
            {
                OwnershipNodeId = 12,
                TicketId = ticketId,
                NodeId = 12,
                Status = StatusType.PROCESSED,
            };
            this.unitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));
            this.mockTicketRepository.Setup(a => a.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(new Ticket { TicketGroupId = "1" });
            this.mockOwnershipNodeRepository.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<OwnershipNode> { onwrshipNodeOne });
            this.mockTicketRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(new List<Ticket> { new Ticket { TicketId = 2 } });

            // Act
            await this.ownershipService.HandleFailureAsync(ticketId, errors, new List<OwnershipErrorMovement>(), new List<OwnershipErrorInventory>(), false).ConfigureAwait(false);

            // Assert
            this.mockTicketRepository.Verify(a => a.GetByIdAsync(It.IsAny<object>()), Times.Once);
            this.unitOfWorkFactory.Verify(a => a.GetUnitOfWork(), Times.Once);
            this.unitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Ticket>(), Times.Once);
            this.unitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
            this.mockTicketRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Ticket, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Updates the ticket status to failed should handle validation failure for ticket already processed asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetUnprocessedTicketsAsync_ShouldHGetUnprocessedTicketsAsync()
        {
            var ticketId = 12375;
            this.mockTicketRepository.Setup(a => a.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(new Ticket());
            this.mockTicketRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(It.IsAny<List<Ticket>>());

            // Act
            await this.ownershipService.GetUnprocessedTicketsAsync(ticketId).ConfigureAwait(false);

            // Assert
            this.unitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Ticket>(), Times.Once);
            this.mockTicketRepository.Verify(a => a.GetByIdAsync(It.IsAny<object>()), Times.Once);
            this.mockTicketRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Ticket, bool>>>()), Times.Once);
        }
    }
}