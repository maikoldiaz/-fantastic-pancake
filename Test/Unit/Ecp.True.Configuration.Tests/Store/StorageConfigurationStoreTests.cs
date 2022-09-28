// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageConfigurationStoreTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Configuration.Tests.Store
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Caching;
    using Ecp.True.DataAccess;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Ioc.Interfaces;
    using Ecp.True.Logging;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// Storage Configuration Store Tests.
    /// </summary>
    [TestClass]
    public class StorageConfigurationStoreTests
    {
        /// <summary>
        /// The mock configuration store.
        /// </summary>
        private Mock<IConfigurationStore> mockConfigurationStore;

        /// <summary>
        /// The mock configuration store factory.
        /// </summary>
        private Mock<IConfigurationStoreFactory> mockConfigurationStoreFactory;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<StorageConfigurationStore>> mockLogger;

        private Mock<IResolver> mockResolver;

        /// <summary>
        /// The mock repository.
        /// </summary>
        private MockRepository mockRepository;

        /// <summary>
        /// The table provider.
        /// </summary>
        private Mock<ITableProvider> mockTableProvider;

        /// <summary>
        /// Gets from store asynchronous should return configuration value for valid key.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetFromStoreAsync_ShouldReturnConfigValue_ForValidStoreConfigKeyAsync()
        {
            // Arrange
            var testKey = "testKey";
            var configurationSetting = new ConfigurationSetting
            {
                Key = "configKey",
                Value = "configValue_##secretKey##",
            };

            this.mockTableProvider.Setup(x => x.GetByRowKeyAndPartitionKeyAsync<ConfigurationSetting>(testKey, It.IsAny<string>())).ReturnsAsync(configurationSetting);
            this.mockConfigurationStoreFactory.SetupGet(x => x.SecretStore).Returns(this.mockConfigurationStore.Object);
            this.mockConfigurationStore.Setup(x => x.GetFromStoreAsync<string>("secretKey")).ReturnsAsync("secretValue");

            // Act
            var storageConfigurationStore = this.CreateStorageConfigurationStore();
            var configValue = await storageConfigurationStore
                    .GetFromStoreAsync<string>($"{ConfigurationConstants.DataStoreConfigurationPrefix}{testKey}")
                    .ConfigureAwait(false);
            InMemoryCacheManager.Remove("secretKey");

            // Assert
            Assert.IsNotNull(configValue);
            Assert.AreEqual("configValue_secretValue", configValue);
        }

        /// <summary>
        /// Gets from store asynchronous should return configuration value for valid key.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetFromStoreAsync_ShouldEncodeConfigValue_ForValidStoreConfigKeyInKeyVaultAsync()
        {
            // Arrange
            var testKey = "testKey";
            var configurationSetting = new ConfigurationSetting
            {
                Key = "configKey",
                Value = "configValue_#secretKey#_blah_blah_#secretKey2#",
            };
            var secretValue = "secret\"value\"";
            var secretValue2 = "secret\"value2\"";
            this.mockTableProvider.Setup(x => x.GetByRowKeyAndPartitionKeyAsync<ConfigurationSetting>(testKey, It.IsAny<string>())).ReturnsAsync(configurationSetting);
            this.mockConfigurationStoreFactory.SetupGet(x => x.SecretStore).Returns(this.mockConfigurationStore.Object);
            this.mockConfigurationStore.Setup(x => x.GetFromStoreAsync<string>("secretKey")).ReturnsAsync(secretValue);
            this.mockConfigurationStore.Setup(x => x.GetFromStoreAsync<string>("secretKey2")).ReturnsAsync(secretValue2);

            // Act
            var storageConfigurationStore = this.CreateStorageConfigurationStore();
            var configValue = await storageConfigurationStore
                    .GetFromStoreAsync<string>($"{ConfigurationConstants.DataStoreConfigurationPrefix}{testKey}")
                    .ConfigureAwait(false);
            InMemoryCacheManager.Remove("secretKey");
            InMemoryCacheManager.Remove("secretKey2");

            // Assert
            Assert.IsNotNull(configValue);
            Assert.AreEqual($"configValue_{secretValue}_blah_blah_{secretValue2}", configValue);
        }

        /// <summary>
        /// Gets from store asynchronous should return configuration value for valid key.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetFromStoreAsync_ShouldReturnDefaultValue_ForInValidStoreConfigValueAsync()
        {
            // Arrange
            var testKey = "testKey";
            var configurationSetting = new ConfigurationSetting
            {
                Key = "configKey",
                Value = "configValue_#secretKey#",
            };
            this.mockTableProvider.Setup(x => x.GetByRowKeyAndPartitionKeyAsync<ConfigurationSetting>(testKey, It.IsAny<string>())).ReturnsAsync(configurationSetting);
            this.mockConfigurationStoreFactory.SetupGet(x => x.SecretStore)
                    .Returns(this.mockConfigurationStore.Object);
            this.mockConfigurationStore.Setup(x => x.GetFromStoreAsync<string>("secretKey"))
                    .ReturnsAsync("secretValue");
            this.mockLogger.Setup(x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>()));
            this.mockResolver.Setup(x => x.GetInstance<ITrueLogger<StorageConfigurationStore>>()).Returns(this.mockLogger.Object);

            // Act
            var storageConfigurationStore = this.CreateStorageConfigurationStore();
            var configValue = await storageConfigurationStore
                    .GetFromStoreAsync<DateTimeOffset>($"{ConfigurationConstants.DataStoreConfigurationPrefix}{testKey}")
                    .ConfigureAwait(false);
            InMemoryCacheManager.Remove("secretKey");

            // Assert
            Assert.IsNotNull(configValue);
            Assert.AreEqual(default(DateTimeOffset), configValue);
        }

        /// <summary>
        /// Gets from store asynchronous should throw not supported exception if should cache flag is used.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public async Task GetFromStoreAsync_ShouldThrowNotSupportedException_IfShouldCacheFlagIsUsedAsync()
        {
            // Act
            var storageConfigurationStore = this.CreateStorageConfigurationStore();
            await storageConfigurationStore.GetFromStoreAsync<string>("testKey", true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets from store asynchronous should return all configuration value for valid partition key.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetFromStoreAsync_ShouldReturnAllConfigValue_ForValidPartitionKeyAsync()
        {
            // Arrange
            var configurationSetting = new ConfigurationSetting
            {
                Key = "ModuleAvailabilityTrueCutOff.Settings",
                Value = "{\"Name\":\"TrueCutOff\",\"Resources\":[\"FA-AEU-ECP-DEV-TRUECALCULATE\"]}",
            };

            this.mockTableProvider.Setup(x => x.ExecuteQueryAsync(It.IsAny<TableQuery<ConfigurationSetting>>())).ReturnsAsync(new List<ConfigurationSetting> { configurationSetting });

            // Act
            var storageConfigurationStore = this.CreateStorageConfigurationStore();
            var configValue = await storageConfigurationStore
                    .GetAllFromStoreAsync<ConfigurationSetting>()
                    .ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(configValue);
            Assert.AreEqual(1, configValue.Count());
        }

        /// <summary>
        /// The cleanup.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            this.mockRepository.VerifyAll();
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockConfigurationStoreFactory = this.mockRepository.Create<IConfigurationStoreFactory>();
            this.mockLogger = this.mockRepository.Create<ITrueLogger<StorageConfigurationStore>>();
            this.mockTableProvider = this.mockRepository.Create<ITableProvider>();
            this.mockConfigurationStore = this.mockRepository.Create<IConfigurationStore>();
            this.mockResolver = this.mockRepository.Create<IResolver>();
        }

        /// <summary>
        /// Creates the storage configuration store.
        /// </summary>
        /// <returns>
        /// Creates the storage configuration store as <see cref="StorageConfigurationStore" />.
        /// </returns>
        private StorageConfigurationStore CreateStorageConfigurationStore()
        {
            return new StorageConfigurationStore(
                this.mockTableProvider.Object,
                this.mockConfigurationStoreFactory.Object,
                this.mockResolver.Object,
                It.IsAny<string>());
        }
    }
}