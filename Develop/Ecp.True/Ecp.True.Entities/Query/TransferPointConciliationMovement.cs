// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransferPointConciliationMovement.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Query
{
    using System;

    /// <summary>
    /// The category.
    /// </summary>
    public class TransferPointConciliationMovement : QueryEntity
    {
        /// <summary>
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>
        public string MovementId { get; set; }

        /// <summary>
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>
        public int MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the SegmentId.
        /// </summary>
        /// <value>
        /// The SegmentId.
        /// </value>
        public int SegmentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the movement type.
        /// </summary>
        /// <value>
        /// The name of the movement type.
        /// </value>
        public string MovementTypeName { get; set; }

        /// <summary>
        /// Gets or sets the movement type identifier.
        /// </summary>
        /// <value>
        /// The movement type identifier.
        /// </value>
        public int? MovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the source node.
        /// </summary>
        /// <value>
        /// The source node.
        /// </value>
        public string SourceNodeName { get; set; }

        /// <summary>
        /// Gets or sets the destination node.
        /// </summary>
        /// <value>
        /// The destination node.
        /// </value>
        public string DestinationNodeName { get; set; }

        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        public int SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the source node Segment identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        public int SourceNodeSegmentId { get; set; }

        /// <summary>
        /// Gets or sets the destination node identifier.
        /// </summary>
        /// <value>
        /// The destination node identifier.
        /// </value>
        public int DestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination node Segment identifier.
        /// </summary>
        /// <value>
        /// The destination node identifier.
        /// </value>
        public int DestinationNodeSegmentId { get; set; }

        /// <summary>
        /// Gets or sets the source product.
        /// </summary>
        /// <value>
        /// The source product.
        /// </value>
        public string SourceProductName { get; set; }

        /// <summary>
        /// Gets or sets the destination product.
        /// </summary>
        /// <value>
        /// The destination product.
        /// </value>
        public string DestinationProductName { get; set; }

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
        /// Gets or sets the movement volume.
        /// </summary>
        /// <value>
        /// The movement volume.
        /// </value>
        public decimal NetStandardVolume { get; set; }

        /// <summary>
        /// Gets or sets the ownership ticket id.
        /// </summary>
        /// <value>
        /// The ownership ticket id.
        /// </value>
        public int? OwnershipTicketId { get; set; }

        /// <summary>
        /// Gets or sets the operational date.
        /// </summary>
        /// <value>
        /// The operational date.
        /// </value>
        public DateTime OperationalDate { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        public int MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets the percentage owner.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public decimal? OwnershipPercentage { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        /// <value>
        /// The ownership volume.
        /// </value>
        public decimal? OwnershipVolume { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        /// <value>
        /// The ownership volume.
        /// </value>
        public string OwnershipValueUnit { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public int? OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the scenario id.
        /// </summary>
        /// <value>
        /// The scenario id.
        /// </value>
        public int ScenarioId { get; set; }

        /// <summary>
        /// Gets or sets UncertaintyPercentage.
        /// </summary>
        /// <value>
        /// The UncertaintyPercentage.
        /// </value>
        public decimal? UncertaintyPercentage { get; set; }
    }
}
