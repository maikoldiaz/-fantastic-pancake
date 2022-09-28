// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidatedMovementSpecification.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Specifications;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// Specifies a movement that should be included in the consolidated movement collection.
    /// </summary>
    public class ConsolidatedMovementSpecification : CompositeSpecification<ConsolidatedMovement>
    {
        private readonly ConsolidatedMovementNodesArePendingOfficialSpecification nodesArePendingOfficialSpecification;
        private readonly ConsolidatedMovementWithTicketDatesSpecification datesSpecificationAreSameAsTicket;
        private readonly ConsolidatedMovementNodesAreApprovedSpecification nodesAreApproved;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsolidatedMovementSpecification"/> class.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="pendingOfficialNodes">The pending official movements node list.</param>
        public ConsolidatedMovementSpecification(Ticket ticket, ICollection<int> pendingOfficialNodes)
        {
            this.nodesArePendingOfficialSpecification = new ConsolidatedMovementNodesArePendingOfficialSpecification(pendingOfficialNodes);
            this.datesSpecificationAreSameAsTicket = new ConsolidatedMovementWithTicketDatesSpecification(ticket);
            this.nodesAreApproved = new ConsolidatedMovementNodesAreApprovedSpecification();

            this.Include(this.nodesArePendingOfficialSpecification.IncludeProperties)
                .Include(this.datesSpecificationAreSameAsTicket.IncludeProperties)
                .Include(this.nodesAreApproved.IncludeProperties);
        }

        /// <inheritdoc />
        public override ICollection<string> IncludeProperties { get; } = new List<string> { $"{nameof(ConsolidatedMovement.ConsolidatedOwners)}" };

        /// <inheritdoc />
        public override Expression<Func<ConsolidatedMovement, bool>> ToExpression()
        {
            return this.nodesArePendingOfficialSpecification
                .AndAlso(this.datesSpecificationAreSameAsTicket)
                .AndAlso(this.nodesAreApproved);
        }
    }
}