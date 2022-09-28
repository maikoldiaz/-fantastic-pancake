// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Interfaces;

    /// <summary>
    /// The DeltaProcessor.
    /// </summary>
    public class DeltaProcessor : ProcessorBase, IDeltaProcessor
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<DeltaProcessor> logger;

        /// <summary>
        /// The ownership rule service.
        /// </summary>
        private readonly IExecutionChainBuilder executionChainBuilder;

        /// <summary>
        /// The execution manager.
        /// </summary>
        private readonly IExecutionManager executionManager;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The finalizer.
        /// </summary>
        private readonly IFinalizer finalizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaProcessor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="executionChainBuilder">The execution chain builder.</param>
        /// <param name="executionManagerFactory">The execution manager.</param>
        /// <param name="officialDeltaProcessor">The officialDelta processor.</param>
        /// <param name="finalizerFactory">The finalizer factory.</param>
        /// <param name="transformationOfficialDeltaNode">The transformation official delta processor.</param>
        public DeltaProcessor(
           ITrueLogger<DeltaProcessor> logger,
           IRepositoryFactory factory,
           IUnitOfWorkFactory unitOfWorkFactory,
           IExecutionChainBuilder executionChainBuilder,
           IExecutionManagerFactory executionManagerFactory,
           IFinalizerFactory finalizerFactory)
           : base(factory)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            ArgumentValidators.ThrowIfNull(executionManagerFactory, nameof(executionManagerFactory));
            ArgumentValidators.ThrowIfNull(finalizerFactory, nameof(finalizerFactory));

            this.logger = logger;
            this.executionChainBuilder = executionChainBuilder;
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.executionManager = executionManagerFactory.GetExecutionManager(True.Entities.Enumeration.TicketType.Delta);
            this.finalizer = finalizerFactory.GetFinalizer(TicketType.Delta);
        }

        /// <summary>
        /// Processes the asynchronous.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="chainType">The chain type.</param>
        /// <returns>The object.</returns>
        public async Task<DeltaData> ProcessAsync(object data, ChainType chainType)
        {
            ArgumentValidators.ThrowIfNull(data, nameof(data));

            var deltaData = (DeltaData)data;
            if (deltaData.HasProcessingErrors)
            {
                this.logger.LogInformation($"Delta processing aborted for chain {chainType}.", $"{deltaData.Ticket.TicketId}");
                return deltaData;
            }

            this.logger.LogInformation($"Delta processing for chain {chainType} started.", $"{deltaData.Ticket.TicketId}");

            var executor = this.executionChainBuilder.Build(ProcessType.Delta, chainType);
            this.logger.LogInformation($"Delta rule processing will start from {executor.GetType().Name}", $"{deltaData.Ticket.TicketId}");

            this.executionManager.Initialize(executor);
            var result = await this.executionManager.ExecuteChainAsync(deltaData).ConfigureAwait(false);

            this.logger.LogInformation($"Delta processing for chain {chainType} finished.", $"{deltaData.Ticket.TicketId}");

            return (DeltaData)result;
        }

        /// <summary>
        /// Validates the ticket asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>
        /// The isValid and ticket entity.
        /// </returns>
        public async Task<(bool isValid, Ticket ticket)> ValidateTicketAsync(int ticketId)
        {
            var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();
            var ticket = await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);

            if (ticket == null || ticket.TicketTypeId != TicketType.Delta || ticket.Status != StatusType.PROCESSING)
            {
                this.logger.LogInformation($"Ticket {ticketId} does not exists or is already processed.", $"{ticketId}");
                return (false, null);
            }

            var ticketCount = await ticketRepository.GetCountAsync(a =>
                          a.TicketId != ticketId &&
                          a.CategoryElementId == ticket.CategoryElementId &&
                          a.Status == StatusType.PROCESSING &&
                          (a.TicketTypeId == TicketType.Cutoff ||
                          a.TicketTypeId == TicketType.Delta ||
                          a.TicketTypeId == TicketType.OfficialDelta ||
                           a.TicketTypeId == TicketType.Ownership)).ConfigureAwait(false);

            return (ticketCount == 0, ticket);
        }

        /// <inheritdoc />
        public Task FinalizeProcessAsync(int ticketId)
        {
            return this.finalizer.ProcessAsync(ticketId);
        }

        /// <summary>
        /// Gets the official delta period.
        /// </summary>
        /// <param name="segmentId">The segment identifier.</param>
        /// <param name="years">The years.</param>
        /// <param name="isPerNodeReport">if set to <c>true</c> [is per node report].</param>
        /// <returns>
        /// The period list.
        /// </returns>
        public async Task<OfficialDeltaPeriodInfo> GetOfficialDeltaPeriodAsync(int segmentId, int years, bool isPerNodeReport)
        {
            ArgumentValidators.ThrowIfNull(segmentId, nameof(segmentId));
            ArgumentValidators.ThrowIfNull(years, nameof(years));
            var parameters = new Dictionary<string, object> { { "@SegmentId", segmentId }, { "@NoOfYears", years }, { "@IsPerNodeReport", isPerNodeReport } };
            var repository = this.unitOfWork.CreateRepository<OfficialDeltaMovementPeriodInfo>();
            var periodInfo = await repository.ExecuteQueryAsync(Repositories.Constants.GetOfficialDeltaPeriod, parameters).ConfigureAwait(false);
            var groupedPeriodInfo = periodInfo.GroupBy(x => x.YearInfo);
            var result = new OfficialDeltaPeriodInfo();
            foreach (var period in groupedPeriodInfo)
            {
                result.OfficialPeriods.Add(
                    Convert.ToString(period.Key, CultureInfo.InvariantCulture),
                    period.ToArray()[0].MonthInfo == 0 ? new List<OfficialDeltaMovementPeriodInfo>() : period.OrderByDescending(a => a.MonthInfo).ToList());
            }

            return result;
        }

        /// <inheritdoc />
        public async Task<bool> ValidatePreviousOfficialPeriodAsync(Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var parameters = new Dictionary<string, object>
            {
                { "@StartDate", ticket.StartDate },
                { "@EndDate", ticket.EndDate },
                { "@SegmentId", ticket.CategoryElementId },
            };
            var repository = this.unitOfWork.CreateRepository<ValidatePreviousOfficialPeriod>();
            var isNotApproved = await repository.ExecuteQueryAsync(Repositories.Constants.ValidatePreviousOfficialPeriod, parameters).ConfigureAwait(false);
            return isNotApproved.FirstOrDefault().UnApprovedNodes != 0;
        }

        /// <inheritdoc />
        public async Task<string> GetOfficialDeltaTicketProcessingStatusAsync(int segmentId)
        {
            var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();
            var ticketsInProcessing = segmentId == 0 ? await ticketRepository.GetAllAsync(a => a.Status == StatusType.PROCESSING && a.TicketTypeId == TicketType.OfficialDelta).ConfigureAwait(false) :
                await ticketRepository.GetAllAsync(a => a.CategoryElementId == segmentId && a.Status == StatusType.PROCESSING && a.TicketTypeId == TicketType.OfficialDelta).ConfigureAwait(false);

            if (ticketsInProcessing != null && ticketsInProcessing.Any())
            {
                return TicketType.OfficialDelta.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the unapproved official nodes asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>Unapproved Official Nodes.</returns>
        public async Task<IEnumerable<UnapprovedOfficialNodes>> GetUnapprovedOfficialNodesAsync(Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            var parameters = new Dictionary<string, object>
            {
                { "@SegmentId", ticket.CategoryElementId },
                { "@NodeId", ticket.NodeId.GetValueOrDefault() },
                { "@StartDate", ticket.StartDate },
                { "@EndDate", ticket.EndDate },
            };
            var repository = this.unitOfWork.CreateRepository<UnapprovedOfficialNodes>();
            var nodes = await repository.ExecuteQueryAsync(Repositories.Constants.GetUnapprovedOfficialNodes, parameters).ConfigureAwait(false);
            return nodes;
        }
    }
}