// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Inventory.cs" company="Microsoft">
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
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The Inventory class.
    /// </summary>
    public class Inventory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Inventory"/> class.
        /// </summary>
        public Inventory()
        {
            this.Products = new List<InventoryProduct>();
            this.Results = new List<OwnershipResult>();
        }

        /// <summary>
        /// Gets or sets the inventory transaction identifier.
        /// </summary>
        /// <value>
        /// The inventory transaction identifier.
        /// </value>
        public int InventoryTransactionId { get; set; }

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
        [Range(1, int.MaxValue, ErrorMessage = Constants.NodeIdRequired)]
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
        [StringLength(50, ErrorMessage = Constants.ScenarioLengthExceeded)]
        public string Scenario { get; set; }

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
        public string Operator { get; set; }

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
        /// Gets the inventory product.
        /// </summary>
        /// <value>
        /// The inventory product.
        /// </value>
        [MustNotBeEmpty(ErrorMessage = Constants.ProductsRequired)]
        public virtual ICollection<InventoryProduct> Products { get; }

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
    }
}