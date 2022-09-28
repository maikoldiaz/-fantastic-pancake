// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRetryHandler.cs" company="Microsoft">
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

    /// <summary>
    /// The RetryHandler interface.
    /// </summary>
    public interface IRetryHandler
    {
        /// <summary>
        /// Gets the handler type.
        /// </summary>
        /// <value>
        /// The handler type.
        /// </value>
        string HandlerType { get; }

        /// <summary>
        /// Determines whether [is faulty response] [the specified response].
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>
        /// <c>true</c> if [is faulty response] [the specified response]; otherwise, <c>false</c>.
        /// </returns>
        bool IsFaultyResponse(object response);

        /// <summary>
        /// Determines whether [is transient fault] [the specified exception].
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>
        /// <c>true</c> if [is transient fault] [the specified exception]; otherwise, <c>false</c>.
        /// </returns>
        bool IsTransientFault(Exception exception);
    }
}