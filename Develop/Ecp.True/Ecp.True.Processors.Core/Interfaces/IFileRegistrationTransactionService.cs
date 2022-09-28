// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileRegistrationTransactionService.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Core.Interfaces
{
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The OData Service class.
    /// </summary>
    public interface IFileRegistrationTransactionService
    {
        /// <summary>
        /// Gets the file registration transaction asynchronous.
        /// </summary>
        /// <param name="fileRegistrationTransactionId">The file registration transaction identifier.</param>
        /// <returns>Returns file registration transaction.</returns>
        Task<FileRegistrationTransaction> GetFileRegistrationTransactionAsync(int fileRegistrationTransactionId);

        /// <summary>
        /// Gets the file registration asynchronous.
        /// </summary>
        /// <param name="uploadId">The upload identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<FileRegistration> GetFileRegistrationAsync(string uploadId);

        /// <summary>
        /// Inserts the file registration asynchronous.
        /// </summary>
        /// <param name="fileRegistration">The file registration.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task InsertFileRegistrationAsync(FileRegistration fileRegistration);

        /// <summary>
        /// Inserts the pending transactions asynchronous.
        /// </summary>
        /// <param name="pendingTransactions">The pending transactions.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task InsertPendingTransactionsAsync(ConcurrentBag<PendingTransaction> pendingTransactions);

        /// <summary>
        /// Updates the file registration asynchronous.
        /// </summary>
        /// <param name="fileRegistration">The file registration.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task UpdateFileRegistrationAsync(FileRegistration fileRegistration);

        /// <summary>
        /// Inserting the file registration transactions against the homologated messages.
        /// </summary>
        /// <param name="fileRegistration">The file registrations.</param>
        /// <param name="inventory">The homologated messages.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task InsertFileRegistrationTransactionsForInventoryAsync(FileRegistration fileRegistration, JArray inventory);

        /// <summary>
        /// Inserting the file registration transactions against the homologated messages.
        /// </summary>
        /// <param name="fileRegistration">The file registrations.</param>
        /// <param name="movements">The movements.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task InsertFileRegistrationTransactionsForMovementsAsync(FileRegistration fileRegistration, JArray movements);

        /// <summary>
        /// Updating the file registration transaction status.
        /// </summary>
        /// <param name="fileRegistrationTransactionId">The file registrations transaction identifier.</param>
        /// <param name="statusType">The file registrations transaction status.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task UpdateFileRegistrationTransactionStatusAsync(int fileRegistrationTransactionId, StatusType statusType);

        /// <summary>
        /// Registers the failure asynchronous.
        /// </summary>
        /// <param name="pendingTransaction">The pending transaction.</param>
        /// <param name="fileRegistrationTransactionId">The file registration transaction identifier.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorParams">The error parameters.</param>
        /// <returns>The task.</returns>
        Task RegisterFailureAsync(PendingTransaction pendingTransaction, int fileRegistrationTransactionId, object exception, string errorMessage, params object[] errorParams);
    }
}
