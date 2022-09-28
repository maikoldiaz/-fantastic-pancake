// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationalCutOffInfo.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Processors.Balance.Calculation.Input
{
    using System.Collections.Generic;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The operational cut off input.
    /// </summary>
    public class OperationalCutOffInfo : OrchestratorMetaData
    {
        /// <summary>
        /// Gets or sets the balance input.
        /// </summary>
        /// <value>
        /// The balance input.
        /// </value>
        public BalanceInput BalanceInput { get; set; }

        /// <summary>
        /// Gets or sets the ticket.
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        public Ticket Ticket { get; set; }

        /// <summary>
        /// Gets or sets the balance message.
        /// </summary>
        /// <value>
        /// The balance message.
        /// </value>
        public MovementCalculationStep Step { get; set; }

        /// <summary>
        /// Gets the unbalances.
        /// </summary>
        /// <value>
        /// The unbalances.
        /// </value>
        public IDictionary<MovementCalculationStep, IEnumerable<Unbalance>> Unbalances { get; } = new Dictionary<MovementCalculationStep, IEnumerable<Unbalance>>();

        /// <summary>
        /// Gets or sets the segment unbalances.
        /// </summary>
        /// <value>
        /// The segment unbalances.
        /// </value>
        public IEnumerable<SegmentUnbalance> SegmentUnbalances { get; set; } = new List<SegmentUnbalance>();

        /// <summary>
        /// Gets or sets the system unbalances.
        /// </summary>
        /// <value>
        /// The system unbalances.
        /// </value>
        public IEnumerable<SystemUnbalance> SystemUnbalances { get; set; } = new List<SystemUnbalance>();

        /// <summary>
        /// Gets or sets the movements.
        /// </summary>
        /// <value>
        /// The movements.
        /// </value>
        public IEnumerable<Movement> Movements { get; set; } = new List<Movement>();
    }
}
