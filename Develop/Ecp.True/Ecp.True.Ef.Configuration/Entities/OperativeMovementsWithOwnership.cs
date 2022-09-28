using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class OperativeMovementsWithOwnership
    {
        public int OperativeMovementsWithOwnershipId { get; set; }
        public DateTime OperationalDate { get; set; }
        public string MovementType { get; set; }
        public string SourceProduct { get; set; }
        public string SourceStorageLocation { get; set; }
        public string DestinationProduct { get; set; }
        public string DestinationStorageLocation { get; set; }
        public decimal OwnershipVolume { get; set; }
        public string TransferPoint { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int DayOfMonth { get; set; }
        public string SourceSystem { get; set; }
        public DateTime LoadDate { get; set; }
        public Guid? ExecutionId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
