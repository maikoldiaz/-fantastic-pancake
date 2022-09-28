// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EthereumException.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Azure
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    /// <summary>
    ///  Custom exception for Blockchain Ethereum Connector related operations.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class EthereumException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EthereumException"/> class.
        /// </summary>
        public EthereumException()
            : base(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EthereumException" /> class .
        /// </summary>
        /// <param name="message">Exception message.</param>
        public EthereumException(string message)
        : base(message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EthereumException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public EthereumException(string message, Exception innerException)
        : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EthereumException"/> class.
        /// </summary>
        /// <param name="info">The holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The that contains contextual information about the source or destination.</param>
        protected EthereumException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {
        }
    }
}
