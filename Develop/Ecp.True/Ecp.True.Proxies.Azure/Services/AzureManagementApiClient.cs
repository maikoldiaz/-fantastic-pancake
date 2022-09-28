// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AzureManagementApiClient.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Azure
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Logging;

    /// <summary>
    /// The AzureManagementApi Client.
    /// </summary>
    /// <seealso cref="Ecp.True.Proxies.Azure.IAzureManagementApiClient" />
    [ExcludeFromCodeCoverage]
    [IoCRegistration(IoCLifetime.ContainerControlled)]
    public class AzureManagementApiClient : IAzureManagementApiClient
    {
        /// <summary>
        /// The HTTP client proxy.
        /// </summary>
        private readonly IHttpClientProxy httpClientProxy;

        /// <summary>
        /// The token provider.
        /// </summary>
        private readonly ITokenProvider tokenProvider;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<AzureManagementApiClient> logger;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        private AvailabilitySettings configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureManagementApiClient" /> class.
        /// </summary>
        /// <param name="httpClientProxy">The HTTP client proxy.</param>
        /// <param name="tokenProvider">The token provider.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="telemetry">The telemetry.</param>
        public AzureManagementApiClient(IHttpClientProxy httpClientProxy, ITokenProvider tokenProvider, ITrueLogger<AzureManagementApiClient> logger, ITelemetry telemetry)
        {
            this.httpClientProxy = httpClientProxy;
            this.tokenProvider = tokenProvider;
            this.logger = logger;
            this.telemetry = telemetry;
        }

        /// <inheritdoc/>
        public void Initialize(AvailabilitySettings configuration)
        {
            ArgumentValidators.ThrowIfNull(configuration, nameof(configuration));
            this.configuration = configuration;
        }

        /// <inheritdoc/>
        public async Task<string> GetAsync(Uri uri)
        {
            this.telemetry.StartTrackingMetric(LoggingConstants.AzureManagementApiTag, $"AzureManagementApiClient: GetAsync");
            var token = await this.GetTokenAsync().ConfigureAwait(false);
            var response = await this.httpClientProxy.SendAsync(
                HttpMethod.Get,
                uri,
                null,
                token).ConfigureAwait(false);
            this.LogResponse(response);
            this.telemetry.StartTrackingMetric(LoggingConstants.AzureManagementApiTag, $"AzureManagementApiClient: GetAsync");
            response.EnsureSuccessStatusCode();
            return await response.Content.DeserializeHttpContentAsync().ConfigureAwait(false);
        }

        private async Task<string> GetTokenAsync()
        {
            return await this.tokenProvider.GetAppTokenAsync(
                                                    this.configuration.TenantId,
                                                    this.configuration.Resource,
                                                    this.configuration.ClientId,
                                                    this.configuration.ClientSecret).ConfigureAwait(false);
        }

        private void LogResponse(HttpResponseMessage response)
        {
            if (response == null || !response.IsSuccessStatusCode)
            {
                this.logger.LogError(response?.ReasonPhrase, Constants.ServicesAvailabilitySync);
            }
        }
    }
}