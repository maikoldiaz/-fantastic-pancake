// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Contract.cs" company="Microsoft">
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
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The event.
    /// </summary>
    public class Contract : AuditableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Contract"/> class.
        /// </summary>
        public Contract()
        {
            this.MovementContracts = new List<MovementContract>();
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int ContractId { get; set; }

        /// <summary>
        /// Gets or sets the document identifier.
        /// </summary>
        /// <value>
        /// The document identifier.
        /// </value>
        [Range(1, int.MaxValue, ErrorMessage = Constants.PurchaseAndSellDocumentRequiredValidation)]
        public int DocumentNumber { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The contract position.
        /// </value>
        [Range(1, int.MaxValue, ErrorMessage = Constants.PurchaseAndSellPositionRequiredValidation)]
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The contract type identifier.
        /// </value>
        [Range(1, int.MaxValue, ErrorMessage = Constants.PurchaseAndSellTypeRequiredValidation)]
        public int MovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        public int? SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination node identifier.
        /// </summary>
        /// <value>
        /// The destination node identifier.
        /// </value>
        public int? DestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [Required(ErrorMessage = Constants.PurchaseAndSellProductRequiredValidation)]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.EventStartDateIsMandatory)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.EventEndDateIsMandatory)]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the first owner identifier.
        /// </summary>
        /// <value>
        /// The first owner identifier.
        /// </value>
        public int? Owner1Id { get; set; }

        /// <summary>
        /// Gets or sets the second owner identifier.
        /// </summary>
        /// <value>
        /// The second owner identifier.
        /// </value>
        [Range(1, int.MaxValue, ErrorMessage = Constants.Owner2Requiredvalidation)]
        public int? Owner2Id { get; set; }

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        [Range(1, int.MaxValue, ErrorMessage = Constants.PurchaseAndSellValueRequiredValidation)]
        public decimal Volume { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        [Range(1, int.MaxValue, ErrorMessage = Entities.Constants.PurchaseAndSellUnitRequiredValidation)]
        public int MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets the Source System.
        /// </summary>
        /// <value>
        /// The Source System.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.ContractSourceSystemRequired)]
        public string SourceSystem { get; set; }

        /// <summary>
        /// Gets or sets the date Received PO.
        /// </summary>
        /// <value>
        /// The date Received PO.
        /// </value>
        public DateTime? DateReceivedPo { get; set; }

        /// <summary>
        /// Gets or sets the Date_Order.
        /// </summary>
        /// <value>
        /// The Date_Order.
        /// </value>
        public DateTime? DateOrder { get; set; }

        /// <summary>
        /// Gets or sets the MessageId.
        /// </summary>
        /// <value>
        /// The MessageId.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.ContractMessageIdRequired)]
        public string MessageId { get; set; }

        /// <summary>
        /// Gets or sets the MessageId.
        /// </summary>
        /// <value>
        /// The MessageId.
        /// </value>
        public int? PurchaseOrderType { get; set; }

        /// <summary>
        /// Gets or sets the ExpeditionClass.
        /// </summary>
        /// <value>
        /// The ExpeditionClass.
        /// </value>
        public int? ExpeditionClass { get; set; }

        /// <summary>
        /// Gets or sets the ExpeditionClass.
        /// </summary>
        /// <value>
        /// The ExpeditionClass.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the ExpeditionClass.
        /// </summary>
        /// <value>
        /// The ExpeditionClass.
        /// </value>
        public string EventType { get; set; }

        /// <summary>
        /// Gets or sets the StatusCredit.
        /// </summary>
        /// <value>
        /// The StatusCredit.
        /// </value>
        public string StatusCredit { get; set; }

        /// <summary>
        /// Gets or sets the Description Status.
        /// </summary>
        /// <value>
        /// The Description Status.
        /// </value>
        public string DescriptionStatus { get; set; }

        /// <summary>
        /// Gets or sets the Position Status.
        /// </summary>
        /// <value>
        /// The Position Status.
        /// </value>
        public string PositionStatus { get; set; }

        /// <summary>
        /// Gets or sets the Frequency.
        /// </summary>
        /// <value>
        /// The Frequency.
        /// </value>
        public string Frequency { get; set; }

        /// <summary>
        /// Gets or sets the Estimated Volume.
        /// </summary>
        /// <value>
        /// The Estimated Volume.
        /// </value>
        public decimal? EstimatedVolume { get; set; }

        /// <summary>
        /// Gets or sets the Tolerance.
        /// </summary>
        /// <value>
        /// The Tolerance.
        /// </value>
        public decimal? Tolerance { get; set; }

        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        /// <value>
        /// The Value.
        /// </value>
        public decimal? Value { get; set; }

        /// <summary>
        /// Gets or sets the Property.
        /// </summary>
        /// <value>
        /// The Property.
        /// </value>
        public string Property { get; set; }

        /// <summary>
        /// Gets or sets the Uom.
        /// </summary>
        /// <value>
        /// The Uom.
        /// </value>
        public string Uom { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        /// <value>
        /// The type of the action.
        /// </value>
        public string ActionType { get; set; }

        /// <summary>
        /// Gets or sets the file registration transaction identifier.
        /// </summary>
        /// <value>
        /// The file registration transaction identifier.
        /// </value>
        public int? FileRegistrationTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the DestinationStorageLocationId.
        /// </summary>
        /// <value>
        /// The DestinationStorageLocationId.
        /// </value>
        public string DestinationStorageLocationId { get; set; }

        /// <summary>
        /// Gets or sets the Batch.
        /// </summary>
        /// <value>
        /// The Batch.
        /// </value>
        public string Batch { get; set; }

        /// <summary>
        /// Gets or sets the OriginMessageId.
        /// </summary>
        /// <value>
        /// The OriginMessageId.
        /// </value>
        public string OriginMessageId { get; set; }

        /// <summary>
        /// Gets or sets the source node.
        /// </summary>
        /// <value>
        /// The source node.
        /// </value>
        public virtual Node SourceNode { get; set; }

        /// <summary>
        /// Gets or sets the destination node.
        /// </summary>
        /// <value>
        /// The destination node.
        /// </value>
        public virtual Node DestinationNode { get; set; }

        /// <summary>
        /// Gets or sets the destination product.
        /// </summary>
        /// <value>
        /// The destination product.
        /// </value>
        public virtual Product Product { get; set; }

        /// <summary>
        /// Gets or sets the first owner.
        /// </summary>
        /// <value>
        /// The first owner.
        /// </value>
        public virtual CategoryElement Owner1 { get; set; }

        /// <summary>
        /// Gets or sets the second owner.
        /// </summary>
        /// <value>
        /// The second owner.
        /// </value>
        public virtual CategoryElement Owner2 { get; set; }

        /// <summary>
        /// Gets or sets the contract event.
        /// </summary>
        /// <value>
        /// The ownership contract.
        /// </value>
        public virtual CategoryElement MovementType { get; set; }

        /// <summary>
        /// Gets or sets the contract event.
        /// </summary>
        /// <value>
        /// The ownership contract.
        /// </value>
        public virtual CategoryElement MeasurementUnitDetail { get; set; }

        /// <summary>
        /// Gets the destination contracts.
        /// </summary>
        /// <value>
        /// The destination contracts.
        /// </value>
        public virtual ICollection<MovementContract> MovementContracts { get; private set; }

        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            True.Core.ArgumentValidators.ThrowIfNull(entity, nameof(entity));

            var contractObj = (Contract)entity;
            this.Volume = contractObj.Volume;
            this.MeasurementUnit = contractObj.MeasurementUnit;
            this.Owner1Id = contractObj.Owner1Id;
            this.Owner2Id = contractObj.Owner2Id;
            this.SourceNodeId = contractObj.SourceNodeId;
            this.DestinationNodeId = contractObj.DestinationNodeId;
            this.ProductId = contractObj.ProductId;
            this.Volume = contractObj.Volume;
            this.Position = contractObj.Position;
            this.MovementTypeId = contractObj.MovementTypeId;
            this.DocumentNumber = contractObj.DocumentNumber;
            this.StartDate = contractObj.StartDate;
            this.EndDate = contractObj.EndDate;
            this.PurchaseOrderType = contractObj.PurchaseOrderType;
            this.Tolerance = contractObj.Tolerance;
            this.EstimatedVolume = contractObj.EstimatedVolume;
            this.Frequency = contractObj.Frequency;
            this.Status = contractObj.Status;
            this.SourceSystem = contractObj.SourceSystem;
            this.ActionType = contractObj.ActionType;
            this.DescriptionStatus = contractObj.DescriptionStatus;
            this.EventType = contractObj.EventType;
            this.OriginMessageId = contractObj.OriginMessageId;
            this.FileRegistrationTransactionId = contractObj.FileRegistrationTransactionId;
        }
    }
}
