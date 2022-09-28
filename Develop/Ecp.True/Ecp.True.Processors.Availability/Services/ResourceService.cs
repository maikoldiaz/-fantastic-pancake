// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Processors.Availability.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Availability.Interfaces;
    using Ecp.True.Processors.Availability.Response;
    using Ecp.True.Proxies.Azure;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Resource Service.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Availability.Interfaces.IResourceService" />
    public class ResourceService : IResourceService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<ResourceService> logger;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceService" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        public ResourceService(
            ITrueLogger<ResourceService> logger,
            IAzureClientFactory azureClientFactory)
        {
            this.logger = logger;
            this.azureClientFactory = azureClientFactory;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ResourceDetails>> GetResourcesAsync(AvailabilitySettings availabilitySettings)
        {
            ArgumentValidators.ThrowIfNull(availabilitySettings, nameof(availabilitySettings));
            var finalCollection = new List<ResourceDetails>();

            foreach (var item in availabilitySettings.ResourceGroups)
            {
                this.logger.LogInformation($"Request resource details for resource group name : {item}");
                var uri = availabilitySettings.BuildResourceDetailUri(item);
                var result = await this.azureClientFactory.AzureManagementApiClient.GetAsync(uri).ConfigureAwait(false);
                var token = JObject.Parse(result).SelectToken("value").ToString();
                var resourceResponse = JsonConvert.DeserializeObject<IEnumerable<ResourceDetails>>(
                    token, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                finalCollection.AddRange(resourceResponse);
            }

            return finalCollection;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ResourceAvailability>> GetAvailabilityAsync(AvailabilitySettings availabilitySettings)
        {
            ArgumentValidators.ThrowIfNull(availabilitySettings, nameof(availabilitySettings));
            var finalCollection = new List<ResourceAvailability>();

            foreach (var item in availabilitySettings.ResourceGroups)
            {
                this.logger.LogInformation($"Request availability for resource group name : {item}");
                var uri = availabilitySettings.BuildAvailabilityUri(item);
                var result = await this.azureClientFactory.AzureManagementApiClient.GetAsync(uri).ConfigureAwait(false);
                var token = JObject.Parse(result).SelectToken("value").ToString();
                var resourceResponse = JsonConvert.DeserializeObject<IEnumerable<AvailabilityResponse>>(
                    token, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

                resourceResponse.ForEach(x =>
                {
                    var resourceAvailability = new ResourceAvailability();
                    resourceAvailability.Id = x.Id;
                    resourceAvailability.AvailabilityState = x.Properties.AvailabilityState;
                    resourceAvailability.Message = x.Properties.Summary;
                    finalCollection.Add(resourceAvailability);
                });
            }

            return finalCollection;
        }
    }
}