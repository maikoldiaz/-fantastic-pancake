// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileRegistrationInfo.cs" company="Microsoft">
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
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;

    /// <summary>
    /// The Register File entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class FileRegistrationInfo : Entity
    {
        /// <summary>
        /// Gets or sets the error count.
        /// </summary>
        /// <value>
        /// The error count.
        /// </value>
        public int ErrorCount { get; set; }

        /// <summary>
        /// Gets or sets the file upload status.
        /// </summary>
        /// <value>
        /// The file upload status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the upload file identifier.
        /// </summary>
        /// <value>
        /// The upload identifier.
        public string UploadId { get; set; }

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        /// <value>
        /// The type of the action.
        /// </value>
        public string ActionType { get; set; }

        /// <summary>
        /// Gets or sets the segment name.
        /// </summary>
        /// <value>
        /// The segment name.
        /// </value>
        public string SegmentName { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the system type identifier.
        /// </summary>
        /// <value>
        /// The system type identifier.
        /// </value>
        public SystemType SystemTypeId { get; set; }

        /// <summary>
        /// Gets or sets the records processed.
        /// </summary>
        /// <value>
        /// The records processed.
        /// </value>
        public int RecordsProcessed { get; set; }

        /// <summary>
        /// Gets or sets the is parsed.
        /// </summary>
        /// <value>
        /// The is parsed.
        /// </value>
        public bool? IsParsed { get; set; }
    }
}
