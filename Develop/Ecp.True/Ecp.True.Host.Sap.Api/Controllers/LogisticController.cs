// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogisticController.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Host.Sap.Api.Filter;
    using Ecp.True.Processors.Transform.Input.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The Sales controller.
    /// </summary>
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Sap")]
    public class LogisticController : ControllerBase
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
        /// Initializes a new instance of the <see cref="LogisticController" /> class.
        /// </summary>
        /// <param name="businessContext">The business context.</param>
        /// <param name="processor">The processor.</param>
        public LogisticController(IBusinessContext businessContext, IInputFactory processor)
        {
            this.businessContext = businessContext;
            this.processor = processor;
        }

        /// <summary>
        /// Creates new sales.
        /// </summary>
        /// <param name="logisticMovement">The logistic movement response.</param>
        /// <returns>Returns the status.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/logisticmovements")]
        [ValidateSapRequestFilter("LogisticMovement")]
        public Task<IActionResult> ProcessLogisticMovementResponseAsync([FromBody] LogisticMovementResponse logisticMovement)
        {
            return this.SaveAsync(JObject.Parse(JsonConvert.SerializeObject(logisticMovement)), MessageType.Logistic);
        }

        /// <summary>
        /// Saves the SAP input.
        /// </summary>
        /// <param name="item">The items.</param>
        /// <param name="type">The type.</param>
        /// <returns>Returns the status.</returns>
        private async Task<IActionResult> SaveAsync(JObject item, MessageType type)
        {
            ArgumentValidators.ThrowIfNull(item, nameof(item));

            var uploadId = Guid.NewGuid().ToString();

            var blobPath = $"{SystemType.SAP.ToString().ToLowerCase()}/{type}/{uploadId}";
            var trueMessage = new TrueMessage(SystemType.SAP, type, uploadId, blobPath, this.businessContext.ActivityId, false, IntegrationType.RESPONSE);

            await this.processor.SaveSapLogisticJsonAsync(item, trueMessage).ConfigureAwait(false);

            return new EntityResult(new Guid(trueMessage.MessageId));
        }
    }
}
