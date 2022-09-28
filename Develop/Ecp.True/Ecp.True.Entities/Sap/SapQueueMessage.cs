// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapQueueMessage.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Sap
{
    using Ecp.True.Entities.Enumeration;

    /// <summary>
    /// The Sap Queue Message.
    /// </summary>
    public class SapQueueMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SapQueueMessage"/> class.
        /// </summary>
        public SapQueueMessage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SapQueueMessage" /> class.
        /// </summary>
        /// <param name="requestType">Type of the request.</param>
        /// <param name="uploadId">The upload identifier.</param>
        public SapQueueMessage(SapRequestType requestType, string uploadId)
        {
            this.RequestType = requestType;
            this.UploadId = uploadId;
        }

        /// <summary>
        /// Gets or sets the type of the request.
        /// </summary>
        /// <value>
        /// The type of the request.
        /// </value>
        public SapRequestType RequestType { get; set; }

        /// <summary>
        /// Gets or sets the message identifier.
        /// </summary>
        /// <value>
        /// The message identifier.
        /// </value>
        public int MessageId { get; set; }

        /// <summary>
        /// Gets or sets the previous movement identifier.
        /// </summary>
        /// <value>
        /// The previous movement identifier.
        /// </value>
        public int? PreviousMovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the upload identifier.
        /// </summary>
        /// <value>
        /// The upload identifier.
        /// </value>
        public string UploadId { get; set; }
    }
}
