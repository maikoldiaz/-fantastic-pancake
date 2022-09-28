// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapMappingSynchronizer.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Sap
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Configuration.Entities;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Sap.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;

    /// <summary>
    /// The SAP PO Node Synchronizer.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Functions.Core.FunctionBase" />
    public class SapMappingSynchronizer : FunctionBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<SapMappingSynchronizer> logger;

        /// <summary>
        /// The SAP processor.
        /// </summary>
        private readonly ISapProcessor sapProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SapMappingSynchronizer"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="sapProcessor">The sap processor.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        public SapMappingSynchronizer(
            ITrueLogger<SapMappingSynchronizer> logger,
            IServiceProvider serviceProvider,
            ISapProcessor sapProcessor)
            : base(serviceProvider)
        {
            this.logger = logger;
            this.sapProcessor = sapProcessor;
        }

        /// <summary>
        /// Synchronizes the Nodes information scheduled asynchronous.
        /// </summary>
        /// <param name="timer">The timer.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("SyncSapMappings")]
        public async Task SyncSapMappingsAsync([TimerTrigger("0 0 * * * *")] TimerInfo timer, ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(timer, nameof(timer));
            this.logger.LogInformation($"SyncSapMappings function triggered with schedule: {timer.Schedule}", Constants.SapMappingSync);
            try
            {
                await this.TryInitializeAsync().ConfigureAwait(false);
                this.logger.LogInformation($"The SyncSapMappings scheduled synchronization job has started with schedule: {timer.Schedule}", Constants.SapMappingSync);
                await this.sapProcessor.SyncAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message, Constants.SapMappingSync);
            }

            this.logger.LogInformation($"The SyncSapMappings scheduled synchronization job has finished at: {DateTime.UtcNow.ToTrue()}", Constants.SapMappingSync);
        }

        /// <summary>
        /// Does the initialize asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        protected override async Task DoInitializeAsync()
        {
            var configurationHandler = this.Resolve<IConfigurationHandler>();
            var azureClientFactory = this.Resolve<IAzureClientFactory>();

            if (azureClientFactory.IsReady)
            {
                return;
            }

            var serviceBusSettings = await configurationHandler.GetConfigurationAsync<ServiceBusSettings>(ConfigurationConstants.ServiceBusSettings).ConfigureAwait(false);
            var analysisSettings = await configurationHandler.GetConfigurationAsync<AnalysisSettings>(ConfigurationConstants.AnalysisSettings).ConfigureAwait(false);
            var storageSettings = await configurationHandler.GetConfigurationAsync<StorageSettings>(ConfigurationConstants.StorageSettings).ConfigureAwait(false);

            azureClientFactory.Initialize(new AzureConfiguration(analysisSettings, storageSettings, serviceBusSettings));
        }
    }
}
