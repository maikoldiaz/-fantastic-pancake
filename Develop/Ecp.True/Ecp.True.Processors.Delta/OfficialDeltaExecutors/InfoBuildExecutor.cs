// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfoBuildExecutor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.OfficialDeltaExecutors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Interfaces;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.Specifications;

    /// <summary>
    /// The InfoBuildExecutor.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.Execution.ExecutorBase" />
    public class InfoBuildExecutor : ExecutorBase, IInfoBuildExecutor
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="InfoBuildExecutor" /> class.
        /// </summary>
        /// <param name="logger">the logger.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        public InfoBuildExecutor(
            ITrueLogger<InfoBuildExecutor> logger,
            IUnitOfWorkFactory unitOfWorkFactory)
            : base(logger)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        }

        /// <summary>
        /// Gets the Order.
        /// </summary>
        public override int Order => 1;

        /// <summary>
        /// Gets the type of the process.
        /// </summary>
        /// <value>
        /// The type of the process.
        /// </value>
        public override ProcessType ProcessType => ProcessType.OfficialDelta;

        /// <summary>
        /// Process the ticket async.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="assignableMovements">The assignable movements.</param>
        /// <returns>The task.</returns>
        public async Task<object> BuildAsync(Ticket ticket, IList<Movement> assignableMovements)
        {
            var data = new OfficialDeltaData
            {
                Ticket = ticket,
            };

            data.PendingOfficialMovements = assignableMovements
                .Select(
                    a => new PendingOfficialMovement
                    {
                        DestinationNodeId = a.MovementDestination?.DestinationNodeId,
                        SourceNodeId = a.MovementSource?.SourceNodeId,
                        DestinationNodeSegmentID = a.MovementDestination?.DestinationNode?.SegmentId,
                        SourceNodeSegmentId = a.MovementSource?.SourceNode?.SegmentId,
                    }).AsEnumerable();

            await this.BuildAsync(data)
                .ConfigureAwait(false);

            return data;
        }

        /// <summary>
        /// The ExecuteAsync.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The Task.</returns>
        public override async Task ExecuteAsync(object input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));

            var officialDeltaData = (OfficialDeltaData)input;

            this.Logger.LogInformation($"Started {nameof(InfoBuildExecutor)} for ticket {officialDeltaData.Ticket.TicketId}", officialDeltaData.Ticket.TicketId);

            await this.BuildAsync(officialDeltaData).ConfigureAwait(false);

            this.Logger.LogInformation($"Completed {nameof(InfoBuildExecutor)} for ticket {officialDeltaData.Ticket.TicketId}", officialDeltaData.Ticket.TicketId);
            this.ShouldContinue = true;

            await this.ExecuteNextAsync(officialDeltaData).ConfigureAwait(false);
        }

        /// <summary>
        /// Builds the asynchronous.
        /// </summary>
        /// <param name="officialDeltaData">The delta data.</param>
        /// <returns>
        /// The task.
        /// </returns>
        private async Task BuildAsync(OfficialDeltaData officialDeltaData)
        {
            ArgumentValidators.ThrowIfNull(officialDeltaData, nameof(officialDeltaData));
            await Task.WhenAll(
                this.GetDataFromAnnulationRepositoryAsync(officialDeltaData),
                this.GetDataFromConsolidatedInventoryProductRepositoryAsync(officialDeltaData),
                this.GetDataFromConsolidatedMovementRepositoryAsync(officialDeltaData)).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the data from annulation repository asynchronous.
        /// </summary>
        /// <param name="officialDeltaData">The official delta data.</param>
        private async Task GetDataFromAnnulationRepositoryAsync(OfficialDeltaData officialDeltaData)
        {
            var annulationRepository = this.unitOfWork.CreateRepository<Annulation>();
            officialDeltaData.CancellationTypes = await annulationRepository.GetAllAsync(x => x.IsActive.HasValue && x.IsActive.Value).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the data from consolidated movement repository asynchronous.
        /// </summary>
        /// <param name="officialDeltaData">The official delta data.</param>
        private async Task GetDataFromConsolidatedMovementRepositoryAsync(OfficialDeltaData officialDeltaData)
        {
            var pendingMovementNodes = officialDeltaData.GetPendingOfficialMovementNodes().ToList();

            var consolidatedMovementRepository = this.unitOfWork.CreateRepository<ConsolidatedMovement>();

            var consolidatedMovementSpec = new ConsolidatedMovementSpecification(officialDeltaData.Ticket, pendingMovementNodes);

            officialDeltaData.ConsolidationMovements = await consolidatedMovementRepository
                .GetAllSpecificAsync(consolidatedMovementSpec)
                .ConfigureAwait(false);
        }

        private async Task GetDataFromConsolidatedInventoryProductRepositoryAsync(OfficialDeltaData officialDeltaData)
        {
            var nodeList = officialDeltaData.GetPendingOfficialMovementNodes();
            var consolidatedInventoryProductRepository = this.unitOfWork.CreateRepository<ConsolidatedInventoryProduct>();
            officialDeltaData.ConsolidationInventories = await consolidatedInventoryProductRepository.GetAllAsync(
            x => (x.InventoryDate.Date == officialDeltaData.Ticket.StartDate.AddDays(-1).Date
            || x.InventoryDate.Date == officialDeltaData.Ticket.EndDate.Date)
            && nodeList.Contains(x.NodeId),
            "ConsolidatedOwners").ConfigureAwait(false);
        }
    }
}
