// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaNode.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The delta error.
    /// </summary>
    public class DeltaNode : AuditableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaNode"/> class.
        /// </summary>
        public DeltaNode()
        {
            this.DeltaNodeErrors = new List<DeltaNodeError>();
        }

        /// <summary>
        /// Gets or sets the delta error identifier.
        /// </summary>
        /// <value>
        /// The delta error identifier.
        /// </value>
        public int DeltaNodeId { get; set; }

        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public int TicketId { get; set; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public OwnershipNodeStatusType Status { get; set; }

        /// <summary>
        /// Gets or sets the last approved date.
        /// </summary>
        /// <value>
        /// The last approved date.
        /// </value>
        public DateTime? LastApprovedDate { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the editor.
        /// </summary>
        /// <value>
        /// The editor.
        /// </value>
        public string Editor { get; set; }

        /// <summary>
        /// Gets or sets the approvers.
        /// </summary>
        /// <value>
        /// The approvers.
        /// </value>
        public string Approvers { get; set; }

        /// <summary>
        /// Gets or sets the ticket.
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        public virtual Ticket Ticket { get; set; }

        /// <summary>
        /// Gets or sets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        public virtual Node Node { get; set; }

        /// <summary>
        /// Gets the delta node errors.
        /// </summary>
        /// <value>
        /// The delta node errors.
        /// </value>
        public virtual ICollection<DeltaNodeError> DeltaNodeErrors { get; private set; }
    }
}
