// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrueMessage.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Dto
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The true message entity.
    /// </summary>
    public class TrueMessage
    {
        /// <summary>
        /// The failed records.
        /// </summary>
        private int failedEvents;

        /// <summary>
        /// The failed inventory.
        /// </summary>
        private int failedInventory;

        /// <summary>
        /// The failed movements.
        /// </summary>
        private int failedMovements;

        /// <summary>
        /// The contract events.
        /// </summary>
        private int failedContracts;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrueMessage" /> class.
        /// </summary>
        public TrueMessage()
        {
            this.Errors = new ConcurrentDictionary<string, string>();
            this.FileRegistration = new FileRegistration();
            this.PendingTransactions = new ConcurrentBag<PendingTransaction>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrueMessage" /> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="body">The body.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="activityId">The activity identifier.</param>
        public TrueMessage(string label, string body, string messageId, Guid activityId)
            : this()
        {
            ArgumentValidators.ThrowIfNullOrEmpty(label, nameof(label));
            ArgumentValidators.ThrowIfNull(body, nameof(body));

            var index = label.Split('_');

            if (index.Length == 3)
            {
                this.SourceSystem = index[0].ToEnum<SystemType>();
                this.Message = index[1].ToEnum<MessageType>();
                this.TargetSystem = index[2].ToEnum<SystemType>();
            }

            this.InputBlobPath = body;
            this.MessageId = messageId;
            this.ActivityId = activityId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrueMessage" /> class.
        /// </summary>
        /// <param name="system">The system.</param>
        /// <param name="message">The message.</param>
        /// <param name="blobName">Name of the BLOB.</param>
        /// <param name="blobPath">The BLOB path.</param>
        /// <param name="activityId">The activity identifier.</param>
        public TrueMessage(SystemType system, MessageType message, string blobName, string blobPath, Guid activityId)
            : this()
        {
            this.SourceSystem = system;
            this.TargetSystem = SystemType.TRUE;
            this.Message = message;
            this.XmlBlobName = blobName;
            this.MessageId = blobName;
            this.InputBlobPath = blobPath;
            this.ActivityId = activityId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrueMessage" /> class.
        /// </summary>
        /// <param name="system">The system.</param>
        /// <param name="message">The message.</param>
        /// <param name="blobName">Name of the BLOB.</param>
        /// <param name="blobPath">The BLOB path.</param>
        /// <param name="activityId">The activity identifier.</param>
        /// <param name="isOfficialSapMovement">The official sap movement.</param>
        /// <param name="integrationType">The integration type.</param>
        public TrueMessage(SystemType system, MessageType message, string blobName, string blobPath, Guid activityId, bool isOfficialSapMovement, IntegrationType integrationType)
            : this()
        {
            this.SourceSystem = system;
            this.TargetSystem = SystemType.TRUE;
            this.Message = message;
            this.XmlBlobName = blobName;
            this.MessageId = blobName;
            this.InputBlobPath = blobPath;
            this.ActivityId = activityId;
            this.IsOfficialSapMovement = isOfficialSapMovement;
            this.IntegrationType = integrationType;
        }

        /// <summary>
        /// Gets the name of the container.
        /// </summary>
        /// <value>
        /// The name of the container.
        /// </value>
        public static string ContainerName => SystemType.TRUE.ToString("G").ToLowerCase();

        /// <summary>
        /// Gets or sets the file registration transaction identifier.
        /// </summary>
        /// <value>
        /// The file registration transaction identifier.
        /// </value>
        public int FileRegistrationTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the system.
        /// </summary>
        /// <value>
        /// The system.
        /// </value>
        public SystemType SourceSystem { get; set; }

        /// <summary>
        /// Gets or sets the target system.
        /// </summary>
        /// <value>
        /// The target system.
        /// </value>
        public SystemType TargetSystem { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public MessageType Message { get; set; }

        /// <summary>
        /// Gets or sets the XML BLOB path.
        /// </summary>
        /// <value>
        /// The XML BLOB path.
        /// </value>
        public string InputBlobPath { get; set; }

        /// <summary>
        /// Gets or sets the name of the XML BLOB.
        /// </summary>
        /// <value>
        /// The name of the XML BLOB.
        /// </value>
        public string XmlBlobName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is homologation required.
        /// </summary>
        /// <value>
        /// The value for ShouldHomologate.
        /// </value>
        public bool ShouldHomologate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is homologated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is homologated; otherwise, <c>false</c>.
        /// </value>
        public bool IsHomologated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is retry.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is retry; otherwise, <c>false</c>.
        /// </value>
        public bool IsRetry { get; set; }

        /// <summary>
        /// Gets the json BLOB name.
        /// </summary>
        /// <value>
        /// The json BLOB name.
        /// </value>
        public string JsonBlobName
        {
            get
            {
                return $"{this.SourceSystem}/json/{this.Message}/{this.MessageId}".ToLowerCase();
            }
        }

        /// <summary>
        /// Gets the name of the inventory json BLOB.
        /// </summary>
        /// <value>
        /// The name of the inventory json BLOB.
        /// </value>
        public string InventoryJsonBlobName
        {
            get
            {
                return $"{this.SourceSystem}/json/inventory/{this.MessageId}".ToLowerCase();
            }
        }

        /// <summary>
        /// Gets the name of the movement json BLOB.
        /// </summary>
        /// <value>
        /// The name of the movement json BLOB.
        /// </value>
        public string MovementJsonBlobName
        {
            get
            {
                return $"{this.SourceSystem}/json/movement/{this.MessageId}".ToLowerCase();
            }
        }

        /// <summary>
        /// Gets the name of the event json BLOB.
        /// </summary>
        /// <value>
        /// The name of the event json BLOB.
        /// </value>
        public string EventJsonBlobName
        {
            get
            {
                return $"{this.SourceSystem}/json/event/{this.MessageId}".ToLowerCase();
            }
        }

        /// <summary>
        /// Gets or sets the messsage identifier.
        /// </summary>
        /// <value>
        /// The messsage identifier.
        /// </value>
        public string MessageId { get; set; }

        /// <summary>
        /// Gets or sets the activity identifier.
        /// </summary>
        /// <value>
        /// The activity identifier.
        /// </value>
        public Guid ActivityId { get; set; }

        /// <summary>
        /// Gets a value indicating whether is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid => this.SourceSystem != 0 && this.TargetSystem != 0 && this.Message != 0;

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        /// <value>
        /// The type of the action.
        /// </value>
        public string ActionType { get; set; }

        /// <summary>
        /// Gets or sets the failed events.
        /// </summary>
        /// <value>
        /// The failed events.
        /// </value>
        public int FailedEvents { get; set; }

        /// <summary>
        /// Gets or sets the failed movements.
        /// </summary>
        /// <value>
        /// The failed movements.
        /// </value>
        public int FailedMovements { get; set; }

        /// <summary>
        /// Gets or sets the failed inventory.
        /// </summary>
        /// <value>
        /// The failed inventory.
        /// </value>
        public int FailedInventory { get; set; }

        /// <summary>
        /// Gets or sets the failed contracts.
        /// </summary>
        /// <value>
        /// The failed contracts.
        /// </value>
        public int FailedContracts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is official sap movement.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is official sap movement; otherwise, <c>false</c>.
        /// </value>
        public bool IsOfficialSapMovement { get; set; }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public ConcurrentDictionary<string, string> Errors { get; set; }

        /// <summary>
        /// Gets or sets the file registration.
        /// </summary>
        /// <value>
        /// The file registration.
        /// </value>
        public FileRegistration FileRegistration { get; set; }

        /// <summary>
        /// Gets or sets the pending transactions.
        /// </summary>
        /// <value>
        /// The pending transactions.
        /// </value>
        public ConcurrentBag<PendingTransaction> PendingTransactions { get; set; }

        /// <summary>
        /// Gets or sets the type of the integration.
        /// </summary>
        /// <value>
        /// The type of the integration.
        /// </value>
        public IntegrationType IntegrationType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrueMessage"/> class.
        /// </summary>
        /// <param name="fileRegistrationTransaction">The file registration transaction.</param>
        public void AddFileRegistrationTransaction(FileRegistrationTransaction fileRegistrationTransaction)
        {
            ArgumentValidators.ThrowIfNull(fileRegistrationTransaction, nameof(fileRegistrationTransaction));
            ArgumentValidators.ThrowIfNull(fileRegistrationTransaction.FileRegistration, nameof(fileRegistrationTransaction.FileRegistration));

            fileRegistrationTransaction.UploadId = fileRegistrationTransaction.FileRegistration.UploadId;
            fileRegistrationTransaction.SystemTypeId = fileRegistrationTransaction.FileRegistration.SystemTypeId;
            fileRegistrationTransaction.ActionType = fileRegistrationTransaction.FileRegistration.ActionType;
            fileRegistrationTransaction.FileRegistrationCreatedDate = fileRegistrationTransaction.FileRegistration.CreatedDate;
            fileRegistrationTransaction.SkipValidation = false;

            this.IsRetry = true;
            this.SourceSystem = fileRegistrationTransaction.FileRegistration.SystemTypeId;
            this.TargetSystem = SystemType.TRUE;

            this.XmlBlobName = fileRegistrationTransaction.FileRegistration.UploadId;
            this.MessageId = fileRegistrationTransaction.FileRegistration.UploadId;
            this.InputBlobPath = fileRegistrationTransaction.BlobPath;

            this.FileRegistration = fileRegistrationTransaction.FileRegistration;
        }

        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        /// <param name="type">if set to <c>true</c> [is movement].</param>
        /// <returns>message type.</returns>
        public MessageType? GetMessageType(string type)
        {
            if (type.EqualsIgnoreCase(MessageType.Movement.ToString()))
            {
                return MessageType.Movement;
            }
            else if (type.EqualsIgnoreCase(MessageType.Events.ToString()))
            {
                return MessageType.Events;
            }
            else if (type.EqualsIgnoreCase(MessageType.Contract.ToString()) || type.EqualsIgnoreCase(MessageType.Purchase.ToString()) || type.EqualsIgnoreCase(MessageType.Sale.ToString()))
            {
                return MessageType.Contract;
            }
            else if (type.EqualsIgnoreCase(Constants.FailedParsing) || type.EqualsIgnoreCase(Constants.FailedRetry))
            {
                return null;
            }
            else
            {
                return MessageType.Inventory;
            }
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// The name.
        /// </returns>
        public override string ToString()
        {
            return $"{this.SourceSystem}_{this.TargetSystem}";
        }

        /// <summary>
        /// Adds the error.
        /// </summary>
        /// <param name="messageId">The message ID.</param>
        /// <param name="error">The error.</param>
        public void AddError(string messageId, string error)
        {
            if (!string.IsNullOrWhiteSpace(messageId))
            {
                this.Errors.AddOrUpdate(messageId, error, (a, b) => error);
            }
        }

        /// <summary>
        /// Records the failure.
        /// </summary>
        public void RecordEventFailure()
        {
            Interlocked.Increment(ref this.failedEvents);
            this.FailedEvents = this.failedEvents;
        }

        /// <summary>
        /// Records the movement failure.
        /// </summary>
        public void RecordMovementFailure()
        {
            Interlocked.Increment(ref this.failedMovements);
        }

        /// <summary>
        /// Records the inventory failure.
        /// </summary>
        public void RecordInventoryFailure()
        {
            Interlocked.Increment(ref this.failedInventory);
        }

        /// <summary>
        /// Records the contract failure.
        /// </summary>
        public void RecordContractFailure()
        {
            Interlocked.Increment(ref this.failedContracts);
        }

        /// <summary>
        /// Populates the failed records.
        /// </summary>
        public void PopulateFailedRecords()
        {
            this.FailedEvents = this.failedEvents;
            this.FailedContracts = this.failedContracts;
            this.FailedInventory = this.failedInventory;
            this.FailedMovements = this.failedMovements;
        }
    }
}
