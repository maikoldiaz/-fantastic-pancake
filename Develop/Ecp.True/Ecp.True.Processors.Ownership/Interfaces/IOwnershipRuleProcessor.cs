// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOwnershipRuleProcessor.cs" company="Microsoft">
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
    using System.Threading.Tasks;

    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Ownership.Entities;

    /// <summary>
    /// The Ownership Rule Processor Interface.
    /// </summary>
    public interface IOwnershipRuleProcessor
    {
        /// <summary>
        /// Processes the asynchronous.
        /// </summary>
        /// <param name="ownershipRuleData">The ownership rule data.</param>
        /// <param name="chainType">The chain type.</param>
        /// <returns>The task.</returns>
        Task<OwnershipRuleData> ProcessAsync(OwnershipRuleData ownershipRuleData, ChainType chainType);

        /// <summary>
        /// Cleans the ownership data.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>The task.</returns>
        Task CleanOwnershipDataAsync(int ticketId);

        /// <summary>
        /// Synchronizes the ownership rules asynchronous.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>Returns a task.</returns>
        Task SyncOwnershipRulesAsync(string source);

        /// <summary>
        /// Queues the synchronize ownership rule asynchronous.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>Returns a task.</returns>
        Task QueueSyncOwnershipRuleAsync(string source);

        /// <summary>
        /// Gets the ownership ticket by ownership node identifier asynchronous.
        /// </summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <returns>The ticket Id.</returns>
        Task<int> GetOwnershipTicketByOwnershipNodeIdAsync(int ownershipNodeId);

        /// <summary>
        /// Finalizes the process asynchronous.
        /// </summary>
        /// <param name="ownershipRuleData">The ticket identifier.</param>
        /// <returns>The task.</returns>
        Task FinalizeProcessAsync(OwnershipRuleData ownershipRuleData);
    }
}
