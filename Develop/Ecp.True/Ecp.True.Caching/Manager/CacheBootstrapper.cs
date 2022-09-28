// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheBootstrapper.cs" company="Microsoft">
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
    using Ecp.True.Core.Attributes;
    using Ecp.True.Logging;

    /// <summary>
    /// The cache bootstrap.
    /// </summary>
    [IoCRegistration(IoCLifetime.ContainerControlled)]
    public class CacheBootstrapper : ICacheBootstrapper
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<CacheBootstrapper> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheBootstrapper"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public CacheBootstrapper(ITrueLogger<CacheBootstrapper> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Gets the cache strategy.
        /// </summary>
        /// <value>
        /// The cache strategy.
        /// </value>
        public ICacheStrategy CacheStrategy { get; private set; }

        /// <summary>
        /// Setups the specified cache mode.
        /// </summary>
        /// <param name="cacheMode">The cache mode.</param>
        /// <param name="expiration">The expiration.</param>
        /// <param name="redisConnectionString">The redis connection string.</param>
        /// <param name="sliding">if set to <c>true</c> [sliding].</param>
        public void Setup(CacheMode cacheMode, TimeSpan expiration, string redisConnectionString, bool sliding)
        {
            switch (cacheMode)
            {
                case CacheMode.LocalWithBackplane:
                    this.CacheStrategy = new LocalWithBackplaneCacheStrategy(this.logger, redisConnectionString, expiration, sliding);
                    break;
                case CacheMode.LocalOnly:
                    this.CacheStrategy = new LocalOnlyCacheStrategy(this.logger, expiration, sliding);
                    break;
                case CacheMode.DistributedOnly:
                    this.CacheStrategy = new DistributedOnlyCacheStrategy(this.logger, redisConnectionString, expiration, sliding);
                    break;
                default:
                    this.CacheStrategy = new NoCacheStrategy();
                    break;
            }
        }
    }
}
