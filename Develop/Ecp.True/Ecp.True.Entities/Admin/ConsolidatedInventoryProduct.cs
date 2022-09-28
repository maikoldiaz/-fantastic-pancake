// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidatedInventoryProduct.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The Consolidated Inventory Product.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class ConsolidatedInventoryProduct : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsolidatedInventoryProduct"/> class.
        /// </summary>
        public ConsolidatedInventoryProduct()
        {
            this.ConsolidatedOwners = new List<ConsolidatedOwner>();
            this.DeltaNodeErrors = new List<DeltaNodeError>();
            this.Movements = new List<Movement>();
        }

        /// <summary>
        /// Gets or sets the consolidated movement identifier.
        /// </summary>
        /// <value>
        /// The consolidated movement identifier.
        /// </value>
        public int ConsolidatedInventoryProductId { get; set; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime InventoryDate { get; set; }

        /// <summary>
        /// Gets or sets the net standard volume.
        /// </summary>
        /// <value>
        /// The net standard volume.
        /// </value>
        public decimal ProductVolume { get; set; }

        /// <summary>
        /// Gets or sets the gross standard quantity.
        /// </summary>
        /// <value>
        /// The gross standard quantity.
        /// </value>
        public decimal? GrossStandardQuantity { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        public string MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public int TicketId { get; set; }

        /// <summary>
        /// Gets or sets the segment identifier.
        /// </summary>
        /// <value>
        /// The segment identifier.
        /// </value>
        public int SegmentId { get; set; }

        /// <summary>
        /// Gets or sets the source system identifier.
        /// </summary>
        /// <value>
        /// The source system identifier.
        /// </value>
        public int SourceSystemId { get; set; }

        /// <summary>
        /// Gets or sets the execution date.
        /// </summary>
        /// <value>
        /// The execution date.
        /// </value>
        public DateTime ExecutionDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        public virtual Node Node { get; set; }

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
        /// Gets or sets the segment.
        /// </summary>
        /// <value>
        /// The segment.
        /// </value>
        public virtual CategoryElement Segment { get; set; }

        /// <summary>
        /// Gets or sets the source system.
        /// </summary>
        /// <value>
        /// The source system.
        /// </value>
        public virtual CategoryElement SourceSystem { get; set; }

        /// <summary>
        /// Gets the consolidated owners.
        /// </summary>
        /// <value>
        /// The consolidated owners.
        /// </value>
        public virtual ICollection<ConsolidatedOwner> ConsolidatedOwners { get; }

        /// <summary>
        /// Gets the delta node errors.
        /// </summary>
        /// <value>
        /// The delta node errors.
        /// </value>
        public virtual ICollection<DeltaNodeError> DeltaNodeErrors { get; }

        /// <summary>
        /// Gets the movements.
        /// </summary>
        /// <value>
        /// The movements.
        /// </value>
        public virtual ICollection<Movement> Movements { get; }
    }
}
