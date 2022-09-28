// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AvailabilityGenerator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Availability
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Configuration.Entities;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Availability.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;

    /// <summary>
    /// The OwnershipGenerator.
    /// </summary>
    public class AvailabilityGenerator : FunctionBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<AvailabilityGenerator> logger;

        /// <summary>
        /// The availability processor.
        /// </summary>
        private readonly IAvailabilityProcessor availabilityProcessor;

        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvailabilityGenerator" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="availabilityProcessor">The availability processor.</param>
        /// <param name="telemetry">The telemetry.</param>
        public AvailabilityGenerator(
            ITrueLogger<AvailabilityGenerator> logger,
            IConfigurationHandler configurationHandler,
            IAzureClientFactory azureClientFactory,
            IServiceProvider serviceProvider,
            IAvailabilityProcessor availabilityProcessor,
            ITelemetry telemetry)
            : base(serviceProvider)
        {
            this.logger = logger;
            this.configurationHandler = configurationHandler;
            this.azureClientFactory = azureClientFactory;
            this.availabilityProcessor = availabilityProcessor;
            this.telemetry = telemetry;
        }

        /// <summary>
        /// Serviceses the availability asynchronous.
        /// </summary>
        /// <param name="timer">The timer.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("ServicesAvailability")]
        public async Task ServicesAvailabilityAsync(
           [TimerTrigger("%AvailabilityInterval%")] TimerInfo timer, ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(timer, nameof(timer));

            this.logger.LogInformation($"ServicesAvailability function triggered with schedule: {timer.Schedule}", Constants.ServicesAvailabilitySync);

            try
            {
                await this.TryInitializeAsync().ConfigureAwait(false);
                this.logger.LogInformation($"The service availability job has started with schedule: {timer.Schedule}", Constants.ServicesAvailabilitySync);
                var isChaos = TryEvaluate(Environment.GetEnvironmentVariable(FunctionNames.AvailabilityChaos));
                await this.availabilityProcessor.CheckAndReportAvailabilityAsync(isChaos).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var error = ex.InnerException ?? ex;
                this.logger.LogError(error, error.Message);
                this.telemetry.TrackEvent(Constants.Critical, EventName.ResourcesAvailabilityFailed.ToString("G"));
            }

            this.logger.LogInformation($"The service availability job has finished at: {DateTime.UtcNow.ToTrue()}", Constants.ServicesAvailabilitySync);
        }

        /// <summary>
        /// Does the initialize asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        protected override async Task DoInitializeAsync()
        {
            if (this.azureClientFactory.IsReady)
            {
                return;
            }

            var availabilitySettings = await this.configurationHandler.GetConfigurationAsync<AvailabilitySettings>(ConfigurationConstants.AvailabilitySettings).ConfigureAwait(false);
            var storageSettings = await this.configurationHandler.GetConfigurationAsync<StorageSettings>(ConfigurationConstants.StorageSettings).ConfigureAwait(false);
            var serviceBusSettings = await this.configurationHandler.GetConfigurationAsync<ServiceBusSettings>(ConfigurationConstants.ServiceBusSettings).ConfigureAwait(false);

            this.azureClientFactory.Initialize(new AzureConfiguration(availabilitySettings, storageSettings, serviceBusSettings));
        }

        private static bool TryEvaluate(string value)
        {
            return bool.TryParse(value, out bool result) && result;
        }
    }
}