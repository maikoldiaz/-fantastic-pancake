// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaInventory.cs" company="Microsoft">
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
    /// The OfficialDeltaInventory.
    /// </summary>
    [DisplayName("deltasOficialesInventarios")]
    public class OfficialDeltaInventory
    {
        /// <summary>
        /// Gets or sets the Movement Transaction Id.
        /// </summary>
        /// <value>
        /// The Movement Transaction Id.
        /// </value>
        [JsonProperty("idMovimientoDeltaInv")]
        public string MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the Movement Owner Id.
        /// </summary>
        /// <value>
        /// The Movement Owner Id.
        /// </value>
        [JsonProperty("idMovimientoPropietarioDeltaInv")]
        public string MovementOwnerId { get; set; }

        /// <summary>
        /// Gets or sets the Movement Date.
        /// </summary>
        /// <value>
        /// The Movement Date.
        /// </value>
        [JsonProperty("fecha")]
        public string OperationalDate { get; set; }

        /// <summary>
        /// Gets or sets the Node Id.
        /// </summary>
        /// <value>
        /// The Node Id.
        /// </value>
        [JsonProperty("idNodo")]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the Product Id.
        /// </summary>
        /// <value>
        /// The Product Id.
        /// </value>
        [JsonProperty("idProducto")]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the Owner Id.
        /// </summary>
        /// <value>
        /// The Owner Id.
        /// </value>
        [JsonProperty("idPropietario")]
        public string OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the Ownership Value.
        /// </summary>
        /// <value>
        /// The Ownership Value.
        /// </value>
        [JsonProperty("valorPropiedad")]
        public double OwnershipVolume { get; set; }
    }
}
