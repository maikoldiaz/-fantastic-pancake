// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITrueLoggerBase.cs" company="Microsoft">
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
    /// The base logger.
    /// </summary>
    public interface ITrueLogger
    {
        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        void LogError(Exception exception, string message, params object[] args);

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        void LogError(EventId eventId, Exception exception, string message, params object[] args);

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        void LogError(string message, params object[] args);

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        void LogInformation(string message, params object[] args);

        /// <summary>
        /// Logs the specified log level.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        void Log(LogLevel logLevel, EventId eventId, string message, params object[] args);

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        void LogWarning(string message, params object[] args);
    }
}
