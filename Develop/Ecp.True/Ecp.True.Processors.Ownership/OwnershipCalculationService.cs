// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipCalculationService.cs" company="Microsoft">
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
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Conciliation.Interfaces;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Processors.Ownership.Services.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.Request;

    /// <summary>
    /// The Ownership Calculation Service.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Ownership.Calculation.Interfaces.IOwnershipCalculationService"/>
    public class OwnershipCalculationService : ProcessorBase, IOwnershipCalculationService
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The ownership service.
        /// </summary>
        private readonly ILogisticsService logisticsService;

        /// <summary>
        /// The ownership processor.
        /// </summary>
        private readonly IConciliationProcessor conciliationProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipCalculationService" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="logisticsService">The logistics service.</param>
        /// <param name="conciliationProcessor">The ownership conciliation.</param>
        public OwnershipCalculationService(IUnitOfWorkFactory unitOfWorkFactory, IRepositoryFactory factory, ILogisticsService logisticsService, IConciliationProcessor conciliationProcessor)
            : base(factory)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));

            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.logisticsService = logisticsService;
            this.conciliationProcessor = conciliationProcessor;
        }

        /// <summary>
        /// Gets the logistics details asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="systemType">The systemType identifier.</param>
        /// <returns>The tuple containing collection of LogisticsData and ticket details.</returns>
        public async Task<LogisticsInfo> GetLogisticsDetailsAsync(int ticketId, int systemType)
        {
            var ticketRepository = this.RepositoryFactory.CreateRepository<Ticket>();
            var ticket = await ticketRepository.FirstOrDefaultAsync(
                t => t.TicketId == ticketId && (t.TicketTypeId == TicketType.Logistics ||
                t.TicketTypeId == TicketType.LogisticMovements),
                "Owner",
                "CategoryElement").ConfigureAwait(false);
            var parameters = new Dictionary<string, object>
                {
                    { "@TicketId", ticket.TicketId },
                };

            var logisticMovementDetail = await this.unitOfWork.CreateRepository<GenericLogisticsMovement>().
                ExecuteQueryAsync(Repositories.Constants.LogisticMovementDetailsProcedureName, parameters).ConfigureAwait(false);
            var parametersInventory = new Dictionary<string, object>
                {
                    { "@SegmentId", ticket.CategoryElementId },
                    { "@StartDate", ticket.StartDate.Date },
                    { "@EndDate", ticket.EndDate.Date },
                    { "@OwnerId", ticket.OwnerId },
                    { "@NodeId", ticket.NodeId },
                };
            var logisticInventoryDetail = await this.unitOfWork.CreateRepository<LogisticsInventoryDetail>()
                .ExecuteQueryAsync(Repositories.Constants.LogisticInventoryDetailsProcedureName, parametersInventory).ConfigureAwait(false);

            if (!logisticMovementDetail.Any())
            {
                await this.UpdateTicketErrorsAsync(ticket.TicketId, string.Format(CultureInfo.InvariantCulture, LogisticsConstants.NoDataError, LogisticsConstants.Operative)).ConfigureAwait(false);
                return null;
            }

            var officialMovements = await this.logisticsService.TransformAsync(logisticMovementDetail, ticket, (SystemType)systemType, ScenarioType.OPERATIONAL).ConfigureAwait(false);
            var errorMessage = officialMovements.FirstOrDefault(x => x.Status == StatusType.EMPTY)?.ErrorMessage;
            if (string.IsNullOrEmpty(errorMessage))
            {
                await this.UpdateTicketStatusAndBlobpathAsync(ticket.TicketId, (SystemType)systemType).ConfigureAwait(false);
            }
            else
            {
                await this.UpdateTicketErrorsAsync(ticket.TicketId, errorMessage).ConfigureAwait(false);
            }

            if ((SystemType)systemType == SystemType.SIV)
            {
                var operativeLogisticMovement = new List<OperativeLogisticsMovement>();
                officialMovements.Where(x => x.NodeApproved &&
                x.Status == StatusType.VISUALIZATION).ForEach(x => operativeLogisticMovement.Add(MapToDetailsDto(x)));
                return operativeLogisticMovement.Any()
                    ? new LogisticsInfo
                    {
                        LogisticMovementDetail = operativeLogisticMovement,
                        LogisticInventoryDetail = logisticInventoryDetail,
                        Ticket = ticket,
                    }
                    : null;
            }
            else
            {
                await this.logisticsService.DoFinalizeAsync(officialMovements, ticket).ConfigureAwait(false);
                return default;
            }
        }

        /// <inheritdoc />
        public async Task PopulateOwnershipRuleRequestDataAsync(OwnershipRuleData ownershipRuleData)
        {
            ArgumentValidators.ThrowIfNull(ownershipRuleData, nameof(ownershipRuleData));

            var ownershipDetails = new OwnershipRuleRequest();
            IEnumerable<CancellationMovementDetail> cancellationMovements = new List<CancellationMovementDetail>();

            var parameters = new Dictionary<string, object>
            {
                { "@TicketId", ownershipRuleData.TicketId },
            };

            await Task.WhenAll(
                this.GetDataFromRepositoryAsync<InventoryOperationalData>(
                (inv) => ownershipDetails.InventoryOperationalData = inv,
                Repositories.Constants.InventoryDetailsProcedureName,
                parameters),
                this.GetDataFromRepositoryAsync<MovementOperationalData>(
                (mov) => ownershipDetails.MovementsOperationalData = mov,
                Repositories.Constants.MovementDetailsProcedureName,
                parameters),
                this.GetDataFromRepositoryAsync<NodeConfiguration>(
                (nod) => ownershipDetails.NodeConfigurations = nod,
                Repositories.Constants.NodeConfigurationDetailsProcedureName,
                parameters),
                this.GetDataFromRepositoryAsync<True.Entities.Query.NodeConnection>(
                (nc) => ownershipDetails.NodeConnections = nc,
                Repositories.Constants.NodeConnectionConfigurationDetailsProcedureName,
                parameters),
                this.GetDataFromRepositoryAsync<PreviousMovementOperationalData>(
                (pm) => ownershipDetails.PreviousMovementsOperationalData = pm,
                Repositories.Constants.PreviousMovementDetailsProcedureName,
                parameters),
                this.GetDataFromRepositoryAsync<PreviousInventoryOperationalData>(
                (pi) => ownershipDetails.PreviousInventoryOperationalData = pi,
                Repositories.Constants.PreviousInventoryDetailsProcedureName,
                parameters),
                this.GetDataFromRepositoryAsync<True.Entities.Query.Event>(
                (ev) => ownershipDetails.Events = ev,
                Repositories.Constants.EventDataProcedureName,
                parameters),
                this.GetDataFromRepositoryAsync<True.Entities.Query.Contract>(
                (co) => ownershipDetails.Contracts = co,
                Repositories.Constants.ContractDataProcedureName,
                parameters),
                this.GetDataFromRepositoryAsync<True.Entities.Query.CancellationMovementDetail>(
                (can) => cancellationMovements = can,
                Repositories.Constants.CancellationMovementDataProcedureName,
                parameters)).ConfigureAwait(false);

            Ticket ticket;
            var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();
            ticket = await ticketRepository.GetByIdAsync(ownershipRuleData.TicketId).ConfigureAwait(false);
            var predecessorAndLaterMovements = await this.GetMovementForLaterAndPredecessorNodeAsync(ticket).ConfigureAwait(false);
            var validatedMovements = ValidateTransferPointMovements(ownershipDetails.MovementsOperationalData, predecessorAndLaterMovements, ticket)
                                    .GroupBy(g => g.SegmentId);
            foreach (var item in validatedMovements)
            {
                var result = this.GetProperty(item);
                ownershipDetails = UpdateMovements(result.ToList(), ownershipDetails);
            }

            ownershipDetails.PreviousMovementsOperationalData = MapToPreviousMovementsOperationalData(ownershipDetails);

            ownershipDetails.AdjustOwnershipVolumeForDecimalRoundOff();
            if (cancellationMovements.Any())
            {
                TransformCancellationMovementData(cancellationMovements);
                ownershipDetails.MovementsOperationalData = GetOperationalDataWithCancellationMovements(
                    ownershipDetails.MovementsOperationalData.ToList(),
                    cancellationMovements,
                    ownershipRuleData.TicketId);
                ownershipRuleData.CancellationMovements = cancellationMovements;
            }

            ownershipRuleData.OwnershipRuleRequest = ownershipDetails;
        }

        /// <summary>
        /// Updates the segment node details asynchronous.
        /// </summary>
        /// <param name="nodeIds">The node identifiers.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>Returns completed task.</returns>
        public Task AddOwnershipNodesAsync(IEnumerable<int> nodeIds, int ticketId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@TicketId", ticketId },
                { "@NodeList", nodeIds.ToDataTable(Repositories.Constants.NodeIdColumnName, Repositories.Constants.NodeListType) },
            };

            var repository = this.CreateRepository<OwnershipNode>();
            return repository.ExecuteAsync(Repositories.Constants.SaveSegmentNodeDetails, parameters);
        }

        /// <summary>
        /// Updates the ticket errors asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>Returns completed task.</returns>
        public async Task UpdateTicketErrorsAsync(int ticketId, string errorMessage)
        {
            var repo = this.CreateRepository<Ticket>();
            var ticketDetails = await repo.GetByIdAsync(ticketId).ConfigureAwait(false);
            ticketDetails.ErrorMessage = errorMessage;
            ticketDetails.Status = StatusType.ERROR;
            repo.Update(ticketDetails);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the ticket status and blobpath asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket.</param>
        /// <param name="systemType">The systemType.</param>
        /// <returns>The tasks.</returns>
        public async Task UpdateTicketStatusAndBlobpathAsync(int ticketId, SystemType systemType)
        {
            ArgumentValidators.ThrowIfNull(ticketId, nameof(ticketId));
            var repo = this.unitOfWork.CreateRepository<Ticket>();
            var ticketDetails = await repo.GetByIdAsync(ticketId).ConfigureAwait(false);
            string blobName = systemType == SystemType.SIV ? $"ReporteLogistico_{ticketDetails.CategoryElement.Name}_{ticketDetails.Owner.Name}_{ticketDetails.TicketId}.xlsx" : string.Empty;

            ticketDetails.BlobPath = blobName;
            ticketDetails.Status = systemType == SystemType.SIV ? StatusType.PROCESSED : StatusType.VISUALIZATION;

            repo.Update(ticketDetails);

            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        private static IEnumerable<TransferPointConciliationMovement> ValidateTransferPointMovements(
            IEnumerable<MovementOperationalData> movements,
            IEnumerable<TransferPointConciliationMovement> predecessorAndLaterMovements,
            Ticket ticket)
        {
            return predecessorAndLaterMovements
                .Where(x => movements
                        .Any(pre => pre.SourceNodeId.GetValueOrDefault() == x.SourceNodeId &&
                             pre.DestinationNodeId.GetValueOrDefault() == x.DestinationNodeId &&
                             pre.SourceProductId == x.SourceProductId &&
                             pre.DestinationProductId == x.DestinationProductId &&
                             pre.OperationalDate == x.OperationalDate &&
                             x.SegmentId != ticket.CategoryElementId));
        }

        private static void AdjustOwnershipVolume(List<MovementOperationalData> movements, int movementId)
        {
            var movementList = movements.Where(x => x.MovementTransactionId == movementId);
            var lastOwnership = movementList.Where(x => x.OwnerId != Constants.EcopetrolCategoryElementId).AsEnumerable().LastOrDefault();
            if (lastOwnership != null)
            {
                lastOwnership.OwnershipValue = lastOwnership.NetVolume - movementList.Sum(x => x.OwnershipValue) + lastOwnership.OwnershipValue;
            }
        }

        private static IEnumerable<PreviousMovementOperationalData> MapToPreviousMovementsOperationalData(OwnershipRuleRequest ownershipDetails)
        {
            List<PreviousMovementOperationalData> previousMovements = new List<PreviousMovementOperationalData>();
            previousMovements.AddRange(ownershipDetails.PreviousMovementsOperationalData.Where(m => ownershipDetails.MovementsOperationalData.Any(x => x.MovementTransactionId == m.MovementId
            && x.OwnershipValue == null)));

            List<PreviousMovementOperationalData> previousMovementsOperationalData = ownershipDetails.PreviousMovementsOperationalData
                .Where(m => ownershipDetails.MovementsOperationalData.Any(x => x.MovementTransactionId == m.MovementId && x.OwnershipValue != null)).ToList();

            foreach (var movement in previousMovementsOperationalData)
            {
                if (previousMovements.Any(x => x.MovementId == movement.MovementId))
                {
                    continue;
                }

                var movementOperationalList = ownershipDetails.MovementsOperationalData.Where(x => x.MovementTransactionId == movement.MovementId);

                foreach (var movementOperational in movementOperationalList)
                {
                    PreviousMovementOperationalData previousMovement = new PreviousMovementOperationalData();

                    bool isPercentage = IsPercentage(movementOperational);

                    if (!isPercentage && movementOperational.OwnershipValue != null)
                    {
                        previousMovement.OwnershipPercentage = Math.Round((decimal)(movementOperational.OwnershipValue / movementOperational.NetVolume * 100), 2);
                        previousMovement.OwnershipVolume = Math.Round((decimal)movementOperational.OwnershipValue, 2);
                    }

                    if (isPercentage && movementOperational.OwnershipValue != null)
                    {
                        previousMovement.OwnershipVolume = Math.Round((decimal)(movementOperational.OwnershipValue * movementOperational.NetVolume / 100), 2);
                        previousMovement.OwnershipPercentage = Math.Round((decimal)movementOperational.OwnershipValue, 2);
                    }

                    previousMovement.OwnerId = movementOperational.OwnerId;
                    previousMovement.MovementId = movement.MovementId;
                    previousMovement.AppliedRule = movement.AppliedRule;
                    previousMovement.NetStandardVolume = movement.NetStandardVolume;
                    previousMovements.Add(previousMovement);
                }
            }

            return previousMovements;
        }

        /// <summary>
        /// Map to detail entity.
        /// </summary>
        /// <param name="movement">movement. </param>
        /// <returns>OperativeLogisticsMovement. </returns>
        private static OperativeLogisticsMovement MapToDetailsDto(GenericLogisticsMovement movement)
        {
            var volume = movement.OwnershipValueUnit == "%" ? (movement.OwnershipValue * movement.NetStandardVolume) / 100 : movement.OwnershipValue;
            var details = new OperativeLogisticsMovement();
            details.Movement = movement.HomologatedMovementType;
            details.StorageSource = string.IsNullOrEmpty(movement.SourceStorageLocation) ? movement.CostCenterName : movement.SourceStorageLocation;
            details.ProductOrigin = movement.SourceProduct;
            details.StorageDestination = string.IsNullOrEmpty(movement.DestinationStorageLocation) ? movement.CostCenterName : movement.DestinationStorageLocation;
            details.ProductDestination = movement.DestinationProduct;
            details.Value = Math.Abs(volume.ToTrueDecimal().GetValueOrDefault());
            details.Uom = movement.MeasurementUnitName;
            details.DateOperation = movement.OperationDate;
            details.PosPurchase = movement.PosPurchase;
            details.OrderPurchase = movement.OrderPurchase;
            return details;
        }

        /// <summary>
        /// transform cancellation movement.
        /// </summary>
        /// <param name="cancellationMovements">cancellationMovements. </param>
        private static void TransformCancellationMovementData(IEnumerable<CancellationMovementDetail> cancellationMovements)
        {
            int tempMovementId = int.MaxValue - cancellationMovements.Count() - 1;

            cancellationMovements.ForEach(movement =>
            {
                if (movement.MovementTypeId == (int)MovementType.InputEvacuation)
                {
                    movement.ToInputCancellation();
                }
                else
                {
                    movement.ToOutputCancellation();
                }

                movement.MovementTransactionId = tempMovementId;
                tempMovementId += 1;
            });
        }

        private static ICollection<MovementOperationalData> GetOperationalDataWithCancellationMovements(
                ICollection<MovementOperationalData> movementOperationalData,
                IEnumerable<CancellationMovementDetail> cancellationMovements,
                int ticketId)
        {
            movementOperationalData.AddRange(cancellationMovements.Select(x => x.ToMovementOperationalData(ticketId)));

            return movementOperationalData;
        }

        private static bool IsPercentage(MovementOperationalData movement)
        {
            return movement.OwnershipUnit == Constants.OwnershipPercentageUnit
                        || movement.OwnershipUnit == Constants.OwnershipPercentageUnitId
                        || movement.OwnershipUnit == Constants.Percentage;
        }

        private static OwnershipRuleRequest UpdateMovements(List<TransferPointConciliationMovement> calculatedMovementsProperty, OwnershipRuleRequest ownershipDetails)
        {
            if (calculatedMovementsProperty.Any())
            {
                ownershipDetails.MovementsOperationalData = MapToMovements(calculatedMovementsProperty, ownershipDetails);
            }

            return ownershipDetails;
        }

        private static List<MovementOperationalData> MapToMovements(List<TransferPointConciliationMovement> calculatedMovementsProperty, OwnershipRuleRequest ownershipDetails)
        {
            List<MovementOperationalData> movements = new List<MovementOperationalData>();
            foreach (var item in ownershipDetails.MovementsOperationalData)
            {
                var externalMovements = GetCalculatedMovementsProperty(calculatedMovementsProperty, item);
                var ownerExternalMovements = externalMovements.GroupBy(x => x.OwnerId);
                if (movements.FirstOrDefault(x => x.MovementTransactionId == item.MovementTransactionId) != null)
                {
                    continue;
                }

                foreach (var externalMovement in ownerExternalMovements)
                {
                    var percentageOwnerExternal = externalMovement.FirstOrDefault().OwnershipPercentage;
                    var movementOwner = InitializeMovementOperationalData(item);
                    movementOwner.OwnershipValue = Math.Round((decimal)(movementOwner.NetVolume * percentageOwnerExternal / 100), 2);
                    movementOwner.OwnershipUnit = Constants.Volume;
                    movementOwner.OwnerId = externalMovement.FirstOrDefault().OwnerId;
                    movements.Add(movementOwner);
                }

                AdjustOwnershipVolume(movements, item.MovementTransactionId);
            }

            var filteredMovements = ownershipDetails.MovementsOperationalData.Where(x => !movements.Any(o => o.MovementTransactionId == x.MovementTransactionId)).ToList();
            movements.AddRange(filteredMovements);
            return movements;
        }

        /// <summary>
        /// Initialize Movement Operational Data.
        /// </summary>
        /// <param name="movementOperationalData">movementOperationalData.</param>
        /// <returns>MovementOperationalData.</returns>
        private static MovementOperationalData InitializeMovementOperationalData(MovementOperationalData movementOperationalData)
        {
            return new MovementOperationalData
            {
                CreatedBy = movementOperationalData.CreatedBy,
                SourceProductId = movementOperationalData.SourceProductId,
                DestinationProductId = movementOperationalData.DestinationProductId,
                CreatedDate = movementOperationalData.CreatedDate,
                DestinationNodeId = movementOperationalData.DestinationNodeId,
                MessageTypeId = movementOperationalData.MessageTypeId,
                MovementId = movementOperationalData.MovementId,
                MovementTransactionId = movementOperationalData.MovementTransactionId,
                MovementTypeId = movementOperationalData.MovementTypeId,
                NetVolume = movementOperationalData.NetVolume,
                OperationalDate = movementOperationalData.OperationalDate,
                SourceNodeId = movementOperationalData.SourceNodeId,
                Ticket = movementOperationalData.Ticket,
            };
        }

        /// <summary>
        /// Get Calculated Movements Property.
        /// </summary>
        /// <param name="calculatedMovementsProperty">calculatedMovementsProperty.</param>
        /// <param name="movementOperationalData">movementOperationalData.</param>
        /// <returns>IEnumerable TransferPointConciliationMovement.</returns>
        private static IEnumerable<TransferPointConciliationMovement> GetCalculatedMovementsProperty(
            List<TransferPointConciliationMovement> calculatedMovementsProperty,
            MovementOperationalData movementOperationalData)
        {
            return calculatedMovementsProperty.Where(x => x.SourceNodeId == movementOperationalData.SourceNodeId && x.DestinationNodeId == movementOperationalData.DestinationNodeId
                && x.SourceProductId == movementOperationalData.SourceProductId && x.DestinationProductId == movementOperationalData.DestinationProductId);
        }

        /// <summary>
        /// Gets the initial and final nodes from a ticket.
        /// </summary>
        /// <param name="ticket">ticket.</param>
        /// <returns>two collection with initial and final nodes. </returns>
        private async Task<IEnumerable<TransferPointConciliationMovement>> GetMovementForLaterAndPredecessorNodeAsync(Ticket ticket)
        {
            if (ticket != null)
            {
                var nodesResult = await this.conciliationProcessor.GetConciliationNodeConnectionsAsync(null, ticket.CategoryElementId).ConfigureAwait(false);
                var movements = await this.conciliationProcessor.GetConciliationTransferPointMovementsAsync(new ConnectionConciliationNodesResquest
                {
                    ConciliationNodes = nodesResult,
                    EndDate = ticket.EndDate,
                    StartDate = ticket.StartDate,
                }).ConfigureAwait(false);
                return movements.Where(x => x.SegmentId != ticket.CategoryElementId);
            }
            else
            {
                return new List<TransferPointConciliationMovement>();
            }
        }

        private IEnumerable<TransferPointConciliationMovement> GetProperty(IEnumerable<TransferPointConciliationMovement> movements)
        {
            List<TransferPointConciliationMovement> movementProperties = new List<TransferPointConciliationMovement>();
            var calculatedMovements = movements.GroupBy(g => new { g.SourceProductId, g.SourceNodeId, g.DestinationNodeId, g.DestinationProductId });
            foreach (var calculatedMovement in calculatedMovements)
            {
                var calculatedMovementList = calculatedMovement.GroupBy(x => x.OwnerId).Select(x => new TransferPointConciliationMovement
                {
                    OwnerId = x.FirstOrDefault().OwnerId,
                    OwnershipVolume = x.Sum(sum => sum.OwnershipVolume),
                    DestinationProductId = x.FirstOrDefault().DestinationProductId,
                    DestinationNodeId = x.FirstOrDefault().DestinationNodeId,
                    SourceNodeId = x.FirstOrDefault().SourceNodeId,
                    SourceProductId = x.FirstOrDefault().SourceProductId,
                    OwnershipPercentage = Math.Round((decimal)(x.Sum(sum => sum.OwnershipVolume) / calculatedMovement.Sum(x => x.OwnershipVolume)) * 100, 2),
                });
                var values = calculatedMovementList.Sum(x => x.OwnershipPercentage);
                var calculate = calculatedMovementList.LastOrDefault(x => x.OwnerId != Constants.EcopetrolCategoryElementId);
                if (values < 100 && calculate != null)
                {
                    var newMovement = calculate;
                    newMovement.OwnershipPercentage += (decimal)(100 - values);
                    movementProperties.Add(newMovement);
                    movementProperties.AddRange(calculatedMovementList.Where(x => x.OwnerId != calculate?.OwnerId));
                }
                else
                {
                    movementProperties.AddRange(calculatedMovementList);
                }
            }

            return movementProperties;
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
    }
}
