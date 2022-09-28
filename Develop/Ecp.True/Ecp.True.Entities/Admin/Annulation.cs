// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Annulation.cs" company="Microsoft">
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
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The Annulation.
    /// </summary>
    public class Annulation : EditableEntity
    {
        /// <summary>
        /// Gets or sets the movement relationship type identifier.
        /// </summary>
        /// <value>
        /// The movement relationship type identifier.
        /// </value>
        public int AnnulationId { get; set; }

        /// <summary>
        /// Gets or sets the source movement type identifier.
        /// </summary>
        /// <value>
        /// The source movement type identifier.
        /// </value>
        public int SourceMovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the reversed movement type identifier.
        /// </summary>
        /// <value>
        /// The reversed movement type identifier.
        /// </value>
        public int AnnulationMovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        public int SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination node identifier.
        /// </summary>
        /// <value>
        /// The destination node identifier.
        /// </value>
        public int DestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the source product identifier.
        /// </summary>
        /// <value>
        /// The source product identifier.
        /// </value>
        public int SourceProductId { get; set; }

        /// <summary>
        /// Gets or sets the destination product identifier.
        /// </summary>
        /// <value>
        /// The destination product identifier.
        /// </value>
        public int DestinationProductId { get; set; }

        /// <summary>
        /// Gets or sets the Transaction Code identifier de Sap.
        /// </summary>
        /// <value>
        /// The Transaction Code identifier de Sap.
        /// </value>
        public int? SapTransactionCodeId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.AnnulationStatusIsRequired)]
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets the categoryelement.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public virtual CategoryElement SapTransactionCode { get; set; }

        /// <summary>
        /// Gets or sets the categoryelement.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public virtual CategoryElement SourceCategoryElement { get; set; }

        /// <summary>
        /// Gets or sets the categoryelement.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public virtual CategoryElement AnnulationCategoryElement { get; set; }

        /// <summary>
        /// Gets or sets the type of the source node origin.
        /// </summary>
        /// <value>
        /// The type of the source node origin.
        /// </value>
        public virtual OriginType SourceNodeOriginType { get; set; }

        /// <summary>
        /// Gets or sets the type of the destination node origin.
        /// </summary>
        /// <value>
        /// The type of the destination node origin.
        /// </value>
        public virtual OriginType DestinationNodeOriginType { get; set; }

        /// <summary>
        /// Gets or sets the type of the source product origin.
        /// </summary>
        /// <value>
        /// The type of the source product origin.
        /// </value>
        public virtual OriginType SourceProductOriginType { get; set; }

        /// <summary>
        /// Gets or sets the type of the destination product origin.
        /// </summary>
        /// <value>
        /// The type of the destination product origin.
        /// </value>
        public virtual OriginType DestinationProductOriginType { get; set; }

        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));

            var annulation = (Annulation)entity;
            this.SourceMovementTypeId = annulation.SourceMovementTypeId;
            this.AnnulationMovementTypeId = annulation.AnnulationMovementTypeId;
            this.SourceNodeId = annulation.SourceNodeId;
            this.DestinationNodeId = annulation.DestinationNodeId;
            this.SourceProductId = annulation.SourceProductId;
            this.DestinationProductId = annulation.DestinationProductId;
            this.SapTransactionCodeId = annulation.SapTransactionCodeId;
            this.IsActive = annulation.IsActive ?? this.IsActive;
            this.RowVersion = annulation.RowVersion;
        }
    }
}