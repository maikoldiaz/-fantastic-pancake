// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileRegistrationTransaction.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.TransportBalance
{
    using System;
    using System.Collections.Generic;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Newtonsoft.Json;

    /// <summary>
    /// The FileRegistrationTransaction.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class FileRegistrationTransaction : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileRegistrationTransaction"/> class.
        /// </summary>
        public FileRegistrationTransaction()
        {
            this.Inventories = new List<InventoryProduct>();
            this.Movements = new List<Movement>();
        }

        /// <summary>
        /// Gets or sets the file registration transaction identifier.
        /// </summary>
        /// <value>
        /// The file registration transaction identifier.
        /// </value>
        public int FileRegistrationTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the file registration identifier.
        /// </summary>
        /// <value>
        /// The file registration identifier.
        /// </value>
        public int FileRegistrationId { get; set; }

        /// <summary>
        /// Gets or sets the BLOB path.
        /// </summary>
        /// <value>
        /// The BLOB path.
        /// </value>
        public string BlobPath { get; set; }

        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the type of the status.
        /// </summary>
        /// <value>
        /// The type of the status.
        /// </value>
        public StatusType StatusTypeId { get; set; }

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
        public string UploadId { get; set; }

        /// <summary>
        /// Gets or sets the record identifier.
        /// </summary>
        /// <value>
        /// The record identifier.
        /// </value>
        public string RecordId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="FileRegistrationTransaction"/> is validate.
        /// </summary>
        /// <value>
        ///   <c>true</c> if validate; otherwise, <c>false</c>.
        /// </value>
        public bool SkipValidation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is retry.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is retry; otherwise, <c>false</c>.
        /// </value>
        public bool IsRetry { get; set; }

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        /// <value>
        /// The type of the action.
        /// </value>
        public FileRegistrationActionType? ActionType { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        public DateTime? FileRegistrationCreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the message type.
        /// </summary>
        /// <value>
        /// The message type.
        /// </value>
        public MessageType MessageType { get; set; }

        /// <summary>
        /// Gets or sets the file registration.
        /// </summary>
        /// <value>
        /// The file registration.
        /// </value>
        [JsonIgnore]
        public virtual FileRegistration FileRegistration { get; set; }

        /// <summary>
        /// Gets the inventories.
        /// </summary>
        /// <value>
        /// The inventories.
        /// </value>
        public virtual ICollection<InventoryProduct> Inventories { get; }

        /// <summary>
        /// Gets the movements.
        /// </summary>
        /// <value>
        /// The movements.
        /// </value>
        public virtual ICollection<Movement> Movements { get; }
    }
}
