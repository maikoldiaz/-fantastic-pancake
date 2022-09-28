// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipNodeMovementInventoryDetails.cs" company="Microsoft">
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
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The system type.
    /// </summary>
    public class OwnershipNodeMovementInventoryDetails : Entity
    {
        /// <summary>
        /// Gets or sets the Movement or Inventory transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        public int TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the type of the movement.
        /// </summary>
        /// <value>
        /// The type of the movement.
        /// </value>
        public string MovementType { get; set; }

        /// <summary>
        /// Gets or sets the operational date.
        /// </summary>
        /// <value>
        /// The operational date.
        /// </value>
        public DateTime OperationalDate { get; set; }

        /// <summary>
        /// Gets or sets the name of the tank.
        /// </summary>
        /// <value>
        /// The name of the tank.
        /// </value>
        public string TankName { get; set; }

        /// <summary>
        /// Gets or sets the source node.
        /// </summary>
        /// <value>
        /// The source node.
        /// </value>
        public string SourceNode { get; set; }

        /// <summary>
        /// Gets or sets the destination node.
        /// </summary>
        /// <value>
        /// The destination node.
        /// </value>
        public string DestinationNode { get; set; }

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
        /// Gets or sets the net volume.
        /// </summary>
        /// <value>
        /// The net volume.
        /// </value>
        public decimal? NetVolume { get; set; }

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets the unit identifier.
        /// </summary>
        /// <value>
        /// The unit identifier.
        /// </value>
        public int UnitId { get; set; }

        /// <summary>
        /// Gets or sets the ownership volume.
        /// </summary>
        /// <value>
        /// The ownership volume.
        /// </value>
        public decimal? OwnershipVolume { get; set; }

        /// <summary>
        /// Gets or sets the ownership percentage.
        /// </summary>
        /// <value>
        /// The ownership percentage.
        /// </value>
        public decimal? OwnershipPercentage { get; set; }

        /// <summary>
        /// Gets or sets the ownership function.
        /// </summary>
        /// <value>
        /// The ownership function.
        /// </value>
        public string OwnershipFunction { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public int OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the owner.
        /// </summary>
        /// <value>
        /// The name of the owner.
        /// </value>
        public string OwnerName { get; set; }

        /// <summary>
        /// Gets or sets the source product identifier.
        /// </summary>
        /// <value>
        /// The source product identifier.
        /// </value>
        public string SourceProductId { get; set; }

        /// <summary>
        /// Gets or sets the destination product identifier.
        /// </summary>
        /// <value>
        /// The destination product identifier.
        /// </value>
        public string DestinationProductId { get; set; }

        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        public int? SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination node identifier.
        /// </summary>
        /// <value>
        /// The destination node identifier.
        /// </value>
        public int? DestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the reason identifier.
        /// </summary>
        /// <value>
        /// The reason identifier.
        /// </value>
        public int? ReasonId { get; set; }

        /// <summary>
        /// Gets or sets the reason.
        /// </summary>
        /// <value>
        /// The reason.
        /// </value>
        public string Reason { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is movement.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is movement; otherwise, <c>false</c>.
        /// </value>
        public int IsMovement { get; set; }

        /// <summary>
        /// Gets or sets a value Variable Type Id.
        /// </summary>
        /// <value>
        /// The Comment.
        /// </value>
        public int VariableTypeId { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The Color.
        /// </value>
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the contract identifier.
        /// </summary>
        /// <value>
        /// The contract identifier.
        /// </value>
        public int? ContractId { get; set; }

        /// <summary>
        /// Gets or sets the movement contract identifier.
        /// </summary>
        /// <value>
        /// The movement contract identifier.
        /// </value>
        public int? MovementContractId { get; set; }

        /// <summary>
        /// Gets or sets the document number.
        /// </summary>
        /// <value>
        /// The document number.
        /// </value>
        public int? DocumentNumber { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public int? Position { get; set; }

        /// <summary>
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>
        public string MovementId { get; set; }

        /// <summary>
        /// Gets or sets the source movement identifier.
        /// </summary>
        /// <value>
        /// The source movement identifier.
        /// </value>
        public string SourceMovementId { get; set; }

        /// <summary>
        /// Gets or sets the movement type identifier.
        /// </summary>
        /// <value>
        /// The movement type identifier.
        /// </value>
        public int? MovementTypeId { get; set; }
    }
}