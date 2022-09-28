// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.ExceptionHandling.Entities
{
    /// <summary>
    /// The constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The under score punctuation.
        /// </summary>
        public static readonly string UnderScorePunctuation = "_";

        /// <summary>
        /// The retry policy convention.
        /// </summary>
        public static readonly string RetryPolicyConvention = "{0}RetryPolicy";

        /// <summary>
        /// The circuit breaker convention.
        /// </summary>
        public static readonly string CircuitBreakerConvention = "{0}CircuitBreakerPolicy";

        /// <summary>
        /// The retry with circuit breaker convention.
        /// </summary>
        public static readonly string RetryWithCircuitBreakerConvention = "{0}RetryWithCircuitBreakerPolicy";
    }
}