// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SegmentOwnershipCalculationServiceTests.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Ownership;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The OwnershipProcessorTests.
    /// </summary>
    [TestClass]
    public class SegmentOwnershipCalculationServiceTests
    {
        /// <summary>
        /// The mock repository factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockRepositoryFactory;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The mock calculate ownership.
        /// </summary>
        private Mock<ICalculateOwnership> mockCalculateOwnership;

        /// <summary>
        /// The mock ownership calculation repository.
        /// </summary>
        private Mock<IRepository<SegmentOwnershipCalculation>> mockOwnershipCalculationRepository;

        /// <summary>
        /// The mock movement repository.
        /// </summary>
        private Mock<IRepository<Movement>> mockMovementRepository;

        /// <summary>
        /// The mock ticket repository.
        /// </summary>
        private Mock<IRepository<Ticket>> mockTicketRepository;

        /// <summary>
        /// The mock inventory product repository.
        /// </summary>
        private Mock<IRepository<InventoryProduct>> mockInventoryProductRepository;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The segment ownership calculation service.
        /// </summary>
        private SegmentOwnershipCalculationService segmentOwnershipCalculationService;

        /// <summary>
        /// The segment calculator mock.
        /// </summary>
        private Mock<IRepository<SegmentNodeDto>> mockSegmentNodeDtoRepository;

        /// <summary>
        /// Intilizes the method.
        /// </summary>
        [TestInitialize]
        public void IntilizeMethod()
        {
            this.mockRepositoryFactory = new Mock<IRepositoryFactory>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockMovementRepository = new Mock<IRepository<Movement>>();
            this.mockInventoryProductRepository = new Mock<IRepository<InventoryProduct>>();
            this.mockTicketRepository = new Mock<IRepository<Ticket>>();
            this.mockCalculateOwnership = new Mock<ICalculateOwnership>();
            this.mockSegmentNodeDtoRepository = new Mock<IRepository<SegmentNodeDto>>();

            this.mockOwnershipCalculationRepository = new Mock<IRepository<SegmentOwnershipCalculation>>();

            this.mockOwnershipCalculationRepository.Setup(a => a.Insert(It.IsAny<SegmentOwnershipCalculation>()));

            this.mockUnitOfWork.Setup(a => a.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<InventoryProduct>()).Returns(this.mockInventoryProductRepository.Object);

            this.mockUnitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<SegmentOwnershipCalculation>()).Returns(this.mockOwnershipCalculationRepository.Object);
            this.mockRepositoryFactory.Setup(a => a.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);
            this.mockRepositoryFactory.Setup(s => s.CreateRepository<SegmentNodeDto>()).Returns(this.mockSegmentNodeDtoRepository.Object);
            this.mockUnitOfWork.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));

            this.segmentOwnershipCalculationService = new SegmentOwnershipCalculationService(this.mockRepositoryFactory.Object, this.mockCalculateOwnership.Object);
        }

        /// <summary>
        /// Processes the ownership asynchronous resultexcel should process ownership asynchronous.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task ProcessSegmentOwnershipAsync_ShouldProcess_WithSuccessAsync()
        {
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

            var dates = new List<DateTime>();
            dates.Add(DateTime.Now.AddDays(-1));

            var ownership = new Ownership
            {
                OwnerId = 27,
                BlockchainStatus = Entities.Core.StatusType.PROCESSED,
            };

            var movements = new List<Movement>
            {
                new Movement
                {
                    MovementId = "1",
                    MeasurementUnit = 31,
                    MovementDestination = new MovementDestination
                    {
                        DestinationNodeId = 1,
                        DestinationProductId = "1",
                    },
                    MovementSource = new MovementSource
                    {
                        SourceNodeId = 2,
                        SourceProductId = "1",
                    },
                    OperationalDate = DateTime.Now.AddDays(-1),
                    MessageTypeId = 1,
                    BlockchainStatus = Entities.Core.StatusType.PROCESSED,
                },
            };
            movements[0].Ownerships.Add(ownership);

            var inventories = new List<InventoryProduct>();
            var product1 = new InventoryProduct
            {
                InventoryProductId = 1,
                MeasurementUnit = 31,
                InventoryDate = DateTime.Now.AddDays(-1),
                NodeId = 1,
                BlockchainStatus = Entities.Core.StatusType.PROCESSED,
            };
            product1.Ownerships.Add(ownership);

            inventories.Add(product1);
            this.mockTicketRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ticket);
            this.mockSegmentNodeDtoRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(segmentNodes);

            await this.segmentOwnershipCalculationService.ProcessSegmentOwnershipAsync(inventories, movements, 1).ConfigureAwait(false);

            this.mockCalculateOwnership.Verify(m => m.CalculateAndRegisterForSegment(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<Movement>>(), It.IsAny<IEnumerable<Movement>>(), It.IsAny<IEnumerable<InventoryProduct>>(), It.IsAny<IEnumerable<InventoryProduct>>(), It.IsAny<IEnumerable<Movement>>(), It.IsAny<IEnumerable<int>>()));
        }
    }
}
