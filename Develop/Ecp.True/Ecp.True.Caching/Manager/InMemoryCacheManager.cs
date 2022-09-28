// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InMemoryCacheManager.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Ecp.True.Configuration")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Ecp.True.KeyStore")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Ecp.True.DataAccess.Sql")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Ecp.True.Caching.Tests")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Ecp.True.Configuration.Tests")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Ecp.True.KeyStore.Tests")]

namespace Ecp.True.Caching
{
    using System;

    using Microsoft.Extensions.Caching.Memory;

    /// <summary>
    /// In Memory Cache Manager class.
    /// </summary>
    internal static class InMemoryCacheManager
    {
        /// <summary>
        /// Memory cache instance.
        /// </summary>
        private static readonly IMemoryCache MemCache = new MemoryCache(new MemoryCacheOptions());

        /// <summary>
        /// Adds the specified key name.
        /// </summary>
        /// <typeparam name="T">Type of the cached item.</typeparam>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="item">The item.</param>
        /// <param name="expirationDurationInSeconds">The expiration duration in seconds. Setting non-positive value will make it never expire.</param>
        public static void Add<T>(string keyName, T item, double expirationDurationInSeconds)
        {
            Add(keyName, item, expirationDurationInSeconds, false);
        }

        /// <summary>
        /// Adds the specified key name.
        /// </summary>
        /// <typeparam name="T">Type of the cached item.</typeparam>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="item">The item.</param>
        /// <param name="expirationDurationInSeconds">The expiration duration in seconds. Setting non-positive value will make it never expire.</param>
        /// <param name="isSlidingExpiration">if set to <c>true</c> [is sliding expiration].</param>
        public static void Add<T>(string keyName, T item, double expirationDurationInSeconds, bool isSlidingExpiration)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions();
            if (isSlidingExpiration)
            {
                cacheEntryOptions.SlidingExpiration = TimeSpan.FromSeconds(expirationDurationInSeconds);
            }
            else
            {
                cacheEntryOptions.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(expirationDurationInSeconds);
            }

            MemCache.Set(keyName, item, cacheEntryOptions);
        }

        /// <summary>
        /// Adds the specified key name.
        /// </summary>
        /// <typeparam name="T">Type of the cached item.</typeparam>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="item">The item.</param>
        public static void Add<T>(string keyName, T item)
        {
            MemCache.Set(keyName, item);
        }

        /// <summary>
        /// Gets the cache item for a given key.
        /// </summary>
        /// <typeparam name="T">Type of the cached item.</typeparam>
        /// <param name="keyName">Name of the key.</param>
        /// <returns>Cached item.</returns>
        public static T Get<T>(string keyName)
        {
            return MemCache.Get<T>(keyName);
        }

        /// <summary>
        /// Removes item from the cache.
        /// </summary>
        /// <param name="keyName">Key name of the cache item that needs to removed.</param>
        public static void Remove(string keyName)
        {
            MemCache.Remove(keyName);
        }
    }
}