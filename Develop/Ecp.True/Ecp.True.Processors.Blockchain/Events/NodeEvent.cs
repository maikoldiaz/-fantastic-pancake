// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeEvent.cs" company="Microsoft">
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
    /// The node event.
    /// </summary>
    /// <seealso cref="Ecp.True.Proxies.Azure.Interfaces.IEvent" />
    [Event("TrueNodeLog")]
    public class NodeEvent : Event
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [Parameter("uint8", "Version", 2, false)]
        [JsonIgnore]
        public override int Version { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Parameter("string", "Name", 3, false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        [Parameter("uint8", "State", 4, false)]
        [JsonIgnore]
        public int State { get; set; }

        /// <summary>
        /// Gets or sets the last update date.
        /// </summary>
        /// <value>
        /// The last update date.
        /// </value>
        [Parameter("int64", "LastUpdateDate", 5, false)]
        [JsonIgnore]
        public long Date { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        [Parameter("bool", "IsActive", 6, false)]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>
        /// The timestamp.
        /// </value>
        [Parameter("uint", "Timestamp", 7, false)]
        [JsonIgnore]
        public int Timestamp { get; set; }

        /// <summary>
        /// Gets the last update date.
        /// </summary>
        /// <value>
        /// The last update date.
        /// </value>
        public string LastUpdateDate => this.Date.ToTrueDateString();

        /// <summary>
        /// Gets the state of the node.
        /// </summary>
        /// <value>
        /// The state of the node.
        /// </value>
        public string NodeState => this.State.ToNodeState();
    }
}
