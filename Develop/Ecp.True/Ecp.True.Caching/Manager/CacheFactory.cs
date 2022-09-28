// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Caching.Manager
{
    using System.Diagnostics.CodeAnalysis;
    using CacheManager.Core;
    using CacheManagerCacheFactory = CacheManager.Core.CacheFactory;

    /// <summary>
    /// The cache factory.
    /// </summary>
    /// <seealso cref="Ecp.True.Caching.ICacheFactory" />
    [ExcludeFromCodeCoverage]
    public class CacheFactory : ICacheFactory
    {
        /// <summary>
        /// Gets the cache configuration.
        /// </summary>
        /// <typeparam name="TCacheValue">The type of the cache value.</typeparam>
        /// <param name="configuration">The configuration.</param>
        /// <returns>
        /// The cache manager.
        /// </returns>
        public ICacheManager<TCacheValue> FromConfiguration<TCacheValue>(ICacheManagerConfiguration configuration)
        {
            return CacheManagerCacheFactory.FromConfiguration<TCacheValue>(configuration);
        }
    }
}
