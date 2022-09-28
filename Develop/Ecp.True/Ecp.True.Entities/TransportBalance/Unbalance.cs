// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Unbalance.cs" company="Microsoft">
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
    using System;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The unbalance.
    /// </summary>
    public class Unbalance : BlockchainEntity
    {
        /// <summary>
        /// Gets or sets the unbalance identifier.
        /// </summary>
        /// <value>
        /// The unbalance identifier.
        /// </value>
        public int UnbalanceId { get; set; }

        /// <summary>
        /// Gets or sets the ticket number.
        /// </summary>
        /// <value>
        /// The ticket number.
        /// </value>
        public int TicketId { get; set; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the initial inventory.
        /// </summary>
        /// <value>
        /// The initial inventory.
        /// </value>
        public decimal InitialInventory { get; set; }

        /// <summary>
        /// Gets or sets the inputs.
        /// </summary>
        /// <value>
        /// The inputs.
        /// </value>
        public decimal Inputs { get; set; }

        /// <summary>
        /// Gets or sets the inputs.
        /// </summary>
        /// <value>
        /// The inputs.
        /// </value>
        public decimal Outputs { get; set; }

        /// <summary>
        /// Gets or sets the final inventory.
        /// </summary>
        /// <value>
        /// The final inventory.
        /// </value>
        public decimal FinalInventory { get; set; }

        /// <summary>
        /// Gets or sets the identified losses.
        /// </summary>
        /// <value>
        /// The identified losses.
        /// </value>
        public decimal IdentifiedLosses { get; set; }

        /// <summary>
        /// Gets or sets the unbalance.
        /// </summary>
        /// <value>
        /// The unbalance.
        /// </value>
        public decimal? UnbalanceAmount { get; set; }

        /// <summary>
        /// Gets or sets the interface.
        /// </summary>
        /// <value>
        /// The interface.
        /// </value>
        public decimal Interface { get; set; }

        /// <summary>
        /// Gets or sets the tolerance.
        /// </summary>
        /// <value>
        /// The tolerance.
        /// </value>
        public decimal? Tolerance { get; set; }

        /// <summary>
        /// Gets or sets the unidentified losses.
        /// </summary>
        /// <value>
        /// The unidentified losses.
        /// </value>
        public decimal? UnidentifiedLosses { get; set; }

        /// <summary>
        /// Gets or sets the interface unbalance.
        /// </summary>
        /// <value>
        /// The interface unbalance.
        /// </value>
        public decimal? InterfaceUnbalance { get; set; }

        /// <summary>
        /// Gets or sets the tolerance unbalance.
        /// </summary>
        /// <value>
        /// The tolerance unbalance.
        /// </value>
        public decimal? ToleranceUnbalance { get; set; }

        /// <summary>
        /// Gets or sets the unidentified unbalance.
        /// </summary>
        /// <value>
        /// The unidentified unbalance.
        /// </value>
        public decimal? UnidentifiedLossesUnbalance { get; set; }

        /// <summary>
        /// Gets or sets the calculation date.
        /// </summary>
        /// <value>
        /// The execution date.
        /// </value>
        public DateTime CalculationDate { get; set; }

        /// <summary>
        /// Gets or sets the standard uncertainty.
        /// </summary>
        /// <value>
        /// The standard uncertainty.
        /// </value>
        public decimal? StandardUncertainty { get; set; }

        /// <summary>
        /// Gets or sets the average uncertainty.
        /// </summary>
        /// <value>
        /// The average uncertainty.
        /// </value>
        public decimal? AverageUncertainty { get; set; }

        /// <summary>
        /// Gets or sets the warning.
        /// </summary>
        /// <value>
        /// The warning.
        /// </value>
        public decimal? Warning { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public decimal? Action { get; set; }

        /// <summary>
        /// Gets or sets the control tolerance.
        /// </summary>
        /// <value>
        /// The control tolerance.
        /// </value>
        public decimal? ControlTolerance { get; set; }

        /// <summary>
        /// Gets or sets the tolerance identified losses.
        /// </summary>
        /// <value>
        /// The tolerance identified losses.
        /// </value>
        public decimal? ToleranceIdentifiedLosses { get; set; }

        /// <summary>
        /// Gets or sets the tolerance inputs.
        /// </summary>
        /// <value>
        /// The tolerance inputs.
        /// </value>
        public decimal? ToleranceInputs { get; set; }

        /// <summary>
        /// Gets or sets the tolerance outputs.
        /// </summary>
        /// <value>
        /// The tolerance outputs.
        /// </value>
        public decimal? ToleranceOutputs { get; set; }

        /// <summary>
        /// Gets or sets the tolerance initial inventory.
        /// </summary>
        /// <value>
        /// The tolerance initial inventory.
        /// </value>
        public decimal? ToleranceInitialInventory { get; set; }

        /// <summary>
        /// Gets or sets the tolerance final inventory.
        /// </summary>
        /// <value>
        /// The tolerance final inventory.
        /// </value>
        public decimal? ToleranceFinalInventory { get; set; }

        /// <summary>
        /// Gets or sets the average uncertainty unbalance percentage.
        /// </summary>
        /// <value>
        /// The average uncertainty unbalance percentage.
        /// </value>
        public decimal? AverageUncertaintyUnbalancePercentage { get; set; }

        /// <summary>
        /// Gets or sets the name of the node.
        /// </summary>
        /// <value>
        /// The name of the node.
        /// </value>
        public virtual Node Node { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
        public virtual Product Product { get; set; }

        /// <summary>
        /// Gets or sets the ticket.
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        public virtual Ticket Ticket { get; set; }
    }
}
