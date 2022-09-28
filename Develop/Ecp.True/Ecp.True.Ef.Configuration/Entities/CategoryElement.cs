using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class CategoryElement
    {
        public CategoryElement()
        {
            ContractMeasurementUnitNavigation = new HashSet<Contract>();
            ContractMovementType = new HashSet<Contract>();
            ContractOwner1 = new HashSet<Contract>();
            ContractOwner2 = new HashSet<Contract>();
            EventEventType = new HashSet<Event>();
            EventOwner1 = new HashSet<Event>();
            EventOwner2 = new HashSet<Event>();
            FileRegistration = new HashSet<FileRegistration>();
            Inventory = new HashSet<Inventory>();
            InventoryProductReason = new HashSet<InventoryProduct>();
            InventoryProductSegment = new HashSet<InventoryProduct>();
            MovementContractMeasurementUnitNavigation = new HashSet<MovementContract>();
            MovementContractMovementType = new HashSet<MovementContract>();
            MovementContractOwner1 = new HashSet<MovementContract>();
            MovementContractOwner2 = new HashSet<MovementContract>();
            MovementEventEventType = new HashSet<MovementEvent>();
            MovementEventOwner1 = new HashSet<MovementEvent>();
            MovementEventOwner2 = new HashSet<MovementEvent>();
            MovementReason = new HashSet<Movement>();
            MovementSegment = new HashSet<Movement>();
            Node = new HashSet<Node>();
            NodeConnectionProductOwner = new HashSet<NodeConnectionProductOwner>();
            NodeStorageLocation = new HashSet<NodeStorageLocation>();
            NodeTag = new HashSet<NodeTag>();
            Ownership = new HashSet<Ownership>();
            OwnershipAnalyticsDestinationNodeType = new HashSet<OwnershipAnalytics>();
            OwnershipAnalyticsOwner = new HashSet<OwnershipAnalytics>();
            OwnershipAnalyticsSourceNodeType = new HashSet<OwnershipAnalytics>();
            OwnershipCalculation = new HashSet<OwnershipCalculation>();
            OwnershipCalculationResult = new HashSet<OwnershipCalculationResult>();
            OwnershipNode = new HashSet<OwnershipNode>();
            PendingTransactionOwner = new HashSet<PendingTransaction>();
            PendingTransactionSegment = new HashSet<PendingTransaction>();
            PendingTransactionTypeNavigation = new HashSet<PendingTransaction>();
            SegmentOwnershipCalculationOwner = new HashSet<SegmentOwnershipCalculation>();
            SegmentOwnershipCalculationSegment = new HashSet<SegmentOwnershipCalculation>();
            SegmentUnbalance = new HashSet<SegmentUnbalance>();
            StorageLocationProduct = new HashSet<StorageLocationProduct>();
            StorageLocationProductOwner = new HashSet<StorageLocationProductOwner>();
            SystemOwnershipCalculationOwner = new HashSet<SystemOwnershipCalculation>();
            SystemOwnershipCalculationSegment = new HashSet<SystemOwnershipCalculation>();
            SystemOwnershipCalculationSystem = new HashSet<SystemOwnershipCalculation>();
            SystemUnbalanceSegment = new HashSet<SystemUnbalance>();
            SystemUnbalanceSystem = new HashSet<SystemUnbalance>();
            TicketCategoryElement = new HashSet<Ticket>();
            TicketOwner = new HashSet<Ticket>();
            TransformationDestinationMeasurement = new HashSet<Transformation>();
            TransformationOriginMeasurement = new HashSet<Transformation>();
        }

        public int ElementId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public bool? IsActive { get; set; }
        public int? IconId { get; set; }
        public string Color { get; set; }
        public byte[] RowVersion { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual Category Category { get; set; }
        public virtual Icon Icon { get; set; }
        public virtual ICollection<Contract> ContractMeasurementUnitNavigation { get; set; }
        public virtual ICollection<Contract> ContractMovementType { get; set; }
        public virtual ICollection<Contract> ContractOwner1 { get; set; }
        public virtual ICollection<Contract> ContractOwner2 { get; set; }
        public virtual ICollection<Event> EventEventType { get; set; }
        public virtual ICollection<Event> EventOwner1 { get; set; }
        public virtual ICollection<Event> EventOwner2 { get; set; }
        public virtual ICollection<FileRegistration> FileRegistration { get; set; }
        public virtual ICollection<Inventory> Inventory { get; set; }
        public virtual ICollection<InventoryProduct> InventoryProductReason { get; set; }
        public virtual ICollection<InventoryProduct> InventoryProductSegment { get; set; }
        public virtual ICollection<MovementContract> MovementContractMeasurementUnitNavigation { get; set; }
        public virtual ICollection<MovementContract> MovementContractMovementType { get; set; }
        public virtual ICollection<MovementContract> MovementContractOwner1 { get; set; }
        public virtual ICollection<MovementContract> MovementContractOwner2 { get; set; }
        public virtual ICollection<MovementEvent> MovementEventEventType { get; set; }
        public virtual ICollection<MovementEvent> MovementEventOwner1 { get; set; }
        public virtual ICollection<MovementEvent> MovementEventOwner2 { get; set; }
        public virtual ICollection<Movement> MovementReason { get; set; }
        public virtual ICollection<Movement> MovementSegment { get; set; }
        public virtual ICollection<Node> Node { get; set; }
        public virtual ICollection<NodeConnectionProductOwner> NodeConnectionProductOwner { get; set; }
        public virtual ICollection<NodeStorageLocation> NodeStorageLocation { get; set; }
        public virtual ICollection<NodeTag> NodeTag { get; set; }
        public virtual ICollection<Ownership> Ownership { get; set; }
        public virtual ICollection<OwnershipAnalytics> OwnershipAnalyticsDestinationNodeType { get; set; }
        public virtual ICollection<OwnershipAnalytics> OwnershipAnalyticsOwner { get; set; }
        public virtual ICollection<OwnershipAnalytics> OwnershipAnalyticsSourceNodeType { get; set; }
        public virtual ICollection<OwnershipCalculation> OwnershipCalculation { get; set; }
        public virtual ICollection<OwnershipCalculationResult> OwnershipCalculationResult { get; set; }
        public virtual ICollection<OwnershipNode> OwnershipNode { get; set; }
        public virtual ICollection<PendingTransaction> PendingTransactionOwner { get; set; }
        public virtual ICollection<PendingTransaction> PendingTransactionSegment { get; set; }
        public virtual ICollection<PendingTransaction> PendingTransactionTypeNavigation { get; set; }
        public virtual ICollection<SegmentOwnershipCalculation> SegmentOwnershipCalculationOwner { get; set; }
        public virtual ICollection<SegmentOwnershipCalculation> SegmentOwnershipCalculationSegment { get; set; }
        public virtual ICollection<SegmentUnbalance> SegmentUnbalance { get; set; }
        public virtual ICollection<StorageLocationProduct> StorageLocationProduct { get; set; }
        public virtual ICollection<StorageLocationProductOwner> StorageLocationProductOwner { get; set; }
        public virtual ICollection<SystemOwnershipCalculation> SystemOwnershipCalculationOwner { get; set; }
        public virtual ICollection<SystemOwnershipCalculation> SystemOwnershipCalculationSegment { get; set; }
        public virtual ICollection<SystemOwnershipCalculation> SystemOwnershipCalculationSystem { get; set; }
        public virtual ICollection<SystemUnbalance> SystemUnbalanceSegment { get; set; }
        public virtual ICollection<SystemUnbalance> SystemUnbalanceSystem { get; set; }
        public virtual ICollection<Ticket> TicketCategoryElement { get; set; }
        public virtual ICollection<Ticket> TicketOwner { get; set; }
        public virtual ICollection<Transformation> TransformationDestinationMeasurement { get; set; }
        public virtual ICollection<Transformation> TransformationOriginMeasurement { get; set; }
    }
}
