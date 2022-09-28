// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaErrorExecutorTests.cs" company="Microsoft">
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
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.Interfaces;
    using Ecp.True.Processors.Delta.OfficialDeltaExecutors;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Error Executor Tests.
    /// </summary>
    [TestClass]
    public class OfficialDeltaErrorExecutorTests
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
        private OfficialDeltaData deltaData;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private Mock<ICompositeOfficialDeltaBuilder> mockCompositeOfficialDeltaBuilder;

        /// <summary>
        /// The mock ownership repository.
        /// </summary>
        private Mock<IRepository<DeltaNodeError>> mockDeltaErrorRepository;

        /// <summary>
        /// The mock ownership repository.
        /// </summary>
        private Mock<IRepository<DeltaNode>> mockDeltaNodeRepository;

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
            this.mockIUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockLogger = new Mock<ITrueLogger<ErrorExecutor>>();
            this.mockCompositeOfficialDeltaBuilder = new Mock<ICompositeOfficialDeltaBuilder>();
            this.mockDeltaErrorRepository = new Mock<IRepository<DeltaNodeError>>();
            this.mockDeltaNodeRepository = new Mock<IRepository<DeltaNode>>();
            this.mockTicketRepository = new Mock<IRepository<Ticket>>();
            this.deltaData = new OfficialDeltaData
            {
                Ticket = new Ticket { TicketId = 25281 },
            };
            var deltaNodes = new List<DeltaNode> { new DeltaNode { DeltaNodeId = 1, TicketId = 25281 } };
            var deltaErrors = new List<DeltaNodeError> { new DeltaNodeError { DeltaNodeId = 1, ErrorMessage = "test" } };
            this.mockCompositeOfficialDeltaBuilder.Setup(x => x.BuildErrorsAsync(It.IsAny<OfficialDeltaData>())).ReturnsAsync(deltaErrors);
            this.mockDeltaNodeRepository.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<DeltaNode, bool>>>())).ReturnsAsync(deltaNodes);
            this.mockTicketRepository.Setup(a => a.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(this.deltaData.Ticket);
            this.mockIUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<DeltaNode>()).Returns(this.mockDeltaNodeRepository.Object);
            this.mockIUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<DeltaNodeError>()).Returns(this.mockDeltaErrorRepository.Object);
            this.mockIUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);
            this.mockLogger = new Mock<ITrueLogger<ErrorExecutor>>();
            this.errorExecutor = new ErrorExecutor(this.mockLogger.Object, this.mockIUnitOfWorkFactory.Object, this.mockCompositeOfficialDeltaBuilder.Object);
            this.deltaData = new OfficialDeltaData
            {
                Ticket = new Ticket { TicketId = 25281 },
            };
        }

        [TestMethod]
        public void Type_ShouldReturnOrder_WhenInvoked()
        {
            var result = this.errorExecutor.Order;
            Assert.AreEqual(3, result);
        }

        /// <summary>
        /// Executes the asynchronous should process errors when inventory error is not empty asynchronous.
        /// </summary>
        /// <returns>The Delta.</returns>
        [TestMethod]
        public async Task ExecuteAsync_ShouldProcessErrors_WhenInventoryErrorIsNotEmptyAsync()
        {
            var inventoryErrors = new OfficialErrorInventory()
            {
                InventoryProductId = 5451,
                Description = "test",
            };

            this.deltaData.InventoryErrors = new List<OfficialErrorInventory> { inventoryErrors };
            await this.errorExecutor.ExecuteAsync(this.deltaData).ConfigureAwait(false);
            this.mockTicketRepository.Verify(r => r.GetByIdAsync(25281), Times.Once);
            Assert.IsTrue(this.deltaData.InventoryErrors.Any());
        }

        /// <summary>
        /// Executes the asynchronous should process errors when movement error is not empty asynchronous.
        /// </summary>
        /// <returns>The Delta.</returns>
        [TestMethod]
        public async Task ExecuteAsync_ShouldProcessErrors_WhenMovementErrorIsNotEmptyAsync()
        {
            var movementErrors = new OfficialErrorMovement()
            {
                MovementTransactionId = 5451,
                Description = "test",
            };

            this.deltaData.MovementErrors = new List<OfficialErrorMovement> { movementErrors };
            await this.errorExecutor.ExecuteAsync(this.deltaData).ConfigureAwait(false);
            this.mockTicketRepository.Verify(r => r.GetByIdAsync(25281), Times.Once);
            Assert.IsTrue(this.deltaData.MovementErrors.Any());
        }
    }
}
