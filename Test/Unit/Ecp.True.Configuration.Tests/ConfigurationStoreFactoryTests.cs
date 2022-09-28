// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationStoreFactoryTests.cs" company="Microsoft">
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
    using System.Threading.Tasks;

    using Ecp.True.DataAccess;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Ioc.Interfaces;
    using Ecp.True.KeyStore.Entities;
    using Ecp.True.KeyStore.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// Configuration Store Factory Tests.
    /// </summary>
    [TestClass]
    public class ConfigurationStoreFactoryTests
    {
        /// <summary>
        /// The mock file configuration store.
        /// </summary>
        private Mock<IFileConfigurationStore> mockFileConfigurationStore;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<IResolver> mockResolver;

        /// <summary>
        /// The mock repository.
        /// </summary>
        private MockRepository mockRepository;

        /// <summary>
        /// The mock secret management provider.
        /// </summary>
        private Mock<ISecretManagementProvider> mockSecretManagementProvider;

        /// <summary>
        /// The mock table provider.
        /// </summary>
        private Mock<ITableProvider> mockTableProvider;

        /// <summary>
        /// Gets the configuration store should argument exception for data store configuration prefix when regional secret store is not set.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetConfigurationStore_ShouldArgumentException_ForDataStoreConfigurationPrefix_WhenRegionalSecretStoreIsNotSetAsync()
        {
            // Act
            var factory = this.CreateFactory();
            await factory.GetConfigurationStoreAsync($"{ConfigurationConstants.DataStoreConfigurationPrefix}Key").ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the configuration store should return file configuration store for file configuration prefix.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetConfigurationStore_ShouldReturnFileConfigStore_ForFileConfigurationPrefixAsync()
        {
            // Act
            var factory = this.CreateFactory();
            var result = await factory.GetConfigurationStoreAsync($"{ConfigurationConstants.FileConfigurationPrefix}Key").ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(this.mockFileConfigurationStore.Object, result);
        }

        /// <summary>
        /// Gets the configuration store should return secret configuration store for secret configuration prefix.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetConfigurationStore_ShouldReturnSecretConfigStore_ForSecretConfigurationPrefixAsync()
        {
            // Act
            var factory = this.CreateFactory();
            var result = await factory.GetConfigurationStoreAsync($"{ConfigurationConstants.SecretConfigurationPrefix}Key").ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(SecretConfigurationStore), result.GetType());
        }

        /// <summary>
        /// Gets the configuration store should return storage configuration store for data store configuration prefix.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetConfigurationStore_ShouldReturnStorageConfigurationStore_ForDataStoreConfigurationPrefixAsync()
        {
            // Arrange
            var keyvaultConfig = new KeyVaultConfiguration();
            ////var secretBundle = new SecretBundle
            ////{
            ////    Value = "secretValue",
            ////};
            ////this.mockSecretManagementProvider.Setup(x => x.GetSecretAsync(ConfigurationConstants.ConfigurationVersion.Replace(ConfigurationConstants.SecretConfigurationPrefix, string.Empty, StringComparison.OrdinalIgnoreCase)))
            ////        .ReturnsAsync(secretBundle);

            // Act
            var factory = this.CreateFactory();
            await factory.InitializeConfigurationStoresAsync(keyvaultConfig).ConfigureAwait(false);
            var result = await factory.GetConfigurationStoreAsync($"{ConfigurationConstants.DataStoreConfigurationPrefix}Key").ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(StorageConfigurationStore), result.GetType());

            result = await factory.GetConfigurationStoreAsync($"{ConfigurationConstants.DataStoreConfigurationPrefix}Key").ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(StorageConfigurationStore), result.GetType());
            ////this.mockSecretManagementProvider.Verify(x => x.GetSecretAsync(ConfigurationConstants.ConfigurationVersion.Replace(ConfigurationConstants.SecretConfigurationPrefix, string.Empty, StringComparison.OrdinalIgnoreCase)), Times.Once);
        }

        /// <summary>
        /// Gets the configuration store should return storage configuration store for incorrect configuration prefix.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task GetConfigurationStore_ShouldThrowException_ForIncorrectConfigurationPrefixAsync()
        {
            // Act
            var factory = this.CreateFactory();
            var result = await factory.GetConfigurationStoreAsync($"Incorrect_Key").ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// Initializes the configuration stores should initialize regional secret store when configuration not provided.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task InitializeConfigurationStores_ShouldInitializeRegionalSecretStore_WhenConfigurationNotProvidedAsync()
        {
            // Arrange
            KeyVaultConfiguration callBackKeyvaultConfig = null;
            this.mockSecretManagementProvider.Setup(x => x.InitializeSettings(It.IsAny<KeyVaultConfiguration>()))
                    .Callback<KeyVaultConfiguration>(x => callBackKeyvaultConfig = x);
            this.mockFileConfigurationStore.Setup(x => x.GetFromStoreAsync<string>(ConfigurationConstants.KeyVaultName))
                    .ReturnsAsync("vaultname");

            // Act
            var factory = this.CreateFactory();
            var result = await factory.InitializeConfigurationStoresAsync(null).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(typeof(SecretConfigurationStore), factory.SecretStore.GetType());
            Assert.IsNotNull(callBackKeyvaultConfig);
            Assert.AreEqual("https://vaultname.vault.azure.net", callBackKeyvaultConfig.VaultAddress);
        }

        /// <summary>
        /// Initializes the configuration stores should initialize regional secret store when configuration provided.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task InitializeConfigurationStores_ShouldInitializeRegionalSecretStore_WhenConfigurationProvidedAsync()
        {
            // Arrange
            var keyvaultConfig = new KeyVaultConfiguration();
            this.mockSecretManagementProvider.Setup(x => x.InitializeSettings(keyvaultConfig));

            // Act
            var factory = this.CreateFactory();
            var result = await factory.InitializeConfigurationStoresAsync(keyvaultConfig).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(typeof(SecretConfigurationStore), factory.SecretStore.GetType());
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
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockFileConfigurationStore = this.mockRepository.Create<IFileConfigurationStore>();
            this.mockSecretManagementProvider = this.mockRepository.Create<ISecretManagementProvider>();
            this.mockTableProvider = this.mockRepository.Create<ITableProvider>();
            this.mockResolver = this.mockRepository.Create<IResolver>();
        }

        /// <summary>
        /// Creates the factory.
        /// </summary>
        /// <returns>
        /// Creates the factory as <see cref="ConfigurationStoreFactory" />.
        /// </returns>
        private ConfigurationStoreFactory CreateFactory()
        {
            return new ConfigurationStoreFactory(
                this.mockFileConfigurationStore.Object,
                this.mockSecretManagementProvider.Object,
                this.mockTableProvider.Object,
                this.mockResolver.Object);
        }
    }
}