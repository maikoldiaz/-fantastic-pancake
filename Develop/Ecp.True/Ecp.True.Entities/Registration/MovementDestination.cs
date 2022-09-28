// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementDestination.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Registration
{
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The movemement Destination.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class MovementDestination : Entity
    {
        /// <summary>
        /// Gets or sets the movement destination identifier.
        /// </summary>
        /// <value>
        /// The movement destination identifier.
        /// </value>
        public int MovementDestinationId { get; set; }

        /// <summary>
        /// Gets or sets the movement transaction identifier.
        /// </summary>
        /// <value>
        /// The movement transaction identifier.
        /// </value>
        public int MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the destination node identifier.
        /// </summary>
        /// <value>
        /// The destination node identifier.
        /// </value>
        [Required(ErrorMessage = Constants.NodeConnectionDestinationNodeIdRequired)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.NodeConnectionDestinationNodeIdRequired)]
        public int? DestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination storage location identifier.
        /// </summary>
        /// <value>
        /// The destination storage location identifier.
        /// </value>
        public int? DestinationStorageLocationId { get; set; }

        /// <summary>
        /// Gets or sets the destination product identifier.
        /// </summary>
        /// <value>
        /// The destination product identifier.
        /// </value>
        public string DestinationProductId { get; set; }

        /// <summary>
        /// Gets or sets the destination product type identifier.
        /// </summary>
        /// <value>
        /// The destination product type identifier.
        /// </value>
        public int? DestinationProductTypeId { get; set; }

        /// <summary>
        /// Gets or sets the destination node.
        /// </summary>
        /// <value>
        /// The destination node.
        /// </value>
        public virtual Node DestinationNode { get; set; }

        /// <summary>
        /// Gets or sets the destination product.
        /// </summary>
        /// <value>
        /// The destination product.
        /// </value>
        public virtual Product DestinationProduct { get; set; }

        /// <summary>
        /// Gets or sets the destination storage location.
        /// </summary>
        /// <value>
        /// The destination storage location.
        /// </value>
        public virtual NodeStorageLocation DestinationStorageLocation { get; set; }

        /// <summary>
        /// Gets or sets the movement transaction.
        /// </summary>
        /// <value>
        /// The movement transaction.
        /// </value>
        public virtual Movement MovementTransaction { get; set; }

        /// <summary>
        /// Gets or sets the destination product type.
        /// </summary>
        /// <value>
        /// The destination product type.
        /// </value>
        public virtual CategoryElement DestinationProductType { get; set; }

        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            True.Core.ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var element = (MovementDestination)entity;

            this.DestinationNodeId = element.DestinationNodeId ?? this.DestinationNodeId;
            this.DestinationStorageLocationId = element.DestinationStorageLocationId ?? this.DestinationStorageLocationId;
            this.DestinationProductId = element.DestinationProductId ?? this.DestinationProductId;
            this.DestinationProductTypeId = element.DestinationProductTypeId ?? this.DestinationProductTypeId;
        }
    }
}
