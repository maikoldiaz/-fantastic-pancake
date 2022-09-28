// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnnulationController.cs" company="Microsoft">
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
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
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
    public class AnnulationController : ODataController
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private readonly IAnnulationProcessor processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnulationController"/> class.
        /// </summary>
        /// <param name="processor">The processor.</param>
        public AnnulationController(IAnnulationProcessor processor)
        {
            this.processor = processor;
        }

        /// <summary>
        /// Gets all the annulations.
        /// This method supports ODATA query.
        /// </summary>
        /// <returns>The annulation response.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("annulations")]
        [ODataRoute("annulations")]
        [TrueAuthorize(Role.Administrator)]
        public Task<IQueryable<Annulation>> QueryAnnulationsAsync()
        {
            return this.processor.QueryAllAsync<Annulation>(null);
        }

        /// <summary>
        /// Creates a new annulation.
        /// </summary>
        /// <param name="annulation">The reversal.</param>
        /// <returns>Returns the status.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/annulations")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> CreateAnnulationRelationshipAsync([FromBody] Annulation annulation)
        {
            await this.processor.CreateAnnulationRelationshipAsync(annulation).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.AnnulationCreatedSuccessfully);
        }

        /// <summary>
        /// Updates the annulation relationship asynchronous.
        /// </summary>
        /// <param name="annulation">The annulation.</param>
        /// <returns>Returns the status.</returns>
        [HttpPut]
        [Route("api/v{version:apiVersion}/annulations")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> UpdateAnnulationRelationshipAsync([FromBody] Annulation annulation)
        {
            await this.processor.UpdateAnnulationRelationshipAsync(annulation).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.AnnulationUpdatedSuccessfully);
        }

        /// <summary>
        /// Check annulation exists.
        /// </summary>
        /// <param name="annulation">The reversal.</param>
        /// <returns>Returns boolen.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/annulations/exists")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> ExistsAnnulationsRelationshipAsync([FromBody] Annulation annulation)
        {
            var reversalRelationship = await this.processor.ExistsAnnulationRelationshipAsync(annulation).ConfigureAwait(false);
            return new EntityResult(reversalRelationship, Entities.Constants.AnnulationDoesNotExist);
        }
    }
}
