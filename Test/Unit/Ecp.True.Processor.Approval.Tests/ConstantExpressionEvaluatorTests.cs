// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstantExpressionEvaluatorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Approval.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Approval;
    using Ecp.True.Processors.Approval.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The expression evaluator processor tests.
    /// </summary>
    [TestClass]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Test Class")]
    public class ConstantExpressionEvaluatorTests
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
        /// The unit of work mock.
        /// </summary>
        private Mock<IApprovalProcessor> approvalProcessorMock;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> configurationHandler;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<ConstantExpressionEvaluator>> logger;

        /// <summary>
        /// The rules repository.
        /// </summary>
        private Mock<IRepository<ApprovalRule>> rulesRepositoryMock;

        /// <summary>
        /// The unbalance repository mock.
        /// </summary>
        private Mock<IRepository<OwnershipNode>> ownershipNodeRepositoryMock;

        /// <summary>
        /// The logger.
        /// </summary>
        private ITrueExpressionEvaluator trueExpressionEvaluator;

        /// <summary>
        /// The expression Evaluator Processor.
        /// </summary>
        private ConstantExpressionEvaluator expressionEvaluatorProcessor;

        /// <summary>
        /// The ownership node balance summary aggregate mock.
        /// </summary>
        private Mock<IRepository<BalanceSummaryAggregate>> balanceSummaryAggregateMock;

        /// <summary>
        /// The mock service bus queue client.
        /// </summary>
        private Mock<IServiceBusQueueClient> mockServiceBusQueueClient;

        [TestInitialize]
        public void Initialize()
        {
            this.repositoryFactory = new Mock<IRepositoryFactory>();
            this.unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.configurationHandler = new Mock<IConfigurationHandler>();
            this.mockServiceBusQueueClient = new Mock<IServiceBusQueueClient>();
            this.unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.unitOfWorkMock = new Mock<IUnitOfWork>();
            this.approvalProcessorMock = new Mock<IApprovalProcessor>();
            this.logger = new Mock<ITrueLogger<ConstantExpressionEvaluator>>();
            this.trueExpressionEvaluator = new TrueExpressionEvaluator();
            this.balanceSummaryAggregateMock = new Mock<IRepository<BalanceSummaryAggregate>>();
            this.rulesRepositoryMock = new Mock<IRepository<ApprovalRule>>();
            this.ownershipNodeRepositoryMock = new Mock<IRepository<OwnershipNode>>();

            this.repositoryFactory.Setup(x => x.CreateRepository<OwnershipNode>()).Returns(this.ownershipNodeRepositoryMock.Object);
            this.repositoryFactory.Setup(x => x.CreateRepository<ApprovalRule>()).Returns(this.rulesRepositoryMock.Object);
            this.repositoryFactory.Setup(x => x.CreateRepository<BalanceSummaryAggregate>()).Returns(this.balanceSummaryAggregateMock.Object);

            this.unitOfWorkFactory.Setup(x => x.GetUnitOfWork()).Returns(this.unitOfWorkMock.Object);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<ApprovalRule>()).Returns(this.rulesRepositoryMock.Object);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<OwnershipNode>()).Returns(this.ownershipNodeRepositoryMock.Object);

            this.expressionEvaluatorProcessor = new ConstantExpressionEvaluator(
               this.repositoryFactory.Object,
               this.trueExpressionEvaluator,
               this.unitOfWorkFactory.Object,
               this.mockAzureClientFactory.Object,
               this.approvalProcessorMock.Object,
               this.logger.Object);
        }

        [TestMethod]
        public async Task EvaluateExpressionAsync_Return_False_Should_SendMessageToServiceBusQueueAsync()
        {
            var balanceSummaryAggregates = new List<BalanceSummaryAggregate>
            {
                new BalanceSummaryAggregate
                {
                    Volume = 0,
                    IdentifiedLosses = 5,
                    Inputs = 2,
                    OwnershipStatusId = OwnershipNodeStatusType.OWNERSHIP,
                },
            };

            var rules = new List<ApprovalRule>();
            rules.Add(new ApprovalRule { ApprovalRuleId = 1, Rule = "PI/E < 0.2" });
            this.rulesRepositoryMock.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<ApprovalRule, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<ApprovalRule> { rules.FirstOrDefault() });

            this.balanceSummaryAggregateMock.Setup(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(balanceSummaryAggregates);

            this.configurationHandler.Setup(a => a.GetConfigurationAsync<ServiceBusSettings>(It.IsAny<string>())).ReturnsAsync(new ServiceBusSettings());
            this.mockAzureClientFactory.Setup(a => a.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);
            this.mockServiceBusQueueClient.Setup(a => a.QueueMessageAsync(It.IsAny<object>()));

            await this.expressionEvaluatorProcessor.EvaluateAsync(1).ConfigureAwait(false);

            this.mockAzureClientFactory.Verify(a => a.GetQueueClient(It.IsAny<string>()), Times.Once);
            this.mockServiceBusQueueClient.Verify(a => a.QueueMessageAsync(It.IsAny<object>()), Times.Once);
        }

        [TestMethod]
        public async Task EvaluateExpressionAsync_Return_True_Should_UpdateOwnershipNodeStatusAsync()
        {
            var balanceSummaryAggregates = new List<BalanceSummaryAggregate>
            {
                new BalanceSummaryAggregate
                {
                    Volume = 0,
                    IdentifiedLosses = 5,
                    Inputs = 30,
                    OwnershipStatusId = OwnershipNodeStatusType.OWNERSHIP,
                },
            };

            OwnershipNode ownershipNode = new OwnershipNode()
            {
                OwnershipNodeId = 1,
                Comment = string.Empty,
                OwnershipStatus = OwnershipNodeStatusType.OWNERSHIP,
            };

            var rules = new List<ApprovalRule>();
            rules.Add(new ApprovalRule { ApprovalRuleId = 1, Rule = "PI/E < 0.2" });
            this.rulesRepositoryMock.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<ApprovalRule, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<ApprovalRule> { rules.FirstOrDefault() });

            this.balanceSummaryAggregateMock.Setup(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(balanceSummaryAggregates);
            this.unitOfWorkMock.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));
            this.ownershipNodeRepositoryMock.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>())).ReturnsAsync(ownershipNode);

            await this.expressionEvaluatorProcessor.EvaluateAsync(1).ConfigureAwait(false);

            Assert.AreEqual("Nodo aprobado automáticamente", ownershipNode.Comment);
            Assert.AreEqual(OwnershipNodeStatusType.APPROVED , ownershipNode.OwnershipStatus);
        }

        [TestMethod]
        public async Task EvaluateExpressionAsync_Return_False_TechnicalError_Should_SendMessageToServiceBusQueueAsync()
        {
            var balanceSummaryAggregates = new List<BalanceSummaryAggregate>
            {
                new BalanceSummaryAggregate
                {
                    Volume = 0,
                    IdentifiedLosses = 5,
                    Inputs = 2,
                    OwnershipStatusId = OwnershipNodeStatusType.OWNERSHIP,
                },
            };

            var rules = new List<ApprovalRule>();
            rules.Add(new ApprovalRule { ApprovalRuleId = 1, Rule = "/PI/E == 0.2" });
            this.rulesRepositoryMock.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<ApprovalRule, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<ApprovalRule> { rules.FirstOrDefault() });

            this.balanceSummaryAggregateMock.Setup(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(balanceSummaryAggregates);
            this.mockAzureClientFactory.Setup(a => a.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);
            this.mockServiceBusQueueClient.Setup(a => a.QueueMessageAsync(It.IsAny<object>()));

            await this.expressionEvaluatorProcessor.EvaluateAsync(1).ConfigureAwait(false);

            this.mockAzureClientFactory.Verify(a => a.GetQueueClient(It.IsAny<string>()), Times.Once);
            this.mockServiceBusQueueClient.Verify(a => a.QueueMessageAsync(It.IsAny<object>()), Times.Once);
        }

        [TestMethod]
        public async Task EvaluateExpressionAsync_Save_ShouldBeInvoked_WhenApprovalStatusIsApprovedAsync()
        {
            var balanceSummaryAggregates = new List<BalanceSummaryAggregate>
            {
                new BalanceSummaryAggregate
                {
                    Volume = 0,
                    IdentifiedLosses = 5,
                    Inputs = 30,
                    OwnershipStatusId = OwnershipNodeStatusType.OWNERSHIP,
                },
            };

            var ownershipNode = new OwnershipNode
            {
                OwnershipNodeId = 1,
                Comment = string.Empty,
                OwnershipStatus = OwnershipNodeStatusType.OWNERSHIP,
            };

            var rules = new List<ApprovalRule>();
            rules.Add(new ApprovalRule { ApprovalRuleId = 1, Rule = "PI/E < 0.2" });
            this.rulesRepositoryMock.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<ApprovalRule, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<ApprovalRule> { rules.FirstOrDefault() });

            this.balanceSummaryAggregateMock.Setup(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(balanceSummaryAggregates);
            this.unitOfWorkMock.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));
            this.approvalProcessorMock.Setup(x => x.SaveOperativeMovementsAsync(It.IsAny<int>())).Returns(Task.FromResult(1));
            this.ownershipNodeRepositoryMock.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>())).ReturnsAsync(ownershipNode);

            await this.expressionEvaluatorProcessor.EvaluateAsync(1).ConfigureAwait(false);

            this.approvalProcessorMock.Verify(a => a.SaveOperativeMovementsAsync(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public async Task EvaluateExpressionAsync_Save_ShouldNotBeInvoked_WhenApprovalStatusIsRejectedAsync()
        {
            var balanceSummaryAggregates = new List<BalanceSummaryAggregate>
            {
                new BalanceSummaryAggregate
                {
                    Volume = 1,
                    IdentifiedLosses = 5,
                    Inputs = 2,
                    OwnershipStatusId = OwnershipNodeStatusType.OWNERSHIP,
                },
            };

            var rules = new List<ApprovalRule>();
            rules.Add(new ApprovalRule { ApprovalRuleId = 1, Rule = "/PI/E == 0.2" });
            this.rulesRepositoryMock.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<ApprovalRule, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<ApprovalRule> { rules.FirstOrDefault() });

            this.balanceSummaryAggregateMock.Setup(m => m.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(balanceSummaryAggregates);

            this.configurationHandler.Setup(a => a.GetConfigurationAsync<ServiceBusSettings>(It.IsAny<string>())).ReturnsAsync(new ServiceBusSettings());
            this.mockAzureClientFactory.Setup(a => a.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);
            this.mockServiceBusQueueClient.Setup(a => a.QueueMessageAsync(It.IsAny<object>()));

            await this.expressionEvaluatorProcessor.EvaluateAsync(1).ConfigureAwait(false);

            this.mockAzureClientFactory.Verify(a => a.GetQueueClient(It.IsAny<string>()), Times.Once);
            this.mockServiceBusQueueClient.Verify(a => a.QueueMessageAsync(It.IsAny<object>()), Times.Once);
            this.approvalProcessorMock.Verify(a => a.SaveOperativeMovementsAsync(It.IsAny<int>()), Times.Never);
        }
    }
}
