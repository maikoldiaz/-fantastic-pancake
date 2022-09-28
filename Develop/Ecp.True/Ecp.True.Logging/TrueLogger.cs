// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrueLogger.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Logging
{
    using System;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The True logger.
    /// </summary>
    /// <typeparam name="T">Type of logger.</typeparam>
    /// <seealso cref="Ecp.True.Logging.ITrueLogger{T}" />
    public class TrueLogger<T> : ITrueLogger<T>
        where T : class
    {
        private readonly ILogger<T> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrueLogger{T}"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public TrueLogger(ILogger<T> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public void Log(LogLevel logLevel, EventId eventId, string message, params object[] args)
        {
            this.logger.Log(logLevel, eventId, message, args);
        }

        /// <inheritdoc />
        public void LogError(Exception exception, string message, params object[] args)
        {
            this.logger.LogError(exception, message, args);
        }

        /// <inheritdoc />
        public void LogError(EventId eventId, Exception exception, string message, params object[] args)
        {
            this.logger.LogError(eventId, exception, message, args);
        }

        /// <inheritdoc />
        public void LogError(string message, params object[] args)
        {
            this.logger.LogError(message, args);
        }

        /// <inheritdoc />
        public void LogInformation(string message, params object[] args)
        {
            this.logger.LogInformation(message, args);
        }

        /// <inheritdoc />
        public void LogWarning(string message, params object[] args)
        {
            this.logger.LogWarning(message, args);
        }
    }
}
