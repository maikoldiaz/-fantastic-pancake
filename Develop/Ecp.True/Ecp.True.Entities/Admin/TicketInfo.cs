// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TicketInfo.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The ticket info.
    /// </summary>
    public class TicketInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TicketInfo"/> class.
        /// </summary>
        /// <param name="generatedMovements">Generated movements.</param>
        /// <param name="inventories">Inventories.</param>
        /// <param name="movements">Movements.</param>
        /// <param name="ticket">Ticket.</param>
        public TicketInfo(Ticket ticket, IDictionary<string, int> inventories, IDictionary<string, int> movements, IDictionary<string, int> generatedMovements)
        {
            this.Inventories = inventories;
            this.Movements = movements;
            this.GeneratedMovements = generatedMovements;
            this.Ticket = ticket;
        }

        /// <summary>
        /// Gets the Ticket.
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        public Ticket Ticket { get; }

        /// <summary>
        /// Gets the inventory dictionary.
        /// </summary>
        /// <value>
        /// The inventory dictionary.
        /// </value>
        public IDictionary<string, int> Inventories { get; }

        /// <summary>
        /// Gets the movements dictionary.
        /// </summary>
        /// <value>
        /// The movements dictionary.
        /// </value>
        public IDictionary<string, int> Movements { get; }

        /// <summary>
        /// Gets the GeneratedMovements dictionary.
        /// </summary>
        /// <value>
        /// The GeneratedMovements dictionary.
        /// </value>
        public IDictionary<string, int> GeneratedMovements { get; }
    }
}
