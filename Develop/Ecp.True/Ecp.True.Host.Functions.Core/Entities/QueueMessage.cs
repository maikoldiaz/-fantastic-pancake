// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueueMessage.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Core.Entities
{
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;

    /// <summary>
    /// The queue message.
    /// </summary>
    public class QueueMessage
    {
        /// <summary>
        /// Gets or sets the system type identifier.
        /// </summary>
        /// <value>
        /// The system type identifier.
        /// </value>
        public SystemType SystemTypeId { get; set; }

        /// <summary>
        /// Gets or sets the upload file identifier.
        /// </summary>
        /// <value>
        /// The upload file identifier.
        /// </value>
        public string UploadId { get; set; }

        /// <summary>
        /// Gets or sets the upload file identifier.
        /// </summary>
        /// <value>
        /// The upload file identifier.
        /// </value>
        public string UploadFileId { get; set; }

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        /// <value>
        /// The type of the action.
        /// </value>
        public FileRegistrationActionType ActionType { get; set; }
    }
}
