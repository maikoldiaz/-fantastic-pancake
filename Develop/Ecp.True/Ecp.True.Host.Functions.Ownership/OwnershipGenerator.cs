// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipGenerator.cs" company="Microsoft">
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
    using Ecp.True.Entities.Dto;
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
    public class OwnershipGenerator : FunctionBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<OwnershipGenerator> logger;

        /// <summary>
        /// The ownership rule processor.
        /// </summary>
        private readonly IOwnershipRuleProcessor ownershipRuleProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipGenerator" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="ownershipCalculationService">The ownership calculation service.</param>
        /// <param name="dataGeneratorService">The data generator service.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="excelService">The excel service.</param>
        /// <param name="ownershipRuleProcessor">The ownership rule processor.</param>
        public OwnershipGenerator(
            ITrueLogger<OwnershipGenerator> logger,
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
        /// <param name="recalculateOwnershipMessage">The ownershipNode identifier.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("ReCalculateOwnershipData")]
        public async Task ReCalculateOwnershipAsync(
           [ServiceBusTrigger("%CalculateOwnershipQueue%", Connection = "IntegrationServiceBusConnectionString")]RecalculateOwnershipMessage recalculateOwnershipMessage,
           string label,
           string replyTo,
           ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(recalculateOwnershipMessage, nameof(recalculateOwnershipMessage));

            this.logger.LogInformation(
                $"Recalculate ownership processing is requested for ownership nodeid {recalculateOwnershipMessage.OwnershipNodeId}", $"{recalculateOwnershipMessage.OwnershipNodeId}");
            try
            {
                await this.InitializeAsync().ConfigureAwait(false);
                this.ProcessMetadata(label, FunctionNames.Ownership, replyTo);

                // Get ownership ticket id
                var ticketId = await this.ownershipRuleProcessor.GetOwnershipTicketByOwnershipNodeIdAsync(recalculateOwnershipMessage.OwnershipNodeId).ConfigureAwait(false);

                this.logger.LogInformation($"Ownership recalculation is triggered for ticket {ticketId} with instance id {context.InvocationId}", $"{ticketId}");
                var ownershipRuleData = new OwnershipRuleData
                {
                    TicketId = ticketId,
                    OwnershipNodeId = recalculateOwnershipMessage.OwnershipNodeId,
                    Errors = new List<ErrorInfo>(),
                    HasDeletedMovementOwnerships = recalculateOwnershipMessage.HasDeletedMovementOwnerships,
                };

                await this.ownershipRuleProcessor.ProcessAsync(ownershipRuleData, ChainType.CalculateOwnershipData).ConfigureAwait(false);
                await this.ownershipRuleProcessor.FinalizeProcessAsync(ownershipRuleData).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(
                    ex, $"Exception while processing ownership nodeid: {recalculateOwnershipMessage.OwnershipNodeId} for re ownership calculation.", $"{recalculateOwnershipMessage.OwnershipNodeId}");
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