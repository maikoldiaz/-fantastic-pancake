// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainProcessorTests.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processors.Blockchain;
    using Ecp.True.Processors.Blockchain.Entities;
    using Ecp.True.Processors.Blockchain.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Nethereum.RPC.Eth.DTOs;

    /// <summary>
    /// The blockchain processor tests.
    /// </summary>
    [TestClass]
    public class BlockchainProcessorTests
    {
        /// <summary>
        /// The factory mock.
        /// </summary>
        private Mock<IAzureClientFactory> factoryMock;

        /// <summary>
        /// The ethereum mock.
        /// </summary>
        private Mock<IEthereumClient> ethereumMock;

        /// <summary>
        /// The decoder factory mock.
        /// </summary>
        private Mock<IEventDecoderFactory> decoderFactoryMock;

        /// <summary>
        /// The processor.
        /// </summary>
        private BlockchainProcessor processor;

        [TestInitialize]
        public void Initialize()
        {
            this.factoryMock = new Mock<IAzureClientFactory>();
            this.ethereumMock = new Mock<IEthereumClient>();
            this.decoderFactoryMock = new Mock<IEventDecoderFactory>();

            this.ethereumMock.Setup(m => m.GetBlockAsync(10000)).ReturnsAsync(new BlockWithTransactionHashes { BlockHash = "Hash" });

            this.ethereumMock.Setup(m => m.GetEventsAsync<BlockchainEventV2>(It.IsAny<ulong>(), It.IsAny<ulong>())).ReturnsAsync(new List<BlockchainEventV2>());
            this.ethereumMock.Setup(m => m.GetEventsAsync<BlockchainEventV1>(It.IsAny<ulong>(), It.IsAny<ulong>())).ReturnsAsync(new List<BlockchainEventV1>());
            this.factoryMock.Setup(f => f.EthereumClient).Returns(this.ethereumMock.Object);

            this.processor = new BlockchainProcessor(this.factoryMock.Object, this.decoderFactoryMock.Object);
        }

        /// <summary>
        /// Determines whether [has block should return block when invoked].
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task HasBlock_ShouldReturnBlock_WhenInvokedAsync()
        {
            var result = await this.processor.HasBlockAsync(10000).ConfigureAwait(false);
            Assert.IsTrue(result);

            this.ethereumMock.Verify(m => m.GetBlockAsync(10000), Times.Once);
        }

        /// <summary>
        /// Determines whether [has block should return block when invoked].
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetPagedEvents_ShouldGetEventsFromLastBlock_WhenLastHeadIsNotProvidedAsync()
        {
            ulong tail = 5000;
            ulong head = 1;
            this.ethereumMock.Setup(m => m.GetLatestBlockNumberAsync()).Returns(Task.FromResult(tail));

            var result = await this.processor.GetPagedEventsAsync(100, null).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(100, result.PageSize);
            Assert.AreEqual(tail, result.TailBlock);
            Assert.IsTrue(result.HeadBlock == default);

            this.ethereumMock.Verify(m => m.GetLatestBlockNumberAsync(), Times.Once);
            this.ethereumMock.Verify(m => m.GetEventsAsync<BlockchainEventV2>(head, tail), Times.Once);
            this.ethereumMock.Verify(m => m.GetEventsAsync<BlockchainEventV1>(head, tail), Times.Once);
        }

        /// <summary>
        /// Determines whether [has block should return block when invoked].
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetPagedEvents_ShouldGetEventsFromLastHead_WhenLastHeadIsProvidedAsync()
        {
            ulong eventHead = 1000;
            ulong tailBlock = 2000;
            this.ethereumMock.Setup(m => m.GetLatestBlockNumberAsync()).Returns(Task.FromResult(eventHead));
            this.ethereumMock.Setup(m => m.GetEventsAsync<BlockchainEventV2>(1, 2000)).ReturnsAsync(new List<BlockchainEventV2> { new BlockchainEventV2 { BlockNumber = eventHead } });
            this.ethereumMock.Setup(m => m.GetEventsAsync<BlockchainEventV1>(1, 2000)).ReturnsAsync(new List<BlockchainEventV1>());
            this.ethereumMock.Setup(m => m.GetEventsAsync<BlockchainEventV2>(1, 1000)).ReturnsAsync(new List<BlockchainEventV2>());
            this.ethereumMock.Setup(m => m.GetEventsAsync<BlockchainEventV1>(1, 1000)).ReturnsAsync(new List<BlockchainEventV1>());

            var result = await this.processor.GetPagedEventsAsync(100, tailBlock).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(100, result.PageSize);
            Assert.AreEqual(tailBlock, result.TailBlock);
            Assert.AreEqual(eventHead, result.HeadBlock);

            this.ethereumMock.Verify(m => m.GetLatestBlockNumberAsync(), Times.Never);

            this.ethereumMock.Verify(m => m.GetEventsAsync<BlockchainEventV2>(1, 2000), Times.Once);
            this.ethereumMock.Verify(m => m.GetEventsAsync<BlockchainEventV1>(1, 2000), Times.Once);
        }

        /// <summary>
        /// Validates the block range asynchronous should return true if block exists when head and tail is provided asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ValidateBlockRangeAsync_ShouldReturnTrueIfBlockExists_WhenHeadAndTailIsProvidedAsync()
        {
            var block = new BlockWithTransactionHashes() { BlockHash = "hash" };
            this.ethereumMock.Setup(m => m.GetBlockAsync(It.IsAny<ulong>())).Returns(Task.FromResult(block));

            var result = await this.processor.ValidateBlockRangeAsync(new BlockRangeRequest()).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.ethereumMock.Verify(m => m.GetBlockAsync(It.IsAny<ulong>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Validates the block range asynchronous should throw exception when head and tail is provided asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task ValidateBlockRangeAsync_ShouldThrowException_WhenHeadAndTailIsProvidedAsync()
        {
            var block = new BlockWithTransactionHashes() { };
            this.ethereumMock.Setup(m => m.GetBlockAsync(It.IsAny<ulong>())).Returns(Task.FromResult(block));

            var result = await this.processor.ValidateBlockRangeAsync(new BlockRangeRequest()).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.ethereumMock.Verify(m => m.GetBlockAsync(It.IsAny<ulong>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Gets the events in range asynchronous should return events when head and tail is provided asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetEventsInRangeAsync_ShouldReturnEvents_WhenHeadAndTailIsProvidedAsync()
        {
            var request = new BlockRangeRequest() { HeadBlock = 1, Event = 3 };
            this.ethereumMock.Setup(m => m.GetEventsAsync<BlockchainEventV1>(It.IsAny<ulong>(), It.IsAny<ulong>())).ReturnsAsync(new List<BlockchainEventV1>() { new BlockchainEventV1() { Type = 3 } });
            this.ethereumMock.Setup(m => m.GetEventsAsync<BlockchainEventV2>(It.IsAny<ulong>(), It.IsAny<ulong>())).ReturnsAsync(new List<BlockchainEventV2>());

            var result = await this.processor.GetEventsInRangeAsync(request).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.ethereumMock.Verify(m => m.GetEventsAsync<BlockchainEventV1>(It.IsAny<ulong>(), It.IsAny<ulong>()), Times.Once);
        }
    }
}
