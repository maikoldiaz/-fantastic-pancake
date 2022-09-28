// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventDecoderV2Tests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Blockchain.Tests.Decoders
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Processors.Blockchain.Decoders;
    using Ecp.True.Processors.Blockchain.Events;
    using Ecp.True.Processors.Blockchain.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The event decoder version 2 tests.
    /// </summary>
    [TestClass]
    public class EventDecoderV2Tests
    {
        /// <summary>
        /// The block number.
        /// </summary>
        private const ulong BlockNumber = 10987;

        /// <summary>
        /// The transaction hash.
        /// </summary>
        private const string TransactionHash = "Hash";

        /// <summary>
        /// The decoder.
        /// </summary>
        private EventDecoderV2 decoder;

        /// <summary>
        /// The factory mock.
        /// </summary>
        private Mock<IAzureClientFactory> factoryMock;

        /// <summary>
        /// The ethereum mock.
        /// </summary>
        private Mock<IEthereumClient> ethereumMock;

        /// <summary>
        /// The event mock.
        /// </summary>
        private Mock<IBlockchainEventV2> eventMock;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.ethereumMock = new Mock<IEthereumClient>();
            this.factoryMock = new Mock<IAzureClientFactory>();
            this.eventMock = new Mock<IBlockchainEventV2>();
            this.eventMock.Setup(e => e.BlockNumber).Returns(BlockNumber);
            this.eventMock.Setup(e => e.TransactionHash).Returns(TransactionHash);

            this.factoryMock.SetupGet(s => s.EthereumClient).Returns(this.ethereumMock.Object);
            this.decoder = new EventDecoderV2(this.factoryMock.Object);
        }

        /// <summary>
        /// Decoders the should decode node when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Decoder_ShouldDecodeToEmtpyJObject_WhenEventTypeIsUnknownAsync()
        {
            this.eventMock.Setup(e => e.Type).Returns(11);

            var eventInfo = await this.decoder.DecodeAsync(this.eventMock.Object).ConfigureAwait(false);

            Assert.IsNotNull(eventInfo);
            Assert.IsTrue(eventInfo.Count == 0);
            this.ethereumMock.Verify(e => e.GetEventsAsync<MovementOwnerEvent>(BlockNumber, TransactionHash), Times.Never);
        }

        /// <summary>
        /// Decoders the should decode node when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Decoder_ShouldDecodeMovementOwner_WhenInvokedAsync()
        {
            this.eventMock.Setup(e => e.Type).Returns(2);
            var events = new[] { new MovementOwnerEvent() };
            this.ethereumMock.Setup(e => e.GetEventsAsync<MovementOwnerEvent>(BlockNumber, TransactionHash)).ReturnsAsync(events);

            var eventInfo = await this.decoder.DecodeAsync(this.eventMock.Object).ConfigureAwait(false);

            Assert.IsNotNull(eventInfo);
            Assert.IsTrue(eventInfo.Count > 0);

            this.ethereumMock.Setup(e => e.GetEventsAsync<MovementOwnerEvent>(BlockNumber, TransactionHash)).ReturnsAsync(new List<MovementOwnerEvent>());
            eventInfo = await this.decoder.DecodeAsync(this.eventMock.Object).ConfigureAwait(false);

            Assert.IsNotNull(eventInfo);
            Assert.IsTrue(eventInfo.Count == 0);

            this.ethereumMock.Verify(e => e.GetEventsAsync<MovementOwnerEvent>(BlockNumber, TransactionHash), Times.Exactly(2));
        }

        /// <summary>
        /// Decoders the should decode node when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Decoder_ShouldDecodeInventoryOwner_WhenInvokedAsync()
        {
            this.eventMock.Setup(e => e.Type).Returns(4);
            var events = new[] { new InventoryProductOwnerEvent() };
            this.ethereumMock.Setup(e => e.GetEventsAsync<InventoryProductOwnerEvent>(BlockNumber, TransactionHash)).ReturnsAsync(events);

            var eventInfo = await this.decoder.DecodeAsync(this.eventMock.Object).ConfigureAwait(false);

            Assert.IsNotNull(eventInfo);
            Assert.IsTrue(eventInfo.Count > 0);

            this.ethereumMock.Setup(e => e.GetEventsAsync<InventoryProductOwnerEvent>(BlockNumber, TransactionHash)).ReturnsAsync(new List<InventoryProductOwnerEvent>());
            eventInfo = await this.decoder.DecodeAsync(this.eventMock.Object).ConfigureAwait(false);

            Assert.IsNotNull(eventInfo);
            Assert.IsTrue(eventInfo.Count == 0);

            this.ethereumMock.Verify(e => e.GetEventsAsync<InventoryProductOwnerEvent>(BlockNumber, TransactionHash), Times.Exactly(2));
        }

        /// <summary>
        /// Decoders the should decode node when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Decoder_ShouldDecodeMovementOwnership_WhenInvokedAsync()
        {
            this.eventMock.Setup(e => e.Type).Returns(5);
            var events = new[] { new MovementOwnershipEvent() };
            this.ethereumMock.Setup(e => e.GetEventsAsync<MovementOwnershipEvent>(BlockNumber, TransactionHash)).ReturnsAsync(events);

            var eventInfo = await this.decoder.DecodeAsync(this.eventMock.Object).ConfigureAwait(false);

            Assert.IsNotNull(eventInfo);
            Assert.IsTrue(eventInfo.Count > 0);

            this.ethereumMock.Setup(e => e.GetEventsAsync<MovementOwnershipEvent>(BlockNumber, TransactionHash)).ReturnsAsync(new List<MovementOwnershipEvent>());
            eventInfo = await this.decoder.DecodeAsync(this.eventMock.Object).ConfigureAwait(false);

            Assert.IsNotNull(eventInfo);
            Assert.IsTrue(eventInfo.Count == 0);

            this.ethereumMock.Verify(e => e.GetEventsAsync<MovementOwnershipEvent>(BlockNumber, TransactionHash), Times.Exactly(2));
        }

        /// <summary>
        /// Decoders the should decode node when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Decoder_ShouldDecodeInventoryOwnership_WhenInvokedAsync()
        {
            this.eventMock.Setup(e => e.Type).Returns(6);
            var events = new[] { new InventoryProductOwnershipEvent() };
            this.ethereumMock.Setup(e => e.GetEventsAsync<InventoryProductOwnershipEvent>(BlockNumber, TransactionHash)).ReturnsAsync(events);

            var eventInfo = await this.decoder.DecodeAsync(this.eventMock.Object).ConfigureAwait(false);

            Assert.IsNotNull(eventInfo);
            Assert.IsTrue(eventInfo.Count > 0);

            this.ethereumMock.Setup(e => e.GetEventsAsync<InventoryProductOwnershipEvent>(BlockNumber, TransactionHash)).ReturnsAsync(new List<InventoryProductOwnershipEvent>());
            eventInfo = await this.decoder.DecodeAsync(this.eventMock.Object).ConfigureAwait(false);

            Assert.IsNotNull(eventInfo);
            Assert.IsTrue(eventInfo.Count == 0);

            this.ethereumMock.Verify(e => e.GetEventsAsync<InventoryProductOwnershipEvent>(BlockNumber, TransactionHash), Times.Exactly(2));
        }

        /// <summary>
        /// Decoders the should decode node when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Decoder_ShouldDecodeNode_WhenInvokedAsync()
        {
            this.eventMock.Setup(e => e.Type).Returns(8);
            var events = new[] { new NodeEvent() };
            this.ethereumMock.Setup(e => e.GetEventsAsync<NodeEvent>(BlockNumber, TransactionHash)).ReturnsAsync(events);

            var eventInfo = await this.decoder.DecodeAsync(this.eventMock.Object).ConfigureAwait(false);

            Assert.IsNotNull(eventInfo);
            Assert.IsTrue(eventInfo.Count > 0);

            this.ethereumMock.Setup(e => e.GetEventsAsync<NodeEvent>(BlockNumber, TransactionHash)).ReturnsAsync(new List<NodeEvent>());
            eventInfo = await this.decoder.DecodeAsync(this.eventMock.Object).ConfigureAwait(false);

            Assert.IsNotNull(eventInfo);
            Assert.IsTrue(eventInfo.Count == 0);

            this.ethereumMock.Verify(e => e.GetEventsAsync<NodeEvent>(BlockNumber, TransactionHash), Times.Exactly(2));
        }

        /// <summary>
        /// Decoders the should decode node when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Decoder_ShouldDecodeNodeConnection_WhenInvokedAsync()
        {
            this.eventMock.Setup(e => e.Type).Returns(9);
            var events = new[] { new NodeConnectionEvent() };
            this.ethereumMock.Setup(e => e.GetEventsAsync<NodeConnectionEvent>(BlockNumber, TransactionHash)).ReturnsAsync(events);

            var eventInfo = await this.decoder.DecodeAsync(this.eventMock.Object).ConfigureAwait(false);

            Assert.IsNotNull(eventInfo);
            Assert.IsTrue(eventInfo.Count > 0);

            this.ethereumMock.Setup(e => e.GetEventsAsync<NodeConnectionEvent>(BlockNumber, TransactionHash)).ReturnsAsync(new List<NodeConnectionEvent>());
            eventInfo = await this.decoder.DecodeAsync(this.eventMock.Object).ConfigureAwait(false);

            Assert.IsNotNull(eventInfo);
            Assert.IsTrue(eventInfo.Count == 0);

            this.ethereumMock.Verify(e => e.GetEventsAsync<NodeConnectionEvent>(BlockNumber, TransactionHash), Times.Exactly(2));
        }
    }
}
