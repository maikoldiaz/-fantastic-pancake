// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeOwnershipProcessor.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
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
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Core;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.OwnershipRules.Interfaces;
    using EntityConstants = Ecp.True.Entities.Constants;

    /// <summary>
    /// The Node Ownership processor.
    /// </summary>
    public class NodeOwnershipProcessor : ProcessorBase, INodeOwnershipProcessor
    {
        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IOwnershipRuleProxy ownershipRuleService;

        /// <summary>
        /// The unit of work factory.
        /// </summary>

        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The business context.
        /// </summary>
        private readonly IBusinessContext businessContext;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly ICommunicator communicator;

        /// <summary>
        /// The configuration handler..
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>Initializes a new instance of the <see cref="NodeOwnershipProcessor"/> class.</summary>
        /// <param name="factory">The factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="businessContext">The business context.</param>
        /// <param name="ownershipRuleService">The ownership rule service.</param>
        /// <param name="communicator">The communicator.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        public NodeOwnershipProcessor(
            IRepositoryFactory factory,
            IUnitOfWorkFactory unitOfWorkFactory,
            IAzureClientFactory azureClientFactory,
            IBusinessContext businessContext,
            IOwnershipRuleProxy ownershipRuleService,
            ICommunicator communicator,
            IConfigurationHandler configurationHandler)
            : base(factory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.azureClientFactory = azureClientFactory;
            this.businessContext = businessContext;
            this.ownershipRuleService = ownershipRuleService;
            this.communicator = communicator;
            this.configurationHandler = configurationHandler;
        }

        /// <summary>
        /// Gets the owners for movement.
        /// </summary>
        /// <param name="sourceNodeId">The source node identifier.</param>
        /// <param name="destinationNodeId">The destination node identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The list of owners.</returns>
        public Task<IEnumerable<NodeConnectionProductOwner>> GetOwnersForMovementAsync(int sourceNodeId, int destinationNodeId, string productId)
        {
            return this.RepositoryFactory.NodeOwnershipRepository.GetOwnersForMovementAsync(sourceNodeId, destinationNodeId, productId);
        }

        /// <summary>Reopens the ownership node asynchronous.</summary>
        /// <param name="reopenTicket">The reopen ticket object.</param>
        /// <returns>The task.</returns>
        /// <inheritdoc/>
        public async Task ReopenOwnershipNodeAsync(ReopenTicket reopenTicket)
        {
            ArgumentValidators.ThrowIfNull(reopenTicket, nameof(reopenTicket));
            string validState = OwnershipNodeStatusType.APPROVED.ToString();
            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var repository = unitOfWork.CreateRepository<OwnershipNode>();
                var ownershipNode = await repository.GetByIdAsync(reopenTicket.OwnershipNodeId).ConfigureAwait(false);
                if (ownershipNode == null || reopenTicket.Status != validState)
                {
                    throw new KeyNotFoundException(EntityConstants.OwnershipNodeNotFound);
                }

                ownershipNode.Comment = reopenTicket.Message;
                ownershipNode.OwnershipStatus = OwnershipNodeStatusType.REOPENED;
                repository.Update(ownershipNode);

                await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the ownership node identifier asynchronous.
        /// </summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <returns>The ownership nodes query.</returns>
        public Task<OwnershipNode> GetOwnershipNodeIdAsync(int ownershipNodeId)
        {
            return this.CreateRepository<OwnershipNode>().SingleOrDefaultAsync(
                a => a.OwnershipNodeId == ownershipNodeId,
                "Ticket",
                "Ticket.CategoryElement",
                "Ticket.CategoryElement.Category",
                "Node");
        }

        /// <summary>
        /// Blocks or Unblocks the ownership node asynchronous.
        /// </summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <param name="ownershipNode">The ownership node.</param>
        /// <returns>Returns the status of update.</returns>
        public async Task UpdateOwnershipNodeStatusAsync(int ownershipNodeId, OwnershipNode ownershipNode)
        {
            ArgumentValidators.ThrowIfNull(ownershipNode, nameof(ownershipNode));

            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var repository = unitOfWork.CreateRepository<OwnershipNode>();
                var existingOwnershipNode = await repository.SingleOrDefaultAsync(p => p.OwnershipNodeId == ownershipNodeId).ConfigureAwait(false);
                if (existingOwnershipNode == null)
                {
                    throw new KeyNotFoundException(EntityConstants.OwnershipNodeNotFound);
                }

                existingOwnershipNode.OwnershipStatus = ownershipNode.OwnershipStatus;
                existingOwnershipNode.LastModifiedDate = DateTime.UtcNow.ToTrue();
                existingOwnershipNode.Editor = ownershipNode.Editor;
                existingOwnershipNode.EditorConnectionId = ownershipNode.EditorConnectionId;

                repository.Update(existingOwnershipNode);
                await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the owners for movement.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The list of owners.</returns>
        public Task<IEnumerable<StorageLocationProductOwner>> GetOwnersForInventoryAsync(int nodeId, string productId)
        {
            return this.RepositoryFactory.NodeOwnershipRepository.GetOwnersForInventoryAsync(nodeId, productId);
        }

        /// <summary>
        /// Gets the locked ownership node by editor and connectionId.
        /// </summary>
        /// <param name="editor">The editor.</param>
        /// <param name="connectionId">The connection identifier.</param>
        /// <returns>
        /// The OwnershipNode.
        /// </returns>
        public Task<OwnershipNode> GetLockedOwnershipNodeByEditorAndConnectionIdAsync(string editor, string connectionId)
        {
            return this.CreateRepository<OwnershipNode>().SingleOrDefaultAsync(
                a =>
                !string.IsNullOrEmpty(a.Editor) &&
                a.Editor.EqualsIgnoreCase(editor) &&
                !string.IsNullOrEmpty(a.EditorConnectionId) &&
                a.EditorConnectionId.EqualsIgnoreCase(connectionId) &&
                a.OwnershipStatus == OwnershipNodeStatusType.LOCKED);
        }

        /// <summary>
        /// Gets the conditional ownership node by identifier asynchronous.
        /// </summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <returns>
        /// The OwnershipNode.
        /// </returns>
        public async Task<OwnershipNode> GetConditionalOwnershipNodeByIdAsync(int ownershipNodeId)
        {
            var ownershipNodeRepository = this.RepositoryFactory.CreateRepository<OwnershipNode>();
            var ownershipNode = await ownershipNodeRepository.SingleOrDefaultAsync(
                a => a.OwnershipNodeId == ownershipNodeId,
                "Node",
                "Ticket",
                "Ticket.CategoryElement",
                "Ticket.CategoryElement.Category").ConfigureAwait(false);

            if (ownershipNode == null)
            {
                throw new KeyNotFoundException(Entities.Constants.OwnershipNodeDoseNotExists);
            }

            var ticketRepository = this.RepositoryFactory.CreateRepository<Ticket>();

            var lastOwnershipTickets = await ticketRepository.GetAllAsync(a =>
                a.TicketTypeId == TicketType.Ownership && a.CategoryElementId == ownershipNode.Ticket.CategoryElementId).ConfigureAwait(false);
            var lastOwnershipTicket = lastOwnershipTickets.OrderByDescending(a => a.TicketId).FirstOrDefault();
            if (
                lastOwnershipTicket != null &&
                lastOwnershipTicket.TicketId == ownershipNode.TicketId &&
                !(ownershipNode.OwnershipStatus == OwnershipNodeStatusType.SENT ||
                ownershipNode.OwnershipStatus == OwnershipNodeStatusType.FAILED ||
                ownershipNode.OwnershipStatus == OwnershipNodeStatusType.PUBLISHING))
            {
                return ownershipNode;
            }

            throw new UnauthorizedAccessException();
        }

        /// <summary>
        /// Gets the status of the refresh.
        /// </summary>
        /// <returns>
        /// Returns [true] if in progress otherwise [false].
        /// </returns>
        public async Task<bool> IsSyncInProgressAsync()
        {
            var status = await this.CreateRepository<OwnershipRuleRefreshHistory>().FirstOrDefaultAsync(x => x.Status).ConfigureAwait(false);
            return status != null;
        }

        /// <inheritdoc/>
        public async Task<StatusType> TryRefreshRulesAsync()
        {
            if (await this.IsSyncInProgressAsync().ConfigureAwait(false))
            {
                return StatusType.PROCESSING;
            }

            try
            {
                var ownershipRuleQueueClient = this.azureClientFactory.GetQueueClient(QueueConstants.OwnershipRuleQueue);
                await ownershipRuleQueueClient.QueueSessionMessageAsync(this.businessContext.Email, True.Core.Constants.OwnershipRulesSync).ConfigureAwait(false);
            }
            catch
            {
                return StatusType.FAILED;
            }

            return StatusType.PROCESSED;
        }

        /// <summary>
        /// Validates the ownership nodes.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The validation result.</returns>
        public async Task<IEnumerable<OwnershipValidationResult>> ValidateOwnershipNodesAsync(Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));

            var ownershipRuleSettings = await this.configurationHandler.GetConfigurationAsync<OwnershipRuleSettings>(ConfigurationConstants.OwnershipRuleSettings).ConfigureAwait(false);
            this.ownershipRuleService.Initialize(ownershipRuleSettings);

            var inactiveRules = await this.ownershipRuleService.GetInactiveRulesAsync().ConfigureAwait(false);

            var dt = new System.Data.DataTable(Repositories.Constants.KeyValueType);
            dt.Locale = CultureInfo.InvariantCulture;
            dt.Columns.Add("Key");
            dt.Columns.Add("Value");

            var parameters = new Dictionary<string, object>
            {
                { "@SegmentId", ticket.CategoryElementId },
                { "@StartDate", ticket.StartDate },
                { "@EndDate", ticket.EndDate },
                {
                    "@DtNodeInActiveRules",
                    inactiveRules != null && inactiveRules.NodeOwnershipRules != null ?
                        inactiveRules.NodeOwnershipRules.Select(x => new { Key = x.RuleId, Value = x.Name }).ToDataTable(Repositories.Constants.KeyValueType)
                        : dt
                },
                {
                    "@DtNodeProductInActiveRules",
                    inactiveRules != null && inactiveRules.NodeProductOwnershipRules != null ?
                        inactiveRules.NodeProductOwnershipRules.Select(x => new { Key = x.RuleId, Value = x.Name }).ToDataTable(Repositories.Constants.KeyValueType)
                        : dt
                },
                {
                    "@DtConnectionProductInActiveRules",
                    inactiveRules != null && inactiveRules.OwnershipRuleConnections != null ?
                        inactiveRules.OwnershipRuleConnections.Select(x => new { Key = x.RuleId, Value = x.Name }).ToDataTable(Repositories.Constants.KeyValueType)
                        : dt
                },
            };

            return await this.CreateRepository<OwnershipValidationResult>().ExecuteQueryAsync(Repositories.Constants.ValidateOwnershipInputs, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Publishes the node ownership.
        /// </summary>
        /// <param name="ownershipUpdates">The ownership updates.</param>
        /// <returns>
        /// Returns the Status of update.
        /// </returns>
        public async Task PublishNodeOwnershipAsync(PublishedNodeOwnership ownershipUpdates)
        {
            ArgumentValidators.ThrowIfNull(ownershipUpdates, nameof(ownershipUpdates));
            await this.communicator.RegisterOwnerShipAsync(ownershipUpdates).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the owner ship node movement inventory details asynchronous.
        /// </summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <returns>The list of Ownership Node Movement Inventory Details.</returns>
        public Task<IEnumerable<OwnershipNodeMovementInventoryDetails>> GetOwnerShipNodeMovementInventoryDetailsAsync(int ownershipNodeId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@OwnershipNodeId", ownershipNodeId },
            };

            return this.CreateRepository<OwnershipNodeMovementInventoryDetails>().ExecuteQueryAsync(Repositories.Constants.GetOwnershipNodeMovementInventoryDetailsProcedureName, parameters);
        }

        /// <summary>
        /// Gets the ownership node balance summary.
        /// </summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <returns>
        /// The list of ownership node balance summary.
        /// </returns>
        public Task<IEnumerable<OwnershipNodeBalanceSummary>> GetOwnershipNodeBalanceSummaryAsync(int ownershipNodeId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@OwnershipNodeId", ownershipNodeId },
            };

            return this.CreateRepository<OwnershipNodeBalanceSummary>().ExecuteQueryAsync(Repositories.Constants.GetOwnershipNodeBalanceSummaryProcedureName, parameters);
        }
    }
}