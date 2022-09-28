// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapNotifier.cs" company="Microsoft">
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
    /// The Sap Notifier.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Functions.Core.FunctionBase" />
    public class SapNotifier : FunctionBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<SapNotifier> logger;

        /// <summary>
        /// The sap status processor.
        /// </summary>
        private readonly ISapStatusProcessor sapStatusProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SapNotifier" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="sapProcessor">The sap processor.</param>
        /// <param name="sapStatusProcessor">The sap status processor.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        public SapNotifier(
            ITrueLogger<SapNotifier> logger,
            IServiceProvider serviceProvider,
            ISapStatusProcessor sapStatusProcessor)
            : base(serviceProvider)
        {
            this.logger = logger;
            this.sapStatusProcessor = sapStatusProcessor;
        }

        /// <summary>
        /// Updates the transfer point asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="context">The context.</param>
        /// <returns>The Task.</returns>
        [FunctionName("UpdateTransferPoint")]
        public async Task UpdateTransferPointAsync(
            [ServiceBusTrigger("%Sap%", Connection = "IntegrationServiceBusConnectionString", IsSessionsEnabled = true)] SapQueueMessage request,
            string label,
            string replyTo,
            ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(request, nameof(request));
            this.logger.LogInformation($"Update TransferPoint is requested for messageId {request.MessageId}", $"{request.MessageId}");
            await this.TryInitializeAsync().ConfigureAwait(false);

            // Handle chaos
            this.ProcessMetadata(label, FunctionNames.Sap, replyTo);
            if (request.RequestType != SapRequestType.Movement)
            {
                await this.sapStatusProcessor.TryUploadStatusAsync(request.UploadId).ConfigureAwait(false);
                this.logger.LogInformation($"Update Upload status is triggered for uploadId: {request.UploadId}", $"{request.UploadId}");
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