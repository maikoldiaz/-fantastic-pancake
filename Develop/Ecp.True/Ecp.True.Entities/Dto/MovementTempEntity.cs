// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementTempEntity.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Dto
{
    using System;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Interfaces;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The MovementTempEntity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Interfaces.ITempEntity" />
    public class MovementTempEntity : ITempEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovementTempEntity"/> class.
        /// </summary>
        /// <param name="movement">The movement.</param>
        /// <param name="tempId">The temporary identifier.</param>
        /// <param name="createdBy">The created by.</param>
        public MovementTempEntity(Movement movement, int tempId, string createdBy)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));

            this.TempId = tempId;
            this.MovementTypeId = movement.MovementTypeId;
            this.MessageTypeId = movement.MessageTypeId;
            this.SystemTypeId = movement.SystemTypeId;
            this.SourceSystemId = movement.SourceSystemId;
            this.EventType = movement.EventType;
            this.MovementId = movement.MovementId;
            this.IsSystemGenerated = movement.IsSystemGenerated;
            this.OfficialDeltaTicketId = movement.OfficialDeltaTicketId;
            this.ScenarioId = (int)movement.ScenarioId;
            this.Observations = movement.Observations;
            this.Classification = movement.Classification;
            this.PendingApproval = movement.PendingApproval;
            this.NetStandardVolume = movement.NetStandardVolume;
            this.SourceMovementTransactionId = movement.SourceMovementTransactionId;
            this.MeasurementUnit = movement.MeasurementUnit;
            this.SegmentId = movement.SegmentId;
            this.OperationalDate = movement.OperationalDate;
            this.OfficialDeltaMessageTypeId = movement.OfficialDeltaMessageTypeId != null ? (int)movement.OfficialDeltaMessageTypeId : (int?)null;
            this.PeriodStartTime = movement.Period?.StartTime;
            this.PeriodEndTime = movement.Period?.EndTime;
            this.SourceNodeId = movement.MovementSource?.SourceNodeId;
            this.SourceProductId = movement.MovementSource?.SourceProductId;
            this.SourceProductTypeId = movement.MovementSource?.SourceProductTypeId;
            this.DestinationNodeId = movement.MovementDestination?.DestinationNodeId;
            this.DestinationProductId = movement.MovementDestination?.DestinationProductId;
            this.DestinationProductTypeId = movement.MovementDestination?.DestinationProductTypeId;
            this.SourceInventoryProductId = movement.SourceInventoryProductId;
            this.ConsolidatedMovementTransactionId = movement.ConsolidatedMovementTransactionId;
            this.ConsolidatedInventoryProductId = movement.ConsolidatedInventoryProductId;
            this.OriginalMovementTransactionId = movement.OriginalMovementTransactionId;
            this.BlockchainStatus = (int)StatusType.PROCESSING;
            this.CreatedBy = createdBy;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int? Id { get; }

        /// <summary>
        /// Gets or sets the temporary identifier.
        /// </summary>
        /// <value>
        /// The temporary identifier.
        /// </value>
        public int TempId { get; set; }

        /// <summary>
        /// Gets the movement type identifier.
        /// </summary>
        /// <value>
        /// The movement type identifier.
        /// </value>
        public int MovementTypeId { get; }

        /// <summary>
        /// Gets the message type identifier.
        /// </summary>
        /// <value>
        /// The message type identifier.
        /// </value>
        public int MessageTypeId { get; }

        /// <summary>
        /// Gets the system type identifier.
        /// </summary>
        /// <value>
        /// The system type identifier.
        /// </value>
        public int SystemTypeId { get; }

        /// <summary>
        /// Gets the source system identifier.
        /// </summary>
        /// <value>
        /// The source system identifier.
        /// </value>
        public int? SourceSystemId { get; }

        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        /// <value>
        /// The type of the event.
        /// </value>
        public string EventType { get; }

        /// <summary>
        /// Gets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>
        public string MovementId { get; }

        /// <summary>
        /// Gets the is system generated.
        /// </summary>
        /// <value>
        /// The is system generated.
        /// </value>
        public bool? IsSystemGenerated { get; }

        /// <summary>
        /// Gets the official delta ticket identifier.
        /// </summary>
        /// <value>
        /// The official delta ticket identifier.
        /// </value>
        public int? OfficialDeltaTicketId { get; }

        /// <summary>
        /// Gets the scenario identifier.
        /// </summary>
        /// <value>
        /// The scenario identifier.
        /// </value>
        public int ScenarioId { get; }

        /// <summary>
        /// Gets the observations.
        /// </summary>
        /// <value>
        /// The observations.
        /// </value>
        public string Observations { get; }

        /// <summary>
        /// Gets the classification.
        /// </summary>
        /// <value>
        /// The classification.
        /// </value>
        public string Classification { get; }

        /// <summary>
        /// Gets the pending approval.
        /// </summary>
        /// <value>
        /// The pending approval.
        /// </value>
        public bool? PendingApproval { get; }

        /// <summary>
        /// Gets  the net standard volume.
        /// </summary>
        /// <value>
        /// The net standard volume.
        /// </value>
        public decimal? NetStandardVolume { get; }

        /// <summary>
        /// Gets the source movement transaction identifier.
        /// </summary>
        /// <value>
        /// The source movement transaction identifier.
        /// </value>
        public int? SourceMovementTransactionId { get; }

        /// <summary>
        /// Gets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        public int? MeasurementUnit { get; }

        /// <summary>
        /// Gets the segment identifier.
        /// </summary>
        /// <value>
        /// The segment identifier.
        /// </value>
        public int? SegmentId { get; }

        /// <summary>
        /// Gets the operational date.
        /// </summary>
        /// <value>
        /// The operational date.
        /// </value>
        public DateTime OperationalDate { get; }

        /// <summary>
        /// Gets the official delta message type identifier.
        /// </summary>
        /// <value>
        /// The official delta message type identifier.
        /// </value>
        public int? OfficialDeltaMessageTypeId { get; }

        /// <summary>
        /// Gets the period start time.
        /// </summary>
        /// <value>
        /// The period start time.
        /// </value>
        public DateTime? PeriodStartTime { get; }

        /// <summary>
        /// Gets the period end time.
        /// </summary>
        /// <value>
        /// The period end time.
        /// </value>
        public DateTime? PeriodEndTime { get; }

        /// <summary>
        /// Gets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        public int? SourceNodeId { get; }

        /// <summary>
        /// Gets the source product identifier.
        /// </summary>
        /// <value>
        /// The source product identifier.
        /// </value>
        public string SourceProductId { get; }

        /// <summary>
        /// Gets the source product type identifier.
        /// </summary>
        /// <value>
        /// The source product type identifier.
        /// </value>
        public int? SourceProductTypeId { get; }

        /// <summary>
        /// Gets the destination node identifier.
        /// </summary>
        /// <value>
        /// The destination node identifier.
        /// </value>
        public int? DestinationNodeId { get; }

        /// <summary>
        /// Gets the destination product identifier.
        /// </summary>
        /// <value>
        /// The destination product identifier.
        /// </value>
        public string DestinationProductId { get; }

        /// <summary>
        /// Gets the destination product type identifier.
        /// </summary>
        /// <value>
        /// The destination product type identifier.
        /// </value>
        public int? DestinationProductTypeId { get; }

        /// <summary>
        /// Gets or sets the source inventory product identifier.
        /// </summary>
        /// <value>
        /// The source inventory product identifier.
        /// </value>
        public int? SourceInventoryProductId { get; set; }

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
        /// Gets or sets the blockchain status.
        /// </summary>
        /// <value>
        /// The blockchain status.
        /// </value>
        public int BlockchainStatus { get; set; }

        /// <summary>
        /// Gets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        public string CreatedBy { get; }
    }
}
