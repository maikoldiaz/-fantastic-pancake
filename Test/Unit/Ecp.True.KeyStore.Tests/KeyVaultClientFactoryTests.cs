// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyVaultClientFactoryTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.KeyStore.Tests
{
    using System;
    using Ecp.True.Ioc.Interfaces;
    using Ecp.True.KeyStore.Entities;
    using Ecp.True.KeyStore.Interfaces;
    using Ecp.True.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Key Vault Client Factory Tests.
    /// </summary>
    [TestClass]
    public class KeyVaultClientFactoryTests
    {
        /// <summary>
        /// The mock key vault context.
        /// </summary>
        private readonly Mock<IKeyVaultContext> mockKeyVaultContext = new Mock<IKeyVaultContext>();

        /// <summary>
        /// The mock logger.
        /// </summary>
        private readonly Mock<ITrueLogger<KeyVaultClientFactory>> mockLogger = new Mock<ITrueLogger<KeyVaultClientFactory>>();

        private readonly Mock<IResolver> mockResolver = new Mock<IResolver>();

        /// <summary>
        /// The mock logger.
        /// </summary>
        private IKeyVaultClientFactory keyVaultClientFactory;

        /// <summary>
        /// Gets the key vault client should throw invalid operation exception if keyvault client is not initialized.
        /// </summary>
        [TestMethod]
        public void GetKeyVaultClientShouldReturnExistingKeyVaultClientIfSameVaultAddressIsSent()
        {
            // Arrange
            this.mockKeyVaultContext.SetupGet(x => x.Initialized).Returns(true);
            this.mockKeyVaultContext.SetupGet(x => x.Settings).Returns(new KeyVaultConfiguration { VaultAddress = "vaultAddress", AuthenticationMode = AuthenticationMode.CertificateThumbprint, ClientAuthId = "test", CertificateThumbprint = "test" });

            // Act
            var keyVaultClient1 = this.keyVaultClientFactory.GetKeyVaultClient();
            var keyVaultClient2 = this.keyVaultClientFactory.GetKeyVaultClient();

            // Assert
            Assert.IsNotNull(keyVaultClient1);
            Assert.IsNotNull(keyVaultClient2);
            Assert.AreEqual(keyVaultClient1, keyVaultClient2);
        }

        /// <summary>
        /// Gets the key vault client should throw invalid operation exception if keyvault client is not initialized.
        /// </summary>
        [TestMethod]
        public void GetKeyVaultClientShouldReturnKeyVaultClient()
        {
            // Arrange
            this.mockKeyVaultContext.SetupGet(x => x.Initialized).Returns(true);
            this.mockKeyVaultContext.SetupGet(x => x.Settings).Returns(new KeyVaultConfiguration { VaultAddress = "vaultAddress", AuthenticationMode = AuthenticationMode.CertificateThumbprint, ClientAuthId = "test", CertificateThumbprint = "test" });

            // Act
            var keyVaultClient = this.keyVaultClientFactory.GetKeyVaultClient();

            // Assert
            Assert.IsNotNull(keyVaultClient);
        }

        /// <summary>
        /// Gets the key vault client should throw invalid operation exception if keyvault initialized is false.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetKeyVaultClientShouldThrowInvalidOperationExceptionIfKeyVaultClientInitializedIsFalse()
        {
            // Arrange
            this.mockKeyVaultContext.SetupGet(x => x.Initialized).Returns(false);

            // Act
            this.keyVaultClientFactory.GetKeyVaultClient();

            // Assert
        }

        /// <summary>
        /// Gets the key vault client should throw invalid operation exception if keyvault client is not initialized.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetKeyVaultClientShouldThrowInvalidOperationExceptionIfKeyVaultClientIsNotInitialized()
        {
            // Arrange
            this.mockKeyVaultContext.SetupGet(x => x.Settings).Returns(default(KeyVaultConfiguration));

            // Act
            this.keyVaultClientFactory.GetKeyVaultClient();

            // Assert
        }

        /// <summary>
        /// InitializeSettingsShouldCallKeyVaultInitializeSettings.
        /// </summary>
        [TestMethod]
        public void GetKeyVaultConfigurationShouldReturnConfiguration()
        {
            // Arrange
            var keyVaultConfiguration = new KeyVaultConfiguration { VaultAddress = Guid.NewGuid().ToString() };
            this.mockKeyVaultContext.SetupGet(x => x.Initialized).Returns(true);
            this.mockKeyVaultContext.SetupGet(x => x.Settings).Returns(keyVaultConfiguration);

            // Act
            var configuration = this.keyVaultClientFactory.GetKeyVaultClientConfiguration();

            // Assert
            Assert.IsNotNull(configuration);
            Assert.AreEqual(configuration.VaultAddress, keyVaultConfiguration.VaultAddress);
        }

        /// <summary>
        /// Gets the key vault settings should throw invalid operation exception if keyvault initialized is false.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetKeyVaultSettingsShouldThrowInvalidOperationExceptionIfKeyVaultClientInitializedIsFalse()
        {
            // Arrange
            this.mockKeyVaultContext.SetupGet(x => x.Initialized).Returns(false);

            // Act
            this.keyVaultClientFactory.GetKeyVaultClientConfiguration();

            // Assert
        }

        /// <summary>
        /// Gets the key vault settings should throw invalid operation exception if keyvault client is not initialized.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetKeyVaultSettingsShouldThrowInvalidOperationExceptionIfKeyVaultClientIsNotInitialized()
        {
            // Arrange
            this.mockKeyVaultContext.SetupGet(x => x.Settings).Returns(default(KeyVaultConfiguration));

            // Act
            this.keyVaultClientFactory.GetKeyVaultClientConfiguration();

            // Assert
        }

        /// <summary>
        /// InitializeSettingsShouldCallKeyVaultInitializeSettings.
        /// </summary>
        [TestMethod]
        public void InitializeSettingsShouldCallKeyVaultCOntextInitializeSettings()
        {
            // Arrange
            this.mockKeyVaultContext.Setup(m => m.InitializeSettings(It.IsAny<KeyVaultConfiguration>()));

            // Act
            this.keyVaultClientFactory.InitializeSettings(new KeyVaultConfiguration());

            // Assert
            this.mockKeyVaultContext.Verify(m => m.InitializeSettings(It.IsAny<KeyVaultConfiguration>()), Times.Once);
        }

        /// <summary>
        /// Setups this instance.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            this.mockResolver.Setup(x => x.GetInstance<ITrueLogger<KeyVaultClientFactory>>()).Returns(this.mockLogger.Object);
            this.keyVaultClientFactory = new KeyVaultClientFactory(this.mockKeyVaultContext.Object, this.mockResolver.Object);
        }
    }
}