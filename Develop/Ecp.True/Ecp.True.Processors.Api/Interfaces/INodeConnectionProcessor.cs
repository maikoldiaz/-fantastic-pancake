// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INodeConnectionProcessor.cs" company="Microsoft">
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
    using QueryEntity = Ecp.True.Entities.Query;

    /// <summary>
    /// The node connection processor.
    /// </summary>
    public interface INodeConnectionProcessor : IProcessor
    {
        /// <summary>
        /// Gets the node connection asynchronous.
        /// </summary>
        /// <param name="sourceNodeId">The source node identifier.</param>
        /// <param name="destinationNodeId">The destination node identifier.</param>
        /// <returns>
        /// Return node.
        /// </returns>
        Task<NodeConnection> GetNodeConnectionAsync(int sourceNodeId, int destinationNodeId);

        /// <summary>
        /// Creates the node connection asynchronous.
        /// </summary>
        /// <param name="nodeConnection">The node connection.</param>
        /// <returns>Return the task.</returns>
        Task CreateNodeConnectionAsync(NodeConnection nodeConnection);

        /// <summary>
        /// Creates a list of node connections asynchronous.
        /// </summary>
        /// <param name="nodeConnectionList">The node connection.</param>
        /// <returns>Return the task.</returns>
        Task<IEnumerable<NodeConnectionInfo>> CreateNodeConnectionListAsync(IEnumerable<NodeConnection> nodeConnectionList);

        /// <summary>
        /// Updates the node connection asynchronous.
        /// </summary>
        /// <param name="nodeConnection">The node connection.</param>
        /// <returns>
        /// Return the task.
        /// </returns>
        Task UpdateNodeConnectionAsync(NodeConnection nodeConnection);

        /// <summary>
        /// Deletes the node connection asynchronous.
        /// </summary>
        /// <param name="sourceNodeId">The source node identifier.</param>
        /// <param name="destinationNodeId">The destination node identifier.</param>
        /// <returns>
        /// Return the delete node connection.
        /// </returns>
        Task DeleteNodeConnectionAsync(int sourceNodeId, int destinationNodeId);

        /// <summary>
        /// Updates the node connection product asynchronous.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns>The task.</returns>
        Task UpdateNodeConnectionProductAsync(NodeConnectionProduct product);

        /// <summary>
        /// Saves the node connection product owners asynchronous.
        /// </summary>
        /// <param name="productOwners">The product owners.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task SaveNodeConnectionProductOwnersAsync(UpdateNodeConnectionProductOwners productOwners);

        /// <summary>
        /// Gets all the destination node by source node id.
        /// </summary>
        /// <param name="sourceNodeId">The source node identifier.</param>
        /// <returns>Returns the destinations node.</returns>
        Task<IEnumerable<Node>> GetDestinationNodesBySourceNodeIdAsync(int sourceNodeId);

        /// <summary>
        /// Gets the node type asynchronous.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns>Graphical network.</returns>
        Task<QueryEntity.GraphicalNetwork> GetGraphicalNetworkAsync(int elementId, int nodeId);

        /// <summary>
        /// Gets the graphical network destination nodes details by source node identifier asynchronous.
        /// </summary>
        /// <param name="sourceNodeId">The source node identifier.</param>
        /// <returns>Graphical network destination nodes details.</returns>
        Task<QueryEntity.GraphicalNetwork> GetGraphicalNetworkDestinationNodesDetailsBySourceNodeIdAsync(int sourceNodeId);

        /// <summary>
        /// Gets the graphical network source nodes details by destination node identifier asynchronous.
        /// </summary>
        /// <param name="destinationNodeId">The destination node identifier.</param>
        /// <returns>Graphical network source nodes details.</returns>
        Task<QueryEntity.GraphicalNetwork> GetGraphicalNetworkSourceNodesDetailsByDestinationNodeIdAsync(int destinationNodeId);

        /// <summary>
        /// Get node connection product rules.
        /// </summary>
        /// <returns>
        /// The rules.
        /// </returns>
        Task<IEnumerable<NodeConnectionProductRule>> GetProductRulesAsync();
    }
}
