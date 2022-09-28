// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiService.cs" company="Microsoft">
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
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.UI.Services.Core;
    using Ecp.True.Host.UI.Services.Core.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// The API service.
    /// </summary>
    public class ApiService : IApiService
    {
        /// <summary>
        /// The HTTP client proxy.
        /// </summary>
        private readonly IHttpClientProxy httpClientProxy;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// The settings.
        /// </summary>
        private readonly JsonSerializerSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiService" /> class.
        /// </summary>
        /// <param name="httpClientProxy">The HTTP client proxy.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        public ApiService(IHttpClientProxy httpClientProxy, IConfigurationHandler configurationHandler)
        {
            this.httpClientProxy = httpClientProxy;
            this.configurationHandler = configurationHandler;
            this.settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }

        /// <inheritdoc />
        public async Task<T> GetAsync<T>(string api)
        {
            using var response = await this.GetResponseAsync(api, HttpMethod.Get).ConfigureAwait(false);
            await response.ThrowIfErrorAsync(true).ConfigureAwait(false);

            return await response.Content.DeserializeHttpContentAsync<T>().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<bool> PostAsync<T>(string api, T entity)
        {
            using var result = await this.GetResponseAsync(api, entity, HttpMethod.Post).ConfigureAwait(false);
            await result.ThrowIfErrorAsync(true).ConfigureAwait(false);
            var content = await result.Content.DeserializeHttpContentAsync().ConfigureAwait(false);
            return string.IsNullOrEmpty(content) || JsonConvert.DeserializeObject<bool>(content);
        }

        /// <inheritdoc />
        public async Task<bool> PostAsync(string api, object entity)
        {
            using var result = await this.GetResponseAsync(api, entity, HttpMethod.Post).ConfigureAwait(false);
            await result.ThrowIfErrorAsync(true).ConfigureAwait(false);
            var content = await result.Content.DeserializeHttpContentAsync().ConfigureAwait(false);
            return string.IsNullOrEmpty(content) || JsonConvert.DeserializeObject<bool>(content);
        }

        /// <inheritdoc />
        public async Task<T> PostAndGetResultAsync<T>(string api, T entity)
        {
            using var result = await this.GetResponseAsync(api, entity, HttpMethod.Post).ConfigureAwait(false);
            await result.ThrowIfErrorAsync(true).ConfigureAwait(false);
            return await result.Content.DeserializeHttpContentAsync<T>().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<TResult> PostAndGetResultAsync<TResult, T>(string api, T entity)
        {
            using var result = await this.GetResponseAsync(api, entity, HttpMethod.Post).ConfigureAwait(false);
            await result.ThrowIfErrorAsync(true).ConfigureAwait(false);
            return await result.Content.DeserializeHttpContentAsync<TResult>().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<bool> PutAsync<T>(string api, T entity)
        {
            using var result = await this.GetResponseAsync(api, entity, HttpMethod.Put).ConfigureAwait(false);
            await result.ThrowIfErrorAsync(true).ConfigureAwait(false);
            return result.IsSuccessStatusCode;
        }

        /// <inheritdoc />
        public async Task<TResult> PutAndGetResultAsync<TResult, T>(string api, T entity)
        {
            using var result = await this.GetResponseAsync(api, entity, HttpMethod.Put).ConfigureAwait(false);
            await result.ThrowIfErrorAsync(true).ConfigureAwait(false);
            return await result.Content.DeserializeHttpContentAsync<TResult>().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(string api)
        {
            using var response = await this.GetResponseAsync(api, HttpMethod.Delete).ConfigureAwait(false);
            await response.ThrowIfErrorAsync(false).ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }

        /// <inheritdoc />
        public async Task<TResult> DeleteAndGetResultAsync<TResult, T>(string api, T entity)
        {
            using var result = await this.GetResponseAsync(api, entity, HttpMethod.Delete).ConfigureAwait(false);
            await result.ThrowIfErrorAsync(true).ConfigureAwait(false);
            return await result.Content.DeserializeHttpContentAsync<TResult>().ConfigureAwait(false);
        }

        private async Task<HttpResponseMessage> GetResponseAsync(string api, object entity, HttpMethod method)
        {
            var uri = await this.BuildApiUriAsync(api).ConfigureAwait(false);

            using var content = new StringContent(JsonConvert.SerializeObject(entity, this.settings), Encoding.UTF8, "application/json");
            return await this.httpClientProxy.SendAsync(method, uri, content).ConfigureAwait(false);
        }

        private async Task<HttpResponseMessage> GetResponseAsync(string api, HttpMethod method)
        {
            var uri = await this.BuildApiUriAsync(api).ConfigureAwait(false);
            return await this.httpClientProxy.SendAsync(method, uri, null).ConfigureAwait(false);
        }

        private async Task<Uri> BuildApiUriAsync(string api)
        {
            if (Debugger.IsAttached)
            {
                return new Uri($"http://localhost:{PortFinder.Instance.PortNumber}/{api}");
            }

            var config = await this.configurationHandler.GetConfigurationAsync(ConfigurationConstants.ApiEndpoint).ConfigureAwait(false);
            return new Uri($"https://{config}/{api}");
        }
    }
}
