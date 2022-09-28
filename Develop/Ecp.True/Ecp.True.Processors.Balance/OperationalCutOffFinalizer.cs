// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationalCutOffFinalizer.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Balance
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
    using Ecp.True.ExceptionHandling.Core;
    using Ecp.True.ExceptionHandling.Entities;
    using Ecp.True.Processors.Balance.Interfaces;
    using Ecp.True.Processors.Core;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// Operational cutoff finalizer.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.Interfaces.IFinalizer" />
    public class OperationalCutOffFinalizer : FinalizerBase
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
        private readonly ICutOffBalanceRetryHandler retryHandler;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationalCutOffFinalizer" /> class.
        /// </summary>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="retryPolicyFactory">The retry policy factory.</param>
        /// <param name="retryHandler">The retry handler.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public OperationalCutOffFinalizer(
            IAzureClientFactory azureClientFactory,
            IRetryPolicyFactory retryPolicyFactory,
            ICutOffBalanceRetryHandler retryHandler,
            IConfigurationHandler configurationHandler,
            IUnitOfWorkFactory unitOfWorkFactory)
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
        public override TicketType Type => TicketType.Cutoff;

        /// <summary>
        /// Gets the type of the finalizer.
        /// </summary>
        /// <value>
        /// The type of the finalizer.
        /// </value>
        public override FinalizerType Finalizer => FinalizerType.Cutoff;

        /// <summary>
        /// Processes the asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public override async Task ProcessAsync(int ticketId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@TicketId", ticketId },
            };

            IRetryPolicy retryPolicy;
            var systemConfig = await this.configurationHandler.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings).ConfigureAwait(false);

            var retrySettings = new RetrySettings
            {
                RetryCount = systemConfig.MaxCutOffRetryCount.GetValueOrDefault(),
                RetryIntervalInSeconds = systemConfig.CutOffRetryIntervalInSeconds.GetValueOrDefault(),
                RetryStrategy = (RetryStrategy)systemConfig.CutOffRetryStrategy.GetValueOrDefault(),
            };

            retryPolicy = this.retryPolicyFactory.GetRetryPolicy("CutOffBalance", retrySettings, this.retryHandler);
            await retryPolicy.ExecuteWithRetryAsync(this.ExecuteStoredProcedures(parameters, ticketId)).ConfigureAwait(false);

            await this.azureClientFactory.AnalysisServiceClient.RefreshCalculationAsync(ticketId).ConfigureAwait(false);

            var ticket = await this.GetTicketByIdAsync(ticketId).ConfigureAwait(false);
            if (ticket == null || ticket.Status != StatusType.PROCESSED)
            {
                return;
            }

            var tasks = new List<Task>
            {
                this.SendMovementTransactionIdsToQueueAsync(ticketId, null),
                this.SendNodeIdsToQueueAsync(ticketId),
                this.SendTicketIdToQueueAsync(ticketId),
            };
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task SendTicketIdToQueueAsync(int ticketId)
        {
            var unbalances = await this.UnitOfWork.CreateRepository<Unbalance>().GetAllAsync(x => x.TicketId == ticketId).ConfigureAwait(false);
            var unbalanceIds = unbalances.OrderBy(x => x.UnbalanceId).Select(x => x.UnbalanceId).Distinct();
            await this.SendSessionMessageToQueueAsync(unbalanceIds, QueueConstants.BlockchainNodeProductCalculationQueue).ConfigureAwait(false);
        }

        private async Task SendNodeIdsToQueueAsync(int ticketId)
        {
            var unbalanceNodes = await this.UnitOfWork.CreateRepository<Unbalance>().GetAllAsync(x => x.TicketId == ticketId, "Node").ConfigureAwait(false);
            await this.PublishOffchainNodesAsync(unbalanceNodes.Select(n => n.Node), NodeState.OperativeBalanceCalculated).ConfigureAwait(false);
        }

        private Func<Task<bool>> ExecuteStoredProcedures(Dictionary<string, object> parameters, int ticketId) => async () => await this.GetRetryCutOffAsync(parameters, ticketId).ConfigureAwait(false);

        private async Task<bool> GetRetryCutOffAsync(Dictionary<string, object> parameters, int ticketId)
        {
            var repository = this.UnitOfWork.CreateRepository<Unbalance>();
            try
            {
                var storedProcTasks = new List<Task>();
                this.StoredProceduresPerType.ForEach(x => storedProcTasks.Add(repository.ExecuteAsync(x, parameters)));
                await Task.WhenAll(storedProcTasks).ConfigureAwait(false);

                return true;
            }
            catch (Exception)
            {
                await repository.ExecuteAsync(Repositories.Constants.PurgeCutoffreportdataProcedureName, parameters).ConfigureAwait(false);
                await this.azureClientFactory.AnalysisServiceClient.RefreshCalculationAsync(ticketId).ConfigureAwait(false);
                throw;
            }
        }
    }
}
