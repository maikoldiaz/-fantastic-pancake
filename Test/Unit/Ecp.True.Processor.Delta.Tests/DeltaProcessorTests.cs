// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaProcessorTests.cs" company="Microsoft">
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
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta;
    using Ecp.True.Processors.Delta.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The DeltaProcessorTests.
    /// </summary>
    [TestClass]
    public class DeltaProcessorTests
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
        private Mock<ITrueLogger<DeltaProcessor>> mockLogger;

        /// <summary>
        /// The mock execution chain builder.
        /// </summary>
        private Mock<IExecutionChainBuilder> mockExecutionChainBuilder;

        /// <summary>
        /// The mock repository factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockRepositoryFactory;

        /// <summary>
        /// The mock execution manager.
        /// </summary>
        private Mock<IExecutionManager> mockExecutionManager;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IExecutionManagerFactory> mockExecutionManagerFactory;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock executor.
        /// </summary>
        private Mock<IExecutor> mockExecutor;

        /// <summary>
        /// The delta processor.
        /// </summary>
        private DeltaProcessor deltaProcessor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<DeltaProcessor>>();
            this.mockExecutionChainBuilder = new Mock<IExecutionChainBuilder>();
            this.mockRepositoryFactory = new Mock<IRepositoryFactory>();
            this.mockExecutionManager = new Mock<IExecutionManager>();
            this.mockExecutionManagerFactory = new Mock<IExecutionManagerFactory>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockExecutor = new Mock<IExecutor>();

            this.mockUnitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockExecutionManagerFactory.Setup(a => a.GetExecutionManager(It.IsAny<TicketType>())).Returns(this.mockExecutionManager.Object);
            this.mockDeltaFinalizerFactory.Setup(a => a.GetFinalizer(It.IsAny<TicketType>())).Returns(this.mockDeltaFinalizer.Object);

            this.deltaProcessor = new DeltaProcessor(
                this.mockLogger.Object,
                this.mockRepositoryFactory.Object,
                this.mockUnitOfWorkFactory.Object,
                this.mockExecutionChainBuilder.Object,
                this.mockExecutionManagerFactory.Object,
                this.mockDeltaFinalizerFactory.Object);
        }

        /// <summary>
        /// Processes the asynchronous should process when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ProcessAsync_ShouldProcess_WhenInvokedAsync()
        {
            var deltaData = new DeltaData { Ticket = new Ticket { TicketId = 123 } };

            this.mockExecutionChainBuilder.Setup(a => a.Build(It.IsAny<ProcessType>(), It.IsAny<ChainType>())).Returns(this.mockExecutor.Object);
            this.mockExecutionManager.Setup(a => a.Initialize(It.IsAny<IExecutor>()));
            this.mockExecutionManager.Setup(a => a.ExecuteChainAsync(It.IsAny<DeltaData>())).ReturnsAsync(deltaData);

            var result = await this.deltaProcessor.ProcessAsync(deltaData, ChainType.GetDelta).ConfigureAwait(false);

            this.mockExecutionChainBuilder.Verify(a => a.Build(It.IsAny<ProcessType>(), It.IsAny<ChainType>()), Times.Once);
            this.mockExecutionManager.Verify(a => a.Initialize(It.IsAny<IExecutor>()), Times.Once);
            this.mockExecutionManager.Verify(a => a.ExecuteChainAsync(It.IsAny<DeltaData>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(123, result.Ticket.TicketId);
        }

        /// <summary>
        /// Validates the ticket asynchronous should return false for ticket not exists when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ValidateTicketAsync_ShouldReturnFalse_ForTicketNotExists_WhenInvokedAsync()
        {
            var mockTicketRepository = new Mock<IRepository<Ticket>>();
            mockTicketRepository.Setup(a => a.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);
            mockTicketRepository.Setup(a => a.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(1);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<Ticket>()).Returns(mockTicketRepository.Object);

            var result = await this.deltaProcessor.ValidateTicketAsync(1).ConfigureAwait(false);

            mockTicketRepository.Verify(a => a.GetByIdAsync(It.IsAny<int>()), Times.Once);
            mockTicketRepository.Verify(a => a.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>()), Times.Never);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<Ticket>(), Times.Once);
            Assert.AreEqual(false, result.isValid);
        }

        /// <summary>
        /// Validates the ticket asynchronous should return false for ticket type not delta when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ValidateTicketAsync_ShouldReturnFalse_ForTicketTypeNotDelta_WhenInvokedAsync()
        {
            var ticket = new Ticket { TicketId = 1, TicketTypeId = Entities.Enumeration.TicketType.Cutoff };
            var mockTicketRepository = new Mock<IRepository<Ticket>>();
            mockTicketRepository.Setup(a => a.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ticket);
            mockTicketRepository.Setup(a => a.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(1);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<Ticket>()).Returns(mockTicketRepository.Object);

            var result = await this.deltaProcessor.ValidateTicketAsync(1).ConfigureAwait(false);

            mockTicketRepository.Verify(a => a.GetByIdAsync(It.IsAny<int>()), Times.Once);
            mockTicketRepository.Verify(a => a.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>()), Times.Never);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<Ticket>(), Times.Once);
            Assert.AreEqual(false, result.isValid);
        }

        /// <summary>
        /// Validates the ticket asynchronous should return false for ticket status not processing when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ValidateTicketAsync_ShouldReturnFalse_ForTicketStatusNotProcessing_WhenInvokedAsync()
        {
            var ticket = new Ticket { TicketId = 1, TicketTypeId = Entities.Enumeration.TicketType.Delta, Status = StatusType.PROCESSED };
            var mockTicketRepository = new Mock<IRepository<Ticket>>();
            mockTicketRepository.Setup(a => a.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ticket);
            mockTicketRepository.Setup(a => a.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(1);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<Ticket>()).Returns(mockTicketRepository.Object);

            var result = await this.deltaProcessor.ValidateTicketAsync(1).ConfigureAwait(false);

            mockTicketRepository.Verify(a => a.GetByIdAsync(It.IsAny<int>()), Times.Once);
            mockTicketRepository.Verify(a => a.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>()), Times.Never);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<Ticket>(), Times.Once);
            Assert.AreEqual(false, result.isValid);
        }

        /// <summary>
        /// Validates the ticket asynchronous should return true for ticket exists when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ValidateTicketAsync_ShouldReturnTrue_ForTicketExists_WhenInvokedAsync()
        {
            var ticket = new Ticket
            {
                TicketId = 1,
                TicketTypeId = Entities.Enumeration.TicketType.Delta,
                Status = StatusType.PROCESSING,
            };
            var mockTicketRepository = new Mock<IRepository<Ticket>>();
            mockTicketRepository.Setup(a => a.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ticket);
            mockTicketRepository.Setup(a => a.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(0);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<Ticket>()).Returns(mockTicketRepository.Object);

            var result = await this.deltaProcessor.ValidateTicketAsync(1).ConfigureAwait(false);

            mockTicketRepository.Verify(a => a.GetByIdAsync(It.IsAny<int>()), Times.Once);
            mockTicketRepository.Verify(a => a.GetCountAsync(It.IsAny<Expression<Func<Ticket, bool>>>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<Ticket>(), Times.Once);
            Assert.AreEqual(true, result.isValid);
        }

        [TestMethod]
        public async Task DeltaProcessor_FinalizeProcessAsync_WhenInvokedAsync()
        {
            this.mockDeltaFinalizer.Setup(m => m.ProcessAsync(It.IsAny<int>()));

            // Act
            await this.deltaProcessor.FinalizeProcessAsync(1).ConfigureAwait(false);

            // Assert
            this.mockDeltaFinalizer.Verify(m => m.ProcessAsync(It.IsAny<int>()), Times.Exactly(1));
        }

        /// <summary>
        /// Gets the official delta period asynchronous should return period list when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetOfficialDeltaPeriodAsync_ShouldReturnPeriodList_WhenInvokedAsync()
        {
            int segmentId = 10;
            int years = 5;
            var periodInfo = new List<OfficialDeltaMovementPeriodInfo> { new OfficialDeltaMovementPeriodInfo { YearInfo = 2020, MonthInfo = 6 } };
            var info = new OfficialDeltaPeriodInfo();
            info.OfficialPeriods.Add("2020", new List<OfficialDeltaMovementPeriodInfo> { new OfficialDeltaMovementPeriodInfo { YearInfo = 2020, MonthInfo = 6 } });

            var repoMock = new Mock<IRepository<OfficialDeltaMovementPeriodInfo>>();
            this.mockUnitOfWork.Setup(m => m.CreateRepository<OfficialDeltaMovementPeriodInfo>()).Returns(repoMock.Object);
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(periodInfo);
            var result = await this.deltaProcessor.GetOfficialDeltaPeriodAsync(segmentId, years, true).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.OfficialPeriods.Count, info.OfficialPeriods.Count);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<OfficialDeltaMovementPeriodInfo>(), Times.Once);
        }

        [TestMethod]
        public async Task GetOfficialDeltaPeriodAsync_ShouldReturnPeriodListDEscendingOrder_WhenInvokedAsync()
        {
            int segmentId = 10;
            int years = 5;
            var periodInfo = new List<OfficialDeltaMovementPeriodInfo>
            {
                new OfficialDeltaMovementPeriodInfo { YearInfo = 2020, MonthInfo = 1 },
                new OfficialDeltaMovementPeriodInfo { YearInfo = 2020, MonthInfo = 2 },
                new OfficialDeltaMovementPeriodInfo { YearInfo = 2020, MonthInfo = 3 },
            };

            var repoMock = new Mock<IRepository<OfficialDeltaMovementPeriodInfo>>();
            this.mockUnitOfWork.Setup(m => m.CreateRepository<OfficialDeltaMovementPeriodInfo>()).Returns(repoMock.Object);
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(periodInfo);
            var result = await this.deltaProcessor.GetOfficialDeltaPeriodAsync(segmentId, years, true).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.OfficialPeriods.Count);

            var list = result.OfficialPeriods["2020"].ToList();
            Assert.AreEqual(3, list.Count);

            Assert.AreEqual(3, list.ElementAt(0).MonthInfo);
            Assert.AreEqual(2, list.ElementAt(1).MonthInfo);
            Assert.AreEqual(1, list.ElementAt(2).MonthInfo);

            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<OfficialDeltaMovementPeriodInfo>(), Times.Once);
        }

        [TestMethod]
        public async Task ValidatePreviousOfficialPeriod_ShouldReturnNotApprovedCount_WhenInvokedAsync()
        {
            var notApproved = new List<ValidatePreviousOfficialPeriod> { new ValidatePreviousOfficialPeriod() { UnApprovedNodes = 0, } };
            var ticket = new Ticket();
            var repoMock = new Mock<IRepository<ValidatePreviousOfficialPeriod>>();
            this.mockUnitOfWork.Setup(m => m.CreateRepository<ValidatePreviousOfficialPeriod>()).Returns(repoMock.Object);
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(notApproved);
            var result = await this.deltaProcessor.ValidatePreviousOfficialPeriodAsync(ticket).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<ValidatePreviousOfficialPeriod>(), Times.Once);
        }

        [TestMethod]
        public async Task GetOfficialDeltaTicketProcessingStatusAsync_ShouldReturnProcessingStatus_IfInvokedAsync()
        {
            var ticket = new Ticket();
            int segmentId = 10;

            IEnumerable<Ticket> tickets = new List<Ticket> { ticket };
            var repoMock = new Mock<IRepository<Ticket>>();
            repoMock.Setup(r => r.GetAllAsync(a => a.CategoryElementId == segmentId && a.Status == StatusType.PROCESSING && a.TicketTypeId == TicketType.OfficialDelta)).Returns(Task.FromResult(tickets));
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Ticket>()).Returns(repoMock.Object);
            var result = await this.deltaProcessor.GetOfficialDeltaTicketProcessingStatusAsync(segmentId).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(TicketType.OfficialDelta.ToString(), result);
        }

        [TestMethod]
        public async Task GetOfficialDeltaTicketProcessingStatusAsync_ShouldReturnNull_IfInvokedAsync()
        {
            int segmentId = 10;
            var repoMock = new Mock<IRepository<Ticket>>();
            repoMock.Setup(r => r.GetAllAsync(a => a.CategoryElementId == segmentId && a.Status == StatusType.PROCESSING && a.TicketTypeId == TicketType.OfficialDelta)).ReturnsAsync(new List<Ticket>());
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Ticket>()).Returns(repoMock.Object);
            var result = await this.deltaProcessor.GetOfficialDeltaTicketProcessingStatusAsync(segmentId).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public async Task GetNodeStatusAsync_ShouldReturnNodes_IfInvokedAsync()
        {
            var ticket = new Ticket();
            var node = new UnapprovedOfficialNodes();
            node.NodeName = "Automation_16cbu";
            node.NodeStatus = "deltas";
            node.OperationDate = DateTime.UtcNow.ToTrue();
            IEnumerable<UnapprovedOfficialNodes> nodes = new List<UnapprovedOfficialNodes> { node };
            var repoMock = new Mock<IRepository<UnapprovedOfficialNodes>>();
            repoMock.Setup(r => r.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(nodes);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<UnapprovedOfficialNodes>()).Returns(repoMock.Object);
            var result = await this.deltaProcessor.GetUnapprovedOfficialNodesAsync(ticket).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result, nodes);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<UnapprovedOfficialNodes>(), Times.Once);
        }
    }
}
