// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementRegistrationServiceTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The movement registration transaction service tests.
    /// </summary>
    [TestClass]
    public class MovementRegistrationServiceTests
    {
        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The mock logger.
        /// </summary>
        private readonly Mock<ITrueLogger<MovementRegistrationService>> mockLogger = new Mock<ITrueLogger<MovementRegistrationService>>();

        /// <summary>
        /// The mock owner repository.
        /// </summary>
        private readonly Mock<IRepository<Owner>> mockOwnerRepository = new Mock<IRepository<Owner>>();

        /// <summary>
        /// The mock attribute repository.
        /// </summary>
        private readonly Mock<IRepository<AttributeEntity>> mockAttributeRepository = new Mock<IRepository<AttributeEntity>>();

        /// <summary>
        /// The unit of work mock.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWorkMock;

        /// <summary>
        /// The movement repository.
        /// </summary>
        private Mock<IRepository<Movement>> mockMovementRepository;

        /// <summary>
        /// The movement registration service.
        /// </summary>
        private MovementRegistrationService movementRegistrationService;

        /// <summary>
        /// The mock inventory movement index repository.
        /// </summary>
        private Mock<IRepository<InventoryMovementIndex>> mockInventoryMovementIndexRepository;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockMovementRepository = new Mock<IRepository<Movement>>();
            this.mockInventoryMovementIndexRepository = new Mock<IRepository<InventoryMovementIndex>>();
            this.mockUnitOfWorkMock = new Mock<IUnitOfWork>();
            this.movementRegistrationService = new MovementRegistrationService(this.mockLogger.Object, this.mockAzureClientFactory.Object);
        }

        /// <summary>
        /// Movements the registration service insert should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationService_Insert_Should_RegisterAsync()
        {
            var movement = this.GetMovement("Insert", false, StatusType.PROCESSING);

            this.mockUnitOfWorkMock.Setup(a => a.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);
            this.mockMovementRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Movement> { movement });
            this.mockMovementRepository.Setup(a => a.Insert(It.IsAny<Movement>()));
            this.mockUnitOfWorkMock.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));
            this.mockUnitOfWorkMock.Setup(m => m.CreateRepository<Owner>()).Returns(this.mockOwnerRepository.Object);
            this.mockUnitOfWorkMock.Setup(m => m.CreateRepository<AttributeEntity>()).Returns(this.mockAttributeRepository.Object);
            this.mockUnitOfWorkMock.Setup(a => a.CreateRepository<InventoryMovementIndex>()).Returns(this.mockInventoryMovementIndexRepository.Object);
            this.mockInventoryMovementIndexRepository.Setup(a => a.Insert(It.IsAny<InventoryMovementIndex>()));

            await this.movementRegistrationService.RegisterMovementAsync(movement, this.mockUnitOfWorkMock.Object, false).ConfigureAwait(false);

            this.mockUnitOfWorkMock.Verify(a => a.CreateRepository<Movement>(), Times.Exactly(2));
            this.mockMovementRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockMovementRepository.Verify(a => a.Insert(It.IsAny<Movement>()), Times.Once);
            this.mockUnitOfWorkMock.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
            this.mockUnitOfWorkMock.Verify(a => a.CreateRepository<InventoryMovementIndex>(), Times.Exactly(1));
            this.mockInventoryMovementIndexRepository.Verify(a => a.Insert(It.IsAny<InventoryMovementIndex>()), Times.Exactly(1));
        }

        /// <summary>
        /// Movements the registration service update should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationService_Update_Should_RegisterAsync()
        {
            var receivedMovement = this.GetMovement("Update", true, StatusType.PROCESSING);
            var existingMovement = this.GetMovement("Update", true, StatusType.PROCESSING);
            existingMovement.NetStandardVolume = Convert.ToDecimal(12234.00);
            existingMovement.GrossStandardVolume = Convert.ToDecimal(55667.00);
            var movementsFromRepoCall = new List<Movement>();

            this.mockUnitOfWorkMock.Setup(a => a.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);
            this.mockMovementRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Movement> { existingMovement });
            this.mockMovementRepository.Setup(a => a.Insert(It.IsAny<Movement>())).Callback<Movement>(o => movementsFromRepoCall.Add(o));
            this.mockUnitOfWorkMock.Setup(m => m.CreateRepository<Owner>()).Returns(this.mockOwnerRepository.Object);
            this.mockUnitOfWorkMock.Setup(m => m.CreateRepository<AttributeEntity>()).Returns(this.mockAttributeRepository.Object);
            this.mockUnitOfWorkMock.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));
            this.mockUnitOfWorkMock.Setup(a => a.CreateRepository<InventoryMovementIndex>()).Returns(this.mockInventoryMovementIndexRepository.Object);
            this.mockInventoryMovementIndexRepository.Setup(a => a.Insert(It.IsAny<InventoryMovementIndex>()));

            await this.movementRegistrationService.RegisterMovementAsync(receivedMovement, this.mockUnitOfWorkMock.Object, false).ConfigureAwait(false);

            this.mockUnitOfWorkMock.Verify(a => a.CreateRepository<Movement>(), Times.Exactly(3));
            this.mockMovementRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockMovementRepository.Verify(a => a.Insert(It.IsAny<Movement>()), Times.Exactly(2));
            this.mockUnitOfWorkMock.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
            this.mockUnitOfWorkMock.Verify(a => a.CreateRepository<InventoryMovementIndex>(), Times.Exactly(2));
            this.mockInventoryMovementIndexRepository.Verify(a => a.Insert(It.IsAny<InventoryMovementIndex>()), Times.Exactly(2));
            Assert.AreEqual(true, movementsFromRepoCall.Any(x => x.NetStandardVolume == existingMovement.NetStandardVolume * -1));
            Assert.AreEqual(true, movementsFromRepoCall.Any(x => x.GrossStandardVolume == existingMovement.GrossStandardVolume * -1));
        }

        /// <summary>
        /// Movements the registration service update should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationService_UpdateIfMovementSourceIsNull_Should_RegisterAsync()
        {
            var movement = this.GetMovement("Update", true, StatusType.PROCESSING);
            movement.MovementSource = null;

            this.mockUnitOfWorkMock.Setup(a => a.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);
            this.mockMovementRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Movement> { movement });
            this.mockMovementRepository.Setup(a => a.Insert(It.IsAny<Movement>()));
            this.mockUnitOfWorkMock.Setup(m => m.CreateRepository<Owner>()).Returns(this.mockOwnerRepository.Object);
            this.mockUnitOfWorkMock.Setup(m => m.CreateRepository<AttributeEntity>()).Returns(this.mockAttributeRepository.Object);
            this.mockUnitOfWorkMock.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));
            this.mockUnitOfWorkMock.Setup(a => a.CreateRepository<InventoryMovementIndex>()).Returns(this.mockInventoryMovementIndexRepository.Object);
            this.mockInventoryMovementIndexRepository.Setup(a => a.Insert(It.IsAny<InventoryMovementIndex>()));

            await this.movementRegistrationService.RegisterMovementAsync(movement, this.mockUnitOfWorkMock.Object, false).ConfigureAwait(false);

            this.mockUnitOfWorkMock.Verify(a => a.CreateRepository<Movement>(), Times.Exactly(3));
            this.mockMovementRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockMovementRepository.Verify(a => a.Insert(It.IsAny<Movement>()), Times.Exactly(2));
            this.mockUnitOfWorkMock.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
            this.mockUnitOfWorkMock.Verify(a => a.CreateRepository<InventoryMovementIndex>(), Times.Exactly(2));
            this.mockInventoryMovementIndexRepository.Verify(a => a.Insert(It.IsAny<InventoryMovementIndex>()), Times.Exactly(2));
        }

        /// <summary>
        /// Movements the registration service update should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationService_UpdateIfMovementDestinationIsNull_Should_RegisterAsync()
        {
            var movement = this.GetMovement("Update", true, StatusType.PROCESSING);
            movement.MovementDestination = null;

            this.mockUnitOfWorkMock.Setup(a => a.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);
            this.mockMovementRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Movement> { movement });
            this.mockUnitOfWorkMock.Setup(m => m.CreateRepository<Owner>()).Returns(this.mockOwnerRepository.Object);
            this.mockUnitOfWorkMock.Setup(m => m.CreateRepository<AttributeEntity>()).Returns(this.mockAttributeRepository.Object);
            this.mockMovementRepository.Setup(a => a.Insert(It.IsAny<Movement>()));
            this.mockUnitOfWorkMock.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));
            this.mockUnitOfWorkMock.Setup(a => a.CreateRepository<InventoryMovementIndex>()).Returns(this.mockInventoryMovementIndexRepository.Object);
            this.mockInventoryMovementIndexRepository.Setup(a => a.Insert(It.IsAny<InventoryMovementIndex>()));

            await this.movementRegistrationService.RegisterMovementAsync(movement, this.mockUnitOfWorkMock.Object, false).ConfigureAwait(false);

            this.mockUnitOfWorkMock.Verify(a => a.CreateRepository<Movement>(), Times.Exactly(3));
            this.mockMovementRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockMovementRepository.Verify(a => a.Insert(It.IsAny<Movement>()), Times.Exactly(2));
            this.mockUnitOfWorkMock.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
            this.mockUnitOfWorkMock.Verify(a => a.CreateRepository<InventoryMovementIndex>(), Times.Exactly(2));
            this.mockInventoryMovementIndexRepository.Verify(a => a.Insert(It.IsAny<InventoryMovementIndex>()), Times.Exactly(2));
        }

        /// <summary>
        /// Movements the registration service delete should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationService_Delete_Should_RegisterAsync()
        {
            var movement = this.GetMovement("Delete", true, StatusType.PROCESSING);

            this.mockUnitOfWorkMock.Setup(a => a.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);
            this.mockMovementRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Movement> { movement });
            this.mockMovementRepository.Setup(a => a.Insert(It.IsAny<Movement>()));
            this.mockUnitOfWorkMock.Setup(m => m.CreateRepository<Owner>()).Returns(this.mockOwnerRepository.Object);
            this.mockUnitOfWorkMock.Setup(m => m.CreateRepository<AttributeEntity>()).Returns(this.mockAttributeRepository.Object);
            this.mockUnitOfWorkMock.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));
            this.mockUnitOfWorkMock.Setup(a => a.CreateRepository<InventoryMovementIndex>()).Returns(this.mockInventoryMovementIndexRepository.Object);
            this.mockInventoryMovementIndexRepository.Setup(a => a.Insert(It.IsAny<InventoryMovementIndex>()));

            await this.movementRegistrationService.RegisterMovementAsync(movement, this.mockUnitOfWorkMock.Object, false).ConfigureAwait(false);

            this.mockUnitOfWorkMock.Verify(a => a.CreateRepository<Movement>(), Times.Exactly(2));
            this.mockMovementRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockMovementRepository.Verify(a => a.Insert(It.IsAny<Movement>()), Times.Exactly(1));
            this.mockUnitOfWorkMock.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Exactly(1));
            this.mockUnitOfWorkMock.Verify(a => a.CreateRepository<InventoryMovementIndex>(), Times.Exactly(1));
            this.mockInventoryMovementIndexRepository.Verify(a => a.Insert(It.IsAny<InventoryMovementIndex>()), Times.Exactly(1));
        }

        /// <summary>
        /// Movements the registration service delete should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationService_Delete_Should_RegisterWithAbsoluteOwnerValueAsync()
        {
            var movement = this.GetMovement("Delete", true, StatusType.PROCESSING);
            movement.Owners.ElementAt(0).OwnershipValueUnit = True.Core.Constants.OwnershipPercentageUnit;

            this.mockUnitOfWorkMock.Setup(a => a.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);
            this.mockMovementRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Movement> { movement });
            this.mockMovementRepository.Setup(a => a.Insert(It.IsAny<Movement>()));
            this.mockUnitOfWorkMock.Setup(m => m.CreateRepository<Owner>()).Returns(this.mockOwnerRepository.Object);
            this.mockUnitOfWorkMock.Setup(m => m.CreateRepository<AttributeEntity>()).Returns(this.mockAttributeRepository.Object);
            this.mockUnitOfWorkMock.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));
            this.mockUnitOfWorkMock.Setup(a => a.CreateRepository<InventoryMovementIndex>()).Returns(this.mockInventoryMovementIndexRepository.Object);
            this.mockInventoryMovementIndexRepository.Setup(a => a.Insert(It.IsAny<InventoryMovementIndex>()));

            await this.movementRegistrationService.RegisterMovementAsync(movement, this.mockUnitOfWorkMock.Object, false).ConfigureAwait(false);

            Assert.AreEqual(1.00M, movement.Owners.ElementAt(0).OwnershipValue);
            this.mockUnitOfWorkMock.Verify(a => a.CreateRepository<Movement>(), Times.Exactly(2));
            this.mockMovementRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockMovementRepository.Verify(a => a.Insert(It.IsAny<Movement>()), Times.Exactly(1));
            this.mockUnitOfWorkMock.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Exactly(1));
            this.mockUnitOfWorkMock.Verify(a => a.CreateRepository<InventoryMovementIndex>(), Times.Exactly(1));
            this.mockInventoryMovementIndexRepository.Verify(a => a.Insert(It.IsAny<InventoryMovementIndex>()), Times.Exactly(1));
        }

        /// <summary>
        /// Movements the registration service delete should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationService_DeleteIfMovementSourceIsNull_Should_RegisterAsync()
        {
            var movement = this.GetMovement("Delete", true, StatusType.PROCESSING);
            movement.MovementSource = null;

            this.mockUnitOfWorkMock.Setup(a => a.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);
            this.mockMovementRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Movement> { movement });
            this.mockMovementRepository.Setup(a => a.Insert(It.IsAny<Movement>()));
            this.mockUnitOfWorkMock.Setup(m => m.CreateRepository<Owner>()).Returns(this.mockOwnerRepository.Object);
            this.mockUnitOfWorkMock.Setup(m => m.CreateRepository<AttributeEntity>()).Returns(this.mockAttributeRepository.Object);
            this.mockUnitOfWorkMock.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));
            this.mockUnitOfWorkMock.Setup(a => a.CreateRepository<InventoryMovementIndex>()).Returns(this.mockInventoryMovementIndexRepository.Object);
            this.mockInventoryMovementIndexRepository.Setup(a => a.Insert(It.IsAny<InventoryMovementIndex>()));

            await this.movementRegistrationService.RegisterMovementAsync(movement, this.mockUnitOfWorkMock.Object, false).ConfigureAwait(false);

            this.mockUnitOfWorkMock.Verify(a => a.CreateRepository<Movement>(), Times.Exactly(2));
            this.mockMovementRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockMovementRepository.Verify(a => a.Insert(It.IsAny<Movement>()), Times.Exactly(1));
            this.mockUnitOfWorkMock.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Exactly(1));
            this.mockUnitOfWorkMock.Verify(a => a.CreateRepository<InventoryMovementIndex>(), Times.Exactly(1));
            this.mockInventoryMovementIndexRepository.Verify(a => a.Insert(It.IsAny<InventoryMovementIndex>()), Times.Exactly(1));
        }

        /// <summary>
        /// Movements the registration service delete should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationService_DeleteIfMovementDestinationIsNull_Should_RegisterAsync()
        {
            var movement = this.GetMovement("Delete", true, StatusType.PROCESSING);
            movement.MovementDestination = null;

            this.mockUnitOfWorkMock.Setup(a => a.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);
            this.mockUnitOfWorkMock.Setup(a => a.CreateRepository<InventoryMovementIndex>()).Returns(this.mockInventoryMovementIndexRepository.Object);
            this.mockMovementRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Movement> { movement });
            this.mockMovementRepository.Setup(a => a.Insert(It.IsAny<Movement>()));
            this.mockUnitOfWorkMock.Setup(m => m.CreateRepository<Owner>()).Returns(this.mockOwnerRepository.Object);
            this.mockUnitOfWorkMock.Setup(m => m.CreateRepository<AttributeEntity>()).Returns(this.mockAttributeRepository.Object);
            this.mockInventoryMovementIndexRepository.Setup(a => a.Insert(It.IsAny<InventoryMovementIndex>()));
            this.mockUnitOfWorkMock.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));

            await this.movementRegistrationService.RegisterMovementAsync(movement, this.mockUnitOfWorkMock.Object, false).ConfigureAwait(false);

            this.mockUnitOfWorkMock.Verify(a => a.CreateRepository<Movement>(), Times.Exactly(2));
            this.mockUnitOfWorkMock.Verify(a => a.CreateRepository<InventoryMovementIndex>(), Times.Exactly(1));
            this.mockMovementRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()), Times.Once);
            this.mockMovementRepository.Verify(a => a.Insert(It.IsAny<Movement>()), Times.Exactly(1));
            this.mockInventoryMovementIndexRepository.Verify(a => a.Insert(It.IsAny<InventoryMovementIndex>()), Times.Exactly(1));
            this.mockUnitOfWorkMock.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Exactly(1));
        }

        /// <summary>
        /// Movements the registration service should update asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationService_Should_UpdateAsync()
        {
            var movementReceived = this.GetMovement("Insert", false, StatusType.PROCESSING);
            movementReceived.IsOfficial = true;
            movementReceived.BackupMovementId = "123";
            movementReceived.GlobalMovementId = "22333";

            var movementToUpdate = this.GetMovement("Insert", false, StatusType.PROCESSING);

            movementToUpdate.IsOfficial = movementReceived.IsOfficial;
            movementToUpdate.BackupMovementId = movementReceived.BackupMovementId;
            movementToUpdate.GlobalMovementId = movementReceived.GlobalMovementId;

            this.mockUnitOfWorkMock.Setup(a => a.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);
            this.mockMovementRepository.Setup(a => a.Update(It.IsAny<Movement>()));
            this.mockUnitOfWorkMock.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));

            await this.movementRegistrationService.UpdateMovementOffChainDbAsync(movementToUpdate, movementReceived, this.mockUnitOfWorkMock.Object).ConfigureAwait(false);

            this.mockUnitOfWorkMock.Verify(a => a.CreateRepository<Movement>(), Times.Once);
            this.mockMovementRepository.Verify(a => a.Update(It.IsAny<Movement>()), Times.Once);
            this.mockUnitOfWorkMock.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Movements the registration service should get movement with sap details asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationService_Should_GetMovementWithSapDetailsAsync()
        {
            var movement = this.GetMovement("Insert", true, StatusType.PROCESSING);
            this.mockUnitOfWorkMock.Setup(a => a.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);
            this.mockMovementRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Movement> { movement });

            await this.movementRegistrationService.GetMovementsWithSapTrackingAsync("1", this.mockUnitOfWorkMock.Object).ConfigureAwait(false);

            this.mockUnitOfWorkMock.Verify(a => a.CreateRepository<Movement>(), Times.Once);
            this.mockMovementRepository.Verify(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>()), Times.Once);
        }

        /// <summary>
        /// Movements the registration strategy validate transfer point should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationStrategy_ValidateTransferPoint_Should_RegisterAsync()
        {
            var movement = this.GetMovement("Insert", true, StatusType.PROCESSING);
            movement.ScenarioId = ScenarioType.OPERATIONAL;
            var mockClient = new Mock<IServiceBusQueueClient>();
            mockClient.Setup(a => a.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()));
            this.mockUnitOfWorkMock.Setup(a => a.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);
            this.mockMovementRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Movement> { movement });
            this.mockUnitOfWorkMock.Setup(m => m.CreateRepository<Owner>()).Returns(this.mockOwnerRepository.Object);
            this.mockUnitOfWorkMock.Setup(m => m.CreateRepository<AttributeEntity>()).Returns(this.mockAttributeRepository.Object);
            this.mockMovementRepository.Setup(a => a.Insert(It.IsAny<Movement>()));
            this.mockUnitOfWorkMock.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));
            this.mockUnitOfWorkMock.Setup(a => a.CreateRepository<InventoryMovementIndex>()).Returns(this.mockInventoryMovementIndexRepository.Object);
            this.mockInventoryMovementIndexRepository.Setup(a => a.Insert(It.IsAny<InventoryMovementIndex>()));
            this.mockAzureClientFactory.Setup(a => a.GetQueueClient(It.IsAny<string>())).Returns(mockClient.Object);
            this.mockMovementRepository.Setup(a => a.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(movement);
            this.mockMovementRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).Verifiable();
            this.mockMovementRepository.Setup(a => a.ExecuteAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).Verifiable();
            var tranasctionId = await this.movementRegistrationService.RegisterMovementAsync(movement, this.mockUnitOfWorkMock.Object, true).ConfigureAwait(false);
            Assert.IsNotNull(tranasctionId);
            mockClient.Verify(a => a.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()), Times.Never);
            this.mockUnitOfWorkMock.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
            this.mockUnitOfWorkMock.Verify(a => a.CreateRepository<InventoryMovementIndex>(), Times.Exactly(1));
            this.mockInventoryMovementIndexRepository.Verify(a => a.Insert(It.IsAny<InventoryMovementIndex>()), Times.Exactly(1));
            this.mockAzureClientFactory.Verify(a => a.GetQueueClient(It.IsAny<string>()), Times.Never);
        }

        /// <summary>
        /// Movements the registration strategy validate transfer point should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationStrategy_ValidateTransferPoint_ShouldNotRegister_ForDeleteAsync()
        {
            var movement = this.GetMovement("Delete", true, StatusType.PROCESSING);
            movement.ScenarioId = ScenarioType.OPERATIONAL;
            var mockClient = new Mock<IServiceBusQueueClient>();
            mockClient.Setup(a => a.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()));
            this.mockUnitOfWorkMock.Setup(a => a.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);
            this.mockMovementRepository.Setup(a => a.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<Movement> { movement });
            this.mockMovementRepository.Setup(a => a.Insert(It.IsAny<Movement>()));
            this.mockUnitOfWorkMock.Setup(m => m.CreateRepository<Owner>()).Returns(this.mockOwnerRepository.Object);
            this.mockUnitOfWorkMock.Setup(m => m.CreateRepository<AttributeEntity>()).Returns(this.mockAttributeRepository.Object);
            this.mockUnitOfWorkMock.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));
            this.mockUnitOfWorkMock.Setup(a => a.CreateRepository<InventoryMovementIndex>()).Returns(this.mockInventoryMovementIndexRepository.Object);
            this.mockInventoryMovementIndexRepository.Setup(a => a.Insert(It.IsAny<InventoryMovementIndex>()));
            this.mockAzureClientFactory.Setup(a => a.GetQueueClient(It.IsAny<string>())).Returns(mockClient.Object);
            this.mockMovementRepository.Setup(a => a.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(movement);
            this.mockMovementRepository.Setup(a => a.ExecuteQueryAsync(It.IsAny<object>(), It.IsAny<Dictionary<string, object>>())).Verifiable();
            var tranasctionId = await this.movementRegistrationService.RegisterMovementAsync(movement, this.mockUnitOfWorkMock.Object, true).ConfigureAwait(false);
            Assert.IsNotNull(tranasctionId);
            mockClient.Verify(a => a.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()), Times.Never);
            this.mockUnitOfWorkMock.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
            this.mockUnitOfWorkMock.Verify(a => a.CreateRepository<InventoryMovementIndex>(), Times.Exactly(1));
            this.mockInventoryMovementIndexRepository.Verify(a => a.Insert(It.IsAny<InventoryMovementIndex>()), Times.Exactly(1));
            this.mockAzureClientFactory.Verify(a => a.GetQueueClient(It.IsAny<string>()), Times.Never);
        }

        /// <summary>
        /// Gets the movement.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="hasPreviousBlockchainTransactionId">if set to <c>true</c> [has previous blockchain transaction identifier].</param>
        /// <param name="blockchainStatus">if set to <c>true</c> [blockchain status].</param>
        /// <returns>The movement.</returns>
        private Movement GetMovement(string eventType, bool hasPreviousBlockchainTransactionId, StatusType blockchainStatus)
        {
            var movement = new Movement
            {
                MovementId = "1",
                MovementTransactionId = 1,
                BlockchainStatus = blockchainStatus,
                MovementSource = new MovementSource
                {
                    SourceNodeId = 300,
                    SourceNode = new Node { NodeId = 300, Name = "Rebombeos de caño limon-ayacucho GCX" },
                    SourceProductId = "1",
                    SourceProduct = new Product { ProductId = "1", Name = "Crudo caño limón (CCL)" },
                },
                MovementDestination = new MovementDestination
                {
                    DestinationNodeId = 100,
                    DestinationNode = new Node { NodeId = 100, Name = "AYACUCHO CRD-GALAN 14" },
                    DestinationProductId = "1",
                    DestinationProduct = new Product { ProductId = "1", Name = "Crudo caño limón (CCL)" },
                },
                NetStandardVolume = Convert.ToDecimal(-1),
                MovementTypeId = 1,
                OperationalDate = new DateTime(2019, 9, 2, 8, 10, 0),
                FileRegistrationTransaction = new FileRegistrationTransaction
                {
                    FileRegistrationId = 1,
                },
                EventType = eventType,
            };

            var owner = new Owner
            {
                OwnerId = 1,
                OwnershipValue = 1,
                OwnershipValueUnit = "BBL",
            };

            movement.Owners.Add(owner);

            if (hasPreviousBlockchainTransactionId)
            {
                movement.PreviousBlockchainMovementTransactionId = Guid.NewGuid();
            }

            return movement;
        }
    }
}
