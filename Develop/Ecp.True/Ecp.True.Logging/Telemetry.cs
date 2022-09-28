// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Telemetry.cs" company="Microsoft">
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
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// The telemetry.
    /// </summary>
    [IoCRegistration(IoCLifetime.Hierarchical)]
    public class Telemetry : ITelemetry
    {
        /// <summary>
        /// The telemetry client.
        /// </summary>
        private readonly TelemetryClient telemetryClient;

        /// <summary>
        /// The log source context.
        /// </summary>
        private readonly bool logSourceContext = true;

        /// <summary>
        /// The logger properties.
        /// </summary>
        private readonly ConcurrentDictionary<string, string> loggerProperties = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// The stopwatch.
        /// </summary>
        private Stopwatch stopwatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="Telemetry"/> class.
        /// </summary>
        /// <param name="telemetryClient">The Telemetry Client.</param>
        public Telemetry(TelemetryClient telemetryClient)
        {
            this.telemetryClient = telemetryClient;
        }

        /// <inheritdoc/>
        public void TrackDependency(object telemetry)
        {
            var telemetryObj = (DependencyTelemetry)telemetry;
            ArgumentValidators.ThrowIfNull(telemetryObj, nameof(telemetryObj));
            telemetryObj.Context.Operation.Id = this.telemetryClient.Context.Operation.Id;

            this.telemetryClient.TrackDependency(telemetryObj);
        }

        /// <inheritdoc/>
        public void TrackEvent(object telemetry)
        {
            var telemetryObj = (EventTelemetry)telemetry;
            ArgumentValidators.ThrowIfNull(telemetryObj, nameof(telemetryObj));
            telemetryObj.Context.Operation.Id = this.telemetryClient.Context.Operation.Id;

            this.telemetryClient.TrackEvent(telemetryObj);
        }

        /// <inheritdoc/>
        public void TrackEvent(
            string tagGuid,
            string eventName,
            IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null,
            [CallerFilePath] string sourceFile = "",
            [CallerLineNumber] int sourceLine = 0,
            [CallerMemberName] string member = "")
        {
            var telemetryMessage = new EventTelemetry(eventName);
            foreach (KeyValuePair<string, string> item in this.loggerProperties)
            {
                telemetryMessage.Properties[item.Key] = item.Value;
            }

            if (properties != null)
            {
                foreach (KeyValuePair<string, string> item in properties)
                {
                    telemetryMessage.Properties[item.Key] = item.Value;
                }
            }

            if (metrics != null)
            {
                foreach (KeyValuePair<string, double> item in metrics)
                {
                    telemetryMessage.Metrics[item.Key] = item.Value;
                }
            }

            this.AddCallProperties(tagGuid, telemetryMessage.Properties, sourceFile, sourceLine, member);
            this.telemetryClient.TrackEvent(telemetryMessage);
        }

        /// <inheritdoc/>
        public void TrackException(object telemetry)
        {
            var telemetryObj = (ExceptionTelemetry)telemetry;
            ArgumentValidators.ThrowIfNull(telemetryObj, nameof(telemetryObj));
            telemetryObj.Context.Operation.Id = this.telemetryClient.Context.Operation.Id;

            this.telemetryClient.TrackException(telemetryObj);
        }

        /// <inheritdoc/>
        public void TrackMetric(object telemetry)
        {
            var telemetryObj = (MetricTelemetry)telemetry;
            ArgumentValidators.ThrowIfNull(telemetryObj, nameof(telemetryObj));
            telemetryObj.Context.Operation.Id = this.telemetryClient.Context.Operation.Id;

            this.telemetryClient.TrackMetric(telemetryObj);
        }

        /// <inheritdoc/>
        public void TrackMetric(
            string tagGuid,
            string name,
            double value,
            IDictionary<string, string> properties = null,
            [CallerFilePath] string sourceFile = "",
            [CallerLineNumber] int sourceLine = 0,
            [CallerMemberName] string member = "")
        {
            var telemetryMessage = new MetricTelemetry(name, value);
            foreach (KeyValuePair<string, string> item in this.loggerProperties)
            {
                telemetryMessage.Properties[item.Key] = item.Value;
            }

            if (properties != null)
            {
                foreach (KeyValuePair<string, string> item in properties)
                {
                    telemetryMessage.Properties[item.Key] = item.Value;
                }
            }

            this.AddCallProperties(tagGuid, telemetryMessage.Properties, sourceFile, sourceLine, member);
            this.telemetryClient.TrackMetric(telemetryMessage);
        }

        /// <inheritdoc/>
        public void TrackRequest(object telemetry)
        {
            var telemetryObj = (RequestTelemetry)telemetry;
            ArgumentValidators.ThrowIfNull(telemetryObj, nameof(telemetryObj));
            telemetryObj.Context.Operation.Id = this.telemetryClient.Context.Operation.Id;

            this.telemetryClient.TrackRequest(telemetryObj);
        }

        /// <inheritdoc/>
        public void TrackTrace(object telemetry)
        {
            var telemetryObj = (TraceTelemetry)telemetry;
            ArgumentValidators.ThrowIfNull(telemetryObj, nameof(telemetryObj));
            telemetryObj.Context.Operation.Id = this.telemetryClient.Context.Operation.Id;

            this.telemetryClient.TrackTrace(telemetryObj);
        }

        /// <summary>
        /// Starts the track metric.
        /// </summary>
        /// <param name="tagGuid">The tag unique identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="sourceLine">The source line.</param>
        /// <param name="member">The member.</param>
        public void StartTrackingMetric(
            string tagGuid,
            string name,
            IDictionary<string, string> properties = null,
            [CallerFilePath] string sourceFile = "",
            [CallerLineNumber] int sourceLine = 0,
            [CallerMemberName] string member = "")
        {
            this.stopwatch = Stopwatch.StartNew();
            this.TrackMetric(tagGuid, name, 0, properties, sourceFile, sourceLine, member);
        }

        /// <summary>
        /// Stops the track metric.
        /// </summary>
        /// <param name="tagGuid">The tag unique identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="sourceLine">The source line.</param>
        /// <param name="member">The member.</param>
        public void StopTrackingMetric(
            string tagGuid,
            string name,
            IDictionary<string, string> properties = null,
            [CallerFilePath] string sourceFile = "",
            [CallerLineNumber] int sourceLine = 0,
            [CallerMemberName] string member = "")
        {
            this.stopwatch.Stop();
            this.TrackMetric(tagGuid, name, this.stopwatch.ElapsedMilliseconds, properties, sourceFile, sourceLine, member);
        }

        /// <inheritdoc />
        public void TrackAvailability(
            string testName,
            bool status,
            TimeSpan elapsedTimeSpan,
            string message = "")
        {
            var availability = new AvailabilityTelemetry
            {
                Name = testName,
                RunLocation = Constants.Location,
                Success = status,
            };

            availability.Message = message;
            availability.Duration = elapsedTimeSpan;
            availability.Timestamp = DateTime.UtcNow;

            this.telemetryClient.TrackAvailability(availability);
            //// call flush to ensure telemetry is sent
            this.telemetryClient.Flush();
        }

        private void AddCallProperties(string tagGuid, IDictionary<string, string> properties, string sourceFile, int sourceLine, string member)
        {
            properties.Add(InstrumentConstants.TagGuidName, tagGuid);
            if (this.logSourceContext)
            {
                properties.Add(InstrumentConstants.SourceFileName, sourceFile);
                properties.Add(InstrumentConstants.SourceLineName, sourceLine.ToString(CultureInfo.InvariantCulture));
                properties.Add(InstrumentConstants.CallingMemberName, member);
            }
        }
    }
}
