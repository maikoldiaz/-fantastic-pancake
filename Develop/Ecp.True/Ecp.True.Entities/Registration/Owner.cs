// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Owner.cs" company="Microsoft">
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
    using System;
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The Owner class.
    /// </summary>
    public class Owner : BlockchainEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [Required(ErrorMessage = Constants.OwnerIdRequired)]
        public int OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the ownership value.
        /// </summary>
        /// <value>
        /// The ownership value.
        /// </value>
        [Required(ErrorMessage = Constants.OwnershipValueRequired)]
        public decimal? OwnershipValue { get; set; }

        /// <summary>
        /// Gets or sets the ownership value unit.
        /// </summary>
        /// <value>
        /// The ownership value unit.
        /// </value>
        [Required(ErrorMessage = Constants.OwnershipValueUnitRequired)]
        public string OwnershipValueUnit { get; set; }

        /// <summary>
        /// Gets or sets the inventory product identifier.
        /// </summary>
        /// <value>
        /// The inventory product identifier.
        /// </value>
        public int? InventoryProductId { get; set; }

        /// <summary>
        /// Gets or sets the movement transaction identifier.
        /// </summary>
        /// <value>
        /// The movement transaction identifier.
        /// </value>
        public int? MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the blockchain movement transaction identifier.
        /// </summary>
        /// <value>
        /// The blockchain movement transaction identifier.
        /// </value>
        public Guid? BlockchainMovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the blockchain inventory product transaction identifier.
        /// </summary>
        /// <value>
        /// The blockchain inventory product transaction identifier.
        /// </value>
        public Guid? BlockchainInventoryProductTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the inventory product.
        /// </summary>
        /// <value>
        /// The inventory product.
        /// </value>
        public virtual InventoryProduct InventoryProduct { get; set; }

        /// <summary>
        /// Gets or sets the movement transaction.
        /// </summary>
        /// <value>
        /// The movement transaction.
        /// </value>
        public virtual Movement MovementTransaction { get; set; }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>
        /// The owner.
        /// </value>
        public virtual CategoryElement OwnerElement { get; set; }

        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            True.Core.ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var element = (Owner)entity;

            this.OwnerId = element.OwnerId;
            this.OwnershipValue = element.OwnershipValue;
            this.OwnershipValueUnit = element.OwnershipValueUnit;
            this.MovementTransactionId = element.MovementTransactionId;
            this.InventoryProductId = element.InventoryProductId;
        }
    }
}