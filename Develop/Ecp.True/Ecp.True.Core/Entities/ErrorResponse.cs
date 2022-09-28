// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorResponse.cs" company="Microsoft">
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
    using Newtonsoft.Json;

    /// <summary>
    /// API response.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponse" /> class.
        /// </summary>
        /// <param name="errors">The error.</param>
        public ErrorResponse(IList<ErrorInfo> errors)
        {
            this.ErrorCodes = errors;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        public ErrorResponse(string error)
        {
            this.ErrorCodes = new List<ErrorInfo>
            {
                new ErrorInfo(error),
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        [JsonConstructor]
        public ErrorResponse(ErrorInfo error)
        {
            this.ErrorCodes = new List<ErrorInfo>
            {
                error,
            };
        }

        /// <summary>
        /// Gets the error codes.
        /// </summary>
        /// <value>
        /// The error codes.
        /// </value>
        public IEnumerable<ErrorInfo> ErrorCodes { get; private set; }
    }
}