// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperativeNodeRelationship.cs" company="Microsoft">
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
    ///     The operative node relationships.
    /// </summary>
    public class OperativeNodeRelationship : EditableEntity
    {
        /// <summary>
        /// Gets or sets the operative node relationship identifier.
        /// </summary>
        /// <value>
        /// The operative node relationship identifier.
        /// </value>
        public int OperativeNodeRelationshipId { get; set; }

        /// <summary>
        /// Gets or sets the transfer point.
        /// </summary>
        /// <value>
        /// The transfer point.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.TransferPointCannotBeNullOrEmpty)]
        public string TransferPoint { get; set; }

        /// <summary>
        /// Gets or sets the source field.
        /// </summary>
        /// <value>
        /// The source field.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.SourceNodeCannotBeNullOrEmpty)]
        [MaxLength(200, ErrorMessage =Entities.Constants.Max200CharactersAllowed)]
        public string SourceField { get; set; }

        /// <summary>
        /// Gets or sets the field water production.
        /// </summary>
        /// <value>
        /// The field water production.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.FieldWaterProductionCannotBeNullOrEmpty)]
        [MaxLength(200, ErrorMessage = Entities.Constants.Max200CharactersAllowed)]
        public string FieldWaterProduction { get; set; }

        /// <summary>
        /// Gets or sets the related source field.
        /// </summary>
        /// <value>
        /// The related source field.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.RelatedSourceFieldCannotBeNullOrEmpty)]
        [MaxLength(200, ErrorMessage = Entities.Constants.Max200CharactersAllowed)]
        public string RelatedSourceField { get; set; }

        /// <summary>
        /// Gets or sets the destination node.
        /// </summary>
        /// <value>
        /// The destination node.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.DestinationNodeCannotBeNullOrEmpty)]
        public string DestinationNode { get; set; }

        /// <summary>
        /// Gets or sets the destination node type.
        /// </summary>
        /// <value>
        /// The type of the destination node.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.DestinationNodeTypeCannotBeNullOrEmpty)]
        public string DestinationNodeType { get; set; }

        /// <summary>
        /// Gets or sets the movement type.
        /// </summary>
        /// <value>
        /// The type of the movement.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.MovementTypeCannotBeNullOrEmpty)]
        public string MovementType { get; set; }

        /// <summary>
        /// Gets or sets the source node.
        /// </summary>
        /// <value>
        /// The source node.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.SourceNodeCannotBeNullOrEmpty)]
        public string SourceNode { get; set; }

        /// <summary>
        /// Gets or sets the source node type.
        /// </summary>
        /// <value>
        /// The type of the source node.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.SourceNodeTypeCannotBeNullOrEmpty)]
        public string SourceNodeType { get; set; }

        /// <summary>
        /// Gets or sets the source product.
        /// </summary>
        /// <value>
        /// The source product.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.SourceProductCannotBeNullOrEmpty)]
        public string SourceProduct { get; set; }

        /// <summary>
        /// Gets or sets the source product type.
        /// </summary>
        /// <value>
        /// The type of the source product.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.SourceProductTypeCannotBeNullOrEmpty)]
        public string SourceProductType { get; set; }

        /// <summary>
        /// Gets or sets the load time.
        /// </summary>
        /// <value>
        /// The load time.
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
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        [RequiredIf("IsDeleted", true, ErrorMessage = Entities.Constants.NotesCannotBeNullOrEmpty)]
        [MaxLength(1000, ErrorMessage = Entities.Constants.Max1000CharactersAllowed)]
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the is deleted.
        /// </summary>
        /// <value>
        /// The is deleted.
        /// </value>
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// Copies from.
        /// </summary>
        /// <param name="OperativeNodeRelationship">The Operative Node Relationship With Ownership.</param>
        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var operativeNodeRelationship = (OperativeNodeRelationship)entity;

            this.SourceProduct = operativeNodeRelationship.SourceProduct ?? this.SourceProduct;
            this.SourceField = operativeNodeRelationship.SourceField ?? this.SourceField;
            this.FieldWaterProduction = operativeNodeRelationship.FieldWaterProduction ?? this.FieldWaterProduction;
            this.RelatedSourceField = operativeNodeRelationship.RelatedSourceField ?? this.RelatedSourceField;
            this.SourceNode = operativeNodeRelationship.SourceNode ?? this.SourceNode;
            this.DestinationNode = operativeNodeRelationship.DestinationNode ?? this.DestinationNode;
            this.SourceProductType = operativeNodeRelationship.SourceProductType ?? this.SourceProductType;
            this.SourceNodeType = operativeNodeRelationship.SourceNodeType ?? this.SourceNodeType;
            this.DestinationNodeType = operativeNodeRelationship.DestinationNodeType ?? this.DestinationNodeType;
            this.CopyFromExtended(entity);
        }

        private void CopyFromExtended(IEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var operativeNodeRelationship = (OperativeNodeRelationship)entity;

            this.Notes = operativeNodeRelationship.Notes ?? this.Notes;
            this.IsDeleted = operativeNodeRelationship.IsDeleted;
            this.RowVersion = operativeNodeRelationship.RowVersion;
            this.TransferPoint = operativeNodeRelationship.TransferPoint ?? this.TransferPoint;
            this.MovementType = operativeNodeRelationship.MovementType ?? this.MovementType;
        }
    }
}