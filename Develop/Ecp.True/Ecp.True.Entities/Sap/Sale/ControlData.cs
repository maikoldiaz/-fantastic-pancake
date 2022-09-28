// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlData.cs" company="Microsoft">
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
    using System.Linq;
    using Newtonsoft.Json;

    /// <summary>
    /// The ControlData class.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class ControlData : IValidatableObject
    {
        /// <summary>
        /// Gets or sets the Date Received Po.
        /// </summary>
        /// <value>
        /// The Date Received Po.
        /// </value>
        [JsonProperty("DATERECEIVEDPO")]
        public DateTime? DateReceivedPo { get; set; }

        /// <summary>
        /// Gets or sets the Source System.
        /// </summary>
        /// <value>
        /// The Source System.
        /// </value>
        [JsonProperty("SOURCESYSTEM")]
        public string SourceSystem { get; set; }

        /// <summary>
        /// Gets or sets the Destination System.
        /// </summary>
        /// <value>
        /// The Destination System.
        /// </value>
        [JsonProperty("DESTINATIONSYSTEM")]
        public string DestinationSystem { get; set; }

        /// <summary>
        /// Gets or sets the Message Id.
        /// </summary>
        /// <value>
        /// The Message Id.
        /// </value>
        [JsonProperty("MESSAGEID")]
        [Required(ErrorMessage = SapConstants.MessageIdRequired)]
        [StringLength(32, ErrorMessage = SapConstants.MessageIdLength)]
        public string MessageId { get; set; }

        /// <summary>
        /// Gets or sets the Event Sap Po.
        /// </summary>
        /// <value>
        /// The Event Sap Po.
        /// </value>
        [StringLength(10, ErrorMessage = SapConstants.EventSapPoLengthExceeded)]
        [Required(ErrorMessage = SapConstants.EventSapPoRequired)]
        [JsonProperty("EVENT_SAPPO")]
        public string EventSapPo { get; set; }

        /// <summary>
        /// Data Validation.
        /// </summary>
        /// <param name="validationContext">The context.</param>
        /// <returns>The result validation.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            IEnumerable<string> values = new List<string> { Constants.EventSapCreate, Constants.EventSapUpdate };
            var result = new List<ValidationResult>();

            if (values.ToList().Find(x => x.Equals(this.EventSapPo, StringComparison.Ordinal)) == null)
            {
                result.Add(new ValidationResult(SapConstants.InvalidEventSapPo));
            }

            return result;
        }
    }
}
