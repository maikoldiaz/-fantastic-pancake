// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TicketInfoRepository.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Repositories.Specialized
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// TicketInfo Repository.
    /// </summary>
    public class TicketInfoRepository : ITicketInfoRepository
    {
        /// <summary>
        /// The Ticket SQL data access.
        /// </summary>
        private readonly ISqlDataAccess<Ticket> ticketSqlDataAccess;

        /// <summary>
        /// The Movement SQL data access.
        /// </summary>
        private readonly ISqlDataAccess<Movement> movementSqlDataAccess;

        /// <summary>
        /// The Inventory SQL data access.
        /// </summary>
        private readonly ISqlDataAccess<InventoryProduct> inventorySqlDataAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketInfoRepository"/> class.
        /// </summary>
        /// <param name="ticketSqlDataAccess">The ticket SQL data access.</param>
        /// <param name="movementSqlDataAccess">The movement SQL data access.</param>
        /// <param name="inventorySqlDataAccess">The inventory SQL data access.</param>
        public TicketInfoRepository(
            ISqlDataAccess<Ticket> ticketSqlDataAccess,
            ISqlDataAccess<Movement> movementSqlDataAccess,
            ISqlDataAccess<InventoryProduct> inventorySqlDataAccess)
        {
            this.ticketSqlDataAccess = ticketSqlDataAccess;
            this.movementSqlDataAccess = movementSqlDataAccess;
            this.inventorySqlDataAccess = inventorySqlDataAccess;
        }

        /// <summary>
        /// Gets the ticket.
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        private DbSet<Ticket> Tickets => this.ticketSqlDataAccess.Set<Ticket>();

        /// <summary>
        /// Gets the inventories.
        /// </summary>
        /// <value>
        /// The inventory.
        /// </value>
        private DbSet<InventoryProduct> Inventories => this.inventorySqlDataAccess.Set<InventoryProduct>();

        /// <summary>
        /// Gets the destination.
        /// </summary>
        /// <value>
        /// The destination.
        /// </value>
        private DbSet<Movement> Movements => this.movementSqlDataAccess.Set<Movement>();

        /// <summary>
        /// Gets informantion required for charts.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>
        ///   <c>true</c> if [has movement exists for connection] [the specified source node identifier]; otherwise, <c>false</c>.
        /// </returns>
        public async Task<TicketInfo> GetTicketInfoAsync(int ticketId)
        {
            var ticket = await this.ticketSqlDataAccess.FirstOrDefaultAsync(t => t.TicketId == ticketId, "CategoryElement").ConfigureAwait(false);

            if (ticket == null)
            {
                return null;
            }

            var inventories = await this.GetInventoriesAsync(ticketId).ConfigureAwait(false);
            var movements = await this.GetMovementsAsync(ticketId).ConfigureAwait(false);
            var generatedMovements = await this.GetGeneratedMovementsAsync(ticketId).ConfigureAwait(false);
            return new TicketInfo(ticket, inventories, movements, generatedMovements);
        }

        /// <inheritdoc/>
        public Task<int> GetLastTicketIdAsync()
        {
            var query = from ticket in this.Tickets orderby ticket.TicketId descending select ticket.TicketId;
            return query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the ownership processing and cutoff date.
        /// </summary>
        /// <param name="segmentId">The segment identifier.</param>
        /// <returns>The dates.</returns>
        public async Task<Dictionary<TicketType, DateTime?>> GetOwnershipBySegmentAsync(int segmentId)
        {
            var dates = new Dictionary<TicketType, DateTime?>();

            DateTime? startDate = await this.GetEnddate(segmentId, TicketType.Ownership).DefaultIfEmpty().MaxAsync().ConfigureAwait(false);

            startDate = startDate == DateTime.MinValue ? await this.GetStartdate(segmentId, TicketType.Cutoff).DefaultIfEmpty().MinAsync().ConfigureAwait(false)
                                                       : startDate.Value.AddDays(1);

            dates.Add(TicketType.Ownership, startDate == DateTime.MinValue ? null : startDate);
            DateTime? cutoffDate = await this.GetEnddate(segmentId, TicketType.Cutoff).DefaultIfEmpty().MaxAsync().ConfigureAwait(false);
            dates.Add(TicketType.Cutoff, cutoffDate == DateTime.MinValue ? null : cutoffDate);

            return dates;
        }

        /// <summary>
        /// Get last ticket async.
        /// </summary>
        /// <param name="segmentId">the segmentId.</param>
        /// <param name="ticketType">the ticketType.</param>
        /// <returns>ticket.</returns>
        public Task<Ticket> GetLastTicketAsync(int segmentId,  TicketType ticketType)
        {
             return this.Tickets.Where(x => x.CategoryElementId == segmentId && x.TicketTypeId == ticketType && x.Status != StatusType.FAILED).
                OrderByDescending(x => x.TicketId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets Inventory information required for charts.
        /// </summary>
        /// <returns>The inventories.</returns>
        private Task<Dictionary<string, int>> GetInventoriesAsync(int ticketId)
        {
            var inventoriesQuery = from ticket in this.Tickets
                                   join inv in this.Inventories
                                   on ticket.TicketId equals inv.TicketId
                                   where ticket.TicketId == ticketId
                                   group inv by inv.SourceSystemElement.Name into inventoryGroup
                                   select new { sourcesystem = inventoryGroup.Key, Count = inventoryGroup.Count() };

            return inventoriesQuery.ToDictionaryAsync(x => x.sourcesystem, x => x.Count);
        }

        /// <summary>
        /// Gets Movements information required for charts.
        /// </summary>
        /// <returns>The Movements.</returns>
        private Task<Dictionary<string, int>> GetMovementsAsync(int ticketId)
        {
            var movementsQuery = from ticket in this.Tickets
                                 join movement in this.Movements
                                 on ticket.TicketId equals movement.TicketId
                                 where ticket.TicketId == ticketId && movement.VariableTypeId == null
                                 group movement by movement.SourceSystemElement.Name into movementGroup
                                 select new { sourcesystem = movementGroup.Key, Count = movementGroup.Count() };

            return movementsQuery.ToDictionaryAsync(x => x.sourcesystem, x => x.Count);
        }

        /// <summary>
        /// Gets Generated Movements information required for charts.
        /// </summary>
        /// <returns>The Generated Movements.</returns>
        private Task<Dictionary<string, int>> GetGeneratedMovementsAsync(int ticketId)
        {
            var generatedMovementsQuery = from ticket in this.Tickets
                                          join movement in this.Movements
                                          on ticket.TicketId equals movement.TicketId
                                          where ticket.TicketId == ticketId && movement.VariableTypeId != null
                                          group movement by movement.VariableTypeId into movementGroup
                                          select new { VariableType = movementGroup.Key, Count = movementGroup.Count() };

            return generatedMovementsQuery.ToDictionaryAsync(x => x.VariableType.ToString(), x => x.Count);
        }

        /// <summary>
        /// Getdates the specified segment identifier.
        /// </summary>
        /// <param name="segmentId">The segment identifier.</param>
        /// <param name="ticketType">Type of the ticket.</param>
        /// <returns>The date.</returns>
        private IQueryable<DateTime> GetEnddate(int segmentId, TicketType ticketType)
        {
            return from x in this.Tickets
                   where x.CategoryElementId == segmentId
                   && x.TicketTypeId == ticketType
                   && x.Status != StatusType.FAILED
                   select x.EndDate;
        }

        /// <summary>
        /// Gets the startdate.
        /// </summary>
        /// <param name="segmentId">The segment identifier.</param>
        /// <param name="ticketType">Type of the ticket.</param>
        /// <returns>The date.</returns>
        private IQueryable<DateTime> GetStartdate(int segmentId, TicketType ticketType)
        {
            return from x in this.Tickets
                   where x.CategoryElementId == segmentId
                   && x.TicketTypeId == ticketType
                   && x.Status != StatusType.FAILED
                   select x.StartDate;
        }
    }
}
