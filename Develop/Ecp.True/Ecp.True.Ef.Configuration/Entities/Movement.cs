using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class Movement
    {
        public Movement()
        {
            Attribute = new HashSet<Attribute>();
            Owner = new HashSet<Owner>();
            Ownership = new HashSet<Ownership>();
            OwnershipNodeError = new HashSet<OwnershipNodeError>();
            OwnershipResult = new HashSet<OwnershipResult>();
        }

        public int MovementTransactionId { get; set; }
        public int MessageTypeId { get; set; }
        public int SystemTypeId { get; set; }
        public string SourceSystem { get; set; }
        public string EventType { get; set; }
        public string MovementId { get; set; }
        public string MovementTypeId { get; set; }
        public int? TicketId { get; set; }
        public int? SegmentId { get; set; }
        public DateTime OperationalDate { get; set; }
        public decimal? GrossStandardVolume { get; set; }
        public decimal NetStandardVolume { get; set; }
        public decimal? UncertaintyPercentage { get; set; }
        public string MeasurementUnit { get; set; }
        public int? ScenarioId { get; set; }
        public string Observations { get; set; }
        public string Classification { get; set; }
        public bool IsDeleted { get; set; }
        public bool? IsSystemGenerated { get; set; }
        public int? VariableTypeId { get; set; }
        public int? FileRegistrationTransactionId { get; set; }
        public int? OwnershipTicketId { get; set; }
        public string SystemName { get; set; }
        public int? ReasonId { get; set; }
        public string Comment { get; set; }
        public int? MovementContractId { get; set; }
        public bool BlockchainStatus { get; set; }
        public Guid? BlockchainMovementTransactionId { get; set; }
        public Guid? PreviousBlockchainMovementTransactionId { get; set; }
        public decimal? Tolerance { get; set; }
        public string Operator { get; set; }
        public int? BackupMovementId { get; set; }
        public string GlobalMovementId { get; set; }
        public string BalanceStatus { get; set; }
        public string SapprocessStatus { get; set; }
        public int? MovementEventId { get; set; }
        public int? SourceMovementId { get; set; }
        public string TransactionHash { get; set; }
        public string BlockNumber { get; set; }
        public int RetryCount { get; set; }
        public bool IsOfficial { get; set; }
        public string Version { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual FileRegistrationTransaction FileRegistrationTransaction { get; set; }
        public virtual MessageType MessageType { get; set; }
        public virtual MovementContract MovementContract { get; set; }
        public virtual MovementEvent MovementEvent { get; set; }
        public virtual Ticket OwnershipTicket { get; set; }
        public virtual CategoryElement Reason { get; set; }
        public virtual ScenarioType Scenario { get; set; }
        public virtual CategoryElement Segment { get; set; }
        public virtual SystemType SystemType { get; set; }
        public virtual Ticket Ticket { get; set; }
        public virtual VariableType VariableType { get; set; }
        public virtual MovementDestination MovementDestination { get; set; }
        public virtual MovementPeriod MovementPeriod { get; set; }
        public virtual MovementSource MovementSource { get; set; }
        public virtual ICollection<Attribute> Attribute { get; set; }
        public virtual ICollection<Owner> Owner { get; set; }
        public virtual ICollection<Ownership> Ownership { get; set; }
        public virtual ICollection<OwnershipNodeError> OwnershipNodeError { get; set; }
        public virtual ICollection<OwnershipResult> OwnershipResult { get; set; }
    }
}
