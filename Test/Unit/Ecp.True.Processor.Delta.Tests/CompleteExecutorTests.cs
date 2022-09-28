// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompleteExecutorTests.cs" company="Microsoft">
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
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Executors;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Complete Executor Tests.
    /// </summary>
    [TestClass]
    public class CompleteExecutorTests
    {
        /// <summary>
        /// The mock registration strategy factory.
        /// </summary>
        private readonly Mock<IRegistrationStrategyFactory> mockRegistrationStrategyFactory = new Mock<IRegistrationStrategyFactory>();

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<CompleteExecutor>> mockLogger;

        /// <summary>
        /// The delta data.
        /// </summary>
        private DeltaData deltaData;

        /// <summary>
        /// The build executor.
        /// </summary>
        private IExecutor completeExecutor;

        /// <summary>
        /// The mock ownership repository.
        /// </summary>
        private Mock<IRepository<DeltaError>> mockDeltaErrorRepository;

        /// <summary>
        /// The mock movement repository.
        /// </summary>
        private Mock<IRepository<Movement>> mockMovementRepository;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock ticket repository.
        /// </summary>
        private Mock<IRepository<Ticket>> mockTicketRepository;

        /// <summary>
        /// The mock inventory repository.
        /// </summary>
        private Mock<IRepository<InventoryProduct>> mockInventoryRepository;

        /// <summary>
        /// The mock i unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockIUnitOfWorkFactory;

        /// <summary>
        /// The mock inventory product repository.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigurationHandler;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockIUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockTicketRepository = new Mock<IRepository<Ticket>>();
            this.mockMovementRepository = new Mock<IRepository<Movement>>();
            this.mockInventoryRepository = new Mock<IRepository<InventoryProduct>>();
            this.mockLogger = new Mock<ITrueLogger<CompleteExecutor>>();
            this.mockConfigurationHandler = new Mock<IConfigurationHandler>();
            this.mockDeltaErrorRepository = new Mock<IRepository<DeltaError>>();
            this.mockIUnitOfWorkFactory.Setup(x => x.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<ServiceBusSettings>(It.IsAny<string>())).ReturnsAsync(new ServiceBusSettings());

            this.deltaData = new DeltaData
            {
                Ticket = new Entities.TransportBalance.Ticket { TicketId = 25281 },
            };
            this.completeExecutor = new CompleteExecutor(
                this.mockRegistrationStrategyFactory.Object,
                this.mockIUnitOfWorkFactory.Object,
                this.mockLogger.Object);
        }

        [TestMethod]
        public void Type_ShouldReturnOrder_WhenInvoked()
        {
            var result = this.completeExecutor.Order;
            Assert.AreEqual(7, result);
        }

        /// <summary>
        /// Executes the asynchronous build executor asynchronous.
        /// </summary>
        /// <returns>Delta data.</returns>
        [TestMethod]
        public async Task ExecuteAsync_CompleteExecutorAsync()
        {
            var ticket = new Ticket()
            {
                TicketId = 25281,
            };
            var tickets = new List<Ticket>();
            tickets.Add(ticket);
            var resultMovement = new ResultMovement()
            {
                MovementId = "1",
                MovementTransactionId = 2,
            };
            var updatedMovement = new UpdatedMovement()
            {
                MovementId = "1",
                MovementTransactionId = 2,
            };
            var updatedInventory = new UpdatedInventory()
            {
                InventoryProductUniqueId = "1",
                InventoryProductId = 1,
            };
            this.deltaData.ResultMovements = new List<ResultMovement> { resultMovement };
            this.deltaData.UpdatedMovements = new List<UpdatedMovement> { updatedMovement };
            this.deltaData.UpdatedInventories = new List<UpdatedInventory> { updatedInventory };
            this.mockUnitOfWork.Setup(a => a.CreateRepository<DeltaError>()).Returns(this.mockDeltaErrorRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<InventoryProduct>()).Returns(this.mockInventoryRepository.Object);
            this.mockTicketRepository.Setup(r => r.GetByIdAsync(25281)).ReturnsAsync(ticket);
            var movements = new List<Movement>() { new Movement { MovementId = "1" } };
            var inventories = new List<InventoryProduct>() { new InventoryProduct { InventoryId = "1", InventoryProductUniqueId = "1" } };
            this.mockMovementRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>())).ReturnsAsync(movements);
            this.mockInventoryRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>())).ReturnsAsync(inventories);
            this.mockUnitOfWork.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));
            this.mockRegistrationStrategyFactory.Setup(m => m.MovementRegistrationStrategy.Insert(It.IsAny<IEnumerable<object>>(), It.IsAny<UnitOfWork>()));

            await this.completeExecutor.ExecuteAsync(this.deltaData).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(a => a.CreateRepository<Ticket>(), Times.Once);
            this.mockTicketRepository.Verify(r => r.GetByIdAsync(25281), Times.Once);
            this.mockMovementRepository.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>()), Times.Once);
            this.mockInventoryRepository.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<InventoryProduct, bool>>>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Builds the result executor should return the execution order when invoked.
        /// </summary>
        [TestMethod]
        public void CompleteExecutor_ShouldReturnTheExecutionOrder_WhenInvoked()
        {
            Assert.AreEqual(7, this.completeExecutor.Order);
        }

        /// <summary>
        /// Builds the result executor should return the process type when invoked.
        /// </summary>
        [TestMethod]
        public void CompleteExecutor_ShouldReturnTheProcessType_WhenInvoked()
        {
            Assert.AreEqual(ProcessType.Delta, this.completeExecutor.ProcessType);
        }
    }
}
