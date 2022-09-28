// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodesForSegmentResult.cs" company="Microsoft">
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

    /// <summary>
    /// The NodesForSegment DTO.
    /// </summary>
    public class NodesForSegmentResult : QueryEntity
    {
        /// <summary>
        /// Gets or sets the SegmentId the ticket.
        /// </summary>
        /// <value>
        /// the SegmentId.
        /// </value>
        public int SegmentId { get; set; }

        /// <summary>
        /// Gets or sets the NodeId.
        /// </summary>
        /// <value>
        /// The NodeId list.
        /// </value>
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets the NodeName.
        /// </summary>
        /// <value>
        /// The NodeName list.
        /// </value>
        public string NodeName { get; set; }

        /// <summary>
        /// Gets or sets the OperationDate.
        /// </summary>
        /// <value>
        /// The OperationDate.
        /// </value>
        public DateTime OperationDate { get; set; }

        /// <summary>
        /// Gets or sets the StartDate.
        /// </summary>
        /// <value>
        /// The StartDate.
        /// </value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the EndDate.
        /// </summary>
        /// <value>
        /// The EndDate.
        /// </value>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the StatusId.
        /// </summary>
        /// <value>
        /// The StatusId.
        /// </value>
        public int? StatusId { get; set; }

        /// <summary>
        /// Gets or sets the StatusName.
        /// </summary>
        /// <value>
        /// The StatusName list.
        /// </value>
        public string StatusName { get; set; }

        /// <summary>
        /// Gets or sets the TicketStatusName.
        /// </summary>
        /// <value>
        /// The TicketStatusName list.
        /// </value>
        public string TicketStatusName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the IsEnabledForSendToSap.
        /// </summary>
        /// <value>
        /// The IsEnabledForSendToSap.
        /// </value>
        public bool IsEnabledForSendToSap { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the IsApproved.
        /// </summary>
        /// <value>
        /// The IsApproved.
        /// </value>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the IsActiveInBatch.
        /// </summary>
        /// <value>
        /// The IsActiveInBatch.
        /// </value>
        public bool IsActiveInBatch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the PredecessorIsApproved.
        /// </summary>
        /// <value>
        /// The PredecessorIsApproved.
        /// </value>
        public bool PredecessorIsApproved { get; set; }
    }
}
