// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementRegistrationStrategyTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Tests.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Core.Registration;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The file registration transaction service tests.
    /// </summary>
    [TestClass]
    public class MovementRegistrationStrategyTests
    {
        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The unit of work mock.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWorkMock;

        /// <summary>
        /// The movement repository.
        /// </summary>
        private Mock<IRepository<Movement>> mockMovementRepository;

        /// <summary>
        /// The movement registration strategy.
        /// </summary>
        private MovementRegistrationStrategy movementRegistrationStrategy;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger> mockLogger;

        /// <summary>
        /// The mock movement registration service.
        /// </summary>
        private Mock<IMovementRegistrationService> mockMovementRegistrationService;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockMovementRepository = new Mock<IRepository<Movement>>();
            this.mockUnitOfWorkMock = new Mock<IUnitOfWork>();
            this.mockLogger = new Mock<ITrueLogger>();
            this.mockMovementRegistrationService = new Mock<IMovementRegistrationService>();

            this.movementRegistrationStrategy =
                new MovementRegistrationStrategy(
                    this.mockLogger.Object,
                    this.mockAzureClientFactory.Object,
                    this.mockMovementRegistrationService.Object);
        }

        /// <summary>
        /// Movements the registration strategy should insert.
        /// </summary>
        [TestMethod]
        public void MovementRegistrationStrategy_Should_Insert()
        {
            this.mockUnitOfWorkMock.Setup(a => a.CreateRepository<Movement>()).Returns(this.mockMovementRepository.Object);
            var movement = new Movement
            {
                TicketId = 1,
                EventType = "Insert",
                MovementSource = new MovementSource
                {
                    SourceNode = new Node
                    {
                        NodeId = 1,
                    },
                    SourceProduct = new Product
                    {
                        ProductId = "someProduct1",
                    },
                },
                MovementDestination = new MovementDestination
                {
                    DestinationNode = new Node
                    {
                        NodeId = 1,
                    },
                    DestinationProduct = new Product
                    {
                        ProductId = "someProduct1",
                    },
                },
            };
            var ownership = new Ownership
            {
                EventType = "Insert",
            };
            movement.Ownerships.Add(ownership);
            IEnumerable<Movement> movementsToBeRegistered = new List<Movement>
            {
                movement,
            };

            this.mockMovementRepository.Setup(a => a.InsertAll(It.IsAny<List<Movement>>()));

            this.movementRegistrationStrategy.Insert(movementsToBeRegistered, this.mockUnitOfWorkMock.Object);

            this.mockUnitOfWorkMock.Verify(a => a.CreateRepository<Movement>(), Times.Once);
            this.mockMovementRepository.Verify(a => a.InsertAll(It.IsAny<List<Movement>>()), Times.Once);
        }

        /// <summary>
        /// Movements the registration strategy sap movement should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationStrategy_SapMovement_Should_RegisterAsync()
        {
            var movement = this.GetMovement(false, null, EventType.Insert.ToString(), 1);
            movement.GlobalMovementId = null;
            var mockClient = new Mock<IServiceBusQueueClient>();
            mockClient.Setup(a => a.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()));

            this.mockMovementRegistrationService.Setup(a => a.RegisterMovementAsync(It.IsAny<Movement>(), It.IsAny<IUnitOfWork>(), It.IsAny<bool>())).ReturnsAsync(1);
            this.mockAzureClientFactory.Setup(a => a.GetQueueClient(It.IsAny<string>())).Returns(mockClient.Object);
            await this.movementRegistrationStrategy.RegisterAsync(movement, this.mockUnitOfWorkMock.Object).ConfigureAwait(false);

            mockClient.Verify(a => a.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()), Times.Once);
            this.mockMovementRegistrationService.Verify(a => a.RegisterMovementAsync(It.IsAny<Movement>(), It.IsAny<IUnitOfWork>(), It.IsAny<bool>()), Times.Once);
            this.mockAzureClientFactory.Verify(a => a.GetQueueClient(It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Movements the registration strategy backup movement not available should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationStrategy_BackupMovement_NotAvailable_Should_RegisterAsync()
        {
            var movement = this.GetMovement(false, null, EventType.Insert.ToString(), 1);
            var mockClient = new Mock<IServiceBusQueueClient>();
            mockClient.Setup(a => a.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()));

            this.mockMovementRegistrationService.Setup(a => a.GetMovementsWithSapTrackingAsync(It.IsAny<string>(), It.IsAny<IUnitOfWork>())).ReturnsAsync(() => null);
            this.mockMovementRegistrationService.Setup(a => a.RegisterMovementAsync(It.IsAny<Movement>(), It.IsAny<IUnitOfWork>(), It.IsAny<bool>())).ReturnsAsync(1);
            this.mockAzureClientFactory.Setup(a => a.GetQueueClient(It.IsAny<string>())).Returns(mockClient.Object);
            await this.movementRegistrationStrategy.RegisterAsync(movement, this.mockUnitOfWorkMock.Object).ConfigureAwait(false);

            mockClient.Verify(a => a.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()), Times.Once);
            this.mockMovementRegistrationService.Verify(a => a.GetMovementsWithSapTrackingAsync(It.IsAny<string>(), It.IsAny<IUnitOfWork>()), Times.Once);
            this.mockMovementRegistrationService.Verify(a => a.RegisterMovementAsync(It.IsAny<Movement>(), It.IsAny<IUnitOfWork>(), It.IsAny<bool>()), Times.Once);
            this.mockAzureClientFactory.Verify(a => a.GetQueueClient(It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Movements the registration strategy backup delete movementexist not available sap tracking result should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationStrategy_BackupDeleteMovementexist_NotAvailable_SapTrackingResult_Should_RegisterAsync()
        {
            var receivedmovement = this.GetMovement(true, "079", EventType.Insert.ToString(), 1);
            var latestMovementFromTrue = this.GetMovement(false, null, EventType.Insert.ToString(), 2);
            var mockClient = new Mock<IServiceBusQueueClient>();
            mockClient.Setup(a => a.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()));

            this.mockMovementRegistrationService.Setup(a => a.GetMovementsWithSapTrackingAsync(It.IsAny<string>(), It.IsAny<IUnitOfWork>())).ReturnsAsync(new List<Movement>() { latestMovementFromTrue });
            this.mockMovementRegistrationService.Setup(a => a.RegisterMovementAsync(It.IsAny<Movement>(), It.IsAny<IUnitOfWork>(), It.IsAny<bool>())).ReturnsAsync(1);
            this.mockAzureClientFactory.Setup(a => a.GetQueueClient(It.IsAny<string>())).Returns(mockClient.Object);
            await this.movementRegistrationStrategy.RegisterAsync(receivedmovement, this.mockUnitOfWorkMock.Object).ConfigureAwait(false);

            mockClient.Verify(a => a.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()), Times.Once);
            this.mockMovementRegistrationService.Verify(a => a.GetMovementsWithSapTrackingAsync(It.IsAny<string>(), It.IsAny<IUnitOfWork>()), Times.Once);
            this.mockMovementRegistrationService.Verify(a => a.RegisterMovementAsync(It.IsAny<Movement>(), It.IsAny<IUnitOfWork>(), It.IsAny<bool>()), Times.Once);
            this.mockAzureClientFactory.Verify(a => a.GetQueueClient(It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Movements the registration strategy backup insert or update movementexist not available sap tracking result withdifferent volume should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationStrategy_BackupInsertOrUpdateMovementexist_NotAvailable_SapTrackingResult_WithdifferentVolume_Should_RegisterAsync()
        {
            var receivedmovement = this.GetMovement(false, null, EventType.Insert.ToString(), 1);
            var latestMovementFromTrue = this.GetMovement(false, null, EventType.Insert.ToString(), 2);
            var mockClient = new Mock<IServiceBusQueueClient>();
            mockClient.Setup(a => a.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()));

            this.mockMovementRegistrationService.Setup(a => a.GetMovementsWithSapTrackingAsync(It.IsAny<string>(), It.IsAny<IUnitOfWork>())).ReturnsAsync(new List<Movement>() { latestMovementFromTrue });
            this.mockMovementRegistrationService.Setup(a => a.RegisterMovementAsync(It.IsAny<Movement>(), It.IsAny<IUnitOfWork>(), It.IsAny<bool>())).ReturnsAsync(1);
            this.mockAzureClientFactory.Setup(a => a.GetQueueClient(It.IsAny<string>())).Returns(mockClient.Object);
            await this.movementRegistrationStrategy.RegisterAsync(receivedmovement, this.mockUnitOfWorkMock.Object).ConfigureAwait(false);

            mockClient.Verify(a => a.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()), Times.Once);
            this.mockMovementRegistrationService.Verify(a => a.GetMovementsWithSapTrackingAsync(It.IsAny<string>(), It.IsAny<IUnitOfWork>()), Times.Once);
            this.mockMovementRegistrationService.Verify(a => a.RegisterMovementAsync(It.IsAny<Movement>(), It.IsAny<IUnitOfWork>(), It.IsAny<bool>()), Times.Once);
            this.mockAzureClientFactory.Verify(a => a.GetQueueClient(It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Movements the registration strategy backup insert or update movementexist not available sap tracking result withsame volume should update asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationStrategy_BackupInsertOrUpdateMovementexist_NotAvailable_SapTrackingResult_WithsameVolume_Should_UpdateAsync()
        {
            var receivedmovement = this.GetMovement(false, null, EventType.Insert.ToString(), 1);
            var latestMovementFromTrue = this.GetMovement(false, null, EventType.Insert.ToString(), 1);

            this.mockMovementRegistrationService.Setup(a => a.GetMovementsWithSapTrackingAsync(It.IsAny<string>(), It.IsAny<IUnitOfWork>())).ReturnsAsync(new List<Movement>() { latestMovementFromTrue });
            this.mockMovementRegistrationService.Setup(a => a.UpdateMovementOffChainDbAsync(It.IsAny<Movement>(), It.IsAny<Movement>(), It.IsAny<IUnitOfWork>()));
            await this.movementRegistrationStrategy.RegisterAsync(receivedmovement, this.mockUnitOfWorkMock.Object).ConfigureAwait(false);

            this.mockMovementRegistrationService.Verify(a => a.GetMovementsWithSapTrackingAsync(It.IsAny<string>(), It.IsAny<IUnitOfWork>()), Times.Once);
            this.mockMovementRegistrationService.Verify(a => a.UpdateMovementOffChainDbAsync(It.IsAny<Movement>(), It.IsAny<Movement>(), It.IsAny<IUnitOfWork>()), Times.Once);
        }

        /// <summary>
        /// Movements the registration strategy backup movementexist available sap tracking result should update latest asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationStrategy_BackupMovementexist_Available_SapTrackingResult_Should_UpdateLatestAsync()
        {
            var receivedmovement = this.GetMovement(false, null, EventType.Insert.ToString(), 1);
            var movementFromTrue1 = this.GetMovement(false, null, EventType.Insert.ToString(), 100);
            movementFromTrue1.MovementTransactionId = 1;
            SapTracking sapTracking1 = new SapTracking
            {
                MovementTransactionId = 1,
                SapTrackingId = (int)StatusType.PROCESSED,
                OperationalDate = DateTime.Now.AddDays(-1),
            };
            movementFromTrue1.SapTracking.Add(sapTracking1);

            var movementFromTrue2 = this.GetMovement(false, null, EventType.Update.ToString(), 200);
            movementFromTrue2.MovementTransactionId = 2;
            SapTracking sapTracking2 = new SapTracking
            {
                MovementTransactionId = 2,
                SapTrackingId = (int)StatusType.PROCESSED,
                OperationalDate = DateTime.Now,
            };

            movementFromTrue2.SapTracking.Add(sapTracking2);

            this.mockMovementRegistrationService.Setup(a => a.GetMovementsWithSapTrackingAsync(It.IsAny<string>(), It.IsAny<IUnitOfWork>())).ReturnsAsync(new List<Movement>() { movementFromTrue1, movementFromTrue2 });
            this.mockMovementRegistrationService.Setup(a => a.UpdateMovementOffChainDbAsync(It.IsAny<Movement>(), It.IsAny<Movement>(), It.IsAny<IUnitOfWork>()));
            await this.movementRegistrationStrategy.RegisterAsync(receivedmovement, this.mockUnitOfWorkMock.Object).ConfigureAwait(false);

            this.mockMovementRegistrationService.Verify(a => a.GetMovementsWithSapTrackingAsync(It.IsAny<string>(), It.IsAny<IUnitOfWork>()), Times.Once);
            this.mockMovementRegistrationService.Verify(a => a.UpdateMovementOffChainDbAsync(It.IsAny<Movement>(), It.IsAny<Movement>(), It.IsAny<IUnitOfWork>()), Times.Once);
        }

        /// <summary>
        /// Movements the registration strategy official movement not available should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationStrategy_OfficialMovement_NotAvailable_Should_RegisterAsync()
        {
            var movement = this.GetMovement(true, "78", EventType.Insert.ToString(), 1);
            var mockClient = new Mock<IServiceBusQueueClient>();
            mockClient.Setup(a => a.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()));

            this.mockMovementRegistrationService.Setup(a => a.GetMovementsWithSapTrackingAsync(It.IsAny<string>(), It.IsAny<IUnitOfWork>())).ReturnsAsync(() => null);
            this.mockMovementRegistrationService.Setup(a => a.RegisterMovementAsync(It.IsAny<Movement>(), It.IsAny<IUnitOfWork>(), It.IsAny<bool>())).ReturnsAsync(1);
            this.mockAzureClientFactory.Setup(a => a.GetQueueClient(It.IsAny<string>())).Returns(mockClient.Object);
            await this.movementRegistrationStrategy.RegisterAsync(movement, this.mockUnitOfWorkMock.Object).ConfigureAwait(false);

            mockClient.Verify(a => a.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()), Times.Once);
            this.mockMovementRegistrationService.Verify(a => a.GetMovementsWithSapTrackingAsync(It.IsAny<string>(), It.IsAny<IUnitOfWork>()), Times.Once);
            this.mockMovementRegistrationService.Verify(a => a.RegisterMovementAsync(It.IsAny<Movement>(), It.IsAny<IUnitOfWork>(), It.IsAny<bool>()), Times.Once);
            this.mockAzureClientFactory.Verify(a => a.GetQueueClient(It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Movements the registration strategy official delete movementexist not available sap tracking result should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationStrategy_OfficialDeleteMovementexist_NotAvailable_SapTrackingResult_Should_RegisterAsync()
        {
            var receivedmovement = this.GetMovement(true, "8079", EventType.Insert.ToString(), 1);
            var latestMovementFromTrue = this.GetMovement(false, null, EventType.Insert.ToString(), 2);
            var mockClient = new Mock<IServiceBusQueueClient>();
            mockClient.Setup(a => a.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()));

            this.mockMovementRegistrationService.Setup(a => a.GetMovementsWithSapTrackingAsync(It.IsAny<string>(), It.IsAny<IUnitOfWork>())).ReturnsAsync(new List<Movement>() { latestMovementFromTrue });
            this.mockMovementRegistrationService.Setup(a => a.RegisterMovementAsync(It.IsAny<Movement>(), It.IsAny<IUnitOfWork>(), It.IsAny<bool>())).ReturnsAsync(1);
            this.mockAzureClientFactory.Setup(a => a.GetQueueClient(It.IsAny<string>())).Returns(mockClient.Object);
            await this.movementRegistrationStrategy.RegisterAsync(receivedmovement, this.mockUnitOfWorkMock.Object).ConfigureAwait(false);

            mockClient.Verify(a => a.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()), Times.Once);
            this.mockMovementRegistrationService.Verify(a => a.GetMovementsWithSapTrackingAsync(It.IsAny<string>(), It.IsAny<IUnitOfWork>()), Times.Once);
            this.mockMovementRegistrationService.Verify(a => a.RegisterMovementAsync(It.IsAny<Movement>(), It.IsAny<IUnitOfWork>(), It.IsAny<bool>()), Times.Once);
            this.mockAzureClientFactory.Verify(a => a.GetQueueClient(It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Movements the registration strategy official insert or update movementexist not available sap tracking result withdifferent volume should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationStrategy_OfficialInsertOrUpdateMovementexist_NotAvailable_SapTrackingResult_WithdifferentVolume_Should_RegisterAsync()
        {
            var receivedmovement = this.GetMovement(true, "879", EventType.Insert.ToString(), 1);
            var latestMovementFromTrue = this.GetMovement(false, null, EventType.Insert.ToString(), 2);
            var mockClient = new Mock<IServiceBusQueueClient>();
            mockClient.Setup(a => a.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()));

            this.mockMovementRegistrationService.Setup(a => a.GetMovementsWithSapTrackingAsync(It.IsAny<string>(), It.IsAny<IUnitOfWork>())).ReturnsAsync(new List<Movement>() { latestMovementFromTrue });
            this.mockMovementRegistrationService.Setup(a => a.RegisterMovementAsync(It.IsAny<Movement>(), It.IsAny<IUnitOfWork>(), It.IsAny<bool>())).ReturnsAsync(1);
            this.mockAzureClientFactory.Setup(a => a.GetQueueClient(It.IsAny<string>())).Returns(mockClient.Object);
            await this.movementRegistrationStrategy.RegisterAsync(receivedmovement, this.mockUnitOfWorkMock.Object).ConfigureAwait(false);

            mockClient.Verify(a => a.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()), Times.Once);
            this.mockMovementRegistrationService.Verify(a => a.GetMovementsWithSapTrackingAsync(It.IsAny<string>(), It.IsAny<IUnitOfWork>()), Times.Once);
            this.mockMovementRegistrationService.Verify(a => a.RegisterMovementAsync(It.IsAny<Movement>(), It.IsAny<IUnitOfWork>(), It.IsAny<bool>()), Times.Once);
            this.mockAzureClientFactory.Verify(a => a.GetQueueClient(It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Movements the registration strategy official insert or update movementexist not available sap tracking result withsame volume should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationStrategy_OfficialInsertOrUpdateMovementexist_NotAvailable_SapTrackingResult_WithsameVolume_Should_UpdateAsync()
        {
            var receivedmovement = this.GetMovement(true, "2342", EventType.Insert.ToString(), 5);
            var latestMovementFromTrue = this.GetMovement(true, "2342", EventType.Insert.ToString(), 5);

            this.mockMovementRegistrationService.Setup(a => a.GetMovementsWithSapTrackingAsync(It.IsAny<string>(), It.IsAny<IUnitOfWork>())).ReturnsAsync(new List<Movement>() { latestMovementFromTrue });
            this.mockMovementRegistrationService.Setup(a => a.UpdateMovementOffChainDbAsync(It.IsAny<Movement>(), It.IsAny<Movement>(), It.IsAny<IUnitOfWork>()));
            await this.movementRegistrationStrategy.RegisterAsync(receivedmovement, this.mockUnitOfWorkMock.Object).ConfigureAwait(false);

            this.mockMovementRegistrationService.Verify(a => a.GetMovementsWithSapTrackingAsync(It.IsAny<string>(), It.IsAny<IUnitOfWork>()), Times.Once);
            this.mockMovementRegistrationService.Verify(a => a.UpdateMovementOffChainDbAsync(It.IsAny<Movement>(), It.IsAny<Movement>(), It.IsAny<IUnitOfWork>()), Times.Once);
        }

        /// <summary>
        /// Movements the registration strategy official movementexist available sap tracking result should update latest asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationStrategy_OfficialMovementexist_Available_SapTrackingResult_Should_UpdateLatestAsync()
        {
            var receivedmovement = this.GetMovement(true, "242345", EventType.Insert.ToString(), 1);
            var movementFromTrue1 = this.GetMovement(true, null, EventType.Insert.ToString(), 100);
            movementFromTrue1.MovementTransactionId = 1;
            SapTracking sapTracking1 = new SapTracking
            {
                MovementTransactionId = 1,
                SapTrackingId = (int)StatusType.PROCESSED,
                OperationalDate = DateTime.Now.AddDays(-1),
            };
            movementFromTrue1.SapTracking.Add(sapTracking1);

            var movementFromTrue2 = this.GetMovement(false, null, EventType.Update.ToString(), 200);
            movementFromTrue2.MovementTransactionId = 2;
            SapTracking sapTracking2 = new SapTracking
            {
                MovementTransactionId = 2,
                SapTrackingId = (int)StatusType.PROCESSED,
                OperationalDate = DateTime.Now,
            };

            movementFromTrue2.SapTracking.Add(sapTracking2);

            this.mockMovementRegistrationService.Setup(a => a.GetMovementsWithSapTrackingAsync(It.IsAny<string>(), It.IsAny<IUnitOfWork>())).ReturnsAsync(new List<Movement>() { movementFromTrue1, movementFromTrue2 });
            this.mockMovementRegistrationService.Setup(a => a.UpdateMovementOffChainDbAsync(It.IsAny<Movement>(), It.IsAny<Movement>(), It.IsAny<IUnitOfWork>()));
            await this.movementRegistrationStrategy.RegisterAsync(receivedmovement, this.mockUnitOfWorkMock.Object).ConfigureAwait(false);

            this.mockMovementRegistrationService.Verify(a => a.GetMovementsWithSapTrackingAsync(It.IsAny<string>(), It.IsAny<IUnitOfWork>()), Times.Once);
            this.mockMovementRegistrationService.Verify(a => a.UpdateMovementOffChainDbAsync(It.IsAny<Movement>(), It.IsAny<Movement>(), It.IsAny<IUnitOfWork>()), Times.Once);
        }

        /// <summary>
        /// Movements the registration strategy backup delete movementexist not available sap tracking result received insert backup movement should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementRegistrationStrategy_BackupDeleteMovementexist_NotAvailableSapTrackingResult_ReceivedInsertBackupMovement_Should_RegisterAsync()
        {
            var receivedmovement = this.GetMovement(true, "079", EventType.Insert.ToString(), 1);
            var latestMovementFromTrue = this.GetMovement(false, null, EventType.Delete.ToString(), 2);
            var mockClient = new Mock<IServiceBusQueueClient>();
            mockClient.Setup(a => a.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()));

            this.mockMovementRegistrationService.Setup(a => a.GetMovementsWithSapTrackingAsync(It.IsAny<string>(), It.IsAny<IUnitOfWork>())).ReturnsAsync(new List<Movement>() { latestMovementFromTrue });
            this.mockMovementRegistrationService.Setup(a => a.RegisterMovementAsync(It.IsAny<Movement>(), It.IsAny<IUnitOfWork>(), It.IsAny<bool>())).ReturnsAsync(1);
            this.mockAzureClientFactory.Setup(a => a.GetQueueClient(It.IsAny<string>())).Returns(mockClient.Object);
            await this.movementRegistrationStrategy.RegisterAsync(receivedmovement, this.mockUnitOfWorkMock.Object).ConfigureAwait(false);

            mockClient.Verify(a => a.QueueSessionMessageAsync(It.IsAny<object>(), It.IsAny<string>()), Times.Once);
            this.mockMovementRegistrationService.Verify(a => a.GetMovementsWithSapTrackingAsync(It.IsAny<string>(), It.IsAny<IUnitOfWork>()), Times.Once);
            this.mockMovementRegistrationService.Verify(a => a.RegisterMovementAsync(It.IsAny<Movement>(), It.IsAny<IUnitOfWork>(), It.IsAny<bool>()), Times.Once);
            this.mockAzureClientFactory.Verify(a => a.GetQueueClient(It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Gets the movement.
        /// </summary>
        /// <param name="isOfficial">Type of the movement official or backup.</param>
        /// <param name="backupMovementId">if set to <c>true</c> [has previous blockchain transaction identifier].</param>
        /// <returns>The movement.</returns>
        private Movement GetMovement(bool isOfficial, string backupMovementId, string eventType, int netVolume)
        {
            var movement = new Movement
            {
                MovementId = "1",
                MovementTransactionId = 1,
                IsOfficial = isOfficial,
                BackupMovementId = backupMovementId,
                GlobalMovementId = "12",
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
                NetStandardVolume = Convert.ToDecimal(netVolume),
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

            return movement;
        }
    }
}
