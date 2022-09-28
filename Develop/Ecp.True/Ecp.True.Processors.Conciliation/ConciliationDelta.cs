// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConciliationDelta.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Conciliation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Ecp.True.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// the ConciliationDelta.
    /// </summary>
    public class ConciliationDelta
    {

        /// <summary>
        /// Calculate Delta Conciliation.
        /// </summary>
        /// <param name="segmentMovements">segmentMovements.</param>
        /// <param name="otherSegmentMovements">otherSegmentMovements.</param>
        /// <returns>MovementConciliations.</returns>
        public MovementConciliations CalculateDeltaConciliation(
            IEnumerable<MovementConciliationDto> segmentMovements,
            IEnumerable<MovementConciliationDto> otherSegmentMovements)
        {
            ArgumentValidators.ThrowIfNull(segmentMovements, nameof(segmentMovements));
            ArgumentValidators.ThrowIfNull(otherSegmentMovements, nameof(otherSegmentMovements));
            var conciliatedMovements = new List<MovementConciliationDto>();
            var noConciliatedMovements = new List<MovementConciliationDto>();
            var errorMovements = new List<MovementConciliationDto>();
            errorMovements.AddRange(GetErrorMovement(segmentMovements));
            errorMovements.AddRange(GetErrorMovement(otherSegmentMovements));
            var auxSegmentMovements = GetGroupMovement(segmentMovements);
            var auxOtherSegmentMovements = GetGroupMovement(otherSegmentMovements);
            foreach (var osMovement in auxOtherSegmentMovements)
            {
                var result = GetMovementsByMovement(auxSegmentMovements, osMovement).FirstOrDefault();
                if (result != null)
                {
                    decimal? sign = Math.Round((decimal)(osMovement.OwnershipVolume - result.OwnershipVolume), 2);
                    string ans = sign > 0 ? Constants.Positive : Constants.Negative;
                    osMovement.Sign = sign == 0 ? Constants.Igual : ans;
                    osMovement.DeltaConciliated = Math.Abs(sign ?? 0);
                    osMovement.IsReconciled = true;
                    conciliatedMovements.Add(osMovement);
                }
                else
                {
                    var auxNoConciliatedMovements = GetMovementsByMovement(otherSegmentMovements, osMovement);
                    auxNoConciliatedMovements.ForEach(x => { x.Description = string.Format(CultureInfo.InvariantCulture, Constants.NotFoundInformation, "movimientosOtrosSegmentos"); x.IsReconciled = false; });
                    noConciliatedMovements.AddRange(auxNoConciliatedMovements);
                    if (auxSegmentMovements.Any())
                    {
                        osMovement.Sign = Constants.Positive;
                        osMovement.DeltaConciliated = osMovement.OwnershipVolume;
                        osMovement.IsReconciled = false;
                        conciliatedMovements.Add(osMovement);
                    }
                }
            }

            foreach (var oMovement in auxSegmentMovements)
            {
                var result = GetMovementsByMovement(auxOtherSegmentMovements, oMovement).FirstOrDefault();
                if (result == null)
                {
                    var auxNoConciliatedMovements = GetMovementsByMovement(segmentMovements, oMovement);
                    auxNoConciliatedMovements.ForEach(x => { x.Description = string.Format(CultureInfo.InvariantCulture, Constants.NotFoundInformation, "movimientosSegmento"); x.IsReconciled = false; });
                    noConciliatedMovements.AddRange(auxNoConciliatedMovements);
                    if (auxOtherSegmentMovements.Any())
                    {
                        oMovement.Sign = Constants.Negative;
                        oMovement.DeltaConciliated = oMovement.OwnershipVolume;
                        oMovement.IsReconciled = false;
                        conciliatedMovements.Add(oMovement);
                    }
                }
            }

            return new MovementConciliations(conciliatedMovements, noConciliatedMovements, errorMovements);
        }

        /// <summary>
        /// Get error movement.
        /// </summary>
        /// <param name="movements">movements.</param>
        /// <returns>IEnumerable<MovementConciliationDto>.</returns>
        private static IEnumerable<MovementConciliationDto> GetErrorMovement(IEnumerable<MovementConciliationDto> movements)
        {
            return movements.Where(x => !IsSuccessful(x))
            .Select(s => new MovementConciliationDto
            {
                MovementTransactionId = s.MovementTransactionId,
                SourceNodeId = s.SourceNodeId,
                DestinationNodeId = s.DestinationNodeId,
                Description = Constants.InsufficientInformationForCalculation,
            }).ToList();
        }

        /// <summary>
        /// Get group movement.
        /// </summary>
        /// <param name="movements">movements.</param>
        /// <returns>IEnumerable<MovementConciliationDto>.</returns>
        private static IEnumerable<MovementConciliationDto> GetGroupMovement(IEnumerable<MovementConciliationDto> movements)
        {
            return movements.Where(w => IsSuccessful(w))
            .GroupBy(g => new { g.SourceNodeId, g.DestinationNodeId, g.SourceProductId, g.DestinationProductId, g.OwnerId, g.MeasurementUnit })
            .Select(s => new MovementConciliationDto
            {
                SourceNodeId = s.First().SourceNodeId,
                DestinationNodeId = s.First().DestinationNodeId,
                SourceProductId = s.First().SourceProductId,
                DestinationProductId = s.First().DestinationProductId,
                OwnerId = s.First().OwnerId,
                MeasurementUnit = s.First().MeasurementUnit,
                OwnershipVolume = s.Sum(x => x.OwnershipVolume),
                OperationalDate = s.First().OperationalDate,
                UncertaintyPercentage = s.First().UncertaintyPercentage,
                IsReconciled = s.First().IsReconciled,
                SegmentId = s.First().SegmentId,
            }).ToList();
        }

        /// <summary>
        /// Get movements by movement.
        /// </summary>
        /// <param name="movements">movements.</param>
        /// <returns>IEnumerable<MovementConciliationDto>.</returns>
        private static IEnumerable<MovementConciliationDto> GetMovementsByMovement(IEnumerable<MovementConciliationDto> movements, MovementConciliationDto movement)
        {
            return movements.Where(w =>
            (w.SourceNodeId == movement.SourceNodeId &&
            w.DestinationNodeId == movement.DestinationNodeId &&
            w.SourceProductId == movement.SourceProductId &&
            w.DestinationProductId == movement.DestinationProductId &&
            w.OwnerId == movement.OwnerId &&
            w.MeasurementUnit == movement.MeasurementUnit));
        }

        /// <summary>
        /// Validate If Is Successful Conciliation Movement.
        /// </summary>
        /// <param name="obj">object.</param>
        /// <returns>bool.</returns>
        private static bool IsSuccessful(object obj)
        {
            string[] excludeNames = { "Sign", "DeltaConciliated", "Description", "UncertaintyPercentage", "IsReconciled" };
            string[] propertyNames = obj.GetType().GetProperties().Select(p => p.Name).ToArray();
            foreach (var prop in propertyNames)
            {
                object propValue = obj.GetType().GetProperty(prop).GetValue(obj, null);
                if (!excludeNames.Contains(prop) && string.IsNullOrEmpty(propValue?.ToString()))
                {
                    return false;
                }
            }

            return true;
        }

    }
}
