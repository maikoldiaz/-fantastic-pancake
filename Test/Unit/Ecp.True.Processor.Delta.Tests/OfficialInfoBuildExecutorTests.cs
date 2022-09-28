// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialInfoBuildExecutorTests.cs" company="Microsoft">
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
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.OfficialDeltaExecutors;
    using Ecp.True.Processors.Delta.Specifications;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The OfficialInfoBuildExecutorTests.
    /// </summary>
    [TestClass]
    public class OfficialInfoBuildExecutorTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<InfoBuildExecutor>> mockLogger;

        /// <summary>
        /// the mockIRepositoryFactory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockIUnitOfWorkFactory;

        /// <summary>
        /// The information build executor.
        /// </summary>
        private IExecutor infoBuildExecutor;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock repository factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockRepositoryFactory;

        /// <summary>
        /// The mock ticket repository.
        /// </summary>
        private Mock<IRepository<Annulation>> mockAnnulationRepository;

        /// <summary>
        /// The mock ticket repository.
        /// </summary>
        private Mock<IRepository<ConsolidatedMovement>> mockConsolidatedMovementRepository;

        /// <summary>
        /// The mock ticket repository.
        /// </summary>
        private Mock<IRepository<ConsolidatedInventoryProduct>> mockConsolidatedInventoryProductRepository;

        /// <summary>
        /// The mock ticket repository.
        /// </summary>
        private Mock<IRepository<OfficialDeltaMovement>> mockOfficialDeltaMovementRepository;

        /// <summary>
        /// The mock ticket repository.
        /// </summary>
        private Mock<IRepository<OfficialDeltaInventory>> mockOfficialDeltaInventoryRepository;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockRepositoryFactory = new Mock<IRepositoryFactory>();
            this.mockAnnulationRepository = new Mock<IRepository<Annulation>>();
            this.mockConsolidatedMovementRepository = new Mock<IRepository<ConsolidatedMovement>>();
            this.mockConsolidatedInventoryProductRepository = new Mock<IRepository<ConsolidatedInventoryProduct>>();
            this.mockLogger = new Mock<ITrueLogger<InfoBuildExecutor>>();
            this.mockIUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockIUnitOfWorkFactory.Setup(x => x.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockOfficialDeltaMovementRepository = new Mock<IRepository<OfficialDeltaMovement>>();
            this.mockOfficialDeltaInventoryRepository = new Mock<IRepository<OfficialDeltaInventory>>();
            this.infoBuildExecutor = new InfoBuildExecutor(this.mockLogger.Object, this.mockIUnitOfWorkFactory.Object);
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
            var officialDeltaData = new OfficialDeltaData { Ticket = new Ticket { TicketId = 123, StartDate = DateTime.Now.AddMonths(-1), EndDate = DateTime.Now } };
            var cancellationTypes = new List<Annulation>() { new Annulation { IsActive = true } };
            var consolidatedMovement = new List<ConsolidatedMovement>() { new ConsolidatedMovement { Ticket = new Ticket { TicketId = 123 } } };
            var consolidatedInventoryProduct = new List<ConsolidatedInventoryProduct>() { new ConsolidatedInventoryProduct { Ticket = new Ticket { TicketId = 123 } } };
            this.mockUnitOfWork.Setup(a => a.CreateRepository<Annulation>()).Returns(this.mockAnnulationRepository.Object);
            this.mockAnnulationRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Annulation, bool>>>())).ReturnsAsync(cancellationTypes);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedMovement>()).Returns(this.mockConsolidatedMovementRepository.Object);
            this.mockConsolidatedMovementRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<ConsolidatedMovement, bool>>>())).ReturnsAsync(consolidatedMovement);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidatedInventoryProduct>()).Returns(this.mockConsolidatedInventoryProductRepository.Object);
            this.mockConsolidatedInventoryProductRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<ConsolidatedInventoryProduct, bool>>>())).ReturnsAsync(consolidatedInventoryProduct);
            this.mockUnitOfWork.Setup(x => x.CreateRepository<OfficialDeltaMovement>()).Returns(this.mockOfficialDeltaMovementRepository.Object);
            this.mockUnitOfWork.Setup(x => x.CreateRepository<OfficialDeltaInventory>()).Returns(this.mockOfficialDeltaInventoryRepository.Object);
            this.mockOfficialDeltaMovementRepository.Setup(x => x.ExecuteAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()));
            this.mockOfficialDeltaInventoryRepository.Setup(x => x.ExecuteAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()));
            await this.infoBuildExecutor.ExecuteAsync(officialDeltaData).ConfigureAwait(false);

            this.mockAnnulationRepository.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Annulation, bool>>>()), Times.Once);
            this.mockConsolidatedInventoryProductRepository.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<ConsolidatedInventoryProduct, bool>>>(), "ConsolidatedOwners"), Times.Once);
            this.mockConsolidatedMovementRepository
                .Verify(
                    r => r.GetAllSpecificAsync(
                It.IsAny<ConsolidatedMovementSpecification>()), Times.Once);
        }
    }
}
