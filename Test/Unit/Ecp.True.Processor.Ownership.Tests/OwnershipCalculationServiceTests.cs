// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipCalculationServiceTests.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Conciliation.Interfaces;
    using Ecp.True.Processors.Ownership;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Services.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The Ownership Calculation Service Tests class.
    /// </summary>
    [TestClass]
    public class OwnershipCalculationServiceTests
    {
        /// <summary>
        /// The mock repository factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockRepositoryFactory;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The ownership calculation service.
        /// </summary>
        private OwnershipCalculationService ownershipCalculationService;

        /// <summary>
        /// The mock inventory operational data repository.
        /// </summary>
        private Mock<IRepository<InventoryOperationalData>> mockInventoryOperationalDataRepository;

        /// <summary>
        /// The mock movement operational data repository.
        /// </summary>
        private Mock<IRepository<MovementOperationalData>> mockMovementOperationalDataRepository;

        /// <summary>
        /// The mock node configuration repository.
        /// </summary>
        private Mock<IRepository<NodeConfiguration>> mockNodeConfigurationRepository;

        /// <summary>
        /// The mock node connection repository.
        /// </summary>
        private Mock<IRepository<True.Entities.Query.NodeConnection>> mockNodeConnectionRepository;

        /// <summary>
        /// The mock previous movement operational data repository.
        /// </summary>
        private Mock<IRepository<PreviousMovementOperationalData>> mockPreviousMovementOperationalDataRepository;

        /// <summary>
        /// The mock previous inventory operational data repository.
        /// </summary>
        private Mock<IRepository<PreviousInventoryOperationalData>> mockPreviousInventoryOperationalDataRepository;

        /// <summary>
        /// The mock ticket repository.
        /// </summary>
        private Mock<IRepository<Ticket>> mockTicketRepository;

        /// <summary>
        /// The mock ownership node repository.
        /// </summary>
        private Mock<IRepository<OwnershipNode>> mockOwnershipNodeRepository;

        /// <summary>
        /// The mock event repository.
        /// </summary>
        private Mock<IRepository<True.Entities.Query.Event>> mockEventRepository;

        /// <summary>
        /// The mock contract repository.
        /// </summary>
        private Mock<IRepository<Contract>> mockContractRepository;

        /// <summary>
        /// The mock contract repository.
        /// </summary>
        private Mock<IRepository<GenericLogisticsMovement>> mockGenericLogisticsMovement;

        /// <summary>
        /// The mock transferPointConciliationMovement repository.
        /// </summary>
        private Mock<IRepository<TransferPointConciliationMovement>> mockTransferPointConciliationMovement;

        /// <summary>
        /// The mock conciliationNodesResult repository.
        /// </summary>
        private Mock<IRepository<ConciliationNodesResult>> mockConciliationNodesResult;

        /// <summary>
        /// The mock contract repository.
        /// </summary>
        private Mock<IRepository<LogisticsInventoryDetail>> mockLogisticsInventoryDetail;

        /// <summary>
        /// The mock cancellation movement repository.
        /// </summary>
        private Mock<IRepository<CancellationMovementDetail>> mockCancellationMovementRepository;

        /// <summary>
        /// The mock logistics detail repository.
        /// </summary>
        private Mock<IRepository<LogisticsMovementDetail>> mockLogisticsMovementDetailRepository;

        /// <summary>
        /// The mock logistics inventory detail repository.
        /// </summary>
        private Mock<IRepository<LogisticsInventoryDetail>> mockLogisticsInventoryDetailRepository;

        /// <summary>
        /// the mock ownership processor.
        /// </summary>
        private Mock<IConciliationProcessor> mockOwnershipConciliation;

        /// <summary>
        /// The mock logistic service.
        /// </summary>
        private Mock<ILogisticsService> mockLogisticService;

        /// <summary>
        /// Intilizes the method.
        /// </summary>
        [TestInitialize]
        public void IntilizeMethod()
        {
            this.mockRepositoryFactory = new Mock<IRepositoryFactory>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockInventoryOperationalDataRepository = new Mock<IRepository<InventoryOperationalData>>();
            this.mockMovementOperationalDataRepository = new Mock<IRepository<MovementOperationalData>>();
            this.mockNodeConfigurationRepository = new Mock<IRepository<NodeConfiguration>>();
            this.mockNodeConnectionRepository = new Mock<IRepository<True.Entities.Query.NodeConnection>>();
            this.mockPreviousMovementOperationalDataRepository = new Mock<IRepository<PreviousMovementOperationalData>>();
            this.mockPreviousInventoryOperationalDataRepository = new Mock<IRepository<PreviousInventoryOperationalData>>();
            this.mockLogisticsMovementDetailRepository = new Mock<IRepository<LogisticsMovementDetail>>();
            this.mockLogisticsInventoryDetailRepository = new Mock<IRepository<LogisticsInventoryDetail>>();
            this.mockTicketRepository = new Mock<IRepository<Ticket>>();
            this.mockContractRepository = new Mock<IRepository<Contract>>();
            this.mockCancellationMovementRepository = new Mock<IRepository<CancellationMovementDetail>>();
            this.mockOwnershipNodeRepository = new Mock<IRepository<OwnershipNode>>();
            this.mockTicketRepository.Setup(s => s.Update(It.IsAny<Ticket>()));
            this.mockEventRepository = new Mock<IRepository<True.Entities.Query.Event>>();
            this.mockLogisticService = new Mock<ILogisticsService>();
            this.mockGenericLogisticsMovement = new Mock<IRepository<GenericLogisticsMovement>>();
            this.mockLogisticsInventoryDetail = new Mock<IRepository<LogisticsInventoryDetail>>();
            this.mockTransferPointConciliationMovement = new Mock<IRepository<TransferPointConciliationMovement>>();
            this.mockConciliationNodesResult = new Mock<IRepository<ConciliationNodesResult>>();
            this.mockTicketRepository.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Ticket()
            {
                CategoryElementId = 1,
                TicketTypeId = Entities.Enumeration.TicketType.Ownership,
                CreatedBy = "system",
                CreatedDate = DateTime.Now,
                EndDate = DateTime.Now,
                ErrorMessage = string.Empty,
                LastModifiedBy = "system",
                LastModifiedDate = DateTime.Now,
                StartDate = DateTime.Now,
                Status = StatusType.PROCESSING,
                TicketId = 0,
            });

            this.mockRepositoryFactory.Setup(s => s.CreateRepository<InventoryOperationalData>()).Returns(this.mockInventoryOperationalDataRepository.Object);
            this.mockRepositoryFactory.Setup(s => s.CreateRepository<MovementOperationalData>()).Returns(this.mockMovementOperationalDataRepository.Object);
            this.mockRepositoryFactory.Setup(s => s.CreateRepository<NodeConfiguration>()).Returns(this.mockNodeConfigurationRepository.Object);
            this.mockRepositoryFactory.Setup(s => s.CreateRepository<True.Entities.Query.NodeConnection>()).Returns(this.mockNodeConnectionRepository.Object);
            this.mockRepositoryFactory.Setup(s => s.CreateRepository<PreviousMovementOperationalData>()).Returns(this.mockPreviousMovementOperationalDataRepository.Object);
            this.mockRepositoryFactory.Setup(s => s.CreateRepository<PreviousInventoryOperationalData>()).Returns(this.mockPreviousInventoryOperationalDataRepository.Object);
            this.mockRepositoryFactory.Setup(s => s.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);
            this.mockRepositoryFactory.Setup(s => s.CreateRepository<Contract>()).Returns(this.mockContractRepository.Object);
            this.mockRepositoryFactory.Setup(s => s.CreateRepository<CancellationMovementDetail>()).Returns(this.mockCancellationMovementRepository.Object);
            this.mockRepositoryFactory.Setup(s => s.CreateRepository<OwnershipNode>()).Returns(this.mockOwnershipNodeRepository.Object);
            this.mockRepositoryFactory.Setup(s => s.CreateRepository<True.Entities.Query.Event>()).Returns(this.mockEventRepository.Object);
            this.mockRepositoryFactory.Setup(s => s.CreateRepository<LogisticsMovementDetail>()).Returns(this.mockLogisticsMovementDetailRepository.Object);
            this.mockRepositoryFactory.Setup(s => s.CreateRepository<LogisticsInventoryDetail>()).Returns(this.mockLogisticsInventoryDetailRepository.Object);
            this.mockOwnershipConciliation = new Mock<IConciliationProcessor>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockUnitOfWorkFactory.Setup(s => s.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);

            this.mockUnitOfWork.Setup(s => s.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
            this.mockOwnershipNodeRepository.Setup(s => s.ExecuteAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()));

            this.ownershipCalculationService = new OwnershipCalculationService(
                    this.mockUnitOfWorkFactory.Object,
                    this.mockRepositoryFactory.Object,
                    this.mockLogisticService.Object,
                    this.mockOwnershipConciliation.Object);
        }

        /// <summary>
        /// Updates the segment node details test asynchronous.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task UpdateSegmentNodeDetails_TestAsync()
        {
            await this.ownershipCalculationService.AddOwnershipNodesAsync(new List<int> { 1 }, 1).ConfigureAwait(false);
            this.mockOwnershipNodeRepository.Verify(m => m.ExecuteAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Gets the ownership data test asynchronous.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task GetOwnershipData_TestAsync()
        {
            var inventoryOperationalData = new List<InventoryOperationalData>();
            var movementsOperationalData = new List<MovementOperationalData>();
            var nodeConfigurations = new List<NodeConfiguration>();
            var nodeConnections = new List<True.Entities.Query.NodeConnection>();
            var previousInventoryOperationalData = new List<PreviousInventoryOperationalData>();
            var previousMovementsOperationalData = new List<PreviousMovementOperationalData>();
            var cancellationMovements = new List<True.Entities.Query.CancellationMovementDetail>();
            var contractData = new List<True.Entities.Query.Contract>();
            var eventData = new List<True.Entities.Query.Event>();
            var date = DateTime.Now;
            var ticket = new Ticket { TicketId = 1, CategoryElementId = 11, EndDate = date, StartDate = date };
            var transferPoint = new List<TransferPointConciliationMovement>
            {
                new TransferPointConciliationMovement
                {
                    OwnerId = 45,
                    SegmentId = 1,
                    DestinationNodeId = 2,
                    SourceNodeId = 3,
                    DestinationProductId = "3",
                    SourceProductId = "242",
                    OwnershipVolume = 22,
                    OwnershipValueUnit = "brl",
                    MovementId = "0",
                    OperationalDate = date,
                },
            };

            var ownershipRuleData = new OwnershipRuleData() { TicketId = 1 };

            for (int j = 0; j < 10; j++)
            {
                inventoryOperationalData.Add(new InventoryOperationalData()
                {
                    InventoryId = j,
                    NetVolume = 10045 + j,
                    NodeId = 20 + j,
                    OperationalDate = DateTime.Now.Date,
                    OwnerId = 66 + j,
                    OwnershipUnit = "brl",
                    OwnershipValue = 77 + j,
                    ProductId = (33 + j).ToString(CultureInfo.InvariantCulture),
                    Ticket = 99 + j,
                });

                movementsOperationalData.Add(new MovementOperationalData()
                {
                    DestinationNodeId = 2 + j,
                    DestinationProductId = (3 + j).ToString(CultureInfo.InvariantCulture),
                    MovementTransactionId = j,
                    MovementTypeId = (1 + j).ToString(CultureInfo.InvariantCulture),
                    NetVolume = 100337 + j,
                    OwnerId = 45 + j,
                    OwnershipUnit = "brl",
                    OwnershipValue = 22 + j,
                    SourceNodeId = 3 + j,
                    SourceProductId = (242 + j).ToString(CultureInfo.InvariantCulture),
                    Ticket = 99 + j,
                    OperationalDate = date,
                });

                nodeConfigurations.Add(new NodeConfiguration()
                {
                    NodeId = 99 + j,
                    NodeOrder = 2 + j,
                    OwnerId = 664 + j,
                    ProductId = (36 + j).ToString(CultureInfo.InvariantCulture),
                });

                nodeConnections.Add(new True.Entities.Query.NodeConnection()
                {
                    DestinationNodeId = 55 + j,
                    Prioritization = 4 + j,
                    ProductId = (9 + j).ToString(CultureInfo.InvariantCulture),
                    SourceNodeId = 3 + j,
                });

                previousInventoryOperationalData.Add(new PreviousInventoryOperationalData()
                {
                    InventoryId = j,
                    NodeId = 64 + j,
                    OwnerId = 30 + j,
                    OwnershipVolume = 33 + j,
                    ProductId = (11 + j).ToString(CultureInfo.InvariantCulture),
                });

                previousMovementsOperationalData.Add(new PreviousMovementOperationalData()
                {
                    MovementId = j,
                    OwnerId = 93 + j,
                    OwnershipVolume = 59 + j,
                });

                eventData.Add(new True.Entities.Query.Event()
                {
                    EventIdentifier = j,
                    SourceNodeId = 3 + j,
                    DestinationNodeId = 2 + j,
                    SourceProductId = (242 + j).ToString(CultureInfo.InvariantCulture),
                    DestinationProductId = (3 + j).ToString(CultureInfo.InvariantCulture),
                });
            }

            this.mockInventoryOperationalDataRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(inventoryOperationalData);
            this.mockMovementOperationalDataRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(movementsOperationalData);
            this.mockNodeConfigurationRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(nodeConfigurations);
            this.mockNodeConnectionRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(nodeConnections);
            this.mockPreviousMovementOperationalDataRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(previousMovementsOperationalData);
            this.mockPreviousInventoryOperationalDataRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(previousInventoryOperationalData);
            this.mockCancellationMovementRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(cancellationMovements);
            this.mockContractRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(contractData);
            this.mockEventRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(eventData);
            this.mockUnitOfWork.Setup(s => s.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);
            this.mockTicketRepository.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ticket);
            this.mockUnitOfWork.Setup(s => s.CreateRepository<TransferPointConciliationMovement>()).Returns(this.mockTransferPointConciliationMovement.Object);
            this.mockOwnershipConciliation.Setup(s => s.GetConciliationTransferPointMovementsAsync(It.IsAny<ConnectionConciliationNodesResquest>())).ReturnsAsync(transferPoint);
            await this.ownershipCalculationService.PopulateOwnershipRuleRequestDataAsync(ownershipRuleData).ConfigureAwait(false);
            var result = ownershipRuleData.OwnershipRuleRequest;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.InventoryOperationalData.ToList().Count, inventoryOperationalData.Count);
            Assert.AreEqual(result.MovementsOperationalData.ToList().Count, movementsOperationalData.Count);
            Assert.AreEqual(result.NodeConfigurations.ToList().Count, nodeConfigurations.Count);
            Assert.AreEqual(result.NodeConnections.ToList().Count, nodeConnections.Count);
            Assert.AreEqual(result.PreviousMovementsOperationalData.ToList().Count, previousMovementsOperationalData.Count);
            Assert.AreEqual(result.PreviousInventoryOperationalData.ToList().Count, previousInventoryOperationalData.Count);
            Assert.AreEqual(result.Events.ToList().Count, eventData.Count);
        }

        [TestMethod]
        public async Task VerifyEvacuationMovementsTransformation_WhenPopulatingOwnershipRuleRequestTestAsync()
        {
            var inventoryOperationalData = new List<InventoryOperationalData>();
            var movementsOperationalData = new List<MovementOperationalData>();
            var nodeConfigurations = new List<NodeConfiguration>();
            var nodeConnections = new List<True.Entities.Query.NodeConnection>();
            var previousInventoryOperationalData = new List<PreviousInventoryOperationalData>();
            var previousMovementsOperationalData = new List<PreviousMovementOperationalData>();
            var cancellationMovements = new List<True.Entities.Query.CancellationMovementDetail>();
            var contractData = new List<True.Entities.Query.Contract>();
            var eventData = new List<True.Entities.Query.Event>();
            var ownershipRuleData = new OwnershipRuleData() { TicketId = 1 };

            cancellationMovements.Add(
                new CancellationMovementDetail()
                {
                    MovementTransactionId = 1001,
                    DestinationNodeId = 1234,
                    DestinationProductId = "1111",
                    MovementTypeId = (int)MovementType.InputEvacuation,
                    MovementType = MovementType.InputEvacuation.ToString(),
                });

            cancellationMovements.Add(
                new CancellationMovementDetail()
                {
                    MovementTransactionId = 1002,
                    SourceNodeId = 1234,
                    SourceProductId = "1111",
                    MovementTypeId = (int)MovementType.OutputEvacuation,
                    MovementType = MovementType.OutputEvacuation.ToString(),
                });

            this.mockInventoryOperationalDataRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(inventoryOperationalData);
            this.mockMovementOperationalDataRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(movementsOperationalData);
            this.mockNodeConfigurationRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(nodeConfigurations);
            this.mockNodeConnectionRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(nodeConnections);
            this.mockPreviousMovementOperationalDataRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(previousMovementsOperationalData);
            this.mockPreviousInventoryOperationalDataRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(previousInventoryOperationalData);
            this.mockCancellationMovementRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(cancellationMovements);
            this.mockContractRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(contractData);
            this.mockEventRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(eventData);
            this.mockUnitOfWork.Setup(s => s.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);

            await this.ownershipCalculationService.PopulateOwnershipRuleRequestDataAsync(ownershipRuleData).ConfigureAwait(false);

            foreach (var movement in ownershipRuleData.CancellationMovements)
            {
                if (movement.MovementTypeId == (int)MovementType.InputCancellation)
                {
                    Assert.AreEqual(1234, movement.SourceNodeId);
                    Assert.AreEqual("1111", movement.SourceProductId);
                    Assert.AreEqual(MovementType.InputCancellation.ToString(), movement.MovementType);
                    Assert.IsNull(movement.DestinationNodeId);
                    Assert.IsNull(movement.DestinationProductId);
                }

                if (movement.MovementTypeId == (int)MovementType.OutputCancellation)
                {
                    Assert.AreEqual(1234, movement.DestinationNodeId);
                    Assert.AreEqual("1111", movement.DestinationProductId);
                    Assert.AreEqual(MovementType.OutputCancellation.ToString(), movement.MovementType);
                    Assert.IsNull(movement.SourceNodeId);
                    Assert.IsNull(movement.SourceProductId);
                }
            }
        }

        [TestMethod]
        public async Task VerifyMovementOperationalDataForCancellationMovements_WhenPopulatingOwnershipRuleRequestTestAsync()
        {
            var inventoryOperationalData = new List<InventoryOperationalData>();
            var movementsOperationalData = new List<MovementOperationalData>();
            var nodeConfigurations = new List<NodeConfiguration>();
            var nodeConnections = new List<True.Entities.Query.NodeConnection>();
            var previousInventoryOperationalData = new List<PreviousInventoryOperationalData>();
            var previousMovementsOperationalData = new List<PreviousMovementOperationalData>();
            var cancellationMovements = new List<True.Entities.Query.CancellationMovementDetail>();
            var contractData = new List<True.Entities.Query.Contract>();
            var eventData = new List<True.Entities.Query.Event>();
            var ownershipRuleData = new OwnershipRuleData() { TicketId = 1 };
            var movement = new CancellationMovementDetail()
            {
                MovementTransactionId = 1001,
                DestinationNodeId = 1234,
                DestinationProductId = "1111",
                MovementTypeId = (int)MovementType.InputEvacuation,
                MovementType = MovementType.InputEvacuation.ToString(),
            };

            cancellationMovements.Add(movement);
            this.mockUnitOfWork.Setup(s => s.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);
            this.mockInventoryOperationalDataRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(inventoryOperationalData);
            this.mockMovementOperationalDataRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(movementsOperationalData);
            this.mockNodeConfigurationRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(nodeConfigurations);
            this.mockNodeConnectionRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(nodeConnections);
            this.mockPreviousMovementOperationalDataRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(previousMovementsOperationalData);
            this.mockPreviousInventoryOperationalDataRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(previousInventoryOperationalData);
            this.mockCancellationMovementRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(cancellationMovements);
            this.mockContractRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(contractData);
            this.mockEventRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(eventData);

            await this.ownershipCalculationService.PopulateOwnershipRuleRequestDataAsync(ownershipRuleData).ConfigureAwait(false);

            Assert.IsTrue(ownershipRuleData.OwnershipRuleRequest.MovementsOperationalData.Any());

            var movementData = ownershipRuleData.OwnershipRuleRequest.MovementsOperationalData.First();
            Assert.AreEqual(movement.SourceNodeId, movementData.SourceNodeId);
            Assert.AreEqual(movement.SourceProductId, movementData.SourceProductId);
            Assert.AreEqual(movement.DestinationNodeId, movementData.DestinationNodeId);
            Assert.AreEqual(movement.SourceProductId, movementData.SourceProductId);
            Assert.AreEqual(movement.MovementTransactionId, movementData.MovementTransactionId);
        }

        /// <summary>
        /// Updates the ticket errors test asynchronous.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task UpdateTicketErrors_TestAsync()
        {
            this.mockUnitOfWork.Setup(s => s.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);
            await this.ownershipCalculationService.UpdateTicketErrorsAsync(1, "error message").ConfigureAwait(false);
            this.mockTicketRepository.Verify(m => m.Update(It.IsAny<Ticket>()), Times.Once);
        }

        /// <summary>
        /// Gets the logistics details asynchronous test asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetLogisticsDetailsNoMovementsAsync_TestAsync()
        {
            var logisticsMovementDetailList = new List<LogisticsMovementDetail>();
            var logisticsInventoryDetailList = new List<LogisticsInventoryDetail>();

            for (int j = 0; j < 10; j++)
            {
                logisticsMovementDetailList.Add(new LogisticsMovementDetail
                {
                    Movement = $"Movement-{j}",
                    StorageSource = $"StorageSource-{j}",
                    ProductOrigin = $"ProductOrigin-{j}",
                    StorageDestination = $"StorageDestination-{j}",
                    ProductDestination = $"ProductDestination-{j}",
                    OrderPurchase = 1 * j,
                    PosPurchase = 1 * j,
                    Value = 10 * j,
                    Uom = $"UON-{j}",
                    Status = $"Status-{j}",
                    Order = $"Order-{j}",
                    DateOperation = Convert.ToDateTime("02/25/2020", CultureInfo.InvariantCulture),
                });
            }

            for (int j = 0; j < 10; j++)
            {
                logisticsInventoryDetailList.Add(new LogisticsInventoryDetail
                {
                    Inventory = $"Movement-{j}",
                    StorageLocation = $"StorageSource-{j}",
                    Product = $"ProductOrigin-{j}",
                    Value = 10 * j,
                    Uom = $"UON-{j}",
                    Status = $"Status-{j}",
                    Order = $"Order-{j}",
                    DateOperation = Convert.ToDateTime("02/25/2020", CultureInfo.InvariantCulture),
                });
            }

            var ticketDetails = new Ticket
            {
                TicketId = 12,
                OwnerId = 123,
                Owner = new CategoryElement { Name = "Ecopetrol" },
                CategoryElementId = 10,
                CategoryElement = new CategoryElement { Name = "Segment" },
                TicketTypeId = Entities.Enumeration.TicketType.Logistics,
                CreatedBy = "system",
                CreatedDate = DateTime.Now,
                EndDate = DateTime.Now,
                ErrorMessage = string.Empty,
                LastModifiedBy = "system",
                LastModifiedDate = DateTime.Now,
                StartDate = DateTime.Now,
                Status = StatusType.PROCESSING,
            };
            this.mockUnitOfWork.Setup(s => s.CreateRepository<LogisticsInventoryDetail>()).Returns(this.mockLogisticsInventoryDetail.Object);
            this.mockUnitOfWork.Setup(s => s.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);
            this.mockLogisticsInventoryDetail.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()));
            this.mockGenericLogisticsMovement.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()));
            this.mockUnitOfWork.Setup(s => s.CreateRepository<GenericLogisticsMovement>()).Returns(this.mockGenericLogisticsMovement.Object);
            this.mockTicketRepository.Setup(s => s.FirstOrDefaultAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(ticketDetails);
            this.mockLogisticsMovementDetailRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(logisticsMovementDetailList);
            this.mockLogisticsInventoryDetailRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(logisticsInventoryDetailList);
            this.mockTicketRepository.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ticketDetails);
            await this.ownershipCalculationService.GetLogisticsDetailsAsync(ticketDetails.TicketId, (int)SystemType.SIV).ConfigureAwait(false);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<LogisticsInventoryDetail>(), Times.AtLeastOnce);
        }

        /// <summary>
        /// Gets the logistics Siv details asynchronous test asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetLogisticsDetailsSivAsync_TestAsync()
        {
            var logisticsMovementDetailList = new List<LogisticsMovementDetail>();
            var logisticsInventoryDetailList = new List<LogisticsInventoryDetail>();

            for (int j = 0; j < 10; j++)
            {
                logisticsMovementDetailList.Add(new LogisticsMovementDetail
                {
                    Movement = $"Movement-{j}",
                    StorageSource = $"StorageSource-{j}",
                    ProductOrigin = $"ProductOrigin-{j}",
                    StorageDestination = $"StorageDestination-{j}",
                    ProductDestination = $"ProductDestination-{j}",
                    OrderPurchase = 1 * j,
                    PosPurchase = 1 * j,
                    Value = 10 * j,
                    Uom = $"UON-{j}",
                    Status = $"Status-{j}",
                    Order = $"Order-{j}",
                    DateOperation = Convert.ToDateTime("02/25/2020", CultureInfo.InvariantCulture),
                });
            }

            for (int j = 0; j < 10; j++)
            {
                logisticsInventoryDetailList.Add(new LogisticsInventoryDetail
                {
                    Inventory = $"Movement-{j}",
                    StorageLocation = $"StorageSource-{j}",
                    Product = $"ProductOrigin-{j}",
                    Value = 10 * j,
                    Uom = $"UON-{j}",
                    Status = $"Status-{j}",
                    Order = $"Order-{j}",
                    DateOperation = Convert.ToDateTime("02/25/2020", CultureInfo.InvariantCulture),
                });
            }

            var ticketDetails = new Ticket
            {
                TicketId = 12,
                OwnerId = 123,
                Owner = new CategoryElement { Name = "Ecopetrol" },
                CategoryElementId = 10,
                CategoryElement = new CategoryElement { Name = "Segment" },
                TicketTypeId = Entities.Enumeration.TicketType.Logistics,
                CreatedBy = "system",
                CreatedDate = DateTime.Now,
                EndDate = DateTime.Now,
                ErrorMessage = string.Empty,
                LastModifiedBy = "system",
                LastModifiedDate = DateTime.Now,
                StartDate = DateTime.Now,
                Status = StatusType.PROCESSING,
            };

            var movements = new List<GenericLogisticsMovement>
            {
                new GenericLogisticsMovement()
                {
                    HasAnnulation = true,
                    MovementTypeId = 1,
                    SourceNodeId = 1,
                    DestinationNodeId = 2,
                    DestinationProductId = "2",
                    MeasurementUnit = 31,
                    Status = StatusType.VISUALIZATION,
                    NodeApproved = true,
                },
            };
            this.mockLogisticService.Setup(a => a.TransformAsync(It.IsAny<List<GenericLogisticsMovement>>(), It.IsAny<Ticket>(), It.IsAny<SystemType>(), It.IsAny<ScenarioType>())).ReturnsAsync(movements);
            this.mockUnitOfWork.Setup(s => s.CreateRepository<LogisticsInventoryDetail>()).Returns(this.mockLogisticsInventoryDetail.Object);
            this.mockUnitOfWork.Setup(s => s.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);
            this.mockLogisticsInventoryDetail.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()));
            this.mockGenericLogisticsMovement.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(movements);
            this.mockUnitOfWork.Setup(s => s.CreateRepository<GenericLogisticsMovement>()).Returns(this.mockGenericLogisticsMovement.Object);
            this.mockTicketRepository.Setup(s => s.FirstOrDefaultAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(ticketDetails);
            this.mockLogisticsMovementDetailRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(logisticsMovementDetailList);
            this.mockLogisticsInventoryDetailRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(logisticsInventoryDetailList);
            this.mockTicketRepository.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ticketDetails);
            await this.ownershipCalculationService.GetLogisticsDetailsAsync(ticketDetails.TicketId, (int)SystemType.SIV).ConfigureAwait(false);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<LogisticsInventoryDetail>(), Times.AtLeastOnce);
        }

        /// <summary>
        /// Gets the logistics Sap details asynchronous test asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetLogisticsDetailsSapAsync_TestAsync()
        {
            var logisticsMovementDetailList = new List<LogisticsMovementDetail>();
            var logisticsInventoryDetailList = new List<LogisticsInventoryDetail>();

            for (int j = 0; j < 10; j++)
            {
                logisticsMovementDetailList.Add(new LogisticsMovementDetail
                {
                    Movement = $"Movement-{j}",
                    StorageSource = $"StorageSource-{j}",
                    ProductOrigin = $"ProductOrigin-{j}",
                    StorageDestination = $"StorageDestination-{j}",
                    ProductDestination = $"ProductDestination-{j}",
                    OrderPurchase = 1 * j,
                    PosPurchase = 1 * j,
                    Value = 10 * j,
                    Uom = $"UON-{j}",
                    Status = $"Status-{j}",
                    Order = $"Order-{j}",
                    DateOperation = Convert.ToDateTime("02/25/2020", CultureInfo.InvariantCulture),
                });
            }

            for (int j = 0; j < 10; j++)
            {
                logisticsInventoryDetailList.Add(new LogisticsInventoryDetail
                {
                    Inventory = $"Movement-{j}",
                    StorageLocation = $"StorageSource-{j}",
                    Product = $"ProductOrigin-{j}",
                    Value = 10 * j,
                    Uom = $"UON-{j}",
                    Status = $"Status-{j}",
                    Order = $"Order-{j}",
                    DateOperation = Convert.ToDateTime("02/25/2020", CultureInfo.InvariantCulture),
                });
            }

            var ticketDetails = new Ticket
            {
                TicketId = 12,
                OwnerId = 123,
                Owner = new CategoryElement { Name = "Ecopetrol" },
                CategoryElementId = 10,
                CategoryElement = new CategoryElement { Name = "Segment" },
                TicketTypeId = Entities.Enumeration.TicketType.Logistics,
                CreatedBy = "system",
                CreatedDate = DateTime.Now,
                EndDate = DateTime.Now,
                ErrorMessage = string.Empty,
                LastModifiedBy = "system",
                LastModifiedDate = DateTime.Now,
                StartDate = DateTime.Now,
                Status = StatusType.PROCESSING,
            };

            var movements = new List<GenericLogisticsMovement>
            {
                new GenericLogisticsMovement()
                {
                    HasAnnulation = true,
                    MovementTypeId = 1,
                    SourceNodeId = 1,
                    DestinationNodeId = 2,
                    DestinationProductId = "2",
                    MeasurementUnit = 31,
                    ErrorMessage = "Error",
                    Status = StatusType.EMPTY,
                },
            };
            this.mockLogisticService.Setup(a => a.TransformAsync(It.IsAny<List<GenericLogisticsMovement>>(),It.IsAny<Ticket>(),It.IsAny<SystemType>(),It.IsAny<ScenarioType>())).ReturnsAsync(movements);
            this.mockUnitOfWork.Setup(s => s.CreateRepository<LogisticsInventoryDetail>()).Returns(this.mockLogisticsInventoryDetail.Object);
            this.mockUnitOfWork.Setup(s => s.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);
            this.mockLogisticsInventoryDetail.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()));
            this.mockGenericLogisticsMovement.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(movements);
            this.mockUnitOfWork.Setup(s => s.CreateRepository<GenericLogisticsMovement>()).Returns(this.mockGenericLogisticsMovement.Object);
            this.mockTicketRepository.Setup(s => s.FirstOrDefaultAsync(It.IsAny<Expression<Func<Ticket, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(ticketDetails);
            this.mockLogisticsMovementDetailRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(logisticsMovementDetailList);
            this.mockLogisticsInventoryDetailRepository.Setup(s => s.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(logisticsInventoryDetailList);
            this.mockTicketRepository.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ticketDetails);
            await this.ownershipCalculationService.GetLogisticsDetailsAsync(ticketDetails.TicketId, (int)SystemType.SAP).ConfigureAwait(false);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<LogisticsInventoryDetail>(), Times.AtLeastOnce);
        }

        /// <summary>
        /// Updates the ticket errors test asynchronous.
        /// </summary>
        /// <returns>Returns a completed task.</returns>
        [TestMethod]
        public async Task UpdateTicketStatusAndBlobpathAsync_TestAsync()
        {
            var ticketDetails = new Ticket
            {
                TicketId = 12,
                OwnerId = 123,
                Owner = new CategoryElement { Name = "Ecopetrol" },
                CategoryElementId = 10,
                CategoryElement = new CategoryElement { Name = "Segment" },
                TicketTypeId = Entities.Enumeration.TicketType.Logistics,
                CreatedBy = "system",
                CreatedDate = DateTime.Now,
                EndDate = DateTime.Now,
                ErrorMessage = string.Empty,
                LastModifiedBy = "system",
                LastModifiedDate = DateTime.Now,
                StartDate = DateTime.Now,
                Status = StatusType.PROCESSING,
            };
            this.mockUnitOfWork.Setup(s => s.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);
            this.mockTicketRepository.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ticketDetails);
            this.mockUnitOfWork.Setup(s => s.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
            await this.ownershipCalculationService.UpdateTicketStatusAndBlobpathAsync(ticketDetails.TicketId, SystemType.SIV).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(s => s.CreateRepository<Ticket>(), Times.Once);
            this.mockTicketRepository.Verify(s => s.GetByIdAsync(It.IsAny<int>()), Times.Once);
            this.mockTicketRepository.Verify(s => s.Update(It.IsAny<Ticket>()), Times.Once);
            this.mockUnitOfWork.Verify(s => s.SaveAsync(It.IsAny<CancellationToken>()));
        }
    }
}
