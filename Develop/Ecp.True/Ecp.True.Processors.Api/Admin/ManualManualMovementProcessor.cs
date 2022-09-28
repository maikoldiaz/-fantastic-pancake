// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManualManualMovementProcessor.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Interfaces;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Api.Specifications;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;

    /// <inheritdoc cref="IManualMovementProcessor"/>
    public class ManualManualMovementProcessor : IManualMovementProcessor
    {
        /// <summary>
        /// The movement repository.
        /// </summary>
        private readonly IRepository<Movement> movementRepo;

        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The finalizer.
        /// </summary>
        private readonly IFinalizer finalizer;

        /// <summary>
        /// The infoExecutor.
        /// </summary>
        private readonly IInfoBuildExecutor infoExecutor;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<ManualManualMovementProcessor> logger;

        /// <summary>
        /// Official delta executor.
        /// </summary>
        private readonly IExecutor officialDeltaExecutor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManualManualMovementProcessor" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="finalizerFactory">The finalizer factory.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="infoBuildExecutor">The info builder.</param>
        /// <param name="executors">The executors.</param>
        public ManualManualMovementProcessor(
            IUnitOfWorkFactory unitOfWorkFactory,
            IFinalizerFactory finalizerFactory,
            IInfoBuildExecutor infoBuildExecutor,
            IEnumerable<IExecutor> executors,
            ITrueLogger<ManualManualMovementProcessor> logger)
        {
            ArgumentValidators.ThrowIfNull(finalizerFactory, nameof(finalizerFactory));
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));

            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.infoExecutor = infoBuildExecutor;
            this.officialDeltaExecutor = executors.Single(e => e.ProcessType == ProcessType.OfficialDelta && e.Order == 6);
            this.logger = logger;
            this.finalizer = finalizerFactory.GetFinalizer(TicketType.OfficialDelta);
            this.movementRepo = this.unitOfWork.CreateRepository<Movement>();
        }

        /// <inheritdoc />
        public async Task<IQueryable<Movement>> GetAssignableMovementsAsync(int nodeId, DateTime startTime, DateTime endTime)
        {
            var isMovement = new OfficialManualMovementsSpecification(nodeId, startTime, endTime);
            var isInventory = new OfficialManualInventorySpecification(nodeId, startTime, endTime);

            var isAssignable = isMovement.OrElse(isInventory);

            var query = await this.movementRepo.QueryAllAsync(
                isAssignable,
                nameof(Movement.Period),
                nameof(Movement.MovementSource),
                nameof(Movement.SourceInventoryProduct),
                nameof(Movement.MovementDestination)).ConfigureAwait(false);

            return query.OrderBy(m => m.MovementTypeId);
        }

        /// <inheritdoc />
        public async Task UpdateTicketManualMovementsAsync(int deltaNodeId, int[] movementIds)
        {
            var deltaNode = await this.GetDeltaNodeToAddManualMovementsAsync(deltaNodeId)
                .ConfigureAwait(false);

            var assignableMovements = await this.GetAssignableMovementsAsync(movementIds, deltaNode)
                .ConfigureAwait(false);

            await this.SaveAssignableMovementsToTicketAsync(deltaNode, assignableMovements)
                .ConfigureAwait(false);

            await this.UpdateReportAsync(deltaNode, assignableMovements)
                .ConfigureAwait(false);
        }

        private async Task UpdateReportAsync(DeltaNode deltaNode, List<Movement> assignableMovements)
        {
            this.logger.LogInformation(
                $"Started update  for official ticket {deltaNode.TicketId} " +
                $"for period {assignableMovements.First()?.Period.StartTime} - " +
                $"{assignableMovements.First()?.Period.EndTime}");

            var data = await this.infoExecutor.BuildAsync(deltaNode.Ticket, assignableMovements).ConfigureAwait(false);
            await this.officialDeltaExecutor.ExecuteAsync(data).ConfigureAwait(false);

            await this.finalizer.ProcessAsync(deltaNode.TicketId).ConfigureAwait(false);

            this.logger.LogInformation(
                $"Finished update  for official ticket {deltaNode.TicketId} " +
                $"for period {assignableMovements.First()?.Period.StartTime} - " +
                $"{assignableMovements.First()?.Period.EndTime}");
        }

        /// <summary>
        /// Saver the manual movements to a ticket.
        /// </summary>
        /// <param name="deltaNode">The deltaNode.</param>
        /// <param name="assignableMovements">The assignable movements.</param>
        /// <returns>The task.</returns>
        private async Task SaveAssignableMovementsToTicketAsync(DeltaNode deltaNode, List<Movement> assignableMovements)
        {
            assignableMovements.ForEach(m => m.OfficialDeltaTicketId = deltaNode.TicketId);

            this.movementRepo.UpdateAll(assignableMovements);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the delta node to add manual movements.
        /// </summary>
        /// <param name="deltaNodeId">The delta node id.</param>
        /// <returns>The delta node.</returns>
        private async Task<DeltaNode> GetDeltaNodeToAddManualMovementsAsync(int deltaNodeId)
        {
            var deltaNodeRepo = this.unitOfWork.CreateRepository<DeltaNode>();

            var deltaNodeSpec = new DeltaNodeToAddManualMovementsSpec(deltaNodeId);

            var deltaNode = await deltaNodeRepo.FirstOrDefaultAsync(deltaNodeSpec, nameof(DeltaNode.Ticket))
                .ConfigureAwait(false);

            if (deltaNode is null)
            {
                throw new KeyNotFoundException(Constants.DeltaNodeNotFound);
            }

            return deltaNode;
        }

        /// <summary>
        /// Gets the assignable movements to a official delta node.
        /// </summary>
        /// <param name="movementIds">The movements ids requested.</param>
        /// <param name="deltaNode">The delta node.</param>
        /// <returns>The list of movements.</returns>
        private async Task<List<Movement>> GetAssignableMovementsAsync(int[] movementIds, DeltaNode deltaNode)
        {
            var isMovement = new OfficialManualMovementsSpecification(
                deltaNode.NodeId,
                deltaNode.Ticket.StartDate,
                deltaNode.Ticket.EndDate);

            var isInventory = new OfficialManualInventorySpecification(
                deltaNode.NodeId,
                deltaNode.Ticket.StartDate,
                deltaNode.Ticket.EndDate);

            var wasRequested = new RequestedMovementSpecification(movementIds);

            var isAssignable = wasRequested
                .AndAlso(isInventory.OrElse(isMovement));

            var assignableMovements = await this.movementRepo.QueryAllAsync(
                    isAssignable,
                    nameof(Movement.MovementSource),
                    nameof(Movement.MovementDestination),
                    $"{nameof(Movement.MovementSource)}.{nameof(MovementSource.SourceNode)}",
                    $"{nameof(Movement.MovementDestination)}.{nameof(MovementDestination.DestinationNode)}",
                    nameof(Movement.Period),
                    nameof(Movement.SourceInventoryProduct),
                    nameof(Movement.Ownerships))
                .ConfigureAwait(false);

            if (!assignableMovements.Any())
            {
                throw new KeyNotFoundException(Constants.NoManualMovementsWereFoundForTicket);
            }

            return assignableMovements.ToList();
        }
    }
}