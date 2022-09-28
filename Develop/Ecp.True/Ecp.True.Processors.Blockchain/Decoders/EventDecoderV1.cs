// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventDecoderV1.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Processors.Blockchain.Interfaces;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The version 1 event decoder.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Blockchain.Interfaces.IEventDecoder" />
    public class EventDecoderV1 : IEventDecoder
    {
        /// <summary>
        /// The type decoders.
        /// </summary>
        private readonly IDictionary<int, Func<IBlockchainEventV1, JObject>> typeDecoders;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventDecoderV1"/> class.
        /// </summary>
        public EventDecoderV1()
        {
            this.typeDecoders = new Dictionary<int, Func<IBlockchainEventV1, JObject>>();
            this.Initialize();
        }

        /// <inheritdoc/>
        public int Version => 1;

        /// <inheritdoc/>
        public Task<JObject> DecodeAsync(IBlockchainEvent blockchainEvent)
        {
            ArgumentValidators.ThrowIfNull(blockchainEvent, nameof(blockchainEvent));

            var eventInfo = (IBlockchainEventV1)blockchainEvent;
            var decoder = this.typeDecoders[blockchainEvent.Type];
            return Task.FromResult(decoder(eventInfo));
        }

        private static JObject DecodeMovement(IBlockchainEventV1 eventInfo)
        {
            var entity = new JObject();
            entity.Add("TransactionId", eventInfo.Id);

            var values = eventInfo.Data.Split(',');
            entity.Add(new JProperty("BackupMovementId", values[0]));
            entity.Add(new JProperty("CreatedDate", values[1]));
            entity.Add(new JProperty("EndTime", values[2]));
            entity.Add(new JProperty("StartTime", values[3]));
            entity.Add(new JProperty("EventType", values[4]));
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

            return entity;
        }

        private static JObject DecodeInventory(IBlockchainEventV1 eventInfo)
        {
            var entity = new JObject();
            entity.Add("TransactionId", eventInfo.Id);

            var values = eventInfo.Data.Split(',');
            entity.Add(new JProperty("BatchId", values[0]));
            entity.Add(new JProperty("CreatedDate", values[1]));
            entity.Add(new JProperty("EventType", values[2]));
            entity.Add(new JProperty("ProductType", values[3]));
            entity.Add(new JProperty("ScenarioId", values[4]));
            entity.Add(new JProperty("SegmentId", values[5]));
            entity.Add(new JProperty("SourceSystemId", values[6]));
            entity.Add(new JProperty("TankName", values[7]));
            entity.Add(new JProperty("UncertaintyPercentage", values[8]));
            entity.Add(new JProperty("Version", values[9]));
            entity.Add(new JProperty("NodeId", values[10]));
            entity.Add(new JProperty("ProductId", values[11]));

            return entity;
        }

        private static JObject DecodeOwner(IBlockchainEventV1 eventInfo)
        {
            var entity = new JObject();
            entity.Add("Id", eventInfo.Id);
            entity.Add("OwnerId", eventInfo.Data);

            return entity;
        }

        private static JObject DecodeOwnership(IBlockchainEventV1 eventInfo)
        {
            var entity = new JObject();
            entity.Add("OwnershipId", eventInfo.Id);
            entity.Add("OwnerId", eventInfo.Data.Split('-')[0]);
            entity.Add("TransactionId", eventInfo.Data.Split('-')[1]);

            return entity;
        }

        private static JObject DecodeUnbalance(IBlockchainEventV1 eventInfo)
        {
            var entity = new JObject();
            entity.Add("UnbalanceId", eventInfo.Id);

            var values = eventInfo.Data.Split(',');
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

        private static JObject DecodeNode(IBlockchainEventV1 eventInfo)
        {
            var entity = new JObject();
            entity.Add("NodeId", eventInfo.Id);
            entity.Add("Name", eventInfo.Data);

            return entity;
        }

        private static JObject DecodeNodeConnection(IBlockchainEventV1 eventInfo)
        {
            var entity = new JObject();
            entity.Add("NodeConnectionId", eventInfo.Id);

            return entity;
        }

        private void Initialize()
        {
            this.typeDecoders.Add(1, DecodeMovement);
            this.typeDecoders.Add(2, DecodeOwner);
            this.typeDecoders.Add(3, DecodeInventory);
            this.typeDecoders.Add(4, DecodeOwner);
            this.typeDecoders.Add(5, DecodeOwnership);
            this.typeDecoders.Add(6, DecodeOwnership);
            this.typeDecoders.Add(7, DecodeUnbalance);
            this.typeDecoders.Add(8, DecodeNode);
            this.typeDecoders.Add(9, DecodeNodeConnection);
        }
    }
}
