// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogisticQueueMessage.cs" company="Microsoft">
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
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;

    /// <summary>
    /// The Sap Logistic Movement Queue Message.
    /// </summary>
    public class LogisticQueueMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogisticQueueMessage"/> class.
        /// </summary>
        public LogisticQueueMessage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogisticQueueMessage" /> class.
        /// </summary>
        /// <param name="requestType">Type of the request.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="process">The process identifier.</param>
        public LogisticQueueMessage(SapRequestType requestType, int ticketId, StatusType process)
        {
            this.RequestType = requestType;
            this.TicketId = ticketId;
            this.Process = process;
        }

        /// <summary>
        /// Gets or sets the type of the request.
        /// </summary>
        /// <value>
        /// The type of the request.
        /// </value>
        public SapRequestType RequestType { get; set; }

        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public int TicketId { get; set; }

        /// <summary>
        /// Gets or sets the process identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public StatusType Process { get; set; }
    }
}
