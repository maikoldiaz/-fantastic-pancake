// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryProductEvent.cs" company="Microsoft">
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
    /// The movement event.
    /// </summary>
    /// <seealso cref="Ecp.True.Proxies.Azure.Interfaces.IEvent" />
    [Event("TrueInventoryProductLog")]
    public class InventoryProductEvent : TransactionEvent
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
        /// Gets or sets the operational date.
        /// </summary>
        /// <value>
        /// The operational date.
        /// </value>
        [Parameter("int64", "InventoryDate", 3, true)]
        [JsonIgnore]
        public long Date { get; set; }

        /// <summary>
        /// Gets the ownership.
        /// </summary>
        /// <value>
        /// The ownership.
        /// </value>
        public string InventoryDate => this.Date.ToTrueDateString();

        /// <summary>
        /// Gets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public decimal Volume => this.Value.ToDecimal();

        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        /// <value>
        /// The type of the event.
        /// </value>
        public string EventType => this.ActionType.ToEventType();
    }
}
