// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRetryWithCircuitBreakerPolicy.cs" company="Microsoft">
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
    /// <summary>
    /// Generic Interface for retry with circuit breaker.
    /// </summary>
    public interface IRetryWithCircuitBreakerPolicy : IRetryPolicy
    {
        /// <summary>
        /// Gets the circuit name.
        /// </summary>
        /// <value>
        /// The circuit name.
        /// </value>
        string CircuitName { get; }

        /// <summary>
        /// Closes the circuit.
        /// </summary>
        void CloseCircuit();

        /// <summary>
        /// Opens the circuit.
        /// </summary>
        void OpenCircuit();
    }
}