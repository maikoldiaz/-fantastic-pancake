// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReconciler.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Deadletter.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Core.Dto;
    using Ecp.True.Entities.Dto;

    /// <summary>
    /// The reconciler.
    /// </summary>
    public interface IReconciler
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        ServiceType Type { get; }

        /// <summary>
        /// Gets the deadlettered message asynchronous.
        /// </summary>
        /// <returns>
        /// The message.
        /// </returns>
        Task ReconcileAsync();

        /// <summary>
        /// Reconciles the asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The task.</returns>
        Task ReconcileAsync(OffchainMessage message);

        /// <summary>
        /// Gets Reconciles the records.
        /// </summary>
        /// <param name="isCritical">The critical flag.</param>
        /// <param name="takeRecords">The records number to get.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task<IEnumerable<FailedRecord>> GetFailuresAsync(bool isCritical, int? takeRecords);

        /// <summary>
        /// Resets the critical records.
        /// </summary>
        /// <param name="info">The reconciliation Info.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task ResetAsync(BlockchainFailures info);

        /// <summary>
        /// Reconciles the failure asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        Task ReconcileFailureAsync();
    }
}
