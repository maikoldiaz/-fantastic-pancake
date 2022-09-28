// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionInfo.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Admin
{
    using System;
    using Ecp.True.Entities.Query;

    /// <summary>
    /// Exception information.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class ExceptionInfo : QueryEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the error identifier.
        /// </summary>
        /// <value>
        /// The error identifier.
        /// </value>
        public int ErrorId { get; set; }

        /// <summary>
        /// Gets or sets the message identifier.
        /// </summary>
        /// <value>
        /// The message identifier.
        /// </value>
        public string MessageId { get; set; }

        /// <summary>
        /// Gets or sets the name of the system.
        /// </summary>
        /// <value>
        /// The name of the system.
        /// </value>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the name of the system type.
        /// </summary>
        /// <value>
        /// The name of the system type.
        /// </value>
        public string SystemTypeName { get; set; }

        /// <summary>
        /// Gets or sets the process.
        /// </summary>
        /// <value>
        /// The process.
        /// </value>
        public string Process { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is retry.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is retry; otherwise, <c>false</c>.
        /// </value>
        public bool IsRetry { get; set; }

        /// <summary>
        /// Gets or sets the file registration transaction identifier.
        /// </summary>
        /// <value>
        /// The file registration transaction identifier.
        /// </value>
        public int? FileRegistrationTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the upload identifier.
        /// </summary>
        /// <value>
        /// The upload identifier.
        /// </value>
        public string UploadId { get; set; }
    }
}
