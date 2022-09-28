// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidationProcessor.cs" company="Microsoft">
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
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Interfaces;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// The ConsolidationProcessor.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Delta.Interfaces.IConsolidationProcessor" />
    public class ConsolidationProcessor : IConsolidationProcessor
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<ConsolidationProcessor> logger;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The consolidation strategy factory.
        /// </summary>
        private readonly IConsolidationStrategyFactory consolidationStrategyFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsolidationProcessor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="consolidationStrategyFactory">The consolidation strategy factory.</param>
        public ConsolidationProcessor(
            ITrueLogger<ConsolidationProcessor> logger,
            IUnitOfWorkFactory unitOfWorkFactory,
            IAzureClientFactory azureClientFactory,
            IConsolidationStrategyFactory consolidationStrategyFactory)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));

            this.logger = logger;
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.azureClientFactory = azureClientFactory;
            this.consolidationStrategyFactory = consolidationStrategyFactory;
        }

        /// <summary>
        /// Consolidates the asynchronous.
        /// </summary>
        /// <param name="batchInfo">The batchInfo data.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task ConsolidateAsync(ConsolidationBatch batchInfo)
        {
            await this.consolidationStrategyFactory.MovementConsolidationStrategy.ConsolidateAsync(batchInfo, this.unitOfWork).ConfigureAwait(false);
            await this.consolidationStrategyFactory.InventoryProductConsolidationStrategy.ConsolidateAsync(batchInfo, this.unitOfWork).ConfigureAwait(false);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the consolidation batches.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>
        /// The collection of BatchInfo.
        /// </returns>
        public async Task<IEnumerable<ConsolidationBatch>> GetConsolidationBatchesAsync(Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));

            var isValid = await ValidateConsolidatedMovementsAndConsolidatedInventoryProductExistsAsync(ticket, this.unitOfWork).ConfigureAwait(false);

            if (!isValid)
            {
                this.logger.LogInformation(
                   $"Consolidated Movements or Consolidated Inventory Product already exists for segmentId {ticket.CategoryElementId} " +
                   $"for period {ticket.StartDate}-{ticket.EndDate}", $"{ticket.TicketId}");

                return new List<ConsolidationBatch>();
            }

            var consolidationDataNodes = await this.GetConsolidationNodeDataAsync(ticket).ConfigureAwait(false);
            var batches = consolidationDataNodes.Select(x => new ConsolidationBatch
            {
                Ticket = ticket,
                ConsolidationNodes = x,
            }).ToList();

            var firstBatch = batches.FirstOrDefault();
            if (firstBatch != null)
            {
                firstBatch.ShouldProcessInventory = true;
            }

            return batches;
        }

        /// <summary>
        /// Completes the consolidation asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="segmentId">The segment identifier.</param>
        /// <returns>The task.</returns>
        public async Task CompleteConsolidationAsync(int ticketId, int segmentId)
        {
            ArgumentValidators.ThrowIfNull(ticketId, nameof(ticketId));
            var parameters = new Dictionary<string, object>
                {
                    { "@TicketId", ticketId },
                };

            var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();
            await ticketRepository.ExecuteAsync(Repositories.Constants.CompleteConsolidation, parameters).ConfigureAwait(false);
            this.logger.LogInformation($"The stored procedure {Repositories.Constants.CompleteConsolidation} executed successfully", $"{ticketId}");

            await this.PushMessageToServiceBusAsync(QueueConstants.OfficialDeltaQueue, ticketId, segmentId).ConfigureAwait(false);
        }

        /// <summary>
        /// Validates the ticket asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>
        /// The isValid and ticket entity.
        /// </returns>
        public async Task<(bool isValid, Ticket ticket, string errorMessage)> ValidateTicketAsync(int ticketId)
        {
            var errorMessage = string.Empty;
            var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();
            var ticket = await ticketRepository.SingleOrDefaultAsync(a => a.TicketId == ticketId, "CategoryElement").ConfigureAwait(false);

            if (ticket == null || ticket.TicketTypeId != TicketType.OfficialDelta || ticket.Status != StatusType.PROCESSING)
            {
                errorMessage = $"Ticket {ticketId} does not exists or is already processed.";
                this.logger.LogInformation(errorMessage, $"{ticketId}");
                return (false, null, errorMessage);
            }

            var isValid = await ValidateOfficialDeltaTicketExistsForSegmentAsync(ticketRepository, ticket).ConfigureAwait(false);
            if (!isValid)
            {
                return (false, ticket, Constants.OfficialDeltaCalculationInProgress);
            }

            isValid = await ValidateDeltaIsApprovedForPreviousPeriodAsync(ticketRepository, this.unitOfWork, ticket).ConfigureAwait(false);
            if (!isValid)
            {
                return (false, ticket, Constants.OfficialDeltaWithoutApprovalInPreviousPeriod);
            }

            return (true, ticket, errorMessage);
        }

        private static async Task<bool> ValidateConsolidatedMovementsAndConsolidatedInventoryProductExistsAsync(Ticket ticket, IUnitOfWork unitOfWork)
        {
            var consolidatedMovementRepository = unitOfWork.CreateRepository<ConsolidatedMovement>();
            var consolidatedInventoryRepository = unitOfWork.CreateRepository<ConsolidatedInventoryProduct>();

            var existingConsolidatedMovementsCount = await consolidatedMovementRepository.GetCountAsync(
                a => a.SegmentId == ticket.CategoryElementId &&
                (a.StartDate <= ticket.StartDate &&
                a.EndDate >= ticket.StartDate) &&
                a.IsActive).ConfigureAwait(false);

            var existingConsolidatedInventoryProductsForFinalDateCount = await consolidatedInventoryRepository.GetCountAsync(
                x => x.SegmentId == ticket.CategoryElementId &&
                x.InventoryDate == ticket.EndDate &&
                x.IsActive).ConfigureAwait(false);

            return existingConsolidatedMovementsCount == 0 && existingConsolidatedInventoryProductsForFinalDateCount == 0;
        }

        private static async Task<bool> ValidateOfficialDeltaTicketExistsForSegmentAsync(IRepository<Ticket> ticketRepository, Ticket ticket)
        {
            var officialDeltaTicketsForSegment = await ticketRepository.GetCountAsync(
               a => a.TicketId != ticket.TicketId &&
               a.CategoryElementId == ticket.CategoryElementId &&
               a.TicketTypeId == TicketType.OfficialDelta &&
               a.Status == StatusType.PROCESSING).ConfigureAwait(false);

            return officialDeltaTicketsForSegment == 0;
        }

        private static async Task<bool> ValidateDeltaIsApprovedForPreviousPeriodAsync(IRepository<Ticket> ticketRepository, IUnitOfWork unitOfWork, Ticket ticket)
        {
            var lastTickets = await ticketRepository.OrderByDescendingAsync(
                a => a.CategoryElementId == ticket.CategoryElementId &&
                a.TicketTypeId == TicketType.OfficialDelta &&
                a.EndDate < ticket.StartDate,
                s => s.TicketId,
                1).ConfigureAwait(false);

            if (!lastTickets.Any())
            {
                return true;
            }

            var deltaNodeRepository = unitOfWork.CreateRepository<DeltaNode>();
            var notApprovedDeltaNodes =
                await deltaNodeRepository.GetCountAsync(a => a.TicketId == lastTickets.First().TicketId && a.Status != OwnershipNodeStatusType.APPROVED).ConfigureAwait(false);

            return notApprovedDeltaNodes == 0;
        }

        private static IEnumerable<IEnumerable<T>> Batch<T>(IEnumerable<T> collection, int batchSize)
        {
            var nextbatch = new List<T>(batchSize);
            foreach (T item in collection)
            {
                nextbatch.Add(item);
                if (nextbatch.Count == batchSize)
                {
                    yield return nextbatch;
                    nextbatch = new List<T>(batchSize);
                }
            }

            if (nextbatch.Count > 0)
            {
                yield return nextbatch;
            }
        }

        private async Task<IEnumerable<IEnumerable<ConsolidationNodeData>>> GetConsolidationNodeDataAsync(Ticket ticket)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@SegmentId", ticket.CategoryElementId },
                { "@StartDate", ticket.StartDate },
                { "@EndDate", ticket.EndDate },
            };

            var consolidationNodeDataRepository = this.unitOfWork.CreateRepository<ConsolidationNodeData>();
            var consolidationNodes = await consolidationNodeDataRepository.ExecuteQueryAsync(Repositories.Constants.GetMovementNodesForConsolidation, parameters).ConfigureAwait(false);

            var batchSize = Math.Abs(consolidationNodes.Count() / 5);
            batchSize = batchSize == 0 ? 1 : batchSize;
            return Batch(consolidationNodes, batchSize);
        }

        /// <summary>
        /// Pushes the message to service bus asynchronous.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="segmentId">The segment identifier.</param>
        /// <returns>The task.</returns>
        private async Task PushMessageToServiceBusAsync(string queueName, int ticketId, int segmentId)
        {
            var queueClient = this.azureClientFactory.GetQueueClient(queueName);
            await queueClient.QueueSessionMessageAsync(ticketId, segmentId.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
        }
    }
}
