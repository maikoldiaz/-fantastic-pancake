// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISapProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Sap.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Sap;

    /// <summary>
    /// The SAP processor.
    /// </summary>
    public interface ISapProcessor
    {
        /// <summary>
        /// Sync Async.
        /// </summary>
        /// <returns>Task.</returns>
        Task SyncAsync();

        /// <summary>
        /// Updates the transfer point asynchronous.
        /// </summary>
        /// <param name="movementTransactionId">The movement transaction identifier.</param>
        /// <param name="previousMovementTransactionId">The previous movement transaction identifier.</param>
        /// <returns>The task.</returns>
        [Obsolete("This Method is Deprecated", false)]
        Task UpdateTransferPointAsync(int movementTransactionId, int? previousMovementTransactionId);

        /// <summary>
        /// Updates the transfer point asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket identifier.</param>
        /// <returns>The bool.</returns>
        public Task ProcessLogisticMovementAsync(LogisticQueueMessage ticket);
    }
}
