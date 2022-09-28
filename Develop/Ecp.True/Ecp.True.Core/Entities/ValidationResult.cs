// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationResult.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Core.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// The Validation result.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        public ValidationResult()
        {
            this.ErrorInfo = new List<ErrorInfo>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        /// <param name="errorInfo">The error response.</param>
        public ValidationResult(IList<ErrorInfo> errorInfo)
        {
            this.ErrorInfo = errorInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        public ValidationResult(string error)
            : this()
        {
            this.ErrorInfo.Add(new ErrorInfo(error));
        }

        /// <summary>
        /// Gets the success validation result.
        /// </summary>
        /// <value>
        /// The success.
        /// </value>
        public static ValidationResult Success => new ValidationResult { IsSuccess = true };

        /// <summary>
        /// Gets the error response.
        /// </summary>
        /// <value>
        /// The error response.
        /// </value>
        public IList<ErrorInfo> ErrorInfo { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is failure.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is failure; otherwise, <c>false</c>.
        /// </value>
        public bool IsSuccess { get; set; }
    }
}
