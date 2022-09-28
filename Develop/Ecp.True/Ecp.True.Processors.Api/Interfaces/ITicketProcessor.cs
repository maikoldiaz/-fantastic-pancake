// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITicketProcessor.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// The ticket processor interface.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.Interfaces.IProcessor" />
    public interface ITicketProcessor : IProcessor
    {
        /// <summary>
        /// Gets the ticket by identifier.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>
        /// Return the ticket.
        /// </returns>
        Task<Ticket> GetTicketByIdAsync(int ticketId);

        /// <summary>
        /// Gets the ticket info.
        /// </summary>
        /// <param name="ticketId">the ticket Id.</param>
        /// <returns>Return the ticket info.</returns>
        Task<TicketInfo> GetTicketInfoAsync(int ticketId);

        /// <summary>
        /// Gets the ticket validation status.
        /// </summary>
        /// <param name="ticket">The start Date.</param>
        /// <returns>The last ticket id.</returns>
        Task<bool> ValidateCutOffAsync(Ticket ticket);

        /// <summary>
        /// Exists the ticket asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>
        /// return true/false.
        /// </returns>
        Task<bool> ExistsTicketAsync(Ticket ticket);

        /// <summary>
        /// Exists the delta ticket asynchronous.
        /// </summary>
        /// <param name="segmentId">The segment identifier.</param>
        /// <returns>return true/false.</returns>
        Task<bool> ExistsDeltaTicketAsync(int segmentId);

        /// <summary>
        /// Saves the ticket asynchronous.
        /// </summary>
        /// <param name="operationalCutOff">The operational cut off.</param>
        /// <returns>Returns the Status of update.</returns>
        Task SaveTicketAsync(OperationalCutOff operationalCutOff);

        /// <summary>
        /// Gets the delta exceptions details.
        /// </summary>
        /// <param name="ticketId">the ticket Id.</param>
        /// <param name="ticketType">Ticket Type.</param>
        /// <returns>Return the delta exceptions details.</returns>
        Task<IEnumerable<DeltaExceptions>> GetDeltaExceptionsDetailsAsync(int ticketId, TicketType ticketType);

        /// <summary>
        /// Updates the comment asynchronous.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <returns>The task.</returns>
        Task UpdateCommentAsync(OperationalCutOffBatch batch);

        /// <summary>
        /// Approves the official node delta asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The task.</returns>
        Task<DeltaNodeApprovalResponse> SendDeltaNodeForApprovalAsync(DeltaNodeStatusRequest request);

        /// <summary>
        /// Get the delta node for reopen asynchronous.
        /// </summary>
        /// <param name="deltaNodeId">The delta node identifier.</param>
        /// <returns>The list of delta node reopen response.</returns>
        Task<IEnumerable<DeltaNodeReopenResponse>> GetDeltaNodesForReopenAsync(int deltaNodeId);

        /// <summary>
        /// Reopens the delta nodes asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>the task.</returns>
        Task ReopenDeltaNodesAsync(DeltaNodeReopenRequest request);
    }
}
