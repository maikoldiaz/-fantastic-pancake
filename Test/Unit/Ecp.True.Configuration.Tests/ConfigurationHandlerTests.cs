// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationHandlerTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Configuration.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Caching;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.KeyStore.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Configuration Handler Tests.
    /// </summary>
    [TestClass]
    public class ConfigurationHandlerTests
    {
        /// <summary>
        /// The callback key.
        /// </summary>
        private string callbackKey;

        /// <summary>
        /// The callback key vault configuration.
        /// </summary>
        private KeyVaultConfiguration callbackKeyVaultConfiguration;

        /// <summary>
        /// The mock configuration store.
        /// </summary>
        private Mock<IConfigurationStore> mockConfigStore;

        /// <summary>
        /// The mock configuration store factory.
        /// </summary>
        private Mock<IConfigurationStoreFactory> mockConfigurationStoreFactory;

        /// <summary>
        /// The mock repository.
        /// </summary>
        private MockRepository mockRepository;

        /// <summary>
        /// Gets the configuration should call configuration store when key is not present in memory cache.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetConfiguration_ShouldCallConfigStore_WhenKeyIsNotPresentInMemoryCacheAsync()
        {
            // Arrange
            InMemoryCacheManager.Remove("testKey");
            this.SetupMocks("testValue");

            // Act
            var configurationHandler = this.CreateConfigurationHandler();
            var configValue = await configurationHandler.GetConfigurationAsync("testKey").ConfigureAwait(false);
            var configValueStoredInCache = InMemoryCacheManager.Get<string>("testKey");
            InMemoryCacheManager.Remove("testKey");

            Assert.AreEqual("testValue", configValue);
            Assert.AreEqual("testValue", configValueStoredInCache);
            Assert.AreEqual("testKey", this.callbackKey);
            this.mockConfigurationStoreFactory.Verify(x => x.GetConfigurationStoreAsync(It.IsAny<string>()), Times.Once);
            this.mockConfigStore.Verify(x => x.GetFromStoreAsync<string>(It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Gets the configuration should call configuration store when key is not present in memory cache.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetConfigurationOrDefault_ShouldReturnDefaultConfigurationAsync()
        {
            // Arrange
            var solutionConfig = new ServiceBusMessagingConfig { QueueName = "Queue" };
            this.mockConfigurationStoreFactory.Setup(x => x.GetConfigurationStoreAsync(It.IsAny<string>()))
                    .ReturnsAsync(this.mockConfigStore.Object);
            this.mockConfigStore.Setup(x => x.GetFromStoreAsync<ServiceBusMessagingConfig>(It.IsAny<string>()))
                .ReturnsAsync(default(ServiceBusMessagingConfig));

            // Act
            var configurationHandler = this.CreateConfigurationHandler();
            var configValue = await configurationHandler.GetConfigurationOrDefaultAsync("testKey", solutionConfig).ConfigureAwait(false);

            Assert.IsNull(configValue);
        }

        /// <summary>
        /// Gets the configuration should throw argument null exception when key is empty.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetConfiguration_ShouldReturnConfigFromMemoryCache_WhenKeyIsAlreadyCachedAsync()
        {
            // Arrange
            InMemoryCacheManager.Add("testKey", "testValue");

            // Act
            var configurationHandler = this.CreateConfigurationHandler();
            var configValue = await configurationHandler.GetConfigurationAsync("testKey").ConfigureAwait(false);
            InMemoryCacheManager.Remove("testKey");

            Assert.AreEqual("testValue", configValue);
            this.mockConfigurationStoreFactory.Verify(x => x.GetConfigurationStoreAsync(It.IsAny<string>()), Times.Never);
        }

        /// <summary>
        /// Gets the configuration should return configuration value if found when default value is provided.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetConfiguration_ShouldReturnConfigValueIfFound_WhenDefaultValueIsProvidedAsync()
        {
            // Arrange
            InMemoryCacheManager.Remove("testKey");
            this.SetupMocks("testValue");

            // Act
            var configurationHandler = this.CreateConfigurationHandler();
            var configValue = await configurationHandler.GetConfigurationAsync("testKey", "defultValue").ConfigureAwait(false);
            InMemoryCacheManager.Remove("testKey");

            Assert.AreEqual("testValue", configValue);
            Assert.AreEqual("testKey", this.callbackKey);
            this.mockConfigurationStoreFactory.Verify(x => x.GetConfigurationStoreAsync(It.IsAny<string>()), Times.Once);
            this.mockConfigStore.Verify(x => x.GetFromStoreAsync<string>(It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Gets the configuration should return default value if configuration not found when default value is provided.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetConfiguration_ShouldReturnDefaultValueIfConfigNotFound_WhenDefaultValueIsProvidedAsync()
        {
            // Arrange
            InMemoryCacheManager.Remove("testKey");
            this.SetupMocks(default(string));

            // Act
            var configurationHandler = this.CreateConfigurationHandler();
            var configValue = await configurationHandler.GetConfigurationAsync("testKey", "defultValue").ConfigureAwait(false);
            InMemoryCacheManager.Remove("testKey");

            Assert.AreEqual("defultValue", configValue);
            Assert.AreEqual("testKey", this.callbackKey);
            this.mockConfigurationStoreFactory.Verify(x => x.GetConfigurationStoreAsync(It.IsAny<string>()), Times.Once);
            this.mockConfigStore.Verify(x => x.GetFromStoreAsync<string>(It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Gets the configuration should throw argument null exception when key is empty.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetConfiguration_ShouldThrowArgumentNullException_WhenKeyIsEmptyAsync()
        {
            // Act
            var configurationHandler = this.CreateConfigurationHandler();
            await configurationHandler.GetConfigurationAsync(string.Empty).ConfigureAwait(false);
        }

        /// <summary>
        /// Initializes should initialize configuration store when no configuration is provided.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task Initialize_ShouldInitializeConfigurationStore_WhenNoConfigurationIsProvidedAsync()
        {
            // Arrange
            this.SetupInitializeConfigStoreMocks();

            // Act
            var configurationHandler = this.CreateConfigurationHandler();
            var status = await configurationHandler.InitializeAsync().ConfigureAwait(false);

            // Assert
            Assert.IsTrue(status);
            Assert.IsNull(this.callbackKeyVaultConfiguration);
            this.mockConfigurationStoreFactory.Verify(x => x.InitializeConfigurationStoresAsync(It.IsAny<KeyVaultConfiguration>()), Times.Once);
        }

        /// <summary>
        /// Initializes should initialize configuration store with configuration provided.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task Initialize_ShouldInitializeConfigurationStore_WithConfigurationProvidedAsync()
        {
            // Arrange
            this.SetupInitializeConfigStoreMocks();
            var keyvalutConfig = new KeyVaultConfiguration
            {
                KeyName = "TestKey",
            };

            // Act
            var configurationHandler = this.CreateConfigurationHandler();
            var status = await configurationHandler.InitializeAsync(keyvalutConfig).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(status);
            Assert.AreEqual(keyvalutConfig, this.callbackKeyVaultConfiguration);
            this.mockConfigurationStoreFactory.Verify(x => x.InitializeConfigurationStoresAsync(It.IsAny<KeyVaultConfiguration>()), Times.Once);
        }

        /// <summary>
        /// Gets the collection configuration should return all configuration value if found when key pattern is provided asynchronous.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetCollectionConfiguration_ShouldReturnAllConfigValueIfFound_WhenKeyPatternIsProvidedAsync()
        {
            // Arrange
            this.mockConfigurationStoreFactory.Setup(x => x.GetConfigurationStoreAsync(It.IsAny<string>()))
                    .ReturnsAsync(this.mockConfigStore.Object);
            this.mockConfigStore.Setup(x => x.GetAllFromStoreAsync<ConfigurationSetting>()).ReturnsAsync(new List<ConfigurationSetting>() { new ConfigurationSetting { Key = "ModuleAvailabilityTrueCutOff.Settings", Value = "{\"Name\":\"TrueCutOff\",\"Resources\":[\"FA-AEU-ECP-DEV-TRUECALCULATE\"]}" } });

            // Act
            var configurationHandler = this.CreateConfigurationHandler();
            var configValue = await configurationHandler.GetCollectionConfigurationAsync<ModuleAvailabilitySettings>(ConfigurationConstants.ModuleAvailability).ConfigureAwait(false);

            Assert.AreEqual("TrueCutOff", configValue.ElementAt(0).Name);
            this.mockConfigurationStoreFactory.Verify(x => x.GetConfigurationStoreAsync(It.IsAny<string>()), Times.Once);
            this.mockConfigStore.Verify(x => x.GetAllFromStoreAsync<ConfigurationSetting>(), Times.Once);
        }

        /// <summary>
        /// Tests the cleanup.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            this.mockRepository.VerifyAll();
        }

        /// <summary>
        /// Tests the initialize.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockConfigurationStoreFactory = this.mockRepository.Create<IConfigurationStoreFactory>();
            this.mockConfigStore = this.mockRepository.Create<IConfigurationStore>();
        }

        /// <summary>
        /// Creates the configuration handler.
        /// </summary>
        /// <returns>
        /// Creates the configuration handler as <see cref="ConfigurationHandler" />.
        /// </returns>
        private ConfigurationHandler CreateConfigurationHandler()
        {
            return new ConfigurationHandler(this.mockConfigurationStoreFactory.Object);
        }

        /// <summary>
        /// Setups the initialize configuration store mocks.
        /// </summary>
        /// <param name="status">if set to <c>true</c> [status].</param>
        private void SetupInitializeConfigStoreMocks(bool status = true)
        {
            this.mockConfigurationStoreFactory
                    .Setup(x => x.InitializeConfigurationStoresAsync(It.IsAny<KeyVaultConfiguration>()))
                    .ReturnsAsync(status).Callback<KeyVaultConfiguration>(x => this.callbackKeyVaultConfiguration = x);
        }

        /// <summary>
        /// Setups the mocks.
        /// </summary>
        /// <param name="configValue">The configuration value.</param>
        private void SetupMocks(string configValue)
        {
            this.mockConfigurationStoreFactory.Setup(x => x.GetConfigurationStoreAsync(It.IsAny<string>()))
                    .ReturnsAsync(this.mockConfigStore.Object);
            this.mockConfigStore.Setup(x => x.GetFromStoreAsync<string>(It.IsAny<string>())).ReturnsAsync(configValue)
                    .Callback<string>(x => this.callbackKey = x);
        }
    }
}