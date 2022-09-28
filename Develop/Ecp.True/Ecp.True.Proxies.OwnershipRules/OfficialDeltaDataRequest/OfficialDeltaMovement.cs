// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaMovement.cs" company="Microsoft">
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
    /// The OfficialDeltaMovement.
    /// </summary>
    [DisplayName("deltasOficialesMovimientos")]
    public class OfficialDeltaMovement
    {
        /// <summary>
        /// Gets or sets the Movement Transaction Id.
        /// </summary>
        /// <value>
        /// The Movement Transaction Id.
        /// </value>
        [JsonProperty("idMovimientoTRUE")]
        public string MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the Movement Owner Id.
        /// </summary>
        /// <value>
        /// The Movement Owner Id.
        /// </value>
        [JsonProperty("idMovimientoPropietario")]
        public string MovementOwnerId { get; set; }

        /// <summary>
        /// Gets or sets the Source Node Id.
        /// </summary>
        /// <value>
        /// The Source Node Id.
        /// </value>
        [JsonProperty("idNodoOrigen")]
        public string SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the Destination Node Id.
        /// </summary>
        /// <value>
        /// The Destination Node Id.
        /// </value>
        [JsonProperty("idNodoDestino")]
        public string DestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the Origin Product Id.
        /// </summary>
        /// <value>
        /// The Origin Product Id.
        /// </value>
        [JsonProperty("idProductoOrigen")]
        public string SourceProductId { get; set; }

        /// <summary>
        /// Gets or sets the Destination Product Id.
        /// </summary>
        /// <value>
        /// The Destination Product Id.
        /// </value>
        [JsonProperty("idProductoDestino")]
        public string DestinationProductId { get; set; }

        /// <summary>
        /// Gets or sets the Movement Type Id.
        /// </summary>
        /// <value>
        /// The Movement Type Id.
        /// </value>
        [JsonProperty("idTipoMovimiento")]
        public string MovementTypeId { get; set; }

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
