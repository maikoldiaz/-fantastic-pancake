// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Microsoft">
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

    /// <summary>
    /// The cache constants.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// Gets the cache tag.
        /// </summary>
        /// <value>
        /// The cache tag.
        /// </value>
        public static string CacheTag => "1B38011B-0C4C-4E70-B082-E6D2AC21B9F0";

        /// <summary>
        /// Gets the name of the memory cache instance.
        /// </summary>
        /// <value>
        /// The name of the memory cache instance.
        /// </value>
        public static string MemoryCacheInstanceName => "truememory";

        /// <summary>
        /// Gets the distributed cache configuration key.
        /// </summary>
        /// <value>
        /// The distributed cache configuration key.
        /// </value>
        public static string DistributedCacheConfigurationKey => "trueredis";

        /// <summary>
        /// Gets the redis maximum retry.
        /// </summary>
        /// <value>
        /// The redis maximum retry.
        /// </value>
        public static int RedisMaxRetry => 3;

        /// <summary>
        /// Gets the redis connect time out.
        /// </summary>
        /// <value>
        /// The redis connect time out.
        /// </value>
        public static TimeSpan RedisConnectTimeOut => TimeSpan.FromSeconds(15);

        /// <summary>
        /// Gets the redis synchronize time out.
        /// </summary>
        /// <value>
        /// The redis synchronize time out.
        /// </value>
        public static TimeSpan RedisSyncTimeOut => TimeSpan.FromSeconds(15);

        /// <summary>
        /// Gets the redis retry time out.
        /// </summary>
        /// <value>
        /// The redis retry time out.
        /// </value>
        public static TimeSpan RedisRetryTimeOut => TimeSpan.FromSeconds(15);

        /// <summary>
        /// Gets the default cache time out.
        /// </summary>
        /// <value>
        /// The default cache time out.
        /// </value>
        public static TimeSpan DefaultCacheTimeOut => TimeSpan.FromHours(1);
    }
}
