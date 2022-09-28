using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class AuditLog
    {
        public int AuditLogId { get; set; }
        public DateTime LogDate { get; set; }
        public string LogType { get; set; }
        public string User { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Field { get; set; }
        public string Entity { get; set; }
        public string NodeCode { get; set; }
        public string StoreLocationCode { get; set; }
        public string Pk { get; set; }
    }
}
