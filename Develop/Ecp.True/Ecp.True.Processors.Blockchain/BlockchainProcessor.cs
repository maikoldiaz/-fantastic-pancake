// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Blockchain
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processor.Blockchain.Events;
    using Ecp.True.Processors.Blockchain.Entities;
    using Ecp.True.Processors.Blockchain.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Nethereum.Hex.HexTypes;
    using Nethereum.RPC.Eth.DTOs;

    /// <summary>
    /// The blockchain processor.
    /// </summary>
    public class BlockchainProcessor : IBlockchainProcessor
    {
        /// <summary>
        /// The factory.
        /// </summary>
        private readonly IAzureClientFactory factory;

        /// <summary>
        /// The decoder factory.
        /// </summary>
        private readonly IEventDecoderFactory decoderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockchainProcessor" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="decoderFactory">The decoder factory.</param>
        public BlockchainProcessor(IAzureClientFactory factory, IEventDecoderFactory decoderFactory)
        {
            this.factory = factory;
            this.decoderFactory = decoderFactory;
        }

        /// <inheritdoc/>
        public async Task<EventsPage> GetPagedEventsAsync(int pageSize, ulong? lastHead)
        {
            var tailBlock = lastHead ?? await this.factory.EthereumClient.GetLatestBlockNumberAsync().ConfigureAwait(false);
            var eventPage = new EventsPage(pageSize, tailBlock);
            const ulong blocksToNavigate = 5000;

            var fetchEvents = true;
            while (fetchEvents)
            {
                ulong headBlock = tailBlock > blocksToNavigate ? tailBlock - blocksToNavigate : 1;
                fetchEvents = headBlock < tailBlock;

                if (fetchEvents)
                {
                    var events = await this.GetAllEventsAsync(headBlock, tailBlock).ConfigureAwait(false);
                    var ordered = events.OrderByDescending(a => a.BlockNumber).ToList();

                    var index = 0;
                    while (!eventPage.IsPageFull && ordered.Count > index)
                    {
                        var evt = ordered.ElementAt(index++);
                        eventPage.AddEvent(evt);
                    }

                    fetchEvents = !eventPage.IsPageFull;
                    tailBlock = headBlock;
                }
            }

            return eventPage;
        }

        /// <inheritdoc/>
        public async Task<EventsPage> GetEventsInRangeAsync(BlockRangeRequest request)
        {
            ArgumentValidators.ThrowIfNull(request, nameof(request));
            var events = await this.GetAllEventsAsync(request.HeadBlock, request.TailBlock).ConfigureAwait(false);
            if (request.Event != 0)
            {
                events = events.Where(x => x.Type == request.Event).ToList();
            }

            var eventPage = new EventsPage(events.Count, request.HeadBlock, request.TailBlock, events.OrderByDescending(a => a.BlockNumber));
            return eventPage;
        }

        /// <inheritdoc/>
        public async Task<BlockTransactionResponse> GetTransactionDetailsAsync(BlockTransactionRequest request)
        {
            ArgumentValidators.ThrowIfNull(request, nameof(request));

            var block = await this.TryGetBlockAsync(request).ConfigureAwait(false);
            var response = new BlockTransactionResponse(request)
            {
                GasUsed = block.GasUsed.ToUlong(),
                GasLimit = block.GasLimit.ToUlong(),
                BlockHash = block.BlockHash,
            };

            var blockchainEvent = await this.GetBlockchainEventAsync(request.BlockNumber, request.TransactionHash).ConfigureAwait(false);
            if (blockchainEvent == null)
            {
                return response;
            }

            response.TransactionTime = blockchainEvent.Timestamp.ToDateTimeFromEpoch();
            response.Type = blockchainEvent.Type;
            response.Id = blockchainEvent.Identifier;
            response.Address = blockchainEvent.Address;

            var decoder = this.decoderFactory.GetDecoder(blockchainEvent.Version);
            var content = await decoder.DecodeAsync(blockchainEvent).ConfigureAwait(false);

            response.Content = content;
            return response;
        }

        /// <inheritdoc/>
        public async Task<bool> HasBlockAsync(ulong blockNumber)
        {
            var block = await this.factory.EthereumClient.GetBlockAsync(blockNumber).ConfigureAwait(false);
            return !string.IsNullOrWhiteSpace(block.BlockHash);
        }

        /// <inheritdoc/>
        public async Task<Block> TryGetBlockAsync(BlockTransactionRequest request)
        {
            ArgumentValidators.ThrowIfNull(request, nameof(request));
            var block = await this.factory.EthereumClient.GetBlockAsync(request.BlockNumber).ConfigureAwait(false);

            if (block == null || !block.TransactionHashes.Contains(request.TransactionHash))
            {
                throw new KeyNotFoundException(True.Entities.Constants.InvalidTransactionHashSupplied);
            }

            if (string.IsNullOrWhiteSpace(block.BlockHash))
            {
                throw new KeyNotFoundException(True.Entities.Constants.InvalidBlockNumberSupplied);
            }

            return block;
        }

        /// <inheritdoc/>
        public async Task<bool> ValidateBlockRangeAsync(BlockRangeRequest request)
        {
            ArgumentValidators.ThrowIfNull(request, nameof(request));
            var headBlock = await this.factory.EthereumClient.GetBlockAsync(request.HeadBlock).ConfigureAwait(false);
            var tailBlock = await this.factory.EthereumClient.GetBlockAsync(request.TailBlock).ConfigureAwait(false);

            if (headBlock == null || tailBlock == null || string.IsNullOrWhiteSpace(headBlock.BlockHash) || string.IsNullOrWhiteSpace(tailBlock.BlockHash))
            {
                throw new KeyNotFoundException(True.Entities.Constants.InvalidBlockNumberSupplied);
            }

            return true;
        }

        private async Task<IBlockchainEvent> GetBlockchainEventAsync(ulong blockNumber, string transactionHash)
        {
            var eventsVersion2 = await this.factory.EthereumClient.GetEventsAsync<BlockchainEventV2>(blockNumber, transactionHash).ConfigureAwait(false);
            if (eventsVersion2.Any())
            {
                return eventsVersion2.FirstOrDefault();
            }

            var eventsVersion1 = await this.factory.EthereumClient.GetEventsAsync<BlockchainEventV1>(blockNumber, transactionHash).ConfigureAwait(false);
            if (eventsVersion1.Any())
            {
                return eventsVersion1.FirstOrDefault();
            }

            return null;
        }

        private async Task<IList<IBlockchainEvent>> GetAllEventsAsync(ulong head, ulong tail)
        {
            var eventsVersion2 = await this.factory.EthereumClient.GetEventsAsync<BlockchainEventV2>(head, tail).ConfigureAwait(false);
            var eventsVersion1 = await this.factory.EthereumClient.GetEventsAsync<BlockchainEventV1>(head, tail).ConfigureAwait(false);

            var allEvents = new List<IBlockchainEvent>();
            allEvents.AddRange(eventsVersion2);
            allEvents.AddRange(eventsVersion1);

            return allEvents;
        }
    }
}