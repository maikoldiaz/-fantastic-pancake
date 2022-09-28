// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryDeltaStrategy.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// ---------
namespace Ecp.True.Processors.Delta.Delta
{
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Delta.Entities;

    /// <summary>
    /// The inventory delta strategy.
    /// </summary>
    public class InventoryDeltaStrategy : DeltaStrategyBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryDeltaStrategy" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public InventoryDeltaStrategy(
           ITrueLogger logger)
           : base(logger)
        {
        }

        /// <summary>
        /// build inventory object.
        /// </summary>
        /// <param name="deltaData">The deltaData.</param>
        /// <returns>inventory.</returns>
        public override IEnumerable<Movement> Build(DeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));
            return this.CreateInventoryMovement(deltaData);
        }

        private IEnumerable<Movement> CreateInventoryMovement(DeltaData deltaData)
        {
            var movements = new List<Movement>();
            foreach (var inventoryResult in deltaData.ResultInventories)
            {
                var updatedInventory = deltaData.InventoryProducts.First(x => x.InventoryProductId == inventoryResult.InventoryProductId);
                var originalInventoryRecord = deltaData.OriginalInventories.SingleOrDefault(
                    x => x.InventoryProductUniqueId.Equals(inventoryResult.InventoryProductUniqueId, System.StringComparison.Ordinal));
                InventoryProduct originalInventoryProduct = null;
                if (originalInventoryRecord != null)
                {
                    originalInventoryProduct = deltaData.InventoryProducts.First(x => x.InventoryProductId == originalInventoryRecord.InventoryProductId);
                }

                var movement = this.CreateMovement(deltaData, inventoryResult.Delta);
                movement.MovementTypeId = 44;
                movement.MeasurementUnit = updatedInventory.MeasurementUnit;
                movement.SourceInventoryProductId = updatedInventory.InventoryProductId;

                movement.MovementDestination = inventoryResult.Sign ? new MovementDestination
                {
                    DestinationNodeId = updatedInventory.NodeId,
                    DestinationProductId = updatedInventory.ProductId,
                }
                : null;

                movement.MovementSource = !inventoryResult.Sign ? new MovementSource
                {
                    SourceNodeId = updatedInventory.NodeId,
                    SourceProductId = updatedInventory.ProductId,
                }
                : null;

                if (originalInventoryProduct != null)
                {
                    movement.Owners.AddRange(this.CalculateOwners(originalInventoryProduct.Ownerships, inventoryResult.Delta));
                }

                movements.Add(movement);
            }

            return deltaData.GeneratedMovements = movements;
        }
    }
}
