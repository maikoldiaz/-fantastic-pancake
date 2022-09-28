// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FinalizerBase.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// Finalizer base.
    /// </summary>
    public abstract class FinalizerBase : IFinalizer
    {
        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IDictionary<TicketType, IEnumerable<string>> storedProcConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="FinalizerBase" /> class.
        /// </summary>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        protected FinalizerBase(
            IAzureClientFactory azureClientFactory,
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));

            this.UnitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.azureClientFactory = azureClientFactory;
            this.storedProcConfig = new Dictionary<TicketType, IEnumerable<string>>
            {
            {
                TicketType.Cutoff, new List<string>
            {
                Repositories.Constants.SaveAttributeDetails,
                Repositories.Constants.SaveBackupMovementDetails,
                Repositories.Constants.SaveMovementDetails,
                Repositories.Constants.SaveInventoryDetails,
                Repositories.Constants.SaveKPIDataByCategoryElementNode,
                Repositories.Constants.SaveMovementsByProduct,
                Repositories.Constants.SaveQualityDetails,
                Repositories.Constants.SaveBalanceControl,
            }
            },
            {
                TicketType.Ownership, new List<string>
            {
                Repositories.Constants.SaveAttributeDetailsWithOwner,
                Repositories.Constants.SaveBackupMovementDetailsWithOwner,
                Repositories.Constants.SaveMovementDetailsWithOwner,
                Repositories.Constants.SaveMovementDetailsWithOwnerOtherSegment,
                Repositories.Constants.SaveInventoryDetailsWithOwner,
                Repositories.Constants.SaveKPIDataByCategoryElementNodeWithOwnership,
                Repositories.Constants.SaveMovementsByProductWithOwner,
                Repositories.Constants.SaveQualityDetailsWithOwner,
            }
            },
            {
                TicketType.OfficialDelta, new List<string>
            {
                Repositories.Constants.SaveOfficialDeltaBalance,
                Repositories.Constants.SaveOfficialDeltaMovementDetails,
                Repositories.Constants.SaveOfficialDeltaInventoryDetails,
            }
            },
            };
        }

        /// <summary>
        /// Gets the type of the ticket.
        /// </summary>
        /// <value>
        /// The type of the ticket.
        /// </value>
        public abstract TicketType Type { get; }

        /// <summary>
        /// Gets the type of the type.
        /// </summary>
        /// <value>
        /// The type of the finalizer.
        /// </value>
        public abstract FinalizerType Finalizer { get; }

        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        /// <value>
        /// The unit of work.
        /// </value>
        protected IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Gets the stored procedures.
        /// </summary>
        /// <returns> The stored procedures. </returns>
        protected IEnumerable<string> StoredProceduresPerType => this.storedProcConfig[this.Type];

        /// <summary>
        /// Processes the asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public virtual Task ProcessAsync(int ticketId)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Processes the asynchronous.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public virtual Task ProcessAsync(object data)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Sends to queue asynchronous.
        /// </summary>
        /// <param name="entityIds">The message.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns> The task. </returns>
        protected async Task SendSessionMessageToQueueAsync(IEnumerable<int> entityIds, string queueName)
        {
            ArgumentValidators.ThrowIfNull(entityIds, nameof(entityIds));

            var queueClient = this.azureClientFactory.GetQueueClient(queueName);
            foreach (var id in entityIds)
            {
                await queueClient.QueueSessionMessageAsync(id, id.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Sends the session message to queue asynchronous.
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns> The task. </returns>
        protected async Task SendSessionMessageToQueueAsync(IEnumerable<SessionMessage> messages, string queueName)
        {
            ArgumentValidators.ThrowIfNull(messages, nameof(messages));

            var queueClient = this.azureClientFactory.GetQueueClient(queueName);
            foreach (var message in messages)
            {
                await queueClient.QueueSessionMessageAsync(message.Id, message.SessionId).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Sends the movement transaction ids to queue.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns> The task.</returns>
        protected async Task SendMovementTransactionIdsToQueueAsync(int ticketId, int? nodeId)
        {
            var movements = await this.UnitOfWork.CreateRepository<Movement>().GetAllAsync(x => (x.TicketId == ticketId || x.OwnershipTicketId == ticketId)
            && x.IsSystemGenerated == true && x.BlockchainStatus == StatusType.PROCESSING
            && (nodeId == null || (x.MovementSource.SourceNodeId == nodeId || x.MovementDestination.DestinationNodeId == nodeId))).ConfigureAwait(false);
            var movementTransactionIds = movements.OrderBy(x => x.MovementTransactionId).Select(x => x.MovementTransactionId);
            await this.SendSessionMessageToQueueAsync(movementTransactionIds, QueueConstants.BlockchainMovementQueue).ConfigureAwait(false);
        }

        /// <summary>
        /// Builds the offchain node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        /// The offchain node.
        /// </returns>
        protected OffchainNode BuildOffchainNode(Node node, NodeState state)
        {
            ArgumentValidators.ThrowIfNull(node, nameof(node));
            return new OffchainNode
            {
                NodeId = node.NodeId,
                NodeStateTypeId = (int)state,
                IsActive = node.IsActive.GetValueOrDefault(),
                LastUpdateDate = DateTime.UtcNow.ToTrue(),
                Name = node.Name,
                BlockchainStatus = StatusType.PROCESSING,
            };
        }

        /// <summary>
        /// Publishes the offchain nodes asynchronous.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <param name="state">The state.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async Task PublishOffchainNodesAsync(IEnumerable<Node> nodes, NodeState state)
        {
            var offchainNodes = nodes.Distinct(ExpressionEqualityComparer.Create<Node, int>(n => n.NodeId)).Select(x => this.BuildOffchainNode(x, state)).ToArray();

            var repo = this.UnitOfWork.CreateRepository<OffchainNode>();
            repo.InsertAll(offchainNodes);

            await this.UnitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);

            var messages = offchainNodes.Select(n => new SessionMessage(n.Id, n.NodeId.ToString(CultureInfo.InvariantCulture)));
            await this.SendSessionMessageToQueueAsync(messages, QueueConstants.BlockchainNodeQueue).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the ticket by identifier asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns> The task.</returns>
        protected async Task<Ticket> GetTicketByIdAsync(int ticketId)
        {
            var ticketRepository = this.UnitOfWork.CreateRepository<Ticket>();
            return await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);
        }
    }
}
