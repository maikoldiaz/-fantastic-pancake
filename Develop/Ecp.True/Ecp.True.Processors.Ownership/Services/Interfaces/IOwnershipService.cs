// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOwnershipService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core.Entities;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Proxies.OwnershipRules.Response;

    /// <summary>
    /// The IOwnershipService.
    /// </summary>
    public interface IOwnershipService
    {
        /// <summary>
        /// Build the ownership results .
        /// </summary>
        /// <param name="inventoryResultList">List of ownership result for inventories.</param>
        /// <param name="movementResultList">List of ownership result for movements.</param>
        /// <returns>The task.</returns>
        IEnumerable<Ownership> BuildOwnershipResults(
            IEnumerable<OwnershipResultInventory> inventoryResultList,
            IEnumerable<OwnershipResultMovement> movementResultList);

        /// <summary>
        /// Consolidates the inventory result.
        /// </summary>
        /// <param name="inventoryResultList">The inventory result list.</param>
        /// <param name="previousInventories">The previous inventory.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>
        /// The ownership result inventory.
        /// </returns>
        IEnumerable<OwnershipResultInventory> ConsolidateInventoryResults(
            IEnumerable<OwnershipResultInventory> inventoryResultList,
            IEnumerable<PreviousInventoryOperationalData> previousInventories,
            int ticketId);

        /// <summary>
        /// Consolidates the movement results.
        /// </summary>
        /// <param name="movementResultList">The movement result list.</param>
        /// <param name="previousMovements">The previous movements.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>The ownership result movement.</returns>
        IEnumerable<OwnershipResultMovement> ConsolidateMovementResults(
            IEnumerable<OwnershipResultMovement> movementResultList,
            IEnumerable<PreviousMovementOperationalData> previousMovements,
            int ticketId);

        /// <summary>
        /// Registers the ownership results asynchronous.
        /// </summary>
        /// <param name="ownershipRuleData">The ownershipRuleData.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task RegisterResultsAsync(OwnershipRuleData ownershipRuleData);

        /// <summary>
        /// Handles validation failures asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket id.</param>
        /// <param name="errorInfos">The error list.</param>
        /// <param name="ownershipErrorMovements"> The error movements.</param>
        /// <param name="ownershipErrorInventories">The error inventories.</param>
        /// <param name="hasProcessingErrors">The Processing errors check.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task HandleFailureAsync(
            int ticketId,
            IEnumerable<ErrorInfo> errorInfos,
            IEnumerable<OwnershipErrorMovement> ownershipErrorMovements,
            IEnumerable<OwnershipErrorInventory> ownershipErrorInventories,
            bool hasProcessingErrors);

        /// <summary>
        /// Gets unprocessed tickets asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket id.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task<IEnumerable<Ticket>> GetUnprocessedTicketsAsync(int ticketId);

        /// <summary>
        /// Build the ownership results .
        /// </summary>
        /// <param name="commercialMovementsResults">List of commercial movements results.</param>
        /// <param name="newMovementsList">List of new movements.</param>
        /// <param name="cancellationMovements">List of cancellation movements.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>The task.</returns>
        Task<IEnumerable<Movement>> BuildOwnershipMovementResultsAsync(
            IEnumerable<CommercialMovementsResult> commercialMovementsResults,
            IEnumerable<NewMovement> newMovementsList,
            IEnumerable<CancellationMovementDetail> cancellationMovements,
            int ticketId,
            IUnitOfWork unitOfWork);
    }
}
