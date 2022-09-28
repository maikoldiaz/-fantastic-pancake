using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ViewGetParsingErrors
    {
        public int? Id { get; set; }
        public int ErrorId { get; set; }
        public string MessageId { get; set; }
        public string SystemName { get; set; }
        public string Process { get; set; }
        public string FileName { get; set; }
        public DateTime CreationDate { get; set; }
        public bool? IsRetry { get; set; }
        public int? FileRegistrationTransactionId { get; set; }
        public string UploadId { get; set; }
    }
}
