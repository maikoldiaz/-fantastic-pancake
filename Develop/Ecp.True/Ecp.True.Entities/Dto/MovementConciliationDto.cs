// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementConciliationDto.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Dto
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    ///  The MovementConciliationDto.
    /// </summary>
    public class MovementConciliationDto
    {
        /// <summary>
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement transaction identifier.
        /// </value>
        [JsonProperty("idMovimientoTRUE")]
        public int MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the movement type identifier.
        /// </summary>
        /// <value>
        /// The movement type identifier.
        /// </value>
        [JsonProperty("idTipoMovimiento")]
        public int? MovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        [JsonProperty("idNodoOrigen")]
        public int SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination node identifier.
        /// </summary>
        /// <value>
        /// The destination node identifier.
        /// </value>
        [JsonProperty("idNodoDestino")]
        public int DestinationNodeId { get; set; }

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
        /// Gets or sets the property value.
        /// </summary>
        /// <value>
        /// The ownership volume.
        /// </value>
        [JsonProperty("valorPropiedad")]
        public decimal? OwnershipVolume { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonProperty("idPropietario")]
        public int? OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the unit identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonProperty("idUnidad")]
        public int MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets the segment identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonProperty("idSegmento")]
        public int? SegmentId { get; set; }

        /// <summary>
        /// Gets or sets the percentage owner.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonProperty("porcentajePropietario")]
        public decimal? OwnershipPercentage { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonProperty("cantidad")]
        public decimal? NetStandardVolume { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonProperty("descripcion")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Sign.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonProperty("signo")]
        public string Sign { get; set; }

        /// <summary>
        /// Gets or sets the delta conciled.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonProperty("deltaConciliado")]
        public decimal? DeltaConciliated { get; set; }

        /// <summary>
        /// Gets or sets OperationalDate.
        /// </summary>
        /// <value>
        /// The OperationalDate.
        /// </value>
        [JsonProperty("OperationalDate")]
        public DateTime OperationalDate { get; set; }

        /// <summary>
        /// Gets or sets UncertaintyPercentage.
        /// </summary>
        /// <value>
        /// The UncertaintyPercentage.
        /// </value>
        [JsonProperty("UncertaintyPercentage")]
        public decimal? UncertaintyPercentage { get; set; }

        /// <summary>
        /// Gets or sets IsReconciled.
        /// </summary>
        /// <value>
        /// The IsReconciled.
        /// </value>
        [JsonProperty("IsReconciled")]
        public bool? IsReconciled { get; set; }
    }
}
