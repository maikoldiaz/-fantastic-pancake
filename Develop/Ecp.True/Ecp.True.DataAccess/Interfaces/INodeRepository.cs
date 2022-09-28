// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INodeRepository.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;

    /// <summary>
    /// Node Repository Interface.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Interfaces.IRepository{Ecp.True.Entities.Admin.Node}" />
    public interface INodeRepository : IRepository<Node>
    {
        /// <summary>
        /// Filters the nodes asynchronous.
        /// </summary>
        /// <param name="nodesFilter">The nodes filter.</param>
        /// <returns>The collection of nodes.</returns>
        IQueryable<Node> FilterNodes(NodesFilter nodesFilter);

        /// <summary>
        /// Gets the node with same order asynchronous.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="segmentId">The segment identifier.</param>
        /// <param name="order">The order.</param>
        /// <returns>The node.</returns>
        Task<Node> GetNodeWithSameOrderAsync(int nodeId, int segmentId, int order);

        /// <summary>
        /// Validates the order.
        /// </summary>
        /// <param name="segmentId">The segment ID.</param>
        /// <param name="order">The order.</param>
        /// <returns>Whether the order is valid.</returns>
        Task<IEnumerable<Node>> GetNodesWithSameOrHigherOrderAsync(int segmentId, int order);

        /// <summary>
        /// Get Segment Detail for Node.
        /// </summary>
        /// <param name="nodeId">The Node Id.</param>
        /// <returns>The segment detail.</returns>
        Task<CategoryElement> GetSegmentDetailForNodeAsync(int nodeId);

        /// <summary>
        /// Gets the node type for node asynchronous.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns>The node type.</returns>
        Task<CategoryElement> GetNodeTypeForNodeAsync(int nodeId);
    }
}
