// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipProcessorBase.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership
{
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Core;

    /// <summary>
    /// The segment system calculation service.
    /// </summary>
    public class OwnershipProcessorBase : ProcessorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipProcessorBase"/> class.
        /// </summary>
        /// <param name="repositoryFactory">The repository factory.</param>
        protected OwnershipProcessorBase(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipProcessorBase"/> class.
        /// </summary>
        /// <param name="repositoryFactory">The repository factory.</param>
        protected OwnershipProcessorBase()
        {
        }

        /// <summary>
        /// Gets the owners.
        /// </summary>
        /// <param name="movementsByDate">The movements by date.</param>
        /// <param name="inventoriesByPreviousAndCurrentDate">The inventories by previous and current date.</param>
        /// <param name="productId">The product.</param>
        /// <param name="measurementUnit">The Measurement Unit.</param>
        /// <param name="nodeIds">The nodes.</param>
        /// <returns>
        /// The collection of interface.
        /// </returns>
        protected static IEnumerable<int> GetOwners(
            IEnumerable<Movement> movementsByDate,
            IEnumerable<InventoryProduct> inventoriesByPreviousAndCurrentDate,
            string productId,
            int measurementUnit,
            IEnumerable<int> nodeIds)
        {
            var sourceNodeMovements = movementsByDate.Where(x => nodeIds.Any(y => y == x.MovementSource?.SourceNodeId));
            var destinationNodeMovements = movementsByDate.Where(x => nodeIds.Any(y => y == x.MovementDestination?.DestinationNodeId));
            var nodeInventories = inventoriesByPreviousAndCurrentDate.Where(x => nodeIds.Any(y => y == x.NodeId));

            var distinctSourceMovementOwners = sourceNodeMovements.Where(x => x.MovementSource?.SourceProductId == productId && x.MeasurementUnit == measurementUnit).SelectMany(a => a.Ownerships
            .Select(o => o.OwnerId).Distinct()).Distinct();

            var distinctDestinationMovementOwners = destinationNodeMovements.Where(x => x.MovementDestination?.DestinationProductId == productId
                                                                                            && x.MeasurementUnit == measurementUnit).SelectMany(a => a.Ownerships
                                                                                            .Select(o => o.OwnerId).Distinct()).Distinct();

            var distinctInventoryOwners = nodeInventories.Where(x => x.ProductId == productId && x.MeasurementUnit == measurementUnit).SelectMany(a => a.Ownerships
            .Select(o => o.OwnerId).Distinct()).Distinct();

            return distinctSourceMovementOwners.Union(distinctDestinationMovementOwners).Union(distinctInventoryOwners).Where(x => x != 0);
        }
    }
}
