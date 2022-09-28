// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipErrorInventory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.OwnershipRules.Response
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using Ecp.True.Core;
    using Newtonsoft.Json;

    /// <summary>
    /// The Inventory Error class.
    /// </summary>
    [DisplayName("InventariosErrores")]
    public class OwnershipErrorInventory
    {
        /// <summary>
        /// Gets or sets the response inventory id.
        /// </summary>
        /// <value>
        /// Return the response inventory id.
        /// </value>
        [JsonProperty("IdInventario")]
        public string ResponseInventoryId { get; set; }

        /// <summary>
        /// Gets the inventory id.
        /// </summary>
        /// <value>
        /// Return the inventory id.
        /// </value>
        [JsonIgnore]
        public int InventoryId => Convert.ToInt32(this.ResponseInventoryId, CultureInfo.InvariantCulture);

        /// <summary>
        /// Gets or sets the response node id.
        /// </summary>
        /// <value>
        /// Return the response node id.
        /// </value>
        [JsonProperty("IdNodo")]
        public string ResponseNodeId { get; set; }

        /// <summary>
        /// Gets the node id.
        /// </summary>
        /// <value>
        /// Return the node id.
        /// </value>
        [JsonIgnore]
        public int NodeId => Convert.ToInt32(this.ResponseNodeId, CultureInfo.InvariantCulture);

        /// <summary>
        /// Gets or sets the ticket id.
        /// </summary>
        /// <value>
        /// Return the ticket id.
        /// </value>
        [JsonProperty("IdTiquete")]
        public string Ticket { get; set; }

        /// <summary>
        /// Gets or sets the error description.
        /// </summary>
        /// <value>
        /// Return the error description.
        /// </value>
        [JsonProperty("DescriptionError")]
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Gets the execution date.
        /// </summary>
        /// <value>
        /// Return the execution date.
        /// </value>
        [JsonIgnore]
        public DateTime ExecutionDate => DateTime.UtcNow.ToTrue();
    }
}