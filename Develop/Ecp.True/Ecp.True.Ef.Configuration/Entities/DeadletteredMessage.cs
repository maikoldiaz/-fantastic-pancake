using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class DeadletteredMessage
    {
        public int DeadletteredMessageId { get; set; }
        public string BlobPath { get; set; }
        public string ProcessName { get; set; }
        public string QueueName { get; set; }
        public bool Status { get; set; }
        public string ErrorMessage { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
