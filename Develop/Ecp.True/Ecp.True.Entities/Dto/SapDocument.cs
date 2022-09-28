// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapDocument.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Ecp.True.Entities.Query;
    using Newtonsoft.Json;

    /// <summary>
    /// The Document Class.
    /// </summary>
    public class SapDocument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SapDocument" /> class.
        /// </summary>
        /// <param name="documentId">The document identifier.</param>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <param name="uploads">The uploads.</param>
        public SapDocument(string documentId, int? transactionId, IEnumerable<SapUpload> uploads)
        {
            this.DocumentId = documentId;
            this.TransactionId = Convert.ToString(transactionId, CultureInfo.InvariantCulture);
            this.Errors = uploads.Where(x => x.DocumentId == documentId && x.TransactionId == transactionId).Select(y => new SapError
            {
                ErrorCode = y.ErrorCode,
                ErrorDescription = y.ErrorMessage,
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SapDocument" /> class.
        /// </summary>
        /// <param name="documentId">The document identifier.</param>
        /// <param name="transactionId">The transaction identifier.</param>
        [JsonConstructor]
        public SapDocument(string documentId, string transactionId)
        {
            this.DocumentId = documentId;
            this.TransactionId = Convert.ToString(transactionId, CultureInfo.InvariantCulture);
        }

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
        public string TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public IEnumerable<SapError> Errors { get; set; }
    }
}
