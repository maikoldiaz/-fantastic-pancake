// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapPurchaseOrder.cs" company="Microsoft">
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
    /// The SAP PO Purchase Order class.
    /// </summary>
    public class SapPurchaseOrder : IValidatableObject
    {
        /// <summary>
        /// Gets or sets event sap po.
        /// </summary>
        [Required(ErrorMessage = SapConstants.EventSapPoRequired)]
        [StringLength(21, ErrorMessage = SapConstants.EventLengthExceeded)]
        [JsonProperty("EVENTO_SAPPO")]
        public string Event { get; set; }

        /// <summary>
        /// Gets or sets date.
        /// </summary>
        [JsonProperty("DATE")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets source system.
        /// </summary>
        [Required(ErrorMessage = SapConstants.SystemRequired)]
        [StringLength(20, ErrorMessage = SapConstants.SystemLengthExceededPurchase)]
        [JsonProperty("SYSTEM")]
        public string SourceSystem { get; set; }

        /// <summary>
        /// Gets or sets message identifier.
        /// </summary>
        [Required(ErrorMessage = SapConstants.MessageIdRequired)]
        [StringLength(32, ErrorMessage = SapConstants.MessageIdLength)]
        [JsonProperty("MESSAGEID")]
        public string MessageId { get; set; }

        /// <summary>
        /// Gets or sets purchase order.
        /// </summary>
        [Required(ErrorMessage = SapConstants.PurchaseOrderRequired)]
        [JsonProperty("PURCHASEORDER")]
        public SapOrder PurchaseOrder { get; set; }

        /// <summary>
        /// Data Validation.
        /// </summary>
        /// <param name="validationContext">The context.</param>
        /// <returns>The result validation.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var values = new List<string> { Constants.EventSapCreate, Constants.EventSapUpdate };
            var result = new List<ValidationResult>();

            if (values.Find(x => x.Equals(this.Event, StringComparison.Ordinal)) == null)
            {
                result.Add(new ValidationResult(SapConstants.InvalidEventSapPo));
            }

            return result;
        }
    }
}
