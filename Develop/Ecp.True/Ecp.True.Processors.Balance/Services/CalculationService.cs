// <copyright file="CalculationService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Balance.Operation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Balance.Calculation.Input;
    using Ecp.True.Processors.Balance.Calculation.Output;

    /// <summary>
    /// Calculation Service.
    /// </summary>
    public abstract class CalculationService : ICalculationService
    {
        /// <summary>
        /// Gets the variable type.
        /// </summary>
        /// <value>
        /// The variable type.
        /// </value>
        public abstract MovementCalculationStep Type { get; }

        /// <summary>
        /// Processes the interfaces asynchronous.
        /// </summary>
        /// <param name="balanceInputInfo">The balance input info.</param>
        /// <param name="ticket">The ticket.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>
        /// The collection of interface.
        /// </returns>
        public abstract Task<CalculationOutput> ProcessAsync(
            BalanceInputInfo balanceInputInfo, Ticket ticket, IUnitOfWork unitOfWork);

        /// <summary>Do Generate Base.</summary>
        /// <returns>movement object.</returns>
        /// <param name="balanceInputInfo">The input.</param>
        /// <param name="node">The node.</param>
        /// <param name="ticket">The ticket.</param>
        protected static CalculationInput BuildCalculationInput(BalanceInputInfo balanceInputInfo, NodeInput node, Ticket ticket)
        {
            ArgumentValidators.ThrowIfNull(ticket, nameof(ticket));
            ArgumentValidators.ThrowIfNull(balanceInputInfo, nameof(balanceInputInfo));
            var initialInventories = balanceInputInfo.InitialInventories.Where(y => y.NodeId == node.NodeId);
            var finalInventories = balanceInputInfo.FinalInventories.Where(y => y.NodeId == node.NodeId);
            var movements = balanceInputInfo.Movements.Where(y => y.DestinationNodeId == node.NodeId || y.SourceNodeId == node.NodeId);
            var calculationInput = new CalculationInput();
            initialInventories.ForEach(x =>
            {
                AddToDictionary(calculationInput.ProductsInput, x.ProductId, x.ProductName);
            });
            finalInventories.ForEach(x =>
            {
                AddToDictionary(calculationInput.ProductsInput, x.ProductId, x.ProductName);
            });
            movements.Where(x => x.SourceNodeId == node.NodeId).ForEach(x =>
            {
                AddToDictionary(calculationInput.ProductsInput, x.SourceProductId, x.SourceProductName);
            });
            movements.Where(x => x.DestinationNodeId == node.NodeId).ForEach(x =>
            {
                AddToDictionary(calculationInput.ProductsInput, x.DestinationProductId, x.DestinationProductName);
            });

            calculationInput.Node = node;
            calculationInput.TicketId = ticket.TicketId;
            calculationInput.CalculationDate = balanceInputInfo.CalculationDate;
            calculationInput.Movements = movements;
            calculationInput.InitialInventories = initialInventories;
            calculationInput.FinalInventories = finalInventories;
            return calculationInput;
        }

        private static void AddToDictionary(IDictionary<string, string> products, string productId, string productName)
        {
            if (productId != null && !products.ContainsKey(productId))
            {
                products.Add(productId, productName);
            }
        }
    }
}
