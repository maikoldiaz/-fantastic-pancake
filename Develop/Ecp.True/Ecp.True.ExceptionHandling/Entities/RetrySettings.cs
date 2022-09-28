// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RetrySettings.cs" company="Microsoft">
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
    using System.Collections.Generic;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Settings for Retry and Circuit Breaker.
    /// </summary>
    public class RetrySettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RetrySettings" /> class.
        /// </summary>
        public RetrySettings()
        {
            this.Parameters = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public Dictionary<string, object> Parameters { get; }

        /// <summary>
        /// Gets or sets the circuit breaker settings.
        /// </summary>
        /// <value>
        /// The circuit breaker settings.
        /// </value>
        public CircuitSettings CircuitBreakerSettings { get; set; }

        /// <summary>
        /// Gets or sets the Retry Count.
        /// </summary>
        /// <value>
        /// The Retry Count.
        /// </value>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets the retry back off time in seconds.
        /// </summary>
        /// <value>
        /// The back off time in seconds.
        /// </value>
        public int RetryIntervalInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the type of the retry.
        /// </summary>
        /// <value>
        /// The type of the retry.
        /// </value>
        [JsonConverter(typeof(StringEnumConverter))]
        public RetryStrategy RetryStrategy { get; set; }
    }
}