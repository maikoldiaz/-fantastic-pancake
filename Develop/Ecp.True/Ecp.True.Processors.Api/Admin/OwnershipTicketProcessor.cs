// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipTicketProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Core;

    /// <summary>
    /// The ownership ticket processor.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Api.ProcessorBase" />
    public class OwnershipTicketProcessor : ProcessorBase, IOwnershipTicketProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipTicketProcessor" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public OwnershipTicketProcessor(
            IRepositoryFactory factory)
             : base(factory)
        {
        }

        /// <summary>
        /// Validates the existing ticket asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The result of validation.</returns>
        public async Task<bool> ValidateExistingTicketAsync(Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var ticketRepository = this.RepositoryFactory.CreateRepository<Ticket>();

            var numOfTickets = await ticketRepository.GetCountAsync(x => x.CategoryElementId == ticket.CategoryElementId
                && x.TicketTypeId == TicketType.Ownership && x.Status == StatusType.PROCESSING).ConfigureAwait(false);

            return numOfTickets == 0;
        }

        /// <summary>
        /// Gets the ownership last performed date by segment asynchronous.
        /// </summary>
        /// <param name="segmentId">The segment identifier.</param>
        /// <returns>The ownership last performed date.</returns>
        public async Task<DateTime> GetOwnershipLastPerformedDateBySegmentAsync(int segmentId)
        {
            var ticketRepository = this.RepositoryFactory.CreateRepository<Ticket>();

            var tickets = await ticketRepository.GetAllAsync(a =>
            a.CategoryElementId == segmentId &&
            a.TicketTypeId == TicketType.Ownership &&
            a.Status == StatusType.PROCESSED).ConfigureAwait(false);

            return tickets.OrderByDescending(a => a.EndDate).Select(a => a.EndDate).FirstOrDefault();
        }

        /// <summary>
        /// Gets the ownership processing and cutoff date.
        /// </summary>
        /// <param name="segmentId">The segment identifier.</param>
        /// <returns>The dates.</returns>
        public Task<Dictionary<TicketType, DateTime?>> GetOwnershipBySegmentAsync(int segmentId)
        {
            return this.RepositoryFactory.TicketInfoRepository.GetOwnershipBySegmentAsync(segmentId);
        }

        /// <summary>
        /// Gets the pending delta inventories.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The inventories.</returns>
        public async Task<IEnumerable<OperationalDeltaInventory>> GetDeltaInventoriesAsync(Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var parameters = new Dictionary<string, object>
            {
                { "@Segmentid", ticket.CategoryElementId },
                { "@StartDate", ticket.StartDate },
                { "@EndDate", ticket.EndDate },
                { "@IsOriginal", false },
            };
            var result = await this.CreateRepository<OperationalDeltaInventory>().ExecuteQueryAsync(
                Repositories.Constants.OriginalOrUpdatedInventoriesProcedureName, parameters).ConfigureAwait(false);
            return result.OrderByDescending(x => x.InventoryDate).ThenByDescending(y => y.InventoryId);
        }

        /// <summary>
        /// Gets the pending delta movements.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The inventories.</returns>
        public async Task<IEnumerable<OperationalDeltaMovement>> GetDeltaMovementsAsync(Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var parameters = new Dictionary<string, object>
            {
                { "@Segmentid", ticket.CategoryElementId },
                { "@StartDate", ticket.StartDate },
                { "@EndDate", ticket.EndDate },
                { "@IsOriginal", false },
            };
            var result = await this.CreateRepository<OperationalDeltaMovement>().ExecuteQueryAsync(
                Repositories.Constants.OriginalOrUpdatedMovementsProcedureName, parameters).ConfigureAwait(false);
            return result.OrderByDescending(x => x.OperationalDate).ThenByDescending(y => y.MovementId);
        }

        /// <summary>
        /// Gets the ticket processing status.
        /// </summary>
        /// <param name="segmentId">The segment identifier.</param>
        /// <param name="isOwnershipCheck">The ticket type.</param>
        /// <returns>The ticket processing status.</returns>
        public async Task<string> GetTicketProcessingStatusAsync(int segmentId, bool isOwnershipCheck)
        {
            var ticketRepository = this.RepositoryFactory.CreateRepository<Ticket>();
            var ticketsInProcessing = await ticketRepository.GetAllAsync(a =>
                a.CategoryElementId == segmentId &&
                a.Status == StatusType.PROCESSING).ConfigureAwait(false);

            if (ticketsInProcessing == null)
            {
                return null;
            }
            else if (isOwnershipCheck && ticketsInProcessing.Any(x => x.TicketTypeId == TicketType.Ownership))
            {
                return TicketType.Ownership.ToString();
            }
            else if (!isOwnershipCheck && ticketsInProcessing.Any(x => x.TicketTypeId == TicketType.Cutoff))
            {
                return TicketType.Cutoff.ToString();
            }
            else if (!isOwnershipCheck && ticketsInProcessing.Any(x => x.TicketTypeId == TicketType.Delta))
            {
                return TicketType.Delta.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
