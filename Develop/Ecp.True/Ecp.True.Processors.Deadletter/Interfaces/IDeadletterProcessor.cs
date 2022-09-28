// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDeadletterProcessor.cs" company="Microsoft">
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
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core.Dto;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// The dead letter processor.
    /// </summary>
    public interface IDeadletterProcessor : IProcessor
    {
        /// <summary>
        /// Gets the deadlettered message asynchronous.
        /// </summary>
        /// <param name="message">The deadlettered message.</param>
        /// <returns>
        /// The message.
        /// </returns>
        Task ProcessAsync(DeadletteredMessage message);

        /// <summary>
        /// Retriggers the deadlettered message asynchronous.
        /// </summary>
        /// <param name="messageIds">The deadlettered messages.</param>
        /// <returns>
        /// The status.
        /// </returns>
        Task<bool> RetriggerAsync(IEnumerable<int> messageIds);

        /// <summary>
        /// Get Reconciliation Records.
        /// </summary>
        /// <param name="failuresRequest">The request get.</param>
        /// <returns>
        /// The records.
        /// </returns>
        Task<BlockchainFailures> GetFailuresAsync(BlockchainFailuresRequest failuresRequest);

        /// <summary>
        /// Resets the failed records.
        /// </summary>
        /// <param name="failures">The failures.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task ResetAsync(BlockchainFailures failures);

        /// <summary>
        /// Handles the registration failure asynchronous.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <returns>The task.</returns>
        Task HandleRegistrationFailureAsync(string transactionId);

        /// <summary>
        /// Handles the registration retry failure asynchronous.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <returns>The task.</returns>
        Task HandleRegistrationRetryFailureAsync(string transactionId);

        /// <summary>
        /// Handles the sap failure asynchronous.
        /// </summary>
        /// <param name="sapRequest">The sap request.</param>
        /// <returns>The Task.</returns>
        Task HandleSapFailureAsync(SapQueueMessage sapRequest);

        /// <summary>
        /// Handles the report failure asynchronous.
        /// </summary>
        /// <param name="executionId">The execution identifier.</param>
        /// <param name="type">The type.</param>
        /// <returns>The task.</returns>
        Task HandleReportFailureAsync(int executionId, ReportType type);

        /// <summary>
        /// Handles the offchain failure.
        /// </summary>
        /// <param name="message">The message.</param>
        void HandleOffchainFailure(OffchainMessage message);
    }
}
