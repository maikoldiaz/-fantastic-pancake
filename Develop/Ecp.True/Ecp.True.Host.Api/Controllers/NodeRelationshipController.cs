// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeRelationshipController.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Analytics;
    using Ecp.True.Entities.Core;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The node configuration controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNet.OData.ODataController" />
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Admin")]
    public class NodeRelationshipController : ODataController
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private readonly INodeRelationshipProcessor processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeRelationshipController"/> class.
        /// </summary>
        /// <param name="processor">The processor.</param>
        public NodeRelationshipController(INodeRelationshipProcessor processor)
        {
            this.processor = processor;
        }

        /// <summary>
        /// Queries the node relationships asynchronously.
        /// </summary>
        /// <returns>Node relationship collection.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("operatives")]
        [ODataRoute("operatives")]
        [TrueAuthorize]
        public Task<IQueryable<OperativeNodeRelationship>> QueryNodeRelationshipsAsync()
        {
            return this.processor.QueryAllAsync<OperativeNodeRelationship>(x => !x.IsDeleted.Value);
        }

        /// <summary>
        /// Gets the node relationship asynchronously.
        /// </summary>
        /// <param name="nodeRelationshipId">The node relationship identifier.</param>
        /// <returns>
        /// Node relationship details.
        /// </returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/nodes/relationships/{nodeRelationshipId}")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> GetNodeRelationshipAsync(int nodeRelationshipId)
        {
            var result = await this.processor.GetNodeRelationshipAsync(nodeRelationshipId).ConfigureAwait(false);
            return new EntityResult(result, Entities.Constants.NodeRelationshipNotFound);
        }

        /// <summary>
        /// Creates the node relationship asynchronously.
        /// </summary>
        /// <param name="nodeRelationship">The node relationship.</param>
        /// <returns> Success/Failure message.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/nodes/relationships/operatives")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> CreateNodeRelationshipAsync([FromBody] OperativeNodeRelationship nodeRelationship)
        {
            ArgumentValidators.ThrowIfNull(nodeRelationship, nameof(nodeRelationship));
            await this.processor.CreateNodeRelationshipAsync(nodeRelationship).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.NodeRelationshipCreatedSuccessfully);
        }

        /// <summary>
        /// Creates the logistic transfer relationship asynchronous.
        /// </summary>
        /// <param name="nodeRelationship">The node relationship.</param>
        /// <returns>The Success/Failure message.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/nodes/relationships/operative/exists")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> ExistsOperativeTransferRelationshipAsync([FromBody] OperativeNodeRelationship nodeRelationship)
        {
            ArgumentValidators.ThrowIfNull(nodeRelationship, nameof(nodeRelationship));

            var relationship = await this.processor.OperativeTransferRelationshipExistsAsync(nodeRelationship).ConfigureAwait(false);
            return new EntityExistsResult(nameof(OperativeNodeRelationship), relationship);
        }

        /// <summary>
        /// Updates the node relationship asynchronously.
        /// </summary>
        /// <param name="nodeRelationship">The node relationship.</param>
        /// <returns>Success/Failure message.</returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/nodes/relationships/operatives")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> UpdateNodeRelationshipAsync([FromBody] OperativeNodeRelationship nodeRelationship)
        {
            ArgumentValidators.ThrowIfNull(nodeRelationship, nameof(nodeRelationship));
            await this.processor.UpdateNodeRelationshipAsync(nodeRelationship).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.NodeRelationshipUpdatedSuccessfully);
        }

        /// <summary>
        /// Deletes the node relationship asynchronously.
        /// </summary>
        /// <param name="nodeRelationship">The node relationship information.</param>
        /// <returns>
        /// Success/Failure message.
        /// </returns>
        [HttpDelete]
        [Route("api/v{version:apiVersion}/nodes/relationships/operatives")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> DeleteNodeRelationshipAsync([FromBody] OperativeNodeRelationship nodeRelationship)
        {
            await this.processor.UpdateNodeRelationshipAsync(nodeRelationship).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.NodeRelationshipDeletedSuccessfully);
        }

        /// <summary>
        /// Queries the node relationships asynchronously.
        /// </summary>
        /// <returns>Node relationship collection.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("logistics")]
        [ODataRoute("logistics")]
        [TrueAuthorize(Role.Administrator)]
        public Task<IQueryable<OperativeNodeRelationshipWithOwnership>> QueryLogisticTransferRelationshipAsync()
        {
            return this.processor.QueryAllAsync<OperativeNodeRelationshipWithOwnership>(x => !x.IsDeleted);
        }

        /// <summary>
        /// Creates the logistic transfer relationship asynchronous.
        /// </summary>
        /// <param name="nodeRelationship">The node relationship.</param>
        /// <returns>The Success/Failure message.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/nodes/relationships/logistics")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> CreateLogisticTransferRelationshipAsync([FromBody] OperativeNodeRelationshipWithOwnership nodeRelationship)
        {
            ArgumentValidators.ThrowIfNull(nodeRelationship, nameof(nodeRelationship));
            await this.processor.CreateLogisticTransferRelationshipAsync(nodeRelationship).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.NodeRelationshipCreatedSuccessfully);
        }

        /// <summary>
        /// Deletes the logistic transfer relationship asynchronous.
        /// </summary>
        /// <param name="nodeRelationship">The node relationship.</param>
        /// <returns>The Success/Failure message.</returns>
        [HttpDelete]
        [Route("api/v{version:apiVersion}/nodes/relationships/logistics")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> DeleteLogisticTransferRelationshipAsync([FromBody] OperativeNodeRelationshipWithOwnership nodeRelationship)
        {
            ArgumentValidators.ThrowIfNull(nodeRelationship, nameof(nodeRelationship));
            await this.processor.DeleteLogisticTransferRelationshipAsync(nodeRelationship).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.NodeRelationshipDeletedSuccessfully);
        }

        /// <summary>
        /// Creates the logistic transfer relationship asynchronous.
        /// </summary>
        /// <param name="nodeRelationship">The node relationship.</param>
        /// <returns>The Success/Failure message.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/nodes/relationships/logistics/exists")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> ExistsLogisticTransferRelationshipAsync([FromBody] OperativeNodeRelationshipWithOwnership nodeRelationship)
        {
            ArgumentValidators.ThrowIfNull(nodeRelationship, nameof(nodeRelationship));

            var relationship = await this.processor.LogisticTransferRelationshipExistsAsync(nodeRelationship).ConfigureAwait(false);
            return new EntityExistsResult(nameof(OperativeNodeRelationshipWithOwnership), relationship);
        }
    }
}