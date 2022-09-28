// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConciliationController.cs" company="Microsoft">
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
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Conciliation.Interfaces;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The category controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Admin")]
    public class ConciliationController : ODataController
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private readonly IConciliationProcessor conciliationProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConciliationController"/> class.
        /// </summary>
        /// <param name="conciliationProcessor">The conciliation ownership processor.</param>
        public ConciliationController(IConciliationProcessor conciliationProcessor)
        {
            this.conciliationProcessor = conciliationProcessor;
        }

        /// <summary>
        /// Ownership manual conciliation.
        /// </summary>
        /// <param name="conciliationRequest">The conciliation request.</param>
        /// <returns>The result of conciliation.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/conciliation/initialize")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> InitializeConciliationAsync([FromBody] ConciliationNodesResquest conciliationRequest)
        {
            ArgumentValidators.ThrowIfNull(conciliationRequest, nameof(conciliationRequest));
            await this.conciliationProcessor.InitializeConciliationAsync(conciliationRequest).ConfigureAwait(false);
            return new EntityResult();
        }
    }
}
