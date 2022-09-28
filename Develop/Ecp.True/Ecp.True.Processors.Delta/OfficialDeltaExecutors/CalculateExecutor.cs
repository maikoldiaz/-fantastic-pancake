// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalculateExecutor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.OfficialDeltaExecutors
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Delta.Calculation;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;

    /// <summary>
    /// The CalculateExecutor class.
    /// </summary>
    public class CalculateExecutor : ExecutorBase
    {
        /// <summary>
        /// The movement map.
        /// </summary>
        private readonly ConcurrentDictionary<string, List<Movement>> movementMap =
            new ConcurrentDictionary<string, List<Movement>>();

        /// <summary>
        /// The consolidated movement map.
        /// </summary>
        private readonly ConcurrentDictionary<string, List<ConsolidatedMovement>> consolidatedMovementMap =
            new ConcurrentDictionary<string, List<ConsolidatedMovement>>();

        /// <summary>
        /// The consolidated inventory map.
        /// </summary>
        private readonly ConcurrentDictionary<string, List<ConsolidatedInventoryProduct>> consolidatedInventoryMap =
            new ConcurrentDictionary<string, List<ConsolidatedInventoryProduct>>();

        /// <summary>
        /// The calculator.
        /// </summary>
        private readonly IDeltaBalanceCalculator deltaBalanceCalculator;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculateExecutor"/> class.
        /// </summary>
        /// <param name="deltaBalanceCalculator">The delta balance calculator.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="logger">The logger.</param>
        public CalculateExecutor(
             IDeltaBalanceCalculator deltaBalanceCalculator,
             IUnitOfWorkFactory unitOfWorkFactory,
             ITrueLogger<CalculateExecutor> logger)
            : base(logger)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            this.deltaBalanceCalculator = deltaBalanceCalculator;
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        }

        /// <inheritdoc/>
        public override int Order => 6;

        /// <summary>
        /// Gets the type of the process.
        /// </summary>
        /// <value>
        /// The type of the process.
        /// </value>
        public override ProcessType ProcessType => ProcessType.OfficialDelta;

        /// <inheritdoc/>
        public override async Task ExecuteAsync(object input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));

            var deltaData = (OfficialDeltaData)input;
            var deltaNodes = deltaData.GetPendingOfficialMovementNodes();

            var movements = await this.GetMovementsAsync(deltaNodes, deltaData.Ticket).ConfigureAwait(false);

            this.Logger.LogInformation($"Successfully fetched official movements for pre-calculation for Ticket: {deltaData.Ticket.TicketId}", $"{deltaData.Ticket.TicketId}");

            this.PopulateMovementMap(movements, deltaNodes);
            this.PopulateConsolidatedMovementMap(deltaData.ConsolidationMovements, deltaNodes);
            this.PopulateConsolidatedInventoryMap(deltaData.ConsolidationInventories);

            var deltaBalances = this.deltaBalanceCalculator.Calculate(deltaData.Ticket, this.movementMap, this.consolidatedMovementMap, this.consolidatedInventoryMap);

            await this.RegisterCalculationAsync(deltaBalances, deltaNodes, deltaData.Ticket).ConfigureAwait(false);
            this.Logger.LogInformation($"Registered pre-calculated official delta balance for Ticket: {deltaData.Ticket.TicketId}", $"{deltaData.Ticket.TicketId}");

            this.ShouldContinue = false;
            await this.ExecuteNextAsync(deltaData).ConfigureAwait(false);
        }

        private void PopulateMovementMap(IEnumerable<Movement> movements, IEnumerable<int> deltaNodes)
        {
            movements.ForEach(x =>
            {
                x.Owners.ForEach(y =>
                {
                    this.AddMovement(x, y, deltaNodes);
                });
            });
        }

        private void AddMovement(Movement x, Owner y, IEnumerable<int> deltaNodes)
        {
            var uniqueKeyForSource = string.Empty;
            if (x.MovementSource != null && deltaNodes.Contains(x.MovementSource.SourceNodeId.GetValueOrDefault()))
            {
                uniqueKeyForSource = IdHelper.BuildOfficialCalculationUniqueKey(
                    x.MovementSource.SourceNodeId.ToString(),
                    x.MovementSource.SourceProductId,
                    y.OwnerId.ToString(CultureInfo.InvariantCulture),
                    x.MeasurementUnit.HasValue ? x.MeasurementUnit.ToString() : null);
                this.AddMovementToDictionary(x, uniqueKeyForSource);
            }

            if (x.MovementDestination != null && deltaNodes.Contains(x.MovementDestination.DestinationNodeId.GetValueOrDefault()))
            {
                var uniqueKeyForDestination = IdHelper.BuildOfficialCalculationUniqueKey(
                    x.MovementDestination.DestinationNodeId.ToString(),
                    x.MovementDestination.DestinationProductId,
                    y.OwnerId.ToString(CultureInfo.InvariantCulture),
                    x.MeasurementUnit.HasValue ? x.MeasurementUnit.ToString() : null);

                if (uniqueKeyForSource != uniqueKeyForDestination)
                {
                    this.AddMovementToDictionary(x, uniqueKeyForDestination);
                }
            }
        }

        private void AddMovementToDictionary(Movement x, string uniqueKey)
        {
            this.movementMap.TryGetValue(uniqueKey, out List<Movement> movements);
            if (movements != null)
            {
                movements.Add(x);
            }

            this.movementMap.AddOrUpdate(uniqueKey, new List<Movement> { x }, (key, oldValue) => movements);
        }

        private void PopulateConsolidatedMovementMap(IEnumerable<ConsolidatedMovement> movements, IEnumerable<int> deltaNodes)
        {
            movements.ForEach(x =>
            {
                x.ConsolidatedOwners.ForEach(y =>
                {
                    this.AddConsolidatedMovement(x, y, deltaNodes);
                });
            });
        }

        private void AddConsolidatedMovement(ConsolidatedMovement x, ConsolidatedOwner y, IEnumerable<int> deltaNodes)
        {
            var uniqueKeyForSource = string.Empty;
            if (x.SourceNodeId != null && deltaNodes.Contains(x.SourceNodeId.GetValueOrDefault()))
            {
                uniqueKeyForSource = IdHelper.BuildOfficialCalculationUniqueKey(
                    x.SourceNodeId.ToString(),
                    x.SourceProductId,
                    y.OwnerId.ToString(CultureInfo.InvariantCulture),
                    x.MeasurementUnit);
                this.AddConsolidatedMovementToDictionary(x, uniqueKeyForSource);
            }

            if (x.DestinationNodeId != null && deltaNodes.Contains(x.DestinationNodeId.GetValueOrDefault()))
            {
                var uniqueKeyForDestination = IdHelper.BuildOfficialCalculationUniqueKey(
                    x.DestinationNodeId.ToString(),
                    x.DestinationProductId,
                    y.OwnerId.ToString(CultureInfo.InvariantCulture),
                    x.MeasurementUnit);

                if (uniqueKeyForSource != uniqueKeyForDestination)
                {
                    this.AddConsolidatedMovementToDictionary(x, uniqueKeyForDestination);
                }
            }
        }

        private void AddConsolidatedMovementToDictionary(ConsolidatedMovement x, string uniqueKey)
        {
            this.consolidatedMovementMap.TryGetValue(uniqueKey, out List<ConsolidatedMovement> movements);
            if (movements != null)
            {
                movements.Add(x);
            }

            this.consolidatedMovementMap.AddOrUpdate(uniqueKey, new List<ConsolidatedMovement> { x }, (key, oldValue) => movements);
        }

        private void PopulateConsolidatedInventoryMap(IEnumerable<ConsolidatedInventoryProduct> inventories)
        {
            inventories.ForEach(x =>
            {
                x.ConsolidatedOwners.ForEach(y =>
                {
                    this.AddConsolidatedInventory(x, y);
                });
            });
        }

        private void AddConsolidatedInventory(ConsolidatedInventoryProduct x, ConsolidatedOwner y)
        {
            var uniqueKey = IdHelper.BuildOfficialCalculationUniqueKey(
                 x.NodeId.ToString(CultureInfo.InvariantCulture),
                 x.ProductId,
                 y.OwnerId.ToString(CultureInfo.InvariantCulture),
                 x.MeasurementUnit);
            this.AddConsolidatedInventoryToDictionary(x, uniqueKey);
        }

        private void AddConsolidatedInventoryToDictionary(ConsolidatedInventoryProduct x, string uniqueKey)
        {
            this.consolidatedInventoryMap.TryGetValue(uniqueKey, out List<ConsolidatedInventoryProduct> inventories);
            if (inventories != null)
            {
                inventories.Add(x);
            }

            this.consolidatedInventoryMap.AddOrUpdate(uniqueKey, new List<ConsolidatedInventoryProduct> { x }, (key, oldValue) => inventories);
        }

        private async Task<IEnumerable<Movement>> GetMovementsAsync(IEnumerable<int> nodeIds, Ticket ticket)
        {
            return await this.unitOfWork.MovementRepository.GetMovementsForOfficialDeltaCalculationAsync(nodeIds, ticket).ConfigureAwait(false);
        }

        private async Task RegisterCalculationAsync(IEnumerable<DeltaBalance> deltaBalances, IEnumerable<int> deltaNodes, Ticket ticket)
        {
            var deltaBalanceRepository = this.unitOfWork.CreateRepository<DeltaBalance>();
            var existingDeltaBalances = await deltaBalanceRepository.GetAllAsync(x =>
                deltaNodes.Contains(x.NodeId)
                && x.StartDate.Date == ticket.StartDate.Date && x.EndDate.Date == ticket.EndDate.Date
                && x.SegmentId == ticket.CategoryElementId).ConfigureAwait(false);

            deltaBalanceRepository.DeleteAll(existingDeltaBalances);
            deltaBalanceRepository.InsertAll(deltaBalances);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }
    }
}
