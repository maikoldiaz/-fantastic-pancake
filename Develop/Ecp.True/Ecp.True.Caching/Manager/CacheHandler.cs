// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheHandler.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using CacheManager.Core;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Microsoft.Extensions.Caching.Distributed;

    /// <summary>
    /// The cache handler.
    /// </summary>
    /// <typeparam name="T">The type of cache entity.</typeparam>
    [IoCRegistration(IoCLifetime.ContainerControlled)]
    public class CacheHandler<T> : ICacheHandler<T>
    {
        /// <summary>
        /// The cache manager.
        /// </summary>
        private readonly ICacheManager<T> cacheManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheHandler{T}" /> class.
        /// </summary>
        /// <param name="cacheBootstrapper">The cache bootstrapper.</param>
        /// <param name="cacheFactory">The cache factory.</param>
        public CacheHandler(ICacheBootstrapper cacheBootstrapper, ICacheFactory cacheFactory)
        {
            ArgumentValidators.ThrowIfNull(cacheBootstrapper, nameof(cacheBootstrapper));
            ArgumentValidators.ThrowIfNull(cacheFactory, nameof(cacheFactory));

            var config = cacheBootstrapper.CacheStrategy.CacheConfiguration;
            this.cacheManager = cacheFactory.FromConfiguration<T>(config);
        }

        /// <inheritdoc/>
        public Task<T> GetAsync(string key, string region)
        {
            return Task.FromResult(this.cacheManager.Get(key, region));
        }

        /// <inheritdoc/>
        public Task SetAsync(string key, T value, string region)
        {
            this.cacheManager.Put(new CacheItem<T>(key, region, value));
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task SetAsync(string key, T value, string region, DistributedCacheEntryOptions options)
        {
            ExpirationMode expirationMode;
            TimeSpan timeout;
            if (options?.AbsoluteExpirationRelativeToNow != null)
            {
                expirationMode = ExpirationMode.Absolute;
                timeout = options.AbsoluteExpirationRelativeToNow.Value;
            }
            else if (options?.AbsoluteExpiration != null)
            {
                expirationMode = ExpirationMode.Absolute;
                timeout = options.AbsoluteExpiration.Value - DateTimeOffset.Now;
            }
            else if (options?.SlidingExpiration != null)
            {
                expirationMode = ExpirationMode.Sliding;
                timeout = options.SlidingExpiration.Value;
            }
            else
            {
                expirationMode = ExpirationMode.Default;
                timeout = Constants.DefaultCacheTimeOut;
            }

            this.cacheManager.Put(new CacheItem<T>(key, region, value, expirationMode, timeout));
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task<bool> DeleteAsync(string key, string region)
        {
            return Task.FromResult(this.cacheManager.Remove(key, region));
        }
    }
}
