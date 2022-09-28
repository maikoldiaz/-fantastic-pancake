// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaFinalizer.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// Operational cutoff finalizer.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.Interfaces.IFinalizer" />
    public class OfficialDeltaFinalizer : FinalizerBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<OfficialDeltaFinalizer> logger;

        /// <summary>
        /// The client.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OfficialDeltaFinalizer" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public OfficialDeltaFinalizer(
            ITrueLogger<OfficialDeltaFinalizer> logger,
            IAzureClientFactory azureClientFactory,
            IUnitOfWorkFactory unitOfWorkFactory)
            : base(azureClientFactory, unitOfWorkFactory)
        {
            this.logger = logger;
            this.azureClientFactory = azureClientFactory;
        }

        /// <summary>
        /// Gets the type of the ticket.
        /// </summary>
        /// <value>
        /// The type of the ticket.
        /// </value>
        public override TicketType Type => TicketType.OfficialDelta;

        /// <summary>
        /// Gets the type of the finalizer.
        /// </summary>
        /// <value>
        /// The type of the finalizer.
        /// </value>
        public override FinalizerType Finalizer => FinalizerType.OfficialDelta;

        /// <summary>
        /// Processes the asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public override async Task ProcessAsync(int ticketId)
        {
            var ticket = await this.GetTicketByIdAsync(ticketId).ConfigureAwait(false);
            var parameters = new Dictionary<string, object>
            {
                { "@SegmentId", ticket.CategoryElementId },
                { "@StartDate", ticket.StartDate },
                { "@EndDate", ticket.EndDate },
            };

            var repository = this.UnitOfWork.CreateRepository<DeltaBalance>();

            this.logger.LogInformation($"Executing finalizer SP's for ticketId: {ticket.TicketId}", ticket.TicketId);
            var storedProcTasks = new List<Task>();
            this.StoredProceduresPerType.ForEach(x => storedProcTasks.Add(repository.ExecuteAsync(x, parameters)));
            await Task.WhenAll(storedProcTasks).ConfigureAwait(false);

            this.logger.LogInformation($"Refreshing Analysis Service for Official Delta ticketId: {ticket.TicketId}", ticket.TicketId);
            await this.azureClientFactory.AnalysisServiceClient.RefreshOfficialDeltaAsync(ticketId).ConfigureAwait(false);

            this.logger.LogInformation($"Updating Delta Nodes and ticket's status for  ticketId: {ticket.TicketId}", ticket.TicketId);
            await this.UpdateDeltaNodesStatusToSuccessAsync(ticketId).ConfigureAwait(false);
            await this.UpdateTicketToSuccessAsync(ticketId).ConfigureAwait(false);
            await this.UnitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Process the ticket asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        private async Task UpdateDeltaNodesStatusToSuccessAsync(int ticketId)
        {
            var deltaNodeRepository = this.UnitOfWork.CreateRepository<DeltaNode>();
            var deltaNodes = await deltaNodeRepository.GetAllAsync(x => x.TicketId == ticketId).ConfigureAwait(false);
            deltaNodes.ForEach(y => y.Status = OwnershipNodeStatusType.DELTAS);
            deltaNodeRepository.UpdateAll(deltaNodes);
        }

        /// <summary>
        /// Process the ticket asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        private async Task UpdateTicketToSuccessAsync(int ticketId)
        {
            var ticketRepository = this.UnitOfWork.CreateRepository<Ticket>();
            var ticket = await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);
            ticket.Status = StatusType.DELTA;
            ticketRepository.Update(ticket);
        }
    }
}
