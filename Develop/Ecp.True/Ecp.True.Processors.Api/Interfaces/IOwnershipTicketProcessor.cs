// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOwnershipTicketProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// The ownership ticket processor interface.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.Interfaces.IProcessor" />
    public interface IOwnershipTicketProcessor : IProcessor
    {
        /// <summary>
        /// Validates the existing ticket asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The result of validation.</returns>
        Task<bool> ValidateExistingTicketAsync(Ticket ticket);

        /// <summary>
        /// Gets the ownership processing and cutoff date.
        /// </summary>
        /// <param name="segmentId">The segment identifier.</param>
        /// <returns>The dates.</returns>
        Task<Dictionary<TicketType, DateTime?>> GetOwnershipBySegmentAsync(int segmentId);

        /// <summary>
        /// Gets the ownership last performed date by segment asynchronous.
        /// </summary>
        /// <param name="segmentId">The segment identifier.</param>
        /// <returns>The ownership last performed date.</returns>
        Task<DateTime> GetOwnershipLastPerformedDateBySegmentAsync(int segmentId);

        /// <summary>
        /// Gets the ticket processing status.
        /// </summary>
        /// <param name="segmentId">The segment identifier.</param>
        /// <param name="isOwnershipCheck">To check whether ownership processing is going on.</param>
        /// <returns>The ticket processing status.</returns>
        Task<string> GetTicketProcessingStatusAsync(int segmentId, bool isOwnershipCheck);

        /// <summary>
        /// Gets the pending delta inventories.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The inventories.</returns>
        Task<IEnumerable<OperationalDeltaInventory>> GetDeltaInventoriesAsync(Ticket ticket);

        /// <summary>
        /// Gets the pending delta movements.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The movements.</returns>
        Task<IEnumerable<OperationalDeltaMovement>> GetDeltaMovementsAsync(Ticket ticket);
    }
}
