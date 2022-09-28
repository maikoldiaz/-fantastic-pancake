// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RetrySettingsTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.ExceptionHandling.Tests
{
    using Ecp.True.ExceptionHandling.Entities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    /// <summary>
    /// The retry settings tests.
    /// </summary>
    [TestClass]
    public class RetrySettingsTests
    {
        /// <summary>
        /// Retry settings with circuit breaker settings should be deserialized from JSON.
        /// </summary>
        [TestMethod]
        public void RetrySettingsWithCircuitBreakerShouldBeDeserializedFromJson()
        {
            var retrySettings = new RetrySettings
            {
                RetryStrategy = RetryStrategy.Exponential,
                RetryCount = 1,
                RetryIntervalInSeconds = 10,
                CircuitBreakerSettings = new CircuitSettings
                {
                    DurationOfBreakInSeconds = 10,
                    FailureThreshold = 1.0,
                    MinimumThroughput = 1,
                    SamplingDurationInSeconds = 10,
                },
            };

            retrySettings.Parameters.Add("Source", "EventHub");
            retrySettings.Parameters.Add("Destination", "DataLake");

            var json = JsonConvert.SerializeObject(retrySettings);

            var newSettings = JsonConvert.DeserializeObject<RetrySettings>(json);

            Assert.IsNotNull(newSettings);
            Assert.AreEqual(retrySettings.Parameters.Count, newSettings.Parameters.Count);
            Assert.AreEqual(retrySettings.RetryStrategy, newSettings.RetryStrategy);
            Assert.AreEqual(retrySettings.RetryCount, newSettings.RetryCount);
            Assert.AreEqual(retrySettings.RetryIntervalInSeconds, newSettings.RetryIntervalInSeconds);

            Assert.AreEqual(retrySettings.Parameters["Source"], newSettings.Parameters["Source"]);
            Assert.AreEqual(retrySettings.Parameters["Destination"], newSettings.Parameters["Destination"]);

            Assert.IsNotNull(newSettings.CircuitBreakerSettings);
            Assert.AreEqual(retrySettings.CircuitBreakerSettings.DurationOfBreakInSeconds, newSettings.CircuitBreakerSettings.DurationOfBreakInSeconds);
            Assert.AreEqual(retrySettings.CircuitBreakerSettings.FailureThreshold, newSettings.CircuitBreakerSettings.FailureThreshold);
            Assert.AreEqual(retrySettings.CircuitBreakerSettings.MinimumThroughput, newSettings.CircuitBreakerSettings.MinimumThroughput);
            Assert.AreEqual(retrySettings.CircuitBreakerSettings.SamplingDurationInSeconds, newSettings.CircuitBreakerSettings.SamplingDurationInSeconds);
        }

        /// <summary>
        /// Retry settings should be deserialized from JSON.
        /// </summary>
        [TestMethod]
        public void RetrySettingsWithoutCircuitBreakerShouldBeDeserializedFromJson()
        {
            var retrySettings = new RetrySettings
            {
                RetryStrategy = RetryStrategy.Exponential,
                RetryCount = 1,
                RetryIntervalInSeconds = 10,
            };

            retrySettings.Parameters.Add("Source", "EventHub");
            retrySettings.Parameters.Add("Destination", "DataLake");

            var json = JsonConvert.SerializeObject(retrySettings);

            var newSettings = JsonConvert.DeserializeObject<RetrySettings>(json);

            Assert.IsNotNull(newSettings);
            Assert.AreEqual(retrySettings.Parameters.Count, newSettings.Parameters.Count);
            Assert.AreEqual(retrySettings.RetryStrategy, newSettings.RetryStrategy);
            Assert.AreEqual(retrySettings.RetryCount, newSettings.RetryCount);
            Assert.AreEqual(retrySettings.RetryIntervalInSeconds, newSettings.RetryIntervalInSeconds);

            Assert.AreEqual(retrySettings.Parameters["Source"], newSettings.Parameters["Source"]);
            Assert.AreEqual(retrySettings.Parameters["Destination"], newSettings.Parameters["Destination"]);

            Assert.IsNull(newSettings.CircuitBreakerSettings);
        }
    }
}