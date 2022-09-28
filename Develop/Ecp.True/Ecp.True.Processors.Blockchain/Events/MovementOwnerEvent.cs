// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementOwnerEvent.cs" company="Microsoft">
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
    /// The movement owner event.
    /// </summary>
    /// <seealso cref="Ecp.True.Proxies.Azure.Interfaces.IEvent" />
    [Event("TrueMovementOwnerLog")]
    public class MovementOwnerEvent : OwnerEvent
    {
        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        [Parameter("bytes32", "MovementId", 2, true)]
        [JsonIgnore]
        public byte[] MovementId { get; set; }

        /// <summary>
        /// Gets the ownership.
        /// </summary>
        /// <value>
        /// The ownership.
        /// </value>
        public decimal Ownership => this.Value.ToDecimal();

        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        /// <value>
        /// The type of the event.
        /// </value>
        public string EventType => this.ActionType.ToEventType();
    }
}
