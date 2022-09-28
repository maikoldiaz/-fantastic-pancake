// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PendingTransactionErrorProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Admin
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Core;

    /// <summary>
    /// The volumetric processor.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Api.Interfaces.IPendingTransactionProcessor" />
    public class PendingTransactionErrorProcessor : ProcessorBase, IPendingTransactionErrorProcessor
    {
        private readonly IPendingTransactionErrorRepository pendingTransactionErrorRepository;

        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PendingTransactionErrorProcessor"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="pendingTransactionErrorRepository">The error repository.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public PendingTransactionErrorProcessor(IRepositoryFactory factory, IPendingTransactionErrorRepository pendingTransactionErrorRepository, IUnitOfWorkFactory unitOfWorkFactory)
             : base(factory)
        {
            this.pendingTransactionErrorRepository = pendingTransactionErrorRepository;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Gets the cut off message exceptions.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>
        /// Returns the cutoff message exceptions.
        /// </returns>
        public Task<IEnumerable<PendingTransactionErrorDto>> GetPendingTransactionsAsync(Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));

            return this.pendingTransactionErrorRepository.GetPendingTransactionErrorsAsync(ticket);
        }

        /// <summary>
        /// Saves the comments asynchronous.
        /// </summary>
        /// <param name="errorComment">The error comment dto.</param>
        /// <returns>Return the status of update comment.</returns>
        public async Task SaveCommentsAsync(ErrorComment errorComment)
        {
            ArgumentValidators.ThrowIfNull(errorComment, nameof(errorComment));

            using (var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var repository = unitOfWork.CreateRepository<PendingTransactionError>();
                foreach (int errorId in errorComment.ErrorId)
                {
                    var pendingTransaction = await repository.GetByIdAsync(errorId).ConfigureAwait(false);
                    pendingTransaction.Comment = errorComment.Comment;
                    repository.Update(pendingTransaction);
                }

                await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the error details asynchronous.
        /// </summary>
        /// <param name="errorId">The error identifier. Format: d_id_p/f_I/M.</param>
        /// <param name="canRetry">if set to <c>true</c> [can retry].</param>
        /// <returns>
        /// the error details.
        /// </returns>
        public Task<IEnumerable<ErrorDetail>> GetErrorDetailsAsync(string errorId, bool canRetry)
        {
            ArgumentValidators.ThrowIfNull(errorId, nameof(errorId));

            var parameters = new Dictionary<string, object>
            {
                { "@PendingTransactionId", !canRetry ? errorId : null },
                { "@RecordId", canRetry ? errorId : null },
            };

            return this.CreateRepository<ErrorDetail>().ExecuteQueryAsync(Repositories.Constants.GetErrorDetailsProcedureName, parameters);
        }
    }
}
