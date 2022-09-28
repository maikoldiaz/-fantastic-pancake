// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Ticket.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.TransportBalance
{
    using System;
    using System.Collections.Generic;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Newtonsoft.Json;

    /// <summary>
    /// The ticket entity.
    /// </summary>
    /// <seealso cref="Entity" />
    public class Ticket : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ticket"/> class.
        /// </summary>
        public Ticket()
        {
            this.Movements = new List<Movement>();
            this.MovementOwnerships = new List<Movement>();
            this.InventoryProducts = new List<InventoryProduct>();
            this.InventoryOwnerships = new List<InventoryProduct>();
            this.PendingTransactions = new List<PendingTransaction>();
            this.Unbalances = new List<Unbalance>();
            this.UnbalanceComments = new List<UnbalanceComment>();
            this.Ownerships = new List<Ownership>();
            this.OwnershipCalculations = new List<OwnershipCalculation>();
            this.OwnershipNodes = new List<OwnershipNode>();
            this.DeltaMovements = new List<Movement>();
            this.DeltaInventoryProducts = new List<InventoryProduct>();
            this.Initialize();
        }

        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public int TicketId { get; set; }

        /// <summary>
        /// Gets or sets the category element identifier.
        /// </summary>
        /// <value>
        /// The category element identifier.
        /// </value>
        public int CategoryElementId { get; set; }

        /// <summary>
        /// Gets or sets the BLOB path.
        /// </summary>
        /// <value>
        /// The BLOB path.
        /// </value>
        public string BlobPath { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public int? OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        /// <value>
        /// The end time.
        /// </value>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Ticket"/> is status.
        /// </summary>
        /// <value>
        ///   <c>true</c> if status; otherwise, <c>false</c>.
        /// </value>
        public StatusType Status { get; set; }

        /// <summary>
        /// Gets or sets the ticket type identifier.
        /// </summary>
        /// <value>
        /// The ticket type identifier.
        /// </value>
        public TicketType TicketTypeId { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        ///   The error message.
        /// </value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        public int? NodeId { get; set; }

        /// <summary>
        /// Gets or sets the Scenario Type Id identifier.
        /// </summary>
        /// <value>
        /// The Scenario Type Id identifier.
        /// </value>
        public ScenarioType? ScenarioTypeId { get; set; }

        /// <summary>
        /// Gets or sets the ticket group identifier.
        /// </summary>
        /// <value>
        /// The ticket group identifier.
        /// </value>
        public string TicketGroupId { get; set; }

        /// <summary>
        /// Gets or sets the category element.
        /// </summary>
        /// <value>
        /// The category element.
        /// </value>
        public virtual CategoryElement CategoryElement { get; set; }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>
        /// The owner.
        /// </value>
        public virtual CategoryElement Owner { get; set; }

        /// <summary>
        /// Gets or sets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        public virtual Node Node { get; set; }

        /// <summary>
        /// Gets the movements.
        /// </summary>
        /// <value>
        /// The movements.
        /// </value>
        public virtual ICollection<Movement> Movements { get; }

        /// <summary>
        /// Gets the movement ownerships.
        /// </summary>
        /// <value>
        /// The movement ownerships.
        /// </value>
        public virtual ICollection<Movement> MovementOwnerships { get; }

        /// <summary>
        /// Gets the inventory products.
        /// </summary>
        /// <value>
        /// The inventory products.
        /// </value>
        public virtual ICollection<InventoryProduct> InventoryProducts { get; }

        /// <summary>
        /// Gets the inventory products.
        /// </summary>
        /// <value>
        /// The inventory products.
        /// </value>
        public virtual ICollection<InventoryProduct> InventoryOwnerships { get; }

        /// <summary>
        /// Gets the pending transactions.
        /// </summary>
        /// <value>
        /// The pending transactions.
        /// </value>
        public virtual ICollection<PendingTransaction> PendingTransactions { get; private set; }

        /// <summary>
        /// Gets the unbalances.
        /// </summary>
        /// <value>
        /// The unbalances.
        /// </value>
        [JsonIgnore]
        public virtual ICollection<Unbalance> Unbalances { get; }

        /// <summary>
        /// Gets the unbalance comments.
        /// </summary>
        /// <value>
        /// The unbalance comments.
        /// </value>
        public virtual ICollection<UnbalanceComment> UnbalanceComments { get; }

        /// <summary>
        /// Gets the ownerships.
        /// </summary>
        /// <value>
        /// The ownerships.
        /// </value>
        public virtual ICollection<Ownership> Ownerships { get; private set; }

        /// <summary>
        /// Gets the ownership calculations.
        /// </summary>
        /// <value>
        /// The ownership calculations.
        /// </value>
        public virtual ICollection<OwnershipCalculation> OwnershipCalculations { get; private set; }

        /// <summary>
        /// Gets the ownership nodes.
        /// </summary>
        /// <value>
        /// The ownership nodes.
        /// </value>
        public virtual ICollection<OwnershipNode> OwnershipNodes { get; private set; }

        /// <summary>
        /// Gets the segment ownership calculations.
        /// </summary>
        /// <value>
        /// The segment ownership calculations.
        /// </value>
        public virtual ICollection<SegmentOwnershipCalculation> SegmentOwnershipCalculations { get; private set; }

        /// <summary>
        /// Gets the system ownership calculations.
        /// </summary>
        /// <value>
        /// The system ownership calculations.
        /// </value>
        public virtual ICollection<SystemOwnershipCalculation> SystemOwnershipCalculations { get; private set; }

        /// <summary>
        /// Gets the segment unbalance.
        /// </summary>
        /// <value>
        /// The segment unbalance.
        /// </value>
        public virtual ICollection<SegmentUnbalance> SegmentUnbalances { get; private set; }

        /// <summary>
        /// Gets the system unbalance.
        /// </summary>
        /// <value>
        /// The system unbalance.
        /// </value>
        public virtual ICollection<SystemUnbalance> SystemUnbalances { get; private set; }

        /// <summary>
        /// Gets the delta error.
        /// </summary>
        /// <value>
        /// The delta error.
        /// </value>
        public virtual ICollection<DeltaError> DeltaErrors { get; private set; }

        /// <summary>
        /// Gets the delta movements.
        /// </summary>
        /// <value>
        /// The delta movements.
        /// </value>
        public virtual ICollection<Movement> DeltaMovements { get; private set; }

        /// <summary>
        /// Gets the delta inventory products.
        /// </summary>
        /// <value>
        /// The delta inventory products.
        /// </value>
        public virtual ICollection<InventoryProduct> DeltaInventoryProducts { get; private set; }

        /// <summary>
        /// Gets the delta ownerships.
        /// </summary>
        /// <value>
        /// The delta ownerships.
        /// </value>
        public virtual ICollection<Ownership> DeltaOwnerships { get; private set; }

        /// <summary>
        /// Gets the consolidated movements.
        /// </summary>
        /// <value>
        /// The consolidated movements.
        /// </value>
        public virtual ICollection<ConsolidatedMovement> ConsolidatedMovements { get; private set; }

        /// <summary>
        /// Gets the consolidated inventory products.
        /// </summary>
        /// <value>
        /// The consolidated inventory products.
        /// </value>
        public virtual ICollection<ConsolidatedInventoryProduct> ConsolidatedInventoryProducts { get; private set; }

        /// <summary>
        /// Gets the delta nodes.
        /// </summary>
        /// <value>
        /// The delta nodes.
        /// </value>
        public virtual ICollection<DeltaNode> DeltaNodes { get; private set; }

        /// <summary>
        /// Gets the official delta movements.
        /// </summary>
        /// <value>
        /// The official delta movements.
        /// </value>
        public virtual ICollection<Movement> OfficialDeltaMovements { get; private set; }

        /// <summary>
        /// Gets the official delta inventory products.
        /// </summary>
        /// <value>
        /// The official delta inventory products.
        /// </value>
        public virtual ICollection<InventoryProduct> OfficialDeltaInventoryProducts { get; private set; }

        /// <summary>
        /// Gets the official delta inventory products.
        /// </summary>
        /// <value>
        /// The official delta inventory products.
        /// </value>
        public virtual ICollection<DeltaNodeApprovalHistory> DeltaNodeApprovalHistories { get; private set; }

        /// <summary>
        /// Gets or Sets the TicketNodes to Tickets.
        /// </summary>
        /// <value>
        /// The TicketNodes to Tickets.
        /// </value>
        public IEnumerable<TicketNode> TicketNodes { get; set; }

        /// <summary>
        /// Gets the LogisticMovements to Tickets.
        /// </summary>
        /// <value>
        /// The LogisticMovements to Tickets.
        /// </value>
        public virtual ICollection<LogisticMovement> LogisticMovements { get; private set; }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            this.SegmentOwnershipCalculations = new List<SegmentOwnershipCalculation>();
            this.SystemOwnershipCalculations = new List<SystemOwnershipCalculation>();
            this.SegmentUnbalances = new List<SegmentUnbalance>();
            this.SystemUnbalances = new List<SystemUnbalance>();
            this.DeltaErrors = new List<DeltaError>();
            this.DeltaOwnerships = new List<Ownership>();
            this.ConsolidatedMovements = new List<ConsolidatedMovement>();
            this.ConsolidatedInventoryProducts = new List<ConsolidatedInventoryProduct>();
            this.DeltaNodes = new List<DeltaNode>();
            this.OfficialDeltaMovements = new List<Movement>();
            this.OfficialDeltaInventoryProducts = new List<InventoryProduct>();
            this.DeltaNodeApprovalHistories = new List<DeltaNodeApprovalHistory>();
            this.TicketNodes = new List<TicketNode>();
        }
    }
}