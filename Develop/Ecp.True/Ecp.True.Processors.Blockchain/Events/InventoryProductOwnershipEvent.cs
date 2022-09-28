// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryProductOwnershipEvent.cs" company="Microsoft">
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
    /// The inventory ownership event.
    /// </summary>
    /// <seealso cref="Ecp.True.Proxies.Azure.Interfaces.IEvent" />
    [Event("TrueInventoryProductOwnershipLog")]
    public class InventoryProductOwnershipEvent : OwnershipEvent
    {
        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        [Parameter("bytes32", "InventoryProductId", 2, true)]
        [JsonIgnore]
        public byte[] InventoryProductId { get; set; }

        /// <summary>
        /// Gets the ownership volume.
        /// </summary>
        /// <value>
        /// The ownership volume.
        /// </value>
        public decimal OwnershipVolume => this.Volume.ToDecimal();

        /// <summary>
        /// Gets the ownership percentage.
        /// </summary>
        /// <value>
        /// The ownership percentage.
        /// </value>
        public decimal OwnershipPercentage => this.Percentage.ToDecimal();

        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        /// <value>
        /// The type of the event.
        /// </value>
        public string EventType => this.ActionType.ToEventType();

        /// <summary>
        /// Gets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public string OwnerId => !string.IsNullOrWhiteSpace(this.Metadata) ? this.Metadata.Split('|')[0] : string.Empty;

        /// <summary>
        /// Gets the rule.
        /// </summary>
        /// <value>
        /// The rule.
        /// </value>
        public string Rule => !string.IsNullOrWhiteSpace(this.Metadata) ? this.Metadata.Split('|')[1] : string.Empty;

        /// <summary>
        /// Gets the rule version.
        /// </summary>
        /// <value>
        /// The rule version.
        /// </value>
        public string RuleVersion => !string.IsNullOrWhiteSpace(this.Metadata) ? this.Metadata.Split('|')[2] : string.Empty;
    }
}
