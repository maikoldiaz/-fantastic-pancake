// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TicketNode.cs" company="Microsoft">
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
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The Ticket Nodes.
    /// </summary>
    public class TicketNode : Entity
    {
        /// <summary>
        /// Gets or sets the Ticket Node identifier.
        /// </summary>
        /// <value>
        /// The Ticket Node identifier.
        /// </value>
        public int TicketNodeId { get; set; }

        /// <summary>
        /// Gets or sets the Ticket identifier.
        /// </summary>
        /// <value>
        /// The Ticket identifier.
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
        /// Gets or sets the Ticket Nodes to Tickets.
        /// </summary>
        /// <value>
        /// The Ticket Nodes to Tickets.
        /// </value>
        public virtual Ticket Ticket { get; set; }

        /// <summary>
        /// Gets or sets the Ticket Node to Node.
        /// </summary>
        /// <value>
        /// The Ticket Nodes to Node.
        /// </value>
        public virtual Node Node { get; set; }
    }
}