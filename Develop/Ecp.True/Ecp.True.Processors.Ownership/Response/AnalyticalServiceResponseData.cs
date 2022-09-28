// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalyticalServiceResponseData.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Calculation.Response
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Analytical movement operational data.
    /// </summary>
    public class AnalyticalServiceResponseData
    {
        /// <summary>
        /// Gets or sets the algorithm identifier.
        /// </summary>
        /// <value>
        /// The algorithm identifier.
        /// </value>
        [DisplayName("IdAlgoritmo")]
        public string AlgorithmId { get; set; }

        /// <summary>
        /// Gets or sets the movement type name.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>
        [DisplayName("Movimiento_TP")]
        public string MovementType { get; set; }

        /// <summary>
        /// Gets or sets the name of the source node.
        /// </summary>
        /// <value>
        /// The name of the source node.
        /// </value>
        [DisplayName("Origen")]
        public string SourceNode { get; set; }

        /// <summary>
        /// Gets or sets the name of the source node type.
        /// </summary>
        /// <value>
        /// The name of the source node type.
        /// </value>
        [DisplayName("Origen_TP")]
        public string SourceNodeType { get; set; }

        /// <summary>
        /// Gets or sets the name of the destintion node.
        /// </summary>
        /// <value>
        /// The name of the destintion node.
        /// </value>
        [DisplayName("Destino")]
        public string DestinationNode { get; set; }

        /// <summary>
        /// Gets or sets the name of the destintion type node.
        /// </summary>
        /// <value>
        /// The name of the destintion type node.
        /// </value>
        [DisplayName("Destino_TP")]
        public string DestinationNodeType { get; set; }

        /// <summary>
        /// Gets or sets the name of the source product.
        /// </summary>
        /// <value>
        /// The name of the source product.
        /// </value>
        [DisplayName("Producto")]
        public string SourceProduct { get; set; }

        /// <summary>
        /// Gets or sets the name of the source product type.
        /// </summary>
        /// <value>
        /// The name of the source product type.
        /// </value>
        [DisplayName("Producto_TP")]
        public string SourceProductType { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        [DisplayName("FechaInicio")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        [DisplayName("FechaFin")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the owner ship percentage.
        /// </summary>
        /// <value>
        /// The owner ship percentage.
        /// </value>
        [DisplayName("PorcentajePropiedad")]
        public decimal OwnershipPercentage { get; set; }

        /// <summary>
        /// Gets or sets the execution date.
        /// </summary>
        /// <value>
        /// The execution date.
        /// </value>
        public DateTime OperationalDate { get; set; }

        /// <summary>
        /// Gets or sets the transfer point.
        /// </summary>
        /// <value>
        /// The transfer point.
        /// </value>
        public string TransferPoint { get; set; }
    }
}
