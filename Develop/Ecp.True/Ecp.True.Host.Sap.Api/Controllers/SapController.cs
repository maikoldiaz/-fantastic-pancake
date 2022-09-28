// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapController.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Sap.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Host.Sap.Api.Filter;
    using Ecp.True.Processors.Transform.Input.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The category controller.
    /// </summary>
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Sap")]
    [Authorize(Roles = HostConstants.SapRoleClaimType)]
    public class SapController : ControllerBase
    {
        /// <summary>
        /// The business context.
        /// </summary>
        private readonly IBusinessContext businessContext;

        /// <summary>
        /// The processor.
        /// </summary>
        private readonly IInputFactory processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SapController" /> class.
        /// </summary>
        /// <param name="businessContext">The business context.</param>
        /// <param name="processor">The processor.</param>
        public SapController(IBusinessContext businessContext, IInputFactory processor)
        {
            this.businessContext = businessContext;
            this.processor = processor;
        }

        /// <summary>
        /// Creates new movements.
        /// </summary>
        /// <param name="movements">The movements array.</param>
        /// <returns>Returns the status.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/movements")]
        [ValidateSapRequestFilter("movements")]
        public Task<IActionResult> CreateMovementsAsync([FromBody] IEnumerable<SapMovement> movements)
        {
            ArgumentValidators.ThrowIfNull(movements, nameof(movements));

            // Ignore backup movement
            movements.ForEach(x => { x.BackupMovement = null; });

            return this.SaveAsync(JArray.FromObject(movements), MessageType.Movement);
        }

        /// <summary>
        /// Updates the official points asynchronous.
        /// </summary>
        /// <param name="movements">The movements.</param>
        /// <returns>Returns the status.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/movements/official")]
        [ValidateOfficialPoints("movements")]
        public Task<IActionResult> UpdateOfficialPointsAsync([FromBody] IEnumerable<SapMovement> movements)
        {
            if (movements.Count() == 1)
            {
                // Ignore backup movement id for single movement
                movements.ForEach(x => { x.BackupMovement.BackupMovementId = null; });
            }

            if (movements.Count() == 2)
            {
                // Ignore backup movement id for backup movement
                movements.ForEach(x =>
                {
                    if (x.BackupMovement.IsOfficial == false)
                    {
                        x.BackupMovement.BackupMovementId = null;
                    }
                });
            }

            return this.SaveAsync(JArray.FromObject(movements), MessageType.Movement, true);
        }

        /// <summary>
        /// Creates new inventories.
        /// </summary>
        /// <param name="inventories">The inventories array.</param>
        /// <returns>Returns the status.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/inventories")]
        [ValidateSapRequestFilter("inventories")]
        public Task<IActionResult> CreateInventoryAsync([FromBody] IEnumerable<SapInventory> inventories)
        {
            return this.SaveAsync(JArray.FromObject(inventories), MessageType.Inventory);
        }

        /// <summary>
        /// Saves the SAP input.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="type">The type.</param>
        /// <param name="isOfficial">The isOfficial.</param>
        /// <returns>Returns the status.</returns>
        private async Task<IActionResult> SaveAsync(JArray items, MessageType type, bool isOfficial = false)
        {
            ArgumentValidators.ThrowIfNull(items, nameof(items));

            var uploadId = Guid.NewGuid().ToString();

            var blobPath = $"{SystemType.SAP.ToString().ToLowerCase()}/{type}/{uploadId}";
            var trueMessage = new TrueMessage(SystemType.SAP, type, uploadId, blobPath, this.businessContext.ActivityId, isOfficial, IntegrationType.REQUEST);
            await this.processor.SaveSapJsonArrayAsync(items, trueMessage).ConfigureAwait(false);

            return isOfficial ? new EntityResult() : new EntityResult(new Guid(trueMessage.MessageId));
        }
    }
}
