// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Scheduler.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Ownership
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Configuration.Entities;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;

    /// <summary>
    /// The Scheduler.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Functions.Core.FunctionBase" />
    public class Scheduler : FunctionBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<Scheduler> logger;

        /// <summary>
        /// The ownership rule processor.
        /// </summary>
        private readonly IOwnershipRuleProcessor ownershipRuleProcessor;

        /// <summary>
        /// The client.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scheduler" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="ownershipRuleProcessor">The ownership rule processor.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        public Scheduler(
            ITrueLogger<Scheduler> logger,
            IServiceProvider serviceProvider,
            IOwnershipRuleProcessor ownershipRuleProcessor,
            IAzureClientFactory azureClientFactory)
            : base(serviceProvider)
        {
            this.logger = logger;
            this.ownershipRuleProcessor = ownershipRuleProcessor;
            this.azureClientFactory = azureClientFactory;
        }

        /// <summary>
        /// Synchronizes the ownership rules scheduled asynchronous.
        /// </summary>
        /// <param name="timer">The timer.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("ScheduledSyncOwnershipRules")]
        public async Task ScheduledSyncOwnershipRulesAsync(
           [TimerTrigger("0 0 * * * *")]TimerInfo timer, ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(timer, nameof(timer));

            this.logger.LogInformation($"ScheduledSyncOwnershipRules function triggered with schedule: {timer.Schedule}", Constants.OwnershipRulesSync);

            try
            {
                await this.TryInitializeAsync().ConfigureAwait(false);
                this.logger.LogInformation($"The ownership rule scheduled synchronization job has started with schedule: {timer.Schedule}", Constants.OwnershipRulesSync);
                await this.ownershipRuleProcessor.QueueSyncOwnershipRuleAsync("System").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message, Constants.OwnershipRulesSync);
            }

            this.logger.LogInformation($"The ownership rule scheduled synchronization job has finished at: {DateTime.UtcNow.ToTrue()}", Constants.OwnershipRulesSync);
        }

        /// <summary>
        /// Synchronizes the ownership rules scheduled asynchronous.
        /// </summary>
        /// <param name="timer">The timer.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("ScheduledSyncAuditReports")]
        public async Task ScheduledSyncAuditReportsAsync(
           [TimerTrigger("0 0 0 * * *")]TimerInfo timer, ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(timer, nameof(timer));

            this.logger.LogInformation($"ScheduledSyncAuditReports function triggered with schedule: {timer.Schedule}", Constants.AuditReportsSync);

            try
            {
                await this.TryInitializeAsync().ConfigureAwait(false);
                this.logger.LogInformation($"The audit report scheduled synchronization job has started with schedule: {timer.Schedule}", Constants.AuditReportsSync);
                await this.azureClientFactory.AnalysisServiceClient.RefreshAuditReportsAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message, Constants.AuditReportsSync);
            }

            this.logger.LogInformation($"The ownership rule scheduled synchronization job has finished at: {DateTime.UtcNow.ToTrue()}", Constants.AuditReportsSync);
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

            var configurationHandler = this.Resolve<IConfigurationHandler>();

            var serviceBusSettings = await configurationHandler.GetConfigurationAsync<ServiceBusSettings>(ConfigurationConstants.ServiceBusSettings).ConfigureAwait(false);
            var analysisSettings = await configurationHandler.GetConfigurationAsync<AnalysisSettings>(ConfigurationConstants.AnalysisSettings).ConfigureAwait(false);
            var storageSettings = await configurationHandler.GetConfigurationAsync<StorageSettings>(ConfigurationConstants.StorageSettings).ConfigureAwait(false);

            this.azureClientFactory.Initialize(new AzureConfiguration(analysisSettings, storageSettings, serviceBusSettings));
        }
    }
}
