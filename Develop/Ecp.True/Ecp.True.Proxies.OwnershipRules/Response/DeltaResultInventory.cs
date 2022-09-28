// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaResultInventory.cs" company="Microsoft">
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
    using System.ComponentModel;
    using Newtonsoft.Json;

    /// <summary>
    /// The DeltaResultInventory.
    /// </summary>
    [DisplayName("resultadoInventario")]
    public class DeltaResultInventory
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
        /// Gets or sets the inventory transaction identifier.
        /// </summary>
        /// <value>
        /// The inventory transaction identifier.
        /// </value>
        [JsonProperty("idInventarioTRUE")]
        public int InventoryTransactionId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DeltaResultInventory"/> is sign.
        /// </summary>
        /// <value>
        ///   <c>true</c> if sign; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("signo")]
        public string Sign { get; set; }

        /// <summary>
        /// Gets or sets the delta.
        /// </summary>
        /// <value>
        /// The delta.
        /// </value>
        [JsonProperty("delta")]
        public decimal Delta { get; set; }
    }
}
