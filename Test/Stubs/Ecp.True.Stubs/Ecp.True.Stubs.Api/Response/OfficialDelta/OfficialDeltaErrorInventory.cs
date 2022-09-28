// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaErrorInventory.cs" company="Microsoft">
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
    using System.Diagnostics.CodeAnalysis;
    using Ecp.True.Entities.Enumeration;
    using Newtonsoft.Json;

    /// <summary>
    /// The DeltaErrorInventory.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [DisplayName("inventariosErrores")]
    public class OfficialDeltaErrorInventory
    {
        /// <summary>
        /// Gets or sets the inventory transaction identifier.
        /// </summary>
        /// <value>
        /// The inventory transaction identifier.
        /// </value>
        [JsonProperty("idInventarioTRUE")]
        public string InventoryTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [JsonProperty("descriptionError")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the origin.
        /// </summary>
        /// <value>
        /// The origin.
        /// </value>
        [JsonProperty("origen")]
        public OriginType Origin { get; set; }
    }
}
