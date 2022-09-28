// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainEventV1.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Blockchain.Entities
{
    using System;
    using Ecp.True.Core;
    using Ecp.True.Processors.Blockchain.Interfaces;
    using Nethereum.ABI.FunctionEncoding.Attributes;

    /// <summary>
    /// The True Log Event.
    /// </summary>
    [Event(Constants.BlockchainEventName)]
    public class BlockchainEventV1 : IBlockchainEventV1
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [Parameter("int64", "Type", 1, true)]
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [Parameter("int64", "Version", 2, true)]
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        /// <value>
        /// The type of the event.
        /// </value>
        [Parameter("string", "Id", 3, false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [Parameter("string", "Data", 4, false)]
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>
        /// The timestamp.
        /// </value>
        [Parameter("uint", "Timestamp", 5, false)]
        public int Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the block number.
        /// </summary>
        /// <value>
        /// The block number.
        /// </value>
        public ulong BlockNumber { get; set; }

        /// <summary>
        /// Gets or sets the transaction hash.
        /// </summary>
        /// <value>
        /// The transaction hash.
        /// </value>
        public string TransactionHash { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public string Address { get; set; }

        /// <summary>
        /// Gets the transaction time.
        /// </summary>
        /// <value>
        /// The transaction time.
        /// </value>
        public DateTime? TransactionTime => this.Timestamp.ToDateTimeFromEpoch();

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Identifier => this.Id;
    }
}
