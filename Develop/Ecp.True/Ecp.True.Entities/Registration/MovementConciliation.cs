// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementConciliation.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Registration
{
    using System;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;

    /// <summary>
    ///  The MovementConciliationDto.
    /// </summary>
    public class MovementConciliation : Entity
    {
        /// <summary>
        /// Gets or sets the movement conciliation identifier.
        /// </summary>
        /// <value>
        /// The movement conciliation identifier.
        /// </value>
        public int MovementConciliationId { get; set; }

        /// <summary>
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement transaction identifier.
        /// </value>
        public int? MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the movement type identifier.
        /// </summary>
        /// <value>
        /// The movement type identifier.
        /// </value>
        public int? MovementTypeId { get; set; }

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
        /// Gets or sets the source product identifier.
        /// </summary>
        /// <value>
        /// The source product identifier.
        /// </value>
        public string SourceProductId { get; set; }

        /// <summary>
        /// Gets or sets the destination product identifier.
        /// </summary>
        /// <value>
        /// The destination product identifier.
        /// </value>
        public string DestinationProductId { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        /// <value>
        /// The ownership volume.
        /// </value>
        public decimal? OwnershipVolume { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public int? OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the unit identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public int? MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets the segment identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public int? SegmentId { get; set; }

        /// <summary>
        /// Gets or sets the percentage owner.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public decimal? OwnershipPercentage { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public decimal? NetStandardVolume { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Sign.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public string Sign { get; set; }

        /// <summary>
        /// Gets or sets the delta conciliated.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public decimal? DeltaConciliated { get; set; }

        /// <summary>
        /// Gets or sets OperationalDate.
        /// </summary>
        /// <value>
        /// The OperationalDate.
        /// </value>
        public DateTime? OperationalDate { get; set; }

        /// <summary>
        /// Gets or sets UncertaintyPercentage.
        /// </summary>
        /// <value>
        /// The UncertaintyPercentage.
        /// </value>
        public decimal? UncertaintyPercentage { get; set; }

        /// <summary>
        /// Gets or sets CollectionType.
        /// </summary>
        /// <value>
        /// The CollectionType.
        /// </value>
        public ConciliationMovementCollectionType CollectionType { get; set; }

        /// <summary>
        /// Gets or sets the Ownership Ticket Conciliation identifier.
        /// </summary>
        /// <value>
        /// The Ownership Ticket Conciliation identifier.
        /// </value>
        public int OwnershipTicketConciliationId { get; set; }
    }
}