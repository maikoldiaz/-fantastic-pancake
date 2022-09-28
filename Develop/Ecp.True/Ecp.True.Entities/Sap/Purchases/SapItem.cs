// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapItem.cs" company="Microsoft">
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
    using System.Linq;
    using Newtonsoft.Json;

    /// <summary>
    /// The SAP PO PurchaseItem class.
    /// </summary>
    public class SapItem : IValidatableObject
    {
        /// <summary>
        /// Gets or sets identifier.
        /// </summary>
        [Required(ErrorMessage = SapConstants.IdRequired)]
        [JsonProperty("ID")]
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets estimated volume.
        /// </summary>
        [RegularExpression(SapConstants.ExpresionMaxTwoDecimals, ErrorMessage = SapConstants.MaxTwoDecimals)]
        [JsonProperty("ESTIMATEDVOLUME")]
        public decimal? EstimatedVolume { get; set; }

        /// <summary>
        /// Gets or sets tolerance.
        /// </summary>
        [Required(ErrorMessage = SapConstants.ToleranceRequired)]
        [RegularExpression(SapConstants.ExpresionMaxTwoDecimals, ErrorMessage = SapConstants.MaxTwoDecimals)]
        [JsonProperty("TOLERANCE")]
        public decimal? Tolerance { get; set; }

        /// <summary>
        /// Gets or sets position status.
        /// </summary>
        [JsonProperty("POSITIONSTATUS")]
        public string PositionStatus { get; set; }

        /// <summary>
        /// Gets or sets expedition class.
        /// </summary>
        [JsonProperty("EXPEDITIONCLASS")]
        public string ExpeditionClass { get; set; }

        /// <summary>
        /// Gets or sets frequency.
        /// </summary>
        [JsonProperty("FREQUENCY")]
        public string Frequency { get; set; }

        /// <summary>
        /// Gets or sets commodity.
        /// </summary>
        [Required(ErrorMessage = SapConstants.CommodityRequired)]
        [JsonProperty("COMMODITY")]
        public SapCommodity Commodity { get; set; }

        /// <summary>
        /// Gets or sets location.
        /// </summary>
        [Required(ErrorMessage = SapConstants.LocationsRequired)]
        [JsonProperty("LOCATIONS")]
        public SapLocation Location { get; set; }

        /// <summary>
        /// Gets or sets facilities.
        /// </summary>
        [JsonProperty("FACILITIES")]
        public SapFacilities Facilities { get; set; }

        /// <summary>
        /// Gets or sets period.
        /// </summary>
        [Required(ErrorMessage = SapConstants.PeriodRequired)]
        [JsonProperty("PERIOD")]
        public SapPeriod Period { get; set; }

        /// <summary>
        /// Gets or sets criterion.
        /// </summary>
        [Required(ErrorMessage = SapConstants.CriterionRequired)]
        [JsonProperty("CRITERION")]
        public SapCriterion Criterion { get; set; }

        /// <summary>
        /// Validate purchase item class.
        /// </summary>
        /// <param name="validationContext">Contains validation context.</param>
        /// <returns>Response result.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            IEnumerable<string> values = new List<string> { string.Empty, "L", "S" };
            var result = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(this.Frequency))
            {
                this.Frequency = SapConstants.DefaultFrequency;
            }

            if (this.PositionStatus != null && values.ToList().Find(x => x.Equals(this.PositionStatus, StringComparison.Ordinal)) == null)
            {
                result.Add(new ValidationResult(SapConstants.InvalidPositionStatus));
            }

            return result;
        }
    }
}
