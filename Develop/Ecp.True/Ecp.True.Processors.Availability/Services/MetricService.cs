// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetricService.cs" company="Microsoft">
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
    using System.Linq;
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
    /// <seealso cref="Ecp.True.Processors.Availability.Interfaces.IMetricService" />
    public class MetricService : IMetricService
    {
        /// <summary>
        /// The failure metric response.
        /// </summary>
        private const string FailureMetricResponse = "{\"value\":[{\"errorCode\":\"Failure\"}]}";

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<MetricService> logger;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetricService" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        public MetricService(
            ITrueLogger<MetricService> logger,
            ITelemetry telemetry,
            IAzureClientFactory azureClientFactory)
        {
            this.logger = logger;
            this.telemetry = telemetry;
            this.azureClientFactory = azureClientFactory;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ResourceAvailability>> GetAvailabilityAsync(IEnumerable<ResourceDetails> notAvailableResources, AvailabilitySettings availabilitySettings)
        {
            ArgumentValidators.ThrowIfNull(notAvailableResources, nameof(notAvailableResources));
            ArgumentValidators.ThrowIfNull(availabilitySettings, nameof(availabilitySettings));
            var listOfTasks = new List<Task<ResourceAvailability>>();
            foreach (var item in notAvailableResources)
            {
                listOfTasks.Add(this.DoGetAvailabilityAsync(item, availabilitySettings));
            }

            return await Task.WhenAll(listOfTasks).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public void ReportAvailability(
            IEnumerable<ResourceAvailability> resourceAvailabilities, IEnumerable<ModuleAvailabilitySettings> moduleAvailabilitySettings, TimeSpan elapsedTimeSpan, bool isChaos)
        {
            ArgumentValidators.ThrowIfNull(moduleAvailabilitySettings, nameof(moduleAvailabilitySettings));
            ArgumentValidators.ThrowIfNull(resourceAvailabilities, nameof(resourceAvailabilities));
            foreach (var item in moduleAvailabilitySettings)
            {
                var unavailableResources = resourceAvailabilities.Where(a => item.Resources.Any(y => a.Id.Contains(y, StringComparison.OrdinalIgnoreCase)
                && (!a.AvailabilityState.EqualsIgnoreCase(Constants.AvailableState) || isChaos)));

                if (unavailableResources.Any())
                {
                    unavailableResources.ForEach(x =>
                    {
                        //// Logging the error.
                        var resourceName = item.Resources.FirstOrDefault(y => x.Id.Contains(y, StringComparison.OrdinalIgnoreCase));
                        this.logger.LogError(new Exception($"Availability status failed for resource {resourceName}"), x.Message + " Resource Id: " + x.Id);
                    });

                    this.telemetry.TrackAvailability(item.Name, false, elapsedTimeSpan, $"Reporting Availability for module {item.Name}");
                    continue;
                }

                this.telemetry.TrackAvailability(item.Name, true, elapsedTimeSpan, $"Reporting Availability for module {item.Name}");
            }
        }

        private async Task<ResourceAvailability> DoGetAvailabilityAsync(ResourceDetails item, AvailabilitySettings availabilitySettings)
        {
            this.logger.LogInformation($"Request metric availability for resource uri : {item}");
            var uri = availabilitySettings.BuildMetricUri(item.Id);
            string result;
            try
            {
                result = await this.azureClientFactory.AzureManagementApiClient.GetAsync(uri).ConfigureAwait(false);
                result ??= FailureMetricResponse;
            }
            catch (Exception ex)
            {
                result = FailureMetricResponse;
                var error = ex.InnerException ?? ex;
                this.logger.LogError(error, error.Message);
                this.telemetry.TrackEvent(Constants.Critical, EventName.MetricAvailabilityFailed.ToString("G"));
            }

            var token = JObject.Parse(result).SelectToken("value").ToString();
            var metricResponse = JsonConvert.DeserializeObject<IEnumerable<MetricResponse>>(
                token, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            var resourceAvailability = new ResourceAvailability();
            resourceAvailability.Id = item.Id;
            resourceAvailability.AvailabilityState = metricResponse.ElementAt(0).Status.EqualsIgnoreCase(Constants.SuccessStatus) ? Constants.AvailableState : Constants.UnavailableState;
            resourceAvailability.Message = metricResponse.ElementAt(0).Status.EqualsIgnoreCase(Constants.SuccessStatus) ?
                $"Resource : {item.Name} reported available from metric api." : $"Resource : {item.Name} reported unavailable from metric api";
            return resourceAvailability;
        }
    }
}