// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogisticsService.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
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
    using Ecp.True.Entities.Sap;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Ownership.Services.Interfaces;

    /// <summary>
    /// The Logistics Service.
    /// </summary>
    public class LogisticsService : ILogisticsService
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The repository factory.
        /// </summary>
        private readonly IRepositoryFactory repositoryFactory;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<LogisticsService> logger;

        /// <summary>
        /// The excel service.
        /// </summary>
        private readonly IExcelService excelService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogisticsService" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory">The unit of work.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="failureHandlerFactory">The failure handler factory.</param>
        /// <param name="excelService">The excel service.</param>
        public LogisticsService(
            IUnitOfWorkFactory unitOfWorkFactory,
            IRepositoryFactory repositoryFactory,
            ITrueLogger<LogisticsService> logger,
            IFailureHandlerFactory failureHandlerFactory,
            IExcelService excelService)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            ArgumentValidators.ThrowIfNull(failureHandlerFactory, nameof(failureHandlerFactory));
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.repositoryFactory = repositoryFactory;
            this.logger = logger;
            this.excelService = excelService;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<GenericLogisticsMovement>> TransformAsync(IEnumerable<GenericLogisticsMovement> movements, Ticket ticket, SystemType systemType, ScenarioType scenarioType)
        {
            ArgumentValidators.ThrowIfNull(movements, nameof(movements));
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            this.logger.LogInformation($"Start official logistics  process for ticket:  {ticket.TicketId}", $"{ticket.TicketId}");
            (var tautologies, var homologations, var annulations, var elements) = await this.InitializeAsync(movements, systemType).ConfigureAwait(false);

            var nodesForSegments = await this.LogisticMovementNodeValidationsAsync(await this.GetTicketNodesdAsync(ticket).ConfigureAwait(false), scenarioType).ConfigureAwait(false);
            var transformedMovements = new List<GenericLogisticsMovement>();
            if (ticket.NodeId != null)
            {
                ticket.TicketNodes = null;
            }

            movements.ForEach(x => transformedMovements.Add(
                this.TransformMovementAsync(
                x,
                annulations,
                homologations,
                nodesForSegments,
                tautologies,
                elements,
                systemType).Result));
            return transformedMovements;
        }

        /// <inheritdoc/>
        public DataSet Generate(IEnumerable<OfficialLogisticsDetails> officialLogisticsDetails)
        {
            ArgumentValidators.ThrowIfNull(officialLogisticsDetails, nameof(officialLogisticsDetails));
            var dataSet = new DataSet { Locale = CultureInfo.InvariantCulture };
            var logisticsMovementDataTable = Ecp.True.Core.Extensions.ToDataTable<OfficialLogisticsDetails>(officialLogisticsDetails, string.Empty);
            dataSet.Tables.Add(logisticsMovementDataTable);
            return dataSet;
        }

        /// <inheritdoc/>
        public async Task ProcessLogisticMovementAsync(LogisticMovementResponse logisticMovement)
        {
            ArgumentValidators.ThrowIfNull(logisticMovement, nameof(logisticMovement));
            LogisticMovement movementToUpdate = await this.GetLogisticMovementByMovementIdAsync(logisticMovement.MovementId).ConfigureAwait(false);
            var logisticMovementRepository = this.unitOfWork.CreateRepository<LogisticMovement>();

            if (movementToUpdate != null)
            {
                movementToUpdate.SapSentDate = DateTime.Now;
                movementToUpdate.StatusProcessId = logisticMovement.StatusMessage.Equals(Constants.SapSuccessProcess, StringComparison.OrdinalIgnoreCase)
                    ? StatusType.PROCESSED : StatusType.FAILED;

                if (movementToUpdate.StatusProcessId == StatusType.PROCESSED)
                {
                    movementToUpdate.SapTransactionId = logisticMovement.TransactionId;
                }
                else
                {
                    movementToUpdate.MessageProcess = logisticMovement.Information;
                }

                logisticMovementRepository.Update(movementToUpdate);

                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);

                await this.ProcessBatchStatusAsync(movementToUpdate).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task DoFinalizeOfficialProcessAsync(IEnumerable<GenericLogisticsMovement> officialLogisticsMovement, Ticket ticket, SystemType systemType)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            if (officialLogisticsMovement.Any())
            {
                if (systemType == SystemType.SIV)
                {
                    var officialLogisticsDetails = new List<OfficialLogisticsDetails>();
                    officialLogisticsMovement.Where(x => x.NodeApproved && x.Status == StatusType.VISUALIZATION).ForEach(x => officialLogisticsDetails.Add(MapToDetailsDto(x, ticket)));
                    if (officialLogisticsDetails.Any())
                    {
                        using DataSet data = this.Generate(officialLogisticsDetails);
                        await this.excelService.ExportAndUploadLogisticsExcelAsync(
                        data,
                        SystemType.TRUE.ToString().ToLowerCase(),
                        "logistics",
                        ticket.TicketId.ToString(CultureInfo.InvariantCulture),
                        ticket.CategoryElement.Name,
                        ticket.Owner.Name,
                        LogisticsConstants.ReportName).ConfigureAwait(false);
                    }
                }
                else
                {
                    await this.DoFinalizeAsync(officialLogisticsMovement, ticket).ConfigureAwait(false);
                }
            }
        }

        /// <inheritdoc/>
        public async Task DoFinalizeAsync(IEnumerable<GenericLogisticsMovement> officialLogisticsMovement, Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            if (officialLogisticsMovement.Any())
            {
                officialLogisticsMovement = this.ApplyOrder(officialLogisticsMovement);
                int index = 0;
                var logisticMovement = new List<LogisticMovement>();
                officialLogisticsMovement.Where(x => x.NodeApproved)
                    .ForEach(x => logisticMovement.Add(this.MapToLogisticsMovement(
                        x,
                        ticket,
                        index++,
                        officialLogisticsMovement.Any(x => x.Status == StatusType.EMPTY))));
                var repository = this.unitOfWork.CreateRepository<LogisticMovement>();
                repository.InsertAll(logisticMovement);
                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Get a logistic movement.
        /// </summary>
        /// <param name="movementId">The movementId param.</param>
        /// <returns>The logistic movement to update.</returns>
        public async Task<LogisticMovement> GetLogisticMovementByMovementIdAsync(string movementId)
        {
            return await this.unitOfWork.CreateRepository<LogisticMovement>()
                .FirstOrDefaultAsync(m => m.MovementTransaction.MovementId == movementId
                && m.StatusProcessId == StatusType.SENT
                && m.IsCheck == 1).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the logistic movement.
        /// </summary>
        /// <param name="movementId">The movementId.</param>
        /// <returns>Exits logistic movement.</returns>
        public async Task<bool> ExistLogisticMovementByMovementIdAsync(string movementId)
        {
            var logisticMovement = await this.unitOfWork.CreateRepository<LogisticMovement>()
                .FirstOrDefaultAsync(m => m.MovementTransaction.MovementId == movementId
                && m.IsCheck == 1).ConfigureAwait(false);

            return logisticMovement == null;
        }

        private static int CategoryMovement(GenericLogisticsMovement mov, int? currentNode)
        {
            if ((mov.SourceNodeId ?? 0) == 0 && mov.DestinationNodeId == currentNode)
            {
                return 1;
            }
            else if (mov.SourceNodeId != currentNode && mov.DestinationNodeId == currentNode)
            {
                return 2;
            }
            else if (mov.SourceNodeId == currentNode && (mov.DestinationNodeId ?? 0) == 0)
            {
                return 3;
            }
            else if (mov.SourceNodeId == currentNode && mov.DestinationNodeId == currentNode)
            {
                return 4;
            }
            else if (mov.SourceNodeId == currentNode && mov.DestinationNodeId != currentNode)
            {
                return 5;
            }
            else
            {
                return 6;
            }
        }

        private static Dictionary<string, string> GetTautologies()
        {
            return new Dictionary<string, string>
            {
                { "0000", LogisticsConstants.MaterialToMaterial },
                { "0010", LogisticsConstants.MaterialToMaterial },
                { "0100", LogisticsConstants.MaterialToMaterial },
                { "0110", LogisticsConstants.MaterialToMaterial },
                { "1000", LogisticsConstants.MoveCEToCE },
                { "1010", LogisticsConstants.MoveCEToCE },
                { "1100", LogisticsConstants.WarehouseToWarehouse },
                { "0001", LogisticsConstants.CancellationMaterialToMaterial },
                { "0011", LogisticsConstants.CancellationMaterialToMaterial },
                { "0101", LogisticsConstants.CancellationMaterialToMaterial },
                { "0111", LogisticsConstants.CancellationMaterialToMaterial },
                { "1001", LogisticsConstants.CancellationMoveCEToCE },
                { "1011", LogisticsConstants.CancellationMoveCEToCE },
                { "1101", LogisticsConstants.CancellationWarehouseToWarehouse },
                { "1110", LogisticsConstants.MoveCEToCE },
                { "1111", LogisticsConstants.CancellationMoveCEToCE },
            };
        }

        /// <summary>
        /// Map to detail entity.
        /// </summary>
        /// <param name="movement">movement. </param>
        /// <param name="ticket">ticket. </param>
        /// <returns>OfficialLogisticsDetails. </returns>
        private static OfficialLogisticsDetails MapToDetailsDto(GenericLogisticsMovement movement, Ticket ticket)
        {
            var volume = movement.OwnershipValueUnit == "%" ? (movement.OwnershipValue * movement.NetStandardVolume) / 100 : movement.OwnershipValue;
            var details = new OfficialLogisticsDetails();
            details.SetDefaultValues(ticket.CategoryElement.Name);
            details.Movement = movement.HomologatedMovementType;
            details.StorageSource = string.IsNullOrEmpty(movement.SourceStorageLocation) ? movement.CostCenterName : movement.SourceStorageLocation;
            details.ProductOrigin = movement.SourceProduct;
            details.StorageDestination = string.IsNullOrEmpty(movement.DestinationStorageLocation) ? movement.CostCenterName : movement.DestinationStorageLocation;
            details.ProductDestination = movement.DestinationProduct;
            details.Value = Math.Abs(volume.ToTrueDecimal().GetValueOrDefault());
            details.Uom = movement.MeasurementUnitName;
            details.Order = Convert.ToInt32(movement.Order, CultureInfo.InvariantCulture);
            details.StartDate = movement.StartDate.Date;
            details.EndDate = movement.EndDate.Date;
            details.OperationDate = movement.OperationDate;
            details.PosPurchase = movement.PosPurchase;
            details.OrderPurchase = movement.OrderPurchase;
            return details;
            }

        /// <summary>
        /// Validate node for logistics movements.
        /// </summary>
        /// <param name="ticket">ticket. </param>
        /// <param name="scenarioType">scenarioType. </param>
        /// <returns>List of NodesForSegmentResult. </returns>
        private Task<IEnumerable<NodesForSegmentResult>> LogisticMovementNodeValidationsAsync(Ticket ticket, ScenarioType scenarioType)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var parameters = new Dictionary<string, object>
            {
                { "@SegmentId", ticket.CategoryElementId },
                { "@StartDate", ticket.StartDate },
                { "@EndDate", ticket.EndDate },
                { "@ScenarioTypeId", (int)scenarioType },
                { "@OwnerId", ticket.OwnerId },
                {
                    "@DtNodes", ticket.TicketNodes.Select(ticketNode => ticketNode.NodeId)
                     .ToDataTable(Repositories.Constants.NodeIdColumnName, Repositories.Constants.NodeListType)
                },
            };
            return this.unitOfWork.CreateRepository<NodesForSegmentResult>().ExecuteQueryAsync(Repositories.Constants.GetLogisticMovementNodeValidations, parameters);
        }

        /// <summary>
        /// Transform movements.
        /// </summary>
        /// <param name="movement">movement. </param>
        /// <param name="annulations">annulations. </param>
        /// <param name="homologations">homologations. </param>
        /// <param name="nodesForSegments">nodesForSegments. </param>
        /// <param name="tautologies">tautologies. </param>
        /// <param name="elements">elements. </param>
        /// <param name="systemType">systemType. </param>
        /// <returns>GenericLogisticsMovement. </returns>
        private async Task<GenericLogisticsMovement> TransformMovementAsync(
           GenericLogisticsMovement movement,
           IEnumerable<Annulation> annulations,
           IEnumerable<HomologationDataMapping> homologations,
           IEnumerable<NodesForSegmentResult> nodesForSegments,
           IDictionary<string, string> tautologies,
           IEnumerable<CategoryElement> elements,
           SystemType systemType)
        {
            movement.CheckNodesApproved(nodesForSegments);
            movement.MeasurementUnitName = elements.First(x => x.ElementId == movement.MeasurementUnit.GetValueOrDefault()).Name;
            var annulation = annulations.FirstOrDefault(x => x.AnnulationMovementTypeId == movement.MovementTypeId);
            movement.HasAnnulation = annulation != null;
            movement.Status = StatusType.VISUALIZATION;
            string movementTypeId = string.Empty;
            movement.CheckNodes(LogisticsConstants.NodeErrorSendToSap, nodesForSegments);
            movement.CheckCostCenter(LogisticsConstants.CostCenterError);
            movementTypeId = !string.IsNullOrEmpty(movement.LogisticsTautologyKey)
                ? tautologies[movement.LogisticsTautologyKey]
                : movement.LogisticsSpatialCase.ToString(CultureInfo.InvariantCulture);
            var logisticsMovementTypeElement = elements.FirstOrDefault(x => x.ElementId.Equals(Convert.ToInt32(movementTypeId, CultureInfo.InvariantCulture)));
            movement.LogisticsMovementType = Convert.ToString(logisticsMovementTypeElement?.ElementId, CultureInfo.InvariantCulture);

            var homologatedValue = homologations.FirstOrDefault(x => x.SourceValue == movement.LogisticsMovementType);
            if (homologatedValue == null)
            {
                movement.Status = StatusType.EMPTY;
                movement.ErrorMessage = string.Format(
                        CultureInfo.InvariantCulture,
                        LogisticsConstants.HomologationError,
                        systemType,
                        logisticsMovementTypeElement?.Name);
            }

            movement.HomologatedMovementType = homologatedValue?.DestinationValue;

            if (movement.HasAnnulation)
            {
                movement.Annulate(annulation);
            }

            movement.GmCode = await this.GetTransactionCodeAsync(movement).ConfigureAwait(false);
            if (string.IsNullOrEmpty(movement.GmCode))
            {
                movement.Status = StatusType.EMPTY;
                movement.ErrorMessage = string.Format(
                    CultureInfo.InvariantCulture,
                    LogisticsConstants.GmCodeError,
                    movement.MovementTypeName);
            }

            return movement;
        }

        /// <summary>
        /// Map to logistics movement entity.
        /// </summary>
        /// <param name="officialLogisticMovement">officialLogisticMovement. </param>
        /// <param name="ticket">ticket. </param>
        /// <param name="index">index. </param>
        /// <returns>LogisticMovement. </returns>
        private LogisticMovement MapToLogisticsMovement(GenericLogisticsMovement officialLogisticMovement, Ticket ticket, int index, bool ticketError)
        {
            var volume = officialLogisticMovement.OwnershipValueUnit == "%" ?
                (officialLogisticMovement.OwnershipValue * officialLogisticMovement.NetStandardVolume) / 100 : officialLogisticMovement.OwnershipValue;
            return new LogisticMovement
            {
                CreatedDate = officialLogisticMovement.CreatedDate,
                CostCenterId = officialLogisticMovement.CostCenter,
                SapTransactionCode = officialLogisticMovement.GmCode,
                DestinationLogisticCenterId = officialLogisticMovement.DestinationLogisticCenterId,
                LastModifiedDate = (DateTime?)DateTime.Now,
                MeasurementUnit = officialLogisticMovement.MeasurementUnit,
                OwnershipVolume = Math.Abs(volume.ToTrueDecimal().GetValueOrDefault()),
                SourceLogisticCenterId = officialLogisticMovement.SourceLogisticCenterId,
                StartTime = (DateTime?)officialLogisticMovement.StartDate,
                IsCheck = 0,
                EventType = Constants.EventType,
                DestinationSystem = Constants.DestinationSystem,
                LogisticMovementTypeId = officialLogisticMovement.LogisticsMovementType,
                HomologatedMovementType = officialLogisticMovement.HomologatedMovementType,
                MovementTransactionId = officialLogisticMovement.MovementTransactionId,
                MovementOrder = index,
                NodeOrder = index,
                StatusProcessId = ticketError && officialLogisticMovement.Status != StatusType.EMPTY ? StatusType.CANCELLED : officialLogisticMovement.Status,
                MessageProcess = officialLogisticMovement.ErrorMessage,
                TicketId = ticket.TicketId,
                SourceProductId = officialLogisticMovement.SourceProductId,
                DestinationProductId = officialLogisticMovement.DestinationProductId,
                Position = officialLogisticMovement.PosPurchase,
                DocumentNumber = officialLogisticMovement.OrderPurchase?.ToString(CultureInfo.InvariantCulture),
                SourceLogisticNodeId = officialLogisticMovement.SourceNodeId,
                DestinationLogisticNodeId = officialLogisticMovement.DestinationNodeId,
                ConcatMovementId = officialLogisticMovement.ConcatMovementId,
            };
        }

        private async Task<(
            Dictionary<string, string>,
            IEnumerable<HomologationDataMapping>,
            IEnumerable<Annulation>,
            IEnumerable<CategoryElement>)>
            InitializeAsync(IEnumerable<GenericLogisticsMovement> movements, SystemType systemType)
        {
            var tautologies = GetTautologies();
            var homologations = await this.GetHomologationsAsync(systemType).ConfigureAwait(false);
            var annulations = await this.GetAnnulationsAsync(movements).ConfigureAwait(false);
            var elements = await this.GetElementsAsync().ConfigureAwait(false);
            return (tautologies, homologations, annulations, elements);
        }

        private async Task<IEnumerable<HomologationDataMapping>> GetHomologationsAsync(SystemType systemType)
        {
            var homologations = await this.repositoryFactory.HomologationRepository.GetAllAsync(
                                 x => x.SourceSystemId == (int)SystemType.TRUE && x.DestinationSystemId == (int)systemType
                                 && x.HomologationGroups.Any(
                                    g => g.Group.Name == "Tipo Movimiento" &&
                                    g.HomologationObjects.Any(o => o.HomologationObjectType.Name == "MovementTypeId" || o.HomologationObjectType.Name == "LogisticMovementTypeId")),
                                 "HomologationGroups",
                                 "HomologationGroups.Group",
                                 "HomologationGroups.HomologationObjects",
                                 "HomologationGroups.HomologationDataMapping")
                                .ConfigureAwait(false);
            return homologations.SelectMany(x => x.HomologationGroups.SelectMany(g => g.HomologationDataMapping));
        }

        /// <summary>
        /// Get annulations.
        /// </summary>
        /// <param name="movements">movements. </param>
        /// <returns>List of annulations. </returns>
        private async Task<IEnumerable<Annulation>> GetAnnulationsAsync(IEnumerable<GenericLogisticsMovement> movements)
        {
            var movementTypeIds = movements.Select(x => x.MovementTypeId);
            var repository = this.unitOfWork.CreateRepository<Annulation>();
            return await repository.GetAllAsync(x => movementTypeIds.Contains(x.AnnulationMovementTypeId) && x.IsActive == true).ConfigureAwait(false);
        }

        private async Task<IEnumerable<CategoryElement>> GetElementsAsync()
        {
            var repository = this.unitOfWork.CreateRepository<CategoryElement>();
            return await repository.GetAllAsync(x => x.CategoryId == 9 || x.CategoryId == 6).ConfigureAwait(false);
        }

        /// <summary>
        /// Process batch status after movement update.
        /// </summary>
        /// <param name="movement">The movement.</param>
        private async Task ProcessBatchStatusAsync(LogisticMovement movement)
        {
            var movementList = await this.unitOfWork.CreateRepository<LogisticMovement>().GetAllAsync(
               m => m.TicketId == movement.TicketId && m.IsCheck == 1).ConfigureAwait(false);

            ArgumentValidators.ThrowIfNull(movementList, nameof(movementList));

            var logisticMovementList = movementList.ToList();

            if (logisticMovementList.FindAll(m => m.StatusProcessId == StatusType.SENT).Count == 0)
            {
                if (logisticMovementList.FindAll(m => m.StatusProcessId == StatusType.FAILED).Count > 0)
                {
                    await this.UpdateTicketAsync(movement.TicketId, StatusType.FAILED).ConfigureAwait(false);
                }
                else
                {
                    await this.UpdateTicketAsync(movement.TicketId, StatusType.PROCESSED).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Update Ticket entity after data validation.
        /// </summary>
        /// <param name="ticketId">The ticketId.</param>
        /// <param name="status">The new status.</param>
        private async Task UpdateTicketAsync(int ticketId, StatusType status)
        {
            var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();

            var ticketToUpdate = await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);

            ticketToUpdate.Status = status;

            ticketRepository.Update(ticketToUpdate);

            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Get transaction code.
        /// </summary>
        /// <param name="officialLogisticMovement">officialLogisticMovement. </param>
        /// <returns>string. </returns>
        private async Task<string> GetTransactionCodeAsync(GenericLogisticsMovement officialLogisticMovement)
        {
            var repository = this.unitOfWork.CreateRepository<Annulation>();
            var anulation = await repository.FirstOrDefaultAsync(
                x => x.IsActive.GetValueOrDefault() &&
            (officialLogisticMovement.HasAnnulation ? x.AnnulationMovementTypeId.Equals(officialLogisticMovement.MovementTypeId) :
            x.SourceMovementTypeId.Equals(officialLogisticMovement.MovementTypeId)), "SapTransactionCode").ConfigureAwait(false);
            return anulation?.SapTransactionCodeId == null ? string.Empty : anulation.SapTransactionCode.Name;
        }

        /// <summary>
        /// Get nodes for a ticket.
        /// </summary>
        /// <param name="ticket">ticket. </param>
        /// <returns>Ticket. </returns>
        private async Task<Ticket> GetTicketNodesdAsync(Ticket ticket)
        {
            try
            {
                if (ticket.NodeId != null)
                {
                    ticket.TicketNodes = new List<TicketNode>
                    {
                        new TicketNode
                        {
                            TicketId = ticket.TicketId,
                            NodeId = (int)ticket.NodeId,
                        },
                    };
                }
                else
                {
                    var repository = this.unitOfWork.CreateRepository<TicketNode>();
                    ticket.TicketNodes = await repository.GetAllAsync(x => x.TicketId == ticket.TicketId).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                ticket.TicketGroupId = ex.Message;
            }

            return ticket;
        }

        /// <summary>
        /// Applay order on logistics movement.
        /// </summary>
        /// <param name="logisticsMovement">logisticsMovement. </param>
        /// <returns>List of GenericLogisticsMovement. </returns>
        private IEnumerable<GenericLogisticsMovement> ApplyOrder(IEnumerable<GenericLogisticsMovement> logisticsMovement)
        {
            IEnumerable<GenericLogisticsMovement> finalMovements = new Collection<GenericLogisticsMovement>();

            var nodes = logisticsMovement.Where(x => (x.SourceNodeId ?? 0) != 0).Select(x => new { NodeId = x.SourceNodeId, Order = x.SourceNodeOrder }).Distinct();
            nodes = nodes.Union(logisticsMovement.Where(x => (x.DestinationNodeId ?? 0) != 0).Select(x => new
            {
                NodeId = x.DestinationNodeId,
                Order = x.DestinationNodeOrder,
            }).Distinct()).OrderBy(n => n.Order);

            foreach (var node in nodes)
            {
                finalMovements = finalMovements.Union(logisticsMovement.Where(f => f.SourceNodeId == node.NodeId || f.DestinationNodeId == node.NodeId).
                                 OrderBy(x => CategoryMovement(x, node.NodeId)));
            }

            return finalMovements;
        }
    }
}
