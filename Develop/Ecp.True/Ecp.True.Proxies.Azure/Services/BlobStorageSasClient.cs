// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlobStorageSasClient.cs" company="Microsoft">
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
    using global::Azure.Storage.Blobs;

    /// <summary>
    /// I am so tired.
    /// </summary>
    [IoCRegistration(IoCLifetime.ContainerControlled)]
    [ExcludeFromCodeCoverage]
    public class BlobStorageSasClient : IBlobStorageSasClient
    {
        private readonly BlobClient blobClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobStorageSasClient" /> class with parameters.
        /// </summary>
        /// <param name="uri">The URI.</param>
        public BlobStorageSasClient(Uri uri)
        {
            this.blobClient = new BlobClient(uri);
        }

        /// <inheritdoc/>
        public Task CreateBlobAsync(Stream stream)
        {
            return this.blobClient.UploadAsync(stream, true);
        }

        /// <inheritdoc/>
        public async Task CreateBlobAsync(string content)
        {
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(content ?? string.Empty));
            await this.blobClient.UploadAsync(ms, true).ConfigureAwait(false); // Override the existing file.
        }

        /// <inheritdoc/>
        public async Task<Stream> GetCloudBlobStreamAsync()
        {
            var stream = new MemoryStream();
            await this.blobClient.DownloadToAsync(stream).ConfigureAwait(false);
            stream.Position = 0;
            return stream;
        }

        /// <inheritdoc/>
        public async Task<T> ParseAsync<T>()
        {
            using var stream = await this.GetCloudBlobStreamAsync().ConfigureAwait(false);
            return stream.DeserializeStream<T>();
        }

        /// <inheritdoc/>
        public Task DeleteBlobAsync()
        {
            return this.blobClient.DeleteIfExistsAsync();
        }
    }
}