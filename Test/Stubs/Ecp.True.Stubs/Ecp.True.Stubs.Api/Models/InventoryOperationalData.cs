// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryOperationalData.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Stubs.Api.Models
{
    using System;
    using System.ComponentModel;
    using Newtonsoft.Json;

    /// <summary>
    /// Inventory Operational Data.
    /// </summary>
    [DisplayName("Inventarios")]
    public class InventoryOperationalData
    {
        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        [JsonProperty("tiquete")]
        public int Ticket { get; set; }

        /// <summary>
        /// Gets or sets the inventory identifier.
        /// </summary>
        /// <value>
        /// The inventory identifier.
        /// </value>
        [JsonProperty("idInventario")]
        public int InventoryId { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonProperty("idPropietario")]
        public int? OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        [JsonProperty("idNodo")]
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [JsonProperty("idProducto")]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the net volume.
        /// </summary>
        /// <value>
        /// The net volume.
        /// </value>
        [JsonProperty("volumenNeto")]
        public decimal? NetVolume { get; set; }

        /// <summary>
        /// Gets or sets the ownership value.
        /// </summary>
        /// <value>
        /// The ownership value.
        /// </value>
        [JsonProperty("valorPropiedad")]
        public decimal? OwnershipValue { get; set; }

        /// <summary>
        /// Gets or sets the ownership unit.
        /// </summary>
        /// <value>
        /// The ownership unit.
        /// </value>
        [JsonProperty("unidadPropiedad")]
        public string OwnershipUnit { get; set; }

        /// <summary>
        /// Gets or sets the operational date.
        /// </summary>
        /// <value>
        /// The operational date.
        /// </value>
        [JsonIgnore]
        public DateTime OperationalDate { get; set; }

        /// <summary>
        /// Gets or sets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        [JsonIgnore]
        public string Node { get; set; }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        [JsonIgnore]
        public string Product { get; set; }

        /// <summary>
        /// Gets or sets the name of the tank.
        /// </summary>
        /// <value>
        /// The name of the tank.
        /// </value>
        [JsonIgnore]
        public string Tank { get; set; }

        /// <summary>
        /// Gets or sets the volume unit.
        /// </summary>
        /// <value>
        /// The volume unit.
        /// </value>
        [JsonIgnore]
        public string VolumeUnit { get; set; }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>
        /// The owner.
        /// </value>
        [JsonIgnore]
        public string Owner { get; set; }
    }
}
