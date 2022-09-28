// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialLogisticsDetails.cs" company="Microsoft">
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
    using System.ComponentModel;

    /// <summary>
    /// The LogisticsDetail.
    /// </summary>
    [DisplayName("Movimientos")]
    public class OfficialLogisticsDetails
    {
        private const string OfficialLogisticsStaticMessage = "Información oficial para el segmento de ";

        /// <summary>
        /// Gets or sets the movement.
        /// </summary>
        /// <value>
        /// The movement.
        /// </value>
        [DisplayName("MOVIMIENTO")]
        public string Movement { get; set; }

        /// <summary>
        /// Gets or sets the storage source.
        /// </summary>
        /// <value>
        /// The storage source.
        /// </value>
        [DisplayName("ALMACEN-ORIGEN")]
        public string StorageSource { get; set; }

        /// <summary>
        /// Gets or sets the product origin.
        /// </summary>
        /// <value>
        /// The product origin.
        /// </value>
        [DisplayName("PRODUCTO-ORIGEN")]
        public string ProductOrigin { get; set; }

        /// <summary>
        /// Gets or sets the storage destination.
        /// </summary>
        /// <value>
        /// The storage destination.
        /// </value>
        [DisplayName("ALMACEN-DESTINO")]
        public string StorageDestination { get; set; }

        /// <summary>
        /// Gets or sets the product destination.
        /// </summary>
        /// <value>
        /// The product destination.
        /// </value>
        [DisplayName("PRODUCTO-DESTINO")]
        public string ProductDestination { get; set; }

        /// <summary>
        /// Gets or sets the order purchase.
        /// </summary>
        /// <value>
        /// The order purchase.
        /// </value>
        [DisplayName("ORDEN-COMPRA")]
        public int? OrderPurchase { get; set; }

        /// <summary>
        /// Gets or sets the position purchase.
        /// </summary>
        /// <value>
        /// The position purchase.
        /// </value>
        [DisplayName("POS-COMPRA")]
        public int? PosPurchase { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [DisplayName("VALOR")]
        public decimal Value { get; set; }

        /// <summary>
        /// Gets or sets the uom.
        /// </summary>
        /// <value>
        /// The uom.
        /// </value>
        [DisplayName("UOM")]
        public string Uom { get; set; }

        /// <summary>
        /// Gets or sets the finding.
        /// </summary>
        /// <value>
        /// The finding.
        /// </value>
        [DisplayName("HALLAZGO")]
        public string Finding { get; set; }

        /// <summary>
        /// Gets or sets the diagnostic.
        /// </summary>
        /// <value>
        /// The diagnostic.
        /// </value>
        [DisplayName("DIAGNÓSTICO")]
        public string Diagnostic { get; set; }

        /// <summary>
        /// Gets or sets the impact.
        /// </summary>
        /// <value>
        /// The impact.
        /// </value>
        [DisplayName("IMPACTO")]
        public string Impact { get; set; }

        /// <summary>
        /// Gets or sets the solution.
        /// </summary>
        /// <value>
        /// The solution.
        /// </value>
        [DisplayName("SOLUCIÓN")]
        public string Solution { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [DisplayName("ESTADO")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        [DisplayName("ORDEN")]
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the date operation.
        /// </summary>
        /// <value>
        /// The date operation.
        /// </value>
        [DisplayName("FECHA-INICIO")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the date operation.
        /// </summary>
        /// <value>
        /// The date operation.
        /// </value>
        [DisplayName("FECHA-FIN")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the date operation.
        /// </summary>
        /// <value>
        /// The date operation.
        /// </value>
        [DisplayName("FECHA-CONTABILIZACION")]
        public DateTime? OperationDate { get; set; }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <param name="segment">The segment.</param>
        public void SetDefaultValues(string segment)
        {
            var message = $"{OfficialLogisticsStaticMessage}{segment}";
            this.Finding = message;
            this.Impact = message;
            this.Diagnostic = message;
            this.Solution = message;
            this.OrderPurchase = null;
            this.PosPurchase = null;
            this.Status = string.Empty;
        }
    }
}