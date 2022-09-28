// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaInventoryResultMovementBuilder.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.Interfaces;

    /// <summary>
    /// The official Delta inventory FICO response to create movements.
    /// </summary>
    public class OfficialDeltaInventoryResultMovementBuilder : OfficialDeltaBuilderBase<OfficialResultInventory>, IOfficialDeltaBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OfficialDeltaInventoryResultMovementBuilder" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public OfficialDeltaInventoryResultMovementBuilder(ITrueLogger<OfficialDeltaInventoryResultMovementBuilder> logger)
                        : base(logger)
        {
        }

        /// <inheritdoc/>
        public override Task<IEnumerable<DeltaNodeError>> BuildErrorsAsync(OfficialDeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));
            var deltaErrors = new List<DeltaNodeError>();
            var officialMovementErrors = deltaData.InventoryErrors.Where(x => x.Origin == True.Entities.Enumeration.OriginType.DELTAOFICIAL);

            foreach (var deltaMovementError in officialMovementErrors)
            {
                var movementNodeIds = GetMovementNodeIds(deltaMovementError, deltaData);
                foreach (var nodeId in movementNodeIds)
                {
                    var movementError = new DeltaNodeError
                    {
                        ErrorMessage = deltaMovementError.Description,
                        DeltaNodeId = deltaData.DeltaNodes.FirstOrDefault(x => x.NodeId == nodeId).DeltaNodeId,
                    };

                    movementError.MovementTransactionId = deltaMovementError.InventoryProductId;
                    deltaErrors.Add(movementError);
                }
            }

            return Task.FromResult(deltaErrors as IEnumerable<DeltaNodeError>);
        }

        /// <inheritdoc/>
        public override async Task<IEnumerable<Movement>> BuildMovementsAsync(OfficialDeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));

            this.Logger.LogInformation($"{nameof(OfficialDeltaInventoryResultMovementBuilder)}: Building DeltaNodeErrors for ticket {deltaData.Ticket.TicketId}", deltaData.Ticket.TicketId);
            return await this.DoBuildAsync(deltaData).ConfigureAwait(false);
        }

        private static IEnumerable<int> GetMovementNodeIds(OfficialErrorInventory officialErrorMovement, OfficialDeltaData deltaData)
        {
            var nodeIds = new List<int>();
            var officialMovement = deltaData.OfficialDeltaInventories.FirstOrDefault(x => x.MovementTransactionId == officialErrorMovement.InventoryProductId);
            if (officialMovement != null && officialMovement.SourceNodeId.HasValue)
            {
                nodeIds.Add(officialMovement.SourceNodeId.Value);
            }

            if (officialMovement != null && officialMovement.DestinationNodeId.HasValue)
            {
                nodeIds.Add(officialMovement.DestinationNodeId.Value);
            }

            return nodeIds.Where(x => deltaData.DeltaNodes.Any(y => y.NodeId == x)).Distinct();
        }

        private async Task<IEnumerable<Movement>> DoBuildAsync(OfficialDeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));
            var movements = new List<Movement>();
            var tasks = new List<Task<Movement>>();
            var officialResultInventories = deltaData.OfficialResultInventories.Where(x => x.Origin == True.Entities.Enumeration.OriginType.DELTAOFICIAL);

            officialResultInventories.ForEach(officialResultInventory =>
            {
                tasks.Add(this.GetMovementAsync(officialResultInventory, deltaData, officialResultInventory.Sign));
            });

            await Task.WhenAll(tasks).ConfigureAwait(false);
            tasks.ForEach(r => movements.Add(r.Result));

            return movements;
        }

        private Task<Movement> GetMovementAsync(
            OfficialResultInventory officialResultInventory,
            OfficialDeltaData deltaData,
            bool isPositive)
        {
            var originalInventory = deltaData.OfficialDeltaInventories.FirstOrDefault(x => x.MovementTransactionId == officialResultInventory.TransactionId);
            return this.BuildMovementAsync(officialResultInventory, originalInventory, deltaData, isPositive);
        }

        private Task<Movement> BuildMovementAsync(
            OfficialResultInventory officialResultInventory,
            OfficialDeltaInventory officialInventory,
            OfficialDeltaData deltaData,
            bool isPositive)
        {
            var movement = this.CreateMovement(deltaData.Ticket.TicketId);
            movement.NetStandardVolume = officialResultInventory.OfficialDelta;
            movement.SourceMovementTransactionId = officialResultInventory.TransactionId;
            movement.MeasurementUnit = officialInventory.MeasurementUnit;
            movement.SegmentId = officialInventory.SegmentId;
            movement.OfficialDeltaMessageTypeId = OfficialDeltaMessageType.OfficialInventoryDelta;
            movement.OperationalDate = officialInventory.OperationalDate.Date;
            movement.Period = new MovementPeriod { StartTime = movement.OperationalDate, EndTime = movement.OperationalDate };
            movement.MovementTypeId = Constants.DeltaInventory;

            if (!isPositive)
            {
                movement.MovementSource = new MovementSource
                {
                    SourceNodeId = officialInventory.DestinationNodeId,
                    SourceProductId = officialInventory.DestinationProductId,
                };
            }
            else
            {
                movement.MovementDestination = new MovementDestination
                {
                    DestinationNodeId = officialInventory.SourceNodeId,
                    DestinationProductId = officialInventory.SourceProductId,
                };
            }

            movement.Owners.AddRange(this.CalculateOwners(officialResultInventory, false));
            return Task.FromResult(movement);
        }
    }
}
