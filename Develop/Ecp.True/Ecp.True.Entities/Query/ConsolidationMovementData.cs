// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidationMovementData.cs" company="Microsoft">
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
    /// <summary>
    /// The ConsolidationMovementData.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Query.QueryEntity" />
    public class ConsolidationMovementData : QueryEntity
    {
        /// <summary>
        /// Gets or sets the movement transaction identifier.
        /// </summary>
        /// <value>
        /// The movement transaction identifier.
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
        public int MovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        public int? MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets the original movement transaction identifier.
        /// </summary>
        /// <value>
        /// The source movement transaction identifier.
        /// </value>
        public int? OriginalMovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the source movement type identifier.
        /// </summary>
        /// <value>
        /// The source movement type identifier.
        /// </value>
        public int? SourceMovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public int OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the net standard volume.
        /// </summary>
        /// <value>
        /// The net standard volume.
        /// </value>
        public decimal NetStandardVolume { get; set; }

        /// <summary>
        /// Gets or sets the ownership volume.
        /// </summary>
        /// <value>
        /// The ownership volume.
        /// </value>
        public decimal OwnershipVolume { get; set; }

        /// <summary>
        /// Gets or sets the gross standard volume.
        /// </summary>
        /// <value>
        /// The gross standard volume.
        /// </value>
        public decimal? GrossStandardVolume { get; set; }

        /// <summary>
        /// Gets or sets the ownership percentage.
        /// </summary>
        /// <value>
        /// The ownership percentage.
        /// </value>
        public decimal? OwnershipPercentage { get; set; }

        /// <summary>
        /// Gets or sets the ownership value unit.
        /// </summary>
        /// <value>
        /// The ownership value unit.
        /// </value>
        public string OwnershipValueUnit { get; set; }

        /// <summary>
        /// Gets or sets the ownership ticket id.
        /// </summary>
        /// <value>
        /// The ownership ticket id.
        /// </value>
        public int? OwnershipTicketId { get; set; }
    }
}
