// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaPendingOfficialMovement.cs" company="Microsoft">
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
    /// The OfficialDeltaPendingOfficialMovement.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Query.QueryEntity" />
    [DisplayName("balanceOficialMovimientos")]
    public class OfficialDeltaPendingOfficialMovement
    {
        /// <summary>
        /// Gets or sets the movement transactionId.
        /// </summary>
        /// <value>
        /// The movement transactionId.
        /// </value>
        [JsonProperty("idMovimientoTRUE")]
        public string MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the movementId.
        /// </summary>
        /// <value>
        /// The movementId.
        /// </value>
        [JsonProperty("idMovimientoPropietario")]
        public string MovementOwnerId { get; set; }

        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        [JsonProperty("idNodoOrigen")]
        public string SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination node identifier.
        /// </summary>
        /// <value>
        /// The destination node identifier.
        /// </value>
        [JsonProperty("idNodoDestino")]
        public string DestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the source product Id.
        /// </summary>
        /// <value>
        /// The source product Id.
        /// </value>
        [JsonProperty("idProductoOrigen")]
        public string SourceProductId { get; set; }

        /// <summary>
        /// Gets or sets the DestinationProductID.
        /// </summary>
        /// <value>
        /// The DestinationProductID.
        /// </value>
        [JsonProperty("idProductoDestino")]
        public string DestinationProductId { get; set; }

        /// <summary>
        /// Gets or sets the movementId.
        /// </summary>
        /// <value>
        /// The movementId.
        /// </value>
        [JsonProperty("idTipoMovimiento")]
        public string MovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonProperty("idPropietario")]
        public string OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the OwnerTransactionID.
        /// </summary>
        /// <value>
        /// The OwnerTransactionID.
        /// </value>
        [JsonProperty("valorPropiedad")]
        public decimal OwnerShipValue { get; set; }
    }
}
