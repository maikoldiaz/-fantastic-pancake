// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipResultServiceTests.cs" company="Microsoft">
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
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Ownership.Services;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The OwnershipResultServiceTests.
    /// </summary>
    [TestClass]
    public class OwnershipResultServiceTests
    {
        /// <summary>
        /// The movement service.
        /// </summary>
        private OwnershipResultService movementService;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockunitOfWork;

        /// <summary>
        /// The mock contract repository.
        /// </summary>
        private Mock<IRepository<Ecp.True.Entities.Registration.Contract>> mockContractRepository;

        /// <summary>
        /// The mock movement repository.
        /// </summary>
        private Mock<IRepository<Movement>> mockMovementRepository;

        /// <summary>
        /// The mock ticket repository.
        /// </summary>
        private Mock<IRepository<Ticket>> mockTicketRepository;

        /// <summary>
        /// The mock movement repository.
        /// </summary>
        private Mock<IRepository<True.Entities.Registration.Event>> mockEventRepository;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.movementService = new OwnershipResultService();
            this.mockunitOfWork = new Mock<IUnitOfWork>();
            this.mockContractRepository = new Mock<IRepository<Entities.Registration.Contract>>();
            this.mockMovementRepository = new Mock<IRepository<Movement>>();
            this.mockTicketRepository = new Mock<IRepository<Ticket>>();
            this.mockEventRepository = new Mock<IRepository<Entities.Registration.Event>>();
        }

        /// <summary>
        /// Gets the movement ownerships should process.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task BuildOwnershipMovementResultsAsync_ShouldBuildMovements_Collaboration_Async()
        {
            var commercialMovementsResult = new CommercialMovementsResult
            {
                MovementId = 120,
                MovementType = Constants.PurchaseMovementType,
                ContractId = 10,
                OwnerId = 30,
                Volume = 8000,
                AppliedRule = "6",
                RuleVersion = "1",
            };

            var newMovement = new NewMovement
            {
                AgreementType = Constants.Collaboration,
                EventId = 1,
                MovementId = 120,
                CreditorOwnerId = 29,
                DebtorOwnerId = 30,
                OwnershipVolume = 2000,
                AppliedRule = "6",
                RuleVersion = "1",
            };

            var cancellationMovement = new CancellationMovementDetail
            {
                MovementTransactionId = 120,
                NetVolume = 8000,
                MessageTypeId = 41,
                MovementType = "20",
                OperationalDate = DateTime.UtcNow,
                SegmentId = 40,
                Unit = 30,
                OwnershipPercentage = 100.00M,
                OwnershipVolume = 8000,
                OwnerId = 29,
            };

            var contract = new Ecp.True.Entities.Registration.Contract
            {
                ContractId = 10,
                DocumentNumber = 12300102,
                Position = 1,
                SourceNodeId = 368,
                DestinationNodeId = 367,
                ProductId = "10000002318",
                Owner1Id = 30,
                Owner2Id = 29,
                Volume = 8000,
                MeasurementUnit = 31,
                IsDeleted = false,
                MovementTypeId = 20,
            };

            var movement = new Movement
            {
                MovementTransactionId = 120,
                MovementDestination = new MovementDestination
                {
                    DestinationNodeId = 20,
                    DestinationProductId = "30",
                    DestinationProductTypeId = 40,
                },
                MeasurementUnit = 31,
                SegmentId = 1042,
                OperationalDate = DateTime.UtcNow,
                GrossStandardVolume = 4000,
                NetStandardVolume = 9000,
            };

            var event1 = new Entities.Registration.Event
            {
                EventId = 1,
                EventTypeId = 45,
                SourceNodeId = 878,
                DestinationNodeId = 879,
                SourceProductId = "10000002318",
                DestinationProductId = "10000002372",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow,
                Owner1Id = 29,
                Owner2Id = 30,
                Volume = 9000,
                MeasurementUnit = "31",
                IsDeleted = false,
            };

            var ticket = new Ticket
            {
                TicketId = 12375,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow,
            };

            this.mockunitOfWork.Setup(x => x.CreateRepository<Ecp.True.Entities.Registration.Contract>()).Returns(this.mockContractRepository.Object);
            this.mockunitOfWork.Setup(x => x.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);
            this.mockunitOfWork.Setup(x => x.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);
            this.mockunitOfWork.Setup(x => x.CreateRepository<Entities.Registration.Event>()).Returns(this.mockEventRepository.Object);
            this.mockContractRepository.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Entities.Registration.Contract, bool>>>())).ReturnsAsync(new List<Entities.Registration.Contract> { contract });
            this.mockMovementRepository.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>())).ReturnsAsync(new List<Movement> { movement });
            this.mockMovementRepository.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<Movement> { movement });
            this.mockEventRepository.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Entities.Registration.Event, bool>>>())).ReturnsAsync(new List<Entities.Registration.Event> { event1 });
            this.mockTicketRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ticket);

            var result = await this.movementService.BuildOwnershipMovementResultsAsync(
                new List<CommercialMovementsResult> { commercialMovementsResult },
                new List<NewMovement> { newMovement },
                new List<CancellationMovementDetail> { cancellationMovement },
                12375,
                this.mockunitOfWork.Object).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockunitOfWork.Verify(x => x.CreateRepository<Movement>(), Times.Exactly(2));
            this.mockunitOfWork.Verify(x => x.CreateRepository<Entities.Registration.Event>(), Times.Once);
            this.mockunitOfWork.Verify(x => x.CreateRepository<Ecp.True.Entities.Registration.Contract>(), Times.Once);
        }

        [TestMethod]
        public async Task BuildOwnershipMovementResultsAsync_ShouldBuildMovements_Evacuation_Async()
        {
            var commercialMovementsResult = new CommercialMovementsResult
            {
                MovementId = 120,
                MovementType = Constants.PurchaseMovementType,
                ContractId = 10,
                OwnerId = 30,
                Volume = 8000,
                AppliedRule = "6",
                RuleVersion = "1",
            };

            var newMovement = new NewMovement
            {
                AgreementType = Constants.Evacuation,
                NodeId = 1,
                ProductId = "Prd",
                MovementId = 120,
                CreditorOwnerId = 29,
                DebtorOwnerId = 30,
                OwnershipVolume = 2000,
                AppliedRule = "6",
                RuleVersion = "1",
            };

            var cancellationMovement = new CancellationMovementDetail
            {
                MovementTransactionId = 120,
                NetVolume = 8000,
                MessageTypeId = 41,
                MovementType = "20",
                OperationalDate = DateTime.UtcNow,
                SegmentId = 40,
                Unit = 30,
                OwnershipPercentage = 100.00M,
                OwnershipVolume = 8000,
                OwnerId = 29,
            };

            var contract = new Ecp.True.Entities.Registration.Contract
            {
                ContractId = 10,
                DocumentNumber = 12300102,
                Position = 1,
                SourceNodeId = 368,
                DestinationNodeId = 367,
                ProductId = "10000002318",
                Owner1Id = 30,
                Owner2Id = 29,
                Volume = 8000,
                MeasurementUnit = 31,
                IsDeleted = false,
                MovementTypeId = 20,
            };

            var movement = new Movement
            {
                MovementTransactionId = 120,
                MovementDestination = new MovementDestination
                {
                    DestinationNodeId = 20,
                    DestinationProductId = "30",
                    DestinationProductTypeId = 40,
                },
                MeasurementUnit = 31,
                SegmentId = 1042,
                OperationalDate = DateTime.UtcNow,
                GrossStandardVolume = 4000,
                NetStandardVolume = 9000,
            };

            var event1 = new Entities.Registration.Event
            {
                EventId = 1,
                EventTypeId = 45,
                SourceNodeId = 878,
                DestinationNodeId = 879,
                SourceProductId = "10000002318",
                DestinationProductId = "10000002372",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow,
                Owner1Id = 29,
                Owner2Id = 30,
                Volume = 9000,
                MeasurementUnit = "31",
                IsDeleted = false,
            };

            var ticket = new Ticket
            {
                TicketId = 12375,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow,
            };

            this.mockunitOfWork.Setup(x => x.CreateRepository<Ecp.True.Entities.Registration.Contract>()).Returns(this.mockContractRepository.Object);
            this.mockunitOfWork.Setup(x => x.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);
            this.mockunitOfWork.Setup(x => x.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);
            this.mockunitOfWork.Setup(x => x.CreateRepository<Entities.Registration.Event>()).Returns(this.mockEventRepository.Object);
            this.mockContractRepository.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Entities.Registration.Contract, bool>>>())).ReturnsAsync(new List<Entities.Registration.Contract> { contract });
            this.mockMovementRepository.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>())).ReturnsAsync(new List<Movement> { movement });
            this.mockMovementRepository.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<Movement> { movement });
            this.mockEventRepository.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Entities.Registration.Event, bool>>>())).ReturnsAsync(new List<Entities.Registration.Event> { event1 });
            this.mockTicketRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ticket);

            var result = await this.movementService.BuildOwnershipMovementResultsAsync(
                new List<CommercialMovementsResult> { commercialMovementsResult },
                new List<NewMovement> { newMovement },
                new List<CancellationMovementDetail> { cancellationMovement },
                12375,
                this.mockunitOfWork.Object).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.mockunitOfWork.Verify(x => x.CreateRepository<Movement>(), Times.Exactly(2));
            this.mockunitOfWork.Verify(x => x.CreateRepository<Entities.Registration.Event>(), Times.Once);
            this.mockunitOfWork.Verify(x => x.CreateRepository<Ecp.True.Entities.Registration.Contract>(), Times.Once);
        }

        /// <summary>
        /// Gets the movement ownerships should process.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task BuildOwnershipMovementResultsAsync_ShouldReturnEmptyMovementsAsync()
        {
            var ticket = new Ticket
            {
                TicketId = 12375,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow,
            };

            this.mockunitOfWork.Setup(x => x.CreateRepository<Ticket>()).Returns(this.mockTicketRepository.Object);
            this.mockTicketRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ticket);
            var result = await this.movementService.BuildOwnershipMovementResultsAsync(
                new List<CommercialMovementsResult>(),
                new List<NewMovement>(),
                new List<CancellationMovementDetail>(),
                12375,
                this.mockunitOfWork.Object).ConfigureAwait(false);
            Assert.AreEqual(0, result.ToList().Count);
        }
    }
}
