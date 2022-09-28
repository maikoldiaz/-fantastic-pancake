// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventDecoderV2.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Blockchain.Decoders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Processors.Blockchain.Events;
    using Ecp.True.Processors.Blockchain.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Azure.Interfaces;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The version 1 event decoder.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Blockchain.Interfaces.IEventDecoder" />
    public class EventDecoderV2 : IEventDecoder
    {
        /// <summary>
        /// The type decoders.
        /// </summary>
        private readonly IDictionary<int, Func<IBlockchainEventV2, Task<JObject>>> typeDecoders;

        /// <summary>
        /// The factory.
        /// </summary>
        private readonly IAzureClientFactory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventDecoderV2" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public EventDecoderV2(IAzureClientFactory factory)
        {
            this.factory = factory;
            this.typeDecoders = new Dictionary<int, Func<IBlockchainEventV2, Task<JObject>>>();

            this.Initialize();
        }

        /// <inheritdoc/>
        public int Version => 2;

        /// <inheritdoc/>
        public Task<JObject> DecodeAsync(IBlockchainEvent blockchainEvent)
        {
            ArgumentValidators.ThrowIfNull(blockchainEvent, nameof(blockchainEvent));

            var eventInfo = (IBlockchainEventV2)blockchainEvent;
            if (!this.typeDecoders.ContainsKey(blockchainEvent.Type))
            {
                return Task.FromResult(new JObject());
            }

            var decoder = this.typeDecoders[blockchainEvent.Type];
            return decoder(eventInfo);
        }

        private async Task<JObject> DecodeEventAsync<TEvent>(IBlockchainEventV2 eventInfo)
            where TEvent : IEvent, new()
        {
            var events = await this.factory.EthereumClient.GetEventsAsync<TEvent>(eventInfo.BlockNumber, eventInfo.TransactionHash).ConfigureAwait(false);
            return events.Any() ? JObject.FromObject(events.First()) : new JObject();
        }

        private async Task<JObject> DecodeMovementEventAsync(IBlockchainEventV2 eventInfo)
        {
            var events = await this.factory.EthereumClient.GetEventsAsync<MovementEvent>(eventInfo.BlockNumber, eventInfo.TransactionHash).ConfigureAwait(false);

            if (events.Any())
            {
                var evt = events.First();
                var entity = JObject.FromObject(evt);

                var values = evt.Metadata.Split(',');
                entity.Add(new JProperty("BackupMovementId", values[0]));
                entity.Add(new JProperty("CreatedDate", values[1]));
                entity.Add(new JProperty("EndTime", values[2]));
                entity.Add(new JProperty("StartTime", values[3]));
                entity.Add(new JProperty("GlobalMovementId", values[5]));
                entity.Add(new JProperty("IsOfficial", values[6]));
                entity.Add(new JProperty("MovementContractId", values[7]));
                entity.Add(new JProperty("MovementEventId", values[8]));
                entity.Add(new JProperty("ScenarioId", values[9]));
                entity.Add(new JProperty("SegmentId", values[10]));
                entity.Add(new JProperty("SourceSystemId", values[11]));
                entity.Add(new JProperty("UncertaintyPercentage", values[12]));
                entity.Add(new JProperty("Version", values[13]));
                entity.Add(new JProperty("SourceNodeId", values[14]));
                entity.Add(new JProperty("SourceProductId", values[15]));
                entity.Add(new JProperty("DestinationNodeId", values[16]));
                entity.Add(new JProperty("DestinationProductId", values[17]));

                entity.Add(new JProperty("MovementId", values.Length > 18 ? values[18] : string.Empty));
                entity.Add(new JProperty("MovementTypeId", values.Length > 19 ? values[19] : string.Empty));
                return entity;
            }

            return new JObject();
        }

        private async Task<JObject> DecodeInventoryProductEventAsync(IBlockchainEventV2 eventInfo)
        {
            var events = await this.factory.EthereumClient.GetEventsAsync<InventoryProductEvent>(eventInfo.BlockNumber, eventInfo.TransactionHash).ConfigureAwait(false);

            if (events.Any())
            {
                var evt = events.First();
                var entity = JObject.FromObject(evt);

                var values = evt.Metadata.Split(',');
                entity.Add(new JProperty("BatchId", values[0]));
                entity.Add(new JProperty("CreatedDate", values[1]));
                entity.Add(new JProperty("ProductType", values[3]));
                entity.Add(new JProperty("ScenarioId", values[4]));
                entity.Add(new JProperty("SegmentId", values[5]));
                entity.Add(new JProperty("SourceSystemId", values[6]));
                entity.Add(new JProperty("TankName", values[7]));
                entity.Add(new JProperty("UncertaintyPercentage", values[8]));
                entity.Add(new JProperty("Version", values[9]));
                entity.Add(new JProperty("NodeId", values[10]));
                entity.Add(new JProperty("ProductId", values[11]));

                entity.Add(new JProperty("InventoryId", values.Length > 12 ? values[12] : string.Empty));
                entity.Add(new JProperty("InventoryProductUniqueId", values.Length > 13 ? values[13] : string.Empty));
                return entity;
            }

            return new JObject();
        }

        private async Task<JObject> DecodeUnbalanceEventAsync(IBlockchainEventV2 eventInfo)
        {
            var events = await this.factory.EthereumClient.GetEventsAsync<UnbalanceEvent>(eventInfo.BlockNumber, eventInfo.TransactionHash).ConfigureAwait(false);

            if (events.Any())
            {
                var evt = events.First();
                var entity = JObject.FromObject(evt);

                var values = evt.Values.Split(',');
                entity.Add(new JProperty("InitialInventory", values[0].ToDecimal()));
                entity.Add(new JProperty("FinalInventory", values[1].ToDecimal()));
                entity.Add(new JProperty("Inputs", values[2].ToDecimal()));
                entity.Add(new JProperty("Outputs", values[3].ToDecimal()));
                entity.Add(new JProperty("IdentifiedLosses", values[4].ToDecimal()));
                entity.Add(new JProperty("Interface", values[5].ToDecimal()));
                entity.Add(new JProperty("Tolerance", values[6].ToDecimal()));
                entity.Add(new JProperty("UnidentifiedLosses", values[7].ToDecimal()));
                entity.Add(new JProperty("UnbalanceAmount", values[8].ToDecimal()));
                return entity;
            }

            return new JObject();
        }

        private void Initialize()
        {
            this.typeDecoders.Add(1, this.DecodeMovementEventAsync);
            this.typeDecoders.Add(2, async e => await this.DecodeEventAsync<MovementOwnerEvent>(e).ConfigureAwait(false));
            this.typeDecoders.Add(3, this.DecodeInventoryProductEventAsync);
            this.typeDecoders.Add(4, async e => await this.DecodeEventAsync<InventoryProductOwnerEvent>(e).ConfigureAwait(false));
            this.typeDecoders.Add(5, async e => await this.DecodeEventAsync<MovementOwnershipEvent>(e).ConfigureAwait(false));
            this.typeDecoders.Add(6, async e => await this.DecodeEventAsync<InventoryProductOwnershipEvent>(e).ConfigureAwait(false));
            this.typeDecoders.Add(7, this.DecodeUnbalanceEventAsync);
            this.typeDecoders.Add(8, async e => await this.DecodeEventAsync<NodeEvent>(e).ConfigureAwait(false));
            this.typeDecoders.Add(9, async e => await this.DecodeEventAsync<NodeConnectionEvent>(e).ConfigureAwait(false));
        }
    }
}
