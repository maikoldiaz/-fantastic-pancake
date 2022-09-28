// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidationProcessorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Delta.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Delta;
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ConsolidationProcessorTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<ConsolidationProcessor>> mockLogger;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureClientFactory;

        /// <summary>
        /// The mock service bus queue client.
        /// </summary>
        private Mock<IServiceBusQueueClient> mockServiceBusQueueClient;

        /// <summary>
        /// The mock consolidation strategy factory.
        /// </summary>
        private Mock<IConsolidationStrategyFactory> mockConsolidationStrategyFactory;

        /// <summary>
        /// The consolidation processor.
        /// </summary>
        private ConsolidationProcessor consolidationProcessor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<ConsolidationProcessor>>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockAzureClientFactory = new Mock<IAzureClientFactory>();
            this.mockServiceBusQueueClient = new Mock<IServiceBusQueueClient>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();

            this.mockUnitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockConsolidationStrategyFactory = new Mock<IConsolidationStrategyFactory>();

            this.mockServiceBusQueueClient.Setup(a => a.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()));
            this.mockAzureClientFactory.Setup(a => a.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);

            this.consolidationProcessor = new ConsolidationProcessor(
                this.mockLogger.Object,
                this.mockUnitOfWorkFactory.Object,
                this.mockAzureClientFactory.Object,
                this.mockConsolidationStrategyFactory.Object);
        }

        /// <summary>
        /// Consolidates the asynchronous should consolidate data when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ConsolidateAsync_ShouldConsolidateData_WhenInvokedAsync()
        {
            var consolidationBatch = new ConsolidationBatch { Ticket = new Ticket { TicketId = 123 } };

            var mockMovementConsolidationStrategy = new Mock<IConsolidationStrategy>();
            mockMovementConsolidationStrategy.Setup(a => a.ConsolidateAsync(It.IsAny<ConsolidationBatch>(), It.IsAny<IUnitOfWork>()));

            var mockInventoryProductConsolidationStrategy = new Mock<IConsolidationStrategy>();
            mockInventoryProductConsolidationStrategy.Setup(a => a.ConsolidateAsync(It.IsAny<ConsolidationBatch>(), It.IsAny<IUnitOfWork>()));

            this.mockConsolidationStrategyFactory.Setup(a => a.MovementConsolidationStrategy).Returns(mockMovementConsolidationStrategy.Object);
            this.mockConsolidationStrategyFactory.Setup(a => a.InventoryProductConsolidationStrategy).Returns(mockInventoryProductConsolidationStrategy.Object);

            await this.consolidationProcessor.ConsolidateAsync(consolidationBatch).ConfigureAwait(false);

            mockMovementConsolidationStrategy.Verify(a => a.ConsolidateAsync(It.IsAny<ConsolidationBatch>(), It.IsAny<IUnitOfWork>()), Times.Once);
            mockInventoryProductConsolidationStrategy.Verify(a => a.ConsolidateAsync(It.IsAny<ConsolidationBatch>(), It.IsAny<IUnitOfWork>()), Times.Once);
            this.mockConsolidationStrategyFactory.Verify(a => a.MovementConsolidationStrategy, Times.Once);
            this.mockConsolidationStrategyFactory.Verify(a => a.InventoryProductConsolidationStrategy, Times.Once);
        }

        /// <summary>
        /// Gets the consolidation batches should return consolidation batch for even nodes when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetConsolidationBatches_ShouldReturn_5_ConsolidationBatchFor_100_Nodes_WhenInvokedAsync()
        {
            var date = new DateTime(2020, 07, 06);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var ticket = new Ticket
            {
                TicketId = 123,
                StartDate = firstDayOfMonth,
                EndDate = firstDayOfMonth.AddMonths(1).AddDays(-1),
            };

            var consolidationNodeDatas = new List<ConsolidationNodeData>();

            for (var i = 1; i <= 100; i++)
            {
                consolidationNodeDatas.Add(new ConsolidationNodeData { SourceNodeId = i, DestinationNodeId = i });
            }

            var mockConsolidationNodeDataRepository = new Mock<IRepository<ConsolidationNodeData>>();
            mockConsolidationNodeDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(consolidationNodeDatas);

            var mockConsolidatedMovementRepository = new Mock<IRepository<ConsolidatedMovement>>();
            mockConsolidatedMovementRepository.Setup(a => a.GetCountAsync(It.IsAny<Expression<Func<ConsolidatedMovement, bool>>>())).ReturnsAsync(0);

            var mockConsolidatedInventoryProductRepository = new Mock<IRepository<ConsolidatedInventoryProduct>>();
            mockConsolidatedInventoryProductRepository.Setup(a => a.GetCountAsync(It.IsAny<Expression<Func<ConsolidatedInventoryProduct, bool>>>())).ReturnsAsync(0);

            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidationNodeData>()).Returns(mockConsolidationNodeDataRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedMovement>()).Returns(mockConsolidatedMovementRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedInventoryProduct>()).Returns(mockConsolidatedInventoryProductRepository.Object);

            var batches = await this.consolidationProcessor.GetConsolidationBatchesAsync(ticket).ConfigureAwait(false);

            Assert.IsNotNull(batches);
            Assert.AreEqual(5, batches.Count());
            var index = 1;
            foreach (var batch in batches)
            {
                foreach (var consolidationNodeData in batch.ConsolidationNodes)
                {
                    Assert.AreEqual(index, consolidationNodeData.SourceNodeId);
                    Assert.AreEqual(index, consolidationNodeData.DestinationNodeId);
                    index++;
                }
            }

            var batchDatas = batches.SelectMany(a => a.ConsolidationNodes);
            Assert.AreEqual(true, batchDatas.Count() == batchDatas.Distinct().Count());

            mockConsolidatedMovementRepository.Verify(a => a.GetCountAsync(It.IsAny<Expression<Func<ConsolidatedMovement, bool>>>()), Times.Once);
            mockConsolidatedInventoryProductRepository.Verify(a => a.GetCountAsync(It.IsAny<Expression<Func<ConsolidatedInventoryProduct, bool>>>()), Times.Exactly(1));
        }

        /// <summary>
        /// Gets the consolidation batches should return 1 consolidation batch for 3 nodes when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetConsolidationBatches_ShouldReturn_1_ConsolidationBatchFor_3_Nodes_WhenInvokedAsync()
        {
            var date = new DateTime(2020, 07, 06);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var ticket = new Ticket
            {
                TicketId = 123,
                StartDate = firstDayOfMonth,
                EndDate = firstDayOfMonth.AddMonths(1).AddDays(-1),
            };

            var consolidationNodeDatas = new List<ConsolidationNodeData>();

            for (var i = 1; i <= 3; i++)
            {
                consolidationNodeDatas.Add(new ConsolidationNodeData { SourceNodeId = i, DestinationNodeId = i });
            }

            var mockConsolidationNodeDataRepository = new Mock<IRepository<ConsolidationNodeData>>();
            mockConsolidationNodeDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(consolidationNodeDatas);

            var mockConsolidatedMovementRepository = new Mock<IRepository<ConsolidatedMovement>>();
            mockConsolidatedMovementRepository.Setup(a => a.GetCountAsync(It.IsAny<Expression<Func<ConsolidatedMovement, bool>>>())).ReturnsAsync(0);

            var mockConsolidatedInventoryProductRepository = new Mock<IRepository<ConsolidatedInventoryProduct>>();
            mockConsolidatedInventoryProductRepository.Setup(a => a.GetCountAsync(It.IsAny<Expression<Func<ConsolidatedInventoryProduct, bool>>>())).ReturnsAsync(0);

            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidationNodeData>()).Returns(mockConsolidationNodeDataRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedMovement>()).Returns(mockConsolidatedMovementRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedInventoryProduct>()).Returns(mockConsolidatedInventoryProductRepository.Object);

            var batches = await this.consolidationProcessor.GetConsolidationBatchesAsync(ticket).ConfigureAwait(false);

            Assert.IsNotNull(batches);
            Assert.AreEqual(3, batches.Count());
            var index = 1;
            foreach (var batch in batches)
            {
                foreach (var consolidationNodeData in batch.ConsolidationNodes)
                {
                    Assert.AreEqual(index, consolidationNodeData.SourceNodeId);
                    Assert.AreEqual(index, consolidationNodeData.DestinationNodeId);
                    index++;
                }
            }

            var batchDatas = batches.SelectMany(a => a.ConsolidationNodes);
            Assert.AreEqual(true, batchDatas.Count() == batchDatas.Distinct().Count());

            mockConsolidatedMovementRepository.Verify(a => a.GetCountAsync(It.IsAny<Expression<Func<ConsolidatedMovement, bool>>>()), Times.Once);
            mockConsolidatedInventoryProductRepository.Verify(a => a.GetCountAsync(It.IsAny<Expression<Func<ConsolidatedInventoryProduct, bool>>>()), Times.Exactly(1));
        }

        /// <summary>
        /// Gets the consolidation batches should return 6 consolidation batch for 81 nodes when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetConsolidationBatches_ShouldReturn_6_ConsolidationBatchFor_81_Nodes_WhenInvokedAsync()
        {
            var date = new DateTime(2020, 07, 06);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var ticket = new Ticket
            {
                TicketId = 123,
                StartDate = firstDayOfMonth,
                EndDate = firstDayOfMonth.AddMonths(1).AddDays(-1),
            };

            var consolidationNodeDatas = new List<ConsolidationNodeData>();

            for (var i = 1; i <= 81; i++)
            {
                consolidationNodeDatas.Add(new ConsolidationNodeData { SourceNodeId = i, DestinationNodeId = i });
            }

            var mockConsolidationNodeDataRepository = new Mock<IRepository<ConsolidationNodeData>>();
            mockConsolidationNodeDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(consolidationNodeDatas);

            var mockConsolidatedMovementRepository = new Mock<IRepository<ConsolidatedMovement>>();
            mockConsolidatedMovementRepository.Setup(a => a.GetCountAsync(It.IsAny<Expression<Func<ConsolidatedMovement, bool>>>())).ReturnsAsync(0);

            var mockConsolidatedInventoryProductRepository = new Mock<IRepository<ConsolidatedInventoryProduct>>();
            mockConsolidatedInventoryProductRepository.Setup(a => a.GetCountAsync(It.IsAny<Expression<Func<ConsolidatedInventoryProduct, bool>>>())).ReturnsAsync(0);

            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidationNodeData>()).Returns(mockConsolidationNodeDataRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedMovement>()).Returns(mockConsolidatedMovementRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedInventoryProduct>()).Returns(mockConsolidatedInventoryProductRepository.Object);

            var batches = await this.consolidationProcessor.GetConsolidationBatchesAsync(ticket).ConfigureAwait(false);

            Assert.IsNotNull(batches);
            Assert.AreEqual(6, batches.Count());
            var index = 1;
            foreach (var batch in batches)
            {
                foreach (var consolidationNodeData in batch.ConsolidationNodes)
                {
                    Assert.AreEqual(index, consolidationNodeData.SourceNodeId);
                    Assert.AreEqual(index, consolidationNodeData.DestinationNodeId);
                    index++;
                }
            }

            var batchDatas = batches.SelectMany(a => a.ConsolidationNodes);
            Assert.AreEqual(true, batchDatas.Count() == batchDatas.Distinct().Count());

            mockConsolidatedMovementRepository.Verify(a => a.GetCountAsync(It.IsAny<Expression<Func<ConsolidatedMovement, bool>>>()), Times.Once);
            mockConsolidatedInventoryProductRepository.Verify(a => a.GetCountAsync(It.IsAny<Expression<Func<ConsolidatedInventoryProduct, bool>>>()), Times.Exactly(1));
        }

        /// <summary>
        /// Gets the consolidation batches should return empty consolidation batches if consolidated movements or consolidated inventory products already exists when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetConsolidationBatches_ShouldReturn_Empty_ConsolidationBatches_If_ConsolidatedMovementsORConsolidatedInventoryProductsAlreadyExists_WhenInvokedAsync()
        {
            var date = new DateTime(2020, 07, 06);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var ticket = new Ticket
            {
                TicketId = 123,
                StartDate = firstDayOfMonth,
                EndDate = firstDayOfMonth.AddMonths(1).AddDays(-1),
            };

            var consolidationNodeDatas = new List<ConsolidationNodeData>();

            for (var i = 1; i <= 81; i++)
            {
                consolidationNodeDatas.Add(new ConsolidationNodeData { SourceNodeId = i, DestinationNodeId = i });
            }

            var mockConsolidationNodeDataRepository = new Mock<IRepository<ConsolidationNodeData>>();
            mockConsolidationNodeDataRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(consolidationNodeDatas);

            var mockConsolidatedMovementRepository = new Mock<IRepository<ConsolidatedMovement>>();
            mockConsolidatedMovementRepository.Setup(a => a.GetCountAsync(It.IsAny<Expression<Func<ConsolidatedMovement, bool>>>())).ReturnsAsync(2);

            var mockConsolidatedInventoryProductRepository = new Mock<IRepository<ConsolidatedInventoryProduct>>();
            mockConsolidatedInventoryProductRepository.Setup(a => a.GetCountAsync(It.IsAny<Expression<Func<ConsolidatedInventoryProduct, bool>>>())).ReturnsAsync(0);

            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidationNodeData>()).Returns(mockConsolidationNodeDataRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedMovement>()).Returns(mockConsolidatedMovementRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedInventoryProduct>()).Returns(mockConsolidatedInventoryProductRepository.Object);

            var batches = await this.consolidationProcessor.GetConsolidationBatchesAsync(ticket).ConfigureAwait(false);

            Assert.IsNotNull(batches);
            Assert.AreEqual(0, batches.Count());

            mockConsolidatedMovementRepository.Verify(a => a.GetCountAsync(It.IsAny<Expression<Func<ConsolidatedMovement, bool>>>()), Times.Once);
            mockConsolidatedInventoryProductRepository.Verify(a => a.GetCountAsync(It.IsAny<Expression<Func<ConsolidatedInventoryProduct, bool>>>()), Times.Exactly(1));
        }

        /// <summary>
        /// Completes the consolidation asynchronous should process when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CompleteConsolidationAsync_ShouldProcess_WhenInvokedAsync()
        {
            var ticket = new Ticket { TicketId = 123, CategoryElementId = 12 };
            var ticketRepositoryMock = new Mock<IRepository<Ticket>>();
            this.mockUnitOfWork.Setup(x => x.CreateRepository<Ticket>()).Returns(ticketRepositoryMock.Object);
            var parameters = new Dictionary<string, object>
                {
                    { "@TicketId", ticket.TicketId },
                };

            await this.consolidationProcessor.CompleteConsolidationAsync(ticket.TicketId, ticket.CategoryElementId).ConfigureAwait(false);

            this.mockServiceBusQueueClient.Verify(a => a.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            this.mockAzureClientFactory.Verify(a => a.GetQueueClient(It.IsAny<string>()), Times.Once);
            ticketRepositoryMock.Verify(m => m.ExecuteAsync(Repositories.Constants.CompleteConsolidation, parameters), Times.Once);
        }

        /// <summary>
        /// Validates the ticket asynchronous should return is valid false for ticket not exists when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ValidateTicketAsync_ShouldReturnIsValid_False_ForTicketNotExists_WhenInvokedAsync()
        {
            var ticketId = 123;
            var mockTicketRepository = new Mock<IRepository<Ticket>>();
            mockTicketRepository.Setup(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(() => null);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<Ticket>()).Returns(mockTicketRepository.Object);

            (bool isValid, Ticket ticket, string errorMessage) = await this.consolidationProcessor.ValidateTicketAsync(ticketId).ConfigureAwait(false);

            Assert.AreEqual(false, isValid);
            Assert.AreEqual(null, ticket);
            Assert.AreEqual($"Ticket {ticketId} does not exists or is already processed.", errorMessage);
            mockTicketRepository.Verify(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), It.IsAny<string[]>()), Times.Once);
        }

        /// <summary>
        /// Validates the ticket asynchronous should return is valid false for ticket type not official delta when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ValidateTicketAsync_ShouldReturnIsValid_False_ForTicketTypeNotOfficialDelta_WhenInvokedAsync()
        {
            var existingTicket = new Ticket { TicketId = 123, TicketTypeId = Entities.Enumeration.TicketType.Delta };
            var mockTicketRepository = new Mock<IRepository<Ticket>>();
            mockTicketRepository.Setup(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(existingTicket);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<Ticket>()).Returns(mockTicketRepository.Object);

            (bool isValid, Ticket ticket, string errorMessage) = await this.consolidationProcessor.ValidateTicketAsync(existingTicket.TicketId).ConfigureAwait(false);

            Assert.AreEqual(false, isValid);
            Assert.IsNull(ticket);
            Assert.AreEqual($"Ticket {existingTicket.TicketId} does not exists or is already processed.", errorMessage);
            mockTicketRepository.Verify(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), It.IsAny<string[]>()), Times.Once);
        }

        /// <summary>
        /// Validates the ticket asynchronous should return is valid false for ticket status not processing when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ValidateTicketAsync_ShouldReturnIsValid_False_ForTicketStatusNotProcessing_WhenInvokedAsync()
        {
            var existingTicket = new Ticket { TicketId = 123, TicketTypeId = Entities.Enumeration.TicketType.OfficialDelta, Status = StatusType.PROCESSED };
            var mockTicketRepository = new Mock<IRepository<Ticket>>();
            mockTicketRepository.Setup(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(existingTicket);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<Ticket>()).Returns(mockTicketRepository.Object);

            (bool isValid, Ticket ticket, string errorMessage) = await this.consolidationProcessor.ValidateTicketAsync(existingTicket.TicketId).ConfigureAwait(false);

            Assert.AreEqual(false, isValid);
            Assert.IsNull(ticket);
            Assert.AreEqual($"Ticket {existingTicket.TicketId} does not exists or is already processed.", errorMessage);
            mockTicketRepository.Verify(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), It.IsAny<string[]>()), Times.Once);
        }

        [TestMethod]
        public async Task ValidateTicketAsync_ShouldReturnIsValid_False_ForOfficialDeltaTicketAlreadyExistsForSegment_WhenInvokedAsync()
        {
            var existingTicket = new Ticket { TicketId = 123, TicketTypeId = Entities.Enumeration.TicketType.OfficialDelta, Status = StatusType.PROCESSING };
            var mockTicketRepository = new Mock<IRepository<Ticket>>();
            mockTicketRepository.Setup(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(existingTicket);
            mockTicketRepository.Setup(a => a.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(1);

            this.mockUnitOfWork.Setup(a => a.CreateRepository<Ticket>()).Returns(mockTicketRepository.Object);

            (bool isValid, Ticket ticket, string errorMessage) = await this.consolidationProcessor.ValidateTicketAsync(existingTicket.TicketId).ConfigureAwait(false);

            Assert.AreEqual(false, isValid);
            Assert.IsNotNull(ticket);
            Assert.AreEqual(Constants.OfficialDeltaCalculationInProgress, errorMessage);
            mockTicketRepository.Verify(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), It.IsAny<string[]>()), Times.Once);
            mockTicketRepository.Verify(a => a.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>()), Times.Once);
        }

        [TestMethod]
        public async Task ValidateTicketAsync_ShouldReturnIsValid_True_ForLastPeriodRunTicketNotExists_WhenInvokedAsync()
        {
            var existingTicket = new Ticket { TicketId = 123, TicketTypeId = Entities.Enumeration.TicketType.OfficialDelta, Status = StatusType.PROCESSING };
            var lastTickets = new List<Ticket>();
            var mockTicketRepository = new Mock<IRepository<Ticket>>();
            mockTicketRepository.Setup(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(existingTicket);
            mockTicketRepository.Setup(a => a.OrderByDescendingAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), a => a.TicketId, It.IsAny<int?>())).ReturnsAsync(lastTickets);

            var mockDeltaNodeRepository = new Mock<IRepository<DeltaNode>>();
            mockDeltaNodeRepository.Setup(a => a.GetCountAsync(It.IsAny<Expression<Func<DeltaNode, bool>>>())).ReturnsAsync(1);

            this.mockUnitOfWork.Setup(a => a.CreateRepository<Ticket>()).Returns(mockTicketRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<DeltaNode>()).Returns(mockDeltaNodeRepository.Object);

            (bool isValid, Ticket ticket, string errorMessage) = await this.consolidationProcessor.ValidateTicketAsync(existingTicket.TicketId).ConfigureAwait(false);

            Assert.AreEqual(true, isValid);
            Assert.IsNotNull(ticket);
            Assert.AreEqual(string.Empty, errorMessage);
            mockTicketRepository.Verify(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), It.IsAny<string[]>()), Times.Once);
            mockTicketRepository.Verify(a => a.OrderByDescendingAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), a => a.TicketId, It.IsAny<int?>()), Times.Once);
            mockDeltaNodeRepository.Verify(a => a.GetCountAsync(It.IsAny<Expression<Func<DeltaNode, bool>>>()), Times.Never);
        }

        [TestMethod]
        public async Task ValidateTicketAsync_ShouldReturnIsValid_False_ForNotApprovedDeltaNodeExistsForLastPeriod_WhenInvokedAsync()
        {
            var existingTicket = new Ticket { TicketId = 123, TicketTypeId = Entities.Enumeration.TicketType.OfficialDelta, Status = StatusType.PROCESSING };
            var lastTickets = new List<Ticket>
            {
                new Ticket { TicketId = 123, TicketTypeId = Entities.Enumeration.TicketType.OfficialDelta, Status = StatusType.PROCESSED, CreatedDate = DateTime.UtcNow },
            };
            var mockTicketRepository = new Mock<IRepository<Ticket>>();
            mockTicketRepository.Setup(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(existingTicket);
            mockTicketRepository.Setup(a => a.OrderByDescendingAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), a => a.TicketId, It.IsAny<int?>())).ReturnsAsync(lastTickets);

            var mockDeltaNodeRepository = new Mock<IRepository<DeltaNode>>();
            mockDeltaNodeRepository.Setup(a => a.GetCountAsync(It.IsAny<Expression<Func<DeltaNode, bool>>>())).ReturnsAsync(1);

            this.mockUnitOfWork.Setup(a => a.CreateRepository<Ticket>()).Returns(mockTicketRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<DeltaNode>()).Returns(mockDeltaNodeRepository.Object);

            (bool isValid, Ticket ticket, string errorMessage) = await this.consolidationProcessor.ValidateTicketAsync(existingTicket.TicketId).ConfigureAwait(false);

            Assert.AreEqual(false, isValid);
            Assert.IsNotNull(ticket);
            Assert.AreEqual(Constants.OfficialDeltaWithoutApprovalInPreviousPeriod, errorMessage);
            mockTicketRepository.Verify(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), It.IsAny<string[]>()), Times.Once);
            mockTicketRepository.Verify(a => a.OrderByDescendingAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), a => a.TicketId, It.IsAny<int?>()), Times.Once);
            mockDeltaNodeRepository.Verify(a => a.GetCountAsync(It.IsAny<Expression<Func<DeltaNode, bool>>>()), Times.Once);
        }
    }
}
