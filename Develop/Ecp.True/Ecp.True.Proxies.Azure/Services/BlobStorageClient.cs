// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlobStorageClient.cs" company="Microsoft">
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
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Entities.Admin;

    using global::Azure.Storage.Blobs;
    using global::Azure.Storage.Sas;

    /// <summary>
    /// The Blob Storage Client service.
    /// </summary>
    [IoCRegistration(IoCLifetime.ContainerControlled)]
    [ExcludeFromCodeCoverage]
    public class BlobStorageClient : IBlobStorageClient
    {
        /// <summary>
        /// The BLOB client.
        /// </summary>
        private readonly BlobContainerClient blobContainerClient;

        /// <summary>
        /// The BLOB service client.
        /// </summary>
        private readonly BlobServiceClient blobServiceClient;

        /// <summary>
        /// The account name.
        /// </summary>
        private readonly string accountName;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobStorageClient" /> class.
        /// </summary>
        /// <param name="blobContainerClient">The blob container client.</param>
        /// <param name="blobServiceClient">The blob service client.</param>
        /// <param name="accountName">The account name.</param>
        public BlobStorageClient(BlobContainerClient blobContainerClient, BlobServiceClient blobServiceClient, string accountName)
        {
            this.blobContainerClient = blobContainerClient;
            this.accountName = accountName;
            this.blobServiceClient = blobServiceClient;
        }

        /// <inheritdoc/>
        public Task CreateBlobAsync(string blobName, Stream stream)
        {
            var blobClient = this.blobContainerClient.GetBlobClient(blobName);
            return blobClient.UploadAsync(stream, true);
        }

        /// <inheritdoc/>
        public async Task CreateBlobAsync(string blobName, string content)
        {
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(content ?? string.Empty));
            var blobClient = this.blobContainerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(ms, true).ConfigureAwait(false); // Override the existing file.
        }

        /// <inheritdoc/>
        public async Task<Stream> GetCloudBlobStreamAsync(string blobName)
        {
            var blobClient = this.blobContainerClient.GetBlobClient(blobName);
            var stream = new MemoryStream();
            await blobClient.DownloadToAsync(stream).ConfigureAwait(false);
            stream.Position = 0;
            return stream;
        }

        /// <inheritdoc/>
        public async Task<T> ParseAsync<T>(string blobPath)
        {
            using var stream = await this.GetCloudBlobStreamAsync(blobPath).ConfigureAwait(false);
            return stream.DeserializeStream<T>();
        }

        /// <inheritdoc/>
        public Task DeleteBlobAsync(string containerName, string blobPath)
        {
            return this.blobContainerClient.DeleteBlobIfExistsAsync(blobPath);
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
        public async Task<FileAccessInfo> GetSasTokenAsync(
            string containerName,
            string blobFileName,
            int accessExpiryTime,
            params BlobSasPermissions[] permissions)
        {
            var key = await this.blobServiceClient.GetUserDelegationKeyAsync(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddMinutes(accessExpiryTime)).ConfigureAwait(false);
            var builder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = blobFileName,
                Resource = blobFileName == null ? "c" : "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(accessExpiryTime),
            };

            permissions.ForEach(p => builder.SetPermissions(p));

            var sasToken = builder.ToSasQueryParameters(key, this.accountName).ToString();
            return new FileAccessInfo
            {
                AccountName = this.accountName,
                BlobPath = blobFileName,
                ContainerName = containerName,
                SasToken = sasToken,
            };
        }
    }
}
