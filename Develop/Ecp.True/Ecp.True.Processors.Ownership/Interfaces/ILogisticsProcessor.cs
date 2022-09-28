// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogisticsProcessor.cs" company="Microsoft">
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
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The ILogisticsProcessor.
    /// </summary>
    public interface ILogisticsProcessor
    {
        /// <summary>
        /// Generates the official logistics.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="systemType">The systemType.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        Task GenerateOfficialLogisticsAsync(Ticket ticket, int systemType);

        /// <summary>
        /// Gets the ticket asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>The ticket.</returns>
        Task<Ticket> GetTicketAsync(int ticketId);

        /// <summary>
        /// Cancel Batch.
        /// </summary>
        /// <param name="ticketId">Id Batch.</param>
        /// <returns>[True] if the creation is success.</returns>
        Task CancelBatchAsync(int ticketId);

        /// <summary>
        /// Validating available nodes.
        /// </summary>
        /// <param name="ticket">The request.</param>
        /// <returns>the Task IEnumerable Ticket.</returns>
        Task<IEnumerable<NodesForSegmentResult>> LogisticMovementNodeValidationsAsync(Ticket ticket);

        /// <summary>
        /// Confirm the logistic movements for sent to sap.
        /// </summary>
        /// <param name="ticket">The request.</param>
        /// <returns>the task Ticket.</returns>
        Task<LogisticMovementsTicketRequest> ConfirmLogisticMovementsAsync(LogisticMovementsTicketRequest ticket);

        /// <summary>
        /// Resent the logistic movements to sap.
        /// </summary>
        /// <param name="ticket">The request.</param>
        /// <returns>the task Ticket.</returns>
        Task ForwardLogisticMovementsAsync(LogisticMovementsTicketRequest ticket);

        /// <summary>
        /// Get movement detail.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>the result.</returns>
        Task<IEnumerable<SapLogisticMovementDetail>> GetLogisticMovementDetailAsync(int ticketId);

        /// <summary>
        /// get the failed logistic movements.
        /// </summary>
        /// <param name="ticket">The request.</param>
        /// <returns>the Task IEnumerable Ticket.</returns>
        Task<IEnumerable<SapLogisticMovementDetail>> FailedLogisticMovementAsync(Ticket ticket);
    }
}
