// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipFinalizer.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.ExceptionHandling.Core;
    using Ecp.True.ExceptionHandling.Entities;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// Ownership finalizer.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.Interfaces.IFinalizer" />
    public class OwnershipFinalizer : FinalizerBase
    {
        /// <summary>
        /// The client.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The retry policy factory.
        /// </summary>
        private readonly IRetryPolicyFactory retryPolicyFactory;

        /// <summary>
        /// The retry handler.
        /// </summary>
        private readonly IOwnershipBalanceRetryHandler retryHandler;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipFinalizer" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="retryPolicyFactory">The retry policy factory.</param>
        /// <param name="retryHandler">The retry handler.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        public OwnershipFinalizer(
            IUnitOfWorkFactory unitOfWorkFactory,
            IAzureClientFactory azureClientFactory,
            IRetryPolicyFactory retryPolicyFactory,
            IOwnershipBalanceRetryHandler retryHandler,
            IConfigurationHandler configurationHandler)
            : base(azureClientFactory, unitOfWorkFactory)
        {
            this.azureClientFactory = azureClientFactory;
            this.retryPolicyFactory = retryPolicyFactory;
            this.retryHandler = retryHandler;
            this.configurationHandler = configurationHandler;
        }

        /// <summary>
        /// Gets the type of the ticket.
        /// </summary>
        /// <value>
        /// The type of the ticket.
        /// </value>
        public override TicketType Type => TicketType.Ownership;

        /// <summary>
        /// Gets the type of the finalizer.
        /// </summary>
        /// <value>
        /// The type of the finalizer.
        /// </value>
        public override FinalizerType Finalizer => FinalizerType.Ownership;

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
            var ownershipRuleData = (OwnershipRuleData)data;
            ArgumentValidators.ThrowIfNull(ownershipRuleData, nameof(ownershipRuleData));

            var parameters = new Dictionary<string, object>
            {
                { "@OwnershipTicketId", ownershipRuleData.TicketId },
                { "@NodeId", null },
            };
            var repository = this.UnitOfWork.CreateRepository<Ownership>();

            if (ownershipRuleData.HasDeletedMovementOwnerships)
            {
                await repository.ExecuteAsync(Repositories.Constants.DeleteMovementInformationForReport, parameters).ConfigureAwait(false);
            }

            IRetryPolicy retryPolicy;
            var systemConfig = await this.configurationHandler.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings).ConfigureAwait(false);

            var retrySettings = new RetrySettings
            {
                RetryCount = systemConfig.MaxOwnershipRetryCount.GetValueOrDefault(),
                RetryIntervalInSeconds = systemConfig.OwnerShipRetryIntervalInSeconds.GetValueOrDefault(),
                RetryStrategy = (RetryStrategy)systemConfig.OwnerShipRetryStrategy.GetValueOrDefault(),
            };

            retryPolicy = this.retryPolicyFactory.GetRetryPolicy("OwnerShipBalance", retrySettings, this.retryHandler);
            await retryPolicy.ExecuteWithRetryAsync(this.ExecuteStoredProcedures(parameters, ownershipRuleData.TicketId)).ConfigureAwait(false);
            await this.azureClientFactory.AnalysisServiceClient.RefreshOwnershipAsync(ownershipRuleData.TicketId).ConfigureAwait(false);

            var ticket = await this.GetTicketByIdAsync(ownershipRuleData.TicketId).ConfigureAwait(false);
            if (ticket == null || ticket.Status != StatusType.PROCESSED)
            {
                return;
            }

            var tasks = new List<Task>
            {
                this.SendMovementTransactionIdsToQueueAsync(ownershipRuleData.TicketId, null),
                this.SendOwnershipIdsToQueueAsync(ownershipRuleData.TicketId),
                this.SendOwnershipNodeIdsToQueueAsync(ownershipRuleData.TicketId),
            };
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task SendOwnershipNodeIdsToQueueAsync(int ticketId)
        {
            var ownershipNodes = await this.UnitOfWork.CreateRepository<OwnershipNode>().GetAllAsync(
                x => x.TicketId == ticketId, "Node").ConfigureAwait(false);
            await this.PublishOffchainNodesAsync(ownershipNodes.Select(n => n.Node), NodeState.OperativeBalanceCalculatedWithOwnership).ConfigureAwait(false);
        }

        private async Task SendOwnershipIdsToQueueAsync(int ticketId)
        {
            // Send the ownerships for which blockchain status is processing, exception the negated records for update event
            var ownerships = await this.UnitOfWork.CreateRepository<Ownership>().GetAllAsync(x => x.TicketId == ticketId && x.BlockchainStatus == StatusType.PROCESSING
            && !(x.EventType == EventType.Update.ToString() && !x.PreviousBlockchainOwnershipId.HasValue)).ConfigureAwait(false);
            var ownershipIds = ownerships.OrderBy(x => x.OwnershipId).Select(x => x.OwnershipId);
            await this.SendSessionMessageToQueueAsync(ownershipIds, QueueConstants.BlockchainOwnershipQueue).ConfigureAwait(false);
        }

        private Func<Task<bool>> ExecuteStoredProcedures(
            Dictionary<string, object> parameters,
            int ticketId) => async () => await this.GetRetryOwnershipAsync(parameters, ticketId).ConfigureAwait(false);

        private async Task<bool> GetRetryOwnershipAsync(Dictionary<string, object> parameters, int ticketId)
        {
            var repository = this.UnitOfWork.CreateRepository<Ownership>();
            try
            {
                var storedProcTasks = new List<Task>();
                this.StoredProceduresPerType.ForEach(x => storedProcTasks.Add(repository.ExecuteAsync(x, parameters)));
                await Task.WhenAll(storedProcTasks).ConfigureAwait(false);
                return true;
            }
            catch (Exception)
            {
                await repository.ExecuteAsync(Repositories.Constants.PurgeOwnershipreportdataProcedureName, parameters).ConfigureAwait(false);
                await this.azureClientFactory.AnalysisServiceClient.RefreshOwnershipAsync(ticketId).ConfigureAwait(false);
                throw;
            }
        }
    }
}
