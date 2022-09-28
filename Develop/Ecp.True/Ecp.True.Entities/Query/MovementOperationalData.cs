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

namespace Ecp.True.Entities.Query
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Newtonsoft.Json;
    using Constants = Ecp.True.Core.Constants;

    /// <summary>
    /// The Movements OperationalData.
    /// </summary>
    [DisplayName("Movimientos")]
    public class MovementOperationalData : QueryEntity
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
        public int MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        [JsonProperty("idNodoOrigen")]
        public int? SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination node identifier.
        /// </summary>
        /// <value>
        /// The destination node identifier.
        /// </value>
        [JsonProperty("idNodoDestino")]
        public int? DestinationNodeId { get; set; }

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
        /// Gets the response movement type identifier.
        /// </summary>
        /// <value>
        /// The response movement type identifier.
        /// </value>
        [JsonProperty("tipoMovimiento")]
        public string ResponseMovementTypeId
        {
            get
            {
                Dictionary<string, string> movementTypes = new Dictionary<string, string>();

                movementTypes.Add(((int)MovementType.Interface).ToString(CultureInfo.InvariantCulture), "INTERFASE");
                movementTypes.Add(((int)MovementType.Tolerance).ToString(CultureInfo.InvariantCulture), "TOLERANCIA");
                movementTypes.Add(((int)MovementType.UnidentifiedLoss).ToString(CultureInfo.InvariantCulture), "PNI");
                movementTypes.Add(((int)MovementType.InputCancellation).ToString(CultureInfo.InvariantCulture), "ANULACIONENTRADA");
                movementTypes.Add(((int)MovementType.OutputCancellation).ToString(CultureInfo.InvariantCulture), "ANULACIONSALIDA");
                movementTypes.Add(Constants.Cancellation, "ANULACION");
                movementTypes.Add(((int)MovementType.DeltaInventory).ToString(CultureInfo.InvariantCulture), "ANULACION");
                movementTypes.Add(((int)MovementType.DeltaAnnulationEMEvacuation).ToString(CultureInfo.InvariantCulture), "ANULACION");
                movementTypes.Add(((int)MovementType.DeltaAnnulationSMEvacuation).ToString(CultureInfo.InvariantCulture), "ANULACION");

                if (this.MovementTypeId != null && movementTypes.ContainsKey(this.MovementTypeId))
                {
                    return movementTypes[this.MovementTypeId];
                }
                else if (this.MessageTypeId == MessageType.Loss)
                {
                    return "PI";
                }
                else
                {
                    return "MOVIMIENTO";
                }
            }
        }

        /// <summary>
        /// Gets or sets the movement type identifier.
        /// </summary>
        /// <value>
        /// The movement type identifier.
        /// </value>
        [JsonIgnore]
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
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>
        [JsonIgnore]
        public string MovementId { get; set; }
    }
}