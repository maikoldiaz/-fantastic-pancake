// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TicketProcessor.cs" company="Microsoft">
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
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Caching;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Core.HandleFailure;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Extensions.Caching.Distributed;
    using EntitiesConstants = Ecp.True.Entities.Constants;

    /// <summary>
    /// The Ticket Processor.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Api.ProcessorBase" />
    /// <seealso cref="Ecp.True.Processors.Api.Interfaces.ITicketProcessor" />
    public class TicketProcessor : ProcessorBase, ITicketProcessor
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<TicketProcessor> logger;

        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        /// <summary>
        /// The business context.
        /// </summary>
        private readonly IBusinessContext businessContext;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The cache handler.
        /// </summary>
        private readonly ICacheHandler<string> cacheHandler;

        /// <summary>
        /// The failure handler factory.
        /// </summary>
        private readonly IFailureHandlerFactory failureHandlerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketProcessor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="failureHandlerFactory">The failure handler factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="businessContext">The business context.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="cacheHandler">The cache handler.</param>
        public TicketProcessor(
            ITrueLogger<TicketProcessor> logger,
            IFailureHandlerFactory failureHandlerFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            IRepositoryFactory factory,
            IBusinessContext businessContext,
            IConfigurationHandler configurationHandler,
            IAzureClientFactory azureClientFactory,
            ICacheHandler<string> cacheHandler)
             : base(factory)
        {
            this.logger = logger;
            this.failureHandlerFactory = failureHandlerFactory;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.businessContext = businessContext;
            this.configurationHandler = configurationHandler;
            this.azureClientFactory = azureClientFactory;
            this.cacheHandler = cacheHandler;
        }

        /// <inheritdoc />
        public Task<Ticket> GetTicketByIdAsync(int ticketId)
        {
            return this.CreateRepository<Ticket>().GetByIdAsync(ticketId);
        }

        /// <inheritdoc />
        public Task<TicketInfo> GetTicketInfoAsync(int ticketId)
        {
            return this.RepositoryFactory.TicketInfoRepository.GetTicketInfoAsync(ticketId);
        }

        /// <inheritdoc />
        public async Task<bool> ValidateCutOffAsync(Ticket ticket)
        {
            var movementRepository = this.RepositoryFactory.CreateRepository<Movement>();
            var numOfMovements = await movementRepository.GetCountAsync(x => x.OperationalDate.Date >= ticket.StartDate.Date
            && x.SegmentId == ticket.CategoryElementId &&
            x.OperationalDate.Date <= ticket.EndDate.Date && x.ScenarioId == ScenarioType.OPERATIONAL).ConfigureAwait(false);
            return numOfMovements >= 1;
        }

        /// <summary>
        /// Saves the operational cut off asynchronous.
        /// </summary>
        /// <param name="operationalCutOff">The operational cut off dto.</param>
        /// <returns>Returns the Status of update.</returns>
        public async Task SaveTicketAsync(OperationalCutOff operationalCutOff)
        {
            ArgumentValidators.ThrowIfNull(operationalCutOff, nameof(operationalCutOff));
            ArgumentValidators.ThrowIfNull(operationalCutOff.Ticket, nameof(operationalCutOff.Ticket));

            try
            {
                await this.ValidateCutoffTicketAsync(operationalCutOff).ConfigureAwait(false);

                int? scenarioType = null;
                if (operationalCutOff.Ticket.ScenarioTypeId != null)
                {
                    scenarioType = (int)operationalCutOff.Ticket.ScenarioTypeId;
                }

                var parameters = new Dictionary<string, object>
                {
                    { "@CategoryElementId", operationalCutOff.Ticket.CategoryElementId },
                    { "@StartDate", operationalCutOff.Ticket.StartDate },
                    { "@EndDate", operationalCutOff.Ticket.EndDate },
                    { "@UserId", this.businessContext.UserId },
                    { "@TicketTypeId", (int)operationalCutOff.Ticket.TicketTypeId },
                    { "@FirstTimeNodes", operationalCutOff.FirstTimeNodes.ToDataTable(Repositories.Constants.NodeIdColumnName, Repositories.Constants.NodeListType) },
                    { "@ScenarioTypeId", scenarioType },
                    {
                        "@LogisticNodes", operationalCutOff.Ticket.TicketNodes.Select(ticketNode => ticketNode.NodeId)
                        .ToDataTable(Repositories.Constants.NodeIdColumnName, Repositories.Constants.NodeListType)
                    },
                    {
                        "@FailedLogisticsMovements", operationalCutOff.FailedLogisticsMovements.Select(x => x)
                        .ToDataTable(Repositories.Constants.KeyTypeColumnName, Repositories.Constants.KeyType)
                    },
                };

                switch (operationalCutOff.Ticket.TicketTypeId)
                {
                    case TicketType.Cutoff:
                        await this.GetCutoffParametersAsync(operationalCutOff, parameters).ConfigureAwait(false);
                        break;
                    case TicketType.OfficialLogistics:
                    case TicketType.Logistics:
                        GetLogisticsParameters(parameters, operationalCutOff.Ticket.OwnerId.Value, operationalCutOff.Ticket.NodeId);
                        break;
                    case TicketType.Delta:
                        GetDeltaParameters(parameters);
                        break;
                    case TicketType.LogisticMovements:
                        GetLogisticsParameters(parameters, operationalCutOff.Ticket.OwnerId.Value, operationalCutOff.Ticket.NodeId);
                        break;
                    default:
                        GetOwnershipParameters(parameters);
                        break;
                }

                var repository = this.CreateRepository<SaveTicketResult>();
                var tickets = await repository.ExecuteQueryAsync(Repositories.Constants.SaveTicketProcedureName, parameters).ConfigureAwait(false);
                var categoryElementId = operationalCutOff.Ticket.CategoryElementId.ToString(CultureInfo.InvariantCulture);
                await this.PushMessageToQueueAsync(tickets, operationalCutOff.Ticket.TicketTypeId, categoryElementId, operationalCutOff.Ticket.ScenarioTypeId).ConfigureAwait(false);
            }
            finally
            {
                await this.cacheHandler.DeleteAsync(Convert.ToString(operationalCutOff.Ticket.CategoryElementId, CultureInfo.InvariantCulture), "East US").ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Exists the ticket asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>
        /// returns true/false.
        /// </returns>
        public async Task<bool> ExistsTicketAsync(Ticket ticket)
        {
            var ticketRepository = this.CreateRepository<Ticket>();
            var ticketCount = await ticketRepository.GetCountAsync(
                a => a.CategoryElementId == ticket.CategoryElementId &&
                a.TicketTypeId == ticket.TicketTypeId && a.Status == StatusType.PROCESSING).ConfigureAwait(false);

            if (ticketCount > 0)
            {
                return true;
            }

            var duplicateTicketCount = await ticketRepository.GetCountAsync(
                a => a.CategoryElementId == ticket.CategoryElementId &&
                a.TicketTypeId == ticket.TicketTypeId &&
                a.Status != StatusType.FAILED &&
               ticket.StartDate >= a.StartDate && ticket.StartDate <= a.EndDate).ConfigureAwait(false);

            return duplicateTicketCount != 0;
        }

        /// <summary>
        /// Exists the delta ticket asynchronous.
        /// </summary>
        /// <param name="segmentId">The segment identifier.</param>
        /// <returns>returns true/false.</returns>
        public async Task<bool> ExistsDeltaTicketAsync(int segmentId)
        {
            var ticketCount = await this.CreateRepository<Ticket>().GetCountAsync(
                a => a.CategoryElementId == segmentId && a.TicketTypeId == TicketType.Delta && a.Status == StatusType.PROCESSING)
                              .ConfigureAwait(false);
            return ticketCount != 0;
        }

        /// <summary>
        /// Gets the delta exceptions details asynchronous.
        /// </summary>
        /// <param name="ticketId">The error identifier. Format: d_id_p/f_I/M.</param>
        /// <param name="ticketType">Ticket Type.</param>
        /// <returns>
        /// the error details.
        /// </returns>
        public async Task<IEnumerable<DeltaExceptions>> GetDeltaExceptionsDetailsAsync(int ticketId, TicketType ticketType)
        {
            ArgumentValidators.ThrowIfNull(ticketId, nameof(ticketId));
            string storeProc = string.Empty;
            var parameters = new Dictionary<string, object>();
            if (ticketType == TicketType.Delta)
            {
                storeProc = Repositories.Constants.GetDeltaExceptionsDetailsProcedureName;
                parameters = new Dictionary<string, object>
                {
                    { "@TicketId", ticketId },
                };
            }
            else if (ticketType == TicketType.OfficialDelta)
            {
                storeProc = Repositories.Constants.GetOfficialDeltaExceptionsDetailsProcedureName;
                parameters = new Dictionary<string, object>
                {
                    { "@DeltaNodeId", ticketId },
                };
            }
            else
            {
                ArgumentValidators.ThrowIfNullOrEmpty(storeProc, nameof(storeProc));
            }

            var exceptionDetails = await this.CreateRepository<DeltaExceptions>().ExecuteQueryAsync(
                storeProc, parameters).ConfigureAwait(false);
            return exceptionDetails?.OrderBy(x => x.Type).ThenBy(y => y.Identifier);
        }

        /// <summary>
        /// Updates the comment asynchronous.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <returns>The task.</returns>
        public async Task UpdateCommentAsync(OperationalCutOffBatch batch)
        {
            ArgumentValidators.ThrowIfNull(batch, nameof(batch));
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@SessionId", batch.SessionId },
                    { "@Type", batch.Type },
                    { "@UserIdValue", this.businessContext.UserId },
                    { "@SegmentId", batch.SegmentId },
                };

                await this.CheckSessionExistsForSegmentAsync(batch.SessionId, batch.SegmentId).ConfigureAwait(false);

                switch (batch.Type)
                {
                    case 1:
                        await this.UpdateErrorsCommentAsync(batch, parameters).ConfigureAwait(false);
                        break;
                    case 2:
                        await this.UpdateUnbalancesCommentAsync(batch, parameters).ConfigureAwait(false);
                        break;
                    case 3:
                        await this.UpdateTransferPointsCommentAsync(batch, parameters).ConfigureAwait(false);
                        break;
                    default:
                        await this.UpdateTransferPointsCommentAsync(batch, parameters).ConfigureAwait(false);
                        break;
                }
            }
            catch
            {
                await this.cacheHandler.DeleteAsync(batch.SegmentId, Repositories.Constants.CacheRegionName).ConfigureAwait(false);
                throw;
            }
        }

        /// <summary>
        /// Approves the official node delta asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The task.</returns>
        public async Task<DeltaNodeApprovalResponse> SendDeltaNodeForApprovalAsync(DeltaNodeStatusRequest request)
        {
            ArgumentValidators.ThrowIfNull(request, nameof(request));
            var response = new DeltaNodeApprovalResponse();
            var isApproverExist = await this.ExistDeltaNodeApproverAsync(request.NodeId).ConfigureAwait(false);
            if (isApproverExist)
            {
                response.IsApproverExist = true;
                using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
                {
                    var repository = unitOfWork.CreateRepository<DeltaNode>();
                    var deltaNode = await repository.FirstOrDefaultAsync(x => x.DeltaNodeId == request.DeltaNodeId, "Ticket").ConfigureAwait(false);
                    if (deltaNode != null && deltaNode.Ticket != null && deltaNode.Ticket.Status == StatusType.DELTA)
                    {
                        var isValidOfficialDeltaNode = await ValidateOfficialDeltaNodeAsync(request, repository).ConfigureAwait(false);
                        if (isValidOfficialDeltaNode)
                        {
                            deltaNode.Status = OwnershipNodeStatusType.SUBMITFORAPPROVAL;
                            deltaNode.Editor = this.businessContext.Email;
                            repository.Update(deltaNode);
                            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
                            response.IsValidOfficialDeltaNode = true;
                            var client = this.azureClientFactory.GetQueueClient(QueueConstants.DeltaApprovalsQueue);
                            await client.QueueMessageAsync(request.DeltaNodeId).ConfigureAwait(false);
                        }
                    }
                }
            }

            return response;
        }

        /// <summary>
        /// Sends the delta node for reopen asynchronous.
        /// </summary>
        /// <param name="deltaNodeId">The delta node identifier.</param>
        /// <returns>List of delta node reopen request.</returns>
        public async Task<IEnumerable<DeltaNodeReopenResponse>> GetDeltaNodesForReopenAsync(int deltaNodeId)
        {
            var repository = this.CreateRepository<DeltaNodeReopenResponse>();
            var parameters = new Dictionary<string, object>
            {
                    { "@DeltaNodeID", deltaNodeId },
            };
            return await repository.ExecuteQueryAsync(Repositories.Constants.GetDependentsOfficialNodeDeltaProcedureName, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Reopens the delta nodes asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The task.</returns>
        public async Task ReopenDeltaNodesAsync(DeltaNodeReopenRequest request)
        {
            ArgumentValidators.ThrowIfNull(request, nameof(request));
            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var repository = unitOfWork.CreateRepository<DeltaNode>();
                foreach (int id in request.DeltaNodeId)
                {
                    var deltaNode = await repository.GetByIdAsync(id).ConfigureAwait(false);
                    if (deltaNode != null && deltaNode.Status == OwnershipNodeStatusType.APPROVED)
                    {
                        deltaNode.Status = OwnershipNodeStatusType.REOPENED;
                        deltaNode.Comment = request.Comment;
                        repository.Update(deltaNode);
                        await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
                    }
                }
            }
        }

        /// <summary>
        /// Validates the official delta node asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="repository">The repository.</param>
        /// <returns>the task.</returns>
        private static async Task<bool> ValidateOfficialDeltaNodeAsync(DeltaNodeStatusRequest request, IRepository<DeltaNode> repository)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@SegmentID", request.SegmentId },
                    { "@NodeID", request.NodeId },
                    { "@DeltaNodeID", request.DeltaNodeId },
                };
                await repository.ExecuteAsync(Repositories.Constants.ApproveOfficialNodeDeltaProcedureName, parameters).ConfigureAwait(false);
                return true;
            }
            catch (SqlException ex)
            {
                if (ex.Message == Constants.ApproveOfficialNodeDeltaFail)
                {
                    return false;
                }

                throw;
            }
        }

        /// <summary>
        /// Gets the ownership parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        private static void GetOwnershipParameters(IDictionary<string, object> parameters)
        {
            parameters.Add("@UncertaintyPercentage", DBNull.Value);
            parameters.Add("@OwnerId", DBNull.Value);
            parameters.Add("@NodeId", DBNull.Value);
            parameters.Add("@TicketGroupId", Guid.NewGuid().ToString());
            parameters.Add("@SessionId", DBNull.Value);
        }

        /// <summary>
        /// Gets the logistics parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="ownerId">The owner identifier.</param>
        private static void GetLogisticsParameters(IDictionary<string, object> parameters, int ownerId, int? nodeId)
        {
            parameters.Add("@UncertaintyPercentage", DBNull.Value);
            parameters.Add("@OwnerId", ownerId);
            parameters.Add("@NodeId", nodeId);
            parameters.Add("@TicketGroupId", DBNull.Value);
            parameters.Add("@SessionId", DBNull.Value);
        }

        /// <summary>
        /// Gets the Delta parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        private static void GetDeltaParameters(IDictionary<string, object> parameters)
        {
            parameters.Add("@UncertaintyPercentage", DBNull.Value);
            parameters.Add("@OwnerId", DBNull.Value);
            parameters.Add("@NodeId", DBNull.Value);
            parameters.Add("@TicketGroupId", DBNull.Value);
            parameters.Add("@SessionId", DBNull.Value);
        }

        /// <summary>
        /// Validates the cutoff ticket asynchronous.
        /// </summary>
        /// <param name="operationalCutOff">The operational cut off.</param>
        /// <returns>The task.</returns>
        private async Task ValidateCutoffTicketAsync(OperationalCutOff operationalCutOff)
        {
            if (operationalCutOff.Ticket.TicketTypeId == TicketType.Cutoff)
            {
                var cutOffTicket = await this.ExistsTicketAsync(operationalCutOff.Ticket).ConfigureAwait(false);
                if (cutOffTicket)
                {
                    throw new InvalidDataException(EntitiesConstants.CutoffAlreadyRunning);
                }

                var deltaTicket = await this.ExistsDeltaTicketAsync(operationalCutOff.Ticket.CategoryElementId).ConfigureAwait(false);

                if (deltaTicket)
                {
                    throw new InvalidDataException(EntitiesConstants.DeltaAlreadyRunning);
                }
            }
        }

        /// <summary>
        /// Exists the approver delta node asynchronous.
        /// </summary>
        /// <param name="nodeId">The  node identifier.</param>
        /// <returns>The task.</returns>
        private async Task<bool> ExistDeltaNodeApproverAsync(int nodeId)
        {
            var approverConfig = await this.CreateRepository<DeltaNodeApproval>().FirstOrDefaultAsync(a => a.Level == 1 && a.NodeId == nodeId).ConfigureAwait(false);
            return approverConfig != null && !string.IsNullOrWhiteSpace(approverConfig.Approvers);
        }

        /// <summary>
        /// Pushes the message to queue asynchronous.
        /// </summary>
        /// <param name="ticketResults">The ticket results.</param>
        /// <param name="ticketType">Type of the ticket.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="scenarioType">The scenario Type.</param>
        private async Task PushMessageToQueueAsync(IEnumerable<SaveTicketResult> ticketResults, TicketType ticketType, string sessionId, ScenarioType? scenarioType)
        {
            try
            {
                IServiceBusQueueClient client;
                if (ticketType == TicketType.Logistics)
                {
                    client = this.azureClientFactory.GetQueueClient(
                    QueueConstants.LogisticsQueue);
                    await client.QueueMessageAsync(
                        new QueueMessage
                        {
                            TicketId = ticketResults.FirstOrDefault().TicketId,
                            CorrelationId = Guid.NewGuid().ToString(),
                            SystemTypeId = (int)SystemType.SIV,
                        }).ConfigureAwait(false);
                    return;
                }

                if (ticketType == TicketType.OfficialLogistics)
                {
                    client = this.azureClientFactory.GetQueueClient(QueueConstants.OfficialLogisticsQueue);
                    await client.QueueSessionMessageAsync(
                        new QueueMessage
                        {
                            TicketId = ticketResults.FirstOrDefault().TicketId,
                            CorrelationId = Guid.NewGuid().ToString(),
                            SystemTypeId = (int)SystemType.SIV,
                        }, sessionId).ConfigureAwait(false);
                    return;
                }

                if (ticketType == TicketType.LogisticMovements)
                {
                    if (scenarioType == ScenarioType.OPERATIONAL)
                    {
                        client = this.azureClientFactory.GetQueueClient(
                        QueueConstants.LogisticsQueue);
                        await client.QueueMessageAsync(
                            new QueueMessage
                            {
                                TicketId = ticketResults.FirstOrDefault().TicketId,
                                CorrelationId = Guid.NewGuid().ToString(),
                                SystemTypeId = (int)SystemType.SAP,
                            }).ConfigureAwait(false);
                        return;
                    }
                    else
                    {
                        client = this.azureClientFactory.GetQueueClient(QueueConstants.OfficialLogisticsQueue);
                        await client.QueueSessionMessageAsync(
                            new QueueMessage
                            {
                                TicketId = ticketResults.FirstOrDefault().TicketId,
                                CorrelationId = Guid.NewGuid().ToString(),
                                SystemTypeId = (int)SystemType.SAP,
                            }, sessionId).ConfigureAwait(false);
                        return;
                    }
                }

                if (ticketType == TicketType.Delta)
                {
                    client = this.azureClientFactory.GetQueueClient(
                    QueueConstants.DeltaQueue);
                    await client.QueueSessionMessageAsync(ticketResults.FirstOrDefault().TicketId, sessionId).ConfigureAwait(false);
                    return;
                }

                if (ticketType == TicketType.OfficialDelta)
                {
                    client = this.azureClientFactory.GetQueueClient(QueueConstants.ConsolidationQueue);
                    var tasks = new List<Task>();
                    tasks.AddRange(ticketResults.Select(t => this.SendOfficialDeltaMessagesAsync(client, t.TicketId, t.CategoryElementId.ToString())));
                    await Task.WhenAll(tasks).ConfigureAwait(false);
                    return;
                }

                client = this.azureClientFactory.GetQueueClient(
                    ticketType == TicketType.Cutoff ? QueueConstants.OperationalCutoffQueue : QueueConstants.OwnershipQueue);
                await client.QueueSessionMessageAsync(ticketResults.FirstOrDefault().TicketId, sessionId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var ticketId = ticketResults.FirstOrDefault().TicketId;
                var error = ex.InnerException ?? ex;
                this.logger.LogError(error, Constants.TechnicalExceptionErrorMessage, ticketId);
                await this.HandleTicketFailureAsync(ticketId, ticketType).ConfigureAwait(false);
            }
        }

        private async Task SendOfficialDeltaMessagesAsync(IServiceBusQueueClient client, int ticketId, string categoryId)
        {
            try
            {
                await client.QueueSessionMessageAsync(ticketId, categoryId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var error = ex.InnerException ?? ex;
                this.logger.LogError(error, Constants.TechnicalExceptionErrorMessage, ticketId);

                await this.HandleTicketFailureAsync(ticketId, TicketType.OfficialDelta).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the cutoff parameters asynchronous.
        /// </summary>
        /// <param name="operationalCutOff">The operational cut off.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The Task.</returns>
        private async Task GetCutoffParametersAsync(OperationalCutOff operationalCutOff, IDictionary<string, object> parameters)
        {
            var systemConfig = await this.configurationHandler.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings).ConfigureAwait(false);
            parameters.Add("@UncertaintyPercentage", systemConfig.StandardUncertaintyPercentage);
            parameters.Add("@OwnerId", DBNull.Value);
            parameters.Add("@NodeId", DBNull.Value);
            parameters.Add("@TicketGroupId", DBNull.Value);
            parameters.Add("@SessionId", operationalCutOff.SessionId);
        }

        /// <summary>
        /// Updates the unbalances comment.
        /// </summary>
        /// <param name="batch">The batch.</param>
        private async Task UpdateUnbalancesCommentAsync(OperationalCutOffBatch batch, IDictionary<string, object> parameters)
        {
            var unbalanceData = batch.Unbalances.Select(x => new { x.NodeId, x.ProductId, x.Unbalance, x.Units, x.UnbalancePercentage, x.ControlLimit, x.Comment });
            parameters.Add(
                "@PendingTransactionErrorMessages",
                new List<PendingTransactionError>().Select(x => new { x.ErrorId, x.Comment }).ToDataTable(Repositories.Constants.PendingTransactionErrorsType));
            parameters.Add("@UnbalancesMessages", unbalanceData.ToDataTable(Repositories.Constants.UnbalanceType));
            parameters.Add(
                "@SapTrackingMessages",
                new List<SapTracking>().Select(x => new { x.MovementTransactionId, x.SapTrackingId, x.Comment }).ToDataTable(Repositories.Constants.SapTrackingType));
            var repository = this.CreateRepository<UpdateCutOffComment>();
            await repository.ExecuteQueryAsync(Repositories.Constants.UpdateCutOffCommentProcedureName, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the transfer points comment asynchronous.
        /// </summary>
        /// <param name="batch">The batch.</param>
        private async Task UpdateTransferPointsCommentAsync(OperationalCutOffBatch batch, IDictionary<string, object> parameters)
        {
            var transferPoints = batch.TransferPoints.Select(x => new { x.SapTrackingId, x.MovementTransactionId, x.Comment });
            parameters.Add(
                "@PendingTransactionErrorMessages",
                new List<PendingTransactionError>().Select(x => new { x.ErrorId, x.Comment }).ToDataTable(Repositories.Constants.PendingTransactionErrorsType));
            parameters.Add(
                "@UnbalancesMessages",
                new List<UnbalanceComment>().Select(x => new { x.NodeId, x.ProductId, x.Unbalance, x.Units, x.UnbalancePercentage, x.ControlLimit, x.Comment, })
                .ToDataTable(Repositories.Constants.UnbalanceType));
            parameters.Add("@SapTrackingMessages", transferPoints.ToDataTable(Repositories.Constants.SapTrackingType));
            var repository = this.CreateRepository<UpdateCutOffComment>();
            await repository.ExecuteQueryAsync(Repositories.Constants.UpdateCutOffCommentProcedureName, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the errors comment asynchronous.
        /// </summary>
        /// <param name="batch">The batch.</param>
        private async Task UpdateErrorsCommentAsync(OperationalCutOffBatch batch, IDictionary<string, object> parameters)
        {
            var pendingTransactionErrors = batch.Errors.Select(x => new { x.ErrorId, x.Comment });
            parameters.Add("@PendingTransactionErrorMessages", pendingTransactionErrors.ToDataTable(Repositories.Constants.PendingTransactionErrorsType));
            parameters.Add(
                "@UnbalancesMessages",
                new List<UnbalanceComment>().Select(x => new { x.NodeId, x.ProductId, x.Unbalance, x.Units, x.UnbalancePercentage, x.ControlLimit, x.Comment, })
                .ToDataTable(Repositories.Constants.UnbalanceType));
            parameters.Add(
                "@SapTrackingMessages",
                new List<SapTracking>().Select(x => new { x.MovementTransactionId, x.SapTrackingId, x.Comment }).ToDataTable(Repositories.Constants.SapTrackingType));
            var repository = this.CreateRepository<UpdateCutOffComment>();
            await repository.ExecuteQueryAsync(Repositories.Constants.UpdateCutOffCommentProcedureName, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Checks the session exists for session.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="segmentId">The segment identifier.</param>
        /// <returns>If operation is valid or not.</returns>
        private async Task CheckSessionExistsForSegmentAsync(string sessionId, string segmentId)
        {
            var existing = await this.cacheHandler.GetAsync(segmentId, Repositories.Constants.CacheRegionName).ConfigureAwait(false);
            var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2) };
            if (string.IsNullOrWhiteSpace(existing))
            {
                await this.cacheHandler.SetAsync(segmentId, sessionId, Repositories.Constants.CacheRegionName, options).ConfigureAwait(false);
                var existingAfterSet = await this.cacheHandler.GetAsync(segmentId, Repositories.Constants.CacheRegionName).ConfigureAwait(false);
                if (existingAfterSet != sessionId)
                {
                    throw new InvalidDataException(EntitiesConstants.CutoffAlreadyRunning);
                }
            }
            else if (existing == sessionId)
            {
                await this.cacheHandler.SetAsync(segmentId, sessionId, Repositories.Constants.CacheRegionName, options).ConfigureAwait(false);
            }
            else
            {
                throw new InvalidDataException(EntitiesConstants.CutoffAlreadyRunning);
            }
        }

        private async Task HandleTicketFailureAsync(int ticketId, TicketType ticketType)
        {
            var failureHandler = this.failureHandlerFactory.GetFailureHandler(ticketType);
            if (failureHandler != null)
            {
                using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
                {
                    await failureHandler.HandleFailureAsync(unitOfWork, new FailureInfo(ticketId, Constants.TechnicalExceptionErrorMessage)).ConfigureAwait(false);
                }
            }
        }
    }
}
