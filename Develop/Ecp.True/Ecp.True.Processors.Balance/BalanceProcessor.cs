// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BalanceProcessor.cs" company="Microsoft">
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
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Balance.Calculation.Input;
    using Ecp.True.Processors.Balance.Calculation.Output;
    using Ecp.True.Processors.Balance.Interfaces;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// The ticket processor.
    /// </summary>
    public class BalanceProcessor : ProcessorBase, IBalanceProcessor
    {
        /// <summary>
        /// The repository factory.
        /// </summary>
        private readonly IRepositoryFactory repositoryFactory;

        /// <summary>
        /// The balance service.
        /// </summary>
        private readonly IBalanceService balanceService;

        /// <summary>
        /// The balance service.
        /// </summary>
        private readonly ISegmentBalanceService segmentBalanceService;

        /// <summary>
        /// The balance service.
        /// </summary>
        private readonly ISystemBalanceService systemBalanceService;

        /// <summary>
        /// The finalizer.
        /// </summary>
        private readonly IFinalizer finalizer;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<BalanceProcessor> logger;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;
        private readonly IRegistrationStrategyFactory registrationStrategyFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="BalanceProcessor" /> class.
        /// </summary>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="balanceService">The balance service.</param>
        /// <param name="segmentBalanceService">The segment balance service.</param>
        /// <param name="systemBalanceService">The system balance service.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="registrationStrategyFactory">The registration factory.</param>
        /// <param name="finalizer">The finalizer.</param>
        public BalanceProcessor(
            IRepositoryFactory repositoryFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            IBalanceService balanceService,
            ISegmentBalanceService segmentBalanceService,
            ISystemBalanceService systemBalanceService,
            ITrueLogger<BalanceProcessor> logger,
            IConfigurationHandler configurationHandler,
            IRegistrationStrategyFactory registrationStrategyFactory,
            IFinalizer finalizer)
            : base(repositoryFactory)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            this.repositoryFactory = repositoryFactory;
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.balanceService = balanceService;
            this.segmentBalanceService = segmentBalanceService;
            this.systemBalanceService = systemBalanceService;
            this.logger = logger;
            this.configurationHandler = configurationHandler;
            this.registrationStrategyFactory = registrationStrategyFactory;
            this.finalizer = finalizer;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UnbalanceComment>> CalculateAsync(UnbalanceRequest getUnbalances)
        {
            ArgumentValidators.ThrowIfNull(getUnbalances, nameof(getUnbalances));
            var calculationOutput = await this.balanceService.ProcessStepAsync(getUnbalances, MovementCalculationStep.Unbalance, null, this.logger).ConfigureAwait(false);
            var unbalances = calculationOutput.Unbalances;
            unbalances = unbalances.Where(x => x.UnbalancePercentage > x.AcceptableBalance);

            var i = 1;
            unbalances.ForEach(x => x.UnbalanceId = i++);
            return unbalances.OrderBy(x => x.CalculationDate);
        }

        /// <inheritdoc/>
        public async Task<Ticket> GetTicketByIdAsync(int ticketId)
        {
            this.logger.LogInformation($"Getting the ticket with {ticketId}.", $"{ticketId}");
            var ticketRepository = this.repositoryFactory.CreateRepository<Ticket>();
            return await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<BalanceInput> GetBalanceInputAsync(Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            this.logger.LogInformation($"Getting the balance input for the ticket with {ticket.TicketId}.", $"{ticket.TicketId}");
            return await this.balanceService.GetBalanceInputAsync(new UnbalanceRequest(ticket)).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<CalculationOutput> ProcessCalculationAsync(OperationalCutOffInfo operationalCutOffInput)
        {
            ArgumentValidators.ThrowIfNull(operationalCutOffInput, nameof(operationalCutOffInput));
            this.logger.LogInformation($"Processing the calculation for the ticket with {operationalCutOffInput.Ticket.TicketId}.", $"{operationalCutOffInput.Ticket.TicketId}");
            return await this.balanceService.ProcessCalculationAsync(
                operationalCutOffInput.BalanceInput,
                operationalCutOffInput.Ticket,
                this.unitOfWork,
                operationalCutOffInput.Step).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<SegmentUnbalance>> ProcessSegmentAsync(int ticketId)
        {
            this.logger.LogInformation($"Processing the segment for the ticket with {ticketId}.", $"{ticketId}");
            return this.segmentBalanceService.ProcessSegmentAsync(ticketId, this.unitOfWork);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<SystemUnbalance>> ProcessSystemAsync(int ticketId)
        {
            this.logger.LogInformation($"Processing the system for the ticket with {ticketId}.", $"{ticketId}");
            return this.systemBalanceService.ProcessSystemAsync(ticketId, this.unitOfWork);
        }

        /// <inheritdoc/>
        public async Task RegisterAsync(OperationalCutOffInfo operationalCutOffInput)
        {
            ArgumentValidators.ThrowIfNull(operationalCutOffInput, nameof(operationalCutOffInput));
            this.registrationStrategyFactory.MovementRegistrationStrategy.Insert(operationalCutOffInput.Movements, this.unitOfWork);

            var unbalanceRepository = this.unitOfWork.CreateRepository<Unbalance>();
            var unbalanceList = await this.BuildUnbalancesAsync(operationalCutOffInput).ConfigureAwait(false);
            unbalanceRepository.InsertAll(unbalanceList);

            this.logger.LogInformation($"Registering the movements and unbalances for ticket {operationalCutOffInput.Ticket.TicketId}.", $"{operationalCutOffInput.Ticket.TicketId}");
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task CompleteAsync(OperationalCutOffInfo operationalCutOffInput)
        {
            ArgumentValidators.ThrowIfNull(operationalCutOffInput, nameof(operationalCutOffInput));

            var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();
            operationalCutOffInput.Ticket.Status = StatusType.PROCESSED;
            ticketRepository.Update(operationalCutOffInput.Ticket);

            var segmentUnbalanceRepository = this.unitOfWork.CreateRepository<SegmentUnbalance>();
            segmentUnbalanceRepository.InsertAll(operationalCutOffInput.SegmentUnbalances);

            var systemUnbalanceRepository = this.unitOfWork.CreateRepository<SystemUnbalance>();
            systemUnbalanceRepository.InsertAll(operationalCutOffInput.SystemUnbalances);

            this.logger.LogInformation($"Completing the ticket with {operationalCutOffInput.Ticket.TicketId}.", $"{operationalCutOffInput.Ticket.TicketId}");
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);

            await this.SaveOperativeNodeRelationshipAsync(ticketRepository, operationalCutOffInput.Ticket.TicketId).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task CleanOperationalCutOffDataAsync(int ticketId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@TicketId", ticketId },
            };

            var repository = this.unitOfWork.CreateRepository<Ownership>();
            await repository.ExecuteAsync(Repositories.Constants.OperationalCutOffAndOwnershipCleanupProcedureName, parameters).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task HandleFailureAsync(int ticketId, string errorMessage)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@TicketId", ticketId },
            };

            var repository = this.unitOfWork.CreateRepository<Ownership>();
            await repository.ExecuteAsync(Repositories.Constants.OperationalCutOffAndOwnershipCleanupProcedureName, parameters).ConfigureAwait(false);

            var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();
            var ticket = await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);
            ticket.Status = StatusType.FAILED;
            ticket.ErrorMessage = errorMessage;
            ticketRepository.Update(ticket);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Getting the nodes.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The list.</returns>
        public Task<IEnumerable<OwnershipInitialInventoryNode>> ValidateOwnershipInitialInventoryAsync(Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var ownershipRepository = this.unitOfWork.CreateRepository<OwnershipInitialInventoryNode>();
            var parameters = new Dictionary<string, object>
            {
                { "@SegmentId", ticket.CategoryElementId },
                { "@StartDate", ticket.StartDate },
                { "@EndDate", ticket.EndDate },
            };
            return ownershipRepository.ExecuteQueryAsync(Repositories.Constants.ValidateInitialInventoriesForOwnershipProcedureName, parameters);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<OfficialTransferPointMovement>> GetTransferPointsAsync(Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var officialPointsRepository = this.unitOfWork.CreateRepository<OfficialTransferPointMovement>();
            var parameters = new Dictionary<string, object>
            {
                { "@SegmentId", ticket.CategoryElementId },
                { "@StartDate", ticket.StartDate },
                { "@EndDate", ticket.EndDate },
            };

            return officialPointsRepository.ExecuteQueryAsync(Repositories.Constants.GetOfficialTransferPointMovements, parameters);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SapTrackingError>> GetSapTrackingErrorsAsync(int sapTrackingId)
        {
            ArgumentValidators.ThrowIfNull(sapTrackingId, nameof(sapTrackingId));
            var errorRepository = this.unitOfWork.CreateRepository<SapTrackingError>();
            var errors = await errorRepository.GetAllAsync(x => x.SapTrackingId == sapTrackingId).ConfigureAwait(false);
            return errors.OrderBy(x => x.ErrorCode);
        }

        /// <inheritdoc />
        public Task FinalizeProcessAsync(int ticketId)
        {
            return this.finalizer.ProcessAsync(ticketId);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<int>> GetFirstTimeNodesAsync(Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var initialNodeRepository = this.unitOfWork.CreateRepository<InitialNode>();
            var parameters = new Dictionary<string, object>
            {
                { "@SegmentId", ticket.CategoryElementId },
                { "@StartDate", ticket.StartDate },
                { "@EndDate", ticket.EndDate },
            };
            var result = await initialNodeRepository.ExecuteQueryAsync(Repositories.Constants.GetFirstTimeNodesProcedureName, parameters).ConfigureAwait(false);
            return result.Select(x => x.NodeId);
        }

        /// <inheritdoc/>
        public async Task DeleteBalanceAsync<TEntity>(int ticketId)
        where TEntity : class, ITicketEntity
        {
            var balanceRepository = this.unitOfWork.CreateRepository<TEntity>();
            var movements = await balanceRepository.GetAllAsync(x => x.TicketId == ticketId).ConfigureAwait(false);
            if (movements.Any())
            {
                balanceRepository.DeleteAll(movements);
                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        private async Task<IEnumerable<Unbalance>> BuildUnbalancesAsync(OperationalCutOffInfo operationalCutOffInput)
        {
            var unbalanceDictionary = operationalCutOffInput.Unbalances;
            var unbalanceList = await this.BuildAllUnbalancesAsync(operationalCutOffInput).ConfigureAwait(false);

            foreach (var unidentifiedLossItem in unbalanceDictionary[MovementCalculationStep.UnidentifiedLosses])
            {
                var unbalance = unbalanceList
                    .Single(y => y.NodeId == unidentifiedLossItem.NodeId && y.ProductId == unidentifiedLossItem.ProductId && y.CalculationDate == unidentifiedLossItem.CalculationDate);

                unbalance.UnidentifiedLosses = unidentifiedLossItem.UnidentifiedLosses;
                unbalance.UnidentifiedLossesUnbalance = unidentifiedLossItem.UnidentifiedLossesUnbalance;
            }

            foreach (var unbalance in unbalanceList)
            {
                var unbalanceItem = unbalanceDictionary[MovementCalculationStep.Unbalance]
                    .Single(y => y.NodeId == unbalance.NodeId && y.ProductId == unbalance.ProductId && y.CalculationDate == unbalance.CalculationDate);
                unbalance.UnbalanceAmount = unbalanceItem.UnbalanceAmount;
                unbalance.BlockchainStatus = StatusType.PROCESSING;
            }

            return unbalanceList;
        }

        private async Task<IEnumerable<Unbalance>> BuildAllUnbalancesAsync(OperationalCutOffInfo operationalCutOffInput)
        {
            var unbalanceDictionary = operationalCutOffInput.Unbalances;
            var unbalanceList = new List<Unbalance>();
            unbalanceList.AddRange(unbalanceDictionary[MovementCalculationStep.Interface]);
            var systemConfig = await this.configurationHandler.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings).ConfigureAwait(false);

            foreach (var balanceToleranceItem in unbalanceDictionary[MovementCalculationStep.BalanceTolerances])
            {
                var unbalance = unbalanceList
                    .SingleOrDefault(y => y.NodeId == balanceToleranceItem.NodeId && y.ProductId == balanceToleranceItem.ProductId && y.CalculationDate == balanceToleranceItem.CalculationDate);
                if (unbalance != null)
                {
                    unbalance.IdentifiedLosses = balanceToleranceItem.IdentifiedLosses;
                    unbalance.Tolerance = balanceToleranceItem.Tolerance;
                    unbalance.ToleranceUnbalance = balanceToleranceItem.ToleranceUnbalance;
                    unbalance.StandardUncertainty = balanceToleranceItem.StandardUncertainty;
                    unbalance.ToleranceIdentifiedLosses = balanceToleranceItem.IdentifiedLosses;
                    unbalance.ToleranceInputs = balanceToleranceItem.ToleranceInputs;
                    unbalance.ToleranceOutputs = balanceToleranceItem.ToleranceOutputs;
                    unbalance.ToleranceInitialInventory = balanceToleranceItem.ToleranceInitialInventory;
                    unbalance.ToleranceFinalInventory = balanceToleranceItem.ToleranceFinalInventory;
                    if (balanceToleranceItem.ToleranceInputs != 0.0m)
                    {
                        unbalance.AverageUncertainty = (balanceToleranceItem.StandardUncertainty / balanceToleranceItem.ToleranceInputs) * 100;
                        unbalance.Warning = unbalance.AverageUncertainty * systemConfig.WarningLimit.GetValueOrDefault();
                        unbalance.Action = unbalance.AverageUncertainty * systemConfig.ActionLimit.GetValueOrDefault();
                        unbalance.ControlTolerance = unbalance.AverageUncertainty * systemConfig.ToleranceLimit.GetValueOrDefault();
                        unbalance.AverageUncertaintyUnbalancePercentage = (balanceToleranceItem.ToleranceUnbalance / balanceToleranceItem.ToleranceInputs) * 100;
                    }
                }
                else
                {
                    unbalanceList.Add(balanceToleranceItem);
                }
            }

            return unbalanceList;
        }

        private async Task SaveOperativeNodeRelationshipAsync(IRepository<Ticket> ticketRepository, int ticketId)
        {
            ArgumentValidators.ThrowIfNull(ticketId, nameof(ticketId));

            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@TicketId", ticketId },
                };

                await ticketRepository.ExecuteAsync(Repositories.Constants.SaveOperativeNodeRelationship, parameters).ConfigureAwait(false);
                this.logger.LogInformation($"The stored procedure {Repositories.Constants.SaveOperativeNodeRelationship} executed successfully", $"{ticketId}");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message, $"{ticketId}");
            }
        }
    }
}
