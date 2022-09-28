// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMovementRepository.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The movement custom repository.
    /// </summary>
    public interface IMovementRepository : IRepository<Movement>
    {
        /// <summary>
        /// Determines whether [has movement exists for connection] [the specified source node identifier].
        /// </summary>
        /// <param name="sourceNodeId">The source node identifier.</param>
        /// <param name="destinationNodeId">The destination node identifier.</param>
        /// <returns>
        ///   <c>true</c> if [has movement exists for connection] [the specified source node identifier]; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> HasActiveMovementForConnectionAsync(int sourceNodeId, int destinationNodeId);

        /// <summary>
        /// Gets the latest net standard volume by movement identifier.
        /// </summary>
        /// <param name="movementId">The movement identifier.</param>
        /// <returns>The net standard volume.</returns>
        Task<Movement> GetLatestMovementAsync(string movementId);

        /// <summary>
        /// Gets the latest net standard volume by movement identifier.
        /// </summary>
        /// <param name="nodeIds">The movement identifier.</param>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The net standard volume.</returns>
        Task<IEnumerable<Movement>> GetMovementsForOfficialDeltaCalculationAsync(IEnumerable<int> nodeIds, Ticket ticket);

        /// <summary>
        /// Gets the latest net standard volume by movement identifier.
        /// </summary>
        /// <param name="movementId">The movement identifier.</param>
        /// <returns>
        /// The net standard volume.
        /// </returns>
        Task<Movement> GetLatestBlockchainMovementAsync(string movementId);
    }
}