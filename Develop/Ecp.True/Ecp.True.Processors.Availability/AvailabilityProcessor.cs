// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AvailabilityProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Processors.Availability
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Availability.Interfaces;
    using Ecp.True.Processors.Availability.Response;

    /// <summary>
    /// Availability Processor.
    /// </summary>
    public class AvailabilityProcessor : IAvailabilityProcessor
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<AvailabilityProcessor> logger;

        /// <summary>
        /// The resource service.
        /// </summary>
        private readonly IResourceService resourceService;

        /// <summary>
        /// The metric service.
        /// </summary>
        private readonly IMetricService metricService;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvailabilityProcessor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="resourceService">The resource service.</param>
        /// <param name="metricService">The metric service.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        public AvailabilityProcessor(
            ITrueLogger<AvailabilityProcessor> logger,
            IResourceService resourceService,
            IMetricService metricService,
            IConfigurationHandler configurationHandler)
        {
            this.logger = logger;
            this.resourceService = resourceService;
            this.metricService = metricService;
            this.configurationHandler = configurationHandler;
        }

        /// <inheritdoc />
        public async Task CheckAndReportAvailabilityAsync(bool isChaos)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            this.logger.LogInformation(Constants.CheckAvailabilityAsync);

            //// Get all the availability settings.
            var configuration = await this.configurationHandler.GetConfigurationAsync<AvailabilitySettings>(ConfigurationConstants.AvailabilitySettings).ConfigureAwait(false);
            if (configuration == null)
            {
                this.logger.LogInformation(Constants.CheckAvailabilitySettingsAsync);
                return;
            }

            //// Get all the resource details.
            var resources = await this.resourceService.GetResourcesAsync(configuration).ConfigureAwait(false);

            //// Get all the availability of the resources from resource service
            var resourcesAvailability = await this.resourceService.GetAvailabilityAsync(configuration).ConfigureAwait(false);

            //// Get all the module config resources
            var moduleConfigResources = await this.configurationHandler.GetCollectionConfigurationAsync<ModuleAvailabilitySettings>(
                ConfigurationConstants.ModuleAvailability).ConfigureAwait(false);

            if (moduleConfigResources.Any())
            {
                //// Get all the distinct module config resources
                var distinctModuleConfigResources = moduleConfigResources.SelectMany(x => x.Resources).Distinct();

                //// Get all the resources which are not in availability.
                var notInAvailability = distinctModuleConfigResources.Where(x => !resourcesAvailability.Any(y => y.Id.Contains(x, StringComparison.OrdinalIgnoreCase)));

                if (notInAvailability.Any())
                {
                    //// Get all the resources which are not in availability from resource details.
                    var notAvailableResources = notInAvailability.SelectMany(x => resources.Where(y => y.Name.EqualsIgnoreCase(x))).ToList();
                    if (notAvailableResources.Any())
                    {
                        //// Calls the resource metric service and add into resourcesAvailability collections.
                        var result = await this.metricService.GetAvailabilityAsync(notAvailableResources, configuration).ConfigureAwait(false);
                        ((List<ResourceAvailability>)resourcesAvailability).AddRange(result);
                    }
                }
            }

            if (resourcesAvailability.Any() && moduleConfigResources.Any())
            {
                stopwatch.Stop();
                //// Logs the report availability.
                this.metricService.ReportAvailability(resourcesAvailability, moduleConfigResources, stopwatch.Elapsed, isChaos);
            }
        }
    }
}