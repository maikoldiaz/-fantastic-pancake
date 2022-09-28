// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SecretConfigurationStoreTests.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.KeyStore.Interfaces;

    using Microsoft.Azure.KeyVault.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// Secret Configuration Store Tests.
    /// </summary>
    [TestClass]
    public class SecretConfigurationStoreTests
    {
        /// <summary>
        /// The mock repository..
        /// </summary>
        private MockRepository mockRepository;

        /// <summary>
        /// The mock secret management provider.
        /// </summary>
        private Mock<ISecretManagementProvider> mockSecretManagementProvider;

        /// <summary>
        /// Gets from store asynchronous should return configuration value for valid key.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetFromStoreAsync_ShouldReturnConfigValue_ForValidKeyAsync()
        {
            // Arrange
            var testKey = "testKey";
            var secretBundle = new SecretBundle
            {
                Value = "secretValue",
            };
            this.mockSecretManagementProvider.Setup(x => x.GetSecretAsync(testKey)).ReturnsAsync(secretBundle);

            // Act
            var secretConfigurationStore = this.CreateSecretConfigurationStore();
            var configValue = await secretConfigurationStore
                    .GetFromStoreAsync<string>($"{ConfigurationConstants.SecretConfigurationPrefix}{testKey}")
                    .ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(configValue);
            Assert.AreEqual("secretValue", configValue);
        }

        /// <summary>
        /// Gets from store asynchronous should return configuration value for valid key.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GetFromStoreAsync_ShouldThrowError_ForInValidConfigValueAsync()
        {
            // Arrange
            var testKey = "testKey";
            var secretBundle = new SecretBundle
            {
                Value = null,
            };
            this.mockSecretManagementProvider.Setup(x => x.GetSecretAsync(testKey)).ReturnsAsync(secretBundle);

            // Act
            var secretConfigurationStore = this.CreateSecretConfigurationStore();
            var configValue = await secretConfigurationStore
                    .GetFromStoreAsync<TimeSpan>($"{ConfigurationConstants.SecretConfigurationPrefix}{testKey}")
                    .ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(configValue);
            Assert.AreEqual(default(TimeSpan), configValue);
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
            var secretConfigurationStore = this.CreateSecretConfigurationStore();
            await secretConfigurationStore.GetFromStoreAsync<string>("testKey", true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets from store asynchronous should return error for null secret.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GetFromStoreAsync_ShouldThrowError_ForNullSecretAsync()
        {
            // Arrange
            var testKey = "testKey";
            this.mockSecretManagementProvider.Setup(x => x.GetSecretAsync(testKey)).ReturnsAsync((SecretBundle)null);

            // Act
            var secretConfigurationStore = this.CreateSecretConfigurationStore();
            var configValue = await secretConfigurationStore
                    .GetFromStoreAsync<TimeSpan>($"{ConfigurationConstants.SecretConfigurationPrefix}{testKey}")
                    .ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(configValue);
            Assert.AreEqual(default(TimeSpan), configValue);
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
            this.mockSecretManagementProvider = this.mockRepository.Create<ISecretManagementProvider>();
        }

        /// <summary>
        /// Creates the secret configuration store.
        /// </summary>
        /// <returns>
        /// Creates the secret configuration store as <see cref="SecretConfigurationStore" />.
        /// </returns>
        private SecretConfigurationStore CreateSecretConfigurationStore()
        {
            return new SecretConfigurationStore(this.mockSecretManagementProvider.Object);
        }
    }
}