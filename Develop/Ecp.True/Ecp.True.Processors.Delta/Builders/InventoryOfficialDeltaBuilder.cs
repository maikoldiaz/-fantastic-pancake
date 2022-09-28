// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryOfficialDeltaBuilder.cs" company="Microsoft">
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
    /// The error movement builder.
    /// </summary>
    public class InventoryOfficialDeltaBuilder : OfficialDeltaBuilderBase<OfficialResultInventory>, IOfficialDeltaBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryOfficialDeltaBuilder"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public InventoryOfficialDeltaBuilder(ITrueLogger<InventoryOfficialDeltaBuilder> logger)
                 : base(logger)
        {
        }

        /// <inheritdoc/>
        public override Task<IEnumerable<DeltaNodeError>> BuildErrorsAsync(OfficialDeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));

            this.Logger.LogInformation($"{nameof(InventoryOfficialDeltaBuilder)}: Building DeltaNodeErrors for ticket {deltaData.Ticket.TicketId}", deltaData.Ticket.TicketId);

            var deltaErrors = new List<DeltaNodeError>();
            var officialInventoryErrors = deltaData.InventoryErrors.Where(x => x.Origin == True.Entities.Enumeration.OriginType.OFICIAL);

            foreach (var deltaInventoryError in officialInventoryErrors)
            {
                var inventoryNodeIds = GetInventoryNodeIds(deltaInventoryError, deltaData);
                foreach (var nodeId in inventoryNodeIds)
                {
                    var inventoryError = new DeltaNodeError
                    {
                        ErrorMessage = deltaInventoryError.Description,
                        DeltaNodeId = deltaData.DeltaNodes.FirstOrDefault(x => x.NodeId == nodeId).DeltaNodeId,
                    };

                    inventoryError.InventoryProductId = deltaInventoryError.InventoryProductId;
                    deltaErrors.Add(inventoryError);
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

        private static IEnumerable<int> GetInventoryNodeIds(OfficialErrorInventory officialErrorInventory, OfficialDeltaData deltaData)
        {
            return deltaData.PendingOfficialInventories.Where(x => x.InventoryProductID == officialErrorInventory.InventoryProductId).Select(y => y.NodeId).Distinct();
        }

        private async Task<IEnumerable<Movement>> DoBuildAsync(OfficialDeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));
            var movements = new List<Movement>();
            var tasks = new List<Task<Movement>>();
            var officialResultInventories = deltaData.OfficialResultInventories.Where(x => x.Origin == True.Entities.Enumeration.OriginType.OFICIAL);

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
            var originalInventory = deltaData.PendingOfficialInventories.FirstOrDefault(x => x.InventoryProductID == officialResultInventory.TransactionId);
            return this.BuildMovementAsync(officialResultInventory, originalInventory, deltaData, isPositive);
        }

        private Task<Movement> BuildMovementAsync(
            OfficialResultInventory officialResultInventory,
            PendingOfficialInventory officialInventory,
            OfficialDeltaData deltaData,
            bool isPositive)
        {
            var movement = this.CreateMovement(deltaData.Ticket.TicketId);
            movement.NetStandardVolume = officialResultInventory.OfficialDelta;
            movement.SourceInventoryProductId = officialResultInventory.TransactionId;
            movement.MeasurementUnit = officialInventory.MeasurementUnit;
            movement.SegmentId = officialInventory.SegmentId;
            movement.OfficialDeltaMessageTypeId = OfficialDeltaMessageType.OfficialInventoryDelta;
            movement.OperationalDate = officialInventory.InventoryDate.Date == deltaData.Ticket.StartDate.Date ?
                officialInventory.InventoryDate.AddDays(-1).Date : officialInventory.InventoryDate.Date;
            movement.Period = new MovementPeriod { StartTime = movement.OperationalDate, EndTime = movement.OperationalDate };
            movement.MovementTypeId = 187;

            if (!isPositive)
            {
                movement.MovementSource = new MovementSource
                {
                    SourceNodeId = officialInventory.NodeId,
                    SourceProductId = officialInventory.ProductID,
                };
            }
            else
            {
                movement.MovementDestination = new MovementDestination
                {
                    DestinationNodeId = officialInventory.NodeId,
                    DestinationProductId = officialInventory.ProductID,
                };
            }

            movement.Owners.AddRange(this.CalculateOwners(officialResultInventory, false));
            return Task.FromResult(movement);
        }
    }
}
