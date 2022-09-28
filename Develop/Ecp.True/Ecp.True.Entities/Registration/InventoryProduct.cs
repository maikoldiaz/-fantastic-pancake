// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryProduct.cs" company="Microsoft">
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
    using Ecp.True.Core.Attributes;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Argument = Ecp.True.Core;

    /// <summary>
    /// The Inventory Product.
    /// </summary>
    public class InventoryProduct : BlockchainEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryProduct"/> class.
        /// </summary>
        public InventoryProduct()
        {
            this.Attributes = new List<AttributeEntity>();
            this.Owners = new List<Owner>();
            this.Ownerships = new List<Ownership>();
            this.NodeErrors = new List<OwnershipNodeError>();
            this.Results = new List<OwnershipResult>();
            this.DeltaErrors = new List<DeltaError>();
            this.DeltaMovements = new List<Movement>();
            this.DeltaNodeErrors = new List<DeltaNodeError>();
        }

        /// <summary>
        /// Gets or sets the inventory product identifier.
        /// </summary>
        /// <value>
        /// The inventory product identifier.
        /// </value>
        public int InventoryProductId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [Required(ErrorMessage = Constants.ProductsIdRequired)]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the type of the product.
        /// </summary>
        /// <value>
        /// The type of the product.
        /// </value>
        public int? ProductType { get; set; }

        /// <summary>
        /// Gets or sets the product volume.
        /// </summary>
        /// <value>
        /// The product volume.
        /// </value>
        [Required(ErrorMessage = Constants.ProductsVolumeRequired)]
        [NumberValidator(ErrorMessage = Constants.DecimalValidationMessage)]
        public decimal? ProductVolume { get; set; }

        /// <summary>
        /// Gets or sets the gross standard volume.
        /// </summary>
        /// <value>
        /// The Gross Standard Quantity.
        /// </value>
        [NumberValidator(ErrorMessage = Constants.DecimalValidationMessage)]
        public decimal? GrossStandardQuantity { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        public int? MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets the ownership ticket identifier.
        /// </summary>
        /// <value>
        /// The ownership ticket identifier.
        /// </value>
        public int? OwnershipTicketId { get; set; }

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
        /// Gets or sets the blockchain inventory product transaction identifier.
        /// </summary>
        /// <value>
        /// The blockchain inventory product transaction identifier.
        /// </value>
        public Guid? BlockchainInventoryProductTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the previous blockchain inventory product transaction identifier.
        /// </summary>
        /// <value>
        /// The previous blockchain inventory product transaction identifier.
        /// </value>
        public Guid? PreviousBlockchainInventoryProductTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the message type identifier.
        /// </summary>
        /// <value>
        /// The message type identifier.
        /// </value>
        public int SystemTypeId { get; set; }

        /// <summary>
        /// Gets or sets the destination system.
        /// </summary>
        /// <value>
        /// The destination system.
        /// </value>
        [StringLength(25, ErrorMessage = Constants.DestinationSystemLengthExceeded)]
        public string DestinationSystem { get; set; }

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
        /// Gets or sets the name of the tank.
        /// </summary>
        /// <value>
        /// The name of the tank.
        /// </value>
        [StringLength(20, ErrorMessage = Constants.TankNameLengthExceeded)]
        public string TankName { get; set; }

        /// <summary>
        /// Gets or sets the inventory identifier.
        /// </summary>
        /// <value>
        /// The inventory identifier.
        /// </value>
        [Required(ErrorMessage = Constants.InventoryIdRequired)]
        public string InventoryId { get; set; }

        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public int? TicketId { get; set; }

        /// <summary>
        /// Gets or sets the inventory date.
        /// </summary>
        /// <value>
        /// The inventory date.
        /// </value>
        [Required(ErrorMessage = Constants.InventoryDateRequired)]
        public DateTime? InventoryDate { get; set; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        [Required(ErrorMessage = Constants.NodeIdRequired)]
        [Range(1, int.MaxValue, ErrorMessage = Constants.NodeIdValueFailed)]
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets the observations.
        /// </summary>
        /// <value>
        /// The observations.
        /// </value>
        [StringLength(150, ErrorMessage = Constants.ObservationLengthExceeded)]
        public string Observations { get; set; }

        /// <summary>
        /// Gets or sets the scenario.
        /// </summary>
        /// <value>
        /// The scenario.
        /// </value>
        [Range(1, 3, ErrorMessage = Constants.ScenarioIdValueRangeFailed)]
        public ScenarioType ScenarioId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the is deleted.
        /// </summary>
        /// <value>
        /// The is deleted.
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
        /// Gets or sets the segment identifier.
        /// </summary>
        /// <value>
        /// The segment identifier.
        /// </value>
        public int? SegmentId { get; set; }

        /// <summary>
        /// Gets or sets the uncertainty percentage.
        /// </summary>
        /// <value>
        /// The uncertainty percentage.
        /// </value>
        [NumberValidator(0.00, 100.00, ErrorMessage = Constants.PercentageValidationMessage)]
        public decimal? UncertaintyPercentage { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public int? OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the batch identifier.
        /// </summary>
        /// <value>
        /// The batch identifier.
        /// </value>
        [MaxLength(25, ErrorMessage = Constants.InventoryBatchIdMax25Characters)]
        public string BatchId { get; set; }

        /// <summary>
        /// Gets or sets the inventory product unique identifier.
        /// </summary>
        /// <value>
        /// The inventory product unique identifier.
        /// </value>
        public string InventoryProductUniqueId { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [MaxLength(50, ErrorMessage = Constants.VersionIdMax50Characters)]
        public string Version { get; set; }

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
        /// Gets the ownership result.
        /// </summary>
        /// <value>
        /// The ownership result.
        /// </value>
        public virtual ICollection<OwnershipResult> Results { get; }

        /// <summary>
        /// Gets or sets the file registration transaction.
        /// </summary>
        /// <value>
        /// The file registration transaction.
        /// </value>
        public virtual FileRegistrationTransaction FileRegistrationTransaction { get; set; }

        /// <summary>
        /// Gets or sets the type of the system.
        /// </summary>
        /// <value>
        /// The type of the system.
        /// </value>
        public virtual SystemTypeEntity SystemType { get; set; }

        /// <summary>
        /// Gets or sets the reason.
        /// </summary>
        /// <value>
        /// The reason.
        /// </value>
        public virtual CategoryElement Reason { get; set; }

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
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public virtual Ticket OwnershipTicket { get; set; }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        public virtual Product Product { get; set; }

        /// <summary>
        /// Gets or sets the ticket.
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        public virtual Ticket Ticket { get; set; }

        /// <summary>
        /// Gets or sets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        public virtual Node Node { get; set; }

        /// <summary>
        /// Gets or sets the segment.
        /// </summary>
        /// <value>
        /// The segment.
        /// </value>
        public virtual CategoryElement Segment { get; set; }

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <value>
        /// The attribute.
        /// </value>
        public virtual ICollection<AttributeEntity> Attributes { get; }

        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>
        /// The owner.
        /// </value>
        public virtual ICollection<Owner> Owners { get; }

        /// <summary>
        /// Gets the ownerships.
        /// </summary>
        /// <value>
        /// The ownerships.
        /// </value>
        public virtual ICollection<Ownership> Ownerships { get; }

        /// <summary>
        /// Gets the ownership node error.
        /// </summary>
        /// <value>
        /// The ownership node error.
        /// </value>
        public virtual ICollection<OwnershipNodeError> NodeErrors { get; }

        /// <summary>
        /// Gets the delta error.
        /// </summary>
        /// <value>
        /// The delta error.
        /// </value>
        public virtual ICollection<DeltaError> DeltaErrors { get; }

        /// <summary>
        /// Gets the movement.
        /// </summary>
        /// <value>
        /// The movement.
        /// </value>
        public virtual ICollection<Movement> DeltaMovements { get; }

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
        /// Gets or sets the product type.
        /// </summary>
        /// <value>
        /// The product type.
        /// </value>
        public virtual CategoryElement ProductTypeElement { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        public virtual CategoryElement MeasurementUnitElement { get; set; }

        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            Argument.ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var element = (InventoryProduct)entity;

            this.ProductId = element.ProductId ?? this.ProductId;
            this.ProductType = element.ProductType ?? this.ProductType;
            this.ProductVolume = element.ProductVolume ?? this.ProductVolume;
            this.MeasurementUnit = element.MeasurementUnit ?? this.MeasurementUnit;
            this.UncertaintyPercentage = element.UncertaintyPercentage ?? this.UncertaintyPercentage;
            this.Reason = element.Reason ?? this.Reason;
            this.Comment = element.Comment ?? this.Comment;
            this.CopyFromInventory(element);
            this.CopyProperties(element);
            this.CopyPropertiesFromInventory(element);
        }

        private void CopyFromInventory(InventoryProduct element)
        {
            this.NodeId = element.NodeId;
            this.SystemTypeId = element.SystemTypeId;
            this.DestinationSystem = element.DestinationSystem ?? this.DestinationSystem;
            this.EventType = element.EventType ?? this.EventType;
            this.TankName = element.TankName ?? this.TankName;
            this.InventoryId = element.InventoryId ?? this.InventoryId;
        }

        private void CopyProperties(InventoryProduct element)
        {
            this.InventoryDate = element.InventoryDate ?? this.InventoryDate;
            this.Observations = element.Observations ?? this.Observations;
            this.ScenarioId = element.ScenarioId;
            this.FileRegistrationTransactionId = element.FileRegistrationTransactionId ?? this.FileRegistrationTransactionId;
            this.SegmentId = element.SegmentId ?? this.SegmentId;
            this.OperatorId = element.OperatorId ?? this.OperatorId;
            this.BatchId = element.BatchId ?? this.BatchId;
            this.InventoryProductUniqueId = element.InventoryProductUniqueId ?? this.InventoryProductUniqueId;
            this.SourceSystemId = element.SourceSystemId ?? this.SourceSystemId;
        }

        private void CopyPropertiesFromInventory(InventoryProduct element)
        {
            this.Version = element.Version ?? this.Version;
            this.GrossStandardQuantity = element.GrossStandardQuantity ?? this.GrossStandardQuantity;
            this.SystemId = element.SystemId ?? this.SystemId;
        }
    }
}