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

namespace Ecp.True.Host.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Delta.Interfaces;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The delta controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Admin")]
    public class DeltaController : ODataController
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private readonly IDeltaProcessor processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaController"/> class.
        /// </summary>
        /// <param name="processor">The processor.</param>
        public DeltaController(IDeltaProcessor processor)
        {
            this.processor = processor;
        }

        /// <summary>
        /// Gets all the delta nodes.
        /// This method supports ODATA query.
        /// </summary>
        /// <returns>The delta nodes response.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("deltanodes")]
        [ODataRoute("deltanodes")]
        [TrueAuthorize(Role.Administrator, Role.Query, Role.Chain)]
        public Task<IQueryable<DeltaNodeInfo>> QueryDeltaNodesAsync()
        {
            return this.processor.QueryViewAsync<DeltaNodeInfo>();
        }

        /// <summary>
        /// Gets the official delta period.
        /// </summary>
        /// <param name="segmentId">The segment identifier.</param>
        /// <param name="years">The years.</param>
        /// <param name="isPerNodeReport">if set to <c>true</c> [is per node report].</param>
        /// <returns>
        /// The period info.
        /// </returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/officialdelta/periods/{segmentId}/{years}/{isPerNodeReport}")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Query, Role.Chain)]
        public async Task<IActionResult> GetOfficialDeltaPeriodAsync(int segmentId, int years, bool isPerNodeReport)
        {
            var result = await this.processor.GetOfficialDeltaPeriodAsync(segmentId, years, isPerNodeReport).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Validates the previous period nodes asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>If nodes of previous period are approved.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/officialdelta/previousperiods/validate")]
        [TrueAuthorize(Role.Administrator, Role.Chain)]
        public async Task<IActionResult> ValidatePreviousPeriodNodesAsync([FromBody]Ticket ticket)
        {
            var result = await this.processor.ValidatePreviousOfficialPeriodAsync(ticket).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Gets the official delta ticket processing status asynchronous.
        /// </summary>
        /// <param name="segmentId">The segment identifier.</param>
        /// <returns>The official delta ticket processing status.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("api/v{version:apiVersion}/officialdelta/segments/{segmentId}/ticketprocessingstatus")]
        [TrueAuthorize(Role.Administrator, Role.Chain)]
        public async Task<IActionResult> GetOfficialDeltaTicketProcessingStatusAsync(int segmentId)
        {
            var result = await this.processor.GetOfficialDeltaTicketProcessingStatusAsync(segmentId).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Gets the unapproved official nodes asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>unapproved official nodes.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/officialdelta/nodes/unapproved")]
        [TrueAuthorize(Role.Administrator, Role.Chain, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> GetUnapprovedOfficialNodesAsync([FromBody] Ticket ticket)
        {
            var result = await this.processor.GetUnapprovedOfficialNodesAsync(ticket).ConfigureAwait(false);
            return new EntityResult(result);
        }
    }
}
