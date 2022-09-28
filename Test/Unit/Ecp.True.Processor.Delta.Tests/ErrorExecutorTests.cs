// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorExecutorTests.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Executors;
    using Ecp.True.Proxies.OwnershipRules.Request;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Error Executor Tests.
    /// </summary>
    [TestClass]
    public class ErrorExecutorTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<ErrorExecutor>> mockLogger;

        /// <summary>
        /// The error executor.
        /// </summary>
        private IExecutor errorExecutor;

        /// <summary>
        /// The delta data.
        /// </summary>
        private DeltaData deltaData;

        /// <summary>
        /// The delta request.
        /// </summary>
        private DeltaRequest deltaRequest;

        /// <summary>
        /// The delta response.
        /// </summary>
        private DeltaResponse deltaResponse;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock ownership repository.
        /// </summary>
        private Mock<IRepository<DeltaError>> mockDeltaErrorRepository;

        /// <summary>
        /// The mock ticket repository.
        /// </summary>
        private Mock<IRepository<Ticket>> mockTicketRepository;

        /// <summary>
        /// The mock i unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockIUnitOfWorkFactory;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockIUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockLogger = new Mock<ITrueLogger<ErrorExecutor>>();
            this.mockDeltaErrorRepository = new Mock<IRepository<DeltaError>>();
            this.mockTicketRepository = new Mock<IRepository<Ticket>>();
            this.deltaData = new DeltaData
            {
                Ticket = new Entities.TransportBalance.Ticket { TicketId = 25281 },
            };
            this.mockIUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<DeltaError>()).Returns(this.mockDeltaErrorRepository.Object);
            this.mockIUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<DeltaError>()).Returns(this.mockDeltaErrorRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);
            this.mockDeltaErrorRepository.Setup(a => a.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()));
            this.mockTicketRepository.Setup(a => a.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()));
            this.mockLogger = new Mock<ITrueLogger<ErrorExecutor>>();
            this.errorExecutor = new ErrorExecutor(this.mockLogger.Object, this.mockIUnitOfWorkFactory.Object);
            this.deltaData = new DeltaData
            {
                Ticket = new Entities.TransportBalance.Ticket { TicketId = 25281 },
            };
            this.deltaRequest = new DeltaRequest();
            this.deltaResponse = new DeltaResponse();
        }

        [TestMethod]
        public void Type_ShouldReturnOrder_WhenInvoked()
        {
            var result = this.errorExecutor.Order;
            Assert.AreEqual(4, result);
        }

        /// <summary>
        /// Executes the asynchronous should process errors when inventory error is not empty asynchronous.
        /// </summary>
        /// <returns>The Delta.</returns>
        [TestMethod]
        public async Task ExecuteAsync_ShouldProcessErrors_WhenInventoryErrorIsNotEmptyAsync()
        {
            var inventoryErrors = new ErrorInventory()
            {
                InventoryId = "5452",
                InventoryProductId = 5451,
                Description = "test",
            };
            var deltaInventoryErrors = new DeltaErrorInventory()
            {
                InventoryProductUniqueId = "5452",
                InventoryTransactionId = 5451,
                Description = "test",
            };
            var ticket = new Ticket()
            {
                TicketId = 25281,
            };
            this.deltaData.ErrorMovements = new List<ErrorMovement>();
            this.deltaData.ErrorInventories = new List<ErrorInventory> { inventoryErrors };
            this.deltaResponse.ErrorInventories = new List<DeltaErrorInventory> { deltaInventoryErrors };
            this.deltaResponse.ErrorMovements = new List<DeltaErrorMovement>();
            this.mockTicketRepository.Setup(r => r.GetByIdAsync(25281)).ReturnsAsync(ticket);
            await this.errorExecutor.ExecuteAsync(this.deltaData).ConfigureAwait(false);
            this.mockTicketRepository.Verify(r => r.GetByIdAsync(25281), Times.Once);
            Assert.IsTrue(this.deltaData.ErrorInventories.Any());
        }

        /// <summary>
        /// Executes the asynchronous should process errors when movement error is not empty asynchronous.
        /// </summary>
        /// <returns>The Delta.</returns>
        [TestMethod]
        public async Task ExecuteAsync_ShouldProcessErrors_WhenMovementErrorIsNotEmptyAsync()
        {
            var movementErrors = new ErrorMovement()
            {
                MovementId = "5452",
                MovementTransactionId = 5451,
                Description = "test",
            };

            var deltaMovementErrors = new DeltaErrorMovement()
            {
                MovementId = "5452",
                MovementTransactionId = 5451,
                Description = "test",
            };
            var ticket = new Ticket()
            {
                TicketId = 25281,
            };
            this.deltaData.ErrorMovements = new List<ErrorMovement> { movementErrors };
            this.deltaData.ErrorInventories = new List<ErrorInventory>();
            this.deltaResponse.ErrorInventories = new List<DeltaErrorInventory>();
            this.deltaResponse.ErrorMovements = new List<DeltaErrorMovement> { deltaMovementErrors };
            this.mockTicketRepository.Setup(r => r.GetByIdAsync(25281)).ReturnsAsync(ticket);

            await this.errorExecutor.ExecuteAsync(this.deltaData).ConfigureAwait(false);

            this.mockTicketRepository.Verify(r => r.GetByIdAsync(25281), Times.Once);
            Assert.IsTrue(this.deltaData.ErrorMovements.Any());
        }

        /// <summary>
        /// Errors the executor should return the execution order when invoked.
        /// </summary>
        [TestMethod]
        public void ErrorExecutor_ShouldReturnTheExecutionOrder_WhenInvoked()
        {
            Assert.AreEqual(4, this.errorExecutor.Order);
        }

        /// <summary>
        /// Errors the executor should return the process type when invoked.
        /// </summary>
        [TestMethod]
        public void ErrorExecutor_ShouldReturnTheProcessType_WhenInvoked()
        {
            Assert.AreEqual(ProcessType.Delta, this.errorExecutor.ProcessType);
        }
    }
}
