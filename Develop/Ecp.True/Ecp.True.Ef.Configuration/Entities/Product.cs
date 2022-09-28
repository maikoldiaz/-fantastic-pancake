using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class Product
    {
        public Product()
        {
            Contract = new HashSet<Contract>();
            EventDestinationProduct = new HashSet<Event>();
            EventSourceProduct = new HashSet<Event>();
            InventoryProduct = new HashSet<InventoryProduct>();
            MovementContract = new HashSet<MovementContract>();
            MovementDestination = new HashSet<MovementDestination>();
            MovementEventDestinationProduct = new HashSet<MovementEvent>();
            MovementEventSourceProduct = new HashSet<MovementEvent>();
            MovementSource = new HashSet<MovementSource>();
            NodeConnectionProduct = new HashSet<NodeConnectionProduct>();
            OwnershipAnalytics = new HashSet<OwnershipAnalytics>();
            OwnershipCalculation = new HashSet<OwnershipCalculation>();
            OwnershipResult = new HashSet<OwnershipResult>();
            SegmentOwnershipCalculation = new HashSet<SegmentOwnershipCalculation>();
            SegmentUnbalance = new HashSet<SegmentUnbalance>();
            StorageLocationProduct = new HashSet<StorageLocationProduct>();
            StorageLocationProductMapping = new HashSet<StorageLocationProductMapping>();
            SystemOwnershipCalculation = new HashSet<SystemOwnershipCalculation>();
            SystemUnbalance = new HashSet<SystemUnbalance>();
            TransformationDestinationDestinationProduct = new HashSet<Transformation>();
            TransformationDestinationSourceProduct = new HashSet<Transformation>();
            TransformationOriginDestinationProduct = new HashSet<Transformation>();
            TransformationOriginSourceProduct = new HashSet<Transformation>();
            Unbalance = new HashSet<Unbalance>();
            UnbalanceComment = new HashSet<UnbalanceComment>();
        }

        public string ProductId { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual ICollection<Contract> Contract { get; set; }
        public virtual ICollection<Event> EventDestinationProduct { get; set; }
        public virtual ICollection<Event> EventSourceProduct { get; set; }
        public virtual ICollection<InventoryProduct> InventoryProduct { get; set; }
        public virtual ICollection<MovementContract> MovementContract { get; set; }
        public virtual ICollection<MovementDestination> MovementDestination { get; set; }
        public virtual ICollection<MovementEvent> MovementEventDestinationProduct { get; set; }
        public virtual ICollection<MovementEvent> MovementEventSourceProduct { get; set; }
        public virtual ICollection<MovementSource> MovementSource { get; set; }
        public virtual ICollection<NodeConnectionProduct> NodeConnectionProduct { get; set; }
        public virtual ICollection<OwnershipAnalytics> OwnershipAnalytics { get; set; }
        public virtual ICollection<OwnershipCalculation> OwnershipCalculation { get; set; }
        public virtual ICollection<OwnershipResult> OwnershipResult { get; set; }
        public virtual ICollection<SegmentOwnershipCalculation> SegmentOwnershipCalculation { get; set; }
        public virtual ICollection<SegmentUnbalance> SegmentUnbalance { get; set; }
        public virtual ICollection<StorageLocationProduct> StorageLocationProduct { get; set; }
        public virtual ICollection<StorageLocationProductMapping> StorageLocationProductMapping { get; set; }
        public virtual ICollection<SystemOwnershipCalculation> SystemOwnershipCalculation { get; set; }
        public virtual ICollection<SystemUnbalance> SystemUnbalance { get; set; }
        public virtual ICollection<Transformation> TransformationDestinationDestinationProduct { get; set; }
        public virtual ICollection<Transformation> TransformationDestinationSourceProduct { get; set; }
        public virtual ICollection<Transformation> TransformationOriginDestinationProduct { get; set; }
        public virtual ICollection<Transformation> TransformationOriginSourceProduct { get; set; }
        public virtual ICollection<Unbalance> Unbalance { get; set; }
        public virtual ICollection<UnbalanceComment> UnbalanceComment { get; set; }
    }
}
