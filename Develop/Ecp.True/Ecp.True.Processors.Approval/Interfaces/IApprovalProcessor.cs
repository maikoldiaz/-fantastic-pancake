// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApprovalProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Approval.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core.Entities;
    using Ecp.True.Entities.Dto;

    /// <summary>
    /// The approval processor.
    /// </summary>
    public interface IApprovalProcessor
    {
        /// <summary>
        /// Updates the ownership node status.
        /// </summary>
        /// <param name="approvalRequest">The approval request.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        Task<List<ErrorInfo>> UpdateOwnershipNodeStatusAsync(NodeOwnershipApprovalRequest approvalRequest);

        /// <summary>
        /// Gets the ownership node balance summary asynchronous.
        /// </summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <returns>
        /// Ownership node balance summary collection.
        /// </returns>
        Task<OwnershipNodeBalanceDetails> GetOwnershipNodeBalanceDetailsAsync(int ownershipNodeId);

        /// <summary>
        /// Gets the ownership node identifier asynchronous.
        /// </summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <returns>The ownership nodes query.</returns>
        Task SendOwnershipNodeIdForApprovalAsync(int ownershipNodeId);

        /// <summary>
        /// Updates the ownership node state asynchronous.
        /// </summary>
        /// <param name="approvalRequest">The approval request.</param>
        /// <returns>The Task.</returns>
        Task UpdateOwnershipNodeStateAsync(NodeOwnershipApprovalRequest approvalRequest);

        /// <summary>
        /// Saves the operative movements with ownership percentage asynchronous.
        /// </summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <returns>The Task.</returns>
        Task SaveOperativeMovementsAsync(int ownershipNodeId);
    }
}
