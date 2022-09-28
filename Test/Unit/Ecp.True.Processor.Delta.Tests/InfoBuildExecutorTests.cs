// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfoBuildExecutorTests.cs" company="Microsoft">
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
    using Ecp.True.DataAccess.Interfaces;
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
    /// The InfoBuildExecutorTests.
    /// </summary>
    [TestClass]
    public class InfoBuildExecutorTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<InfoBuildExecutor>> mockLogger;

        /// <summary>
        /// the mockIRepositoryFactory.
        /// </summary>
        private Mock<IRepositoryFactory> mockIRepositoryFactory;

        /// <summary>
        /// The information build executor.
        /// </summary>
        private IExecutor infoBuildExecutor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<InfoBuildExecutor>>();
            this.mockIRepositoryFactory = new Mock<IRepositoryFactory>();
            this.infoBuildExecutor = new InfoBuildExecutor(this.mockIRepositoryFactory.Object, this.mockLogger.Object);
        }

        /// <summary>
        /// Informations the build executor should return order.
        /// </summary>
        [TestMethod]
        public void InfoBuildExecutor_ShouldReturnOrder()
        {
            Assert.AreEqual(1, this.infoBuildExecutor.Order);
        }

        /// <summary>
        /// Executes the asynchronous should format data when request formed asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExecuteAsync_ShouldFormatData_WhenRequestFormedAsync()
        {
            var deltaData = new DeltaData { Ticket = new Ticket { TicketId = 123 } };
            var originalMovements = new List<OriginalMovement> { new OriginalMovement { } };
            var updatedMovement = new List<UpdatedMovement> { new UpdatedMovement { } };
            var originalInventory = new List<OriginalInventory> { new OriginalInventory { } };
            var updatedInventory = new List<UpdatedInventory> { new UpdatedInventory { } };

            var mockOriginalMovementRepository = new Mock<IRepository<OriginalMovement>>();
            mockOriginalMovementRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(originalMovements);

            var mockUpdatedMovementRepository = new Mock<IRepository<UpdatedMovement>>();
            mockUpdatedMovementRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(updatedMovement);

            var mockOriginalInventoryRepository = new Mock<IRepository<OriginalInventory>>();
            mockOriginalInventoryRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(originalInventory);

            var mockUpdatedInventoryRepository = new Mock<IRepository<UpdatedInventory>>();
            mockUpdatedInventoryRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(updatedInventory);

            this.mockIRepositoryFactory.Setup(a => a.CreateRepository<OriginalMovement>()).Returns(mockOriginalMovementRepository.Object);
            this.mockIRepositoryFactory.Setup(a => a.CreateRepository<UpdatedMovement>()).Returns(mockUpdatedMovementRepository.Object);
            this.mockIRepositoryFactory.Setup(a => a.CreateRepository<OriginalInventory>()).Returns(mockOriginalInventoryRepository.Object);
            this.mockIRepositoryFactory.Setup(a => a.CreateRepository<UpdatedInventory>()).Returns(mockUpdatedInventoryRepository.Object);

            await this.infoBuildExecutor.ExecuteAsync(deltaData).ConfigureAwait(false);

            this.mockIRepositoryFactory.Verify(a => a.CreateRepository<OriginalMovement>(), Times.Exactly(1));
            this.mockIRepositoryFactory.Verify(a => a.CreateRepository<UpdatedMovement>(), Times.Exactly(1));
            this.mockIRepositoryFactory.Verify(a => a.CreateRepository<OriginalInventory>(), Times.Exactly(1));
            this.mockIRepositoryFactory.Verify(a => a.CreateRepository<UpdatedInventory>(), Times.Exactly(1));
        }

        /// <summary>
        /// Informations the build executor should return the process type when invoked.
        /// </summary>
        [TestMethod]
        public void InfoBuildExecutor_ShouldReturnTheProcessType_WhenInvoked()
        {
            Assert.AreEqual(ProcessType.Delta, this.infoBuildExecutor.ProcessType);
        }
    }
}
