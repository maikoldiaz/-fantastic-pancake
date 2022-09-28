// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryElement.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Admin
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using EfCore.Models;
    using Newtonsoft.Json;

    /// <summary>
    /// The category element.
    /// </summary>
    public class CategoryElement : EditableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryElement"/> class.
        /// </summary>
        public CategoryElement()
        {
            this.FileRegistrations = new List<FileRegistration>();
            this.InventoryProducts = new List<InventoryProduct>();
            this.Movements = new List<Movement>();
            this.NodeStorageLocations = new List<NodeStorageLocation>();
            this.NodeConnectionProductOwners = new List<NodeConnectionProductOwner>();
            this.NodeTags = new List<NodeTag>();
            this.StorageLocationProducts = new List<StorageLocationProduct>();
            this.StorageLocationProductOwners = new List<StorageLocationProductOwner>();
            this.Tickets = new List<Ticket>();
            this.DestinationTransformations = new List<Transformation>();
            this.OriginTransformations = new List<Transformation>();
            this.Attributes = new List<AttributeEntity>();
            this.ValueAttributeUnits = new List<AttributeEntity>();
            this.Initialize();
        }

        /// <summary>
        /// Gets or sets the element identifier.
        /// </summary>
        /// <value>
        /// The element identifier.
        /// </value>
        public int ElementId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.CategoryElementNameRequired)]
        [StringLength(150, ErrorMessage = Entities.Constants.NameMaxLength150)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the element description.
        /// </summary>
        /// <value>
        /// The element description.
        /// </value>
        [StringLength(1000, ErrorMessage = Entities.Constants.DescriptionMaxLength1000)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.ElementStatusRequired)]
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.CategoryIdRequired)]
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the icon identifier.
        /// </summary>
        /// <value>
        /// The icon identifier.
        /// </value>
        public int? IconId { get; set; }

        /// <summary>
        /// Gets or sets the color identifier.
        /// </summary>
        /// <value>
        /// The color identifier.
        /// </value>
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the flag for operational segment.
        /// </summary>
        /// <value>
        /// The  the flag for operational segment.
        /// </value>
        public bool? IsOperationalSegment { get; set; }

        /// <summary>
        /// Gets or sets the Deviation Percentage segment.
        /// </summary>
        /// <value>
        /// The  the Deviation Percentage segment.
        /// </value>
        public decimal? DeviationPercentage { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public virtual Category Category { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>
        /// The icon.
        /// </value>
        public virtual Icon Icon { get; set; }

        /// <summary>
        /// Gets the node storage locations.
        /// </summary>
        /// <value>
        /// The node storage locations.
        /// </value>
        [JsonIgnore]
        public ICollection<NodeStorageLocation> NodeStorageLocations { get; private set; }

        /// <summary>Gets the storage location product owners.</summary>
        /// <value>The storage location product owners.</value>
        [JsonIgnore]
        public ICollection<StorageLocationProductOwner> StorageLocationProductOwners { get; private set; }

        /// <summary>
        /// Gets the storage location products.
        /// </summary>
        /// <value>
        /// The storage location products.
        /// </value>
        public virtual ICollection<StorageLocationProduct> StorageLocationProducts { get; private set; }

        /// <summary>
        /// Gets the node category tagging.
        /// </summary>
        /// <value>
        /// The node category tagging.
        /// </value>
        public virtual ICollection<NodeTag> NodeTags { get; private set; }

        /// <summary>
        /// Gets the tickets.
        /// </summary>
        /// <value>
        /// The tickets.
        /// </value>
        [JsonIgnore]
        public virtual ICollection<Ticket> Tickets { get; private set; }

        /// <summary>
        /// Gets the node connection product owners.
        /// </summary>
        /// <value>
        /// The node connection product owners.
        /// </value>
        [JsonIgnore]
        public ICollection<NodeConnectionProductOwner> NodeConnectionProductOwners { get; private set; }

        /// <summary>
        /// Gets the transformation destination measurement.
        /// </summary>
        /// <value>
        /// The transformation destination measurement.
        /// </value>
        public virtual ICollection<Transformation> DestinationTransformations { get; private set; }

        /// <summary>
        /// Gets the transformation origin measurement.
        /// </summary>
        /// <value>
        /// The transformation origin measurement.
        /// </value>
        public virtual ICollection<Transformation> OriginTransformations { get; private set; }

        /// <summary>
        /// Gets the file registrations.
        /// </summary>
        /// <value>
        /// The file registrations.
        /// </value>
        public virtual ICollection<FileRegistration> FileRegistrations { get; private set; }

        /// <summary>
        /// Gets the movements.
        /// </summary>
        /// <value>
        /// The movements.
        /// </value>
        public virtual ICollection<Movement> Movements { get; private set; }

        /// <summary>
        /// Gets the ownership calculation result.
        /// </summary>
        /// <value>
        /// The ownership calculation result.
        /// </value>
        public virtual ICollection<OwnershipCalculationResult> OwnershipCalculationResults { get; private set; }

        /// <summary>
        /// Gets the ownership.
        /// </summary>
        /// <value>
        /// The ownership.
        /// </value>
        public virtual ICollection<Ownership> Ownerships { get; private set; }

        /// <summary>
        /// Gets the first event owners.
        /// </summary>
        /// <value>
        /// The first event owners.
        /// </value>
        public virtual ICollection<Event> Owner1Events { get; private set; }

        /// <summary>
        /// Gets the event types.
        /// </summary>
        /// <value>
        /// The event types.
        /// </value>
        public virtual ICollection<Event> EventTypes { get; private set; }

        /// <summary>
        /// Gets the ticket owners.
        /// </summary>
        /// <value>
        /// The ticket owners.
        /// </value>
        public virtual ICollection<Ticket> TicketOwners { get; private set; }

        /// <summary>
        /// Gets the pending transaction owners.
        /// </summary>
        /// <value>
        /// The pending transaction owners.
        /// </value>
        public virtual ICollection<PendingTransaction> PendingTransactionOwners { get; private set; }

        /// <summary>
        /// Gets the pending transaction types.
        /// </summary>
        /// <value>
        /// The pending transaction types.
        /// </value>
        public virtual ICollection<PendingTransaction> PendingTransactionTypes { get; private set; }

        /// <summary>
        /// Gets the ownership calculations.
        /// </summary>
        /// <value>
        /// The ownership calculations.
        /// </value>
        public virtual ICollection<OwnershipCalculation> OwnershipCalculations { get; private set; }

        /// <summary>
        /// Gets the segment ownership calculations.
        /// </summary>
        /// <value>
        /// The segment ownership calculations.
        /// </value>
        public virtual ICollection<SegmentOwnershipCalculation> SegmentOwnerships { get; private set; }

        /// <summary>
        /// Gets the segment ownership owners.
        /// </summary>
        /// <value>
        /// The segment ownership owners.
        /// </value>
        public virtual ICollection<SegmentOwnershipCalculation> SegmentOwnershipOwners { get; private set; }

        /// <summary>
        /// Gets the system ownership segments.
        /// </summary>
        /// <value>
        /// The system ownership segments.
        /// </value>
        public virtual ICollection<SystemOwnershipCalculation> SystemOwnershipSegments { get; private set; }

        /// <summary>
        /// Gets the system ownership owners.
        /// </summary>
        /// <value>
        /// The system ownership owners.
        /// </value>
        public virtual ICollection<SystemOwnershipCalculation> SystemOwnershipOwners { get; private set; }

        /// <summary>
        /// Gets the system ownerships.
        /// </summary>
        /// <value>
        /// The system ownerships.
        /// </value>
        public virtual ICollection<SystemOwnershipCalculation> SystemOwnerships { get; private set; }

        /// <summary>
        /// Gets the system unbalances.
        /// </summary>
        /// <value>
        /// The system unbalances.
        /// </value>
        public virtual ICollection<SystemUnbalance> SystemUnbalances { get; private set; }

        /// <summary>
        /// Gets the system unbalance segments.
        /// </summary>
        /// <value>
        /// The system unbalance segments.
        /// </value>
        public virtual ICollection<SystemUnbalance> SystemUnbalanceSegments { get; private set; }

        /// <summary>
        /// Gets the segment unbalances.
        /// </summary>
        /// <value>
        /// The segment unbalances.
        /// </value>
        public virtual ICollection<SegmentUnbalance> SegmentUnbalances { get; private set; }

        /// <summary>
        /// Gets the reason movements.
        /// </summary>
        /// <value>
        /// The reason movements.
        /// </value>
        public virtual ICollection<Movement> ReasonMovements { get; private set; }

        /// <summary>
        /// Gets the reason movements.
        /// </summary>
        /// <value>
        /// The reason movements.
        /// </value>
        public virtual ICollection<InventoryProduct> ReasonInventoryProducts { get; private set; }

        /// <summary>
        /// Gets the system inventory products.
        /// </summary>
        /// <value>
        /// The system inventory products.
        /// </value>
        public virtual ICollection<InventoryProduct> SystemInventoryProducts { get; private set; }

        /// <summary>
        /// Gets the system movements.
        /// </summary>
        /// <value>
        /// The system movements.
        /// </value>
        public virtual ICollection<Movement> SystemMovements { get; private set; }

        /// <summary>
        /// Gets the operator movements.
        /// </summary>
        /// <value>
        /// The operator movements.
        /// </value>
        public virtual ICollection<Movement> OperatorMovements { get; private set; }

        /// <summary>
        /// Gets the operator inventory products.
        /// </summary>
        /// <value>
        /// The operator inventory products.
        /// </value>
        public virtual ICollection<InventoryProduct> OperatorInventoryProducts { get; private set; }

        /// <summary>
        /// Gets the source system movements.
        /// </summary>
        /// <value>
        /// The source system movements.
        /// </value>
        public virtual ICollection<Movement> SourceSystemMovements { get; private set; }

        /// <summary>
        /// Gets the source system inventory products.
        /// </summary>
        /// <value>
        /// The source system inventory products.
        /// </value>
        public virtual ICollection<InventoryProduct> SourceSystemInventoryProducts { get; private set; }

        /// <summary>
        /// Gets the inventory products.
        /// </summary>
        /// <value>
        /// The inventory products.
        /// </value>
        public virtual ICollection<InventoryProduct> InventoryProducts { get; private set; }

        /// <summary>
        /// Gets the first contract owners.
        /// </summary>
        /// <value>
        /// The first contract owners.
        /// </value>
        public virtual ICollection<Contract> Owner1Contracts { get; private set; }

        /// <summary>
        /// Gets the contract types.
        /// </summary>
        /// <value>
        /// The contract types.
        /// </value>
        public virtual ICollection<Contract> ContractMovementTypes { get; private set; }

        /// <summary>
        /// Gets the contract units.
        /// </summary>
        /// <value>
        /// The contract units.
        /// </value>
        public virtual ICollection<Contract> Units { get; private set; }

        /// <summary>
        /// Gets the second event owners.
        /// </summary>
        /// <value>
        /// The second event owners.
        /// </value>
        public virtual ICollection<Event> Owner2Events { get; private set; }

        /// <summary>
        /// Gets the second contract owners.
        /// </summary>
        /// <value>
        /// The second contract owners.
        /// </value>
        public virtual ICollection<Contract> Owner2Contracts { get; private set; }

        /// <summary>
        /// Gets the node units.
        /// </summary>
        /// <value>
        /// The node units.
        /// </value>
        public virtual ICollection<Node> NodeUnits { get; private set; }

        /// <summary>
        /// Gets the movement contract types.
        /// </summary>
        /// <value>
        /// The movement contract types.
        /// </value>
        public virtual ICollection<MovementContract> MovementContractMovementTypes { get; private set; }

        /// <summary>
        /// Gets the owner1 movement contracts.
        /// </summary>
        /// <value>
        /// The owner1 movement contracts.
        /// </value>
        public virtual ICollection<MovementContract> Owner1MovementContracts { get; private set; }

        /// <summary>
        /// Gets the owner2 movement contracts.
        /// </summary>
        /// <value>
        /// The owner2 movement contracts.
        /// </value>
        public virtual ICollection<MovementContract> Owner2MovementContracts { get; private set; }

        /// <summary>
        /// Gets the movement contract units.
        /// </summary>
        /// <value>
        /// The movement contract units.
        /// </value>
        public virtual ICollection<MovementContract> MovementContractUnits { get; private set; }

        /// <summary>
        /// Gets the movement event types.
        /// </summary>
        /// <value>
        /// The movement event types.
        /// </value>
        public virtual ICollection<MovementEvent> MovementEventTypes { get; private set; }

        /// <summary>
        /// Gets the owner1 movement events.
        /// </summary>
        /// <value>
        /// The owner1 movement events.
        /// </value>
        public virtual ICollection<MovementEvent> Owner1MovementEvents { get; private set; }

        /// <summary>
        /// Gets the owner2 movement events.
        /// </summary>
        /// <value>
        /// The owner2 movement events.
        /// </value>
        public virtual ICollection<MovementEvent> Owner2MovementEvents { get; private set; }

        /// <summary>
        /// Gets the Source Movements.
        /// </summary>
        /// <value>
        /// The Source Movements.
        /// </value>
        public virtual ICollection<Annulation> AnnulationSourceMovements { get; private set; }

        /// <summary>
        /// Gets the Annulation Movements.
        /// </summary>
        /// <value>
        /// The Annulation Movements.
        /// </value>
        public virtual ICollection<Annulation> AnnulationMovements { get; private set; }

        /// <summary>
        /// Gets the Annulation SAP Transaction.
        /// </summary>
        /// <value>
        /// The Annulation Movements.
        /// </value>
        public virtual ICollection<Annulation> AnnulationSapTransaction { get; private set; }

        /// <summary>
        /// Gets the consolidated movement segments.
        /// </summary>
        /// <value>
        /// The consolidated movement segments.
        /// </value>
        public virtual ICollection<ConsolidatedMovement> ConsolidatedMovementSegments { get; private set; }

        /// <summary>
        /// Gets the consolidated movement source systems.
        /// </summary>
        /// <value>
        /// The consolidated movement source systems.
        /// </value>
        public virtual ICollection<ConsolidatedMovement> ConsolidatedMovementSourceSystems { get; private set; }

        /// <summary>
        /// Gets the consolidated inventory product owners.
        /// </summary>
        /// <value>
        /// The consolidated inventory product owners.
        /// </value>
        public virtual ICollection<ConsolidatedOwner> ConsolidatedOwners { get; private set; }

        /// <summary>
        /// Gets the consolidated inventory product segments.
        /// </summary>
        /// <value>
        /// The consolidated inventory product segments.
        /// </value>
        public virtual ICollection<ConsolidatedInventoryProduct> ConsolidatedInventoryProductSegments { get; private set; }

        /// <summary>
        /// Gets the consolidated inventory product source systems.
        /// </summary>
        /// <value>
        /// The consolidated inventory product source systems.
        /// </value>
        public virtual ICollection<ConsolidatedInventoryProduct> ConsolidatedInventoryProductSourceSystems { get; private set; }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public virtual ICollection<AttributeEntity> Attributes { get; private set; }

        /// <summary>
        /// Gets the value attribute units.
        /// </summary>
        /// <value>
        /// The value attribute units.
        /// </value>
        public virtual ICollection<AttributeEntity> ValueAttributeUnits { get; private set; }

        /// <summary>
        /// Gets the report executions.
        /// </summary>
        /// <value>
        /// The report executions.
        /// </value>
        public virtual ICollection<ReportExecution> ReportExecutions { get; private set; }

        /// <summary>
        /// Gets the grand owners.
        /// </summary>
        /// <value>
        /// The grand owners.
        /// </value>
        public virtual ICollection<PartnerOwnerMapping> GrandOwners { get; private set; }

        /// <summary>
        /// Gets the partner owners.
        /// </summary>
        /// <value>
        /// The partner owners.
        /// </value>
        public virtual ICollection<PartnerOwnerMapping> PartnerOwners { get; private set; }

        /// <summary>
        /// Gets the inventory product product types.
        /// </summary>
        /// <value>
        /// The inventory product product types.
        /// </value>
        public virtual ICollection<InventoryProduct> InventoryProductProductTypes { get; private set; }

        /// <summary>
        /// Gets the inventory product measurement units.
        /// </summary>
        /// <value>
        /// The inventory product measurement units.
        /// </value>
        public virtual ICollection<InventoryProduct> InventoryProductMeasurementUnits { get; private set; }

        /// <summary>
        /// Gets the movement movement types.
        /// </summary>
        /// <value>
        /// The movement movement types.
        /// </value>
        public virtual ICollection<Movement> MovementMovementTypes { get; private set; }

        /// <summary>
        /// Gets the movement measurement units.
        /// </summary>
        /// <value>
        /// The movement measurement units.
        /// </value>
        public virtual ICollection<Movement> MovementMeasurementUnits { get; private set; }

        /// <summary>
        /// Gets the movement source product types.
        /// </summary>
        /// <value>
        /// The movement source product types.
        /// </value>
        public virtual ICollection<MovementSource> MovementSourceProductTypes { get; private set; }

        /// <summary>
        /// Gets the movement destination product types.
        /// </summary>
        /// <value>
        /// The movement destination product types.
        /// </value>
        public virtual ICollection<MovementDestination> MovementDestinationProductTypes { get; private set; }

        /// <summary>
        /// Gets the owners.
        /// </summary>
        /// <value>
        /// The owners.
        /// </value>
        public virtual ICollection<Owner> Owners { get; private set; }

        /// <summary>
        /// Gets the pending transaction units.
        /// </summary>
        /// <value>
        /// The pending transaction units.
        /// </value>
        public virtual ICollection<PendingTransaction> PendingTransactionUnits { get; private set; }

        /// <summary>
        /// Gets the Node CostCenter Movements.
        /// </summary>
        /// <value>
        /// The Node Cost Center Movements.
        /// </value>
        public virtual ICollection<NodeCostCenter> MovementTypeNodeCostCenter { get; private set; }

        /// <summary>
        /// Gets the Node Cost Center Movements.
        /// </summary>
        /// <value>
        /// The Node Cost Center Movements.
        /// </value>
        public virtual ICollection<NodeCostCenter> CostCenterNodeCostCenter { get; private set; }

        /// <summary>
        /// Gets the Node Cost Center Movements.
        /// </summary>
        /// <value>
        /// The Node Cost Center Movements.
        /// </value>
        public virtual ICollection<LogisticMovement> LogisticMovements { get; private set; }

        /// <inheritdoc/>
        public override void CopyFrom(IEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var element = (CategoryElement)entity;

            this.Name = element.Name ?? this.Name;
            this.Description = element.Description ?? this.Description;
            this.IsActive = element.IsActive ?? this.IsActive;
            this.CategoryId = element.CategoryId ?? this.CategoryId;
            this.IconId = element.IconId ?? this.IconId;
            this.Color = element.Color ?? this.Color;
            this.RowVersion = element.RowVersion;
            this.DeviationPercentage = element.DeviationPercentage ?? this.DeviationPercentage;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            this.OwnershipCalculationResults = new List<OwnershipCalculationResult>();
            this.Ownerships = new List<Ownership>();
            this.Owner1Events = new List<Event>();
            this.EventTypes = new List<Event>();
            this.Owner1Contracts = new List<Contract>();
            this.ContractMovementTypes = new List<Contract>();
            this.Units = new List<Contract>();
            this.TicketOwners = new List<Ticket>();
            this.PendingTransactionOwners = new List<PendingTransaction>();
            this.PendingTransactionTypes = new List<PendingTransaction>();
            this.OwnershipCalculations = new List<OwnershipCalculation>();
            this.SegmentOwnerships = new List<SegmentOwnershipCalculation>();
            this.SegmentOwnershipOwners = new List<SegmentOwnershipCalculation>();
            this.SystemOwnerships = new List<SystemOwnershipCalculation>();
            this.SystemOwnershipOwners = new List<SystemOwnershipCalculation>();
            this.SystemOwnershipSegments = new List<SystemOwnershipCalculation>();
            this.SystemUnbalances = new List<SystemUnbalance>();
            this.InitializeMore();
        }

        /// <summary>
        /// Initializes the more.
        /// </summary>
        private void InitializeMore()
        {
            this.SystemUnbalanceSegments = new List<SystemUnbalance>();
            this.SegmentUnbalances = new List<SegmentUnbalance>();
            this.ReasonMovements = new List<Movement>();
            this.ReasonInventoryProducts = new List<InventoryProduct>();
            this.SystemInventoryProducts = new List<InventoryProduct>();
            this.OperatorMovements = new List<Movement>();
            this.OperatorInventoryProducts = new List<InventoryProduct>();
            this.SourceSystemMovements = new List<Movement>();
            this.SourceSystemInventoryProducts = new List<InventoryProduct>();
            this.SystemMovements = new List<Movement>();
            this.Owner2Events = new List<Event>();
            this.Owner2Contracts = new List<Contract>();
            this.NodeUnits = new List<Node>();
            this.MovementContractMovementTypes = new List<MovementContract>();
            this.Owner1MovementContracts = new List<MovementContract>();
            this.Owner2MovementContracts = new List<MovementContract>();
            this.MovementContractUnits = new List<MovementContract>();
            this.MovementEventTypes = new List<MovementEvent>();
            this.Owner1MovementEvents = new List<MovementEvent>();
            this.Owner2MovementEvents = new List<MovementEvent>();
            this.AnnulationSourceMovements = new List<Annulation>();
            this.AnnulationMovements = new List<Annulation>();
            this.ConsolidatedMovementSegments = new List<ConsolidatedMovement>();
            this.ConsolidatedMovementSourceSystems = new List<ConsolidatedMovement>();
            this.ConsolidatedOwners = new List<ConsolidatedOwner>();
            this.ConsolidatedInventoryProductSegments = new List<ConsolidatedInventoryProduct>();
            this.ConsolidatedInventoryProductSourceSystems = new List<ConsolidatedInventoryProduct>();
            this.ReportExecutions = new List<ReportExecution>();
            this.GrandOwners = new List<PartnerOwnerMapping>();
            this.PartnerOwners = new List<PartnerOwnerMapping>();
            this.InventoryProductProductTypes = new List<InventoryProduct>();
            this.InventoryProductMeasurementUnits = new List<InventoryProduct>();
            this.MovementMovementTypes = new List<Movement>();
            this.MovementMeasurementUnits = new List<Movement>();
            this.MovementSourceProductTypes = new List<MovementSource>();
            this.MovementDestinationProductTypes = new List<MovementDestination>();
            this.Owners = new List<Owner>();
            this.PendingTransactionUnits = new List<PendingTransaction>();
        }
    }
}