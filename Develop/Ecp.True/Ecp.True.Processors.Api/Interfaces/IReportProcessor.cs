// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReportProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// The report processor interface.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.Interfaces.IProcessor" />
    public interface IReportProcessor : IProcessor
    {
        /// <summary>
        /// Saves the operational data without cut off asynchronous.
        /// </summary>
        /// <param name="execution">The execution.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        Task<int> SaveOperationalDataWithoutCutoffAsync(ReportExecution execution);

        /// <summary>
        /// Gets the operational data cut off status asynchronous.
        /// </summary>
        /// <param name="executionId">The execution Id.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        Task<StatusType> GetReportStatusAsync(int executionId);

        /// <summary>
        /// Validates the logistic ticket asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The result of validation.</returns>
        Task<IEnumerable<LogisticsTicketValidationResult>> ValidateLogisticTicketAsync(Ticket ticket);

        /// <summary>
        /// Saves the Ticket Node Status asynchronous.
        /// </summary>
        /// <param name="ticketNodeStatusData">The ticket node status data.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        Task SaveTicketNodeStatusAsync(TicketNodeStatusData ticketNodeStatusData);

        /// <summary>
        /// Saves the request data sent for event contracts report asynchronous.
        /// </summary>
        /// <param name="eventContractReportRequest">The event contract report request data.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        Task SaveEventContractReportRequestAsync(EventContractReportRequest eventContractReportRequest);

        /// <summary>
        /// Saves the request data sent for event contracts report asynchronous.
        /// </summary>
        /// <param name="execution">The event send to SAP report request data.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        Task<int> SaveEventSendToSapReportRequestAsync(ReportExecution execution);

        /// <summary>
        /// Saves the request data user roles and permission report asynchronous.
        /// </summary>
        /// <param name="execution">The event user roles and permission report request data.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        Task<int> SaveUserRolesAndPermissionsReportRequestAsync(ReportExecution execution);

        /// <summary>
        /// Saves the request data sent for node configuration report asynchronous.
        /// </summary>
        /// <param name="nodeConfigurationReportRequest">The node configuration report request data.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        Task SaveNodeConfigurationReportRequestAsync(NodeConfigurationReportRequest nodeConfigurationReportRequest);

        /// <summary>
        /// Saves the official balance report asynchronous.
        /// </summary>
        /// <param name="execution">The execution.</param>
        /// <param name="reportType">Type of the report.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task<int> SaveOfficialBalanceReportAsync(ReportExecution execution, ReportType reportType);

        /// <summary>
        /// Saves the request data official node status report asynchronous.
        /// </summary>
        /// <param name="officialNodeStatusReportRequest">The event official node status report request data.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        Task SaveEventOfficialNodeStatusReportRequestAsync(OfficialNodeStatusReportRequest officialNodeStatusReportRequest);

        /// <summary>
        /// Exists the report execution asynchronous.
        /// </summary>
        /// <param name="reportExecution">The report execution.</param>
        /// <param name="reportType">Type of the report.</param>
        /// <returns>Returns the ReportExecutionId.</returns>
        Task<int> ExistsReportExecutionAsync(ReportExecution reportExecution, ReportType reportType);
    }
}
