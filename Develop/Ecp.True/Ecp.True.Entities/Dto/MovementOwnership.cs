// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementOwnership.cs" company="Microsoft">
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
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The MovementOwnership.
    /// </summary>
    public class MovementOwnership
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovementOwnership" /> class.
        /// </summary>
        public MovementOwnership()
        {
            this.Ownerships = new List<Ownership>();
        }

        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public int TicketId { get; set; }

        /// <summary>
        /// Gets or sets the movement transaction identifier.
        /// </summary>
        /// <value>
        /// The movement transaction identifier.
        /// </value>
        public int MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>
        public string MovementId { get; set; }

        /// <summary>
        /// Gets or sets the event type.
        /// </summary>
        /// <value>
        /// The event type.
        /// </value>
        public EventType? EventType { get; set; }

        /// <summary>
        /// Gets the ownerships.
        /// </summary>
        /// <value>
        /// The ownerships.
        /// </value>
        public ICollection<Ownership> Ownerships { get; }
    }
}