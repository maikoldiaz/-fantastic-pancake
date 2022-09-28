// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryValidatorTests.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core.Entities;
    using Ecp.True.DataAccess.Interfaces;
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
    public class InventoryValidatorTests
    {
        /// <summary>
        /// The file registration transaction mock.
        /// </summary>
        private Mock<IInventoryProductRepository> mockInventoryProductRepoMock;

        /// <summary>
        /// The composite validator.
        /// </summary>
        private Mock<ICompositeValidatorFactory> mockCompositeValidator;

        /// <summary>
        /// The movement composite validator.
        /// </summary>
        private Mock<ICompositeValidator<InventoryProduct>> mockInventoryCompositeValidator;

        /// <summary>
        /// The BLOB client mock.
        /// </summary>
        private Mock<IBlobStorageClient> mockBlobClientMock;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureClientFactory;

        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The unit of work mock.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The file registration transaction service.
        /// </summary>
        private Mock<IFileRegistrationTransactionService> mockFileRegistrationService;

        /// <summary>
        /// The blob operations.
        /// </summary>
        private Mock<IBlobOperations> mockBlobOperations;

        /// <summary>
        /// The configuration handlermock.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigurationHandlerMock;

        /// <summary>
        /// The mock queue client.
        /// </summary>
        private Mock<IServiceBusQueueClient> mockQueueClient;

        /// <summary>
        /// The registration processor.
        /// </summary>
        private InventoryValidator inventoryValidator;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<InventoryValidator>> mockLogger;

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
            this.mockCompositeValidator = new Mock<ICompositeValidatorFactory>();
            this.mockInventoryCompositeValidator = new Mock<ICompositeValidator<InventoryProduct>>();
            this.mockBlobClientMock = new Mock<IBlobStorageClient>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockQueueClient = new Mock<IServiceBusQueueClient>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockFileRegistrationService = new Mock<IFileRegistrationTransactionService>();
            this.mockConfigurationHandlerMock = new Mock<IConfigurationHandler>();
            this.mockBlobOperations = new Mock<IBlobOperations>();
            this.mockLogger = new Mock<ITrueLogger<InventoryValidator>>();
            this.mockAzureClientFactory = new Mock<IAzureClientFactory>();
            this.mockInventoryProductRepoMock = new Mock<IInventoryProductRepository>();
            this.mockUnitOfWork.Setup(m => m.InventoryProductRepository).Returns(this.mockInventoryProductRepoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
            this.mockBlobSasClient = new Mock<IBlobStorageSasClient>();
            this.fileRegistrationTransactionObject = new FileRegistrationTransaction
            {
                ActionType = Entities.Enumeration.FileRegistrationActionType.Insert,
                BlobPath = @"InventoryJson/Inventory.json",
                CreatedBy = "sysadmin",
                CreatedDate = DateTime.Now,
                FileRegistrationId = 1,
                FileRegistrationTransactionId = 1,
                SystemTypeId = SystemType.SINOPER,
                UploadId = "1",
            };

            this.mockCompositeValidator.Setup(x => x.InventoryCompositeValidator).Returns(this.mockInventoryCompositeValidator.Object);
            this.mockAzureClientFactory.Setup(x => x.GetBlobStorageSaSClient(It.IsAny<string>(), It.IsAny<string>())).Returns(this.mockBlobSasClient.Object);
            this.mockAzureClientFactory.Setup(a => a.GetQueueClient(It.IsAny<string>())).Returns(this.mockQueueClient.Object);

            var systemConfig = new ServiceBusSettings
            {
                ConnectionString = "TestConnectionString",
            };

            this.mockConfigurationHandlerMock.Setup(x => x.GetConfigurationAsync<ServiceBusSettings>(It.IsAny<string>())).ReturnsAsync(systemConfig);

            this.inventoryValidator = new InventoryValidator(
                this.mockLogger.Object,
                this.mockBlobOperations.Object,
                this.mockCompositeValidator.Object,
                this.mockUnitOfWorkFactory.Object,
                this.mockAzureClientFactory.Object,
                this.mockFileRegistrationService.Object);
        }

        /// <summary>
        /// Inventories the validator insert should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task InventoryValidator_Insert_ShouldReturnTrueAsync()
        {
            var inventory = JObject.Parse(FormJson("Insert", 123, "10000002372"));
            var inventoryProduct = inventory.ToObject<InventoryProduct>();
            this.mockInventoryProductRepoMock.Setup(a => a.GetLatestInventoryProductAsync(It.IsAny<string>())).ReturnsAsync(default(InventoryProduct));
            this.mockBlobOperations.Setup(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(inventoryProduct, new List<string>(), null as object));
            this.mockInventoryCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<InventoryProduct>())).ReturnsAsync(ValidationResult.Success);

            // Act
            (bool isValid, InventoryProduct inventoryObj) = await this.inventoryValidator.ValidateInventoryAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsTrue(isValid);
            this.mockInventoryProductRepoMock.Verify(a => a.GetLatestInventoryProductAsync(It.IsAny<string>()), Times.Once);
            this.mockBlobOperations.Verify(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>()), Times.Once);
            this.mockInventoryCompositeValidator.Verify(x => x.ValidateAsync(It.IsAny<InventoryProduct>()), Times.Once);

            this.mockFileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
        }

        /// <summary>
        /// Inventories the validator insert should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task InventoryValidator_ShouldInsert_WhenInventoryProductIsDeletedAsync()
        {
            var inventory = JObject.Parse(FormJson("Insert", 123, "10000002372"));
            var inventoryProduct = inventory.ToObject<InventoryProduct>();
            this.mockInventoryProductRepoMock.Setup(a => a.GetLatestInventoryProductAsync(It.IsAny<string>())).ReturnsAsync(new InventoryProduct { EventType = EventType.Delete.ToString("G"), ProductVolume = -10.00M });
            this.mockBlobOperations.Setup(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(inventoryProduct, new List<string>(), null as object));
            this.mockInventoryCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<InventoryProduct>())).ReturnsAsync(ValidationResult.Success);

            // Act
            (bool isValid, InventoryProduct inventoryObj) = await this.inventoryValidator.ValidateInventoryAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsTrue(isValid);
            this.mockInventoryProductRepoMock.Verify(a => a.GetLatestInventoryProductAsync(It.IsAny<string>()), Times.Once);
            this.mockBlobOperations.Verify(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>()), Times.Once);
            this.mockInventoryCompositeValidator.Verify(x => x.ValidateAsync(It.IsAny<InventoryProduct>()), Times.Once);

            this.mockFileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
        }

        /// <summary>
        /// Inventories the validator insert should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task InventoryValidator_InsertShouldFail_WhenInventoryProductIsNotDeletedAsync()
        {
            var inventory = JObject.Parse(FormJson("Insert", 123, "10000002372"));
            var inventoryProduct = inventory.ToObject<InventoryProduct>();
            this.mockInventoryProductRepoMock.Setup(a => a.GetLatestInventoryProductAsync(It.IsAny<string>())).ReturnsAsync(new InventoryProduct { EventType = EventType.Update.ToString("G"), ProductVolume = 10.00M });
            this.mockBlobOperations.Setup(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(inventoryProduct, new List<string>(), null as object));
            this.mockInventoryCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<InventoryProduct>())).ReturnsAsync(ValidationResult.Success);

            // Act
            (bool isValid, InventoryProduct inventoryObj) = await this.inventoryValidator.ValidateInventoryAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsFalse(isValid);
            this.mockInventoryProductRepoMock.Verify(a => a.GetLatestInventoryProductAsync(It.IsAny<string>()), Times.Once);
            this.mockFileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        /// <summary>
        /// Inventories the validator update should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task InventoryValidator_Update_ShouldReturnTrueAsync()
        {
            var inventory = JObject.Parse(FormJson("Update", 123, "10000002372"));
            var inventoryProduct = inventory.ToObject<InventoryProduct>();
            this.mockInventoryProductRepoMock.Setup(a => a.GetLatestInventoryProductAsync(It.IsAny<string>())).ReturnsAsync(new InventoryProduct { ProductVolume = 10000.00M, EventType = EventType.Insert.ToString("G") });
            this.mockBlobOperations.Setup(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(inventoryProduct, new List<string>(), null as object));
            this.mockInventoryCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<InventoryProduct>())).ReturnsAsync(ValidationResult.Success);

            // Act
            (bool isValid, InventoryProduct inventoryObj) = await this.inventoryValidator.ValidateInventoryAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsTrue(isValid);
            this.mockInventoryProductRepoMock.Verify(a => a.GetLatestInventoryProductAsync(It.IsAny<string>()), Times.Once);
            this.mockBlobOperations.Verify(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>()), Times.Once);
            this.mockInventoryCompositeValidator.Verify(x => x.ValidateAsync(It.IsAny<InventoryProduct>()), Times.Once);

            this.mockFileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
        }

        /// <summary>
        /// Inventories the validator update should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task InventoryValidator_UpdateShouldFail_WhenLastRecordIsDeleteAsync()
        {
            var inventory = JObject.Parse(FormJson("Update", 123, "10000002372"));
            var inventoryProduct = inventory.ToObject<InventoryProduct>();
            this.mockInventoryProductRepoMock.Setup(a => a.GetLatestInventoryProductAsync(It.IsAny<string>())).ReturnsAsync(new InventoryProduct { ProductVolume = 0.00M, EventType = EventType.Delete.ToString("G") });
            this.mockBlobOperations.Setup(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(inventoryProduct, new List<string>(), null as object));
            this.mockInventoryCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<InventoryProduct>())).ReturnsAsync(ValidationResult.Success);

            // Act
            (bool isValid, InventoryProduct inventoryObj) = await this.inventoryValidator.ValidateInventoryAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsFalse(isValid);

            this.mockInventoryProductRepoMock.Verify(a => a.GetLatestInventoryProductAsync(It.IsAny<string>()), Times.Once);
            this.mockFileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        /// <summary>
        /// Inventories the validator update should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task InventoryValidator_UpdateShouldFail_WhenLastRecordIsNotPresentAsync()
        {
            var inventory = JObject.Parse(FormJson("Update", 123, "10000002372"));
            var inventoryProduct = inventory.ToObject<InventoryProduct>();
            this.mockInventoryProductRepoMock.Setup(a => a.GetLatestInventoryProductAsync(It.IsAny<string>())).ReturnsAsync(default(InventoryProduct));
            this.mockBlobOperations.Setup(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(inventoryProduct, new List<string>(), null as object));
            this.mockInventoryCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<InventoryProduct>())).ReturnsAsync(ValidationResult.Success);

            // Act
            (bool isValid, InventoryProduct inventoryObj) = await this.inventoryValidator.ValidateInventoryAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsFalse(isValid);

            this.mockInventoryProductRepoMock.Verify(a => a.GetLatestInventoryProductAsync(It.IsAny<string>()), Times.Once);
            this.mockFileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        /// <summary>
        /// Inventories the validator update should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task InventoryValidator_UpdateShouldFail_WhenLastRecordIsNegativeAsync()
        {
            var inventory = JObject.Parse(FormJson("Update", 123, "10000002372"));
            var inventoryProduct = inventory.ToObject<InventoryProduct>();
            this.mockInventoryProductRepoMock.Setup(a => a.GetLatestInventoryProductAsync(It.IsAny<string>())).ReturnsAsync(new InventoryProduct { ProductVolume = -10000.00M, EventType = EventType.Delete.ToString("G") });
            this.mockBlobOperations.Setup(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(inventoryProduct, new List<string>(), null as object));
            this.mockInventoryCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<InventoryProduct>())).ReturnsAsync(ValidationResult.Success);

            // Act
            (bool isValid, InventoryProduct inventoryObj) = await this.inventoryValidator.ValidateInventoryAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsFalse(isValid);

            this.mockInventoryProductRepoMock.Verify(a => a.GetLatestInventoryProductAsync(It.IsAny<string>()), Times.Once);
            this.mockFileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        /// <summary>
        /// Inventories the validator delete should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task InventoryValidator_Delete_ShouldReturnTrueAsync()
        {
            var inventory = JObject.Parse(FormJson("Delete", 123, "10000002372"));
            var inventoryProduct = inventory.ToObject<InventoryProduct>();
            this.mockInventoryProductRepoMock.Setup(a => a.GetLatestInventoryProductAsync(It.IsAny<string>())).ReturnsAsync(new InventoryProduct { ProductVolume = 10000.00M, EventType = EventType.Update.ToString("G") });
            this.mockBlobOperations.Setup(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(inventoryProduct, new List<string>(), null as object));
            this.mockInventoryCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<InventoryProduct>())).ReturnsAsync(ValidationResult.Success);

            // Act
            (bool isValid, InventoryProduct inventoryObj) = await this.inventoryValidator.ValidateInventoryAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsTrue(isValid);
            this.mockInventoryProductRepoMock.Verify(a => a.GetLatestInventoryProductAsync(It.IsAny<string>()), Times.Once);
            this.mockBlobOperations.Verify(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>()), Times.Once);
            this.mockInventoryCompositeValidator.Verify(x => x.ValidateAsync(It.IsAny<InventoryProduct>()), Times.Once);

            this.mockFileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
        }

        /// <summary>
        /// Inventories the validator delete should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task InventoryValidator_DeleteShouldFail_WhenLastRecordIsDeleteAsync()
        {
            var inventory = JObject.Parse(FormJson("Delete", 123, "10000002372"));
            var inventoryProduct = inventory.ToObject<InventoryProduct>();
            this.mockInventoryProductRepoMock.Setup(a => a.GetLatestInventoryProductAsync(It.IsAny<string>())).ReturnsAsync(new InventoryProduct { ProductVolume = 0.00M, EventType = EventType.Delete.ToString("G") });
            this.mockBlobOperations.Setup(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(inventoryProduct, new List<string>(), null as object));
            this.mockInventoryCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<InventoryProduct>())).ReturnsAsync(ValidationResult.Success);

            // Act
            (bool isValid, InventoryProduct inventoryObj) = await this.inventoryValidator.ValidateInventoryAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsFalse(isValid);
            this.mockInventoryProductRepoMock.Verify(a => a.GetLatestInventoryProductAsync(It.IsAny<string>()), Times.Once);
            this.mockFileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        /// <summary>
        /// Inventories the validator delete should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task InventoryValidator_DeleteShouldFail_WhenLastRecordIsNegativeAsync()
        {
            var inventory = JObject.Parse(FormJson("Delete", 123, "10000002372"));
            var inventoryProduct = inventory.ToObject<InventoryProduct>();
            this.mockInventoryProductRepoMock.Setup(a => a.GetLatestInventoryProductAsync(It.IsAny<string>())).ReturnsAsync(new InventoryProduct { ProductVolume = -10.00M, EventType = EventType.Delete.ToString("G") });
            this.mockBlobOperations.Setup(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(inventoryProduct, new List<string>(), null as object));
            this.mockInventoryCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<InventoryProduct>())).ReturnsAsync(ValidationResult.Success);

            // Act
            (bool isValid, InventoryProduct inventoryObj) = await this.inventoryValidator.ValidateInventoryAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsFalse(isValid);
            this.mockInventoryProductRepoMock.Verify(a => a.GetLatestInventoryProductAsync(It.IsAny<string>()), Times.Once);
            this.mockFileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        /// <summary>
        /// Inventories the validator delete should return true asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task InventoryValidator_DeleteShouldFail_WhenLastRecordIsNotPresentAsync()
        {
            var inventory = JObject.Parse(FormJson("Delete", 123, "10000002372"));
            var inventoryProduct = inventory.ToObject<InventoryProduct>();
            this.mockInventoryProductRepoMock.Setup(a => a.GetLatestInventoryProductAsync(It.IsAny<string>())).ReturnsAsync(default(InventoryProduct));
            this.mockBlobOperations.Setup(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(inventoryProduct, new List<string>(), null as object));
            this.mockInventoryCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<InventoryProduct>())).ReturnsAsync(ValidationResult.Success);

            // Act
            (bool isValid, InventoryProduct inventoryObj) = await this.inventoryValidator.ValidateInventoryAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsFalse(isValid);
            this.mockInventoryProductRepoMock.Verify(a => a.GetLatestInventoryProductAsync(It.IsAny<string>()), Times.Once);
            this.mockFileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        /// <summary>
        /// Inventories the validator insert should return false log to pending transaction and update file registration status to failed asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task InventoryValidator_Insert_ShouldReturnFalse_LogToPendingTransaction_And_UpdateFileRegistrationStatusToFailed_Async()
        {
            var inventory = JObject.Parse(FormJson("Insert", 123, "10000002372"));
            var inventoryProduct = inventory.ToObject<InventoryProduct>();
            this.mockInventoryProductRepoMock.Setup(a => a.GetLatestInventoryProductAsync(It.IsAny<string>())).ReturnsAsync(new InventoryProduct { ProductVolume = 10000.00M, EventType = EventType.Update.ToString("G") });
            this.mockBlobOperations.Setup(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(inventoryProduct, new List<string>(), null as object));
            this.mockInventoryCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<InventoryProduct>())).ReturnsAsync(ValidationResult.Success);
            this.mockFileRegistrationService.Setup(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()));

            // Act
            (bool isValid, InventoryProduct inventoryObj) = await this.inventoryValidator.ValidateInventoryAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsFalse(isValid);
            this.mockInventoryProductRepoMock.Verify(a => a.GetLatestInventoryProductAsync(It.IsAny<string>()), Times.Once);
            this.mockBlobOperations.Verify(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>()), Times.Once);
            this.mockInventoryCompositeValidator.Verify(x => x.ValidateAsync(It.IsAny<InventoryProduct>()), Times.Once);
            this.mockFileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Inventories the validator update should return false log to pending transaction and update file registration status to failed asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task InventoryValidator_Update_ShouldReturnFalse_LogToPendingTransaction_And_UpdateFileRegistrationStatusToFailed_Async()
        {
            var inventory = JObject.Parse(FormJson("Update", 123, "10000002372"));
            var inventoryProduct = inventory.ToObject<InventoryProduct>();
            this.mockInventoryProductRepoMock.Setup(a => a.GetLatestInventoryProductAsync(It.IsAny<string>())).ReturnsAsync(default(InventoryProduct));
            this.mockBlobOperations.Setup(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(inventoryProduct, new List<string>(), null as object));
            this.mockInventoryCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<InventoryProduct>())).ReturnsAsync(ValidationResult.Success);
            this.mockFileRegistrationService.Setup(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()));

            // Act
            (bool isValid, InventoryProduct inventoryObj) = await this.inventoryValidator.ValidateInventoryAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsFalse(isValid);
            this.mockInventoryProductRepoMock.Verify(a => a.GetLatestInventoryProductAsync(It.IsAny<string>()), Times.Once);
            this.mockBlobOperations.Verify(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>()), Times.Once);
            this.mockInventoryCompositeValidator.Verify(x => x.ValidateAsync(It.IsAny<InventoryProduct>()), Times.Once);
            this.mockFileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Inventories the validator delete should return false log to pending transaction and update file registration status to failed asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task InventoryValidator_Delete_ShouldReturnFalse_LogToPendingTransaction_And_UpdateFileRegistrationStatusToFailed_Async()
        {
            var inventory = JObject.Parse(FormJson("Delete", 123, "10000002372"));
            var inventoryProduct = inventory.ToObject<InventoryProduct>();
            this.mockInventoryProductRepoMock.Setup(a => a.GetLatestInventoryProductAsync(It.IsAny<string>())).ReturnsAsync(default(InventoryProduct));
            this.mockBlobOperations.Setup(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(inventoryProduct, new List<string>(), null as object));
            this.mockInventoryCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<InventoryProduct>())).ReturnsAsync(ValidationResult.Success);
            this.mockFileRegistrationService.Setup(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()));

            // Act
            (bool isValid, InventoryProduct inventoryObj) = await this.inventoryValidator.ValidateInventoryAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsFalse(isValid);
            this.mockInventoryProductRepoMock.Verify(a => a.GetLatestInventoryProductAsync(It.IsAny<string>()), Times.Once);
            this.mockBlobOperations.Verify(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>()), Times.Once);
            this.mockInventoryCompositeValidator.Verify(x => x.ValidateAsync(It.IsAny<InventoryProduct>()), Times.Once);
            this.mockFileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Inventories the validator should return false if homologated json is not valid log to pending transaction and update file registration status to failed asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task InventoryValidator_ShouldReturnFalse_If_HomologatedJsonIsNotValid_LogToPendingTransaction_And_UpdateFileRegistrationStatusToFailed_Async()
        {
            var inventory = JObject.Parse(FormJson("Insert", 123, "10000002372"));

            var tuple = new Tuple<InventoryProduct, List<string>, object>(null, new List<string>(), null as object);
            this.mockBlobOperations.Setup(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(tuple);
            this.mockFileRegistrationService.Setup(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()));

            // Act
            (bool isValid, InventoryProduct inventoryObj) = await this.inventoryValidator.ValidateInventoryAsync(this.fileRegistrationTransactionObject, inventory).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsFalse(isValid);
            this.mockInventoryCompositeValidator.Verify(x => x.ValidateAsync(It.IsAny<InventoryProduct>()), Times.Never);

            this.mockBlobOperations.Verify(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>()), Times.Once);
            this.mockFileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        /// <summary>
        /// Inventories the validator insert should return false asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task InventoryValidator_ShouldFail_WhenNotHasVersionInsertAsync()
        {
            var inventory = JObject.Parse(FormJson("Insert", 123, "10000002372"));
            var inventoryProduct = inventory.ToObject<InventoryProduct>();
            inventoryProduct.ScenarioId = ScenarioType.OFFICER;
            inventoryProduct.Version = null;
            this.mockInventoryProductRepoMock.Setup(a => a.GetLatestInventoryProductAsync(It.IsAny<string>())).ReturnsAsync(new InventoryProduct { EventType = EventType.Delete.ToString("G"), ProductVolume = -10.00M });
            this.mockBlobOperations.Setup(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(inventoryProduct, new List<string>(), null as object));
            this.mockInventoryCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<InventoryProduct>())).ReturnsAsync(ValidationResult.Success);

            // Act
            (bool isValid, InventoryProduct inventoryObj) = await this.inventoryValidator.ValidateInventoryAsync(this.fileRegistrationTransactionObject, new JObject()).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsFalse(isValid);
            this.mockInventoryProductRepoMock.Verify(a => a.GetLatestInventoryProductAsync(It.IsAny<string>()), Times.Once);
            this.mockBlobOperations.Verify(x => x.GetHomologatedObject<InventoryProduct>(It.IsAny<JToken>(), It.IsAny<string>()), Times.Once);
            this.mockInventoryCompositeValidator.Verify(x => x.ValidateAsync(It.IsAny<InventoryProduct>()), Times.Once);

            this.mockFileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        /// <summary>
        /// Forms the json.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="productVolume">The product volumne.</param>
        /// <param name="productId">The product id.</param>
        /// <returns>
        /// The string.
        /// </returns>
        private static string FormJson(string eventType, int productVolume, string productId)
        {
            return "{  \"SourceSystemId\": \"164\", \"ScenarioId\": \"1\",\"SegmentId\": \"1\", \"SystemName\": \"EXCEL - OCENSA\", \"DestinationSystem\": \"TRUE\",  \"EventType\": " + "\"" + eventType + "\"," +
                " \"InventoryId\": \"COURTOFF 13014\",  \"InventoryDate\": \"2019-11-04T23:59:59\",  \"NodeId\": 2620, " +
                " \"TankName\": null,  \"Scenario\": \"OPERATIVO\",  \"Observations\": \"Reporte Operativo Cusiana -Fecha\"," +
                "  \"Tolerance\": 0.21,  \"Type\": \"Inventory\",  \"ProductId\":" + "\"" + productId + "\",      \"ProductType\": \"1\", " +
                "     \"ProductVolume\":  " + "\"" + productVolume + "\",      \"GrossProductVolume\": 120,      \"MeasurementUnit\": 31,      \"Owners\": [ " +
                "       {          \"OwnerId\": \"1\",          \"OwnershipValue\": 60,          \"OwnershipValueUnit\": \"%\" " +
                "       }      ],      \"Attributes\": [        {          \"AttributeId\": 123,          \"AttributeValue\": 22.4, " +
                "         \"ValueAttributeUnit\": 100, \"AttributeDescription\": \"\"        }      ]    }";
        }
    }
}