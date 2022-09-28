// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PendingOfficialMovement.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Query
{
    using System;
    using Ecp.True.Entities.Common;

    /// <summary>
    /// The Pending Official Movements.
    /// </summary>
    public class PendingOfficialMovement : QueryEntity, IPrototype<PendingOfficialMovement>
    {
        /// <summary>
        /// Gets or sets the movement transactionId.
        /// </summary>
        /// <value>
        /// The movement transactionId.
        /// </value>
        public int MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the movementId.
        /// </summary>
        /// <value>
        /// The movementId.
        /// </value>
        public string MovementId { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public int OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        public int? SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the SourceNodeSegmentId.
        /// </summary>
        /// <value>
        /// The source node segment identifier.
        /// </value>
        public int? SourceNodeSegmentId { get; set; }

        /// <summary>
        /// Gets or sets the SourceNodeOrder.
        /// </summary>
        /// <value>
        /// The source node order.
        /// </value>
        public int? SourceNodeOrder { get; set; }

        /// <summary>
        /// Gets or sets the SourceNodeSystem.
        /// </summary>
        /// <value>
        /// The source node system.
        /// </value>
        public int? SourceNodeSystem { get; set; }

        /// <summary>
        /// Gets or sets the destination node identifier.
        /// </summary>
        /// <value>
        /// The destination node identifier.
        /// </value>
        public int? DestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination node segment identifier.
        /// </summary>
        /// <value>
        /// The destination node segment identifier.
        /// </value>
        public int? DestinationNodeSegmentID { get; set; }

        /// <summary>
        /// Gets or sets the destination node order.
        /// </summary>
        /// <value>
        /// The destination node order.
        /// </value>
        public int? DestinationNodeOrder { get; set; }

        /// <summary>
        /// Gets or sets the destination node system.
        /// </summary>
        /// <value>
        /// The destination node system.
        /// </value>
        public int? DestinationNodeSystem { get; set; }

        /// <summary>
        /// Gets or sets the source product Id.
        /// </summary>
        /// <value>
        /// The source product Id.
        /// </value>
        public string SourceProductId { get; set; }

        /// <summary>
        /// Gets or sets the DestionationProductID.
        /// </summary>
        /// <value>
        /// The DestionationProductID.
        /// </value>
        public string DestinationProductId { get; set; }

        /// <summary>
        /// Gets or sets the MovementTypeID.
        /// </summary>
        /// <value>
        /// The MovementTypeID.
        /// </value>
        public int MovementTypeID { get; set; }

        /// <summary>
        /// Gets or sets the SystemId.
        /// </summary>
        /// <value>
        /// The SystemId.
        /// </value>
        public int? SystemId { get; set; }

        /// <summary>
        /// Gets or sets the OwnerTransactionID.
        /// </summary>
        /// <value>
        /// The OwnerTransactionID.
        /// </value>
        public decimal OwnerShipValue { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        public int? MeasurementUnit { get; set; }

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
        public DateTime OperationalDate { get; set; }

        /// <summary>
        /// Gets or sets the source product type identifier.
        /// </summary>
        /// <value>
        /// The source product type identifier.
        /// </value>
        public int? SourceProductTypeId { get; set; }

        /// <summary>
        /// Gets or sets the destination product type identifier.
        /// </summary>
        /// <value>
        /// The destination product type identifier.
        /// </value>
        public int? DestinationProductTypeId { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        /// <value>
        /// The end time.
        /// </value>
        public DateTime? EndDate { get; set; }

        /// <inheritdoc />
        public PendingOfficialMovement ShallowCopy()
        {
            return this.MemberwiseClone() as PendingOfficialMovement;
        }
    }
}
