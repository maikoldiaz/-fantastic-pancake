// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigBase.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.DataGenerator.Console.Entities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The config base.
    /// </summary>
    public class ConfigBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigBase"/> class.
        /// </summary>
        public ConfigBase()
        {
            this.ConsolidationSegments = new List<DataGeneratorSegment>();
        }

        /// <summary>
        /// Gets or sets the movement type identifier.
        /// </summary>
        /// <value>
        /// The movement type identifier.
        /// </value>
        public int MovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the cancellation type identifier.
        /// </summary>
        /// <value>
        /// The cancellation type identifier.
        /// </value>
        public int CancellationTypeId { get; set; }

        /// <summary>
        /// Gets or sets the segment identifier.
        /// </summary>
        /// <value>
        /// The segment identifier.
        /// </value>
        public int SegmentId { get; set; }

        /// <summary>
        /// Gets or sets the system identifier.
        /// </summary>
        /// <value>
        /// The system identifier.
        /// </value>
        public int SystemId { get; set; }

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
        /// Gets or sets the operational date.
        /// </summary>
        /// <value>
        /// The operational date.
        /// </value>
        public DateTime OperationalDate { get; set; }

        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        public int SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination node identifier.
        /// </summary>
        /// <value>
        /// The invalid destination node identifier.
        /// </value>
        public int DestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the invalid source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        public int InvalidSourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the invalid destination node identifier.
        /// </summary>
        /// <value>
        /// The invalid destination node identifier.
        /// </value>
        public int InvalidDestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the cutoff ticket identifier.
        /// </summary>
        /// <value>
        /// The cutoff ticket identifier.
        /// </value>
        public int CutoffTicketId { get; set; }

        /// <summary>
        /// Gets or sets the ownership ticket identifier.
        /// </summary>
        /// <value>
        /// The ownership ticket identifier.
        /// </value>
        public int OwnershipTicketId { get; set; }

        /// <summary>
        /// Gets or sets the official delta ticket identifier.
        /// </summary>
        /// <value>
        /// The official delta ticket identifier.
        /// </value>
        public int OfficialDeltaTicketId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is cancellation case.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is cancellation case; otherwise, <c>false</c>.
        /// </value>
        public bool IsCancellationCase { get; set; }

        /// <summary>
        /// Gets or sets the test identifier.
        /// </summary>
        /// <value>
        /// The test identifier.
        /// </value>
        public string TestId { get; set; }

        /// <summary>
        /// Gets the consolidation segments.
        /// </summary>
        /// <value>
        /// The consolidation segments.
        /// </value>
        public IList<DataGeneratorSegment> ConsolidationSegments { get; }

        /// <summary>
        /// Gets  or sets the DifferentSegmentId segments.
        /// </summary>
        /// <value>
        /// The DifferentSegmentId segments.
        /// </value>
        public int DifferentSegmentId { get; set; }

        /// <summary>
        /// Gets  or sets the DifferentSystemId.
        /// </summary>
        /// <value>
        /// The DifferentSegmentId segments.
        /// </value>
        public int DifferentSystemId { get; set; }

        /// <summary>
        /// Gets or sets the official delta ticket identifier.
        /// </summary>
        /// <value>
        /// The official delta ticket identifier.
        /// </value>
        public int OfficialDeltaTicketId2 { get; set; }

        /// <summary>
        /// Gets or sets the product type identifier.
        /// </summary>
        /// <value>
        /// The product type identifier.
        /// </value>
        public int ProductTypeId { get; set; }
    }
}
