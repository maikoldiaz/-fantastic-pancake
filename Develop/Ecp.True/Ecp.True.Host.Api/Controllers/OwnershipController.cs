// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipController.cs" company="Microsoft">
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
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The category controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Admin")]
    public class OwnershipController : ODataController
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private readonly IOwnershipProcessor processor;

        /// <summary>
        /// The queue processor.
        /// </summary>
        private readonly IQueueProcessor queueprocessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipController"/> class.
        /// </summary>
        /// <param name="processor">The processor.</param>
        /// <param name="queueprocessor">The queueprocessor.</param>
        public OwnershipController(IOwnershipProcessor processor, IQueueProcessor queueprocessor)
        {
            this.processor = processor;
            this.queueprocessor = queueprocessor;
        }

        /// <summary>
        /// Queries the system ownership calculation asynchronous.
        /// </summary>
        /// <returns>The System Ownership Calculation.</returns>
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 3)]
        [Route("systemownershipcalculations")]
        [ODataRoute("systemownershipcalculations")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Query)]
        public Task<IQueryable<SystemOwnershipCalculation>> QuerySystemOwnershipCalculationAsync()
        {
            return this.processor.QueryAllAsync<SystemOwnershipCalculation>(null);
        }

        /// <summary>
        /// ownership.
        /// </summary>
        /// <param name="ticketId">The ticketId.</param>
        /// <returns>The Ownership Recalculation.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/ownership/recalculateBalance")]
        [TrueAuthorize]
        public async Task<IActionResult> RecalculateOwnershipAsync(int ticketId)
        {
            await this.queueprocessor.PushQueueMessageAsync(ticketId, QueueConstants.RecalculateOwnershipBalanceQueue).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.PushMessageOwnerShipCreateSuccess);
        }
    }
}
