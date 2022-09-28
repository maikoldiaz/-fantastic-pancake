// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipRuleClient.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.OwnershipRules
{
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.ExceptionHandling.Core;
    using Ecp.True.ExceptionHandling.Entities;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.OwnershipRules.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;
    using Constants = Ecp.True.Core.Constants;

    /// <summary>
    /// Analysis Service Client.
    /// </summary>
    /// <seealso cref="OwnershipRuleClient" />
    public class OwnershipRuleClient : IOwnershipRuleClient
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<OwnershipRuleClient> logger;

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
        /// Initializes a new instance of the <see cref="OwnershipRuleClient" /> class.
        /// </summary>
        /// <param name="httpClientProxy">The HTTP client proxy.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="retryPolicyFactory">The retry policy factory.</param>
        /// <param name="retryHandler">The retry handler.</param>
        /// <param name="telemetry">The telemetry.</param>
        public OwnershipRuleClient(IHttpClientProxy httpClientProxy, ITrueLogger<OwnershipRuleClient> logger, IRetryPolicyFactory retryPolicyFactory, IRetryHandler retryHandler, ITelemetry telemetry)
        {
            this.httpClientProxy = httpClientProxy;
            this.logger = logger;
            this.retryPolicyFactory = retryPolicyFactory;
            this.retryHandler = retryHandler;
            this.settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            this.telemetry = telemetry;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public OwnershipRuleSettings Configuration { get; private set; }

        /// <inheritdoc/>
        public void Initialize(OwnershipRuleSettings configuration)
        {
            ArgumentValidators.ThrowIfNull(configuration, nameof(configuration));
            this.Configuration = configuration;

            var retrySettings = new RetrySettings
            {
                RetryCount = 3,
                RetryIntervalInSeconds = 10,
                RetryStrategy = RetryStrategy.FixedInterval,
            };

            this.retryPolicy = this.retryPolicyFactory.GetRetryPolicy("OwnershipRule", retrySettings, this.retryHandler);
        }

        /// <inheritdoc/>
        public async Task<HttpResponseMessage> PostAsync(string path, object payload)
        {
            var uri = new Uri(path);
            var token = await this.GenerateTokenAsync(
                this.Configuration.RegistrationPath,
                this.Configuration.ClientId,
                this.Configuration.ClientSecret,
                this.Configuration.TimeoutInMinutes).ConfigureAwait(false);

            return await this.retryPolicy.ExecuteWithRetryAsync(
            async () =>
            {
                using (var content = new StringContent(JsonConvert.SerializeObject(payload, this.settings), Encoding.UTF8, "application/json"))
                {
                    this.logger.LogInformation($"Calling ownership rule service: {uri}", Constants.OwnershipRulesSync);

                    this.telemetry.TrackMetric(LoggingConstants.OwnershipTag, $"OwnershipRuleClient: PostAsync", 0);
                    var stopwatch = Stopwatch.StartNew();
                    var response =
                    await this.httpClientProxy.SendAsync(HttpMethod.Post, uri, content, token, this.Configuration.IsCompressed, this.Configuration.TimeoutInMinutes).ConfigureAwait(false);
                    stopwatch.Stop();
                    this.LogResponse(response, stopwatch.ElapsedMilliseconds);
                    this.telemetry.TrackMetric(LoggingConstants.OwnershipTag, $"OwnershipRuleClient: PostAsync", stopwatch.ElapsedMilliseconds);
                    response.EnsureSuccessStatusCode();
                    return response;
                }
            }).ConfigureAwait(false);
        }

        private void LogResponse(HttpResponseMessage response, long responseTime)
        {
            if (response == null || !response.IsSuccessStatusCode)
            {
                this.logger.LogError(response?.ReasonPhrase, Constants.OwnershipRulesSync);
                return;
            }

            this.logger.LogInformation($"Time taken (in milliseconds) to get response from ownership rule service: {responseTime}ms", Constants.OwnershipRulesSync);
        }

        private async Task<string> GenerateTokenAsync(string registrationPath, string clientId, string clientSecret, int timeoutInMinutes)
        {
            var authEndpoint = new Uri(registrationPath);
            var payLoad = new JObject();
            payLoad.Add("clientId", clientId);
            payLoad.Add("secret", clientSecret);

            this.logger.LogInformation($"Calling the endpoint to fetch token: {authEndpoint}", Constants.OwnershipRulesSync);

            return await this.retryPolicy.ExecuteWithRetryAsync(
            async () =>
            {
                using (var content = new StringContent(JsonConvert.SerializeObject(payLoad, this.settings), Encoding.UTF8, "application/json"))
                {
                    var stopwatch = Stopwatch.StartNew();
                    using (var response = await this.httpClientProxy.SendAsync(
                        HttpMethod.Post, authEndpoint, content, string.Empty, false, timeoutInMinutes).ConfigureAwait(false))
                    {
                        stopwatch.Stop();
                        this.LogResponse(response, stopwatch.ElapsedMilliseconds);

                        response.EnsureSuccessStatusCode();
                        return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    }
                }
            }).ConfigureAwait(false);
        }
    }
}