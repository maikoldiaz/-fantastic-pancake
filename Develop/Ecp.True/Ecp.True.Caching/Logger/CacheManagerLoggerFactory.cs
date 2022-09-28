// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheManagerLoggerFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Ecp.True.Caching
{
    using Ecp.True.Logging;

    /// <summary>
    /// The cache manager logger factory.
    /// </summary>
    /// <seealso cref="CacheManager.Core.Logging.ILoggerFactory" />
    internal class CacheManagerLoggerFactory : CacheManager.Core.Logging.ILoggerFactory
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheManagerLoggerFactory"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public CacheManagerLoggerFactory(ITrueLogger logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc/>
        public CacheManager.Core.Logging.ILogger CreateLogger(string categoryName)
        {
            return new CacheManagerLogger(this.logger);
        }

        /// <inheritdoc/>
        public CacheManager.Core.Logging.ILogger CreateLogger<T>(T instance)
        {
            return new CacheManagerLogger(this.logger);
        }
    }
}
