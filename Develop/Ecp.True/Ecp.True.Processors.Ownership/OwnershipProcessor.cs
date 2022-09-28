// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipProcessor.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.Report;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Processors.Ownership.Services.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.OwnershipRules.Response;

    /// <summary>
    /// The OwnershipProcessor.
    /// </summary>
    public class OwnershipProcessor : OwnershipProcessorBase, IOwnershipProcessor
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The ownership service.
        /// </summary>
        private readonly IOwnershipService ownershipService;

        /// <summary>
        /// The calculate ownership.
        /// </summary>
        private readonly ICalculateOwnership calculateOwnership;

        /// <summary>
        /// The calculate ownership.
        /// </summary>
        private readonly ISegmentOwnershipCalculationService segmentOwnershipCalculationService;

        /// <summary>
        /// The calculate ownership.
        /// </summary>
        private readonly ISystemOwnershipCalculationService systemOwnershipCalculationService;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<OwnershipProcessor> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipProcessor" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="ownershipService">The ownership service.</param>
        /// <param name="calculateOwnership">The calculate ownership.</param>
        /// <param name="segmentOwnershipCalculationService">The segment ownership calculation service.</param>
        /// <param name="systemOwnershipCalculationService">The system ownership calculation service.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <param name="logger">The logger.</param>
        public OwnershipProcessor(
            IUnitOfWorkFactory unitOfWorkFactory,
            IOwnershipService ownershipService,
            ICalculateOwnership calculateOwnership,
            ISegmentOwnershipCalculationService segmentOwnershipCalculationService,
            ISystemOwnershipCalculationService systemOwnershipCalculationService,
            IAzureClientFactory azureClientFactory,
            IRepositoryFactory repositoryFactory,
            ITrueLogger<OwnershipProcessor> logger)
            : base(repositoryFactory)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.ownershipService = ownershipService;
            this.calculateOwnership = calculateOwnership;
            this.segmentOwnershipCalculationService = segmentOwnershipCalculationService;
            this.systemOwnershipCalculationService = systemOwnershipCalculationService;
            this.azureClientFactory = azureClientFactory;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Tuple<IEnumerable<OwnershipCalculation>, IEnumerable<SegmentOwnershipCalculation>, IEnumerable<SystemOwnershipCalculation>>>
            CalculateOwnershipAsync(int ticketId)
        {
            ArgumentValidators.ThrowIfNull(ticketId, nameof(ticketId));

            var finalOwnershipCalculations = new List<OwnershipCalculation>();
            var ticket = await this.GetTicketAsync(ticketId).ConfigureAwait(false);

            var (inventoryProducts, movements) = await this.GetInventoriesAndMovementsAsync(ticket).ConfigureAwait(false);
            var calculationDate = ticket.StartDate.Date;

            while (calculationDate <= ticket.EndDate.Date)
            {
                var movementsByDate = movements.Where(x => x.OperationalDate.Date == calculationDate.Date);
                var inventoriesByPreviousAndCurrentDate = inventoryProducts.Where(
                    x =>
                    x.InventoryDate.GetValueOrDefault().Date == calculationDate.Date || x.InventoryDate.GetValueOrDefault().Date == calculationDate.AddDays(-1).Date);

                var destinationNodes = movementsByDate.Select(a => Convert.ToInt32(a.MovementDestination?.DestinationNodeId, CultureInfo.InvariantCulture)).Where(x => x != 0).Distinct();
                var sourceNodes = movementsByDate.Select(a => Convert.ToInt32(a.MovementSource?.SourceNodeId, CultureInfo.InvariantCulture)).Where(x => x != 0).Distinct();
                var inventoryNodes = inventoriesByPreviousAndCurrentDate.Select(a => a.NodeId).Where(x => x != 0).Distinct();
                var nodesByDate = destinationNodes.Union(sourceNodes).Union(inventoryNodes);
                foreach (var node in nodesByDate)
                {
                    var products = GetProducts(movementsByDate, inventoriesByPreviousAndCurrentDate, node);
                    foreach (ReportProductByUnit product in products)
                    {
                        var distinctOwners = GetOwners(
                            movementsByDate,
                            inventoriesByPreviousAndCurrentDate,
                            product.ProductId,
                            (int)product.MeasurementUnit,
                            new List<int> { node });

                        finalOwnershipCalculations.AddRange(this.ProcessForEachOwner(
                            distinctOwners,
                            node,
                            product.ProductId,
                            (int)product.MeasurementUnit,
                            calculationDate,
                            movementsByDate,
                            inventoriesByPreviousAndCurrentDate,
                            ticketId));
                    }
                }

                calculationDate = calculationDate.AddDays(1);
            }

            var finalSegmentOwnershipCalculation = await this.segmentOwnershipCalculationService.ProcessSegmentOwnershipAsync(
                inventoryProducts,
                movements,
                ticketId).ConfigureAwait(false);

            var finalSystemOwnershipCalculations = await this.systemOwnershipCalculationService.ProcessSystemOwnershipAsync(
                inventoryProducts,
                movements,
                ticketId).ConfigureAwait(false);

            return Tuple.Create(finalOwnershipCalculations.AsEnumerable(), finalSegmentOwnershipCalculation, finalSystemOwnershipCalculations);
        }

        /// <inheritdoc/>
        public async Task CompleteAsync(OwnershipRuleData ownershipRuleData)
        {
            ArgumentValidators.ThrowIfNull(ownershipRuleData, nameof(ownershipRuleData));
            var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();

            // Updating the calculation data
            await this.InsertCalculationDataAsync(
                ownershipRuleData.TicketId,
                ownershipRuleData.OwnershipCalculations,
                ownershipRuleData.SegmentOwnershipCalculations,
                ownershipRuleData.SystemOwnershipCalculations).ConfigureAwait(false);

            // Updating the ticket status
            if (!ownershipRuleData.OwnershipNodeId.HasValue)
            {
                await UpdateTicketSuccessAsync(ticketRepository, ownershipRuleData.TicketId).ConfigureAwait(false);
            }

            // Update the ownership node status
            if (ownershipRuleData.OwnershipNodeId.HasValue)
            {
                await this.UpdateOwnershipNodeStatusAsync(ownershipRuleData.OwnershipNodeId.Value).ConfigureAwait(false);
            }

            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);

            // Updating the unprocessed ticket
            if (!ownershipRuleData.OwnershipNodeId.HasValue)
            {
                await this.UpdateUnprocessedTicketAsync(ownershipRuleData.TicketId).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the output movements.
        /// </summary>
        /// <param name="nodeId">The node.</param>
        /// <param name="movements">The movements.</param>
        /// <param name="productId">The product.</param>
        /// <param name="measurementUnit">The measurement Unit.</param>
        /// <param name="date">The date.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <returns>The collection of Output Movement.</returns>
        private static IEnumerable<Movement> GetOutputMovements(
            int nodeId,
            IEnumerable<Movement> movements,
            string productId,
            int measurementUnit,
            DateTime date,
            int ownerId)
        {
            var outputMovements = movements.Where(x => MessageTypeMovementValidation(x)
                        && x.MovementSource?.SourceNodeId == nodeId);
            return outputMovements.Where(x =>
                        x.MovementSource?.SourceProductId == productId && x.OperationalDate.Date == date.Date && x.MeasurementUnit == measurementUnit
                        && x.Ownerships.Any(y => y.OwnerId == ownerId));
        }

        private static IEnumerable<ReportProductByUnit> GetProducts(IEnumerable<Movement> movements, IEnumerable<InventoryProduct> inventories, int nodeId)
        {
            var sourceNodeMovements = movements.Where(x => x.MovementSource?.SourceNodeId == nodeId);
            var destinationNodeMovements = movements.Where(x => x.MovementDestination?.DestinationNodeId == nodeId);
            var nodeInventories = inventories.Where(x => x.NodeId == nodeId);

            var movementProducts = sourceNodeMovements.Select(x => new ReportProductByUnit
                                  { ProductId = x.MovementSource?.SourceProductId, MeasurementUnit = x.MeasurementUnit }).Where(y => y.ProductId != null).Distinct()
                                  .Union(destinationNodeMovements.Select(x => new ReportProductByUnit
                                  { ProductId = x.MovementDestination?.DestinationProductId, MeasurementUnit = x.MeasurementUnit }).Where(y => y.ProductId != null).Distinct())
                                  .Union(nodeInventories.Select(x => new ReportProductByUnit { ProductId = x.ProductId, MeasurementUnit = x.MeasurementUnit })
                                  .Where(y => y.ProductId != null).Distinct());

            var consolidatemovementProductUnit = movementProducts
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

            return consolidatemovementProductUnit;
        }

        private static IEnumerable<Movement> GetInputMovements(
            int nodeId,
            IEnumerable<Movement> movements,
            string productId,
            int measurementUnit,
            DateTime date,
            int ownerId)
        {
            var inputMovements = movements.Where(x => MessageTypeMovementValidation(x)
                                    && x.MovementDestination?.DestinationNodeId == nodeId);
            return inputMovements.Where(x =>
                         x.MovementDestination?.DestinationProductId == productId && x.OperationalDate.Date == date.Date && x.MeasurementUnit == measurementUnit
                         && x.Ownerships.Any(y => y.OwnerId == ownerId));
        }

        /// <summary>
        /// Gets the inventories and movements.
        /// </summary>
        /// <param name="nodeId">The node.</param>
        /// <param name="productId">The product.</param>
        /// <param name="measurementUnit">The measurement Unit.</param>
        /// <param name="date">The date.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <param name="inventoryProducts">The inventory products.</param>
        /// <param name="movements">The movements.</param>
        /// <returns>
        /// The Input and Output Movements, Initial and Final inventories.
        /// </returns>
        private static (IEnumerable<Movement> inputMovements, IEnumerable<Movement> outputMovements, IEnumerable<InventoryProduct> initialInventories, IEnumerable<InventoryProduct> finalInventories)
            GetInventoriesAndMovements(
             int nodeId,
             string productId,
             int measurementUnit,
             DateTime date,
             int ownerId,
             IEnumerable<InventoryProduct> inventoryProducts,
             IEnumerable<Movement> movements)
        {
            var inputMovements = GetInputMovements(nodeId, movements, productId, measurementUnit, date, ownerId);
            var outputMovements = GetOutputMovements(nodeId, movements, productId, measurementUnit, date, ownerId);

            var initialInventories = inventoryProducts.Where(
                y =>
                y.NodeId == nodeId &&
                y.InventoryDate.GetValueOrDefault().Date == date.AddDays(-1).Date &&
                y.ProductId == productId && y.MeasurementUnit == measurementUnit &&
                y.Ownerships.Any(x => x.OwnerId == ownerId));

            var finalInventories = inventoryProducts.Where(
                y =>
                y.NodeId == nodeId &&
                y.InventoryDate.GetValueOrDefault().Date == date.Date &&
                y.ProductId == productId && y.MeasurementUnit == measurementUnit &&
                y.Ownerships.Any(x => x.OwnerId == ownerId));

            return (inputMovements, outputMovements, initialInventories, finalInventories);
        }

        private static (IEnumerable<Movement> inputMovements, IEnumerable<Movement> outputMovements, IEnumerable<InventoryProduct> initialInventories, IEnumerable<InventoryProduct> finalInventories)
        GetData(int nodeId, string productId, int measurementUnit, DateTime date, IEnumerable<InventoryProduct> inventoryProducts, IEnumerable<Movement> movements, int ownerId)
        {
            return GetInventoriesAndMovements(
                                              nodeId,
                                              productId,
                                              measurementUnit,
                                              date,
                                              ownerId,
                                              inventoryProducts,
                                              movements);
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

        private static async Task UpdateTicketSuccessAsync(IRepository<Ticket> ticketRepository, int ticketId)
        {
            var ticketDetails = await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);
            ticketDetails.Status = ticketDetails.Status == StatusType.CONCILIATIONFAILED ? StatusType.CONCILIATIONFAILED : StatusType.PROCESSED;
            ticketRepository.Update(ticketDetails);
        }

        private async Task UpdateOwnershipNodeStatusAsync(int ownershipNodeId)
        {
            var ownershipNodeRepository = this.unitOfWork.CreateRepository<OwnershipNode>();
            var ownershipNode = await ownershipNodeRepository.GetByIdAsync(ownershipNodeId).ConfigureAwait(false);
            if (ownershipNode != null)
            {
                ownershipNode.OwnershipStatus = OwnershipNodeStatusType.PUBLISHED;
                ownershipNodeRepository.Update(ownershipNode);
            }
        }

        private async Task UpdateUnprocessedTicketAsync(int ticketId)
        {
            try
            {
                var unprocessedTickets = await this.ownershipService.GetUnprocessedTicketsAsync(ticketId).ConfigureAwait(false);
                var nextTicket = unprocessedTickets.OrderBy(t => t.StartDate).FirstOrDefault();
                if (nextTicket != null)
                {
                    this.logger.LogInformation($"Next ownership processing is requested for un processed ticket {nextTicket.TicketId}");
                    var client = this.azureClientFactory.GetQueueClient(
                                    QueueConstants.OwnershipQueue);
                    await client.QueueSessionMessageAsync(nextTicket.TicketId, nextTicket.TicketId.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
                }
                else
                {
                    this.logger.LogInformation($"No more tickets in processing state for particular group of ticket with ticketId: {ticketId}");
                }
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, $"{ticketId}", $"Error occurred while processing unprocessed ticket: {exception.Message}");

                await this.ownershipService.HandleFailureAsync(
                    ticketId,
                    new List<ErrorInfo> { new ErrorInfo(Constants.TechnicalExceptionErrorMessage) },
                    new List<OwnershipErrorMovement>(),
                    new List<OwnershipErrorInventory>(),
                    false).ConfigureAwait(false);
            }
        }

        private async Task<(IEnumerable<InventoryProduct> inventoryProducts, IEnumerable<Movement> movements)> GetInventoriesAndMovementsAsync(Ticket ticket)
        {
            var movements = await this.GetMovementsAsync(ticket.TicketId).ConfigureAwait(false);
            var inventoryProducts = await this.GetInventoriesAsync(ticket).ConfigureAwait(false);

            return (inventoryProducts, movements);
        }

        private async Task<IEnumerable<Movement>> GetMovementsAsync(int ticketId)
        {
            var movementRepository = this.unitOfWork.CreateRepository<Movement>();
            var result = await movementRepository.GetAllAsync(
                 a =>
                 a.OwnershipTicketId == ticketId,
                 "MovementSource",
                 "MovementDestination",
                 "Ownerships").ConfigureAwait(false);
            return result;
        }

        private async Task<IEnumerable<InventoryProduct>> GetInventoriesAsync(Ticket ticket)
        {
            var inventoryProductRepository = this.unitOfWork.CreateRepository<InventoryProduct>();
            return await inventoryProductRepository.GetAllAsync(
                 a => a.TicketId != null &&
                 a.SegmentId == ticket.CategoryElementId &&
                 a.InventoryDate >= ticket.StartDate.AddDays(-1).Date && a.InventoryDate < ticket.EndDate.Date.AddDays(1),
                 "Ownerships").ConfigureAwait(false);
        }

        private async Task<Ticket> GetTicketAsync(int ticketId)
        {
            var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();
            return await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);
        }

        private async Task InsertCalculationDataAsync(
            int ticketId,
            IEnumerable<OwnershipCalculation> ownershipCalculations,
            IEnumerable<SegmentOwnershipCalculation> segmentOwnershipCalculations,
            IEnumerable<SystemOwnershipCalculation> systemOwnershipCalculations)
        {
            await this.DeleteEIfExistingAndInsertOwnershipCalculationDataAsync(ticketId, ownershipCalculations).ConfigureAwait(false);
            await this.DeleteEIfExistingAndInsertSegmentOwnershipCalculationDataAsync(ticketId, segmentOwnershipCalculations).ConfigureAwait(false);
            await this.DeleteEIfExistingAndInsertSystemOwnershipCalculationDataAsync(ticketId, systemOwnershipCalculations).ConfigureAwait(false);
        }

        private async Task DeleteEIfExistingAndInsertOwnershipCalculationDataAsync(
            int ticketId,
            IEnumerable<OwnershipCalculation> ownershipCalculations)
        {
            var ownershipCalculationRepository = this.unitOfWork.CreateRepository<OwnershipCalculation>();
            var existingOwnershipCalculations = await ownershipCalculationRepository.QueryAllAsync(
                        x => x.OwnershipTicketId == ticketId).ConfigureAwait(false);
            ownershipCalculationRepository.DeleteAll(existingOwnershipCalculations);
            ownershipCalculationRepository.InsertAll(ownershipCalculations);
        }

        private async Task DeleteEIfExistingAndInsertSegmentOwnershipCalculationDataAsync(
            int ticketId,
            IEnumerable<SegmentOwnershipCalculation> segmentOwnershipCalculations)
        {
            var segmentOwnershipCalculationRepository = this.unitOfWork.CreateRepository<SegmentOwnershipCalculation>();
            var existingOwnershipCalculations = await segmentOwnershipCalculationRepository.QueryAllAsync(
            x => x.OwnershipTicketId == ticketId).ConfigureAwait(false);
            segmentOwnershipCalculationRepository.DeleteAll(existingOwnershipCalculations);
            segmentOwnershipCalculationRepository.InsertAll(segmentOwnershipCalculations);
        }

        private async Task DeleteEIfExistingAndInsertSystemOwnershipCalculationDataAsync(
            int ticketId,
            IEnumerable<SystemOwnershipCalculation> systemOwnershipCalculations)
        {
            var systemOwnershipCalculationRepository = this.unitOfWork.CreateRepository<SystemOwnershipCalculation>();
            var existingOwnershipCalculations = await systemOwnershipCalculationRepository.QueryAllAsync(
                 x => x.OwnershipTicketId == ticketId).ConfigureAwait(false);
            systemOwnershipCalculationRepository.DeleteAll(existingOwnershipCalculations);
            systemOwnershipCalculationRepository.InsertAll(systemOwnershipCalculations);
        }

        private IEnumerable<OwnershipCalculation> ProcessForEachOwner(
          IEnumerable<int> owners,
          int nodeId,
          string productId,
          int measurementUnit,
          DateTime date,
          IEnumerable<Movement> movements,
          IEnumerable<InventoryProduct> inventories,
          int ticketId)
        {
            var resultOwnershipCalculation = new List<OwnershipCalculation>();

            foreach (var ownerId in owners)
            {
                var (inputMovements, outputMovements, initialInventories, finalInventories) = GetData(nodeId, productId, measurementUnit, date, inventories, movements, ownerId);

                var result = this.calculateOwnership.Calculate(
                                nodeId,
                                productId,
                                measurementUnit,
                                date,
                                ownerId,
                                ticketId,
                                inputMovements,
                                outputMovements,
                                initialInventories,
                                finalInventories,
                                movements);

                resultOwnershipCalculation.Add(result);
            }

            return this.calculateOwnership.CalculatePercentageAndRegister(resultOwnershipCalculation);
        }
    }
}
