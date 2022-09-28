// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRetryPolicyFactory.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.ExceptionHandling.Core
{
    using Ecp.True.ExceptionHandling.Entities;

    /// <summary>
    /// Factory for building retry policy.
    /// </summary>
    public interface IRetryPolicyFactory
    {
        /// <summary>
        /// Gets the retry policy.
        /// </summary>
        /// <param name="targetType">
        /// Type of the target.
        /// </param>
        /// <param name="retrySettings">
        /// The retry settings.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <returns>
        /// Retry Policy.
        /// </returns>
        IRetryPolicy GetRetryPolicy(string targetType, RetrySettings retrySettings, IRetryHandler handler);

        /// <summary>
        /// Gets the retry policy.
        /// </summary>
        /// <param name="targetType">
        /// Type of the target.
        /// </param>
        /// <param name="retrySettings">
        /// The retry settings.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <param name="useCachedPolicy">
        /// if set to <c>true</c> [use cached policy].
        /// </param>
        /// <returns>
        /// Retry Policy.
        /// </returns>
        IRetryPolicy GetRetryPolicy(string targetType, RetrySettings retrySettings, IRetryHandler handler, bool useCachedPolicy);

        /// <summary>
        /// Gets the retry policy with circuit breaker.
        /// </summary>
        /// <param name="targetType">
        /// Type of the target.
        /// </param>
        /// <param name="retrySettings">
        /// The retry settings.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <returns>
        /// Retry Policy with circuit Breaker.
        /// </returns>
        IRetryWithCircuitBreakerPolicy GetRetryPolicyWithCircuitBreaker(string targetType, RetrySettings retrySettings, IRetryHandler handler);

        /// <summary>
        /// Gets the retry policy with circuit breaker.
        /// </summary>
        /// <param name="targetType">
        /// Type of the target.
        /// </param>
        /// <param name="retrySettings">
        /// The retry settings.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <param name="useCachedPolicy">
        /// if set to <c>true</c> [use cached policy].
        /// </param>
        /// <returns>
        /// Retry Policy with circuit Breaker.
        /// </returns>
        IRetryWithCircuitBreakerPolicy GetRetryPolicyWithCircuitBreaker(string targetType, RetrySettings retrySettings, IRetryHandler handler, bool useCachedPolicy);
    }
}