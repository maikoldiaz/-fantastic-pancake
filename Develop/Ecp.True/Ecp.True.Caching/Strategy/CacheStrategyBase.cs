// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheStrategyBase.cs" company="Microsoft">
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
    /// The base cache strategy.
    /// </summary>
    /// <seealso cref="Ecp.True.Caching.ICacheStrategy" />
    internal class CacheStrategyBase : ICacheStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheStrategyBase"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="expiration">The expiration.</param>
        /// <param name="sliding">if set to <c>true</c> [sliding].</param>
        protected CacheStrategyBase(ITrueLogger logger, TimeSpan expiration, bool sliding)
        {
            this.Logger = logger;
            this.Expiration = expiration;
            this.Sliding = sliding;
        }

        /// <summary>
        /// Gets the cache configuration.
        /// </summary>
        /// <value>
        /// The cache configuration.
        /// </value>
        public virtual ICacheManagerConfiguration CacheConfiguration { get; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        protected ITrueLogger Logger { get; }

        /// <summary>
        /// Gets the expiration.
        /// </summary>
        /// <value>
        /// The expiration.
        /// </value>
        protected TimeSpan Expiration { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="CacheStrategyBase"/> is sliding.
        /// </summary>
        /// <value>
        ///   <c>true</c> if sliding; otherwise, <c>false</c>.
        /// </value>
        protected bool Sliding { get; }
    }
}
