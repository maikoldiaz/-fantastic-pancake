// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementConsolidationStrategy.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Consolidation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Delta.Entities;

    /// <summary>
    /// The MovementConsolidationStrategy.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Delta.Consolidation.ConsolidationStrategyBase" />
    public class MovementConsolidationStrategy : ConsolidationStrategyBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovementConsolidationStrategy"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public MovementConsolidationStrategy(
         ITrueLogger logger)
         : base(logger)
        {
        }

        /// <summary>
        /// Consolidates the asynchronous.
        /// </summary>
        /// <param name="batchInfo">The batch info.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        public override async Task ConsolidateAsync(ConsolidationBatch batchInfo, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(batchInfo, nameof(batchInfo));
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));

            var repository = unitOfWork.CreateRepository<ConsolidationMovementData>();
            var parameters = new Dictionary<string, object>
            {
                { "@SegmentId", batchInfo.Ticket.CategoryElementId },
                { "@StartDate", batchInfo.Ticket.StartDate },
                { "@EndDate", batchInfo.Ticket.EndDate },
                { "@Node", batchInfo.ConsolidationNodes.ToDataTable(Repositories.Constants.MovementNodeType) },
            };

            var movementConsolidationData = (await repository.ExecuteQueryAsync(Repositories.Constants.GetMovementsForConsolidation, parameters).ConfigureAwait(false)).ToList();
            var groupedConsolidatedMovementsList = movementConsolidationData.GroupBy(x => x.MovementTransactionId).Select(grouped => grouped.ToList());

            foreach (var groupedConsolidatedMovements in groupedConsolidatedMovementsList)
            {
                if (this.ValidateOwnershipPercentage(groupedConsolidatedMovements))
                {
                    continue;
                }

                var totalVolume = 0.00M;
                var index = 0;
                foreach (var groupedConsolidatedMovement in groupedConsolidatedMovements)
                {
                    index++;
                    groupedConsolidatedMovement.OwnershipPercentage = groupedConsolidatedMovement.OwnershipVolume;
                    if (index == groupedConsolidatedMovements.Count)
                    {
                        groupedConsolidatedMovement.OwnershipVolume = groupedConsolidatedMovement.NetStandardVolume - totalVolume;
                    }
                    else
                    {
                        groupedConsolidatedMovement.OwnershipVolume = Math.Round(groupedConsolidatedMovement.NetStandardVolume * groupedConsolidatedMovement.OwnershipVolume / 100, 2);
                        totalVolume += groupedConsolidatedMovement.OwnershipVolume;
                    }
                }
            }

            var deltaResultMovements = movementConsolidationData.Where(x => x.OriginalMovementTransactionId != null && x.SourceMovementTypeId != null).ToList();
            var excludedMovements = movementConsolidationData.Where(x => deltaResultMovements.Any(y => y.OriginalMovementTransactionId == x.MovementTransactionId));
            var movementsToConsolidate = movementConsolidationData.Where(x => !deltaResultMovements.Any(y => y.MovementTransactionId == x.MovementTransactionId)
                                         && !excludedMovements.Any(y => y.MovementTransactionId == x.MovementTransactionId)).ToList();

            foreach (var excludedMovement in excludedMovements)
            {
                var deltaResultMovement = deltaResultMovements.First(x => x.OriginalMovementTransactionId == excludedMovement.MovementTransactionId
                && x.OwnerId == excludedMovement.OwnerId);
                excludedMovement.NetStandardVolume -= deltaResultMovement.NetStandardVolume;
                excludedMovement.OwnershipVolume -= deltaResultMovement.OwnershipVolume;
                excludedMovement.GrossStandardVolume -= deltaResultMovement.GrossStandardVolume;
            }

            excludedMovements.ForEach(a => movementsToConsolidate.Add(a));

            var consolidatedMovementRepository = unitOfWork.CreateRepository<ConsolidatedMovement>();

            var consolidatedMovements = BuildConsolidatedMovements(movementsToConsolidate, batchInfo);

            consolidatedMovementRepository.InsertAll(consolidatedMovements);
        }

        private static IEnumerable<ConsolidatedMovement> BuildConsolidatedMovements(
            IEnumerable<ConsolidationMovementData> consolidationMovementData,
            ConsolidationBatch batchInfo)
        {
            var comparer = ExpressionEqualityComparer.Create<ConsolidationMovementData, int>(x => x.MovementTransactionId);

            var consolidatedMovements = consolidationMovementData.Distinct(comparer).GroupBy(x => new
            {
                x.SourceNodeId,
                x.SourceProductId,
                x.DestinationNodeId,
                x.DestinationProductId,
                x.MovementTypeId,
                x.MeasurementUnit,
            }).Select(y => new ConsolidatedMovement
            {
                SourceNodeId = y.Key.SourceNodeId,
                SourceProductId = y.Key.SourceProductId,
                DestinationNodeId = y.Key.DestinationNodeId,
                DestinationProductId = y.Key.DestinationProductId,
                MovementTypeId = y.Key.MovementTypeId.ToString(CultureInfo.InvariantCulture),
                MeasurementUnit = y.Key.MeasurementUnit.HasValue ? y.Key.MeasurementUnit.ToString() : null,
                SourceSystemId = (int)SourceSystem.TRUE,
                StartDate = batchInfo.Ticket.StartDate,
                EndDate = batchInfo.Ticket.EndDate,
                SegmentId = batchInfo.Ticket.CategoryElementId,
                ExecutionDate = DateTime.UtcNow.ToTrue(),
                TicketId = batchInfo.Ticket.TicketId,
                NetStandardVolume = y.Sum(z => z.NetStandardVolume),
                GrossStandardVolume = y.Sum(z => z.GrossStandardVolume.GetValueOrDefault()),
            }).ToList();

            foreach (var i in consolidatedMovements)
            {
                var consolidationMovements = consolidationMovementData.Where(
                   x => x.SourceNodeId == i.SourceNodeId &&
                   string.Equals(x.SourceProductId, i.SourceProductId, StringComparison.OrdinalIgnoreCase) &&
                   x.DestinationNodeId == i.DestinationNodeId &&
                   string.Equals(x.DestinationProductId, i.DestinationProductId, StringComparison.OrdinalIgnoreCase) &&
                   x.MovementTypeId == Convert.ToInt32(i.MovementTypeId, CultureInfo.InvariantCulture));

                var owners = BuildOwners(consolidationMovements);
                owners.ForEach(o => i.ConsolidatedOwners.Add(o));
            }

            return consolidatedMovements;
        }

        private static IEnumerable<ConsolidatedOwner> BuildOwners(IEnumerable<ConsolidationMovementData> consolidationMovements)
        {
            var consolidatedOwners = new List<ConsolidatedOwner>();
            var index = 0;
            var totalPercentages = 0.00M;
            var totalOwnershipVolume = consolidationMovements.Sum(x => x.OwnershipVolume);
            var groupedConsolidationInventoryProducts = consolidationMovements.GroupBy(x => x.OwnerId);
            var length = groupedConsolidationInventoryProducts.Count();
            foreach (var x in groupedConsolidationInventoryProducts)
            {
                index++;
                var ownershipVolume = x.Sum(y => y.OwnershipVolume);
                var percentage = totalOwnershipVolume == 0.0M ? 0.0M : Math.Round((ownershipVolume / totalOwnershipVolume) * 100, 2);
                consolidatedOwners.Add(new ConsolidatedOwner
                {
                    OwnerId = x.Key,
                    OwnershipVolume = ownershipVolume,
                    OwnershipPercentage = index == length && totalOwnershipVolume != 0.0M ? 100 - totalPercentages : percentage,
                });
                totalPercentages += percentage;
            }

            return consolidatedOwners;
        }

        /// <summary>
        /// Validates the ownership percentage.
        /// </summary>
        /// <param name="groupedConsolidationMovementData">The grouped consolidation movement data.</param>
        /// <returns>The boolean.</returns>
        private bool ValidateOwnershipPercentage(IEnumerable<ConsolidationMovementData> groupedConsolidationMovementData)
        {
            if (groupedConsolidationMovementData.Any(a => string.Equals(a.OwnershipValueUnit, "%", StringComparison.OrdinalIgnoreCase)))
            {
                var percentageSum = Math.Round(groupedConsolidationMovementData.Sum(a => a.OwnershipVolume), 2);
                if (!groupedConsolidationMovementData.All(a => string.Equals(a.OwnershipValueUnit, "%", StringComparison.OrdinalIgnoreCase)) || percentageSum != 100.00M)
                {
                    var errorMessage = $"The sum of ownership percentage is not 100.00% for movementTransactionId: {groupedConsolidationMovementData.First().MovementTransactionId}";
                    this.Logger.LogInformation(errorMessage, $"{groupedConsolidationMovementData.First().MovementTransactionId}");
                    throw new InvalidOperationException(errorMessage);
                }

                return false;
            }

            return true;
        }
    }
}
