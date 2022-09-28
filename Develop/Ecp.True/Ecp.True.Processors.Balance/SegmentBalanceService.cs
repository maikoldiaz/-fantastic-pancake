// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SegmentBalanceService.cs" company="Microsoft">
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
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Balance.Calculation.Interfaces;

    /// <summary>
    /// The segment balance service.
    /// </summary>
    public class SegmentBalanceService : SystemSegmentBalanceBase, ISegmentBalanceService
    {
        /// <summary>
        /// The segment calculator.
        /// </summary>
        private readonly ISegmentCalculator segmentCalculator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentBalanceService" /> class.
        /// </summary>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <param name="segmentCalculator">The segment calculator.</param>
        public SegmentBalanceService(
            ISegmentCalculator segmentCalculator)
        {
            this.segmentCalculator = segmentCalculator;
        }

        /// <summary>
        /// Processes the ownership asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>The Task.</returns>
        public async Task<IEnumerable<SegmentUnbalance>> ProcessSegmentAsync(int ticketId, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            var ticketRepository = unitOfWork.CreateRepository<Ticket>();
            var ticket = await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);
            var inventoriesAndMovements = await GetInventoriesAndMovementsAsync(ticket, unitOfWork).ConfigureAwait(false);
            var nodes = await this.GetSegmentNodesAsync(ticket, unitOfWork).ConfigureAwait(false);
            var segmentUnbalances = new List<SegmentUnbalance>();

            var date = ticket.StartDate.Date;
            while (date <= ticket.EndDate)
            {
                var movementsByDate = inventoriesAndMovements.movements.Where(x => x.OperationalDate.Date == date.Date);
                var inventoriesByPreviousAndCurrentDate = inventoriesAndMovements.inventoryProducts.Where(
                    x =>
                    x.InventoryDate.GetValueOrDefault().Date == date.Date || x.InventoryDate.GetValueOrDefault().Date == date.AddDays(-1).Date);
                var segmentNodesByDate = nodes.segmentNodes.Where(x => x.OperationDate.Date == date.Date);
                var initialNodesByDate = nodes.initialNodes.Where(x => x.OperationDate.Date == date.Date);
                var finalNodesByDate = nodes.finalNodes.Where(x => x.OperationDate.Date == date.Date);
                var products = GetDateNodeProductsAndOwners(segmentNodesByDate, inventoriesByPreviousAndCurrentDate, movementsByDate);
                foreach (var product in products)
                {
                    var data = GetInventoriesAndMovementsForSegmentAndProduct(
                               product,
                               date,
                               inventoriesByPreviousAndCurrentDate,
                               movementsByDate,
                               segmentNodesByDate,
                               initialNodesByDate,
                               finalNodesByDate);
                    var segmentUnbalance = this.segmentCalculator.CalculateAndGetSegmentUnbalance(
                               product,
                               date,
                               ticketId,
                               ticket.CategoryElementId,
                               data.inputMovements,
                               data.outputMovements,
                               data.initialInventories,
                               data.finalInventories,
                               data.sourceNodeMovements,
                               data.destinationNodeMovements,
                               unitOfWork);
                    segmentUnbalances.Add(segmentUnbalance);
                }

                date = date.AddDays(1);
            }

            return segmentUnbalances;
        }

        /// <summary>
        /// Gets the input movements.
        /// </summary>
        /// <param name="movements">The movements.</param>
        /// <param name="productId">The product.</param>
        /// <param name="segmentNodes">The segment nodes.</param>
        /// <param name="initialSegmentNodes">The initial segment nodes.</param>
        /// <returns>The collection of Input Movement.</returns>
        private static IEnumerable<Movement> GetSegmentInputMovements(
            IEnumerable<Movement> movements,
            string productId,
            IEnumerable<SegmentNodeDto> segmentNodes,
            IEnumerable<SegmentNodeDto> initialSegmentNodes)
        {
            var inputMovements = GetSegmentInputMovementsByProduct(movements, productId);
            return inputMovements.Where(x => (IsProductConvertionInSegment(segmentNodes, x) ||
            (initialSegmentNodes.Any(y => y.NodeId == x.MovementDestination?.DestinationNodeId) && !segmentNodes.Any(y => y.NodeId == x.MovementSource?.SourceNodeId))));
        }

        private static bool IsProductConvertionInSegment(IEnumerable<SegmentNodeDto> segmentNodes, Movement movement)
        {
            return segmentNodes.Any(y => y.NodeId == movement.MovementDestination?.DestinationNodeId)
                && segmentNodes.Any(y => y.NodeId == movement.MovementSource?.SourceNodeId)
                && movement.MovementSource?.SourceProductId != movement.MovementDestination?.DestinationProductId;
        }

        /// <summary>
        /// Gets the input movements by product.
        /// </summary>
        /// <param name="movements">The movements.</param>
        /// <param name="productId">The product.</param>
        /// <returns>The collection of Input Movement by Product.</returns>
        private static IEnumerable<Movement> GetSegmentInputMovementsByProduct(IEnumerable<Movement> movements, string productId)
        {
            return movements.Where(x => (x.MessageTypeId == (int)MessageType.Movement)
                       && x.MovementDestination?.DestinationProductId == productId);
        }

        /// <summary>
        /// Gets the output movements.
        /// </summary>
        /// <param name="movements">The movements.</param>
        /// <param name="productId">The product.</param>
        /// <param name="segmentNodes">The segment nodes.</param>
        /// <param name="finalSegmentNodes">The final segment nodes.</param>
        /// <returns>The collection of Output Movement.</returns>
        private static IEnumerable<Movement> GetSegmentOutputMovements(
            IEnumerable<Movement> movements,
            string productId,
            IEnumerable<SegmentNodeDto> segmentNodes,
            IEnumerable<SegmentNodeDto> finalSegmentNodes)
        {
            var outputMovements = GetSegmentOutputMovementsByProduct(movements, productId);
            return outputMovements.Where(x => (IsProductConvertionInSegment(segmentNodes, x) ||
            (finalSegmentNodes.Any(y => y.NodeId == x.MovementSource?.SourceNodeId) && !segmentNodes.Any(y => y.NodeId == x.MovementDestination?.DestinationNodeId))));
        }

        /// <summary>
        /// Gets the output movements by product.
        /// </summary>
        /// <param name="movements">The movements.</param>
        /// <param name="productId">The product.</param>
        /// <returns>The collection of Output Movement by Product.</returns>
        private static IEnumerable<Movement> GetSegmentOutputMovementsByProduct(IEnumerable<Movement> movements, string productId)
        {
            return movements.Where(x => (x.MessageTypeId == (int)MessageType.Movement)
                       && x.MovementSource?.SourceProductId == productId);
        }

        /// <summary>
        /// Gets the inventories and movements.
        /// </summary>
        /// <param name="productId">The product.</param>
        /// <param name="date">The date.</param>
        /// <param name="inventoryProducts">The inventory products.</param>
        /// <param name="movements">The movements.</param>
        /// <param name="segmentNodes">The segment nodes.</param>
        /// <param name="initialSegmentNodes">The initial segment nodes.</param>
        /// <param name="finalSegmentNodes">The final segment nodes.</param>
        /// <returns>The Input and Output Movements, Initial and Final inventories.</returns>
        private static (IEnumerable<Movement> inputMovements, IEnumerable<Movement> outputMovements, IEnumerable<InventoryProduct> initialInventories,
            IEnumerable<InventoryProduct> finalInventories, IEnumerable<Movement> sourceNodeMovements, IEnumerable<Movement> destinationNodeMovements)
            GetInventoriesAndMovementsForSegmentAndProduct(
             string productId,
             DateTime date,
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

            var inputMovements = GetSegmentInputMovements(movements, productId, segmentNodes, initialSegmentNodes);
            var outputMovements = GetSegmentOutputMovements(movements, productId, segmentNodes, finalSegmentNodes);
            var nodeMovements = GetNodeMovements(segmentNodes, movements);

            var initialInventories = inventoryProducts.Where(
                y =>
                segmentNodes.Any(x => x.NodeId == y.NodeId) &&
                y.InventoryDate.GetValueOrDefault().Date == date.AddDays(-1).Date &&
                y.ProductId == productId);

            var finalInventories = inventoryProducts.Where(
                y =>
                segmentNodes.Any(x => x.NodeId == y.NodeId) &&
                y.InventoryDate.GetValueOrDefault().Date == date.Date &&
                y.ProductId == productId);

            return (inputMovements, outputMovements, initialInventories, finalInventories, nodeMovements.sourceNodeMovements, nodeMovements.destinationNodeMovements);
        }

        private static (IEnumerable<Movement> sourceNodeMovements, IEnumerable<Movement> destinationNodeMovements)
            GetNodeMovements(IEnumerable<SegmentNodeDto> segmentNodes, IEnumerable<Movement> movements)
        {
            var sourceNodeMovements = movements.Where(x => segmentNodes.Any(y => y.NodeId == x.MovementSource?.SourceNodeId));
            var destinationNodeMovements = movements.Where(x => segmentNodes.Any(y => y.NodeId == x.MovementDestination?.DestinationNodeId));
            return (sourceNodeMovements, destinationNodeMovements);
        }

        /// <summary>
        /// Gets the output movements.
        /// </summary>
        /// <param name="segmentNodes">The segment nodes.</param>
        /// <param name="inventoryProducts">The inventory products.</param>
        /// <param name="movements">The movements.</param>
        /// <returns>The collection of Output Movement.</returns>
        private static IEnumerable<string> GetDateNodeProductsAndOwners(
            IEnumerable<SegmentNodeDto> segmentNodes,
            IEnumerable<InventoryProduct> inventoryProducts,
            IEnumerable<Movement> movements)
        {
            var nodeMovements = GetNodeMovements(segmentNodes, movements);
            var movementProducts = nodeMovements.sourceNodeMovements.Select(x => x.MovementSource?.SourceProductId).Distinct()
                .Union(nodeMovements.destinationNodeMovements.Select(x => x.MovementDestination?.DestinationProductId).Distinct());
            var inventoriesForSegmentNodes = inventoryProducts.Where(x => segmentNodes.Any(y => y.NodeId == x.NodeId));
            var invProducts = inventoriesForSegmentNodes.Select(x => x.ProductId).Distinct();
            var products = movementProducts.Union(invProducts);
            products = products.Where(x => x != null);

            return products;
        }

        private static async Task GetDataFromRepositoryAsync<T>(Action<IEnumerable<T>> setter, string storedProcedureName, IDictionary<string, object> parameters, IUnitOfWork unitOfWork)
where T : class, IEntity
        {
            setter(await unitOfWork.CreateRepository<T>()
                            .ExecuteQueryAsync(storedProcedureName, parameters).ConfigureAwait(false));
        }

        /// <summary>
        /// Gets the segment nodes asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The collection of Segment Nodes.</returns>
        private async Task<(IEnumerable<SegmentNodeDto> segmentNodes, IEnumerable<SegmentNodeDto> initialNodes, IEnumerable<SegmentNodeDto> finalNodes)>
            GetSegmentNodesAsync(Ticket ticket, IUnitOfWork unitOfWork)
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
            GetDataFromRepositoryAsync<SegmentNodeDto>(
            (nodes) => segmentNodes = nodes.ToList(),
            Repositories.Constants.GetSegmentNodesProcedureName,
            parameters,
            unitOfWork),
            GetDataFromRepositoryAsync<SegmentNodeDto>(
            (nodes) => initialNodes = nodes.ToList(),
            Repositories.Constants.GetInitialNodesProcedureName,
            parameters,
            unitOfWork),
            GetDataFromRepositoryAsync<SegmentNodeDto>(
            (nodes) => finalNodes = nodes.ToList(),
            Repositories.Constants.GetFinalNodesProcedureName,
            parameters,
            unitOfWork)).ConfigureAwait(false);

            return (segmentNodes: segmentNodes.AsEnumerable(), initialNodes: initialNodes.AsEnumerable(), finalNodes: finalNodes.AsEnumerable());
        }
    }
}
