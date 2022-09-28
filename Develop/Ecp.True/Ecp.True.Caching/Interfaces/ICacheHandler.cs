// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICacheHandler.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Caching
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Distributed;

    /// <summary>
    /// ICache handler.
    /// </summary>
    /// <typeparam name="T">T Type.</typeparam>
    public interface ICacheHandler<T>
    {
        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="region">The region.</param>
        /// <returns>The Task.</returns>
        Task<T> GetAsync(string key, string region);

        /// <summary>
        /// Sets the asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="region">The region.</param>
        /// <returns>The Task.</returns>
        Task SetAsync(string key, T value, string region);

        /// <summary>
        /// Sets the asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="region">The region.</param>
        /// <param name="options">The options.</param>
        /// <returns>The Task.</returns>
        Task SetAsync(string key, T value, string region, DistributedCacheEntryOptions options);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="region">The region.</param>
        /// <returns>The Task.</returns>
        Task<bool> DeleteAsync(string key, string region);
    }
}
