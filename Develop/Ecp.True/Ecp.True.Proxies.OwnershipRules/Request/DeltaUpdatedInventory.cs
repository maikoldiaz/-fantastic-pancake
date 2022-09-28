// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaUpdatedInventory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.OwnershipRules.Request
{
    using System.ComponentModel;
    using Newtonsoft.Json;

    /// <summary>
    /// The DeltaUpdatedInventory.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Query.QueryEntity" />
    [DisplayName("inventarioActualizados")]
    public class DeltaUpdatedInventory
    {
        /// <summary>
        /// Gets or sets the InventoryProductUniqueId identifier.
        /// </summary>
        /// <value>
        /// The InventoryProductUniqueId identifier.
        /// </value>
        [JsonProperty("idInventario")]
        public string InventoryProductUniqueId { get; set; }

        /// <summary>
        /// Gets or sets the inventory product identifier.
        /// </summary>
        /// <value>
        /// The inventory transaction identifier.
        /// </value>
        [JsonProperty("idInventarioTRUE")]
        public int InventoryProductId { get; set; }

        /// <summary>
        /// Gets or sets the product volume.
        /// </summary>
        /// <value>
        /// The product volume.
        /// </value>
        [JsonProperty("valor")]
        public decimal ProductVolume { get; set; }

        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        /// <value>
        /// The type of the event.
        /// </value>
        [JsonProperty("accion")]
        public string EventType { get; set; }
    }
}
