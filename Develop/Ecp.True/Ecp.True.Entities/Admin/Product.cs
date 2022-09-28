// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Product.cs" company="Microsoft">
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
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Newtonsoft.Json;

    /// <summary>
    /// The Product.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class Product : EditableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        public Product()
        {
            this.ProductInventory = new List<InventoryProduct>();
            this.Destinations = new List<MovementDestination>();
            this.Sources = new List<MovementSource>();
            this.ProductLocations = new List<StorageLocationProduct>();
            this.NodeConnectionProducts = new List<NodeConnectionProduct>();
            this.UnbalanceComments = new List<UnbalanceComment>();
            this.Unbalances = new List<Unbalance>();
            this.OwnershipCalculations = new List<OwnershipCalculation>();
            this.OwnershipResults = new List<OwnershipResult>();
            this.Initialize();
        }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [MaxLength(20, ErrorMessage = Constants.Max20Characters)]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        [MaxLength(150, ErrorMessage = Constants.Max150Characters)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        [Required]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets the product locations.
        /// </summary>
        /// <value>
        /// The product locations.
        /// </value>
        public virtual ICollection<StorageLocationProduct> ProductLocations { get; private set; }

        /// <summary>
        /// Gets the node connection products.
        /// </summary>
        /// <value>
        /// The node connection products.
        /// </value>
        public virtual ICollection<NodeConnectionProduct> NodeConnectionProducts { get; private set; }

        /// <summary>
        /// Gets the destinations.
        /// </summary>
        /// <value>
        /// The destinations.
        /// </value>
        public virtual ICollection<MovementDestination> Destinations { get; private set; }

        /// <summary>
        /// Gets the sources.
        /// </summary>
        /// <value>
        /// The sources.
        /// </value>
        public virtual ICollection<MovementSource> Sources { get; private set; }

        /// <summary>
        /// Gets the product inventory.
        /// </summary>
        /// <value>
        /// The product inventory.
        /// </value>
        public virtual ICollection<InventoryProduct> ProductInventory { get; private set; }

        /// <summary>
        /// Gets the unbalance data.
        /// </summary>
        /// <value>
        /// The unbalance data.
        /// </value>
        [JsonIgnore]
        public virtual ICollection<UnbalanceComment> UnbalanceComments { get; private set; }

        /// <summary>
        /// Gets the unbalances.
        /// </summary>
        /// <value>
        /// The unbalances.
        /// </value>
        [JsonIgnore]
        public virtual ICollection<Unbalance> Unbalances { get; private set; }

        /// <summary>
        /// Gets the ownership calculations.
        /// </summary>
        /// <value>
        /// The ownership calculations.
        /// </value>
        public virtual ICollection<OwnershipCalculation> OwnershipCalculations { get; private set; }

        /// <summary>
        /// Gets the ownership results.
        /// </summary>
        /// <value>
        /// The ownership results.
        /// </value>
        public virtual ICollection<OwnershipResult> OwnershipResults { get; private set; }

        /// <summary>
        /// Gets the transformation destination destination products.
        /// </summary>
        /// <value>
        /// The transformation destination destination products.
        /// </value>
        public virtual ICollection<Transformation> DestinationDestinationProductTransformations { get; private set; }

        /// <summary>
        /// Gets the transformation destination source products.
        /// </summary>
        /// <value>
        /// The transformation destination source products.
        /// </value>
        public virtual ICollection<Transformation> DestinationSourceProductTransformations { get; private set; }

        /// <summary>
        /// Gets the transformation origin destination products.
        /// </summary>
        /// <value>
        /// The transformation origin destination products.
        /// </value>
        public virtual ICollection<Transformation> OriginDestinationProductTransformations { get; private set; }

        /// <summary>
        /// Gets the transformation origin source products.
        /// </summary>
        /// <value>
        /// The transformation origin source products.
        /// </value>
        public virtual ICollection<Transformation> OriginSourceProductTransformations { get; private set; }

        /// <summary>
        /// Gets the Storage Location Product Mapping.
        /// </summary>
        /// <value>
        /// The Storage Location Product Mapping.
        /// </value>
        public virtual ICollection<StorageLocationProductMapping> StorageLocationProductMappings { get; private set; }

        /// <summary>
        /// Gets the destination events.
        /// </summary>
        /// <value>
        /// The destination events.
        /// </value>
        public virtual ICollection<Event> DestinationEvents { get; private set; }

        /// <summary>
        /// Gets the source events.
        /// </summary>
        /// <value>
        /// The source events.
        /// </value>
        public virtual ICollection<Event> SourceEvents { get; private set; }

        /// <summary>
        /// Gets the segment ownership calculations.
        /// </summary>
        /// <value>
        /// The segment ownership calculations.
        /// </value>
        public virtual ICollection<SegmentOwnershipCalculation> SegmentOwnershipCalculations { get; private set; }

        /// <summary>
        /// Gets the system ownership calculations.
        /// </summary>
        /// <value>
        /// The system ownership calculations.
        /// </value>
        public virtual ICollection<SystemOwnershipCalculation> SystemOwnershipCalculations { get; private set; }

        /// <summary>
        /// Gets the segment unbalance.
        /// </summary>
        /// <value>
        /// The segment unbalance.
        /// </value>
        public virtual ICollection<SegmentUnbalance> SegmentUnbalances { get; private set; }

        /// <summary>
        /// Gets the system unbalance.
        /// </summary>
        /// <value>
        /// The system unbalance.
        /// </value>
        public virtual ICollection<SystemUnbalance> SystemUnbalances { get; private set; }

        /// <summary>
        /// Gets the destination contracts.
        /// </summary>
        /// <value>
        /// The destination contracts.
        /// </value>
        public virtual ICollection<Contract> Contracts { get; private set; }

        /// <summary>
        /// Gets the movement contracts.
        /// </summary>
        /// <value>
        /// The movement contracts.
        /// </value>
        public virtual ICollection<MovementContract> MovementContracts { get; private set; }

        /// <summary>
        /// Gets the source movement events.
        /// </summary>
        /// <value>
        /// The source movement events.
        /// </value>
        public virtual ICollection<MovementEvent> SourceMovementEvents { get; private set; }

        /// <summary>
        /// Gets the destination movement events.
        /// </summary>
        /// <value>
        /// The destination movement events.
        /// </value>
        public virtual ICollection<MovementEvent> DestinationMovementEvents { get; private set; }

        /// <summary>
        /// Gets the source consolidated movements.
        /// </summary>
        /// <value>
        /// The source consolidated movements.
        /// </value>
        public virtual ICollection<ConsolidatedMovement> SourceConsolidatedMovements { get; private set; }

        /// <summary>
        /// Gets the destination consolidated movements.
        /// </summary>
        /// <value>
        /// The destination consolidated movements.
        /// </value>
        public virtual ICollection<ConsolidatedMovement> DestinationConsolidatedMovements { get; private set; }

        /// <summary>
        /// Gets the consolidated inventory products.
        /// </summary>
        /// <value>
        /// The consolidated inventory products.
        /// </value>
        public virtual ICollection<ConsolidatedInventoryProduct> ConsolidatedInventoryProducts { get; private set; }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            this.DestinationDestinationProductTransformations = new List<Transformation>();
            this.DestinationSourceProductTransformations = new List<Transformation>();
            this.OriginDestinationProductTransformations = new List<Transformation>();
            this.OriginSourceProductTransformations = new List<Transformation>();
            this.StorageLocationProductMappings = new List<StorageLocationProductMapping>();
            this.DestinationEvents = new List<Event>();
            this.SourceEvents = new List<Event>();
            this.SegmentOwnershipCalculations = new List<SegmentOwnershipCalculation>();
            this.SystemOwnershipCalculations = new List<SystemOwnershipCalculation>();
            this.SegmentUnbalances = new List<SegmentUnbalance>();
            this.SystemUnbalances = new List<SystemUnbalance>();
            this.Contracts = new List<Contract>();
            this.MovementContracts = new List<MovementContract>();
            this.InitializeMore();
        }

        /// <summary>
        /// Initializes the more.
        /// </summary>
        private void InitializeMore()
        {
            this.SourceMovementEvents = new List<MovementEvent>();
            this.DestinationMovementEvents = new List<MovementEvent>();
            this.SourceConsolidatedMovements = new List<ConsolidatedMovement>();
            this.DestinationConsolidatedMovements = new List<ConsolidatedMovement>();
            this.ConsolidatedInventoryProducts = new List<ConsolidatedInventoryProduct>();
        }
    }
}