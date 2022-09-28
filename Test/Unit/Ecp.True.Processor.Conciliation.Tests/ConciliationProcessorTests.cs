// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConciliationProcessorTests.cs" company="Microsoft">
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
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Conciliation;
    using Ecp.True.Processors.Conciliation.Entities;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Proxies.Azure;
    using EfCore.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The OwnershipConciliationTests class.
    /// </summary>
    [TestClass]
    public class ConciliationProcessorTests
    {
        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

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
        /// The mock mockOwnershipNodeRepository.
        /// </summary>
        private Mock<IRepository<OwnershipNode>> mockOwnershipNodeRepository;

        /// <summary>
        /// The mock mockOwnershipNodeDataRepository.
        /// </summary>
        private Mock<IRepository<OwnershipNodeData>> mockOwnershipNodeDataRepository;

        /// <summary>
        /// The mock mockMovementRepository.
        /// </summary>
        private Mock<IRepository<Movement>> mockMovementRepository;

        /// <summary>
        /// The mock mockTicketRepository.
        /// </summary>
        private Mock<IRepository<Ticket>> mockTicketRepository;

        /// <summary>
        /// The mock mockTicketRepository.
        /// </summary>
        private Mock<IRepository<SegmentNodeDto>> mockSegmentNodeDtoRepository;

        /// <summary>
        /// The mock repoInventoryMovementIndex.
        /// </summary>
        private Mock<IRepository<InventoryMovementIndex>> mockInventoryMovementRepository;

        /// <summary>
        /// The ownership calculation service.
        /// </summary>
        private ConciliationProcessor ownershipConciliation;

        /// <summary>
        /// The mock service bus queue client.
        /// </summary>
        private Mock<IServiceBusQueueClient> mockServiceBusQueueClient;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<ConciliationProcessor>> mockLogger;

        /// <summary>
        /// The mock finalizer.
        /// </summary>
        private Mock<IFinalizer> mockFinalizer;

        /// <summary>
        /// The mock finalizer factory.
        /// </summary>
        private Mock<IFinalizerFactory> mockFinalizerFactory;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private Mock<ITelemetry> telemetryMock;

        /// <summary>
        /// Intilizes the method.
        /// </summary>
        [TestInitialize]
        public void IntilizeMethod()
        {
            var repoOwnershipNodeError = new Mock<IRepository<OwnershipNodeError>>();
            this.mockRepositoryFactory = new Mock<IRepositoryFactory>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockMovementRepository = new Mock<IRepository<Movement>>();
            this.mockOwnershipNodeRepository = new Mock<IRepository<OwnershipNode>>();
            this.mockSegmentNodeDtoRepository = new Mock<IRepository<SegmentNodeDto>>();
            this.mockInventoryMovementRepository = new Mock<IRepository<InventoryMovementIndex>>();
            this.mockTicketRepository = new Mock<IRepository<Ticket>>();
            this.mockOwnershipNodeDataRepository = new Mock<IRepository<OwnershipNodeData>>();
            this.mockFinalizer = new Mock<IFinalizer>();
            this.mockFinalizerFactory = new Mock<IFinalizerFactory>();
            this.mockServiceBusQueueClient = new Mock<IServiceBusQueueClient>();
            this.mockLogger = new Mock<ITrueLogger<ConciliationProcessor>>();
            this.mockAzureClientFactory.Setup(x => x.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);
            this.mockRepositoryFactory.Setup(s => s.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);
            this.mockRepositoryFactory.Setup(s => s.CreateRepository<OwnershipNode>()).Returns(this.mockOwnershipNodeRepository.Object);
            this.mockRepositoryFactory.Setup(s => s.CreateRepository<SegmentNodeDto>()).Returns(this.mockSegmentNodeDtoRepository.Object);
            this.mockRepositoryFactory.Setup(s => s.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);
            this.mockRepositoryFactory.Setup(s => s.CreateRepository<OwnershipNodeData>()).Returns(this.mockOwnershipNodeDataRepository.Object);
            this.mockRepositoryFactory.Setup(s => s.CreateRepository<InventoryMovementIndex>()).Returns(this.mockInventoryMovementRepository.Object);
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockUnitOfWork.Setup(s => s.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
            this.mockUnitOfWorkFactory.Setup(s => s.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<OwnershipNodeError>()).Returns(repoOwnershipNodeError.Object);
            this.mockOwnershipNodeDataRepository.Setup(o => o.ExecuteViewAsync()).ReturnsAsync(new List<OwnershipNodeData>().AsQueryable());
            this.mockFinalizerFactory.Setup(f => f.GetFinalizer(It.IsAny<FinalizerType>())).Returns(this.mockFinalizer.Object);
            this.telemetryMock = new Mock<ITelemetry>();
            this.ownershipConciliation = new ConciliationProcessor(
                this.mockUnitOfWorkFactory.Object,
                this.mockAzureClientFactory.Object,
                this.mockFinalizerFactory.Object,
                this.mockLogger.Object,
                this.mockRepositoryFactory.Object,
                this.telemetryMock.Object);
        }

        /// <summary>
        /// Test of Get Conciliation TransferPoints.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetConciliationTransferPointsAsync_ShouldBeExecuteQueryAsync()
        {
            var conciliationNodes = new[] { new TransferPointConciliationMovement() };
            var repoMock = new Mock<IRepository<TransferPointConciliationMovement>>();
            this.mockUnitOfWork.Setup(m => m.CreateRepository<TransferPointConciliationMovement>()).Returns(repoMock.Object);
            repoMock.Setup(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(conciliationNodes);
            await this.ownershipConciliation.GetConciliationTransferPointMovementsAsync(new ConnectionConciliationNodesResquest() { EndDate = DateTime.Today, StartDate = DateTime.Now, ConciliationNodes = new List<ConciliationNodesResult> { new ConciliationNodesResult() { SourceNodeId = 1, DestinationNodeId = 2 } } }).ConfigureAwait(false);
            this.mockUnitOfWork.Verify(m => m.CreateRepository<TransferPointConciliationMovement>(), Times.Once);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// the Calculate Conciliation ErrorMovementsAsync.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CalculateConciliation_ErrorMovementsAsync()
        {
            var ticket = new Ticket { TicketId = 1, Owner = new CategoryElement { Name = "owner" }, CategoryElement = new CategoryElement { Name = "element" } };
            var movementError = new MovementConciliationDto
            {
                DeltaConciliated = 19,
                Description = "Description",
                DestinationNodeId = 1,
                MovementTransactionId = 1,
                OwnerId = 1,
                SourceNodeId = 1,
                DestinationProductId = "1",
                SourceProductId = "1",
                MovementTypeId = 1,
                SegmentId = 1,
                OwnershipVolume = 1,
                OwnershipPercentage = 1,
                NetStandardVolume = 1,
                Sign = string.Empty,
                MeasurementUnit = 1,
            };

            List<MovementConciliationDto> noConciledMovements = new List<MovementConciliationDto>();
            List<MovementConciliationDto> conciledMovements = new List<MovementConciliationDto>();
            MovementConciliations movementConciliations = new MovementConciliations(
                conciledMovements,
                noConciledMovements,
                new List<MovementConciliationDto> { movementError });
            var otherSegmentMovements = new List<MovementConciliationDto> { movementError };
            var repoMockSegmentNode = new Mock<IRepository<SegmentNodeDto>>();
            var repoOwnerShipNode = new Mock<IRepository<OwnershipNode>>();
            var nodeForSegment = new List<SegmentNodeDto> { new SegmentNodeDto() { SegmentId = 1, NodeId = 1 } };
            var ownershipNode = new OwnershipNode() { NodeId = 1, TicketId = 1 };
            repoOwnerShipNode.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>())).ReturnsAsync(ownershipNode);
            repoOwnerShipNode.Setup(r => r.UpdateAll(It.IsAny<IEnumerable<OwnershipNode>>()));
            repoOwnerShipNode.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>())).ReturnsAsync(new List<OwnershipNode> { ownershipNode });
            repoMockSegmentNode.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<SegmentNodeDto, bool>>>())).ReturnsAsync(nodeForSegment);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<SegmentNodeDto>()).Returns(repoMockSegmentNode.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<OwnershipNode>()).Returns(repoOwnerShipNode.Object);
            repoMockSegmentNode.Setup(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(nodeForSegment);
            var repoMockTicket = new Mock<IRepository<Ticket>>();
            repoMockTicket.Setup(r => r.GetByIdAsync(ticket.TicketId)).ReturnsAsync(ticket);
            repoMockTicket.Setup(r => r.Update(It.IsAny<Ticket>()));
            this.mockUnitOfWork.Setup(u => u.CreateRepository<Ticket>()).Returns(repoMockTicket.Object);
            await this.ownershipConciliation.CalculateConciliationAsync(movementConciliations, ticket, otherSegmentMovements).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(m => m.CreateRepository<SegmentNodeDto>(), Times.AtLeastOnce);
            repoMockSegmentNode.Verify(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockUnitOfWork.Verify(u => u.CreateRepository<Ticket>(), Times.Once);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(CancellationToken.None), Times.AtLeastOnce);
        }

        /// <summary>
        /// the Calculate Conciliation ResultMovementsAsync.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task CalculateConciliation_ResultMovementsAsync()
        {
            var ticket = new Ticket { TicketId = 1, Owner = new CategoryElement { Name = "owner" }, CategoryElement = new CategoryElement { Name = "element" } };
            var resultMovement = new MovementConciliationDto
            {
                DeltaConciliated = 19,
                Description = "Description",
                DestinationNodeId = 1,
                MovementTransactionId = 1,
                OwnerId = 1,
                SourceNodeId = 1,
                DestinationProductId = "1",
                SourceProductId = "1",
                MovementTypeId = 1,
                SegmentId = 1,
                OwnershipVolume = 1,
                OwnershipPercentage = 1,
                NetStandardVolume = 1,
                Sign = "POSITIVO",
                MeasurementUnit = 1,
            };

            List<MovementConciliationDto> noConciledMovements = new List<MovementConciliationDto>();
            List<MovementConciliationDto> errorMovements = new List<MovementConciliationDto>();
            MovementConciliations movementConciliations = new MovementConciliations(
                new List<MovementConciliationDto> { resultMovement },
                noConciledMovements,
                errorMovements);
            var otherSegmentMovements = new List<MovementConciliationDto> { resultMovement };
            var repoNodeTag = new Mock<IRepository<NodeTag>>();
            var repoMockSegmentNode = new Mock<IRepository<SegmentNodeDto>>();
            var repoOwnerShipNode = new Mock<IRepository<OwnershipNode>>();
            var repoMovement = new Mock<IRepository<Movement>>();
            var repoMockMovementConciliation = new Mock<IRepository<MovementConciliation>>();
            var repoInventoryMovementIndex = new Mock<IRepository<InventoryMovementIndex>>();
            var nodeForSegment = new List<SegmentNodeDto> { new SegmentNodeDto() { SegmentId = 1, NodeId = 1 } };
            var nodeTag = new NodeTag { NodeId = 1 };
            var ownershipNode = new OwnershipNode() { NodeId = 1, TicketId = 1 };
            repoMovement.Setup(r => r.InsertAll(It.IsAny<IEnumerable<Movement>>()));
            repoOwnerShipNode.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>())).ReturnsAsync(ownershipNode);
            repoOwnerShipNode.Setup(r => r.UpdateAll(It.IsAny<IEnumerable<OwnershipNode>>()));
            repoMockSegmentNode.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<SegmentNodeDto, bool>>>())).ReturnsAsync(nodeForSegment);
            repoNodeTag.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<NodeTag, bool>>>())).ReturnsAsync(nodeTag);
            repoInventoryMovementIndex.Setup(r => r.Insert(It.IsAny<InventoryMovementIndex>()));
            this.mockUnitOfWork.Setup(m => m.CreateRepository<SegmentNodeDto>()).Returns(repoMockSegmentNode.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<OwnershipNode>()).Returns(repoOwnerShipNode.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<MovementConciliation>()).Returns(repoMockMovementConciliation.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Movement>()).Returns(repoMovement.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<NodeTag>()).Returns(repoNodeTag.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<InventoryMovementIndex>()).Returns(repoInventoryMovementIndex.Object);
            repoMockSegmentNode.Setup(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(nodeForSegment);
            var repoMockTicket = new Mock<IRepository<Ticket>>();
            repoMockTicket.Setup(r => r.GetByIdAsync(ticket.TicketId)).ReturnsAsync(ticket);
            repoMockTicket.Setup(r => r.Update(It.IsAny<Ticket>()));
            this.mockUnitOfWork.Setup(u => u.CreateRepository<Ticket>()).Returns(repoMockTicket.Object);
            await this.ownershipConciliation.CalculateConciliationAsync(movementConciliations, ticket, otherSegmentMovements).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(m => m.CreateRepository<SegmentNodeDto>(), Times.AtLeastOnce);
            repoMockSegmentNode.Verify(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockUnitOfWork.Verify(u => u.CreateRepository<Ticket>(), Times.Once);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(CancellationToken.None), Times.AtLeastOnce);
            repoMovement.Verify(m => m.InsertAll(It.IsAny<IEnumerable<Movement>>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// the calculate conciliation Result Movements Async.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task CalculateConciliation_ResultMovements_DestinationNodeAsync()
        {
            var ticket = new Ticket { TicketId = 1, Owner = new CategoryElement { Name = "owner" }, CategoryElement = new CategoryElement { Name = "element" } };
            var resultMovement = new MovementConciliationDto
            {
                DeltaConciliated = 19,
                Description = "Description",
                DestinationNodeId = 1,
                MovementTransactionId = 1,
                OwnerId = 1,
                SourceNodeId = 2,
                DestinationProductId = "1",
                SourceProductId = "1",
                MovementTypeId = 1,
                SegmentId = 1,
                OwnershipVolume = 1,
                OwnershipPercentage = 1,
                NetStandardVolume = 1,
                Sign = "POSITIVO",
                MeasurementUnit = 1,
            };

            List<MovementConciliationDto> noConciledMovements = new List<MovementConciliationDto>();
            List<MovementConciliationDto> errorMovements = new List<MovementConciliationDto>();
            MovementConciliations movementConciliations = new MovementConciliations(
                new List<MovementConciliationDto> { resultMovement },
                noConciledMovements,
                errorMovements);
            var otherSegmentMovements = new List<MovementConciliationDto> { resultMovement };
            var repoMockSegmentNode = new Mock<IRepository<SegmentNodeDto>>();
            var repoOwnerShipNode = new Mock<IRepository<OwnershipNode>>();
            var repoMovement = new Mock<IRepository<Movement>>();
            var repoMockMovementConciliation = new Mock<IRepository<MovementConciliation>>();
            var repoInventoryMovementIndex = new Mock<IRepository<InventoryMovementIndex>>();
            var nodeForSegment = new List<SegmentNodeDto> { new SegmentNodeDto() { SegmentId = 1, NodeId = 1 } };
            var ownershipNode = new OwnershipNode() { NodeId = 1, TicketId = 1 };
            var repoNodeTag = new Mock<IRepository<NodeTag>>();
            var nodeTag = new NodeTag { NodeId = 1 };
            repoNodeTag.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<NodeTag, bool>>>())).ReturnsAsync(nodeTag);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<NodeTag>()).Returns(repoNodeTag.Object);
            repoMovement.Setup(r => r.InsertAll(It.IsAny<IEnumerable<Movement>>()));
            repoOwnerShipNode.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>())).ReturnsAsync(ownershipNode);
            repoOwnerShipNode.Setup(r => r.UpdateAll(It.IsAny<IEnumerable<OwnershipNode>>()));
            repoMockSegmentNode.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<SegmentNodeDto, bool>>>())).ReturnsAsync(nodeForSegment);
            repoInventoryMovementIndex.Setup(r => r.Insert(It.IsAny<InventoryMovementIndex>()));
            this.mockUnitOfWork.Setup(m => m.CreateRepository<MovementConciliation>()).Returns(repoMockMovementConciliation.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<SegmentNodeDto>()).Returns(repoMockSegmentNode.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<OwnershipNode>()).Returns(repoOwnerShipNode.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Movement>()).Returns(repoMovement.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<InventoryMovementIndex>()).Returns(repoInventoryMovementIndex.Object);
            repoMockSegmentNode.Setup(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(nodeForSegment);
            var repoMockTicket = new Mock<IRepository<Ticket>>();
            repoMockTicket.Setup(r => r.GetByIdAsync(ticket.TicketId)).ReturnsAsync(ticket);
            repoMockTicket.Setup(r => r.Update(It.IsAny<Ticket>()));
            this.mockUnitOfWork.Setup(u => u.CreateRepository<Ticket>()).Returns(repoMockTicket.Object);
            await this.ownershipConciliation.CalculateConciliationAsync(movementConciliations, ticket, otherSegmentMovements).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(m => m.CreateRepository<SegmentNodeDto>(), Times.AtLeastOnce);
            repoMockSegmentNode.Verify(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockUnitOfWork.Verify(u => u.CreateRepository<Ticket>(), Times.Once);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(CancellationToken.None), Times.AtLeastOnce);
            repoMovement.Verify(m => m.InsertAll(It.IsAny<IEnumerable<Movement>>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// the Calculate Conciliation NoConciledMovementsAsync.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task CalculateConciliation_NoConciledMovementsAsync()
        {
            var ticket = new Ticket { TicketId = 1, Owner = new CategoryElement { Name = "owner" }, CategoryElement = new CategoryElement { Name = "element" } };
            var noConciledMovement = new MovementConciliationDto
            {
                DeltaConciliated = 19,
                Description = "Description",
                DestinationNodeId = 1,
                MovementTransactionId = 1,
                OwnerId = 1,
                SourceNodeId = 1,
                DestinationProductId = "1",
                SourceProductId = "1",
                MovementTypeId = 1,
                SegmentId = 1,
                OwnershipVolume = 1,
                OwnershipPercentage = 1,
                NetStandardVolume = 1,
                Sign = string.Empty,
                MeasurementUnit = 1,
            };

            List<MovementConciliationDto> movementError = new List<MovementConciliationDto>();
            List<MovementConciliationDto> conciledMovements = new List<MovementConciliationDto>();
            MovementConciliations movementConciliations = new MovementConciliations(
                conciledMovements,
                new List<MovementConciliationDto> { noConciledMovement },
                movementError);
            var otherSegmentMovements = new List<MovementConciliationDto> { noConciledMovement };
            var repoMockSegmentNode = new Mock<IRepository<SegmentNodeDto>>();
            var repoOwnerShipNode = new Mock<IRepository<OwnershipNode>>();
            var nodeForSegment = new List<SegmentNodeDto> { new SegmentNodeDto() { SegmentId = 1, NodeId = 1 } };
            var ownershipNode = new OwnershipNode() { NodeId = 1, TicketId = 1 };
            repoOwnerShipNode.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>())).ReturnsAsync(ownershipNode);
            repoOwnerShipNode.Setup(r => r.UpdateAll(It.IsAny<IEnumerable<OwnershipNode>>()));
            repoMockSegmentNode.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<SegmentNodeDto, bool>>>())).ReturnsAsync(nodeForSegment);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<SegmentNodeDto>()).Returns(repoMockSegmentNode.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<OwnershipNode>()).Returns(repoOwnerShipNode.Object);
            repoMockSegmentNode.Setup(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(nodeForSegment);
            var repoMockTicket = new Mock<IRepository<Ticket>>();
            repoMockTicket.Setup(r => r.GetByIdAsync(ticket.TicketId)).ReturnsAsync(ticket);
            repoMockTicket.Setup(r => r.Update(It.IsAny<Ticket>()));
            this.mockUnitOfWork.Setup(u => u.CreateRepository<Ticket>()).Returns(repoMockTicket.Object);
            await this.ownershipConciliation.CalculateConciliationAsync(movementConciliations, ticket, otherSegmentMovements).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(m => m.CreateRepository<SegmentNodeDto>(), Times.AtLeastOnce);
            repoMockSegmentNode.Verify(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockUnitOfWork.Verify(u => u.CreateRepository<Ticket>(), Times.Once);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(CancellationToken.None), Times.AtLeastOnce);
        }

        /// <summary>
        /// the Calculate Conciliation ResultMovements and NoConciledMovementsAsync.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task CalculateConciliation_ResultMovements_and_NoConciledMovementsAsync()
        {
            var ticket = new Ticket { TicketId = 1, Owner = new CategoryElement { Name = "owner" }, CategoryElement = new CategoryElement { Name = "element" } };
            var resultMovement = new MovementConciliationDto
            {
                DeltaConciliated = 19,
                Description = "Description",
                DestinationNodeId = 1,
                MovementTransactionId = 1,
                OwnerId = 1,
                SourceNodeId = 1,
                DestinationProductId = "1",
                SourceProductId = "1",
                MovementTypeId = 1,
                SegmentId = 1,
                OwnershipVolume = 1,
                OwnershipPercentage = 1,
                NetStandardVolume = 1,
                Sign = "NEGATIVO",
                MeasurementUnit = 1,
            };

            var noConciledMovement = new MovementConciliationDto
            {
                DeltaConciliated = 19,
                Description = "Description",
                DestinationNodeId = 1,
                MovementTransactionId = 1,
                OwnerId = 1,
                SourceNodeId = 1,
                DestinationProductId = "1",
                SourceProductId = "1",
                MovementTypeId = 1,
                SegmentId = 1,
                OwnershipVolume = 1,
                OwnershipPercentage = 1,
                NetStandardVolume = 1,
                Sign = string.Empty,
                MeasurementUnit = 1,
            };

            List<MovementConciliationDto> errorMovements = new List<MovementConciliationDto>();
            MovementConciliations movementConciliations = new MovementConciliations(
                new List<MovementConciliationDto> { resultMovement },
                new List<MovementConciliationDto> { noConciledMovement },
                errorMovements);
            var otherSegmentMovements = new List<MovementConciliationDto> { resultMovement };
            var repoMockSegmentNode = new Mock<IRepository<SegmentNodeDto>>();
            var repoMockMovementConciliation = new Mock<IRepository<MovementConciliation>>();
            var repoOwnerShipNode = new Mock<IRepository<OwnershipNode>>();
            var repoMovement = new Mock<IRepository<Movement>>();
            var repoInventoryMovementIndex = new Mock<IRepository<InventoryMovementIndex>>();
            var nodeForSegment = new List<SegmentNodeDto> { new SegmentNodeDto() { SegmentId = 1, NodeId = 1 } };
            var ownershipNode = new OwnershipNode() { NodeId = 1, TicketId = 1 };
            var repoNodeTag = new Mock<IRepository<NodeTag>>();
            var nodeTag = new NodeTag { NodeId = 1 };
            repoNodeTag.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<NodeTag, bool>>>())).ReturnsAsync(nodeTag);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<NodeTag>()).Returns(repoNodeTag.Object);
            repoMovement.Setup(r => r.InsertAll(It.IsAny<IEnumerable<Movement>>()));
            repoOwnerShipNode.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>())).ReturnsAsync(ownershipNode);
            repoOwnerShipNode.Setup(r => r.UpdateAll(It.IsAny<IEnumerable<OwnershipNode>>()));
            repoMockSegmentNode.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<SegmentNodeDto, bool>>>())).ReturnsAsync(nodeForSegment);
            repoInventoryMovementIndex.Setup(r => r.Insert(It.IsAny<InventoryMovementIndex>()));
            this.mockUnitOfWork.Setup(m => m.CreateRepository<SegmentNodeDto>()).Returns(repoMockSegmentNode.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<MovementConciliation>()).Returns(repoMockMovementConciliation.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<OwnershipNode>()).Returns(repoOwnerShipNode.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Movement>()).Returns(repoMovement.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<InventoryMovementIndex>()).Returns(repoInventoryMovementIndex.Object);
            repoMockSegmentNode.Setup(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(nodeForSegment);
            var repoMockTicket = new Mock<IRepository<Ticket>>();
            repoMockTicket.Setup(r => r.GetByIdAsync(ticket.TicketId)).ReturnsAsync(ticket);
            repoMockTicket.Setup(r => r.Update(It.IsAny<Ticket>()));
            this.mockUnitOfWork.Setup(u => u.CreateRepository<Ticket>()).Returns(repoMockTicket.Object);
            await this.ownershipConciliation.CalculateConciliationAsync(movementConciliations, ticket, otherSegmentMovements).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(m => m.CreateRepository<SegmentNodeDto>(), Times.AtLeastOnce);
            repoMockSegmentNode.Verify(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockUnitOfWork.Verify(u => u.CreateRepository<Ticket>(), Times.Once);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(CancellationToken.None), Times.AtLeastOnce);
            repoMovement.Verify(m => m.InsertAll(It.IsAny<IEnumerable<Movement>>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// The Calculate Conciliation Result Movements To Sign Equals Async.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task CalculateConciliation_ResultMovements_ToSignEqualsAsync()
        {
            var ticket = new Ticket { TicketId = 1, Owner = new CategoryElement { Name = "owner" }, CategoryElement = new CategoryElement { Name = "element" } };
            var resultMovement = new MovementConciliationDto
            {
                DeltaConciliated = 19,
                Description = "Description",
                DestinationNodeId = 1,
                MovementTransactionId = 1,
                OwnerId = 1,
                SourceNodeId = 1,
                DestinationProductId = "1",
                SourceProductId = "1",
                MovementTypeId = 1,
                SegmentId = 1,
                OwnershipVolume = 1,
                OwnershipPercentage = 1,
                NetStandardVolume = 1,
                Sign = "IGUAL",
                MeasurementUnit = 1,
            };

            List<MovementConciliationDto> noConciledMovements = new List<MovementConciliationDto>();
            List<MovementConciliationDto> errorMovements = new List<MovementConciliationDto>();
            MovementConciliations movementConciliations = new MovementConciliations(
                new List<MovementConciliationDto> { resultMovement },
                noConciledMovements,
                errorMovements);
            var otherSegmentMovements = new List<MovementConciliationDto> { resultMovement };
            var repoMockSegmentNode = new Mock<IRepository<SegmentNodeDto>>();
            var repoOwnerShipNode = new Mock<IRepository<OwnershipNode>>();
            var repoInventoryMovementIndex = new Mock<IRepository<InventoryMovementIndex>>();
            var nodeForSegment = new List<SegmentNodeDto> { new SegmentNodeDto() { SegmentId = 1, NodeId = 1 } };
            var ownershipNode = new OwnershipNode() { NodeId = 1, TicketId = 1 };
            var repoNodeTag = new Mock<IRepository<NodeTag>>();
            var nodeTag = new NodeTag { NodeId = 1 };
            repoNodeTag.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<NodeTag, bool>>>())).ReturnsAsync(nodeTag);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<NodeTag>()).Returns(repoNodeTag.Object);
            repoOwnerShipNode.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>())).ReturnsAsync(ownershipNode);
            repoOwnerShipNode.Setup(r => r.UpdateAll(It.IsAny<IEnumerable<OwnershipNode>>()));
            repoMockSegmentNode.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<SegmentNodeDto, bool>>>())).ReturnsAsync(nodeForSegment);
            repoInventoryMovementIndex.Setup(r => r.Insert(It.IsAny<InventoryMovementIndex>()));
            this.mockUnitOfWork.Setup(m => m.CreateRepository<SegmentNodeDto>()).Returns(repoMockSegmentNode.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<OwnershipNode>()).Returns(repoOwnerShipNode.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<InventoryMovementIndex>()).Returns(repoInventoryMovementIndex.Object);
            repoMockSegmentNode.Setup(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(nodeForSegment);
            var repoMockTicket = new Mock<IRepository<Ticket>>();
            repoMockTicket.Setup(r => r.GetByIdAsync(ticket.TicketId)).ReturnsAsync(ticket);
            repoMockTicket.Setup(r => r.Update(It.IsAny<Ticket>()));
            this.mockUnitOfWork.Setup(u => u.CreateRepository<Ticket>()).Returns(repoMockTicket.Object);
            await this.ownershipConciliation.CalculateConciliationAsync(movementConciliations, ticket, otherSegmentMovements).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(m => m.CreateRepository<SegmentNodeDto>(), Times.AtLeastOnce);
            repoMockSegmentNode.Verify(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            this.mockUnitOfWork.Verify(u => u.CreateRepository<Ticket>(), Times.Once);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(CancellationToken.None), Times.AtLeastOnce);
        }

        /// <summary>
        /// Update Movements and execute Query.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task DoConciliationOwnershipAsync_ShouldBeExecuteQueryAsync()
        {
            // Arrange
            var ownershipTicketId = 1000;
            var conciliationNodes = new[] { new ConciliationNodesResult() };
            var repoMock = new Mock<IRepository<ConciliationNodesResult>>();
            var repoTicket = new Mock<IRepository<Ticket>>();
            var listTransfer = new List<TransferPointConciliationMovement>
            {
                new TransferPointConciliationMovement() { MovementTransactionId = 1, MovementTypeId = 3, SourceNodeId = 3, DestinationNodeId = 4, SegmentId = 5, OwnerId = 5, MeasurementUnit = 1, OwnershipPercentage = 10, NetStandardVolume = 10, OwnershipVolume = 10, DestinationProductId = "5", SourceProductId = "10" },
                new TransferPointConciliationMovement() { MovementTransactionId = 2, MovementTypeId = 3, SourceNodeId = 3, DestinationNodeId = 4, SegmentId = 5, OwnerId = 5, MeasurementUnit = 1, OwnershipPercentage = 10, NetStandardVolume = 10, OwnershipVolume = 10, DestinationProductId = "5", SourceProductId = "10" },
            };
            var listOwnerShipNode = new List<OwnershipNode>
            {
                new OwnershipNode() { TicketId = ownershipTicketId, NodeId = 3 },
                new OwnershipNode() { TicketId = ownershipTicketId, NodeId = 4 },
            };
            var repoMockTransfer = new Mock<IRepository<TransferPointConciliationMovement>>();
            var repoMockSegment = new Mock<IRepository<SegmentNodeDto>>();
            var repoInventoryMovementIndex = new Mock<IRepository<InventoryMovementIndex>>();
            var repoMockOwner = new Mock<IRepository<OwnershipNode>>();
            var repoNodeTag = new Mock<IRepository<NodeTag>>();
            var repoMovement = new Mock<IRepository<Movement>>();
            var repoMockMovementConciliation = new Mock<IRepository<MovementConciliation>>();

            this.mockUnitOfWork.Setup(m => m.CreateRepository<InventoryMovementIndex>()).Returns(repoInventoryMovementIndex.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<SegmentNodeDto>()).Returns(repoMockSegment.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<OwnershipNode>()).Returns(repoMockOwner.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<TransferPointConciliationMovement>()).Returns(repoMockTransfer.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<NodeTag>()).Returns(repoNodeTag.Object);
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<TransferPointConciliationMovement>()).Returns(repoMockTransfer.Object);
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<OwnershipNode>()).Returns(repoMockOwner.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<MovementConciliation>()).Returns(repoMockMovementConciliation.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Movement>()).Returns(repoMovement.Object);
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<ConciliationNodesResult>()).Returns(repoMock.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Ticket>()).Returns(repoTicket.Object);

            repoMockMovementConciliation.Setup(o => o.InsertAll(It.IsAny<List<MovementConciliation>>()));
            repoMockSegment.Setup(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<SegmentNodeDto> { new SegmentNodeDto() { NodeId = 3 }, new SegmentNodeDto() { NodeId = 4 } });
            repoTicket.Setup(a => a.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(new Ticket { TicketId = ownershipTicketId, CreatedDate = DateTime.Today, EndDate = DateTime.Today, CategoryElementId = 1 });
            repoMockOwner.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>(), It.IsAny<string>())).ReturnsAsync(listOwnerShipNode);
            repoMockOwner.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>())).ReturnsAsync(new OwnershipNode { Editor = "TestUser", Node = new Node { Name = "TestNode" }, Ticket = new Ticket { StartDate = DateTime.Parse("1/1/2020", CultureInfo.InvariantCulture) } });
            repoMock.Setup(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(conciliationNodes);
            repoMockTransfer.Setup(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(listTransfer);
            repoNodeTag.Setup(n => n.FirstOrDefaultAsync(It.IsAny<Expression<Func<NodeTag, bool>>>())).ReturnsAsync(new NodeTag());

            // Act
            await this.ownershipConciliation.DoConciliationAsync(new ConciliationNodesResquest() { TicketId = ownershipTicketId, NodeId = 1 }).ConfigureAwait(false);

            // Assert
            repoTicket.Verify(m => m.GetByIdAsync(It.IsAny<object>()), Times.Exactly(2));
            repoMockTransfer.Verify(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Update Movements and execute Query.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task DoConciliationAsync_ShouldBeExecuteQueryAsync()
        {
            // Arrange
            var ownershipTicketId = 1000;
            var conciliationNodes = new[] { new ConciliationNodesResult() };
            var repoMock = new Mock<IRepository<ConciliationNodesResult>>();
            var repoTicket = new Mock<IRepository<Ticket>>();
            var listOwnerShipNode = new List<OwnershipNode>
            {
                new OwnershipNode() { TicketId = ownershipTicketId, NodeId = 3 },
                new OwnershipNode() { TicketId = ownershipTicketId, NodeId = 4 },
            };
            var repoMockTransfer = new Mock<IRepository<TransferPointConciliationMovement>>();
            var repoMockSegment = new Mock<IRepository<SegmentNodeDto>>();
            var repoMockOwner = new Mock<IRepository<OwnershipNode>>();
            var repoNodeTag = new Mock<IRepository<NodeTag>>();
            var repoMockMovementConciliation = new Mock<IRepository<MovementConciliation>>();
            this.mockUnitOfWork.Setup(m => m.CreateRepository<SegmentNodeDto>()).Returns(repoMockSegment.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<OwnershipNode>()).Returns(repoMockOwner.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<TransferPointConciliationMovement>()).Returns(repoMockTransfer.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<NodeTag>()).Returns(repoNodeTag.Object);
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<TransferPointConciliationMovement>()).Returns(repoMockTransfer.Object);
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<OwnershipNode>()).Returns(repoMockOwner.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<MovementConciliation>()).Returns(repoMockMovementConciliation.Object);
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<ConciliationNodesResult>()).Returns(repoMock.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Ticket>()).Returns(repoTicket.Object);
            repoMockSegment.Setup(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<SegmentNodeDto> { new SegmentNodeDto() { NodeId = 3 }, new SegmentNodeDto() { NodeId = 4 } });
            repoTicket.Setup(a => a.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(new Ticket { TicketId = ownershipTicketId, CreatedDate = DateTime.Today, EndDate = DateTime.Today, CategoryElementId = 1 });
            repoMockOwner.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>(), It.IsAny<string>())).ReturnsAsync(listOwnerShipNode);
            repoMockOwner.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>())).ReturnsAsync(new OwnershipNode { Editor = "TestUser", Node = new Node { Name = "TestNode" }, Ticket = new Ticket { StartDate = DateTime.Parse("1/1/2020", CultureInfo.InvariantCulture) } });
            repoMock.Setup(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(conciliationNodes);
            repoMockTransfer.Setup(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<TransferPointConciliationMovement>());
            repoNodeTag.Setup(n => n.FirstOrDefaultAsync(It.IsAny<Expression<Func<NodeTag, bool>>>())).ReturnsAsync(new NodeTag());

            // Act
            await this.ownershipConciliation.DoConciliationAsync(new ConciliationNodesResquest() { TicketId = ownershipTicketId, NodeId = 1 }).ConfigureAwait(false);

            // Assert
            repoTicket.Verify(m => m.GetByIdAsync(It.IsAny<object>()), Times.Exactly(2));
            repoMockTransfer.Verify(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Update Movements and not execute Query.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task DoConciliationOwnershipAsync_ShouldNotBeExecuteQueryAsync()
        {
            // Arrange
            var repoMock = new Mock<IRepository<ConciliationNodesResult>>();
            var repoMockTransfer = new Mock<IRepository<TransferPointConciliationMovement>>();
            var repoTicket = new Mock<IRepository<Ticket>>();
            var repoNodeTag = new Mock<IRepository<NodeTag>>();
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<ConciliationNodesResult>()).Returns(repoMock.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<TransferPointConciliationMovement>()).Returns(repoMockTransfer.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Ticket>()).Returns(repoTicket.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<NodeTag>()).Returns(repoNodeTag.Object);
            repoMock.Setup(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(new List<ConciliationNodesResult>());
            repoMockTransfer.Setup(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()));
            repoTicket.Setup(a => a.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(new Ticket { TicketId = 10, CreatedDate = DateTime.Today, EndDate = DateTime.Today, CategoryElementId = 1 });
            repoNodeTag.Setup(n => n.FirstOrDefaultAsync(It.IsAny<Expression<Func<NodeTag, bool>>>())).ReturnsAsync(new NodeTag());

            // Act
            await this.ownershipConciliation.DoConciliationAsync(new ConciliationNodesResquest() { TicketId = 10 }).ConfigureAwait(false);

            // Assert
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            repoMockTransfer.Verify(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Never);
        }

        /// <summary>
        /// Update ticket status query.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task DoConciliationOwnershipAsync_ShouldUpdateTicketStatusQueryAsync()
        {
            // Arrange
            var repoMock = new Mock<IRepository<ConciliationNodesResult>>();
            var repoTicket = new Mock<IRepository<Ticket>>();
            var repoNodeTag = new Mock<IRepository<NodeTag>>();
            List<ConciliationNodesResult> nodes = new List<ConciliationNodesResult>();
            nodes.Add(new ConciliationNodesResult() { DestinationNodeId = 11 });
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<ConciliationNodesResult>()).Returns(repoMock.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Ticket>()).Returns(repoTicket.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<NodeTag>()).Returns(repoNodeTag.Object);
            repoMock.Setup(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(nodes);
            repoTicket.Setup(a => a.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(new Ticket { TicketId = 10, CreatedDate = DateTime.Today, EndDate = DateTime.Today, CategoryElementId = 1 });
            repoNodeTag.Setup(n => n.FirstOrDefaultAsync(It.IsAny<Expression<Func<NodeTag, bool>>>()));

            // Act
            await this.ownershipConciliation.DoConciliationAsync(new ConciliationNodesResquest() { TicketId = 10, NodeId = 123 }).ConfigureAwait(false);

            // Assert
            repoTicket.Verify(m => m.GetByIdAsync(It.IsAny<object>()), Times.Exactly(2));
            repoMock.Verify(m => m.ExecuteQueryAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
        }

        /// <summary>
        /// Conciliation send to Queue.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task InitializeManualConciliationOwnershipQueueAsync_ShouldBeReturnTrueAsync()
        {
            var repoTicket = new Mock<IRepository<Ticket>>();
            var repoOwnershipNode = new Mock<IRepository<OwnershipNode>>();
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Ticket>()).Returns(repoTicket.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<OwnershipNode>()).Returns(repoOwnershipNode.Object);
            this.mockServiceBusQueueClient.Setup(a => a.QueueSessionMessageAsync(It.IsAny<ConciliationNodesResquest>(), It.IsAny<string>()));
            repoTicket.Setup(a => a.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Ticket { TicketId = 10, CreatedDate = DateTime.Today, EndDate = DateTime.Today, CategoryElementId = 1 });
            repoOwnershipNode.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>())).ReturnsAsync(new List<OwnershipNode>());

            await this.ownershipConciliation.InitializeConciliationAsync(new ConciliationNodesResquest { TicketId = 10 }).ConfigureAwait(false);

            this.mockServiceBusQueueClient.Verify(a => a.QueueSessionMessageAsync(It.IsAny<ConciliationNodesResquest>(), It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Manual Conciliation exception register in log.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task InitializeConciliationAsyncQueueAsync_ShouldBeReturnFalseAsync()
        {
            var repoTicket = new Mock<IRepository<Ticket>>();
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Ticket>()).Returns(repoTicket.Object);
            this.mockServiceBusQueueClient.Setup(a => a.QueueSessionMessageAsync(It.IsAny<ConciliationNodesResquest>(), It.IsAny<string>())).ThrowsAsync(new Exception());
            this.mockLogger.Setup(x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<int>()));
            repoTicket.Setup(a => a.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(new Ticket { TicketId = 10, CreatedDate = DateTime.Today, EndDate = DateTime.Today, CategoryElementId = 1 });

            await this.ownershipConciliation.InitializeConciliationAsync(new ConciliationNodesResquest()).ConfigureAwait(false);

            this.mockLogger.Verify(x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        /// <summary>
        /// Conciliation Ownership Finalize execute process async.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ConciliationOwnershipFinalizaAsync_ShouldExecuteAsync()
        {
            this.mockFinalizer.Setup(f => f.ProcessAsync(It.IsAny<ConciliationRuleData>()));
            await this.ownershipConciliation.FinalizeProcessAsync(new ConciliationRuleData()).ConfigureAwait(false);
            this.mockFinalizer.Verify(f => f.ProcessAsync(It.IsAny<ConciliationRuleData>()), Times.Once);
        }

        /// <summary>
        /// Update ownership node query async.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateOwnershipNodeAsync_ShouldBeExecuteQueryAsync()
        {
            // Arrange
            var repoOwnerShipNode = new Mock<IRepository<OwnershipNode>>();
            this.mockUnitOfWork.Setup(f => f.CreateRepository<OwnershipNode>()).Returns(repoOwnerShipNode.Object);
            repoOwnerShipNode.Setup(o => o.GetAllAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>())).ReturnsAsync(new List<OwnershipNode>
            {
                new OwnershipNode() { TicketId = 10 },
            });
            repoOwnerShipNode.Setup(o => o.UpdateAll(It.IsAny<IEnumerable<OwnershipNode>>()));

            // Act
            await this.ownershipConciliation.UpdateOwnershipNodeAsync(10, StatusType.PROCESSING, OwnershipNodeStatusType.RECONCILED, 20).ConfigureAwait(false);

            // Assert
            repoOwnerShipNode.Verify(o => o.GetAllAsync(It.IsAny<Expression<Func<OwnershipNode, bool>>>()), Times.Once);
            repoOwnerShipNode.Verify(o => o.UpdateAll(It.IsAny<IEnumerable<OwnershipNode>>()), Times.Once);
        }

        /// <summary>
        /// Get movements conciliation query async.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetMovementConciliationAsync_ShouldBeExecuteQueryAsync()
        {
            // Arrange
            var repoMovement = new Mock<IRepository<Movement>>();
            this.mockUnitOfWork.Setup(f => f.CreateRepository<Movement>()).Returns(repoMovement.Object);
            repoMovement.Setup(o => o.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>())).ReturnsAsync(new List<Movement>());

            // Act
            await this.ownershipConciliation.GetConciliationMovementsAsync(10, 20).ConfigureAwait(false);

            // Assert
            repoMovement.Verify(o => o.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>()), Times.Exactly(1));
        }

        /// <summary>
        /// Register negative movement and send to blockchain queue.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task RegisterNegativeMovementAsync_ShouldExecuteQueueAsync()
        {
            // Arrange
            var repoMovement = new Mock<IRepository<Movement>>();
            var repoInventoryMovementIndex = new Mock<IRepository<InventoryMovementIndex>>();
            this.mockUnitOfWork.Setup(f => f.CreateRepository<InventoryMovementIndex>()).Returns(repoInventoryMovementIndex.Object);
            this.mockUnitOfWork.Setup(f => f.CreateRepository<Movement>()).Returns(repoMovement.Object);
            this.mockUnitOfWork.Setup(f => f.SaveAsync(It.IsAny<CancellationToken>()));
            repoMovement.Setup(o => o.InsertAll(It.IsAny<List<Movement>>()));
            repoInventoryMovementIndex.Setup(i => i.Insert(It.IsAny<InventoryMovementIndex>()));

            // Act
            var movement = new Movement
            {
                MovementSource = new MovementSource(),
                MovementDestination = new MovementDestination(),
                Period = new MovementPeriod(),
                NetStandardVolume = 10,
                GrossStandardVolume = 10,
                OperationalDate = DateTime.Now,
            };
            movement.Owners.Add(new Owner { OwnershipValueUnit = "%", OwnershipValue = 10 });
            movement.Attributes.Add(new AttributeEntity());
            movement.SapTracking.Add(new SapTracking());
            await this.ownershipConciliation.RegisterNegativeMovementsAsync(new List<Movement> { movement }).ConfigureAwait(false);

            // Assert
            repoMovement.Verify(o => o.InsertAll(It.IsAny<List<Movement>>()), Times.Once);
            this.mockServiceBusQueueClient.Verify(a => a.QueueSessionMessageAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Delete relationship other segment movements.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DeleteRelationshipOtherSegmentMovementsAsync_ShouldExecuteQueueAsync()
        {
            // Arrange
            var movements = new List<Movement>
            {
                new Movement
                {
                    MovementTransactionId = 12321,
                    MovementDestination = new MovementDestination { MovementTransactionId = 12321, DestinationNodeId = 123 },
                    MovementSource = new MovementSource { MovementTransactionId = 12321, SourceNodeId = 123 },
                },
            };

            var repoMovement = new Mock<IRepository<Movement>>();
            this.mockUnitOfWork.Setup(f => f.CreateRepository<Movement>()).Returns(repoMovement.Object);
            this.mockUnitOfWork.Setup(f => f.SaveAsync(It.IsAny<CancellationToken>()));
            repoMovement.Setup(o => o.UpdateAll(It.IsAny<List<Movement>>()));
            repoMovement.Setup(o => o.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(movements);

            // Act
            int ticketId = 1234;
            await this.ownershipConciliation.DeleteRelationshipOtherSegmentMovementsAsync(ticketId, 123).ConfigureAwait(false);

            // Assert
            repoMovement.Verify(o => o.UpdateAll(It.IsAny<List<Movement>>()), Times.Once);
        }

        /// <summary>
        /// Delete conciliation movements collectionAsync.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DeleteConciliationMovementsCollectionAsync_ShouldExecuteQueueAsync()
        {
            // Arrange
            var movements = new List<MovementConciliation> { new MovementConciliation { MovementTransactionId = 12321 } };
            var repoMovement = new Mock<IRepository<MovementConciliation>>();
            this.mockUnitOfWork.Setup(f => f.CreateRepository<MovementConciliation>()).Returns(repoMovement.Object);
            this.mockUnitOfWork.Setup(f => f.SaveAsync(It.IsAny<CancellationToken>()));
            repoMovement.Setup(o => o.UpdateAll(It.IsAny<List<MovementConciliation>>()));
            repoMovement.Setup(o => o.GetAllAsync(It.IsAny<Expression<Func<MovementConciliation, bool>>>())).ReturnsAsync(movements);

            // Act
            int ticketId = 1234;
            await this.ownershipConciliation.DeleteConciliationMovementsAsync(ticketId, null).ConfigureAwait(false);

            // Assert
            repoMovement.Verify(o => o.DeleteAll(It.IsAny<List<MovementConciliation>>()), Times.Once);
        }
    }
}
