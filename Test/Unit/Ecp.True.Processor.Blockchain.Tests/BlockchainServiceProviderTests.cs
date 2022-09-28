// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainServiceProviderTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Blockchain.Tests
{
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Processors.Blockchain;
    using Ecp.True.Processors.Blockchain.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The blockchain service provider tests.
    /// </summary>
    [TestClass]
    public class BlockchainServiceProviderTests
    {
        /// <summary>
        /// The provider.
        /// </summary>
        private BlockchainServiceProvider provider;

        /// <summary>
        /// The configuration mock.
        /// </summary>
        private Mock<IConfigurationHandler> configMock;

        /// <summary>
        /// The inventory service mock.
        /// </summary>
        private Mock<IBlockchainService> inventoryServiceMock;

        /// <summary>
        /// The node service mock.
        /// </summary>
        private Mock<IBlockchainService> nodeServiceMock;

        /// <summary>
        /// The settings.
        /// </summary>
        private BlockchainSettings settings;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.configMock = new Mock<IConfigurationHandler>();
            this.inventoryServiceMock = new Mock<IBlockchainService>();
            this.nodeServiceMock = new Mock<IBlockchainService>();

            this.settings = new BlockchainSettings();
            this.configMock.Setup(c => c.GetConfigurationAsync<BlockchainSettings>(ConfigurationConstants.BlockchainSettings)).ReturnsAsync(this.settings);

            this.nodeServiceMock.SetupGet(n => n.Type).Returns(ServiceType.Node);
            this.nodeServiceMock.Setup(n => n.Initialize(this.settings));

            this.inventoryServiceMock.SetupGet(n => n.Type).Returns(ServiceType.InventoryProduct);
            this.inventoryServiceMock.Setup(n => n.Initialize(this.settings));

            var services = new[] { this.nodeServiceMock.Object, this.inventoryServiceMock.Object };
            this.provider = new BlockchainServiceProvider(this.configMock.Object, services);
        }

        /// <summary>
        /// Gets the blockchain service should return node service when invoked for node type asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetBlockchainService_ShouldReturnNodeService_WhenInvokedForNodeTypeAsync()
        {
            var service = await this.provider.GetBlockchainServiceAsync(ServiceType.Node).ConfigureAwait(false);

            Assert.AreEqual(this.nodeServiceMock.Object, service);

            this.nodeServiceMock.Verify(n => n.Initialize(this.settings), Times.Once);
            this.configMock.Verify(c => c.GetConfigurationAsync<BlockchainSettings>(ConfigurationConstants.BlockchainSettings), Times.Once);
        }

        /// <summary>
        /// Gets the blockchain service should return inventory service when invoked for inventory type asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetBlockchainService_ShouldReturnInventoryService_WhenInvokedForInventoryTypeAsync()
        {
            var service = await this.provider.GetBlockchainServiceAsync(ServiceType.InventoryProduct).ConfigureAwait(false);

            Assert.AreEqual(this.inventoryServiceMock.Object, service);

            this.inventoryServiceMock.Verify(n => n.Initialize(this.settings), Times.Once);
            this.configMock.Verify(c => c.GetConfigurationAsync<BlockchainSettings>(ConfigurationConstants.BlockchainSettings), Times.Once);
        }
    }
}
