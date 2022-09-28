using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class MessageType
    {
        public MessageType()
        {
            Movement = new HashSet<Movement>();
            Ownership = new HashSet<Ownership>();
            OwnershipResult = new HashSet<OwnershipResult>();
            PendingTransaction = new HashSet<PendingTransaction>();
            RegistrationProgress = new HashSet<RegistrationProgress>();
            Transformation = new HashSet<Transformation>();
        }

        public int MessageTypeId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual ICollection<Movement> Movement { get; set; }
        public virtual ICollection<Ownership> Ownership { get; set; }
        public virtual ICollection<OwnershipResult> OwnershipResult { get; set; }
        public virtual ICollection<PendingTransaction> PendingTransaction { get; set; }
        public virtual ICollection<RegistrationProgress> RegistrationProgress { get; set; }
        public virtual ICollection<Transformation> Transformation { get; set; }
    }
}
