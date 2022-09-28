// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BalanceProcessorTests.cs" company="Microsoft">
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
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Balance;
    using Ecp.True.Processors.Balance.Calculation.Input;
    using Ecp.True.Processors.Balance.Calculation.Output;
    using Ecp.True.Processors.Core.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;

    /// <summary>
    /// The balance processor tests.
    /// </summary>
    [TestClass]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Test Class")]
    public class BalanceProcessorTests
    {
        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IFinalizer> mockOperationalCutOffFinalizer = new Mock<IFinalizer>();

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
        /// The unbalance repository mock.
        /// </summary>
        private Mock<IRepository<Unbalance>> unbalanceRepositoryMock;

        /// <summary>
        /// The balance service mock.
        /// </summary>
        private Mock<IBalanceService> balanceServiceMock;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<BalanceProcessor>> logger;

        /// <summary>
        /// The balance calculator.
        /// </summary>
        private BalanceProcessor balanceProcessor;

        /// <summary>
        /// The ticket.
        /// </summary>
        private Ticket ticket;

        /// <summary>
        /// The ticket repository.
        /// </summary>
        private Mock<IRepository<Ticket>> ticketRepository;

        /// <summary>
        /// The balance service.
        /// </summary>
        private Mock<ISegmentBalanceService> segmentBalanceServiceMock;

        /// <summary>
        /// The balance service.
        /// </summary>
        private Mock<ISystemBalanceService> systemBalanceServiceMock;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> configurationHandlerMock;

        /// <summary>
        /// The registration strategy factory mock.
        /// </summary>
        private Mock<IRegistrationStrategyFactory> registrationStrategyFactoryMock;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.repositoryFactory = new Mock<IRepositoryFactory>();
            this.registrationStrategyFactoryMock = new Mock<IRegistrationStrategyFactory>();
            this.ticket = JsonConvert.DeserializeObject<Ticket>(File.ReadAllText("TicketJson/Ticket.json"));

            this.ticketRepository = new Mock<IRepository<Ticket>>();
            this.ticketRepository.Setup(x => x.GetByIdAsync(It.IsAny<object>())).Returns(Task.FromResult(this.ticket));
            this.repositoryFactory.Setup(x => x.CreateRepository<Ticket>()).Returns(this.ticketRepository.Object);

            this.unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.unitOfWorkMock = new Mock<IUnitOfWork>();
            this.unbalanceRepositoryMock = new Mock<IRepository<Unbalance>>();
            this.balanceServiceMock = new Mock<IBalanceService>();
            this.segmentBalanceServiceMock = new Mock<ISegmentBalanceService>();
            this.systemBalanceServiceMock = new Mock<ISystemBalanceService>();
            this.unitOfWorkFactory.Setup(x => x.GetUnitOfWork()).Returns(this.unitOfWorkMock.Object);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<Unbalance>()).Returns(this.unbalanceRepositoryMock.Object);
            this.unitOfWorkMock.Setup(x => x.CreateRepository<Ticket>()).Returns(this.ticketRepository.Object);
            this.unitOfWorkMock.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));
            this.configurationHandlerMock = new Mock<IConfigurationHandler>();

            this.configurationHandlerMock.Setup(x => x.GetConfigurationAsync<ServiceBusSettings>(It.IsAny<string>())).ReturnsAsync(new ServiceBusSettings());

            this.logger = new Mock<ITrueLogger<BalanceProcessor>>();
            this.balanceProcessor = new BalanceProcessor(
                this.repositoryFactory.Object,
                this.unitOfWorkFactory.Object,
                this.balanceServiceMock.Object,
                this.segmentBalanceServiceMock.Object,
                this.systemBalanceServiceMock.Object,
                this.logger.Object,
                this.configurationHandlerMock.Object,
                this.registrationStrategyFactoryMock.Object,
                this.mockOperationalCutOffFinalizer.Object);
        }

        [TestMethod]
        public async Task BalanceProcessor_ShouldCalculate_WithSuccessAsync()
        {
            // Act
            var ticketInfo = new UnbalanceRequest { Ticket = new Ticket { TicketId = 1, CategoryElementId = 2, StartDate = new DateTime(2019, 02, 12), EndDate = new DateTime(2019, 02, 12), Status = 0, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null }, TransferPoints = new List<TransferPoints>() };
            var calculationOutput = new CalculationOutput();

            calculationOutput.Movements.ToList().Add(new Movement());
            calculationOutput.Unbalances.ToList().Add(new UnbalanceComment());
            this.balanceServiceMock.Setup(x => x.ProcessStepAsync(It.IsAny<UnbalanceRequest>(), MovementCalculationStep.Unbalance, null, this.logger.Object)).Returns(Task.FromResult(calculationOutput));
            var output = await this.balanceProcessor.CalculateAsync(ticketInfo).ConfigureAwait(false);

            Assert.IsNotNull(output);
        }

        [TestMethod]
        public async Task BalanceProcessor_CalculateAsync_ShouldOrderUnbalancesByCalculationDateAsync()
        {
            var unbalances = new List<UnbalanceComment>
            {
                new UnbalanceComment
                {
                    UnbalanceId = 1,
                    CreatedDate = DateTime.UtcNow.AddDays(-3),
                },
                new UnbalanceComment
                {
                    UnbalanceId = 2,
                    CreatedDate = DateTime.UtcNow.AddDays(-2),
                },
                new UnbalanceComment
                {
                    UnbalanceId = 3,
                    CreatedDate = DateTime.UtcNow,
                },
            };

            var calculationOutput = new CalculationOutput(new List<Movement> { new Movement() }, unbalances);
            this.ticket.Status = StatusType.PROCESSING;
            this.ticket.CategoryElementId = 3;

            this.balanceServiceMock.Setup(x => x.ProcessStepAsync(It.IsAny<UnbalanceRequest>(), It.IsAny<MovementCalculationStep>(), null, It.IsAny<ITrueLogger>())).ReturnsAsync(calculationOutput);

            await this.balanceProcessor.CalculateAsync(new UnbalanceRequest()).ConfigureAwait(false);

            Assert.AreEqual(1, calculationOutput.Unbalances.FirstOrDefault().UnbalanceId);
            Assert.AreEqual(2, calculationOutput.Unbalances.ToArray()[1].UnbalanceId);
            Assert.AreEqual(3, calculationOutput.Unbalances.ToArray()[2].UnbalanceId);
        }

        [TestMethod]
        public async Task BalanceProcessor_GetTicketByIdAsync_ShouldGetTicketAsync()
        {
            var ticketLocal = new Ticket
            {
                TicketId = 1,
                TicketTypeId = TicketType.Cutoff,
            };

            this.ticketRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ticketLocal);

            // Act
            var ticketReceived = await this.balanceProcessor.GetTicketByIdAsync(1).ConfigureAwait(false);

            // Assert or Verify
            Assert.AreEqual(TicketType.Cutoff, ticketReceived.TicketTypeId);
        }

        [TestMethod]
        public async Task BalanceProcessor_GetBalanceInputAsync_ShouldGetBalanceInputAsync()
        {
            var ticketLocal = new Ticket
            {
                TicketId = 1,
                TicketTypeId = TicketType.Cutoff,
            };

            var balanceInput = new BalanceInput
            {
                Movements = new List<MovementCalculationInput>
                {
                    new MovementCalculationInput
                    {
                        MovementId = "1",
                    },
                },
                Nodes = new List<NodeInput>
                {
                    new NodeInput
                    {
                        NodeId = 1,
                    },
                },
            };

            this.balanceServiceMock.Setup(x => x.GetBalanceInputAsync(It.IsAny<UnbalanceRequest>())).ReturnsAsync(balanceInput);

            var balanceInputReceived = await this.balanceProcessor.GetBalanceInputAsync(ticketLocal).ConfigureAwait(false);

            Assert.AreEqual(balanceInput.Movements.ToArray()[0].MovementId, balanceInputReceived.Movements.ToArray()[0].MovementId);
            Assert.AreEqual(balanceInput.Nodes.ToArray()[0].NodeId, balanceInputReceived.Nodes.ToArray()[0].NodeId);
        }

        [TestMethod]
        public async Task BalanceProcessor_ProcessCalculationAsync_ShouldReturnCalculationOutputAsync()
        {
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
                Ticket = new Ticket
                {
                    TicketId = 1,
                },
                Step = MovementCalculationStep.Unbalance,
            };

            var unbalances = new List<Unbalance>
            {
                new Unbalance
                {
                    UnbalanceId = 1,
                    CreatedDate = DateTime.UtcNow.AddDays(-3),
                },
                new Unbalance
                {
                    UnbalanceId = 2,
                    CreatedDate = DateTime.UtcNow.AddDays(-2),
                },
                new Unbalance
                {
                    UnbalanceId = 3,
                    CreatedDate = DateTime.UtcNow,
                },
            };

            var calculationOutput = new CalculationOutput(new List<Movement> { new Movement() }, unbalances);
            this.balanceServiceMock.Setup(x => x.ProcessCalculationAsync(
                operationalCutOffInfo.BalanceInput,
                operationalCutOffInfo.Ticket,
                this.unitOfWorkMock.Object,
                operationalCutOffInfo.Step)).ReturnsAsync(calculationOutput);

            var calculationOutputReceived = await this.balanceProcessor.ProcessCalculationAsync(operationalCutOffInfo).ConfigureAwait(false);

            Assert.AreEqual(calculationOutput.UnbalanceList.ToArray()[0].UnbalanceId, calculationOutputReceived.UnbalanceList.ToArray()[0].UnbalanceId);
            Assert.AreEqual(calculationOutput.UnbalanceList.ToArray()[1].UnbalanceId, calculationOutputReceived.UnbalanceList.ToArray()[1].UnbalanceId);
            Assert.AreEqual(calculationOutput.UnbalanceList.ToArray()[2].UnbalanceId, calculationOutputReceived.UnbalanceList.ToArray()[2].UnbalanceId);
        }

        [TestMethod]
        public async Task BalanceProcessor_ProcessSegmentAsync_ShouldProcessSegmentAsync()
        {
            var ticketLocal = new Ticket
            {
                TicketId = 1,
                TicketTypeId = TicketType.Cutoff,
            };

            IEnumerable<SegmentUnbalance> segmentUnbalanceList = new List<SegmentUnbalance>
            {
                new SegmentUnbalance
                {
                    SegmentUnbalanceId = 1,
                },
            };

            this.segmentBalanceServiceMock.Setup(x => x.ProcessSegmentAsync(It.IsAny<int>(), this.unitOfWorkMock.Object)).Returns(Task.FromResult(segmentUnbalanceList));
            var segmentUnbalancesReceived = await this.balanceProcessor.ProcessSegmentAsync(ticketLocal.TicketId).ConfigureAwait(false);
            Assert.AreEqual(segmentUnbalanceList.ToArray()[0].SegmentUnbalanceId, segmentUnbalancesReceived.ToArray()[0].SegmentUnbalanceId);
        }

        [TestMethod]
        public async Task BalanceProcessor_ProcessSystemAsync_ShouldProcessSystemAsync()
        {
            var ticketLocal = new Ticket
            {
                TicketId = 1,
                TicketTypeId = TicketType.Cutoff,
            };

            IEnumerable<SystemUnbalance> systemUnbalanceList = new List<SystemUnbalance>
            {
                new SystemUnbalance
                {
                    SystemUnbalanceId = 1,
                },
            };

            this.systemBalanceServiceMock.Setup(x => x.ProcessSystemAsync(It.IsAny<int>(), this.unitOfWorkMock.Object)).Returns(Task.FromResult(systemUnbalanceList));
            var systemUnbalancesReceived = await this.balanceProcessor.ProcessSystemAsync(ticketLocal.TicketId).ConfigureAwait(false);
            Assert.AreEqual(systemUnbalanceList.ToArray()[0].SystemUnbalanceId, systemUnbalancesReceived.ToArray()[0].SystemUnbalanceId);
        }

        [TestMethod]
        public async Task BalanceProcessor_RegisterAsync_ShoulRegisterMovementsAndUnbalancesAsync()
        {
            var calculationDate = DateTime.UtcNow.AddDays(-3);
            var unbalances = new List<Unbalance>
            {
                new Unbalance
                {
                    UnbalanceId = 1,
                    CreatedDate = DateTime.UtcNow.AddDays(-3),
                    NodeId = 1,
                    ProductId = "1",
                    CalculationDate = calculationDate,
                    ToleranceUnbalance = 2,
                    ToleranceInputs = 3,
                    UnbalanceAmount = 200,
                },
                new Unbalance
                {
                    UnbalanceId = 2,
                    CreatedDate = DateTime.UtcNow.AddDays(-2),
                    ProductId = "4",
                    NodeId = 2,
                    CalculationDate = calculationDate,
                    ToleranceUnbalance = 2,
                    ToleranceInputs = 3,
                    UnbalanceAmount = 200,
                },
                new Unbalance
                {
                    UnbalanceId = 3,
                    NodeId = 3,
                    CreatedDate = DateTime.UtcNow,
                    ProductId = "5",
                    CalculationDate = calculationDate,
                    ToleranceUnbalance = 2,
                    ToleranceInputs = 3,
                    UnbalanceAmount = 200,
                },
            };
            var toleranceUnbalances = new List<Unbalance>
            {
                new Unbalance
                {
                    UnbalanceId = 1,
                    CreatedDate = DateTime.UtcNow.AddDays(-3),
                    NodeId = 1,
                    ProductId = "1",
                    CalculationDate = calculationDate,
                    ToleranceUnbalance = 2,
                    ToleranceInputs = 3,
                },
                new Unbalance
                {
                    UnbalanceId = 2,
                    CreatedDate = DateTime.UtcNow.AddDays(-2),
                    ProductId = "4",
                    NodeId = 2,
                    CalculationDate = calculationDate,
                    ToleranceUnbalance = 2,
                    ToleranceInputs = 3,
                },
                new Unbalance
                {
                    UnbalanceId = 3,
                    NodeId = 3,
                    CreatedDate = DateTime.UtcNow,
                    ProductId = "5",
                    CalculationDate = calculationDate,
                    ToleranceUnbalance = 2,
                    ToleranceInputs = 3,
                },
            };
            var interfaceUnbalances = new List<Unbalance>
            {
                new Unbalance
                {
                    UnbalanceId = 1,
                    CreatedDate = DateTime.UtcNow.AddDays(-3),
                    NodeId = 1,
                    ProductId = "1",
                    CalculationDate = calculationDate,
                },
                new Unbalance
                {
                    UnbalanceId = 2,
                    CreatedDate = DateTime.UtcNow.AddDays(-2),
                    NodeId = 2,
                    ProductId = "4",
                    CalculationDate = calculationDate,
                },
                new Unbalance
                {
                    UnbalanceId = 3,
                    NodeId = 3,
                    CreatedDate = DateTime.UtcNow,
                    ProductId = "5",
                    CalculationDate = calculationDate,
                },
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
                Ticket = new Ticket
                {
                    TicketId = 1,
                },
                Step = MovementCalculationStep.Unbalance,
            };
            operationalCutOffInfo.Unbalances.Add(MovementCalculationStep.BalanceTolerances, toleranceUnbalances);
            operationalCutOffInfo.Unbalances.Add(MovementCalculationStep.Interface, interfaceUnbalances);
            operationalCutOffInfo.Unbalances.Add(MovementCalculationStep.UnidentifiedLosses, new List<Unbalance>());
            operationalCutOffInfo.Unbalances.Add(MovementCalculationStep.Unbalance, unbalances);

            this.configurationHandlerMock.Setup(m => m.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings))
                .ReturnsAsync(new SystemSettings
                {
                    WarningLimit = 2,
                    ActionLimit = 3,
                    ToleranceLimit = 4,
                });
            IEnumerable<Unbalance> callbackUnbalanceList = new List<Unbalance>();
            this.unbalanceRepositoryMock.Setup(x => x.InsertAll(It.IsAny<IEnumerable<Unbalance>>())).
                Callback<IEnumerable<Unbalance>>((unbalanceList) => { callbackUnbalanceList = unbalanceList; });
            this.registrationStrategyFactoryMock.Setup(x => x.MovementRegistrationStrategy.Insert(It.IsAny<IEnumerable<object>>(), this.unitOfWorkMock.Object)).Verifiable();
            await this.balanceProcessor.RegisterAsync(operationalCutOffInfo).ConfigureAwait(false);
            this.registrationStrategyFactoryMock.Verify(x => x.MovementRegistrationStrategy.Insert(It.IsAny<IEnumerable<object>>(), this.unitOfWorkMock.Object), Times.Once);
            this.unbalanceRepositoryMock.Verify(x => x.InsertAll(It.IsAny<IEnumerable<Unbalance>>()), Times.Once);
            this.unitOfWorkMock.Verify(x => x.SaveAsync(CancellationToken.None), Times.Once);
            Assert.AreEqual(callbackUnbalanceList.ToArray()[0].UnbalanceId, unbalances.ToArray()[0].UnbalanceId);
            Assert.AreEqual(callbackUnbalanceList.ToArray()[1].UnbalanceId, unbalances.ToArray()[1].UnbalanceId);
            Assert.AreEqual(callbackUnbalanceList.ToArray()[2].UnbalanceId, unbalances.ToArray()[2].UnbalanceId);
            Assert.AreEqual(callbackUnbalanceList.ToArray()[0].UnbalanceAmount, unbalances.ToArray()[0].UnbalanceAmount);
            Assert.AreEqual(callbackUnbalanceList.ToArray()[1].UnbalanceAmount, unbalances.ToArray()[1].UnbalanceAmount);
            Assert.AreEqual(callbackUnbalanceList.ToArray()[2].UnbalanceAmount, unbalances.ToArray()[2].UnbalanceAmount);
        }

        [TestMethod]
        public async Task BalanceProcessor_CompleteAsync_ShouldCompleteUnbalanceCalculationsAsync()
        {
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
                Ticket = new Ticket
                {
                    TicketId = 1,
                },
                Step = MovementCalculationStep.Unbalance,
                SegmentUnbalances = new List<SegmentUnbalance>
                {
                    new SegmentUnbalance
                    {
                        SegmentUnbalanceId = 5,
                    },
                },
                SystemUnbalances = new List<SystemUnbalance>
                {
                    new SystemUnbalance
                    {
                        SystemUnbalanceId = 6,
                    },
                },
            };

            var segmentRepoMock = new Mock<IRepository<SegmentUnbalance>>();
            IEnumerable<SegmentUnbalance> callbackSegmentUnbalances = new List<SegmentUnbalance>();
            this.unitOfWorkMock.Setup(x => x.CreateRepository<SegmentUnbalance>()).Returns(segmentRepoMock.Object);
            segmentRepoMock.Setup(m => m.InsertAll(It.IsAny<IEnumerable<SegmentUnbalance>>()))
                .Callback<IEnumerable<SegmentUnbalance>>((list) => { callbackSegmentUnbalances = list; })
                .Verifiable();

            var systemRepoMock = new Mock<IRepository<SystemUnbalance>>();
            IEnumerable<SystemUnbalance> callbackSystemUnbalances = new List<SystemUnbalance>();
            this.unitOfWorkMock.Setup(x => x.CreateRepository<SystemUnbalance>()).Returns(systemRepoMock.Object);
            systemRepoMock.Setup(m => m.InsertAll(It.IsAny<IEnumerable<SystemUnbalance>>()))
                .Callback<IEnumerable<SystemUnbalance>>((list) => { callbackSystemUnbalances = list; })
                .Verifiable();

            var ticketRepositoryMock = new Mock<IRepository<Ticket>>();
            var callbackTicket = new Ticket();
            this.unitOfWorkMock.Setup(x => x.CreateRepository<Ticket>()).Returns(ticketRepositoryMock.Object);
            ticketRepositoryMock.Setup(m => m.Update(It.IsAny<Ticket>()))
                .Callback<Ticket>((ticket) => { callbackTicket = ticket; });

            var parameters = new Dictionary<string, object>
                {
                    { "@TicketId", operationalCutOffInfo.Ticket.TicketId },
                };

            await this.balanceProcessor.CompleteAsync(operationalCutOffInfo).ConfigureAwait(false);

            segmentRepoMock.Verify(m => m.InsertAll(It.IsAny<IEnumerable<SegmentUnbalance>>()), Times.Once);
            systemRepoMock.Verify(m => m.InsertAll(It.IsAny<IEnumerable<SystemUnbalance>>()), Times.Once);
            this.unitOfWorkMock.Verify(x => x.SaveAsync(CancellationToken.None), Times.Once);
            ticketRepositoryMock.Verify(m => m.ExecuteAsync(Repositories.Constants.SaveOperativeNodeRelationship, parameters), Times.Once);
            Assert.AreEqual(operationalCutOffInfo.SegmentUnbalances.ToArray()[0].SegmentUnbalanceId, callbackSegmentUnbalances.ToArray()[0].SegmentUnbalanceId);
            Assert.AreEqual(operationalCutOffInfo.SystemUnbalances.ToArray()[0].SystemUnbalanceId, callbackSystemUnbalances.ToArray()[0].SystemUnbalanceId);
            Assert.AreEqual(operationalCutOffInfo.Ticket.TicketId, callbackTicket.TicketId);
        }

        [TestMethod]
        public async Task BalanceProcessor_CleanOperationalCutOffDataAsync_ShouldCleanOperationalCutOffDataAsync()
        {
            var ticketLocal = new Ticket
            {
                TicketId = 1,
                TicketTypeId = TicketType.Cutoff,
            };

            var parameters = new Dictionary<string, object>
            {
                { "@TicketId", ticketLocal.TicketId },
            };

            var ownershipRepoMock = new Mock<IRepository<Ownership>>();
            ownershipRepoMock.Setup(m => m.ExecuteAsync(Repositories.Constants.OperationalCutOffAndOwnershipCleanupProcedureName, parameters)).Verifiable();
            this.unitOfWorkMock.Setup(x => x.CreateRepository<Ownership>()).Returns(ownershipRepoMock.Object);
            await this.balanceProcessor.CleanOperationalCutOffDataAsync(ticketLocal.TicketId).ConfigureAwait(false);
            ownershipRepoMock.Verify(m => m.ExecuteAsync(Repositories.Constants.OperationalCutOffAndOwnershipCleanupProcedureName, parameters), Times.Once);
        }

        [TestMethod]
        public async Task BalanceProcessor_ValidateUnbalanceTicketAsyncAsync_ShouldReturnNodesAsync()
        {
            var ticketLocal = new Ticket
            {
                TicketId = 1,
                TicketTypeId = TicketType.Cutoff,
                StartDate = DateTime.UtcNow.AddDays(-2),
                EndDate = DateTime.UtcNow,
                CategoryElementId = 1,
            };

            var parameters = new Dictionary<string, object>
            {
                { "@SegmentId", ticketLocal.CategoryElementId },
                { "@StartDate", ticketLocal.StartDate },
                { "@EndDate", ticketLocal.EndDate },
            };

            IEnumerable<OwnershipInitialInventoryNode> nodeList = new List<OwnershipInitialInventoryNode>
            {
                new OwnershipInitialInventoryNode
                {
                    NodeName = "someNode",
                    InventoryDate = DateTime.UtcNow,
                    Type = 1,
                },
            };

            var ownershipInitialInventoryRepoMock = new Mock<IRepository<OwnershipInitialInventoryNode>>();
            ownershipInitialInventoryRepoMock.Setup(m => m.ExecuteQueryAsync(Repositories.Constants.ValidateInitialInventoriesForOwnershipProcedureName, parameters)).Returns(Task.FromResult(nodeList)).Verifiable();
            this.unitOfWorkMock.Setup(x => x.CreateRepository<OwnershipInitialInventoryNode>()).Returns(ownershipInitialInventoryRepoMock.Object);
            var result = await this.balanceProcessor.ValidateOwnershipInitialInventoryAsync(ticketLocal).ConfigureAwait(false);
            ownershipInitialInventoryRepoMock.Verify(m => m.ExecuteQueryAsync(Repositories.Constants.ValidateInitialInventoriesForOwnershipProcedureName, parameters), Times.Once);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(nodeList.FirstOrDefault().NodeName, result.FirstOrDefault().NodeName);
            Assert.AreEqual(nodeList.FirstOrDefault().InventoryDate, result.FirstOrDefault().InventoryDate);
            Assert.AreEqual(nodeList.FirstOrDefault().Type, result.FirstOrDefault().Type);
        }

        [TestMethod]
        public async Task BalanceProcessor_GetTransferPointsAsync_ShouldReturnMovementsAsync()
        {
            var ticketLocal = new Ticket
            {
                StartDate = DateTime.UtcNow.AddDays(-2),
                EndDate = DateTime.UtcNow,
                CategoryElementId = 10,
            };

            var parameters = new Dictionary<string, object>
            {
                { "@SegmentId", ticketLocal.CategoryElementId },
                { "@StartDate", ticketLocal.StartDate },
                { "@EndDate", ticketLocal.EndDate },
            };

            IEnumerable<OfficialTransferPointMovement> movements = new List<OfficialTransferPointMovement>
            {
                new OfficialTransferPointMovement
                {
                    MovementId = "1",
                    OperationalDate = DateTime.UtcNow,
                    SapTrackingId = 1,
                },
            };

            var officialTransferPointMovementRepoMock = new Mock<IRepository<OfficialTransferPointMovement>>();
            officialTransferPointMovementRepoMock.Setup(m => m.ExecuteQueryAsync(Repositories.Constants.GetOfficialTransferPointMovements, parameters)).Returns(Task.FromResult(movements)).Verifiable();
            this.unitOfWorkMock.Setup(x => x.CreateRepository<OfficialTransferPointMovement>()).Returns(officialTransferPointMovementRepoMock.Object);
            var result = await this.balanceProcessor.GetTransferPointsAsync(ticketLocal).ConfigureAwait(false);
            officialTransferPointMovementRepoMock.Verify(m => m.ExecuteQueryAsync(Repositories.Constants.GetOfficialTransferPointMovements, parameters), Times.Once);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(movements.FirstOrDefault().MovementId, result.FirstOrDefault().MovementId);
            Assert.AreEqual(movements.FirstOrDefault().OperationalDate, result.FirstOrDefault().OperationalDate);
            Assert.AreEqual(movements.FirstOrDefault().SapTrackingId, result.FirstOrDefault().SapTrackingId);
        }

        [TestMethod]
        public async Task BalanceProcessor_FinalizeProcessAsync_WhenInvokedAsync()
        {
            this.mockOperationalCutOffFinalizer.Setup(m => m.ProcessAsync(It.IsAny<int>()));

            // Act
            await this.balanceProcessor.FinalizeProcessAsync(1).ConfigureAwait(false);

            // Assert
            this.mockOperationalCutOffFinalizer.Verify(m => m.ProcessAsync(It.IsAny<int>()), Times.Exactly(1));
        }

        [TestMethod]
        public async Task BalanceProcessor_GetSapTrackingErrorsAsyncAsync_ShouldGetSapTrackingErrorsAsync()
        {
            var repoMock = new Mock<IRepository<SapTrackingError>>();
            this.unitOfWorkMock.Setup(x => x.CreateRepository<SapTrackingError>()).Returns(repoMock.Object);
            repoMock.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<SapTrackingError, bool>>>())).ReturnsAsync(new List<SapTrackingError> { new SapTrackingError() });

            await this.balanceProcessor.GetSapTrackingErrorsAsync(It.IsAny<int>()).ConfigureAwait(false);
            repoMock.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<SapTrackingError, bool>>>()), Times.Once);
        }

        [TestMethod]
        public async Task BalanceProcessor_GetFirstTimeNodesAsync_ShouldReturnNodesAsync()
        {
            var ticketLocal = new Ticket
            {
                StartDate = DateTime.UtcNow.AddDays(-2),
                EndDate = DateTime.UtcNow,
                CategoryElementId = 10,
            };

            var parameters = new Dictionary<string, object>
            {
                { "@SegmentId", ticketLocal.CategoryElementId },
                { "@StartDate", ticketLocal.StartDate },
                { "@EndDate", ticketLocal.EndDate },
            };

            IEnumerable<InitialNode> nodes = new List<InitialNode>
            {
                new InitialNode
                {
                    NodeId = 1,
                },
            };

            var initialNodeRepoMock = new Mock<IRepository<InitialNode>>();
            initialNodeRepoMock.Setup(m => m.ExecuteQueryAsync(Repositories.Constants.GetFirstTimeNodesProcedureName, parameters)).Returns(Task.FromResult(nodes)).Verifiable();
            this.unitOfWorkMock.Setup(x => x.CreateRepository<InitialNode>()).Returns(initialNodeRepoMock.Object);
            var result = await this.balanceProcessor.GetFirstTimeNodesAsync(ticketLocal).ConfigureAwait(false);
            initialNodeRepoMock.Verify(m => m.ExecuteQueryAsync(Repositories.Constants.GetFirstTimeNodesProcedureName, parameters), Times.Once);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(1, result.FirstOrDefault());
        }

        /// <summary>
        /// Delete Segment Balance.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DeleteSegmentBalanceAsync_ShouldDeleteSegmentBalanceAsync()
        {
            // Arrange
            var movements = new List<SegmentUnbalance> { new SegmentUnbalance { SegmentUnbalanceId = 12321 } };
            var repoMovement = new Mock<IRepository<SegmentUnbalance>>();
            this.unitOfWorkMock.Setup(f => f.CreateRepository<SegmentUnbalance>()).Returns(repoMovement.Object);
            this.unitOfWorkMock.Setup(f => f.SaveAsync(It.IsAny<CancellationToken>()));
            repoMovement.Setup(o => o.DeleteAll(It.IsAny<List<SegmentUnbalance>>()));
            repoMovement.Setup(o => o.GetAllAsync(It.IsAny<Expression<Func<SegmentUnbalance, bool>>>())).ReturnsAsync(movements);

            // Act
            int ticketId = 1234;
            await this.balanceProcessor.DeleteBalanceAsync<SegmentUnbalance>(ticketId).ConfigureAwait(false);

            // Assert
            repoMovement.Verify(o => o.DeleteAll(It.IsAny<List<SegmentUnbalance>>()), Times.Once);
        }

        /// <summary>
        /// Delete System Balance.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DeleteSystemBalanceAsync_ShouldDeleteSegmentBalanceAsync()
        {
            // Arrange
            var movements = new List<SystemUnbalance> { new SystemUnbalance { SystemUnbalanceId = 12321 } };
            var repoMovement = new Mock<IRepository<SystemUnbalance>>();
            this.unitOfWorkMock.Setup(f => f.CreateRepository<SystemUnbalance>()).Returns(repoMovement.Object);
            this.unitOfWorkMock.Setup(f => f.SaveAsync(It.IsAny<CancellationToken>()));
            repoMovement.Setup(o => o.DeleteAll(It.IsAny<List<SystemUnbalance>>()));
            repoMovement.Setup(o => o.GetAllAsync(It.IsAny<Expression<Func<SystemUnbalance, bool>>>())).ReturnsAsync(movements);

            // Act
            int ticketId = 1234;
            await this.balanceProcessor.DeleteBalanceAsync<SystemUnbalance>(ticketId).ConfigureAwait(false);

            // Assert
            repoMovement.Verify(o => o.DeleteAll(It.IsAny<List<SystemUnbalance>>()), Times.Once);
        }
    }
}
