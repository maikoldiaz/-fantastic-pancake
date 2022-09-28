// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemCalculator.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Balance.Calculation.Interfaces;

    /// <summary>
    /// Segment Calculator.
    /// </summary>
    public class SystemCalculator : SystemSegmentCalculatorBase, ISystemCalculator
    {
        /// <summary>
        /// Calculates the and register asynchronous.
        /// </summary>
        /// <param name="productId">The product.</param>
        /// <param name="date">The date.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="segmentId">The segment identifier.</param>
        /// <param name="systemId">The system identifier.</param>
        /// <param name="inputMovements">The input movements.</param>
        /// <param name="outputMovements">The output movements.</param>
        /// <param name="initialInventories">The initial inventories.</param>
        /// <param name="finalInventories">The final inventories.</param>
        /// <param name="sourceNodeMovements">The source node movements.</param>
        /// <param name="destinationNodeMovements">The destination node movements.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public SystemUnbalance CalculateAndGetSystemUnbalance(
              string productId,
              DateTime date,
              int ticketId,
              int segmentId,
              int systemId,
              IEnumerable<Movement> inputMovements,
              IEnumerable<Movement> outputMovements,
              IEnumerable<InventoryProduct> initialInventories,
              IEnumerable<InventoryProduct> finalInventories,
              IEnumerable<Movement> sourceNodeMovements,
              IEnumerable<Movement> destinationNodeMovements,
              IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));

            var inputMovementValue = GetMovementUnbalance(inputMovements);
            var outputMovementValue = GetMovementUnbalance(outputMovements);
            var initialInventoryValue = GetInventoryUnbalance(initialInventories);
            var finalInventoryValue = GetInventoryUnbalance(finalInventories);
            var interfaceValue = GetMovementTypeUnbalance(sourceNodeMovements, destinationNodeMovements, productId, VariableType.Interface);
            var toleranceValue = GetMovementTypeUnbalance(sourceNodeMovements, destinationNodeMovements, productId, VariableType.BalanceTolerance);
            var unidentifiedValue = GetMovementTypeUnbalance(sourceNodeMovements, destinationNodeMovements, productId, VariableType.UnidentifiedLosses);
            var identifiedLossValue = GetIdentifiedLossUnbalance(sourceNodeMovements, productId);

            var systemUnbalanceCalculation = new SystemUnbalance
            {
                Date = date.Date,
                ProductId = productId,
                TicketId = ticketId,
                SegmentId = segmentId,
                SystemId = systemId,
                InputVolume = inputMovementValue,
                OutputVolume = outputMovementValue,
                InitialInventoryVolume = initialInventoryValue,
                FinalInventoryVolume = finalInventoryValue,
                InterfaceVolume = interfaceValue,
                ToleranceVolume = toleranceValue,
                UnidentifiedLossesVolume = unidentifiedValue,
                IdentifiedLossesVolume = identifiedLossValue,
                UnbalanceVolume = GetUnbalanceVolume(
                    initialInventoryValue,
                    inputMovementValue,
                    outputMovementValue,
                    finalInventoryValue,
                    identifiedLossValue,
                    interfaceValue,
                    toleranceValue,
                    unidentifiedValue),
            };

            return systemUnbalanceCalculation;
        }
    }
}
