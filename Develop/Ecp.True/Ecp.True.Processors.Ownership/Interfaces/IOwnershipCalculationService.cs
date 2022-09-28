// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOwnershipCalculationService.cs" company="Microsoft">
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
    using Ecp.True.Processors.Ownership.Entities;

    /// <summary>
    /// The Ownership calculation service interface.
    /// </summary>
    public interface IOwnershipCalculationService
    {
        /// <summary>
        /// Gets the ownership data asynchronous.
        /// </summary>
        /// <param name="ownershipRuleData">The ownership rule data.</param>
        /// <returns>Returns ownership data.</returns>
        Task PopulateOwnershipRuleRequestDataAsync(OwnershipRuleData ownershipRuleData);

        /// <summary>
        /// Gets the logistics details asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="systemType">The systemType identifier.</param>
        /// <returns>The tuple containing collection of LogisticsData and ticket details.</returns>
        Task<LogisticsInfo> GetLogisticsDetailsAsync(int ticketId, int systemType);

        /// <summary>
        /// Adds ownership nodes asynchronous.
        /// </summary>
        /// <param name="nodeIds">The node identifiers.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>Returns completed task.</returns>
        Task AddOwnershipNodesAsync(IEnumerable<int> nodeIds, int ticketId);

        /// <summary>
        /// Updates the ticket errors asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>Returns completed task.</returns>
        Task UpdateTicketErrorsAsync(int ticketId, string errorMessage);

        /// <summary>
        /// Updates the ticket status and blobpath asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket.</param>
        /// <param name="systemType">The systemType.</param>
        /// <returns>The tasks.</returns>
        Task UpdateTicketStatusAndBlobpathAsync(int ticketId, SystemType systemType);
    }
}
