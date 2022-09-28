// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaController.cs" company="Microsoft">
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
    using Ecp.True.Processors.Approval.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The Delta controller.
    /// </summary>
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Flow")]
    [Authorize(Roles = HostConstants.FlowRoleClaimType)]
    public class DeltaController : ControllerBase
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private readonly IDeltaProcessor deltaProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaController" /> class.
        /// </summary>
        /// <param name="processor">The processor.</param>
        public DeltaController(IDeltaProcessor processor)
        {
            this.deltaProcessor = processor;
        }

        /// <summary>
        /// Gets Delta By Delta NodeId asynchronous.
        /// </summary>
        /// <param name="deltaNodeId">The delta node identifier.</param>
        /// <returns>The Result.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/nodes/{deltaNodeId}/delta")]
        public async Task<IActionResult> GetDeltaByDeltaNodeIdAsync(int deltaNodeId)
        {
            var deltaDetails = await this.deltaProcessor.GetDeltaByDeltaNodeIdAsync(deltaNodeId).ConfigureAwait(false);
            return new EntityResult(deltaDetails);
        }

        /// <summary>
        /// Changes the Delta Approval state Async.
        /// </summary>
        /// <param name="approvalRequest">The approval request.</param>
        /// <returns>The Task.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/nodes/delta")]
        public async Task<IActionResult> UpdateDeltaApprovalStateAsync([FromBody] DeltaNodeApprovalRequest approvalRequest)
        {
            await this.deltaProcessor.UpdateDeltaApprovalStateAsync(approvalRequest).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.DeltaNodeUpdatedSuccessfully);
        }

        /// <summary>
        /// Generate Delta Movements Asynchronous.
        /// </summary>
        /// <param name="deltaNodeId">The delta node identifier.</param>
        /// <returns>The Result.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/nodes/{deltaNodeId}/delta/movements")]
        public async Task<IActionResult> GenerateDeltaMovementsAsync(int deltaNodeId)
        {
            await this.deltaProcessor.GenerateDeltaMovementsAsync(deltaNodeId).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.DeltaNodeUpdatedSuccessfully);
        }
    }
}
