// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITicketInfoRepository.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.DataAccess.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The movement custom repository.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Interfaces.IRepository{Ecp.True.Entities.Admin.TicketChartInfo}" />
    public interface ITicketInfoRepository
    {
        /// <summary>
        /// Gets informantion required for charts.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>The ticket info for given ticketId.</returns>
        Task<TicketInfo> GetTicketInfoAsync(int ticketId);

        /// <summary>
        /// Gets the last ticket identifier asynchronous.
        /// </summary>
        /// <returns>The last ticket id.</returns>
        Task<int> GetLastTicketIdAsync();

        /// <summary>
        /// Gets the ownership processing and cutoff date.
        /// </summary>
        /// <param name="segmentId">The segment identifier.</param>
        /// <returns>The dates.</returns>
        Task<Dictionary<TicketType, DateTime?>> GetOwnershipBySegmentAsync(int segmentId);

        /// <summary>
        /// Get the Last ticket.
        /// </summary>
        /// <param name="segmentId">The segment identifier.</param>
        /// <param name="ticketType">The ticketType identifier.</param>
        /// <returns>The ticket.</returns>
        Task<Ticket> GetLastTicketAsync(int segmentId, TicketType ticketType);
    }
}