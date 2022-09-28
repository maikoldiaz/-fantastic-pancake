// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Movement.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Registration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Constants = Ecp.True.Entities.Constants;

    /// <summary>
    /// The movement class.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class Movement : BlockchainEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Movement"/> class.
        /// </summary>
        public Movement()
        {
            this.Attributes = new List<AttributeEntity>();
            this.Owners = new List<Owner>();
            this.Ownerships = new List<Ownership>();
            this.Results = new List<OwnershipResult>();
            this.NodeErrors = new List<OwnershipNodeError>();
            this.DeltaErrors = new List<DeltaError>();
            this.DeltaMovements = new List<Movement>();
            this.SapTracking = new List<SapTracking>();
            this.DeltaNodeErrors = new List<DeltaNodeError>();
            this.OriginalMovements = new List<Movement>();
        }

        /// <summary>
        /// Gets or sets the movement transaction identifier.
        /// </summary>
        /// <value>
        /// The movement transaction identifier.
        /// </value>
        public int MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the message type identifier.
        /// </summary>
        /// <value>
        /// The message type identifier.
        /// </value>
        public int MessageTypeId { get; set; }

        /// <summary>
        /// Gets or sets the message type identifier.
        /// </summary>
        /// <value>
        /// The message type identifier.
        /// </value>
        public int SystemTypeId { get; set; }

        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        /// <value>
        /// The type of the event.
        /// </value>
        [Required(ErrorMessage = Constants.EventTypeRequired)]
        [StringLength(10, ErrorMessage = Constants.EventTypeLengthExceeded)]
        [RegularExpression(Constants.AllowLettersOnly, ErrorMessage = Constants.InvalidEventType)]
        public string EventType { get; set; }

        /// <summary>
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>
        [Required(ErrorMessage = Constants.MovementIdentifierRequired)]
        public string MovementId { get; set; }

        /// <summary>
        /// Gets or sets the movement type identifier.
        /// </summary>
        /// <value>
        /// The movement type identifier.
        /// </value>
        [Required(ErrorMessage = Constants.MovementTypeRequired)]
        public int MovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public int? TicketId { get; set; }

        /// <summary>
        /// Gets or sets the ownership ticket identifier.
        /// </summary>
        /// <value>
        /// The ownership ticket identifier.
        /// </value>
        public int? OwnershipTicketId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is system generated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is system generated; otherwise, <c>false</c>.
        /// </value>
        public bool? IsSystemGenerated { get; set; }

        /// <summary>
        /// Gets or sets the segment identifier.
        /// </summary>
        /// <value>
        /// The segment identifier.
        /// </value>
        public int? SegmentId { get; set; }

        /// <summary>
        /// Gets or sets the operational date.
        /// </summary>
        /// <value>
        /// The operational date.
        /// </value>
        [Required(ErrorMessage = Constants.OperationDateRequired)]
        public DateTime OperationalDate { get; set; }

        /// <summary>
        /// Gets or sets the gross standard volume.
        /// </summary>
        /// <value>
        /// The gross standard volume.
        /// </value>
        [NumberValidator(Constants.MinNumberValidator, Constants.MaxNumberValidator, ErrorMessage = Constants.DecimalValidationMessage)]
        public decimal? GrossStandardVolume { get; set; }

        /// <summary>
        /// Gets or sets the net standard volume.
        /// </summary>
        /// <value>
        /// The net standard volume.
        /// </value>
        [Required(ErrorMessage = Constants.NetStandardVolumeIsMandatory)]
        [NumberValidator(Constants.MinNumberValidator, Constants.MaxNumberValidator, ErrorMessage = Constants.DecimalValidationMessage)]
        public decimal? NetStandardVolume { get; set; }

        /// <summary>
        /// Gets or sets the uncertainty percentage.
        /// </summary>
        /// <value>
        /// The uncertainty percentage.
        /// </value>
        [NumberValidator(0.00, 100.00, ErrorMessage = Constants.PercentageValidationMessage)]
        public decimal? UncertaintyPercentage { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        [Required(ErrorMessage = Constants.MeasurementUnitRequired)]
        public int? MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets the scenario.
        /// </summary>
        /// <value>
        /// The scenario.
        /// </value>
        [Range(1, 3, ErrorMessage = Constants.ScenarioIdValueRangeFailed)]
        public ScenarioType ScenarioId { get; set; }

        /// <summary>
        /// Gets or sets the observations.
        /// </summary>
        /// <value>
        /// The observations.
        /// </value>
        [StringLength(150, ErrorMessage = Constants.ObservationLengthExceeded)]
        public string Observations { get; set; }

        /// <summary>
        /// Gets or sets the classification.
        /// </summary>
        /// <value>
        /// The classification.
        /// </value>
        [Required(ErrorMessage = Constants.MovementClassificationIsMandatory)]
        [StringLength(30, ErrorMessage = Constants.ClassificationLengthExceeded)]
        [RegularExpression(Constants.AllowLettersOnly, ErrorMessage = Constants.InvalidClassificationMessage)]
        public string Classification { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the file registration transaction identifier.
        /// </summary>
        /// <value>
        /// The file registration transaction identifier.
        /// </value>
        public int? FileRegistrationTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the blockchain movement transaction identifier.
        /// </summary>
        /// <value>
        /// The blockchain movement transaction identifier.
        /// </value>
        public Guid? BlockchainMovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the previous blockchain movement transaction identifier.
        /// </summary>
        /// <value>
        /// The previous blockchain movement transaction identifier.
        /// </value>
        public Guid? PreviousBlockchainMovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the source movement identifier.
        /// </summary>
        /// <value>
        /// The source movement identifier.
        /// </value>
        public int? SourceMovementId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is official.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is official; otherwise, <c>false</c>.
        /// </value>
        public bool IsOfficial { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [MaxLength(50, ErrorMessage = Constants.VersionIdMax50Characters)]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the ticket.
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        public virtual Ticket Ticket { get; set; }

        /// <summary>
        /// Gets or sets the ownership ticket.
        /// </summary>
        /// <value>
        /// The ownership ticket.
        /// </value>
        public virtual Ticket OwnershipTicket { get; set; }

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        /// <value>
        /// The type of the action.
        /// </value>
        public VariableType? VariableTypeId { get; set; }

        /// <summary>
        /// Gets or sets the reason identifier.
        /// </summary>
        /// <value>
        /// The reason identifier.
        /// </value>
        public int? ReasonId { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        /// <value>
        /// The type of the action.
        /// </value>
        public int? MovementContractId { get; set; }

        /// <summary>
        /// Gets or sets the tolerance.
        /// </summary>
        /// <value>
        /// The tolerance.
        /// </value>
        public decimal? Tolerance { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public int? OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the global movement identifier.
        /// </summary>
        /// <value>
        /// The global movement identifier.
        /// </value>
        public string GlobalMovementId { get; set; }

        /// <summary>
        /// Gets or sets the backup movement identifier.
        /// </summary>
        /// <value>
        /// The backup movement identifier.
        /// </value>
        public string BackupMovementId { get; set; }

        /// <summary>
        /// Gets or sets the balance status.
        /// </summary>
        /// <value>
        /// The balance status.
        /// </value>
        public string BalanceStatus { get; set; }

        /// <summary>
        /// Gets or sets the sap process status.
        /// </summary>
        /// <value>
        /// The sap process status.
        /// </value>
        [StringLength(50, ErrorMessage = Constants.SapProcessStatusLengthExceeded)]
        public string SapProcessStatus { get; set; }

        /// <summary>
        /// Gets or sets the movement event identifier.
        /// </summary>
        /// <value>
        /// The movement event identifier.
        /// </value>
        public int? MovementEventId { get; set; }

        /// <summary>
        /// Gets or sets the batch identifier.
        /// </summary>
        /// <value>
        /// The batch identifier.
        /// </value>
        [MaxLength(25, ErrorMessage = Constants.MovementBatchIdMax25Characters)]
        public string BatchId { get; set; }

        /// <summary>
        /// Gets or sets the system identifier.
        /// </summary>
        /// <value>
        /// The system identifier.
        /// </value>
        public int? SystemId { get; set; }

        /// <summary>
        /// Gets or sets the source system identifier.
        /// </summary>
        /// <value>
        /// The source system identifier.
        /// </value>
        public int? SourceSystemId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is a transfer point or not.
        /// </summary>
        /// <value>
        /// The flag for transfer point.
        /// </value>
        public bool IsTransferPoint { get; set; }

        /// <summary>
        /// Gets or sets the source movement identifier.
        /// </summary>
        /// <value>
        /// The source movement identifier.
        /// </value>
        public int? SourceMovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the source inventory product identifier.
        /// </summary>
        /// <value>
        /// The source inventory product identifier.
        /// </value>
        public int? SourceInventoryProductId { get; set; }

        /// <summary>
        /// Gets or sets the delta ticket identifier.
        /// </summary>
        /// <value>
        /// The delta ticket identifier.
        /// </value>
        public int? DeltaTicketId { get; set; }

        /// <summary>
        /// Gets or sets the official delta ticket identifier.
        /// </summary>
        /// <value>
        /// The official delta ticket identifier.
        /// </value>
        public int? OfficialDeltaTicketId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [pending approval].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [pending approval]; otherwise, <c>false</c>.
        /// </value>
        public bool? PendingApproval { get; set; }

        /// <summary>
        /// Gets or sets the type of the official delta message.
        /// </summary>
        /// <value>
        /// The type of the official delta message.
        /// </value>
        public OfficialDeltaMessageType? OfficialDeltaMessageTypeId { get; set; }

        /// <summary>
        /// Gets or sets the consolidated movement identifier.
        /// </summary>
        /// <value>
        /// The consolidated movement identifier.
        /// </value>
        public int? ConsolidatedMovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the consolidated inventory product identifier.
        /// </summary>
        /// <value>
        /// The consolidated inventory product identifier.
        /// </value>
        public int? ConsolidatedInventoryProductId { get; set; }

        /// <summary>
        /// Gets or sets the original movement transaction identifier.
        /// </summary>
        /// <value>
        /// The delta original movement transaction identifier.
        /// </value>
        public int? OriginalMovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the Ownership Ticket Conciliation identifier.
        /// </summary>
        /// <value>
        /// The Ownership Ticket Conciliation identifier.
        /// </value>
        public int? OwnershipTicketConciliationId { get; set; }

        /// <summary>
        /// Gets or sets the movement is reconciled.
        /// </summary>
        /// <value>
        /// The movement is reconciled.
        /// </value>
        public bool? IsReconciled { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is valid classification.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid classification; otherwise, <c>false</c>.
        /// </value>
        public bool IsValidClassification
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Classification) &&
                        (this.Classification.EqualsIgnoreCase(Ecp.True.Core.Constants.MovementClassification) ||
                         this.Classification.EqualsIgnoreCase(Ecp.True.Core.Constants.LossClassification) ||
                         this.Classification.EqualsIgnoreCase(Ecp.True.Core.Constants.SpecialMovementClassification));
            }
        }

        /// <summary>
        /// Gets or sets the reason.
        /// </summary>
        /// <value>
        /// The reason.
        /// </value>
        public virtual CategoryElement Reason { get; set; }

        /// <summary>
        /// Gets or sets the segment.
        /// </summary>
        /// <value>
        /// The segment.
        /// </value>
        public virtual CategoryElement Segment { get; set; }

        /// <summary>
        /// Gets or sets the System.
        /// </summary>
        /// <value>
        /// The System.
        /// </value>
        public virtual CategoryElement System { get; set; }

        /// <summary>
        /// Gets or sets the System.
        /// </summary>
        /// <value>
        /// The System.
        /// </value>
        public virtual CategoryElement Operator { get; set; }

        /// <summary>
        /// Gets or sets the System.
        /// </summary>
        /// <value>
        /// The System.
        /// </value>
        public virtual CategoryElement SourceSystemElement { get; set; }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attribute.
        /// </value>
        public virtual ICollection<AttributeEntity> Attributes { get; }

        /// <summary>
        /// Gets or sets the movement destination.
        /// </summary>
        /// <value>
        /// The movement destination.
        /// </value>
        [ValidateObject]
        public virtual MovementDestination MovementDestination { get; set; }

        /// <summary>
        /// Gets or sets the period.
        /// </summary>
        /// <value>
        /// The period.
        /// </value>
        [Required(ErrorMessage = Constants.PeriodIsMandatory)]
        public virtual MovementPeriod Period { get; set; }

        /// <summary>
        /// Gets or sets the movement source.
        /// </summary>
        /// <value>
        /// The movement source.
        /// </value>
        [OptionalIf("MovementDestination", ErrorMessage = Constants.BothSourceDestinationMandatory)]
        [ValidateObject]
        public virtual MovementSource MovementSource { get; set; }

        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>
        /// The owner.
        /// </value>
        public virtual ICollection<Owner> Owners { get; }

        /// <summary>
        /// Gets or sets the file registration transaction.
        /// </summary>
        /// <value>
        /// The file registration transaction.
        /// </value>
        public virtual FileRegistrationTransaction FileRegistrationTransaction { get; set; }

        /// <summary>
        /// Gets or sets the event.
        /// </summary>
        /// <value>
        /// The event.
        /// </value>
        public virtual MovementEvent MovementEvent { get; set; }

        /// <summary>
        /// Gets or sets the ownership.
        /// </summary>
        /// <value>
        /// The ownership.
        /// </value>
        public virtual ICollection<Ownership> Ownerships { get; set; }

        /// <summary>
        /// Gets the ownership result.
        /// </summary>
        /// <value>
        /// The ownership result.
        /// </value>
        public virtual ICollection<OwnershipResult> Results { get; }

        /// <summary>
        /// Gets the ownership node error.
        /// </summary>
        /// <value>
        /// The ownership node error.
        /// </value>
        public virtual ICollection<OwnershipNodeError> NodeErrors { get; }

        /// <summary>
        /// Gets or sets the MovementContract.
        /// </summary>
        /// <value>
        /// The MovementContract.
        /// </value>
        public virtual MovementContract MovementContract { get; set; }

        /// <summary>
        /// Gets or sets the source movement.
        /// </summary>
        /// <value>
        /// The source movement.
        /// </value>
        public virtual Movement SourceMovement { get; set; }

        /// <summary>
        /// Gets or sets the original movement.
        /// </summary>
        /// <value>
        /// The original movement.
        /// </value>
        public virtual Movement OriginalMovement { get; set; }

        /// <summary>
        /// Gets or sets the source inventory product.
        /// </summary>
        /// <value>
        /// The source inventory product.
        /// </value>
        public virtual InventoryProduct SourceInventoryProduct { get; set; }

        /// <summary>
        /// Gets the delta error.
        /// </summary>
        /// <value>
        /// The delta error.
        /// </value>
        public virtual ICollection<DeltaError> DeltaErrors { get; }

        /// <summary>
        /// Gets the movements.
        /// </summary>
        /// <value>
        /// The movements.
        /// </value>
        public virtual ICollection<Movement> DeltaMovements { get; }

        /// <summary>
        /// Gets the original movements.
        /// </summary>
        /// <value>
        /// The original movements.
        /// </value>
        public virtual ICollection<Movement> OriginalMovements { get; }

        /// <summary>
        /// Gets the SAP Tracking.
        /// </summary>
        /// <value>
        /// The SAP Tracking.
        /// </value>
        public virtual ICollection<SapTracking> SapTracking { get; }

        /// <summary>
        /// Gets or sets the delta ticket.
        /// </summary>
        /// <value>
        /// The delta ticket.
        /// </value>
        public virtual Ticket DeltaTicket { get; set; }

        /// <summary>
        /// Gets or sets the official delta ticket.
        /// </summary>
        /// <value>
        /// The official delta ticket.
        /// </value>
        public virtual Ticket OfficialDeltaTicket { get; set; }

        /// <summary>
        /// Gets the delta node errors.
        /// </summary>
        /// <value>
        /// The delta node errors.
        /// </value>
        public virtual ICollection<DeltaNodeError> DeltaNodeErrors { get; }

        /// <summary>
        /// Gets or sets the consolidated movement.
        /// </summary>
        /// <value>
        /// The consolidated movement.
        /// </value>
        public virtual ConsolidatedMovement ConsolidatedMovement { get; set; }

        /// <summary>
        /// Gets or sets the consolidated inventory product.
        /// </summary>
        /// <value>
        /// The consolidated inventory product.
        /// </value>
        public virtual ConsolidatedInventoryProduct ConsolidatedInventoryProduct { get; set; }

        /// <summary>
        /// Gets or sets the movement type.
        /// </summary>
        /// <value>
        /// The movement type.
        /// </value>
        public virtual CategoryElement MovementType { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        public virtual CategoryElement MeasurementUnitElement { get; set; }

        /// <summary>
        /// Gets or sets the Movement Logistic.
        /// </summary>
        /// <value>
        /// The Movement Logistic.
        /// </value>
        public virtual LogisticMovement LogisticMovement { get; set; }

        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var element = (Movement)entity;

            this.MessageTypeId = element.MessageTypeId;
            this.SystemTypeId = element.SystemTypeId;
            this.EventType = element.EventType ?? this.EventType;
            this.SegmentId = element.SegmentId ?? this.SegmentId;
            this.OperatorId = element.OperatorId ?? this.OperatorId;
            this.GrossStandardVolume = element.GrossStandardVolume ?? this.GrossStandardVolume;
            this.MovementId = element.MovementId ?? this.MovementId;
            this.MovementTypeId = element.MovementTypeId;
            this.MeasurementUnit = element.MeasurementUnit ?? this.MeasurementUnit;
            this.UncertaintyPercentage = element.UncertaintyPercentage ?? this.UncertaintyPercentage;
            this.OperationalDate = element.OperationalDate;
            this.Reason = element.Reason ?? this.Reason;
            this.Comment = element.Comment ?? this.Comment;
            this.MovementContractId = element.MovementContractId ?? this.MovementContractId;
            this.Classification = element.Classification ?? this.Classification;
            this.VariableTypeId = element.VariableTypeId ?? this.VariableTypeId;
            this.ScenarioId = element.ScenarioId;
            this.Observations = element.Observations ?? this.Observations;
            this.BatchId = element.BatchId ?? this.BatchId;
            this.SystemId = element.SystemId ?? this.SystemId;
            this.SourceSystemId = element.SourceSystemId ?? this.SourceSystemId;
            this.Version = element.Version ?? this.Version;
            this.BackupMovementId = element.BackupMovementId ?? this.BackupMovementId;
            this.GlobalMovementId = element.GlobalMovementId ?? this.GlobalMovementId;
            this.IsTransferPoint = element.IsTransferPoint;
        }
    }
}