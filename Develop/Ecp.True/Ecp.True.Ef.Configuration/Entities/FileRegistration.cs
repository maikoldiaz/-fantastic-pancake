using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class FileRegistration
    {
        public FileRegistration()
        {
            FileRegistrationError = new HashSet<FileRegistrationError>();
            FileRegistrationTransaction = new HashSet<FileRegistrationTransaction>();
        }

        public int FileRegistrationId { get; set; }
        public string UploadId { get; set; }
        public DateTime UploadDate { get; set; }
        public string Name { get; set; }
        public int Action { get; set; }
        public int Status { get; set; }
        public int SystemTypeId { get; set; }
        public int? SegmentId { get; set; }
        public int RecordsProcessed { get; set; }
        public bool IsActive { get; set; }
        public bool IsHomologated { get; set; }
        public string BlobPath { get; set; }
        public string HomologationInventoryBlobPath { get; set; }
        public string HomologationMovementBlobPath { get; set; }
        public Guid? PreviousUploadId { get; set; }
        public bool? IsParsed { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual RegisterFileActionType ActionNavigation { get; set; }
        public virtual CategoryElement Segment { get; set; }
        public virtual FileUploadState StatusNavigation { get; set; }
        public virtual SystemType SystemType { get; set; }
        public virtual ICollection<FileRegistrationError> FileRegistrationError { get; set; }
        public virtual ICollection<FileRegistrationTransaction> FileRegistrationTransaction { get; set; }
    }
}
