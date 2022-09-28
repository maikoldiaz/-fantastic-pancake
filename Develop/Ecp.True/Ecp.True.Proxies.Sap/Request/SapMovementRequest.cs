// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapMovementRequest.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Sap.Request
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// The SAP PO Movement class.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class SapMovementRequest
    {
        /// <summary>
        /// Gets or sets the source system.
        /// </summary>
        /// <value>
        /// The source system.
        /// </value>
        [JsonProperty("sourceSystem")]
        public string SourceSystemId { get; set; }

        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        /// <value>
        /// The type of the event.
        /// </value>
        [JsonProperty("eventType")]
        public string EventType { get; set; }

        /// <summary>
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>
        [JsonProperty("movementId")]
        public string MovementId { get; set; }

        /// <summary>
        /// Gets or sets the movement type identifier.
        /// </summary>
        /// <value>
        /// The movement type identifier.
        /// </value>
        [JsonProperty("movementTypeId")]
        public string MovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the operational date.
        /// </summary>
        /// <value>
        /// The operational date.
        /// </value>
        [JsonProperty("operationalDate")]
        public DateTime? OperationalDate { get; set; }

        /// <summary>
        /// Gets or sets the period.
        /// </summary>
        /// <value>
        /// The period.
        /// </value>
        [JsonProperty("period")]
        public SapMovementRequestPeriod Period { get; set; }

        /// <summary>
        /// Gets or sets the movement source.
        /// </summary>
        /// <value>
        /// The movement source.
        /// </value>
        [JsonProperty("movementSource")]
        public SapMovementRequestSource MovementSource { get; set; }

        /// <summary>
        /// Gets or sets the movement destination.
        /// </summary>
        /// <value>
        /// The movement destination.
        /// </value>
        [JsonProperty("movementDestination")]
        public SapMovementRequestDestination MovementDestination { get; set; }

        /// <summary>
        /// Gets or sets the gross standard volume.
        /// </summary>
        /// <value>
        /// The gross standard volume.
        /// </value>
        [JsonProperty("grossStandardQuantity")]
        public decimal? GrossStandardVolume { get; set; }

        /// <summary>
        /// Gets or sets the net standard volume.
        /// </summary>
        /// <value>
        /// The net standard volume.
        /// </value>
        [JsonProperty("netStandardQuantity")]
        public decimal? NetStandardVolume { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        [JsonProperty("measurementUnit")]
        public string MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets the tolerance.
        /// </summary>
        /// <value>
        /// The tolerance.
        /// </value>
        [JsonProperty("uncertainty")]
        public decimal? Tolerance { get; set; }

        /// <summary>
        /// Gets or sets the segment.
        /// </summary>
        /// <value>
        /// The segment.
        /// </value>
        [JsonProperty("segmentId")]
        public string SegmentId { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        [JsonProperty("operatorId")]
        public string OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the scenario.
        /// </summary>
        /// <value>
        /// The scenario.
        /// </value>
        [JsonProperty("scenarioId")]
        public string ScenarioId { get; set; }

        /// <summary>
        /// Gets or sets the observations.
        /// </summary>
        /// <value>
        /// The observations.
        /// </value>
        [JsonProperty("observations")]
        public string Observations { get; set; }

        /// <summary>
        /// Gets or sets the classification.
        /// </summary>
        /// <value>
        /// The classification.
        /// </value>
        [JsonProperty("classification")]
        public string Classification { get; set; }

        /// <summary>
        /// Gets or sets the sap process status.
        /// </summary>
        /// <value>
        /// The sap process status.
        /// </value>
        [JsonProperty("sapProcessStatus")]
        public string SapProcessStatus { get; set; }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        [JsonProperty("attributes")]
        public IEnumerable<SapMovementRequestAttribute> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the owners.
        /// </summary>
        /// <value>
        /// The owners.
        /// </value>
        [JsonProperty("owners")]
        public IEnumerable<SapMovementRequestOwner> Owners { get; set; }

        /// <summary>
        /// Gets or sets the backup movement.
        /// </summary>
        /// <value>
        /// The backup movement.
        /// </value>
        [JsonProperty("officialInformation")]
        public SapMovementRequestBackupMovement BackupMovement { get; set; }

        /// <summary>
        /// Gets or sets the batch Id.
        /// </summary>
        /// <value>
        /// The batch Id.
        /// </value>
        [JsonProperty("batchId")]
        public string BatchId { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [JsonProperty("version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the system.
        /// </summary>
        /// <value>
        /// The system.
        /// </value>
        [JsonProperty("system")]
        public string SystemId { get; set; }
    }
}
