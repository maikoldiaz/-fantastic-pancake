// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SegmentOwnershipCalculationService.cs" company="Microsoft">
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
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.Report;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Ownership.Interfaces;

    /// <summary>
    /// The OwnershipProcessor.
    /// </summary>
    public class SegmentOwnershipCalculationService : OwnershipProcessorBase, ISegmentOwnershipCalculationService
    {
        /// <summary>
        /// The repository factory.
        /// </summary>
        private readonly IRepositoryFactory repositoryFactory;

        /// <summary>
        /// The calculate ownership.
        /// </summary>
        private readonly ICalculateOwnership calculateOwnership;

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentOwnershipCalculationService" /> class.
        /// </summary>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <param name="calculateOwnership">The calculate ownership.</param>
        public SegmentOwnershipCalculationService(IRepositoryFactory repositoryFactory, ICalculateOwnership calculateOwnership)
        : base(repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
            this.calculateOwnership = calculateOwnership;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SegmentOwnershipCalculation>> ProcessSegmentOwnershipAsync(
            IEnumerable<InventoryProduct> inventories,
            IEnumerable<Movement> movements,
            int ticketId)
        {
            ArgumentValidators.ThrowIfNull(movements, nameof(movements));
            ArgumentValidators.ThrowIfNull(inventories, nameof(inventories));
            var ticketRepository = this.repositoryFactory.CreateRepository<Ticket>();
            var ticket = await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);
            var nodes = await this.GetSegmentNodesAsync(ticket).ConfigureAwait(false);
            var finalSegmentOwnershipCalculations = new List<SegmentOwnershipCalculation>();
            var calculationDate = ticket.StartDate.Date;

            while (calculationDate <= ticket.EndDate.Date)
            {
                var movementsByDate = movements.Where(x => x.OperationalDate.Date == calculationDate.Date);
                var inventoriesByPreviousAndCurrentDate = inventories.Where(
                    x =>
                    x.InventoryDate.GetValueOrDefault().Date == calculationDate.Date || x.InventoryDate.GetValueOrDefault().Date == calculationDate.AddDays(-1).Date);
                var segmentNodesByDate = nodes.segmentNodes.Where(x => x.OperationDate.Date == calculationDate.Date);
                var initialNodesByDate = nodes.initialNodes.Where(x => x.OperationDate.Date == calculationDate.Date);
                var finalNodesByDate = nodes.finalNodes.Where(x => x.OperationDate.Date == calculationDate.Date);
                var products = GetDateNodeProducts(segmentNodesByDate, inventoriesByPreviousAndCurrentDate, movementsByDate);
                foreach (ReportProductByUnit product in products)
                {
                    var distinctOwners = GetOwners(movementsByDate, inventoriesByPreviousAndCurrentDate, product.ProductId, (int)product.MeasurementUnit, segmentNodesByDate.Select(y => y.NodeId));
                    finalSegmentOwnershipCalculations.AddRange(this.ProcessForEachOwner(
                        distinctOwners,
                        product.ProductId,
                        (int)product.MeasurementUnit,
                        calculationDate,
                        inventoriesByPreviousAndCurrentDate,
                        movementsByDate,
                        segmentNodesByDate,
                        initialNodesByDate,
                        finalNodesByDate,
                        ticketId,
                        ticket.CategoryElementId));
                }

                calculationDate = calculationDate.AddDays(1);
            }

            return finalSegmentOwnershipCalculations;
        }

        /// <summary>
        /// Gets the input movements.
        /// </summary>
        /// <param name="movements">The movements.</param>
        /// <param name="productId">The product.</param>
        /// <param name="measurementUnit">The measurement Unit.</param>
        /// <param name="segmentNodes">The segment nodes.</param>
        /// <param name="initialSegmentNodes">The initial segment nodes.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <returns>The collection of Input Movement.</returns>
        private static IEnumerable<Movement> GetSegmentInputMovements(
            IEnumerable<Movement> movements,
            string productId,
            int measurementUnit,
            IEnumerable<SegmentNodeDto> segmentNodes,
            IEnumerable<SegmentNodeDto> initialSegmentNodes,
            int ownerId)
        {
            var inputMovements = movements.Where(x => MessageTypeMovementValidation(x) && DestinationMovementValidation(x, productId, measurementUnit));
            var result = inputMovements.Where(x => x.Ownerships.Any(y => y.OwnerId == ownerId) && initialSegmentNodes.Any() &&
            (IsProductConvertionInSegment(segmentNodes, x) ||
            ((x.MovementSource?.SourceNodeId == null || !segmentNodes.Any(y => y.NodeId == x.MovementSource?.SourceNodeId)) &&
            segmentNodes.Any(y => y.NodeId == x.MovementDestination?.DestinationNodeId))));

            return result;
        }

        /// <summary>
        /// Gets the output movements.
        /// </summary>
        /// <param name="movements">The movements.</param>
        /// <param name="productId">The product.</param>
        /// <param name="measurementUnit">The measurement Unit.</param>
        /// <param name="segmentNodes">The segment nodes.</param>
        /// <param name="finalSegmentNodes">The final segment nodes.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <returns>The collection of Output Movement.</returns>
        private static IEnumerable<Movement> GetSegmentOutputMovements(
            IEnumerable<Movement> movements,
            string productId,
            int measurementUnit,
            IEnumerable<SegmentNodeDto> segmentNodes,
            IEnumerable<SegmentNodeDto> finalSegmentNodes,
            int ownerId)
        {
            var outputMovements = movements.Where(x => MessageTypeMovementValidation(x) && SourceMovementValidation(x, productId, measurementUnit));
            var result = outputMovements.Where(x => x.Ownerships.Any(y => y.OwnerId == ownerId) && finalSegmentNodes.Any() &&
            (IsProductConvertionInSegment(segmentNodes, x) ||
            ((x.MovementDestination?.DestinationNodeId == null || !segmentNodes.Any(y => y.NodeId == x.MovementDestination?.DestinationNodeId)) &&
            segmentNodes.Any(y => y.NodeId == x.MovementSource?.SourceNodeId))));

            return result;
        }

        private static bool IsProductConvertionInSegment(IEnumerable<SegmentNodeDto> segmentNodes, Movement movement)
        {
            return segmentNodes.Any(y => y.NodeId == movement.MovementDestination?.DestinationNodeId)
                && segmentNodes.Any(y => y.NodeId == movement.MovementSource?.SourceNodeId)
                && movement.MovementSource?.SourceProductId != movement.MovementDestination?.DestinationProductId;
        }

        private static bool MessageTypeMovementValidation(Movement movement)
        {
            List<int> listId = new List<int>
            {
                    (int)MovementType.CancellationTransferConciliation,
                    (int)MovementType.ConciliationTransfer,
                    (int)MovementType.EMConciliation,
                    (int)MovementType.SMConciliation,
            };

            return
                !(movement.IsDeleted && listId.Any(x => x == movement.MovementTypeId)) &&
                (movement.MessageTypeId == (int)MessageType.Movement ||
                (movement.MessageTypeId == (int)MessageType.SpecialMovement && movement.VariableTypeId == null));
        }

        private static bool SourceMovementValidation(
            Movement movement,
            string productId,
            int measurementUnit) => movement.MovementSource?.SourceProductId == productId && movement.MeasurementUnit == measurementUnit;

        private static bool DestinationMovementValidation(
            Movement movement,
            string productId,
            int measurementUnit) => movement.MovementDestination?.DestinationProductId == productId && movement.MeasurementUnit == measurementUnit;

        /// <summary>
        /// Gets the inventories and movements.
        /// </summary>
        /// <param name="productId">The product.</param>
        /// <param name="measurementUnit">The measurement Unit.</param>
        /// <param name="date">The date.</param>
        /// <param name="inventoryProducts">The inventory products.</param>
        /// <param name="movements">The movements.</param>
        /// <param name="segmentNodes">The segment nodes.</param>
        /// <param name="initialSegmentNodes">The initial segment nodes.</param>
        /// <param name="finalSegmentNodes">The final segment nodes.</param>
        /// <returns>The Input and Output Movements, Initial and Final inventories.</returns>
        private static (IEnumerable<Movement> inputMovements, IEnumerable<Movement> outputMovements, IEnumerable<InventoryProduct> initialInventories,
            IEnumerable<InventoryProduct> finalInventories)
            GetInventoriesAndMovementsForSegmentAndProduct(
             string productId,
             int measurementUnit,
             DateTime date,
             int ownerId,
             IEnumerable<InventoryProduct> inventoryProducts,
             IEnumerable<Movement> movements,
             IEnumerable<SegmentNodeDto> segmentNodes,
             IEnumerable<SegmentNodeDto> initialSegmentNodes,
             IEnumerable<SegmentNodeDto> finalSegmentNodes)
        {
            var genericNodes = segmentNodes.Where(x => x.NodeName.Contains("Genérico", StringComparison.InvariantCulture));
            if (genericNodes.Any())
            {
                inventoryProducts = inventoryProducts.Where(x => !genericNodes.Any(y => y.NodeId == x.NodeId));
                segmentNodes = segmentNodes.Where(x => !genericNodes.Any(y => y.NodeId == x.NodeId));
            }

            var inputMovements = GetSegmentInputMovements(movements, productId, measurementUnit, segmentNodes, initialSegmentNodes, ownerId);
            var outputMovements = GetSegmentOutputMovements(movements, productId, measurementUnit, segmentNodes, finalSegmentNodes, ownerId);

            var initialInventories = inventoryProducts.Where(
                y =>
                segmentNodes.Any(x => x.NodeId == y.NodeId) &&
                y.InventoryDate.GetValueOrDefault().Date == date.AddDays(-1).Date &&
                y.ProductId == productId && y.MeasurementUnit == measurementUnit &&
                y.Ownerships.Any(x => x.OwnerId == ownerId));

            var finalInventories = inventoryProducts.Where(
                y =>
                segmentNodes.Any(x => x.NodeId == y.NodeId) &&
                y.InventoryDate.GetValueOrDefault().Date == date.Date &&
                y.ProductId == productId && y.MeasurementUnit == measurementUnit &&
                y.Ownerships.Any(x => x.OwnerId == ownerId));

            return (inputMovements, outputMovements, initialInventories, finalInventories);
        }

        /// <summary>
        /// Gets the output movements.
        /// </summary>
        /// <param name="segmentNodes">The segment nodes.</param>
        /// <param name="inventoryProducts">The inventory products.</param>
        /// <param name="movements">The movements.</param>
        /// <returns>The collection of Output Movement.</returns>
        private static IEnumerable<ReportProductByUnit> GetDateNodeProducts(
            IEnumerable<SegmentNodeDto> segmentNodes,
            IEnumerable<InventoryProduct> inventoryProducts,
            IEnumerable<Movement> movements)
        {
            var sourceMovementsForSegmentNodes = movements.Where(x => segmentNodes.Any(y => y.NodeId == x.MovementSource?.SourceNodeId));
            var destinationMovementsForSegmentNodes = movements.Where(x => segmentNodes.Any(y => y.NodeId == x.MovementDestination?.DestinationNodeId));
            var inventoriesForSegmentNodes = inventoryProducts.Where(x => segmentNodes.Any(y => y.NodeId == x.NodeId));

            var movementProducts = sourceMovementsForSegmentNodes.Select(x => new ReportProductByUnit
            { ProductId = x.MovementSource?.SourceProductId, MeasurementUnit = x.MeasurementUnit }).Where(y => y != null).Distinct()
                .Union(destinationMovementsForSegmentNodes.Select(x => new ReportProductByUnit
                { ProductId = x.MovementDestination?.DestinationProductId, MeasurementUnit = x.MeasurementUnit }).Where(y => y != null).Distinct())
                .Union(inventoriesForSegmentNodes.Select(x => new ReportProductByUnit { ProductId = x.ProductId, MeasurementUnit = x.MeasurementUnit }).Where(y => y != null).Distinct());

            var products = movementProducts
                                                    .GroupBy(c => new
                                                    {
                                                        c.ProductId,
                                                        c.MeasurementUnit,
                                                    })
                                                    .Select(x => new ReportProductByUnit
                                                    {
                                                        ProductId = x.Key.ProductId,
                                                        MeasurementUnit = x.Key.MeasurementUnit,
                                                    });

            return products;
        }

        private IEnumerable<SegmentOwnershipCalculation> ProcessForEachOwner(
          IEnumerable<int> owners,
          string productId,
          int measurementUnit,
          DateTime date,
          IEnumerable<InventoryProduct> inventories,
          IEnumerable<Movement> movementsByDate,
          IEnumerable<SegmentNodeDto> segmentNodesByDate,
          IEnumerable<SegmentNodeDto> initialNodesByDate,
          IEnumerable<SegmentNodeDto> finalNodesByDate,
          int ticketId,
          int segmentId)
        {
            var resultSegmentOwnershipCalculation = new List<SegmentOwnershipCalculation>();
            foreach (var ownerId in owners)
            {
                var data = GetInventoriesAndMovementsForSegmentAndProduct(
                           productId,
                           measurementUnit,
                           date,
                           ownerId,
                           inventories,
                           movementsByDate,
                           segmentNodesByDate,
                           initialNodesByDate,
                           finalNodesByDate);
                var result = this.calculateOwnership.CalculateAndRegisterForSegment(
                productId,
                measurementUnit,
                date,
                ownerId,
                ticketId,
                segmentId,
                data.inputMovements,
                data.outputMovements,
                data.initialInventories,
                data.finalInventories,
                movementsByDate,
                segmentNodesByDate.Select(y => y.NodeId));
                resultSegmentOwnershipCalculation.Add(result);
            }

            return this.calculateOwnership.CalculatePercentageAndRegisterForSegment(resultSegmentOwnershipCalculation);
        }

        /// <summary>
        /// Gets the segment nodes asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The collection of Segment Nodes.</returns>
        private async Task<(IEnumerable<SegmentNodeDto> segmentNodes, IEnumerable<SegmentNodeDto> initialNodes, IEnumerable<SegmentNodeDto> finalNodes)> GetSegmentNodesAsync(Ticket ticket)
        {
            var segmentNodes = new List<SegmentNodeDto>();
            var initialNodes = new List<SegmentNodeDto>();
            var finalNodes = new List<SegmentNodeDto>();
            var parameters = new Dictionary<string, object>
            {
                { "@SegmentId", ticket.CategoryElementId },
                { "@StartDate", ticket.StartDate.Date },
                { "@EndDate", ticket.EndDate.Date },
            };

            await Task.WhenAll(
            this.GetDataFromRepositoryAsync<SegmentNodeDto>(
            (nodes) => segmentNodes = nodes.ToList(),
            Repositories.Constants.GetSegmentNodesProcedureName,
            parameters),
            this.GetDataFromRepositoryAsync<SegmentNodeDto>(
            (nodes) => initialNodes = nodes.ToList(),
            Repositories.Constants.GetInitialNodesProcedureName,
            parameters),
            this.GetDataFromRepositoryAsync<SegmentNodeDto>(
            (nodes) => finalNodes = nodes.ToList(),
            Repositories.Constants.GetFinalNodesProcedureName,
            parameters)).ConfigureAwait(false);

            return (segmentNodes: segmentNodes.AsEnumerable(), initialNodes: initialNodes.AsEnumerable(), finalNodes: finalNodes.AsEnumerable());
        }

        private async Task GetDataFromRepositoryAsync<T>(Action<IEnumerable<T>> setter, string storedProcedureName, IDictionary<string, object> parameters)
where T : class, IEntity
        {
            setter(await this.repositoryFactory.CreateRepository<T>()
                            .ExecuteQueryAsync(storedProcedureName, parameters).ConfigureAwait(false));
        }
    }
}
