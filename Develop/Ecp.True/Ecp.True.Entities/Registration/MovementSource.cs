// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementSource.cs" company="Microsoft">
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
    /// The movement source.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class MovementSource : Entity
    {
        /// <summary>
        /// Gets or sets the movement source identifier.
        /// </summary>
        /// <value>
        /// The movement source identifier.
        /// </value>
        public int MovementSourceId { get; set; }

        /// <summary>
        /// Gets or sets the movement transaction identifier.
        /// </summary>
        /// <value>
        /// The movement transaction identifier.
        /// </value>
        public int MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        [Required(ErrorMessage = Constants.NodeConnectionSourceNodeIdRequired)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.NodeConnectionSourceNodeIdRequired)]
        public int? SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the source storage location identifier.
        /// </summary>
        /// <value>
        /// The source storage location identifier.
        /// </value>
        public int? SourceStorageLocationId { get; set; }

        /// <summary>
        /// Gets or sets the source product identifier.
        /// </summary>
        /// <value>
        /// The source product identifier.
        /// </value>
        [Required(ErrorMessage = Constants.SourceProductIdRequired)]
        public string SourceProductId { get; set; }

        /// <summary>
        /// Gets or sets the source product type identifier.
        /// </summary>
        /// <value>
        /// The source product type identifier.
        /// </value>
        public int? SourceProductTypeId { get; set; }

        /// <summary>
        /// Gets or sets the movement transaction.
        /// </summary>
        /// <value>
        /// The movement transaction.
        /// </value>
        public virtual Movement MovementTransaction { get; set; }

        /// <summary>
        /// Gets or sets the source node.
        /// </summary>
        /// <value>
        /// The source node.
        /// </value>
        public virtual Node SourceNode { get; set; }

        /// <summary>
        /// Gets or sets the source product.
        /// </summary>
        /// <value>
        /// The source product.
        /// </value>
        public virtual Product SourceProduct { get; set; }

        /// <summary>
        /// Gets or sets the source storage location.
        /// </summary>
        /// <value>
        /// The source storage location.
        /// </value>
        public virtual NodeStorageLocation SourceStorageLocation { get; set; }

        /// <summary>
        /// Gets or sets the source product type.
        /// </summary>
        /// <value>
        /// The source product type.
        /// </value>
        public virtual CategoryElement SourceProductType { get; set; }

        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            True.Core.ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var element = (MovementSource)entity;

            this.SourceNodeId = element.SourceNodeId ?? this.SourceNodeId;
            this.SourceStorageLocationId = element.SourceStorageLocationId ?? this.SourceStorageLocationId;
            this.SourceProductId = element.SourceProductId ?? this.SourceProductId;
            this.SourceProductTypeId = element.SourceProductTypeId ?? this.SourceProductTypeId;
        }
    }
}
