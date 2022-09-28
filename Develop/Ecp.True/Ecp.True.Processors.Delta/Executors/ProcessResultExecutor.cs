// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessResultExecutor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Executors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Delta.Entities;
    using EfCore.Models;

    /// <summary>
    /// Process Result Executor.
    /// </summary>
    public class ProcessResultExecutor : ExecutorBase
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessResultExecutor"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="unitOfWorkFactory">The unitOfWorkFactory.</param>
        public ProcessResultExecutor(ITrueLogger<ProcessResultExecutor> logger, IUnitOfWorkFactory unitOfWorkFactory)
                   : base(logger)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        }

        /// <summary>
        /// Gets the Order.
        /// </summary>
        public override int Order => 5;

        /// <summary>
        /// Gets the type of the process.
        /// </summary>
        /// <value>
        /// The type of the process.
        /// </value>
        public override ProcessType ProcessType => ProcessType.Delta;

        /// <summary>
        /// The ExecuteAsync.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The Task.</returns>
        public override async Task ExecuteAsync(object input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            var deltaData = (DeltaData)input;
            await this.ProcessAsync(deltaData).ConfigureAwait(false);
            this.ShouldContinue = true;

            await this.ExecuteNextAsync(deltaData).ConfigureAwait(false);
        }

        /// <summary>
        /// Populate movement and inventory Data.
        /// </summary>
        /// <param name="deltaData">The delta data.</param>
        /// <returns> task.</returns>
        private async Task<DeltaData> ProcessAsync(DeltaData deltaData)
        {
            // Fetch the movements, inventoryProducts, cancellation types, nodeTags.
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));
            deltaData.NextCutOffDate = await this.GetNextCutOffDateAsync(deltaData).ConfigureAwait(false);
            deltaData.Movements = await this.GetMovementAsync(deltaData).ConfigureAwait(false);
            deltaData.InventoryProducts = await this.GetInventoryProductAsync(deltaData).ConfigureAwait(false);
            deltaData.NodeTags = await this.GetNodeTagAsync(deltaData).ConfigureAwait(false);
            deltaData.CancellationTypes = await this.GetCancellationTypesAsync(deltaData).ConfigureAwait(false);
            return deltaData;
        }

        /// <summary>
        /// Get Cancellatin Types.
        /// </summary>
        /// <param name="deltaData">The delta data.</param>
        /// <returns>annulation.</returns>
        private Task<IEnumerable<Annulation>> GetCancellationTypesAsync(DeltaData deltaData)
        {
            var movementTypedIds = deltaData.Movements.Select(x => x.MovementTypeId);
            var annulationRepository = this.unitOfWork.CreateRepository<Annulation>();
            return annulationRepository.GetAllAsync(x => movementTypedIds.Contains(x.SourceMovementTypeId));
        }

        /// <summary>
        /// Get Movement By MovementId.
        /// </summary>
        /// <param name="deltaData">the movement Id.</param>
        /// <returns> the task.</returns>
        private Task<IEnumerable<Movement>> GetMovementAsync(DeltaData deltaData)
        {
            var movementRepository = this.unitOfWork.CreateRepository<Movement>();
            var movementTransactionIds = deltaData.OriginalMovements.Select(x => x.MovementTransactionId).Union(deltaData.UpdatedMovements.Select(y => y.MovementTransactionId));
            return movementRepository.GetAllAsync(
            f =>
            movementTransactionIds.Contains(f.MovementTransactionId),
            "MovementSource",
            "MovementDestination",
            "Ownerships");
        }

        /// <summary>
        /// fetch node tag.
        /// </summary>
        /// <param name="deltaData">deltaData.</param>
        /// <returns>node tag.</returns>
        private Task<IEnumerable<NodeTag>> GetNodeTagAsync(DeltaData deltaData)
        {
            var sourceNodeIds = deltaData.Movements.Where(x => x.MovementSource != null)
                .Select(x => x.MovementSource.SourceNodeId.GetValueOrDefault());
            var destinationNodeIds = deltaData.Movements.Where(x => x.MovementDestination != null)
                .Select(x => x.MovementDestination.DestinationNodeId.GetValueOrDefault());
            var inventoryNodeIds = deltaData.InventoryProducts.Select(x => x.NodeId);
            var nodeIds = sourceNodeIds.Union(destinationNodeIds).Union(inventoryNodeIds);
            var nodeTagRepository = this.unitOfWork.CreateRepository<NodeTag>();
            return nodeTagRepository.GetAllAsync(
                 x =>
                 nodeIds.Contains(x.NodeId) && x.ElementId == deltaData.Ticket.CategoryElementId &&
                 deltaData.NextCutOffDate >= x.StartDate && deltaData.NextCutOffDate <= x.EndDate);
        }

        /// <summary>
        /// get next cut off date.
        /// </summary>
        /// <param name="deltaData">The delta data.</param>
        /// <returns>date time.</returns>
        private async Task<DateTime> GetNextCutOffDateAsync(DeltaData deltaData)
        {
            var ticket = await this.unitOfWork.TicketInfoRepository.GetLastTicketAsync(deltaData.Ticket.CategoryElementId, TicketType.Cutoff).ConfigureAwait(false);
            return ticket.EndDate.AddDays(1);
        }

        /// <summary>
        /// Get inventry products.
        /// </summary>
        /// <param name="deltaData">deltaData.</param>
        /// <returns>list  of inventory product.</returns>
        private Task<IEnumerable<InventoryProduct>> GetInventoryProductAsync(DeltaData deltaData)
        {
            var inventoryRepository = this.unitOfWork.CreateRepository<InventoryProduct>();
            var inventoryTransactionIds = deltaData.OriginalInventories.Select(x => x.InventoryProductId).Union(deltaData.UpdatedInventories.Select(y => y.InventoryProductId));
            return inventoryRepository.GetAllAsync(
            f =>
            inventoryTransactionIds.Contains(f.InventoryProductId),
            "Ownerships");
        }
    }
}
