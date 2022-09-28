// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransientRetryWithCircuitBreakerPolicy.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.ExceptionHandling.Policy
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.ExceptionHandling.Core;
    using Ecp.True.ExceptionHandling.Entities;
    using Polly;
    using Polly.CircuitBreaker;

    /// <summary>
    /// Base Implementation for Retry with Circuit Breaker Policy.
    /// </summary>
    public class TransientRetryWithCircuitBreakerPolicy : TransientRetryPolicy, IRetryWithCircuitBreakerPolicy
    {
        /// <summary>
        /// The circuit breaker policy.
        /// </summary>
        private readonly AsyncCircuitBreakerPolicy<object> circuitBreakerPolicy;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransientRetryWithCircuitBreakerPolicy" /> class.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <param name="circuitName">
        /// The circuit name.
        /// </param>
        public TransientRetryWithCircuitBreakerPolicy(RetrySettings settings, IRetryHandler handler, string circuitName)
            : base(settings, handler)
        {
            ArgumentValidators.ThrowIfNull(settings, nameof(settings));
            ArgumentValidators.ThrowIfNull(settings.CircuitBreakerSettings, nameof(settings.CircuitBreakerSettings));

            this.CircuitName = circuitName;

            // MinimumThroughput -1 denotes that circuit breaker is disabled.
            // Fallback is basic retry
            if (settings.CircuitBreakerSettings.MinimumThroughput != -1)
            {
                this.circuitBreakerPolicy = this.BuildCircuitBreakerRetryPolicy();
            }
        }

        /// <summary>
        /// Gets the circuit name.
        /// </summary>
        /// <value>
        /// The circuit name.
        /// </value>
        public string CircuitName { get; }

        /// <summary>
        /// Executes the with retry and circuit breaker asynchronous.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="retryDelegate">The retry delegate.</param>
        /// <param name="continueOnCapturedContext">if set to <c>true</c> [continue on captured context].</param>
        /// <returns>
        /// The Task.
        /// </returns>
        public override async Task<TResult> ExecuteWithRetryAsync<TResult>(Func<Task<TResult>> retryDelegate, bool continueOnCapturedContext)
        {
            if (this.circuitBreakerPolicy != null && retryDelegate != null)
            {
                var value = await this.RetryPolicy.ExecuteAsync(
                    async () => await this.circuitBreakerPolicy.ExecuteAndCaptureAsync(
                            async () => await retryDelegate().ConfigureAwait(continueOnCapturedContext))
                        .ConfigureAwait(continueOnCapturedContext))
                    .ConfigureAwait(continueOnCapturedContext);
                return value.GetType() == typeof(TResult) ? (TResult)value : default(TResult);
            }

            return await base.ExecuteWithRetryAsync(retryDelegate, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext);
        }

        /// <summary>
        /// Breaks the circuit.
        /// </summary>
        public void OpenCircuit()
        {
            this.circuitBreakerPolicy.Isolate();
        }

        /// <summary>
        /// Resets the circuit.
        /// </summary>
        public void CloseCircuit()
        {
            this.circuitBreakerPolicy.Reset();
        }

        /// <summary>
        /// The build circuit breaker retry policy.
        /// </summary>
        /// <returns>
        /// The <see cref="CircuitBreakerPolicy" />.
        /// </returns>
        private AsyncCircuitBreakerPolicy<object> BuildCircuitBreakerRetryPolicy()
        {
            return
                    Policy.Handle<Exception>(this.RetryHandler.IsTransientFault)
                            .OrResult<object>(this.RetryHandler.IsFaultyResponse)
                            .AdvancedCircuitBreakerAsync(
                                this.RetrySettings.CircuitBreakerSettings.FailureThreshold,
                                TimeSpan.FromSeconds(this.RetrySettings.CircuitBreakerSettings.SamplingDurationInSeconds),
                                this.RetrySettings.CircuitBreakerSettings.MinimumThroughput,
                                TimeSpan.FromSeconds(this.RetrySettings.CircuitBreakerSettings.DurationOfBreakInSeconds));
        }
    }
}