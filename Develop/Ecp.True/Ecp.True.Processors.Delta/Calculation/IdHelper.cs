// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdHelper.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Calculation
{
    using Ecp.True.Core;

    /// <summary>
    /// The IdHelper.
    /// </summary>
    public static class IdHelper
    {
        /// <summary>
        /// Generates the official calculation unique identifier.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="ownerId">The owner identifier.</param>
        /// <param name="measurementUnit">The measurementUnit.</param>
        /// <returns>Returns official calculation unique identifier.</returns>
        public static string BuildOfficialCalculationUniqueKey(string nodeId, string productId, string ownerId, string measurementUnit)
        {
            return string.Join(
             "-",
             new[] { nodeId, productId, ownerId, measurementUnit });
        }

        /// <summary>
        /// Gets node identifier.
        /// </summary>
        /// <param name="uniqueKey">The unique identifier.</param>
        /// <returns>Returns node identifier.</returns>
        public static string GetNodeId(string uniqueKey)
        {
            ArgumentValidators.ThrowIfNull(uniqueKey, nameof(uniqueKey));
            return uniqueKey.Split("-")[0];
        }

        /// <summary>
        /// Gets node identifier.
        /// </summary>
        /// <param name="uniqueKey">The unique identifier.</param>
        /// <returns>Returns node identifier.</returns>
        public static string GetProductId(string uniqueKey)
        {
            ArgumentValidators.ThrowIfNull(uniqueKey, nameof(uniqueKey));
            return uniqueKey.Split("-")[1];
        }

        /// <summary>
        /// Gets node identifier.
        /// </summary>
        /// <param name="uniqueKey">The unique identifier.</param>
        /// <returns>Returns node identifier.</returns>
        public static string GetOwnerId(string uniqueKey)
        {
            ArgumentValidators.ThrowIfNull(uniqueKey, nameof(uniqueKey));
            return uniqueKey.Split("-")[2];
        }

        /// <summary>
        /// Gets node identifier.
        /// </summary>
        /// <param name="uniqueKey">The unique identifier.</param>
        /// <returns>Returns node identifier.</returns>
        public static string GetMeasurementUnit(string uniqueKey)
        {
            ArgumentValidators.ThrowIfNull(uniqueKey, nameof(uniqueKey));
            return uniqueKey.Split("-")[3];
        }
    }
}
