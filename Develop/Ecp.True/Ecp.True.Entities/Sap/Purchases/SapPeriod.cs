// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapPeriod.cs" company="Microsoft">
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
    /// The SAP PO Period class.
    /// </summary>
    public class SapPeriod : IValidatableObject
    {
        /// <summary>
        /// Gets or sets start period.
        /// </summary>
        [Required(ErrorMessage = SapConstants.StartPeriodRequired)]
        [JsonProperty("STARTPERIOD")]
        public DateTime? StartPeriod { get; set; }

        /// <summary>
        /// Gets or sets end period.
        /// </summary>
        [Required(ErrorMessage = SapConstants.EndPeriodRequired)]
        [JsonProperty("ENDPERIOD")]
        public DateTime? EndPeriod { get; set; }

        /// <summary>
        /// Validate periods.
        /// </summary>
        /// <param name="validationContext">Contains the validation context.</param>
        /// <returns>Result validate.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if ((this.StartPeriod != null && this.EndPeriod != null) && (this.EndPeriod < this.StartPeriod))
            {
                result.Add(new ValidationResult(SapConstants.EndPeriodGreaterThanStartPeriod));
            }

            return result;
        }
    }
}
