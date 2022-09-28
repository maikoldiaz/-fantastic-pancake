// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapClient.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Sap.Services
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.ExceptionHandling.Core;
    using Ecp.True.ExceptionHandling.Entities;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Sap.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Constants = Ecp.True.Core.Constants;

    /// <summary>
    /// Analysis Service Client.
    /// </summary>
    /// <seealso cref="SapClient" />
    public class SapClient : ISapClient
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<SapClient> logger;

        /// <summary>
        /// The HTTP client proxy.
        /// </summary>
        private readonly IHttpClientProxy httpClientProxy;

        /// <summary>
        /// The settings.
        /// </summary>
        private readonly JsonSerializerSettings settings;

        /// <summary>
        /// The retry policy factory.
        /// </summary>
        private readonly IRetryPolicyFactory retryPolicyFactory;

        /// <summary>
        /// The retry handler.
        /// </summary>
        private readonly IRetryHandler retryHandler;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// The retry policy.
        /// </summary>
        private IRetryPolicy retryPolicy;

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        private SapSettings configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SapClient" /> class.
        /// </summary>
        /// <param name="httpClientProxy">The HTTP client proxy.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="retryPolicyFactory">The retry policy factory.</param>
        /// <param name="retryHandler">The retry handler.</param>
        /// <param name="telemetry">The telemetry.</param>
        public SapClient(IHttpClientProxy httpClientProxy, ITrueLogger<SapClient> logger, IRetryPolicyFactory retryPolicyFactory, IRetryHandler retryHandler, ITelemetry telemetry)
        {
            this.httpClientProxy = httpClientProxy;
            this.logger = logger;
            this.retryPolicyFactory = retryPolicyFactory;
            this.retryHandler = retryHandler;
            this.settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            this.telemetry = telemetry;
        }

        /// <inheritdoc/>
        public void Initialize(SapSettings configuration)
        {
            ArgumentValidators.ThrowIfNull(configuration, nameof(configuration));
            this.configuration = configuration;

            var retrySettings = new RetrySettings
            {
                RetryCount = configuration.RetryCount,
                RetryIntervalInSeconds = configuration.RetryInterval,
                RetryStrategy = RetryStrategy.Exponential,
            };

            this.retryPolicy = this.retryPolicyFactory.GetRetryPolicy("Sap", retrySettings, this.retryHandler);
        }

        /// <inheritdoc/>
        public async Task<HttpResponseMessage> PostAsync(string path, object payload)
        {
            return await this.retryPolicy.ExecuteWithRetryAsync(
            async () =>
            {
                using (var content = new StringContent(JsonConvert.SerializeObject(payload, this.settings), Encoding.UTF8, "application/json"))
                {
                    this.telemetry.StartTrackingMetric(LoggingConstants.SapTag, $"SapClient: PostAsync");
                    var response = await this.httpClientProxy.SendAsync(
                        HttpMethod.Post,
                        this.BuildServiceUri(path),
                        content,
                        this.configuration.UserName,
                        this.configuration.Password).ConfigureAwait(false);
                    this.LogResponse(response);
                    this.telemetry.StopTrackingMetric(LoggingConstants.SapTag, $"SapClient: PostAsync");
                    return response;
                }
            }).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<HttpResponseMessage> GetAsync(string path)
        {
            return await this.retryPolicy.ExecuteWithRetryAsync(
            async () =>
            {
                this.telemetry.StartTrackingMetric(LoggingConstants.SapTag, $"SapClient: GetAsync");
                var response = await this.httpClientProxy.SendAsync(
                    HttpMethod.Get,
                    this.BuildServiceUri(path),
                    null,
                    this.configuration.UserName,
                    this.configuration.Password).ConfigureAwait(false);
                this.LogResponse(response);
                this.telemetry.StartTrackingMetric(LoggingConstants.SapTag, $"SapClient: GetAsync");
                response.EnsureSuccessStatusCode();
                return response;
            }).ConfigureAwait(false);
        }

        private void LogResponse(HttpResponseMessage response)
        {
            if (response == null || !response.IsSuccessStatusCode)
            {
                this.logger.LogError(response?.ReasonPhrase, Constants.SapSync);
            }
        }

        /// <summary>
        /// Builds the service URI.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Returns Uri.</returns>
        private Uri BuildServiceUri(string path)
        {
            return new Uri($"{this.configuration.BasePath}/{path}");
        }
    }
}