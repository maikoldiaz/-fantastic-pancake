// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MyTelemetryInitializer.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Core
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// MyTelemetryInitializer.
    /// </summary>
    public class MyTelemetryInitializer : ITelemetryInitializer
    {
        private static string baseUrl = string.Empty;

        /// <inheritdoc/>
        public void Initialize(ITelemetry telemetry)
        {
            var exceptionTelemetry = telemetry as ExceptionTelemetry;
            var traceTelemetry = telemetry as TraceTelemetry;
            var dependencyTelemetry = telemetry as DependencyTelemetry;
            var eventTelemetry = telemetry as EventTelemetry;
            if (eventTelemetry != null && eventTelemetry.Properties.ContainsKey("Uri"))
            {
                SetEventTelemetry(eventTelemetry);
                return;
            }

            if (dependencyTelemetry != null)
            {
                SetDependencyTelemetry(dependencyTelemetry);
                return;
            }

            if (traceTelemetry != null && (traceTelemetry.Properties.ContainsKey("Uri") || traceTelemetry.Properties.ContainsKey("prop__Uri")))
            {
                SetTraceTelemetry(traceTelemetry);
                return;
            }

            if (exceptionTelemetry != null && exceptionTelemetry.Properties.ContainsKey("Uri"))
            {
                SetExceptionTelemetry(exceptionTelemetry);
            }
        }

        private static void SetEventTelemetry(EventTelemetry eventTelemetry)
        {
            if (eventTelemetry.Properties["Uri"].Contains(Constants.TransactionNodeQuorum, StringComparison.InvariantCulture))
            {
                eventTelemetry.Name = Constants.Quorum;
                eventTelemetry.Properties["Uri"] = Constants.QuorumBlockchainService;
            }
        }

        private static void SetDependencyTelemetry(DependencyTelemetry dependencyTelemetry)
        {
            if (dependencyTelemetry.Type == Constants.Http && dependencyTelemetry.Data.Contains(Constants.TransactionNodeQuorum, StringComparison.InvariantCulture))
            {
                dependencyTelemetry.Success = true;
                dependencyTelemetry.Name = Constants.Quorum;
                dependencyTelemetry.Data = dependencyTelemetry.Target;
                dependencyTelemetry.Context.GlobalProperties["Uri"] = Constants.QuorumBlockchainService;
            }
        }

        private static void SetExceptionTelemetry(ExceptionTelemetry exceptionTelemetry)
        {
            if (exceptionTelemetry.Properties["Uri"].Contains(Constants.TransactionNodeQuorum, StringComparison.InvariantCulture))
            {
                exceptionTelemetry.Message = Constants.Quorum;
                exceptionTelemetry.Properties["Uri"] = Constants.QuorumBlockchainService;
            }
        }

        private static void SetTraceTelemetry(TraceTelemetry traceTelemetry)
        {
            if (traceTelemetry != null &&
            (traceTelemetry.Message.Contains(Constants.TransactionNodeQuorum, StringComparison.InvariantCulture) ||
            (traceTelemetry.Properties.ContainsKey("Uri") && traceTelemetry.Properties["Uri"].Contains(Constants.TransactionNodeQuorum, StringComparison.InvariantCulture)) ||
            (traceTelemetry.Properties.ContainsKey("prop__Uri") && traceTelemetry.Properties["prop__Uri"].Contains(Constants.TransactionNodeQuorum, StringComparison.InvariantCulture))))
            {
                int index = traceTelemetry.Message.IndexOf('/', StringComparison.InvariantCulture) - 6;
                if (string.IsNullOrEmpty(baseUrl))
                {
                    int lastIndex = traceTelemetry.Message.LastIndexOf('/');
                    baseUrl = traceTelemetry.Message.Substring(index, lastIndex - index);
                }

                string firstMessage = traceTelemetry.Message.Contains(Constants.Received, StringComparison.InvariantCulture)
                || traceTelemetry.Message.Contains(Constants.End, StringComparison.InvariantCulture)
                    ? traceTelemetry.Message
                    : traceTelemetry.Message.Substring(0, index - 6);
                traceTelemetry.Message = firstMessage + baseUrl;
                traceTelemetry.Properties["Uri"] = Constants.QuorumBlockchainService;
                traceTelemetry.Properties["prop__Uri"] = Constants.QuorumBlockchainService;
            }
        }
    }
}
