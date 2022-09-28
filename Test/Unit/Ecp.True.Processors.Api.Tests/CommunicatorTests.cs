// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommunicatorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [Ignore]
    public class CommunicatorTests
    {
        private readonly Mock<IRegistrationStrategyFactory> mockRegistrationStrategyFactory = new Mock<IRegistrationStrategyFactory>();

        /// <summary>
        /// The repository mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>
        /// The unit of work mock factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The azureclient factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureclientFactory;

        private Mock<IConfigurationHandler> mockConfigurationHandler;

        private Mock<IServiceBusQueueClient> mockServiceBusQueueClient;

        private Mock<IRepository<Movement>> repoMovementMock;

        private Mock<IRepository<Contract>> repoContractMock;

        private Mock<IRepository<Ownership>> repoOwnershipMock;

        private Mock<IRepository<MovementContract>> repoMovementContractMock;

        private ServiceBusSettings integrationServiceBusSettings;

        private Communicator communicator;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.InitializeMockObjects();
            this.integrationServiceBusSettings = new ServiceBusSettings
            {
                ConnectionString = string.Empty,
            };
            this.communicator = new Communicator(
                                                 this.mockAzureclientFactory.Object,
                                                 this.mockConfigurationHandler.Object,
                                                 this.mockUnitOfWorkFactory.Object,
                                                 this.mockRegistrationStrategyFactory.Object);
        }

        [TestMethod]
        public async Task CreateMovement_WithOwnerships_WhenTransactionIdsAreNegativeAsync()
        {
            var nodeOwnership = this.CreatePublishedNodeOwnership();
            nodeOwnership.InventoryOwnerships = Enumerable.Empty<EditOwnershipInfo<InventoryOwnership>>();
            var token = new CancellationToken(false);

            var contracts = new List<Contract>
            {
                new Contract
                {
                    ContractId = 1,
                    DocumentNumber = 101,
                    Position = 1,
                },
            };

            this.DoCommonFunctionCallSetups();
            this.mockAzureclientFactory.Setup(m => m.GetBlobClient(It.IsAny<string>()).CreateBlobAsync(It.IsAny<string>(), It.IsAny<Stream>())).Returns(Task.CompletedTask);
            this.mockAzureclientFactory.Setup(m => m.GetQueueClient(QueueConstants.HomologatedMovementsQueue)).Returns(this.mockServiceBusQueueClient.Object);
            this.mockServiceBusQueueClient.Setup(m => m.QueueSessionMessageAsync(It.IsAny<OwnershipMessage>(), It.IsAny<string>()));
            this.repoMovementMock.Setup(m => m.InsertAll(It.IsAny<IEnumerable<Movement>>()));
            this.repoContractMock.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Contract, bool>>>())).ReturnsAsync(() => contracts);
            this.mockAzureclientFactory.Setup(m => m.GetBlobStorageSaSClient(It.IsAny<string>(), It.IsAny<string>()).CreateBlobAsync(It.IsAny<Stream>())).Returns(Task.CompletedTask);
            this.mockAzureclientFactory.Setup(m => m.GetBlobStorageSaSClient(It.IsAny<string>(), It.IsAny<string>()).CreateBlobAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            await this.communicator.RegisterOwnerShipAsync(nodeOwnership).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Movement>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Exactly(3));
        }

        [TestMethod]
        public async Task DeleteMovementOwnerships_WhenDeletedTransactionIdsAreGivenAsync()
        {
            var nodeOwnership = this.CreatePublishedNodeOwnership();
            nodeOwnership.InventoryOwnerships = Enumerable.Empty<EditOwnershipInfo<InventoryOwnership>>();
            var token = new CancellationToken(false);
            var contracts = new List<Contract>
            {
                new Contract
                {
                    ContractId = 1,
                    DocumentNumber = 101,
                    Position = 1,
                },
            };
            this.DoCommonFunctionCallSetups();
            nodeOwnership.DeletedTransactionIds = this.GetDeletedTransactionIds();
            nodeOwnership.InventoryOwnerships = Enumerable.Empty<EditOwnershipInfo<InventoryOwnership>>();

            var movement = nodeOwnership.Movements.FirstOrDefault();
            movement.MovementTransactionId = 3072;
            movement.EventType = EventType.Delete.ToString();

            var updatedOwnerships = movement.Ownerships.ToList();
            updatedOwnerships.ForEach(x => x.MovementTransactionId = 3072);

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Ownership>()).Returns(this.repoOwnershipMock.Object);
            this.repoMovementMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(movement);
            this.repoContractMock.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Contract, bool>>>())).ReturnsAsync(() => contracts);
            this.repoOwnershipMock.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Ownership, bool>>>())).ReturnsAsync(() => updatedOwnerships);
            this.repoMovementMock.Setup(m => m.InsertAll(It.IsAny<IEnumerable<Movement>>()));
            this.mockAzureclientFactory.Setup(m => m.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);

            await this.communicator.RegisterOwnerShipAsync(nodeOwnership).ConfigureAwait(false);

            this.DoCommonFunctionCallVerifications();
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Ownership>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Exactly(2));

            foreach (var ownership in movement.Ownerships)
            {
                Assert.IsTrue(ownership.IsDeleted);
            }
        }

        [TestMethod]
        public async Task UpdateMovementOwnerships_WhenOwnershipPercentagesAreModifiedAsync()
        {
            var nodeOwnership = this.CreatePublishedNodeOwnership();
            nodeOwnership.InventoryOwnerships = Enumerable.Empty<EditOwnershipInfo<InventoryOwnership>>();
            var token = new CancellationToken(false);

            this.DoCommonFunctionCallSetups();
            nodeOwnership.InventoryOwnerships = Enumerable.Empty<EditOwnershipInfo<InventoryOwnership>>();
            nodeOwnership.DeletedTransactionIds = Enumerable.Empty<int>();

            var movement = nodeOwnership.Movements.FirstOrDefault();
            movement.MovementTransactionId = 3070;
            movement.EventType = EventType.Update.ToString();

            var updatedOwnerships = movement.Ownerships.ToList();
            updatedOwnerships.ForEach(x => x.MovementTransactionId = 3070);

            updatedOwnerships.ElementAt(0).OwnershipPercentage = 50;
            updatedOwnerships.ElementAt(0).OwnershipVolume = 500;
            updatedOwnerships.ElementAt(1).OwnershipPercentage = 50;
            updatedOwnerships.ElementAt(1).OwnershipVolume = 500;

            var contracts = new List<Contract>
            {
                new Contract
                {
                    ContractId = 1,
                    DocumentNumber = 101,
                    Position = 1,
                },
            };

            var movementContractData = new MovementContract
            {
                MovementContractId = 1,
            };

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Ownership>()).Returns(this.repoOwnershipMock.Object);
            this.repoMovementMock.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>())).ReturnsAsync(nodeOwnership.Movements);
            this.repoContractMock.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Contract, bool>>>())).ReturnsAsync(() => contracts);
            this.repoOwnershipMock.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Ownership, bool>>>())).ReturnsAsync(() => updatedOwnerships);
            this.repoMovementContractMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(movementContractData);
            this.repoMovementContractMock.Setup(m => m.Update(It.IsAny<MovementContract>()));
            this.repoMovementMock.Setup(m => m.InsertAll(It.IsAny<IEnumerable<Movement>>()));
            this.mockAzureclientFactory.Setup(m => m.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);

            await this.communicator.RegisterOwnerShipAsync(nodeOwnership).ConfigureAwait(false);

            this.DoCommonFunctionCallVerifications();
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Ownership>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Exactly(2));

            Assert.AreEqual(50, movement.Ownerships.ElementAt(0).OwnershipPercentage);
            Assert.AreEqual(500, movement.Ownerships.ElementAt(0).OwnershipVolume);
            Assert.AreEqual(50, movement.Ownerships.ElementAt(1).OwnershipPercentage);
            Assert.AreEqual(500, movement.Ownerships.ElementAt(1).OwnershipVolume);
        }

        [TestMethod]
        public async Task UpdateMovementContract_WhenOwnershipContractisModifiedAsync()
        {
            var nodeOwnership = this.CreatePublishedNodeOwnership();
            nodeOwnership.InventoryOwnerships = Enumerable.Empty<EditOwnershipInfo<InventoryOwnership>>();
            var token = new CancellationToken(false);

            this.DoCommonFunctionCallSetups();
            nodeOwnership.InventoryOwnerships = Enumerable.Empty<EditOwnershipInfo<InventoryOwnership>>();
            nodeOwnership.DeletedTransactionIds = Enumerable.Empty<int>();

            var movement = nodeOwnership.Movements.FirstOrDefault();
            movement.MovementTransactionId = 3070;
            movement.EventType = EventType.Update.ToString();

            var updatedOwnerships = movement.Ownerships.ToList();
            updatedOwnerships.ForEach(x => x.MovementTransactionId = 3070);

            updatedOwnerships.ElementAt(0).OwnershipPercentage = 50;
            updatedOwnerships.ElementAt(0).OwnershipVolume = 500;
            updatedOwnerships.ElementAt(1).OwnershipPercentage = 50;
            updatedOwnerships.ElementAt(1).OwnershipVolume = 500;

            var movements = this.CreateMovements().ToList();
            movements[0].MovementContractId = 2;

            var contracts = new List<Contract>
            {
                new Contract
                {
                    ContractId = 1,
                    DocumentNumber = 101,
                    Position = 1,
                },
            };

            var movementContractData = new MovementContract
            {
                MovementContractId = 1,
            };

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Ownership>()).Returns(this.repoOwnershipMock.Object);
            this.repoMovementMock.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>())).ReturnsAsync(movements);
            this.repoContractMock.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Contract, bool>>>())).ReturnsAsync(() => contracts);
            this.repoOwnershipMock.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Ownership, bool>>>())).ReturnsAsync(() => updatedOwnerships);
            this.repoMovementContractMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(movementContractData);
            this.repoMovementContractMock.Setup(m => m.Update(It.IsAny<MovementContract>()));
            this.repoMovementMock.Setup(m => m.InsertAll(It.IsAny<IEnumerable<Movement>>()));
            this.mockAzureclientFactory.Setup(m => m.GetQueueClient(It.IsAny<string>())).Returns(this.mockServiceBusQueueClient.Object);

            await this.communicator.RegisterOwnerShipAsync(nodeOwnership).ConfigureAwait(false);

            this.DoCommonFunctionCallVerifications();
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Ownership>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Exactly(2));

            Assert.AreEqual(50, movement.Ownerships.ElementAt(0).OwnershipPercentage);
            Assert.AreEqual(500, movement.Ownerships.ElementAt(0).OwnershipVolume);
            Assert.AreEqual(50, movement.Ownerships.ElementAt(1).OwnershipPercentage);
            Assert.AreEqual(500, movement.Ownerships.ElementAt(1).OwnershipVolume);
        }

        private void InitializeMockObjects()
        {
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.mockAzureclientFactory = new Mock<IAzureClientFactory>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockConfigurationHandler = new Mock<IConfigurationHandler>();
            this.mockServiceBusQueueClient = new Mock<IServiceBusQueueClient>();
            this.repoMovementMock = new Mock<IRepository<Movement>>();
            this.repoOwnershipMock = new Mock<IRepository<Ownership>>();
            this.repoContractMock = new Mock<IRepository<Contract>>();
            this.repoMovementContractMock = new Mock<IRepository<MovementContract>>();
        }

        private void DoCommonFunctionCallSetups()
        {
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Movement>()).Returns(this.repoMovementMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Contract>()).Returns(this.repoContractMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<MovementContract>()).Returns(this.repoMovementContractMock.Object);
            this.mockAzureclientFactory.Setup(m => m.GetBlobStorageSaSClient(It.IsAny<string>(), It.IsAny<string>()).CreateBlobAsync(It.IsAny<Stream>())).Returns(Task.CompletedTask);
            this.mockAzureclientFactory.Setup(m => m.GetBlobStorageSaSClient(It.IsAny<string>(), It.IsAny<string>()).CreateBlobAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            this.mockConfigurationHandler.Setup(m => m.GetConfigurationAsync<ServiceBusSettings>(ConfigurationConstants.ServiceBusSettings)).Returns(Task.FromResult(this.integrationServiceBusSettings));
            this.mockAzureclientFactory.Setup(m => m.GetQueueClient("ownershipmovements")).Returns(this.mockServiceBusQueueClient.Object);
            this.mockServiceBusQueueClient.Setup(m => m.QueueSessionMessageAsync(It.IsAny<FileRegistrationTransaction>(), It.IsAny<string>())).Returns(Task.CompletedTask);
        }

        private void DoCommonFunctionCallVerifications()
        {
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Movement>(), Times.Once);
        }

        private PublishedNodeOwnership CreatePublishedNodeOwnership()
        {
            PublishedNodeOwnership publishNodeOwnership = new PublishedNodeOwnership
            {
                Movements = this.CreateMovements(),
                OwnershipNodeId = 400,
                TicketId = 25029,
            };

            return publishNodeOwnership;
        }

        private IEnumerable<Movement> CreateMovements()
        {
            MovementSource movementSource = new MovementSource()
            {
                SourceNodeId = 5307,
                SourceProductId = "10000002318",
            };

            MovementPeriod period = new MovementPeriod()
            {
                StartTime = DateTime.Parse("02/01/2020", CultureInfo.InvariantCulture),
                EndTime = DateTime.Parse("06/01/2020", CultureInfo.InvariantCulture),
                MovementTransactionId = -1,
            };

            Movement newMovement = new Movement()
            {
                MovementTransactionId = -1,
                MovementTypeId = (int)MessageType.Movement,
                TicketId = 25029,
                SegmentId = 15177,
                Classification = "Movimiento",
                OperationalDate = DateTime.Now,
                NetStandardVolume = 1000,
                MeasurementUnit = 31,
                VariableTypeId = VariableType.BalanceTolerance,
                ReasonId = 54,
                Comment = "creating new movement",
                EventType = EventType.Insert.ToString(),
                MovementId = DateTime.Now.ToString("d", CultureInfo.InvariantCulture),
                MovementSource = movementSource,
                MovementDestination = new MovementDestination(),
                Period = period,
                MovementContractId = 1,
            };

            this.CreateOwnerships(newMovement);

            List<Movement> movements = new List<Movement>
            {
                newMovement,
            };

            return movements;
        }

        private void CreateOwnerships(Movement movement)
        {
            Ownership ownership1 = new Ownership()
            {
                TicketId = 25029,
                MovementTransactionId = -1,
                OwnerId = 29,
                OwnershipPercentage = 60,
                OwnershipVolume = 600,
                AppliedRule = "Propiedad Manual",
                RuleVersion = "1",
                ExecutionDate = DateTime.Parse("06/02/2020", CultureInfo.InvariantCulture),
            };

            Ownership ownership2 = new Ownership()
            {
                TicketId = 25029,
                MovementTransactionId = -1,
                OwnerId = 27,
                OwnershipPercentage = 40,
                OwnershipVolume = 400,
                AppliedRule = "Propiedad Manual",
                RuleVersion = "1",
                ExecutionDate = DateTime.Parse("06/02/2020", CultureInfo.InvariantCulture),
            };

            movement.Ownerships.Add(ownership1);
            movement.Ownerships.Add(ownership2);
        }

        private List<int> GetDeletedTransactionIds()
        {
            return new List<int>() { 3072 };
        }
    }
}
