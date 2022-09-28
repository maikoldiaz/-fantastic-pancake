// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogisticController.cs" company="Microsoft">
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
    using Ecp.True.Entities.Sap;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The NodeController.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Sap")]
    public class LogisticController : ODataController
    {
        /// <summary>
        /// The register file processor.
        /// </summary>
        private readonly ILogisticsProcessor processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogisticController"/> class.
        /// </summary>
        /// <param name="logisticsProcessor">The register file processor.</param>
        public LogisticController(ILogisticsProcessor logisticsProcessor)
        {
            this.processor = logisticsProcessor;
        }

        /// <summary>
        /// Cancel Batch.
        /// </summary>
        /// <param name="ticketId">Id Batch.</param>
        /// <returns>[True] if the creation is success.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/Logistics/{ticketId}")]
        [TrueAuthorize(Role.Administrator, Role.Query, Role.Chain)]
        public async Task<IActionResult> CancelBatchAsync(int ticketId)
        {
            await this.processor.CancelBatchAsync(ticketId).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.RegisterFilesUploadedSuccessfully);
        }

        /// <summary>
        /// Send Confirm Movements To Sap.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The result of confirm.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/logistics/confirmmovements")]
        [TrueAuthorize(Role.Administrator, Role.Query, Role.Chain)]
        public async Task<IActionResult> ConfirmLogisticMovementsAsync([FromBody] LogisticMovementsTicketRequest ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            await this.processor.ConfirmLogisticMovementsAsync(ticket).ConfigureAwait(false);
            return new EntityResult();
        }

        /// <summary>
        /// Forward logistic movements to SAP.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The result of confirm.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/logistics/forward")]
        [TrueAuthorize(Role.Administrator, Role.Query, Role.Chain)]
        public async Task<IActionResult> ForwardLogisticMovementsAsync([FromBody] LogisticMovementsTicketRequest ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            await this.processor.ForwardLogisticMovementsAsync(ticket).ConfigureAwait(false);
            return new EntityResult();
        }

        /// <summary>
        /// Validates the logistic nodes availables.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The result of validation.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/logistics/validatenodesavailables")]
        [TrueAuthorize(Role.Administrator, Role.Query, Role.Chain)]
        public async Task<IActionResult> ValidateNodeAvailablesLogisticMovementAsync([FromBody] Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var result = await this.processor.LogisticMovementNodeValidationsAsync(ticket).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Gets the logistic movement asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>Odata logistic movement.</returns>
        /// <response code="200">The ODATA query response.</response>
        [HttpGet]
        [EnableQuery]
        [Route("logisticmovement")]
        [ODataRoute("logisticmovement")]
        [TrueAuthorize(Role.Administrator, Role.Query, Role.Chain)]
        public async Task<IQueryable<SapLogisticMovementDetail>> QueryLogisticMovementAsync([FromQuery] int ticketId)
        {
            var result = await this.processor.GetLogisticMovementDetailAsync(ticketId).ConfigureAwait(false);
            return result.AsQueryable();
        }

        /// <summary>
        /// Gets the failed logistic movement asynchronous.
        /// </summary>
        /// <param name="categoryElementId">The categoryElement identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="scenarioTypeId">The scenarioType identifier.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <param name="ticketNodes">The ticked node list.</param>
        /// <returns>Odata failed logistic movement.</returns>
        /// <response code="200">The ODATA query response.</response>
        [HttpGet]
        [EnableQuery]
        [Route("failedlogisticmovement")]
        [ODataRoute("failedlogisticmovement")]
        [TrueAuthorize(Role.Administrator, Role.Query, Role.Chain)]
        public async Task<IQueryable<SapLogisticMovementDetail>> QueryFailedLogisticMovementAsync(
            [FromQuery] int categoryElementId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int scenarioTypeId,
            [FromQuery] int ownerId,
            [FromQuery] int[] ticketNodes)
        {
            Ticket ticket = new Ticket();
            ticket.CategoryElementId = categoryElementId;
            ticket.StartDate = startDate;
            ticket.EndDate = endDate;
            ticket.ScenarioTypeId = (ScenarioType)scenarioTypeId;
            ticket.OwnerId = ownerId;
            ticket.TicketNodes = ticketNodes.Select(x => new Entities.Admin.TicketNode { NodeId = x });
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var result = await this.processor.FailedLogisticMovementAsync(ticket).ConfigureAwait(false);
            return result.AsQueryable();
        }
    }
}
