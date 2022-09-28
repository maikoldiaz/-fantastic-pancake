// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PreviousInventoryOperationalData.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Query
{
    using System.ComponentModel;
    using Newtonsoft.Json;

    /// <summary>
    /// The Previous Inventory Operational Data.
    /// </summary>
    [DisplayName("inventariosIniciales")]
    public class PreviousInventoryOperationalData : QueryEntity
    {
        /// <summary>
        /// Gets or sets the ownership volume.
        /// </summary>
        /// <value>
        /// The ownership volume.
        /// </value>
        [JsonProperty("volumenPropiedad")]
        public decimal? OwnershipVolume { get; set; }

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
        /// Gets or sets a value indicating whether this instance is ownership calculated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is ownership calculated; otherwise, <c>false</c>.
        /// </value>
        public bool IsOwnershipCalculated { get; set; }

        /// <summary>
        /// Gets or sets the ownership percentage.
        /// </summary>
        /// <value>
        /// The ownership percentage.
        /// </value>
        public decimal? OwnershipPercentage { get; set; }

        /// <summary>
        /// Gets or sets the net standard volume.
        /// </summary>
        /// <value>
        /// The net standard volume.
        /// </value>
        public decimal? NetStandardVolume { get; set; }
    }
}