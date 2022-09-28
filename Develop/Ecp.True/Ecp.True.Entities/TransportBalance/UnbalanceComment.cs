// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnbalanceComment.cs" company="Microsoft">
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
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// Segment Unbalances.
    /// </summary>
    public class UnbalanceComment : Entity, IComment
    {
        /// <summary>
        /// Gets or sets the unbalance identifier.
        /// </summary>
        /// <value>
        /// The unbalance identifier.
        /// </value>
        public int UnbalanceId { get; set; }

        /// <summary>
        /// Gets or sets the ticket number.
        /// </summary>
        /// <value>
        /// The ticket number.
        /// </value>
        public int? TicketId { get; set; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets the node name.
        /// </summary>
        /// <value>
        /// The node name.
        /// </value>
        public string NodeName { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        /// <value>
        /// The node name.
        /// </value>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the unbalance.
        /// </summary>
        /// <value>
        /// The unbalance.
        /// </value>
        public decimal Unbalance { get; set; }

        /// <summary>
        /// Gets or sets the initial inventory.
        /// </summary>
        /// <value>
        /// The initial inventory.
        /// </value>
        public decimal InitialInventory { get; set; }

        /// <summary>
        /// Gets or sets the inputs.
        /// </summary>
        /// <value>
        /// The inputs.
        /// </value>
        public decimal Inputs { get; set; }

        /// <summary>
        /// Gets or sets the inputs.
        /// </summary>
        /// <value>
        /// The inputs.
        /// </value>
        public decimal Outputs { get; set; }

        /// <summary>
        /// Gets or sets the final inventory.
        /// </summary>
        /// <value>
        /// The final inventory.
        /// </value>
        public decimal FinalInventory { get; set; }

        /// <summary>
        /// Gets or sets the identified losses.
        /// </summary>
        /// <value>
        /// The identified losses.
        /// </value>
        public decimal IdentifiedLosses { get; set; }

        /// <summary>
        /// Gets or sets the units.
        /// </summary>
        /// <value>
        /// The units.
        /// </value>
        public string Units { get; set; }

        /// <summary>
        /// Gets or sets the unbalance percentage.
        /// </summary>
        /// <value>
        /// The unbalance percentage.
        /// </value>
        public decimal UnbalancePercentage { get; set; }

        /// <summary>
        /// Gets or sets the limits.
        /// </summary>
        /// <value>
        /// The limits.
        /// </value>
        public decimal ControlLimit { get; set; }

        /// <summary>
        /// Gets or sets the acceptable balance.
        /// </summary>
        /// <value>
        /// The acceptable balance.
        /// </value>
        public decimal AcceptableBalance { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.CommentsRequired)]
        [StringLength(1000, ErrorMessage = Entities.Constants.CommentMaxLength1000)]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Unbalance"/> is status.
        /// </summary>
        /// <value>
        ///   <c>true</c> if status; otherwise, <c>false</c>.
        /// </value>
        public bool Status { get; set; }

        /// <summary>
        /// Gets or sets the unbalance percentage text.
        /// </summary>
        /// <value>
        /// The unbalance percentage text.
        /// </value>
        public string UnbalancePercentageText { get; set; }

        /// <summary>
        /// Gets or sets the calculation date.
        /// </summary>
        /// <value>
        /// The execution date.
        /// </value>
        public DateTime CalculationDate { get; set; }

        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the name of the node.
        /// </summary>
        /// <value>
        /// The name of the node.
        /// </value>
        public virtual Node Node { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
        public virtual Product Product { get; set; }

        /// <summary>
        /// Gets or sets the ticket.
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        public virtual Ticket Ticket { get; set; }

        /// <summary>
        /// Gets or sets the unit name.
        /// </summary>
        /// <value>
        /// The unit name.
        /// </value>
        public string UnitName { get; set; } = "Bbl";
    }
}
