// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICalculateOwnership.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The ICalculateOwnership.
    /// </summary>
    public interface ICalculateOwnership
    {
        /// <summary>
        /// Calculates the and register.
        /// </summary>
        /// <param name="nodeId">The node.</param>
        /// <param name="productId">The product.</param>
        /// <param name="measurementUnit">The measurement Unit.</param>
        /// <param name="date">The date.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="inputMovements">The input movements.</param>
        /// <param name="outputMovements">The output movements.</param>
        /// <param name="initialInventories">The initial inventories.</param>
        /// <param name="finalInventories">The final inventories.</param>
        /// <param name="movements">The movements.</param>
        /// <returns>The OwnershipCalculation.</returns>
        OwnershipCalculation Calculate(
              int nodeId,
              string productId,
              int measurementUnit,
              DateTime date,
              int ownerId,
              int ticketId,
              IEnumerable<Movement> inputMovements,
              IEnumerable<Movement> outputMovements,
              IEnumerable<InventoryProduct> initialInventories,
              IEnumerable<InventoryProduct> finalInventories,
              IEnumerable<Movement> movements);

        /// <summary>
        /// Calculates the percentage and register asynchronous.
        /// </summary>
        /// <param name="resultOwnershipCalculation">The result ownership calculation.</param>
        /// <returns>The ownership calculations.</returns>
        IEnumerable<OwnershipCalculation> CalculatePercentageAndRegister(IEnumerable<OwnershipCalculation> resultOwnershipCalculation);

        /// <summary>
        /// Calculates the and register.
        /// </summary>
        /// <param name="productId">The product.</param>
        /// <param name="measurementUnit">The measurement Unit.</param>
        /// <param name="date">The date.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="segmentId">The segment identifier.</param>
        /// <param name="inputMovements">The input movements.</param>
        /// <param name="outputMovements">The output movements.</param>
        /// <param name="initialInventories">The initial inventories.</param>
        /// <param name="finalInventories">The final inventories.</param>
        /// <param name="movements">The movements.</param>
        /// <param name="nodeIds">The node identifiers.</param>
        /// <returns>The SegmentOwnershipCalculation.</returns>
        SegmentOwnershipCalculation CalculateAndRegisterForSegment(
          string productId,
          int measurementUnit,
          DateTime date,
          int ownerId,
          int ticketId,
          int segmentId,
          IEnumerable<Movement> inputMovements,
          IEnumerable<Movement> outputMovements,
          IEnumerable<InventoryProduct> initialInventories,
          IEnumerable<InventoryProduct> finalInventories,
          IEnumerable<Movement> movements,
          IEnumerable<int> nodeIds);

        /// <summary>
        /// Calculates the percentage and register for segment.
        /// </summary>
        /// <param name="resultOwnershipCalculation">The result ownership calculation.</param>
        /// <returns>The segment ownership calculations.</returns>
        IEnumerable<SegmentOwnershipCalculation> CalculatePercentageAndRegisterForSegment(IEnumerable<SegmentOwnershipCalculation> resultOwnershipCalculation);

        /// <summary>
        /// Calculates the and register.
        /// </summary>
        /// <param name="productId">The product.</param>
        /// <param name="measurementUnit">The measurement Unit.</param>
        /// <param name="date">The date.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="segmentId">The segment identifier.</param>
        /// <param name="systemId">The system identifier.</param>
        /// <param name="inputMovements">The input movements.</param>
        /// <param name="outputMovements">The output movements.</param>
        /// <param name="initialInventories">The initial inventories.</param>
        /// <param name="finalInventories">The final inventories.</param>
        /// <param name="movements">The movements.</param>
        /// <param name="nodeIds">The node identifiers.</param>
        /// <returns>
        /// The SystemOwnershipCalculation.
        /// </returns>
        SystemOwnershipCalculation CalculateAndRegisterForSystem(
              string productId,
              int measurementUnit,
              DateTime date,
              int ownerId,
              int ticketId,
              int segmentId,
              int systemId,
              IEnumerable<Movement> inputMovements,
              IEnumerable<Movement> outputMovements,
              IEnumerable<InventoryProduct> initialInventories,
              IEnumerable<InventoryProduct> finalInventories,
              IEnumerable<Movement> movements,
              IEnumerable<int> nodeIds);

        /// <summary>
        /// Calculates the percentage and register.
        /// </summary>
        /// <param name="resultOwnershipCalculation">The result ownership calculation.</param>
        /// <returns>The system ownership calculations.</returns>
        IEnumerable<SystemOwnershipCalculation> CalculatePercentageAndRegisterForSystem(IEnumerable<SystemOwnershipCalculation> resultOwnershipCalculation);
    }
}
