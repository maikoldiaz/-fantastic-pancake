// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeOwnershipProcessorTests.cs" company="Microsoft">
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
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Api.Admin;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.OwnershipRules.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class NodeOwnershipProcessorTests : ProcessorTestBase
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private NodeOwnershipProcessor processor;

        /// <summary>
        /// The mock repository factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockRepositoryFactory;

        /// <summary>
        /// The mock configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigurationHandler;

        /// <summary>
        /// The mock business context.
        /// </summary>
        private Mock<IBusinessContext> mockBusinessContext;

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureClientFactory;

        /// <summary>
        /// The mock business context.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The mock ownership rule service.
        /// </summary>
        private Mock<IOwnershipRuleProxy> mockOwnershipRuleService;

        /// <summary>
        /// The mock communicator.
        /// </summary>
        private Mock<ICommunicator> mockCommunicator;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockOwnershipRuleService = new Mock<IOwnershipRuleProxy>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockRepositoryFactory = new Mock<IRepositoryFactory>();
            this.mockConfigurationHandler = new Mock<IConfigurationHandler>();
            this.mockBusinessContext = new Mock<IBusinessContext>();
            this.mockAzureClientFactory = new Mock<IAzureClientFactory>();
            this.mockCommunicator = new Mock<ICommunicator>();

            this.processor = new NodeOwnershipProcessor(
                                    this.mockRepositoryFactory.Object,
                                    this.mockUnitOfWorkFactory.Object,
                                    this.mockAzureClientFactory.Object,
                                    this.mockBusinessContext.Object,
                                    this.mockOwnershipRuleService.Object,
                                    this.mockCommunicator.Object,
                                    this.mockConfigurationHandler.Object);
        }

        /// <summary>
        /// Reopens the ticket asynchronous reopen ticket when invoked asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task ReopenTicketAsync_ReopenTicket_WhenInvokedAsync()
        {
            var reopenTicket = new ReopenTicket()
            {
                OwnershipNodeId = 297,
                Message = string.Empty,
                Status = "APPROVED",
            };

            var repoMock = new Mock<IRepository<OwnershipNode>>();
            repoMock.Setup(r => r.GetByIdAsync(reopenTicket.OwnershipNodeId)).ReturnsAsync(new OwnershipNode());
            repoMock.Setup(r => r.Update(It.IsAny<OwnershipNode>()));
            var token = new CancellationToken(false);

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<OwnershipNode>()).Returns(repoMock.Object);

            await this.processor.ReopenOwnershipNodeAsync(reopenTicket).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<OwnershipNode>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Once);
            repoMock.Verify(r => r.Update(It.IsAny<OwnershipNode>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task ReopenTicketAsync_ThrowKeyNotFoundException_WhenInvokedAsync()
        {
            var reopenTicket = new ReopenTicket()
            {
                OwnershipNodeId = 297,
                Message = string.Empty,
                Status = "REOPENED",
            };

            var repoMock = new Mock<IRepository<OwnershipNode>>();
            repoMock.Setup(r => r.GetByIdAsync(reopenTicket.OwnershipNodeId)).ReturnsAsync(new OwnershipNode());
            repoMock.Setup(r => r.Update(It.IsAny<OwnershipNode>()));
            var token = new CancellationToken(false);

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<OwnershipNode>()).Returns(repoMock.Object);

            await this.processor.ReopenOwnershipNodeAsync(reopenTicket).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<OwnershipNode>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Once);
            repoMock.Verify(r => r.Update(It.IsAny<OwnershipNode>()), Times.Once);
        }

        /// <summary>
        /// Gets the ownership node identifier asynchronous should ownership node from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetOwnershipNodeIdAsync_ShouldOwnershipNodeFromRepository_WhenInvokedAsync()
        {
            var ownershipNodeRepo = new Mock<IRepository<OwnershipNode>>();
            ownershipNodeRepo.Setup(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new OwnershipNode());
            this.mockRepositoryFactory.Setup(a => a.CreateRepository<OwnershipNode>()).Returns(ownershipNodeRepo.Object);

            var result = await this.processor.GetOwnershipNodeIdAsync(1).ConfigureAwait(false);

            ownershipNodeRepo.Verify(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            this.mockRepositoryFactory.Verify(a => a.CreateRepository<OwnershipNode>(), Times.Once);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Gets the is synchronize in progress should return the status of synchronize when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetIsSyncInProgress_ShouldReturnTheStatusOfSync_whenInvokedAsync()
        {
            var ownershipRuleRefreshHistory = new Mock<IRepository<OwnershipRuleRefreshHistory>>();
            ownershipRuleRefreshHistory.Setup(a => a.FirstOrDefaultAsync(It.IsAny<Expression<Func<OwnershipRuleRefreshHistory, bool>>>()));
            this.mockRepositoryFactory.Setup(a => a.CreateRepository<OwnershipRuleRefreshHistory>()).Returns(ownershipRuleRefreshHistory.Object);
            var result = await this.processor.IsSyncInProgressAsync().ConfigureAwait(false);
            ownershipRuleRefreshHistory.Verify(a => a.FirstOrDefaultAsync(It.IsAny<Expression<Func<OwnershipRuleRefreshHistory, bool>>>()), Times.Once);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Updates the ownership node status asynchronous should update ownership node when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateOwnershipNodeStatusAsync_ShouldUpdateOwnershipNode_WhenInvokedAsync()
        {
            var ownershipNodeRepo = new Mock<IRepository<OwnershipNode>>();
            ownershipNodeRepo.Setup(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>())).ReturnsAsync(new OwnershipNode());
            ownershipNodeRepo.Setup(a => a.Update(It.IsAny<OwnershipNode>()));

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(a => a.CreateRepository<OwnershipNode>()).Returns(ownershipNodeRepo.Object);
            mockUnitOfWork.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));
            this.mockUnitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(mockUnitOfWork.Object);

            await this.processor.UpdateOwnershipNodeStatusAsync(1, new OwnershipNode()).ConfigureAwait(false);

            mockUnitOfWork.Verify(a => a.CreateRepository<OwnershipNode>(), Times.Once);
            ownershipNodeRepo.Verify(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>()), Times.Once);
            ownershipNodeRepo.Verify(a => a.Update(It.IsAny<OwnershipNode>()), Times.Once);
            mockUnitOfWork.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
            this.mockUnitOfWorkFactory.Verify(a => a.GetUnitOfWork(), Times.Once);
        }

        /// <summary>
        /// Updates the ownership node status asynchronous should throw key not found exception when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task UpdateOwnershipNodeStatusAsync_ShouldThrowKeyNotFoundException_WhenInvokedAsync()
        {
            var ownershipNodeRepo = new Mock<IRepository<OwnershipNode>>();
            ownershipNodeRepo.Setup(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>())).ReturnsAsync(() => null);
            ownershipNodeRepo.Setup(a => a.Update(It.IsAny<OwnershipNode>()));

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(a => a.CreateRepository<OwnershipNode>()).Returns(ownershipNodeRepo.Object);
            mockUnitOfWork.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));
            this.mockUnitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(mockUnitOfWork.Object);

            await this.processor.UpdateOwnershipNodeStatusAsync(1, new OwnershipNode()).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the movement owners data in repository when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetOwnersForMovementAsync_ShouldGetOwnerDataAsync()
        {
            var movementOwners = new List<NodeConnectionProductOwner>();
            movementOwners.Add(new NodeConnectionProductOwner { NodeConnectionProductOwnerId = 100, NodeConnectionProductId = 10, IsDeleted = false });
            this.mockRepositoryFactory.Setup(m => m.NodeOwnershipRepository.GetOwnersForMovementAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(movementOwners);

            var result = await this.processor.GetOwnersForMovementAsync(10, 20, "tetsProductId").ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);

            this.mockRepositoryFactory.Verify(f => f.NodeOwnershipRepository.GetOwnersForMovementAsync(10, 20, "tetsProductId"), Times.Once());
        }

        /// <summary>
        /// Get the inventory owners data in repository when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetOwnersForInventoryAsync_ShouldGetOwnerDataAsync()
        {
            var inventoryOwners = new List<StorageLocationProductOwner>();
            inventoryOwners.Add(new StorageLocationProductOwner { StorageLocationProductOwnerId = 100, OwnershipPercentage = 100, StorageLocationProductId = 10, IsDeleted = false });
            this.mockRepositoryFactory.Setup(m => m.NodeOwnershipRepository.GetOwnersForInventoryAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(inventoryOwners);

            var result = await this.processor.GetOwnersForInventoryAsync(10, "tetsProductId").ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);

            this.mockRepositoryFactory.Verify(f => f.NodeOwnershipRepository.GetOwnersForInventoryAsync(10, "tetsProductId"), Times.Once());
        }

        /// <summary>
        /// Determines whether [is synchronize in progress should get status of in progress record asynchronous].
        /// </summary>
        /// <returns> The status of Inprogress.</returns>
        [TestMethod]
        public async Task IsSyncInProgress_ShouldGetStatusOfInProgressRecordAsync()
        {
            var ownershipruleRefreshHistory = new OwnershipRuleRefreshHistory() { Status = true };
            var ownershipRuleRefreshHistoryRepository = new Mock<IRepository<OwnershipRuleRefreshHistory>>();
            ownershipRuleRefreshHistoryRepository.Setup(a => a.FirstOrDefaultAsync(It.IsAny<Expression<Func<OwnershipRuleRefreshHistory, bool>>>(), It.IsAny<string[]>())).Returns(Task.FromResult(ownershipruleRefreshHistory));
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<OwnershipRuleRefreshHistory>()).Returns(ownershipRuleRefreshHistoryRepository.Object);
            var result = await this.processor.IsSyncInProgressAsync().ConfigureAwait(false);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Gets the conditional ownership node by identifier asynchronous should return ownership node asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task GetConditionalOwnershipNodeByIdAsync_ShouldReturnOwnershipNodeAsync()
        {
            var ownershipNode = new OwnershipNode
            {
                OwnershipNodeId = 12,
                TicketId = 123,
                OwnershipStatus = OwnershipNodeStatusType.LOCKED,
            };
            var ownershipNodeRepository = new Mock<IRepository<OwnershipNode>>();
            ownershipNodeRepository.Setup(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(ownershipNode);

            var ticket = new Ticket
            {
                TicketTypeId = TicketType.Ownership,
                TicketId = 123,
            };

            var mockTicketRepository = new Mock<IRepository<Ticket>>();
            mockTicketRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(new List<Ticket> { ticket });

            this.mockRepositoryFactory.Setup(m => m.CreateRepository<OwnershipNode>()).Returns(ownershipNodeRepository.Object);
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(mockTicketRepository.Object);

            var result = await this.processor.GetConditionalOwnershipNodeByIdAsync(ownershipNode.OwnershipNodeId).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(12, result.OwnershipNodeId);
            Assert.AreEqual(123, result.TicketId);

            this.mockRepositoryFactory.Verify(m => m.CreateRepository<OwnershipNode>(), Times.Once);
            this.mockRepositoryFactory.Verify(m => m.CreateRepository<Ticket>(), Times.Once);
            ownershipNodeRepository.Verify(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>(), It.IsAny<string[]>()), Times.Once);
            mockTicketRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Ticket, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Gets the conditional ownership node by identifier asynchronous should return null for ownership node does not exists asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task GetConditionalOwnershipNodeByIdAsync_ShouldReturnKeyNotFoundExceptionForOwnershipNodeDoesNotExistsAsync()
        {
            var ownershipNodeRepository = new Mock<IRepository<OwnershipNode>>();
            ownershipNodeRepository.Setup(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(() => null);

            var ticket = new Ticket
            {
                TicketTypeId = TicketType.Ownership,
                TicketId = 123,
            };

            var mockTicketRepository = new Mock<IRepository<Ticket>>();
            mockTicketRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(new List<Ticket> { ticket });

            this.mockRepositoryFactory.Setup(m => m.CreateRepository<OwnershipNode>()).Returns(ownershipNodeRepository.Object);
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(mockTicketRepository.Object);

            await this.processor.GetConditionalOwnershipNodeByIdAsync(123).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the conditional ownership node by identifier asynchronous should return empty for ownership node status sent asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public async Task GetConditionalOwnershipNodeByIdAsync_ShouldReturn_UnauthorizedAccessException_ForOwnershipNodeStatus_Sent_Async()
        {
            var ownershipNode = new OwnershipNode
            {
                OwnershipNodeId = 12,
                TicketId = 123,
                OwnershipStatus = OwnershipNodeStatusType.SENT,
            };
            var ownershipNodeRepository = new Mock<IRepository<OwnershipNode>>();
            ownershipNodeRepository.Setup(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(ownershipNode);

            var ticket = new Ticket
            {
                TicketTypeId = TicketType.Ownership,
                TicketId = 123,
            };

            var mockTicketRepository = new Mock<IRepository<Ticket>>();
            mockTicketRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(new List<Ticket> { ticket });

            this.mockRepositoryFactory.Setup(m => m.CreateRepository<OwnershipNode>()).Returns(ownershipNodeRepository.Object);
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(mockTicketRepository.Object);

            await this.processor.GetConditionalOwnershipNodeByIdAsync(123).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the conditional ownership node by identifier asynchronous should return empty for ownership node status failed asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public async Task GetConditionalOwnershipNodeByIdAsync_ShouldReturn_UnauthorizedAccessException_ForOwnershipNodeStatus_Failed_Async()
        {
            var ownershipNode = new OwnershipNode
            {
                OwnershipNodeId = 12,
                TicketId = 123,
                OwnershipStatus = OwnershipNodeStatusType.FAILED,
            };
            var ownershipNodeRepository = new Mock<IRepository<OwnershipNode>>();
            ownershipNodeRepository.Setup(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(ownershipNode);

            var ticket = new Ticket
            {
                TicketTypeId = TicketType.Ownership,
                TicketId = 123,
            };

            var mockTicketRepository = new Mock<IRepository<Ticket>>();
            mockTicketRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(new List<Ticket> { ticket });

            this.mockRepositoryFactory.Setup(m => m.CreateRepository<OwnershipNode>()).Returns(ownershipNodeRepository.Object);
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(mockTicketRepository.Object);

            await this.processor.GetConditionalOwnershipNodeByIdAsync(123).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the conditional ownership node by identifier asynchronous should return empty for ownership node status publishing asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public async Task GetConditionalOwnershipNodeByIdAsync_ShouldReturn_UnauthorizedAccessException_ForOwnershipNodeStatus_Publishing_Async()
        {
            var ownershipNode = new OwnershipNode
            {
                OwnershipNodeId = 12,
                TicketId = 123,
                OwnershipStatus = OwnershipNodeStatusType.PUBLISHING,
            };
            var ownershipNodeRepository = new Mock<IRepository<OwnershipNode>>();
            ownershipNodeRepository.Setup(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(ownershipNode);

            var ticket = new Ticket
            {
                TicketTypeId = TicketType.Ownership,
                TicketId = 123,
            };

            var mockTicketRepository = new Mock<IRepository<Ticket>>();
            mockTicketRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(new List<Ticket> { ticket });

            this.mockRepositoryFactory.Setup(m => m.CreateRepository<OwnershipNode>()).Returns(ownershipNodeRepository.Object);
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(mockTicketRepository.Object);

            await this.processor.GetConditionalOwnershipNodeByIdAsync(123).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the conditional ownership node by identifier asynchronous should return empty no latest ticket with ticket type ownership asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public async Task GetConditionalOwnershipNodeByIdAsync_ShouldReturn_UnauthorizedAccessException_LatestTicket_With_TicketType_Ownership_Async()
        {
            var ownershipNode = new OwnershipNode
            {
                OwnershipNodeId = 12,
                TicketId = 123,
                OwnershipStatus = OwnershipNodeStatusType.PUBLISHING,
            };
            var ownershipNodeRepository = new Mock<IRepository<OwnershipNode>>();
            ownershipNodeRepository.Setup(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(ownershipNode);

            var ticket = new Ticket
            {
                TicketTypeId = TicketType.Cutoff,
                TicketId = 123,
            };

            var mockTicketRepository = new Mock<IRepository<Ticket>>();
            mockTicketRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(new List<Ticket> { ticket });

            this.mockRepositoryFactory.Setup(m => m.CreateRepository<OwnershipNode>()).Returns(ownershipNodeRepository.Object);
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<Ticket>()).Returns(mockTicketRepository.Object);

            await this.processor.GetConditionalOwnershipNodeByIdAsync(123).ConfigureAwait(false);
        }

        /// <summary>
        /// Validates the ownership nodes should query from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ValidateOwnershipNodes_ShouldQueryFromRepository_WhenInvokedAsync()
        {
            var ticket = new Ticket { CategoryElementId = 1, StartDate = DateTime.UtcNow.AddDays(-2), EndDate = DateTime.UtcNow };
            IEnumerable<OwnershipValidationResult> validationResult = new List<OwnershipValidationResult>();
            var repoMock = new Mock<IRepository<OwnershipValidationResult>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(validationResult);
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<OwnershipValidationResult>()).Returns(repoMock.Object);
            var result = await this.processor.ValidateOwnershipNodesAsync(ticket).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockRepositoryFactory.Verify(m => m.CreateRepository<OwnershipValidationResult>(), Times.Once);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Gets the ownership node balance summary should query from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetOwnershipNodeBalanceSummary_ShouldQueryFromRepository_WhenInvokedAsync()
        {
            int ownershipNodeId = 1;
            IEnumerable<OwnershipNodeBalanceSummary> ownershipNodeBalanceSummaryResult = new List<OwnershipNodeBalanceSummary>();
            var repoMock = new Mock<IRepository<OwnershipNodeBalanceSummary>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(ownershipNodeBalanceSummaryResult);
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<OwnershipNodeBalanceSummary>()).Returns(repoMock.Object);
            var result = await this.processor.GetOwnershipNodeBalanceSummaryAsync(ownershipNodeId).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockRepositoryFactory.Verify(m => m.CreateRepository<OwnershipNodeBalanceSummary>(), Times.Once);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Gets the ownership node movement inventory details should query from repository when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetOwnershipNodeMovementInventoryDetails_ShouldQueryFromRepository_WhenInvokedAsync()
        {
            int ownershipNodeId = 1;
            IEnumerable<OwnershipNodeMovementInventoryDetails> ownershipNodeMovementInventoryDetailsResult = new List<OwnershipNodeMovementInventoryDetails>();
            var repoMock = new Mock<IRepository<OwnershipNodeMovementInventoryDetails>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(ownershipNodeMovementInventoryDetailsResult);
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<OwnershipNodeMovementInventoryDetails>()).Returns(repoMock.Object);
            var result = await this.processor.GetOwnerShipNodeMovementInventoryDetailsAsync(ownershipNodeId).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockRepositoryFactory.Verify(m => m.CreateRepository<OwnershipNodeMovementInventoryDetails>(), Times.Once);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }
    }
}