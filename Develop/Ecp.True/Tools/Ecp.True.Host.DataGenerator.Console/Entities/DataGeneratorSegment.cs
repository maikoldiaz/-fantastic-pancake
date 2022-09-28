// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGeneratorSegment.cs" company="Microsoft">
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
    using System.Collections.Generic;

    /// <summary>
    /// The Data Generator Segment.
    /// </summary>
    public class DataGeneratorSegment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGeneratorSegment"/> class.
        /// </summary>
        public DataGeneratorSegment()
        {
            this.SourceNodeIds = new List<int>();
            this.DestinationNodeIds = new List<int>();
        }

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
        /// Gets or sets a value indicating whether this instance is son.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is son; otherwise, <c>false</c>.
        /// </value>
        public bool IsSon { get; set; }

        /// <summary>
        /// Gets or sets the ownership ticket identifier.
        /// </summary>
        /// <value>
        /// The ownership ticket identifier.
        /// </value>
        public int OwnershipTicketId { get; set; }

        /// <summary>
        /// Gets or sets the cutoff ticket identifier.
        /// </summary>
        /// <value>
        /// The cutoff ticket identifier.
        /// </value>
        public int CutoffTicketId { get; set; }

        /// <summary>
        /// Gets or sets the delta ticket identifier.
        /// </summary>
        /// <value>
        /// The delta ticket identifier.
        /// </value>
        public int DeltaTicketId { get; set; }

        /// <summary>
        /// Gets or sets the OfficialDeltaTicketId identifier.
        /// </summary>
        /// <value>
        /// The OfficialDeltaTicketId.
        /// </value>
        public int OfficialDeltaTicketId { get; set; }

        /// <summary>
        /// Gets the node ids.
        /// </summary>
        /// <value>
        /// The node ids.
        /// </value>
        public IList<int> SourceNodeIds { get; }

        /// <summary>
        /// Gets the destination node ids.
        /// </summary>
        /// <value>
        /// The destination node ids.
        /// </value>
        public IList<int> DestinationNodeIds { get; }
    }
}
