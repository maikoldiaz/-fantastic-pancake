// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RetryPolicyFactoryTests.cs" company="Microsoft">
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
    using Ecp.True.ExceptionHandling.Core;

    using Ecp.True.ExceptionHandling.Entities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// The retry policy factory tests.
    /// </summary>
    [TestClass]
    public class RetryPolicyFactoryTests
    {
        /// <summary>
        /// The target type.
        /// </summary>
        private const string TargetType = "TestTargetType";

        /// <summary>
        /// The handler mock.
        /// </summary>
        private Mock<IRetryHandler> handlerMock;

        /// <summary>
        /// The policy factory.
        /// </summary>
        private IRetryPolicyFactory policyFactory;

        /// <summary>
        /// The factory should create retry policy when invoked.
        /// </summary>
        [TestMethod]
        public void FactoryShouldCreateRetryPolicyWhenCreatePolicyIsInvokedWithDefaultRetry()
        {
            var settings = this.GetRetrySettings();
            settings.RetryStrategy = RetryStrategy.None;

            var policy = this.policyFactory.GetRetryPolicy(TargetType, settings, this.handlerMock.Object);

            Assert.IsNotNull(policy);
            Assert.IsInstanceOfType(policy, typeof(IRetryPolicy));
        }

        /// <summary>
        /// The factory should create retry policy when invoked.
        /// </summary>
        [TestMethod]
        public void FactoryShouldCreateRetryPolicyWhenCreatePolicyIsInvokedWithExponentialRetry()
        {
            var settings = this.GetRetrySettings();
            settings.RetryStrategy = RetryStrategy.Exponential;

            var policy = this.policyFactory.GetRetryPolicy(TargetType, settings, this.handlerMock.Object);

            Assert.IsNotNull(policy);
            Assert.IsInstanceOfType(policy, typeof(IRetryPolicy));
        }

        /// <summary>
        /// The factory should create retry policy when invoked.
        /// </summary>
        [TestMethod]
        public void FactoryShouldCreateRetryPolicyWhenCreatePolicyIsInvokedWithFixedIntervalRetry()
        {
            var settings = this.GetRetrySettings();
            settings.RetryStrategy = RetryStrategy.FixedInterval;

            var policy = this.policyFactory.GetRetryPolicy(TargetType, settings, this.handlerMock.Object);

            Assert.IsNotNull(policy);
            Assert.IsInstanceOfType(policy, typeof(IRetryPolicy));
        }

        /// <summary>
        /// The factory should create retry policy when invoked.
        /// </summary>
        [TestMethod]
        public void FactoryShouldCreateRetryWithCircuitBreakerPolicyWhenCreatePolicyIsInvokedWithDefaultRetry()
        {
            var settings = this.GetCircuitBreakerRetrySettings();
            settings.RetryStrategy = RetryStrategy.None;

            var policy = this.policyFactory.GetRetryPolicyWithCircuitBreaker(TargetType, settings, this.handlerMock.Object);

            Assert.IsNotNull(policy);
            Assert.IsInstanceOfType(policy, typeof(IRetryWithCircuitBreakerPolicy));
            Assert.AreEqual(TargetType, policy.CircuitName);
        }

        /// <summary>
        /// The factory should create retry policy when invoked.
        /// </summary>
        [TestMethod]
        public void FactoryShouldCreateRetryWithCircuitBreakerPolicyWhenCreatePolicyIsInvokedWithExponentialRetry()
        {
            var settings = this.GetCircuitBreakerRetrySettings();
            settings.RetryStrategy = RetryStrategy.Exponential;

            var policy = this.policyFactory.GetRetryPolicyWithCircuitBreaker(TargetType, settings, this.handlerMock.Object);

            Assert.IsNotNull(policy);
            Assert.IsInstanceOfType(policy, typeof(IRetryWithCircuitBreakerPolicy));
            Assert.AreEqual(TargetType, policy.CircuitName);
        }

        /// <summary>
        /// The factory should create retry policy when invoked.
        /// </summary>
        [TestMethod]
        public void FactoryShouldCreateRetryWithCircuitBreakerPolicyWhenCreatePolicyIsInvokedWithFixedIntervalRetry()
        {
            var settings = this.GetCircuitBreakerRetrySettings();
            settings.RetryStrategy = RetryStrategy.FixedInterval;

            var policy = this.policyFactory.GetRetryPolicyWithCircuitBreaker(TargetType, settings, this.handlerMock.Object);

            Assert.IsNotNull(policy);
            Assert.IsInstanceOfType(policy, typeof(IRetryWithCircuitBreakerPolicy));
            Assert.AreEqual(TargetType, policy.CircuitName);
        }

        /// <summary>
        /// The setup.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            this.policyFactory = new RetryPolicyFactory();
            this.handlerMock = new Mock<IRetryHandler>();
        }

        /// <summary>
        /// The get circuit breaker retry settings.
        /// </summary>
        /// <returns>
        /// The <see cref="RetrySettings" />.
        /// </returns>
        private RetrySettings GetCircuitBreakerRetrySettings()
        {
            return new RetrySettings
            {
                RetryStrategy = RetryStrategy.Exponential,
                RetryIntervalInSeconds = 5,
                RetryCount = 5,
                CircuitBreakerSettings = new CircuitSettings
                {
                    DurationOfBreakInSeconds = 5,
                    MinimumThroughput = 10,
                    FailureThreshold = 1,
                    SamplingDurationInSeconds = 30,
                },
            };
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
                RetryStrategy = RetryStrategy.Exponential,
                RetryIntervalInSeconds = 5,
                RetryCount = 5,
            };
        }
    }
}