// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaNodeApprovalHistory.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Admin
{
    using System;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The delta error.
    /// </summary>
    public class DeltaNodeApprovalHistory : Entity
    {
        /// <summary>
        /// Gets or sets the delta node approval history identifier.
        /// </summary>
        /// <value>
        /// The delta node approval history identifier.
        /// </value>
        public int DeltaNodeApprovalHistoryId { get; set; }

        /// <summary>
        /// Gets or sets the delta node identifier.
        /// </summary>
        /// <value>
        /// The delta node identifier.
        /// </value>
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public int TicketId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public OwnershipNodeStatusType Status { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the ticket.
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        public virtual Ticket Ticket { get; set; }
    }
}
