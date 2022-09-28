// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Core;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// The Ticket Processor.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Api.ProcessorBase" />
    /// <seealso cref="Ecp.True.Processors.Api.Interfaces.ITicketProcessor" />
    public class ReportProcessor : ProcessorBase, IReportProcessor
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<ReportProcessor> logger;

        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportProcessor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="azureClientFactory">The Azure client factory.</param>
        public ReportProcessor(
            ITrueLogger<ReportProcessor> logger,
            IRepositoryFactory factory,
            IUnitOfWorkFactory unitOfWorkFactory,
            IAzureClientFactory azureClientFactory)
             : base(factory)
        {
            this.logger = logger;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.azureClientFactory = azureClientFactory;
        }

        /// <inheritdoc/>
        public async Task<int> SaveOperationalDataWithoutCutoffAsync(ReportExecution execution)
        {
            ArgumentValidators.ThrowIfNull(execution, nameof(execution));

            return await this.SaveGeneralReportRequestAsync(execution, ReportType.BeforeCutOff).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the operational data cut off status asynchronous.
        /// </summary>
        /// <param name="executionId">The execution Id.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        public async Task<StatusType> GetReportStatusAsync(int executionId)
        {
            var entity = await this.CreateRepository<ReportExecution>().GetByIdAsync(executionId).ConfigureAwait(false);
            return entity.StatusTypeId;
        }

        /// <summary>
        /// Validates the logistic ticket asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The result of validation.</returns>
        public Task<IEnumerable<LogisticsTicketValidationResult>> ValidateLogisticTicketAsync(Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var parameters = new Dictionary<string, object>
            {
                { "@SegmentId", ticket.CategoryElementId },
                { "@StartDate", ticket.StartDate },
                { "@EndDate", ticket.EndDate },
                { "@NodeId", ticket.NodeId },
            };

            return this.CreateRepository<LogisticsTicketValidationResult>().ExecuteQueryAsync(Repositories.Constants.ValidateLogisticNodeTicket, parameters);
        }

        /// <summary>
        /// Saves the ticket node status asynchronous.
        /// </summary>
        /// <param name="ticketNodeStatusData">The ticket node status data.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public Task SaveTicketNodeStatusAsync(TicketNodeStatusData ticketNodeStatusData)
        {
            ArgumentValidators.ThrowIfNull(ticketNodeStatusData, nameof(ticketNodeStatusData));

            var parameters = new Dictionary<string, object>
            {
                { "@Segment", ticketNodeStatusData.ElementName },
                { "@StartDate", ticketNodeStatusData.StartDate },
                { "@EndDate", ticketNodeStatusData.EndDate },
                { "@ExecutionId", ticketNodeStatusData.ExecutionId },
                { "@ANSConfigurationValue", ticketNodeStatusData.AnsConfigurationDays },
            };
            return this.CreateRepository<Ticket>().ExecuteAsync(Repositories.Constants.TicketNodeStatusProcedureName, parameters);
        }

        /// <summary>
        /// Saves the request data sent for event contracts report asynchronous.
        /// </summary>
        /// <param name="eventContractReportRequest">The event contract report request data.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public Task SaveEventContractReportRequestAsync(EventContractReportRequest eventContractReportRequest)
        {
            ArgumentValidators.ThrowIfNull(eventContractReportRequest, nameof(eventContractReportRequest));

            var parameters = new Dictionary<string, object>
            {
                { "@ElementName", eventContractReportRequest.ElementName },
                { "@NodeName", eventContractReportRequest.Node },
                { "@StartDate", eventContractReportRequest.StartDate },
                { "@EndDate", eventContractReportRequest.EndDate },
                { "@ExecutionId", eventContractReportRequest.ExecutionId },
            };
            return this.CreateRepository<Ticket>().ExecuteAsync(Repositories.Constants.EventContractReportRequestProcedureName, parameters);
        }

        /// <summary>
        /// Saves the request data sent for event send to SAP report asynchronous.
        /// </summary>
        /// <param name="execution">The event send to SAP report request data.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public async Task<int> SaveEventSendToSapReportRequestAsync(ReportExecution execution)
        {
            ArgumentValidators.ThrowIfNull(execution, nameof(execution));

            return await this.SaveGeneralReportRequestAsync(execution, ReportType.SapBalance).ConfigureAwait(false);
        }

        /// <summary>
        /// Saves the request data user roles and permission report asynchronous.
        /// </summary>
        /// <param name="execution">The event user roles and permission report request data.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public async Task<int> SaveUserRolesAndPermissionsReportRequestAsync(ReportExecution execution)
        {
            ArgumentValidators.ThrowIfNull(execution, nameof(execution));

            return await this.SaveGeneralReportRequestAsync(execution, ReportType.UserRolesAndPermissions).ConfigureAwait(false);
        }

        /// <summary>
        /// Saves the request data sent for node configuration report asynchronous.
        /// </summary>
        /// <param name="nodeConfigurationReportRequest">The node configuration report request data.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public Task SaveNodeConfigurationReportRequestAsync(NodeConfigurationReportRequest nodeConfigurationReportRequest)
        {
            ArgumentValidators.ThrowIfNull(nodeConfigurationReportRequest, nameof(nodeConfigurationReportRequest));

            var parameters = new Dictionary<string, object>
            {
                { "@Category", nodeConfigurationReportRequest.Category },
                { "@Element", nodeConfigurationReportRequest.Element },
                { "@ExecutionId", nodeConfigurationReportRequest.ExecutionId },
            };
            return this.CreateRepository<Ticket>().ExecuteAsync(Repositories.Constants.NodeConfigurationReportRequestProcedureName, parameters);
        }

        /// <inheritdoc/>
        public async Task<int> SaveOfficialBalanceReportAsync(ReportExecution execution, ReportType reportType)
        {
            ArgumentValidators.ThrowIfNull(execution, nameof(execution));

            return await this.SaveGeneralReportRequestAsync(execution, reportType).ConfigureAwait(false);
        }

        /// <summary>
        /// Exists the report execution asynchronous.
        /// </summary>
        /// <param name="reportExecution">The report execution.</param>
        /// <param name="reportType">Type of the report.</param>
        /// <returns>
        /// returns true or false.
        /// </returns>
        public async Task<int> ExistsReportExecutionAsync(ReportExecution reportExecution, ReportType reportType)
        {
            var reportHash = IdGenerator.GenerateReportHash(reportExecution);
            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var reportExecutionRepository = unitOfWork.CreateRepository<ReportExecution>();
            var report = await reportExecutionRepository.FirstOrDefaultAsync(a => a.Hash == reportHash).ConfigureAwait(false);
            return report != null ? report.ExecutionId : 0;
        }

        /// <summary>
        /// Saves the request data official node status report asynchronous.
        /// </summary>
        /// <param name="officialNodeStatusReportRequest">The event official node status report request data.</param>
        /// <returns>
        /// A <see cref="Task" />representing the asynchronous operation.
        /// </returns>
        public Task SaveEventOfficialNodeStatusReportRequestAsync(OfficialNodeStatusReportRequest officialNodeStatusReportRequest)
        {
            ArgumentValidators.ThrowIfNull(officialNodeStatusReportRequest, nameof(officialNodeStatusReportRequest));

            var parameters = new Dictionary<string, object>
            {
                { "@Segment", officialNodeStatusReportRequest.ElementName },
                { "@InitialDate", officialNodeStatusReportRequest.StartDate },
                { "@EndDate", officialNodeStatusReportRequest.EndDate },
                { "@ExecutionId", officialNodeStatusReportRequest.ExecutionId },
            };
            return this.CreateRepository<Ticket>().ExecuteAsync(Repositories.Constants.NodeOficialStatusReportProcedureName, parameters);
        }

        /// <summary>
        /// Saves the request general data report asynchronous.
        /// </summary>
        /// <param name="execution">The event report request data.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        private async Task<int> SaveGeneralReportRequestAsync(ReportExecution execution, ReportType reportType)
        {
            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var repository = unitOfWork.CreateRepository<ReportExecution>();
            execution.StatusTypeId = StatusType.PROCESSING;
            execution.Hash = IdGenerator.GenerateReportHash(execution);
            repository.Insert(execution);

            // Save the execution Details to db.
            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);

            // Send messages to queue.
            await this.PushMessageToQueueAsync(execution.ExecutionId, reportType).ConfigureAwait(false);

            return execution.ExecutionId;
        }

        /// <summary>
        /// Pushes the message to queue asynchronous.
        /// </summary>
        /// <param name="executionId">The execution identifier.</param>
        /// <param name="reportType">Type of the report.</param>
        private async Task PushMessageToQueueAsync(int executionId, ReportType reportType)
        {
            try
            {
                var queueName = $"{reportType}report".ToLowerCase();
                var client = this.azureClientFactory.GetQueueClient(queueName);
                await client.QueueMessageAsync(executionId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var error = ex.InnerException ?? ex;
                this.logger.LogError(error, ex.Message, executionId);

                using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
                {
                    var reportExecutionRepository = unitOfWork.CreateRepository<ReportExecution>();
                    var report = await reportExecutionRepository.GetByIdAsync(executionId).ConfigureAwait(false);
                    report.StatusTypeId = StatusType.FAILED;
                    reportExecutionRepository.Update(report);

                    await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
                }
            }
        }
    }
}
