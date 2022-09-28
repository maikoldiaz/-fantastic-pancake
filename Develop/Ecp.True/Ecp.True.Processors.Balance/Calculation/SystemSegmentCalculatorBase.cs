// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemSegmentCalculatorBase.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Balance.Calculation
{
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// System Segment Calculator base.
    /// </summary>
    public class SystemSegmentCalculatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSegmentCalculatorBase"/> class.
        /// </summary>
        protected SystemSegmentCalculatorBase()
        {
        }

        /// <summary>
        /// Gets the unbalance.
        /// </summary>
        /// <param name="initialInventory">The initial inventory.</param>
        /// <param name="inputMovement">The input movement.</param>
        /// <param name="outputMovement">The output movement.</param>
        /// <param name="finalInventory">The final inventory.</param>
        /// <param name="identifiedLoss">The identified loss.</param>
        /// <param name="interfaceValue">The interface.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <param name="unidentifiedLoss">The unidentified loss.</param>
        /// <returns>
        /// The task.
        /// </returns>
        protected static decimal GetUnbalanceVolume(
            decimal initialInventory,
            decimal inputMovement,
            decimal outputMovement,
            decimal finalInventory,
            decimal identifiedLoss,
            decimal interfaceValue,
            decimal tolerance,
            decimal unidentifiedLoss)
        {
            return initialInventory + inputMovement - outputMovement - finalInventory - identifiedLoss + interfaceValue + tolerance + unidentifiedLoss;
        }

        /// <summary>
        /// Gets the unbalance.
        /// </summary>
        /// <param name="movements">The movements.</param>
        /// <returns>
        /// The task.
        /// </returns>
        protected static decimal GetMovementUnbalance(IEnumerable<Movement> movements)
        {
            return movements.Sum(o => o.NetStandardVolume) ?? 0;
        }

        /// <summary>
        /// Gets the unbalance.
        /// </summary>
        /// <param name="inventoryProducts">The inventory products.</param>
        /// <returns>
        /// The task.
        /// </returns>
        protected static decimal GetInventoryUnbalance(IEnumerable<InventoryProduct> inventoryProducts)
        {
            return inventoryProducts.Sum(o => o.ProductVolume) ?? 0;
        }

        /// <summary>
        /// Gets the unbalance.
        /// </summary>
        /// <param name="sourceNodeMovements">The movements.</param>
        /// <param name="destinationNodeMovements">The destination node movements.</param>
        /// <param name="productId">The product.</param>
        /// <param name="variableType">The variable type.</param>
        /// <returns>
        /// The task.
        /// </returns>
        protected static decimal GetMovementTypeUnbalance(IEnumerable<Movement> sourceNodeMovements, IEnumerable<Movement> destinationNodeMovements, string productId, VariableType variableType)
        {
            // Get all the movements based upon the movement type
            sourceNodeMovements = sourceNodeMovements.Where(x => x.VariableTypeId == variableType);
            destinationNodeMovements = destinationNodeMovements.Where(x => x.VariableTypeId == variableType);

            // Get all the movements whose destination product matches
            var destinationMovementProducts = destinationNodeMovements.Where(x => x.MovementDestination?.DestinationProductId == productId);

            // Sum for all destination movements
            var destinationVolume = destinationMovementProducts.Sum(o => o.NetStandardVolume) ?? 0;

            // Get all the movements whose source product matches
            var sourceMovementProducts = sourceNodeMovements.Where(x => x.MovementSource?.SourceProductId == productId);

            // Sum for all source movements
            var sourceVolume = sourceMovementProducts.Sum(o => o.NetStandardVolume) ?? 0;

            var volume = destinationVolume - sourceVolume;
            return volume;
        }

        /// <summary>
        /// Gets the unbalance.
        /// </summary>
        /// <param name="movements">The movements.</param>
        /// <param name="productId">The product.</param>
        /// <returns>
        /// The task.
        /// </returns>
        protected static decimal GetIdentifiedLossUnbalance(IEnumerable<Movement> movements, string productId)
        {
            // Get all the loss movements
            movements = movements.Where(x => x.MessageTypeId == (int)MessageType.Loss);

            // Get all the movements whose source product matches
            var lossMovements = movements.Where(x => x.MovementSource?.SourceProductId == productId);

            // Sum for all movements
            var volume = lossMovements.Sum(o => o.NetStandardVolume) ?? 0;
            return volume;
        }
    }
}
