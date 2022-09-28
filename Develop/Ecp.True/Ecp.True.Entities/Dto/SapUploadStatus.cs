// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapUploadStatus.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Entities.Query;

    /// <summary>
    /// The Sap Upload Dto.
    /// </summary>
    public class SapUploadStatus
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SapUploadStatus" /> class.
        /// </summary>
        /// <param name="processId">The process identifier.</param>
        /// <param name="sourceSystemId">The source system identifier.</param>
        /// <param name="uploads">The uploads.</param>
        public SapUploadStatus(string processId, string sourceSystemId, IEnumerable<SapUpload> uploads)
        {
            this.ProcessId = processId;
            this.SourceSystemId = sourceSystemId;
            this.Documents = uploads.GroupBy(x => new { x.DocumentId, x.TransactionId })
                .Select(x => new SapDocument(x.Key.DocumentId, x.Key.TransactionId, x));
        }

        /// <summary>
        /// Gets or sets the process identifier.
        /// </summary>
        /// <value>
        /// The process identifier.
        /// </value>
        public string ProcessId { get; set; }

        /// <summary>
        /// Gets or sets the source system identifier.
        /// </summary>
        /// <value>
        /// The source system identifier.
        /// </value>
        public string SourceSystemId { get; set; }

        /// <summary>
        /// Gets or sets the documents.
        /// </summary>
        /// <value>
        /// The documents.
        /// </value>
        public IEnumerable<SapDocument> Documents { get; set; }
    }
}
