// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationController.cs" company="Microsoft">
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
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Api.Filter;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The NodeController.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Admin")]
    public class HomologationController : ODataController
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private readonly IHomologationProcessor processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomologationController"/> class.
        /// </summary>
        /// <param name="processor">The processor.</param>
        public HomologationController(IHomologationProcessor processor)
        {
            this.processor = processor;
        }

        /// <summary>
        /// Creates a new node.
        /// </summary>
        /// <param name="homologation">The homologation.</param>
        /// <returns>[True] if the creation is success.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/homologations")]
        [TrueAuthorize]
        [ValidateHomologationFilter]
        public async Task<IActionResult> CreateHomologationAsync([FromBody]Homologation homologation)
        {
            ArgumentValidators.ThrowIfNull(homologation, nameof(homologation));

            var isNew = homologation.HomologationGroups.Single().HomologationGroupId == 0;
            await this.processor.SaveHomologationAsync(homologation).ConfigureAwait(false);

            return new EntityResult(isNew ? Entities.Constants.HomologationCreateSuccess : Entities.Constants.HomologationUpdateSuccess);
        }

        /// <summary>
        /// Queries the homologations asynchronous.
        /// </summary>
        /// <returns>The homologations.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("homologations")]
        [ODataRoute("homologations")]
        [TrueAuthorize]
        public Task<IQueryable<Homologation>> QueryHomologationsAsync()
        {
            return this.processor.QueryAllAsync<Homologation>(null);
        }

        /// <summary>
        /// Get Homologation By Id.
        /// </summary>
        /// <param name="homologationId">homologation Id.</param>
        /// <returns>Homologation Data.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/homologations/{homologationId}")]
        [TrueAuthorize]
        public async Task<IActionResult> GetHomologationByIdAsync(int homologationId)
        {
            var result = await this.processor.GetHomologationByIdAsync(homologationId).ConfigureAwait(false);
            return new EntityResult(result, Entities.Constants.HomologationDoesNotExists);
        }

        /// <summary>
        /// Get Homologation By HomologationId And HomologationGroupId.
        /// </summary>
        /// <param name="homologationId">Homologation Id.</param>
        /// <param name="groupId">Homologation Group Id.</param>
        /// <returns>Homologation Data.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/homologations/{homologationId}/groups/{groupId}")]
        [TrueAuthorize]
        public async Task<IActionResult> GetHomologationByIdAndGroupIdAsync(int homologationId, int groupId)
        {
            var result = await this.processor.GetHomologationByIdAndGroupIdAsync(homologationId, groupId).ConfigureAwait(false);
            return new EntityResult(result, Entities.Constants.HomologationGroupDoesNotExists);
        }

        /// <summary>
        /// Deletes the transformation.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns>
        /// Return the task.
        /// </returns>
        [HttpDelete]
        [Route("api/v{version:apiVersion}/homologations/groups")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> DeleteHomologationGroupAsync([FromBody]DeleteHomologationGroup group)
        {
            ArgumentValidators.ThrowIfNull(group, nameof(group));

            await this.processor.DeleteHomologationGroupAsync(group).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.HomologationGroupDeletedSuccessfully);
        }

        /// <summary>
        /// Gets the list of homologation Groups.
        /// </summary>
        /// <returns>Returns the homologation Groups.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("homologationGroups")]
        [ODataRoute("homologationGroups")]
        [TrueAuthorize(Role.Administrator)]
        public Task<IQueryable<HomologationGroup>> QueryHomologationGroupAsync()
        {
            return this.processor.QueryAllAsync<HomologationGroup>(null);
        }

        /// <summary>
        /// Gets the list of homologation Groups.
        /// </summary>
        /// <returns>Returns the homologation Groups.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/homologationObjectTypes")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> GetHomologationObjectTypesAsync()
        {
            var homologationObjectTypes = await this.processor.GetHomologationObjectTypesAsync().ConfigureAwait(false);
            return new EntityResult(homologationObjectTypes);
        }

        /// <summary>
        /// check homologation group exists or not.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="sourceSystemId">The source system identifier.</param>
        /// <param name="destinationSystemId">The destination system identifier.</param>
        /// <returns>List of system types.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/groups/{groupId}/source/{sourceSystemId}/destination/{destinationSystemId}/exists")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> CheckHomologationGroupExistsAsync(int groupId, int sourceSystemId, int destinationSystemId)
        {
            var homologationGroup = await this.processor.CheckHomologationGroupExistsAsync(groupId, sourceSystemId, destinationSystemId).ConfigureAwait(false);
            return new EntityExistsResult(nameof(HomologationGroup), homologationGroup);
        }

        /// <summary>
        /// Gets the detail of homologation Groups.
        /// </summary>
        /// <param name="homologationGroupId">The homologation group identifier.</param>
        /// <returns>Returns the homologation group detail.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/homologations/groups/{homologationGroupId}/mappings")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> GetHomologationGroupMappingsAsync(int homologationGroupId)
        {
            var homologationObjectTypes = await this.processor.GetHomologationGroupMappingsAsync(homologationGroupId).ConfigureAwait(false);
            return new EntityResult(homologationObjectTypes);
        }
    }
}
