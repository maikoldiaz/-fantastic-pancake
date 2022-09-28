// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidatedMovement.cs" company="Microsoft">
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
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The Consolidated Movement.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class ConsolidatedMovement : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsolidatedMovement"/> class.
        /// </summary>
        public ConsolidatedMovement()
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
        public int ConsolidatedMovementId { get; set; }

        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        public int? SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the source product identifier.
        /// </summary>
        /// <value>
        /// The source product identifier.
        /// </value>
        public string SourceProductId { get; set; }

        /// <summary>
        /// Gets or sets the destination node identifier.
        /// </summary>
        /// <value>
        /// The destination node identifier.
        /// </value>
        public int? DestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination product identifier.
        /// </summary>
        /// <value>
        /// The destination product identifier.
        /// </value>
        public string DestinationProductId { get; set; }

        /// <summary>
        /// Gets or sets the movement type identifier.
        /// </summary>
        /// <value>
        /// The movement type identifier.
        /// </value>
        [Required(ErrorMessage = Constants.MovementTypeRequired)]
        public string MovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the net standard volume.
        /// </summary>
        /// <value>
        /// The net standard volume.
        /// </value>
        public decimal NetStandardVolume { get; set; }

        /// <summary>
        /// Gets or sets the gross standard volume.
        /// </summary>
        /// <value>
        /// The gross standard volume.
        /// </value>
        public decimal? GrossStandardVolume { get; set; }

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
        public int? SourceSystemId { get; set; }

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
        /// Gets or sets the source node.
        /// </summary>
        /// <value>
        /// The source node.
        /// </value>
        public virtual Node SourceNode { get; set; }

        /// <summary>
        /// Gets or sets the source product.
        /// </summary>
        /// <value>
        /// The source product.
        /// </value>
        public virtual Product SourceProduct { get; set; }

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
        public virtual Product DestinationProduct { get; set; }

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
