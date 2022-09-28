// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipController.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Flow.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Host.Flow.Api.Filter;
    using Ecp.True.Processors.Approval.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The category controller.
    /// </summary>
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Flow")]
    [Authorize(Roles = HostConstants.FlowRoleClaimType)]
    public class OwnershipController : ControllerBase
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private readonly IApprovalProcessor approvalProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipController" /> class.
        /// </summary>
        /// <param name="processor">The processor.</param>
        public OwnershipController(IApprovalProcessor processor)
        {
            this.approvalProcessor = processor;
        }

        /// <summary>
        /// Changes the ownership node status Async.
        /// </summary>
        /// <param name="approvalRequest">The approval request.</param>
        /// <returns>The Task.</returns>
        [HttpPost]
        [ValidateOwnershipNode]
        [Route("api/v{version:apiVersion}/nodes/ownership")]
        public async Task<IActionResult> UpdateNodeOwnershipAsync([FromBody] NodeOwnershipApprovalRequest approvalRequest)
        {
            await this.approvalProcessor.UpdateOwnershipNodeStateAsync(approvalRequest).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.OwnershipNodeUpdatedSuccessfully);
        }

        /// <summary>
        /// Gets the node ownership data asynchronous.
        /// </summary>
        /// <param name="nodeOwnershipId">The ownership node identifier.</param>
        /// <returns>The Result.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/nodes/{nodeOwnershipId}/ownership")]
        public async Task<IActionResult> GetNodeOwnershipByIdAsync(int nodeOwnershipId)
        {
            var nodeBalanceDetails = await this.approvalProcessor.GetOwnershipNodeBalanceDetailsAsync(nodeOwnershipId).ConfigureAwait(false);
            return new EntityResult(nodeBalanceDetails);
        }
    }
}