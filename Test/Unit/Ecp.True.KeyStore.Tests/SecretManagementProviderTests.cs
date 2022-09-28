// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SecretManagementProviderTests.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Ioc.Interfaces;
    using Ecp.True.KeyStore.Entities;
    using Ecp.True.KeyStore.Interfaces;
    using Ecp.True.Logging;
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.KeyVault.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Secret Management Provider Tests.
    /// </summary>
    [TestClass]
    public class SecretManagementProviderTests
    {
        /// <summary>
        /// The mock key vault client factory.
        /// </summary>
        private readonly Mock<IKeyVaultClientFactory> mockKeyVaultClientFactory = new Mock<IKeyVaultClientFactory>();

        /// <summary>
        /// The mock key vault extension wrapper.
        /// </summary>
        private readonly Mock<IKeyVaultExtensionsWrapper> mockKeyVaultExtensionsWrapper = new Mock<IKeyVaultExtensionsWrapper>();

        /// <summary>
        /// The mock logger.
        /// </summary>
        private readonly Mock<ITrueLogger<SecretManagementProvider>> mockLogger = new Mock<ITrueLogger<SecretManagementProvider>>();

        private readonly Mock<IResolver> mockResolver = new Mock<IResolver>();

        /// <summary>
        /// The secret Management Provider.
        /// </summary>
        private ISecretManagementProvider secretManagementProvider;

        /// <summary>
        /// Delete the secret should call delete secret from key vault wrapper.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task DeleteSecretShouldCallDeleteSecretFromKeyVaultWrapperAsync()
        {
            // Arrange
            var deletedSecretBundle = new DeletedSecretBundle { Id = "Id" };
            this.mockKeyVaultExtensionsWrapper.Setup(m => m.DeleteSecretAsync(null, It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(deletedSecretBundle);
            var secretData = new SetSecretData { SecretName = " test", VaultAddress = "address", SecretValue = "SecretValue" };

            // Act
            var result = await this.secretManagementProvider.DeleteSecretAsync(secretData).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result, deletedSecretBundle);
            this.mockKeyVaultExtensionsWrapper.Verify(m => m.DeleteSecretAsync(null, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Delete the secret test should return null if exception occurs.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous unit test.</returns>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task DeleteSecretTestShouldBubbleIfExceptionOccursAsync()
        {
            // Arrange
            this.mockLogger.Setup(m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>()));
            this.mockKeyVaultExtensionsWrapper.Setup(m => m.DeleteSecretAsync(null, It.IsAny<string>(), It.IsAny<string>())).Throws(new NullReferenceException());
            var secretData = new SetSecretData { SecretName = " test", VaultAddress = "address", SecretValue = "SecretValue" };

            // Act
            var result = await this.secretManagementProvider.DeleteSecretAsync(secretData).ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
            this.mockLogger.Verify(
                m => m.LogError(
                    It.IsAny<Exception>(),
                    It.IsAny<string>()),
                Times.Once);
        }

        /// <summary>
        /// Delete the secret test should return null if secret data is not passed.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task DeleteSecretTestShouldReturnNullIfSecretDataIsNotProvidedAsync()
        {
            // Arrange
            this.mockLogger.Setup(m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>()));

            // Act
            var result = await this.secretManagementProvider.DeleteSecretAsync(null).ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
            this.mockLogger.Verify(
                m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>()),
                Times.Once);
        }

        /// <summary>
        /// Get all the secret should call Get all secret from key vault wrapper.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task GetAllSecretsShouldCallGetSecretsFromKeyVaultWrapperAsync()
        {
            // Arrange
            var secretItems = new Page<SecretItem>();
            this.mockKeyVaultExtensionsWrapper.Setup(m => m.GetSecretsAsync(null, It.IsAny<string>())).ReturnsAsync(secretItems);
            var secretData = new SetSecretData { SecretName = " test", VaultAddress = "address", SecretValue = "SecretValue" };

            // Act
            var result = await this.secretManagementProvider.GetAllSecretsAsync(secretData).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            this.mockKeyVaultExtensionsWrapper.Verify(m => m.GetSecretsAsync(null, It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Get all secret test should return null if exception occurs.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous unit test.</returns>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task GetAllSecretsShouldBubbleIfExceptionOccursAsync()
        {
            // Arrange
            this.mockLogger.Setup(m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>()));
            this.mockKeyVaultExtensionsWrapper.Setup(m => m.GetSecretsAsync(null, It.IsAny<string>())).Throws(new NullReferenceException());
            var secretData = new SetSecretData { SecretName = " test", VaultAddress = "address", SecretValue = "SecretValue" };

            // Act
            var result = await this.secretManagementProvider.GetAllSecretsAsync(secretData).ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
            this.mockLogger.Verify(
                m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>()),
                Times.Once);
        }

        /// <summary>
        /// Get all secret test should return null if secret data is not passed.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task GetAllSecretsTestShouldReturnNullIfSecretDataIsNotProvidedAsync()
        {
            // Arrange
            this.mockLogger.Setup(m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>()));

            // Act
            var result = await this.secretManagementProvider.GetAllSecretsAsync(null).ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
            this.mockLogger.Verify(
                m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>()),
                Times.Once);
        }

        /// <summary>
        /// Gets the secret should call Get Secret from key vault wrapper.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task GetSecretShouldCallGetSecretFromKeyVaultWrapperAsync()
        {
            // Arrange
            var secretBundle = new SecretBundle { Id = Guid.NewGuid().ToString() };
            this.mockKeyVaultExtensionsWrapper.Setup(m => m.GetSecretAsync(It.IsAny<IKeyVaultClient>(), It.IsAny<string>())).ReturnsAsync(secretBundle);

            // Act
            var result = await this.secretManagementProvider.GetSecretAsync("key").ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, secretBundle.Id);
            this.mockKeyVaultExtensionsWrapper.Verify(m => m.GetSecretAsync(null, It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Get secret should return null if exception occurs.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous unit test.</returns>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task GetSecretShouldBubbleIfExceptionOccursAsync()
        {
            // Arrange
            this.mockLogger.Setup(m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>()));
            this.mockKeyVaultExtensionsWrapper.Setup(m => m.GetSecretAsync(It.IsAny<IKeyVaultClient>(), It.IsAny<string>())).Throws(new NullReferenceException());

            // Act
            var result = await this.secretManagementProvider.GetSecretAsync("key").ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
            this.mockLogger.Verify(
                m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>()),
                Times.Once);
        }

        /// <summary>
        /// Sets the secret should call set secret from key vault wrapper.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task SetSecretShouldCallSetSecretFromKeyVaultWrapperAsync()
        {
            // Arrange
            var secretBundle = new SecretBundle { Id = "Id" };
            this.mockKeyVaultExtensionsWrapper.Setup(m => m.SetSecretAsync(null, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(secretBundle);
            var secretData = new SetSecretData { SecretName = " test", VaultAddress = "address", SecretValue = "SecretValue" };

            // Act
            var result = await this.secretManagementProvider.SetSecretAsync(secretData).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result, secretBundle);
            this.mockKeyVaultExtensionsWrapper.Verify(m => m.SetSecretAsync(null, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Set secret should return null if exception occurs.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous unit test.</returns>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task SetSecretShouldBubbleIfExceptionOccursAsync()
        {
            // Arrange
            this.mockLogger.Setup(m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>()));
            this.mockKeyVaultExtensionsWrapper.Setup(m => m.SetSecretAsync(null, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws(new NullReferenceException());
            var secretData = new SetSecretData { SecretName = " test", VaultAddress = "address", SecretValue = "SecretValue" };

            // Act
            var result = await this.secretManagementProvider.SetSecretAsync(secretData).ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
            this.mockLogger.Verify(
                m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>()),
                Times.Once);
        }

        /// <summary>
        /// Set secret test should return null if secret data is not passed.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task SetSecretsTestShouldReturnNullIfSecretDataIsNotProvidedAsync()
        {
            // Arrange
            this.mockLogger.Setup(m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>()));

            // Act
            var result = await this.secretManagementProvider.SetSecretAsync(null).ConfigureAwait(false);

            // Assert
            Assert.IsNull(result);
            this.mockLogger.Verify(
                m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>()),
                Times.Once);
        }

        /// <summary>
        /// Setups this instance.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            this.mockResolver.Setup(m => m.GetInstance<ITrueLogger<SecretManagementProvider>>()).Returns(this.mockLogger.Object);
            this.secretManagementProvider = new SecretManagementProvider(this.mockResolver.Object, this.mockKeyVaultClientFactory.Object, this.mockKeyVaultExtensionsWrapper.Object);
            var keyVaultConfiguration = new KeyVaultConfiguration { VaultAddress = "test" };
            this.secretManagementProvider.InitializeSettings(keyVaultConfiguration);
        }
    }
}