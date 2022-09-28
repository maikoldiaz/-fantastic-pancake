// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMovementAggregationService.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Services
{
    using System.Collections.Generic;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Proxies.OwnershipRules.OfficialDeltaDataRequest;

    /// <summary>
    /// The movement consolidation strategy interface.
    /// </summary>
    public interface IMovementAggregationService
    {
        /// <summary>
        /// Replace when movementType is Tolerance for UnidentifiedLoss.
        /// </summary>
        /// <param name="consolidatedMovements">The Movements list.</param>
        /// <param name="officialDeltaData">The official delta data.</param>
        /// <returns>The totalized consolidated movement enumerable.</returns>
        IEnumerable<OfficialDeltaConsolidatedMovement> AggregateTolerancesAndUnidentifiedLosses(
            IEnumerable<OfficialDeltaConsolidatedMovement> consolidatedMovements, OfficialDeltaData officialDeltaData);
    }
}