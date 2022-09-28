// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransactionEvent.cs" company="Microsoft">
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
    /// The transaction event.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Blockchain.Events.Event" />
    public class TransactionEvent : Event
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionEvent"/> class.
        /// </summary>
        protected TransactionEvent()
        {
        }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [Parameter("uint8", "Version", 4, false)]
        [JsonIgnore]
        public override int Version { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [Parameter("int256", "Volume", 5, false)]
        [JsonIgnore]
        public long Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        [Parameter("uint8", "ActionType", 6, false)]
        [JsonIgnore]
        public int ActionType { get; set; }

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        [Parameter("string", "Unit", 7, false)]
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        [Parameter("string", "Metadata", 8, false)]
        [JsonIgnore]
        public string Metadata { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>
        /// The timestamp.
        /// </value>
        [Parameter("uint", "Timestamp", 9, false)]
        [JsonIgnore]
        public int Timestamp { get; set; }
    }
}
