// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalyticsClient.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Processors.Ownership.Calculation.Request;
    using Ecp.True.Processors.Ownership.Calculation.Response;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// The analytics client.
    /// </summary>
    public class AnalyticsClient : IAnalyticsClient
    {
        /// <summary>
        /// The HTTP client factory.
        /// </summary>
        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// The token provider.
        /// </summary>
        private readonly ITokenProvider tokenProvider;

        /// <summary>
        /// The analytics client path.
        /// </summary>
        private string analyticsClientPath;

        /// <summary>
        /// The indicator whether the response is dummy.
        /// </summary>
        private bool isDummy;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyticsClient" /> class.
        /// </summary>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="tokenProvider">The token provider.</param>
        public AnalyticsClient(IHttpClientFactory httpClientFactory, IConfigurationHandler configurationHandler, ITokenProvider tokenProvider)
        {
            this.httpClientFactory = httpClientFactory;
            this.configurationHandler = configurationHandler;
            this.tokenProvider = tokenProvider;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<AnalyticalServiceResponseData>> GetOwnershipAnalyticsAsync(AnalyticalServiceRequestData request)
        {
            ArgumentValidators.ThrowIfNull(request, nameof(request));
            await this.InitializeAsync().ConfigureAwait(false);

            if (this.isDummy)
            {
                return BuildDummyResponse(request);
            }

            var httpClient = await this.GetHttpClientAsync().ConfigureAwait(false);
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var jsonContent = JsonConvert.SerializeObject(request, settings);

            using var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri($"{this.analyticsClientPath}/ownership"));

            requestMessage.Headers.Accept.Clear();
            requestMessage.Content = content;

            using var response = await httpClient.SendAsync(requestMessage).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.DeserializeHttpContentAsync<IEnumerable<AnalyticalServiceResponseData>>().ConfigureAwait(false);
        }

        private static DateTime ParseToDateTime(string value)
        {
            return DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime)
                ? dateTime : DateTime.UtcNow.ToTrue();
        }

        private static IEnumerable<AnalyticalServiceResponseData> BuildDummyResponse(AnalyticalServiceRequestData request)
        {
            return new List<AnalyticalServiceResponseData>
                {
                    new AnalyticalServiceResponseData
                    {
                        DestinationNode = request.DestinationNode,
                        DestinationNodeType = request.DestinationNodeType,
                        MovementType = request.MovementType,
                        OperationalDate = ParseToDateTime(request.StartDate),
                        OwnershipPercentage = 0.98M,
                        SourceNode = request.SourceNode,
                        SourceNodeType = request.SourceNodeType,
                        SourceProduct = request.SourceProduct,
                        SourceProductType = request.SourceProductType,
                        TransferPoint = "CHICHIMENE",
                    },
                    new AnalyticalServiceResponseData
                    {
                        DestinationNode = request.DestinationNode,
                        DestinationNodeType = request.DestinationNodeType,
                        MovementType = request.MovementType,
                        OperationalDate = ParseToDateTime(request.EndDate),
                        OwnershipPercentage = 0.5M,
                        SourceNode = request.SourceNode,
                        SourceNodeType = request.SourceNodeType,
                        SourceProduct = request.SourceProduct,
                        SourceProductType = request.SourceProductType,
                        TransferPoint = "TRANSFER",
                    },
                };
        }

        private async Task InitializeAsync()
        {
            this.analyticsClientPath = await this.configurationHandler.GetConfigurationAsync<string>(ConfigurationConstants.AnalyticsClientPath).ConfigureAwait(false);
            this.isDummy = await this.configurationHandler.GetConfigurationAsync<bool>(ConfigurationConstants.DummyAnalyticsResponse).ConfigureAwait(false);
        }

        private async Task<HttpClient> GetHttpClientAsync()
        {
            var httpClient = this.httpClientFactory.CreateClient(Constants.DefaultHttpClient);

            var config = await this.configurationHandler.GetConfigurationAsync<AnalyticsSettings>(ConfigurationConstants.AnalyticsSettings).ConfigureAwait(false);
            var token = await this.tokenProvider.GetAppTokenAsync(config.TenantId, config.Scope, config.ClientId, config.ClientSecret).ConfigureAwait(false);

            httpClient.DefaultRequestHeaders.Authorization = token.ToBearer();

            return httpClient;
        }
    }
}
