// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnauthorizedResult.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core
{
    using Ecp.True.Entities.Core;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The UnauthorizedResult.
    /// </summary>
    public class UnauthorizedResult : StatusCodeResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedResult" /> class.
        /// </summary>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="errorCode">The error code.</param>
        public UnauthorizedResult(int httpStatusCode, AuthorizationErrorCode errorCode)
            : base(httpStatusCode)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        public AuthorizationErrorCode ErrorCode { get; set; }
    }
}
