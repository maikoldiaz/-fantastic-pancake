// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Event.cs" company="Microsoft">
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
    using System.ComponentModel;
    using Newtonsoft.Json;

    /// <summary>
    /// The Node Connections.
    /// </summary>
    [DisplayName("Eventos")]
    public class Event : QueryEntity
    {
        /// <summary>
        /// Gets a value indicating whether event is final.
        /// </summary>
        /// <value>
        /// The value indicating whether event is final or not.
        /// </value>
        [JsonProperty("finalizado")]
        public bool IsFinal => false;

        /// <summary>
        /// Gets or sets the event identifier.
        /// </summary>
        /// <value>
        /// The event identifier.
        /// </value>
        [JsonProperty("idEvento")]
        public int EventIdentifier { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether type of the event is agreement.
        /// </summary>
        /// <value>
        /// The value indicating whether type of the event is agreement.
        /// </value>
        [JsonProperty("esAcuerdo")]
        public bool IsAgreement { get; set; }

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
        /// The product identifier.
        /// </value>
        [JsonProperty("idProductoOrigen")]
        public string SourceProductId { get; set; }

        /// <summary>
        /// Gets or sets the source destination identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [JsonProperty("idProductoDestino")]
        public string DestinationProductId { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier 1.
        /// </summary>
        /// <value>
        /// The owner identifier 1.
        /// </value>
        [JsonProperty("idPropietario1")]
        public int OwnerId1 { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier 2.
        /// </summary>
        /// <value>
        /// The owner identifier 2.
        /// </value>
        [JsonProperty("idPropietario2")]
        public int OwnerId2 { get; set; }

        /// <summary>
        /// Gets or sets the ownership value.
        /// </summary>
        /// <value>
        /// The ownership value.
        /// </value>
        [JsonProperty("valorPropiedad")]
        public decimal? OwnershipValue { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        [JsonProperty("unidadPropiedad")]
        public string MeasurementUnit { get; set; }
    }
}