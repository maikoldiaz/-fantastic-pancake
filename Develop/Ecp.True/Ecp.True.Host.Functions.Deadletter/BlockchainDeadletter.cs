// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainDeadletter.cs" company="Microsoft">
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
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Configuration.Entities;
    using Ecp.True.Entities.Core.Dto;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Deadletter.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;
    using Newtonsoft.Json;

    /// <summary>
    /// The BlockchainDeadletter.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Functions.Core.FunctionBase" />
    public class BlockchainDeadletter : FunctionBase
    {
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        private readonly ITrueLogger<BlockchainDeadletter> logger;

        /// <summary>
        /// The Deadletter processor.
        /// </summary>
        private readonly IDeadletterProcessor deadletterProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockchainDeadletter" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="failureHandlerFactory">The failureHandlerFactory.</param>
        /// <param name="unitOfWorkFactory">The unit of work.</param>
        /// <param name="deadletterProcessor">The deadletter processor.</param>
        public BlockchainDeadletter(
            IServiceProvider serviceProvider,
            ITrueLogger<BlockchainDeadletter> logger,
            IFailureHandlerFactory failureHandlerFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            IDeadletterProcessor deadletterProcessor)
            : base(serviceProvider)
        {
            ArgumentValidators.ThrowIfNull(failureHandlerFactory, nameof(failureHandlerFactory));
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));

            this.logger = logger;
            this.deadletterProcessor = deadletterProcessor;
        }

        /// <summary>
        /// Registers the movement deadlettering.
        /// </summary>
        /// <param name="message">The message.</param>
        [FunctionName("RegisterMovementDeadlettering")]
        public void RegisterMovementDeadlettering(
            [ServiceBusTrigger("%BlockchainMovement%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] string message)
        {
            string errorMessage = $"Deadletter processing for Queue {QueueConstants.BlockchainMovementQueue}.Failed to register movement in blockchain for {message}";
            this.logger.LogError(new Exception(errorMessage), errorMessage);
        }

        /// <summary>
        /// Registers the inventory product deadlettering.
        /// </summary>
        /// <param name="message">The message.</param>
        [FunctionName("RegisterInventoryProductDeadlettering")]
        public void RegisterInventoryProductDeadlettering(
            [ServiceBusTrigger("%BlockchainInventoryProduct%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] string message)
        {
            string errorMessage = $"Deadletter processing for Queue {QueueConstants.BlockchainInventoryProductQueue}.Failed to register inventory product in blockchain for {message}";
            this.logger.LogError(new Exception(errorMessage), errorMessage);
        }

        /// <summary>
        /// Registers the owner deadlettering.
        /// </summary>
        /// <param name="message">The message.</param>
        [FunctionName("RegisterOwnerDeadlettering")]
        public void RegisterOwnerDeadlettering(
            [ServiceBusTrigger("%BlockchainOwner%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] string message)
        {
            string errorMessage = $"Deadletter processing for Queue {QueueConstants.BlockchainOwnerQueue}.Failed to register owner in blockchain for {message}";
            this.logger.LogError(new Exception(errorMessage), errorMessage);
        }

        /// <summary>
        /// Registers the ownership deadlettering.
        /// </summary>
        /// <param name="message">The message.</param>
        [FunctionName("RegisterOwnershipDeadlettering")]
        public void RegisterOwnershipDeadlettering(
            [ServiceBusTrigger("%BlockchainOwnership%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] string message)
        {
            string errorMessage = $"Deadletter processing for Queue {QueueConstants.BlockchainOwnershipQueue}.Failed to register ownership in blockchain for {message}";
            this.logger.LogError(new Exception(errorMessage), errorMessage);
        }

        /// <summary>
        /// Registers the node deadlettering.
        /// </summary>
        /// <param name="message">The message.</param>
        [FunctionName("RegisterNodeDeadlettering")]
        public void RegisterNodeDeadlettering(
            [ServiceBusTrigger("%BlockchainNode%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] string message)
        {
            string errorMessage = $"Deadletter processing for Queue {QueueConstants.BlockchainNodeQueue}.Failed to register node in blockchain for {message}";
            this.logger.LogError(new Exception(errorMessage), errorMessage);
        }

        /// <summary>
        /// Registers the node connection deadlettering.
        /// </summary>
        /// <param name="message">The message.</param>
        [FunctionName("RegisterNodeConnectionDeadlettering")]
        public void RegisterNodeConnectionDeadlettering(
            [ServiceBusTrigger("%BlockchainNodeConnection%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] string message)
        {
            string errorMessage = $"Deadletter processing for Queue {QueueConstants.BlockchainNodeConnectionQueue}.Failed to register node connection in blockchain for {message}";
            this.logger.LogError(new Exception(errorMessage), errorMessage);
        }

        /// <summary>
        /// Processes the sap deadlettering asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The task.</returns>
        [FunctionName("OffchainDeadlettering")]
        public async Task ProcessOffchainDeadletteringAsync(
        [ServiceBusTrigger("%OffchainQueue%/$DeadLetterQueue", Connection = "IntegrationServiceBusConnectionString")] string message)
        {
            try
            {
                var offchainMessage = JsonConvert.DeserializeObject<OffchainMessage>(message);
                this.logger.LogInformation($"Deadlettering {offchainMessage.Type:G} offchain message: {offchainMessage.EntityId}");

                await this.TryInitializeAsync().ConfigureAwait(false);
                this.deadletterProcessor.HandleOffchainFailure(offchainMessage);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Exceptions in deadletter offchain message. {ex.Message}");
            }
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

            var storageSettings = await configurationHandler.GetConfigurationAsync<StorageSettings>(ConfigurationConstants.StorageSettings).ConfigureAwait(false);
            var serviceBusSettings = await configurationHandler.GetConfigurationAsync<ServiceBusSettings>(ConfigurationConstants.ServiceBusSettings).ConfigureAwait(false);

            azureClientFactory.Initialize(new AzureConfiguration(storageSettings, serviceBusSettings));
        }
    }
}
