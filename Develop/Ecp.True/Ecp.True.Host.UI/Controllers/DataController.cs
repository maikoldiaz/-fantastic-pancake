// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataController.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.UI.Controllers
{
    using System.Threading.Tasks;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.UI.Models;
    using Ecp.True.Host.UI.Services.Core;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The data controller.
    /// </summary>
    [Authorize]
    public class DataController : Controller
    {
        /// <summary>
        /// The data service.
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataController"/> class.
        /// </summary>
        /// <param name="dataService">The data service.</param>
        public DataController(IDataService dataService)
        {
            this.dataService = dataService;
        }

        /// <summary>
        /// Queries asynchronously.
        /// The method supports ODATA query.
        /// </summary>
        /// <param name="version">the version.</param>
        /// <param name="path">the path.</param>
        /// <returns>The data.</returns>
        [HttpGet]
        [Route("v{version}/odata/{*path}")]
        [TrueAuthorize]
        public async Task<IActionResult> QueryAsync(string version, string path)
        {
            var queryString = $"{this.Request.QueryString.Value}&$count=true";
            queryString = queryString.StartsWith('?') ? queryString : $"?{queryString}";
            var pagedResult = await this.dataService.QueryEntityAsync<PagedResult<object>>(
                version,
                queryString,
                path).ConfigureAwait(false);
            return this.Json(pagedResult);
        }

        /// <summary>
        /// Gets asynchronously.
        /// </summary>
        /// <param name="version">the version.</param>
        /// <param name="path">the path.</param>
        /// <returns>The data.</returns>
        [HttpGet]
        [Route("v{version}/{*path}")]
        [TrueAuthorize]
        public async Task<IActionResult> GetAsync(string version, string path)
        {
            var result = await this.dataService.GetEntityAsync<object>(
                version,
                path).ConfigureAwait(false);
            return this.Json(result);
        }

        /// <summary>
        /// Posts asynchronously.
        /// </summary>
        /// <param name="version">the version.</param>
        /// <param name="path">the path.</param>
        /// <param name="body">the body.</param>
        /// <returns>The data.</returns>
        [HttpPost]
        [Route("v{version}/{*path}")]
        [TrueAuthorize]
        public async Task<IActionResult> PostAsync(string version, string path, [FromBody] object body)
        {
            var result = await this.dataService.SaveAndGetResultAsync<object, object>(
                body,
                version,
                path).ConfigureAwait(false);
            return this.Json(result);
        }

        /// <summary>
        /// Puts asynchronously.
        /// </summary>
        /// <param name="version">the version.</param>
        /// <param name="path">the path.</param>
        /// <param name="body">the body.</param>
        /// <returns>The data.</returns>
        [HttpPut]
        [Route("v{version}/{*path}")]
        [TrueAuthorize]
        public async Task<IActionResult> PutAsync(string version, string path, [FromBody] object body)
        {
            var result = await this.dataService.UpdateEntityAndGetResultAsync<object, object>(
                body,
                version,
                path).ConfigureAwait(false);
            return this.Json(result);
        }

        /// <summary>
        /// Deletes asynchronously.
        /// </summary>
        /// <param name="version">the version.</param>
        /// <param name="path">the path.</param>
        /// <param name="body">the body.</param>
        /// <returns>The data.</returns>
        [HttpDelete]
        [Route("v{version}/{*path}")]
        [TrueAuthorize]
        public async Task<IActionResult> DeleteAsync(string version, string path, [FromBody] object body)
        {
            var result = await this.dataService.RemoveAndGetResultAsync<object, object>(
                body,
                version,
                path).ConfigureAwait(false);
            return this.Json(result);
        }
    }
}
