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

namespace Ecp.True.Stubs.Api.Response
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
        /// Gets or sets the inventory identifier.
        /// </summary>
        /// <value>
        /// The inventory identifier.
        /// </value>
        [JsonProperty("idInventario")]
        public string InventoryId { get; set; }

        /// <summary>
        /// Gets or sets the inventory transaction identifier.
        /// </summary>
        /// <value>
        /// The inventory transaction identifier.
        /// </value>
        [JsonProperty("idInventarioTRUE")]
        public int InventoryProductId { get; set; }

        /// <summary>
        /// Gets or sets the sign.
        /// </summary>
        /// <value>
        /// The sign.
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
