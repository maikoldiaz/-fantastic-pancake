// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Services
{
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
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Processors.Ownership.Services.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.Response;

    /// <summary>
    /// The Ownership Service.
    /// </summary>
    /// <seealso cref="IOwnershipProcessor" />
    public class OwnershipService : IOwnershipService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<OwnershipService> logger;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The inventory ownership service.
        /// </summary>
        private readonly IInventoryOwnershipService inventoryOwnershipService;

        /// <summary>
        /// The movement ownership service.
        /// </summary>
        private readonly IMovementOwnershipService movementOwnershipService;

        /// <summary>
        /// Registration strategy factory.
        /// </summary>
        private readonly IRegistrationStrategyFactory registrationStrategyFactory;

        /// <summary>
        /// The movement service.
        /// </summary>
        private readonly IOwnershipResultService movementService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipService" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="unitOfWorkFactory">The unit of work.</param>
        /// <param name="inventoryOwnershipService">The inventory ownership service.</param>
        /// <param name="movementOwnershipService">The movement ownership service.</param>
        /// <param name="registrationStrategyFactory">The registration strategy factory.</param>
        /// <param name="movementService">The movement service.</param>
        public OwnershipService(
            ITrueLogger<OwnershipService> logger,
            IUnitOfWorkFactory unitOfWorkFactory,
            IInventoryOwnershipService inventoryOwnershipService,
            IMovementOwnershipService movementOwnershipService,
            IRegistrationStrategyFactory registrationStrategyFactory,
            IOwnershipResultService movementService)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));

            this.logger = logger;
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.inventoryOwnershipService = inventoryOwnershipService;
            this.movementOwnershipService = movementOwnershipService;
            this.registrationStrategyFactory = registrationStrategyFactory;
            this.movementService = movementService;
        }

        /// <inheritdoc />
        public IEnumerable<Ownership> BuildOwnershipResults(
            IEnumerable<OwnershipResultInventory> inventoryResultList,
            IEnumerable<OwnershipResultMovement> movementResultList)
        {
            return this.TransformResult(inventoryResultList, movementResultList);
        }

        /// <inheritdoc/>
        public IEnumerable<OwnershipResultMovement> ConsolidateMovementResults(
            IEnumerable<OwnershipResultMovement> movementResultList,
            IEnumerable<PreviousMovementOperationalData> previousMovements,
            int ticketId)
        {
            ArgumentValidators.ThrowIfNull(movementResultList, nameof(movementResultList));
            ArgumentValidators.ThrowIfNull(previousMovements, nameof(previousMovements));

            return movementResultList.Union(previousMovements
                .Where(x => x.OwnerId.HasValue && x.AppliedRule.HasValue)
                .Select(previousMovement => new OwnershipResultMovement
                {
                    OwnerId = previousMovement.OwnerId.Value,
                    OwnershipPercentage = previousMovement.OwnershipPercentage.GetValueOrDefault(),
                    OwnershipVolume = previousMovement.OwnershipVolume.GetValueOrDefault(),
                    AppliedRule = GetAppliedRule(previousMovement.AppliedRule.Value),
                    RuleVersion = previousMovement.RuleVersion,
                    ResponseMovementId = previousMovement.MovementId.ToString(CultureInfo.InvariantCulture),
                    ResponseTicket = ticketId.ToString(CultureInfo.InvariantCulture),
                }));
        }

        /// <inheritdoc/>
        public IEnumerable<OwnershipResultInventory> ConsolidateInventoryResults(
            IEnumerable<OwnershipResultInventory> inventoryResultList,
            IEnumerable<PreviousInventoryOperationalData> previousInventories,
            int ticketId)
        {
            ArgumentValidators.ThrowIfNull(inventoryResultList, nameof(inventoryResultList));
            ArgumentValidators.ThrowIfNull(previousInventories, nameof(previousInventories));

            return inventoryResultList.Union(previousInventories
                .Where(x => x.OwnerId.HasValue && !x.IsOwnershipCalculated)
                .Select(previousInventory => new OwnershipResultInventory
                {
                    OwnerId = previousInventory.OwnerId.Value,
                    OwnershipPercentage = previousInventory.OwnershipPercentage.GetValueOrDefault(),
                    OwnershipVolume = previousInventory.OwnershipVolume.GetValueOrDefault(),
                    AppliedRule = "Propiedad desde la fuente",
                    RuleVersion = 1,
                    ResponseInventoryId = previousInventory.InventoryId.ToString(CultureInfo.InvariantCulture),
                    ResponseTicket = ticketId.ToString(CultureInfo.InvariantCulture),
                }));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Movement>> BuildOwnershipMovementResultsAsync(
            IEnumerable<CommercialMovementsResult> commercialMovementsResults,
            IEnumerable<NewMovement> newMovementsList,
            IEnumerable<CancellationMovementDetail> cancellationMovements,
            int ticketId,
            IUnitOfWork unitOfWork)
        {
            return await this.movementService.BuildOwnershipMovementResultsAsync(commercialMovementsResults, newMovementsList, cancellationMovements, ticketId, unitOfWork).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task RegisterResultsAsync(OwnershipRuleData ownershipRuleData)
        {
            ArgumentValidators.ThrowIfNull(ownershipRuleData, nameof(ownershipRuleData));

            // Updating the node status and node is already inserted in ownership node via sp
            await this.ProcessOwnershipResultsAsync(ownershipRuleData.TicketId).ConfigureAwait(false);

            //// Updating the ownership data
            this.registrationStrategyFactory.OwnershipRegistrationStrategy.Insert(ownershipRuleData.Ownerships, this.unitOfWork);

            if (ownershipRuleData.Movements != null && ownershipRuleData.Movements.Any())
            {
                this.registrationStrategyFactory.MovementRegistrationStrategy.Insert(
                    ownershipRuleData.Movements, this.unitOfWork);
            }

            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task HandleFailureAsync(
            int ticketId,
            IEnumerable<ErrorInfo> errorInfos,
            IEnumerable<OwnershipErrorMovement> ownershipErrorMovements,
            IEnumerable<OwnershipErrorInventory> ownershipErrorInventories,
            bool hasProcessingErrors)
        {
            ArgumentValidators.ThrowIfNull(ownershipErrorMovements, nameof(ownershipErrorMovements));
            ArgumentValidators.ThrowIfNull(ownershipErrorInventories, nameof(ownershipErrorInventories));
            ArgumentValidators.ThrowIfNull(errorInfos, nameof(errorInfos));
            var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();

            var ticket = await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(ticket.TicketGroupId))
            {
                var otherTickets = await ticketRepository.GetAllAsync(t => t.TicketGroupId == ticket.TicketGroupId
                && t.TicketId != ticketId && t.Status == StatusType.PROCESSING).ConfigureAwait(false);
                foreach (var otherTicket in otherTickets)
                {
                    this.logger.LogInformation($"Failing the ticket {otherTicket.TicketId} from group {otherTicket.TicketGroupId}");
                    otherTicket.Status = StatusType.FAILED;
                    otherTicket.ErrorMessage = string.Format(CultureInfo.InvariantCulture, Constants.ErrorMessageForUnprocessTicket, ticketId, ticket.StartDate.ToTrueString());
                    ticketRepository.Update(otherTicket);
                }
            }

            ticket.Status = StatusType.FAILED;
            if (errorInfos.Any())
            {
                ticket.ErrorMessage = string.Join(",", errorInfos.Select(x => x.Message));
            }

            ticketRepository.Update(ticket);

            if (hasProcessingErrors)
            {
                await this.HandleErrorsFailureAsync(ownershipErrorInventories, ownershipErrorMovements, ticketId).ConfigureAwait(false);
            }
            else
            {
                await this.HandleValidationFailureAsync(ticketId).ConfigureAwait(false);
            }

            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Ticket>> GetUnprocessedTicketsAsync(int ticketId)
        {
            ArgumentValidators.ThrowIfNull(ticketId, nameof(ticketId));
            var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();
            var ticket = await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);
            return await ticketRepository.GetAllAsync(t => t.TicketGroupId == ticket.TicketGroupId && t.Status == StatusType.PROCESSING).ConfigureAwait(false);
        }

        private static string GetAppliedRule(int appliedRuleId)
        {
            switch (appliedRuleId)
            {
                case -1:
                    return "Propiedad por defecto";
                case 0:
                    return "Propiedad desde la fuente";
                default:
                    return appliedRuleId.ToString(CultureInfo.InvariantCulture);
            }
        }

        private async Task ProcessOwnershipResultsAsync(int ticketId)
        {
            var ownershipNodeRepository = this.unitOfWork.CreateRepository<OwnershipNode>();

            var ownershipNodes = await ownershipNodeRepository.GetAllAsync(o => o.TicketId == ticketId).ConfigureAwait(false);
            ownershipNodes.ForEach(ownershipNode =>
            {
                ownershipNode.Status = StatusType.PROCESSED;
                ownershipNode.OwnershipStatus = OwnershipNodeStatusType.OWNERSHIP;
            });

            ownershipNodeRepository.UpdateAll(ownershipNodes);
        }

        private async Task HandleErrorsFailureAsync(
            IEnumerable<OwnershipErrorInventory> inventoryList,
            IEnumerable<OwnershipErrorMovement> movementList,
            int ticketId)
        {
            ArgumentValidators.ThrowIfNull(inventoryList, nameof(inventoryList));
            ArgumentValidators.ThrowIfNull(movementList, nameof(movementList));

            var ownershipNodeErrorRepository = this.unitOfWork.CreateRepository<OwnershipNodeError>();
            var ownershipNodeRepo = this.unitOfWork.CreateRepository<OwnershipNode>();

            var existingOwnershipNodes = await ownershipNodeRepo.GetAllAsync(o => o.TicketId == ticketId).ConfigureAwait(false);

            foreach (var i in inventoryList)
            {
                var existingOwnershipNode = existingOwnershipNodes.FirstOrDefault(o => o.NodeId == i.NodeId);
                existingOwnershipNode.Status = StatusType.FAILED;
                existingOwnershipNode.OwnershipStatus = OwnershipNodeStatusType.FAILED;
                ownershipNodeRepo.Update(existingOwnershipNode);
                var existingInventoryProductOwnershipNodeError = await ownershipNodeErrorRepository.GetAllAsync(
                    o => o.InventoryProductId == i.InventoryId
                    && o.OwnershipNodeId == existingOwnershipNode.NodeId).ConfigureAwait(false);

                if (!existingInventoryProductOwnershipNodeError.Any())
                {
                    this.logger.LogInformation($"Inserting error for ownership node {existingOwnershipNode.NodeId}");
                    ownershipNodeErrorRepository.Insert(
                               new OwnershipNodeError
                               {
                                   InventoryProductId = i.InventoryId,
                                   ErrorMessage = i.ErrorDescription,
                                   ExecutionDate = i.ExecutionDate,
                                   OwnershipNode = existingOwnershipNode,
                               });
                }
            }

            foreach (var m in movementList.Where(x => x.SourceNodeId != 0))
            {
                var existingOwnershipNode = existingOwnershipNodes.FirstOrDefault(o => o.NodeId == m.SourceNodeId);
                existingOwnershipNode.Status = StatusType.FAILED;
                existingOwnershipNode.OwnershipStatus = OwnershipNodeStatusType.FAILED;
                ownershipNodeRepo.Update(existingOwnershipNode);
                var existingMovementOwnershipNodeError = await ownershipNodeErrorRepository.GetAllAsync(
                    o => o.MovementTransactionId == m.MovementId
                    && o.OwnershipNodeId == existingOwnershipNode.NodeId).ConfigureAwait(false);
                if (!existingMovementOwnershipNodeError.Any())
                {
                    ownershipNodeErrorRepository.Insert(
                                new OwnershipNodeError
                                {
                                    MovementTransactionId = m.MovementId,
                                    ExecutionDate = m.ExecutionDate,
                                    ErrorMessage = m.ErrorDescription,
                                    OwnershipNode = existingOwnershipNode,
                                });
                }
            }

            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);

            var sentOwnershipNodes = await ownershipNodeRepo.GetAllAsync(o => o.TicketId == ticketId && o.OwnershipStatus == OwnershipNodeStatusType.SENT).ConfigureAwait(false);
            foreach (var deleteOwnershipNode in sentOwnershipNodes)
            {
                ownershipNodeRepo.Delete(deleteOwnershipNode);
            }
        }

        private async Task HandleValidationFailureAsync(
            int ticketId)
        {
            var ownershipNodeRepository = this.unitOfWork.CreateRepository<OwnershipNode>();

            var ownershipNodes = await ownershipNodeRepository.GetAllAsync(o => o.TicketId == ticketId).ConfigureAwait(false);
            ownershipNodes.ForEach(ownershipNode =>
            {
                ownershipNode.Status = StatusType.FAILED;
                ownershipNode.OwnershipStatus = OwnershipNodeStatusType.FAILED;
            });

            ownershipNodeRepository.UpdateAll(ownershipNodes);
        }

        private IEnumerable<Ownership> TransformResult(
            IEnumerable<OwnershipResultInventory> inventoryList, IEnumerable<OwnershipResultMovement> movementList)
        {
            var inventoryOwnerships = this.inventoryOwnershipService.GetInventoryOwnerships(inventoryList);
            var movementOwnerships = this.movementOwnershipService.GetMovementOwnerships(movementList);

            return inventoryOwnerships.Concat(movementOwnerships);
        }
    }
}
