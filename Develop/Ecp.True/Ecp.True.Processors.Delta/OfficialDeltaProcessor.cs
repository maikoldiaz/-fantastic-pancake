// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
// cSpell:ignore cálculo
namespace Ecp.True.Processors.Delta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.Interfaces;

    /// <summary>
    /// The official delta ticket processor.
    /// </summary>
    public class OfficialDeltaProcessor : ProcessorBase, IOfficialDeltaProcessor
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<OfficialDeltaProcessor> logger;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The ownership rule service.
        /// </summary>
        private readonly IExecutionChainBuilder executionChainBuilder;

        /// <summary>
        /// The execution manager.
        /// </summary>
        private readonly IExecutionManager executionManager;

        /// <summary>
        /// The finalizer.
        /// </summary>
        private readonly IFinalizer finalizer;

        /// <summary>
        /// The business context.
        /// </summary>
        private readonly IBusinessContext businessContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="OfficialDeltaProcessor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="executionChainBuilder">The execution chain builder.</param>
        /// <param name="executionManagerFactory">The execution manager.</param>
        /// <param name="finalizerFactory">The finalizer factory.</param>
        /// <param name="businessContext">The business context.</param>
        public OfficialDeltaProcessor(
           ITrueLogger<OfficialDeltaProcessor> logger,
           IRepositoryFactory factory,
           IUnitOfWorkFactory unitOfWorkFactory,
           IExecutionChainBuilder executionChainBuilder,
           IExecutionManagerFactory executionManagerFactory,
           IFinalizerFactory finalizerFactory,
           IBusinessContext businessContext)
           : base(factory)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            ArgumentValidators.ThrowIfNull(executionManagerFactory, nameof(executionManagerFactory));
            ArgumentValidators.ThrowIfNull(finalizerFactory, nameof(finalizerFactory));

            this.logger = logger;
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.finalizer = finalizerFactory.GetFinalizer(TicketType.OfficialDelta);
            this.executionChainBuilder = executionChainBuilder;
            this.executionManager = executionManagerFactory.GetExecutionManager(True.Entities.Enumeration.TicketType.OfficialDelta);
            this.businessContext = businessContext;
        }

        /// <inheritdoc/>
        public async Task<(bool isValid, Ticket ticket, string errorMessage)> ValidateTicketAsync(int ticketId)
        {
            var errorMessage = string.Empty;
            var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();
            var ticket = await ticketRepository.SingleOrDefaultAsync(a => a.TicketId == ticketId, "CategoryElement").ConfigureAwait(false);

            if (ticket == null || ticket.TicketTypeId != True.Entities.Enumeration.TicketType.OfficialDelta || ticket.Status != StatusType.PROCESSING)
            {
                errorMessage = "Se encuentra en procesamiento un cálculo de deltas oficiales para el segmento o la cadena.";
                this.logger.LogInformation($"Ticket {ticketId} does not exists or is already processed.", $"{ticketId}");
                return (false, null, errorMessage);
            }

            return (true, ticket, errorMessage);
        }

        /// <summary>
        /// Builds official delta data.
        /// </summary>
        /// <param name="officialDeltaData">the official delta data.</param>
        /// <returns>task.</returns>
        public async Task BuildOfficialDeltaDataAsync(OfficialDeltaData officialDeltaData)
        {
            ArgumentValidators.ThrowIfNull(officialDeltaData, nameof(officialDeltaData));
            var nodeList = officialDeltaData.GetPendingOfficialMovementNodes();

            var parameters = new Dictionary<string, object>
            {
                { "@TicketId", officialDeltaData.Ticket.TicketId },
                { "@NodeList", nodeList.ToDataTable(Repositories.Constants.NodeIdColumnName, Repositories.Constants.NodeListType) },
            };

            await Task.WhenAll(
                this.GetDataFromRepositoryAsync<OfficialDeltaMovement>(
                (officialDeltaMov) => officialDeltaData.OfficialDeltaMovements = officialDeltaMov,
                Repositories.Constants.GetDeltaOfficialMovement,
                parameters),
                this.GetDataFromRepositoryAsync<OfficialDeltaInventory>(
                (officialDeltaInv) => officialDeltaData.OfficialDeltaInventories = officialDeltaInv,
                Repositories.Constants.GetDeltaOfficialInventory,
                parameters)).ConfigureAwait(false);
        }

        /// <summary>
        /// Register Node.
        /// </summary>
        /// <param name="officialDeltaData">the official delta data.</param>
        /// <returns>task.</returns>
        public async Task<OfficialDeltaData> RegisterAsync(OfficialDeltaData officialDeltaData)
        {
            // get all nodes from pending movement and pending inventories
            ArgumentValidators.ThrowIfNull(officialDeltaData, nameof(officialDeltaData));
            var nodeList = officialDeltaData.GetPendingOfficialMovementNodes();

            var originalParams = new Dictionary<string, object>
            {
                { "@TicketId", officialDeltaData.Ticket.TicketId },
                { "@NodeList", nodeList.ToDataTable(Repositories.Constants.NodeIdColumnName, Repositories.Constants.NodeListType) },
            };
            await this.GetDataFromRepositoryAsync<MovementsToDelete>(
            (originalMov) => officialDeltaData.MovementTransactionIds = originalMov,
            Repositories.Constants.GetPendingNodeOfficialDeltaMovements,
            originalParams).ConfigureAwait(false);

            // updated officialdeltaTicketId for all movements and inventories and save them.
            this.logger.LogInformation($"Updating Movement and InventoryProduct with OfficialDeltaTicket {officialDeltaData.Ticket.TicketId}", officialDeltaData.Ticket.TicketId);
            await this.UpdateMovementTicketAsync(officialDeltaData).ConfigureAwait(false);
            await this.UpdateInventoryTicketAsync(officialDeltaData).ConfigureAwait(false);

            return await Task.FromResult(officialDeltaData).ConfigureAwait(false);
        }

        /// <summary>
        /// exclude data async.
        /// </summary>
        /// <param name="officialDeltaData">the official delta data.</param>
        /// <returns>task.</returns>
        public async Task<OfficialDeltaData> ExcludeDataAsync(OfficialDeltaData officialDeltaData)
        {
            ArgumentValidators.ThrowIfNull(officialDeltaData, nameof(officialDeltaData));
            var movementsToExclude = GetMovementsToExclude(officialDeltaData);
            officialDeltaData.PendingOfficialMovements = officialDeltaData.PendingOfficialMovements
                .Where(x => movementsToExclude.All(y => y.MovementId != x.MovementId));
            return await Task.FromResult(officialDeltaData).ConfigureAwait(false);
        }

        /// <summary>
        /// Builds the asynchronous.
        /// </summary>
        /// <param name="officialDeltaData">The delta data.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task<OfficialDeltaData> BuildOfficialDataAsync(OfficialDeltaData officialDeltaData)
        {
            ArgumentValidators.ThrowIfNull(officialDeltaData, nameof(officialDeltaData));

            var originalParams = new Dictionary<string, object>
            {
                  { "@TicketId", officialDeltaData.Ticket.TicketId },
            };

            await Task.WhenAll(
               this.GetDataFromRepositoryAsync<Ecp.True.Entities.Query.PendingOfficialMovement>(
               (originalMov) => officialDeltaData.PendingOfficialMovements = originalMov,
               Repositories.Constants.GetOfficialDeltaMovements,
               originalParams),
               this.GetDataFromRepositoryAsync<Ecp.True.Entities.Query.PendingOfficialInventory>(
               (originalInv) => officialDeltaData.PendingOfficialInventories = originalInv,
               Repositories.Constants.GetOfficialDeltaInventories,
               originalParams)).ConfigureAwait(false);
            return await Task.FromResult(officialDeltaData).ConfigureAwait(false);
        }

        /// <summary>
        /// Processes the asynchronous.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="chainType">The chain type.</param>
        /// <returns>The object.</returns>
        public async Task<OfficialDeltaData> ProcessAsync(object data, ChainType chainType)
        {
            ArgumentValidators.ThrowIfNull(data, nameof(data));

            var deltaData = (OfficialDeltaData)data;
            if (deltaData.HasProcessingErrors)
            {
                this.logger.LogInformation($"Official Delta processing aborted for chain {chainType}.", $"{deltaData.Ticket.TicketId}");
                return deltaData;
            }

            this.logger.LogInformation($"Official Delta processing for chain {chainType} started.", $"{deltaData.Ticket.TicketId}");

            var executor = this.executionChainBuilder.Build(ProcessType.OfficialDelta, chainType);
            this.logger.LogInformation($"Official Delta rule processing will start from {executor.GetType().Name}", $"{deltaData.Ticket.TicketId}");

            this.executionManager.Initialize(executor);
            var result = await this.executionManager.ExecuteChainAsync(deltaData).ConfigureAwait(false);

            this.logger.LogInformation($"Official Delta processing for chain {chainType} finished.", $"{deltaData.Ticket.TicketId}");

            return (OfficialDeltaData)result;
        }

        /// <inheritdoc />
        public Task FinalizeProcessAsync(int ticketId)
        {
            return this.finalizer.ProcessAsync(ticketId);
        }

        private static void AddMovementsBasedOnSegmentNodeOrderAndSystem(OfficialDeltaData officialDeltaData, List<PendingOfficialMovement> listOfPendingOfficialMovementsToBeRemoved)
        {
            foreach (var movement in officialDeltaData.PendingOfficialMovements)
            {
                if (GetSegmentForNodes(movement).Equals(Constants.AllIncludingSourceAndDestination, StringComparison.OrdinalIgnoreCase)
                    && GetLowestNodeOrder(movement).Equals(Constants.SourceNodeId, StringComparison.OrdinalIgnoreCase)
                    && !ValidateNodeBelongsToSystem(movement, movement.SourceNodeSystem))
                {
                    listOfPendingOfficialMovementsToBeRemoved.Add(movement);
                }
                else if (GetSegmentForNodes(movement).Equals(Constants.AllIncludingSourceAndDestination, StringComparison.OrdinalIgnoreCase)
                         && GetLowestNodeOrder(movement).Equals(Constants.DestinationNodeId, StringComparison.OrdinalIgnoreCase)
                         && !ValidateNodeBelongsToSystem(movement, movement.DestinationNodeSystem))
                {
                    listOfPendingOfficialMovementsToBeRemoved.Add(movement);
                }
                else
                {
                    FilterSourceAndNode(movement, listOfPendingOfficialMovementsToBeRemoved);
                }
            }
        }

        /// <summary>
        /// Gets the lowest node order.
        /// </summary>
        /// <param name="movement">the pending official movement.</param>
        private static string GetLowestNodeOrder(PendingOfficialMovement movement)
        {
            return movement.SourceNodeOrder < movement.DestinationNodeOrder ? Constants.SourceNodeId : Constants.DestinationNodeId;
        }

        /// <summary>
        ///  Validate MovementSystemNodeId and destinationId belongs to the system reported the movement.
        /// </summary>
        /// <param name="pendingOfficialMovement">The pendingOfficialMovement.</param>
        /// <param name="systemId">the systemId.</param>
        /// <returns>node tag.</returns>
        private static bool ValidateNodeBelongsToSystem(PendingOfficialMovement pendingOfficialMovement, int? systemId)
        {
            return pendingOfficialMovement.SystemId == systemId;
        }

        /// <summary>
        /// filter source node and return list.
        /// </summary>
        /// <param name="movement">the pendingOfficialMovement.</param>
        /// <param name="listOfPendingOfficialMovementsToBeRemoved">the listOfPendingOfficialMovementsToBeRemoved.</param>
        private static void FilterSourceAndNode(PendingOfficialMovement movement, List<PendingOfficialMovement> listOfPendingOfficialMovementsToBeRemoved)
        {
            if (GetSegmentForNodes(movement).Equals(Constants.SourceNodeId, StringComparison.OrdinalIgnoreCase) && !ValidateNodeBelongsToSystem(movement, movement.SourceNodeSystem))
            {
                listOfPendingOfficialMovementsToBeRemoved.Add(movement);
            }

            if (GetSegmentForNodes(movement).Equals(Constants.DestinationNodeId, StringComparison.OrdinalIgnoreCase) && !ValidateNodeBelongsToSystem(movement, movement.DestinationNodeSystem))
            {
                listOfPendingOfficialMovementsToBeRemoved.Add(movement);
            }
        }

        /// <summary>
        /// validate movement.
        /// </summary>
        /// <param name="officialMovement">the nodeId.</param>
        /// <returns>string.</returns>
        private static string GetSegmentForNodes(PendingOfficialMovement officialMovement)
        {
            if ((officialMovement.DestinationNodeSegmentID == officialMovement.SegmentId) && (officialMovement.SourceNodeSegmentId == officialMovement.SegmentId))
            {
                return Constants.AllIncludingSourceAndDestination;
            }
            else if (officialMovement.DestinationNodeSegmentID == officialMovement.SegmentId && (officialMovement.SourceNodeSegmentId != officialMovement.SegmentId))
            {
                return Constants.DestinationNodeId;
            }
            else
            {
                if (officialMovement.DestinationNodeSegmentID != officialMovement.SegmentId && (officialMovement.SourceNodeSegmentId == officialMovement.SegmentId))
                {
                    return Constants.SourceNodeId;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Filter Pending official movements.
        /// </summary>
        /// <param name="officialDeltaData">The official data.</param>
        private static IEnumerable<PendingOfficialMovement> GetMovementsToExclude(OfficialDeltaData officialDeltaData)
        {
            var listOfPendingOfficialMovementsToBeRemoved = new List<PendingOfficialMovement>();

            AddMovementsBasedOnSegmentNodeOrderAndSystem(officialDeltaData, listOfPendingOfficialMovementsToBeRemoved);

            return listOfPendingOfficialMovementsToBeRemoved;
        }

        /// <summary>
        /// Gets the data from repository asynchronous.
        /// </summary>
        /// <typeparam name="T">The T type.</typeparam>
        /// <param name="setter">The setter.</param>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The tasks.</returns>
        private async Task GetDataFromRepositoryAsync<T>(Action<IEnumerable<T>> setter, string storedProcedureName, IDictionary<string, object> parameters)
                where T : class, IEntity
        {
            setter(await this.CreateRepository<T>()
                            .ExecuteQueryAsync(storedProcedureName, parameters).ConfigureAwait(false));
        }

        /// <summary>
        /// Get Movement By MovementId.
        /// </summary>
        /// <param name="officialDeltaData">the officialDeltaData.</param>
        /// <returns> the task.</returns>
        private async Task UpdateMovementTicketAsync(OfficialDeltaData officialDeltaData)
        {
            var movementRepository = this.unitOfWork.CreateRepository<Movement>();
            var movementTransactionIds = officialDeltaData.PendingOfficialMovements.Select(x => x.MovementTransactionId);

            var parameters = new Dictionary<string, object>
            {
                { "@TicketId", officialDeltaData.Ticket.TicketId },
                { "@UserIdValue", this.businessContext.UserId },
                { "@MovementTransactionIdList", movementTransactionIds.ToDataTable(Repositories.Constants.KeyTypeColumnName, Repositories.Constants.KeyType) },
            };

            await movementRepository.ExecuteAsync(Repositories.Constants.UpdateMovementWithOfficialDeltaTicket, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets and update the inventory with the official delta ticketId.
        /// </summary>
        /// <param name="officialDeltaData"> The officialDeltaData.</param>
        /// <returns>OfficialDeltaData.</returns>
        private async Task UpdateInventoryTicketAsync(OfficialDeltaData officialDeltaData)
        {
            var inventoryRepository = this.unitOfWork.CreateRepository<InventoryProduct>();
            var inventoryTransactionIds = officialDeltaData.PendingOfficialInventories.Select(x => x.InventoryProductID);

            var parameters = new Dictionary<string, object>
            {
                { "@TicketId", officialDeltaData.Ticket.TicketId },
                { "@UserIdValue", this.businessContext.UserId },
                { "@InventoryProductIdList", inventoryTransactionIds.ToDataTable(Repositories.Constants.KeyTypeColumnName, Repositories.Constants.KeyType) },
            };

            await inventoryRepository.ExecuteAsync(Repositories.Constants.UpdateInventoryProductWithOfficialDeltaTicket, parameters).ConfigureAwait(false);
        }
    }
}
