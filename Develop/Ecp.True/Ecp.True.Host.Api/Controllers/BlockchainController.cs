// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainController.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Api.Filter;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Blockchain.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The blockchain controller.
    /// </summary>
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Admin")]
    public class BlockchainController : ControllerBase
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private readonly IBlockchainProcessor processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockchainController"/> class.
        /// </summary>
        /// <param name="processor">The processor.</param>
        public BlockchainController(IBlockchainProcessor processor)
        {
            this.processor = processor;
        }

        /// <summary>
        /// Gets last {pageSize} events from blockchain.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The events page.
        /// </returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/blockchain/events")]
        [TrueAuthorize(Role.Administrator, Role.Auditor)]
        [ValidateBlockRequest]
        public async Task<IActionResult> GetEventsAsync([FromBody]BlockEventRequest request)
        {
            ArgumentValidators.ThrowIfNull(request, nameof(request));
            var result = await this.processor.GetPagedEventsAsync(request.PageSize, request.LastHead).ConfigureAwait(false);

            return new EntityResult(result);
        }

        /// <summary>
        /// Gets the events in range asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The events in range.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/blockchain/events/range")]
        [TrueAuthorize(Role.Administrator, Role.Auditor)]
        [ValidateBlockRangeRequest]
        public async Task<IActionResult> GetEventsInRangeAsync([FromBody]BlockRangeRequest request)
        {
            ArgumentValidators.ThrowIfNull(request, nameof(request));
            var result = await this.processor.GetEventsInRangeAsync(request).ConfigureAwait(false);

            return new EntityResult(result);
        }

        /// <summary>
        /// Gets the transaction asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The transaction details.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/blockchain/transactions")]
        [TrueAuthorize(Role.Administrator, Role.Auditor)]
        public async Task<IActionResult> GetTransactionAsync([FromBody] BlockTransactionRequest request)
        {
            ArgumentValidators.ThrowIfNull(request, nameof(request));
            var result = await this.processor.GetTransactionDetailsAsync(request).ConfigureAwait(false);

            return new EntityResult(result);
        }

        /// <summary>
        /// Transactions the exists asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>If transaction exists.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/blockchain/transactions/exists")]
        [TrueAuthorize(Role.Administrator, Role.Auditor)]
        public async Task<IActionResult> ExistsTransactionAsync([FromBody] BlockTransactionRequest request)
        {
            ArgumentValidators.ThrowIfNull(request, nameof(request));
            var result = await this.processor.TryGetBlockAsync(request).ConfigureAwait(false);

            return new EntityResult(result);
        }

        /// <summary>
        /// Transaction range exists asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>If transaction exists.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/blockchain/transactions/exists/range")]
        [TrueAuthorize(Role.Administrator, Role.Auditor)]
        public async Task<IActionResult> ExistsTransactionRangeAsync([FromBody]BlockRangeRequest request)
        {
            ArgumentValidators.ThrowIfNull(request, nameof(request));
            var result = await this.processor.ValidateBlockRangeAsync(request).ConfigureAwait(false);

            return new EntityResult(result);
        }
    }
}
