// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CancellationMovementDetail.cs" company="Microsoft">
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
    using Ecp.True.Core;

    /// <summary>
    /// The Movements OperationalData.
    /// </summary>
    public class CancellationMovementDetail : QueryEntity
    {
        /// <summary>
        /// Gets the source system.
        /// </summary>
        /// <value>
        /// The source system.
        /// </value>
        public static string SourceSystem => "TRUE";

        /// <summary>
        /// Gets the execution date.
        /// </summary>
        /// <value>
        /// The execution date.
        /// </value>
        public static DateTime ExecutionDate => DateTime.UtcNow.ToTrue();

        /// <summary>
        /// Gets or sets the rule version.
        /// </summary>
        /// <value>
        /// The rule version.
        /// </value>
        public string RuleVersion { get; set; }

        /// <summary>
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>
        public int MovementTransactionId { get; set; }

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
        /// Gets or sets the net volume.
        /// </summary>
        /// <value>
        /// The net volume.
        /// </value>
        public decimal? NetVolume { get; set; }

        /// <summary>
        /// Gets or sets the message type identifier.
        /// </summary>
        /// <value>
        /// The message type identifier.
        /// </value>
        public int MessageTypeId { get; set; }

        /// <summary>
        /// Gets or sets the movement type identifier.
        /// </summary>
        /// <value>
        /// The movement type identifier.
        /// </value>
        public int MovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the movement type.
        /// </summary>
        /// <value>
        /// The movement type.
        /// </value>
        public string MovementType { get; set; }

        /// <summary>
        /// Gets or sets the operational date.
        /// </summary>
        /// <value>
        /// The operational date.
        /// </value>
        public DateTime OperationalDate { get; set; }

        /// <summary>
        /// Gets or sets the segment id.
        /// </summary>
        /// <value>
        /// The segment id.
        /// </value>
        public int SegmentId { get; set; }

        /// <summary>
        /// Gets or sets the product type.
        /// </summary>
        /// <value>
        /// The product type.
        /// </value>
        public int? ProductType { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public int? OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the ownership unit.
        /// </summary>
        /// <value>
        /// The ownership unit.
        /// </value>
        public int? Unit { get; set; }

        /// <summary>
        /// Gets or sets the ownership value.
        /// </summary>
        /// <value>
        /// The ownership value.
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
        /// Gets or sets the applied rule.
        /// </summary>
        /// <value>
        /// The applied rule.
        /// </value>
        public string AppliedRule { get; set; }
    }
}
