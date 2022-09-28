// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaProxy.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.OwnershipRules.Services
{
    using System.Threading.Tasks;

    using Ecp.True.Core;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.OwnershipRules.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.OfficialDeltaDataRequest;
    using Ecp.True.Proxies.OwnershipRules.Request;
    using Ecp.True.Proxies.OwnershipRules.Response;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// The delta proxy class.
    /// </summary>
    /// <seealso cref="Ecp.True.Proxies.OwnershipRules.Interfaces.IDeltaProxy" />
    public class DeltaProxy : IDeltaProxy
    {
        /// <summary>
        /// The ownership rule client.
        /// </summary>
        private readonly IOwnershipRuleClient ownershipRuleClient;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<DeltaProxy> logger;

        /// <summary>
        /// The ownership rule settings.
        /// </summary>
        private OwnershipRuleSettings ownershipRuleSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaProxy"/> class.
        /// </summary>
        /// <param name="ownershipRuleClient">The ownership rule client.</param>
        /// <param name="logger">The logger.</param>
        public DeltaProxy(
            IOwnershipRuleClient ownershipRuleClient,
            ITrueLogger<DeltaProxy> logger)
        {
            this.ownershipRuleClient = ownershipRuleClient;
            this.logger = logger;
        }

        /// <summary>
        /// Initializes the specified ownership rule settings.
        /// </summary>
        /// <param name="ownershipRuleSettings">The ownership rule settings.</param>
        public void Initialize(OwnershipRuleSettings ownershipRuleSettings)
        {
            this.ownershipRuleSettings = ownershipRuleSettings;
            this.ownershipRuleClient.Initialize(ownershipRuleSettings);
        }

        /// <summary>
        /// Processes the delta asynchronous.
        /// </summary>
        /// <param name="deltaRequest">The delta request.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>
        /// The DeltaResponse.
        /// </returns>
        public async Task<DeltaResponse> ProcessDeltaAsync(DeltaRequest deltaRequest, int ticketId)
        {
            this.logger.LogInformation($"Request payload for calling delta service for ticketId : {ticketId}");
            JObject payload = GenerateRequestpayload(deltaRequest);

            var result = await this.ownershipRuleClient.PostAsync(
                $"{this.ownershipRuleSettings.DeltaBasePath}/{this.ownershipRuleSettings.DeltaApiPath}",
                payload).ConfigureAwait(false);

            var resultContent = await result.Content.DeserializeHttpContentAsync().ConfigureAwait(false);

            var responseObject = JObject.Parse(resultContent);
            var token = responseObject.SelectToken("payload.payloadOutput");
            var tokenString = token.ToString();

            var deltaResponse = JsonConvert.DeserializeObject<DeltaResponse>(tokenString, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            deltaResponse.Content = resultContent;
            return deltaResponse;
        }

        /// <summary>
        /// Processes the delta asynchronous.
        /// </summary>
        /// <param name="deltaRequest">The delta request.</param>
        /// <returns>
        /// The Official Delta Response.
        /// </returns>
        public async Task<OfficialDeltaResponse> ProcessOfficialDeltaAsync(OfficialDeltaRequest deltaRequest)
        {
            JObject payload = GenerateRequestpayload(deltaRequest);

            var result = await this.ownershipRuleClient.PostAsync(
                $"{this.ownershipRuleSettings.OfficialDeltaBasePath}/{this.ownershipRuleSettings.OfficialDeltaApiPath}",
                payload).ConfigureAwait(false);

            var resultContent = await result.Content.DeserializeHttpContentAsync().ConfigureAwait(false);

            var responseObject = JObject.Parse(resultContent);
            var token = responseObject.SelectToken("payload.payloadOutput");
            var tokenString = token.ToString();

            var deltaResponse = JsonConvert.DeserializeObject<OfficialDeltaResponse>(tokenString, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            deltaResponse.Content = resultContent;
            return deltaResponse;
        }

        private static JObject GenerateRequestpayload(DeltaRequest deltaRequest)
        {
            var contractResolver = new IgnorableSerializerContractResolver();

            var serializer = new JsonSerializer { ContractResolver = contractResolver, ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var inner = JObject.FromObject(deltaRequest, serializer);

            return CreateRequestPayload(inner);
        }

        private static JObject GenerateRequestpayload(OfficialDeltaRequest deltaRequest)
        {
            var contractResolver = new IgnorableSerializerContractResolver();

            var serializer = new JsonSerializer { ContractResolver = contractResolver, ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var inner = JObject.FromObject(deltaRequest, serializer);

            return CreateRequestPayload(inner);
        }

        private static JObject CreateRequestPayload(JObject inner)
        {
            var outer = new JObject
            {
                { "payloadInput", inner },
            };

            var payload = new JObject
            {
                { "payload", outer },
            };
            return payload;
        }
    }
}
