// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Position.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Sap
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    /// <summary>
    /// The Position class.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class Position : IValidatableObject
    {
        /// <summary>
        /// Gets or sets the Position Id.
        /// </summary>
        /// <value>
        /// The Position Id.
        /// </value>
        [Required(ErrorMessage = SapConstants.PositionIdRequired)]
        [JsonProperty("ID_POSITION")]
        public int? PositionId { get; set; }

        /// <summary>
        /// Gets or sets the Position Status.
        /// </summary>
        /// <value>
        /// The Position Status.
        /// </value>
        [JsonProperty("ELIMINAR")]
        public string PositionStatus { get; set; }

        /// <summary>
        /// Gets or sets the Frequency.
        /// </summary>
        /// <value>
        /// The Frequency.
        /// </value>
        [JsonProperty("FREQUENCY")]
        public string Frequency { get; set; }

        /// <summary>
        /// Gets or sets the Material.
        /// </summary>
        /// <value>
        /// The Material.
        /// </value>
        [JsonProperty("MATERIAL")]
        public string Material { get; set; }

        /// <summary>
        /// Gets or sets the Quantity.
        /// </summary>
        /// <value>
        /// The Quantity.
        /// </value>
        [JsonProperty("QUANTITY")]
        public string Quantity { get; set; }

        /// <summary>
        /// Gets or sets the QuantityUom.
        /// </summary>
        /// <value>
        /// The QuantityUom.
        /// </value>
        [JsonProperty("QUANTITYUOM")]
        public string QuantityUom { get; set; }

        /// <summary>
        /// Gets or sets the StartTime.
        /// </summary>
        /// <value>
        /// The StartTime.
        /// </value>
        [JsonProperty("STARTTIME")]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the EndTime.
        /// </summary>
        /// <value>
        /// The EndTime.
        /// </value>
        [JsonProperty("ENDTIME")]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the Destination Location Id.
        /// </summary>
        /// <value>
        /// The Destination Location Id.
        /// </value>
        [JsonProperty("DESTINATIONLOCATIONID")]
        public string DestinationLocationId { get; set; }

        /// <summary>
        /// Gets or sets the Destination Storage Location Id.
        /// </summary>
        /// <value>
        /// The Destination Storage Location Id.
        /// </value>
        [JsonProperty("DESTINATIONSTORAGELOCATIONID")]
        public string DestinationStorageLocationId { get; set; }

        /// <summary>
        /// Gets or sets the Batch.
        /// </summary>
        /// <value>
        /// The Batch.
        /// </value>
        [JsonProperty("BATCH")]
        public string Batch { get; set; }

        /// <summary>
        /// Gets or sets the Rejection Reason.
        /// </summary>
        /// <value>
        /// The Rejection Reason.
        /// </value>
        [JsonProperty("MOTIVO_RECHAZO")]
        public string RejectionReason { get; set; }

        /// <summary>
        /// Gets or sets the Key.
        /// </summary>
        /// <value>
        /// The Key.
        /// </value>
        [JsonProperty("KEY")]
        public string Key { get; set; }

        /// <summary>
        /// Validate dates.
        /// </summary>
        /// <param name="validationContext">Contains the validation context.</param>
        /// <returns>Result validation.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var values = new List<string> { string.Empty, "X" };
            var result = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(this.Frequency))
            {
                this.Frequency = SapConstants.DefaultFrequency;
            }

            if (this.PositionStatus != null && values.Find(x => x.Equals(this.PositionStatus, StringComparison.Ordinal)) == null)
            {
                result.Add(new ValidationResult(SapConstants.InvalidPositionStatus));
            }

            return result;
        }
    }
}
