// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecalculateOwnershipBalance.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Configuration.Entities;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;

    /// <summary>
    /// The OwnershipGenerator.
    /// </summary>
    public class RecalculateOwnershipBalance : FunctionBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<RecalculateOwnershipBalance> logger;

        /// <summary>
        /// The ownership rule processor.
        /// </summary>
        private readonly IOwnershipRuleProcessor ownershipRuleProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecalculateOwnershipBalance" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="serviceProvider">The service provider.</param
        /// <param name="ownershipRuleProcessor">The ownership rule processor.</param>
        public RecalculateOwnershipBalance(
            ITrueLogger<RecalculateOwnershipBalance> logger,
            IServiceProvider serviceProvider,
            IOwnershipRuleProcessor ownershipRuleProcessor)
            : base(serviceProvider)
        {
            this.logger = logger;
            this.ownershipRuleProcessor = ownershipRuleProcessor;
        }

        /// <summary>
        /// Res the calculate ownership asynchronous.
        /// </summary>
        /// <param name="ticketId">The ownershipNode identifier.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("RecalculateOwnershipBalance")]
        public async Task RecalculateOwnershipBalanceAsync(
           [ServiceBusTrigger("%RecalculateOwnershipBalanceQueue%", Connection = "IntegrationServiceBusConnectionString")] int? ticketId,
           string label,
           string replyTo,
           ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(ticketId, nameof(ticketId));

            this.logger.LogInformation(
                $"Recalculate ownership balance processing is requested for ownership ticket {ticketId}", $"{ticketId}");
            try
            {
                await this.InitializeAsync().ConfigureAwait(false);
                this.ProcessMetadata(label, FunctionNames.Ownership, replyTo);

                this.logger.LogInformation($"Ownership recalculation balance is triggered for ticket {ticketId} with instance id {context.InvocationId}", $"{ticketId}");
                var ownershipRuleData = new OwnershipRuleData
                {
                    TicketId = (int)ticketId,
                    Errors = new List<ErrorInfo>(),
                };

                await this.ownershipRuleProcessor.ProcessAsync(ownershipRuleData, ChainType.CalculateOwnershipData).ConfigureAwait(false);
                await this.ownershipRuleProcessor.FinalizeProcessAsync(ownershipRuleData).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(
                    ex, $"Exception while processing ownership ticket: {ticketId} for re ownership recalculation balance.", $"{ticketId}");
            }
        }

        /// <summary>
        /// Does the initialize asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        protected override async Task DoInitializeAsync()
        {
            var azureClientFactory = this.Resolve<IAzureClientFactory>();
            if (azureClientFactory.IsReady)
            {
                return;
            }

            var configurationHandler = this.Resolve<IConfigurationHandler>();

            var analysisSettings = await configurationHandler.GetConfigurationAsync<AnalysisSettings>(ConfigurationConstants.AnalysisSettings).ConfigureAwait(false);
            var storageSettings = await configurationHandler.GetConfigurationAsync<StorageSettings>(ConfigurationConstants.StorageSettings).ConfigureAwait(false);
            var serviceBusSettings = await configurationHandler.GetConfigurationAsync<ServiceBusSettings>(ConfigurationConstants.ServiceBusSettings).ConfigureAwait(false);

            azureClientFactory.Initialize(new AzureConfiguration(analysisSettings, storageSettings, serviceBusSettings));
        }

        /// <summary>
        /// Initializes the context.
        /// </summary>
        private Task InitializeAsync()
        {
            return this.TryInitializeAsync();
        }
    }
}