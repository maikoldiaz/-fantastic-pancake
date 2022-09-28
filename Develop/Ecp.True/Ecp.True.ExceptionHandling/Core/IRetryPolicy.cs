// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRetryPolicy.cs" company="Microsoft">
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
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Generic Interface for Retry Policy.
    /// </summary>
    public interface IRetryPolicy
    {
        /// <summary>
        /// Executes the with retry asynchronous.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="retryDelegate">The retry delegate.</param>
        /// <returns>The Task.</returns>
        Task<TResult> ExecuteWithRetryAsync<TResult>(Func<Task<TResult>> retryDelegate);

        /// <summary>
        /// Executes the with retry asynchronous.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="retryDelegate">The retry delegate.</param>
        /// <param name="continueOnCapturedContext">if set to <c>true</c> [continue on captured context].</param>
        /// <returns>The Task.</returns>
        Task<TResult> ExecuteWithRetryAsync<TResult>(Func<Task<TResult>> retryDelegate, bool continueOnCapturedContext);
    }
}