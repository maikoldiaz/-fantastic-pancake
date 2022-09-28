// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileConfigurationStoreTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Services.Api.Tests
{
    using System.Threading.Tasks;

    using Ecp.True.Entities.Configuration;
    using Ecp.True.Host.Common;

    using Microsoft.Extensions.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// The file configuration store tests.
    /// </summary>
    [TestClass]
    public class FileConfigurationStoreTests
    {
        /// <summary>
        /// The configuration mock.
        /// </summary>
        private Mock<IConfiguration> configurationMock;

        /// <summary>
        /// The configuration section mock.
        /// </summary>
        private Mock<IConfigurationSection> configurationSectionMock;

        /// <summary>
        /// The configuration store.
        /// </summary>
        private FileConfigurationStore configurationStore;

        /// <summary>
        /// Constructor should read key vault section when invoked.
        /// </summary>
        [TestMethod]
        public void Constructor_ShouldReadKeyVaultSection_WhenInvoked()
        {
            this.configurationMock.Verify(c => c.GetSection("Settings"), Times.Once);
        }

        /// <summary>
        /// Gets from store asynchronous should return configuration setting when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetFromStoreAsync_ShouldReturnConfigSetting_WhenInvokedAsync()
        {
            var result = await this.configurationStore.GetFromStoreAsync<string>(ConfigurationConstants.KeyVaultName).ConfigureAwait(false);
            Assert.AreEqual("KeyVaultName", result);
        }

        /// <summary>
        /// Gets from store asynchronous should return configuration setting when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetFromStoreAsync_ShouldReturnNull_WhenConfigValueIsEmptyAsync()
        {
            var result = await this.configurationStore.GetFromStoreAsync<string>("InvalidKey").ConfigureAwait(false);
            Assert.IsNull(result);
        }

        /// <summary>
        /// Gets from store asynchronous should return configuration setting when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetFromStoreAsyncWithCache_ShouldReturnDefaultValue_WhenInvokedAsync()
        {
            var result = await this.configurationStore.GetFromStoreAsync<string>(ConfigurationConstants.KeyVaultName, true).ConfigureAwait(false);
            Assert.AreEqual(null, result);
        }

        /// <summary>
        /// Setups this instance.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            this.configurationMock = new Mock<IConfiguration>();
            this.configurationSectionMock = new Mock<IConfigurationSection>();

            this.configurationMock
                    .Setup(c => c.GetSection("Settings"))
                    .Returns(this.configurationSectionMock.Object);

            var keyName = ConfigurationConstants.KeyVaultName.Replace(ConfigurationConstants.FileConfigurationPrefix, string.Empty, System.StringComparison.OrdinalIgnoreCase);
            this.configurationSectionMock.SetupGet(s => s[keyName]).Returns("KeyVaultName");

            this.configurationSectionMock.SetupGet(s => s["InvalidKey"]).Returns(string.Empty);

            this.configurationStore = new FileConfigurationStore(this.configurationMock.Object);
        }
    }
}