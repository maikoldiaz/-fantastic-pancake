// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationalCutOffFinalizerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Balance.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.ExceptionHandling;
    using Ecp.True.ExceptionHandling.Core;
    using Ecp.True.Ioc.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Balance;
    using Ecp.True.Processors.Balance.Interfaces;
    using Ecp.True.Processors.Balance.Retry;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class OperationalCutOffFinalizerTests
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly SystemSettings settings = new SystemSettings();

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
        private readonly Mock<IRepository<Movement>> mockMovementRepository = new Mock<IRepository<Movement>>();

        /// <summary>
        /// The mock unbalance repository.
        /// </summary>
        private readonly Mock<IRepository<Unbalance>> mockUnbalanceRepository = new Mock<IRepository<Unbalance>>();

        /// <summary>
        /// The mock offchain node repository.
        /// </summary>
        private readonly Mock<IRepository<OffchainNode>> mockOffchainNodeRepository = new Mock<IRepository<OffchainNode>>();

        /// <summary>
        /// The client.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The mock queue client.
        /// </summary>
        private readonly Mock<IServiceBusQueueClient> mockQueueClient = new Mock<IServiceBusQueueClient>();

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
        private OperationalCutOffFinalizer operationalCutOffFinalizer;

        /// <summary>
        /// The retry policy factory.
        /// </summary>
        private IRetryPolicyFactory retryPolicyFactory;

        /// <summary>
        /// The CutOff Balance retry handler.
        /// </summary>
        private ICutOffBalanceRetryHandler retryHandler;

        /// <summary>
        /// The configuration mock.
        /// </summary>
        private Mock<IConfigurationHandler> configMock;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.settings.MaxCutOffRetryCount = 3;
            this.settings.CutOffRetryIntervalInSeconds = 10;
            this.settings.CutOffRetryStrategy = 2;

            this.mockQueueClient.Setup(m => m.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()));
            this.mockAzureClientFactory.Setup(m => m.GetQueueClient(It.IsAny<string>())).Returns(this.mockQueueClient.Object);
            this.retryPolicyFactory = new RetryPolicyFactory();
            this.configMock = new Mock<IConfigurationHandler>();
            this.configMock.Setup(c => c.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings)).ReturnsAsync(this.settings);
            var retryHandlerLoggerMock = new Mock<ITrueLogger<CutOffBalanceRetryHandler>>();
            var mockResolver = new Mock<IResolver>();
            mockResolver.Setup(x => x.GetInstance<ITrueLogger<CutOffBalanceRetryHandler>>()).Returns(retryHandlerLoggerMock.Object);
            this.retryHandler = new CutOffBalanceRetryHandler(mockResolver.Object);
            this.mockAnalysisServiceClient.Setup(m => m.RefreshCalculationAsync(1));
            this.mockAzureClientFactory.Setup(m => m.AnalysisServiceClient).Returns(this.mockAnalysisServiceClient.Object);
            this.mockFactory.Setup(m => m.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockMovementRepository.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Movement> { new Movement { MovementTransactionId = 1 } });
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);

            this.mockTicketRepository.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Ticket { Status = StatusType.PROCESSED });
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);

            this.mockUnbalanceRepository.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Unbalance, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Unbalance> { new Unbalance { NodeId = 1, Node = new Node { NodeId = 1 } } });
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Unbalance>()).Returns(this.mockUnbalanceRepository.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<OffchainNode>()).Returns(this.mockOffchainNodeRepository.Object);
            this.mockOffchainNodeRepository.Setup(m => m.InsertAll(It.IsAny<IEnumerable<OffchainNode>>()));
            this.operationalCutOffFinalizer = new OperationalCutOffFinalizer(this.mockAzureClientFactory.Object,this.retryPolicyFactory,this.retryHandler, this.configMock.Object, this.mockFactory.Object);
        }

        /// <summary>
        /// Types the should return ticket type cutoff when invoked.
        /// </summary>
        [TestMethod]
        public void Type_ShouldReturnTicketTypeCutoff_WhenInvoked()
        {
            var result = this.operationalCutOffFinalizer.Type;

            Assert.AreEqual(TicketType.Cutoff, result);
        }

        [TestMethod]
        public async Task ProcessAsync_ShouldSendMessagesToQueues_WhenInvokedAsync()
        {
            var parameters = new Dictionary<string, object>
            {
                { "@TicketId", 1 },
            };

            // Act
            await this.operationalCutOffFinalizer.ProcessAsync(1).ConfigureAwait(false);

            this.mockUnbalanceRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveAttributeDetails, parameters), Times.Once);
            this.mockUnbalanceRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveBackupMovementDetails, parameters), Times.Once);
            this.mockUnbalanceRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveMovementDetails, parameters), Times.Once);
            this.mockUnbalanceRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveInventoryDetails, parameters), Times.Once);
            this.mockUnbalanceRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveKPIDataByCategoryElementNode, parameters), Times.Once);
            this.mockUnbalanceRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveMovementsByProduct, parameters), Times.Once);
            this.mockUnbalanceRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveQualityDetails, parameters), Times.Once);
            this.mockUnbalanceRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveBalanceControl, parameters), Times.Once);

            // Assert
            this.mockQueueClient.Verify(m => m.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Exactly(3));
            this.mockAzureClientFactory.Verify(m => m.GetQueueClient(It.IsAny<string>()), Times.Exactly(3));
            this.mockMovementRepository.Verify(m => m.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockUnbalanceRepository.Verify(m => m.GetAllAsync(It.IsAny<Expression<Func<Unbalance, bool>>>(), It.IsAny<string[]>()), Times.Exactly(2));
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Movement>(), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Unbalance>(), Times.Exactly(3));
            this.mockUnitOfWork.Verify(m => m.CreateRepository<OffchainNode>(), Times.Once);
            this.mockAnalysisServiceClient.Verify(m => m.RefreshCalculationAsync(1), Times.Exactly(1));
            this.mockAzureClientFactory.Verify(m => m.AnalysisServiceClient, Times.Exactly(1));
            this.mockOffchainNodeRepository.Verify(m => m.InsertAll(It.Is<IEnumerable<OffchainNode>>(n => n.Count() == 1 && n.First().NodeId == 1 && n.First().NodeStateTypeId == (int)NodeState.OperativeBalanceCalculated)), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Ticket>(), Times.Once);
        }
    }
}
