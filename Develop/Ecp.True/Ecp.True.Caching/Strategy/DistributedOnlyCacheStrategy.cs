// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DistributedOnlyCacheStrategy.cs" company="Microsoft">
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
    using CacheManager.Core;
    using Ecp.True.Logging;

    /// <summary>
    /// The distributed only cache strategy.
    /// </summary>
    /// <seealso cref="Ecp.True.Caching.CacheStrategyBase" />
    internal class DistributedOnlyCacheStrategy : CacheStrategyBase
    {
        /// <summary>
        /// The redis connection string.
        /// </summary>
        private readonly string redisConnectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributedOnlyCacheStrategy"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="redisConnectionString">The redis connection string.</param>
        /// <param name="expiration">The expiration.</param>
        /// <param name="sliding">if set to <c>true</c> [sliding].</param>
        public DistributedOnlyCacheStrategy(ITrueLogger logger, string redisConnectionString, TimeSpan expiration, bool sliding)
            : base(logger, expiration, sliding)
        {
            this.redisConnectionString = redisConnectionString;
        }

        /// <inheritdoc />
        public override ICacheManagerConfiguration CacheConfiguration
        {
            get
            {
                return new ConfigurationBuilder()
                                 .WithLogging(typeof(CacheManagerLoggerFactory), this.Logger)
                                 .WithRedisConfiguration(Constants.DistributedCacheConfigurationKey, RedisConnectionManager.GetMultiplexer(this.redisConnectionString, this.Logger))
                                 .WithJsonSerializer()
                                 .WithMaxRetries(Constants.RedisMaxRetry)
                                 .WithRetryTimeout((int)Constants.RedisRetryTimeOut.TotalMilliseconds)
                                 .WithRedisCacheHandle(Constants.DistributedCacheConfigurationKey)
                                 .WithExpiration(this.Sliding ? ExpirationMode.Sliding : ExpirationMode.Absolute, this.Expiration)
                                 .Build();
            }
        }
    }
}
