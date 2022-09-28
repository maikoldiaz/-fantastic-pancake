// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITelemetry.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// The telemetry service.
    /// </summary>
    public interface ITelemetry
    {
        /// <summary>
        /// Tracks the event.
        /// </summary>
        /// <param name="telemetry">The telemetry.</param>
        void TrackEvent(object telemetry);

        /// <summary>
        /// Tracks the event.
        /// </summary>
        /// <param name="tagGuid">The tag unique identifier.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="metrics">The metrics.</param>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="sourceLine">The source line.</param>
        /// <param name="member">The member.</param>
        void TrackEvent(
            string tagGuid,
            string eventName,
            IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null,
            [CallerFilePath] string sourceFile = "",
            [CallerLineNumber] int sourceLine = 0,
            [CallerMemberName] string member = "");

        /// <summary>
        /// Tracks the trace.
        /// </summary>
        /// <param name="telemetry">The telemetry.</param>
        void TrackTrace(object telemetry);

        /// <summary>
        /// Tracks the request.
        /// </summary>
        /// <param name="telemetry">The telemetry.</param>
        void TrackRequest(object telemetry);

        /// <summary>
        /// Tracks the dependency.
        /// </summary>
        /// <param name="telemetry">The telemetry.</param>
        void TrackDependency(object telemetry);

        /// <summary>
        /// Tracks the exception.
        /// </summary>
        /// <param name="telemetry">The telemetry.</param>
        void TrackException(object telemetry);

        /// <summary>
        /// Tracks the metric.
        /// </summary>
        /// <param name="telemetry">The telemetry.</param>
        void TrackMetric(object telemetry);

        /// <summary>
        /// Tracks the metric.
        /// </summary>
        /// <param name="tagGuid">The tag unique identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="sourceLine">The source line.</param>
        /// <param name="member">The member.</param>
        void TrackMetric(
            string tagGuid,
            string name,
            double value,
            IDictionary<string, string> properties = null,
            [CallerFilePath] string sourceFile = "",
            [CallerLineNumber] int sourceLine = 0,
            [CallerMemberName] string member = "");

        /// <summary>
        /// Starts the track metric.
        /// </summary>
        /// <param name="tagGuid">The tag unique identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="sourceLine">The source line.</param>
        /// <param name="member">The member.</param>
        void StartTrackingMetric(
            string tagGuid,
            string name,
            IDictionary<string, string> properties = null,
            [CallerFilePath] string sourceFile = "",
            [CallerLineNumber] int sourceLine = 0,
            [CallerMemberName] string member = "");

        /// <summary>
        /// Stops the track metric.
        /// </summary>
        /// <param name="tagGuid">The tag unique identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="sourceLine">The source line.</param>
        /// <param name="member">The member.</param>
        void StopTrackingMetric(
            string tagGuid,
            string name,
            IDictionary<string, string> properties = null,
            [CallerFilePath] string sourceFile = "",
            [CallerLineNumber] int sourceLine = 0,
            [CallerMemberName] string member = "");

        /// <summary>
        /// Tracks the availability.
        /// </summary>
        /// <param name="testName">Name of the test.</param>
        /// <param name="status">if set to <c>true</c> [status].</param>
        /// <param name="elapsedTimeSpan">The elapsed time span.</param>
        /// <param name="message">The message.</param>
        void TrackAvailability(
           string testName,
           bool status,
           TimeSpan elapsedTimeSpan,
           string message = "");
    }
}
