// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementRepository.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The movement custom Repository.
    /// </summary>
    public class MovementRepository : Repository<Movement>, IMovementRepository
    {
        /// <summary>
        /// The SQL data access.
        /// </summary>
        private readonly ISqlDataAccess<Movement> sqlDataAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementRepository"/> class.
        /// </summary>
        /// <param name="sqlDataAccess">The SQL data access.</param>
        public MovementRepository(ISqlDataAccess<Movement> sqlDataAccess)
            : base(sqlDataAccess)
        {
            this.sqlDataAccess = sqlDataAccess;
        }

        /// <summary>
        /// Gets the movements.
        /// </summary>
        /// <value>
        /// The movements.
        /// </value>
        private DbSet<Movement> Movements => this.sqlDataAccess.EntitySet();

        /// <summary>
        /// Gets the latest net standard volume by movement identifier.
        /// </summary>
        /// <param name="movementId">The movement identifier.</param>
        /// <returns>
        /// The net standard volume.
        /// </returns>
        public async Task<Movement> GetLatestMovementAsync(string movementId)
        {
            var result = await this.Movements
                        .Where(x => x.MovementId == movementId)
                        .OrderByDescending(x => x.MovementTransactionId)
                        .Select(x => new
                        {
                            x.NetStandardVolume,
                            x.EventType,
                        })
                        .FirstOrDefaultAsync().ConfigureAwait(false);

            return result != null ? new Movement { EventType = result.EventType, NetStandardVolume = result.NetStandardVolume } : null;
        }

        /// <summary>
        /// Gets the latest net standard volume by movement identifier.
        /// </summary>
        /// <param name="movementId">The movement identifier.</param>
        /// <returns>
        /// The net standard volume.
        /// </returns>
        public async Task<Movement> GetLatestBlockchainMovementAsync(string movementId)
        {
            var result = await this.Movements
                        .Where(x => x.MovementId == movementId && x.BlockchainStatus == Entities.Core.StatusType.PROCESSED && x.EventType == EventType.Insert.ToString("G"))
                        .OrderBy(x => x.MovementTransactionId)
                        .Select(x => new
                        {
                            x.NetStandardVolume,
                            x.EventType,
                            x.BlockNumber,
                            x.TransactionHash,
                        })
                        .FirstOrDefaultAsync().ConfigureAwait(false);

            return result != null ?
                new Movement { EventType = result.EventType, NetStandardVolume = result.NetStandardVolume, BlockNumber = result.BlockNumber, TransactionHash = result.TransactionHash } : null;
        }

        /// <summary>
        /// Determines whether [has movement exists for connection] [the specified source node identifier].
        /// </summary>
        /// <param name="sourceNodeId">The source node identifier.</param>
        /// <param name="destinationNodeId">The destination node identifier.</param>
        /// <returns>
        /// <c>true</c> if [has movement exists for connection] [the specified source node identifier]; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> HasActiveMovementForConnectionAsync(int sourceNodeId, int destinationNodeId)
        {
            var movementsCount = await this.GetCountAsync(
               a =>
               a.MovementSource.SourceNodeId == sourceNodeId &&
               a.MovementDestination.DestinationNodeId == destinationNodeId).ConfigureAwait(false);

            return movementsCount != 0;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Movement>> GetMovementsForOfficialDeltaCalculationAsync(IEnumerable<int> nodeIds, Ticket ticket)
        {
            var query = from mo in this.Movements.Where(x => x.OfficialDeltaTicketId != null && x.SegmentId == ticket.CategoryElementId &&
                        (x.OfficialDeltaMessageTypeId != null || x.SourceSystemId == Core.Constants.ManualInvOfficial || x.SourceSystemId == Core.Constants.ManualMovOfficial))
                        select mo;

            return await query
                    .Include(h => h.MovementSource)
                    .Include(h => h.MovementDestination)
                    .Include(h => h.Owners)
                    .Include(h => h.Period)
                    .Where(y => ((y.MovementSource != null &&
                     y.MovementSource.SourceNodeId != null && nodeIds.Contains(y.MovementSource.SourceNodeId.Value))
                     || (y.MovementDestination != null &&
                     y.MovementDestination.DestinationNodeId != null && nodeIds.Contains(y.MovementDestination.DestinationNodeId.Value)))
                     && y.Period.StartTime >= ticket.StartDate.AddDays(-1) && y.Period.EndTime <= ticket.EndDate)
                    .ToListAsync().ConfigureAwait(false);
        }
    }
}
