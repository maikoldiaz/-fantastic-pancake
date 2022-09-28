// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConciliationFinalizerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Conciliation.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Conciliation;
    using Ecp.True.Processors.Conciliation.Entities;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Conciliation finalizer tests.
    /// </summary>
    [TestClass]
    public class ConciliationFinalizerTests
    {
        /// <summary>
        /// The factory.
        /// </summary>
        private readonly Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();

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
        /// The mock analysis service client.
        /// </summary>
        private readonly Mock<IAnalysisServiceClient> mockAnalysisServiceClient = new Mock<IAnalysisServiceClient>();

        /// <summary>
        /// The mock ticket repository.
        /// </summary>
        private readonly Mock<IRepository<Ticket>> mockTicketRepository = new Mock<IRepository<Ticket>>();

        /// <summary>
        /// The conciliation finalizer.
        /// </summary>
        private ConciliationFinalizer conciliationFinalizer;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockUnitOfWork.Setup(m => m.CreateRepository<OffchainNode>()).Returns(this.mockOffchainNodeRepository.Object);
            this.mockOffchainNodeRepository.Setup(m => m.InsertAll(It.IsAny<IEnumerable<OffchainNode>>()));

            this.mockQueueClient.Setup(m => m.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()));
            this.mockAzureClientFactory.Setup(m => m.GetQueueClient(It.IsAny<string>())).Returns(this.mockQueueClient.Object);

            this.mockOwnershipNodeRepository.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<OwnershipNode> { new OwnershipNode { TicketId = 1, Node = new Node { NodeId = 1 } } });
            this.mockUnitOfWork.Setup(m => m.CreateRepository<OwnershipNode>()).Returns(this.mockOwnershipNodeRepository.Object);

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockMovementRepository.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Movement> { new Movement { MovementTransactionId = 1 } });
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);

            this.mockTicketRepository.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Ticket { Status = StatusType.PROCESSED });
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);

            this.mockAnalysisServiceClient.Setup(m => m.RefreshOwnershipAsync(1));
            this.mockAzureClientFactory.Setup(m => m.AnalysisServiceClient).Returns(this.mockAnalysisServiceClient.Object);

            this.mockOwnershipRepository.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Ownership> { new Ownership { TicketId = 1 } });
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Ownership>()).Returns(this.mockOwnershipRepository.Object);

            this.conciliationFinalizer = new ConciliationFinalizer(this.mockUnitOfWorkFactory.Object, this.mockAzureClientFactory.Object);
        }

        /// <summary>
        /// Types the should return ticket type ownership when invoked.
        /// </summary>
        [TestMethod]
        public void Type_ShouldReturnTicketTypeOwnership_WhenInvoked()
        {
            var result = this.conciliationFinalizer.Type;

            Assert.AreEqual(TicketType.Ownership, result);
        }

        [TestMethod]
        public async Task ProcessAsync_ShouldSendMessagesToQueues_WhenInvokedAsync()
        {
            var parameters = new Dictionary<string, object>
            {
                { "@OwnershipTicketId", 1000 },
                { "@NodeId", 10 },
            };

            // Act
            var conciliationNodes = new ConciliationNodesResquest { TicketId = 1000, NodeId = 10 };
            await this.conciliationFinalizer.ProcessAsync(new ConciliationRuleData { ConciliationNodes = conciliationNodes }).ConfigureAwait(false);

            // Assert
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveAttributeDetailsWithOwner, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveBackupMovementDetailsWithOwner, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveMovementDetailsWithOwner, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveInventoryDetailsWithOwner, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveKPIDataByCategoryElementNodeWithOwnership, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveMovementsByProductWithOwner, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveQualityDetailsWithOwner, parameters), Times.Once);

            this.mockQueueClient.Verify(m => m.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Exactly(2));
            this.mockAzureClientFactory.Verify(m => m.GetQueueClient(It.IsAny<string>()), Times.Exactly(2));
            this.mockMovementRepository.Verify(m => m.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.GetAllAsync(It.IsAny<Expression<Func<Ownership, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Movement>(), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Ownership>(), Times.Exactly(2));
            this.mockAnalysisServiceClient.Verify(m => m.RefreshOwnershipAsync(It.IsAny<int>()), Times.Once);
            this.mockAzureClientFactory.Verify(m => m.AnalysisServiceClient, Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Ticket>(), Times.Once);
        }

        [TestMethod]
        public async Task ProcessAsync_ShouldNotSendMessagesToQueues_TicketStatusDifferentToProcessed_WhenInvokedAsync()
        {
            this.mockTicketRepository.Setup(m => m.GetByIdAsync(1001)).ReturnsAsync(new Ticket { Status = StatusType.FAILED });
            var parameters = new Dictionary<string, object>
            {
                { "@OwnershipTicketId", 1001 },
                { "@NodeId", 10 },
            };

            // Act
            var conciliationNodes = new ConciliationNodesResquest { TicketId = 1001, NodeId = 10 };
            await this.conciliationFinalizer.ProcessAsync(new ConciliationRuleData { ConciliationNodes = conciliationNodes }).ConfigureAwait(false);

            // Assert
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.DeleteMovementInformationForReport, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveAttributeDetailsWithOwner, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveBackupMovementDetailsWithOwner, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveMovementDetailsWithOwner, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveInventoryDetailsWithOwner, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveKPIDataByCategoryElementNodeWithOwnership, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveMovementsByProductWithOwner, parameters), Times.Once);
            this.mockOwnershipRepository.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveQualityDetailsWithOwner, parameters), Times.Once);

            this.mockAnalysisServiceClient.Verify(m => m.RefreshOwnershipAsync(It.IsAny<int>()), Times.Once);
            this.mockAzureClientFactory.Verify(m => m.AnalysisServiceClient, Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Ticket>(), Times.Once);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Ownership>(), Times.Once);

            this.mockQueueClient.Verify(m => m.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            this.mockAzureClientFactory.Verify(m => m.GetQueueClient(It.IsAny<string>()), Times.Never);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Movement>(), Times.Never);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<OwnershipNode>(), Times.Never);
            this.mockOffchainNodeRepository.Verify(m => m.InsertAll(It.Is<IEnumerable<OffchainNode>>(x => x.Count() == 1 && x.First().NodeId == 1 && x.First().NodeStateTypeId == (int)NodeState.OperativeBalanceCalculatedWithOwnership)), Times.Never);
        }
    }
}
