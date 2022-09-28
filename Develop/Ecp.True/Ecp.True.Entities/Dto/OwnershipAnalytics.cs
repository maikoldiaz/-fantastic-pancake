// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipAnalytics.cs" company="Microsoft">
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

    /// <summary>
    /// Ownership analytics DTO.
    /// </summary>
    public class OwnershipAnalytics
    {
        /// <summary>
        /// Gets or sets the ownership analytics identifier.
        /// </summary>
        /// <value>
        /// The ownership analytics identifier.
        /// </value>
        public int OwnershipAnalyticsId { get; set; }

        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public int TicketId { get; set; }

        /// <summary>
        /// Gets or sets the algorithm identifier.
        /// </summary>
        /// <value>
        /// The algorithm identifier.
        /// </value>
        public int AlgorithmId { get; set; }

        /// <summary>
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>
        public string MovementId { get; set; }

        /// <summary>
        /// Gets or sets the movement type identifier.
        /// </summary>
        /// <value>
        /// The movement type identifier.
        /// </value>
        public string MovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        public int SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the source node type identifier.
        /// </summary>
        /// <value>
        /// The source node type identifier.
        /// </value>
        public int SourceNodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the destination node identifier.
        /// </summary>
        /// <value>
        /// The destination node identifier.
        /// </value>
        public int DestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination node type identifier.
        /// </summary>
        /// <value>
        /// The destination node type identifier.
        /// </value>
        public int DestinationNodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the source product identifier.
        /// </summary>
        /// <value>
        /// The source product identifier.
        /// </value>
        public string SourceProductId { get; set; }

        /// <summary>
        /// Gets or sets the source product type identifier.
        /// </summary>
        /// <value>
        /// The source product type identifier.
        /// </value>
        public string SourceProductTypeId { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public int? OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the movement volume.
        /// </summary>
        /// <value>
        /// The movement volume.
        /// </value>
        public decimal NetVolume { get; set; }

        /// <summary>
        /// Gets or sets the ownership volume.
        /// </summary>
        /// <value>
        /// The ownership volume.
        /// </value>
        public decimal? OwnershipVolume { get; set; }

        /// <summary>
        /// Gets or sets the ownership percentage.
        /// </summary>
        /// <value>
        /// The ownership percentage.
        /// </value>
        public decimal? OwnershipPercentage { get; set; }

        /// <summary>
        /// Gets or sets the movement transaction identifier.
        /// </summary>
        /// <value>
        /// The movement transaction identifier.
        /// </value>
        public int MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        public string MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets the execution date.
        /// </summary>
        /// <value>
        /// The execution date.
        /// </value>
        public DateTime ExecutionDate { get; set; }

        /// <summary>
        /// Gets the other copy.
        /// </summary>
        /// <param name="percentage">The percentage.</param>
        /// <param name="volume">The volume.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <returns>Other object.</returns>
        public OwnershipAnalytics GetOtherCopy(decimal percentage, decimal volume, int ownerId)
        {
            return new OwnershipAnalytics
            {
                AlgorithmId = this.AlgorithmId,
                DestinationNodeId = this.DestinationNodeId,
                DestinationNodeTypeId = this.DestinationNodeTypeId,
                ExecutionDate = this.ExecutionDate,
                MeasurementUnit = this.MeasurementUnit,
                MovementId = this.MovementId,
                MovementTransactionId = this.MovementTransactionId,
                MovementTypeId = this.MovementTypeId,
                OwnerId = ownerId,
                OwnershipPercentage = percentage,
                OwnershipVolume = volume,
                SourceNodeId = this.SourceNodeId,
                SourceNodeTypeId = this.SourceNodeTypeId,
                SourceProductId = this.SourceProductId,
                SourceProductTypeId = this.SourceProductTypeId,
                TicketId = this.TicketId,
            };
        }
    }
}
