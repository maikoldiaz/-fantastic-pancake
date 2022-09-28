// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrueWebException.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Host.Core
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;

    /// <summary>
    /// True Web Exception.
    /// </summary>
    //// Important: This attribute is NOT inherited from Exception, and MUST be specified
    //// otherwise serialization will fail with a SerializationException stating that
    //// "Type X in Assembly Y is not marked as serializable."
    [Serializable]
    public class TrueWebException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrueWebException"/> class.
        /// </summary>
        public TrueWebException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrueWebException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorCode">The error code.</param>
        public TrueWebException(string message, string errorCode)
            : base(message)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrueWebException"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public TrueWebException(HttpStatusCode code)
            : this()
        {
            this.StatusCode = code;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrueWebException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TrueWebException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrueWebException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public TrueWebException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrueWebException"/> class.
        /// </summary>
        /// <param name="info">The serialization context that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The serialization context that contains contextual information about the source or destination.</param>
        protected TrueWebException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        public string ErrorCode { get; }

        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public HttpStatusCode? StatusCode { get; }
    }
}
