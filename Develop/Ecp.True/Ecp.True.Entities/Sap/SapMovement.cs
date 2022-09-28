// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapMovement.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Sap
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Entities.Enumeration;
    using Newtonsoft.Json;

    /// <summary>
    /// The SAP PO Movement class.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class SapMovement
    {
        /// <summary>
        /// Gets or sets the source system.
        /// </summary>
        /// <value>
        /// The source system.
        /// </value>
        [Required(ErrorMessage = SapConstants.SourceSystemNameRequired)]
        [StringLength(25, ErrorMessage = SapConstants.SourceSystemLengthExceeded)]
        [JsonProperty("SourceSystem")]
        public string SourceSystemId { get; set; }

        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        /// <value>
        /// The type of the event.
        /// </value>
        [Required(ErrorMessage = SapConstants.EventTypeRequired)]
        [StringLength(25, ErrorMessage = SapConstants.EventTypeLengthExceeded)]
        public string EventType { get; set; }

        /// <summary>
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>
        [Required(ErrorMessage = SapConstants.MovementIdentifierRequired)]
        [StringLength(50, ErrorMessage = SapConstants.MovementIdentifierLengthExceeded)]
        public string MovementId { get; set; }

        /// <summary>
        /// Gets or sets the movement type identifier.
        /// </summary>
        /// <value>
        /// The movement type identifier.
        /// </value>
        [Required(ErrorMessage = SapConstants.MovementTypeRequired)]
        [StringLength(150, ErrorMessage = SapConstants.MovementTypeIdentifierLengthExceeded)]
        public string MovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the operational date.
        /// </summary>
        /// <value>
        /// The operational date.
        /// </value>
        [Required(ErrorMessage = SapConstants.OperationDateRequired)]
        public DateTime? OperationalDate { get; set; }

        /// <summary>
        /// Gets or sets the period.
        /// </summary>
        /// <value>
        /// The period.
        /// </value>
        [Required(ErrorMessage = SapConstants.PeriodIsMandatory)]
        public SapMovementPeriod Period { get; set; }

        /// <summary>
        /// Gets or sets the movement source.
        /// </summary>
        /// <value>
        /// The movement source.
        /// </value>
        [OptionalIf("MovementDestination", ErrorMessage = SapConstants.BothSourceDestinationMandatory)]
        public SapMovementSource MovementSource { get; set; }

        /// <summary>
        /// Gets or sets the movement destination.
        /// </summary>
        /// <value>
        /// The movement destination.
        /// </value>
        public SapMovementDestination MovementDestination { get; set; }

        /// <summary>
        /// Gets or sets the gross standard volume.
        /// </summary>
        /// <value>
        /// The gross standard volume.
        /// </value>
        [JsonProperty("GrossStandardQuantity")]
        public decimal? GrossStandardVolume { get; set; }

        /// <summary>
        /// Gets or sets the net standard volume.
        /// </summary>
        /// <value>
        /// The net standard volume.
        /// </value>
        [Required(ErrorMessage = SapConstants.NetStandardQuantityIsMandatory)]
        [JsonProperty("NetStandardQuantity")]
        public decimal? NetStandardVolume { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        [Required(ErrorMessage = SapConstants.MeasurementUnitRequired)]
        [StringLength(50, ErrorMessage = SapConstants.MeasurementUnitLengthExceeded)]
        public string MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets the tolerance.
        /// </summary>
        /// <value>
        /// The tolerance.
        /// </value>
        [JsonProperty("Uncertainty")]
        public decimal? Tolerance { get; set; }

        /// <summary>
        /// Gets or sets the segment.
        /// </summary>
        /// <value>
        /// The segment.
        /// </value>
        [Required(ErrorMessage = SapConstants.SegmentRequired)]
        [StringLength(150, ErrorMessage = SapConstants.SegmentLengthExceeded)]
        public string SegmentId { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        [StringLength(150, ErrorMessage = SapConstants.OperatorLengthExceeded)]
        public string OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the scenario.
        /// </summary>
        /// <value>
        /// The scenario.
        /// </value>
        [Required(ErrorMessage = SapConstants.ScenarioRequired)]
        [ValidateEnum(typeof(ScenarioType), SapConstants.ScenarioIdValueRangeFailed)]
        public string ScenarioId { get; set; }

        /// <summary>
        /// Gets or sets the observations.
        /// </summary>
        /// <value>
        /// The observations.
        /// </value>
        [StringLength(150, ErrorMessage = SapConstants.ObservationLengthExceeded)]
        public string Observations { get; set; }

        /// <summary>
        /// Gets or sets the classification.
        /// </summary>
        /// <value>
        /// The classification.
        /// </value>
        [Required(ErrorMessage = SapConstants.MovementClassificationIsMandatory)]
        [StringLength(30, ErrorMessage = SapConstants.ClassificationLengthExceeded)]
        public string Classification { get; set; }

        /// <summary>
        /// Gets or sets the sap process status.
        /// </summary>
        /// <value>
        /// The sap process status.
        /// </value>
        [StringLength(50, ErrorMessage = SapConstants.SapProcessStatusLengthExceeded)]
        public string SapProcessStatus { get; set; }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public IEnumerable<SapAttribute> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the owners.
        /// </summary>
        /// <value>
        /// The owners.
        /// </value>
        public IEnumerable<SapOwner> Owners { get; set; }

        /// <summary>
        /// Gets or sets the backup movement.
        /// </summary>
        /// <value>
        /// The backup movement.
        /// </value>
        [JsonProperty("OfficialInformation")]
        public BackupMovement BackupMovement { get; set; }

        /// <summary>
        /// Gets or sets the batch Id.
        /// </summary>
        /// <value>
        /// The batch Id.
        /// </value>
        [StringLength(25, ErrorMessage = SapConstants.BatchIdLengthExceeded)]
        public string BatchId { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [StringLength(50, ErrorMessage = SapConstants.VersionLengthExceeded)]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the system.
        /// </summary>
        /// <value>
        /// The system.
        /// </value>
        [StringLength(150, ErrorMessage = SapConstants.SystemLengthExceeded)]
        [JsonProperty("System")]
        public string SystemId { get; set; }
    }
}
