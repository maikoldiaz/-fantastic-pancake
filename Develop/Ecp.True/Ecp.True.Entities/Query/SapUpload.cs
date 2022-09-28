// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapUpload.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Query
{
    using System;

    /// <summary>
    /// The SAP upload class.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Query.QueryEntity" />
    public class SapUpload : QueryEntity
    {
        /// <summary>
        /// Gets or sets the process identifier.
        /// </summary>
        /// <value>
        /// The process identifier.
        /// </value>
        public string ProcessId { get; set; }

        /// <summary>
        /// Gets or sets the file registration identifier.
        /// </summary>
        /// <value>
        /// The file registration identifier.
        /// </value>
        public int? FileRegistrationId { get; set; }

        /// <summary>
        /// Gets or sets the document identifier.
        /// </summary>
        /// <value>
        /// The document identifier.
        /// </value>
        public string DocumentId { get; set; }

        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        public int? TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the last date.
        /// </summary>
        /// <value>
        /// The last date record.
        /// </value>
        public DateTime LastDate { get; set; }
    }
}
