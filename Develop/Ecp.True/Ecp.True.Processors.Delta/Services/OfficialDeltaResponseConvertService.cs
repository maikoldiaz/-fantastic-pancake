// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaResponseConvertService.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Query;
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using static System.Globalization.CultureInfo;
    using static Ecp.True.Entities.Enumeration.MovementType;

    /// <inheritdoc />
    public class OfficialDeltaResponseConvertService : IOfficialDeltaResponseConvertService
    {
        private OfficialDeltaData deltaData;

        /// <inheritdoc />
        public IEnumerable<OfficialResultMovement> ConvertOfficialDeltaResponse(IEnumerable<OfficialDeltaResultMovement> deltaResults, OfficialDeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaResults, nameof(deltaResults));
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));
            this.deltaData = deltaData;
            var convertedResults = new List<OfficialResultMovement>();

            foreach (var deltaResult in GetValidDeltaResults(deltaResults))
            {
                var unidentifiedLoss = this.GetOriginalMovementInfo(deltaResult);
                var movementIsNegative = deltaResult.Sign.EqualsIgnoreCase(Constants.Negative);
                var ownerVolume = unidentifiedLoss?.OwnershipVolume;

                var deltaIsGreaterThanUnidentifiedLoss = IsGreaterThan(deltaResult, ownerVolume);

                if (movementIsNegative && deltaIsGreaterThanUnidentifiedLoss && IsUnidentifiedLoss(unidentifiedLoss))
                {
                    var unidentifiedLossVolume = AddUnidentifiedLossDelta(convertedResults, deltaResult, ownerVolume);

                    var consolidatedTolerance = this.GetConsolidatedTolerance(unidentifiedLoss);

                    convertedResults.AddToleranceMovementConsolidate(unidentifiedLossVolume, consolidatedTolerance, deltaResult);

                    continue;
                }

                convertedResults.Add(MapNewDeltaMovement(deltaResult));
            }

            return convertedResults;
        }

        private static bool IsUnidentifiedLoss(ConsolidatedOwner unidentifiedLoss)
        {
            return unidentifiedLoss?.ConsolidatedMovement?.MovementTypeId == ((int)UnidentifiedLoss).ToString(InvariantCulture);
        }

        private static bool IsGreaterThan(OfficialDeltaResultMovement deltaResult, decimal? originalOwnerVolume)
        {
            return originalOwnerVolume != null && Math.Abs(deltaResult.DeltaOfficial) >
                   originalOwnerVolume.Value;
        }

        private static decimal? AddUnidentifiedLossDelta(List<OfficialResultMovement> officialResult, OfficialDeltaResultMovement deltaResult, decimal? ownershipVolume)
        {
            if (ownershipVolume is null || deltaResult is null)
            {
                return null;
            }

            var unidentifiedLoss = new OfficialResultMovement
            {
                MovementTransactionId = Convert.ToInt32(
                    deltaResult.MovementTransactionId,
                    InvariantCulture),
                Sign = false,
                OfficialDelta = ownershipVolume.Value - Math.Max(0.0M, ownershipVolume.Value - deltaResult.DeltaOfficial),
                Origin = deltaResult.Origin,
                NetStandardVolume = deltaResult.NetStandardVolume,
                OwnerId = deltaResult.MovementOwnerId,
            };
            officialResult.Add(unidentifiedLoss);

            return unidentifiedLoss.OfficialDelta;
        }

        private static IEnumerable<OfficialDeltaResultMovement> GetValidDeltaResults(IEnumerable<OfficialDeltaResultMovement> deltaResults)
        {
            return deltaResults.Where(a => a.DeltaOfficial != 0.0M && (a.Sign == Constants.Positive || a.Sign == Constants.Negative));
        }

        private static bool OfficialMovementCommonPropertiesAreEqual(PendingOfficialMovement officialMovement, ConsolidatedMovement c)
        {
            return officialMovement is { }
                   && c.SourceNodeId == officialMovement.SourceNodeId
                   && c.DestinationNodeId == officialMovement.DestinationNodeId
                   && c.SourceProductId == officialMovement.SourceProductId
                   && c.DestinationProductId == officialMovement.DestinationProductId
                   && c.MovementTypeId == officialMovement.MovementTypeID.ToString(InvariantCulture);
        }

        private static OfficialResultMovement MapNewDeltaMovement(OfficialDeltaResultMovement deltaResult)
        {
            return new OfficialResultMovement
            {
                MovementTransactionId = Convert.ToInt32(deltaResult.MovementTransactionId, InvariantCulture),
                Sign = deltaResult.Sign.EqualsIgnoreCase(Constants.Positive),
                OfficialDelta = deltaResult.DeltaOfficial,
                Origin = deltaResult.Origin,
                NetStandardVolume = deltaResult.NetStandardVolume,
                OwnerId = deltaResult.MovementOwnerId,
            };
        }

        private ConsolidatedOwner GetConsolidatedTolerance(ConsolidatedOwner unidentifiedLoss)
        {
            var consolidatedMovement = unidentifiedLoss.ConsolidatedMovement;
            return this.deltaData.ConsolidationMovements?
                .FirstOrDefault(
                    c => c.SourceNodeId == consolidatedMovement.SourceNodeId
                         && c.DestinationNodeId == consolidatedMovement.DestinationNodeId
                         && c.SourceProductId == consolidatedMovement.SourceProductId
                         && c.MovementTypeId == ((int)Tolerance).ToString(InvariantCulture))
                .ConsolidatedOwners.FirstOrDefault(o => o.OwnerId == unidentifiedLoss.OwnerId);
        }

        private ConsolidatedOwner GetOriginalMovementInfo(OfficialDeltaResultMovement deltaResult)
        {
            var officialOwnerVolume = this.GetOriginalOfficialMovement(deltaResult);

            var consolidatedOwner = this.GetOriginalConsolidateMovement(deltaResult, officialOwnerVolume);

            return consolidatedOwner;
        }

        private PendingOfficialMovement GetOriginalOfficialMovement(OfficialDeltaResultMovement deltaResult)
        {
            return this.deltaData.PendingOfficialMovements
                .Where(c => c.MovementTransactionId.ToString(InvariantCulture) == deltaResult?.MovementTransactionId)
                .FirstOrDefault(c => c.OwnerId.ToString(InvariantCulture) == deltaResult?.MovementOwnerId);
        }

        private ConsolidatedOwner GetOriginalConsolidateMovement(OfficialDeltaResultMovement deltaResult, PendingOfficialMovement officialMovement)
        {
            return this.deltaData.ConsolidationMovements?
                .Where(c =>
                    c.ConsolidatedMovementId.ToString(InvariantCulture) == deltaResult?.MovementTransactionId
                    || OfficialMovementCommonPropertiesAreEqual(officialMovement, c))
                .SelectMany(c => c.ConsolidatedOwners)
                .FirstOrDefault(c => c.OwnerId.ToString(InvariantCulture) == deltaResult?.MovementOwnerId);
        }
    }
}