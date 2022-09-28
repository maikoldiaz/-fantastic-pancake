// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportController.cs" company="Microsoft">
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
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Api.Filter;
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
    public class ReportController : ODataController
    {
        /// <summary>
        /// The ticket processor.
        /// </summary>
        private readonly IReportProcessor reportProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportController" /> class.
        /// </summary>
        /// <param name="reportProcessor">The report processor.</param>
        public ReportController(IReportProcessor reportProcessor)
        {
            this.reportProcessor = reportProcessor;
        }

        /// <summary>
        /// Saves the operational data cut off asynchronous.
        /// </summary>
        /// <param name="execution">The execution.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/operationaldatawithoutcutoff")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Query, Role.Programmer, Role.Chain)]
        [ValidateReportFilter]
        public async Task<IActionResult> SaveOperationalDataWithoutCutoffAsync([FromBody]ReportExecution execution)
        {
            var executionId = await this.reportProcessor.SaveOperationalDataWithoutCutoffAsync(execution).ConfigureAwait(false);
            return new EntityResult(executionId);
        }

        /// <summary>
        /// Gets the operational data cut off status asynchronous.
        /// </summary>
        /// <param name="executionId">The execution Id.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/reports/{executionId}/status")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Query, Role.Chain, Role.Programmer)]
        public async Task<IActionResult> GetReportStatusAsync(int executionId)
        {
            var status = await this.reportProcessor.GetReportStatusAsync(executionId).ConfigureAwait(false);
            return new EntityResult(status);
        }

        /// <summary>
        /// Validates the logistic ticket asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The result of validation.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/logistics/validate")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Query, Role.Chain)]
        public async Task<IActionResult> ValidateLogisticTicketAsync([FromBody]Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var result = await this.reportProcessor.ValidateLogisticTicketAsync(ticket).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Saves the ticket node status asynchronous.
        /// </summary>
        /// <param name="ticketNodeStatusData">The ticket node status data.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/saveticketnodestatus")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Query, Role.Programmer)]
        public async Task<IActionResult> SaveTicketNodeStatusAsync([FromBody]TicketNodeStatusData ticketNodeStatusData)
        {
            await this.reportProcessor.SaveTicketNodeStatusAsync(ticketNodeStatusData).ConfigureAwait(false);
            return new EntityResult();
        }

        /// <summary>
        /// Saves the request data sent for event contracts report asynchronous.
        /// </summary>
        /// <param name="eventContractReportRequest">The event contract report request data.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/saveeventcontractreportrequest")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Query, Role.Programmer)]
        public async Task<IActionResult> SaveEventContractReportRequestAsync([FromBody]EventContractReportRequest eventContractReportRequest)
        {
            await this.reportProcessor.SaveEventContractReportRequestAsync(eventContractReportRequest).ConfigureAwait(false);
            return new EntityResult();
        }

        /// <summary>
        /// Saves the request data sent for node configuration report asynchronous.
        /// </summary>
        /// <param name="nodeConfigurationReportRequest">The node configuration report request data.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/savenodeconfigurationreportrequest")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Query)]
        public async Task<IActionResult> SaveNodeConfigurationReportRequestAsync([FromBody]NodeConfigurationReportRequest nodeConfigurationReportRequest)
        {
            await this.reportProcessor.SaveNodeConfigurationReportRequestAsync(nodeConfigurationReportRequest).ConfigureAwait(false);
            return new EntityResult();
        }

        /// <summary>
        /// Saves the official initial report asynchronous.
        /// </summary>
        /// <param name="execution">The execution.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/saveofficialinitialbalance")]
        [TrueAuthorize(Role.Administrator, Role.Query, Role.Chain)]
        [ValidateReportFilter]
        public async Task<IActionResult> SaveOfficialInitialReportAsync([FromBody]ReportExecution execution)
        {
            var executionId = await this.reportProcessor.SaveOfficialBalanceReportAsync(execution, ReportType.OfficialInitialBalance).ConfigureAwait(false);
            return new EntityResult(executionId);
        }

        /// <summary>
        /// Saves the non operational segment ownership report asynchronous.
        /// </summary>
        /// <param name="execution">The execution.</param>
        /// <returns>The task.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/savenonoperationalsegmentownership")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Query, Role.Chain, Role.Programmer)]
        public async Task<IActionResult> SaveNonOperationalSegmentOwnershipReportAsync([FromBody] ReportExecution execution)
        {
            var executionId = await this.reportProcessor.SaveOfficialBalanceReportAsync(execution, ReportType.OperativeBalance).ConfigureAwait(false);
            return new EntityResult(executionId);
        }

        /// <summary>
        /// Saves the request data sent for event send to SAP report asynchronous.
        /// </summary>
        /// <param name="execution">The event send to SAP report request data.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/savemovementsendtosapreportrequest")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Query, Role.Programmer, Role.Chain)]
        [ValidateReportFilter]
        public async Task<IActionResult> SaveMovementSendToSapReportRequestAsync([FromBody] ReportExecution execution)
        {
            var executionId = await this.reportProcessor.SaveEventSendToSapReportRequestAsync(execution).ConfigureAwait(false);
            return new EntityResult(executionId);
        }

        /// <summary>
        /// save official node status report request async.
        /// </summary>
        /// <param name="officialNodeStatusReportRequest">The event official node status report request data.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/saveofficialnodestatusreportrequest")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Query, Role.Programmer, Role.Chain)]
        public async Task<IActionResult> SaveOfficialNodeStatusReportRequestAsync([FromBody] OfficialNodeStatusReportRequest officialNodeStatusReportRequest)
        {
            await this.reportProcessor.SaveEventOfficialNodeStatusReportRequestAsync(officialNodeStatusReportRequest).ConfigureAwait(false);
            return new EntityResult();
        }

        /// <summary>
        /// Save the request data for user roles and permissions report asynchronous.
        /// </summary>
        /// <param name="execution">The event report request data.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/saveuserrolesandpermissionsreportrequest")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Auditor, Role.Programmer, Role.Chain)]
        public async Task<IActionResult> SaveUserRolesAndPermissionsReportRequestAsync([FromBody] ReportExecution execution)
        {
            await this.reportProcessor.SaveUserRolesAndPermissionsReportRequestAsync(execution).ConfigureAwait(false);
            return new EntityResult();
        }

        /// <summary>
        /// Checks if the report exists execution asynchronous.
        /// </summary>
        /// <param name="reportExecution">The report execution.</param>
        /// <param name="type">Type of the report.</param>
        /// <returns>The IActionResult.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/report/{type}/exists")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Query, Role.Chain, Role.Programmer)]
        public async Task<IActionResult> ExistsReportExecutionAsync([FromBody]ReportExecution reportExecution, ReportType type)
        {
            var executionId = await this.reportProcessor.ExistsReportExecutionAsync(reportExecution, type).ConfigureAwait(false);
            return new EntityResult(executionId);
        }

        /// <summary>
        /// Queries the report execution asynchronous.
        /// </summary>
        /// <returns>The IQueryable ReportExecution.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("reportexecutions")]
        [ODataRoute("reportexecutions")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Query, Role.Chain, Role.Programmer, Role.Auditor)]
        public Task<IQueryable<ReportExecutionEntity>> QueryReportExecutionAsync()
        {
            return this.reportProcessor.QueryViewAsync<ReportExecutionEntity>();
        }
    }
}
