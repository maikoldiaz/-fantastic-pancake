// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransientRetryPolicy.cs" company="Microsoft">
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
    using Polly.Retry;
    using IRetryPolicy = Ecp.True.ExceptionHandling.Core.IRetryPolicy;

    /// <summary>
    /// The Retry Policy Base Class.
    /// </summary>
    public class TransientRetryPolicy : IRetryPolicy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransientRetryPolicy" /> class.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        public TransientRetryPolicy(RetrySettings settings, IRetryHandler handler)
        {
            this.RetrySettings = settings;
            this.RetryHandler = handler;

            this.RetryPolicy = this.GetRetryPolicy();
        }

        /// <summary>
        /// Gets the retry handler.
        /// </summary>
        /// <value>
        /// The retry handler.
        /// </value>
        protected IRetryHandler RetryHandler { get; }

        /// <summary>
        /// Gets the retry policy.
        /// </summary>
        /// <value>
        /// The retry policy.
        /// </value>
        protected AsyncRetryPolicy<object> RetryPolicy { get; }

        /// <summary>
        /// Gets the retry settings.
        /// </summary>
        /// <value>
        /// The retry settings.
        /// </value>
        protected RetrySettings RetrySettings { get; }

        /// <summary>
        /// Executes the with retry asynchronous.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="retryDelegate">The retry delegate.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        public Task<TResult> ExecuteWithRetryAsync<TResult>(Func<Task<TResult>> retryDelegate)
        {
            return this.ExecuteWithRetryAsync(retryDelegate, false);
        }

        /// <summary>
        /// Executes the with retry asynchronous.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="retryDelegate">The retry delegate.</param>
        /// <param name="continueOnCapturedContext">if set to <c>true</c> [continue on captured context].</param>
        /// <returns>
        /// The Task.
        /// </returns>
        public virtual async Task<TResult> ExecuteWithRetryAsync<TResult>(Func<Task<TResult>> retryDelegate, bool continueOnCapturedContext)
        {
            ArgumentValidators.ThrowIfNull(retryDelegate, nameof(retryDelegate));
            if (this.RetryPolicy != null)
            {
                var result = await this.RetryPolicy.ExecuteAsync(
                    async () => await retryDelegate().ConfigureAwait(continueOnCapturedContext))
                        .ConfigureAwait(continueOnCapturedContext);

                return result != null && result.GetType() == typeof(TResult) ? (TResult)result : default(TResult);
            }

            return await retryDelegate().ConfigureAwait(continueOnCapturedContext);
        }

        /// <summary>
        /// Gets the retry policy.
        /// </summary>
        /// <returns>Retry policy.</returns>
        private AsyncRetryPolicy<object> GetRetryPolicy()
        {
            switch (this.RetrySettings.RetryStrategy)
            {
                case RetryStrategy.Exponential:
                    return Policy.Handle<Exception>(this.RetryHandler.IsTransientFault)
                            .OrResult<object>(this.RetryHandler.IsFaultyResponse).WaitAndRetryAsync(
                                this.RetrySettings.RetryCount,
                                retryAttempt => TimeSpan.FromSeconds(
                                    Math.Pow(this.RetrySettings.RetryIntervalInSeconds, retryAttempt)));
                case RetryStrategy.FixedInterval:
                    return Policy.Handle<Exception>(this.RetryHandler.IsTransientFault)
                            .OrResult<object>(this.RetryHandler.IsFaultyResponse).WaitAndRetryAsync(
                                this.RetrySettings.RetryCount,
                                retryAttempt => TimeSpan.FromSeconds(this.RetrySettings.RetryIntervalInSeconds));
                default:
                    return Policy.Handle<Exception>(this.RetryHandler.IsTransientFault)
                            .OrResult<object>(this.RetryHandler.IsFaultyResponse).RetryAsync(this.RetrySettings.RetryCount);
            }
        }
    }
}