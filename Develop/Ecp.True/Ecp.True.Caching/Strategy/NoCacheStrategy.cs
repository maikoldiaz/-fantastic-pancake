// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NoCacheStrategy.cs" company="Microsoft">
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
    using CacheManager.Core;

    /// <summary>
    /// The no cache strategy.
    /// </summary>
    /// <seealso cref="Ecp.True.Caching.ICacheStrategy" />
    internal class NoCacheStrategy : ICacheStrategy
    {
        /// <summary>
        /// Gets the cache configuration.
        /// </summary>
        /// <value>
        /// The cache configuration.
        /// </value>
        public ICacheManagerConfiguration CacheConfiguration
        {
            get
            {
                return null;
            }
        }
    }
}
