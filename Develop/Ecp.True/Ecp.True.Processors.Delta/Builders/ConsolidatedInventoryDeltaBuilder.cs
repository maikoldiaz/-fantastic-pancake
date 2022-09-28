// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidatedInventoryDeltaBuilder.cs" company="Microsoft">
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
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.Interfaces;

    /// <summary>
    /// The error movement builder.
    /// </summary>
    public class ConsolidatedInventoryDeltaBuilder : OfficialDeltaBuilderBase<OfficialResultInventory>, IOfficialDeltaBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsolidatedInventoryDeltaBuilder"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ConsolidatedInventoryDeltaBuilder(ITrueLogger<ConsolidatedInventoryDeltaBuilder> logger)
                  : base(logger)
        {
        }

        /// <inheritdoc/>
        public override Task<IEnumerable<DeltaNodeError>> BuildErrorsAsync(OfficialDeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));

            this.Logger.LogInformation($"{nameof(ConsolidatedInventoryDeltaBuilder)}: Building DeltaNodeErrors for ticket {deltaData.Ticket.TicketId}", deltaData.Ticket.TicketId);

            var deltaErrors = new List<DeltaNodeError>();
            var consolidatedInventoryErrors = deltaData.InventoryErrors.Where(x => x.Origin == True.Entities.Enumeration.OriginType.CONSOLIDADO);

            foreach (var deltaInventoryError in consolidatedInventoryErrors)
            {
                var inventoryNodeIds = GetInventoryNodeIds(deltaInventoryError, deltaData);
                foreach (var nodeId in inventoryNodeIds)
                {
                    var inventoryError = new DeltaNodeError
                    {
                        ErrorMessage = deltaInventoryError.Description,
                        DeltaNodeId = deltaData.DeltaNodes.FirstOrDefault(x => x.NodeId == nodeId).DeltaNodeId,
                    };

                    inventoryError.ConsolidatedInventoryProductId = deltaInventoryError.InventoryProductId;
                    deltaErrors.Add(inventoryError);
                }
            }

            return Task.FromResult(deltaErrors as IEnumerable<DeltaNodeError>);
        }

        /// <inheritdoc/>
        public override async Task<IEnumerable<Movement>> BuildMovementsAsync(OfficialDeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));

            this.Logger.LogInformation($"{nameof(ConsolidatedInventoryDeltaBuilder)}: Building Movements for ticket {deltaData.Ticket.TicketId}", deltaData.Ticket.TicketId);
            return await this.DoBuildAsync(deltaData).ConfigureAwait(false);
        }

        private static IEnumerable<int> GetInventoryNodeIds(OfficialErrorInventory officialErrorInventory, OfficialDeltaData deltaData)
        {
            return deltaData.ConsolidationInventories.Where(x => x.ConsolidatedInventoryProductId == officialErrorInventory.InventoryProductId).Select(y => y.NodeId).Distinct();
        }

        private async Task<IEnumerable<Movement>> DoBuildAsync(OfficialDeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));
            var movements = new List<Movement>();
            var tasks = new List<Task<Movement>>();
            var consolidatedResultInventories = deltaData.OfficialResultInventories.Where(x => x.Origin == True.Entities.Enumeration.OriginType.CONSOLIDADO);

            consolidatedResultInventories.ForEach(consolidatedResultInventory =>
            {
                tasks.Add(this.GetMovementAsync(consolidatedResultInventory, deltaData, consolidatedResultInventory.Sign));
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
            var originalInventory = deltaData.ConsolidationInventories.FirstOrDefault(x => x.ConsolidatedInventoryProductId == officialResultInventory.TransactionId);
            return this.BuildMovementAsync(officialResultInventory, originalInventory, deltaData, isPositive);
        }

        private Task<Movement> BuildMovementAsync(
            OfficialResultInventory officialResultInventory,
            ConsolidatedInventoryProduct consolidatedInventoryProduct,
            OfficialDeltaData deltaData,
            bool isPositive)
        {
            var movement = this.CreateMovement(deltaData.Ticket.TicketId);
            movement.NetStandardVolume = officialResultInventory.OfficialDelta;
            movement.ConsolidatedInventoryProductId = officialResultInventory.TransactionId;
            movement.MeasurementUnit = consolidatedInventoryProduct.MeasurementUnit.ToNullableInt();
            movement.SegmentId = consolidatedInventoryProduct.SegmentId;
            movement.OfficialDeltaMessageTypeId = OfficialDeltaMessageType.ConsolidatedInventoryDelta;
            movement.OperationalDate = consolidatedInventoryProduct.InventoryDate.Date;
            movement.Period = new MovementPeriod { StartTime = movement.OperationalDate, EndTime = movement.OperationalDate };
            movement.MovementTypeId = 187;
            if (!isPositive)
            {
                movement.MovementSource = new MovementSource
                {
                    SourceNodeId = consolidatedInventoryProduct.NodeId,
                    SourceProductId = consolidatedInventoryProduct.ProductId,
                };
            }
            else
            {
                movement.MovementDestination = new MovementDestination
                {
                    DestinationNodeId = consolidatedInventoryProduct.NodeId,
                    DestinationProductId = consolidatedInventoryProduct.ProductId,
                };
            }

            movement.Owners.AddRange(this.CalculateOwners(officialResultInventory, false));
            return Task.FromResult(movement);
        }
    }
}
