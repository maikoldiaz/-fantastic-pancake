// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalOnlyCacheStrategy.cs" company="Microsoft">
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
    /// The local only cache strategy.
    /// </summary>
    /// <seealso cref="Ecp.True.Caching.CacheStrategyBase" />
    internal class LocalOnlyCacheStrategy : CacheStrategyBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalOnlyCacheStrategy"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="expiration">The expiration.</param>
        /// <param name="sliding">if set to <c>true</c> [sliding].</param>
        public LocalOnlyCacheStrategy(ITrueLogger logger, TimeSpan expiration, bool sliding)
            : base(logger, expiration, sliding)
        {
        }

        /// <summary>
        /// Gets the cache configuration.
        /// </summary>
        /// <value>
        /// The cache configuration.
        /// </value>
        public override ICacheManagerConfiguration CacheConfiguration
        {
            get
            {
                return new ConfigurationBuilder()
                                    .WithLogging(typeof(CacheManagerLoggerFactory), this.Logger)
                                    .WithUpdateMode(CacheUpdateMode.Up)
                                    .WithMicrosoftMemoryCacheHandle(Constants.MemoryCacheInstanceName)
                                    .WithExpiration(this.Sliding ? ExpirationMode.Sliding : ExpirationMode.Absolute, this.Expiration)
                                    .Build();
            }
        }
    }
}
