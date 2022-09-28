// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RetryPolicyFactory.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.ExceptionHandling
{
    using System.Collections.Concurrent;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.ExceptionHandling.Core;
    using Ecp.True.ExceptionHandling.Entities;
    using Ecp.True.ExceptionHandling.Policy;

    /// <summary>
    /// The retry policy factory.
    /// </summary>
    [IoCRegistration(IoCLifetime.ContainerControlled)]
    public class RetryPolicyFactory : IRetryPolicyFactory
    {
        /// <summary>
        /// The retry policies.
        /// </summary>
        private readonly ConcurrentDictionary<string, IRetryPolicy> retryPolicies;

        /// <summary>
        /// The retry with circuit breaker policies.
        /// </summary>
        private readonly ConcurrentDictionary<string, IRetryWithCircuitBreakerPolicy> retryWithCircuitBreakerPolicies;

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryPolicyFactory" /> class.
        /// </summary>
        public RetryPolicyFactory()
        {
            this.retryPolicies = new ConcurrentDictionary<string, IRetryPolicy>();
            this.retryWithCircuitBreakerPolicies = new ConcurrentDictionary<string, IRetryWithCircuitBreakerPolicy>();
        }

        /// <summary>
        /// The get retry policy.
        /// </summary>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="retrySettings">
        /// The retry settings.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <returns>
        /// The <see cref="IRetryPolicy" />.
        /// </returns>
        public IRetryPolicy GetRetryPolicy(string targetType, RetrySettings retrySettings, IRetryHandler handler)
        {
            return this.GetRetryPolicy(targetType, retrySettings, handler, true);
        }

        /// <summary>
        /// The get retry policy.
        /// </summary>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="retrySettings">
        /// The retry settings.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <param name="useCachedPolicy">
        /// The use cached policy.
        /// </param>
        /// <returns>
        /// The <see cref="IRetryPolicy" />.
        /// </returns>
        public IRetryPolicy GetRetryPolicy(
            string targetType,
            RetrySettings retrySettings,
            IRetryHandler handler,
            bool useCachedPolicy)
        {
            ArgumentValidators.ThrowIfNull(handler, nameof(handler));
            if (!useCachedPolicy)
            {
                return CreateRetryPolicy(retrySettings, handler);
            }

            var key = string.Concat(targetType, Entities.Constants.UnderScorePunctuation, handler.HandlerType);
            if (this.retryPolicies.TryGetValue(key, out var retryPolicy))
            {
                return retryPolicy;
            }

            retryPolicy = CreateRetryPolicy(retrySettings, handler);
            this.retryPolicies[key] = retryPolicy;

            return retryPolicy;
        }

        /// <summary>
        /// The get retry policy with circuit breaker.
        /// </summary>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="retrySettings">
        /// The retry settings.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <returns>
        /// The <see cref="IRetryWithCircuitBreakerPolicy" />.
        /// </returns>
        public IRetryWithCircuitBreakerPolicy GetRetryPolicyWithCircuitBreaker(
            string targetType,
            RetrySettings retrySettings,
            IRetryHandler handler)
        {
            return this.GetRetryPolicyWithCircuitBreaker(targetType, retrySettings, handler, true);
        }

        /// <summary>
        /// The get retry policy with circuit breaker.
        /// </summary>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="retrySettings">
        /// The retry settings.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <param name="useCachedPolicy">
        /// The use cached policy.
        /// </param>
        /// <returns>
        /// The <see cref="IRetryWithCircuitBreakerPolicy" />.
        /// </returns>
        public IRetryWithCircuitBreakerPolicy GetRetryPolicyWithCircuitBreaker(
            string targetType,
            RetrySettings retrySettings,
            IRetryHandler handler,
            bool useCachedPolicy)
        {
            ArgumentValidators.ThrowIfNull(handler, nameof(handler));
            if (!useCachedPolicy)
            {
                return CreateCircuitBreakerRetryPolicy(retrySettings, handler, targetType);
            }

            var key = string.Concat(targetType, Entities.Constants.UnderScorePunctuation, handler.HandlerType);
            if (this.retryWithCircuitBreakerPolicies.TryGetValue(key, out var retryPolicy))
            {
                return retryPolicy;
            }

            retryPolicy = CreateCircuitBreakerRetryPolicy(retrySettings, handler, targetType);
            this.retryWithCircuitBreakerPolicies[key] = retryPolicy;

            return retryPolicy;
        }

        /// <summary>
        /// The create circuit breaker retry policy.
        /// </summary>
        /// <param name="retrySettings">
        /// The retry settings.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <returns>
        /// The <see cref="IRetryWithCircuitBreakerPolicy" />.
        /// </returns>
        private static IRetryWithCircuitBreakerPolicy CreateCircuitBreakerRetryPolicy(RetrySettings retrySettings, IRetryHandler handler, string targetType)
        {
            return new TransientRetryWithCircuitBreakerPolicy(retrySettings, handler, targetType);
        }

        /// <summary>
        /// The create retry policy.
        /// </summary>
        /// <param name="retrySettings">
        /// The retry settings.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <returns>
        /// The <see cref="IRetryPolicy" />.
        /// </returns>
        private static IRetryPolicy CreateRetryPolicy(RetrySettings retrySettings, IRetryHandler handler)
        {
            return new TransientRetryPolicy(retrySettings, handler);
        }
    }
}