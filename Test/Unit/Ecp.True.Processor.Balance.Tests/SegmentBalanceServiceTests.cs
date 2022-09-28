// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SegmentBalanceServiceTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Balance.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Balance;
    using Ecp.True.Processors.Balance.Calculation.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class SegmentBalanceServiceTests
    {
        /// <summary>
        /// The unit of work mock.
        /// </summary>
        private Mock<IUnitOfWorkFactory> unitOfWorkFactoryMock;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<IUnitOfWork> unitOfWorkMock;

        /// <summary>
        /// The mock segment balance service.
        /// </summary>
        private SegmentBalanceService segmentBalanceService;

        /// <summary>
        /// The segment calculator mock.
        /// </summary>
        private Mock<ISegmentCalculator> segmentCalculatorMock;

        /// <summary>
        /// The segment calculator mock.
        /// </summary>
        private Mock<IRepository<Ticket>> mockTicketRepository;

        /// <summary>
        /// The segment calculator mock.
        /// </summary>
        private Mock<IRepository<SegmentNodeDto>> mockSegmentNodeDtoRepository;

        /// <summary>
        /// The segment calculator mock.
        /// </summary>
        private Mock<IRepository<Movement>> mockMovementRepository;

        /// <summary>
        /// The segment calculator mock.
        /// </summary>
        private Mock<IRepository<InventoryProduct>> mockInventoryProductRepository;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.unitOfWorkMock = new Mock<IUnitOfWork>();
            this.unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
            this.segmentCalculatorMock = new Mock<ISegmentCalculator>();
            this.segmentBalanceService = new SegmentBalanceService(this.segmentCalculatorMock.Object);
            this.mockTicketRepository = new Mock<IRepository<Ticket>>();
            this.mockSegmentNodeDtoRepository = new Mock<IRepository<SegmentNodeDto>>();
            this.mockMovementRepository = new Mock<IRepository<Movement>>();
            this.mockInventoryProductRepository = new Mock<IRepository<InventoryProduct>>();
            this.unitOfWorkFactoryMock.Setup(x => x.GetUnitOfWork()).Returns(this.unitOfWorkMock.Object);
            this.unitOfWorkMock.Setup(s => s.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);
            this.unitOfWorkMock.Setup(s => s.CreateRepository<SegmentNodeDto>()).Returns(this.mockSegmentNodeDtoRepository.Object);
            this.unitOfWorkMock.Setup(s => s.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);
            this.unitOfWorkMock.Setup(s => s.CreateRepository<InventoryProduct>()).Returns(this.mockInventoryProductRepository.Object);
            this.unitOfWorkMock.Setup(s => s.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
        }

        [TestMethod]
        public async Task SegmentBalanceServiceShouldCallSegmentCalculatorAsync()
        {
            this.segmentCalculatorMock.Setup(x => x.CalculateAndGetSegmentUnbalance(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<Movement>>(), It.IsAny<IEnumerable<Movement>>(), It.IsAny<IEnumerable<InventoryProduct>>(), It.IsAny<IEnumerable<InventoryProduct>>(), It.IsAny<IEnumerable<Movement>>(), It.IsAny<IEnumerable<Movement>>(), It.IsAny<IUnitOfWork>())).Returns(new SegmentUnbalance());
            var ticket = new Ticket
            {
                TicketId = 1,
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(-1),
                CategoryElementId = 1,
            };

            var segmentNodes = new List<SegmentNodeDto>
            {
                new SegmentNodeDto
                {
                    NodeId = 1,
                    OperationDate = DateTime.Now.AddDays(-1),
                    SegmentId = 1,
                    NodeName = "Genérico",
                },
            };

            var movements = new List<Movement>
            {
                new Movement
                {
                    MovementId = "1",
                    MovementDestination = new MovementDestination
                    {
                        DestinationNodeId = 1,
                        DestinationProductId = "1",
                        DestinationProduct = new Product
                        {
                            ProductId = "1",
                        },
                    },
                    MovementSource = new MovementSource
                    {
                        SourceNodeId = 2,
                        SourceProductId = "1",
                        SourceProduct = new Product
                        {
                            ProductId = "1",
                        },
                    },
                    OperationalDate = DateTime.Now.AddDays(-1),
                    MessageTypeId = 1,
                },
            };

            var inventories = new List<InventoryProduct>();
            var product1 = new InventoryProduct
            {
                InventoryProductId = 1,
                InventoryDate = DateTime.Now.AddDays(-1),
                NodeId = 1,
                Product = new Product
                {
                    ProductId = "1",
                },
            };

            inventories.Add(product1);

            this.mockTicketRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ticket);
            this.mockMovementRepository.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(movements);
            this.mockInventoryProductRepository.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(inventories);
            this.mockSegmentNodeDtoRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(segmentNodes);
            await this.segmentBalanceService.ProcessSegmentAsync(1, this.unitOfWorkMock.Object).ConfigureAwait(false);
            this.segmentCalculatorMock.Verify(x => x.CalculateAndGetSegmentUnbalance(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<Movement>>(), It.IsAny<IEnumerable<Movement>>(), It.IsAny<IEnumerable<InventoryProduct>>(), It.IsAny<IEnumerable<InventoryProduct>>(), It.IsAny<IEnumerable<Movement>>(), It.IsAny<IEnumerable<Movement>>(), It.IsAny<IUnitOfWork>()));
        }
    }
}
