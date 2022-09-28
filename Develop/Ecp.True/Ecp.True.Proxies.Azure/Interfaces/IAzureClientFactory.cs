// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAzureClientFactory.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using global::Azure.Storage.Sas;

    /// <summary>
    /// The Azure Client Factory Interface.
    /// </summary>
    public interface IAzureClientFactory
    {
        /// <summary>
        /// Gets a value indicating whether this instance is ready.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is ready; otherwise, <c>false</c>.
        /// </value>
        bool IsReady { get; }

        /// <summary>
        /// Gets the ethereum client.
        /// </summary>
        /// <value>
        /// The get ethereum client.
        /// </value>
        IEthereumClient EthereumClient { get; }

        /// <summary>
        /// Gets the analysis service client.
        /// </summary>
        /// <value>
        /// The analysis service client.
        /// </value>
        IAnalysisServiceClient AnalysisServiceClient { get; }

        /// <summary>
        /// Gets the azure management API client.
        /// </summary>
        /// <value>
        /// The azure management API client.
        /// </value>
        IAzureManagementApiClient AzureManagementApiClient { get; }

        /// <summary>
        /// Initializes the specified azure configuration.
        /// </summary>
        /// <param name="azureConfiguration">The azure configuration.</param>
        void Initialize(AzureConfiguration azureConfiguration);

        /// <summary>
        /// Gets the queue client.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns>
        /// Returns the service bus queue client.
        /// </returns>
        IServiceBusQueueClient GetQueueClient(string queueName);

        /// <summary>
        /// Gets the BLOB client.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <returns>The blob storage client.</returns>
        IBlobStorageClient GetBlobClient(string containerName);

        /// <summary>
        /// Gets the SAS token for Azure blob container and blob.
        /// </summary>
        /// <param name="containerName">container name.</param>
        /// <param name="blobName">blob name.</param>
        /// <returns>SaS query parameters.</returns>
        public IBlobStorageSasClient GetBlobStorageSaSClient(string containerName, string blobName);

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
        Task<FileAccessInfo> GetFileAccessInfoAsync(
            string containerName,
            string blobFileName,
            int accessExpiryTime,
            params BlobSasPermissions[] permissions);
    }
}
