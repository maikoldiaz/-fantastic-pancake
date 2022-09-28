// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDeltaProcessor.cs" company="Microsoft">
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
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta.Entities;

    /// <summary>
    /// The IDeltaProcessor.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.Interfaces.IProcessor" />
    public interface IDeltaProcessor : IProcessor
    {
        /// <summary>
        /// Processes the asynchronous.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="chainType">The chain type.</param>
        /// <returns>The object.</returns>
        Task<DeltaData> ProcessAsync(object data, ChainType chainType);

        /// <summary>
        /// Validates the ticket asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>
        /// The isValid and ticket entity.
        /// </returns>
        Task<(bool isValid, Ticket ticket)> ValidateTicketAsync(int ticketId);

        /// <summary>
        /// Finalizes the process asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>The task.</returns>
        Task FinalizeProcessAsync(int ticketId);

        /// <summary>
        /// Gets the official delta period asynchronous.
        /// </summary>
        /// <param name="segmentId">The segment identifier.</param>
        /// <param name="years">The years.</param>
        /// <param name="isPerNodeReport">if set to <c>true</c> [is per node report].</param>
        /// <returns>The task.</returns>
        Task<OfficialDeltaPeriodInfo> GetOfficialDeltaPeriodAsync(int segmentId, int years, bool isPerNodeReport);

        /// <summary>
        /// Validates the previous period nodes asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>
        /// If nodes of previous periods are approved or not.
        /// </returns>
        Task<bool> ValidatePreviousOfficialPeriodAsync(Ticket ticket);

        /// <summary>
        /// Gets the official delta ticket processing status asynchronous.
        /// </summary>
        /// <param name="segmentId">The segment identifier.</param>
        /// <returns>The official delta ticket processing status.</returns>
        Task<string> GetOfficialDeltaTicketProcessingStatusAsync(int segmentId);

        /// <summary>
        /// Gets the unapproved official nodes asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>Unapproved Official Nodes.</returns>
        Task<IEnumerable<UnapprovedOfficialNodes>> GetUnapprovedOfficialNodesAsync(Ticket ticket);
    }
}
