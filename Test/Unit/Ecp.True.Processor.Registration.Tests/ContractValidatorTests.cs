// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContractValidatorTests.cs" company="Microsoft">
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
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Dto;
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
    public class ContractValidatorTests
    {
        /// <summary>
        /// The file registration transaction mock.
        /// </summary>
        private readonly Mock<IRepository<Contract>> contractRepoMock = new Mock<IRepository<Contract>>();

        /// <summary>
        /// The composite validator.
        /// </summary>
        private Mock<ICompositeValidatorFactory> compositeValidator;

        /// <summary>
        /// The movement composite validator.
        /// </summary>
        private Mock<ICompositeValidator<Contract>> contractCompositeValidator;

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
        /// The registration processor.
        /// </summary>
        private ContractValidator contractValidator;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<ContractValidator>> logger;

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
            this.contractCompositeValidator = new Mock<ICompositeValidator<Contract>>();
            this.blobClientMock = new Mock<IBlobStorageClient>();
            this.unitOfWorkMock = new Mock<IUnitOfWork>();
            this.mockQueueClient = new Mock<IServiceBusQueueClient>();
            this.unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.fileRegistrationService = new Mock<IFileRegistrationTransactionService>();
            this.configurationHandlerMock = new Mock<IConfigurationHandler>();
            this.blobOperations = new Mock<IBlobOperations>();
            this.logger = new Mock<ITrueLogger<ContractValidator>>();
            this.azureClientFactory = new Mock<IAzureClientFactory>();
            this.unitOfWorkMock.Setup(m => m.CreateRepository<Contract>()).Returns(this.contractRepoMock.Object);
            this.unitOfWorkFactory.Setup(m => m.GetUnitOfWork()).Returns(this.unitOfWorkMock.Object);
            this.mockBlobSasClient = new Mock<IBlobStorageSasClient>();
            this.fileRegistrationTransactionObject = new FileRegistrationTransaction
            {
                ActionType = Entities.Enumeration.FileRegistrationActionType.Insert,
                BlobPath = @"ContractJson/Contract.json",
                CreatedBy = "sysadmin",
                CreatedDate = DateTime.Now,
                FileRegistrationId = 1,
                FileRegistrationTransactionId = 1,
                SystemTypeId = SystemType.SINOPER,
                UploadId = "1",
            };

            this.compositeValidator.Setup(x => x.ContractCompositeValidator).Returns(this.contractCompositeValidator.Object);
            this.azureClientFactory.Setup(x => x.GetBlobStorageSaSClient(It.IsAny<string>(), It.IsAny<string>())).Returns(this.mockBlobSasClient.Object);
            this.azureClientFactory.Setup(a => a.GetQueueClient(It.IsAny<string>())).Returns(this.mockQueueClient.Object);

            var systemConfig = new ServiceBusSettings
            {
                ConnectionString = "TestConnectionString",
            };

            this.configurationHandlerMock.Setup(x => x.GetConfigurationAsync<ServiceBusSettings>(It.IsAny<string>())).ReturnsAsync(systemConfig);

            this.contractValidator = new ContractValidator(
                this.logger.Object,
                this.blobOperations.Object,
                this.compositeValidator.Object,
                this.unitOfWorkFactory.Object,
                this.azureClientFactory.Object,
                this.fileRegistrationService.Object);
        }

        /// <summary>
        /// Validators the should validate with contract asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ContractValidator_ValidateAsync()
        {
            var filePath = @"ContractJson/Contract.json";
            var contract = this.GetContractArray();
            var contractList = contract.ToObject<List<Contract>>();

            JObject entity = new JObject();

            entity["SourceSystem"] = "EXCEL";

            this.blobOperations.Setup(x => x.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.GetStreamDetailsForFile(filePath));
            this.blobOperations.Setup(x => x.GetHomologatedObject<Contract>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(contractList[0], new List<string>(), null as object));
            this.contractCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<Contract>())).ReturnsAsync(ValidationResult.Success);

            // Act
            await this.contractValidator.ValidateContractAsync(this.fileRegistrationTransactionObject, entity).ConfigureAwait(false);

            // Assert or Verify
            this.fileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
        }

        /// <summary>
        /// Validators the should validate with contract asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ContractValidator_ShouldFail_WithContractAsync()
        {
            var filePath = @"ContractJson/EmptyContract.json";
            var contract = this.GetContractArray();
            var contractList = contract.ToObject<List<Contract>>();

            JObject entity = new JObject();

            entity["SourceSystem"] = "EXCEL";

            this.blobOperations.Setup(x => x.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.GetStreamDetailsForFile(filePath));
            this.blobOperations.Setup(x => x.GetHomologatedObject<Contract>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(contractList[0], new List<string>(), null as object));
            this.fileRegistrationTransactionObject.BlobPath = filePath;
            this.contractCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<Contract>())).ReturnsAsync(ValidationResult.Success);

            // Act
            await this.contractValidator.ValidateContractAsync(this.fileRegistrationTransactionObject, entity).ConfigureAwait(false);

            // Assert or Verify
            this.fileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
        }

        /// <summary>
        /// Validators the should validate with contract asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ContractValidator_ShouldFail_WithFalseValidationStatusContractAsync()
        {
            var filePath = @"ContractJson/Contract.json";
            var contract = this.GetContractArray();
            var contractList = contract.ToObject<List<Contract>>();

            JObject entity = new JObject();

            entity["SourceSystem"] = "EXCEL";

            this.blobOperations.Setup(x => x.GetHomologatedJsonAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.GetStreamDetailsForFile(filePath));
            this.blobOperations.Setup(x => x.GetHomologatedObject<Contract>(It.IsAny<JToken>(), It.IsAny<string>())).Returns(Tuple.Create(contractList[0], new List<string>(), null as object));
            this.fileRegistrationTransactionObject.BlobPath = filePath;
            this.contractCompositeValidator.Setup(x => x.ValidateAsync(It.IsAny<Contract>())).ReturnsAsync(new ValidationResult() { IsSuccess = false });

            await this.contractValidator.ValidateContractAsync(this.fileRegistrationTransactionObject, entity).ConfigureAwait(false);

            // Assert or Verify
            this.fileRegistrationService.Verify(m => m.RegisterFailureAsync(It.IsAny<PendingTransaction>(), It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
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
        private JArray GetContractArray()
        {
            var json = "[{\r\n\t\"ContractId\": \"1\",\r\n\t\"DocumentNumber\": \"12\",\r\n\t\"Position\": \"1\",\r\n\t\"MovementTypeId\": \"3\",\r\n\t\"SourceNodeId\": 31,\r\n\t\"DestinationNodeId\": \"14\",\r\n\t\"ProductId\": \"123\",\r\n\t\"StartDate\": \"2019-12-01\",\r\n\t\"EndDate\": \"2020-01-08\",\r\n\t\"CommercialId\": \"21\",\r\n\t\"Volume\": \"23\",\r\n\t\"Frequency\": \"Diario\",\r\n\t\"Status\": \"Activa\" \r\n\t}]";
            return JArray.Parse(json);
        }
    }
}