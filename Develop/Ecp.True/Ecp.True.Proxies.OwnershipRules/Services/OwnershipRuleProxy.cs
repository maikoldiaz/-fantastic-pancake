// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipRuleProxy.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.OwnershipRules.Services
{
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.OwnershipRules.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.Request;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// The Ownership Rule Proxy.
    /// </summary>
    /// <seealso cref="Ecp.True.Proxies.OwnershipRules.Interfaces.IOwnershipRuleProxy" />
    public class OwnershipRuleProxy : IOwnershipRuleProxy
    {
        /// <summary>
        /// The ownership rule client.
        /// </summary>
        private readonly IOwnershipRuleClient ownershipRuleClient;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<OwnershipRuleProxy> logger;

        /// <summary>
        /// The ownership rule settings.
        /// </summary>
        private OwnershipRuleSettings ownershipRuleSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipRuleProxy"/> class.
        /// </summary>
        /// <param name="ownershipRuleClient">The ownership rule client.</param>
        /// <param name="logger">The logger.</param>
        public OwnershipRuleProxy(
            IOwnershipRuleClient ownershipRuleClient,
            ITrueLogger<OwnershipRuleProxy> logger)
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

        /// <inheritdoc/>
        public async Task<OwnershipRuleResponse> GetActiveRulesAsync()
        {
            return await this.RequestAsync(true).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<OwnershipRuleResponse> GetInactiveRulesAsync()
        {
            return await this.RequestAsync(false).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public Task<OwnershipRuleResponse> ProcessOwnershipAsync(OwnershipRuleRequest ownershipRuleRequest, int ticketId)
        {
            ArgumentValidators.ThrowIfNull(ownershipRuleRequest, nameof(ownershipRuleRequest));

            return this.RequestAsync(true, ownershipRuleRequest);
        }

        /// <summary>
        /// Generates the requestpayload.
        /// </summary>
        /// <param name="isActive">if set to <c>true</c> [is active].</param>
        /// <param name="ownershipRuleRequest">The ownership rule request.</param>
        /// <returns>The JObject.</returns>
        private static JObject GenerateRequestpayload(bool isActive, OwnershipRuleRequest ownershipRuleRequest = null)
        {
            var contractResolver = new IgnorableSerializerContractResolver();
            contractResolver.Ignore(typeof(PreviousInventoryOperationalData), nameof(PreviousInventoryOperationalData.IsOwnershipCalculated));
            contractResolver.Ignore(typeof(PreviousInventoryOperationalData), nameof(PreviousInventoryOperationalData.OwnershipPercentage));
            contractResolver.Ignore(typeof(PreviousInventoryOperationalData), nameof(PreviousMovementOperationalData.NetStandardVolume));
            contractResolver.Ignore(typeof(PreviousMovementOperationalData), nameof(PreviousMovementOperationalData.AppliedRule));
            contractResolver.Ignore(typeof(PreviousMovementOperationalData), nameof(PreviousMovementOperationalData.OwnershipPercentage));
            contractResolver.Ignore(typeof(PreviousMovementOperationalData), nameof(PreviousMovementOperationalData.NetStandardVolume));

            var serializer = new JsonSerializer { ContractResolver = contractResolver, ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var inner = ownershipRuleRequest != null ? JObject.FromObject(ownershipRuleRequest, serializer) : new JObject();
            inner.Add("tipoLlamada", ownershipRuleRequest == null ? "BUSCA_ESTRATEGIA" : "CALCULA_INVENTARIO");
            inner.Add("estrategiasActivas", isActive);

            var outer = new JObject
            {
                { "volInput", inner },
            };

            var payload = new JObject
            {
                { "volPayload", outer },
            };
            return payload;
        }

        /// <summary>
        /// Requests asynchronous.
        /// </summary>
        /// <param name="isActive">if set to <c>true</c> [is active].</param>
        /// <returns>the ownership rules.</returns>
        private async Task<OwnershipRuleResponse> RequestAsync(bool isActive, OwnershipRuleRequest ownershipRuleRequest = null)
        {
            JObject payload = GenerateRequestpayload(isActive, ownershipRuleRequest);

            if (ownershipRuleRequest != null)
            {
                ownershipRuleRequest.RawRequest = payload.ToString();
            }

            var result = await this.ownershipRuleClient.PostAsync(
                $"{this.ownershipRuleSettings.BasePath}/{this.ownershipRuleSettings.OwnershipRulePath}",
                payload).ConfigureAwait(false);

            var resultContent = await result.Content.DeserializeHttpContentAsync().ConfigureAwait(false);

            var responseObject = JObject.Parse(resultContent);
            var token = responseObject.SelectToken("volPayload.volOutput");
            var tokenString = token.ToString();

            var response = JsonConvert.DeserializeObject<OwnershipRuleResponse>(tokenString, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            this.LogAuditSteps(response);
            response.ResponseContent = resultContent;
            response.RequestContent = payload.ToString();
            return response;
        }

        private void LogAuditSteps(OwnershipRuleResponse response)
        {
            this.logger.LogInformation(Constants.OwnershipRulesSync, $"Audited Steps: {JsonConvert.SerializeObject(response?.AuditedSteps)}");
        }
    }
}
