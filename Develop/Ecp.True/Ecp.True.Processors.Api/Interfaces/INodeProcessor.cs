// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INodeProcessor.cs" company="Microsoft">
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
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// The Node Processor Interface.
    /// </summary>
    public interface INodeProcessor : IProcessor
    {
        /// <summary>
        /// Saves the node asynchronous.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        Task<int> SaveNodeAsync(Node node);

        /// <summary>Updates the node asynchronous.</summary>
        /// <param name="node">The node.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        Task UpdateNodeAsync(Node node);

        /// <summary>
        /// Gets the node by name asynchronous.
        /// </summary>
        /// <param name="nodeName">The node name.</param>
        /// <returns>
        /// Returns [true] is the name does not exists otherwise [false].
        /// </returns>
        Task<Node> GetNodeByNameAsync(string nodeName);

        /// <summary>
        /// Gets the node by identifier asynchronous.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns>
        /// The node.
        /// </returns>
        Task<Node> GetNodeByIdAsync(int nodeId);

        /// <summary>
        /// Gets the storage location by name asynchronous.
        /// </summary>
        /// <param name="storageName">Name of the storage.</param>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns>
        /// Returns [true] is the name does not exists otherwise [false].
        /// </returns>
        Task<NodeStorageLocation> GetStorageLocationByNameAsync(string storageName, int nodeId);

        /// <summary>Updates the node properties asynchronous.</summary>
        /// <param name="node">The node.</param>
        /// <returns>Returns the status of update.</returns>
        Task UpdateNodePropertiesAsync(Node node);

        /// <summary>Updates the storage location product asynchronous.</summary>
        /// <param name="storageLocationProduct">The Storage Location Product.</param>
        /// <returns>Returns the status of update.</returns>
        Task UpdateStorageLocationProductAsync(StorageLocationProduct storageLocationProduct);

        /// <summary>
        /// Updates the storage location product owners asynchronous.
        /// </summary>
        /// <param name="productOwners">The product owners.</param>
        /// <returns>
        /// Returns the status of update.
        /// </returns>
        Task UpdateStorageLocationProductOwnersAsync(UpdateStorageLocationProductOwners productOwners);

        /// <summary>
        /// Filters the nodes asynchronous.
        /// </summary>
        /// <param name="nodesFilter">The nodes filter.</param>
        /// <returns>The collection of nodes.</returns>
        Task<IEnumerable<Node>> FilterNodesAsync(NodesFilter nodesFilter);

        /// <summary>
        /// Gets the node with same order asynchronous.
        /// </summary>
        /// <param name="nodeId">The node Identifier.</param>
        /// <param name="segmentId">The segment identifier.</param>
        /// <param name="order">The order.</param>
        /// <returns>The node.</returns>
        Task<Node> GetNodeWithSameOrderAsync(int nodeId, int segmentId, int order);

        /// <summary>
        /// Gets the node type asynchronous.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns>Node type.</returns>
        Task<CategoryElement> GetNodeTypeAsync(int nodeId);

        /// <summary>
        /// Updates the nodes with the ownership rule provided.
        /// </summary>
        /// <param name="request">The node ownership rule bulk update request.</param>
        /// <returns>The task.</returns>
        Task UpdateNodeOwnershipRulesAsync(OwnershipRuleBulkUpdateRequest request);

        /// <summary>
        /// Gets the node ownership rules asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        Task<IEnumerable<NodeOwnershipRule>> GetNodeOwnershipRulesAsync();

        /// <summary>
        /// Get node product rules.
        /// </summary>
        /// <typeparam name="NodeProductRule">The NodeProductRule type.</typeparam>
        /// <returns>The rules.</returns>
        Task<IEnumerable<NodeProductRule>> GetNodeProductRulesAsync();
    }
}