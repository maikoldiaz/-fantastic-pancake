// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeCostCenterController.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The node cost center controller.
    /// </summary>
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Admin")]
    public class NodeCostCenterController : ODataController
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private readonly INodeCostCenterProcessor processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeCostCenterController"/> class.
        /// </summary>
        /// <param name="processor">The processor.</param>
        public NodeCostCenterController(INodeCostCenterProcessor processor)
        {
            this.processor = processor;
        }

        /// <summary>
        /// Queries the node cost centers asynchronously.
        /// </summary>
        /// <returns>The node cost centers.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("nodecostcenters")]
        [ODataRoute("nodecostcenters")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IQueryable<NodeCostCenter>> QueryNodeCostCentersAsync()
        {
            return await this.processor.QueryAllAsync<NodeCostCenter>(c => !c.IsDeleted).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates the node cost center asynchronous.
        /// </summary>
        /// <param name="nodeCostCenters">The node cost centers.</param>
        /// <returns>Returns the status.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/nodecostcenters")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> CreateNodeCostCenterAsync([FromBody] IEnumerable<NodeCostCenter> nodeCostCenters)
        {
            var results = await this.processor.CreateNodeCostCentersAsync(nodeCostCenters).ConfigureAwait(false);
            if (results.Any(r => r.Status == NodeCostCenterInfo.CreationStatus.Duplicated))
            {
                return new MultiStatusResult(results);
            }

            return new EntityResult(results);
        }

        /// <summary>
        /// Updates the node cost center asynchronous.
        /// </summary>
        /// <param name="nodeCostCenter">The node cost center.</param>
        /// <returns>Returns the status.</returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/nodecostcenters")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> UpdateNodeCostCenterAsync([FromBody] NodeCostCenter nodeCostCenter)
        {
            await this.processor.UpdateNodeCostCenterAsync(nodeCostCenter).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.NodeCosCenterUpdatedSuccessfully);
        }

        /// <summary>
        /// Deletes the node cost center asynchronous.
        /// </summary>
        /// <param name="nodeCostCenterId">The node cost centers.</param>
        /// <returns>Returns the status.</returns>
        [HttpDelete]
        [Route("api/v{version:apiVersion}/nodecostcenters/{nodeCostCenterId:int}")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> DeleteNodeCostCenterAsync([FromRoute] int nodeCostCenterId)
        {
            await this.processor.DeleteNodeCostCenterAsync(nodeCostCenterId).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.NodeCostCenterDeletedSuccessfully);
        }
    }
}
