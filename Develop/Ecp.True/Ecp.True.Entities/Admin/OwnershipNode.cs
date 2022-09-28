// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipNode.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// OwnershipNode Entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class OwnershipNode : AuditableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipNode"/> class.
        /// </summary>
        public OwnershipNode()
        {
            this.OwnershipNodeErrors = new List<OwnershipNodeError>();
        }

        /// <summary>
        /// Gets or sets the ownership node identifier.
        /// </summary>
        /// <value>
        /// The ownership node identifier.
        /// </value>
        public int OwnershipNodeId { get; set; }

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
        public StatusType Status { get; set; }

        /// <summary>
        /// Gets or sets the ownership status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public OwnershipNodeStatusType? OwnershipStatus { get; set; }

        /// <summary>
        /// Gets or sets the editor.
        /// </summary>
        /// <value>
        /// The editor.
        /// </value>
        public string Editor { get; set; }

        /// <summary>Gets or sets the comment.</summary>
        /// <value>The comment.</value>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the editor connection identifier.
        /// </summary>
        /// <value>
        /// The editor connection identifier.
        /// </value>
        public string EditorConnectionId { get; set; }

        /// <summary>
        /// Gets or sets the approver alias.
        /// </summary>
        /// <value>
        /// The approver status.
        /// </value>
        public string ApproverAlias { get; set; }

        /// <summary>
        /// Gets or sets the ownership analytics status.
        /// </summary>
        /// <value>
        /// The ownership analytics status.
        /// </value>
        public int? OwnershipAnalyticsStatus { get; set; }

        /// <summary>
        /// Gets or sets the ownership analytics error message.
        /// </summary>
        /// <value>
        /// The ownership analytics error message.
        /// </value>
        public string OwnershipAnalyticsErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime? RegistrationDate { get; set; }

        /// <summary>
        /// Gets or sets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        public virtual Node Node { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>
        /// The last modified date.
        /// </value>
        public new DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the ticket.
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        public virtual Ticket Ticket { get; set; }

        /// <summary>
        /// Gets or sets the ownership node error.
        /// </summary>
        /// <value>
        /// The ownership node error.
        /// </value>
        public virtual IEnumerable<OwnershipNodeError> OwnershipNodeErrors { get; set; }
    }
}
