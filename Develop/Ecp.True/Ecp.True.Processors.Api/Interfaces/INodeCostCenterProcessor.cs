// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INodeCostCenterProcessor.cs" company="Microsoft">
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
    /// The node cost center processor.
    /// </summary>
    public interface INodeCostCenterProcessor : IProcessor
    {
        /// <summary>
        /// Creates the node cost center asynchronously.
        /// </summary>
        /// <param name="nodeCostCenters">The node cost center.</param>
        /// <returns>The task.</returns>
        Task<IEnumerable<NodeCostCenterInfo>> CreateNodeCostCentersAsync(IEnumerable<NodeCostCenter> nodeCostCenters);

        /// <summary>
        /// Updates the node cost center asynchronously.
        /// </summary>
        /// <param name="nodeCostCenter">The node cost center.</param>
        /// <returns>The task.</returns>
        Task<NodeCostCenter> UpdateNodeCostCenterAsync(NodeCostCenter nodeCostCenter);

        /// <summary>
        /// Deletes the node cost center asynchronously.
        /// </summary>
        /// <param name="nodeCostCenterId">The node cost center.</param>
        /// <returns>The task.</returns>
        Task DeleteNodeCostCenterAsync(int nodeCostCenterId);
    }
}