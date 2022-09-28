// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainControllerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Tests.Admin
{
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processor.Blockchain.Events;
    using Ecp.True.Processors.Blockchain.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Nethereum.RPC.Eth.DTOs;

    /// <summary>
    /// The blockchain controller tests.
    /// </summary>
    [TestClass]
    public class BlockchainControllerTests : ControllerTestBase
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private BlockchainController controller;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<IBlockchainProcessor> processorMock;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.processorMock = new Mock<IBlockchainProcessor>();
            this.controller = new BlockchainController(this.processorMock.Object);
        }

        /// <summary>
        /// Gets unbalances for a ticket when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetEventsAsync_ReturnsEvents_WhenInvokedAsync()
        {
            this.processorMock.Setup(m => m.GetPagedEventsAsync(100, 123654)).ReturnsAsync(new EventsPage(100, 123654));

            var result = await this.controller.GetEventsAsync(new BlockEventRequest { LastHead = 123654, PageSize = 100 }).ConfigureAwait(false);

            // Assert or verify
            this.processorMock.Verify(c => c.GetPagedEventsAsync(100, 123654), Times.Once());
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }

        /// <summary>
        /// Gets the transaction asynchronous returns transaction when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetTransactionAsync_ReturnsTransaction_WhenInvokedAsync()
        {
            var request = new BlockTransactionRequest();
            this.processorMock.Setup(m => m.GetTransactionDetailsAsync(request)).ReturnsAsync(new BlockTransactionResponse(request));

            var result = await this.controller.GetTransactionAsync(request).ConfigureAwait(false);

            // Assert or verify
            this.processorMock.Verify(c => c.GetTransactionDetailsAsync(request), Times.Once());
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }

        /// <summary>
        /// Existses the transaction asynchronous returns if transaction exists when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExistsTransactionAsync_ReturnsIfTransactionExists_WhenInvokedAsync()
        {
            var request = new BlockTransactionRequest();
            var block = new Block();
            this.processorMock.Setup(m => m.TryGetBlockAsync(request)).ReturnsAsync(block);

            var result = await this.controller.ExistsTransactionAsync(request).ConfigureAwait(false);

            // Assert or verify
            this.processorMock.Verify(c => c.TryGetBlockAsync(request), Times.Once());
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }

        /// <summary>
        /// Gets the events in range asynchronous returns events when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetEventsInRangeAsync_ReturnsEvents_WhenInvokedAsync()
        {
            var request = new BlockRangeRequest();
            var events = new EventsPage(1,1);
            this.processorMock.Setup(m => m.GetEventsInRangeAsync(request)).ReturnsAsync(events);

            var result = await this.controller.GetEventsInRangeAsync(request).ConfigureAwait(false);

            // Assert or verify
            this.processorMock.Verify(m => m.GetEventsInRangeAsync(request), Times.Once());
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }

        /// <summary>
        /// Existses the transaction range asynchronous returns true if block exists when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExistsTransactionRangeAsync_ReturnsTrueIfBlockExists_WhenInvokedAsync()
        {
            var request = new BlockRangeRequest();
            this.processorMock.Setup(m => m.ValidateBlockRangeAsync(request)).ReturnsAsync(true);

            var result = await this.controller.ExistsTransactionRangeAsync(request).ConfigureAwait(false);

            // Assert or verify
            this.processorMock.Verify(m => m.ValidateBlockRangeAsync(request), Times.Once());
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsInstanceOfType(result, typeof(EntityResult));
        }
    }
}
