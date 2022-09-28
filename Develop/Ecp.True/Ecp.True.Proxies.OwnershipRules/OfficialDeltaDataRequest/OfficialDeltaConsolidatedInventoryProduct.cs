// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaConsolidatedInventoryProduct.cs" company="Microsoft">
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
    using System.ComponentModel;
    using Newtonsoft.Json;

    /// <summary>
    /// The OfficialDeltaConsolidatedInventoryProduct.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Query.QueryEntity" />
    [DisplayName("consolidadosInventarios")]
    public class OfficialDeltaConsolidatedInventoryProduct
    {
        /// <summary>
        /// Gets or sets the consolidated movement identifier.
        /// </summary>
        /// <value>
        /// The consolidated movement identifier.
        /// </value>
        [JsonProperty("idInventarioTRUE")]
        public string ConsolidatedInventoryProductId { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonProperty("idInventarioPropietario")]
        public string InventoryProductOwnerId { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        [JsonProperty("fecha")]
        public string InventoryDate { get; set; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        [JsonProperty("idNodo")]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [JsonProperty("idProducto")]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonProperty("idPropietario")]
        public string OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the ownership volume.
        /// </summary>
        /// <value>
        /// The ownership volume.
        /// </value>
        [JsonProperty("valorPropiedad")]
        public decimal OwnershipVolume { get; set; }
    }
}
