// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MasterController.cs" company="Microsoft">
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
    using Ecp.True.Core.Entities;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The master controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Admin")]
    public class MasterController : ODataController
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private readonly IMasterProcessor processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterController"/> class.
        /// </summary>
        /// <param name="processor">The processor.</param>
        public MasterController(IMasterProcessor processor)
        {
            this.processor = processor;
        }

        /// <summary>
        /// Gets all logistic centers.
        /// </summary>
        /// <returns>The list of logistic centers.</returns>
        /// <response code="200">The list of all logistic centers.</response>
        /// <response code="500">Unknown error while getting logistic centers.</response>
        [HttpGet]
        [Route("api/v{version:apiVersion}/logisticcenters")]
        [ProducesResponseType(typeof(IEnumerable<LogisticCenter>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [TrueAuthorize]
        public async Task<IActionResult> GetLogisticCentersAsync()
        {
            var result = await this.processor.GetLogisticCentersAsync().ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Gets all storage locations.
        /// </summary>
        /// <returns>The list of storage locations.</returns>
        /// <response code="200">The list of all storage locations.</response>
        /// <response code="500">Unknown error while getting storage locations.</response>
        [HttpGet]
        [EnableQuery]
        [Route("storagelocations")]
        [ODataRoute("storagelocations")]
        [TrueAuthorize]
        public async Task<IQueryable<StorageLocation>> QueryStorageLocationsAsync()
        {
            var result = await this.processor.GetStorageLocationsAsync().ConfigureAwait(false);
            return result.AsQueryable();
        }

        /// <summary>
        /// Gets all scenarios.
        /// </summary>
        /// <returns>The list of scenarios with features.</returns>
        /// <response code="200">The list of all scenarios.</response>
        /// <response code="500">Unknown error while getting scenarios.</response>
        [HttpGet]
        [Route("api/v{version:apiVersion}/scenarios")]
        [ProducesResponseType(typeof(IEnumerable<Scenario>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [TrueAuthorize]
        public async Task<IActionResult> GetScenariosAsync()
        {
            var groupIds = this.HttpContext.User.Claims.Where(claim => claim.Type == "groups").Select(a => a.Value);
            var scenarios = await this.processor.GetScenariosByRoleAsync(groupIds).ConfigureAwait(false);
            return new EntityResult(scenarios);
        }

        /// <summary>
        /// Queries the products asynchronous.
        /// </summary>
        /// <returns>List of Products.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("products")]
        [ODataRoute("products")]
        [TrueAuthorize]
        public Task<IQueryable<Product>> QueryProductsAsync()
        {
            return this.processor.QueryAllAsync<Product>(null);
        }

        /// <summary>Queries the algorithm list asynchronous.</summary>
        /// <returns>Returns the algorithms.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/algorithms")]
        [ProducesResponseType(typeof(IEnumerable<Algorithm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [TrueAuthorize]
        public async Task<IActionResult> GetAlgorithmsListAsync()
        {
            var result = await this.processor.GetAlgorithmsAsync().ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Get all system types.
        /// </summary>
        /// <returns>List of system types.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("systemTypes")]
        [ODataRoute("systemTypes")]
        [TrueAuthorize]
        public async Task<IActionResult> QuerySystemTypesAsync()
        {
            var result = await this.processor.GetSystemTypesAsync().ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>List of users.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/users")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [TrueAuthorize]
        public async Task<IActionResult> GetUsersAsync()
        {
            var result = await this.processor.GetUsersAsync().ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Queries the icons asynchronous.
        /// </summary>
        /// <returns>The icons.</returns>
        /// <response code="200">The ODATA query response.</response>
        [HttpGet]
        [EnableQuery]
        [Route("icons")]
        [ODataRoute("icons")]
        [TrueAuthorize]
        public Task<IQueryable<Icon>> QueryIconsAsync()
        {
            return this.processor.QueryAllAsync<Icon>(null);
        }

        /// <summary>
        /// Gets the variable types asynchronous.
        /// </summary>
        /// <returns>The VariableTypes.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/variabletypes")]
        [ProducesResponseType(typeof(IEnumerable<VariableTypeEntity>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [TrueAuthorize]
        public async Task<IActionResult> GetVariableTypesAsync()
        {
            var result = await this.processor.GetVariableTypesAsync().ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Gets the icons asynchronous.
        /// </summary>
        /// <returns>The Icons.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/icons")]
        [ProducesResponseType(typeof(IEnumerable<Icon>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [TrueAuthorize]
        public async Task<IActionResult> GetIconsAsync()
        {
            var result = await this.processor.GetIconsAsync().ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Gets the origin types asynchronous.
        /// </summary>
        /// <returns>The origin types.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/origintypes")]
        [ProducesResponseType(typeof(IEnumerable<OriginType>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [TrueAuthorize]
        public async Task<IActionResult> GetOriginTypesAsync()
        {
            var result = await this.processor.GetOriginTypesAsync().ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Gets the product mappings.
        /// </summary>
        /// <returns>The task.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("storageLocationProductMappings")]
        [ODataRoute("storageLocationProductMappings")]
        [TrueAuthorize]
        public Task<IQueryable<StorageLocationProductMapping>> QueryStorageLocationProductMappingAsync()
        {
            return this.processor.QueryAllAsync<StorageLocationProductMapping>(null);
        }
    }
}
