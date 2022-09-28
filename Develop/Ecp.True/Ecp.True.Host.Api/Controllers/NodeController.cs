// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeController.cs" company="Microsoft">
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
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Api.Filter;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The NodeController.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Admin")]
    public class NodeController : ODataController
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private readonly INodeProcessor processor;

        /// <summary>Initializes a new instance of the <see cref="NodeController"/> class.</summary>
        /// <param name="processor">The processor.</param>
        public NodeController(INodeProcessor processor)
        {
            this.processor = processor;
        }

        /// <summary>
        /// Creates a new node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>[True] if the creation is success.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/nodes")]
        [ValidateNodeFilter(false)]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> CreateNodeAsync([FromBody]Node node)
        {
            await this.processor.SaveNodeAsync(node).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.NodeCreateSuccess);
        }

        /// <summary>
        /// Creates a new node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>[True] if the creation is success.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/graphicalNode")]
        [ValidateNodeFilter(false)]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> CreateGraphicalNodeAsync([FromBody]Node node)
        {
            var nodeId = await this.processor.SaveNodeAsync(node).ConfigureAwait(false);
            return new EntityResult(nodeId.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Checks if a node exists with same order.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="segmentId">The segment ID.</param>
        /// <param name="order">The order.</param>
        /// <returns>[True] if the creation is success.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/nodes/{nodeId}/{segmentId}/{order}")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> GetNodeWithSameOrderAsync(int nodeId, int segmentId, int order)
        {
            var node = await this.processor.GetNodeWithSameOrderAsync(nodeId, segmentId, order).ConfigureAwait(false);
            return new EntityResult(node ?? new Node { NodeId = 0 });
        }

        /// <summary>
        /// Updates an existing node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The task.</returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/nodes")]
        [ValidateNodeFilter(true)]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> UpdateNodeAsync([FromBody]Node node)
        {
            await this.processor.UpdateNodeAsync(node).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.NodeUpdateSuccess);
        }

        /// <summary>
        /// Gets all the nodes(both active and inactive).
        /// The method supports ODATA query.
        /// </summary>
        /// <returns>The nodes query.</returns>
        [HttpGet]
        [EnableQuery(MaxExpansionDepth =3)]
        [Route("nodes")]
        [ODataRoute("nodes")]
        [TrueAuthorize]
        public Task<IQueryable<Node>> QueryNodesAsync()
        {
            return this.processor.QueryAllAsync<Node>(null);
        }

        /// <summary>
        /// Gets the node by node id.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns>The nodes.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/nodes/{nodeId}")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> GetNodeByIdAsync(int nodeId)
        {
            var result = await this.processor.GetNodeByIdAsync(nodeId).ConfigureAwait(false);
            return new EntityResult(result, Entities.Constants.NodeDoesNotExists);
        }

        /// <summary>
        /// Determines whether the name exists with the same name.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns>
        /// [True] if the Node exists with the same name [False] otherwise.
        /// </returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/nodes/{nodeName}/exists")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> ExistsNodeAsync(string nodeName)
        {
            ArgumentValidators.ThrowIfNull(nodeName, nameof(nodeName));

            var entity = await this.processor.GetNodeByNameAsync(nodeName).ConfigureAwait(false);
            return new EntityExistsResult(nameof(Node), entity);
        }

        /// <summary>
        /// Determines whether the storage location in a node exists with the same name.
        /// </summary>
        /// <param name="nodeStorageLocationName">Name of the node storage location.</param>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns>
        /// [True] if the Node storage Location exists with the same name [False] otherwise.
        /// </returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/nodes/{nodeId}/storagelocations/{nodeStorageLocationName}/exists")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> ExistsNodeStorageLocationAsync(string nodeStorageLocationName, int nodeId)
        {
            ArgumentValidators.ThrowIfNull(nodeStorageLocationName, nameof(nodeStorageLocationName));

            var entity = await this.processor.GetStorageLocationByNameAsync(nodeStorageLocationName, nodeId).ConfigureAwait(false);
            return new EntityExistsResult(nameof(NodeStorageLocation), entity);
        }

        /// <summary>
        /// Gets all storage locations in a node(both active and inactive).
        /// This method supports ODATA query.
        /// </summary>
        /// <param name="nodeId">The key.</param>
        /// <returns>
        /// The nodes query.
        /// </returns>
        [HttpGet]
        [EnableQuery]
        [Route("nodes/{nodeId}/nodestoragelocations")]
        [ODataRoute("nodes/{nodeId}/nodestoragelocations")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public Task<IQueryable<NodeStorageLocation>> QueryNodeStorageLocationsByNodeIdAsync([FromODataUri] int nodeId)
        {
           return this.processor.QueryAllAsync<NodeStorageLocation>(n => n.NodeId == nodeId);
        }

        /// <summary>
        /// Gets all storage locations products.
        /// This method supports ODATA query.
        /// </summary>
        /// <returns>
        /// The nodes query.
        /// </returns>
        [HttpGet]
        [EnableQuery]
        [Route("storagelocationproducts")]
        [ODataRoute("storagelocationproducts")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public Task<IQueryable<StorageLocationProduct>> QueryNodeStorageLocationProductsAsync()
        {
            return this.processor.QueryAllAsync<StorageLocationProduct>(null);
        }

        /// <summary>
        /// Gets the node type asynchronous.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns>Node type.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/nodes/type/{nodeId}")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> GetNodeTypeAsync(int nodeId)
        {
            var result = await this.processor.GetNodeTypeAsync(nodeId).ConfigureAwait(false);
            return new EntityResult(result, Entities.Constants.NodeTypeNotFound);
        }

        /// <summary>
        /// Updates the Node Properties.
        /// </summary>
        /// <param name="node">The Node.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/nodes/attributes")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> UpdateNodePropertiesAsync([FromBody]Node node)
        {
            await this.processor.UpdateNodePropertiesAsync(node).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.NodeUpdateSuccess);
        }

        /// <summary>Updates the storage location product asynchronous.</summary>
        /// <param name="storageLocationProduct">The Storage Location Product.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/nodes/locations/products")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> UpdateStorageLocationProductAsync([FromBody]StorageLocationProduct storageLocationProduct)
        {
            await this.processor.UpdateStorageLocationProductAsync(storageLocationProduct).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.NodeStorageLocationProductUpdatedSuccessfully);
        }

        /// <summary>
        /// Updates Storage Location Product Owners Async.
        /// </summary>
        /// <param name="productOwners">The product owners.</param>
        /// <returns>
        /// A <see cref="Task{TResult}" /> representing the result of the asynchronous operation.
        /// </returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/nodes/locations/products/owners")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> UpdateStorageLocationProductOwnersAsync([FromBody]UpdateStorageLocationProductOwners productOwners)
        {
            ArgumentValidators.ThrowIfNull(productOwners, nameof(productOwners));

            await this.processor.UpdateStorageLocationProductOwnersAsync(productOwners).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.NodeStorageLocationProductOwnersUpdatedSuccessfully);
        }

        /// <summary>
        /// Filters the nodes asynchronous.
        /// </summary>
        /// <param name="nodesFilter">The nodes filter.</param>
        /// <returns>The collection of nodes.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/nodes/filter")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> FilterNodesAsync(NodesFilter nodesFilter)
        {
            var result = await this.processor.FilterNodesAsync(nodesFilter).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Get the nodes with associated ownership rules.
        /// </summary>
        /// <returns>The collection of nodes with the associated ownership rules.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("nodeownershiprules")]
        [ODataRoute("nodeownershiprules")]
        [TrueAuthorize(Role.Administrator)]
        public Task<IQueryable<NodeRuleEntity>> QueryNodesWithOwnershipRulesAsync()
        {
            return this.processor.QueryViewAsync<NodeRuleEntity>();
        }

        /// <summary>
        /// Gets the node product rules asynchronous.
        /// </summary>
        /// <returns>The Node Product rules.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("nodeproductrules")]
        [ODataRoute("nodeproductrules")]
        [TrueAuthorize(Role.Administrator)]
        public Task<IQueryable<NodeProductRuleEntity>> QueryNodeProductRulesAsync()
        {
            return this.processor.QueryViewAsync<NodeProductRuleEntity>();
        }

        /// <summary>
        /// Updates the nodes with the provided ownership rule.
        /// </summary>
        /// <param name="nodeOwnershipRuleBulkUpdateRequest">The node ownership rule bulk update request.</param>
        /// <returns>The result of update.</returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/nodes/rules")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> UpdateNodeOwnershipRulesAsync(OwnershipRuleBulkUpdateRequest nodeOwnershipRuleBulkUpdateRequest)
        {
            await this.processor.UpdateNodeOwnershipRulesAsync(nodeOwnershipRuleBulkUpdateRequest).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.OwnershipRuleBulkUpdateSuccess);
        }

        /// <summary>
        /// Gets the logistic center name asynchronous.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns>The logistic center id.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/nodes/logisticcenter/{nodeName}")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> GetLogisticCenterNameAsync(string nodeName)
        {
            var node = await this.processor.GetNodeByNameAsync(nodeName).ConfigureAwait(false);
            return new EntityResult(node, Entities.Constants.LogisticCenterNameNotFound);
        }

        /// <summary>
        /// Gets the node ownership rules asynchronous.
        /// </summary>
        /// <returns>The result.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/nodes/rules")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> GetNodeOwnershipRulesAsync()
        {
            var result = await this.processor.GetNodeOwnershipRulesAsync().ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Gets the node product rules asynchronous.
        /// </summary>
        /// <returns>The result.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/nodes/products/rules")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> GetNodeProductRulesAsync()
        {
            var result = await this.processor.GetNodeProductRulesAsync().ConfigureAwait(false);
            return new EntityResult(result);
        }
    }
}