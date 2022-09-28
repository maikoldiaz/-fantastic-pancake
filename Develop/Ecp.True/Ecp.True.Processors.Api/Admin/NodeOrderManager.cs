// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeOrderManager.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Admin
{
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Processors.Api.Interfaces;

    /// <summary>
    ///     The Node Processor.
    /// </summary>
    public class NodeOrderManager : INodeOrderManager
    {
        /// <summary>
        /// The repository factory.
        /// </summary>
        private readonly IRepositoryFactory repositoryFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeOrderManager" /> class.
        /// </summary>
        /// <param name="repositoryFactory">The repository factory.</param>
        public NodeOrderManager(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        /// <summary>
        /// Tries the reorder asynchronous.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="nodeRepository">The node repository.</param>
        /// <returns>The task.</returns>
        public Task TryReorderAsync(Node node, IRepository<Node> nodeRepository)
        {
            ArgumentValidators.ThrowIfNull(node, nameof(node));
            if (!node.AutoOrder)
            {
                return Task.CompletedTask;
            }

            return this.ReorderNodesAsync(node.SegmentId.Value, node.Order.Value, nodeRepository);
        }

        private async Task ReorderNodesAsync(int segmentId, int order, IRepository<Node> nodeRepository)
        {
            ArgumentValidators.ThrowIfNull(nodeRepository, nameof(nodeRepository));
            var nodes = await this.repositoryFactory.NodeRepository.GetNodesWithSameOrHigherOrderAsync(segmentId, order).ConfigureAwait(false);
            nodes.ForEach(x =>
            {
                x.IncrementOrder();
                nodeRepository.Update(x);
            });
        }
    }
}