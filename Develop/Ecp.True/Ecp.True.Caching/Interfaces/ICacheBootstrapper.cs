// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICacheBootstrapper.cs" company="Microsoft">
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
    /// The cache bootsrap.
    /// </summary>
    public interface ICacheBootstrapper
    {
        /// <summary>
        /// Gets the cache strategy.
        /// </summary>
        /// <value>
        /// The cache strategy.
        /// </value>
        ICacheStrategy CacheStrategy { get; }

        /// <summary>
        /// Setups the specified cache mode.
        /// </summary>
        /// <param name="cacheMode">The cache mode.</param>
        /// <param name="expiration">The expiration.</param>
        /// <param name="redisConnectionString">The redis connection string.</param>
        /// <param name="sliding">if set to <c>true</c> [sliding].</param>
        void Setup(CacheMode cacheMode, TimeSpan expiration, string redisConnectionString, bool sliding);
    }
}