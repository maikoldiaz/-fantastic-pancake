// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBlobStorageClient.cs" company="Microsoft">
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
    using System.IO;
    using System.Threading.Tasks;

    using Ecp.True.Entities.Admin;

    using global::Azure.Storage.Sas;

    /// <summary>
    /// The Blob Storage Client Interface.
    /// </summary>
    public interface IBlobStorageClient
    {
        /// <summary>
        /// Gets the cloud BLOB stream asynchronous.
        /// </summary>
        /// <param name="blobName">Name of the blob.</param>
        /// <returns>
        /// The task object.
        /// </returns>
        Task<Stream> GetCloudBlobStreamAsync(string blobName);

        /// <summary>
        /// Creates the BLOB asynchronous.
        /// </summary>
        /// <param name="blobName">Name of the BLOB.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task CreateBlobAsync(string blobName, Stream stream);

        /// <summary>
        /// Creates the BLOB asynchronous.
        /// </summary>
        /// <param name="blobName">Name of the BLOB.</param>
        /// <param name="content">The message content.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task CreateBlobAsync(string blobName, string content);

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
        Task<FileAccessInfo> GetSasTokenAsync(
            string containerName,
            string blobFileName,
            int accessExpiryTime,
            params BlobSasPermissions[] permissions);

        /// <summary>
        /// Deletes the blob.
        /// </summary>
        /// <param name="containerName">The container name.</param>
        /// <param name="blobPath">The blob path.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task DeleteBlobAsync(string containerName, string blobPath);

        /// <summary>
        /// Gets the BLOB asynchronous.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="blobPath">The BLOB path.</param>
        /// <returns>The deserialized object.</returns>
        Task<T> ParseAsync<T>(string blobPath);
    }
}