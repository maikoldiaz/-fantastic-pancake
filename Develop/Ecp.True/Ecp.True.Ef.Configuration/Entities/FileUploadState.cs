using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class FileUploadState
    {
        public FileUploadState()
        {
            FileRegistration = new HashSet<FileRegistration>();
        }

        public int FileUploadStateId { get; set; }
        public string FileUploadState1 { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual ICollection<FileRegistration> FileRegistration { get; set; }
    }
}
