// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Registrar.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Transform
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Configuration.Entities;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Transform.Services.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;

    /// <summary>
    /// The Registrar.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Functions.Core.FunctionBase" />
    public class Registrar : FunctionBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<Registrar> logger;

        /// <summary>
        /// The registration processor.
        /// </summary>
        private readonly IRegistrationProcessor registrationProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="Registrar" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="registrationProcessor">The registration processor.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public Registrar(
            ITrueLogger<Registrar> logger,
            IRegistrationProcessor registrationProcessor,
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            this.logger = logger;
            this.registrationProcessor = registrationProcessor;
        }

        /// <summary>
        /// Registers the movement asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="label">The label.</param>
        /// <param name="context">The context.</param>
        /// <returns>The task.</returns>
        [FunctionName("RegisterMovement")]
        public async Task RegisterMovementAsync(
            [ServiceBusTrigger("%HomologatedMovements%", Connection = "IntegrationServiceBusConnectionString", IsSessionsEnabled = true)]FileRegistrationTransaction message,
            string label,
            ExecutionContext context)
        {
            await this.ProcessAsync(context, message, MessageType.Movement, this.registrationProcessor.RegisterMovementAsync, label).ConfigureAwait(false);
        }

        /// <summary>
        /// Registers the inventory asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="label">The label.</param>
        /// <param name="context">The context.</param>
        /// <returns>The task.</returns>
        [FunctionName("RegisterInventory")]
        public async Task RegisterInventoryAsync(
          [ServiceBusTrigger("%HomologatedInventory%", Connection = "IntegrationServiceBusConnectionString", IsSessionsEnabled = true)]FileRegistrationTransaction message,
          string label,
          ExecutionContext context)
        {
            await this.ProcessAsync(context, message, MessageType.Inventory, this.registrationProcessor.RegisterInventoryAsync, label).ConfigureAwait(false);
        }

        /// <summary>
        /// Registers the event asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="label">The label.</param>
        /// <param name="context">The context.</param>
        /// <returns>The task.</returns>
        [FunctionName("RegisterEvent")]
        public async Task RegisterEventAsync(
           [ServiceBusTrigger("%HomologatedEvents%", Connection = "IntegrationServiceBusConnectionString", IsSessionsEnabled = true)]FileRegistrationTransaction message,
           string label,
           ExecutionContext context)
        {
            await this.ProcessAsync(context, message, MessageType.Events, this.registrationProcessor.RegisterEventAsync, label).ConfigureAwait(false);
        }

        /// <summary>
        /// Registers the contract asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="label">The label.</param>
        /// <param name="context">The context.</param>
        /// <returns>The task.</returns>
        [FunctionName("RegisterContract")]
        public async Task RegisterContractAsync(
           [ServiceBusTrigger("%HomologatedContracts%", Connection = "IntegrationServiceBusConnectionString", IsSessionsEnabled = true)]FileRegistrationTransaction message,
           string label,
           ExecutionContext context)
        {
            await this.ProcessAsync(context, message, MessageType.Contract, this.registrationProcessor.RegisterContractAsync, label).ConfigureAwait(false);
        }

        /// <summary>
        /// Does the initialize asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        protected override async Task DoInitializeAsync()
        {
            var azureClientFactory = this.Resolve<IAzureClientFactory>();
            var configurationHandler = this.Resolve<IConfigurationHandler>();

            if (azureClientFactory.IsReady)
            {
                return;
            }

            var storageSettings = await configurationHandler.GetConfigurationAsync<StorageSettings>(ConfigurationConstants.StorageSettings).ConfigureAwait(false);
            var serviceBusSettings = await configurationHandler.GetConfigurationAsync<ServiceBusSettings>(ConfigurationConstants.ServiceBusSettings).ConfigureAwait(false);

            var azureConfiguration = new AzureConfiguration(storageSettings, serviceBusSettings);
            azureClientFactory.Initialize(azureConfiguration);
        }

        /// <summary>
        /// Processes the asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        /// <param name="processor">The processor.</param>
        /// <param name="label">The label.</param>
        private async Task ProcessAsync(ExecutionContext context, FileRegistrationTransaction message, MessageType type, Func<FileRegistrationTransaction, Task> processor, string label)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            await this.InitializeAsync().ConfigureAwait(false);
            this.ProcessMetadata(label, FunctionNames.Transform, null);

            this.logger.LogInformation($"Message with {type} Id: {message.SessionId}.");

            await processor(message).ConfigureAwait(false);

            this.logger.LogInformation($"Message with {type} Id: {message.SessionId}.");
        }

        private Task InitializeAsync()
        {
            return this.TryInitializeAsync();
        }
    }
}
