// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapOrder.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Sap.Purchases
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    /// <summary>
    /// The SAP PO Order class.
    /// </summary>
    public class SapOrder : IValidatableObject
    {
        /// <summary>
        /// Gets or sets purchase order identifier.
        /// </summary>
        [Required(ErrorMessage = SapConstants.PurchaseOrderIdRequired)]
        [JsonProperty("PURCHASEORDERID")]
        public string PurchaseOrderId { get; set; }

        /// <summary>
        /// Gets or sets purchase order type.
        /// </summary>
        [Required(ErrorMessage = SapConstants.PurchaseOrderTypeRequired)]
        [JsonProperty("PURCHASEORDERTYPE")]
        public string PurchaseOrderType { get; set; }

        /// <summary>
        /// Gets or sets source location.
        /// </summary>
        [JsonProperty("SOURCELOCATION")]
        public string SourceLocation { get; set; }

        /// <summary>
        /// Gets or sets supply center.
        /// </summary>
        [JsonProperty("CENTROSUMINISTRADOR")]
        public string SupplyCenter { get; set; }

        /// <summary>
        /// Gets or sets category.
        /// </summary>
        [JsonProperty("CATEGORY")]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets date order.
        /// </summary>
        [JsonProperty("DATEORDER")]
        public DateTime? DateOrder { get; set; }

        /// <summary>
        /// Gets or sets status.
        /// </summary>
        [Required(ErrorMessage = SapConstants.StatusRequired)]
        [JsonProperty("STATUS")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets provider.
        /// </summary>
        [Required(ErrorMessage = SapConstants.ProviderRequired)]
        [JsonProperty("PROVIDER")]
        public SapProvider Provider { get; set; }

        /// <summary>
        /// Gets or sets society.
        /// </summary>
        [Required(ErrorMessage = SapConstants.SocietyRequired)]
        [JsonProperty("SOCIETY")]
        public SapSociety Society { get; set; }

        /// <summary>
        /// Gets or sets other.
        /// </summary>
        [JsonProperty("OTHERS")]
        public SapOther Other { get; set; }

        /// <summary>
        /// Gets or sets purchase items.
        /// </summary>
        [Required(ErrorMessage = SapConstants.PurchaseItemsRequired)]
        [JsonProperty("PURCHASEITEMS")]
        public SapPurchaseItem PurchaseItem { get; set; }

        /// <summary>
        /// Validate order class.
        /// </summary>
        /// <param name="validationContext">Contains validation context.</param>
        /// <returns>Response result.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if (this.Status != SapConstants.StatusActive && this.Status != SapConstants.StatusUnauthorized)
            {
                result.Add(new ValidationResult(SapConstants.StatusInvalid));
            }

            return result;
        }
    }
}
