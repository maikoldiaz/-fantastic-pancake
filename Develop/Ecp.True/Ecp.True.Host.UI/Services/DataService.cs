// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataService.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.UI.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Host.UI.Services.Core;

    /// <summary>
    /// The data service.
    /// </summary>
    public class DataService : IDataService
    {
        /// <summary>
        /// The API service.
        /// </summary>
        private readonly IApiService apiService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataService"/> class.
        /// </summary>
        /// <param name="apiService">The API service.</param>
        public DataService(IApiService apiService)
        {
            this.apiService = apiService;
        }

        /// <inheritdoc />
        public Task<T> GetEntityAsync<T>(string apiVersion, params string[] uriParts)
        {
            return this.apiService.GetAsync<T>(BuildApiPath(apiVersion, uriParts));
        }

        /// <inheritdoc />
        public Task<bool> SaveEntityAsync<T>(T entity, string apiVersion, params string[] uriParts)
        {
            return this.apiService.PostAsync(BuildApiPath(apiVersion, uriParts), entity);
        }

        /// <inheritdoc />
        public Task<bool> SaveAsync(object entity, string apiVersion, params string[] uriParts)
        {
            return this.apiService.PostAsync(BuildApiPath(apiVersion, uriParts), entity);
        }

        /// <inheritdoc />
        public Task<T> SaveAndGetResultAsync<T>(T entity, string apiVersion, params string[] uriParts)
        {
            return this.apiService.PostAndGetResultAsync<T>(BuildApiPath(apiVersion, uriParts), entity);
        }

        /// <inheritdoc />
        public Task<TResult> SaveAndGetResultAsync<TResult, T>(T entity, string apiVersion, params string[] uriParts)
        {
            return this.apiService.PostAndGetResultAsync<TResult, T>(BuildApiPath(apiVersion, uriParts), entity);
        }

        /// <inheritdoc />
        public Task<bool> UpdateEntityAsync<T>(T entity, string apiVersion, params string[] uriParts)
        {
            return this.apiService.PutAsync(BuildApiPath(apiVersion, uriParts), entity);
        }

        /// <inheritdoc />
        public Task<TResult> UpdateEntityAndGetResultAsync<TResult, T>(T entity, string apiVersion, params string[] uriParts)
        {
            return this.apiService.PutAndGetResultAsync<TResult, T>(BuildApiPath(apiVersion, uriParts), entity);
        }

        /// <inheritdoc />
        public Task<T> QueryEntityAsync<T>(string apiVersion, string queryString, params string[] uriParts)
        {
            return this.apiService.GetAsync<T>(BuildQueryPath(apiVersion, uriParts, queryString));
        }

        /// <inheritdoc />
        public Task<IQueryable<T>> GetEntitiesAsync<T>(string apiVersion, params string[] uriParts)
        {
            return this.apiService.GetAsync<IQueryable<T>>(BuildApiPath(apiVersion, uriParts));
        }

        /// <inheritdoc />
        public Task<IQueryable<T>> QueryEntitiesAsync<T>(string apiVersion, string queryString, params string[] uriParts)
        {
            return this.apiService.GetAsync<IQueryable<T>>(BuildApiPath(apiVersion, uriParts, queryString));
        }

        /// <inheritdoc />
        public Task<bool> RemoveEntityAsync(string entityId, string apiVersion, params string[] uriParts)
        {
            var parts = new List<string>();

            parts.AddRange(uriParts);
            parts.Add(entityId);

            return this.apiService.DeleteAsync(BuildApiPath(apiVersion, parts.ToArray()));
        }

        /// <inheritdoc />
        public Task<TResult> RemoveAndGetResultAsync<TResult, T>(T entity, string apiVersion, params string[] uriParts)
        {
            return this.apiService.DeleteAndGetResultAsync<TResult, T>(BuildApiPath(apiVersion, uriParts), entity);
        }

        /// <summary>
        /// Builds the API URL.
        /// </summary>
        /// <param name="apiVersion">The API version.</param>
        /// <param name="uriParts">The Uri parts.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns>
        /// The API URL.
        /// </returns>
        private static string BuildApiPath(string apiVersion, string[] uriParts, string queryString = null)
        {
            var apiParams = new List<string>
            {
                $"api/v{apiVersion}",
            };

            apiParams.AddRange(uriParts);

            var apiUrl = string.Join("/", apiParams);

            return string.IsNullOrWhiteSpace(queryString) ? apiUrl : string.Concat(apiUrl, queryString);
        }

        /// <summary>
        /// Builds the API URL.
        /// </summary>
        /// <param name="apiVersion">The API version.</param>
        /// <param name="uriParts">The Uri parts.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns>
        /// The API URL.
        /// </returns>
        private static string BuildQueryPath(string apiVersion, string[] uriParts, string queryString = null)
        {
            var apiParams = new List<string>
            {
                $"odata/v{apiVersion}",
            };

            apiParams.AddRange(uriParts);

            var apiUrl = string.Join("/", apiParams);

            return string.IsNullOrWhiteSpace(queryString) ? apiUrl : string.Concat(apiUrl, queryString);
        }
    }
}