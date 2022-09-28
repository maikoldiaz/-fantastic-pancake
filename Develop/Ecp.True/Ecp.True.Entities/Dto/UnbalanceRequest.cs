// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnbalanceRequest.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Entities.Dto
{
    using System.Collections.Generic;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The bulk update entity.
    /// </summary>
    public class UnbalanceRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnbalanceRequest"/> class.
        /// </summary>
        public UnbalanceRequest()
        {
            this.TransferPoints = new List<TransferPoints>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnbalanceRequest"/> class.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        public UnbalanceRequest(Ticket ticket)
        {
            this.Ticket = ticket;
            this.TransferPoints = new List<TransferPoints>();
        }

        /// <summary>
        /// Gets or sets the ticket.
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        public Ticket Ticket { get; set; }

        /// <summary>
        /// Gets or sets the transfer points.
        /// </summary>
        /// <value>
        /// The transfer points.
        /// </value>
        public IEnumerable<TransferPoints> TransferPoints { get; set; }

        /// <summary>
        /// Gets or sets the first time nodes.
        /// </summary>
        /// <value>
        /// The first time nodes.
        /// </value>
        public IEnumerable<int> FirstTimeNodes { get; set; }
    }
}
