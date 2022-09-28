// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBlobStorageSasClient.cs" company="Microsoft">
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

    /// <summary>
    /// The Blob Storage Client Interface.
    /// </summary>
    public interface IBlobStorageSasClient
    {
        /// <summary>
        /// Gets the cloud BLOB stream asynchronous.
        /// </summary>
        /// <returns>
        /// The task object.
        /// </returns>
        Task<Stream> GetCloudBlobStreamAsync();

        /// <summary>
        /// Creates the BLOB asynchronous.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task CreateBlobAsync(Stream stream);

        /// <summary>
        /// Creates the BLOB asynchronous.
        /// </summary>
        /// <param name="content">The message content.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task CreateBlobAsync(string content);

        /// <summary>
        /// Deletes the blob.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        Task DeleteBlobAsync();

        /// <summary>
        /// Gets the BLOB asynchronous.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The deserialized object.</returns>
        Task<T> ParseAsync<T>();
    }
}
