// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidatedMovementDeltaBuilder.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.Interfaces;

    /// <summary>
    /// The error movement builder.
    /// </summary>
    public class ConsolidatedMovementDeltaBuilder : OfficialDeltaBuilderBase<OfficialResultMovement>, IOfficialDeltaBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsolidatedMovementDeltaBuilder"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ConsolidatedMovementDeltaBuilder(ITrueLogger<ConsolidatedMovementDeltaBuilder> logger)
                 : base(logger)
        {
        }

        /// <inheritdoc/>
        public override Task<IEnumerable<DeltaNodeError>> BuildErrorsAsync(OfficialDeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));

            this.Logger.LogInformation($"{nameof(ConsolidatedMovementDeltaBuilder)}: Building DeltaNodeErrors for ticket {deltaData.Ticket.TicketId}", deltaData.Ticket.TicketId);

            var deltaErrors = new List<DeltaNodeError>();
            var consolidatedMovementErrors = deltaData.MovementErrors.Where(x => x.Origin == True.Entities.Enumeration.OriginType.CONSOLIDADO);

            foreach (var deltaMovementError in consolidatedMovementErrors)
            {
                var movementNodeIds = GetMovementNodeIds(deltaMovementError, deltaData);
                foreach (var nodeId in movementNodeIds)
                {
                    var movementError = new DeltaNodeError
                    {
                        ErrorMessage = deltaMovementError.Description,
                        DeltaNodeId = deltaData.DeltaNodes.FirstOrDefault(x => x.NodeId == nodeId).DeltaNodeId,
                    };
                    movementError.ConsolidatedMovementId = deltaMovementError.MovementTransactionId;
                    deltaErrors.Add(movementError);
                }
            }

            return Task.FromResult(deltaErrors as IEnumerable<DeltaNodeError>);
        }

        /// <inheritdoc/>
        public override async Task<IEnumerable<Movement>> BuildMovementsAsync(OfficialDeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));

            this.Logger.LogInformation($"{nameof(ConsolidatedMovementDeltaBuilder)}: Building Movements for ticket {deltaData.Ticket.TicketId}", deltaData.Ticket.TicketId);
            return await this.DoBuildAsync(deltaData).ConfigureAwait(false);
        }

        private static IEnumerable<int> GetMovementNodeIds(OfficialErrorMovement officialErrorMovement, OfficialDeltaData deltaData)
        {
            var nodeIds = new List<int>();

            var consolidationMovement = deltaData.ConsolidationMovements.FirstOrDefault(x => x.ConsolidatedMovementId == officialErrorMovement.MovementTransactionId);

            if (consolidationMovement.SourceNodeId.HasValue)
            {
                nodeIds.Add(consolidationMovement.SourceNodeId.Value);
            }

            if (consolidationMovement.DestinationNodeId.HasValue)
            {
                nodeIds.Add(consolidationMovement.DestinationNodeId.Value);
            }

            return nodeIds.Where(x => deltaData.DeltaNodes.Any(y => y.NodeId == x)).Distinct();
        }

        private async Task<IEnumerable<Movement>> DoBuildAsync(OfficialDeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));
            var movements = new List<Movement>();
            var tasks = new List<Task<Movement>>();
            var consolidatedResultMovements = deltaData.OfficialResultMovements.Where(x => x.Origin == True.Entities.Enumeration.OriginType.CONSOLIDADO);

            consolidatedResultMovements.ForEach(consolidatedResultMovement =>
            {
                tasks.Add(this.GetMovementAsync(consolidatedResultMovement, deltaData, consolidatedResultMovement.Sign));
            });

            await Task.WhenAll(tasks).ConfigureAwait(false);
            tasks.ForEach(r => movements.Add(r.Result));

            return movements;
        }

        private Task<Movement> GetMovementAsync(
            OfficialResultMovement officialResultMovement,
            OfficialDeltaData deltaData,
            bool isPositive)
        {
            var originalMovement = deltaData.ConsolidationMovements.FirstOrDefault(x => x.ConsolidatedMovementId == officialResultMovement.MovementTransactionId);
            return this.BuildMovementAsync(officialResultMovement, originalMovement, deltaData, isPositive);
        }

        /// <summary>
        /// CreateSourceDestinationMovement.
        /// </summary>
        /// <param name="officialResultMovement">The officialResultMovement.</param>
        /// <param name="consolidatedMovement">The consolidatedMovement.</param>
        /// <param name="deltaData">The deltaData.</param>
        /// <param  name="isPositive">The positive.</param>
        /// <returns>movement.</returns>
        private Task<Movement> BuildMovementAsync(
            OfficialResultMovement officialResultMovement,
            ConsolidatedMovement consolidatedMovement,
            OfficialDeltaData deltaData,
            bool isPositive)
        {
            var movement = this.CreateMovement(deltaData.Ticket.TicketId);
            var cancelationType = deltaData.CancellationTypes
                .FirstOrDefault(x => x.SourceMovementTypeId == Convert.ToInt32(consolidatedMovement.MovementTypeId, CultureInfo.InvariantCulture));
            movement.NetStandardVolume = isPositive ? officialResultMovement.OfficialDelta : -1 * officialResultMovement.OfficialDelta;
            movement.ConsolidatedMovementTransactionId = officialResultMovement.MovementTransactionId;
            movement.MeasurementUnit = consolidatedMovement.MeasurementUnit.ToNullableInt();
            movement.SegmentId = consolidatedMovement.SegmentId;
            movement.Period = new MovementPeriod { StartTime = consolidatedMovement.StartDate, EndTime = consolidatedMovement.EndDate };
            movement.OperationalDate = consolidatedMovement.StartDate.Date;
            movement.OfficialDeltaMessageTypeId = OfficialDeltaMessageType.ConsolidatedMovementDelta;
            movement.MovementTypeId = !isPositive && cancelationType != null ? cancelationType.AnnulationMovementTypeId
                : Convert.ToInt32(consolidatedMovement.MovementTypeId, CultureInfo.InvariantCulture);

            if (consolidatedMovement.SourceNodeId.HasValue)
            {
                movement.MovementSource = new MovementSource
                {
                    SourceNodeId = consolidatedMovement.SourceNodeId,
                    SourceProductId = consolidatedMovement.SourceProductId,
                };
            }

            if (consolidatedMovement.DestinationNodeId.HasValue)
            {
                movement.MovementDestination = new MovementDestination
                {
                    DestinationNodeId = consolidatedMovement.DestinationNodeId,
                    DestinationProductId = consolidatedMovement.DestinationProductId,
                };
            }

            movement.Owners.AddRange(this.CalculateOwners(officialResultMovement, true));
            return Task.FromResult(movement);
        }
    }
}
