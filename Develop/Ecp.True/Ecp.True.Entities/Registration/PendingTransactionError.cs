// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PendingTransactionError.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Registration
{
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The pending transaction error.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class PendingTransactionError : Entity, IComment
    {
        /// <summary>
        /// Gets or sets the error identifier.
        /// </summary>
        /// <value>
        /// The error identifier.
        /// </value>
        public int ErrorId { get; set; }

        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        public int TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the record identifier.
        /// </summary>
        /// <value>
        /// The record identifier.
        /// </value>
        public string RecordId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is retrying.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is retrying; otherwise, <c>false</c>.
        /// </value>
        public bool IsRetrying { get; set; }

        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the pending transaction.
        /// </summary>
        /// <value>
        /// The pending transaction.
        /// </value>
        public virtual PendingTransaction PendingTransaction { get; set; }
    }
}
