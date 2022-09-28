// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FailureInfo.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.HandleFailure
{
    using System.Collections.Generic;

    /// <summary>
    /// The failure info.
    /// </summary>
    public class FailureInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FailureInfo" /> class.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="errorMessage">The error message.</param>
        public FailureInfo(int ticketId, string errorMessage)
        {
            this.TicketId = ticketId;
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FailureInfo" /> class.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="movementTransactionIds">The movement transaction identifiers.</param>
        public FailureInfo(int ticketId, string errorMessage, IEnumerable<int> movementTransactionIds)
        {
            this.TicketId = ticketId;
            this.ErrorMessage = errorMessage;
            this.MovementTransactionIds = movementTransactionIds;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FailureInfo" /> class.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="errorMessage">The error message.</param>
        public FailureInfo(int ticketId, int? nodeId, string errorMessage)
        {
            this.TicketId = ticketId;
            this.NodeId = nodeId;
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Gets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public int TicketId { get; }

        /// <summary>
        /// Gets the NodeId identifier.
        /// </summary>
        /// <value>
        /// The NodeId identifier.
        /// </value>
        public int? NodeId { get; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public IEnumerable<int> MovementTransactionIds { get; } = new List<int>();
    }
}
