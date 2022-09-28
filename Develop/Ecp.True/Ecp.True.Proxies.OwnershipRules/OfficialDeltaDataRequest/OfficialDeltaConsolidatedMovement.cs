// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaConsolidatedMovement.cs" company="Microsoft">
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
    using Ecp.True.Core;
    using Newtonsoft.Json;

    /// <summary>
    /// The OfficialDeltaConsolidatedMovement.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Query.QueryEntity" />
    [DisplayName("consolidadosMovimientos")]
    public class OfficialDeltaConsolidatedMovement
    {
        /// <summary>
        /// Gets or sets the consolidated movement identifier.
        /// </summary>
        /// <value>
        /// The consolidated movement identifier.
        /// </value>
        [JsonProperty("idMovimientoTRUE")]
        public string ConsolidatedMovementId { get; set; }

        /// <summary>
        /// Gets or sets the movement owner identifier.
        /// </summary>
        /// <value>
        /// The movement owner identifier.
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
        /// Gets or sets the source product identifier.
        /// </summary>
        /// <value>
        /// The source product identifier.
        /// </value>
        [JsonProperty("idProductoOrigen")]
        public string SourceProductId { get; set; }

        /// <summary>
        /// Gets or sets the destination product identifier.
        /// </summary>
        /// <value>
        /// The destination product identifier.
        /// </value>
        [JsonProperty("idProductoDestino")]
        public string DestinationProductId { get; set; }

        /// <summary>
        /// Gets or sets the movement type identifier.
        /// </summary>
        /// <value>
        /// The movement type identifier.
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
        /// Gets or sets the ownership volume.
        /// </summary>
        /// <value>
        /// The ownership volume.
        /// </value>
        [JsonProperty("valorPropiedad")]
        public decimal OwnershipVolume { get; set; }

        /// <summary>
        /// Copies all values from other.
        /// </summary>
        /// <param name="other">The other object.</param>
        public void CopyFrom(OfficialDeltaConsolidatedMovement other)
        {
            ArgumentValidators.ThrowIfNull(other, nameof(other));

            this.ConsolidatedMovementId = other.ConsolidatedMovementId;
            this.MovementOwnerId = other.MovementOwnerId;
            this.SourceNodeId = other.SourceNodeId;
            this.DestinationNodeId = other.DestinationNodeId;
            this.SourceProductId = other.SourceProductId;
            this.DestinationProductId = other.DestinationProductId;
            this.MovementTypeId = other.MovementTypeId;
            this.OwnerId = other.OwnerId;
            this.OwnershipVolume = other.OwnershipVolume;
        }
    }
}
