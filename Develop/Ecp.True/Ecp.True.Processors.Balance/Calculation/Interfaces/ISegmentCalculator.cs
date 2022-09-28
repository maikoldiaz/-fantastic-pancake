// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISegmentCalculator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Balance.Calculation.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The segment calculator.
    /// </summary>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    public interface ISegmentCalculator
    {
        /// <summary>
        /// Calculates the and register asynchronous.
        /// </summary>
        /// <param name="productId">The product.</param>
        /// <param name="date">The date.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="segmentId">The segment identifier.</param>
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
        SegmentUnbalance CalculateAndGetSegmentUnbalance(
              string productId,
              DateTime date,
              int ticketId,
              int segmentId,
              IEnumerable<Movement> inputMovements,
              IEnumerable<Movement> outputMovements,
              IEnumerable<InventoryProduct> initialInventories,
              IEnumerable<InventoryProduct> finalInventories,
              IEnumerable<Movement> sourceNodeMovements,
              IEnumerable<Movement> destinationNodeMovements,
              IUnitOfWork unitOfWork);
    }
}
