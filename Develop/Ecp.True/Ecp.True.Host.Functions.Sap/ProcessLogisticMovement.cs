// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessLogisticMovement.cs" company="Microsoft">
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
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Sap.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;

    /// <summary>
    /// The Process Logistic Movements.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Functions.Core.FunctionBase" />
    public class ProcessLogisticMovement : FunctionBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<ProcessLogisticMovement> logger;

        /// <summary>
        /// The sap processor.
        /// </summary>
        private readonly ISapProcessor sapProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessLogisticMovement" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="sapProcessor">The sap processor.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        public ProcessLogisticMovement(
            ITrueLogger<ProcessLogisticMovement> logger,
            IServiceProvider serviceProvider,
            ISapProcessor sapProcessor)
            : base(serviceProvider)
        {
            this.logger = logger;
            this.sapProcessor = sapProcessor;
        }

        /// <summary>
        /// Process logistic movements asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="context">The context.</param>
        /// <returns>The Task.</returns>
        [FunctionName("ProcessLogisticMovement")]
        public async Task ProcessLogisticMovementsAsync(
            [ServiceBusTrigger("%SapLogistic%", Connection = "IntegrationServiceBusConnectionString", IsSessionsEnabled = true)] LogisticQueueMessage request,
            string label,
            string replyTo,
            ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(request, nameof(request));
            this.logger.LogInformation($"Process logistic movements from ticketId {request.TicketId}", $"{request.TicketId}");
            await this.TryInitializeAsync().ConfigureAwait(false);

            // Handle chaos
            this.ProcessMetadata(label, FunctionNames.ProcessLogisticMovement, replyTo);
            if (request.RequestType == SapRequestType.LogisticMovement)
            {
                await this.sapProcessor.ProcessLogisticMovementAsync(request).ConfigureAwait(false);
                this.logger.LogInformation($"Process logistic movements from ticketId: {request.TicketId}", $"{request.TicketId}");
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
            var storageSettings = await configurationHandler.GetConfigurationAsync<StorageSettings>(ConfigurationConstants.StorageSettings).ConfigureAwait(false);
            var serviceBusSettings = await configurationHandler.GetConfigurationAsync<ServiceBusSettings>(ConfigurationConstants.ServiceBusSettings).ConfigureAwait(false);

            azureClientFactory.Initialize(new AzureConfiguration(storageSettings, serviceBusSettings));
        }
    }
}