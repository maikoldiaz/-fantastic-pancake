// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialRegisterMovementsExecutorTests.cs" company="Microsoft">
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
    using Ecp.True.Configuration;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.OfficialDeltaExecutors;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Complete Executor Tests.
    /// </summary>
    [TestClass]
    public class OfficialRegisterMovementsExecutorTests
    {
        /// <summary>
        /// The mock business context.
        /// </summary>
        private readonly Mock<IBusinessContext> mockBusinessContext = new Mock<IBusinessContext>();

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<RegisterMovementsExecutor>> mockLogger;

        /// <summary>
        /// The delta data.
        /// </summary>
        private OfficialDeltaData deltaData;

        /// <summary>
        /// The build executor.
        /// </summary>
        private IExecutor registerMovementsExecutor;

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
        private Mock<IRepository<DeltaNode>> mockDeltaNodeRepository;

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
            this.mockDeltaNodeRepository = new Mock<IRepository<DeltaNode>>();
            this.mockLogger = new Mock<ITrueLogger<RegisterMovementsExecutor>>();
            this.mockConfigurationHandler = new Mock<IConfigurationHandler>();
            this.mockIUnitOfWorkFactory.Setup(x => x.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<ServiceBusSettings>(It.IsAny<string>())).ReturnsAsync(new ServiceBusSettings());

            this.deltaData = new OfficialDeltaData()
            {
                Ticket = new Ticket { TicketId = 123 },
            };

            this.registerMovementsExecutor = new RegisterMovementsExecutor(
                this.mockBusinessContext.Object,
                this.mockIUnitOfWorkFactory.Object,
                this.mockLogger.Object);
        }

        /// <summary>
        /// Executes the asynchronous build executor asynchronous.
        /// </summary>
        /// <returns>Delta data.</returns>
        [TestMethod]
        public async Task ExecuteAsync_RegisterMovementsExecutorAsync()
        {
            this.mockMovementRepository.Setup(a => a.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()));
            this.mockUnitOfWork.Setup(a => a.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);

            await this.registerMovementsExecutor.ExecuteAsync(this.deltaData).ConfigureAwait(false);

            this.mockMovementRepository.Verify(a => a.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.SaveAsync(CancellationToken.None), Times.Once);
        }

        /// <summary>
        /// Builds the result executor should return the execution order when invoked.
        /// </summary>
        [TestMethod]
        public void RegisterMovementsExecutor_ShouldReturnTheExecutionOrder_WhenInvoked()
        {
            Assert.AreEqual(5, this.registerMovementsExecutor.Order);
        }

        /// <summary>
        /// Builds the result executor should return the process type when invoked.
        /// </summary>
        [TestMethod]
        public void RegisterMovementsExecutor_ShouldReturnTheProcessType_WhenInvoked()
        {
            Assert.AreEqual(ProcessType.OfficialDelta, this.registerMovementsExecutor.ProcessType);
        }
    }
}
