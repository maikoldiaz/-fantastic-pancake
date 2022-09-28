// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeConnectionController.cs" company="Microsoft">
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
    /// The node connection controller.
    /// </summary>
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Admin")]
    public class NodeConnectionController : ODataController
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private readonly INodeConnectionProcessor processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeConnectionController"/> class.
        /// </summary>
        /// <param name="processor">The processor.</param>
        public NodeConnectionController(INodeConnectionProcessor processor)
        {
            this.processor = processor;
        }

        /// <summary>
        /// Creates the node connection asynchronous.
        /// </summary>
        /// <param name="nodeConnection">The node connection.</param>
        /// <returns>Returns the status.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/nodeconnections")]
        [ValidateNodeConnection(true, "nodeConnection")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> CreateNodeConnectionAsync([FromBody] NodeConnection nodeConnection)
        {
            await this.processor.CreateNodeConnectionAsync(nodeConnection).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.NodeConnectionCreatedSuccessfully);
        }

        /// <summary>
        /// Creates multiple node connection asynchronous.
        /// </summary>
        /// <param name="nodeConnections">The node connections.</param>
        /// <returns>Returns the status.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/nodeconnections/list")]
        [ValidateNodeConnectionList]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> CreateNodeConnectionListAsync([FromBody] IEnumerable<NodeConnection> nodeConnections)
        {
            var results = await this.processor.CreateNodeConnectionListAsync(nodeConnections).ConfigureAwait(false);
            return new EntityResult(results);
        }

        /// <summary>
        /// Creates the node connection asynchronous.
        /// </summary>
        /// <param name="nodeConnection">The node connection.</param>
        /// <returns>Return the success message.</returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/nodeconnections")]
        [ValidateNodeConnection(true, "nodeConnection")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> UpdateNodeConnectionAsync([FromBody] NodeConnection nodeConnection)
        {
            await this.processor.UpdateNodeConnectionAsync(nodeConnection).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.NodeConnectionUpdatedSuccessfully);
        }

        /// <summary>
        /// Deletes the node connection by source and destination id.
        /// </summary>
        /// <param name="sourceNodeId">The source node identifier.</param>
        /// <param name="destinationNodeId">The destination node identifier.</param>
        /// <returns>Return the task.</returns>
        [HttpDelete]
        [Route("api/v{version:apiVersion}/nodeconnections/{sourceNodeId}/{destinationNodeId}")]
        [ValidateNodeConnection("sourceNodeId", "destinationNodeId")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> DeleteNodeConnectionAsync(int sourceNodeId, int destinationNodeId)
        {
            await this.processor.DeleteNodeConnectionAsync(sourceNodeId, destinationNodeId).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.NodeConnectionDeletedSuccessfully);
        }

        /// <summary>
        /// Queries the node connections asynchronous.
        /// </summary>
        /// <returns>Return the node connections.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("nodeconnections")]
        [ODataRoute("nodeconnections")]
        [TrueAuthorize]
        public Task<IQueryable<NodeConnection>> QueryNodeConnectionsAsync()
        {
            return this.processor.QueryAllAsync<NodeConnection>(x => !x.IsDeleted);
        }

        /// <summary>
        /// Gets the node connection by source and destination node id.
        /// </summary>
        /// <param name="sourceNodeId">The source node identifier.</param>
        /// <param name="destinationNodeId">The destination node identifier.</param>
        /// <returns>Returns the node connection.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/nodeconnections/{sourceNodeId}/{destinationNodeId}")]
        [ValidateNodeConnection("sourceNodeId", "destinationNodeId")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> GetNodeConnectionByIdAsync(int sourceNodeId, int destinationNodeId)
        {
            var result = await this.processor.GetNodeConnectionAsync(sourceNodeId, destinationNodeId).ConfigureAwait(false);
            return new EntityResult(result, Entities.Constants.NodeConnectionNotFound);
        }

        /// <summary>
        /// Queries the node connection products asynchronous.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <returns>
        /// Return the node connections.
        /// </returns>
        [HttpGet]
        [EnableQuery]
        [Route("nodeconnections/{connectionId}/products")]
        [ODataRoute("nodeconnections/{connectionId}/products")]
        [TrueAuthorize(Role.Administrator)]
        public Task<IQueryable<NodeConnectionProduct>> QueryNodeConnectionProductsAsync(int connectionId)
        {
            return this.processor.QueryAllAsync<NodeConnectionProduct>(x => x.NodeConnectionId == connectionId);
        }

        /// <summary>
        /// Queries the node connection product view asynchronous.
        /// </summary>
        /// <returns>The Node connection product View Data.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("nodeconnectionproductrules")]
        [ODataRoute("nodeconnectionproductrules")]
        [TrueAuthorize(Role.Administrator)]
        public Task<IQueryable<NodeConnectionProductRuleEntity>> QueryNodeConnectionProductRulesViewAsync()
        {
            return this.processor.QueryViewAsync<NodeConnectionProductRuleEntity>();
        }

        /// <summary>
        /// Creates the node connection asynchronous.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns>
        /// Return the success message.
        /// </returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/nodeconnections/products")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> UpdateNodeConnectionProductAsync([FromBody] NodeConnectionProduct product)
        {
            await this.processor.UpdateNodeConnectionProductAsync(product).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.NodeConnectionProductUpdatedSuccessfully);
        }

        /// <summary>
        /// Creates the node connection asynchronous.
        /// </summary>
        /// <param name="productOwners">The owners.</param>
        /// <returns>
        /// Return the success message.
        /// </returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/nodeconnections/products/owners")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> UpdateNodeConnectionProductOwnersAsync([FromBody]UpdateNodeConnectionProductOwners productOwners)
        {
            ArgumentValidators.ThrowIfNull(productOwners, nameof(productOwners));

            await this.processor.SaveNodeConnectionProductOwnersAsync(productOwners).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.NodeConnectionUpdatedSuccessfully);
        }

        /// <summary>
        /// Get the destination node by source node id.
        /// </summary>
        /// <param name="sourceNodeId">The source node identifier.</param>
        /// <returns>Returns the destinations node.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/nodeconnections/{sourceNodeId}/destinationnodes")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> GetDestinationNodesByIdAsync(int sourceNodeId)
        {
            var result = await this.processor.GetDestinationNodesBySourceNodeIdAsync(sourceNodeId).ConfigureAwait(false);
            return new EntityResult(result, Entities.Constants.NodeConnectionNotFound);
        }

        /// <summary>
        /// Gets the node graphical network asynchronous.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns>Graphical network.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/nodeconnections/{nodeId}/elements/{elementId}")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> GetGraphicalNetworkAsync(int elementId, int nodeId)
        {
            var result = await this.processor.GetGraphicalNetworkAsync(elementId, nodeId).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Gets the node graphical network destination nodes details asynchronous.
        /// </summary>
        /// <param name="sourceNodeId">The source nodeid.</param>
        /// <returns>Graphical network.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/nodeconnections/{sourceNodeId}/destinationnodesdetails")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> GetGraphicalNetworkDestinationNodesDetailsByIdAsync(int sourceNodeId)
        {
            var result = await this.processor.GetGraphicalNetworkDestinationNodesDetailsBySourceNodeIdAsync(sourceNodeId).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Gets the node graphical network source nodes details asynchronous.
        /// </summary>
        /// <param name="destinationNodeId">The destination nodeid.</param>
        /// <returns>Graphical network.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/nodeconnections/{destinationNodeId}/sourcenodesdetails")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> GetGraphicalNetworkSourceNodesDetailsByIdAsync(int destinationNodeId)
        {
            var result = await this.processor.GetGraphicalNetworkSourceNodesDetailsByDestinationNodeIdAsync(destinationNodeId).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>Queries active rules asynchronous.</summary>
        /// <returns>Returns the node connection product rules.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/nodeconnections/products/rules")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> GetNodeConnectionProductRulesAsync()
        {
            var result = await this.processor.GetProductRulesAsync().ConfigureAwait(false);
            return new EntityResult(result);
        }
    }
}
