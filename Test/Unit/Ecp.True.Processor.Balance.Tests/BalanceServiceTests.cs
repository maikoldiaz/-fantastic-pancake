// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BalanceServiceTests.cs" company="Microsoft">
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
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Balance;
    using Ecp.True.Processors.Balance.Calculation;
    using Ecp.True.Processors.Balance.Calculation.Input;
    using Ecp.True.Processors.Balance.Calculation.Interfaces;
    using Ecp.True.Processors.Balance.Calculation.Output;
    using Ecp.True.Processors.Balance.Operation.Input;
    using Ecp.True.Processors.Balance.Operation.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class BalanceServiceTests
    {
        /// <summary>
        /// The unit of work mock.
        /// </summary>
        private Mock<IUnitOfWorkFactory> unitOfWorkFactoryMock;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<IUnitOfWork> unitOfWorkMock;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger> loggerMock;

        /// <summary>
        /// The mock balance service.
        /// </summary>
        private BalanceService balanceService;

        /// <summary>
        /// The calculator mock.
        /// </summary>
        private Mock<IInterfaceCalculator> interfaceCalculatorMock;

        /// <summary>
        /// The calculator mock.
        /// </summary>
        private Mock<IBalanceToleranceCalculator> balanceToleranceCalculatorMock;

        /// <summary>
        /// The calculator mock.
        /// </summary>
        private Mock<IUnidentifiedLossCalculator> unidentifiedLossCalculatorMock;

        /// <summary>
        /// The calculator mock.
        /// </summary>
        private Mock<IUnbalanceCalculator> unbalanceCalculatorMock;

        /// <summary>
        /// The calculator mock.
        /// </summary>
        private Mock<ICalculationService> calculationServiceMock;

        /// <summary>
        /// The balance service mock.
        /// </summary>
        private List<ICalculationService> calculationServices;

        /// <summary>
        /// The Interface Movement Generator mock.
        /// </summary>
        private Mock<IInterfaceMovementGenerator> interfaceMovementGenMock;

        /// <summary>
        /// The balance tolerance Movement Generator mock.
        /// </summary>
        private Mock<IBalanceToleranceMovementGenerator> balanceToleranceMovementGenMock;

        /// <summary>
        /// The unidentified loss Movement Generator mock.
        /// </summary>
        private Mock<IUnidentifiedLossMovementGenerator> unidentifiedLossMovementGenMock;

        /// <summary>
        /// The unbalance repository mock.
        /// </summary>
        private Mock<IRepositoryFactory> mockRepositoryFactory;

        /// <summary>
        /// The unbalance repository mock.
        /// </summary>
        private Mock<IRepository<NodeInput>> mockNodeInputRepository;

        /// <summary>
        /// The unbalance repository mock.
        /// </summary>
        private Mock<IRepository<MovementCalculationInput>> mockMovementCalculationInputRepository;

        /// <summary>
        /// The unbalance repository mock.
        /// </summary>
        private Mock<IRepository<InventoryInput>> mockInventoryInputRepository;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.unitOfWorkMock = new Mock<IUnitOfWork>();
            this.unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
            this.loggerMock = new Mock<ITrueLogger>();
            this.mockRepositoryFactory = new Mock<IRepositoryFactory>();
            this.calculationServiceMock = new Mock<ICalculationService>();
            this.calculationServices = new List<ICalculationService> { this.calculationServiceMock.Object };
            this.interfaceCalculatorMock = new Mock<IInterfaceCalculator>();
            this.balanceToleranceCalculatorMock = new Mock<IBalanceToleranceCalculator>();
            this.unidentifiedLossCalculatorMock = new Mock<IUnidentifiedLossCalculator>();
            this.unidentifiedLossCalculatorMock.Setup(m => m.CalculateAsync(It.IsAny<CalculationInput>()))
                .ReturnsAsync(new List<UnIdentifiedLossInfo>
                {
                    new UnIdentifiedLossInfo
                    {
                        NodeId = 1,
                    },
                });
            this.unbalanceCalculatorMock = new Mock<IUnbalanceCalculator>();
            this.interfaceMovementGenMock = new Mock<IInterfaceMovementGenerator>();
            this.balanceToleranceMovementGenMock = new Mock<IBalanceToleranceMovementGenerator>();
            this.unidentifiedLossMovementGenMock = new Mock<IUnidentifiedLossMovementGenerator>();
            this.unidentifiedLossMovementGenMock.Setup(m => m.GenerateAsync(It.IsAny<MovementInput>()))
                .ReturnsAsync(new List<Movement>
                {
                    new Movement
                    {
                        MovementId = "5",
                    },
                });
            this.calculationServices.Add(new UnidentifiedLossCalculationService(this.unidentifiedLossCalculatorMock.Object, this.unidentifiedLossMovementGenMock.Object));
            this.balanceService = new BalanceService(this.calculationServices, this.mockRepositoryFactory.Object);
            this.mockNodeInputRepository = new Mock<IRepository<NodeInput>>();
            this.mockMovementCalculationInputRepository = new Mock<IRepository<MovementCalculationInput>>();
            this.mockInventoryInputRepository = new Mock<IRepository<InventoryInput>>();
            this.mockRepositoryFactory.Setup(s => s.CreateRepository<NodeInput>()).Returns(this.mockNodeInputRepository.Object);
            this.mockRepositoryFactory.Setup(s => s.CreateRepository<MovementCalculationInput>()).Returns(this.mockMovementCalculationInputRepository.Object);
            this.mockRepositoryFactory.Setup(s => s.CreateRepository<InventoryInput>()).Returns(this.mockInventoryInputRepository.Object);
            this.unitOfWorkFactoryMock.Setup(x => x.GetUnitOfWork()).Returns(this.unitOfWorkMock.Object);
            this.unitOfWorkMock.Setup(s => s.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
        }

        [TestMethod]
        public async Task ProcessStepAsyncShouldReturnCalculationOutputForInterfaceAsync()
        {
            var unbalances = new UnbalanceRequest { Ticket = new Ticket { TicketId = 1, CategoryElementId = 2, StartDate = new DateTime(2019, 02, 12), EndDate = new DateTime(2019, 02, 16), Status = 0, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null }, TransferPoints = new List<TransferPoints>() };
            var interfaceInfo = new List<InterfaceInfo>();
            var interface1 = new InterfaceInfo();
            interface1.NodeId = 1;
            interface1.Unbalance = 1.0m;
            interfaceInfo.Add(interface1);
            var calculationOutput = new CalculationOutput();

            calculationOutput.Movements.ToList().Add(new Movement());
            calculationOutput.Unbalances.ToList().Add(new UnbalanceComment());

            this.calculationServiceMock.Setup(x => x.Type).Returns(MovementCalculationStep.Interface);
            this.calculationServiceMock.Setup(x => x.ProcessAsync(It.IsAny<BalanceInputInfo>(), It.IsAny<Ticket>(), It.IsAny<IUnitOfWork>())).Returns(Task.FromResult(calculationOutput));
            this.calculationServices.Add(this.calculationServiceMock.Object);
            this.unbalanceCalculatorMock.Setup(x => x.CalculateAsync(It.IsAny<CalculationInput>())).Returns(Task.FromResult(new List<UnbalanceComment> { new UnbalanceComment() }.AsEnumerable()));
            this.interfaceCalculatorMock.Setup(x => x.CalculateAsync(It.IsAny<CalculationInput>())).Returns(Task.FromResult<IEnumerable<IOutput>>(interfaceInfo));
            this.interfaceMovementGenMock.Setup(x => x.GenerateAsync(It.IsAny<MovementInput>())).Returns(Task.FromResult(new List<Movement> { new Movement() }.AsEnumerable()));
            var nodes = new List<NodeInput>();
            var movements = new List<MovementCalculationInput>();
            var inventories = new List<InventoryInput>();
            nodes.Add(new NodeInput
            {
                NodeId = 1,
                Name = "Test 1",
                AcceptableBalancePercentage = 10,
                CalculationDate = DateTime.Now,
                ControlLimit = 1,
            });

            movements.Add(new MovementCalculationInput
            {
                MovementId = "1",
                MeasurementUnit = "9",
                DestinationNodeId = 1,
                DestinationProductId = "1",
                DestinationProductName = "Test 2",
                MessageTypeId = 1,
                NetStandardVolume = 10,
                SourceNodeId = 2,
                SourceProductId = "2",
                SourceProductName = "Test 3",
                OperationalDate = DateTime.Now,
                UncertaintyPercentage = 9,
            });

            inventories.Add(new InventoryInput
            {
                NodeId = 1,
                ProductId = "1",
                MeasurementUnit = "Bbl",
                ProductName = "Test 4",
                ProductVolume = 8,
                UncertaintyPercentage = 9,
                InventoryId = "1",
                InventoryDate = DateTime.Now,
            });

            var parameters = new Dictionary<string, object>
            {
                { "@catElementId", unbalances.Ticket.CategoryElementId },
                { "@startDate", unbalances.Ticket.StartDate.Date },
                { "@endDate", unbalances.Ticket.EndDate.Date },
            };

            this.mockNodeInputRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), parameters)).ReturnsAsync(nodes);
            this.mockMovementCalculationInputRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(movements);
            this.mockInventoryInputRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(inventories);
            var result = await this.balanceService.ProcessStepAsync(unbalances, MovementCalculationStep.Interface, this.unitOfWorkMock.Object, this.loggerMock.Object).ConfigureAwait(false);
            Assert.IsNotNull(result.Movements);
            Assert.IsNotNull(result.Unbalances);
        }

        [TestMethod]
        public async Task ProcessStepAsyncShouldReturnCalculationOutputForBalanceToleranceAsync()
        {
            var unbalances = new UnbalanceRequest { Ticket = new Ticket { TicketId = 1, CategoryElementId = 2, StartDate = new DateTime(2019, 02, 12), EndDate = new DateTime(2019, 02, 16), Status = 0, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null }, TransferPoints = new List<TransferPoints>() };
            var balanceTolerances = new List<BalanceTolerance>();
            var balanceTolerance = new BalanceTolerance();
            balanceTolerance.NodeId = 1;
            balanceTolerance.Unbalance = 1.0m;
            balanceTolerances.Add(balanceTolerance);
            var calculationOutput = new CalculationOutput();

            calculationOutput.Movements.ToList().Add(new Movement());
            calculationOutput.Unbalances.ToList().Add(new UnbalanceComment());

            this.calculationServiceMock.Setup(x => x.Type).Returns(MovementCalculationStep.BalanceTolerances);
            this.calculationServiceMock.Setup(x => x.ProcessAsync(It.IsAny<BalanceInputInfo>(), It.IsAny<Ticket>(), It.IsAny<IUnitOfWork>())).Returns(Task.FromResult(calculationOutput));
            this.calculationServices.Add(this.calculationServiceMock.Object);
            this.unbalanceCalculatorMock.Setup(x => x.CalculateAsync(It.IsAny<CalculationInput>())).Returns(Task.FromResult(new List<UnbalanceComment> { new UnbalanceComment() }.AsEnumerable()));
            this.balanceToleranceCalculatorMock.Setup(x => x.CalculateAsync(It.IsAny<CalculationInput>())).Returns(Task.FromResult<IEnumerable<IOutput>>(balanceTolerances));
            this.balanceToleranceMovementGenMock.Setup(x => x.GenerateAsync(It.IsAny<MovementInput>())).Returns(Task.FromResult(new List<Movement> { new Movement() }.AsEnumerable()));
            var nodes = new List<NodeInput>();
            var movements = new List<MovementCalculationInput>();
            var inventories = new List<InventoryInput>();
            nodes.Add(new NodeInput
            {
                NodeId = 1,
                Name = "Test 1",
                AcceptableBalancePercentage = 10,
                CalculationDate = DateTime.Now,
                ControlLimit = 1,
            });

            movements.Add(new MovementCalculationInput
            {
                MovementId = "1",
                MeasurementUnit = "9",
                DestinationNodeId = 1,
                DestinationProductId = "1",
                DestinationProductName = "Test 2",
                MessageTypeId = 1,
                NetStandardVolume = 10,
                SourceNodeId = 2,
                SourceProductId = "2",
                SourceProductName = "Test 3",
                OperationalDate = DateTime.Now,
                UncertaintyPercentage = 9,
            });

            inventories.Add(new InventoryInput
            {
                NodeId = 1,
                ProductId = "1",
                MeasurementUnit = "Bbl",
                ProductName = "Test 4",
                ProductVolume = 8,
                UncertaintyPercentage = 9,
                InventoryId = "1",
                InventoryDate = DateTime.Now,
            });

            var parameters = new Dictionary<string, object>
            {
                { "@catElementId", unbalances.Ticket.CategoryElementId },
                { "@startDate", unbalances.Ticket.StartDate.Date },
                { "@endDate", unbalances.Ticket.EndDate.Date },
            };

            this.mockNodeInputRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), parameters)).ReturnsAsync(nodes);
            this.mockMovementCalculationInputRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(movements);
            this.mockInventoryInputRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(inventories);
            var result = await this.balanceService.ProcessStepAsync(unbalances, MovementCalculationStep.BalanceTolerances, this.unitOfWorkMock.Object, this.loggerMock.Object).ConfigureAwait(false);
            Assert.IsNotNull(result.Movements);
            Assert.IsNotNull(result.Unbalances);
        }

        [TestMethod]
        public async Task ProcessStepAsyncShouldReturnCalculationOutputForUnidentifiedLossesAsync()
        {
            var unbalances = new UnbalanceRequest { Ticket = new Ticket { TicketId = 1, CategoryElementId = 2, StartDate = new DateTime(2019, 02, 12), EndDate = new DateTime(2019, 02, 16), Status = 0, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null }, TransferPoints = new List<TransferPoints>() };
            var unidentifiedLoss = new List<UnIdentifiedLossInfo>();
            var unidentifiedLossInfo = new UnIdentifiedLossInfo();
            unidentifiedLossInfo.NodeId = 1;
            unidentifiedLossInfo.Unbalance = 1.0m;
            unidentifiedLoss.Add(unidentifiedLossInfo);
            var calculationOutput = new CalculationOutput();

            calculationOutput.Movements.ToList().Add(new Movement());
            calculationOutput.Unbalances.ToList().Add(new UnbalanceComment());

            this.calculationServiceMock.Setup(x => x.Type).Returns(MovementCalculationStep.UnidentifiedLosses);
            this.calculationServiceMock.Setup(x => x.ProcessAsync(It.IsAny<BalanceInputInfo>(), It.IsAny<Ticket>(), It.IsAny<IUnitOfWork>())).Returns(Task.FromResult(calculationOutput));
            this.calculationServices.Add(this.calculationServiceMock.Object);
            this.unbalanceCalculatorMock.Setup(x => x.CalculateAsync(It.IsAny<CalculationInput>())).Returns(Task.FromResult(new List<UnbalanceComment> { new UnbalanceComment() }.AsEnumerable()));
            this.balanceToleranceCalculatorMock.Setup(x => x.CalculateAsync(It.IsAny<CalculationInput>())).Returns(Task.FromResult<IEnumerable<IOutput>>(unidentifiedLoss));
            this.balanceToleranceMovementGenMock.Setup(x => x.GenerateAsync(It.IsAny<MovementInput>())).Returns(Task.FromResult(new List<Movement> { new Movement() }.AsEnumerable()));
            var nodes = new List<NodeInput>();
            var movements = new List<MovementCalculationInput>();
            var inventories = new List<InventoryInput>();
            nodes.Add(new NodeInput
            {
                NodeId = 1,
                Name = "Test 1",
                AcceptableBalancePercentage = 10,
                CalculationDate = DateTime.Now,
                ControlLimit = 1,
            });

            movements.Add(new MovementCalculationInput
            {
                MovementId = "1",
                MeasurementUnit = "9",
                DestinationNodeId = 1,
                DestinationProductId = "1",
                DestinationProductName = "Test 2",
                MessageTypeId = 1,
                NetStandardVolume = 10,
                SourceNodeId = 2,
                SourceProductId = "2",
                SourceProductName = "Test 3",
                OperationalDate = DateTime.Now,
                UncertaintyPercentage = 9,
            });

            inventories.Add(new InventoryInput
            {
                NodeId = 1,
                ProductId = "1",
                MeasurementUnit = "Bbl",
                ProductName = "Test 4",
                ProductVolume = 8,
                UncertaintyPercentage = 9,
                InventoryId = "1",
                InventoryDate = DateTime.Now,
            });

            var parameters = new Dictionary<string, object>
            {
                { "@catElementId", unbalances.Ticket.CategoryElementId },
                { "@startDate", unbalances.Ticket.StartDate.Date },
                { "@endDate", unbalances.Ticket.EndDate.Date },
            };

            this.mockNodeInputRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), parameters)).ReturnsAsync(nodes);
            this.mockMovementCalculationInputRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(movements);
            this.mockInventoryInputRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(inventories);
            var result = await this.balanceService.ProcessStepAsync(unbalances, MovementCalculationStep.UnidentifiedLosses, this.unitOfWorkMock.Object, this.loggerMock.Object).ConfigureAwait(false);
            Assert.IsNotNull(result.Movements);
            Assert.IsNotNull(result.Unbalances);
        }

        [TestMethod]
        public async Task ProcessCalculationAsync_ShouldProcessCalculations_ForStepPassedAsync()
        {
            var currentDate = DateTime.UtcNow.Date;
            var ticketStartDate = currentDate.AddDays(-2).Date;
            var balanceInput = new BalanceInput
            {
                Nodes = new List<NodeInput>
                {
                    new NodeInput
                    {
                        NodeId = 1,
                        CalculationDate = ticketStartDate,
                    },
                    new NodeInput
                    {
                        NodeId = 2,
                        CalculationDate = ticketStartDate,
                    },
                },
                Inventories = new List<InventoryInput>
                {
                    new InventoryInput
                    {
                        InventoryId = "1",
                        InventoryDate = ticketStartDate,
                    },
                },
                Movements = new List<MovementCalculationInput>
                {
                    new MovementCalculationInput
                    {
                        MovementId = "5",
                    },
                },
            };

            var ticket = new Ticket
            {
                TicketId = 4,
                StartDate = ticketStartDate,
                EndDate = currentDate,
            };
            var calculationDate = ticket.StartDate.Date;
            var balanceInputInfo = new BalanceInputInfo
            {
                CalculationDate = calculationDate,
                Nodes = balanceInput.Nodes.Where(x => x.CalculationDate == calculationDate),
                InterfaceNodes = balanceInput.InterfaceNodes.Where(x => x.CalculationDate == calculationDate),
                InitialInventories = balanceInput.Inventories.Where(x => x.InventoryDate.Date == calculationDate.AddDays(-1).Date),
                FinalInventories = balanceInput.Inventories.Where(x => x.InventoryDate.Date == calculationDate),
                Movements = balanceInput.Movements.Where(x => x.OperationalDate.Date == calculationDate),
            };
            var movements = new List<Movement>
            {
                new Movement
                {
                    MovementId = "5",
                    TicketId = 4,
                },
                new Movement
                {
                    MovementId = "6",
                    TicketId = 5,
                },
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
            var calculationOutput = new CalculationOutput(movements, unbalances);

            this.calculationServiceMock.Setup(m => m.ProcessAsync(balanceInputInfo, ticket, this.unitOfWorkMock.Object)).ReturnsAsync(calculationOutput).Verifiable();
            var result = await this.balanceService.ProcessCalculationAsync(balanceInput, ticket, this.unitOfWorkMock.Object, MovementCalculationStep.UnidentifiedLosses).ConfigureAwait(false);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Movements.Count());
            Assert.AreEqual(2, result.UnbalanceList.Count());
        }
    }
}
