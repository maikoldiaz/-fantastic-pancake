// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapTracking.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Admin
{
    using System;
    using System.Collections.Generic;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The Sap Tracking.
    /// </summary>
    public class SapTracking : Entity, IComment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SapTracking"/> class.
        /// </summary>
        public SapTracking()
        {
            this.SapTrackingErrors = new List<SapTrackingError>();
        }

        /// <summary>
        /// Gets or sets the sap tracking identifier.
        /// </summary>
        /// <value>
        /// The sap tracking identifier.
        /// </value>
        public int SapTrackingId { get; set; }

        /// <summary>
        /// Gets or sets the movement transaction identifier.
        /// </summary>
        /// <value>
        /// The movement transaction identifier.
        /// </value>
        public int? MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the file registration identifier.
        /// </summary>
        /// <value>
        /// The file registration identifier.
        /// </value>
        public int? FileRegistrationId { get; set; }

        /// <summary>
        /// Gets or sets the status type.
        /// </summary>
        /// <value>
        /// The status type.
        /// </value>
        public StatusType StatusTypeId { get; set; }

        /// <summary>
        /// Gets or sets the operational date.
        /// </summary>
        /// <value>
        /// The operational date.
        /// </value>
        public DateTime? OperationalDate { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        /// <value>
        /// The note.
        /// </value>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the blob path.
        /// </summary>
        /// <value>
        /// The blob path.
        /// </value>
        public string BlobPath { get; set; }

        /// <summary>
        /// Gets or sets the file registration.
        /// </summary>
        /// <value>
        /// The file registration.
        /// </value>
        public virtual FileRegistration FileRegistration { get; set; }

        /// <summary>
        /// Gets or sets the Movement.
        /// </summary>
        /// <value>
        /// The Movement.
        /// </value>
        public virtual Movement Movement { get; set; }

        /// <summary>
        /// Gets the sap tracking errors.
        /// </summary>
        /// <value>
        /// The sap tracking errors.
        /// </value>
        public virtual ICollection<SapTrackingError> SapTrackingErrors { get; private set; }

        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            True.Core.ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var element = (SapTracking)entity;

            this.StatusTypeId = element.StatusTypeId;
            this.OperationalDate = element.OperationalDate ?? this.OperationalDate;
            this.ErrorMessage = element.ErrorMessage ?? this.ErrorMessage;
            this.SessionId = element.SessionId ?? this.SessionId;
            this.Comment = element.Comment ?? this.Comment;
        }
    }
}
