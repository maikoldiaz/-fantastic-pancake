// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBalanceProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Balance.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Balance.Calculation.Input;
    using Ecp.True.Processors.Balance.Calculation.Output;
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// The balance processor.
    /// </summary>
    public interface IBalanceProcessor : IProcessor
    {
        /// <summary>
        /// Calculates the asynchronous.
        /// </summary>
        /// <param name="getUnbalances">The get unbalances.</param>
        /// <returns>
        /// The collection of unbalances task.
        /// </returns>
        Task<IEnumerable<UnbalanceComment>> CalculateAsync(UnbalanceRequest getUnbalances);

        /// <summary>
        /// Gets the balance input asynchronously.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The task.</returns>
        Task<BalanceInput> GetBalanceInputAsync(Ticket ticket);

        /// <summary>
        /// Processes the calculation output asynchronously.
        /// </summary>
        /// <param name="operationalCutOffInput">The operational cut off input.</param>
        /// <returns>The task.</returns>
        Task<CalculationOutput> ProcessCalculationAsync(OperationalCutOffInfo operationalCutOffInput);

        /// <summary>
        /// Get the ticket by Ticket ID.
        /// </summary>
        /// <param name="ticketId">The ticket ID.</param>
        /// <returns>The ticket.</returns>
        Task<Ticket> GetTicketByIdAsync(int ticketId);

        /// <summary>
        /// Process the segment asynchronously.
        /// </summary>
        /// <param name="ticketId">The ticket ID.</param>
        /// <returns>The task.</returns>
        Task<IEnumerable<SegmentUnbalance>> ProcessSegmentAsync(int ticketId);

        /// <summary>
        /// Process the system asynchronously.
        /// </summary>
        /// <param name="ticketId">The ticket ID.</param>
        /// <returns>The task.</returns>
        Task<IEnumerable<SystemUnbalance>> ProcessSystemAsync(int ticketId);

        /// <summary>
        /// Complete the balance calculation.
        /// </summary>
        /// <param name="operationalCutOffInput">The operational cut off input.</param>
        /// <returns>The task.</returns>
        Task CompleteAsync(OperationalCutOffInfo operationalCutOffInput);

        /// <summary>
        /// Registers the movements and unbalance.
        /// </summary>
        /// <param name="operationalCutOffInput">The operational cut off input.</param>
        /// <returns>The task.</returns>
        Task RegisterAsync(OperationalCutOffInfo operationalCutOffInput);

        /// <summary>
        /// Cleans the ownership data.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>The task.</returns>
        Task CleanOperationalCutOffDataAsync(int ticketId);

        /// <summary>
        /// Handles failure.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>The task.</returns>
        Task HandleFailureAsync(int ticketId, string errorMessage);

        /// <summary>
        /// Validating the unbalance ticket and returning the corresponding nodes.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The nodes.</returns>
        Task<IEnumerable<OwnershipInitialInventoryNode>> ValidateOwnershipInitialInventoryAsync(Ticket ticket);

        /// <summary>
        /// Gets the transfer points asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task<IEnumerable<OfficialTransferPointMovement>> GetTransferPointsAsync(Ticket ticket);

        /// <summary>
        /// Finalizes the process asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>The task.</returns>
        Task FinalizeProcessAsync(int ticketId);

        /// <summary>
        /// Gets the sap tracking errors asynchronous.
        /// </summary>
        /// <param name="sapTrackingId">The sap tracking identifier.</param>
        /// <returns>The SAP Tracking errors.</returns>
        Task<IEnumerable<SapTrackingError>> GetSapTrackingErrorsAsync(int sapTrackingId);

        /// <summary>
        /// Gets the first time nodes asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The list of first time nodes.</returns>
        Task<IEnumerable<int>> GetFirstTimeNodesAsync(Ticket ticket);

        /// <summary>
        /// Deletes the balance asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The balance type.</typeparam>
        /// <param name="ticketId">The ticket id.</param>
        /// <returns>The task.</returns>
        Task DeleteBalanceAsync<TEntity>(int ticketId)
            where TEntity : class, ITicketEntity;
    }
}
