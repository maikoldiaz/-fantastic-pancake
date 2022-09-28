// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageLocationProductOwner.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Admin
{
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;

    /// <summary>
    ///     The Storage Location Product Owner.
    /// </summary>
    public class StorageLocationProductOwner : AuditableEntity
    {
        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public int StorageLocationProductOwnerId { get; set; }

        /// <summary>
        /// Gets or sets the ownership percentage.
        /// </summary>
        /// <value>
        /// The Ownership Percentage.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.ProductOwnerShipRequired)]
        public decimal? OwnershipPercentage { get; set; }

        /// <summary>
        /// Gets or sets the storage location product identifier.
        /// </summary>
        /// <value>
        /// The node StorageLocationProductId.
        /// </value>
        public int StorageLocationProductId { get; set; }

        /// <summary>Gets or sets the owner identifier.</summary>
        /// <value>The owner identifier.</value>
        [Required(ErrorMessage = Entities.Constants.NodeConnectionProductOwnerRequired)]
        public int? OwnerId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the storage location product.
        /// </summary>
        /// <value>
        /// The StorageLocationProduct.
        /// </value>
        [ColumnIgnore]
        public virtual StorageLocationProduct StorageLocationProduct { get; set; }

        /// <summary>Gets or sets the owner.</summary>
        /// <value>The owner.</value>
        [ColumnIgnore]
        public virtual CategoryElement Owner { get; set; }

        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));

            var owner = (StorageLocationProductOwner)entity;
            this.OwnershipPercentage = owner.OwnershipPercentage ?? this.OwnershipPercentage;
        }
    }
}
