// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationExecutorTests.cs" company="Microsoft">
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
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Executors;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The ValidationExecutorTests.
    /// </summary>
    [TestClass]
    public class ValidationExecutorTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<ValidationExecutor>> mockLogger;

        /// <summary>
        /// The validation executor.
        /// </summary>
        private IExecutor validationExecutor;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

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
            this.mockLogger = new Mock<ITrueLogger<ValidationExecutor>>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();

            this.mockUnitOfWork.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));
            this.mockIUnitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.validationExecutor = new ValidationExecutor(this.mockLogger.Object, this.mockIUnitOfWorkFactory.Object);
        }

        /// <summary>
        /// Requests the invocation executor should return order.
        /// </summary>
        [TestMethod]
        public void RequestInvocationExecutor_ShouldReturnOrder()
        {
            Assert.AreEqual(2, this.validationExecutor.Order);
        }

        /// <summary>
        /// Executes the asynchronous should return reponse asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExecuteAsync_ShouldReturnReponseAsync()
        {
            var originalMovements = new List<OriginalMovement> { new OriginalMovement { } };
            var updatedMovement = new List<UpdatedMovement> { new UpdatedMovement { } };
            var originalInventory = new List<OriginalInventory> { new OriginalInventory { } };
            var updatedInventory = new List<UpdatedInventory> { new UpdatedInventory { } };

            var deltaData = new DeltaData { Ticket = new Ticket { TicketId = 123 } };
            deltaData.OriginalMovements = originalMovements;
            deltaData.UpdatedMovements = updatedMovement;
            deltaData.OriginalInventories = originalInventory;
            deltaData.UpdatedInventories = updatedInventory;

            var mockDeltaErrorRepository = new Mock<IRepository<DeltaError>>();
            mockDeltaErrorRepository.Setup(a => a.InsertAll(It.IsAny<IEnumerable<DeltaError>>()));

            var ticketRepository = new Mock<IRepository<Ticket>>();
            ticketRepository.Setup(a => a.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Ticket());
            ticketRepository.Setup(a => a.Update(It.IsAny<Ticket>()));

            this.mockUnitOfWork.Setup(a => a.CreateRepository<DeltaError>()).Returns(mockDeltaErrorRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<Ticket>()).Returns(ticketRepository.Object);

            await this.validationExecutor.ExecuteAsync(deltaData).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(a => a.CreateRepository<DeltaError>(), Times.Once);
            mockDeltaErrorRepository.Verify(a => a.InsertAll(It.IsAny<IEnumerable<DeltaError>>()), Times.Once);
            ticketRepository.Verify(a => a.GetByIdAsync(It.IsAny<int>()), Times.Once);
            ticketRepository.Verify(a => a.Update(It.IsAny<Ticket>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Validations the executor should return the process type when invoked.
        /// </summary>
        [TestMethod]
        public void ValidationExecutor_ShouldReturnTheProcessType_WhenInvoked()
        {
            Assert.AreEqual(ProcessType.Delta, this.validationExecutor.ProcessType);
        }
    }
}
