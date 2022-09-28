// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheManagerLogger.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Caching
{
    using System;
    using Ecp.True.Core;
    using Ecp.True.Logging;
    using EventLevel = Microsoft.Extensions.Logging.LogLevel;
    using LogLevel = CacheManager.Core.Logging.LogLevel;

    /// <summary>
    /// The cache manager logger.
    /// </summary>
    /// <seealso cref="CacheManager.Core.Logging.ILogger" />
    public class CacheManagerLogger : CacheManager.Core.Logging.ILogger
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheManagerLogger"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public CacheManagerLogger(ITrueLogger logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc/>
        public IDisposable BeginScope(object state)
        {
            return null;
        }

        /// <inheritdoc/>
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= LogLevel.Warning;
        }

        /// <inheritdoc/>
        public void Log(LogLevel logLevel, int eventId, object message, Exception exception)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            if (exception != null)
            {
                this.logger.LogError(eventId, exception, message.ToString());
            }
            else
            {
                this.logger.Log(ToEventLevel(logLevel), eventId, message.ToString());
            }
        }

        private static EventLevel ToEventLevel(CacheManager.Core.Logging.LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    return EventLevel.Debug;
                case LogLevel.Trace:
                    return EventLevel.Trace;
                case LogLevel.Information:
                    return EventLevel.Information;
                case LogLevel.Warning:
                    return EventLevel.Warning;
                case LogLevel.Error:
                    return EventLevel.Error;
                case LogLevel.Critical:
                    return EventLevel.Critical;
                default:
                    return EventLevel.None;
            }
        }
    }
}
