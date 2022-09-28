// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApprovalProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Processors.Approval
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Approval.Interfaces;
    using Ecp.True.Processors.Core;
    using Ecp.True.Proxies.Azure;
    using EfCore.Models;
    using EntityConstants = Ecp.True.Entities.Constants;

    /// <summary>
    /// The Approval Processor.
    /// </summary>
    public class ApprovalProcessor : ProcessorBase, IApprovalProcessor
    {
        /// <summary>
        /// The repository factory.
        /// </summary>
        private readonly IRepositoryFactory repositoryFactory;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// The business context.
        /// </summary>
        private readonly IBusinessContext businessContext;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<ApprovalProcessor> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApprovalProcessor" /> class.
        /// </summary>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <param name="unitOfWorkFactory">The approval service.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="saveOperativeMovements">The save Operative Movements.</param>
        /// <param name="businessContext">The business context.</param>
        /// <param name="logger">The logger.</param>
        public ApprovalProcessor(
            IRepositoryFactory repositoryFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            IAzureClientFactory azureClientFactory,
            IConfigurationHandler configurationHandler,
            IBusinessContext businessContext,
            ITrueLogger<ApprovalProcessor> logger)
            : base(repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.azureClientFactory = azureClientFactory;
            this.configurationHandler = configurationHandler;
            this.businessContext = businessContext;
            this.logger = logger;
        }

        /// <summary>Updates the ownership node status.</summary>
        /// <param name="approvalRequest">The approval request.</param>
        /// <returns>Return status.</returns>
        public async Task<List<ErrorInfo>> UpdateOwnershipNodeStatusAsync(NodeOwnershipApprovalRequest approvalRequest)
        {
            ArgumentValidators.ThrowIfNull(approvalRequest, nameof(approvalRequest));
            var errorsList = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            Validator.TryValidateObject(approvalRequest, new ValidationContext(approvalRequest, null, null), errorsList, true);

            if (errorsList.Any())
            {
                return errorsList.Select(e => new ErrorInfo(e.ErrorMessage)).ToList();
            }

            var ownershipNodeRepository = this.repositoryFactory.CreateRepository<OwnershipNode>();
            var ownershipNode = await ownershipNodeRepository.GetByIdAsync(approvalRequest.OwnershipNodeId).ConfigureAwait(false);

            if (ownershipNode == null)
            {
                return new List<ErrorInfo> { new ErrorInfo(EntityConstants.OwnershipNodeNotFound) };
            }

            if (!ownershipNode.OwnershipStatus.Equals(OwnershipNodeStatusType.SUBMITFORAPPROVAL))
            {
                return new List<ErrorInfo> { new ErrorInfo(EntityConstants.InvalidNodeStateApproval) };
            }

            errorsList.Add(new System.ComponentModel.DataAnnotations.
                ValidationResult(await this.UpdateOwnershipNodeDetailsAsync(ownershipNode.OwnershipNodeId, approvalRequest).ConfigureAwait(false)));
            return errorsList.Where(e => !string.IsNullOrEmpty(e.ErrorMessage)).Select(e => new ErrorInfo(e.ErrorMessage)).ToList();
        }

        /// <summary>
        /// Gets the ownership node balance summary asynchronous.
        /// </summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <returns>
        /// Ownership node balance summary collection.
        /// </returns>
        public async Task<OwnershipNodeBalanceDetails> GetOwnershipNodeBalanceDetailsAsync(int ownershipNodeId)
        {
            var ownershipNode = await this.repositoryFactory.CreateRepository<OwnershipNode>().FirstOrDefaultAsync(n => n.OwnershipNodeId == ownershipNodeId, "Ticket", "Node").ConfigureAwait(false);
            if (ownershipNode == null)
            {
                throw new KeyNotFoundException();
            }

            var segment = await this.repositoryFactory.CreateRepository<NodeTag>().FirstOrDefaultAsync(
            x => x.NodeId == ownershipNode.NodeId
            && x.CategoryElement.CategoryId == 2
            && DateTime.UtcNow.ToTrue() >= x.StartDate
            && DateTime.UtcNow.ToTrue() <= x.EndDate, "CategoryElement").ConfigureAwait(false);

            var balanceProfessionalEmail = string.Empty;
            var balanceProfessionalUserName = string.Empty;
            if (!string.IsNullOrWhiteSpace(ownershipNode.Editor))
            {
                var editor = await this.repositoryFactory.CreateRepository<User>().FirstOrDefaultAsync(x => x.Email == ownershipNode.Editor).ConfigureAwait(false);
                balanceProfessionalEmail = editor.Email;
                balanceProfessionalUserName = editor.Name;
            }

            var systemConfig = await this.configurationHandler.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings).ConfigureAwait(false);

            var details = new OwnershipNodeBalanceDetails
            {
                Summary = await this.GetOwnershipNodeBalanceSummaryAsync(ownershipNodeId).ConfigureAwait(false),
                BalanceProfessionalUserName = balanceProfessionalUserName,
                BalanceProfessionalEmail = balanceProfessionalEmail,
                NodeName = ownershipNode.Node.Name,
                Segment = segment != null ? segment.CategoryElement.Name : string.Empty,
                StartDate = ownershipNode.Ticket.StartDate,
                TicketId = ownershipNode.Ticket.TicketId,
                ReportPath = $"{systemConfig.BasePath}/cutoffreport/manage/{ownershipNodeId}",
            };

            return details;
        }

        /// <summary>
        /// Gets the ownership node balance summary asynchronous.
        /// </summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <returns>The ownership nodes query.</returns>
        public async Task SendOwnershipNodeIdForApprovalAsync(int ownershipNodeId)
        {
            ArgumentValidators.ThrowIfNull(ownershipNodeId, nameof(ownershipNodeId));

            var parameters = new Dictionary<string, object>
            {
               { "@OwnershipNodeId", ownershipNodeId },
            };
            var repository = this.repositoryFactory.CreateRepository<BalanceSummaryAggregate>();
            var balanceSummaryAggregate = await repository.ExecuteQueryAsync(Repositories.Constants.BalanceSummaryAggregate, parameters).ConfigureAwait(false);

            var aggregate = balanceSummaryAggregate.FirstOrDefault();
            if (aggregate?.Volume == 0 && (aggregate.OwnershipStatusId == OwnershipNodeStatusType.OWNERSHIP ||
                    aggregate.OwnershipStatusId == OwnershipNodeStatusType.UNLOCKED || CheckOwnershipNodeStatus(aggregate.OwnershipStatusId)))
            {
                await this.UpdateOwnershipNodeStatusToSubmitToApprovedAsync(ownershipNodeId).ConfigureAwait(false);
                await this.SendMessageToServiceBusApprovalQueueAsync(ownershipNodeId).ConfigureAwait(false);
                return;
            }

            throw new InvalidDataException(Entities.Constants.OwnershipNodeForApprovalFailed);
        }

        /// <summary>
        /// Updates the ownership node state asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        /// <param name="approvalRequest">The approval request.</param>
        public async Task UpdateOwnershipNodeStateAsync(NodeOwnershipApprovalRequest approvalRequest)
        {
            ArgumentValidators.ThrowIfNull(approvalRequest, nameof(approvalRequest));

            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var repository = unitOfWork.CreateRepository<OwnershipNode>();
                var ownershipNode = await repository.GetByIdAsync(approvalRequest.OwnershipNodeId).ConfigureAwait(false);
                ownershipNode.OwnershipStatus = (OwnershipNodeStatusType?)Enum.Parse(typeof(OwnershipNodeStatusType), approvalRequest.Status.ToUpper(CultureInfo.InvariantCulture));
                ownershipNode.Comment = approvalRequest.Comment;
                ownershipNode.ApproverAlias = approvalRequest.ApproverAlias;
                ownershipNode.RegistrationDate = DateTime.UtcNow.ToTrue();
                repository.Update(ownershipNode);

                await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
                if (ownershipNode.OwnershipStatus == OwnershipNodeStatusType.APPROVED)
                {
                    await this.SaveOperativeMovementsAsync(approvalRequest.OwnershipNodeId).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Saves the operative movements with ownership percentage asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        public async Task SaveOperativeMovementsAsync(int ownershipNodeId)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@OwnershipNodeId", ownershipNodeId },
                };

                using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
                var repository = unitOfWork.CreateRepository<OwnershipNode>();
                await repository.ExecuteAsync(Repositories.Constants.SaveOperativeMovements, parameters).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, Constants.AnalyticalProcessInvokeFail);
            }
        }

        /// <summary>
        /// Check Ownership Node Status.
        /// </summary>
        /// <param name="ownershipNodeStatusType">The ownership Node Status Type.</param>
        /// <returns>the status.</returns>
        private static bool CheckOwnershipNodeStatus(OwnershipNodeStatusType? ownershipNodeStatusType)
        {
            return ownershipNodeStatusType.HasValue &&
                (ownershipNodeStatusType == OwnershipNodeStatusType.PUBLISHED ||
                ownershipNodeStatusType == OwnershipNodeStatusType.REJECTED ||
                ownershipNodeStatusType == OwnershipNodeStatusType.REOPENED ||
                ownershipNodeStatusType == OwnershipNodeStatusType.RECONCILED ||
                ownershipNodeStatusType == OwnershipNodeStatusType.NOTRECONCILED);
        }

        /// <summary>
        /// Sends the message to service bus queue asynchronous.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The task.</returns>
        private async Task SendMessageToServiceBusApprovalQueueAsync(object data)
        {
            var approvalQueueClient = this.azureClientFactory.GetQueueClient(QueueConstants.ApprovalQueue);
            await approvalQueueClient.QueueMessageAsync(data).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the ownership node balance summary asynchronous.
        /// </summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <returns>The collection of ownership node balance summary.</returns>
        private async Task<IEnumerable<OwnershipNodeBalanceSummary>> GetOwnershipNodeBalanceSummaryAsync(int ownershipNodeId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@OwnershipNodeId", ownershipNodeId },
            };

            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var repository = unitOfWork.CreateRepository<OwnershipNodeBalanceSummary>();
                return await repository.ExecuteQueryAsync(Repositories.Constants.GetOwnershipNodeBalanceSummaryProcedureName, parameters).ConfigureAwait(false);
            }
        }

        /// <summary>Updates the ownership node status asynchronous.</summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <param name="approvalRequest">The approval request.</param>
        /// <returns>The status of Update.</returns>
        private async Task<string> UpdateOwnershipNodeDetailsAsync(int ownershipNodeId, NodeOwnershipApprovalRequest approvalRequest)
        {
            ArgumentValidators.ThrowIfNull(approvalRequest, nameof(approvalRequest));

            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var repository = unitOfWork.CreateRepository<OwnershipNode>();
                var ownershipNode = await repository.GetByIdAsync(ownershipNodeId).ConfigureAwait(false);

                if (approvalRequest.Status == "APPROVED")
                {
                    ownershipNode.OwnershipStatus = (OwnershipNodeStatusType?)OwnershipNodeStatusType.APPROVED;
                }
                else if (approvalRequest.Status == "REJECTED")
                {
                    ownershipNode.OwnershipStatus = (OwnershipNodeStatusType?)OwnershipNodeStatusType.REJECTED;
                }
                else
                {
                    return EntityConstants.InvalidRequestStatus;
                }

                ownershipNode.Comment = approvalRequest.Comment;
                ownershipNode.ApproverAlias = approvalRequest.ApproverAlias;
                ownershipNode.RegistrationDate = DateTime.UtcNow.ToTrue();
                repository.Update(ownershipNode);

                int checkUpdate = await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
                if (checkUpdate == 0)
                {
                    return EntityConstants.OwnershipNodeUpdateFailure;
                }

                return null;
            }
        }

        /// <summary>Updates the ownership node status asynchronous.</summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <returns>The status of Update.</returns>
        private async Task UpdateOwnershipNodeStatusToSubmitToApprovedAsync(int ownershipNodeId)
        {
            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var ownershipNoderepository = unitOfWork.CreateRepository<OwnershipNode>();
                var ownershipNode = await ownershipNoderepository.SingleOrDefaultAsync(
                a => a.OwnershipNodeId == ownershipNodeId).ConfigureAwait(false);

                ownershipNode.OwnershipStatus = OwnershipNodeStatusType.SUBMITFORAPPROVAL;
                ownershipNode.Editor = this.businessContext.Email;

                ownershipNoderepository.Update(ownershipNode);
                await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }
    }
}