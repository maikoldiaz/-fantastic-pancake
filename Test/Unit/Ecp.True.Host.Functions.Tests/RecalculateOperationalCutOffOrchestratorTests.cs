// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecalculateOperationalCutOffOrchestratorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Chaos.Interfaces;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Balance.Orchestration;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Balance.Calculation.Input;
    using Ecp.True.Processors.Balance.Interfaces;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The operational cut off orchestrator tests.
    /// </summary>
    [TestClass]
    public class RecalculateOperationalCutOffOrchestratorTests
    {
        /// <summary>
        /// The logger mock.
        /// </summary>
        private readonly Mock<ITrueLogger<RecalculateOperationalCutoffBalanceOrchestrator>> loggerMock = new Mock<ITrueLogger<RecalculateOperationalCutoffBalanceOrchestrator>>();

        /// <summary>
        /// The service provider mock.
        /// </summary>
        private readonly Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();

        /// <summary>
        /// The balance processor Mock.
        /// </summary>
        private readonly Mock<IBalanceProcessor> balanceProcessorMock = new Mock<IBalanceProcessor>();

        /// <summary>
        /// The durable orchestration client.
        /// </summary>
        private readonly Mock<IDurableOrchestrationClient> orchestrationClientMock = new Mock<IDurableOrchestrationClient>();

        /// <summary>
        /// The durable orchestration context mock.
        /// </summary>
        private readonly Mock<IDurableOrchestrationContext> durableOrchestrationContextMock = new Mock<IDurableOrchestrationContext>();

        /// <summary>
        /// The durable activity context.
        /// </summary>
        private readonly Mock<IDurableActivityContext> activityContextMock = new Mock<IDurableActivityContext>();

        /// <summary>
        /// Gets the mock SQL token provider.
        /// </summary>
        /// <value>
        /// The mock SQL token provider.
        /// </value>
        private readonly Mock<ISqlTokenProvider> mockSqlTokenProvider = new Mock<ISqlTokenProvider>();

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The configuration handler mock.
        /// </summary>
        private readonly Mock<IConfigurationHandler> configurationHandlerMock = new Mock<IConfigurationHandler>();

        /// <summary>
        /// The failure handler mock.
        /// </summary>
        private readonly Mock<IFailureHandler> cutoffFailureHandlerMock = new Mock<IFailureHandler>();

        /// <summary>
        /// The failure handler mock.
        /// </summary>
        private readonly Mock<ITelemetry> telemetryMock = new Mock<ITelemetry>();

        /// <summary>
        /// The failure handler factory.
        /// </summary>
        private Mock<IFailureHandlerFactory> failureHandlerFactory;

        /// <summary>
        /// The operational cut off orchestrator.
        /// </summary>
        private RecalculateOperationalCutoffBalanceOrchestrator orchestrator;

        /// <summary>
        /// The mock chaos manager.
        /// </summary>
        private Mock<IChaosManager> mockChaosManager;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// Initialize the required set up.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            var connectionFactoryMock = new Mock<IConnectionFactory>();
            var azureClientFactory = new Mock<IAzureClientFactory>();
            connectionFactoryMock.Setup(m => m.IsReady).Returns(true);
            azureClientFactory.Setup(m => m.IsReady).Returns(true);
            this.mockChaosManager = new Mock<IChaosManager>();
            this.failureHandlerFactory = new Mock<IFailureHandlerFactory>();
            this.serviceProviderMock.Setup(m => m.GetService(typeof(IConnectionFactory))).Returns(connectionFactoryMock.Object);
            this.serviceProviderMock.Setup(m => m.GetService(typeof(IAzureClientFactory))).Returns(azureClientFactory.Object);
            this.serviceProviderMock.Setup(m => m.GetService(typeof(ISqlTokenProvider))).Returns(this.mockSqlTokenProvider.Object);
            this.serviceProviderMock.Setup(r => r.GetService(typeof(IChaosManager))).Returns(this.mockChaosManager.Object);
            this.serviceProviderMock.Setup(s => s.GetService(typeof(ITrueLogger<RecalculateOperationalCutoffBalanceOrchestrator>))).Returns(this.loggerMock.Object);
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockUnitOfWork.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockUnitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            var analysisSettings = new AnalysisSettings
            {
                ClientId = "ClientID",
                ClientSecret = "ClientValue",
                ModelName = "ModelName",
                Region = "Region",
                ServerName = "ServerName",
                TenantId = "TenantID",
            };

            this.configurationHandlerMock.Setup(m => m.GetConfigurationAsync<AnalysisSettings>(ConfigurationConstants.AnalysisSettings))
                .ReturnsAsync(analysisSettings);
            this.failureHandlerFactory.Setup(m => m.GetFailureHandler(It.IsAny<TicketType>())).Returns(this.cutoffFailureHandlerMock.Object);
            this.orchestrator = new RecalculateOperationalCutoffBalanceOrchestrator(
                this.loggerMock.Object,
                this.telemetryMock.Object,
                this.serviceProviderMock.Object,
                this.balanceProcessorMock.Object,
                this.configurationHandlerMock.Object,
                this.mockAzureClientFactory.Object,
                this.failureHandlerFactory.Object,
                this.mockUnitOfWorkFactory.Object);
        }

        /// <summary>
        /// The calculate balance should get the ticket by ID when triggered asynchronously.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task RecalculateBalanceAsync_ShouldProcessIfTicketIsValid_WhenTriggeredAsync()
        {
            var ticket = new Ticket
            {
                TicketId = 1,
                Status = StatusType.PROCESSING,
            };
            var mockTicketRepository = new Mock<IRepository<Ticket>>();
            mockTicketRepository.Setup(a => a.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ticket);
            this.mockUnitOfWork.Setup(a => a.CreateRepository<Ticket>()).Returns(mockTicketRepository.Object);

            this.orchestrationClientMock.Setup(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<OperationalCutOffInfo>())).ReturnsAsync("ticket");
            this.balanceProcessorMock.Setup(m => m.GetTicketByIdAsync(It.IsAny<int>())).ReturnsAsync(ticket);
            await this.orchestrator.RecalculateOperationalCutoffBalanceAsync(ticket.TicketId, null, null, this.orchestrationClientMock.Object, new Microsoft.Azure.WebJobs.ExecutionContext()).ConfigureAwait(false);

            mockTicketRepository.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Once);

            this.orchestrationClientMock.Verify(m => m.StartNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<OperationalCutOffInfo>()), Times.Once);
        }

        [TestMethod]
        public async Task ProcessSegmentAsync_ShouldCallProccessor_forTicketAsync()
        {
            var ticket = new Ticket
            {
                TicketId = 1,
                Status = StatusType.PROCESSING,
            };
            var operationalCutOffInfo = new OperationalCutOffInfo
            {
                BalanceInput = new BalanceInput
                {
                    Movements = new List<MovementCalculationInput>
                    {
                        new MovementCalculationInput
                        {
                            MovementId = "1",
                        },
                    },
                },
                Ticket = ticket,
                Step = MovementCalculationStep.Interface,
            };

            IEnumerable<SegmentUnbalance> segmentUnbalanceList = new List<SegmentUnbalance>
            {
                new SegmentUnbalance
                {
                    SegmentUnbalanceId = 1,
                },
            };
            this.balanceProcessorMock.Setup(m => m.ProcessSegmentAsync(1)).ReturnsAsync(segmentUnbalanceList);
            this.activityContextMock.Setup(m => m.GetInput<OperationalCutOffInfo>()).Returns(operationalCutOffInfo);
            var result = await this.orchestrator.ProcessSegmentAsync(this.activityContextMock.Object).ConfigureAwait(false);
            this.balanceProcessorMock.Verify(m => m.ProcessSegmentAsync(1), Times.Once);
            Assert.AreEqual(segmentUnbalanceList.ToArray()[0].SegmentUnbalanceId, result.SegmentUnbalances.ToArray()[0].SegmentUnbalanceId);
        }

        [TestMethod]
        public async Task ProcessSystemAsync_ShouldProcessSystem_forTicketAsync()
        {
            var ticket = new Ticket
            {
                TicketId = 1,
                Status = StatusType.PROCESSING,
            };
            var operationalCutOffInfo = new OperationalCutOffInfo
            {
                BalanceInput = new BalanceInput
                {
                    Movements = new List<MovementCalculationInput>
                    {
                        new MovementCalculationInput
                        {
                            MovementId = "1",
                        },
                    },
                },
                Ticket = ticket,
                Step = MovementCalculationStep.Interface,
            };

            IEnumerable<SystemUnbalance> systemUnbalanceList = new List<SystemUnbalance>
            {
                new SystemUnbalance
                {
                    SystemUnbalanceId = 1,
                },
            };
            this.balanceProcessorMock.Setup(m => m.ProcessSystemAsync(1)).ReturnsAsync(systemUnbalanceList);
            this.activityContextMock.Setup(m => m.GetInput<OperationalCutOffInfo>()).Returns(operationalCutOffInfo);
            var result = await this.orchestrator.RecalculateProcessSystemAsync(this.activityContextMock.Object).ConfigureAwait(false);
            this.balanceProcessorMock.Verify(m => m.ProcessSystemAsync(1), Times.Once);
            Assert.AreEqual(systemUnbalanceList.ToArray()[0].SystemUnbalanceId, result.SystemUnbalances.ToArray()[0].SystemUnbalanceId);
        }

        [TestMethod]
        public async Task RecalculateCompleteCalculationAsync_ShouldCompleteCalculation_forTicketAsync()
        {
            var ticket = new Ticket
            {
                TicketId = 1,
                Status = StatusType.PROCESSING,
            };
            var operationalCutOffInfo = new OperationalCutOffInfo
            {
                BalanceInput = new BalanceInput
                {
                    Movements = new List<MovementCalculationInput>
                    {
                        new MovementCalculationInput
                        {
                            MovementId = "1",
                        },
                    },
                },
                Ticket = ticket,
                Step = MovementCalculationStep.Interface,
            };
            this.activityContextMock.Setup(m => m.GetInput<OperationalCutOffInfo>()).Returns(operationalCutOffInfo);
            await this.orchestrator.RecalculateCompleteCalculationAsync(this.activityContextMock.Object).ConfigureAwait(false);
            this.balanceProcessorMock.Verify(m => m.CompleteAsync(operationalCutOffInfo), Times.Once);
        }

        [TestMethod]
        public async Task FinalizeCutOffAsync_ShouldFinalizeCutOffAsync_forTicketAsync()
        {
            var ticket = new Ticket
            {
                TicketId = 1,
                Status = StatusType.PROCESSING,
            };
            var operationalCutOffInfo = new OperationalCutOffInfo
            {
                BalanceInput = new BalanceInput
                {
                    Movements = new List<MovementCalculationInput>
                    {
                        new MovementCalculationInput
                        {
                            MovementId = "1",
                        },
                    },
                },
                Ticket = ticket,
                Step = MovementCalculationStep.Interface,
            };
            this.activityContextMock.Setup(m => m.GetInput<OperationalCutOffInfo>()).Returns(operationalCutOffInfo);
            await this.orchestrator.RecalculateFinalizeCutOffAsync(this.activityContextMock.Object).ConfigureAwait(false);
            this.balanceProcessorMock.Verify(m => m.FinalizeProcessAsync(operationalCutOffInfo.Ticket.TicketId), Times.Once);
        }

        [TestMethod]
        public async Task RecalculateBalanceAsync_ShouldFail_WhenExceptionThrownAsync()
        {
            var ticket = new Ticket
            {
                TicketId = 1,
                Status = StatusType.PROCESSING,
            };

            var operationalCutOffInfo = new OperationalCutOffInfo
            {
                BalanceInput = new BalanceInput
                {
                    Movements = new List<MovementCalculationInput>
                    {
                        new MovementCalculationInput
                        {
                            MovementId = "1",
                        },
                    },
                },
                Ticket = ticket,
                Step = MovementCalculationStep.Interface,
            };

            this.durableOrchestrationContextMock.Setup(m => m.GetInput<OperationalCutOffInfo>()).Returns(operationalCutOffInfo);
            this.durableOrchestrationContextMock.Setup(x => x.CallActivityAsync<OperationalCutOffInfo>(It.IsAny<string>(), It.IsAny<object>())).ThrowsAsync(new Exception());
            this.orchestrationClientMock.Setup(m => m.GetStatusAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new DurableOrchestrationStatus { RuntimeStatus = OrchestrationRuntimeStatus.Completed });
            await this.orchestrator.RecalculateOperationalCutoffBalanceOrchestratorAsync(this.durableOrchestrationContextMock.Object).ConfigureAwait(false);
            this.durableOrchestrationContextMock.Verify(x => x.CallActivityAsync<OperationalCutOffInfo>(It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(1));
        }

        [TestMethod]
        public async Task DeleteSegmentBalanceAsync_ShouldDeleteSegmentBalanceAsync_forTicketAsync()
        {
            var ticket = new Ticket
            {
                TicketId = 1,
                Status = StatusType.PROCESSING,
            };
            var operationalCutOffInfo = new OperationalCutOffInfo
            {
                BalanceInput = new BalanceInput
                {
                    Movements = new List<MovementCalculationInput>
                    {
                        new MovementCalculationInput
                        {
                            MovementId = "1",
                        },
                    },
                },
                Ticket = ticket,
                Step = MovementCalculationStep.Interface,
            };
            this.activityContextMock.Setup(m => m.GetInput<OperationalCutOffInfo>()).Returns(operationalCutOffInfo);
            await this.orchestrator.DeleteSegmentBalanceAsync(this.activityContextMock.Object).ConfigureAwait(false);
            this.balanceProcessorMock.Verify(m => m.DeleteBalanceAsync<SegmentUnbalance>(operationalCutOffInfo.Ticket.TicketId), Times.Once);
        }

        [TestMethod]
        public async Task DeleteSystemBalanceAsync_ShouldDeleteSystemBalanceAsync_forTicketAsync()
        {
            var ticket = new Ticket
            {
                TicketId = 1,
                Status = StatusType.PROCESSING,
            };
            var operationalCutOffInfo = new OperationalCutOffInfo
            {
                BalanceInput = new BalanceInput
                {
                    Movements = new List<MovementCalculationInput>
                    {
                        new MovementCalculationInput
                        {
                            MovementId = "1",
                        },
                    },
                },
                Ticket = ticket,
                Step = MovementCalculationStep.Interface,
            };
            this.activityContextMock.Setup(m => m.GetInput<OperationalCutOffInfo>()).Returns(operationalCutOffInfo);
            await this.orchestrator.DeleteSystemBalanceAsync(this.activityContextMock.Object).ConfigureAwait(false);
            this.balanceProcessorMock.Verify(m => m.DeleteBalanceAsync<SystemUnbalance>(operationalCutOffInfo.Ticket.TicketId), Times.Once);
        }

        [TestMethod]
        public async Task RecalculateOperationalCutoffOrchestratorAsync_ShouldCallRecalculateOperationalCutoffOrchestrator_WhenTriggeredAsync()
        {
            var ticket = new Ticket
            {
                TicketId = 1,
                Status = StatusType.PROCESSING,
            };

            var info = new OperationalCutOffInfo
            {
                Ticket = ticket,
                Step = MovementCalculationStep.Interface,
                Caller = FunctionNames.RecalculateOperationalCutOff,
                ChaosValue = string.Empty,
                Orchestrator = OrchestratorNames.RecalculateOperationalCutoffBalanceOrchestrator,
                ReplyTo = string.Empty,
            };

            this.durableOrchestrationContextMock.Setup(m => m.GetInput<OperationalCutOffInfo>()).Returns(info);
            this.durableOrchestrationContextMock.Setup(x => x.CallActivityAsync<OperationalCutOffInfo>(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(info);
            await this.orchestrator.RecalculateOperationalCutoffBalanceOrchestratorAsync(this.durableOrchestrationContextMock.Object).ConfigureAwait(false);
            this.durableOrchestrationContextMock.Verify(x => x.CallActivityAsync<OperationalCutOffInfo>(It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(4));
        }
    }
}
