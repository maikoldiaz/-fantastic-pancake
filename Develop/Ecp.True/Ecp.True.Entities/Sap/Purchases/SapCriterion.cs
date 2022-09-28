// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapCriterion.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    /// <summary>
    /// The SAP PO Criterion class.
    /// </summary>
    public class SapCriterion : IValidatableObject
    {
        /// <summary>
        /// Gets or sets value.
        /// </summary>
        [Required(ErrorMessage = SapConstants.ValueRequired)]
        [RegularExpression(SapConstants.ExpresionMaxTwoDecimals, ErrorMessage = SapConstants.MaxTwoDecimals)]
        [JsonProperty("VALUE")]
        public decimal? Value { get; set; }

        /// <summary>
        /// Gets or sets property.
        /// </summary>
        [Required(ErrorMessage = SapConstants.PropertyRequired)]
        [JsonProperty("PROPERTY")]
        public string Property { get; set; }

        /// <summary>
        /// Gets or sets uom.
        /// </summary>
        [Required(ErrorMessage = SapConstants.UomRequired)]
        [JsonProperty("UOM")]
        public string Uom { get; set; }

        /// <summary>
        /// Validate criterion class.
        /// </summary>
        /// <param name="validationContext">Contain validation context.</param>
        /// <returns>Response result.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if (this.Property != SapConstants.PropertyPurchasePercentage && this.Property != SapConstants.PropertyPurchaseVolume)
            {
                result.Add(new ValidationResult(SapConstants.PropertyInvalid));
            }

            if (this.Property == SapConstants.PropertyPurchasePercentage)
            {
                if (this.Value < 0 || this.Value > 100)
                {
                    result.Add(new ValidationResult(SapConstants.ValueInvalidForPurchasePercentage));
                }

                if (this.Uom != SapConstants.ValueUomForPurchasePercentage)
                {
                    result.Add(new ValidationResult(SapConstants.UomInvalidForPurchasePercentage));
                }
            }

            return result;
        }
    }
}
