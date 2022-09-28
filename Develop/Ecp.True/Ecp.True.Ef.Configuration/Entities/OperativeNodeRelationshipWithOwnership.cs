using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class OperativeNodeRelationshipWithOwnership
    {
        public int OperativeNodeRelationshipWithOwnershipId { get; set; }
        public string SourceProduct { get; set; }
        public string LogisticSourceCenter { get; set; }
        public string DestinationProduct { get; set; }
        public string LogisticDestinationCenter { get; set; }
        public string TransferPoint { get; set; }
        public bool IsDeleted { get; set; }
        public string Notes { get; set; }
        public string SourceSystem { get; set; }
        public DateTime LoadDate { get; set; }
        public Guid? ExecutionId { get; set; }
        public byte[] RowVersion { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
