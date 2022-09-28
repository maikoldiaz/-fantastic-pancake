// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApprovalProcessorTests.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Approval;
    using Ecp.True.Processors.Approval.Interfaces;
    using Ecp.True.Proxies.Azure;
    using EfCore.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The approval processor tests.
    /// </summary>
    [TestClass]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Test Class")]
    public class ApprovalProcessorTests
    {
        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The repository factory.
        /// </summary>
        private Mock<IRepositoryFactory> repositoryFactory;

        /// <summary>
        /// The repository factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;

        /// <summary>
        /// The unit of work mock.
        /// </summary>
        private Mock<IUnitOfWork> unitOfWorkMock;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> configurationHandler;

        /// <summary>
        /// The unbalance repository mock.
        /// </summary>
        private Mock<IRepository<OwnershipNode>> ownershipNodeRepositoryMock;

        /// <summary>
        /// The user repository mock.
        /// </summary>
        private Mock<IRepository<User>> userRepositoryMock;

        /// <summary>
        /// The node tag repository mock.
        /// </summary>
        private Mock<IRepository<NodeTag>> nodeTagRepositoryMock;

        /// <summary>
        /// The balance calculator.
        /// </summary>
        private ApprovalProcessor approvalProcessor;

        /// <summary>
        /// The ticket repository.
        /// </summary>
        private Mock<IRepository<Ticket>> ticketRepositoryMock;

        /// <summary>
        /// The node repository.
        /// </summary>
        private Mock<IRepository<Node>> nodeRepositoryMock;

        /// <summary>
        /// The ownership node balance summary repository mock.
        /// </summary>
        private Mock<IRepository<OwnershipNodeBalanceSummary>> ownershipNodeBalanceSummaryRepositoryMock;

        /// <summary>
        /// The ownership node balance summary aggregate mock.
        /// </summary>
        private Mock<IRepository<BalanceSummaryAggregate>> balanceSummaryAggregateMock;

        /// <summary>
        /// The mock service bus queue client.
        /// </summary>
        private Mock<IServiceBusQueueClient> mockServiceBusQueueClient;

        /// <summary>
        /// The mock business context.
        /// </summary>
        private Mock<IBusinessContext> mockBusinessContext;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<IApprovalProcessor> mockProcessor;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<ApprovalProcessor>> logger;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.repositoryFactory = new Mock<IRepositoryFactory>();
            this.unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.configurationHandler = new Mock<IConfigurationHandler>();
            this.mockServiceBusQueueClient = new Mock<IServiceBusQueueClient>();
            this.mockBusinessContext = new Mock<IBusinessContext>();
            this.mockProcessor = new Mock<IApprovalProcessor>();
            this.logger = new Mock<ITrueLogger<ApprovalProcessor>>();

            this.ownershipNodeRepositoryMock = new Mock<IRepository<OwnershipNode>>();
            this.userRepositoryMock = new Mock<IRepository<User>>();
            this.repositoryFactory.Setup(x => x.CreateRepository<OwnershipNode>()).Returns(this.ownershipNodeRepositoryMock.Object);

            this.nodeTagRepositoryMock = new Mock<IRepository<NodeTag>>();
            this.repositoryFactory.Setup(x => x.CreateRepository<NodeTag>()).Returns(this.nodeTagRepositoryMock.Object);

            this.ticketRepositoryMock = new Mock<IRepository<Ticket>>();
            this.nodeRepositoryMock = new Mock<IRepository<Node>>();
            this.ownershipNodeBalanceSummaryRepositoryMock = new Mock<IRepository<OwnershipNodeBalanceSummary>>();
            this.balanceSummaryAggregateMock = new Mock<IRepository<BalanceSummaryAggregate>>();
            this.unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.unitOfWorkMock = new Mock<IUnitOfWork>();

            this.repositoryFactory.Setup(x => x.CreateRepository<Ticket>()).Returns(this.ticketRepositoryMock.Object);
            this.repositoryFactory.Setup(x => x.CreateRepository<OwnershipNode>()).Returns(this.ownershipNodeRepositoryMock.Object);
            this.repositoryFactory.Setup(x => x.CreateRepository<User>()).Returns(this.userRepositoryMock.Object);
            this.repositoryFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepositoryMock.Object);
            this.repositoryFactory.Setup(x => x.CreateRepository<OwnershipNodeBalanceSummary>()).Returns(this.ownershipNodeBalanceSummaryRepositoryMock.Object);
            this.repositoryFactory.Setup(x => x.CreateRepository<BalanceSummaryAggregate>()).Returns(this.balanceSummaryAggregateMock.Object);

            this.unitOfWorkFactory.Setup(x => x.GetUnitOfWork()).Returns(this.unitOfWorkMock.Object);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<OwnershipNode>()).Returns(this.ownershipNodeRepositoryMock.Object);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<Ticket>()).Returns(this.ticketRepositoryMock.Object);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepositoryMock.Object);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<OwnershipNodeBalanceSummary>()).Returns(this.ownershipNodeBalanceSummaryRepositoryMock.Object);
            this.unitOfWorkMock.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            this.approvalProcessor = new ApprovalProcessor(
                this.repositoryFactory.Object,
                this.unitOfWorkFactory.Object,
                this.mockAzureClientFactory.Object,
                this.configurationHandler.Object,
                this.mockBusinessContext.Object,
                this.logger.Object);
        }

        /// <summary>
        /// Updates the ownership node status asynchronous should invoke with success asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateOwnershipNodeStatusAsync_ShouldInvoke_WithSuccessAsync()
        {
            NodeOwnershipApprovalRequest approvalRequest = new NodeOwnershipApprovalRequest()
            {
                OwnershipNodeId = 2,
                ApproverAlias = "shmadhav",
                Comment = string.Empty,
                Status = "APPROVED",
            };
            OwnershipNode ownershipNode = new OwnershipNode()
            {
                OwnershipNodeId = 2,
                ApproverAlias = "shmadhav",
                Comment = string.Empty,
                OwnershipStatus = Entities.Enumeration.OwnershipNodeStatusType.SUBMITFORAPPROVAL,
            };
            this.ownershipNodeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ownershipNode);
            this.unitOfWorkMock.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            var output = await this.approvalProcessor.UpdateOwnershipNodeStatusAsync(approvalRequest).ConfigureAwait(false);
            Assert.AreNotEqual(null,output);
            this.unitOfWorkMock.Verify(u => u.SaveAsync(CancellationToken.None), Times.Once);
        }

        /// <summary>
        /// Updates the ownership node status asynchronous should invoke with errors asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateOwnershipNodeStatusAsync_ShouldInvoke_WithErrorsAsync()
        {
            NodeOwnershipApprovalRequest approvalRequest = new NodeOwnershipApprovalRequest()
            {
                OwnershipNodeId = 2,
                ApproverAlias = string.Empty,
                Comment = string.Empty,
                Status = "REJECTED",
            };
            OwnershipNode ownershipNode = new OwnershipNode()
            {
                OwnershipNodeId = 2,
                ApproverAlias = string.Empty,
                Comment = string.Empty,
                OwnershipStatus = Entities.Enumeration.OwnershipNodeStatusType.SUBMITFORAPPROVAL,
            };
            this.ownershipNodeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ownershipNode);
            this.unitOfWorkMock.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            var output = await this.approvalProcessor.UpdateOwnershipNodeStatusAsync(approvalRequest).ConfigureAwait(false);
            Assert.AreNotEqual(null, output[0].Message);
        }

        /// <summary>
        /// Updates the ownership node status asynchronous should return ownership not found when ownership not found.
        /// </summary>
        /// <returns> The Task.</returns>
        [TestMethod]
        public async Task UpdateOwnershipNodeStatusAsync_ShouldReturnOwnershipNotFound_WhenOwnershipNotFoundAsync()
        {
            NodeOwnershipApprovalRequest approvalRequest = new NodeOwnershipApprovalRequest()
            {
                OwnershipNodeId = 2,
                ApproverAlias = "abgaon",
                Comment = "Unittest",
                Status = "REJECTED",
            };

            this.ownershipNodeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((OwnershipNode)null);
            this.unitOfWorkMock.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            var output = await this.approvalProcessor.UpdateOwnershipNodeStatusAsync(approvalRequest).ConfigureAwait(false);
            Assert.AreEqual(Ecp.True.Entities.Constants.OwnershipNodeNotFound, output[0].Message);
        }

        /// <summary>
        /// Updates the ownership node status asynchronous should return invalid node state when node state not submitted for approval.
        /// </summary>
        /// <returns> The Task.</returns>
        [TestMethod]
        public async Task UpdateOwnershipNodeStatusAsync_ShouldReturnInvalidNodeState_WhenNodeStateNotSubmittedForApprovalAsync()
        {
            NodeOwnershipApprovalRequest approvalRequest = new NodeOwnershipApprovalRequest()
            {
                OwnershipNodeId = 2,
                ApproverAlias = "abgaon",
                Comment = "Unittest",
                Status = "REJECTED",
            };
            OwnershipNode ownershipNode = new OwnershipNode()
            {
                OwnershipNodeId = 2,
                ApproverAlias = string.Empty,
                Comment = string.Empty,
                OwnershipStatus = Entities.Enumeration.OwnershipNodeStatusType.REJECTED,
            };

            this.ownershipNodeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ownershipNode);
            this.unitOfWorkMock.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            var output = await this.approvalProcessor.UpdateOwnershipNodeStatusAsync(approvalRequest).ConfigureAwait(false);
            Assert.AreEqual(Ecp.True.Entities.Constants.InvalidNodeStateApproval, output[0].Message);
        }

        /// <summary>
        /// Gets the ownership node balance details asynchronous should invoke repos when invoked asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetOwnershipNodeBalanceDetailsAsync_ShouldInvokeRepos_WhenInvokedAsync()
        {
            // Arrange
            var settings = new SystemSettings { BasePath = "https://dev-true.ecopetrol.com.co" };
            this.configurationHandler.Setup(a => a.GetConfigurationAsync<SystemSettings>(It.IsAny<string>())).ReturnsAsync(settings);
            this.userRepositoryMock.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(new User { Email = "TestEmail" });
            this.ownershipNodeRepositoryMock.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>(), "Ticket", "Node")).ReturnsAsync(new OwnershipNode { Editor = "TestUser", Node = new Node { Name = "TestNode" }, Ticket = new Ticket { StartDate = DateTime.Parse("1/1/2020", CultureInfo.InvariantCulture) } });
            var nodeTag = new NodeTag
            {
                CategoryElement = new CategoryElement { Name = "Unit Test Category" },
            };
            this.nodeTagRepositoryMock.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<NodeTag, bool>>>(), "CategoryElement")).ReturnsAsync(nodeTag);
            this.ownershipNodeBalanceSummaryRepositoryMock.Setup(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<OwnershipNodeBalanceSummary> { new OwnershipNodeBalanceSummary() });

            // Act
            var details = await this.approvalProcessor.GetOwnershipNodeBalanceDetailsAsync(1).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(details);
            Assert.AreEqual($"{settings.BasePath}/cutoffreport/manage/1", details.ReportPath);
            this.ownershipNodeRepositoryMock.Verify(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>(), "Ticket", "Node"), Times.Once);
            this.ownershipNodeBalanceSummaryRepositoryMock.Verify(x => x.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Gets the ownership node balance details asynchronous should throw key not found exception when node not found asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task GetOwnershipNodeBalanceDetailsAsync_ShouldThrowKeyNotFoundException_WhenNodeNotFoundAsync()
        {
            // Arrange
            var nodeTag = new NodeTag
            {
                CategoryElement = new CategoryElement { Name = "Unit Test Category" },
            };
            this.nodeTagRepositoryMock.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<NodeTag, bool>>>(), "CategoryElement")).ReturnsAsync(nodeTag);

            // Act
            await this.approvalProcessor.GetOwnershipNodeBalanceDetailsAsync(01).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the ownership node status asynchronous should invoke with errors asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task SendOwnershipNodeIdForApprovalAsync_Should_SendMessageToServiceBusApprovalQueueAsync()
        {
            var balanceSummaryAggregates = new List<BalanceSummaryAggregate>
            {
                new BalanceSummaryAggregate
                {
                    Volume = 0,
                    OwnershipStatusId = OwnershipNodeStatusType.PUBLISHED,
                },
            };

            OwnershipNode ownershipNode = new OwnershipNode()
            {
                OwnershipNodeId = 1,
                Comment = string.Empty,
                OwnershipStatus = OwnershipNodeStatusType.OWNERSHIP,
            };

            this.balanceSummaryAggregateMock.Setup(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(balanceSummaryAggregates);
            this.ownershipNodeRepositoryMock.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>())).ReturnsAsync(ownershipNode);

            this.configurationHandler.Setup(a => a.GetConfigurationAsync<ServiceBusSettings>(It.IsAny<string>())).ReturnsAsync(new ServiceBusSettings());
            this.mockAzureClientFactory.Setup(a => a.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);
            this.mockServiceBusQueueClient.Setup(a => a.QueueMessageAsync(It.IsAny<object>()));

            await this.approvalProcessor.SendOwnershipNodeIdForApprovalAsync(1).ConfigureAwait(false);

            this.mockAzureClientFactory.Verify(a => a.GetQueueClient(It.IsAny<string>()), Times.Once);
            this.mockServiceBusQueueClient.Verify(a => a.QueueMessageAsync(It.IsAny<object>()), Times.Once);

            Assert.AreEqual(OwnershipNodeStatusType.SUBMITFORAPPROVAL, ownershipNode.OwnershipStatus);
        }

        /// <summary>
        /// Updates the ownership node status asynchronous should return ownership not found when ownership not found.
        /// </summary>
        /// <returns> The Task.</returns>
        [TestMethod]
        public async Task Save_ShouldNotBeInvoked_WhenApprovalStatusIsRejectedAsync()
        {
            var approvalRequest = new NodeOwnershipApprovalRequest
            {
                OwnershipNodeId = 3,
                ApproverAlias = "approveralias",
                Comment = string.Empty,
                Status = OwnershipNodeStatusType.REJECTED.ToString(),
            };
            var ownershipNode = new OwnershipNode
            {
                OwnershipNodeId = 2,
                ApproverAlias = string.Empty,
                Comment = string.Empty,
                OwnershipStatus = Entities.Enumeration.OwnershipNodeStatusType.SUBMITFORAPPROVAL,
            };

            var parameters = new Dictionary<string, object>
                {
                    { "@OwnershipNodeId", ownershipNode.OwnershipNodeId },
                };
            var repoMock = new Mock<IRepository<OwnershipNode>>();
            repoMock.Setup(r => r.Update(It.IsAny<OwnershipNode>()));
            repoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ownershipNode);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<OwnershipNode>()).Returns(repoMock.Object);
            this.unitOfWorkMock.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));
            this.mockProcessor.Setup(x => x.UpdateOwnershipNodeStateAsync(approvalRequest));
            repoMock.Setup(m => m.ExecuteAsync(Repositories.Constants.SaveOperativeMovements, parameters)).Verifiable();

            await this.approvalProcessor.UpdateOwnershipNodeStateAsync(approvalRequest).ConfigureAwait(false);
            this.unitOfWorkMock.Verify(x => x.CreateRepository<OwnershipNode>(), Times.Once);
            this.unitOfWorkMock.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
            repoMock.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveOperativeMovements, parameters), Times.Never);
        }

        /// <summary>
        /// Updates the ownership node status asynchronous should return ownership not found when ownership not found.
        /// </summary>
        /// <returns> The Task.</returns>
        [TestMethod]
        public async Task Save_ShouldBeInvoked_WhenApprovalStatusIsApprovedAsync()
        {
            var approvalRequest = new NodeOwnershipApprovalRequest
            {
                OwnershipNodeId = 2,
                ApproverAlias = "approveralias",
                Comment = string.Empty,
                Status = OwnershipNodeStatusType.APPROVED.ToString(),
            };
            var ownershipNode = new OwnershipNode
            {
                OwnershipNodeId = 2,
                ApproverAlias = string.Empty,
                Comment = string.Empty,
                OwnershipStatus = Entities.Enumeration.OwnershipNodeStatusType.SUBMITFORAPPROVAL,
            };

            var parameters = new Dictionary<string, object>
                {
                    { "@OwnershipNodeId", ownershipNode.OwnershipNodeId },
                };
            var repoMock = new Mock<IRepository<OwnershipNode>>();
            repoMock.Setup(r => r.Update(It.IsAny<OwnershipNode>()));
            repoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ownershipNode);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<OwnershipNode>()).Returns(repoMock.Object);
            this.unitOfWorkMock.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));
            this.mockProcessor.Setup(x => x.UpdateOwnershipNodeStateAsync(approvalRequest));

            await this.approvalProcessor.UpdateOwnershipNodeStateAsync(approvalRequest).ConfigureAwait(false);
            this.unitOfWorkMock.Verify(x => x.CreateRepository<OwnershipNode>(), Times.Exactly(2));
            this.unitOfWorkMock.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
            repoMock.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveOperativeMovements, parameters), Times.Once);
        }

        [TestMethod]
        public async Task Save_ShouldLogError_WhenExceptionIsRaisedAsync()
        {
            // Arrange
            this.logger.Setup(m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>()));
            this.unitOfWorkMock.Setup(m => m.CreateRepository<OwnershipNode>()).Throws(new NullReferenceException());

            // Act
            await this.approvalProcessor.SaveOperativeMovementsAsync(1).ConfigureAwait(false);

            // Assert
            this.logger.Verify(
                m => m.LogError(It.IsAny<Exception>(), Constants.AnalyticalProcessInvokeFail),
                Times.Once);
        }
    }
}