// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConciliationProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Conciliation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Conciliation.Entities;
    using Ecp.True.Processors.Conciliation.Interfaces;
    using Ecp.True.Proxies.Azure;
    using EfCore.Models;

    /// <summary>
    /// the ConciliationProcessor.
    /// </summary>
    public class ConciliationProcessor : ProcessorBase, IConciliationProcessor
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<ConciliationProcessor> logger;

        /// <summary>
        /// The finalizer.
        /// </summary>
        private readonly IFinalizer finalizer;

        /// <summary>
        /// The conciliation delta.
        /// </summary>
        private readonly ConciliationDelta conciliationDelta;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConciliationProcessor"/> class.
        /// </summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="finalizerFactory">The finalizer factory.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="telemetry">The telemetry.</param>
        public ConciliationProcessor(
            IUnitOfWorkFactory unitOfWorkFactory,
            IAzureClientFactory azureClientFactory,
            IFinalizerFactory finalizerFactory,
            ITrueLogger<ConciliationProcessor> logger,
            IRepositoryFactory factory,
            ITelemetry telemetry)
             : base(factory)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            this.azureClientFactory = azureClientFactory;
            this.logger = logger;
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.finalizer = finalizerFactory.GetFinalizer(FinalizerType.Conciliation);
            this.telemetry = telemetry;
            conciliationDelta = new ConciliationDelta();
        }

        /// <inheritdoc/>
        public async Task CalculateConciliationAsync(MovementConciliations movementConciliations, Ticket ticket, IEnumerable<MovementConciliationDto> otherSegmentMovements)
        {
            ArgumentValidators.ThrowIfNull(movementConciliations, nameof(movementConciliations));
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            ArgumentValidators.ThrowIfNull(otherSegmentMovements, nameof(otherSegmentMovements));
            var nodeForSegment = await this.GetSegmentNodesAsync(ticket).ConfigureAwait(false);
            if (movementConciliations.ErrorMovements.Any())
            {
                await this.MapToMovementsAsync(movementConciliations.ErrorMovements, ticket.TicketId, true, nodeForSegment).ConfigureAwait(false);
                await this.UpdateStatusTicketAsync(ticket.TicketId, StatusType.CONCILIATIONFAILED, string.Empty).ConfigureAwait(false);
                await this.UpdateOwnershipNodeAsync(ticket.TicketId, StatusType.CONCILIATIONFAILED, OwnershipNodeStatusType.CONCILIATIONFAILED, null).ConfigureAwait(false);
            }
            else
            {
                if (movementConciliations.ConciliatedMovements.Any())
                {
                    await this.MapToResultMovementsAsync(movementConciliations.ConciliatedMovements, ticket, nodeForSegment, otherSegmentMovements).ConfigureAwait(false);
                }
                if (movementConciliations.NoConciliatedMovements.Any())
                {
                    await this.MapToMovementsAsync(movementConciliations.NoConciliatedMovements, ticket.TicketId, false, nodeForSegment).ConfigureAwait(false);
                }

                await this.UpdateStatusTicketAsync(ticket.TicketId, StatusType.PROCESSED, string.Empty).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Get Conciliation Transfer Point Movements.
        /// </summary>
        /// <param name="connectionConciliationNodes">Connection conciliationNode.</param>
        /// <returns>TransferPointConciliationMovements.</returns>
        public async Task<IEnumerable<TransferPointConciliationMovement>> GetConciliationTransferPointMovementsAsync(ConnectionConciliationNodesResquest connectionConciliationNodes)
        {
            ArgumentValidators.ThrowIfNull(connectionConciliationNodes, nameof(connectionConciliationNodes));
            IRepository<TransferPointConciliationMovement> transferPointsRepository = this.unitOfWork.CreateRepository<TransferPointConciliationMovement>();
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {
                    "@DtConciliationNodes", connectionConciliationNodes.ConciliationNodes.Select(
                    x => new { x.SourceNodeId, x.DestinationNodeId }).ToDataTable(Repositories.Constants.ConciliationNodeList)
                },
                { "@StartDate", connectionConciliationNodes.StartDate },
                { "@EndDate", connectionConciliationNodes.EndDate },
            };
            return await transferPointsRepository.ExecuteQueryAsync(Repositories.Constants.GetTransferPointsForConciliationNodes, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// This Procedure is used to read point node connections.
        /// </summary>
        /// <param name="nodeId">The node Id.</param>
        /// <param name="segmentId">The segment Id.</param>
        /// <returns>Returns the nodes availables.</returns>
        public Task<IEnumerable<ConciliationNodesResult>> GetConciliationNodeConnectionsAsync(int? nodeId, int? segmentId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                    { "@NodeId", nodeId },
                    { "@SegmentId", segmentId },
            };
            return this.CreateRepository<ConciliationNodesResult>().ExecuteQueryAsync(Repositories.Constants.GetConnectionConciliationNodes, parameters);
        }

        /// <summary>
        /// Start Conciliation.
        /// </summary>
        /// <param name="conciliationNodes">Conciliation Nodes.</param>
        /// <returns>bool.</returns>
        public async Task DoConciliationAsync(ConciliationNodesResquest conciliationNodes)
        {
            ArgumentValidators.ThrowIfNull(conciliationNodes, nameof(conciliationNodes));
            var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();
            var movementRepository = this.unitOfWork.CreateRepository<Movement>();
            Ticket ticket = await ticketRepository.GetByIdAsync(conciliationNodes.TicketId).ConfigureAwait(false);
            List<MovementConciliation> conciliationMovements = new List<MovementConciliation>();
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            NodeTag nodeTag = await this.GetGenericConciliationNodeAsync().ConfigureAwait(false);

            IEnumerable<ConciliationNodesResult> connectionConciliationNodes = await this.GetConciliationNodeConnectionsAsync(
                conciliationNodes.NodeId,
                ticket.CategoryElementId).ConfigureAwait(false);

            if (nodeTag == null && connectionConciliationNodes.Any())
            {
                await this.UpdateStatusTicketAsync(conciliationNodes.TicketId, StatusType.CONCILIATIONFAILED, Constants.ConciliationGenericNodeError).ConfigureAwait(false);
                return;
            }

            if (!connectionConciliationNodes.Any() || ticket.Status != StatusType.PROCESSED && ticket.Status != StatusType.CONCILIATIONFAILED && ticket.Status != StatusType.PROCESSING)
            {
                await this.UpdateStatusTicketAsync(conciliationNodes.TicketId, StatusType.PROCESSED, string.Empty).ConfigureAwait(false);
                return;
            }
            IEnumerable<TransferPointConciliationMovement> transferPointConciliationMovements = await this.GetConciliationTransferPointMovementsAsync(new ConnectionConciliationNodesResquest
            {
                ConciliationNodes = connectionConciliationNodes,
                StartDate = ticket.StartDate,
                EndDate = ticket.EndDate,
            }).ConfigureAwait(false);

            if (!transferPointConciliationMovements.Any())
            {
                await this.UpdateStatusTicketAsync(conciliationNodes.TicketId, StatusType.PROCESSED, string.Empty).ConfigureAwait(false);
                return;
            }

            var otherSegmentList = transferPointConciliationMovements.Where(x => x.SegmentId != ticket.CategoryElementId).GroupBy(x => x.SegmentId).Select(x => x.FirstOrDefault()?.SegmentId);
            IEnumerable<TransferPointConciliationMovement> segmentMovementsWithoutConciliation = transferPointConciliationMovements.Where(x => x.SegmentId == ticket.CategoryElementId
            && (!otherSegmentList.Any(segmentId => segmentId == x.SourceNodeSegmentId) && !otherSegmentList.Any(segmentId => segmentId == x.DestinationNodeSegmentId)));

            IEnumerable<MovementConciliationDto> movements = this.MapToConciliationMovements(transferPointConciliationMovements);
            IEnumerable<MovementConciliation> movementSegments = this.MapToConciliationMovements(movements); 
            movementSegments.ForEach(x => { x.CollectionType = x.SegmentId == ticket.CategoryElementId ? ConciliationMovementCollectionType.SegmentMovement : ConciliationMovementCollectionType.OtherSegmentMovement; x.OwnershipTicketConciliationId = ticket.TicketId; });
            conciliationMovements.AddRange(movementSegments);

            IEnumerable<MovementConciliationDto> movementsWithoutConciliation = movements.Where(x => segmentMovementsWithoutConciliation.Any(m => m.MovementTransactionId == x.MovementTransactionId));

            IEnumerable<MovementConciliationDto> segmentMovements = movements.Where(x => x.SegmentId == ticket.CategoryElementId);

            IEnumerable<MovementConciliationDto> otherSegmentMovements = movements.Where(x => x.SegmentId != ticket.CategoryElementId);
            
            MovementConciliations deltasConciliationMovements = this.conciliationDelta.CalculateDeltaConciliation(segmentMovements.Where(x => !segmentMovementsWithoutConciliation.Any(m => m.MovementTransactionId == x.MovementTransactionId)), otherSegmentMovements);
            movementsWithoutConciliation.ForEach(x => { x.IsReconciled = false; x.Description = string.Format(CultureInfo.InvariantCulture, Constants.NotFoundInformation, "movimientosOtrosSegmentos"); });
            IEnumerable<MovementConciliationDto> NoConciliatedMovements = deltasConciliationMovements.NoConciliatedMovements.Concat(movementsWithoutConciliation);
            deltasConciliationMovements.NoConciliatedMovements = NoConciliatedMovements;

            IEnumerable<MovementConciliation> conciliatedMovements = this.MapToConciliationMovements(deltasConciliationMovements.ConciliatedMovements);
            conciliatedMovements.ForEach(x => { x.CollectionType = ConciliationMovementCollectionType.ResultMovement; x.OwnershipTicketConciliationId = ticket.TicketId; });
            conciliationMovements.AddRange(conciliatedMovements);

            await this.UpdateOtherSegmentMovements(movementRepository, deltasConciliationMovements.NoConciliatedMovements, ticket.TicketId).ConfigureAwait(false);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);


            await this.RegisterConciliationMovementsAsync(conciliationMovements);

            List<MovementConciliationDto> finalMovements = new List<MovementConciliationDto>();

            var conciliatedMovementsOtherSegment = otherSegmentMovements.Where(x => !deltasConciliationMovements.NoConciliatedMovements.Any(o => o.MovementTransactionId == x.MovementTransactionId));
            conciliatedMovementsOtherSegment.ForEach(x => { x.IsReconciled = true; });
            finalMovements.AddRange(conciliatedMovementsOtherSegment);

            var conciliatedMovementsSegment = segmentMovements.Where(x => !deltasConciliationMovements.NoConciliatedMovements.Any(o => o.MovementTransactionId == x.MovementTransactionId));
            conciliatedMovementsSegment.ForEach(x => { x.IsReconciled = true; });
            finalMovements.AddRange(conciliatedMovementsSegment);

            await this.CalculateConciliationAsync(deltasConciliationMovements, ticket, finalMovements).ConfigureAwait(false);


        }

        /// <summary>
        /// Manual Conciliation Queue.
        /// </summary>
        /// <param name="conciliationNodes">conciliationNodes.</param>
        /// <returns>bool.</returns>
        public async Task InitializeConciliationAsync(ConciliationNodesResquest conciliationNodes)
        {
            ArgumentValidators.ThrowIfNull(conciliationNodes, nameof(conciliationNodes));
            var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();
            Ticket ticket = await ticketRepository.GetByIdAsync(conciliationNodes.TicketId).ConfigureAwait(false);
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));

            try
            {
                IServiceBusQueueClient client = this.azureClientFactory.GetQueueClient(QueueConstants.ConciliationQueue);
                var sessionId = Guid.NewGuid().ToString();

                if (conciliationNodes.NodeId == null)
                {
                    await this.UpdateStatusTicketAsync(conciliationNodes.TicketId, StatusType.PROCESSING, string.Empty).ConfigureAwait(false);
                }

                await this.UpdateOwnershipNodeAsync(conciliationNodes.TicketId, ticket.Status, OwnershipNodeStatusType.NOTRECONCILED, conciliationNodes.NodeId).ConfigureAwait(false);

                await client.QueueSessionMessageAsync(conciliationNodes, sessionId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var error = ex.InnerException ?? ex;
                this.logger.LogError(error, Constants.TechnicalExceptionErrorMessage, conciliationNodes.TicketId);
            }
        }

        /// <summary>
        /// Finalize Process.
        /// </summary>
        /// <param name="conciliationRuleData">conciliation Rule Data.</param>
        /// <returns>Task.</returns>
        public Task FinalizeProcessAsync(ConciliationRuleData conciliationRuleData)
        {
            return this.finalizer.ProcessAsync(conciliationRuleData);
        }

        /// <summary>
        /// Update OwnershipNode for ticket.
        /// </summary>
        /// <param name="ticketId">ticketId. </param>
        /// <param name="status">status. </param>
        /// <param name="ownershipStatus">ownership status. </param>
        /// <param name="nodeId">nodeId. </param>
        /// <returns>Task. </returns>
        public async Task UpdateOwnershipNodeAsync(int ticketId, StatusType status, OwnershipNodeStatusType ownershipStatus, int? nodeId)
        {
            var conciliationNodes = await this.GetConciliationNodesAsync(ticketId, nodeId).ConfigureAwait(false);
            var ownershipNodeRepository = this.unitOfWork.CreateRepository<OwnershipNode>();
            var ownerShip = await ownershipNodeRepository.GetAllAsync(x => x.TicketId == ticketId && conciliationNodes.Select(node => node.NodeId).Contains(x.NodeId)).ConfigureAwait(false);
            ownerShip.ForEach(c =>
            {
                c.Status = status;
                c.OwnershipStatus = ownershipStatus;
            });
            ownershipNodeRepository.UpdateAll(ownerShip);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Get conciliation movements for ticket.
        /// </summary>
        /// <param name="ticketId">ticketId. </param>
        /// <param name="nodeId">nodeId. </param>
        /// <returns>Task. </returns>
        public async Task<IEnumerable<Movement>> GetConciliationMovementsAsync(int ticketId, int? nodeId)
        {
            List<int> listId = new List<int>
            {
                    (int)MovementType.CancellationTransferConciliation,
                    (int)MovementType.ConciliationTransfer,
                    (int)MovementType.EMConciliation,
                    (int)MovementType.SMConciliation,
            };
            var movementRepository = this.unitOfWork.CreateRepository<Movement>();

            return await movementRepository.GetAllAsync(
                    x => listId.Contains(x.MovementTypeId)
                    && x.OwnershipTicketId == ticketId
                    && (nodeId == null || (x.MovementSource.SourceNodeId == nodeId || x.MovementDestination.DestinationNodeId == nodeId))
                    && !x.IsDeleted).ConfigureAwait(false);
        }

        /// <summary>
        /// Registers the negative conciliation movements asynchronous.
        /// </summary>
        /// <param name="movements">The movement.</param>
        /// <returns>The task.</returns>
        public async Task RegisterNegativeMovementsAsync(IEnumerable<Movement> movements)
        {
            ArgumentValidators.ThrowIfNull(movements, nameof(movements));
            List<Movement> negativeMovements = new List<Movement>();
            movements.ForEach(x => { x.IsDeleted = true; });
            movements.ForEach(movement => negativeMovements.Add(NegativeMovement(movement)));
            if (negativeMovements.Any())
            {
                IRepository<Movement> movementRepository = this.unitOfWork.CreateRepository<Movement>();
                movementRepository.InsertAll(negativeMovements);
                movementRepository.UpdateAll(movements);
                foreach (var movement in movements)
                {
                    movement.InsertInInventoryMovementIndex(this.unitOfWork, movement.MovementId, movement.EventType, DateTime.Now);
                    var queueClient = this.azureClientFactory.GetQueueClient(QueueConstants.BlockchainMovementQueue);
                    await queueClient.QueueSessionMessageAsync(movement.MovementTransactionId, movement.MovementTransactionId.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
                }
                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Get conciliation nodes.
        /// </summary>
        /// <param name="ticketId">Ticket id.</param>
        /// <param name="nodeId">Node id.</param>
        /// <returns>OwnershipNodeData list task.</returns>
        public async Task<IEnumerable<OwnershipNodeData>> GetConciliationNodesAsync(int ticketId, int? nodeId)
        {
            var ownershipNodeData = await this.QueryViewAsync<OwnershipNodeData>().ConfigureAwait(false);

            return ownershipNodeData
                .Where(x => x.TicketId == ticketId && (nodeId == null || x.NodeId == nodeId) && x.IsTransferPoint)
                .ToList();
        }

        /// <summary>
        /// Update ticket status.
        /// </summary>
        /// <param name="ticketId">ticketId. </param>
        /// <param name="statusType">statusType. </param>
        /// <param name="error">error. </param>
        /// <returns>Task. </returns>
        public async Task UpdateStatusTicketAsync(int ticketId, StatusType statusType, string error)
        {
            var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();
            var ticket = await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);
            ticket.Status = statusType;

            ticket.ErrorMessage = string.IsNullOrEmpty(error) ? null : error;
            ticketRepository.Update(ticket);

            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Update relationship other segment movements.
        /// </summary>
        /// <param name="ticketId">ticketId. </param>
        /// <param name="nodeId">nodeId. </param>
        /// <returns>Task. </returns>
        public async Task DeleteRelationshipOtherSegmentMovementsAsync(int ticketId, int? nodeId)
        {

            var movementRepository = this.unitOfWork.CreateRepository<Movement>();
            var otherSegmentMovementsUpdate = await movementRepository.GetAllAsync(x => x.OwnershipTicketConciliationId == ticketId
            && (nodeId == null || (x.MovementSource.SourceNodeId == nodeId || x.MovementDestination.DestinationNodeId == nodeId)), "MovementSource", "MovementDestination").ConfigureAwait(false);
            if (otherSegmentMovementsUpdate.Any())
            {
                otherSegmentMovementsUpdate.ForEach(x => { x.OwnershipTicketConciliationId = null; x.IsReconciled = null; });
                movementRepository.UpdateAll(otherSegmentMovementsUpdate);
                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Delete  relationship other segment movements.
        /// </summary>
        /// <param name="ticketId">ticketId. </param>
        /// <param name="nodeId">nodeId. </param>
        /// <returns>Task. </returns>
        public async Task DeleteConciliationMovementsAsync(int ticketId, int? nodeId)
        {

            var movementRepository = this.unitOfWork.CreateRepository<MovementConciliation>();
            var movements = await movementRepository.GetAllAsync(x => x.OwnershipTicketConciliationId == ticketId
            && (nodeId == null || (x.SourceNodeId == nodeId || x.DestinationNodeId == nodeId))).ConfigureAwait(false);
            if (movements.Any())
            {
                movementRepository.DeleteAll(movements);
                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }



        /// <summary>
        /// Delete  relationship other segment movements.
        /// </summary>
        /// <param name="ticketId">ticketId. </param>
        /// <returns>Task. </returns>
        private async Task RegisterConciliationMovementsAsync(IEnumerable<MovementConciliation> movements)
        {
            var movementRepository = this.unitOfWork.CreateRepository<MovementConciliation>();
            if (movements.Any())
            {
                movementRepository.InsertAll(movements);
                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Negative the movement asynchronous.
        /// </summary>
        /// <param name="movementObject">The movement object.</param>
        /// <returns>
        /// The task.
        /// </returns>
        private static Movement NegativeMovement(Movement movementObject)
        {
            var negatedmovement = new Movement
            {
                MovementSource = movementObject.MovementSource != null ? new MovementSource() : null,
                MovementDestination = movementObject.MovementDestination != null ? new MovementDestination() : null,
                Period = movementObject.Period != null ? new MovementPeriod() : null,
            };

            negatedmovement.CopyFrom(movementObject);
            if (movementObject.MovementSource != null)
            {
                negatedmovement.MovementSource.CopyFrom(movementObject.MovementSource);
            }

            if (movementObject.MovementDestination != null)
            {
                negatedmovement.MovementDestination.CopyFrom(movementObject.MovementDestination);
            }

            if (movementObject.Period != null)
            {
                negatedmovement.Period.CopyFrom(movementObject.Period);
            }

            movementObject.Owners.ForEach(own =>
            {
                var owner = new Owner();
                owner.CopyFrom(own);
                owner.OwnershipValue = owner.OwnershipValueUnit == Constants.OwnershipPercentageUnit ? Convert.ToDecimal(owner.OwnershipValue.ToString(), CultureInfo.InvariantCulture)
                                       : Convert.ToDecimal(owner.OwnershipValue.ToString(), CultureInfo.InvariantCulture) * -1;
                negatedmovement.Owners.Add(owner);
            });

            movementObject.Attributes.ForEach(attr =>
            {
                var attribute = new AttributeEntity();
                attribute.CopyFrom(attr);

                negatedmovement.Attributes.Add(attribute);
            });

            var previousBlockchainMovementTransactionId = Guid.NewGuid();
            negatedmovement.EventType = EventType.Delete.ToString();

            negatedmovement.NetStandardVolume = Convert.ToDecimal(movementObject.NetStandardVolume.ToString(), CultureInfo.InvariantCulture) * -1;
            negatedmovement.IsDeleted = true;

            if (movementObject.GrossStandardVolume.HasValue)
            {
                negatedmovement.GrossStandardVolume = Convert.ToDecimal(movementObject.GrossStandardVolume.ToString(), CultureInfo.InvariantCulture) * -1;
            }

            negatedmovement.BlockchainMovementTransactionId = previousBlockchainMovementTransactionId;
            negatedmovement.OperationalDate = movementObject.OperationalDate.Date;
            negatedmovement.BlockchainStatus = StatusType.PROCESSING;
            return negatedmovement;
        }

        /// <summary>
        /// Map to negative movement.
        /// </summary>
        /// <param name="movementOld">movement. </param>
        /// <param name="movementNumber">movementNumber. </param>
        /// <param name="isSourceNode">isSourceNode. </param>
        /// <param name="ticket">ticket. </param>
        /// <returns>Movement. </returns>
        private static Movement MapToNewMovement(
            MovementConciliationDto movementOld,
            MovementNumber movementNumber,
            bool isSourceNode,
            Ticket ticket,
            int genericNodeId)
        {
            bool isPositive = !movementOld.Sign.EqualsIgnoreCase(ConciliationConstants.Negative);
            bool isOne = (movementNumber == MovementNumber.One);
            var movement = InitializeMovement(movementOld, ticket);
            movement.MovementTypeId = (int)(isPositive ? MovementType.ConciliationTransfer : MovementType.CancellationTransferConciliation);
            var (movementSource, movementDestination) = GetMovementsSourceAndDestination(movementOld, isSourceNode, isOne, isPositive, genericNodeId);

            if (!isOne)
            {
                bool flagMovementType = isSourceNode ? isPositive : !isPositive;
                movement.MovementTypeId = (int)(flagMovementType ? MovementType.EMConciliation : MovementType.SMConciliation);
                movementSource.SourceProductId = isSourceNode ? movementOld.SourceProductId : movementOld.DestinationProductId;
                movementDestination.DestinationProductId = isSourceNode ? movementOld.SourceProductId : movementOld.DestinationProductId;
                movement.Classification = string.Empty;
            }

            movement.MovementSource = movementSource;
            movement.MovementDestination = movementDestination;
            return movement;
        }

        /// <summary>
        /// Initialize movement
        /// </summary>
        /// <param name="movementOld"></param>
        /// <param name="ticket"></param>
        /// <returns></returns>
        private static Movement InitializeMovement(MovementConciliationDto movementOld, Ticket ticket)
        {
            var movement = new Movement();
            movement.MovementId = Convert.ToString(DateTime.UtcNow.ToTrue().Ticks, CultureInfo.InvariantCulture);
            movement.MessageTypeId = (int)MessageType.Movement;
            movement.SystemTypeId = (int)SystemType.TRUE;
            movement.EventType = EventType.Insert.ToString();
            movement.GrossStandardVolume = movementOld.DeltaConciliated;
            movement.NetStandardVolume = movementOld.DeltaConciliated;
            movement.UncertaintyPercentage = movementOld.UncertaintyPercentage;
            movement.Observations = movementOld.Description;
            movement.IsDeleted = false;
            movement.FileRegistrationTransactionId = null;
            movement.BlockchainMovementTransactionId = Guid.NewGuid();
            movement.OwnershipTicketId = ticket.TicketId;
            movement.SourceSystemId = (int)SourceSystem.TRUE;
            movement.Classification = ConciliationConstants.Conciliation;
            movement.SegmentId = movementOld.SegmentId;
            movement.ScenarioId = ScenarioType.OPERATIONAL;
            movement.MeasurementUnit = movementOld.MeasurementUnit;
            movement.IsSystemGenerated = true;
            movement.SegmentId = ticket.CategoryElementId;
            movement.OperationalDate = movementOld.OperationalDate;
            movement.CreatedDate = DateTime.Now;

            var ownerShips = new List<Ownership>();
            var ownerShip = new Ownership();
            ownerShip.AppliedRule = ConciliationConstants.AppliedRule;
            ownerShip.EventType = EventType.Insert.ToString();
            ownerShip.ExecutionDate = DateTime.Now;
            ownerShip.OwnershipPercentage = 100;
            ownerShip.OwnershipVolume = movementOld.DeltaConciliated.GetValueOrDefault();
            ownerShip.RuleVersion = "1";
            ownerShip.OwnerId = movementOld.OwnerId.GetValueOrDefault();
            ownerShip.MessageTypeId = MessageType.MovementOwnership;
            ownerShip.TicketId = ticket.TicketId;
            ownerShips.Add(ownerShip);
            movement.Ownerships = ownerShips;
            movement.BlockchainStatus = StatusType.PROCESSING;

            movement.Period = new MovementPeriod
            {
                StartTime = movementOld.OperationalDate,
                EndTime = movementOld.OperationalDate,
                CreatedDate = DateTime.Now,
            };

            return movement;
        }

        /// <summary>
        /// Get movements source and destination.
        /// </summary>
        /// <param name="movementOld">movementOld.</param>
        /// <param name="isSourceNode">isSourceNode.</param>
        /// <param name="isPositive">isPositive.</param>
        /// <param name="genericNodeId">genericNodeId.</param>
        /// <returns></returns>
        private static (MovementSource, MovementDestination) GetMovementsSourceAndDestination(MovementConciliationDto movementOld, bool isSourceNode, bool isOne, bool isPositive, int genericNodeId)
        {
            var movementSource = new MovementSource();
            var movementDestination = new MovementDestination();
            movementSource.CreatedDate = DateTime.Now;
            movementDestination.CreatedDate = DateTime.Now;
            if (isSourceNode)
            {
                if (isOne)
                {
                    movementSource.SourceNodeId = isPositive ? movementOld.SourceNodeId : SetNull();
                    movementSource.SourceProductId = isPositive ? movementOld.SourceProductId : null;
                    movementDestination.DestinationNodeId = isPositive ? SetNull() : movementOld.SourceNodeId;
                    movementDestination.DestinationProductId = isPositive ? null : movementOld.SourceProductId;
                }
                else
                {
                    movementSource.SourceNodeId = isPositive ? genericNodeId : movementOld.SourceNodeId;
                    movementSource.SourceProductId = movementOld.SourceProductId;
                    movementDestination.DestinationNodeId = isPositive ? movementOld.SourceNodeId : genericNodeId;
                    movementDestination.DestinationProductId = movementOld.SourceProductId;
                }
            }
            else
            {
                if (isOne)
                {
                    movementSource.SourceNodeId = isPositive ? SetNull() : movementOld.DestinationNodeId;
                    movementSource.SourceProductId = isPositive ? null : movementOld.DestinationProductId;
                    movementDestination.DestinationNodeId = isPositive ? movementOld.DestinationNodeId : SetNull();
                    movementDestination.DestinationProductId = isPositive ? movementOld.DestinationProductId : null;
                }
                else
                {
                    movementSource.SourceNodeId = isPositive ? movementOld.DestinationNodeId : genericNodeId;
                    movementSource.SourceProductId = movementOld.DestinationProductId;
                    movementDestination.DestinationNodeId = isPositive ? genericNodeId : movementOld.DestinationNodeId;
                    movementDestination.DestinationProductId = movementOld.DestinationProductId;
                }
            }

            return (movementSource, movementDestination);
        }

        private static int? SetNull()
        {
            return null;
        }


        /// <summary>
        /// Map to result movement.
        /// </summary>
        /// <param name="movements">Movements.</param>
        /// <param name="ticket">Ticket.</param>
        /// <param name="segmentNodes">Segment nodes.</param>
        /// <returns>Task.</returns>
        private async Task MapToResultMovementsAsync(IEnumerable<MovementConciliationDto> movements, Ticket ticket, IEnumerable<SegmentNodeDto> segmentNodes, IEnumerable<MovementConciliationDto> otherSegmentMovements)
        {
            ArgumentValidators.ThrowIfNull(movements, nameof(movements));
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            ArgumentValidators.ThrowIfNull(otherSegmentMovements, nameof(otherSegmentMovements));
            var ownershipNodeRepository = this.unitOfWork.CreateRepository<OwnershipNode>();
            var ownerShipNodeStatus = new List<OwnershipNode>();
            var newMovements = new List<Movement>();
            NodeTag nodeTag = await this.GetGenericConciliationNodeAsync().ConfigureAwait(false);

            foreach (var movement in movements)
            {
                int nodeId = segmentNodes.First(x => x.NodeId == movement.DestinationNodeId || x.NodeId == movement.SourceNodeId).NodeId;
                if (!movement.Sign.EqualsIgnoreCase(ConciliationConstants.Equal))
                {
                    bool isSourceNode = segmentNodes.Any(y => y.NodeId == movement.SourceNodeId);
                    newMovements.Add(MapToNewMovement(movement, MovementNumber.One, isSourceNode, ticket, nodeTag.NodeId));
                    newMovements.Add(MapToNewMovement(movement, MovementNumber.Two, isSourceNode, ticket, nodeTag.NodeId));
                }

                var node = await ownershipNodeRepository.FirstOrDefaultAsync(x => x.TicketId == ticket.TicketId && x.NodeId == nodeId).ConfigureAwait(false);
                ownerShipNodeStatus.Add(this.UpdateOwnershipNodeStatus(
                     StatusType.PROCESSED,
                     OwnershipNodeStatusType.RECONCILED,
                     movement,
                     node,
                     true));
            }

            if (newMovements.Any())
            {
                newMovements.ForEach(x => { x.Ownerships.ForEach(i => i.BlockchainStatus = StatusType.PROCESSING); });
                var movementRepository = this.unitOfWork.CreateRepository<Movement>();
                movementRepository.InsertAll(newMovements);
                ownershipNodeRepository.UpdateAll(ownerShipNodeStatus);
                await this.UpdateOtherSegmentMovements(movementRepository, otherSegmentMovements, ticket.TicketId).ConfigureAwait(false);
                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
                foreach (var movement in newMovements)
                {
                    movement.InsertInInventoryMovementIndex(this.unitOfWork, movement.MovementId, movement.EventType, movement.CreatedDate);
                    var queueClient = this.azureClientFactory.GetQueueClient(QueueConstants.BlockchainMovementQueue);
                    await queueClient.QueueSessionMessageAsync(movement.MovementTransactionId, movement.MovementTransactionId.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Map to movements.
        /// </summary>
        /// <param name="movementRepository">movementRepository.</param>
        /// <param name="otherSegmentMovements">otherSegmentMovements.</param>
        /// <param name="ticketId">ticketId.</param>
        /// <returns>The Task.</returns>
        private async Task UpdateOtherSegmentMovements(IRepository<Movement> movementRepository, IEnumerable<MovementConciliationDto> otherSegmentMovements, int ticketId)
        {
            var otherSegmentMovementsId = otherSegmentMovements.Select(x => x.MovementTransactionId);
            var otherSegmentMovementsUpdate = await movementRepository.GetAllAsync(x => otherSegmentMovementsId.Contains(x.MovementTransactionId)).ConfigureAwait(false);
            otherSegmentMovementsUpdate.ForEach(x =>
            {
                x.OwnershipTicketConciliationId = ticketId;
                x.IsReconciled = otherSegmentMovements.FirstOrDefault(m => m.MovementTransactionId == x.MovementTransactionId)?.IsReconciled;
                x.Ownerships.ForEach(i => i.BlockchainStatus = StatusType.PROCESSING);
            });
            movementRepository.UpdateAll(otherSegmentMovementsUpdate);
        }
        /// <summary>
        /// Map to movements.
        /// </summary>
        /// <param name="movements">Movements.</param>
        /// <param name="ticketId">Ticket id.</param>
        /// <param name="error">Error.</param>
        /// <param name="segmentNodes">Segment nodes.</param>
        /// <returns>The Task.</returns>
        private async Task MapToMovementsAsync(IEnumerable<MovementConciliationDto> movements, int ticketId, bool error, IEnumerable<SegmentNodeDto> segmentNodes)
        {
            ArgumentValidators.ThrowIfNull(movements, nameof(movements));
            ArgumentValidators.ThrowIfNull(segmentNodes, nameof(segmentNodes));
            var ownershipNodeRepository = this.unitOfWork.CreateRepository<OwnershipNode>();
            var ownerSipNodesError = new List<OwnershipNode>();
            var ownershipNodeErrorRepository = this.unitOfWork.CreateRepository<OwnershipNodeError>();
            var nodeErrors = await ownershipNodeErrorRepository.GetAllAsync(x => movements.Any(y => y.MovementTransactionId == x.MovementTransactionId)).ConfigureAwait(false);
            ownershipNodeErrorRepository.DeleteAll(nodeErrors);
            foreach (var movement in movements)
            {
                int nodeId = segmentNodes.First(x => x.NodeId == movement.DestinationNodeId || x.NodeId == movement.SourceNodeId).NodeId;
                var node = await ownershipNodeRepository.FirstOrDefaultAsync(x => x.TicketId == ticketId && x.NodeId == nodeId).ConfigureAwait(false);

                ownerSipNodesError.Add(this.UpdateOwnershipNodeStatus(
                     error ? StatusType.CONCILIATIONFAILED : StatusType.PROCESSED,
                     error ? OwnershipNodeStatusType.CONCILIATIONFAILED : OwnershipNodeStatusType.NOTRECONCILED,
                     movement,
                     node,
                     false));
            }

            ownershipNodeRepository.UpdateAll(ownerSipNodesError);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Update ownership node status.
        /// </summary>
        /// <param name="status">status. </param>
        /// <param name="nodeStatusType">nodeStatusType. </param>
        /// <param name="movement">movement. </param>
        /// <param name="node">node. </param>
        /// <param name="isMovement">isMovement. </param>
        /// <returns>Task. </returns>
        private OwnershipNode UpdateOwnershipNodeStatus(
            StatusType status,
            OwnershipNodeStatusType nodeStatusType,
            MovementConciliationDto movement,
            OwnershipNode node,
            bool isMovement)
        {
            if (!isMovement)
            {
                var nodeError = new List<OwnershipNodeError>();
                nodeError.Add(new OwnershipNodeError
                {
                    MovementTransactionId = movement.MovementTransactionId,
                    CreatedDate = DateTime.Now,
                    ExecutionDate = DateTime.Now,
                    ErrorMessage = movement.Description,
                    OwnershipNodeId = node.OwnershipNodeId,
                });
                node.OwnershipNodeErrors = nodeError;
            }

            node.Status = status;
            node.OwnershipStatus = nodeStatusType;
            return node;
        }

        /// <summary>
        /// Get segment nodes.
        /// </summary>
        /// <param name="ticket">Ticket. </param>
        /// <returns>SegmentNodeDto list Task. </returns>
        private async Task<IEnumerable<SegmentNodeDto>> GetSegmentNodesAsync(Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var parameters = new Dictionary<string, object>
            {
                { "@SegmentId", ticket.CategoryElementId },
                { "@StartDate", ticket.StartDate.Date },
                { "@EndDate", ticket.EndDate.Date },
            };

            var repository = this.unitOfWork.CreateRepository<SegmentNodeDto>();
            return await repository.ExecuteQueryAsync(Repositories.Constants.GetSegmentNodesProcedureName, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Map to conciliation movements.
        /// </summary>
        /// <param name="conciliationTransferPointMovements">conciliation transfer point movements.</param>
        /// <returns>Conciliation Movements.</returns>
        private IEnumerable<MovementConciliation> MapToConciliationMovements(IEnumerable<MovementConciliationDto> conciliationTransferPointMovements)
        {
            return (from movement in conciliationTransferPointMovements.ToList()
                    select movement).Select(x => new MovementConciliation
                    {
                        MovementTransactionId = x.MovementTransactionId,
                        MovementTypeId = x.MovementTypeId,
                        SourceNodeId = x.SourceNodeId,
                        DestinationNodeId = x.DestinationNodeId,
                        SegmentId = x.SegmentId,
                        OwnerId = x.OwnerId,
                        MeasurementUnit = x.MeasurementUnit,
                        OwnershipPercentage = x.OwnershipPercentage,
                        NetStandardVolume = x.NetStandardVolume,
                        OwnershipVolume = x.OwnershipVolume,
                        DestinationProductId = x.DestinationProductId,
                        SourceProductId = x.SourceProductId,
                        OperationalDate = x.OperationalDate,
                        UncertaintyPercentage = x.UncertaintyPercentage,
                        Description = x.Description,
                        Sign = x.Sign,
                        DeltaConciliated = x.DeltaConciliated
                    }).ToList();
        }

        /// <summary>
        /// Map to conciliation movements.
        /// </summary>
        /// <param name="conciliationTransferPointMovements">conciliation transfer point movements.</param>
        /// <returns>Conciliation Movements.</returns>
        private IEnumerable<MovementConciliationDto> MapToConciliationMovements(IEnumerable<TransferPointConciliationMovement> conciliationTransferPointMovements)
        {
            return (from movement in conciliationTransferPointMovements.ToList()
                    select movement).Select(x => new MovementConciliationDto
                    {
                        MovementTransactionId = x.MovementTransactionId,
                        MovementTypeId = x.MovementTypeId,
                        SourceNodeId = x.SourceNodeId,
                        DestinationNodeId = x.DestinationNodeId,
                        SegmentId = x.SegmentId,
                        OwnerId = x.OwnerId,
                        MeasurementUnit = x.MeasurementUnit,
                        OwnershipPercentage = x.OwnershipPercentage,
                        NetStandardVolume = x.NetStandardVolume,
                        OwnershipVolume = x.OwnershipVolume,
                        DestinationProductId = x.DestinationProductId,
                        SourceProductId = x.SourceProductId,
                        OperationalDate = x.OperationalDate,
                        UncertaintyPercentage = x.UncertaintyPercentage
                    }).ToList();
        }

        /// <summary>
        /// Get generic conciliation node.
        /// </summary>
        /// <returns>Generic conciliation node.</returns>
        private async Task<NodeTag> GetGenericConciliationNodeAsync()
        {
            var nodeRepository = this.unitOfWork.CreateRepository<NodeTag>();
            return await nodeRepository.FirstOrDefaultAsync(l => l.Node.Name.EqualsIgnoreCase("CONCILIACION") &&
            ((l.CategoryElement.Name.ToLower().Contains("generico") || l.CategoryElement.Name.ToLower().Contains("genérico")) &&
            (l.CategoryElement.CategoryId == Constants.TypeSegmentCategoryElement))).ConfigureAwait(false);
        }
    }
}