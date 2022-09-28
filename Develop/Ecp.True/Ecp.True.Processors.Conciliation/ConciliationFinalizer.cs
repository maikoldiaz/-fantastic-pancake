// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConciliationFinalizer.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Processors.Conciliation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Conciliation.Entities;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// Ownership finalizer.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.Interfaces.IFinalizer" />
    public class ConciliationFinalizer : FinalizerBase
    {
        /// <summary>
        /// The client.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConciliationFinalizer" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        public ConciliationFinalizer(
            IUnitOfWorkFactory unitOfWorkFactory,
            IAzureClientFactory azureClientFactory)
            : base(azureClientFactory, unitOfWorkFactory)
        {
            this.azureClientFactory = azureClientFactory;
        }

        /// <summary>
        /// Gets the type of the ticket.
        /// </summary>
        /// <value>
        /// The type of the ticket.
        /// </value>
        public override TicketType Type => TicketType.Ownership;

        /// <summary>
        /// Gets the type of the ticket.
        /// </summary>
        /// <value>
        /// The type of the ticket.
        /// </value>
        public override FinalizerType Finalizer => FinalizerType.Conciliation;

        /// <summary>
        /// Gets the type of the ticket.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <value>
        /// The type of the ticket.
        /// </value>
        /// <returns> The task.</returns>
        public override async Task ProcessAsync(object data)
        {
            var conciliationRuleData = (ConciliationRuleData)data;
            ArgumentValidators.ThrowIfNull(conciliationRuleData, nameof(conciliationRuleData));

            var parameters = new Dictionary<string, object>
            {
                { "@OwnershipTicketId", conciliationRuleData.ConciliationNodes.TicketId },
                { "@NodeId", conciliationRuleData.ConciliationNodes.NodeId },
            };

            var repository = this.UnitOfWork.CreateRepository<Ownership>();
            await repository.ExecuteAsync(Repositories.Constants.DeleteMovementInformationForReport, parameters).ConfigureAwait(false);

            var storedProcTasks = new List<Task>();
            this.StoredProceduresPerType.ForEach(x => storedProcTasks.Add(repository.ExecuteAsync(x, parameters)));
            await Task.WhenAll(storedProcTasks).ConfigureAwait(false);

            await this.azureClientFactory.AnalysisServiceClient.RefreshOwnershipAsync(conciliationRuleData.ConciliationNodes.TicketId).ConfigureAwait(false);

            var ticket = await this.GetTicketByIdAsync(conciliationRuleData.ConciliationNodes.TicketId).ConfigureAwait(false);
            if (ticket == null || ticket.Status != StatusType.PROCESSED)
            {
                return;
            }

            var tasks = new List<Task>
            {
                this.SendMovementTransactionIdsToQueueAsync(conciliationRuleData.ConciliationNodes.TicketId, conciliationRuleData.ConciliationNodes.NodeId),
                this.SendOwnershipIdsToQueueAsync(conciliationRuleData.ConciliationNodes.TicketId),
            };
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task SendOwnershipIdsToQueueAsync(int ticketId)
        {
            List<int> listId = new List<int>
            {
                    (int)MovementType.CancellationTransferConciliation,
                    (int)MovementType.ConciliationTransfer,
                    (int)MovementType.EMConciliation,
                    (int)MovementType.SMConciliation,
            };

            // Send the ownerships for which blockchain status is processing, exception the negated records for update event
            var ownerships = await this.UnitOfWork.CreateRepository<Ownership>().GetAllAsync(x => x.TicketId == ticketId && x.BlockchainStatus == StatusType.PROCESSING
            && !(x.EventType == EventType.Update.ToString() && !x.PreviousBlockchainOwnershipId.HasValue)
            && !x.IsDeleted
            && listId.Contains(x.MovementTransaction.MessageTypeId)).ConfigureAwait(false);
            var ownershipIds = ownerships.OrderBy(x => x.OwnershipId).Select(x => x.OwnershipId);
            await this.SendSessionMessageToQueueAsync(ownershipIds, QueueConstants.BlockchainOwnershipQueue).ConfigureAwait(false);
        }
    }
}
