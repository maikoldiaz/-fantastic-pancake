// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeCostCenter.cs" company="Microsoft">
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
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The Node MovementType By CostCenter.
    /// </summary>
    public class NodeCostCenter : EditableEntity
    {
        /// <summary>
        /// Gets or sets the Node MovementType by CostCenter identifier.
        /// </summary>
        /// <value>
        /// The Node MovementType by CostCenter identifier.
        /// </value>
        public int NodeCostCenterId { get; set; }

        /// <summary>
        /// Gets or sets the Source Node identifier.
        /// </summary>
        /// <value>
        /// The reversed Source Node identifier.
        /// </value>
        [Required(ErrorMessage = Constants.NodeCostCenterSourceNodeRequiredValidation)]
        public int? SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination node identifier.
        /// </summary>
        /// <value>
        /// The destination node identifier.
        /// </value>
        public int? DestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the Movement Type identifier.
        /// </summary>
        /// <value>
        /// The Movement Type identifier.
        /// </value>
        [Required(ErrorMessage = Constants.NodeCostCenterMovementTypeRequiredValidation)]
        public int? MovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Cost Center identifier.
        /// </summary>
        /// <value>
        /// The Cost Center identifier.
        /// </value>
        [Required(ErrorMessage = Constants.NodeCostCenterCostCenterRequiredValidation)]
        public int? CostCenterId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.NodeCostCenterStatusIsRequired)]
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the categoryelement.
        /// </summary>
        /// <value>
        /// The Movement Type.
        /// </value>
        public virtual CategoryElement MovementTypeCategoryElement { get; set; }

        /// <summary>
        /// Gets or sets the categoryelement.
        /// </summary>
        /// <value>
        /// The Cost Center CategoryElement.
        /// </value>
        public virtual CategoryElement CostCenterCategoryElement { get; set; }

        /// <summary>
        /// Gets or sets the type of the source node origin.
        /// </summary>
        /// <value>
        /// The type of the source node origin.
        /// </value>
        public virtual Node SourceNode { get; set; }

        /// <summary>
        /// Gets or sets the type of the destination node origin.
        /// </summary>
        /// <value>
        /// The type of the destination node origin.
        /// </value>
        public virtual Node DestinationNode { get; set; }

        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            if (entity is NodeCostCenter original)
            {
                this.SourceNodeId = original.SourceNodeId;
                this.DestinationNodeId = original.DestinationNodeId;
                this.MovementTypeId = original.MovementTypeId;
                this.IsActive = original.IsActive;
                this.CostCenterId = original.CostCenterId;
                this.DestinationNode = original.DestinationNode;
                this.SourceNode = original.SourceNode;
                this.RowVersion = original.RowVersion;
            }
        }

        /// <summary>
        /// Override Equals to find duplicated NodeCostCenters.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>Whether the NodeCostCenter are equal.</returns>
        public override bool Equals(object obj)
        {
            return obj is NodeCostCenter other
                && this.SourceNodeId == other.SourceNodeId
                && this.DestinationNodeId == other.DestinationNodeId
                && this.CostCenterId == other.CostCenterId
                && this.IsActive == other.IsActive
                && this.MovementTypeId == other.MovementTypeId;
        }

        /// <summary>
        /// Override GetHashCode to find duplicated NodeCostCenters.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>Whether the NodeCostCenter are equal.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(this.SourceNodeId, this.DestinationNodeId, this.MovementTypeId, this.NodeCostCenterId);
        }
    }
}