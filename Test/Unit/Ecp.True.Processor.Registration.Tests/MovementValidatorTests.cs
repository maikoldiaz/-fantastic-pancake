// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementValidatorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Repositories.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Registration;
    using Ecp.True.Processors.Registration.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The Validator tests.
    /// </summary>
    [TestClass]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Test Class")]
    public class MovementValidatorTests
    {
        /// <summary>
        /// The mock movement repo mock.
        /// </summary>
        private Mock<IMovementRepository> mockMovementRepoMock;

        /// <summary>
        /// The composite validator.
        /// </summary>
        private Mock<ICompositeValidatorFactory> compositeValidator;

        /// <summary>
        /// The movement composite validator.
        /// </summary>
        private Mock<ICompositeValidator<Movement>> movementCompositeValidator;

        /// <summary>
        /// The BLOB client mock.
        /// </summary>
        private Mock<IBlobStorageClient> blobClientMock;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private Mock<IAzureClientFactory> azureClientFactory;

        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;

        /// <summary>
        /// The unit of work mock.
        /// </summary>
        private Mock<IUnitOfWork> unitOfWorkMock;

        /// <summary>
        /// The file registration transaction service.
        /// </summary>
        private Mock<IFileRegistrationTransactionService> fileRegistrationService;

        /// <summary>
        /// The registration processor.
        /// </summary>
        private MovementValidator movementValidator;

        /// <summary>
        /// The blob operations.
        /// </summary>
        private Mock<IBlobOperations> blobOperations;

        /// <summary>
        /// The configuration handlermock.
        /// </summary>
        private Mock<IConfigurationHandler> configurationHandlerMock;

        /// <summary>
        /// The mock queue client.
        /// </summary>
        private Mock<IServiceBusQueueClient> mockQueueClient;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<MovementValidator>> logger;

        /// <summary>
        /// The delta node.
        /// </summary>
        private Mock<IRepository<Annulation>> annulationRepository;

        /// <summary>
        /// The mockblob client.
        /// </summary>
        private Mock<IBlobStorageSasClient> mockBlobSasClient;

        /// <summary>
        /// The file Registration transaction Object.
        /// </summary>
        private FileRegistrationTransaction fileRegistrationTransactionObject;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.compositeValidator = new Mock<ICompositeValidatorFactory>();
            this.movementCompositeValidator = new Mock<ICompositeValidator<Movement>>();
            this.blobClientMock = new Mock<IBlobStorageClient>();
            this.unitOfWorkMock = new Mock<IUnitOfWork>();
            this.mockQueueClient = new Mock<IServiceBusQueueClient>();
            this.unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.fileRegistrationService = new Mock<IFileRegistrationTransactionService>();
            this.configurationHandlerMock = new Mock<IConfigurationHandler>();
            this.blobOperations = new Mock<IBlobOperations>();
            this.annulationRepository = new Mock<IRepository<Annulation>>();
            this.logger = new Mock<ITrueLogger<MovementValidator>>();
            this.azureClientFactory = new Mock<IAzureClientFactory>();
            this.mockMovementRepoMock = new Mock<IMovementRepository>();
            this.unitOfWorkMock.Setup(m => m.MovementRepository).Returns(this.mockMovementRepoMock.Object);
            this.unitOfWorkMock.Setup(a => a.CreateRepository<Annulation>()).Returns(this.annulationRepository.Object);
            this.unitOfWorkFactory.Setup(m => m.GetUnitOfWork()).Returns(this.unitOfWorkMock.Object);
            this.mockBlobSasClient = new Mock<IBlobStorageSasClient>();
            this.fileRegistrationTransactionObject = new FileRegistrationTransaction
            {
                ActionType = Entities.Enumeration.FileRegistrationActionType.Insert,
                BlobPath = @"MovementJson/Movement.json",
                CreatedBy = "sysadmin",
                CreatedDate = DateTime.Now,
                FileRegistrationId = 1,
                FileRegistrationTransactionId = 1,
                SystemTypeId = SystemType.SINOPER,
                UploadId = "1",
            };

            this.compositeValidator.Setup(x => x.MovementCompositeValidator).Returns(this.movementCompositeValidator.Object);
            this.azureClientFactory.Setup(x => x.GetBlobStorageSaSClient(It.IsAny<string>(), It.IsAny<string>())).Returns(this.mockBlobSasClient.Object);
            this.azureClientFactory.Setup(a => a.GetQueueClient(It.IsAny<string>())).Returns(this.mockQueueClient.Object);

            var systemConfig = new ServiceBusSettings
            {
                ConnectionString = "TestConnectionString",
            };

            this.configurationHandlerMock.Setup(x => x.GetConfigurationAsync<ServiceBusSettings>(It.IsAny<string>())).ReturnsAsync(systemConfig);

            this.movementValidator = new MovementValidator(this.logger.Object, this.blobOperations.Object, this.compositeValidator.Object, this.unitOfWorkFactory.Object, this.azureClientFactory.Object, this.fileRegistrationService.Object);
        }

        /// <summary>
        /// Validators the should validate with movement asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task MovementValidator_ValidateAsync()
        {
            var filePath = @"MovementJson/Movement.json";
            var movement = this.GetMovementJArray();
            var movementList = movement.ToObject<List<Movement>>();

            this.blobOperations.Setup(x => x.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.GetStreamDetailsForFile(filePath));
            this.blobOperations.Setup(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(movementList[0], new List<string>(), null as object));
            this.movementCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<Movement>())).ReturnsAsync(ValidationResult.Success);

            // Act
            await this.movementValidator.ValidateMovementAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            this.fileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
        }

        /// <summary>
        /// Validators the should validate with movement asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task MovementValidator_ShouldFail_WithMovementAsync()
        {
            var filePath = @"MovementJson/EmptyMovement.json";
            var movement = this.GetMovementJArray();
            var movementList = movement.ToObject<List<Movement>>();

            this.blobOperations.Setup(x => x.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.GetStreamDetailsForFile(filePath));
            this.blobOperations.Setup(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(movementList[0], new List<string>(), null as object));
            this.fileRegistrationTransactionObject.BlobPath = filePath;
            this.movementCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<Movement>())).ReturnsAsync(ValidationResult.Success);

            // Act
            await this.movementValidator.ValidateMovementAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            this.fileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
        }

        /// <summary>
        /// Validators the should validate with movement asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task MovementValidator_ShouldFail_WithFalseValidationStatusMovementAsync()
        {
            var filePath = @"MovementJson/Movement.json";
            var movement = this.GetMovementJArray();
            var movementList = movement.ToObject<List<Movement>>();
            movementList[0].SourceSystemId = 165;

            this.blobOperations.Setup(x => x.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.GetStreamDetailsForFile(filePath));
            this.blobOperations.Setup(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(movementList[0], new List<string>(), null as object));
            this.fileRegistrationTransactionObject.BlobPath = filePath;
            this.movementCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<Movement>())).ReturnsAsync(new ValidationResult() { IsSuccess = false });

            await this.movementValidator.ValidateMovementAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);
        }

        /// <summary>
        /// Inventories the validator insert should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementValidator_ShouldInsert_WhenMovementIsNewAsync()
        {
            var filePath = @"MovementJson/Movement.json";
            var movement = this.GetMovementJArray();
            var movementList = movement.ToObject<List<Movement>>();

            this.blobOperations.Setup(x => x.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.GetStreamDetailsForFile(filePath));
            this.blobOperations.Setup(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(movementList[0], new List<string>(), null as object));

            this.fileRegistrationTransactionObject.BlobPath = filePath;
            this.movementCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<Movement>())).ReturnsAsync(ValidationResult.Success);
            this.mockMovementRepoMock.Setup(a => a.GetLatestMovementAsync(It.IsAny<string>())).ReturnsAsync(default(Movement));

            // Act
            (bool isValid, Movement movementObj) = await this.movementValidator.ValidateMovementAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsTrue(isValid);
            this.mockMovementRepoMock.Verify(a => a.GetLatestMovementAsync(It.IsAny<string>()), Times.Once);
            this.blobOperations.Verify(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>()), Times.Once);
            this.movementCompositeValidator.Verify(x => x.ValidateAsync(It.IsAny<Movement>()), Times.Once);

            this.fileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
        }

        /// <summary>
        /// Inventories the validator insert should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementValidator_ShouldInsert_WhenMovementIsDeletedAsync()
        {
            var filePath = @"MovementJson/Movement.json";
            var movement = this.GetMovementJArray();
            var movementList = movement.ToObject<List<Movement>>();

            this.blobOperations.Setup(x => x.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.GetStreamDetailsForFile(filePath));
            this.blobOperations.Setup(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(movementList[0], new List<string>(), null as object));

            this.fileRegistrationTransactionObject.BlobPath = filePath;
            this.movementCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<Movement>())).ReturnsAsync(ValidationResult.Success);
            this.mockMovementRepoMock.Setup(a => a.GetLatestMovementAsync(It.IsAny<string>())).ReturnsAsync(new Movement { EventType = EventType.Delete.ToString("G"), NetStandardVolume = -10.00M });

            // Act
            (bool isValid, Movement movementObj) = await this.movementValidator.ValidateMovementAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsTrue(isValid);
            this.mockMovementRepoMock.Verify(a => a.GetLatestMovementAsync(It.IsAny<string>()), Times.Once);
            this.blobOperations.Verify(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>()), Times.Once);
            this.movementCompositeValidator.Verify(x => x.ValidateAsync(It.IsAny<Movement>()), Times.Once);

            this.fileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
        }

        /// <summary>
        /// Inventories the validator insert should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementValidator_InsertShouldFail_WhenMovementExistsAsync()
        {
            var filePath = @"MovementJson/Movement.json";
            var movement = this.GetMovementJArray();
            var movementList = movement.ToObject<List<Movement>>();

            this.blobOperations.Setup(x => x.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.GetStreamDetailsForFile(filePath));
            this.blobOperations.Setup(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(movementList[0], new List<string>(), null as object));

            this.fileRegistrationTransactionObject.BlobPath = filePath;
            this.movementCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<Movement>())).ReturnsAsync(ValidationResult.Success);
            this.mockMovementRepoMock.Setup(a => a.GetLatestMovementAsync(It.IsAny<string>())).ReturnsAsync(new Movement { EventType = EventType.Update.ToString("G"), NetStandardVolume = 10.00M });

            // Act
            (bool isValid, Movement movementObj) = await this.movementValidator.ValidateMovementAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsFalse(isValid);
            this.mockMovementRepoMock.Verify(a => a.GetLatestMovementAsync(It.IsAny<string>()), Times.Once);
            this.blobOperations.Verify(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>()), Times.Once);
            this.movementCompositeValidator.Verify(x => x.ValidateAsync(It.IsAny<Movement>()), Times.Once);

            this.fileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        /// <summary>
        /// Inventories the validator insert should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementValidator_ShouldUpdate_WhenLastMovementInInsertAsync()
        {
            var filePath = @"MovementJson/Movement.json";
            var movement = this.GetMovementJArray();
            var movementList = movement.ToObject<List<Movement>>();
            movementList[0].EventType = EventType.Update.ToString("G");

            this.blobOperations.Setup(x => x.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.GetStreamDetailsForFile(filePath));
            this.blobOperations.Setup(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(movementList[0], new List<string>(), null as object));

            this.fileRegistrationTransactionObject.BlobPath = filePath;
            this.movementCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<Movement>())).ReturnsAsync(ValidationResult.Success);
            this.mockMovementRepoMock.Setup(a => a.GetLatestMovementAsync(It.IsAny<string>())).ReturnsAsync(new Movement { EventType = EventType.Insert.ToString("G"), NetStandardVolume = 10.00M });

            // Act
            (bool isValid, Movement movementObj) = await this.movementValidator.ValidateMovementAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsTrue(isValid);
            this.mockMovementRepoMock.Verify(a => a.GetLatestMovementAsync(It.IsAny<string>()), Times.Once);
            this.blobOperations.Verify(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>()), Times.Once);
            this.movementCompositeValidator.Verify(x => x.ValidateAsync(It.IsAny<Movement>()), Times.Once);

            this.fileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
        }

        /// <summary>
        /// Inventories the validator insert should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementValidator_ShouldUpdate_WhenLastMovementInUpdateAsync()
        {
            var filePath = @"MovementJson/Movement.json";
            var movement = this.GetMovementJArray();
            var movementList = movement.ToObject<List<Movement>>();
            movementList[0].EventType = EventType.Update.ToString("G");

            this.blobOperations.Setup(x => x.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.GetStreamDetailsForFile(filePath));
            this.blobOperations.Setup(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(movementList[0], new List<string>(), null as object));

            this.fileRegistrationTransactionObject.BlobPath = filePath;
            this.movementCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<Movement>())).ReturnsAsync(ValidationResult.Success);
            this.mockMovementRepoMock.Setup(a => a.GetLatestMovementAsync(It.IsAny<string>())).ReturnsAsync(new Movement { EventType = EventType.Update.ToString("G"), NetStandardVolume = 10.00M });

            // Act
            (bool isValid, Movement movementObj) = await this.movementValidator.ValidateMovementAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsTrue(isValid);
            this.mockMovementRepoMock.Verify(a => a.GetLatestMovementAsync(It.IsAny<string>()), Times.Once);
            this.blobOperations.Verify(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>()), Times.Once);
            this.movementCompositeValidator.Verify(x => x.ValidateAsync(It.IsAny<Movement>()), Times.Once);

            this.fileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
        }

        /// <summary>
        /// Inventories the validator insert should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementValidator_UpdateShouldFail_WhenMovementIsDeletedAsync()
        {
            var filePath = @"MovementJson/Movement.json";
            var movement = this.GetMovementJArray();
            var movementList = movement.ToObject<List<Movement>>();
            movementList[0].EventType = EventType.Update.ToString("G");

            this.blobOperations.Setup(x => x.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.GetStreamDetailsForFile(filePath));
            this.blobOperations.Setup(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(movementList[0], new List<string>(), null as object));

            this.fileRegistrationTransactionObject.BlobPath = filePath;
            this.movementCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<Movement>())).ReturnsAsync(ValidationResult.Success);
            this.mockMovementRepoMock.Setup(a => a.GetLatestMovementAsync(It.IsAny<string>())).ReturnsAsync(new Movement { EventType = EventType.Delete.ToString("G"), NetStandardVolume = 0.00M });

            // Act
            (bool isValid, Movement movementObj) = await this.movementValidator.ValidateMovementAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsFalse(isValid);
            this.mockMovementRepoMock.Verify(a => a.GetLatestMovementAsync(It.IsAny<string>()), Times.Once);
            this.blobOperations.Verify(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>()), Times.Once);
            this.movementCompositeValidator.Verify(x => x.ValidateAsync(It.IsAny<Movement>()), Times.Once);

            this.fileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        /// <summary>
        /// Inventories the validator insert should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementValidator_UpdateShouldFail_WhenMovementIsNegatedAsync()
        {
            var filePath = @"MovementJson/Movement.json";
            var movement = this.GetMovementJArray();
            var movementList = movement.ToObject<List<Movement>>();
            movementList[0].EventType = EventType.Update.ToString("G");

            this.blobOperations.Setup(x => x.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.GetStreamDetailsForFile(filePath));
            this.blobOperations.Setup(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(movementList[0], new List<string>(), null as object));

            this.fileRegistrationTransactionObject.BlobPath = filePath;
            this.movementCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<Movement>())).ReturnsAsync(ValidationResult.Success);
            this.mockMovementRepoMock.Setup(a => a.GetLatestMovementAsync(It.IsAny<string>())).ReturnsAsync(new Movement { EventType = EventType.Delete.ToString("G"), NetStandardVolume = -10.00M });

            // Act
            (bool isValid, Movement movementObj) = await this.movementValidator.ValidateMovementAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsFalse(isValid);
            this.mockMovementRepoMock.Verify(a => a.GetLatestMovementAsync(It.IsAny<string>()), Times.Once);
            this.blobOperations.Verify(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>()), Times.Once);
            this.movementCompositeValidator.Verify(x => x.ValidateAsync(It.IsAny<Movement>()), Times.Once);

            this.fileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        /// <summary>
        /// Inventories the validator insert should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementValidator_UpdateShouldFail_WhenMovementIsNotPresentAsync()
        {
            var filePath = @"MovementJson/Movement.json";
            var movement = this.GetMovementJArray();
            var movementList = movement.ToObject<List<Movement>>();
            movementList[0].EventType = EventType.Update.ToString("G");

            this.blobOperations.Setup(x => x.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.GetStreamDetailsForFile(filePath));
            this.blobOperations.Setup(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(movementList[0], new List<string>(), null as object));

            this.fileRegistrationTransactionObject.BlobPath = filePath;
            this.movementCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<Movement>())).ReturnsAsync(ValidationResult.Success);
            this.mockMovementRepoMock.Setup(a => a.GetLatestMovementAsync(It.IsAny<string>())).ReturnsAsync(default(Movement));

            // Act
            (bool isValid, Movement movementObj) = await this.movementValidator.ValidateMovementAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsFalse(isValid);
            this.mockMovementRepoMock.Verify(a => a.GetLatestMovementAsync(It.IsAny<string>()), Times.Once);
            this.blobOperations.Verify(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>()), Times.Once);
            this.movementCompositeValidator.Verify(x => x.ValidateAsync(It.IsAny<Movement>()), Times.Once);

            this.fileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        /// <summary>
        /// Inventories the validator insert should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementValidator_ShouldDelete_WhenLastMovementInInsertAsync()
        {
            var filePath = @"MovementJson/Movement.json";
            var movement = this.GetMovementJArray();
            var movementList = movement.ToObject<List<Movement>>();
            movementList[0].EventType = EventType.Delete.ToString("G");

            this.blobOperations.Setup(x => x.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.GetStreamDetailsForFile(filePath));
            this.blobOperations.Setup(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(movementList[0], new List<string>(), null as object));

            this.fileRegistrationTransactionObject.BlobPath = filePath;
            this.movementCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<Movement>())).ReturnsAsync(ValidationResult.Success);
            this.mockMovementRepoMock.Setup(a => a.GetLatestMovementAsync(It.IsAny<string>())).ReturnsAsync(new Movement { EventType = EventType.Insert.ToString("G"), NetStandardVolume = 10.00M });

            // Act
            (bool isValid, Movement movementObj) = await this.movementValidator.ValidateMovementAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsTrue(isValid);
            this.mockMovementRepoMock.Verify(a => a.GetLatestMovementAsync(It.IsAny<string>()), Times.Once);
            this.blobOperations.Verify(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>()), Times.Once);
            this.movementCompositeValidator.Verify(x => x.ValidateAsync(It.IsAny<Movement>()), Times.Once);

            this.fileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
        }

        /// <summary>
        /// Inventories the validator insert should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementValidator_ShouldDelete_WhenLastMovementInUpdateAsync()
        {
            var filePath = @"MovementJson/Movement.json";
            var movement = this.GetMovementJArray();
            var movementList = movement.ToObject<List<Movement>>();
            movementList[0].EventType = EventType.Delete.ToString("G");

            this.blobOperations.Setup(x => x.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.GetStreamDetailsForFile(filePath));
            this.blobOperations.Setup(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(movementList[0], new List<string>(), null as object));

            this.fileRegistrationTransactionObject.BlobPath = filePath;
            this.movementCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<Movement>())).ReturnsAsync(ValidationResult.Success);
            this.mockMovementRepoMock.Setup(a => a.GetLatestMovementAsync(It.IsAny<string>())).ReturnsAsync(new Movement { EventType = EventType.Update.ToString("G"), NetStandardVolume = 10.00M });

            // Act
            (bool isValid, Movement movementObj) = await this.movementValidator.ValidateMovementAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsTrue(isValid);
            this.mockMovementRepoMock.Verify(a => a.GetLatestMovementAsync(It.IsAny<string>()), Times.Once);
            this.blobOperations.Verify(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>()), Times.Once);
            this.movementCompositeValidator.Verify(x => x.ValidateAsync(It.IsAny<Movement>()), Times.Once);

            this.fileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
        }

        /// <summary>
        /// Inventories the validator insert should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementValidator_DeleteShouldFail_WhenMovementIsDeletedAsync()
        {
            var filePath = @"MovementJson/Movement.json";
            var movement = this.GetMovementJArray();
            var movementList = movement.ToObject<List<Movement>>();
            movementList[0].EventType = EventType.Delete.ToString("G");

            this.blobOperations.Setup(x => x.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.GetStreamDetailsForFile(filePath));
            this.blobOperations.Setup(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(movementList[0], new List<string>(), null as object));

            this.fileRegistrationTransactionObject.BlobPath = filePath;
            this.movementCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<Movement>())).ReturnsAsync(ValidationResult.Success);
            this.mockMovementRepoMock.Setup(a => a.GetLatestMovementAsync(It.IsAny<string>())).ReturnsAsync(new Movement { EventType = EventType.Delete.ToString("G"), NetStandardVolume = 0.00M });

            // Act
            (bool isValid, Movement movementObj) = await this.movementValidator.ValidateMovementAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsFalse(isValid);
            this.mockMovementRepoMock.Verify(a => a.GetLatestMovementAsync(It.IsAny<string>()), Times.Once);
            this.blobOperations.Verify(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>()), Times.Once);
            this.movementCompositeValidator.Verify(x => x.ValidateAsync(It.IsAny<Movement>()), Times.Once);

            this.fileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        /// <summary>
        /// Inventories the validator insert should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementValidator_DeleteShouldFail_WhenMovementIsNegatedAsync()
        {
            var filePath = @"MovementJson/Movement.json";
            var movement = this.GetMovementJArray();
            var movementList = movement.ToObject<List<Movement>>();
            movementList[0].EventType = EventType.Delete.ToString("G");
            movementList[0].SourceSystemId = 165;

            this.blobOperations.Setup(x => x.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.GetStreamDetailsForFile(filePath));
            this.blobOperations.Setup(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(movementList[0], new List<string>(), null as object));

            this.fileRegistrationTransactionObject.BlobPath = filePath;
            this.movementCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<Movement>())).ReturnsAsync(ValidationResult.Success);
            this.mockMovementRepoMock.Setup(a => a.GetLatestMovementAsync(It.IsAny<string>())).ReturnsAsync(new Movement { EventType = EventType.Delete.ToString("G"), NetStandardVolume = -10.00M });

            // Act
            (bool isValid, Movement movementObj) = await this.movementValidator.ValidateMovementAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsFalse(isValid);
            this.mockMovementRepoMock.Verify(a => a.GetLatestMovementAsync(It.IsAny<string>()), Times.Once);
            this.blobOperations.Verify(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>()), Times.Once);
            this.movementCompositeValidator.Verify(x => x.ValidateAsync(It.IsAny<Movement>()), Times.Once);

            this.fileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        /// <summary>
        /// Inventories the validator insert should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementValidator_DeleteShouldFail_WhenMovementIsNotPresentAsync()
        {
            var filePath = @"MovementJson/Movement.json";
            var movement = this.GetMovementJArray();
            var movementList = movement.ToObject<List<Movement>>();
            movementList[0].EventType = EventType.Delete.ToString("G");
            movementList[0].SourceSystemId = 165;

            this.blobOperations.Setup(x => x.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.GetStreamDetailsForFile(filePath));
            this.blobOperations.Setup(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(movementList[0], new List<string>(), null as object));

            this.fileRegistrationTransactionObject.BlobPath = filePath;
            this.movementCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<Movement>())).ReturnsAsync(ValidationResult.Success);
            this.mockMovementRepoMock.Setup(a => a.GetLatestMovementAsync(It.IsAny<string>())).ReturnsAsync(default(Movement));

            // Act
            (bool isValid, Movement movementObj) = await this.movementValidator.ValidateMovementAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsFalse(isValid);
            this.mockMovementRepoMock.Verify(a => a.GetLatestMovementAsync(It.IsAny<string>()), Times.Once);
            this.blobOperations.Verify(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>()), Times.Once);
            this.movementCompositeValidator.Verify(x => x.ValidateAsync(It.IsAny<Movement>()), Times.Once);

            this.fileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        /// <summary>
        /// Inventories the validator insert should return false asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task MovementValidator_ShouldFail_WhenNotHasVersionInsertAsync()
        {
            var filePath = @"MovementJson/Movement.json";
            var movement = this.GetMovementJArray();
            var movementList = movement.ToObject<List<Movement>>();
            movementList[0].ScenarioId = ScenarioType.OFFICER;
            movementList[0].EventType = EventType.Insert.ToString("G");
            movementList[0].Version = null;
            movementList[0].SourceSystemId = 165;

            this.blobOperations.Setup(x => x.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.GetStreamDetailsForFile(filePath));
            this.blobOperations.Setup(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(movementList[0], new List<string>(), null as object));

            this.fileRegistrationTransactionObject.BlobPath = filePath;
            this.movementCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<Movement>())).ReturnsAsync(ValidationResult.Success);
            this.mockMovementRepoMock.Setup(a => a.GetLatestMovementAsync(It.IsAny<string>())).ReturnsAsync(new Movement { EventType = EventType.Insert.ToString("G"), NetStandardVolume = 10.00M });

            // Act
            (bool isValid, Movement movementObj) = await this.movementValidator.ValidateMovementAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsFalse(isValid);
            this.mockMovementRepoMock.Verify(a => a.GetLatestMovementAsync(It.IsAny<string>()), Times.Once);
            this.blobOperations.Verify(x => x.GetHomologatedObject<Movement>(It.IsAny<JToken>(), It.IsAny<string>()), Times.Once);
            this.movementCompositeValidator.Verify(x => x.ValidateAsync(It.IsAny<Movement>()), Times.Once);
        }

        private JToken GetStreamDetailsForFile(string filePath)
        {
            var fileText = File.ReadAllText(filePath);
            var fileByteArray = Encoding.UTF8.GetBytes(fileText);
            Stream blobStream = new MemoryStream(fileByteArray);
            return blobStream.DeserializeStream<JToken>();
        }

        /// <summary>
        /// Gets the json array.
        /// </summary>
        /// <returns>Return array.</returns>
        private JArray GetMovementJArray()
        {
            var json = "[{\r\n\t\t\"BlobPath\": \"somePath\",\r\n\"MessageId\":\"279760396583110\",\r\n\"IsMovement\":\"true\",\r\n\t\t\"SourceSystem\": \"SINOPER\",\r\n\t\t\"SourceSystemId\": \"165\",\r\n\t\t\"EventType\": \"INSERT\",\r\n   \"MessageId\": \"279760396583110\",\r\n\t  \"IsMovement\": \"true\",\r\n\t\t\"MovementId\": \"279760396583110\",\r\n\t\t\"MovementTypeId\": \"1\",\r\n\t\t\"OperationalDate\": \"2019-08-21T08:36:00\",\r\n\t\t\"GrossStandardVolume\": null,\r\n\t\t\"NetStandardVolume\": \"195\",\r\n\t\t\"MeasurementUnit\": \"31\",\r\n\t\t\"Scenario\": \"Operativo\",\r\n\t\t\"Observations\": null,\r\n\t\t\"Classification\": \"Movimiento\",\r\n\t\t\"Attributes\": [{\r\n\t\t\t\t\"AttributeId\": 123,\r\n\t\t\t\t\"AttributeValue\": \"195\",\r\n\t\t\t\t\"ValueAttributeUnit\": 100,\r\n\t\t\t\t\"AttributeDescription\": null\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"AttributeId\": 123,\r\n\t\t\t\t\"AttributeValue\": \"ECOPETROL S.A.\",\r\n\t\t\t\t\"ValueAttributeUnit\": 100,\r\n\t\t\t\t\"AttributeDescription\": \"Mayorista\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"AttributeId\": 123,\r\n\t\t\t\t\"AttributeValue\": \"GLP\",\r\n\t\t\t\t\"ValueAttributeUnit\": 100,\r\n\t\t\t\t\"AttributeDescription\": \"Producto Destino\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"Owners\": [{\r\n\t\t\t\t\"OwnerId\": \"1\",\r\n\t\t\t\t\"OwnershipValue\": \"100\",\r\n\t\t\t\t\"OwnershipValueUnit\": \"% volumen\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"OwnerId\": \"1\",\r\n\t\t\t\t\"OwnershipValue\": \"100\",\r\n\t\t\t\t\"OwnershipValueUnit\": \"% volumen\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"Period\": {\r\n\t\t\t\"StartTime\": \"2019-08-21T08:36:00\",\r\n\t\t\t\"EndTime\": \"2019-08-21T23:59:59\"\r\n\t\t},\r\n\t\t\"MovementSource\": {\r\n\t\t\t\"SourceNodeId\": \"8\",\r\n\t\t\t\"SourceStorageLocationId\": null,\r\n\t\t\t\"SourceProductId\": \"GLP\",\r\n\t\t\t\"SourceProductTypeId\": \"1\"\r\n\t\t},\r\n\t\t\"MovementDestination\": {\r\n\t\t\t\"DestinationNodeId\": \"17\",\r\n\t\t\t\"DestinationStorageLocationId\": null,\r\n\t\t\t\"DestinationProductId\": null,\r\n\t\t\t\"DestinationProductTypeId\": null\r\n\t\t}\r\n\t},\r\n\t{\r\n\t\t\"BlobPath\": \"somePath\",\r\n\t\t\"SourceSystem\": \"SINOPER\",\r\n\t\t\"EventType\": \"INSERT\",\r\n\t  \"MessageId\": \"279760396583110\",\r\n\t  \"IsMovement\": \"true\",\r\n\t\t\"MovementId\": \"279760396583110\",\r\n\t\t\"MovementTypeId\": \"1\",\r\n\t\t\"OperationalDate\": \"2019-08-21T08:36:00\",\r\n\t\t\"GrossStandardVolume\": null,\r\n\t\t\"NetStandardVolume\": \"195\",\r\n\t\t\"MeasurementUnit\": \"31\",\r\n\t\t\"Scenario\": \"Operativo\",\r\n\t\t\"Observations\": null,\r\n\t\t\"Classification\": \"Movimiento\",\r\n\t\t\"Attributes\": [{\r\n\t\t\t\t\"AttributeId\": 123,\r\n\t\t\t\t\"AttributeValue\": \"195\",\r\n\t\t\t\t\"ValueAttributeUnit\":100,\r\n\t\t\t\t\"AttributeDescription\": null\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"AttributeId\": 123,\r\n\t\t\t\t\"AttributeValue\": \"ECOPETROL S.A.\",\r\n\t\t\t\t\"ValueAttributeUnit\": 100,\r\n\t\t\t\t\"AttributeDescription\": \"Mayorista\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"AttributeId\": 123,\r\n\t\t\t\t\"AttributeValue\": \"GLP\",\r\n\t\t\t\t\"ValueAttributeUnit\": 100,\r\n\t\t\t\t\"AttributeDescription\": \"Producto Destino\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"Owners\": [{\r\n\t\t\t\t\"OwnerId\": \"1\",\r\n\t\t\t\t\"OwnershipValue\": \"100\",\r\n\t\t\t\t\"OwnershipValueUnit\": \"% volumen\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"OwnerId\": \"1\",\r\n\t\t\t\t\"OwnershipValue\": \"100\",\r\n\t\t\t\t\"OwnershipValueUnit\": \"% volumen\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"Period\": {\r\n\t\t\t\"StartTime\": \"2019-08-21T08:36:00\",\r\n\t\t\t\"EndTime\": \"2019-08-21T23:59:59\"\r\n\t\t},\r\n\t\t\"MovementSource\": {\r\n\t\t\t\"SourceNodeId\": \"8\",\r\n\t\t\t\"SourceStorageLocationId\": null,\r\n\t\t\t\"SourceProductId\": \"GLP\",\r\n\t\t\t\"SourceProductTypeId\": \"1\"\r\n\t\t},\r\n\t\t\"MovementDestination\": {\r\n\t\t\t\"DestinationNodeId\": \"17\",\r\n\t\t\t\"DestinationStorageLocationId\": null,\r\n\t\t\t\"DestinationProductId\": null,\r\n\t\t\t\"DestinationProductTypeId\": null\r\n\t\t}\r\n\t},\r\n\t{\r\n\t\t\"BlobPath\": \"somePath\",\r\n\t\t\"SourceSystem\": \"SINOPER\",\r\n\t\t\"EventType\": \"INSERT\",\r\n\t\t\"MovementId\": \"279760396583111\",\r\n\t  \"MessageId\": \"279760396583111\",\r\n\t  \"IsMovement\": \"true\",\r\n\t\t\"MovementTypeId\": \"1\",\r\n\t\t\"OperationalDate\": \"2019-08-21T08:36:00\",\r\n\t\t\"GrossStandardVolume\": null,\r\n\t\t\"NetStandardVolume\": \"195\",\r\n\t\t\"MeasurementUnit\": \"31\",\r\n\t\t\"Scenario\": \"Operativo\",\r\n\t\t\"Observations\": null,\r\n\t\t\"Classification\": \"Movimiento\",\r\n\t\t\"Attributes\": [{\r\n\t\t\t\t\"AttributeId\": 123,\r\n\t\t\t\t\"AttributeValue\": \"195\",\r\n\t\t\t\t\"ValueAttributeUnit\":100,\r\n\t\t\t\t\"AttributeDescription\": null\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"AttributeId\": 123,\r\n\t\t\t\t\"AttributeValue\": \"ECOPETROL S.A.\",\r\n\t\t\t\t\"ValueAttributeUnit\": 100,\r\n\t\t\t\t\"AttributeDescription\": \"Mayorista\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"AttributeId\": 123,\r\n\t\t\t\t\"AttributeValue\": \"GLP\",\r\n\t\t\t\t\"ValueAttributeUnit\": 100,\r\n\t\t\t\t\"AttributeDescription\": \"Producto Destino\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"Owners\": [{\r\n\t\t\t\t\"OwnerId\": \"1\",\r\n\t\t\t\t\"OwnershipValue\": \"100\",\r\n\t\t\t\t\"OwnershipValueUnit\": \"% volumen\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"OwnerId\": \"1\",\r\n\t\t\t\t\"OwnershipValue\": \"100\",\r\n\t\t\t\t\"OwnershipValueUnit\": \"% volumen\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"Period\": {\r\n\t\t\t\"StartTime\": \"2019-08-21T08:36:00\",\r\n\t\t\t\"EndTime\": \"2019-08-21T23:59:59\"\r\n\t\t},\r\n\t\t\"MovementSource\": {\r\n\t\t\t\"SourceNodeId\": \"8\",\r\n\t\t\t\"SourceStorageLocationId\": null,\r\n\t\t\t\"SourceProductId\": \"GLP\",\r\n\t\t\t\"SourceProductTypeId\": \"1\"\r\n\t\t},\r\n\t\t\"MovementDestination\": {\r\n\t\t\t\"DestinationNodeId\": \"17\",\r\n\t\t\t\"DestinationStorageLocationId\": null,\r\n\t\t\t\"DestinationProductId\": null,\r\n\t\t\t\"DestinationProductTypeId\": null\r\n\t\t}\r\n\t}\r\n]";
            return JArray.Parse(json);
        }
    }
}