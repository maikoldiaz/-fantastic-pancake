// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConsolidationProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Delta.Entities;

    /// <summary>
    /// The ConsolidationProcessor.
    /// </summary>
    public interface IConsolidationProcessor
    {
        /// <summary>
        /// Consolidates the asynchronous.
        /// </summary>
        /// <param name="batchInfo">The batchInfo data.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task ConsolidateAsync(ConsolidationBatch batchInfo);

        /// <summary>
        /// Gets the consolidation batches.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The collection of BatchInfo.</returns>
        Task<IEnumerable<ConsolidationBatch>> GetConsolidationBatchesAsync(Ticket ticket);

        /// <summary>
        /// Completes the consolidation asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="segmentId">The segment identifier.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task CompleteConsolidationAsync(int ticketId, int segmentId);

        /// <summary>
        /// Validates the ticket asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>The isValid and ticket entity.</returns>
        Task<(bool isValid, Ticket ticket, string errorMessage)> ValidateTicketAsync(int ticketId);
    }
}
