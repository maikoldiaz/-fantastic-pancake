// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapTrackingError.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Admin
{
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The delta error.
    /// </summary>
    public class SapTrackingError : Entity
    {
        /// <summary>
        /// Gets or sets the sap tracking error identifier.
        /// </summary>
        /// <value>
        /// The sap tracking error identifier.
        /// </value>
        public int SapTrackingErrorId { get; set; }

        /// <summary>
        /// Gets or sets the sap tracking identifier.
        /// </summary>
        /// <value>
        /// The sap tracking identifier.
        /// </value>
        public int SapTrackingId { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the movement.
        /// </summary>
        /// <value>
        /// The movement.
        /// </value>
        public virtual SapTracking SapTracking { get; set; }
    }
}
