// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationalCutOff.cs" company="Microsoft">
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
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The Operational CutOff DTO.
    /// </summary>
    public class OperationalCutOff
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationalCutOff"/> class.
        /// </summary>
        public OperationalCutOff()
        {
            this.FailedLogisticsMovements = new List<int>();
        }

        /// <summary>
        /// Gets or sets the ticket.
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        public Ticket Ticket { get; set; }

        /// <summary>
        /// Gets or sets the unbalance data list.
        /// </summary>
        /// <value>
        /// The unbalance data list.
        /// </value>
        public IEnumerable<UnbalanceComment> Unbalances { get; set; }

        /// <summary>
        /// Gets or sets the pending transaction errors list.
        /// </summary>
        /// <value>
        /// The pending transaction errors list.
        /// </value>
        public IEnumerable<PendingTransactionError> PendingTransactionErrors { get; set; }

        /// <summary>
        /// Gets or sets the first time nodes.
        /// </summary>
        /// <value>
        /// The first time nodes.
        /// </value>
        public IEnumerable<int> FirstTimeNodes { get; set; }

        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the failed Movements of the element.
        /// </summary>
        /// <value>
        /// Movements.
        /// </value>
        public IEnumerable<int> FailedLogisticsMovements { get; set; }
    }
}
