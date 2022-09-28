// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementController.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The movementController controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Admin")]
    public class MovementController : ODataController
    {
        /// <summary>
        /// The movement processor.
        /// </summary>
        private readonly IManualMovementProcessor manualMovementProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementController"/> class.
        /// </summary>
        /// <param name="processor">The processor.</param>
        public MovementController(IManualMovementProcessor processor)
        {
            this.manualMovementProcessor = processor;
        }

        /// <summary>
        /// Gets manual movements asynchronously.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="startTime">The start date.</param>
        /// <param name="endTime">The end date.</param>
        /// <returns>The ActionResult.</returns>
        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 3)]
        [Route("movements")]
        [ODataRoute("movements")]
        [TrueAuthorize(Role.Chain)]
        public Task<IQueryable<Movement>> QueryManualMovementsAsync(int nodeId, DateTime startTime, DateTime endTime)
        {
            var movements = this.manualMovementProcessor.GetAssignableMovementsAsync(nodeId, startTime, endTime);

            return movements;
        }

        /// <summary>
        /// Updates the manual movements for an existing ticket.
        /// </summary>
        /// <param name="deltaNodeId">The ticket id.</param>
        /// <param name="movementIds">The array of movement ids.</param>
        /// <returns>The IActionResult task.</returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/deltaNodes/{deltaNodeId:int}/manualMovements")]
        [TrueAuthorize(Role.Chain)]
        public async Task<IActionResult> UpdateManualMovementsForTicketAsync(int deltaNodeId, int[] movementIds)
        {
            await this.manualMovementProcessor.UpdateTicketManualMovementsAsync(deltaNodeId, movementIds).ConfigureAwait(false);

            return new EntityResult();
        }
    }
}