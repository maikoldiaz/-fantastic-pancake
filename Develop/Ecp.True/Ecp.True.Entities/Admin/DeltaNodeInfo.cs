// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaNodeInfo.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Admin
{
    using System;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;

    /// <summary>
    /// Delta node information.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class DeltaNodeInfo : QueryEntity
    {
        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public int TicketId { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>
        /// The ticket start time.
        /// </value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        /// <value>
        /// The ticket end time.
        /// </value>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>
        /// The execution date.
        /// </value>
        public DateTime ExecutionDate { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        ///   <c>true</c> if status; otherwise, <c>false</c>.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the Segment.
        /// </summary>
        /// <value>
        /// The Segment.
        /// </value>
        public string Segment { get; set; }

        /// <summary>
        ///     Gets or sets the name of the node.
        /// </summary>
        /// <value>
        ///     The name of the node.
        /// </value>
        public string NodeName { get; set; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets the segment identifier.
        /// </summary>
        /// <value>
        /// The segment identifier.
        /// </value>
        public int SegmentId { get; set; }

        /// <summary>
        /// Gets or sets the ticket type identifier.
        /// </summary>
        /// <value>
        /// The ticket type identifier.
        /// </value>
        public TicketType TicketTypeId { get; set; }

        /// <summary>
        /// Gets or sets the delta node identifier.
        /// </summary>
        /// <value>
        /// The delta node identifier.
        /// </value>
        public int DeltaNodeId { get; set; }

        /// <summary>
        /// Gets or sets the ticket status.
        /// </summary>
        /// <value>
        /// The ticket status.
        /// </value>
        public string TicketStatus { get; set; }

        /// <summary>
        /// Gets or sets the ticket status identifier.
        /// </summary>
        /// <value>
        /// The ticket status identifier.
        /// </value>
        public int TicketStatusId { get; set; }
    }
}
