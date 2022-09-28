// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransferPointMovement.cs" company="Microsoft">
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
    using System.ComponentModel;

    /// <summary>
    /// Anylatical ownership data.
    /// </summary>
    public class TransferPointMovement : QueryEntity
    {
        /// <summary>
        /// Gets or sets the algorithm identifier.
        /// </summary>
        /// <value>
        /// The algorithm identifier.
        /// </value>
        [DisplayName("IdAlgoritmo")]
        public int AlgorithmId { get; set; }

        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        [DisplayName("IdTicketo")]
        public int TicketId { get; set; }

        /// <summary>
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>
        [DisplayName("IdMovimiento")]
        public string MovementId { get; set; }

        /// <summary>
        /// Gets or sets the name of the movement type.
        /// </summary>
        /// <value>
        /// The name of the movement type.
        /// </value>
        [DisplayName("Movimiento_TP")]
        public string MovementType { get; set; }

        /// <summary>
        /// Gets or sets the movement type identifier.
        /// </summary>
        /// <value>
        /// The movement type identifier.
        /// </value>
        [DisplayName("IdMovimiento_TP")]
        public int MovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the source node.
        /// </summary>
        /// <value>
        /// The name of the source node.
        /// </value>
        [DisplayName("Origen")]
        public string SourceNode { get; set; }

        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        [DisplayName("IdOrigen")]
        public int SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the source node type.
        /// </summary>
        /// <value>
        /// The name of the source node type.
        /// </value>
        [DisplayName("Origen_TP")]
        public string SourceNodeType { get; set; }

        /// <summary>
        /// Gets or sets the source node type identifier.
        /// </summary>
        /// <value>
        /// The source node type identifier.
        /// </value>
        [DisplayName("IdOrigen_TP")]
        public int SourceNodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the destintion node.
        /// </summary>
        /// <value>
        /// The name of the destintion node.
        /// </value>
        [DisplayName("Destino")]
        public string DestinationNode { get; set; }

        /// <summary>
        /// Gets or sets the destination node identifier.
        /// </summary>
        /// <value>
        /// The destination node identifier.
        /// </value>
        [DisplayName("IdDestino")]
        public int DestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the destintion type node.
        /// </summary>
        /// <value>
        /// The name of the destintion type node.
        /// </value>
        [DisplayName("Destino_TP")]
        public string DestinationNodeType { get; set; }

        /// <summary>
        /// Gets or sets the destination node type identifier.
        /// </summary>
        /// <value>
        /// The destination node type identifier.
        /// </value>
        [DisplayName("IdDestino_TP")]
        public int DestinationNodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the source product.
        /// </summary>
        /// <value>
        /// The name of the source product.
        /// </value>
        [DisplayName("Producto")]
        public string SourceProduct { get; set; }

        /// <summary>
        /// Gets or sets the source product identifier.
        /// </summary>
        /// <value>
        /// The source product identifier.
        /// </value>
        [DisplayName("IdProducto")]
        public string SourceProductId { get; set; }

        /// <summary>
        /// Gets or sets the name of the source product type.
        /// </summary>
        /// <value>
        /// The name of the source product type.
        /// </value>
        [DisplayName("Producto_TP")]
        public string SourceProductType { get; set; }

        /// <summary>
        /// Gets or sets the source product type identifier.
        /// </summary>
        /// <value>
        /// The source product type identifier.
        /// </value>
        [DisplayName("IdProducto_TP")]
        public int? SourceProductTypeId { get; set; }

        /// <summary>
        /// Gets or sets the movement volume.
        /// </summary>
        /// <value>
        /// The movement volume.
        /// </value>
        public decimal NetVolume { get; set; }

        /// <summary>
        /// Gets or sets the operational date.
        /// </summary>
        /// <value>
        /// The operational date.
        /// </value>
        public DateTime OperationalDate { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        public int? MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets the movement transaction identifier.
        /// </summary>
        /// <value>
        /// The movement transaction identifier.
        /// </value>
        public int MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime EndDate { get; set; }
    }
}
