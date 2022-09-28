// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeadletterController.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Deadletter.Interfaces;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The DeadletterController.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Deadletter")]
    public class DeadletterController : ODataController
    {
        /// <summary>
        /// The deadletter processor.
        /// </summary>
        private readonly IDeadletterProcessor deadletterProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeadletterController"/> class.
        /// </summary>
        /// <param name="deadletterProcessor">The deadletter Processor.</param>
        public DeadletterController(IDeadletterProcessor deadletterProcessor)
        {
            this.deadletterProcessor = deadletterProcessor;
        }

        /// <summary>
        /// Gets all the deadlettred message with status processing.
        /// This method supports ODATA query.
        /// </summary>
        /// <returns>
        /// The deadlettred message.
        /// </returns>
        [HttpGet]
        [EnableQuery]
        [Route("failures")]
        [ODataRoute("failures")]
        [TrueAuthorize]
        public Task<IQueryable<DeadletteredMessage>> QueryDeadletteredMessagesAsync()
        {
            return this.deadletterProcessor.QueryAllAsync<DeadletteredMessage>(x => x.Status);
        }

        /// <summary>
        /// Retrigger deadletter message.
        /// </summary>
        /// <param name="messageIds">The deadletter message.</param>
        /// <returns>The list of messages.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/failures")]
        [TrueAuthorize]
        [DeadLetterFilter]
        public async Task<IActionResult> ReTriggerAsync(IEnumerable<int> messageIds)
        {
            var status = await this.deadletterProcessor.RetriggerAsync(messageIds).ConfigureAwait(false);
            return new EntityResult(status ? Entities.Constants.ReTriggerSuccessful : Entities.Constants.ReTriggerFailed);
        }

        /// <summary>
        /// The blocChain.
        /// </summary>
        /// <param name="isCritical">Is Critical.</param>
        /// <param name="takeRecords">Take Records.</param>
        /// <returns>list.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/failures/blockchain")]
        [TrueAuthorize]
        public Task<BlockchainFailures> GetReconciliationInfoAsync(bool isCritical, int? takeRecords)
        {
            BlockchainFailuresRequest failuresRequest = new BlockchainFailuresRequest { IsCritical = isCritical, TakeRecords = takeRecords };
            return this.deadletterProcessor.GetFailuresAsync(failuresRequest);
        }

        /// <summary>
        /// Resets the failed records.
        /// </summary>
        /// <param name="failures">The failures.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/failures/blockchain")]
        [TrueAuthorize]
        public async Task<IActionResult> ResetAsync(BlockchainFailures failures)
        {
            await this.deadletterProcessor.ResetAsync(failures).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.ResetCriticalSuccessful);
        }
    }
}
