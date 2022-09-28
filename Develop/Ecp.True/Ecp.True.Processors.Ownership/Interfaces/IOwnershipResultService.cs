// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOwnershipResultService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Proxies.OwnershipRules.Response;

    /// <summary>
    /// The IOwnershipResultService.
    /// </summary>
    public interface IOwnershipResultService
    {
        /// <summary>
        /// Saves the movement and inventory results asynchronous.
        /// </summary>
        /// <param name="commercialMovementsResults">The commercial movements results list.</param>
        /// <param name="newMovementsList">The new movements list.</param>
        /// <param name="cancellationMovements">The cancellation movement list.</param>
        /// <param name="ticketId">The ticket Id.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the result of the asynchronous operation.
        /// </returns>
        Task<IEnumerable<Movement>> BuildOwnershipMovementResultsAsync(
            IEnumerable<CommercialMovementsResult> commercialMovementsResults,
            IEnumerable<NewMovement> newMovementsList,
            IEnumerable<CancellationMovementDetail> cancellationMovements,
            int ticketId,
            IUnitOfWork unitOfWork);
    }
}
