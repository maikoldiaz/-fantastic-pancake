// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemSegmentBalanceBase.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Balance
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The segment balance service.
    /// </summary>
    public class SystemSegmentBalanceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSegmentBalanceBase"/> class.
        /// </summary>
        protected SystemSegmentBalanceBase()
        {
        }

        /// <summary>
        /// Gets the inventories and movements asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket identifier.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>The tuple of InventoryProducts abd Movements.</returns>
        protected static async Task<(IEnumerable<InventoryProduct> inventoryProducts, IEnumerable<Movement> movements)> GetInventoriesAndMovementsAsync(Ticket ticket, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            var movements = await GetMovementsAsync(ticket, unitOfWork).ConfigureAwait(false);
            var inventoryProducts = await GetInventoriesAsync(ticket, unitOfWork).ConfigureAwait(false);

            return (inventoryProducts, movements);
        }

        /// <summary>
        /// Gets the movements asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket identifier.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>The collection of Movement.</returns>
        private static async Task<IEnumerable<Movement>> GetMovementsAsync(Ticket ticket, IUnitOfWork unitOfWork)
        {
            var movementRepository = unitOfWork.CreateRepository<Movement>();
            return await movementRepository.GetAllAsync(
                 a =>
                 a.TicketId == ticket.TicketId,
                 "MovementSource",
                 "MovementDestination").ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the inventories asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket identifier.</param>
        /// <returns>The collection of InventoryProduct.</returns>
        private static async Task<IEnumerable<InventoryProduct>> GetInventoriesAsync(Ticket ticket, IUnitOfWork unitOfWork)
        {
            var inventoryProductRepository = unitOfWork.CreateRepository<InventoryProduct>();
            return await inventoryProductRepository.GetAllAsync(
                a =>
                a.TicketId != null &&
                a.SegmentId == ticket.CategoryElementId &&
                a.InventoryDate.Value.Date >= ticket.StartDate.AddDays(-1).Date &&
                a.InventoryDate.Value.Date <= ticket.EndDate.Date).ConfigureAwait(false);
        }
    }
}
