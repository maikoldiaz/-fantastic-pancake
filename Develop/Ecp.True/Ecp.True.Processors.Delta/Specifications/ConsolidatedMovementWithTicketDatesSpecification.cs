// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidatedMovementWithTicketDatesSpecification.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Specifications
{
    using System;
    using System.Linq.Expressions;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Specifications;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// Specifies whether a movement has the same start and end dates as the given ticket.
    /// </summary>
    public class ConsolidatedMovementWithTicketDatesSpecification : CompositeSpecification<ConsolidatedMovement>
    {
        private readonly Ticket ticket;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsolidatedMovementWithTicketDatesSpecification"/> class.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        public ConsolidatedMovementWithTicketDatesSpecification(Ticket ticket)
        {
            this.ticket = ticket;
        }

        /// <inheritdoc />
        public override Expression<Func<ConsolidatedMovement, bool>> ToExpression()
        {
            return movement => this.ticket.StartDate == movement.StartDate
                               && this.ticket.EndDate == movement.EndDate;
        }
    }
}