// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AzureClientFactory.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Azure
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
    using Ecp.True.Chaos.Interfaces;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Configuration.Entities;
    using global::Azure.Storage;
    using global::Azure.Storage.Blobs;
    using global::Azure.Storage.Sas;
    using Microsoft.Azure.ServiceBus;

    /// <summary>
    /// The Cloud Blob Client Factory.
    /// </summary>
    /// <seealso cref="Ecp.True.Proxies.Azure.IAzureClientFactory" />
    [IoCRegistration(IoCLifetime.Hierarchical)]
    public class AzureClientFactory : IAzureClientFactory
    {
        /// <summary>
        /// The queue clients.
        /// </summary>
        private static readonly ConcurrentDictionary<string, QueueClient> QueueClients = new ConcurrentDictionary<string, QueueClient>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The queue clients.
        /// </summary>
        private static readonly ConcurrentDictionary<string, BlobContainerClient> StorageClients = new ConcurrentDictionary<string, BlobContainerClient>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The chaos manager.
        /// </summary>
        private readonly IChaosManager chaosManager;

        /// <summary>
        /// The deadletter manager.
        /// </summary>
        private readonly IDeadLetterManager deadLetterManager;

        /// <summary>
        /// The service bus settings.
        /// </summary>
        private ServiceBusSettings serviceBusSettings;

        /// <summary>
        /// The storage settings.
        /// </summary>
        private StorageSettings storageSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureClientFactory" /> class.
        /// </summary>
        /// <param name="ethereumClient">The ethereum client.</param>
        /// <param name="analysisServiceClient">The analysis service client.</param>
        /// <param name="chaosManager">The chaos manager.</param>
        /// <param name="deadLetterManager">The deadletter manager.</param>
        /// <param name="azureManagementApiClient">The azure management api client.</param>
        public AzureClientFactory(
            IEthereumClient ethereumClient,
            IAnalysisServiceClient analysisServiceClient,
            IChaosManager chaosManager,
            IDeadLetterManager deadLetterManager,
            IAzureManagementApiClient azureManagementApiClient)
        {
            this.EthereumClient = ethereumClient;
            this.AnalysisServiceClient = analysisServiceClient;
            this.chaosManager = chaosManager;
            this.deadLetterManager = deadLetterManager;
            this.AzureManagementApiClient = azureManagementApiClient;
        }

        /// <inheritdoc/>
        public IEthereumClient EthereumClient { get; }

        /// <inheritdoc/>
        public IAnalysisServiceClient AnalysisServiceClient { get; }

        /// <inheritdoc/>
        public IAzureManagementApiClient AzureManagementApiClient { get; }

        /// <inheritdoc/>
        public bool IsReady { get; private set; }

        /// <inheritdoc/>
        public void Initialize(AzureConfiguration azureConfiguration)
        {
            ArgumentValidators.ThrowIfNull(azureConfiguration, nameof(azureConfiguration));
            ArgumentValidators.ThrowIfNull(azureConfiguration.ServiceBusSettings, nameof(azureConfiguration.ServiceBusSettings));
            ArgumentValidators.ThrowIfNull(azureConfiguration.StorageSettings, nameof(azureConfiguration.StorageSettings));

            if (azureConfiguration.QuorumProfile != null)
            {
                this.EthereumClient.Initialize(azureConfiguration.QuorumProfile);
            }

            if (azureConfiguration.AnalysisSettings != null)
            {
                this.AnalysisServiceClient.Initialize(azureConfiguration.AnalysisSettings);
            }

            if (azureConfiguration.AvailabilitySettings != null)
            {
                this.AzureManagementApiClient.Initialize(azureConfiguration.AvailabilitySettings);
            }

            this.serviceBusSettings = azureConfiguration.ServiceBusSettings;
            this.storageSettings = azureConfiguration.StorageSettings;

            this.IsReady = true;
        }

        /// <inheritdoc/>
        public IServiceBusQueueClient GetQueueClient(string queueName)
        {
            if (!QueueClients.TryGetValue(queueName, out QueueClient client))
            {
                var tokenProvider = new ManagedIdentityTokenProvider(this.serviceBusSettings.TenantId);
                var policy = new RetryExponential(
                    TimeSpan.FromSeconds(this.serviceBusSettings.MinimumBackOff),
                    TimeSpan.FromSeconds(this.serviceBusSettings.MaximumBackOff),
                    this.serviceBusSettings.MaximumRetryCount);
                client = new QueueClient(this.serviceBusSettings.Resource, queueName, tokenProvider, TransportType.Amqp, ReceiveMode.PeekLock, policy);
                QueueClients.TryAdd(queueName, client);
            }

            return new ServiceBusQueueClient(client, this.chaosManager, this.deadLetterManager);
        }

        /// <inheritdoc/>
        public IBlobStorageClient GetBlobClient(string containerName)
        {
            var options = new BlobClientOptions();
            options.Retry.MaxDelay = TimeSpan.FromSeconds(this.storageSettings.DeltaBackOff);
            options.Retry.MaxRetries = this.storageSettings.MaxAttempts;
            var serviceClient = new BlobServiceClient(
                this.storageSettings.Resource,
                new ManagedIdentityTokenCredential(this.serviceBusSettings.TenantId),
                options);
            if (!StorageClients.TryGetValue(containerName, out BlobContainerClient client))
            {
                client = serviceClient.GetBlobContainerClient(containerName);
                StorageClients.TryAdd(containerName, client);
            }

            return new BlobStorageClient(client, serviceClient, this.storageSettings.AccountName);
        }

        /// <summary>
        /// Gets the SAS token for Azure blob container and blob.
        /// </summary>
        /// <param name="containerName">container name.</param>
        /// <param name="blobName">blob name.</param>
        /// <returns>SaS query parameters.</returns>
        public IBlobStorageSasClient GetBlobStorageSaSClient(string containerName, string blobName)
        {
            ArgumentValidators.ThrowIfNullOrEmpty(containerName, nameof(containerName));
            ArgumentValidators.ThrowIfNullOrEmpty(blobName, nameof(blobName));

            string accountName = this.storageSettings.AccountName;
            var blobSasBuilder = new BlobSasBuilder
            {
                StartsOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddMinutes(60),
                BlobContainerName = containerName,
                BlobName = blobName,
            };
            blobSasBuilder.SetPermissions(BlobSasPermissions.All);
            var storageSharedKeyCredential = new StorageSharedKeyCredential(this.storageSettings.AccountName, this.storageSettings.StorageAppKey);
            BlobSasQueryParameters sasQueryParameters = blobSasBuilder.ToSasQueryParameters(storageSharedKeyCredential);
            UriBuilder fullUri = new UriBuilder
            {
                Scheme = "https",
                Host = $"{accountName}.blob.core.windows.net",
                Path = $"{containerName}/{blobName}",
                Query = sasQueryParameters.ToString(),
            };

            var blobStorageSasClient = new BlobStorageSasClient(fullUri.Uri);
            return blobStorageSasClient;
        }

        /// <summary>
        /// Gets the BLOB sas URI asynchronous.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobFileName">Name of the BLOB file.</param>
        /// <param name="accessExpiryTime">The access expiry time.</param>
        /// <param name="permissions">The permissions.</param>
        /// <returns>
        /// The sas token.
        /// </returns>
        public async Task<FileAccessInfo> GetFileAccessInfoAsync(
            string containerName,
            string blobFileName,
            int accessExpiryTime,
            params BlobSasPermissions[] permissions)
        {
            string accountname = this.storageSettings.AccountName;
            var builder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = blobFileName,
                Resource = blobFileName == null ? "c" : "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(accessExpiryTime),
            };

            permissions.ForEach(p => builder.SetPermissions(p));
            var storageSharedKeyCredential = new StorageSharedKeyCredential(accountname, this.storageSettings.StorageAppKey);
            BlobSasQueryParameters sasQueryParameters = builder.ToSasQueryParameters(storageSharedKeyCredential);
            string sasToken = sasQueryParameters.ToString();
            await Task.Delay(new TimeSpan(1)).ConfigureAwait(true);
            return new FileAccessInfo
            {
                AccountName = accountname,
                BlobPath = blobFileName,
                ContainerName = containerName,
                SasToken = sasToken,
            };
        }
    }
}
