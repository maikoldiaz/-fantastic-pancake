// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogisticMovementResponse.cs" company="Microsoft">
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

    /// <summary>
    /// The LogisticMovementResponse class.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class LogisticMovementResponse : IValidatableObject
    {
        /// <summary>
        /// Gets or sets the StatusMessage.
        /// </summary>
        [Required(ErrorMessage = SapConstants.StatusMessageRequired)]
        [StringLength(5, ErrorMessage = SapConstants.StatusMessageLengthExceeded)]
        public string StatusMessage { get; set; }

        /// <summary>
        /// Gets or sets the IdMessage.
        /// </summary>
        [Required(ErrorMessage = SapConstants.IdMessageRequired)]
        [StringLength(20, ErrorMessage = SapConstants.IdMessageLengthExceeded)]
        public string IdMessage { get; set; }

        /// <summary>
        /// Gets or sets the Information.
        /// </summary>
        [Required(ErrorMessage = SapConstants.InformationRequired)]
        [StringLength(200, ErrorMessage = SapConstants.InformationLengthExceeded)]
        public string Information { get; set; }

        /// <summary>
        /// Gets or sets the MovementID.
        /// </summary>
        [Required(ErrorMessage = SapConstants.MovementIdRequired)]
        [StringLength(50, ErrorMessage = SapConstants.MovementIdLengthExceeded)]
        public string MovementId { get; set; }

        /// <summary>
        /// Gets or sets the SourceSystem.
        /// </summary>
        [Required(ErrorMessage = SapConstants.SourceSystemRequired)]
        [StringLength(20, ErrorMessage = SapConstants.SapSourceSystemLengthExceeded)]
        public string SourceSystem { get; set; }

        /// <summary>
        /// Gets or sets the DestinationSystem.
        /// </summary>
        [Required(ErrorMessage = SapConstants.DestinationSystemRequired)]
        [StringLength(20, ErrorMessage = SapConstants.SapDestinationSystemLengthExceeded)]
        public string DestinationSystem { get; set; }

        /// <summary>
        /// Gets or sets the TransactionID.
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the DateReceivedSystem.
        /// </summary>
        [Required(ErrorMessage = SapConstants.DateReceivedSystemRequired)]
        public DateTime DateReceivedSystem { get; set; }

        /// <summary>
        /// Validation StatusMessage & TransactionId.
        /// </summary>
        /// <param name="validationContext">Contains the validation context.</param>
        /// <returns>Result validation.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if (this.StatusMessage.Equals("S", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(this.TransactionId))
            {
                result.Add(new ValidationResult(SapConstants.TransactionIdRequired));
            }

            if (!string.IsNullOrEmpty(this.TransactionId) && this.TransactionId.Length > 10)
            {
                result.Add(new ValidationResult(SapConstants.TransactionIdLengthExceeded));
            }

            return result;
        }
    }
}
