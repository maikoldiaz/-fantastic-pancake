// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementOperationalData.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Stubs.Api.Models
{
    using System;
    using System.ComponentModel;
    using Newtonsoft.Json;

    /// <summary>
    /// The Movements OperationalData.
    /// </summary>
    [DisplayName("Movimientos")]
    public class MovementOperationalData
    {
        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        [JsonProperty("tiquete")]
        public int Ticket { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonProperty("idPropietario")]
        public int? OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>
        [JsonProperty("idMovimiento")]
        public int MovementId { get; set; }

        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        [JsonProperty("idNodoOrigen")]
        public int? SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the source product identifier.
        /// </summary>
        /// <value>
        /// The source product identifier.
        /// </value>
        [JsonProperty("idProductoOrigen")]
        public string SourceProductId { get; set; }

        /// <summary>
        /// Gets or sets the destination node identifier.
        /// </summary>
        /// <value>
        /// The destination node identifier.
        /// </value>
        [JsonProperty("idNodoDestino")]
        public int? DestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination product identifier.
        /// </summary>
        /// <value>
        /// The destination product identifier.
        /// </value>
        [JsonProperty("idProductoDestino")]
        public string DestinationProductId { get; set; }

        /// <summary>
        /// Gets or sets the net volume.
        /// </summary>
        /// <value>
        /// The net volume.
        /// </value>
        [JsonProperty("volumenNeto")]
        public decimal? NetVolume { get; set; }

        /// <summary>
        /// Gets or sets the ownership value.
        /// </summary>
        /// <value>
        /// The ownership value.
        /// </value>
        [JsonProperty("valorPropiedad")]
        public decimal? OwnershipValue { get; set; }

        /// <summary>
        /// Gets or sets the ownership unit.
        /// </summary>
        /// <value>
        /// The ownership unit.
        /// </value>
        [JsonProperty("unidadPropiedad")]
        public string OwnershipUnit { get; set; }

        /// <summary>
        /// Gets or sets the movement type identifier.
        /// </summary>
        /// <value>
        /// The movement type identifier.
        /// </value>
        [JsonProperty("tipoMovimiento")]
        public string MovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the message type identifier.
        /// </summary>
        /// <value>
        /// The message type identifier.
        /// </value>
        [JsonIgnore]
        public MessageType? MessageTypeId { get; set; }

        /// <summary>
        /// Gets or sets the operational date.
        /// </summary>
        /// <value>
        /// The operational date.
        /// </value>
        [JsonIgnore]
        public DateTime OperationalDate { get; set; }

        /// <summary>
        /// Gets or sets the type of the movement.
        /// </summary>
        /// <value>
        /// The type of the movement.
        /// </value>
        [JsonIgnore]
        public string MovementType { get; set; }

        /// <summary>
        /// Gets or sets the source node.
        /// </summary>
        /// <value>
        /// The source node.
        /// </value>
        [JsonIgnore]
        public string SourceNode { get; set; }

        /// <summary>
        /// Gets or sets the destination node.
        /// </summary>
        /// <value>
        /// The destination node.
        /// </value>
        [JsonIgnore]
        public string DestinationNode { get; set; }

        /// <summary>
        /// Gets or sets the source product.
        /// </summary>
        /// <value>
        /// The source product.
        /// </value>
        [JsonIgnore]
        public string SourceProduct { get; set; }

        /// <summary>
        /// Gets or sets the source product type identifier.
        /// </summary>
        /// <value>
        /// The source product type identifier.
        /// </value>
        [JsonIgnore]
        public string SourceProductTypeId { get; set; }

        /// <summary>
        /// Gets or sets the type of the source product.
        /// </summary>
        /// <value>
        /// The type of the source product.
        /// </value>
        [JsonIgnore]
        public string SourceProductType { get; set; }

        /// <summary>
        /// Gets or sets the destination product.
        /// </summary>
        /// <value>
        /// The destination product.
        /// </value>
        [JsonIgnore]
        public string DestinationProduct { get; set; }

        /// <summary>
        /// Gets or sets the destination product type identifier.
        /// </summary>
        /// <value>
        /// The destination product type identifier.
        /// </value>
        [JsonIgnore]
        public string DestinationProductTypeId { get; set; }

        /// <summary>
        /// Gets or sets the type of the destination product.
        /// </summary>
        /// <value>
        /// The type of the destination product.
        /// </value>
        [JsonIgnore]
        public string DestinationProductType { get; set; }

        /// <summary>
        /// Gets or sets the volume unit.
        /// </summary>
        /// <value>
        /// The volume unit.
        /// </value>
        [JsonIgnore]
        public string VolumeUnit { get; set; }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>
        /// The owner.
        /// </value>
        [JsonIgnore]
        public string Owner { get; set; }
    }
}