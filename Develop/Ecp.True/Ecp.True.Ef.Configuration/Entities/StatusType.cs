using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class StatusType
    {
        public StatusType()
        {
            FileRegistrationTransaction = new HashSet<FileRegistrationTransaction>();
            OwnershipNode = new HashSet<OwnershipNode>();
            Ticket = new HashSet<Ticket>();
        }

        public int StatusTypeId { get; set; }
        public string StatusType1 { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<FileRegistrationTransaction> FileRegistrationTransaction { get; set; }
        public virtual ICollection<OwnershipNode> OwnershipNode { get; set; }
        public virtual ICollection<Ticket> Ticket { get; set; }
    }
}
