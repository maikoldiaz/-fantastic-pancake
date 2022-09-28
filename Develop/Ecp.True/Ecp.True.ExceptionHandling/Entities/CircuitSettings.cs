// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CircuitSettings.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.ExceptionHandling.Entities
{
    /// <summary>
    /// Settings For Circuit Breaker.
    /// </summary>
    public class CircuitSettings
    {
        /// <summary>
        /// Gets or sets the duration of break in seconds.
        /// </summary>
        /// <value>
        /// The duration of break in seconds.
        /// </value>
        public int DurationOfBreakInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the failure threshold.
        /// This represents the proportion of failures at which to break.
        /// </summary>
        /// <value>
        /// The failure threshold.
        /// </value>
        public double FailureThreshold { get; set; }

        /// <summary>
        /// Gets or sets the minimum throughput.
        /// This many calls must have passed through the circuit within the active samplingDuration for the circuit to consider breaking.
        /// </summary>
        /// <value>
        /// The minimum throughput.
        /// </value>
        public int MinimumThroughput { get; set; }

        /// <summary>
        /// Gets or sets the sampling duration in seconds.
        /// The failure rate is considered for actions over this period. Successes/failures older than the period are discarded from metrics.
        /// </summary>
        /// <value>
        /// The sampling duration in seconds.
        /// </value>
        public int SamplingDurationInSeconds { get; set; }
    }
}