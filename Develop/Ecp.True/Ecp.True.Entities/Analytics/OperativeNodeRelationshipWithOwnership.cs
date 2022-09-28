// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperativeNodeRelationshipWithOwnership.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Analytics
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Entities.Core;

    /// <summary>
    ///     The operative node relationship with ownership.
    /// </summary>
    public class OperativeNodeRelationshipWithOwnership : EditableEntity
    {
        /// <summary>
        /// Gets or sets the operative node relationship with ownership identifier.
        /// </summary>
        /// <value>
        /// The operative node relationship with ownership identifier.
        /// </value>
        public int OperativeNodeRelationshipWithOwnershipId { get; set; }

        /// <summary>
        /// Gets or sets the source product.
        /// </summary>
        /// <value>
        /// The source product.
        /// </value>
        public string SourceProduct { get; set; }

        /// <summary>
        /// Gets or sets the destination product.
        /// </summary>
        /// <value>
        /// The destination product.
        /// </value>
        public string DestinationProduct { get; set; }

        /// <summary>
        /// Gets or sets the transfer point.
        /// </summary>
        /// <value>
        /// The transfer point.
        /// </value>
        public string TransferPoint { get; set; }

        /// <summary>
        /// Gets or sets the source system.
        /// </summary>
        /// <value>
        /// The source system.
        /// </value>
        public string SourceSystem { get; set; }

        /// <summary>
        /// Gets or sets the load date.
        /// </summary>
        /// <value>
        /// The load date.
        /// </value>
        public DateTime LoadDate { get; set; }

        /// <summary>
        /// Gets or sets the execution identifier.
        /// </summary>
        /// <value>
        /// The execution identifier.
        /// </value>
        public Guid? ExecutionID { get; set; }

        /// <summary>
        /// Gets or sets the logistic source center.
        /// </summary>
        /// <value>
        /// The logistic source center.
        /// </value>
        public string LogisticSourceCenter { get; set; }

        /// <summary>
        /// Gets or sets the logistic destination center.
        /// </summary>
        /// <value>
        /// The logistic destination center.
        /// </value>
        public string LogisticDestinationCenter { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        [RequiredIf("IsDeleted", true, ErrorMessage = Entities.Constants.NotesCannotBeNullOrEmpty)]
        [MaxLength(1000, ErrorMessage = Entities.Constants.Max1000CharactersAllowed)]
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the is deleted.
        /// </summary>
        /// <value>
        /// The is deleted.
        /// </value>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Copies from.
        /// </summary>
        /// <param name="OperativeNodeRelationshipWithOwnership">The Operative Node Relationship With Ownership.</param>
        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var operativeNodeRelationshipWithOwnership = (OperativeNodeRelationshipWithOwnership)entity;

            this.SourceProduct = operativeNodeRelationshipWithOwnership.SourceProduct ?? this.SourceProduct;
            this.DestinationProduct = operativeNodeRelationshipWithOwnership.DestinationProduct ?? this.DestinationProduct;
            this.TransferPoint = operativeNodeRelationshipWithOwnership.TransferPoint ?? this.TransferPoint;
            this.SourceSystem = operativeNodeRelationshipWithOwnership.SourceSystem ?? this.SourceSystem;
            this.LogisticSourceCenter = operativeNodeRelationshipWithOwnership.LogisticSourceCenter ?? this.LogisticSourceCenter;
            this.LogisticDestinationCenter = operativeNodeRelationshipWithOwnership.LogisticDestinationCenter ?? this.LogisticDestinationCenter;
            this.Notes = operativeNodeRelationshipWithOwnership.Notes ?? this.Notes;
            this.IsDeleted = operativeNodeRelationshipWithOwnership.IsDeleted;
            this.RowVersion = operativeNodeRelationshipWithOwnership.RowVersion;
        }
    }
}