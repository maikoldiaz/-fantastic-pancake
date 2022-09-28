// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaProcessTests.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Microsoft.EntityFrameworkCore.Internal;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class OfficialDeltaProcessTests
    {
        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IFinalizerFactory> mockDeltaFinalizerFactory = new Mock<IFinalizerFactory>();

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IFinalizer> mockDeltaFinalizer = new Mock<IFinalizer>();

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<OfficialDeltaProcessor>> mockLogger;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The consolidation processor.
        /// </summary>
        private OfficialDeltaProcessor officialDeltaProcessor;

        /// <summary>
        /// The mock repository factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockRepositoryFactory;

        /// <summary>
        /// The mock ownership repository.
        /// </summary>
        private Mock<IRepository<DeltaNode>> mockDeltaNodeRepository;

        /// <summary>
        /// The mock execution chain builder.
        /// </summary>
        private Mock<IExecutionChainBuilder> mockExecutionChainBuilder;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IExecutionManagerFactory> mockExecutionManagerFactory;

        /// <summary>
        /// The mock execution manager.
        /// </summary>
        private Mock<IExecutionManager> mockExecutionManager;

        /// <summary>
        /// The mock executor.
        /// </summary>
        private Mock<IExecutor> mockExecutor;

        /// <summary>
        /// The mock business context.
        /// </summary>
        private Mock<IBusinessContext> mockBusinessContext;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<OfficialDeltaProcessor>>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockRepositoryFactory = new Mock<IRepositoryFactory>();
            this.mockDeltaNodeRepository = new Mock<IRepository<DeltaNode>>();
            this.mockExecutor = new Mock<IExecutor>();
            this.mockExecutionManager = new Mock<IExecutionManager>();
            this.mockExecutionManagerFactory = new Mock<IExecutionManagerFactory>();
            this.mockExecutionChainBuilder = new Mock<IExecutionChainBuilder>();
            this.mockBusinessContext = new Mock<IBusinessContext>();

            this.mockUnitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockExecutionManagerFactory.Setup(a => a.GetExecutionManager(It.IsAny<TicketType>())).Returns(this.mockExecutionManager.Object);
            this.mockDeltaFinalizerFactory.Setup(a => a.GetFinalizer(It.IsAny<TicketType>())).Returns(this.mockDeltaFinalizer.Object);
            this.officialDeltaProcessor = new OfficialDeltaProcessor(
                this.mockLogger.Object,
                this.mockRepositoryFactory.Object,
                this.mockUnitOfWorkFactory.Object,
                this.mockExecutionChainBuilder.Object,
                this.mockExecutionManagerFactory.Object,
                this.mockDeltaFinalizerFactory.Object,
                this.mockBusinessContext.Object);
        }

        /// <summary>
        /// Process the  official delta data when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ProcessAsync_BuildResult_WhenInvokedAsync()
        {
            var officialDeltaData = new OfficialDeltaData
            {
                Ticket = new Ticket
                {
                    TicketId = 245,
                },
            };
            var pendingMovements = new List<PendingOfficialMovement>();
            var pendingInventories = new List<PendingOfficialInventory>();
            var mockPendingMovementRepository = new Mock<IRepository<PendingOfficialMovement>>();
            mockPendingMovementRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(pendingMovements);

            var mockPendingMovementInventory = new Mock<IRepository<PendingOfficialInventory>>();
            mockPendingMovementInventory.Setup(a => a.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(pendingInventories);
            this.mockRepositoryFactory.Setup(a => a.CreateRepository<PendingOfficialMovement>()).Returns(mockPendingMovementRepository.Object);
            this.mockRepositoryFactory.Setup(a => a.CreateRepository<PendingOfficialInventory>()).Returns(mockPendingMovementInventory.Object);

            await this.officialDeltaProcessor.BuildOfficialDataAsync(officialDeltaData).ConfigureAwait(false);

            this.mockRepositoryFactory.Verify(a => a.CreateRepository<PendingOfficialInventory>(), Times.Exactly(1));
            this.mockRepositoryFactory.Verify(a => a.CreateRepository<PendingOfficialMovement>(), Times.Exactly(1));
        }

        /// <summary>
        /// Processes the asynchronous should process when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ProcessAsync_ShouldProcess_WhenInvokedAsync()
        {
            var deltaData = new OfficialDeltaData { Ticket = new Ticket { TicketId = 123 } };

            this.mockExecutionChainBuilder.Setup(a => a.Build(It.IsAny<ProcessType>(), It.IsAny<ChainType>())).Returns(this.mockExecutor.Object);
            this.mockExecutionManager.Setup(a => a.Initialize(It.IsAny<IExecutor>()));
            this.mockExecutionManager.Setup(a => a.ExecuteChainAsync(It.IsAny<OfficialDeltaData>())).ReturnsAsync(deltaData);

            var result = await this.officialDeltaProcessor.ProcessAsync(deltaData, ChainType.RequestOfficialDelta).ConfigureAwait(false);

            this.mockExecutionChainBuilder.Verify(a => a.Build(It.IsAny<ProcessType>(), It.IsAny<ChainType>()), Times.Once);
            this.mockExecutionManager.Verify(a => a.Initialize(It.IsAny<IExecutor>()), Times.Once);
            this.mockExecutionManager.Verify(a => a.ExecuteChainAsync(It.IsAny<OfficialDeltaData>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(123, result.Ticket.TicketId);
        }

        /// <summary>
        /// Exclude the  official delta data when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ProcessAsync_ExcludeResult_WhenInvokedAsync()
        {
            var officialDeltaData = new OfficialDeltaData
            {
                Ticket = new Ticket
                {
                    TicketId = 245,
                },
            };
            var pendingMovements = new List<PendingOfficialMovement>
            {
                new PendingOfficialMovement
                {
                    SystemId = 5,
                    SourceNodeId = 6,
                    DestinationNodeId = 3,
                },
            };
            var mockPendingMovementRepository = new Mock<IRepository<PendingOfficialMovement>>();
            mockPendingMovementRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(pendingMovements);

            this.mockRepositoryFactory.Setup(a => a.CreateRepository<PendingOfficialMovement>()).Returns(mockPendingMovementRepository.Object);
            var result = await this.officialDeltaProcessor.ExcludeDataAsync(officialDeltaData).ConfigureAwait(false);

            Assert.IsTrue(!result.PendingOfficialMovements.Any());
        }

        /// <summary>
        /// register Node when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ProcessAsync_RegisterNode_WhenInvokedAsync()
        {
            var officialDeltaData = new OfficialDeltaData
            {
                Ticket = new Ticket
                {
                    TicketId = 245,
                },
            };
            var movementsToDelete = new List<MovementsToDelete>();
            var mockMovementTransactionRepository = new Mock<IRepository<MovementsToDelete>>();
            mockMovementTransactionRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(movementsToDelete);
            this.mockRepositoryFactory.Setup(a => a.CreateRepository<MovementsToDelete>()).Returns(mockMovementTransactionRepository.Object);

            var movementRepository = new Mock<IRepository<Movement>>();
            movementRepository.Setup(a => a.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()));

            var inventoryRepository = new Mock<IRepository<InventoryProduct>>();
            inventoryRepository.Setup(a => a.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()));

            this.mockUnitOfWork.Setup(a => a.CreateRepository<Movement>()).Returns(movementRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<InventoryProduct>()).Returns(inventoryRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<DeltaNode>()).Returns(this.mockDeltaNodeRepository.Object);

            await this.officialDeltaProcessor.RegisterAsync(officialDeltaData).ConfigureAwait(false);

            movementRepository.Verify(a => a.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
            inventoryRepository.Verify(a => a.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Include Movement When DestinationNode Belongs To The DifferentSegment And SourceNodeBelongsToTheSameSystem.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ProcessAsync_IncludeResult_WhenDestinationNodeBelongsToTheDifferentSegmentAndSourceNodeBelongsToTheSameSystemAsync()
        {
            var officialDeltaData = new OfficialDeltaData
            {
                Ticket = new Ticket
                {
                    TicketId = 245,
                },
            };
            var pendingMovements = new List<PendingOfficialMovement>
            {
                new PendingOfficialMovement
                {
                    SystemId = 5,
                    SegmentId = 6,
                    SourceNodeId = 6,
                    DestinationNodeId = 3,
                    DestinationNodeSegmentID = 7,
                    SourceNodeSegmentId = 6,
                    SourceNodeSystem = 5,
                    DestinationNodeSystem = 5,
                },
            };
            var mockPendingMovementRepository = new Mock<IRepository<PendingOfficialMovement>>();
            mockPendingMovementRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(pendingMovements);
            officialDeltaData.PendingOfficialMovements = pendingMovements;

            this.mockRepositoryFactory.Setup(a => a.CreateRepository<PendingOfficialMovement>()).Returns(mockPendingMovementRepository.Object);
            var result = await this.officialDeltaProcessor.ExcludeDataAsync(officialDeltaData).ConfigureAwait(false);

            Assert.IsTrue(result.PendingOfficialMovements.Any());
        }

        /// <summary>
        /// Build official delta data when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task BuildOfficialDeltaDataAsync_ShouldBuildOfficialDeltaData_WhenInvokedAsync()
        {
            var officialDeltaData = new OfficialDeltaData
            {
                Ticket = new Ticket
                {
                    TicketId = 200,
                },
            };
            var officialDeltaMovements = new List<OfficialDeltaMovement>();
            var officialDeltaInventory = new List<OfficialDeltaInventory>();
            var mockOfficialDeltaMovementRepository = new Mock<IRepository<OfficialDeltaMovement>>();
            var mockOfficialDeltaInventoryRepository = new Mock<IRepository<OfficialDeltaInventory>>();
            mockOfficialDeltaMovementRepository.Setup(x => x.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(officialDeltaMovements);
            mockOfficialDeltaInventoryRepository.Setup(x => x.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(officialDeltaInventory);
            this.mockRepositoryFactory.Setup(a => a.CreateRepository<OfficialDeltaMovement>()).Returns(mockOfficialDeltaMovementRepository.Object);
            this.mockRepositoryFactory.Setup(a => a.CreateRepository<OfficialDeltaInventory>()).Returns(mockOfficialDeltaInventoryRepository.Object);
            await this.officialDeltaProcessor.BuildOfficialDeltaDataAsync(officialDeltaData).ConfigureAwait(false);
            mockOfficialDeltaMovementRepository.Verify(a => a.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
            mockOfficialDeltaInventoryRepository.Verify(a => a.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }
    }
}
