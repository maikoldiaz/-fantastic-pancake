// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeOwnershipController.cs" company="Microsoft">
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
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Approval.Interfaces;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The node ownership controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Transport Balance")]
    public class NodeOwnershipController : ODataController
    {
        /// <summary>
        /// The node ownership processor.
        /// </summary>
        private readonly INodeOwnershipProcessor nodeOwnershipProcessor;

        /// <summary>
        /// The approval processor.
        /// </summary>
        private readonly IApprovalProcessor approvalProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeOwnershipController"/> class.
        /// </summary>
        /// <param name="nodeOwnershipProcessor">The node ownership processor.</param>
        /// <param name="approvalProcessor">The approval processor.</param>
        public NodeOwnershipController(INodeOwnershipProcessor nodeOwnershipProcessor, IApprovalProcessor approvalProcessor)
        {
            this.nodeOwnershipProcessor = nodeOwnershipProcessor;
            this.approvalProcessor = approvalProcessor;
        }

        /// <summary>
        /// Gets the owners for movement asynchronous.
        /// </summary>
        /// <param name="sourceNodeId">The source node identifier.</param>
        /// <param name="destinationNodeId">The destination node identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The list of owners.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/movements/{sourceNodeId}/{destinationNodeId}/products/{productId}/ownership")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Programmer)]
        public async Task<IActionResult> GetOwnersForMovementAsync(int sourceNodeId, int destinationNodeId, string productId)
        {
            var result = await this.nodeOwnershipProcessor.GetOwnersForMovementAsync(sourceNodeId, destinationNodeId, productId).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>Reopens the ownership node asynchronous.</summary>
        /// <param name="reopenTicket">the reopen ticket object.</param>
        /// <returns>Ownership Node reopened.</returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/reopenticket")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Programmer)]
        public async Task<IActionResult> ReopenOwnershipNodeAsync([FromBody] ReopenTicket reopenTicket)
        {
            await this.nodeOwnershipProcessor.ReopenOwnershipNodeAsync(reopenTicket).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.OwnershipNodeReopenedSuccessfully);
        }

        /// <summary>
        /// Gets the owners for inventory asynchronous.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The list of owners.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/nodeownership/{nodeId}/{productId}")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Programmer)]
        public async Task<IActionResult> GetOwnersForInventoryAsync(int nodeId, string productId)
        {
            var result = await this.nodeOwnershipProcessor.GetOwnersForInventoryAsync(nodeId, productId).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Submit ownershipnode data for approval.
        /// </summary>
        /// <param name="ownershipNodeId">The ownershipnode identifier.</param>
        /// <returns>The list of owners.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/nodeownership/submitforapproval/{ownershipNodeId}")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Programmer)]
        public async Task<IActionResult> SendOwnershipNodeForApprovalAsync(int ownershipNodeId)
        {
            await this.approvalProcessor.SendOwnershipNodeIdForApprovalAsync(ownershipNodeId).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.OwnershipNodeForApprovalSentSuccessfully);
        }

        /// <summary>
        /// Queries the ownership node by identifier asynchronous.
        /// </summary>
        /// <param name="ownershipNodeId">The ownershipnode identifier.</param>
        /// <returns>The Ownership Node.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/nodeownership/{ownershipNodeId}")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Query, Role.Programmer)]
        public async Task<IActionResult> GetConditionalOwnershipNodeByIdAsync(int ownershipNodeId)
        {
            var result = await this.nodeOwnershipProcessor.GetConditionalOwnershipNodeByIdAsync(ownershipNodeId).ConfigureAwait(false);
            return new EntityResult(result, Entities.Constants.EntityNotExists);
        }

        /// <summary>Gets the progress of bulk update asynchronous.</summary>
        /// <returns>[True] if there is an active in progress record.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/strategies/progress")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> GetRulesSyncProgressAsync()
        {
            var result = await this.nodeOwnershipProcessor.IsSyncInProgressAsync().ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>Gets the validate in progress record asynchronous.</summary>
        /// <returns>validates that the refresh is ongoing then throws error.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/strategies/refresh")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> UpdateRuleSyncAsync()
        {
            var result = await this.nodeOwnershipProcessor.TryRefreshRulesAsync().ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>Queries the contracts asynchronous.
        /// </summary>
        /// <returns>returns the contracts.
        /// </returns>
        [HttpGet]
        [EnableQuery]
        [Route("contracts")]
        [ODataRoute("contracts")]
        [TrueAuthorize]
        public Task<IQueryable<Contract>> QueryContractsAsync()
        {
            return this.nodeOwnershipProcessor.QueryAllAsync<Contract>(null);
        }

        /// <summary>
        /// Validates the ownership nodes.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The validation result.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/ownership/nodes/exists")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Programmer)]
        public async Task<IActionResult> ValidateOwnershipNodesAsync([FromBody]Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var result = await this.nodeOwnershipProcessor.ValidateOwnershipNodesAsync(ticket).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Queries the ownership node asynchronous.
        /// </summary>
        /// <returns>The Ownership Nodes.</returns>
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 3)]
        [Route("ownershipnodes")]
        [ODataRoute("ownershipnodes")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Query, Role.Programmer)]
        public Task<IQueryable<OwnershipNode>> QueryOwnershipNodeAsync()
        {
            return this.nodeOwnershipProcessor.QueryAllAsync<OwnershipNode>(null);
        }

        /// <summary>
        /// Publishes the node ownership.
        /// </summary>
        /// <param name="ownershipUpdates">The ownership updates.</param>
        /// <returns>The Task.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/ownershipnodes/publish")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Programmer)]
        public async Task<IActionResult> PublishNodeOwnershipAsync([FromBody] PublishedNodeOwnership ownershipUpdates)
        {
            await this.nodeOwnershipProcessor.PublishNodeOwnershipAsync(ownershipUpdates).ConfigureAwait(false);

            return new EntityResult(Entities.Constants.NodeOwnershipPublishSuccess);
        }

        /// <summary>
        /// Queries the ownership node view asynchronous.
        /// </summary>
        /// <returns>The Ownership Node View Data.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("viewownershipnodes")]
        [ODataRoute("viewownershipnodes")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Query, Role.Programmer)]
        public Task<IQueryable<OwnershipNodeData>> QueryOwnershipNodeViewAsync()
        {
            return this.nodeOwnershipProcessor.QueryViewAsync<OwnershipNodeData>();
        }

        /// <summary>
        /// Gets the ownership node errors asynchronous.
        /// </summary>
        /// <returns>Ownership Calculation Node Errors.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("ownershiperrors")]
        [ODataRoute("ownershiperrors")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Programmer)]
        public Task<IQueryable<OwnershipError>> QueryOwnershipNodeErrorsAsync()
        {
            return this.nodeOwnershipProcessor.QueryViewAsync<OwnershipError>();
        }

        /// <summary>
        /// Gets the ownership node balance summary.
        /// </summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <returns>The Entity Result.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/ownershipnodes/{ownershipNodeId}/balance")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Programmer)]
        public async Task<IActionResult> GetOwnershipNodeBalanceSummaryAsync(int ownershipNodeId)
        {
            var result = await this.nodeOwnershipProcessor.GetOwnershipNodeBalanceSummaryAsync(ownershipNodeId).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Gets the ownership node movement inventory details asynchronous.
        /// </summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <returns>The Entity Result.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/ownershipnodes/{ownershipNodeId}/details")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Programmer)]
        public async Task<IActionResult> GetOwnershipNodeMovementInventoryDetailsAsync(int ownershipNodeId)
        {
            var result = await this.nodeOwnershipProcessor.GetOwnerShipNodeMovementInventoryDetailsAsync(ownershipNodeId).ConfigureAwait(false);
            return new EntityResult(result);
        }
    }
}