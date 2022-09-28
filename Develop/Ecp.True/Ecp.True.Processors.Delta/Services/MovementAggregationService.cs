// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementAggregationService.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Proxies.OwnershipRules;
    using Ecp.True.Proxies.OwnershipRules.OfficialDeltaDataRequest;
    using static System.Globalization.CultureInfo;
    using static Ecp.True.Entities.Enumeration.MovementType;

    /// <summary>
    /// The movement aggregation strategy.
    /// </summary>
    public class MovementAggregationService : IMovementAggregationService
    {
        /// <inheritdoc />
        public IEnumerable<OfficialDeltaConsolidatedMovement> AggregateTolerancesAndUnidentifiedLosses(
            IEnumerable<OfficialDeltaConsolidatedMovement> consolidatedMovements, OfficialDeltaData officialDeltaData)
        {
            ArgumentValidators.ThrowIfNull(consolidatedMovements, nameof(consolidatedMovements));
            var consolidatedMovementsList = consolidatedMovements.ToList();
            ArgumentValidators.ThrowIfNull(officialDeltaData, nameof(officialDeltaData));
            if (!consolidatedMovementsList.Any())
            {
                return consolidatedMovementsList;
            }

            var groupsToAggregate = GetGroupsToAggregate(consolidatedMovementsList, officialDeltaData);

            var movementsToAggregate = groupsToAggregate.SelectMany(g => g);

            var aggregatedMovements = consolidatedMovementsList
                .Except(movementsToAggregate)
                .ToList();

            groupsToAggregate.ForEach(g => AddAggregatedUnidentifiedLoss(aggregatedMovements, g));

            return aggregatedMovements;
        }

        private static void AddAggregatedUnidentifiedLoss(List<OfficialDeltaConsolidatedMovement> aggregatedMovementList, IGrouping<object, OfficialDeltaConsolidatedMovement> g)
        {
            aggregatedMovementList.Add(GetAggregatedUnidentifiedLoss(g));
        }

        private static OfficialDeltaConsolidatedMovement GetAggregatedUnidentifiedLoss(IGrouping<object, OfficialDeltaConsolidatedMovement> groupedMovement)
        {
            var aggregatedUnidentifiedLoss = GetPrototypeConsolidatedMovement(groupedMovement);

            aggregatedUnidentifiedLoss.OwnershipVolume = groupedMovement.Sum(o => o.OwnershipVolume);

            return aggregatedUnidentifiedLoss;
        }

        private static OfficialDeltaConsolidatedMovement GetPrototypeConsolidatedMovement(IGrouping<object, OfficialDeltaConsolidatedMovement> groupedMovement)
        {
            var unidentifiedLoss = new OfficialDeltaConsolidatedMovement();
            unidentifiedLoss.CopyFrom(groupedMovement.FirstOrDefault(g => MovementIsOfType(g, UnidentifiedLoss)));
            unidentifiedLoss.OwnershipVolume = 0.0M;
            unidentifiedLoss.MovementTypeId = ((int)UnidentifiedLoss).ToString(InvariantCulture);
            return unidentifiedLoss;
        }

        private static bool MovementIsOfType(OfficialDeltaConsolidatedMovement g, MovementType type)
        {
            return g.MovementTypeId == ((int)type).ToString(InvariantCulture);
        }

        private static List<IGrouping<object, OfficialDeltaConsolidatedMovement>> GetGroupsToAggregate(
            IEnumerable<OfficialDeltaConsolidatedMovement> movementConsolidationData,
            OfficialDeltaData officialDeltaData)
        {
            var withToleranceInOfficialBalance = officialDeltaData
                .PendingOfficialMovements
                .Where(m =>
                    m.IsOfType(Tolerance))
                .GroupBy(m => new
                {
                    SourceNodeId = m.SourceNodeId?.ToString(InvariantCulture),
                    DestinationNodeId = m.DestinationNodeId?.ToString(InvariantCulture),
                    m.SourceProductId,
                    m.DestinationProductId,
                    OwnerId = m.OwnerId.ToString(InvariantCulture),
                });

            var groupedMovements = movementConsolidationData
                .Where(m =>
                    m.IsOfType(Tolerance) ||
                    m.IsOfType(UnidentifiedLoss))
                .GroupBy(m => new
                {
                    m.SourceNodeId,
                    m.DestinationNodeId,
                    m.SourceProductId,
                    m.DestinationProductId,
                    m.OwnerId,
                })
                .Where(g => g.Any(m => MovementIsOfType(m, UnidentifiedLoss)));

            var movementWithoutOfficialTolerance = groupedMovements
                .Where(g =>
                {
                    var withoutTolerance = withToleranceInOfficialBalance
                        .Any(t =>
                            t.Key.SourceNodeId == g.Key.SourceNodeId
                            && t.Key.DestinationNodeId == g.Key.DestinationNodeId
                            && t.Key.SourceProductId == g.Key.SourceProductId
                            && t.Key.DestinationProductId == g.Key.DestinationProductId
                            && t.Key.OwnerId == g.Key.OwnerId);

                    return !withoutTolerance;
                });

            return new List<IGrouping<object, OfficialDeltaConsolidatedMovement>>(movementWithoutOfficialTolerance);
        }
    }
}