// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogisticsInventoryDetail.cs" company="Microsoft">
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
    using Constants = Ecp.True.Entities.Constants;

    /// <summary>
    /// The LogisticsInventoryDetail.
    /// </summary>
    [DisplayName("Inventarios")]
    public class LogisticsInventoryDetail : QueryEntity
    {
        /// <summary>
        /// Gets or sets the inventory.
        /// </summary>
        /// <value>
        /// The inventory.
        /// </value>
        [DisplayName("INVENTARIO")]
        public string Inventory { get; set; }

        /// <summary>
        /// Gets or sets the storage location.
        /// </summary>
        /// <value>
        /// The storage location.
        /// </value>
        [DisplayName("ALMACEN")]
        public string StorageLocation { get; set; }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        [DisplayName("PRODUCTO")]
        public string Product { get; set; }

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
        public string Order { get; set; }

        /// <summary>
        /// Gets or sets the date operation.
        /// </summary>
        /// <value>
        /// The date operation.
        /// </value>
        [DisplayName("FECHA-OPERATIVA")]
        public DateTime? DateOperation { get; set; }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        public void SetDefaultValues()
        {
            this.Finding = Constants.LogisticFileStaticMessage;
            this.Impact = Constants.LogisticFileStaticMessage;
            this.Diagnostic = Constants.LogisticFileStaticMessage;
            this.Solution = Constants.LogisticFileStaticMessage;
        }
    }
}