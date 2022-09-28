// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonQueueMessage.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Core
{
    /// <summary>
    /// The JSON Queue message.
    /// </summary>
    public class JsonQueueMessage
    {
        /// <summary>
        /// Gets or sets the upload file identifier.
        /// </summary>
        /// <value>
        /// The upload file identifier.
        /// </value>
        public string UploadId { get; set; }

        /// <summary>
        /// Gets or sets the file registration transaction identifier.
        /// </summary>
        /// <value>
        /// The file registration transaction identifier.
        /// </value>
        public int FileRegistrationTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the message type identifier.
        /// </summary>
        /// <value>
        /// The message type identifier.
        /// </value>
        public MessageType MessageTypeId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is official sap movement.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is official sap movement; otherwise, <c>false</c>.
        /// </value>
        public bool IsOfficialSapMovement { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is retry.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is retry; otherwise, <c>false</c>.
        /// </value>
        public bool IsRetry
        {
            get
            {
                return this.FileRegistrationTransactionId > 0 && string.IsNullOrWhiteSpace(this.UploadId);
            }
        }
    }
}
