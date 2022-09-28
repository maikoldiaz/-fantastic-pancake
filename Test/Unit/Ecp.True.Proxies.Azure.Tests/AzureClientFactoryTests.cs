// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AzureClientFactoryTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Azure.Tests
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Ecp.True.Chaos.Interfaces;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Configuration.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The azure client factory tests.
    /// </summary>
    [TestClass]
    public class AzureClientFactoryTests
    {
        /// <summary>
        /// The azure client factory.
        /// </summary>
        private IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The azure management API client.
        /// </summary>
        private Mock<IAzureManagementApiClient> azureManagementApiClient;

        /// <summary>
        /// The ethereum client.
        /// </summary>
        private Mock<IEthereumClient> ethereumClient;

        /// <summary>
        /// The analysis service client.
        /// </summary>
        private Mock<IAnalysisServiceClient> analysisServiceClient;

        /// <summary>
        /// The mock chaos manager.
        /// </summary>
        private Mock<IChaosManager> mockChaosManager;

        /// <summary>
        /// The mock dead letter manager.
        /// </summary>
        private Mock<IDeadLetterManager> mockDeadLetterManager;

        /// <summary>
        /// The mock for Azure client factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureClientFactory;

        /// <summary>
        /// The mock for blob client.
        /// </summary>
        private Mock<IBlobStorageClient> mockBlobClient;

        /// <summary>
        /// Initilizes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.azureManagementApiClient = new Mock<IAzureManagementApiClient>();
            this.ethereumClient = new Mock<IEthereumClient>();
            this.analysisServiceClient = new Mock<IAnalysisServiceClient>();
            this.mockChaosManager = new Mock<IChaosManager>();
            this.mockDeadLetterManager = new Mock<IDeadLetterManager>();
            this.mockAzureClientFactory = new Mock<IAzureClientFactory>();
            this.mockBlobClient = new Mock<IBlobStorageClient>();

            this.azureClientFactory = new AzureClientFactory(
                this.ethereumClient.Object,
                this.analysisServiceClient.Object,
                this.mockChaosManager.Object,
                this.mockDeadLetterManager.Object,
                this.azureManagementApiClient.Object);
        }

        /// <summary>
        /// Initializes the should initialize blocb storage client when invoked.
        /// </summary>
        [TestMethod]
        public void Initialize_ShouldInitializeBlobStorageClient_WhenInvoked()
        {
            var storageSettings = new StorageSettings
            {
                MaxAttempts = 5,
                DeltaBackOff = 2,
                ConnectionString = "ConnectionString",
            };

            this.azureClientFactory.Initialize(new AzureConfiguration(storageSettings, new ServiceBusSettings()));

            Assert.IsTrue(this.azureClientFactory.IsReady);
        }

        /// <summary>
        /// Initializes the analysis client when invoke.
        /// </summary>
        [TestMethod]
        public void Initialize_ShouldInitializeAnalysisClient_WhenInvoke()
        {
            var analysisSettings = new AnalysisSettings
            {
                ClientId = "ClientID",
                ClientSecret = "ClientSecret",
                ModelName = "ModelName",
                Region = "Region",
                ServerName = "ServerName",
                TenantId = "TenantID",
            };

            var storageSettings = new StorageSettings
            {
                ConnectionString = "ConnectionString",
                MaxAttempts = 5,
                DeltaBackOff = 2,
            };

            this.analysisServiceClient.Setup(x => x.Initialize(analysisSettings));

            this.azureClientFactory.Initialize(new AzureConfiguration(analysisSettings, storageSettings, new ServiceBusSettings()));

            Assert.IsTrue(this.azureClientFactory.IsReady);
            this.analysisServiceClient.Verify(x => x.Initialize(analysisSettings), Times.Once);
        }

        /// <summary>
        /// Tries to Refresh Analysis Service for Balance.
        /// </summary>
        [TestMethod]
        public void Return_ShouldTryToBalanceRefreshAAS_WhenInvoke()
        {
            var analysisSettings = new AnalysisSettings
            {
                ClientId = "ClientID",
                ClientSecret = "ClientSecret",
                ModelName = "ModelName",
                Region = "Region",
                ServerName = "ServerName",
                TenantId = "TenantID",
            };

            var storageSettings = new StorageSettings
            {
                ConnectionString = "ConnectionString",
                MaxAttempts = 5,
                DeltaBackOff = 2,
            };

            this.analysisServiceClient.Setup(x => x.Initialize(analysisSettings));

            this.azureClientFactory.Initialize(new AzureConfiguration(analysisSettings, storageSettings, new ServiceBusSettings()));

            this.analysisServiceClient.Verify(x => x.RefreshCalculationAsync(1), Times.Never);
        }

        /// <summary>
        /// Tries to Refresh Analysis Service for Ownership.
        /// </summary>
        [TestMethod]
        public void Return_ShouldTryToOwnershipRefreshAAS_WhenInvoke()
        {
            var analysisSettings = new AnalysisSettings
            {
                ClientId = "ClientID",
                ClientSecret = "ClientSecret",
                ModelName = "ModelName",
                Region = "Region",
                ServerName = "ServerName",
                TenantId = "TenantID",
            };

            var storageSettings = new StorageSettings
            {
                ConnectionString = "ConnectionString",
                MaxAttempts = 5,
                DeltaBackOff = 2,
            };

            var serviceBusSettings = new ServiceBusSettings
            {
                ConnectionString = "someString",
            };

            this.analysisServiceClient.Setup(x => x.Initialize(analysisSettings));

            this.azureClientFactory.Initialize(new AzureConfiguration(analysisSettings, storageSettings, serviceBusSettings));

            this.analysisServiceClient.Verify(x => x.RefreshOwnershipAsync(1), Times.Never);
        }

        /// <summary>
        /// Ethereums the client should return ethereum client when invoked.
        /// </summary>
        [TestMethod]
        public void EthereumClient_ShouldReturnEthereumClient_WhenInvoked()
        {
            Assert.IsNotNull(this.azureClientFactory.EthereumClient);
        }

        /// <summary>
        /// Tries to Refresh Analysis Service for Sap Mapping Details.
        /// </summary>
        [TestMethod]
        public void Return_ShouldTryToSapMappingRefreshAAS_WhenInvoke()
        {
            var analysisSettings = new AnalysisSettings
            {
                ClientId = "ClientID",
                ClientSecret = "ClientSecret",
                ModelName = "ModelName",
                Region = "Region",
                ServerName = "ServerName",
                TenantId = "TenantID",
            };

            var storageSettings = new StorageSettings
            {
                ConnectionString = "ConnectionString",
                MaxAttempts = 5,
                DeltaBackOff = 2,
            };

            this.analysisServiceClient.Setup(x => x.Initialize(analysisSettings));

            this.azureClientFactory.Initialize(new AzureConfiguration(analysisSettings, storageSettings, new ServiceBusSettings()));

            this.analysisServiceClient.Verify(x => x.RefreshSapMappingDetailsAsync(), Times.Never);
        }

        /// <summary>
        /// Returns the should try to balance refresh availability settings when invoke.
        /// </summary>
        [TestMethod]
        public void Return_ShouldTryToBalanceRefreshAvailabilitySettings_WhenInvoke()
        {
            var availabilitySettings = new AvailabilitySettings
            {
                ClientId = "ClientID",
                ClientSecret = "ClientSecret",
                ResourceId = "ResourceId",
                TenantId = "TenantId",
            };

            availabilitySettings.ResourceGroups.Add(new ResourceGroup { SubscriptionId = "SubscriptionId", Name = "ResourceName" });

            var storageSettings = new StorageSettings
            {
                ConnectionString = "ConnectionString",
                MaxAttempts = 5,
                DeltaBackOff = 2,
            };

            this.azureManagementApiClient.Setup(x => x.Initialize(availabilitySettings));

            this.azureClientFactory.Initialize(new AzureConfiguration(availabilitySettings, storageSettings, new ServiceBusSettings()));

            this.azureManagementApiClient.Verify(x => x.Initialize(availabilitySettings), Times.Once);

            Assert.IsTrue(this.azureClientFactory.IsReady);
        }

        /// <summary>
        ///  GetBlobClient Should Return client for valid account name and container name.
        /// </summary>
        [TestMethod]
        public void GetQueueClient_ShouldReturnClientForValidParameters()
        {
            // Arrange
            var storageSettings = new StorageSettings
            {
                AccountName = "AccountName",
                StorageAppKey = this.Base64StringEncode("AppKey"),
                ConnectionString = "ConnectionString",
                MaxAttempts = 5,
                DeltaBackOff = 2,
            };

            var serviceBusSettings = new ServiceBusSettings
            {
                Namespace = "Test",
                TenantId = "Test",
                MinimumBackOff = 1,
                MaximumBackOff = 3,
                MaximumRetryCount = 2,
            };

            this.mockBlobClient.Setup(m => m.CreateBlobAsync(It.IsAny<string>(), It.IsAny<Stream>())).Returns(Task.FromResult(0));

            this.azureClientFactory.Initialize(new AzureConfiguration(storageSettings, serviceBusSettings));

            // Act --> Should Return Local Path with /Test/Test
            var result = this.azureClientFactory.GetQueueClient("TestContainerName");

            // Assert
            Assert.IsTrue(this.azureClientFactory.IsReady);
            Assert.IsNotNull(result);
        }

        /// <summary>
        ///  GetBlobClient Should Return client for valid account name and container name.
        /// </summary>
        [TestMethod]
        public void GetBlobClient_ShouldReturnClientForValidParameters()
        {
            // Arrange
            var storageSettings = new StorageSettings
            {
                AccountName = "AccountName",
                StorageAppKey = this.Base64StringEncode("AppKey"),
                ConnectionString = "ConnectionString",
                MaxAttempts = 5,
                DeltaBackOff = 2,
            };

            this.mockBlobClient.Setup(m => m.CreateBlobAsync(It.IsAny<string>(), It.IsAny<Stream>())).Returns(Task.FromResult(0));

            this.azureClientFactory.Initialize(new AzureConfiguration(storageSettings, new ServiceBusSettings()));

            // Act --> Should Return Local Path with /Test/Test
            var result = this.azureClientFactory.GetBlobClient("TestContainerName");

            // Assert
            Assert.IsTrue(this.azureClientFactory.IsReady);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Should Return client for valid account name and container name.
        /// </summary>
        [TestMethod]
        public void GetBlobStorageSaSClient_ShouldReturnClientForValidParameters()
        {
            // Arrange
            var storageSettings = new StorageSettings
            {
                AccountName = "AccountName",
                StorageAppKey = this.Base64StringEncode("AppKey"),
                ConnectionString = "ConnectionString",
                MaxAttempts = 5,
                DeltaBackOff = 2,
            };

            this.mockAzureClientFactory.Setup(x => x.GetBlobStorageSaSClient("ownership", "Test").CreateBlobAsync(It.IsAny<string>()));

            this.azureClientFactory.Initialize(new AzureConfiguration(storageSettings, new ServiceBusSettings()));

            // Act --> Should Return Local Path with /Test/Test
            var result = this.azureClientFactory.GetBlobStorageSaSClient("TestContainerName", "TestBlobname");

            // Assert
            Assert.IsTrue(this.azureClientFactory.IsReady);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Should throw exception for null account name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetBlobStorageSaSClient_ShouldThrowExceptionForNullAccountName()
        {
            // Arrange
            var storageSettings = new StorageSettings
            {
                AccountName = "AccountName",
                StorageAppKey = this.Base64StringEncode("AppKey"),
                ConnectionString = "ConnectionString",
                MaxAttempts = 5,
                DeltaBackOff = 2,
            };

            this.mockAzureClientFactory.Setup(x => x.GetBlobStorageSaSClient("ownership", "Test").CreateBlobAsync(It.IsAny<string>()));

            this.azureClientFactory.Initialize(new AzureConfiguration(storageSettings, new ServiceBusSettings()));

            // Act --> Should Return Local Path with /Test/Test
            IBlobStorageSasClient result = this.azureClientFactory.GetBlobStorageSaSClient(null, "TestBlobname");

            // Assert
            Assert.IsTrue(this.azureClientFactory.IsReady);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Should throw exception for null blob name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetBlobStorageSaSClient_ShouldThrowExceptionForNullBlobName()
        {
            // Arrange
            var storageSettings = new StorageSettings
            {
                AccountName = "AccountName",
                StorageAppKey = this.Base64StringEncode("AppKey"),
                ConnectionString = "ConnectionString",
                MaxAttempts = 5,
                DeltaBackOff = 2,
            };

            this.mockAzureClientFactory.Setup(x => x.GetBlobStorageSaSClient("ownership", "Test").CreateBlobAsync(It.IsAny<string>()));

            this.azureClientFactory.Initialize(new AzureConfiguration(storageSettings, new ServiceBusSettings()));

            // Act --> Should Return Local Path with /Test/Test
            IBlobStorageSasClient result = this.azureClientFactory.GetBlobStorageSaSClient("TestContainerName", null);

            // Assert
            Assert.IsTrue(this.azureClientFactory.IsReady);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Should throw exception for empty account name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetBlobStorageSaSClient_ShouldThrowExceptionForNullEmptyContainerName()
        {
            // Arrange
            var storageSettings = new StorageSettings
            {
                AccountName = "AccountName",
                StorageAppKey = this.Base64StringEncode("AppKey"),
                ConnectionString = "ConnectionString",
                MaxAttempts = 5,
                DeltaBackOff = 2,
            };

            this.mockAzureClientFactory.Setup(x => x.GetBlobStorageSaSClient("ownership", "Test").CreateBlobAsync(It.IsAny<string>()));

            this.azureClientFactory.Initialize(new AzureConfiguration(storageSettings, new ServiceBusSettings()));

            // Act --> Should Return Local Path with /Test/Test
            IBlobStorageSasClient result = this.azureClientFactory.GetBlobStorageSaSClient(string.Empty, "TestBlobname");

            // Assert
            Assert.IsTrue(this.azureClientFactory.IsReady);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Should throw exception for empty blob name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetBlobStorageSaSClient_ShouldThrowExceptionForNullEmptyBlobName()
        {
            // Arrange
            var storageSettings = new StorageSettings
            {
                AccountName = "AccountName",
                StorageAppKey = this.Base64StringEncode("AppKey"),
                ConnectionString = "ConnectionString",
                MaxAttempts = 5,
                DeltaBackOff = 2,
            };

            this.mockAzureClientFactory.Setup(x => x.GetBlobStorageSaSClient("ownership", "Test").CreateBlobAsync(It.IsAny<string>()));

            this.azureClientFactory.Initialize(new AzureConfiguration(storageSettings, new ServiceBusSettings()));

            // Act --> Should Return Local Path with /Test/Test
            IBlobStorageSasClient result = this.azureClientFactory.GetBlobStorageSaSClient("TestContainerName", string.Empty);

            // Assert
            Assert.IsTrue(this.azureClientFactory.IsReady);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Base 64 Convertion.
        /// </summary>
        /// <param name="originalString">input string to convert.</param>
        /// <returns>string converted to base 64.</returns>
        private string Base64StringEncode(string originalString)
        {
            var bytes = Encoding.UTF8.GetBytes(originalString);
            var encodedString = Convert.ToBase64String(bytes);
            return encodedString;
        }
    }
}
