// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipRuleProcessorTests.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Ownership;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.OwnershipRules.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class OwnershipRuleProcessorTests
    {
        /// <summary>
        /// The mock finalizer.
        /// </summary>
        private readonly Mock<IFinalizer> mockOwnershipFinalizer = new Mock<IFinalizer>();

        /// <summary>
        /// The mock finalizer factory.
        /// </summary>
        private readonly Mock<IFinalizerFactory> mockFinalizerFactory = new Mock<IFinalizerFactory>();

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<OwnershipRuleProcessor>> mockLogger;

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureClientFactory;

        /// <summary>
        /// The mock inventory product repository.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigurationHandler;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IExecutionManagerFactory> mockExecutionManagerFactory;

        /// <summary>
        /// The mock ownership rule service.
        /// </summary>
        private Mock<IOwnershipRuleProxy> mockOwnershipRuleService;

        /// <summary>
        /// The mock execution chain builder.
        /// </summary>
        private Mock<IExecutionChainBuilder> mockExecutionChainBuilder;

        /// <summary>
        /// The mock ownership rule refresh history repository.
        /// </summary>
        private Mock<IRepository<OwnershipRuleRefreshHistory>> mockOwnershipRuleRefreshHistoryRepository;

        /// <summary>
        /// The mock node connection product rule repository.
        /// </summary>
        private Mock<IRepository<NodeConnectionProductRule>> mockNodeConnectionProductRuleRepository;

        /// <summary>
        /// The mock node ownership rule repository.
        /// </summary>
        private Mock<IRepository<NodeOwnershipRule>> mockNodeOwnershipRuleRepository;

        /// <summary>
        /// The mock node product rule repository.
        /// </summary>
        private Mock<IRepository<NodeProductRule>> mockNodeProductRuleRepository;

        /// <summary>
        /// The ownership rule processor.
        /// </summary>
        private OwnershipRuleProcessor ownershipRuleProcessor;

        /// <summary>
        /// The mock execution manager.
        /// </summary>
        private Mock<IExecutionManager> mockExecutionManager;

        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>
        /// The mock ownership repository.
        /// </summary>
        private Mock<IRepository<Ownership>> mockOwnershipRepository;

        /// <summary>
        /// The mock ownership node repository.
        /// </summary>
        private Mock<IRepository<OwnershipNode>> mockOwnershipNodeRepository;

        /// <summary>
        /// Intilizes the method.
        /// </summary>
        [TestInitialize]
        public void IntializeMethod()
        {
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockConfigurationHandler = new Mock<IConfigurationHandler>();
            this.mockAzureClientFactory = new Mock<IAzureClientFactory>();
            this.mockExecutionChainBuilder = new Mock<IExecutionChainBuilder>();
            this.mockOwnershipRuleService = new Mock<IOwnershipRuleProxy>();
            this.mockLogger = new Mock<ITrueLogger<OwnershipRuleProcessor>>();
            this.mockExecutionManager = new Mock<IExecutionManager>();
            this.mockExecutionManagerFactory = new Mock<IExecutionManagerFactory>();

            this.mockOwnershipRuleRefreshHistoryRepository = new Mock<IRepository<OwnershipRuleRefreshHistory>>();
            this.mockNodeOwnershipRuleRepository = new Mock<IRepository<NodeOwnershipRule>>();
            this.mockNodeProductRuleRepository = new Mock<IRepository<NodeProductRule>>();
            this.mockNodeConnectionProductRuleRepository = new Mock<IRepository<NodeConnectionProductRule>>();

            this.mockUnitOfWork.Setup(a => a.CreateRepository<OwnershipRuleRefreshHistory>()).Returns(this.mockOwnershipRuleRefreshHistoryRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<NodeOwnershipRule>()).Returns(this.mockNodeOwnershipRuleRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<NodeProductRule>()).Returns(this.mockNodeProductRuleRepository.Object);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<NodeConnectionProductRule>()).Returns(this.mockNodeConnectionProductRuleRepository.Object);

            this.mockUnitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockUnitOfWork.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));
            this.mockExecutionManagerFactory.Setup(a => a.GetExecutionManager(It.IsAny<TicketType>())).Returns(this.mockExecutionManager.Object);

            this.mockFinalizerFactory.Setup(f => f.GetFinalizer(It.IsAny<FinalizerType>())).Returns(this.mockOwnershipFinalizer.Object);

            this.ownershipRuleProcessor = new OwnershipRuleProcessor(
                this.mockFactory.Object,
                this.mockLogger.Object,
                this.mockUnitOfWorkFactory.Object,
                this.mockAzureClientFactory.Object,
                this.mockFinalizerFactory.Object,
                this.mockConfigurationHandler.Object,
                this.mockOwnershipRuleService.Object,
                this.mockExecutionChainBuilder.Object,
                this.mockExecutionManagerFactory.Object);
        }

        /// <summary>
        /// Processes the ownership asynchronous resultexcel should process ownership asynchronous.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task SyncOwnershipRulesAsync_CallFico_Should_ProcessOwnershipRulesAsync()
        {
            this.SetupData();

            await this.ownershipRuleProcessor.SyncOwnershipRulesAsync("system").ConfigureAwait(false);

            this.mockOwnershipRuleService.Verify(m => m.GetActiveRulesAsync(), Times.Once);

            this.mockOwnershipRuleRefreshHistoryRepository.Verify(a => a.Insert(It.IsAny<OwnershipRuleRefreshHistory>()), Times.Once);
            this.mockOwnershipRuleRefreshHistoryRepository.Verify(a => a.Update(It.IsAny<OwnershipRuleRefreshHistory>()), Times.Once);

            this.mockNodeConnectionProductRuleRepository.Verify(a => a.GetAllAsync(null), Times.Once);
            this.mockNodeConnectionProductRuleRepository.Verify(a => a.InsertAll(It.IsAny<IEnumerable<NodeConnectionProductRule>>()), Times.Once);
            this.mockNodeConnectionProductRuleRepository.Verify(a => a.Update(It.IsAny<NodeConnectionProductRule>()), Times.Never);
            this.mockNodeConnectionProductRuleRepository.Verify(a => a.UpdateAll(It.IsAny<IEnumerable<NodeConnectionProductRule>>()), Times.Once);

            this.mockNodeProductRuleRepository.Verify(a => a.GetAllAsync(null), Times.Once);
            this.mockNodeProductRuleRepository.Verify(a => a.InsertAll(It.IsAny<IEnumerable<NodeProductRule>>()), Times.Never);
            this.mockNodeProductRuleRepository.Verify(a => a.Update(It.IsAny<NodeProductRule>()), Times.Exactly(3));
            this.mockNodeProductRuleRepository.Verify(a => a.UpdateAll(It.IsAny<IEnumerable<NodeProductRule>>()), Times.Never);

            this.mockNodeOwnershipRuleRepository.Verify(a => a.GetAllAsync(null), Times.Once);
            this.mockNodeOwnershipRuleRepository.Verify(a => a.InsertAll(It.IsAny<IEnumerable<NodeOwnershipRule>>()), Times.Once);
            this.mockNodeOwnershipRuleRepository.Verify(a => a.Update(It.IsAny<NodeOwnershipRule>()), Times.Exactly(2));
            this.mockNodeOwnershipRuleRepository.Verify(a => a.UpdateAll(It.IsAny<IEnumerable<NodeOwnershipRule>>()), Times.Once);

            this.mockUnitOfWork.Verify(m => m.SaveAsync(It.IsAny<CancellationToken>()), Times.Exactly(3));
        }

        /// <summary>
        /// Cleans the ownership data asynchronous should clean asynchronous.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task CleanOwnershipDataAsync_Should_CleanAsync()
        {
            this.mockOwnershipRepository = new Mock<IRepository<Ownership>>();
            this.mockUnitOfWork.Setup(a => a.CreateRepository<Ownership>()).Returns(this.mockOwnershipRepository.Object);
            this.mockOwnershipRepository.Setup(a => a.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()));
            await this.ownershipRuleProcessor.CleanOwnershipDataAsync(1).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(a => a.CreateRepository<Ownership>(), Times.Exactly(1));
            this.mockOwnershipRepository.Verify(a => a.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Gets the ownership ticket by ownership node identifier asynchronous should get asynchronous.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task GetOwnershipTicketByOwnershipNodeIdAsync_Should_GetAsync()
        {
            this.mockOwnershipNodeRepository = new Mock<IRepository<OwnershipNode>>();
            this.mockUnitOfWork.Setup(a => a.CreateRepository<OwnershipNode>()).Returns(this.mockOwnershipNodeRepository.Object);
            this.mockOwnershipNodeRepository.Setup(a => a.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new OwnershipNode { TicketId = 1 });
            var result = await this.ownershipRuleProcessor.GetOwnershipTicketByOwnershipNodeIdAsync(1).ConfigureAwait(false);

            Assert.IsTrue(result == 1);
            this.mockUnitOfWork.Verify(a => a.CreateRepository<OwnershipNode>(), Times.Exactly(1));
            this.mockOwnershipNodeRepository.Verify(a => a.GetByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public async Task OwnershipProcessor_FinalizeProcessAsync_WhenInvokedAsync()
        {
            this.mockOwnershipFinalizer.Setup(m => m.ProcessAsync(It.IsAny<object>()));

            // Act
            await this.ownershipRuleProcessor.FinalizeProcessAsync(new Processors.Ownership.Entities.OwnershipRuleData { TicketId = 1 }).ConfigureAwait(false);

            // Assert
            this.mockOwnershipFinalizer.Verify(m => m.ProcessAsync(It.IsAny<object>()), Times.Exactly(1));
        }

        private void SetupData()
        {
            var nodeOwnershipRules = new List<NodeOwnershipRule>
            {
                new NodeOwnershipRule
                {
                    RuleId = 1,
                    RuleName = "NodeOwnershipRule1",
                },
                new NodeOwnershipRule
                {
                    RuleId = 2,
                    RuleName = "NodeOwnershipRule2",
                },
                new NodeOwnershipRule
                {
                    RuleId = 3,
                    RuleName = "NodeOwnershipRule3",
                },
            };

            var nodeProductRules = new List<NodeProductRule>
            {
                new NodeProductRule
                {
                    RuleId = 1,
                    RuleName = "NodeProductRule1",
                },
                new NodeProductRule
                {
                    RuleId = 2,
                    RuleName = "NodeProductRule2",
                },
                new NodeProductRule
                {
                    RuleId = 3,
                    RuleName = "NodeProductRule3",
                },
            };

            var nodeConnectionRules = new List<NodeConnectionProductRule>
            {
                new NodeConnectionProductRule
                {
                    RuleId = 1,
                    RuleName = "NodeConnectionProductRule1",
                },
                new NodeConnectionProductRule
                {
                    RuleId = 2,
                    RuleName = "NodeConnectionProductRule2",
                },
                new NodeConnectionProductRule
                {
                    RuleId = 3,
                    RuleName = "NodeConnectionProductRule3",
                },
            };

            var ownershipRuleResponse = new OwnershipRuleResponse()
            {
                AuditedSteps = new List<AuditedStep>
                {
                    new AuditedStep
                    {
                        Scope = "scope",
                        StepMessage = "step 1",
                        StepName = "step",
                        StepNumber = 1,
                    },
                },
                NodeOwnershipRules = new List<OwnershipRule>
                {
                    new OwnershipRule
                    {
                        Name = "FicoNodeOwnershipRule1",
                        RuleId = 1,
                    },
                    new OwnershipRule
                    {
                        Name = "FicoNodeOwnershipRule2",
                        RuleId = 2,
                    },
                    new OwnershipRule
                    {
                        Name = "FicoNodeOwnershipRule4",
                        RuleId = 4,
                    },
                },
                NodeProductOwnershipRules = new List<OwnershipRule>
                {
                    new OwnershipRule
                    {
                        Name = "FicoNodeProductOwnershipRule3",
                        RuleId = 3,
                    },
                    new OwnershipRule
                    {
                        Name = "FicoNodeProductOwnershipRule1",
                        RuleId = 1,
                    },
                    new OwnershipRule
                    {
                        Name = "FicoNodeProductOwnershipRule2",
                        RuleId = 2,
                    },
                },
                OwnershipRuleConnections = new List<OwnershipRule>
                {
                    new OwnershipRule
                    {
                        Name = "FicoNodeConnectionProductOwnershipRule4",
                        RuleId = 4,
                    },
                    new OwnershipRule
                    {
                        Name = "FicoNodeConnectionProductOwnershipRule5",
                        RuleId = 5,
                    },
                    new OwnershipRule
                    {
                        Name = "FicoNodeConnectionProductOwnershipRule6",
                        RuleId = 6,
                    },
                },
            };

            this.mockNodeOwnershipRuleRepository.Setup(a => a.GetAllAsync(null)).ReturnsAsync(nodeOwnershipRules);
            this.mockNodeProductRuleRepository.Setup(a => a.GetAllAsync(null)).ReturnsAsync(nodeProductRules);
            this.mockNodeConnectionProductRuleRepository.Setup(a => a.GetAllAsync(null)).ReturnsAsync(nodeConnectionRules);
            this.mockOwnershipRuleService.Setup(a => a.GetActiveRulesAsync()).ReturnsAsync(ownershipRuleResponse);
        }
    }
}
