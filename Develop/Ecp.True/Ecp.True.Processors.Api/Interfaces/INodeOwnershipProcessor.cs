// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INodeOwnershipProcessor.cs" company="Microsoft">
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
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// The node ownership processor.
    /// </summary>
    public interface INodeOwnershipProcessor : IProcessor
    {
        /// <summary>
        /// Gets the owners for movement.
        /// </summary>
        /// <param name="sourceNodeId">The source node identifier.</param>
        /// <param name="destinationNodeId">The destination node identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The list of owners.</returns>
        Task<IEnumerable<NodeConnectionProductOwner>> GetOwnersForMovementAsync(int sourceNodeId, int destinationNodeId, string productId);

        /// <summary>Reopens the ownership node asynchronous.</summary>
        /// <param name="reopenTicket">The reopen ticket object.</param>
        /// <returns>The task.</returns>
        Task ReopenOwnershipNodeAsync(ReopenTicket reopenTicket);

        /// <summary>
        /// Gets the ownership node identifier asynchronous.
        /// </summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <returns>The ownership nodes query.</returns>
        Task<OwnershipNode> GetOwnershipNodeIdAsync(int ownershipNodeId);

        /// <summary>
        /// Blocks or Unblocks the ownership node asynchronous.
        /// </summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <param name="ownershipNode">The ownership node.</param>
        /// <returns>Returns the status of update.</returns>
        Task UpdateOwnershipNodeStatusAsync(int ownershipNodeId, OwnershipNode ownershipNode);

        /// <summary>
        /// Gets the owners for inventory.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The list of owners.</returns>
        Task<IEnumerable<StorageLocationProductOwner>> GetOwnersForInventoryAsync(int nodeId, string productId);

        /// <summary>
        /// Gets the locked ownership node by editor.
        /// </summary>
        /// <param name="editor">The editor.</param>
        /// <param name="connectionId">The connection identifier.</param>
        /// <returns>
        /// The OwnershipNode.
        /// </returns>
        Task<OwnershipNode> GetLockedOwnershipNodeByEditorAndConnectionIdAsync(string editor, string connectionId);

        /// <summary>
        /// Gets the conditional ownership node by identifier asynchronous.
        /// </summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <returns>The OwnershipNode.</returns>
        Task<OwnershipNode> GetConditionalOwnershipNodeByIdAsync(int ownershipNodeId);

        /// <summary>
        /// Gets the status of refresh progress.
        /// </summary>
        /// <returns>status of the refreshProgress.</returns>
        Task<bool> IsSyncInProgressAsync();

        /// <summary>
        /// Try to refresh rules asynchronous.
        /// </summary>
        /// <returns>The refresh status.</returns>
        Task<StatusType> TryRefreshRulesAsync();

        /// <summary>
        /// Validates the ownership nodes.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The validation result.</returns>
        Task<IEnumerable<OwnershipValidationResult>> ValidateOwnershipNodesAsync(Ticket ticket);

        /// <summary>
        /// Publishes the node ownership.
        /// </summary>
        /// <param name="ownershipUpdates">The ownership updates.</param>
        /// <returns>
        /// Returns the Status of update.
        /// </returns>
        Task PublishNodeOwnershipAsync(PublishedNodeOwnership ownershipUpdates);

        /// <summary>
        /// Gets the ownership node balance summary.
        /// </summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <returns>The list of ownership node balance summary.</returns>
        Task<IEnumerable<OwnershipNodeBalanceSummary>> GetOwnershipNodeBalanceSummaryAsync(int ownershipNodeId);

        /// <summary>
        /// Gets the owner ship node movement inventory details asynchronous.
        /// </summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <returns>The list of Ownership Node Movement Inventory Details.</returns>
        Task<IEnumerable<OwnershipNodeMovementInventoryDetails>> GetOwnerShipNodeMovementInventoryDetailsAsync(int ownershipNodeId);
    }
}
