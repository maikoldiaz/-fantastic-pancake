using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ViewFileRegistrationStatus
    {
        public string UploadId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string SegmentName { get; set; }
        public string Name { get; set; }
        public int SystemTypeId { get; set; }
        public string FileActionType { get; set; }
        public string CreatedBy { get; set; }
        public string Status { get; set; }
        public int? RecordsProcessed { get; set; }
        public int? ErrorCount { get; set; }
        public bool? IsParsed { get; set; }
    }
}
