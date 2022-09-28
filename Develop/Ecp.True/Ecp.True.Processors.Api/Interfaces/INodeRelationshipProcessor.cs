// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INodeRelationshipProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Interfaces
{
    using System.Threading.Tasks;
    using Ecp.True.Entities.Analytics;
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// The node relationship processor interface.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.Interfaces.IProcessor" />
    public interface INodeRelationshipProcessor : IProcessor
    {
        /// <summary>
        /// Gets the node relationship asynchronous.
        /// </summary>
        /// <param name="nodeRelationshipId">The node relationship identifier.</param>
        /// <returns>Node relationship entity.</returns>
        Task<OperativeNodeRelationship> GetNodeRelationshipAsync(int nodeRelationshipId);

        /// <summary>
        /// Creates the node relationship asynchronous.
        /// </summary>
        /// <param name="nodeRelationship">The node relationship.</param>
        /// <returns>Success/Failure message.</returns>
        Task CreateNodeRelationshipAsync(OperativeNodeRelationship nodeRelationship);

        /// <summary>
        /// Updates the node connection asynchronous.
        /// </summary>
        /// <param name="nodeRelationship">The node relationship information.</param>
        /// <returns> Success/Failure message.</returns>
        Task UpdateNodeRelationshipAsync(OperativeNodeRelationship nodeRelationship);

        /// <summary>
        /// Operatives the transfer relationship exists asynchronous.
        /// </summary>
        /// <param name="nodeRelationship">The node relationship.</param>
        /// <returns>The task.</returns>
        Task<OperativeNodeRelationship> OperativeTransferRelationshipExistsAsync(OperativeNodeRelationship nodeRelationship);

        /// <summary>
        /// Creates the logistic transfer relationship asynchronous.
        /// </summary>
        /// <param name="nodeRelationshipWithOwnership">The node relationship with ownership.</param>
        /// <returns> The task.</returns>
        Task CreateLogisticTransferRelationshipAsync(OperativeNodeRelationshipWithOwnership nodeRelationshipWithOwnership);

        /// <summary>
        /// Logistics the transfer point exists async.
        /// </summary>
        /// <param name="nodeRelationshipWithOwnership">The node relationship with ownership.</param>
        /// <returns>The Task.</returns>
        Task<OperativeNodeRelationshipWithOwnership> LogisticTransferRelationshipExistsAsync(OperativeNodeRelationshipWithOwnership nodeRelationshipWithOwnership);

        /// <summary>
        /// Deletes the logistic transfer relationship asynchronous.
        /// </summary>
        /// <param name="nodeRelationship">The node relationship.</param>
        /// <exception cref="KeyNotFoundException">The exception.</exception>
        /// <returns> The task.</returns>
        Task DeleteLogisticTransferRelationshipAsync(OperativeNodeRelationshipWithOwnership nodeRelationship);
    }
}
