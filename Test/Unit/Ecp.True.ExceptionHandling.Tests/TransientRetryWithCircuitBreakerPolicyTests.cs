// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransientRetryWithCircuitBreakerPolicyTests.cs" company="Microsoft">
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
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Ecp.True.ExceptionHandling.Core;
    using Ecp.True.ExceptionHandling.Entities;
    using Ecp.True.ExceptionHandling.Policy;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The transient retry with circuit breaker policy tests.
    /// </summary>
    [TestClass]
    public class TransientRetryWithCircuitBreakerPolicyTests
    {
        /// <summary>
        /// The circuit name.
        /// </summary>
        private const string CircuitName = "TestCircuitName";

        /// <summary>
        /// The counter.
        /// </summary>
        private int retryCounter;

        /// <summary>
        /// The retry handler mock.
        /// </summary>
        private Mock<IRetryHandler> retryHandlerMock;

        /// <summary>
        /// The retry policy.
        /// </summary>
        private TransientRetryWithCircuitBreakerPolicy retryPolicy;

        /// <summary>
        /// The retry settings.
        /// </summary>
        private RetrySettings retrySettings;

        /// <summary>
        /// Transient retry policy with circuit breaker execute with retry should retry when circuit breaker is disabled.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExecuteWithRetryShouldFallBackToTransientRetryWhenFaultOccursAndCircuitBreakerIsDisabledAsync()
        {
            this.retrySettings.CircuitBreakerSettings.MinimumThroughput = -1;
            this.retryPolicy = new TransientRetryWithCircuitBreakerPolicy(this.retrySettings, this.retryHandlerMock.Object, CircuitName);
            this.retryHandlerMock.Setup(s => s.IsTransientFault(It.IsAny<WebException>())).Returns(true);

            var result = await this.retryPolicy.ExecuteWithRetryAsync(async () => await this.GetStatusAsync().ConfigureAwait(false)).ConfigureAwait(false);

            Assert.IsTrue(result);
            Assert.AreEqual(this.retrySettings.RetryCount + 1, this.retryCounter);
            this.retryHandlerMock.Verify(s => s.IsTransientFault(It.IsAny<WebException>()), Times.Exactly(this.retrySettings.RetryCount));
        }

        /// <summary>
        /// Transient retry policy with circuit breaker should have valid circuit name when policy is created.
        /// </summary>
        [TestMethod]
        public void GetCircuitNameShouldBeValidWhenPolicyIsCreated()
        {
            Assert.AreEqual(CircuitName, this.retryPolicy.CircuitName);
        }

        /// <summary>
        /// Transient retry policy with circuit breaker should have valid circuit name when policy is created.
        /// </summary>
        [TestMethod]
        public void OpenCircuitTest()
        {
            this.retryPolicy.OpenCircuit();
            Assert.AreEqual(CircuitName, this.retryPolicy.CircuitName);
        }

        /// <summary>
        /// Transient retry policy with circuit breaker should have valid circuit name when policy is created.
        /// </summary>
        [TestMethod]
        public void CloseCircuitTest()
        {
            this.retryPolicy.CloseCircuit();
            Assert.AreEqual(CircuitName, this.retryPolicy.CircuitName);
        }

        /// <summary>
        /// Setups this instance.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            this.retryCounter = 0;
            this.retrySettings = this.GetRetrySettings();

            this.retryHandlerMock = new Mock<IRetryHandler>();
            this.retryHandlerMock.Setup(s => s.HandlerType).Returns("RestHandler");

            this.retryPolicy = new TransientRetryWithCircuitBreakerPolicy(this.retrySettings, this.retryHandlerMock.Object, CircuitName);
        }

        /// <summary>
        /// The get retry settings.
        /// </summary>
        /// <returns>
        /// The <see cref="RetrySettings" />.
        /// </returns>
        private RetrySettings GetRetrySettings()
        {
            return new RetrySettings
            {
                RetryStrategy = RetryStrategy.FixedInterval,
                RetryIntervalInSeconds = 1,
                RetryCount = 2,
                CircuitBreakerSettings = new CircuitSettings
                {
                    MinimumThroughput = 2,
                    FailureThreshold = 1,
                    DurationOfBreakInSeconds = 1,
                    SamplingDurationInSeconds = 1,
                },
            };
        }

        /// <summary>
        /// Gets the status asynchronous.
        /// </summary>
        /// <returns>The bool.</returns>
        /// <exception cref="ArgumentNullException">The exception.</exception>
        private Task<bool> GetStatusAsync()
        {
            this.retryCounter = this.retryCounter + 1;

            if (this.retryCounter > this.retrySettings.RetryCount)
            {
                return Task.FromResult(true);
            }

            throw new WebException();
        }
    }
}