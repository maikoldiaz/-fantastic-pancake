using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class Node
    {
        public Node()
        {
            ContractDestinationNode = new HashSet<Contract>();
            ContractSourceNode = new HashSet<Contract>();
            EventDestinationNode = new HashSet<Event>();
            EventSourceNode = new HashSet<Event>();
            Inventory = new HashSet<Inventory>();
            InventoryProduct = new HashSet<InventoryProduct>();
            MovementContractDestinationNode = new HashSet<MovementContract>();
            MovementContractSourceNode = new HashSet<MovementContract>();
            MovementDestination = new HashSet<MovementDestination>();
            MovementEventDestinationNode = new HashSet<MovementEvent>();
            MovementEventSourceNode = new HashSet<MovementEvent>();
            MovementSource = new HashSet<MovementSource>();
            NodeConnectionDestinationNode = new HashSet<NodeConnection>();
            NodeConnectionSourceNode = new HashSet<NodeConnection>();
            NodeStorageLocation = new HashSet<NodeStorageLocation>();
            NodeTag = new HashSet<NodeTag>();
            OwnershipAnalyticsDestinationNode = new HashSet<OwnershipAnalytics>();
            OwnershipAnalyticsSourceNode = new HashSet<OwnershipAnalytics>();
            OwnershipCalculation = new HashSet<OwnershipCalculation>();
            OwnershipNode = new HashSet<OwnershipNode>();
            OwnershipResult = new HashSet<OwnershipResult>();
            Ticket = new HashSet<Ticket>();
            TransformationDestinationDestinationNode = new HashSet<Transformation>();
            TransformationDestinationSourceNode = new HashSet<Transformation>();
            TransformationOriginDestinationNode = new HashSet<Transformation>();
            TransformationOriginSourceNode = new HashSet<Transformation>();
            Unbalance = new HashSet<Unbalance>();
            UnbalanceComment = new HashSet<UnbalanceComment>();
        }

        public int NodeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogisticCenterId { get; set; }
        public bool? IsActive { get; set; }
        public bool SendToSap { get; set; }
        public decimal? ControlLimit { get; set; }
        public decimal? AcceptableBalancePercentage { get; set; }
        public int Order { get; set; }
        public int? UnitId { get; set; }
        public decimal? Capacity { get; set; }
        public int? NodeOwnershipRuleId { get; set; }
        public byte[] RowVersion { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual LogisticCenter LogisticCenter { get; set; }
        public virtual NodeOwnershipRule NodeOwnershipRule { get; set; }
        public virtual CategoryElement Unit { get; set; }
        public virtual ICollection<Contract> ContractDestinationNode { get; set; }
        public virtual ICollection<Contract> ContractSourceNode { get; set; }
        public virtual ICollection<Event> EventDestinationNode { get; set; }
        public virtual ICollection<Event> EventSourceNode { get; set; }
        public virtual ICollection<Inventory> Inventory { get; set; }
        public virtual ICollection<InventoryProduct> InventoryProduct { get; set; }
        public virtual ICollection<MovementContract> MovementContractDestinationNode { get; set; }
        public virtual ICollection<MovementContract> MovementContractSourceNode { get; set; }
        public virtual ICollection<MovementDestination> MovementDestination { get; set; }
        public virtual ICollection<MovementEvent> MovementEventDestinationNode { get; set; }
        public virtual ICollection<MovementEvent> MovementEventSourceNode { get; set; }
        public virtual ICollection<MovementSource> MovementSource { get; set; }
        public virtual ICollection<NodeConnection> NodeConnectionDestinationNode { get; set; }
        public virtual ICollection<NodeConnection> NodeConnectionSourceNode { get; set; }
        public virtual ICollection<NodeStorageLocation> NodeStorageLocation { get; set; }
        public virtual ICollection<NodeTag> NodeTag { get; set; }
        public virtual ICollection<OwnershipAnalytics> OwnershipAnalyticsDestinationNode { get; set; }
        public virtual ICollection<OwnershipAnalytics> OwnershipAnalyticsSourceNode { get; set; }
        public virtual ICollection<OwnershipCalculation> OwnershipCalculation { get; set; }
        public virtual ICollection<OwnershipNode> OwnershipNode { get; set; }
        public virtual ICollection<OwnershipResult> OwnershipResult { get; set; }
        public virtual ICollection<Ticket> Ticket { get; set; }
        public virtual ICollection<Transformation> TransformationDestinationDestinationNode { get; set; }
        public virtual ICollection<Transformation> TransformationDestinationSourceNode { get; set; }
        public virtual ICollection<Transformation> TransformationOriginDestinationNode { get; set; }
        public virtual ICollection<Transformation> TransformationOriginSourceNode { get; set; }
        public virtual ICollection<Unbalance> Unbalance { get; set; }
        public virtual ICollection<UnbalanceComment> UnbalanceComment { get; set; }
    }
}
