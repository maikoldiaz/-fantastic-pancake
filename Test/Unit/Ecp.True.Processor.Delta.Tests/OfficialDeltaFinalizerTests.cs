// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaFinalizerTests.cs" company="Microsoft">
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
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Delta;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class OfficialDeltaFinalizerTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private readonly Mock<ITrueLogger<OfficialDeltaFinalizer>> mockLogger = new Mock<ITrueLogger<OfficialDeltaFinalizer>>();

        /// <summary>
        /// The factory.
        /// </summary>
        private readonly Mock<IUnitOfWorkFactory> mockFactory = new Mock<IUnitOfWorkFactory>();

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private readonly Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();

        /// <summary>
        /// The mock movement repository.
        /// </summary>
        private readonly Mock<IRepository<DeltaNode>> mockDeltaNodeRepository = new Mock<IRepository<DeltaNode>>();

        /// <summary>
        /// The mock unbalance repository.
        /// </summary>
        private readonly Mock<IRepository<DeltaBalance>> mockDeltaBalanceRepository = new Mock<IRepository<DeltaBalance>>();

        /// <summary>
        /// The client.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The mock analysis service client.
        /// </summary>
        private readonly Mock<IAnalysisServiceClient> mockAnalysisServiceClient = new Mock<IAnalysisServiceClient>();

        /// <summary>
        /// The mock ticket repository.
        /// </summary>
        private readonly Mock<IRepository<Ticket>> mockTicketRepository = new Mock<IRepository<Ticket>>();

        /// <summary>
        /// The operational cut off finalizer.
        /// </summary>
        private OfficialDeltaFinalizer officialDeltaFinalizer;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockAnalysisServiceClient.Setup(m => m.RefreshCalculationAsync(1));
            this.mockAzureClientFactory.Setup(m => m.AnalysisServiceClient).Returns(this.mockAnalysisServiceClient.Object);
            this.mockFactory.Setup(m => m.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockDeltaNodeRepository.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<DeltaNode, bool>>>())).ReturnsAsync(new List<DeltaNode>());
            this.mockDeltaNodeRepository.Setup(m => m.UpdateAll(It.IsAny<IEnumerable<DeltaNode>>()));
            this.mockUnitOfWork.Setup(m => m.CreateRepository<DeltaNode>()).Returns(this.mockDeltaNodeRepository.Object);

            this.mockTicketRepository.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Ticket { Status = StatusType.PROCESSING });
            this.mockTicketRepository.Setup(m => m.UpdateAll(It.IsAny<IEnumerable<Ticket>>()));
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);

            this.mockDeltaBalanceRepository.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<DeltaBalance, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<DeltaBalance> { new DeltaBalance { NodeId = 1 } });
            this.mockUnitOfWork.Setup(m => m.CreateRepository<DeltaBalance>()).Returns(this.mockDeltaBalanceRepository.Object);
            this.officialDeltaFinalizer = new OfficialDeltaFinalizer(this.mockLogger.Object, this.mockAzureClientFactory.Object, this.mockFactory.Object);
        }

        /// <summary>
        /// Types the should return ticket type cutoff when invoked.
        /// </summary>
        [TestMethod]
        public void Type_ShouldReturnTicketTypeOfficialDelta_WhenInvoked()
        {
            var result = this.officialDeltaFinalizer.Type;

            Assert.AreEqual(TicketType.OfficialDelta, result);
        }

        [TestMethod]
        public async Task ProcessAsync_ShouldCallStoredProceduresAndUpdateTicket_WhenInvokedAsync()
        {
            var parameters = new Dictionary<string, object>
            {
                { "@StartDate", new DateTime(2020, 6, 1) },
                { "@EndDate", new DateTime(2020, 6, 30) },
            };

            // Act
            await this.officialDeltaFinalizer.ProcessAsync(1).ConfigureAwait(false);

            // Assert
            this.mockDeltaBalanceRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveOfficialDeltaBalance, parameters), Times.Never);
            this.mockDeltaBalanceRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveOfficialDeltaMovementDetails, parameters), Times.Never);
            this.mockDeltaBalanceRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveOfficialDeltaInventoryDetails, parameters), Times.Never);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<DeltaNode>(), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Ticket>(), Times.AtLeastOnce);
            this.mockAnalysisServiceClient.Verify(m => m.RefreshCalculationAsync(1), Times.Never);
            this.mockAzureClientFactory.Verify(m => m.AnalysisServiceClient, Times.Once);
            this.mockDeltaNodeRepository.Verify(m => m.UpdateAll(It.IsAny<IEnumerable<DeltaNode>>()), Times.Once);
            this.mockTicketRepository.Verify(m => m.UpdateAll(It.IsAny<IEnumerable<Ticket>>()), Times.Never);
        }
    }
}
