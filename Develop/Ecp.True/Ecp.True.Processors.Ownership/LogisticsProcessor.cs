// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogisticsProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Core.HandleFailure;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Processors.Ownership.Services.Interfaces;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// The LogisticsProcessor.
    /// </summary>
    public class LogisticsProcessor : ProcessorBase, ILogisticsProcessor
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The ownership service.
        /// </summary>
        private readonly ILogisticsService logisticsService;

        /// <summary>
        /// The failure handler.
        /// </summary>
        private readonly IFailureHandler failureHandler;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<LogisticsProcessor> logger;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The business context.
        /// </summary>
        private readonly IBusinessContext businessContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogisticsProcessor" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="logisticsService">The logistics service.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="failureHandlerFactory">The failure handler factory.</param>
        /// <param name="businessContext">The business Context.</param>
        public LogisticsProcessor(
            IUnitOfWorkFactory unitOfWorkFactory,
            IRepositoryFactory factory,
            ILogisticsService logisticsService,
            ITrueLogger<LogisticsProcessor> logger,
            IAzureClientFactory azureClientFactory,
            IFailureHandlerFactory failureHandlerFactory,
            IBusinessContext businessContext)
             : base(factory)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            ArgumentValidators.ThrowIfNull(failureHandlerFactory, nameof(failureHandlerFactory));
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.logisticsService = logisticsService;
            this.failureHandler = failureHandlerFactory.GetFailureHandler(TicketType.OfficialLogistics);
            this.azureClientFactory = azureClientFactory;
            this.logger = logger;
            this.businessContext = businessContext;
        }

        /// <inheritdoc/>
        public async Task GenerateOfficialLogisticsAsync(Ticket ticket, int systemType)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            try
            {
                this.logger.LogInformation($"Start official logistics excel process for ticket:  {ticket.TicketId}", $"{ticket.TicketId}");
                var parameters = new Dictionary<string, object>
            {
                { "@TicketId", ticket.TicketId },
            };
                var repository = this.unitOfWork.CreateRepository<GenericLogisticsMovement>();
                var movements = await repository.ExecuteQueryAsync(Repositories.Constants.GetOfficialLogisticsMovement, parameters).ConfigureAwait(false);
                if (!movements.Any())
                {
                    var failureInfo = new FailureInfo(ticket.TicketId, string.Format(CultureInfo.InvariantCulture, LogisticsConstants.NoDataError, LogisticsConstants.Oficial));
                    await this.failureHandler.HandleFailureAsync(this.unitOfWork, failureInfo).ConfigureAwait(false);
                    return;
                }

                var officialMovements = await this.logisticsService.TransformAsync(movements, ticket, (SystemType)systemType, ScenarioType.OFFICER).ConfigureAwait(false);
                if (officialMovements.Any())
                {
                    await this.logisticsService.DoFinalizeOfficialProcessAsync(officialMovements, ticket, (SystemType)systemType).ConfigureAwait(false);
                    var errorMessage = officialMovements.FirstOrDefault(x => x.Status == StatusType.EMPTY)?.ErrorMessage;
                    await this.UpdateTicketAsync(ticket.TicketId, ticket.CategoryElement.Name, ticket.Owner.Name, errorMessage, (SystemType)systemType, false).ConfigureAwait(false);
                }
            }
            catch (Exception exe)
            {
                await this.UpdateTicketAsync(ticket.TicketId, ticket.CategoryElement.Name, ticket.Owner.Name, exe.Message, (SystemType)systemType, true).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task<Ticket> GetTicketAsync(int ticketId)
        {
            var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();
            return await ticketRepository.FirstOrDefaultAsync(t => t.TicketId == ticketId, "Owner", "CategoryElement").ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task CancelBatchAsync(int ticketId)
        {
            var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();
            var ticketRecord = await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);

            ArgumentValidators.ThrowIfNull(ticketRecord, nameof(ticketRecord));

            ticketRecord.Status = StatusType.CANCELLED;
            ticketRepository.Update(ticketRecord);

            var logisticMovementRepository = this.unitOfWork.CreateRepository<LogisticMovement>();
            var logisticMovementRecords = await logisticMovementRepository.GetAllAsync(x => x.TicketId == ticketId).ConfigureAwait(false);

            logisticMovementRecords.ForEach(x => x.StatusProcessId = StatusType.CANCELLED);

            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Get movement detail.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>the result.</returns>
        public async Task<IEnumerable<SapLogisticMovementDetail>> GetLogisticMovementDetailAsync(int ticketId)
        {
            ArgumentValidators.ThrowIfNull(ticketId, nameof(ticketId));
            var repository = this.CreateRepository<SapLogisticMovementDetail>();
            var parameters = new Dictionary<string, object>
            {
                    { "@TicketId", ticketId },
            };
            return await repository.ExecuteQueryAsync(Repositories.Constants.LogisticMovementDetailsForTicketProcedureName, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// This Procedure is used to validate that the nodes for the next case.
        /// Validate available nodes.
        /// Validate nodes with submission to SAP.
        /// Approved nodes.
        /// Predecessor nodes with submission to SAP.
        /// </summary>
        /// <param name="ticket">The ticket dto.</param>
        /// <returns>Returns the nodes availables.</returns>
        public Task<IEnumerable<NodesForSegmentResult>> LogisticMovementNodeValidationsAsync(Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var parameters = new Dictionary<string, object>
            {
                { "@SegmentId", ticket.CategoryElementId },
                { "@StartDate", ticket.StartDate },
                { "@EndDate", ticket.EndDate },
                { "@ScenarioTypeId", (int)ticket.ScenarioTypeId },
                { "@OwnerId", ticket.OwnerId },
                {
                    "@DtNodes", ticket.TicketNodes.Select(ticketNode => ticketNode.NodeId)
                     .ToDataTable(Repositories.Constants.NodeIdColumnName, Repositories.Constants.NodeListType)
                },
            };
            return this.CreateRepository<NodesForSegmentResult>().ExecuteQueryAsync(Repositories.Constants.GetLogisticMovementNodeValidations, parameters);
        }

        /// <summary>
        /// Confirm the logistic movements for sent to sap.
        /// </summary>
        /// <param name="ticket">The ticket dto.</param>
        /// <returns>Returns the Ticket.</returns>
        public async Task<LogisticMovementsTicketRequest> ConfirmLogisticMovementsAsync(LogisticMovementsTicketRequest ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            await this.UpdateConfirmedLogisticMovementsAsync(ticket).ConfigureAwait(false);
            var sapRequest = new LogisticQueueMessage(SapRequestType.LogisticMovement, ticket.TicketId, StatusType.SENT);
            var client = this.azureClientFactory.GetQueueClient(QueueConstants.SapLogisticQueue);
            var sessionId = Guid.NewGuid().ToString();
            await client.QueueSessionMessageAsync(sapRequest, sessionId).ConfigureAwait(false);

            return ticket;
        }

        /// <summary>
        /// Confirm the logistic movements for sent to sap.
        /// </summary>
        /// <param name="ticket">The ticket dto.</param>
        /// <returns>Returns the Ticket.</returns>
        public async Task ForwardLogisticMovementsAsync(LogisticMovementsTicketRequest ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));

            await this.UpdateTicketStatusAsync(ticket).ConfigureAwait(false);

            await this.UpdateForwardLogisticMovementsAsync(ticket).ConfigureAwait(false);

            var sapRequest = new LogisticQueueMessage(SapRequestType.LogisticMovement, ticket.TicketId, StatusType.FORWARD);
            var client = this.azureClientFactory.GetQueueClient(QueueConstants.SapLogisticQueue);
            var sessionId = Guid.NewGuid().ToString();
            await client.QueueSessionMessageAsync(sapRequest, sessionId).ConfigureAwait(false);
        }

        /// <summary>
        /// Update Confirmed Logistic Movements.
        /// </summary>
        /// <param name="ticket">The session identifier.</param>
        /// <returns>If operation is valid or not.</returns>
        public async Task UpdateConfirmedLogisticMovementsAsync(LogisticMovementsTicketRequest ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@TicketId", ticket.TicketId },
                    {
                        "@DtMovement", ticket.Movements
                         .ToDataTable(Repositories.Constants.TransaccionIdColumnName, Repositories.Constants.MovementListType)
                    },
                };
                var repository = this.CreateRepository<UpdateLogisticMovements>();
                await repository.ExecuteQueryAsync(Repositories.Constants.UpdateConfirmedLogisticMovements, parameters).ConfigureAwait(false);
            }
            catch (Exception)
            {
                this.logger.LogError($"Error in UpdateConfirmedLogisticMovementsAsync for ticket: {ticket.TicketId}", $"{ticket.TicketId}");
                throw;
            }
        }

        /// <summary>
        /// Update ticket status by id.
        /// </summary>
        /// <param name="ticketLogistic">The logistic ticket request.</param>
        /// <returns>If operation is valid or not.</returns>
        public async Task UpdateTicketStatusAsync(LogisticMovementsTicketRequest ticketLogistic)
        {
            ArgumentValidators.ThrowIfNull(ticketLogistic, nameof(ticketLogistic));
            try
            {
                var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();
                var ticket = await ticketRepository.GetByIdAsync(ticketLogistic.TicketId).ConfigureAwait(false);
                ticket.CreatedBy = this.businessContext.UserId;
                ticketRepository.Update(ticket);
                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception)
            {
                this.logger.LogError($"Error in UpdateTicketStatusAsync for ticket: {ticketLogistic.TicketId}", $"{ticketLogistic.TicketId}");
                throw;
            }
        }

        /// <summary>
        /// Update Confirmed Logistic Movements.
        /// </summary>
        /// <param name="ticket">The session identifier.</param>
        /// <returns>If operation is valid or not.</returns>
        public async Task UpdateForwardLogisticMovementsAsync(LogisticMovementsTicketRequest ticket)
        {
            try
            {
                ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));

                var repository = this.unitOfWork.CreateRepository<LogisticMovement>();

                foreach (var item in ticket.Movements)
                {
                    LogisticMovement movement = await repository.FirstOrDefaultAsync(m => m.MovementTransactionId == item
                    && m.TicketId == ticket.TicketId && m.StatusProcessId == StatusType.FAILED).ConfigureAwait(false);
                    movement.StatusProcessId = StatusType.FORWARD;
                    repository.Update(movement);
                    await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                this.logger.LogError($"Error in UpdateForwardLogisticMovementsAsync for ticket: {ticket.TicketId}", $"{ticket.TicketId}");
                throw;
            }
        }

        /// <summary>
        /// This Procedure is used to get the failed logistic movements.
        /// Validate  the start date and end date.
        /// Validate the segment.
        /// Validate the ownership.
        /// validate the stage type.
        /// validate the nodes list.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The failed logistic movements.</returns>
        public Task<IEnumerable<SapLogisticMovementDetail>> FailedLogisticMovementAsync(Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var parameters = new Dictionary<string, object>
            {
                { "@SegmentId", ticket.CategoryElementId },
                { "@StartDate", ticket.StartDate },
                { "@EndDate", ticket.EndDate },
                { "@ScenarioTypeId", (int)ticket.ScenarioTypeId },
                { "@OwnerId", ticket.OwnerId },
                {
                    "@DtNodes", ticket.TicketNodes.Select(ticketNode => ticketNode.NodeId)
                     .ToDataTable(Repositories.Constants.NodeIdColumnName, Repositories.Constants.NodeListType)
                },
            };
            return this.CreateRepository<SapLogisticMovementDetail>().ExecuteQueryAsync(Repositories.Constants.FailedLogisticMovementProcedureName, parameters);
        }

        /// <summary>
        /// Update the ticket.
        /// </summary>
        /// <param name="ticketId">ticketId. </param>
        /// <param name="segmentName">segmentName. </param>
        /// <param name="ownerName">ownerName. </param>
        /// <param name="error">error. </param>
        /// <param name="systemType">systemType. </param>
        /// <param name="sqlError">sqlError. </param>
        /// <returns>Task. </returns>
        private async Task UpdateTicketAsync(int ticketId, string segmentName, string ownerName, string error, SystemType systemType, bool sqlError)
        {
            var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();
            var ticket = await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);
            var status = systemType != SystemType.SIV ? StatusType.VISUALIZATION : StatusType.PROCESSED;
            ticket.Status = !string.IsNullOrEmpty(error)
                ? StatusType.ERROR
                : status;

            ticket.BlobPath = (systemType == SystemType.SIV) && !sqlError ? $"{LogisticsConstants.ReportName}_{segmentName}_{ownerName}_{ticketId}.xlsx" : string.Empty;
            ticket.ErrorMessage = error ?? string.Empty;
            ticketRepository.Update(ticket);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }
    }
}
