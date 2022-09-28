﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainRegistrar.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>

namespace Ecp.True.Host.Functions.Blockchain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Configuration.Entities;
    using Ecp.True.Entities.Core;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Blockchain.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;

    /// <summary>
    /// The azure function service for canonical transformation and homologation.
    /// </summary>
    public class BlockchainRegistrar : FunctionBase
    {
        /// <summary>
        /// The blockchain service provider.
        /// </summary>
        private readonly IBlockchainServiceProvider serviceProvider;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<BlockchainRegistrar> logger;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockchainRegistrar" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="provider">The provider.</param>
        public BlockchainRegistrar(IBlockchainServiceProvider serviceProvider, ITrueLogger<BlockchainRegistrar> logger, ITelemetry telemetry, IServiceProvider provider)
            : base(provider)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
            this.telemetry = telemetry;
        }

        /// <summary>
        /// Transforms the movement asynchronous.
        /// </summary>
        /// <param name="message">The movement queue message.</param>
        /// <param name="context">The context.</param>
        /// <param name="label">The label.</param>
        /// <returns>
        /// The task.
        ///// </returns>
        [FunctionName("RegisterMovement")]
        public async Task RegisterMovementAsync(
            [ServiceBusTrigger("%BlockchainMovement%", Connection = "IntegrationServiceBusConnectionString", IsSessionsEnabled = true)] int movementTransactionId,
            string label,
            ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            await this.TryInitializeAsync().ConfigureAwait(false);
            this.ProcessMetadata(label, FunctionNames.Blockchain, null);

            this.logger.LogInformation($"Blockchain registration for movement {movementTransactionId} started.", $"{movementTransactionId}");

            var service = await this.serviceProvider.GetBlockchainServiceAsync(ServiceType.Movement);
            await service.RegisterAsync(movementTransactionId).ConfigureAwait(false);

            this.logger.LogInformation($"Blockchain registration for movement {movementTransactionId} finished.", $"{movementTransactionId}");
        }

        /// <summary>
        /// Transforms the loss asynchronous.
        /// </summary>
        /// <param name="message">The inventory queue message.</param>
        /// <param name="context">The context.</param>
        /// <param name="label">The label.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("RegisterInventoryProduct")]
        public async Task RegisterInventoryProductAsync(
            [ServiceBusTrigger("%BlockchainInventoryProduct%", Connection = "IntegrationServiceBusConnectionString", IsSessionsEnabled = true)] int inventoryProductId,
            string label,
            ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            await this.TryInitializeAsync().ConfigureAwait(false);
            this.ProcessMetadata(label, FunctionNames.Blockchain, null);

            this.logger.LogInformation($"Blockchain registration for inventory product {inventoryProductId} started.", $"{inventoryProductId}");

            var service = await this.serviceProvider.GetBlockchainServiceAsync(ServiceType.InventoryProduct);
            await service.RegisterAsync(inventoryProductId).ConfigureAwait(false);

            this.logger.LogInformation($"Blockchain registration for inventory product {inventoryProductId} finished.", $"{inventoryProductId}");
        }

        /// <summary>
        /// Registers the owner asynchronous.
        /// </summary>
        /// <param name="ownerId">The ownerId identifier.</param>
        /// <param name="label">The label.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("RegisterOwner")]
        public async Task RegisterOwnerAsync(
            [ServiceBusTrigger("%BlockchainOwner%", Connection = "IntegrationServiceBusConnectionString", IsSessionsEnabled = true)] int ownerId,
            string label,
            ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            await this.TryInitializeAsync().ConfigureAwait(false);
            this.ProcessMetadata(label, FunctionNames.Blockchain, null);

            this.logger.LogInformation($"Blockchain registration for owner {ownerId} started.", $"{ownerId}");

            var service = await this.serviceProvider.GetBlockchainServiceAsync(ServiceType.Owner);
            await service.RegisterAsync(ownerId).ConfigureAwait(false);

            this.logger.LogInformation($"Blockchain registration for owner {ownerId} finished.", $"{ownerId}");
        }

        /// <summary>
        /// Registers the movement ownership asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("RegisterOwnership")]
        public async Task RegisterMovementOwnershipAsync(
            [ServiceBusTrigger("%BlockchainOwnership%", Connection = "IntegrationServiceBusConnectionString", IsSessionsEnabled = true)] int ownershipId,
            string label,
            ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            await this.TryInitializeAsync().ConfigureAwait(false);
            this.ProcessMetadata(label, FunctionNames.Blockchain, null);

            this.logger.LogInformation($"Blockchain registration for ownership {ownershipId} started.", $"{ownershipId}");

            var service = await this.serviceProvider.GetBlockchainServiceAsync(ServiceType.Ownership);
            await service.RegisterAsync(ownershipId).ConfigureAwait(false);

            this.logger.LogInformation($"Blockchain registration for ownership {ownershipId} finished.", $"{ownershipId}");
        }

        /// <summary>
        /// Registers the balance calculations asynchronous.
        /// </summary>
        /// <param name="nodeProductCalculationsMessage">The node product calculations message.</param>
        /// <param name="context">The context.</param>
        /// <param name="label">The label.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("RegisterBalance")]
        public async Task RegisterBalanceAsync(
           [ServiceBusTrigger("%BlockchainNodeProductCalculation%", Connection = "IntegrationServiceBusConnectionString", IsSessionsEnabled = true)] int unbalanceId,
           string label,
           ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            this.logger.LogInformation($"Node product calculation registration for unbalance {unbalanceId} started.", $"{unbalanceId}");

            await this.TryInitializeAsync().ConfigureAwait(false);
            this.ProcessMetadata(label, FunctionNames.Blockchain, null);

            var service = await this.serviceProvider.GetBlockchainServiceAsync(ServiceType.Unbalance);
            await service.RegisterAsync(unbalanceId).ConfigureAwait(false);

            this.logger.LogInformation($"Node product calculation registration for unbalance {unbalanceId} finished.", $"{unbalanceId}");
        }

        /// <summary>
        /// Registers the node asynchronous.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="label">The label.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("RegisterNode")]
        public async Task RegisterNodeAsync(
           [ServiceBusTrigger("%BlockchainNode%", Connection = "IntegrationServiceBusConnectionString", IsSessionsEnabled = true)] int nodeId,
           string label,
           ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            await this.TryInitializeAsync().ConfigureAwait(false);
            this.ProcessMetadata(label, FunctionNames.Blockchain, null);

            this.logger.LogInformation($"Blockchain registration for node {nodeId} started.", $"{nodeId}");

            var service = await this.serviceProvider.GetBlockchainServiceAsync(ServiceType.Node);
            await service.RegisterAsync(nodeId).ConfigureAwait(false);

            this.logger.LogInformation($"Blockchain registration for node {nodeId} finished.", $"{nodeId}");
        }

        /// <summary>
        /// Registers the node asynchronous.
        /// </summary>
        /// <param name="deltaNodeId">The delta node identifier.</param>
        /// <param name="label">The label.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("DeltaNodeApproval")]
        public async Task DeltaNodeApprovalAsync(
           [ServiceBusTrigger("%BlockchainOfficial%", Connection = "IntegrationServiceBusConnectionString", IsSessionsEnabled = true)] int deltaNodeId,
           string label,
           ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(deltaNodeId, nameof(deltaNodeId));

            await this.TryInitializeAsync().ConfigureAwait(false);
            this.ProcessMetadata(label, FunctionNames.Blockchain, null);
            this.logger.LogInformation($"Blockchain DeltaNodeApproval for node {deltaNodeId} started.", $"{deltaNodeId}");
            var service = await this.serviceProvider.GetBlockchainServiceAsync(ServiceType.OfficialMovement);
            await service.RegisterAsync(deltaNodeId).ConfigureAwait(false);
            this.logger.LogInformation($"Blockchain DeltaNodeApproval for node {deltaNodeId} finished.", $"{deltaNodeId}");
        }

        /// <summary>
        /// Registers the node asynchronous.
        /// </summary>
        /// <param name = "adminMessage" > The admin message.</param>
        /// <param name = "context" > The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("RegisterNodeConnection")]
        public async Task RegisterNodeConnectionAsync(
           [ServiceBusTrigger("%BlockchainNodeConnection%", Connection = "IntegrationServiceBusConnectionString", IsSessionsEnabled = true)] int nodeConnectionId,
           string label,
           ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            await this.TryInitializeAsync().ConfigureAwait(false);
            this.ProcessMetadata(label, FunctionNames.Blockchain, null);

            this.logger.LogInformation($"Blockchain registration for node connection {nodeConnectionId} started.", $"{nodeConnectionId}");

            var service = await this.serviceProvider.GetBlockchainServiceAsync(ServiceType.NodeConnection);
            await service.RegisterAsync(nodeConnectionId).ConfigureAwait(false);

            this.logger.LogInformation($"Blockchain registration for node connection {nodeConnectionId} finished.", $"{nodeConnectionId}");
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
            var blockchainConfiguration = await configurationHandler.GetConfigurationAsync<BlockchainSettings>(ConfigurationConstants.BlockchainSettings).ConfigureAwait(false);

            var azureConfiguration = new AzureConfiguration(new QuorumProfile
            {
                Address = blockchainConfiguration.EthereumAccountAddress,
                PrivateKey = blockchainConfiguration.EthereumAccountKey,
                RpcEndpoint = blockchainConfiguration.RpcEndpoint,
                ClientId = blockchainConfiguration.ClientId,
                ClientSecret = blockchainConfiguration.ClientSecret,
                TenantId = blockchainConfiguration.TenantId,
                ResourceId = blockchainConfiguration.ResourceId,
            }, storageSettings, serviceBusSettings);

            azureClientFactory.Initialize(azureConfiguration);
        }
    }
}