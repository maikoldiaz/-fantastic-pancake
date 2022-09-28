// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PendingTransactionErrorController.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The Register file controller.
    /// </summary>
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "balance transport")]
    public class PendingTransactionErrorController : ODataController
    {
        private readonly IPendingTransactionErrorProcessor pendingTransactionErrorProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="PendingTransactionErrorController"/> class.
        /// </summary>
        /// <param name="pendingTransactionErrorProcessor">The pending transaction error processor.</param>
        public PendingTransactionErrorController(IPendingTransactionErrorProcessor pendingTransactionErrorProcessor)
        {
            this.pendingTransactionErrorProcessor = pendingTransactionErrorProcessor;
        }

        /// <summary>
        /// Get Write SasToken for file.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>
        /// Returns the pending transactions.
        /// </returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/transactions/pending")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> GetPendingTransactionErrorsAsync(Ticket ticket)
        {
            var result = await this.pendingTransactionErrorProcessor.GetPendingTransactionsAsync(ticket).ConfigureAwait(false);
            return result.Any() ? new EntityResult(result) : new EntityResult(null, Entities.Constants.PendingTransactionErrorsDoNotExist);
        }

        /// <summary>
        /// Gets exceptions asynchronously.
        /// </summary>
        /// <returns>
        /// Returns the last cutoff date.
        /// </returns>
        [HttpGet]
        [EnableQuery]
        [Route("pendingtransactionerrors")]
        [ODataRoute("pendingtransactionerrors")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public Task<IQueryable<ExceptionInfo>> QueryExceptionsAsync()
        {
            return this.pendingTransactionErrorProcessor.QueryViewAsync<ExceptionInfo>();
        }

        /// <summary>
        /// Saves the comments asynchronous.
        /// </summary>
        /// <param name="errorComment">The error comment.</param>
        /// <returns>Return the status of update comment.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/transactions/saveComment")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> SaveCommentsAsync(ErrorComment errorComment)
        {
            await this.pendingTransactionErrorProcessor.SaveCommentsAsync(errorComment).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.ErrorCommentUpdatedSuccessfully);
        }

        /// <summary>
        /// Gets the error details asynchronous.
        /// </summary>
        /// <param name="errorId">The error identifier.</param>
        /// <returns>
        /// The error details.
        /// </returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/transactions/{errorId}/pending")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> GetNonRetryErrorDetailsAsync(string errorId)
        {
            var result = await this.pendingTransactionErrorProcessor.GetErrorDetailsAsync(errorId, false).ConfigureAwait(false);
            return result.Any() ? new EntityResult(result) : new EntityResult(null, Entities.Constants.RetryErrorsDoNotExist);
        }

        /// <summary>
        /// Gets the error details asynchronous.
        /// </summary>
        /// <param name="errorId">The error identifier.</param>
        /// <returns>
        /// The error details.
        /// </returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/transactions/{errorId}")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> GetRetryErrorDetailsAsync(string errorId)
        {
            var result = await this.pendingTransactionErrorProcessor.GetErrorDetailsAsync(errorId, true).ConfigureAwait(false);
            return result.Any() ? new EntityResult(result) : new EntityResult(null, Entities.Constants.RetryErrorsDoNotExist);
        }
    }
}