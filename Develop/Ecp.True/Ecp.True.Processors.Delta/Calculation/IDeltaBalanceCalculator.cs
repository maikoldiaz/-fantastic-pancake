// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDeltaBalanceCalculator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Calculation
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The calculator interface.
    /// </summary>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    public interface IDeltaBalanceCalculator
    {
        /// <summary>
        /// Calculates the asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="movementMap">The movement map.</param>
        /// <param name="consolidatedMovementMap">The consolidated movement map.</param>
        /// <param name="consolidatedInventoryMap">The consolidated inventory map.</param>
        /// <returns>The delta balance collection.</returns>
        IEnumerable<DeltaBalance> Calculate(
            Ticket ticket,
            ConcurrentDictionary<string, List<Movement>> movementMap,
            ConcurrentDictionary<string, List<ConsolidatedMovement>> consolidatedMovementMap,
            ConcurrentDictionary<string, List<ConsolidatedInventoryProduct>> consolidatedInventoryMap);
    }
}
