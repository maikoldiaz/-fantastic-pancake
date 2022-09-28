// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileRegistration.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Admin
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The Register File entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class FileRegistration : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileRegistration"/> class.
        /// </summary>
        public FileRegistration()
        {
            this.FileRegistrationErrors = new List<FileRegistrationError>();
            this.FileRegistrationTransactions = new List<FileRegistrationTransaction>();
            this.SapTracking = new List<SapTracking>();
        }

        /// <summary>
        /// Gets or sets the register file identifier.
        /// </summary>
        /// <value>
        /// The register file identifier.
        /// </value>
        public int FileRegistrationId { get; set; }

        /// <summary>
        /// Gets or sets the system type identifier.
        /// </summary>
        /// <value>
        /// The system type identifier.
        /// </value>
        public SystemType SystemTypeId { get; set; }

        /// <summary>
        /// Gets or sets the upload file identifier.
        /// </summary>
        /// <value>
        /// The upload file identifier.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.RegisterFileUploadFileIdRequired)]
        public string UploadId { get; set; }

        /// <summary>
        /// Gets or sets the upload date.
        /// </summary>
        /// <value>
        /// The upload date.
        /// </value>
        public DateTime MessageDate { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.RegisterFileNameRequired)]
        [StringLength(500, ErrorMessage = Entities.Constants.NameMaxLength150)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        /// <value>
        /// The type of the action.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.RegisterFileActionTypeRequired)]
        public FileRegistrationActionType? ActionType { get; set; }

        /// <summary>
        /// Gets or sets the file upload status.
        /// </summary>
        /// <value>
        /// The file upload status.
        /// </value>
        public FileUploadStatus FileUploadStatus { get; set; }

        /// <summary>
        /// Gets or sets the BLOB URL.
        /// </summary>
        /// <value>
        /// The BLOB URI.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.RegisterFileBlobPathRequired)]
        public string BlobPath { get; set; }

        /// <summary>
        /// Gets or sets the homologation inventory BLOB path.
        /// </summary>
        /// <value>
        /// The homologation inventory BLOB path.
        /// </value>
        public string HomologationInventoryBlobPath { get; set; }

        /// <summary>
        /// Gets or sets the homologation movement BLOB path.
        /// </summary>
        /// <value>
        /// The homologation movement BLOB path.
        /// </value>
        public string HomologationMovementBlobPath { get; set; }

        /// <summary>
        /// Gets or sets the previous upload identifier.
        /// </summary>
        /// <value>
        /// The previous upload identifier.
        /// </value>
        public Guid? PreviousUploadId { get; set; }

        /// <summary>
        /// Gets or sets the is parsed.
        /// </summary>
        /// <value>
        /// The is parsed.
        /// </value>
        public bool? IsParsed { get; set; }

        /// <summary>
        /// Gets or sets the segment identifier.
        /// </summary>
        /// <value>
        /// The segment identifier.
        /// </value>
        public int? SegmentId { get; set; }

        /// <summary>
        /// Gets or sets the source system.
        /// </summary>
        /// <value>
        /// The source system.
        /// </value>
        public string SourceSystem { get; set; }

        /// <summary>
        /// Gets or sets the source Type.
        /// </summary>
        /// <value>
        /// The source Type.
        /// </value>
        public SystemType? SourceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the type of the integration.
        /// </summary>
        /// <value>
        /// The type of the integration.
        /// </value>
        public IntegrationType? IntegrationType { get; set; }

        /// <summary>
        /// Gets or sets the segment.
        /// </summary>
        /// <value>
        /// The segment.
        /// </value>
        public virtual CategoryElement Segment { get; set; }

        /// <summary>
        /// Gets or sets the file registration transactions.
        /// </summary>
        /// <value>
        /// The file registration transactions.
        /// </value>
        public virtual ICollection<FileRegistrationTransaction> FileRegistrationTransactions { get; set; }

        /// <summary>
        /// Gets the SAP Tracking.
        /// </summary>
        /// <value>
        /// The SAP Tracking.
        /// </value>
        public virtual ICollection<SapTracking> SapTracking { get; }

        /// <summary>
        /// Gets or sets the file registration error.
        /// </summary>
        /// <value>
        /// The file registration error.
        /// </value>
        public virtual ICollection<FileRegistrationError> FileRegistrationErrors { get; set; }

        /// <summary>
        /// Values the tuple.
        /// </summary>
        /// <typeparam name="ICollection`1">The type of the collection`1.</typeparam>
        /// <param name="transactions">The transactions.</param>
        public void AddRecords(IEnumerable<FileRegistrationTransaction> transactions)
        {
            ((List<FileRegistrationTransaction>)this.FileRegistrationTransactions).AddRange(transactions);
        }
    }
}
