// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaPendingOfficialInventory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.OwnershipRules.OfficialDeltaDataRequest
{
    using System;
    using System.ComponentModel;
    using Newtonsoft.Json;

    /// <summary>
    /// The OfficialDeltaPendingOfficialInventory.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Query.QueryEntity" />
    [DisplayName("balanceOficialInventarios")]
    public class OfficialDeltaPendingOfficialInventory
    {
        /// <summary>
        /// Gets or sets the inventory product identifier.
        /// </summary>
        /// <value>
        /// The inventory product identifier.
        /// </value>
        [JsonProperty("idInventarioTRUE")]
        public string InventoryProductID { get; set; }

        /// <summary>
        /// Gets or sets the InventoryProductUniqueId.
        /// </summary>
        /// <value>
        /// TheInventoryProductUniqueId.
        /// </value>
        [JsonProperty("idInventarioPropietario")]
        public string InventoryProductOwnerId { get; set; }

        /// <summary>
        /// Gets or sets the InventoryDate.
        /// </summary>
        /// <value>
        /// InventoryDate.
        /// </value>
        [JsonProperty("fecha")]
        public DateTime InventoryDate { get; set; }

        /// <summary>
        /// Gets or sets the NodeId.
        /// </summary>
        /// <value>
        /// NodeId.
        /// </value>
        [JsonProperty("idNodo")]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the ProductID.
        /// </summary>
        /// <value>
        /// ProductID.
        /// </value>
        [JsonProperty("idProducto")]
        public string ProductID { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonProperty("idPropietario")]
        public string OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the ownership Value.
        /// </summary>
        /// <value>
        /// ownership value.
        /// </value>
        [JsonProperty("valorPropiedad")]
        public decimal OwnerShipValue { get; set; }
    }
}
