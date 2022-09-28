// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AspNetCoreCache.cs" company="Microsoft">
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
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core.Interfaces;
    using Microsoft.Extensions.Caching.Distributed;

    /// <summary>
    /// The ASP NET Core Cache.
    /// </summary>
    public class AspNetCoreCache : IDistributedCache
    {
        /// <summary>
        /// The ASP net cache region.
        /// </summary>
        private const string AspNetCacheRegion = "TrueAspNetCacheRegion";

        /// <summary>
        /// The cache handler.
        /// </summary>
        private readonly ICacheHandler<byte[]> cacheHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="AspNetCoreCache"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="redisConnectionString">The redis connection string.</param>
        public AspNetCoreCache(ILogger logger, string redisConnectionString)
        {
            var cacheBootStrapper = new CacheBootstrapper(logger);
            cacheBootStrapper.Setup(CacheMode.DistributedOnly, TimeSpan.FromHours(1), redisConnectionString, true);
            this.cacheHandler = new CacheHandler<byte[]>(cacheBootStrapper);
        }

        /// <summary>
        /// Gets a value with the given key.
        /// </summary>
        /// <param name="key">A string identifying the requested value.</param>
        /// <returns>
        /// The located value or null.
        /// </returns>
        public byte[] Get(string key)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets a value with the given key.
        /// </summary>
        /// <param name="key">A string identifying the requested value.</param>
        /// <param name="token">Optional. The token used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The task that represents the asynchronous operation, containing the located value or null.
        /// </returns>
        public Task<byte[]> GetAsync(string key, CancellationToken token = default(CancellationToken))
        {
            return this.cacheHandler.GetAsync(key, AspNetCacheRegion);
        }

        /// <summary>
        /// Refreshes a value in the cache based on its key, resetting its sliding expiration timeout (if any).
        /// </summary>
        /// <param name="key">A string identifying the requested value.</param>
        public void Refresh(string key)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Refreshes a value in the cache based on its key, resetting its sliding expiration timeout (if any).
        /// </summary>
        /// <param name="key">A string identifying the requested value.</param>
        /// <param name="token">Optional. The token used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The task that represents the asynchronous operation.
        /// </returns>
        public Task RefreshAsync(string key, CancellationToken token = default(CancellationToken))
        {
            return this.cacheHandler.GetAsync(key, AspNetCacheRegion);
        }

        /// <summary>
        /// Removes the value with the given key.
        /// </summary>
        /// <param name="key">A string identifying the requested value.</param>
        public void Remove(string key)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes the value with the given key.
        /// </summary>
        /// <param name="key">A string identifying the requested value.</param>
        /// <param name="token">Optional. The token used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The Task that represents the asynchronous operation.
        /// </returns>
        public Task RemoveAsync(string key, CancellationToken token = default(CancellationToken))
        {
            return this.cacheHandler.DeleteAsync(key, AspNetCacheRegion);
        }

        /// <summary>
        /// Sets a value with the given key.
        /// </summary>
        /// <param name="key">A string identifying the requested value.</param>
        /// <param name="value">The value to set in the cache.</param>
        /// <param name="options">The cache options for the value.</param>
        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Sets the value with the given key.
        /// </summary>
        /// <param name="key">A string identifying the requested value.</param>
        /// <param name="value">The value to set in the cache.</param>
        /// <param name="options">The cache options for the value.</param>
        /// <param name="token">Optional. The token used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The task that represents the asynchronous operation.
        /// </returns>
        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
        {
            return this.cacheHandler.SetAsync(key, value, AspNetCacheRegion, options);
        }
    }
}
