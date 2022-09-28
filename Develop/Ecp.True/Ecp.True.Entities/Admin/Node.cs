// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Node.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using EfCore.Models;
    using Newtonsoft.Json;

    /// <summary>
    ///     The node.
    /// </summary>
    public class Node : EditableEntity
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Node" /> class.
        /// </summary>
        public Node()
        {
            this.NodeStorageLocations = new List<NodeStorageLocation>();
            this.InventoryProducts = new List<InventoryProduct>();
            this.NodeTags = new List<NodeTag>();
            this.UnbalanceComments = new List<UnbalanceComment>();
            this.Unbalances = new List<Unbalance>();
            this.MovementSources = new List<MovementSource>();
            this.MovementDestinations = new List<MovementDestination>();
            this.SourceConnections = new List<NodeConnection>();
            this.DestinationConnections = new List<NodeConnection>();
            this.Initialize();
        }

        /// <summary>
        ///     Gets or sets the node identifier.
        /// </summary>
        /// <value>
        ///     The node identifier.
        /// </value>
        public int NodeId { get; set; }

        /// <summary>
        ///     Gets or sets the name of the node.
        /// </summary>
        /// <value>
        ///     The name of the node.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.NodeNameRequired)]
        [MaxLength(150, ErrorMessage = Entities.Constants.NodeMax150Characters)]
        [RegularExpression(Entities.Constants.AllowNumbersAndLettersWithSpecialCharactersWithSpaceRegex, ErrorMessage = Entities.Constants.NodeNameSpecialCharactersMessage)]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the node description.
        /// </summary>
        /// <value>
        ///     The node description.
        /// </value>
        [MaxLength(1000, ErrorMessage = Entities.Constants.Max1000Characters)]
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.NodeMustBeActive)]
        public bool? IsActive { get; set; }

        /// <summary>
        ///     Gets or sets the node type identifier.
        /// </summary>
        /// <value>
        ///     The node type identifier.
        /// </value>
        public int? NodeTypeId { get; set; }

        /// <summary>
        ///     Gets or sets the node type.
        /// </summary>
        /// <value>
        ///     The node type.
        /// </value>
        public virtual CategoryElement NodeType { get; set; }

        /// <summary>
        ///     Gets or sets the segment identifier.
        /// </summary>
        /// <value>
        ///     The segment identifier.
        /// </value>
        public int? SegmentId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the node is to be auto ordered.
        /// </summary>
        /// <value>
        ///     Whether the node is to be auto ordered.
        /// </value>
        [ColumnIgnore]
        public bool AutoOrder { get; set; }

        /// <summary>
        ///     Gets or sets the segment.
        /// </summary>
        /// <value>
        ///     The segment.
        /// </value>
        public virtual CategoryElement Segment { get; set; }

        /// <summary>
        ///     Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        ///     The operator identifier.
        /// </value>
        public int? OperatorId { get; set; }

        /// <summary>
        ///     Gets or sets the order.
        /// </summary>
        /// <value>
        ///     The order.
        /// </value>
        [Range(1, int.MaxValue, ErrorMessage = Entities.Constants.DecimalValidationMessage)]
        public int? Order { get; set; }

        /// <summary>
        ///     Gets or sets the operator.
        /// </summary>
        /// <value>
        ///     The operator.
        /// </value>
        public virtual CategoryElement Operator { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [send to SAP].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [send to SAP]; otherwise, <c>false</c>.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.NodeSendToSapRequired)]
        public bool? SendToSap { get; set; }

        /// <summary>
        ///     Gets or sets the SAP code.
        /// </summary>
        /// <value>
        ///     The SAP code.
        /// </value>
        [RequiredIf("SendToSap", true, ErrorMessage = Entities.Constants.SapCodeRequired)]
        public string LogisticCenterId { get; set; }

        /// <summary>
        /// Gets or sets the control limit.
        /// </summary>
        /// <value>
        /// The Control Limit.
        /// </value>
        public decimal? ControlLimit { get; set; }

        /// <summary>
        /// Gets or sets the acceptable balance percentage.
        /// </summary>
        /// <value>
        /// The Acceptable Balance Percentage.
        /// </value>
        public decimal? AcceptableBalancePercentage { get; set; }

        /// <summary>
        /// Gets or sets the unitId.
        /// </summary>
        /// <value>
        /// The Unit.
        /// </value>
        public int? UnitId { get; set; }

        /// <summary>
        /// Gets or sets the capacity.
        /// </summary>
        /// <value>
        /// The Capacity.
        /// </value>
        public decimal? Capacity { get; set; }

        /// <summary>
        /// Gets or sets the node ownership rule identifier.
        /// </summary>
        /// <value>
        /// The node ownership rule identifier.
        /// </value>
        public int? NodeOwnershipRuleId { get; set; }

        /// <summary>
        /// Gets or sets the node Is Exportation.
        /// </summary>
        /// <value>
        /// The node Is Exportation.
        /// </value>
        public bool? IsExportation { get; set; }

        /// <summary>
        /// Gets or sets the logistic center.
        /// </summary>
        /// <value>
        /// The logistic center.
        /// </value>
        public virtual LogisticCenter LogisticCenter { get; set; }

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>
        /// The units.
        /// </value>
        public virtual CategoryElement Unit { get; set; }

        /// <summary>
        /// Gets or sets the node ownership rule.
        /// </summary>
        /// <value>
        /// The node ownership rule.
        /// </value>
        public virtual NodeOwnershipRule NodeOwnershipRule { get; set; }

        /// <summary>
        ///     Gets the node storage locations.
        /// </summary>
        /// <value>
        ///     The node storage locations.
        /// </value>
        [MustNotBeEmptyIf("IsActive", true, ErrorMessage = Entities.Constants.NodeShouldHaveAtleastOneStore)]
        public ICollection<NodeStorageLocation> NodeStorageLocations { get; private set; }

        /// <summary>
        /// Gets the inventory.
        /// </summary>
        /// <value>
        /// The inventory.
        /// </value>
        public virtual ICollection<InventoryProduct> InventoryProducts { get; private set; }

        /// <summary>
        /// Gets the source connection.
        /// </summary>
        /// <value>
        /// The source connection.
        /// </value>
        [JsonIgnore]
        public virtual ICollection<NodeConnection> SourceConnections { get; private set; }

        /// <summary>
        /// Gets the destination connection.
        /// </summary>
        /// <value>
        /// The destination connection.
        /// </value>
        [JsonIgnore]
        public virtual ICollection<NodeConnection> DestinationConnections { get; private set; }

        /// <summary>
        /// Gets the source movements.
        /// </summary>
        /// <value>
        /// The Source Movements.
        /// </value>
        [JsonIgnore]
        public virtual ICollection<MovementSource> MovementSources { get; private set; }

        /// <summary>
        /// Gets the destination movements.
        /// </summary>
        /// <value>
        /// The Destination Movements.
        /// </value>
        [JsonIgnore]
        public virtual ICollection<MovementDestination> MovementDestinations { get; private set; }

        /// <summary>
        /// Gets the node category tagging.
        /// </summary>
        /// <value>
        /// The node category tagging.
        /// </value>
        public virtual ICollection<NodeTag> NodeTags { get; private set; }

        /// <summary>
        /// Gets the unbalance data.
        /// </summary>
        /// <value>
        /// The unbalance data.
        /// </value>
        [JsonIgnore]
        public virtual ICollection<UnbalanceComment> UnbalanceComments { get; private set; }

        /// <summary>
        /// Gets the unbalances.
        /// </summary>
        /// <value>
        /// The unbalances.
        /// </value>
        [JsonIgnore]
        public virtual ICollection<Unbalance> Unbalances { get; private set; }

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
        /// Gets the ownership results.
        /// </summary>
        /// <value>
        /// The ownership results.
        /// </value>
        public virtual ICollection<OwnershipResult> OwnershipResults { get; private set; }

        /// <summary>
        /// Gets the transformation destination destination nodes.
        /// </summary>
        /// <value>
        /// The transformation destination destination nodes.
        /// </value>
        public virtual ICollection<Transformation> DestinationDestinationNodeTransformations { get; private set; }

        /// <summary>
        /// Gets the transformation destination source nodes.
        /// </summary>
        /// <value>
        /// The transformation destination source nodes.
        /// </value>
        public virtual ICollection<Transformation> DestinationSourceNodeTransformations { get; private set; }

        /// <summary>
        /// Gets the transformation origin destination nodes.
        /// </summary>
        /// <value>
        /// The transformation origin destination nodes.
        /// </value>
        public virtual ICollection<Transformation> OriginDestinationNodeTransformations { get; private set; }

        /// <summary>
        /// Gets the transformation origin source nodes.
        /// </summary>
        /// <value>
        /// The transformation origin source nodes.
        /// </value>
        public virtual ICollection<Transformation> OriginSourceNodeTransformations { get; private set; }

        /// <summary>
        /// Gets the destination events.
        /// </summary>
        /// <value>
        /// The destination events.
        /// </value>
        public virtual ICollection<Event> DestinationEvents { get; private set; }

        /// <summary>
        /// Gets the source events.
        /// </summary>
        /// <value>
        /// The source events.
        /// </value>
        public virtual ICollection<Event> SourceEvents { get; private set; }

        /// <summary>
        /// Gets the destination contracts.
        /// </summary>
        /// <value>
        /// The destination contracts.
        /// </value>
        public virtual ICollection<Contract> DestinationContracts { get; private set; }

        /// <summary>
        /// Gets the source contracts.
        /// </summary>
        /// <value>
        /// The source contracts.
        /// </value>
        public virtual ICollection<Contract> SourceContracts { get; private set; }

        /// <summary>
        /// Gets the tickets.
        /// </summary>
        /// <value>
        /// The tickets.
        /// </value>
        public virtual ICollection<Ticket> Tickets { get; private set; }

        /// <summary>
        /// Gets the source movement contracts.
        /// </summary>
        /// <value>
        /// The source movement contracts.
        /// </value>
        public virtual ICollection<MovementContract> SourceMovementContracts { get; private set; }

        /// <summary>
        /// Gets the destination movement contracts.
        /// </summary>
        /// <value>
        /// The destination movement contracts.
        /// </value>
        public virtual ICollection<MovementContract> DestinationMovementContracts { get; private set; }

        /// <summary>
        /// Gets the source movement events.
        /// </summary>
        /// <value>
        /// The source movement events.
        /// </value>
        public virtual ICollection<MovementEvent> SourceMovementEvents { get; private set; }

        /// <summary>
        /// Gets the destination movement events.
        /// </summary>
        /// <value>
        /// The destination movement events.
        /// </value>
        public virtual ICollection<MovementEvent> DestinationMovementEvents { get; private set; }

        /// <summary>
        /// Gets the offchain nodes.
        /// </summary>
        /// <value>
        /// The offchain nodes.
        /// </value>
        public virtual ICollection<OffchainNode> OffchainNodes { get; private set; }

        /// <summary>
        /// Gets the source consolidated movements.
        /// </summary>
        /// <value>
        /// The source consolidated movements.
        /// </value>
        public virtual ICollection<ConsolidatedMovement> SourceConsolidatedMovements { get; private set; }

        /// <summary>
        /// Gets the destination consolidated movements.
        /// </summary>
        /// <value>
        /// The destination consolidated movements.
        /// </value>
        public virtual ICollection<ConsolidatedMovement> DestinationConsolidatedMovements { get; private set; }

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
        /// Gets the delta node approvals.
        /// </summary>
        /// <value>
        /// The delta node approvals.
        /// </value>
        public virtual ICollection<DeltaNodeApproval> DeltaNodeApprovals { get; private set; }

        /// <summary>
        /// Gets the report executions.
        /// </summary>
        /// <value>
        /// The report executions.
        /// </value>
        public virtual ICollection<ReportExecution> ReportExecutions { get; private set; }

        /// <summary>
        /// Gets the source connection.
        /// </summary>
        /// <value>
        /// The source connection.
        /// </value>
        public virtual ICollection<NodeCostCenter> SourceNodeCostCenter { get; private set; }

        /// <summary>
        /// Gets the source connection.
        /// </summary>
        /// <value>
        /// The source connection.
        /// </value>
        public virtual ICollection<NodeCostCenter> DestinationNodeCostCenter { get; private set; }

        /// <summary>
        /// Gets the Nodo identifier.
        /// </summary>
        /// <value>
        /// The Nodo identifier.
        /// </value>
        public virtual ICollection<TicketNode> TicketNodes { get; private set; }

        /// <summary>
        /// Copies from.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var node = (Node)entity;

            this.AcceptableBalancePercentage = node.AcceptableBalancePercentage ?? this.AcceptableBalancePercentage;
            this.ControlLimit = node.ControlLimit ?? this.ControlLimit;
            this.Description = node.Description ?? this.Description;
            this.IsActive = node.IsActive ?? this.IsActive;
            this.LogisticCenterId = node.LogisticCenterId;
            this.SendToSap = node.SendToSap ?? this.SendToSap;
            this.Name = node.Name ?? this.Name;
            this.Order = node.Order ?? this.Order;
            this.RowVersion = node.RowVersion;
            this.DoCopy(entity);
            if (this.NodeStorageLocations != null)
            {
                this.NodeStorageLocations.Merge(node.NodeStorageLocations, o => o.NodeStorageLocationId);
            }

            this.ValidateNodeGroupingData(node);
        }

        /// <summary>
        /// Initializes the node storage locations.
        /// </summary>
        public void ClearNodeStorageLocations()
        {
            this.NodeStorageLocations = null;
        }

        /// <summary>
        /// Incrementing the order Identifier.
        /// </summary>
        public void IncrementOrder()
        {
            this.Order += 1;
        }

        private void DoCopy(IEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var node = (Node)entity;

            this.Capacity = node.Capacity;
            this.UnitId = node.UnitId;
        }

        /// <summary>
        /// Validates the node grouping data.
        /// </summary>
        /// <param name="node">The node.</param>
        private void ValidateNodeGroupingData(Node node)
        {
            this.NodeTypeId = node.NodeTypeId ?? this.NodeTypeId;
            this.SegmentId = node.SegmentId ?? this.SegmentId;
            this.OperatorId = node.OperatorId ?? this.OperatorId;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            this.OwnershipCalculations = new List<OwnershipCalculation>();
            this.OwnershipNodes = new List<OwnershipNode>();
            this.OwnershipResults = new List<OwnershipResult>();
            this.DestinationDestinationNodeTransformations = new List<Transformation>();
            this.DestinationSourceNodeTransformations = new List<Transformation>();
            this.OriginDestinationNodeTransformations = new List<Transformation>();
            this.OriginSourceNodeTransformations = new List<Transformation>();
            this.DestinationEvents = new List<Event>();
            this.SourceEvents = new List<Event>();
            this.DestinationContracts = new List<Contract>();
            this.SourceContracts = new List<Contract>();
            this.Tickets = new List<Ticket>();
            this.InitializeMore();
        }

        /// <summary>
        /// Initializes the more.
        /// </summary>
        private void InitializeMore()
        {
            this.SourceMovementContracts = new List<MovementContract>();
            this.DestinationMovementContracts = new List<MovementContract>();
            this.SourceMovementEvents = new List<MovementEvent>();
            this.DestinationMovementEvents = new List<MovementEvent>();
            this.OffchainNodes = new List<OffchainNode>();
            this.SourceConsolidatedMovements = new List<ConsolidatedMovement>();
            this.DestinationConsolidatedMovements = new List<ConsolidatedMovement>();
            this.ConsolidatedInventoryProducts = new List<ConsolidatedInventoryProduct>();
            this.DeltaNodes = new List<DeltaNode>();
            this.DeltaNodeApprovals = new List<DeltaNodeApproval>();
            this.ReportExecutions = new List<ReportExecution>();
        }
    }
}