// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDeltaProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Approval.Interfaces
{
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;

    /// <summary>
    /// The Delta processor.
    /// </summary>
    public interface IDeltaProcessor
    {
        /// <summary>
        /// Get Delta By Delta NodeID.
        /// </summary>
        /// <param name="deltaNodeId">Delta NodeId.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        Task<DeltaNodeDetails> GetDeltaByDeltaNodeIdAsync(int deltaNodeId);

        /// <summary>
        /// Updates Delta Approval state asynchronous.
        /// </summary>
        /// <param name="approvalRequest">The approval request.</param>
        /// <returns>The Task.</returns>
        Task UpdateDeltaApprovalStateAsync(DeltaNodeApprovalRequest approvalRequest);

        /// <summary>
        /// Generate Delta Movements.
        /// </summary>
        /// <param name="deltaNodeId">The ownership node id.</param>
        /// <returns>The task.</returns>
        Task GenerateDeltaMovementsAsync(int deltaNodeId);
    }
}
