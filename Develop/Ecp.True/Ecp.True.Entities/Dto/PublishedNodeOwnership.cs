// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PublishedNodeOwnership.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Dto
{
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Core;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// Published Node Ownership.
    /// </summary>
    public class PublishedNodeOwnership
    {
        /// <summary>
        /// Gets or sets the movements.
        /// </summary>
        /// <value>
        /// The movements.
        /// </value>
        public IEnumerable<Movement> Movements { get; set; }

        /// <summary>
        /// Gets or sets the inventory ownerships.
        /// </summary>
        /// <value>
        /// The inventory ownerships.
        /// </value>
        public IEnumerable<EditOwnershipInfo<InventoryOwnership>> InventoryOwnerships { get; set; }

        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public int TicketId { get; set; }

        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public IEnumerable<int> DeletedTransactionIds { get; set; }

        /// <summary>
        /// Gets a value indicating whether there are deleted movement ownerships.
        /// </summary>
        /// <value>
        /// true or false.
        /// </value>
        public bool HasDeletedMovementOwnerships
        {
            get
            {
                return this.Movements != null && this.Movements.Any(x => x.EventType.EqualsIgnoreCase(EventType.Delete.ToString("G")));
            }
        }

        /// <summary>
        /// Gets or sets the ownership nodeId.
        /// </summary>
        /// <value>
        /// The ownership nodeId.
        /// </value>
        public int OwnershipNodeId { get; set; }
    }
}
