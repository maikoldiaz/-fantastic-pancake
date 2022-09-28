// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipFinalizerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Ownership.Tests
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
    using Ecp.True.Processors.Ownership;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Processors.Ownership.Retry;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class OwnershipFinalizerTests
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
        private readonly Mock<IRepository<Ownership>> mockOwnershipRepository = new Mock<IRepository<Ownership>>();

        /// <summary>
        /// The mock ownership node repository.
        /// </summary>
        private readonly Mock<IRepository<OwnershipNode>> mockOwnershipNodeRepository = new Mock<IRepository<OwnershipNode>>();

        /// <summary>
        /// The client.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The mock offchain node repository.
        /// </summary>
        private readonly Mock<IRepository<OffchainNode>> mockOffchainNodeRepository = new Mock<IRepository<OffchainNode>>();

        /// <summary>
        /// The mock queue client.
        /// </summary>
        private readonly Mock<IServiceBusQueueClient> mockQueueClient = new Mock<IServiceBusQueueClient>();

        /// <summary>
        /// The mock configuration handler.
        /// </summary>
        private readonly Mock<IConfigurationHandler> mockConfigurationHandler = new Mock<IConfigurationHandler>();

        /// <summary>
        /// The mock analysis service client.
        /// </summary>
        private readonly Mock<IAnalysisServiceClient> mockAnalysisServiceClient = new Mock<IAnalysisServiceClient>();

        /// <summary>
        /// The mock ticket repository.
        /// </summary>
        private readonly Mock<IRepository<Ticket>> mockTicketRepository = new Mock<IRepository<Ticket>>();

        /// <summary>
        /// The ownership finalizer.
        /// </summary>
        private OwnershipFinalizer ownershipFinalizer;

        /// <summary>
        /// The retry policy factory.
        /// </summary>
        private IRetryPolicyFactory retryPolicyFactory;

        /// <summary>
        /// The retry handler.
        /// </summary>
        private IOwnershipBalanceRetryHandler retryHandler;

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
            this.settings.MaxOwnershipRetryCount = 3;
            this.settings.OwnerShipRetryIntervalInSeconds = 10;
            this.settings.OwnerShipRetryStrategy = 2;

            this.mockConfigurationHandler.Setup(m => m.GetConfigurationAsync<ServiceBusSettings>(It.IsAny<string>())).ReturnsAsync(new ServiceBusSettings { ConnectionString = "UTString" });
            this.mockQueueClient.Setup(m => m.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()));
            this.mockAzureClientFactory.Setup(m => m.GetQueueClient(It.IsAny<string>())).Returns(this.mockQueueClient.Object);

            this.mockAnalysisServiceClient.Setup(m => m.RefreshOwnershipAsync(1));
            this.mockAzureClientFactory.Setup(m => m.AnalysisServiceClient).Returns(this.mockAnalysisServiceClient.Object);
            this.retryPolicyFactory = new RetryPolicyFactory();
            this.configMock = new Mock<IConfigurationHandler>();
            this.configMock.Setup(c => c.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings)).ReturnsAsync(this.settings);
            var retryHandlerLoggerMock = new Mock<ITrueLogger<OwnershipBalanceRetryHandler>>();
            var mockResolver = new Mock<IResolver>();
            mockResolver.Setup(x => x.GetInstance<ITrueLogger<OwnershipBalanceRetryHandler>>()).Returns(retryHandlerLoggerMock.Object);
            this.retryHandler = new OwnershipBalanceRetryHandler(mockResolver.Object);
            this.mockFactory.Setup(m => m.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockMovementRepository.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Movement> { new Movement { MovementTransactionId = 1 } });
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);

            this.mockTicketRepository.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Ticket { Status = StatusType.PROCESSED });
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);

            this.mockOwnershipRepository.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Ownership> { new Ownership { TicketId = 1 } });
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Ownership>()).Returns(this.mockOwnershipRepository.Object);

            this.mockOwnershipNodeRepository.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<OwnershipNode> { new OwnershipNode { TicketId = 1, Node = new Node { NodeId = 1 } } });
            this.mockUnitOfWork.Setup(m => m.CreateRepository<OwnershipNode>()).Returns(this.mockOwnershipNodeRepository.Object);

            this.mockUnitOfWork.Setup(m => m.CreateRepository<OffchainNode>()).Returns(this.mockOffchainNodeRepository.Object);
            this.mockOffchainNodeRepository.Setup(m => m.InsertAll(It.IsAny<IEnumerable<OffchainNode>>()));

            this.ownershipFinalizer = new OwnershipFinalizer(this.mockFactory.Object, this.mockAzureClientFactory.Object, this.retryPolicyFactory, this.retryHandler, this.configMock.Object);
        }

        /// <summary>
        /// Types the should return ticket type ownership when invoked.
        /// </summary>
        [TestMethod]
        public void Type_ShouldReturnTicketTypeOwnership_WhenInvoked()
        {
            var result = this.ownershipFinalizer.Type;

            Assert.AreEqual(TicketType.Ownership, result);
        }

        [TestMethod]
        public async Task ProcessAsync_ShouldSendMessagesToQueues_WhenInvokedAsync()
        {
            var parameters = new Dictionary<string, object>
            {
                { "@OwnershipTicketId", 1 },
                { "@NodeId", null },
            };

            // Act
            await this.ownershipFinalizer.ProcessAsync(new OwnershipRuleData { TicketId = 1 }).ConfigureAwait(false);

            // Assert
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveAttributeDetailsWithOwner, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveBackupMovementDetailsWithOwner, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveMovementDetailsWithOwner, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveInventoryDetailsWithOwner, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveKPIDataByCategoryElementNodeWithOwnership, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveMovementsByProductWithOwner, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveQualityDetailsWithOwner, parameters), Times.Once);

            this.mockQueueClient.Verify(m => m.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Exactly(3));
            this.mockAzureClientFactory.Verify(m => m.GetQueueClient(It.IsAny<string>()), Times.Exactly(3));
            this.mockMovementRepository.Verify(m => m.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.GetAllAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockOwnershipNodeRepository.Verify(m => m.GetAllAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Movement>(), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Ownership>(), Times.Exactly(3));
            this.mockUnitOfWork.Verify(m => m.CreateRepository<OwnershipNode>(), Times.Once);
            this.mockAnalysisServiceClient.Verify(m => m.RefreshOwnershipAsync(1), Times.Exactly(1));
            this.mockAzureClientFactory.Verify(m => m.AnalysisServiceClient, Times.Exactly(1));
            this.mockOffchainNodeRepository.Verify(m => m.InsertAll(It.Is<IEnumerable<OffchainNode>>(x => x.Count() == 1 && x.First().NodeId == 1 && x.First().NodeStateTypeId == (int)NodeState.OperativeBalanceCalculatedWithOwnership)), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Ticket>(), Times.Once);
        }

        [TestMethod]
        public async Task ProcessAsync_ShouldCallDeleteMovementInfoForReportStoredProcedure_WhenInvokedAsync()
        {
            var parameters = new Dictionary<string, object>
            {
                { "@OwnershipTicketId", 1 },
                { "@NodeId", null },
            };

            // Act
            await this.ownershipFinalizer.ProcessAsync(new OwnershipRuleData { TicketId = 1, HasDeletedMovementOwnerships = true, }).ConfigureAwait(false);

            // Assert
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.DeleteMovementInformationForReport, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveAttributeDetailsWithOwner, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveBackupMovementDetailsWithOwner, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveMovementDetailsWithOwner, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveInventoryDetailsWithOwner, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveKPIDataByCategoryElementNodeWithOwnership, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveMovementsByProductWithOwner, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveQualityDetailsWithOwner, parameters), Times.Once);

            this.mockQueueClient.Verify(m => m.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Exactly(3));
            this.mockAzureClientFactory.Verify(m => m.GetQueueClient(It.IsAny<string>()), Times.Exactly(3));
            this.mockMovementRepository.Verify(m => m.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.GetAllAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockOwnershipNodeRepository.Verify(m => m.GetAllAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Movement>(), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Ownership>(), Times.Exactly(3));
            this.mockUnitOfWork.Verify(m => m.CreateRepository<OwnershipNode>(), Times.Once);
            this.mockAnalysisServiceClient.Verify(m => m.RefreshOwnershipAsync(1), Times.Exactly(1));
            this.mockAzureClientFactory.Verify(m => m.AnalysisServiceClient, Times.Exactly(1));
            this.mockOffchainNodeRepository.Verify(m => m.InsertAll(It.Is<IEnumerable<OffchainNode>>(x => x.Count() == 1 && x.First().NodeId == 1 && x.First().NodeStateTypeId == (int)NodeState.OperativeBalanceCalculatedWithOwnership)), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Ticket>(), Times.Once);
        }
    }
}
