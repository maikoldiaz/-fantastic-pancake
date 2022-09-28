// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnbalanceEvent.cs" company="Microsoft">
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
    using Nethereum.ABI.FunctionEncoding.Attributes;
    using Newtonsoft.Json;

    /// <summary>
    /// The unbalance event.
    /// </summary>
    /// <seealso cref="Ecp.True.Proxies.Azure.Interfaces.IEvent" />
    [Event("TrueUnbalanceLog")]
    public class UnbalanceEvent : Event
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [Parameter("int64", "TicketId", 2, true)]
        public int TicketId { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [Parameter("uint8", "Version", 3, false)]
        [JsonIgnore]
        public override int Version { get; set; }

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        [Parameter("string", "Values", 4, false)]
        [JsonIgnore]
        public string Values { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>
        /// The timestamp.
        /// </value>
        [Parameter("uint", "Timestamp", 5, false)]
        [JsonIgnore]
        public int Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        [Parameter("string", "Metadata", 6, false)]
        [JsonIgnore]
        public string Metadata { get; set; }

        /// <summary>
        /// Gets the calculation date.
        /// </summary>
        /// <value>
        /// The calculation date.
        /// </value>
        public string CalculationDate => !string.IsNullOrWhiteSpace(this.Metadata) ? this.Metadata.Split('-')[0] : string.Empty;

        /// <summary>
        /// Gets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        public string NodeId => !string.IsNullOrWhiteSpace(this.Metadata) ? this.Metadata.Split('-')[1] : string.Empty;

        /// <summary>
        /// Gets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public string ProductId => !string.IsNullOrWhiteSpace(this.Metadata) ? this.Metadata.Split('-')[2] : string.Empty;
    }
}
