// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CallInterceptor.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Ioc
{
    using System.Diagnostics;
    using Castle.DynamicProxy;
    using Ecp.True.Core;
    using Ecp.True.Logging;

    /// <summary>
    /// Call Logger.
    /// </summary>
    /// <seealso cref="Castle.DynamicProxy.IInterceptor" />
    public class CallInterceptor : AsyncTimingInterceptor
    {
        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallInterceptor" /> class.
        /// </summary>
        /// <param name="telemetry">The telemetry.</param>
        public CallInterceptor(ITelemetry telemetry)
        {
            this.telemetry = telemetry;
        }

        /// <summary>
        /// Override in derived classes to receive signals prior method <paramref name="invocation" /> timing.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        protected override void StartingTiming(IInvocation invocation)
        {
            ArgumentValidators.ThrowIfNull(invocation, nameof(invocation));
            var metricName = $"{InstrumentConstants.TrueLoggerName}: {invocation.TargetType.Name}.{invocation.Method.Name}";
            this.telemetry.TrackMetric(LoggingConstants.InterceptorTag, metricName, 0);
        }

        /// <summary>
        /// Override in derived classes to receive signals after method <paramref name="invocation" /> timing.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="stopwatch">A stopwatch used to time the method <paramref name="invocation" />.</param>
        protected override void CompletedTiming(IInvocation invocation, Stopwatch stopwatch)
        {
            ArgumentValidators.ThrowIfNull(invocation, nameof(invocation));
            ArgumentValidators.ThrowIfNull(stopwatch, nameof(stopwatch));
            var metricName = $"{InstrumentConstants.TrueLoggerName}: {invocation.TargetType.Name}.{invocation.Method.Name}";
            this.telemetry.TrackMetric(LoggingConstants.InterceptorTag, metricName, stopwatch.ElapsedMilliseconds);
        }
    }
}