// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Sap.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
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
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Sap.Interfaces;
    using Ecp.True.Processors.Sap.Services.FactoryUpload;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Sap.Interfaces;
    using Ecp.True.Proxies.Sap.Request;
    using Ecp.True.Proxies.Sap.Response;

    /// <summary>
    /// Interface Calculator.
    /// </summary>
    public class SapProcessor : ISapProcessor
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<SapProcessor> logger;

        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The sap proxy.
        /// </summary>
        private readonly ISapProxy sapProxy;

        /// <summary>
        /// The client.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// The sap tracking.
        /// </summary>
        private readonly ISapTrackingProcessor sapTracking;

        /// <summary>
        /// The file registration transaction service.
        /// </summary>
        private readonly IFileRegistrationTransactionService fileRegistrationTransactionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SapProcessor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="sapProxy">The sap proxy.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="sapTracking">The sapTracking.</param>
        /// <param name="fileRegistrationTransactionService">The file registration transaction service.</param>
        public SapProcessor(
            ITrueLogger<SapProcessor> logger,
            IUnitOfWorkFactory unitOfWorkFactory,
            ISapProxy sapProxy,
            IAzureClientFactory azureClientFactory,
            ITelemetry telemetry,
            ISapTrackingProcessor sapTracking,
            IFileRegistrationTransactionService fileRegistrationTransactionService)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            this.logger = logger;
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.sapProxy = sapProxy;
            this.azureClientFactory = azureClientFactory;
            this.telemetry = telemetry;
            this.sapTracking = sapTracking;
            this.fileRegistrationTransactionService = fileRegistrationTransactionService;
        }

        /// <inheritdoc />
        public async Task SyncAsync()
        {
            var sapResponseMappings = await this.sapProxy.GetMappingsAsync().ConfigureAwait(false);
            var mappingRepo = this.unitOfWork.CreateRepository<SapMapping>();
            var trueMappings = await mappingRepo.GetAllAsync(null).ConfigureAwait(false);
            var sapMappings = this.ConvertMappings(sapResponseMappings);
            mappingRepo.DeleteAll(trueMappings);
            mappingRepo.InsertAll(sapMappings);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            await this.unitOfWork.CreateRepository<SapMapping>().ExecuteAsync(Repositories.Constants.SapMappingDetailProcedureName, new Dictionary<string, object>()).ConfigureAwait(false);
            await this.azureClientFactory.AnalysisServiceClient.RefreshSapMappingDetailsAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the transfer point asynchronous.
        /// </summary>
        /// <param name="movementTransactionId">The movement transaction identifier.</param>
        /// <param name="previousMovementTransactionId">The previous movement transaction identifier.</param>
        /// <returns>The bool.</returns>
        [Obsolete("This Method is Deprecated", false)]
        public async Task UpdateTransferPointAsync(int movementTransactionId, int? previousMovementTransactionId)
        {
            try
            {
                this.logger.LogInformation($"Update TransferPoint is requested for movement Id {movementTransactionId}", $"{movementTransactionId}");
                var sapMovementsObj = await GetSapMovementAsync(movementTransactionId, this.unitOfWork).ConfigureAwait(false);
                var result = await this.sapProxy.UpdateMovementTransferPointAsync(sapMovementsObj, movementTransactionId).ConfigureAwait(false);
                if (!result.IsSuccess)
                {
                    await UpdateSapTrackingErrorsAsync(movementTransactionId, result, this.unitOfWork).ConfigureAwait(false);
                    this.telemetry.TrackEvent(Constants.Critical, EventName.OfficialTransferPointRegistrationFailureEvent.ToString("G"));
                }

                await this.UpdateSapTrackingStatusAsync(movementTransactionId, previousMovementTransactionId, result.IsSuccess, string.Empty).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await this.UpdateSapTrackingStatusAsync(movementTransactionId, previousMovementTransactionId, false, Constants.SapTechnicalError).ConfigureAwait(false);
                this.logger.LogError(ex, $"Exception occurred in Update TransferPoint. {ex.Message}", $"{movementTransactionId}");
                this.telemetry.TrackEvent(Constants.Critical, EventName.OfficialTransferPointRegistrationFailureEvent.ToString("G"));
            }
        }

        /// <summary>
        /// Updates the transfer point asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket identifier.</param>
        /// <returns>The bool.</returns>
        public async Task ProcessLogisticMovementAsync(LogisticQueueMessage ticket)
        {
            var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();
            try
            {
                ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));

                this.logger.LogInformation($"Process logistic movements for Ticket Id: {ticket.TicketId}");
                await this.ProcessTicketAsync(ticket, ticketRepository).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await this.UpdateLogisticMovementsFailAsync(ticket).ConfigureAwait(false);
                await this.UpdateTicketFailAsync(ticket, ticketRepository).ConfigureAwait(false);
                this.logger.LogError(ex, $"Exception occurred in Process Logistic Movements. {ex.Message}", $"{ticket}");
                this.telemetry.TrackEvent(Constants.Critical, EventName.OfficialTransferPointRegistrationFailureEvent.ToString("G"));
            }
        }

        /// <summary>
        /// Updates the tracking status asynchronous.
        /// </summary>
        /// <param name="movementTransactionId">The movement transaction identifier.</param>
        /// <param name="statusType">Type of the status.</param>
        /// <param name="operationalDate">The operational date.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="repository">The repository.</param>
        private static async Task UpdateTrackingStatusAsync(int movementTransactionId, StatusType statusType, DateTime operationalDate, string errorMessage, IRepository<SapTracking> repository)
        {
            var blobPath = string.Format(CultureInfo.InvariantCulture, Constants.TransferPointBlobStoragePath, movementTransactionId);
            var existingSapTrackingEntity = await repository.FirstOrDefaultAsync(x => x.MovementTransactionId == movementTransactionId).ConfigureAwait(false);
            if (existingSapTrackingEntity != null)
            {
                existingSapTrackingEntity.StatusTypeId = statusType;
                existingSapTrackingEntity.OperationalDate = operationalDate;
                existingSapTrackingEntity.ErrorMessage = errorMessage;
                existingSapTrackingEntity.BlobPath = blobPath;
                repository.Update(existingSapTrackingEntity);
            }
        }

        /// <summary>
        /// Update Sap Tracking with Errors.
        /// </summary>
        /// <param name="movementTransactionId">The movement id.</param>
        /// <param name="upload">The SapTrackingStatus.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>The process.</returns>
        [Obsolete("This Method is Deprecated", false)]
        private static async Task UpdateSapTrackingErrorsAsync(int movementTransactionId, SapTrackingStatus upload, IUnitOfWork unitOfWork)
        {
            var sapTrackingRepository = unitOfWork.CreateRepository<SapTracking>();
            var sapTrack = await sapTrackingRepository.FirstOrDefaultAsync(x => x.MovementTransactionId == movementTransactionId).ConfigureAwait(false);
            var errorRepository = unitOfWork.CreateRepository<SapTrackingError>();
            var errors = new List<SapTrackingError>();
            foreach (var error in upload.Document.Errors)
            {
                errors.Add(new SapTrackingError { ErrorCode = error.ErrorCode, ErrorDescription = error.ErrorDescription, SapTrackingId = sapTrack.SapTrackingId });
            }

            errorRepository.InsertAll(errors);
        }

        /// <summary>
        /// Gets the sap movement asynchronous.
        /// </summary>
        /// <param name="movementTransactionId">The movement transaction identifier.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>The object.</returns>
        private static async Task<SapMovementRequest> GetSapMovementAsync(int movementTransactionId, IUnitOfWork unitOfWork)
        {
            var movementObject = new Movement();

            await Task.WhenAll(
                GetFirstRecordFromRepositoryAsync<Movement>(
                    (movement) => movementObject = movement,
                    a => a.MovementTransactionId == movementTransactionId && a.NetStandardVolume > 0,
                    new[] { "MovementSource", "MovementDestination", "Period" },
                    unitOfWork),
                GetDataFromRepositoryAsync<Owner>(x => x.MovementTransactionId == movementTransactionId, unitOfWork),
                GetDataFromRepositoryAsync<AttributeEntity>(x => x.MovementTransactionId == movementTransactionId, unitOfWork))
                .ConfigureAwait(false);

            var movement = await Task.FromResult(movementObject).ConfigureAwait(false);

            var sapMovement = movement.CopyPropertyValuesWithName<Movement, SapMovementRequest>();
            sapMovement.ScenarioId = movement != null ? Convert.ToString((int)movement.ScenarioId, CultureInfo.InvariantCulture) : string.Empty;
            sapMovement.Classification = string.IsNullOrEmpty(sapMovement.Classification) ? "Movimiento" : sapMovement.Classification;
            return sapMovement;
        }

        /// <summary>
        /// Get Contract Values.
        /// </summary>
        /// <param name="logisticMovement"> the logisticMovement.</param>
        /// <returns>data.</returns>
        private static (string documentNumber, string position, string documentNumberSales, string positionSales) GetContractValues(LogisticMovement logisticMovement)
        {
            string documentNumber = string.Empty;
            string position = string.Empty;
            string documentNumberSales = string.Empty;
            string positionSales = string.Empty;
            if (int.Parse(logisticMovement.LogisticMovementTypeId, CultureInfo.InvariantCulture) == (int)MovementType.Sale ||
                         int.Parse(logisticMovement.LogisticMovementTypeId, CultureInfo.InvariantCulture) == (int)MovementType.SelfConsumption)
            {
                documentNumberSales = logisticMovement.DocumentNumber;
                positionSales = Convert.ToString(logisticMovement.Position.Value, CultureInfo.InvariantCulture);
            }

            if (int.Parse(logisticMovement.LogisticMovementTypeId, CultureInfo.InvariantCulture) == (int)MovementType.Purchase ||
                int.Parse(logisticMovement.LogisticMovementTypeId, CultureInfo.InvariantCulture) == (int)MovementType.Transfer)
            {
                documentNumber = logisticMovement.DocumentNumber;
                position = Convert.ToString(logisticMovement.Position.Value, CultureInfo.InvariantCulture);
            }

            return (documentNumber, position, documentNumberSales, positionSales);
        }

        /// <summary>
        /// Get Data From Repository.
        /// </summary>
        /// <typeparam name="T">The generic class.</typeparam>
        /// <param name="predicate">The function.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>The result.</returns>
        private static async Task GetDataFromRepositoryAsync<T>(Expression<Func<T, bool>> predicate, IUnitOfWork unitOfWork)
                where T : class, IEntity
        {
            await unitOfWork.CreateRepository<T>().GetAllAsync(predicate).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the First Record From Repository.
        /// </summary>
        /// <typeparam name="T">The generic class.</typeparam>
        /// <param name="setter">The action.</param>
        /// <param name="predicate">The function.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>The result.</returns>
        private static async Task GetFirstRecordFromRepositoryAsync<T>(Action<T> setter, Expression<Func<T, bool>> predicate, string[] properties, IUnitOfWork unitOfWork)
                where T : class, IEntity
        {
            setter(await unitOfWork.CreateRepository<T>().FirstOrDefaultAsync(predicate, properties).ConfigureAwait(false));
        }

        /// <summary>
        /// Updates the sap tracking status asynchronous.
        /// </summary>
        /// <param name="movementTransactionId">The movement transaction identifier.</param>
        /// <param name="previousMovementTransactionId">The previous movement transaction identifier.</param>
        /// <param name="sapStatus">if set to <c>true</c> [sap status].</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>The task.</returns>
        [Obsolete("This Method is Deprecated", false)]
        private async Task UpdateSapTrackingStatusAsync(int movementTransactionId, int? previousMovementTransactionId, bool sapStatus, string errorMessage)
        {
            var statusType = sapStatus ? StatusType.PROCESSED : StatusType.FAILED;
            var operationalDate = DateTime.UtcNow.ToTrue();
            var sapTrackingRepository = this.unitOfWork.CreateRepository<SapTracking>();
            await UpdateTrackingStatusAsync(movementTransactionId, statusType, operationalDate, errorMessage, sapTrackingRepository).ConfigureAwait(false);
            if (previousMovementTransactionId.HasValue)
            {
                await UpdateTrackingStatusAsync(previousMovementTransactionId.Value, statusType, operationalDate, errorMessage, sapTrackingRepository).ConfigureAwait(false);
            }

            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the sap mapping.
        /// </summary>
        /// <param name="mappingResponse">The mapping.</param>
        /// <returns>The sap mapping.</returns>
        private IEnumerable<SapMapping> ConvertMappings(IEnumerable<SapMappingResponse> mappingResponse)
        {
            return mappingResponse.Select(p => new SapMapping
            {
                DestinationProductId = p.DestinationProductId,
                DestinationMovementTypeId = p.DestinationMovementTypeId,
                DestinationSystemDestinationNodeId = p.DestinationSystemDestinationNodeId,
                DestinationSystemId = p.DestinationSystemId,
                DestinationSystemSourceNodeId = p.DestinationSystemSourceNodeId,
                OfficialSystem = p.OfficialSystem,
                SourceMovementTypeId = p.SourceMovementTypeId,
                SourceProductId = p.SourceProductId,
                SourceSystemDestinationNodeId = p.SourceSystemDestinationNodeId,
                SourceSystemId = p.SourceSystemId,
                SourceSystemSourceNodeId = p.SourceSystemSourceNodeId,
            });
        }

        /// <summary>
        /// Updates the tracking status asynchronous.
        /// </summary>
        /// <param name="ticketQueue">The ticket identifier.</param>
        /// <param name="repository">The repository.</param>
        private async Task ProcessTicketAsync(LogisticQueueMessage ticketQueue, IRepository<Ticket> repository)
        {
            IEnumerable<LogisticMovement> logisticMovementsToProcess = null;

            ArgumentValidators.ThrowIfNull(ticketQueue, nameof(ticketQueue));

            var ticket = await repository.GetByIdAsync(ticketQueue.TicketId).ConfigureAwait(false);

            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));

            if (ticket.Status == StatusType.PROCESSING)
            {
                ticket.Status = StatusType.SENT;
                repository.Update(ticket);
                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);

                logisticMovementsToProcess = await this.GetInitLogisticMovementsByTicketAsync(ticket).ConfigureAwait(false);
            }

            if (ticket.Status == StatusType.FAILED)
            {
                logisticMovementsToProcess = await this.GetLogisticMovementsToForwardByTicketAsync(ticket).ConfigureAwait(false);
            }

            await this.SentMovementsToSapAsync(logisticMovementsToProcess).ConfigureAwait(false);
        }

        /// <summary>
        /// Process all logistic movement by Ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The Process.</returns>
        private async Task<IEnumerable<LogisticMovement>> GetInitLogisticMovementsByTicketAsync(Ticket ticket)
        {
            var logisticRepository = this.unitOfWork.CreateRepository<LogisticMovement>();

            var movements = await logisticRepository.GetAllAsync(
                l => l.TicketId == ticket.TicketId && l.IsCheck == 1,
                "MovementTransaction",
                "MovementTransaction.MovementDestination",
                "MovementTransaction.MovementSource",
                "MovementTransaction.Segment",
                "MovementTransaction.Attributes",
                "CategoryCostCenter").ConfigureAwait(false);

            ArgumentValidators.ThrowIfNull(movements, nameof(movements));

            return movements;
        }

        /// <summary>
        /// Sent logistic movements so SAP.
        /// </summary>
        /// <param name="movements">The logistic movements.</param>
        /// <returns>The process.</returns>
        private async Task SentMovementsToSapAsync(IEnumerable<LogisticMovement> movements)
        {
            bool isTicketFailed = false;
            var sendLogisticMovement = new SendLogisticMovement(this.unitOfWork, this.sapProxy, this.sapTracking, this.telemetry, this.logger, this.fileRegistrationTransactionService);
            var storagesNodeslist = await this.GetStoragenodesAsync(movements.FirstOrDefault().TicketId).ConfigureAwait(false);
            foreach (var item in movements)
            {
                try
                {
                    SapLogisticRequest sapLogisticRequest = this.GetLogisticRequest(movements.Count(), item, storagesNodeslist);

                    ArgumentValidators.ThrowIfNull(sapLogisticRequest, nameof(sapLogisticRequest));

                    await sendLogisticMovement.SendUploadProcessorSapAsync(sapLogisticRequest, item).ConfigureAwait(false);
                }
                catch
                {
                    isTicketFailed = true;
                    await sendLogisticMovement.UpdateLogisticMovementStatusAsync(item, StatusType.FAILED, Constants.SapTechnicalErrorSendLogisticMovement).ConfigureAwait(false);
                }
            }

            if (isTicketFailed)
            {
                await sendLogisticMovement.UpdateTicketStatusAsync(movements.FirstOrDefault().TicketId, StatusType.FAILED).ConfigureAwait(false);
            }
        }

        private async Task<IEnumerable<NodeStorage>> GetStoragenodesAsync(int ticketId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@TicketId", ticketId },
            };
            var repository = this.unitOfWork.CreateRepository<NodeStorage>();
            var list = await repository.ExecuteQueryAsync(Repositories.Constants.GetStorageNode, parameters).ConfigureAwait(false);
            return list;
        }

        /// <summary>
        /// Process all logistic movement by Ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The Process.</returns>
        private async Task<IEnumerable<LogisticMovement>> GetLogisticMovementsToForwardByTicketAsync(Ticket ticket)
        {
            var logisticRepository = this.unitOfWork.CreateRepository<LogisticMovement>();

            var movements = await logisticRepository.GetAllAsync(
                l => l.TicketId == ticket.TicketId && l.IsCheck == 1 && l.StatusProcessId == StatusType.FORWARD,
                "MovementTransaction",
                "MovementTransaction.MovementDestination",
                "MovementTransaction.MovementSource",
                "MovementTransaction.Segment",
                "MovementTransaction.Attributes",
                "CategoryCostCenter").ConfigureAwait(false);

            ArgumentValidators.ThrowIfNull(movements, nameof(movements));

            return movements;
        }

        /// <summary>
        /// Create the SAP request by logistic movement.
        /// </summary>
        /// <param name="totalMovements">The total of movements to sent.</param>
        /// <param name="logisticMovement">The logistic movement.</param>
        /// <param name="nodeStorages">The nodeStorages list.</param>
        /// <returns>The SapLogisticRequest.</returns>
        private SapLogisticRequest GetLogisticRequest(int totalMovements, LogisticMovement logisticMovement, IEnumerable<NodeStorage> nodeStorages)
        {
            var sentDate = DateTime.Now;

            (string documentNumber, string position, string documentNumberSales, string positionSales) = GetContractValues(logisticMovement);
            SapLogisticRequest sapLogisticRequest = new SapLogisticRequest
            {
                Movement = new SapLogisticRequestDto
                {
                    Movement = new SapLogisticDto
                    {
                        SourceSystem = SystemType.TRUE.ToString(),
                        DestinationSystem = SystemType.SAP.ToString(),
                        EventType = Constants.LogisticEventCreation,
                        MovementId = logisticMovement.MovementTransaction.MovementId,
                        Order = this.GetLogisticOrder(totalMovements, logisticMovement),
                        Period = new SapLogisticPeriod
                        {
                            StartTime = sentDate.ToString(Constants.DateStringFormat, CultureInfo.InvariantCulture),
                        },
                        OperationalDate = logisticMovement.CreatedDate.Value.ToString(Constants.DateStringFormat, CultureInfo.InvariantCulture),
                        ScenarioType = logisticMovement.MovementTransaction.ScenarioId == Entities.Enumeration.ScenarioType.OFFICER ? Constants.LogisticOfficialFit : string.Empty,
                        TransactionCode = logisticMovement.SapTransactionCode,
                        MovementSource = this.GetLogisticMovementSource(logisticMovement, nodeStorages),
                        MovementTypeId = logisticMovement.HomologatedMovementType,
                        SalesOrd = documentNumberSales,
                        PositionOrd = positionSales,
                        NetStandardQuantity = logisticMovement.OwnershipVolume == null ? null : Convert.ToString(logisticMovement.OwnershipVolume.Value, CultureInfo.InvariantCulture),
                        MeasurementUnit = logisticMovement.MeasurementUnit == null ? null : Convert.ToString(logisticMovement.MeasurementUnit.Value, CultureInfo.InvariantCulture),
                        PoNumber = documentNumber,
                        PoItem = position,
                        ConstCenter = logisticMovement.CostCenterId == null ? null : logisticMovement.CategoryCostCenter.Name,
                        MovementDestination = this.GetLogisticMovementDestination(logisticMovement, nodeStorages),
                        Attribute = logisticMovement.MovementTransaction.Attributes.Any() ? this.GetLogisticAttribute(logisticMovement.MovementTransaction.Attributes) : null,
                    },
                },
            };

            return sapLogisticRequest;
        }

        /// <summary>
        /// Get logistic attribute.
        /// </summary>
        /// <param name="attributes">The attribute list.</param>
        /// <returns>The SapLogisticAttributeObject.</returns>
        private SapLogisticAttributeObject GetLogisticAttribute(ICollection<AttributeEntity> attributes)
        {
            List<SapLogisticAttributeItem> attributeList = new List<SapLogisticAttributeItem>();

            foreach (var item in attributes)
            {
                if (!string.IsNullOrEmpty(item.AttributeType) && item.ValueAttributeUnit > 0 && item.AttributeId > 0)
                {
                    attributeList.Add(new SapLogisticAttributeItem
                    {
                        AttributeId = item.AttributeId.ToString(CultureInfo.InvariantCulture),
                        AttributeType = item.AttributeType,
                        AttributeUnit = item.ValueAttributeUnit.ToString(CultureInfo.InvariantCulture),
                        AttributeDescription = item.AttributeDescription,
                        AttributeValue = Convert.ToDouble(item.AttributeValue, CultureInfo.InvariantCulture),
                    });
                }
            }

            return new SapLogisticAttributeObject { Attributes = attributeList, };
        }

        /// <summary>
        /// Create the Logistic Destination.
        /// </summary>
        /// <param name="logisticMovement">The logistic movement.</param>
        /// <param name="nodeStorages">The nodeStorages list.</param>
        /// <returns>The SapLogisticDestination.</returns>
        private SapLogisticDestination GetLogisticMovementDestination(LogisticMovement logisticMovement, IEnumerable<NodeStorage> nodeStorages)
        {
            if (logisticMovement?.DestinationLogisticNodeId == null)
            {
                return new SapLogisticDestination();
            }

            var storageLocation = nodeStorages.Where(x => x.NodeId == logisticMovement.DestinationLogisticNodeId &&
            x.ProductId == logisticMovement.DestinationProductId &&
            x.LogisticCenterId == logisticMovement.DestinationLogisticCenterId).Select(y => y.SapStorage).FirstOrDefault();
            return new SapLogisticDestination
            {
                DestinationProductId = logisticMovement.DestinationProductId,
                DestinationPlant = logisticMovement.DestinationLogisticCenterId,
                DestinationStorageLocationId = storageLocation,
            };
        }

        /// <summary>
        /// Create the Logistic Source.
        /// </summary>
        /// <param name="logisticMovement">The logistic movement.</param>
        /// <param name="nodeStorages">The nodeStorages list.</param>
        /// <returns>The SapLogisticSource.</returns>
        private SapLogisticSource GetLogisticMovementSource(LogisticMovement logisticMovement, IEnumerable<NodeStorage> nodeStorages)
        {
            if (logisticMovement?.SourceLogisticNodeId == null)
            {
                return new SapLogisticSource();
            }

            var storageLocation = nodeStorages.Where(x => x.NodeId == logisticMovement.SourceLogisticNodeId &&
            x.ProductId == logisticMovement.SourceProductId &&
            x.LogisticCenterId == logisticMovement.SourceLogisticCenterId).Select(y => y.SapStorage).FirstOrDefault();

            return new SapLogisticSource
            {
                SourceProductId = logisticMovement.SourceProductId,
                SourcePlant = logisticMovement.SourceLogisticCenterId,
                SourceStorageLocationId = storageLocation,
            };
        }

        /// <summary>
        /// Create the Logistic Order.
        /// </summary>
        /// <param name="totalMovements">The total of movements to sent.</param>
        /// <param name="logisticMovement">The logistic movement.</param>
        /// <returns>The SapLogisticOrder.</returns>
        private SapLogisticOrder GetLogisticOrder(int totalMovements, LogisticMovement logisticMovement)
        {
            return new SapLogisticOrder
            {
                OrderNode = logisticMovement.NodeOrder == null ? null : Convert.ToString(logisticMovement.MovementOrder.Value, CultureInfo.InvariantCulture),
                BatchId = Convert.ToString(logisticMovement.TicketId, CultureInfo.InvariantCulture),
                NumReg = Convert.ToString(totalMovements, CultureInfo.InvariantCulture),
                OrderMovement = logisticMovement.MovementOrder == null ? null : Convert.ToString(logisticMovement.MovementOrder.Value, CultureInfo.InvariantCulture),
                Segment = logisticMovement.MovementTransaction.Segment.Name,
            };
        }

        private async Task UpdateTicketFailAsync(LogisticQueueMessage ticket, IRepository<Ticket> ticketRepository)
        {
            var ticketFail = await ticketRepository.GetByIdAsync(ticket?.TicketId).ConfigureAwait(false);
            ticketFail.Status = StatusType.FAILED;
            ticketFail.ErrorMessage = Constants.SapTechnicalErrorSendLogisticMovement;
            ticketRepository.Update(ticketFail);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        private async Task UpdateLogisticMovementsFailAsync(LogisticQueueMessage ticket)
        {
            var logisticRepository = this.unitOfWork.CreateRepository<LogisticMovement>();
            var movementsFail = await logisticRepository.GetAllAsync(x => x.TicketId == ticket.TicketId).ConfigureAwait(false);
            movementsFail.ForEach(x =>
            {
                x.StatusProcessId = StatusType.FAILED;
                x.MessageProcess = Constants.SapTechnicalErrorSendLogisticMovement;
            });
            logisticRepository.UpdateAll(movementsFail);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }
    }
}