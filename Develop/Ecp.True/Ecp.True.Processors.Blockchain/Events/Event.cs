// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Event.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Blockchain.Events
{
    using Ecp.True.Proxies.Azure.Interfaces;
    using Nethereum.ABI.FunctionEncoding.Attributes;
    using Newtonsoft.Json;

    /// <summary>
    /// The event base type.
    /// </summary>
    /// <seealso cref="Ecp.True.Proxies.Azure.Interfaces.IEvent" />
    public abstract class Event : IEvent
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public abstract int Version { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Parameter("bytes32", "Id", 1, true)]
        [JsonIgnore]
        public byte[] Id { get; set; }

        /// <summary>
        /// Gets or sets the block number.
        /// </summary>
        /// <value>
        /// The block number.
        /// </value>
        [JsonIgnore]
        public ulong BlockNumber { get; set; }

        /// <summary>
        /// Gets or sets the transaction hash.
        /// </summary>
        /// <value>
        /// The transaction hash.
        /// </value>
        [JsonIgnore]
        public string TransactionHash { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        [JsonIgnore]
        public string Address { get; set; }
    }
}
