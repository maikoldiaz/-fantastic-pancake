// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainReconciler.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Deadletter
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Configuration.Entities;
    using Ecp.True.Entities.Core.Dto;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Deadletter.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;

    /// <summary>
    /// The Blockchain Reconciler function.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Functions.Core.FunctionBase" />
    public class BlockchainReconciler : FunctionBase
    {
        /// <summary>
        /// The reconciler.
        /// </summary>
        private readonly IReconcileService reconciler;

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        private readonly ITrueLogger<BlockchainReconciler> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockchainReconciler" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="reconciler">The reconcileService.</param>
        /// <param name="logger">The logger.</param>
        public BlockchainReconciler(IServiceProvider serviceProvider, IReconcileService reconciler, ITrueLogger<BlockchainReconciler> logger)
            : base(serviceProvider)
        {
            this.reconciler = reconciler;
            this.logger = logger;
        }

        /// <summary>
        /// Reconciles the blockchain messages.
        /// </summary>
        /// <param name="timer">The timer.</param>
        /// <param name="context">The context.</param>
        /// <returns>The task.</returns>
        [FunctionName("ScheduledReconciler")]
        public async Task ReconcileAsync([TimerTrigger("%ReconcilerInterval%")]TimerInfo timer, ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(timer, nameof(timer));

            this.logger.LogInformation($"Scheduled Reconciliation function triggered with schedule: {timer.Schedule}", Constants.Reconciliation);

            try
            {
                await this.TryInitializeAsync().ConfigureAwait(false);
                this.logger.LogInformation($"The reconciliation job has started with schedule: {timer.Schedule}", Constants.Reconciliation);

                await this.reconciler.ReconcileAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message, Constants.Reconciliation);
            }

            this.logger.LogInformation($"The reconciliation job has finished at: {DateTime.UtcNow.ToTrue()}", Constants.Reconciliation);
        }

        /// <summary>
        /// Reconciles the blockchain messages.
        /// </summary>
        /// <param name="timer">The timer.</param>
        /// <param name="context">The context.</param>
        /// <returns>The task.</returns>
        [FunctionName("ScheduledFailureReconciler")]
        public async Task ReconcileFailureAsync([TimerTrigger("%ReconcilerInterval%")] TimerInfo timer, ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(timer, nameof(timer));

            this.logger.LogInformation($"Scheduled failure reconciliation function triggered with schedule: {timer.Schedule}", Constants.FailureReconciliation);

            try
            {
                await this.TryInitializeAsync().ConfigureAwait(false);
                this.logger.LogInformation($"The failure reconciliation job has started with schedule: {timer.Schedule}", Constants.FailureReconciliation);

                await this.reconciler.ReconcileFailureAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message, Constants.Reconciliation);
            }

            this.logger.LogInformation($"The failure reconciliation job has finished at: {DateTime.UtcNow.ToTrue()}", Constants.FailureReconciliation);
        }

        /// <summary>
        /// Reconciles the offchain messages.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="label">The label.</param>
        /// <param name="context">The context.</param>
        /// <returns>The task.</returns>
        [FunctionName("OffchainReconciler")]
        public async Task ReconcileOffchainAsync(
            [ServiceBusTrigger("%OffchainQueue%", Connection = "IntegrationServiceBusConnectionString")] OffchainMessage message,
            string label,
            ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(message, nameof(message));

            this.logger.LogInformation($"Reconciliation triggered for: {message.Type} with Id: {message.EntityId}", Constants.Reconciliation);

            await this.TryInitializeAsync().ConfigureAwait(false);
            this.ProcessMetadata(label, FunctionNames.Deadletter, null);

            await this.reconciler.ReconcileAsync(message).ConfigureAwait(false);

            this.logger.LogInformation($"The reconciliation job has finished at: {DateTime.UtcNow.ToTrue()}", Constants.Reconciliation);
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

            var analysisSettings = await configurationHandler.GetConfigurationAsync<AnalysisSettings>(ConfigurationConstants.AnalysisSettings).ConfigureAwait(false);
            var storageSettings = await configurationHandler.GetConfigurationAsync<StorageSettings>(ConfigurationConstants.StorageSettings).ConfigureAwait(false);
            var serviceBusSettings = await configurationHandler.GetConfigurationAsync<ServiceBusSettings>(ConfigurationConstants.ServiceBusSettings).ConfigureAwait(false);

            var blockchainConfiguration = await configurationHandler.GetConfigurationAsync<BlockchainSettings>(ConfigurationConstants.BlockchainSettings).ConfigureAwait(false);

            var azureConfiguration = new AzureConfiguration(
                new QuorumProfile
            {
                Address = blockchainConfiguration.EthereumAccountAddress,
                PrivateKey = blockchainConfiguration.EthereumAccountKey,
                RpcEndpoint = blockchainConfiguration.RpcEndpoint,
                ClientId = blockchainConfiguration.ClientId,
                ClientSecret = blockchainConfiguration.ClientSecret,
                TenantId = blockchainConfiguration.TenantId,
                ResourceId = blockchainConfiguration.ResourceId,
            },
                analysisSettings,
                storageSettings,
                serviceBusSettings);

            azureClientFactory.Initialize(azureConfiguration);
        }
    }
}