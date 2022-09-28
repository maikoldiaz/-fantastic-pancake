// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaFailureHandlerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Tests.FailureHandler
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.HandleFailure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Official Delta Failure Handler Tests.
    /// </summary>
    [TestClass]
    public class OfficialDeltaFailureHandlerTests
    {
        /// <summary>
        /// The failure handler mock.
        /// </summary>
        private readonly Mock<ITelemetry> telemetryMock = new Mock<ITelemetry>();

        /// <summary>
        /// The service provider mock.
        /// </summary>
        private readonly Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();

        /// <summary>
        /// The logger mock.
        /// </summary>
        private readonly Mock<ITrueLogger<OfficialDeltaFailureHandler>> loggerMock = new Mock<ITrueLogger<OfficialDeltaFailureHandler>>();

        /// <summary>
        /// The official Delta failure handler.
        /// </summary>
        private OfficialDeltaFailureHandler officialDeltaFailureHandler;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock ticket repository.
        /// </summary>
        private Mock<IRepository<Ticket>> mockTicketRepository;

        /// <summary>
        /// The Consolidation Nodes repository.
        /// </summary>
        private Mock<IRepository<ConsolidationNodesStatus>> mockConsolidationNodesRepository;

        /// <summary>
        /// The DeltaNode  repository.
        /// </summary>
        private Mock<IRepository<DeltaNode>> mockDeltaNodesRepository;

        /// <summary>
        /// Initilizes this instance.
        /// </summary>
        [TestInitialize]
        public void Initilize()
        {
            this.mockDeltaNodesRepository = new Mock<IRepository<DeltaNode>>();
            this.mockTicketRepository = new Mock<IRepository<Ticket>>();
            this.mockConsolidationNodesRepository = new Mock<IRepository<ConsolidationNodesStatus>>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockUnitOfWork.Setup(a => a.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);
            this.mockTicketRepository.Setup(a => a.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Ticket { StartDate = DateTime.Today });
            this.serviceProviderMock.Setup(s => s.GetService(typeof(ITrueLogger<OfficialDeltaFailureHandler>))).Returns(this.loggerMock.Object);
            this.officialDeltaFailureHandler = new OfficialDeltaFailureHandler(this.loggerMock.Object, this.telemetryMock.Object);
        }

        /// <summary>
        /// Gets the official delta failure hanler.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialDeltaHandleFailure_ShouldNodeStatusOk_WhenInvokedAsync()
        {
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidationNodesStatus>()).Returns(this.mockConsolidationNodesRepository.Object);
            this.mockConsolidationNodesRepository.Setup(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<ConsolidationNodesStatus> { new ConsolidationNodesStatus() });
            await this.officialDeltaFailureHandler.HandleFailureAsync(this.mockUnitOfWork.Object, new FailureInfo(1, string.Empty)).ConfigureAwait(false);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<Ticket>(), Times.Exactly(1));
            this.mockTicketRepository.Verify(a => a.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Exactly(1));
            this.mockConsolidationNodesRepository.Verify(x => x.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Gets the official delta failure hanler.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OfficialDeltaHandleFailure_ShouldNodeStatusNoOk_WhenInvokedAsync()
        {
            this.mockUnitOfWork.Setup(a => a.CreateRepository<DeltaNode>()).Returns(this.mockDeltaNodesRepository.Object);
            this.mockDeltaNodesRepository.Setup(a => a.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new DeltaNode { DeltaNodeId = 1 });
            this.mockUnitOfWork.Setup(a => a.CreateRepository<ConsolidationNodesStatus>()).Returns(this.mockConsolidationNodesRepository.Object);
            this.mockConsolidationNodesRepository.Setup(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<ConsolidationNodesStatus> { });
            await this.officialDeltaFailureHandler.HandleFailureAsync(this.mockUnitOfWork.Object, new FailureInfo(1, string.Empty)).ConfigureAwait(false);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<Ticket>(), Times.Exactly(1));
            this.mockUnitOfWork.Verify(a => a.CreateRepository<DeltaNode>(), Times.Exactly(1));
            this.mockConsolidationNodesRepository.Verify(x => x.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
        }
    }
}
