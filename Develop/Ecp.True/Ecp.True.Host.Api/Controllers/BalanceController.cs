// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BalanceController.cs" company="Microsoft">
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
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Balance.Interfaces;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The category controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Admin")]
    public class BalanceController : ODataController
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private readonly IBalanceProcessor processor;

        /// <summary>
        /// The Queue processor.
        /// </summary>
        private readonly IQueueProcessor queueprocessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="BalanceController"/> class.
        /// </summary>
        /// <param name="processor">The processor.</param>
        /// <param name="queueprocessor">The queueprocessor.</param>
        public BalanceController(IBalanceProcessor processor, IQueueProcessor queueprocessor)
        {
            this.processor = processor;
            this.queueprocessor = queueprocessor;
        }

        /// <summary>
        /// Gets the list of nodes.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>Returns the list of nodes.</returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/cutoff/initial")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> ValidateInitialInventoryAsync([FromBody] Ticket ticket)
        {
            var result = await this.processor.ValidateOwnershipInitialInventoryAsync(ticket).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Saves the operational cut off asynchronous.
        /// </summary>
        /// <param name="getUnbalances">The get unbalances.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/unbalances")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> GetUnbalancesAsync([FromBody] UnbalanceRequest getUnbalances)
        {
            var unbalances = await this.processor.CalculateAsync(getUnbalances).ConfigureAwait(false);
            return new EntityResult(unbalances, Entities.Constants.TicketNotExists);
        }

        /// <summary>
        /// Queries the system unbalance asynchronous.
        /// </summary>
        /// <returns>The System Unbalance.</returns>
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 3)]
        [Route("systemunbalances")]
        [ODataRoute("systemunbalances")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Query, Role.Programmer)]
        public Task<IQueryable<SystemUnbalance>> QuerySystemUnbalanceAsync()
        {
            return this.processor.QueryAllAsync<SystemUnbalance>(null);
        }

        /// <summary>
        /// Gets the transfer points asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>
        /// The transfer point movements.
        /// </returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/transferpointmovements")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> GetTransferPointsAsync([FromBody] Ticket ticket)
        {
            var result = await this.processor.GetTransferPointsAsync(ticket).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Gets the sap tracking errors asynchronous.
        /// </summary>
        /// <param name="sapTrackingId">The sap tracking identifier.</param>
        /// <returns>The SAP Tracking errors.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/saptracking/errors/{sapTrackingId}")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> GetSapTrackingErrorsAsync(int sapTrackingId)
        {
            var result = await this.processor.GetSapTrackingErrorsAsync(sapTrackingId).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Gets the list of first time nodes.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>Returns the list of first time nodes.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/cutoff/firsttimenodes")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> GetFirstTimeNodesAsync([FromBody] Ticket ticket)
        {
            var result = await this.processor.GetFirstTimeNodesAsync(ticket).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// ownership.
        /// </summary>
        /// <param name="ticketId">The ticketId.</param>
        /// <returns>The cutoff Recalculation.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/cutoff/recalculateBalance")]
        [TrueAuthorize]
        public async Task<IActionResult> RecalculateCutOffBalanceAsync(int ticketId)
        {
            await this.queueprocessor.PushQueueSessionMessageAsync(ticketId, QueueConstants.RecalculateOperationalCutoffBalanceQueue).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.PushMessagebalanceCreateSuccess);
        }
    }
}
