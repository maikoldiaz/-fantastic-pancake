// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INodeOwnershipRepository.cs" company="Microsoft">
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
    using Ecp.True.Entities.Admin;

    /// <summary>
    /// The node ownership custom repository.
    /// </summary>
    public interface INodeOwnershipRepository
    {
        /// <summary>
        /// Gets the owners for movement.
        /// </summary>
        /// <param name="sourceNodeId">The source node identifier.</param>
        /// <param name="destinationNodeId">The destination node identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The list of owners.</returns>
        Task<IEnumerable<NodeConnectionProductOwner>> GetOwnersForMovementAsync(int sourceNodeId, int destinationNodeId, string productId);

        /// <summary>
        /// Gets the owners for movement.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The list of owners.</returns>
        Task<IEnumerable<StorageLocationProductOwner>> GetOwnersForInventoryAsync(int nodeId, string productId);
    }
}
