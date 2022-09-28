// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessResultExecutorTests.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Executors;
    using EfCore.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Complete Executor Tests.
    /// </summary>
    [TestClass]
    public class ProcessResultExecutorTests
    {
        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock i unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockIUnitOfWorkFactory;

        /// <summary>
        /// The node tag mock.
        /// </summary>
        private Mock<IRepository<NodeTag>> mockNodeTagRespostiory;

        /// <summary>
        /// the iventory product mock.
        /// </summary>
        private Mock<IRepository<InventoryProduct>> mockInventoryProductRespostiory;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<ProcessResultExecutor>> mockLogger;

        /// <summary>
        /// The process executor.
        /// </summary>
        private IExecutor processExecutor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<ProcessResultExecutor>>();
            this.mockIUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockNodeTagRespostiory = new Mock<IRepository<NodeTag>>();
            this.mockInventoryProductRespostiory = new Mock<IRepository<InventoryProduct>>();
            this.mockIUnitOfWorkFactory.Setup(x => x.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.processExecutor = new ProcessResultExecutor(this.mockLogger.Object, this.mockIUnitOfWorkFactory.Object);
        }

        [TestMethod]
        public void Type_ShouldReturnOrder_WhenInvoked()
        {
            var result = this.processExecutor.Order;
            Assert.AreEqual(5, result);
        }

        /// <summary>
        /// Executes the asynchronous build executor asynchronous.
        /// </summary>
        /// <returns>Delta data.</returns>
        [TestMethod]
        public async Task ExecuteAsync_CompleteExecutorAsync()
        {
            var originalMovements = new List<OriginalMovement> { new OriginalMovement { } };
            var updatedMovement = new List<UpdatedMovement> { new UpdatedMovement { } };
            var originalInventory = new List<OriginalInventory> { new OriginalInventory { } };
            var updatedInventory = new List<UpdatedInventory> { new UpdatedInventory { } };
            var cancellationTypes = new List<Annulation> { new Annulation { } };
            var ticket = new Ticket
            {
                EndDate = DateTime.UtcNow,
            };
            var deltaData = new DeltaData { Ticket = new Ticket { TicketId = 123 } };
            deltaData.OriginalMovements = originalMovements;
            deltaData.UpdatedMovements = updatedMovement;
            deltaData.OriginalInventories = originalInventory;
            deltaData.UpdatedInventories = updatedInventory;
            deltaData.CancellationTypes = cancellationTypes;
            var mockAnnulationRepository = new Mock<IRepository<Annulation>>();
            var movementRepository = new Mock<IRepository<Movement>>();
            var mockTicketInfoRepository = new Mock<ITicketInfoRepository>();
            this.mockIUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().TicketInfoRepository).Returns(mockTicketInfoRepository.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Annulation>()).Returns(mockAnnulationRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<NodeTag>()).Returns(this.mockNodeTagRespostiory.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<InventoryProduct>()).Returns(this.mockInventoryProductRespostiory.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<Movement>()).Returns(movementRepository.Object);
            mockTicketInfoRepository.Setup(x => x.GetLastTicketAsync(It.IsAny<int>(), It.IsAny<TicketType>())).ReturnsAsync(ticket);
            mockAnnulationRepository.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Annulation, bool>>>())).ReturnsAsync(deltaData.CancellationTypes);

            await this.processExecutor.ExecuteAsync(deltaData).ConfigureAwait(false);

            Assert.IsNotNull(deltaData.Movements);
            Assert.IsNotNull(deltaData.InventoryProducts);
            Assert.IsNotNull(deltaData.NextCutOffDate);
            Assert.IsNotNull(deltaData.NodeTags);
            Assert.IsNotNull(deltaData.CancellationTypes);
            mockAnnulationRepository.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Annulation, bool>>>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<NodeTag>(), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<InventoryProduct>(), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<Movement>(), Times.Once);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<Annulation>(), Times.Once);
        }

        /// <summary>
        /// Processes the executor should return the process type when invoked.
        /// </summary>
        [TestMethod]
        public void ProcessExecutor_ShouldReturnTheProcessType_WhenInvoked()
        {
            Assert.AreEqual(ProcessType.Delta, this.processExecutor.ProcessType);
        }
    }
}
