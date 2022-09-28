// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Ownership.cs" company="Microsoft">
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
    using System;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// Ownership entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class Ownership : BlockchainEntity
    {
        /// <summary>
        /// Gets or sets the ownership identifier.
        /// </summary>
        /// <value>
        /// The ownership identifier.
        /// </value>
        public int OwnershipId { get; set; }

        /// <summary>
        /// Gets or sets the message type identifier.
        /// </summary>
        /// <value>
        /// The message type identifier.
        /// </value>
        public MessageType MessageTypeId { get; set; }

        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public int TicketId { get; set; }

        /// <summary>
        /// Gets or sets the movement transaction identifier.
        /// </summary>
        /// <value>
        /// The movement transaction identifier.
        /// </value>
        public int? MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the inventory transaction identifier.
        /// </summary>
        /// <value>
        /// The inventory transaction identifier.
        /// </value>
        public int? InventoryProductId { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public int OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the ownership percentage.
        /// </summary>
        /// <value>
        /// The ownership percentage.
        /// </value>
        public decimal OwnershipPercentage { get; set; }

        /// <summary>
        /// Gets or sets the ownership volume.
        /// </summary>
        /// <value>
        /// The ownership volume.
        /// </value>
        public decimal OwnershipVolume { get; set; }

        /// <summary>
        /// Gets or sets the applied rule.
        /// </summary>
        /// <value>
        /// The applied rule.
        /// </value>
        public string AppliedRule { get; set; }

        /// <summary>
        /// Gets or sets the rule version.
        /// </summary>
        /// <value>
        /// The rule version.
        /// </value>
        public string RuleVersion { get; set; }

        /// <summary>
        /// Gets or sets the execution date.
        /// </summary>
        /// <value>
        /// The execution date.
        /// </value>
        public DateTime ExecutionDate { get; set; }

        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        /// <value>
        /// The type of the event.
        /// </value>
        public string EventType { get; set; }

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
        /// Gets or sets the blockchain ownership identifier.
        /// </summary>
        /// <value>
        /// The blockchain ownership identifier.
        /// </value>
        public Guid? BlockchainOwnershipId { get; set; }

        /// <summary>
        /// Gets or sets the previous blockchain ownership identifier.
        /// </summary>
        /// <value>
        /// The previous blockchain ownership identifier.
        /// </value>
        public Guid? PreviousBlockchainOwnershipId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the delta ticket identifier.
        /// </summary>
        /// <value>
        /// The delta ticket identifier.
        /// </value>
        public int? DeltaTicketId { get; set; }

        /// <summary>
        /// Gets or sets the inventory transaction.
        /// </summary>
        /// <value>
        /// The inventory transaction.
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
        public virtual CategoryElement Owner { get; set; }

        /// <summary>
        /// Gets or sets the ticket.
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        public virtual Ticket Ticket { get; set; }

        /// <summary>
        /// Gets or sets the delta ticket.
        /// </summary>
        /// <value>
        /// The delta ticket.
        /// </value>
        public virtual Ticket DeltaTicket { get; set; }

        /// <summary>
        /// Copies from.
        /// </summary>
        /// <param name="entity">The new entity.</param>
        public override void CopyFrom(IEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));

            Ownership ownership = (Ownership)entity;
            this.OwnershipVolume = ownership.OwnershipVolume;
            this.OwnershipPercentage = ownership.OwnershipPercentage;
        }
    }
}
