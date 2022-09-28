// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TicketController.cs" company="Microsoft">
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
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The TicketController.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Transport Balance")]
    public class TicketController : ODataController
    {
        /// <summary>
        /// The ticket processor.
        /// </summary>
        private readonly ITicketProcessor ticketProcessor;

        /// <summary>
        /// The ownership ticket processor.
        /// </summary>
        private readonly IOwnershipTicketProcessor ownershipTicketProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketController"/> class.
        /// </summary>
        /// <param name="ticketProcessor">The ticket processor.</param>
        /// <param name="ownershipTicketProcessor">The ownership ticket processor.</param>
        public TicketController(
            ITicketProcessor ticketProcessor,
            IOwnershipTicketProcessor ownershipTicketProcessor)
        {
            this.ticketProcessor = ticketProcessor;
            this.ownershipTicketProcessor = ownershipTicketProcessor;
        }

        /// <summary>
        /// Gets the last cut off date asynchronous.
        /// </summary>
        /// <returns>Returns the last cutoff date.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("tickets")]
        [ODataRoute("tickets")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Chain, Role.Programmer, Role.Query)]
        public Task<IQueryable<Ticket>> QueryTicketsAsync()
        {
            return this.ticketProcessor.QueryAllAsync<Ticket>(null);
        }

        /// <summary>
        /// Gets ticket Info.
        /// </summary>
        /// <returns>The ticket info.</returns>
        /// <param name="ticketId">Ticket Id.</param>
        /// <response code="200">The list of all latest ticket.</response>
        /// <response code="500">Unknown error while getting ticket.</response>
        [HttpGet]
        [Route("api/v{version:apiVersion}/ticketinfo/{ticketId}")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> GetTicketInfoAsync(int ticketId)
        {
            var result = await this.ticketProcessor.GetTicketInfoAsync(ticketId).ConfigureAwait(false);
            return new EntityResult(result, Entities.Constants.TicketNotExists);
        }

        /// <summary>
        /// Gets ticket Info.
        /// </summary>
        /// <returns>The count of Movement and inventory in date range.</returns>
        /// <param name="ticket">The ticket.</param>
        [HttpPost]
        [Route("api/v{version:apiVersion}/cutoff/validate")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> ValidateCutOffAsync(Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var result = await this.ticketProcessor.ValidateCutOffAsync(ticket).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Validates the existing ticket asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The result of validation.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/ownership/exists")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> ValidateExistingTicketAsync([FromBody] Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var result = await this.ownershipTicketProcessor.ValidateExistingTicketAsync(ticket).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Gets the ownership processing and cutoff date.
        /// </summary>
        /// <param name="segmentId">The segment identifier.</param>
        /// <returns>The dates.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/segments/{segmentId}/ownership")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> GetOwnershipBySegmentAsync(int segmentId)
        {
            var result = await this.ownershipTicketProcessor.GetOwnershipBySegmentAsync(segmentId).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Gets the ownership last performed date by segment asynchronous.
        /// </summary>
        /// <param name="segmentId">The segment identifier.</param>
        /// <returns>The ownership last performed date.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/segments/{segmentId}/logistics/ownershipLastPerformed")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Query, Role.Chain)]
        public async Task<IActionResult> GetOwnershipLastPerformedDateBySegmentAsync(int segmentId)
        {
            var result = await this.ownershipTicketProcessor.GetOwnershipLastPerformedDateBySegmentAsync(segmentId).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Gets the tickets asynchronous.
        /// </summary>
        /// <returns>Odata Tickets.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("ticketentities")]
        [ODataRoute("ticketentities")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Query, Role.Chain)]
        public Task<IQueryable<TicketEntity>> QueryTicketEntitiesAsync()
        {
            return this.ownershipTicketProcessor.QueryViewAsync<TicketEntity>();
        }

        /// <summary>
        /// Checks if the ticket exists asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>
        /// Entity result.
        /// </returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/tickets/exists")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> ExistsTicketAsync(Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));

            var isExists = await this.ticketProcessor.ExistsTicketAsync(ticket).ConfigureAwait(false);
            return new EntityResult(isExists);
        }

        /// <summary>
        /// Checks if the delta ticket exists asynchronous.
        /// </summary>
        /// <param name="segmentId">The segment identifier.</param>
        /// <returns>returns true/false.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/tickets/{segmentId}/delta/exists")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> ExistsDeltaTicketAsync(int segmentId)
        {
            var isExists = await this.ticketProcessor.ExistsDeltaTicketAsync(segmentId).ConfigureAwait(false);
            return new EntityResult(isExists);
        }

        /// <summary>
        /// Saves the operational cut off asynchronous.
        /// </summary>
        /// <param name="operationalCutOff">The operational cut off.</param>
        /// <returns>The Task.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/operationalcutoff")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Query, Role.Chain)]
        public async Task<IActionResult> SaveOperationalCutOffAsync([FromBody] OperationalCutOff operationalCutOff)
        {
            await this.ticketProcessor.SaveTicketAsync(operationalCutOff).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.TicketCreateSuccess);
        }

        /// <summary>
        /// Gets the ticket processing status.
        /// </summary>
        /// <param name="segmentId">The segment ID.</param>
        /// <param name="isOwnershipCheck">To check whether ownership processing is going on.</param>
        /// <returns>The processing status.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("api/v{version:apiVersion}/segments/{segmentId}/{isOwnershipCheck}/ticketprocessingstatus")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Query)]
        public async Task<IActionResult> GetTicketProcessingStatusAsync(int segmentId, bool isOwnershipCheck)
        {
            var result = await this.ownershipTicketProcessor.GetTicketProcessingStatusAsync(segmentId, isOwnershipCheck).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Gets the list of inventories.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>Returns the list of inventories.</returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/cutoff/deltainventories")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> ExistsDeltaInventoriesAsync([FromBody] Ticket ticket)
        {
            var result = await this.ownershipTicketProcessor.GetDeltaInventoriesAsync(ticket).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Gets the list of movements.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>Returns the list of inventories.</returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/cutoff/deltamovements")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> ExistsDeltaMovementsAsync([FromBody] Ticket ticket)
        {
            var result = await this.ownershipTicketProcessor.GetDeltaMovementsAsync(ticket).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Gets Delta exceptions details asynchronous.
        /// </summary>
        /// <returns>The delta exceptions details.</returns>
        /// <param name="ticketId">Ticket Id.</param>
        /// <param name="ticketType">Ticket Type.</param>
        /// <response code="200">The list of all delta exceptions.</response>
        /// <response code="500">Unknown error while getting delta exceptions.</response>
        [HttpGet]
        [Route("api/v{version:apiVersion}/deltaExceptions/{ticketId}/{ticketType}")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Chain)]
        public async Task<IActionResult> GetDeltaExceptionsDetailsAsync(int ticketId, TicketType ticketType)
        {
            var result = await this.ticketProcessor.GetDeltaExceptionsDetailsAsync(ticketId, ticketType).ConfigureAwait(false);
            return new EntityResult(result, Entities.Constants.DeltaExceptionsNotExists);
        }

        /// <summary>
        /// Updates the comment asynchronous.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <returns>The update success.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/updatecomment")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> UpdateCommentAsync([FromBody] OperationalCutOffBatch batch)
        {
            await this.ticketProcessor.UpdateCommentAsync(batch).ConfigureAwait(false);
            return new EntityResult();
        }

        /// <summary>
        /// Approve official node delta asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The task.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/deltanode/submitforapproval")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Chain)]
        public async Task<IActionResult> SendDeltaNodeForApprovalAsync([FromBody] DeltaNodeStatusRequest request)
        {
            var result = await this.ticketProcessor.SendDeltaNodeForApprovalAsync(request).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Send reopen official node delta asynchronous.
        /// </summary>
        /// <param name="deltaNodeId">The delta node identifier.</param>
        /// <returns>The task.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/deltanode/{deltaNodeId}/reopen")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Chain)]
        public async Task<IActionResult> GetDeltaNodesForReopenAsync(int deltaNodeId)
        {
            var result = await this.ticketProcessor.GetDeltaNodesForReopenAsync(deltaNodeId).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Reopens the delta nodes asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The task.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/deltanode/reopen")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Chain)]
        public async Task<IActionResult> ReopenDeltaNodesAsync([FromBody] DeltaNodeReopenRequest request)
        {
            await this.ticketProcessor.ReopenDeltaNodesAsync(request).ConfigureAwait(false);
            return new EntityResult();
        }
    }
}