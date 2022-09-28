// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaProcessor.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Approval.Interfaces;
    using Ecp.True.Processors.Core;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// The Delta Processor.
    /// </summary>
    public class DeltaProcessor : ProcessorBase, IDeltaProcessor
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
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The Transformation Official Delta Node.
        /// </summary>
        private readonly ITransformationOfficialDeltaNode transformationOfficialDeltaNode;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<DeltaProcessor> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaProcessor" /> class.
        /// </summary>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <param name="unitOfWorkFactory">The approval service.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="transformationOfficialDeltaNode">The transformation official delta processor.</param>
        /// <param name="logger">The logger.</param>
        public DeltaProcessor(
            IRepositoryFactory repositoryFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            IConfigurationHandler configurationHandler,
            IAzureClientFactory azureClientFactory,
            ITransformationOfficialDeltaNode transformationOfficialDeltaNode,
            ITrueLogger<DeltaProcessor> logger)
            : base(repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.configurationHandler = configurationHandler;
            this.azureClientFactory = azureClientFactory;
            this.logger = logger;
            this.transformationOfficialDeltaNode = transformationOfficialDeltaNode;
        }

        /// <inheritdoc/>
        public async Task<DeltaNodeDetails> GetDeltaByDeltaNodeIdAsync(int deltaNodeId)
        {
            var deltaNode = await this.repositoryFactory.CreateRepository<DeltaNode>().FirstOrDefaultAsync(n => n.DeltaNodeId == deltaNodeId, "Ticket", "Node").ConfigureAwait(false);
            if (deltaNode == null)
            {
                throw new KeyNotFoundException();
            }

            var deltaNodeApproverDetails = await this.repositoryFactory.CreateRepository<DeltaNodeApproval>().FirstOrDefaultAsync(n => n.NodeId == deltaNode.NodeId && n.Level == 1)
                .ConfigureAwait(false);

            var balanceProfessionalEmail = string.Empty;
            var balanceProfessionalUserName = string.Empty;
            if (!string.IsNullOrWhiteSpace(deltaNode.Editor))
            {
                var editor = await this.repositoryFactory.CreateRepository<User>().FirstOrDefaultAsync(x => x.Email == deltaNode.Editor).ConfigureAwait(false);
                balanceProfessionalEmail = editor?.Email;
                balanceProfessionalUserName = editor?.Name;
            }

            var systemConfig = await this.configurationHandler.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings).ConfigureAwait(false);

            var details = new DeltaNodeDetails
            {
                BalanceProfessionalUserName = balanceProfessionalUserName,
                BalanceProfessionalEmail = balanceProfessionalEmail,
                ApproverMail = deltaNodeApproverDetails?.Approvers,
                NodeName = deltaNode.Node.Name,
                StartDate = deltaNode.Ticket.StartDate,
                TicketId = deltaNode.Ticket.TicketId,
                EndDate = deltaNode.Ticket.EndDate,
                ReportPath = $"{systemConfig.BasePath}/officialdeltanode/manage/{deltaNodeId}",
            };

            return details;
        }

        /// <inheritdoc/>
        public async Task UpdateDeltaApprovalStateAsync(DeltaNodeApprovalRequest approvalRequest)
        {
            ArgumentValidators.ThrowIfNull(approvalRequest, nameof(approvalRequest));
            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var repository = unitOfWork.CreateRepository<DeltaNode>();
                var deltaNode = await repository.GetByIdAsync(approvalRequest.DeltaNodeId).ConfigureAwait(false);
                if (deltaNode != null)
                {
                    deltaNode.Status = (OwnershipNodeStatusType)Enum.Parse(typeof(OwnershipNodeStatusType), approvalRequest.Status.ToUpper(CultureInfo.InvariantCulture));
                    deltaNode.Comment = approvalRequest.Comment;
                    deltaNode.Approvers = approvalRequest.ApproverAlias;
                    if (deltaNode.Status == OwnershipNodeStatusType.APPROVED)
                    {
                        deltaNode.LastApprovedDate = DateTime.UtcNow.ToTrue();
                    }

                    repository.Update(deltaNode);
                    await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
                    if (deltaNode.Status == OwnershipNodeStatusType.APPROVED)
                    {
                        var client = this.azureClientFactory.GetQueueClient(QueueConstants.BlockChainOfficialQueue);
                        await client.QueueSessionMessageAsync(approvalRequest.DeltaNodeId, approvalRequest.DeltaNodeId.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
                    }
                }
            }
        }

        /// <summary>
        /// Generate Delta Movements.
        /// </summary>
        /// <param name="deltaNodeId">The ownership node id.</param>
        /// <returns>The task.</returns>
        public async Task GenerateDeltaMovementsAsync(int deltaNodeId)
        {
            try
            {
                ArgumentValidators.ThrowIfNull(deltaNodeId, nameof(deltaNodeId));
                var deltaNodeRepository = this.CreateRepository<DeltaNode>();
                var deltaNode = await deltaNodeRepository.FirstOrDefaultAsync(a => a.DeltaNodeId == deltaNodeId, "Ticket").ConfigureAwait(false);
                ArgumentValidators.ThrowIfNull(deltaNode, nameof(deltaNode));
                var dateCutOff = await this.GetTimeCutOffAsync(deltaNode.NodeId).ConfigureAwait(false);
                var officialDeltaMovements = await this.GetOfficialDeltaMovementsAsync(deltaNode.TicketId, new List<int> { deltaNode.NodeId }).ConfigureAwait(false);

                using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
                {
                    var newOfficialDeltaMovements = GetNewOfficialDeltaMovements(officialDeltaMovements, deltaNode.TicketId);
                    if (newOfficialDeltaMovements.Any())
                    {
                        var transformMovements = this.transformationOfficialDeltaNode.ApplyTransformationOfficialDelta(newOfficialDeltaMovements, dateCutOff);
                        RegisterTransformedMovements(transformMovements, unitOfWork);
                        foreach (var movement in transformMovements)
                        {
                            movement.InsertInInventoryMovementIndex(unitOfWork, movement.MovementId, movement.EventType, movement.CreatedDate);
                            var queueClient = this.azureClientFactory.GetQueueClient(QueueConstants.BlockchainMovementQueue);
                            await queueClient.QueueSessionMessageAsync(movement.MovementTransactionId, movement.MovementTransactionId.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
                        }
                    }

                    await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message, $"deltaNodeId : {deltaNodeId}");
            }
        }

        private static IEnumerable<OfficialDeltaNodeMovement> GetNewOfficialDeltaMovements(IEnumerable<OfficialDeltaNodeMovement> officialDeltaMovements, int ticketId)
        {
            var newListId = new List<int>
            {
                    (int)MovementType.DeltaInventory,
                    (int)MovementType.OutputEvacuation,
                    (int)MovementType.InputEvacuation,
            };

            return officialDeltaMovements.Where(x => x.OfficialDeltaTicketId == ticketId && x.SourceSystemId == (int)SourceSystem.FICO
            && newListId.Any(movementTypeId => x.MovementTypeId == movementTypeId));
        }

        private static void RegisterTransformedMovements(IEnumerable<Movement> transformMovements, IUnitOfWork unitOfWork)
        {
            var movementRepository = unitOfWork.CreateRepository<Movement>();
            movementRepository.InsertAll(transformMovements);
        }

        /// <summary>
        /// Get Time Cut Off Async.
        /// </summary>
        /// <param name="nodeId">Node Id.</param>
        /// <returns>Task.</returns>
        private async Task<DateTime> GetTimeCutOffAsync(int nodeId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@NodeId", nodeId },
            };

            var ticketRepository = this.CreateRepository<DateDeltaMovement>();
            var tickets = await ticketRepository.ExecuteQueryAsync(Repositories.Constants.CalculateDateForDeltaMovements, parameters).ConfigureAwait(false);
            return tickets.FirstOrDefault().OperationDate;
        }

        /// <summary>
        /// Get Official Delta Movements.
        /// </summary>
        /// <param name="ticketId">the ticket id.</param>
        /// <param name="nodeList">the nodes.</param>
        /// <returns>task.</returns>
        private async Task<IEnumerable<OfficialDeltaNodeMovement>> GetOfficialDeltaMovementsAsync(int ticketId, IEnumerable<int> nodeList)
        {
            ArgumentValidators.ThrowIfNull(ticketId, nameof(ticketId));
            ArgumentValidators.ThrowIfNull(nodeList, nameof(nodeList));
            var movementRepository = this.CreateRepository<OfficialDeltaNodeMovement>();

            var parameters = new Dictionary<string, object>
                {
                    { "@TicketId", ticketId },
                    { "@NodeList", nodeList.ToDataTable(Repositories.Constants.NodeIdColumnName, Repositories.Constants.NodeListType) },
                };
            return await movementRepository.ExecuteQueryAsync(Repositories.Constants.GetDeltaOfficialMovementNodes, parameters).ConfigureAwait(false);
        }
    }
}
