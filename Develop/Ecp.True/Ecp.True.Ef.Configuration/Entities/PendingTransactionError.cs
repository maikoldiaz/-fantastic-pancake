using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class PendingTransactionError
    {
        public int ErrorId { get; set; }
        public int? TransactionId { get; set; }
        public string RecordId { get; set; }
        public string ErrorMessage { get; set; }
        public string Comment { get; set; }
        public bool IsRetrying { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual PendingTransaction Transaction { get; set; }
    }
}
