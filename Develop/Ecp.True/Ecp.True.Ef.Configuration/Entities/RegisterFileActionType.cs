using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class RegisterFileActionType
    {
        public RegisterFileActionType()
        {
            FileRegistration = new HashSet<FileRegistration>();
            PendingTransaction = new HashSet<PendingTransaction>();
        }

        public int ActionTypeId { get; set; }
        public string FileActionType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual ICollection<FileRegistration> FileRegistration { get; set; }
        public virtual ICollection<PendingTransaction> PendingTransaction { get; set; }
    }
}
