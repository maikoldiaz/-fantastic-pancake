// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemOwnershipCalculationService.cs" company="Microsoft">
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
    public class SystemOwnershipCalculationService : OwnershipProcessorBase, ISystemOwnershipCalculationService
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
        /// Initializes a new instance of the <see cref="SystemOwnershipCalculationService" /> class.
        /// </summary>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <param name="calculateOwnership">The calculate ownership.</param>
        public SystemOwnershipCalculationService(IRepositoryFactory repositoryFactory, ICalculateOwnership calculateOwnership)
        : base(repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
            this.calculateOwnership = calculateOwnership;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SystemOwnershipCalculation>> ProcessSystemOwnershipAsync(
            IEnumerable<InventoryProduct> inventories,
            IEnumerable<Movement> movements,
            int ticketId)
        {
            var ticketRepository = this.repositoryFactory.CreateRepository<Ticket>();
            var ticket = await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);
            var systems = await this.GetSegmentSystemsAsync(ticket).ConfigureAwait(false);
            var dates = systems.Select(x => x.OperationDate).Distinct();
            var finalSystemOwnershipCalculations = new List<SystemOwnershipCalculation>();
            foreach (var date in dates)
            {
                var systemIdsByDate = systems.Where(x => x.OperationDate.Date == date.Date).Select(s => s.SystemId);
                foreach (var systemId in systemIdsByDate)
                {
                    var systemNodesByDate = await this.GetSystemNodesAsync(systemId, date).ConfigureAwait(false);
                    var movementsByDate = movements.Where(x => x.OperationalDate.Date == date.Date);
                    var inventoriesByPreviousAndCurrentDate = inventories.Where(
                    x =>
                    x.InventoryDate.GetValueOrDefault().Date == date.Date || x.InventoryDate.GetValueOrDefault().Date == date.AddDays(-1).Date);
                    var products = GetNodeProducts(systemNodesByDate, inventoriesByPreviousAndCurrentDate, movementsByDate);
                    foreach (ReportProductByUnit product in products)
                    {
                        var distinctOwners = GetOwners(movementsByDate, inventoriesByPreviousAndCurrentDate, product.ProductId, (int)product.MeasurementUnit, systemNodesByDate.Select(y => y.NodeId));
                        finalSystemOwnershipCalculations.AddRange(this.ProcessForEachOwner(
                            distinctOwners,
                            product.ProductId,
                            (int)product.MeasurementUnit,
                            date,
                            inventoriesByPreviousAndCurrentDate,
                            movementsByDate,
                            systemNodesByDate,
                            ticketId,
                            ticket.CategoryElementId,
                            systemId));
                    }
                }
            }

            return finalSystemOwnershipCalculations;
        }

        /// <summary>
        /// Gets the input movements.
        /// </summary>
        /// <param name="movements">The movements.</param>
        /// <param name="productId">The product.</param>
        /// <param name="measurementUnit">The measurement Unit.</param>
        /// <param name="systemNodes">The system nodes.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <returns>The collection of Input Movement.</returns>
        private static IEnumerable<Movement> GetSystemInputMovements(
            IEnumerable<Movement> movements,
            string productId,
            int measurementUnit,
            IEnumerable<SegmentNodeDto> systemNodes,
            int ownerId)
        {
            var inputMovements = movements.Where(x => MessageTypeMovementValidation(x) && x.MovementDestination?.DestinationProductId == productId && x.MeasurementUnit == measurementUnit);
            return inputMovements.Where(x => (IsProductConvertionInSystem(systemNodes, x) ||
            (systemNodes.Any(y => y.NodeId == x.MovementDestination?.DestinationNodeId) && !systemNodes.Any(y => y.NodeId == x.MovementSource?.SourceNodeId)))
            && x.Ownerships.Any(y => y.OwnerId == ownerId));
        }

        /// <summary>
        /// Gets the output movements.
        /// </summary>
        /// <param name="movements">The movements.</param>
        /// <param name="productId">The product.</param>
        /// <param name="measurementUnit">The measurement Unit.</param>
        /// <param name="systemNodes">The system nodes.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <returns>The collection of Output Movement.</returns>
        private static IEnumerable<Movement> GetSystemOutputMovements(
            IEnumerable<Movement> movements,
            string productId,
            int measurementUnit,
            IEnumerable<SegmentNodeDto> systemNodes,
            int ownerId)
        {
            var outputMovements = movements.Where(x => MessageTypeMovementValidation(x) && x.MovementSource?.SourceProductId == productId && x.MeasurementUnit == measurementUnit);
            return outputMovements.Where(x => (IsProductConvertionInSystem(systemNodes, x) ||
            (systemNodes.Any(y => y.NodeId == x.MovementSource?.SourceNodeId) && !systemNodes.Any(y => y.NodeId == x.MovementDestination?.DestinationNodeId)))
            && x.Ownerships.Any(y => y.OwnerId == ownerId));
        }

        /// <summary>
        /// Gets the inventories and movements.
        /// </summary>
        /// <param name="productId">The product.</param>
        /// <param name="measurementUnit">The measurement Unit.</param>
        /// <param name="date">The date.</param>
        /// <param name="inventoryProducts">The inventory products.</param>
        /// <param name="movements">The movements.</param>
        /// <param name="systemNodes">The system nodes.</param>
        /// <returns>The Input and Output Movements, Initial and Final inventories.</returns>
        private static (IEnumerable<Movement> inputMovements, IEnumerable<Movement> outputMovements, IEnumerable<InventoryProduct> initialInventories,
            IEnumerable<InventoryProduct> finalInventories)
            GetInventoriesAndMovementsForSystemAndProduct(
             string productId,
             int measurementUnit,
             DateTime date,
             int ownerId,
             IEnumerable<InventoryProduct> inventoryProducts,
             IEnumerable<Movement> movements,
             IEnumerable<SegmentNodeDto> systemNodes)
        {
            var inputMovements = GetSystemInputMovements(movements, productId, measurementUnit, systemNodes, ownerId);
            var outputMovements = GetSystemOutputMovements(movements, productId, measurementUnit, systemNodes, ownerId);

            var initialInventories = inventoryProducts.Where(
                y =>
                systemNodes.Any(x => x.NodeId == y.NodeId) &&
                y.InventoryDate.GetValueOrDefault().Date == date.AddDays(-1).Date &&
                y.ProductId == productId && y.MeasurementUnit == measurementUnit &&
                y.Ownerships.Any(x => x.OwnerId == ownerId));

            var finalInventories = inventoryProducts.Where(
                y =>
                systemNodes.Any(x => x.NodeId == y.NodeId) &&
                y.InventoryDate.GetValueOrDefault().Date == date.Date &&
                y.ProductId == productId && y.MeasurementUnit == measurementUnit &&
                y.Ownerships.Any(x => x.OwnerId == ownerId));

            return (inputMovements, outputMovements, initialInventories, finalInventories);
        }

        private static bool IsProductConvertionInSystem(IEnumerable<SegmentNodeDto> systemNodes, Movement movement)
        {
            return systemNodes.Any(y => y.NodeId == movement.MovementDestination?.DestinationNodeId)
                && systemNodes.Any(y => y.NodeId == movement.MovementSource?.SourceNodeId)
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

        /// <summary>
        /// Gets the output movements.
        /// </summary>
        /// <param name="systemNodes">The segment nodes.</param>
        /// <param name="inventoryProducts">The inventory products.</param>
        /// <param name="movements">The movements.</param>
        /// <returns>The collection of Output Movement.</returns>
        private static IEnumerable<ReportProductByUnit> GetNodeProducts(
            IEnumerable<SegmentNodeDto> systemNodes,
            IEnumerable<InventoryProduct> inventoryProducts,
            IEnumerable<Movement> movements)
        {
            var sourceMovementsForSystemNodes = movements.Where(x => systemNodes.Any(y => y.NodeId == x.MovementSource?.SourceNodeId));
            var destinationMovementsForSystemNodes = movements.Where(x => systemNodes.Any(y => y.NodeId == x.MovementDestination?.DestinationNodeId));
            var inventoriesForSystemNodes = inventoryProducts.Where(x => systemNodes.Any(y => y.NodeId == x.NodeId));

            var movementProducts = sourceMovementsForSystemNodes.Select(x => new ReportProductByUnit
            { ProductId = x.MovementSource?.SourceProductId, MeasurementUnit = x.MeasurementUnit }).Where(y => y != null).Distinct()
                .Union(destinationMovementsForSystemNodes.Select(x => new ReportProductByUnit
                { ProductId = x.MovementDestination?.DestinationProductId, MeasurementUnit = x.MeasurementUnit }).Where(y => y != null).Distinct())
                .Union(inventoriesForSystemNodes.Select(x => new ReportProductByUnit { ProductId = x.ProductId, MeasurementUnit = x.MeasurementUnit }).Where(y => y != null).Distinct());

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

        private IEnumerable<SystemOwnershipCalculation> ProcessForEachOwner(
          IEnumerable<int> owners,
          string productId,
          int measurementUnit,
          DateTime date,
          IEnumerable<InventoryProduct> inventories,
          IEnumerable<Movement> movementsByDate,
          IEnumerable<SegmentNodeDto> systemNodesByDate,
          int ticketId,
          int segmentId,
          int systemId)
        {
            var resultSystemOwnershipCalculation = new List<SystemOwnershipCalculation>();

            foreach (var ownerId in owners)
            {
                var data = GetInventoriesAndMovementsForSystemAndProduct(
                           productId,
                           measurementUnit,
                           date,
                           ownerId,
                           inventories,
                           movementsByDate,
                           systemNodesByDate);
                var result = this.calculateOwnership.CalculateAndRegisterForSystem(
                productId,
                measurementUnit,
                date,
                ownerId,
                ticketId,
                segmentId,
                systemId,
                data.inputMovements,
                data.outputMovements,
                data.initialInventories,
                data.finalInventories,
                movementsByDate,
                systemNodesByDate.Select(x => x.NodeId));
                resultSystemOwnershipCalculation.Add(result);
            }

            return this.calculateOwnership.CalculatePercentageAndRegisterForSystem(resultSystemOwnershipCalculation);
        }

        /// <summary>
        /// Gets the segment nodes asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The collection of Segment Nodes.</returns>
        private async Task<IEnumerable<SystemDto>> GetSegmentSystemsAsync(Ticket ticket)
        {
            var systems = new List<SystemDto>();
            var parameters = new Dictionary<string, object>
            {
                { "@SegmentId", ticket.CategoryElementId },
                { "@StartDate", ticket.StartDate.Date },
                { "@EndDate", ticket.EndDate.Date },
            };

            await this.GetDataFromRepositoryAsync<SystemDto>(
            (systemOutputs) => systems = systemOutputs.ToList(),
            Repositories.Constants.GetAllSystemsInSegmentProcedureName,
            parameters).ConfigureAwait(false);

            return systems.AsEnumerable();
        }

        /// <summary>
        /// Gets the segment nodes asynchronous.
        /// </summary>
        /// <param name="systemId">The system id.</param>
        /// <param name="date">The ticket.</param>
        /// <returns>The collection of Segment Nodes.</returns>
        private async Task<IEnumerable<SegmentNodeDto>> GetSystemNodesAsync(int systemId, DateTime date)
        {
            var systemNodes = new List<SegmentNodeDto>();
            var parameters = new Dictionary<string, object>
            {
                { "@SystemId", systemId },
                { "@StartDate", date.Date },
                { "@EndDate", date.Date },
            };

            await this.GetDataFromRepositoryAsync<SegmentNodeDto>(
            (systemNodesOutput) => systemNodes = systemNodesOutput.ToList(),
            Repositories.Constants.GetAllNodesInSystemProcedureName,
            parameters).ConfigureAwait(false);

            return systemNodes.AsEnumerable();
        }

        private async Task GetDataFromRepositoryAsync<T>(Action<IEnumerable<T>> setter, string storedProcedureName, IDictionary<string, object> parameters)
where T : class, IEntity
        {
            setter(await this.repositoryFactory.CreateRepository<T>()
                            .ExecuteQueryAsync(storedProcedureName, parameters).ConfigureAwait(false));
        }
    }
}
