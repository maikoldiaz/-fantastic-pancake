// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistrationProcessorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Tests
{
    using System;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Core.Registration;
    using Ecp.True.Processors.Registration.Interfaces;
    using Ecp.True.Processors.Transform.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json.Linq;

    [TestClass]
    public class RegistrationProcessorTests
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<RegistrationProcessor>> mockLogger;

        /// <summary>
        /// The movement validator.
        /// </summary>
        private Mock<IMovementValidator> mockMovementValidator;

        /// <summary>
        /// The inventory validator.
        /// </summary>
        private Mock<IInventoryValidator> mockInventoryValidator;

        /// <summary>
        /// The event validator.
        /// </summary>
        private Mock<IEventValidator> mockEventValidator;

        /// <summary>
        /// The contract validator.
        /// </summary>
        private Mock<IContractValidator> mockContractValidator;

        /// <summary>
        /// The BLOB operations.
        /// </summary>
        private Mock<IBlobOperations> mockBlobOperations;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// Registration strategy factory.
        /// </summary>
        private Mock<IRegistrationStrategyFactory> mockRegistrationStrategyFactory;

        /// <summary>
        /// The mock inventory product registration strategy.
        /// </summary>
        private Mock<InventoryProductRegistrationStrategy> mockInventoryProductRegistrationStrategy;

        /// <summary>
        /// The mock movement registration strategy.
        /// </summary>
        private Mock<MovementRegistrationStrategy> mockMovementRegistrationStrategy;

        /// <summary>
        /// The mock event registration strategy.
        /// </summary>
        private Mock<EventRegistrationStrategy> mockEventRegistrationStrategy;

        /// <summary>
        /// The mock contract registration strategy.
        /// </summary>
        private Mock<ContractRegistrationStrategy> mockContractRegistrationStrategy;

        /// <summary>
        /// The mock contract registration strategy.
        /// </summary>
        private Mock<IRepository<Transformation>> mockTransformationRepository;

        /// <summary>
        /// The mock registration strategy.
        /// </summary>
        private Mock<IRegistrationStrategy> mockRegistrationStrategy;

        /// <summary>
        /// The mockfile registration transaction repository.
        /// </summary>
        private Mock<IRepository<FileRegistrationTransaction>> mockfileRegistrationTransactionRepository;

        /// <summary>
        /// The mockfile registration transaction repository.
        /// </summary>
        private Mock<IRepository<Entities.Admin.FileRegistration>> mockfileRegistrationRepository;

        /// <summary>
        /// The mock pending transaction repository.
        /// </summary>
        private Mock<IRepository<PendingTransaction>> mockPendingTransactionRepository;

        /// <summary>
        /// The file registration transaction service.
        /// </summary>
        private Mock<IFileRegistrationTransactionService> mockFileRegistrationService;

        /// <summary>
        /// The registration processor.
        /// </summary>
        private RegistrationProcessor registrationProcessor;

        /// <summary>
        /// Initilizes this instance.
        /// </summary>
        [TestInitialize]
        public void Initilize()
        {
            this.mockLogger = new Mock<ITrueLogger<RegistrationProcessor>>();
            this.mockMovementValidator = new Mock<IMovementValidator>();
            this.mockInventoryValidator = new Mock<IInventoryValidator>();
            this.mockEventValidator = new Mock<IEventValidator>();
            this.mockContractValidator = new Mock<IContractValidator>();
            this.mockBlobOperations = new Mock<IBlobOperations>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockRegistrationStrategyFactory = new Mock<IRegistrationStrategyFactory>();
            this.mockInventoryProductRegistrationStrategy = new Mock<InventoryProductRegistrationStrategy>();
            this.mockMovementRegistrationStrategy = new Mock<MovementRegistrationStrategy>();
            this.mockEventRegistrationStrategy = new Mock<EventRegistrationStrategy>();
            this.mockContractRegistrationStrategy = new Mock<ContractRegistrationStrategy>();
            this.mockTransformationRepository = new Mock<IRepository<Transformation>>();
            this.mockRegistrationStrategy = new Mock<IRegistrationStrategy>();
            this.mockfileRegistrationTransactionRepository = new Mock<IRepository<FileRegistrationTransaction>>();
            this.mockfileRegistrationRepository = new Mock<IRepository<Entities.Admin.FileRegistration>>();
            this.mockPendingTransactionRepository = new Mock<IRepository<PendingTransaction>>();
            this.mockFileRegistrationService = new Mock<IFileRegistrationTransactionService>();

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);

            this.mockfileRegistrationTransactionRepository.Setup(m => m.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(new FileRegistrationTransaction());
            this.mockfileRegistrationTransactionRepository.Setup(m => m.Update(It.IsAny<FileRegistrationTransaction>()));
            this.mockfileRegistrationRepository.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<Entities.Admin.FileRegistration, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new Entities.Admin.FileRegistration());
            this.mockPendingTransactionRepository.Setup(a => a.Insert(It.IsAny<PendingTransaction>()));

            this.mockUnitOfWork.Setup(m => m.CreateRepository<PendingTransaction>()).Returns(this.mockPendingTransactionRepository.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<FileRegistrationTransaction>()).Returns(this.mockfileRegistrationTransactionRepository.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Entities.Admin.FileRegistration>()).Returns(this.mockfileRegistrationRepository.Object);
            this.mockUnitOfWork.Setup(m => m.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            this.registrationProcessor = new RegistrationProcessor(this.mockLogger.Object, this.mockMovementValidator.Object, this.mockInventoryValidator.Object, this.mockEventValidator.Object, this.mockContractValidator.Object, this.mockBlobOperations.Object, this.mockUnitOfWorkFactory.Object, this.mockRegistrationStrategyFactory.Object, this.mockFileRegistrationService.Object);
        }

        /// <summary>
        /// Registers the movement asynchronous should register movements when invoked asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task RegisterMovementAsync_ShouldRegisterMovements_WhenInvokedAsync()
        {
            var movement = new Movement() { SourceSystemId = 165, MovementId = "1" };

            // Arrange
            this.mockBlobOperations.Setup(m => m.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new JArray());
            this.mockMovementValidator.Setup(m => m.ValidateMovementAsync(It.IsAny<FileRegistrationTransaction>(), It.IsAny<JToken>())).ReturnsAsync((true, movement));
            this.mockRegistrationStrategyFactory.Setup(a => a.MovementRegistrationStrategy.RegisterAsync(It.IsAny<Movement>(), It.IsAny<UnitOfWork>()));
            this.mockMovementRegistrationStrategy.Setup(m => m.RegisterAsync(It.IsAny<object>(), It.IsAny<IUnitOfWork>()));
            this.mockRegistrationStrategyFactory.Setup(m => m.MovementRegistrationStrategy).Returns(this.mockRegistrationStrategy.Object);

            var message = new FileRegistrationTransaction { FileRegistrationTransactionId = 1, BlobPath = "UTBlobPath", ActionType = Entities.Enumeration.FileRegistrationActionType.Insert };

            // Act
            await this.registrationProcessor.RegisterMovementAsync(message).ConfigureAwait(false);

            // Assert
            this.mockBlobOperations.Verify(m => m.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            this.mockMovementValidator.Verify(m => m.ValidateMovementAsync(It.IsAny<FileRegistrationTransaction>(), It.IsAny<JToken>()), Times.Once);
        }

        /// <summary>
        /// Registers the movement asynchronous should register movements with movement message type when invoked asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task RegisterMovementAsync_ShouldRegisterMovements_WithMovementMessageType_WhenInvokedAsync()
        {
            // Arrange
            this.mockBlobOperations.Setup(m => m.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new JArray());
            this.mockMovementValidator.Setup(m => m.ValidateMovementAsync(It.IsAny<FileRegistrationTransaction>(), It.IsAny<JToken>())).ReturnsAsync((true, new Movement { Classification = Ecp.True.Core.Constants.MovementClassification }));
            this.mockRegistrationStrategy.Setup(m => m.RegisterAsync(It.IsAny<Movement>(), It.IsAny<IUnitOfWork>()));
            this.mockRegistrationStrategyFactory.Setup(m => m.MovementRegistrationStrategy).Returns(this.mockRegistrationStrategy.Object);

            var message = new FileRegistrationTransaction { FileRegistrationTransactionId = 1, BlobPath = "UTBlobPath", ActionType = Entities.Enumeration.FileRegistrationActionType.Insert };

            // Act
            await this.registrationProcessor.RegisterMovementAsync(message).ConfigureAwait(false);

            // Assert
            this.mockBlobOperations.Verify(m => m.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            this.mockMovementValidator.Verify(m => m.ValidateMovementAsync(It.IsAny<FileRegistrationTransaction>(), It.IsAny<JToken>()), Times.Once);
            this.mockRegistrationStrategy.Verify(m => m.RegisterAsync(It.Is<Movement>(m => m.MessageTypeId == (int)MessageType.Movement), It.IsAny<IUnitOfWork>()), Times.Once);
        }

        /// <summary>
        /// Registers the movement asynchronous should register movements with movement message type when invoked asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task RegisterMovementAsync_ShouldRegisterMovements_WithSpecialMovementMessageType_WhenInvokedAsync()
        {
            // Arrange
            this.mockUnitOfWork.Setup(s => s.CreateRepository<Transformation>());
            this.mockBlobOperations.Setup(m => m.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new JArray());
            this.mockMovementValidator.Setup(m => m.ValidateMovementAsync(It.IsAny<FileRegistrationTransaction>(), It.IsAny<JToken>())).ReturnsAsync((true, new Movement { Classification = Ecp.True.Core.Constants.SpecialMovementClassification }));
            this.mockRegistrationStrategy.Setup(m => m.RegisterAsync(It.IsAny<Movement>(), It.IsAny<IUnitOfWork>()));
            this.mockRegistrationStrategyFactory.Setup(m => m.MovementRegistrationStrategy).Returns(this.mockRegistrationStrategy.Object);

            var message = new FileRegistrationTransaction { FileRegistrationTransactionId = 1, BlobPath = "UTBlobPath", ActionType = Entities.Enumeration.FileRegistrationActionType.Insert };

            // Act
            await this.registrationProcessor.RegisterMovementAsync(message).ConfigureAwait(false);

            // Assert
            this.mockBlobOperations.Verify(m => m.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            this.mockMovementValidator.Verify(m => m.ValidateMovementAsync(It.IsAny<FileRegistrationTransaction>(), It.IsAny<JToken>()), Times.Once);
            this.mockRegistrationStrategy.Verify(m => m.RegisterAsync(It.Is<Movement>(m => m.MessageTypeId == (int)MessageType.SpecialMovement), It.IsAny<IUnitOfWork>()), Times.Once);
        }

        /// <summary>
        /// Registers the movement asynchronous should register movements with movement message type when invoked asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task RegisterMovementAsync_ShouldRegisterMovements_WithLossMessageType_WhenInvokedAsync()
        {
            // Arrange
            this.mockBlobOperations.Setup(m => m.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new JArray());
            this.mockMovementValidator.Setup(m => m.ValidateMovementAsync(It.IsAny<FileRegistrationTransaction>(), It.IsAny<JToken>())).ReturnsAsync((true, new Movement { Classification = Ecp.True.Core.Constants.LossClassification }));
            this.mockRegistrationStrategy.Setup(m => m.RegisterAsync(It.IsAny<Movement>(), It.IsAny<IUnitOfWork>()));
            this.mockRegistrationStrategyFactory.Setup(m => m.MovementRegistrationStrategy).Returns(this.mockRegistrationStrategy.Object);

            var message = new FileRegistrationTransaction { FileRegistrationTransactionId = 1, BlobPath = "UTBlobPath", ActionType = Entities.Enumeration.FileRegistrationActionType.Insert };

            // Act
            await this.registrationProcessor.RegisterMovementAsync(message).ConfigureAwait(false);

            // Assert
            this.mockBlobOperations.Verify(m => m.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            this.mockMovementValidator.Verify(m => m.ValidateMovementAsync(It.IsAny<FileRegistrationTransaction>(), It.IsAny<JToken>()), Times.Once);
            this.mockRegistrationStrategy.Verify(m => m.RegisterAsync(It.Is<Movement>(m => m.MessageTypeId == (int)MessageType.Loss), It.IsAny<IUnitOfWork>()), Times.Once);
        }

        /// <summary>
        /// Registers the movement asynchronous should log pending transactions when invoked asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task RegisterMovementAsync_ShouldLogPendingTransactions_WhenInvokedAsync()
        {
            var movement = new Movement() { SourceSystemId = 165, MovementId = "1" };

            // Arrange
            this.mockBlobOperations.Setup(m => m.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new JArray());
            this.mockMovementValidator.Setup(m => m.ValidateMovementAsync(It.IsAny<FileRegistrationTransaction>(), It.IsAny<JToken>())).ReturnsAsync((true, movement));
            this.mockRegistrationStrategyFactory.Setup(a => a.MovementRegistrationStrategy.RegisterAsync(It.IsAny<Movement>(), It.IsAny<UnitOfWork>()));
            this.mockfileRegistrationTransactionRepository.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<FileRegistrationTransaction, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new FileRegistrationTransaction { FileRegistration = new Entities.Admin.FileRegistration() });
            this.mockMovementRegistrationStrategy.Setup(m => m.RegisterAsync(It.IsAny<object>(), It.IsAny<IUnitOfWork>()));
            this.mockRegistrationStrategyFactory.Setup(m => m.MovementRegistrationStrategy).Throws(new Exception(string.Empty));
            this.mockFileRegistrationService.Setup(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()));

            var message = new FileRegistrationTransaction { FileRegistrationTransactionId = 1, BlobPath = "UTBlobPath", ActionType = Entities.Enumeration.FileRegistrationActionType.Insert };

            // Act
            await this.registrationProcessor.RegisterMovementAsync(message).ConfigureAwait(false);

            // Assert
            this.mockBlobOperations.Verify(m => m.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            this.mockMovementValidator.Verify(m => m.ValidateMovementAsync(It.IsAny<FileRegistrationTransaction>(), It.IsAny<JToken>()), Times.Once);
            this.mockFileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Registers the movement asynchronous should throw argument exception when message is null asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task RegisterMovementAsync_ShouldThrowArgumentException_WhenMessageIsNullAsync()
        {
            await this.registrationProcessor.RegisterMovementAsync(null).ConfigureAwait(false);
        }

        /// <summary>
        /// Registers the movement asynchronous should throw argument exception when message BLOB path is null asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task RegisterMovementAsync_ShouldThrowArgumentException_WhenMessageBlobPathIsNullAsync()
        {
            await this.registrationProcessor.RegisterMovementAsync(new FileRegistrationTransaction { SessionId = "test", UploadId = "test", FileRegistrationTransactionId = 1 }).ConfigureAwait(false);
        }

        /// <summary>
        /// Registers the inventory asynchronous should register inventory asynchronous when invoked asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task RegisterInventoryAsync_ShouldRegisterInventoryAsync_WhenInvokedAsync()
        {
            // Arrange
            var json = "{'SourceSystemId': 'ROMSSGRB','DestinationSystem': 'TRUE', 'EventType': 'Update', 'InventoryId': '21312322','InventoryDate': '2020-09-01T00:00:00', " +
                      "'NodeId': '123', 'ScenarioId': '1.0', 'Observations': 'Reporte Operativo Cusiana -Fecha', 'UncertaintyPercentage': '0.21', 'ProductId': '200033', " +
                      "'ProductType': '45', 'ProductVolume': '1234567.89', 'GrossStandardQuantity': '120.0', 'MeasurementUnit': '31', 'BatchId': 'fprtri0usy', 'TankName': '', " +
                      "'Version': '', 'SystemId': '123', 'OperatorId': '', 'OriginalId': '21312322' }";
            var homologatedToken = JObject.Parse(json);

            this.mockBlobOperations.Setup(m => m.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(homologatedToken);
            this.mockUnitOfWork.Setup(s => s.CreateRepository<Transformation>()).Returns(this.mockTransformationRepository.Object);
            this.mockInventoryValidator.Setup(m => m.ValidateInventoryAsync(It.IsAny<FileRegistrationTransaction>(), It.IsAny<JToken>())).ReturnsAsync((true, new InventoryProduct()));
            this.mockRegistrationStrategyFactory.Setup(a => a.InventoryProductRegistrationStrategy.RegisterAsync(It.IsAny<InventoryProduct>(), It.IsAny<UnitOfWork>()));
            this.mockInventoryProductRegistrationStrategy.Setup(m => m.RegisterAsync(It.IsAny<object>(), It.IsAny<IUnitOfWork>()));
            this.mockRegistrationStrategyFactory.Setup(m => m.InventoryProductRegistrationStrategy).Returns(this.mockRegistrationStrategy.Object);

            var message = new FileRegistrationTransaction { FileRegistrationTransactionId = 1, BlobPath = "UTBlobPath", ActionType = Entities.Enumeration.FileRegistrationActionType.Insert };

            // Act
            await this.registrationProcessor.RegisterInventoryAsync(message).ConfigureAwait(false);

            // Assert
            this.mockBlobOperations.Verify(m => m.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            this.mockInventoryValidator.Verify(m => m.ValidateInventoryAsync(It.IsAny<FileRegistrationTransaction>(), It.IsAny<JToken>()), Times.Once);
        }

        /// <summary>
        /// Registers the inventory asynchronous should log pending transactions when invoked asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task RegisterInventoryAsync_ShouldLogPendingTransactions_WhenInvokedAsync()
        {
            var json = "{'SourceSystemId': '165','DestinationSystem': 'TRUE', 'EventType': 'Update', 'InventoryId': '21312322','InventoryDate': '2020-09-01T00:00:00', " +
                        "'NodeId': '123', 'ScenarioId': '1.0', 'Observations': 'Reporte Operativo Cusiana -Fecha', 'UncertaintyPercentage': '0.21', 'ProductId': '200033', " +
                        "'ProductType': '45', 'ProductVolume': '1234567.89', 'GrossStandardQuantity': '120.0', 'MeasurementUnit': '31', 'BatchId': 'fprtri0usy', 'TankName': '', " +
                        "'Version': '', 'SystemId': '123', 'OperatorId': '', 'OriginalId': '21312322' }";
            var homologatedToken = JObject.Parse(json);
            var inventoryProduct = new InventoryProduct() { SourceSystemId = 165 , InventoryId = "1" };

            // Arrange
            this.mockBlobOperations.Setup(m => m.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(homologatedToken);
            this.mockUnitOfWork.Setup(s => s.CreateRepository<Transformation>()).Returns(this.mockTransformationRepository.Object);
            this.mockInventoryValidator.Setup(m => m.ValidateInventoryAsync(It.IsAny<FileRegistrationTransaction>(), It.IsAny<JToken>())).ReturnsAsync((true, inventoryProduct));
            this.mockRegistrationStrategyFactory.Setup(a => a.InventoryProductRegistrationStrategy.RegisterAsync(It.IsAny<InventoryProduct>(), It.IsAny<UnitOfWork>()));
            this.mockInventoryProductRegistrationStrategy.Setup(m => m.RegisterAsync(It.IsAny<object>(), It.IsAny<IUnitOfWork>()));
            this.mockRegistrationStrategyFactory.Setup(m => m.InventoryProductRegistrationStrategy).Throws(new Exception(string.Empty));
            this.mockFileRegistrationService.Setup(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()));

            var message = new FileRegistrationTransaction { FileRegistrationTransactionId = 1, BlobPath = "UTBlobPath", ActionType = Entities.Enumeration.FileRegistrationActionType.Insert, };
            message.Inventories.Add(inventoryProduct);

            // Act
            await this.registrationProcessor.RegisterInventoryAsync(message).ConfigureAwait(false);

            // Assert
            this.mockBlobOperations.Verify(m => m.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            this.mockInventoryValidator.Verify(m => m.ValidateInventoryAsync(It.IsAny<FileRegistrationTransaction>(), It.IsAny<JToken>()), Times.Once);
            this.mockFileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Registers the inventory asynchronous should throw argument exception when message is null asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task RegisterInventoryAsync_ShouldThrowArgumentException_WhenMessageIsNullAsync()
        {
            await this.registrationProcessor.RegisterInventoryAsync(null).ConfigureAwait(false);
        }

        /// <summary>
        /// Registers the inventory asynchronous should throw argument exception when message BLOB path is null asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task RegisterInventoryAsync_ShouldThrowArgumentException_WhenMessageBlobPathIsNullAsync()
        {
            await this.registrationProcessor.RegisterInventoryAsync(new FileRegistrationTransaction { SessionId = "test", UploadId = "test", FileRegistrationTransactionId = 1 }).ConfigureAwait(false);
        }

        /// <summary>
        /// Registers the event asynchronous should register event when invoked asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task RegisterEventAsync_ShouldRegisterEvent_WhenInvokedAsync()
        {
            // Arrange
            this.mockBlobOperations.Setup(m => m.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new JArray());
            this.mockEventValidator.Setup(m => m.ValidateEventAsync(It.IsAny<FileRegistrationTransaction>(), It.IsAny<JToken>())).ReturnsAsync((true, new Event()));
            this.mockRegistrationStrategyFactory.Setup(a => a.EventRegistrationStrategy.RegisterAsync(It.IsAny<Event>(), It.IsAny<UnitOfWork>()));
            this.mockEventRegistrationStrategy.Setup(m => m.RegisterAsync(It.IsAny<object>(), It.IsAny<IUnitOfWork>()));
            this.mockRegistrationStrategyFactory.Setup(m => m.EventRegistrationStrategy).Returns(this.mockRegistrationStrategy.Object);

            var message = new FileRegistrationTransaction { FileRegistrationTransactionId = 1, BlobPath = "UTBlobPath", ActionType = Entities.Enumeration.FileRegistrationActionType.Insert };

            // Act
            await this.registrationProcessor.RegisterEventAsync(message).ConfigureAwait(false);

            // Assert
            this.mockBlobOperations.Verify(m => m.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            this.mockEventValidator.Verify(m => m.ValidateEventAsync(It.IsAny<FileRegistrationTransaction>(), It.IsAny<JToken>()), Times.Once);
        }

        /// <summary>
        /// Registers the event asynchronous should log pending transactions when invoked asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task RegisterEventAsync_ShouldLogPendingTransactions_WhenInvokedAsync()
        {
            // Arrange
            this.mockBlobOperations.Setup(m => m.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new JArray());
            this.mockEventValidator.Setup(m => m.ValidateEventAsync(It.IsAny<FileRegistrationTransaction>(), It.IsAny<JToken>())).ReturnsAsync((true, new Event()));
            this.mockRegistrationStrategyFactory.Setup(a => a.EventRegistrationStrategy.RegisterAsync(It.IsAny<Event>(), It.IsAny<UnitOfWork>()));
            this.mockEventRegistrationStrategy.Setup(m => m.RegisterAsync(It.IsAny<object>(), It.IsAny<IUnitOfWork>()));
            this.mockRegistrationStrategyFactory.Setup(m => m.EventRegistrationStrategy).Throws(new Exception(string.Empty));
            this.mockFileRegistrationService.Setup(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()));

            var message = new FileRegistrationTransaction { FileRegistrationTransactionId = 1, BlobPath = "UTBlobPath", ActionType = Entities.Enumeration.FileRegistrationActionType.Insert };

            // Act
            await this.registrationProcessor.RegisterEventAsync(message).ConfigureAwait(false);

            // Assert
            this.mockBlobOperations.Verify(m => m.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            this.mockEventValidator.Verify(m => m.ValidateEventAsync(It.IsAny<FileRegistrationTransaction>(), It.IsAny<JToken>()), Times.Once);
            this.mockFileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Registers the event asynchronous should throw argument exception when message is null asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task RegisterEventAsync_ShouldThrowArgumentException_WhenMessageIsNullAsync()
        {
            await this.registrationProcessor.RegisterEventAsync(null).ConfigureAwait(false);
        }

        /// <summary>
        /// Registers the event asynchronous should throw argument exception when message BLOB path is null asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task RegisterEventAsync_ShouldThrowArgumentException_WhenMessageBlobPathIsNullAsync()
        {
            await this.registrationProcessor.RegisterEventAsync(new FileRegistrationTransaction { SessionId = "test", UploadId = "test", FileRegistrationTransactionId = 1 }).ConfigureAwait(false);
        }

        /// <summary>
        /// Registers the contract asynchronous should register contract when invoked asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task RegisterContractAsync_ShouldRegisterContract_WhenInvokedAsync()
        {
            // Arrange
            this.mockBlobOperations.Setup(m => m.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new JArray());
            this.mockContractValidator.Setup(m => m.ValidateContractAsync(It.IsAny<FileRegistrationTransaction>(), It.IsAny<JToken>())).ReturnsAsync((true, new Contract()));
            this.mockRegistrationStrategyFactory.Setup(a => a.ContractRegistrationStrategy.RegisterAsync(It.IsAny<Contract>(), It.IsAny<UnitOfWork>()));
            this.mockContractRegistrationStrategy.Setup(m => m.RegisterAsync(It.IsAny<object>(), It.IsAny<IUnitOfWork>()));
            this.mockRegistrationStrategyFactory.Setup(m => m.ContractRegistrationStrategy).Returns(this.mockRegistrationStrategy.Object);

            var message = new FileRegistrationTransaction { FileRegistrationTransactionId = 1, BlobPath = "UTBlobPath", ActionType = Entities.Enumeration.FileRegistrationActionType.Insert };

            // Act
            await this.registrationProcessor.RegisterContractAsync(message).ConfigureAwait(false);

            // Assert
            this.mockBlobOperations.Verify(m => m.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            this.mockContractValidator.Verify(m => m.ValidateContractAsync(It.IsAny<FileRegistrationTransaction>(), It.IsAny<JToken>()), Times.Once);
        }

        /// <summary>
        /// Registers the contract asynchronous should log pending transactions when invoked asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        public async Task RegisterContractAsync_ShouldLogPendingTransactions_WhenInvokedAsync()
        {
            // Arrange
            this.mockBlobOperations.Setup(m => m.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new JArray());
            this.mockContractValidator.Setup(m => m.ValidateContractAsync(It.IsAny<FileRegistrationTransaction>(), It.IsAny<JToken>())).ReturnsAsync((true, new Contract()));
            this.mockRegistrationStrategyFactory.Setup(a => a.ContractRegistrationStrategy.RegisterAsync(It.IsAny<Contract>(), It.IsAny<UnitOfWork>()));
            this.mockContractRegistrationStrategy.Setup(m => m.RegisterAsync(It.IsAny<object>(), It.IsAny<IUnitOfWork>()));
            this.mockRegistrationStrategyFactory.Setup(m => m.ContractRegistrationStrategy).Throws(new Exception(string.Empty));
            this.mockFileRegistrationService.Setup(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()));

            var message = new FileRegistrationTransaction { FileRegistrationTransactionId = 1, BlobPath = "UTBlobPath", ActionType = Entities.Enumeration.FileRegistrationActionType.Insert };

            // Act
            await this.registrationProcessor.RegisterContractAsync(message).ConfigureAwait(false);

            // Assert
            this.mockBlobOperations.Verify(m => m.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            this.mockContractValidator.Verify(m => m.ValidateContractAsync(It.IsAny<FileRegistrationTransaction>(), It.IsAny<JToken>()), Times.Once);
            this.mockFileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Registers the contract asynchronous should throw argument exception when message is null asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task RegisterContractAsync_ShouldThrowArgumentException_WhenMessageIsNullAsync()
        {
            await this.registrationProcessor.RegisterContractAsync(null).ConfigureAwait(false);
        }

        /// <summary>
        /// Registers the contract asynchronous should throw argument exception when message BLOB path is null asynchronous.
        /// </summary>
        /// <returns> The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task RegisterContractAsync_ShouldThrowArgumentException_WhenMessageBlobPathIsNullAsync()
        {
            await this.registrationProcessor.RegisterContractAsync(new FileRegistrationTransaction { SessionId = "test", UploadId = "test", FileRegistrationTransactionId = 1 }).ConfigureAwait(false);
        }
    }
}
