// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnbalanceCalculationService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Balance.Calculation
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
    using Ecp.True.Processors.Balance.Calculation.Interfaces;
    using Ecp.True.Processors.Balance.Calculation.Output;
    using Ecp.True.Processors.Balance.Operation;

    /// <summary>
    /// Interface Calculator.
    /// </summary>
    public class UnbalanceCalculationService : CalculationService
    {
        /// <summary>
        /// The unbalance calculator.
        /// </summary>
        private readonly IUnbalanceCalculator unbalanceCalculator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnbalanceCalculationService"/> class.
        /// </summary>
        /// <param name="unbalanceCalculator">The unbalance calculator.</param>
        /// <param name="movementGenerators">The movement generators.</param>
        public UnbalanceCalculationService(IUnbalanceCalculator unbalanceCalculator)
        {
            this.unbalanceCalculator = unbalanceCalculator;
        }

        /// <inheritdoc/>
        public override MovementCalculationStep Type => MovementCalculationStep.Unbalance;

        /// <inheritdoc/>
        public override async Task<CalculationOutput> ProcessAsync(
            BalanceInputInfo balanceInputInfo, Ticket ticket, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(balanceInputInfo, nameof(balanceInputInfo));
            var unbalances = await this.ProcessUnbalancesAsync(balanceInputInfo, ticket).ConfigureAwait(false);
            var unbalanceList = GetUnbalances(unbalances);

            return new CalculationOutput(unbalances, unbalanceList);
        }

        /// <summary>
        /// Updates the un identified loss information in unbalance asynchronous.
        /// </summary>
        /// <param name="unbalanceComments">The unbalances.</param>
        /// <returns>The Task.</returns>
        private static IEnumerable<Unbalance> GetUnbalances(
                IEnumerable<UnbalanceComment> unbalanceComments)
        {
            var newUnbalances = unbalanceComments
                .Select(x => new Unbalance
                {
                    NodeId = x.NodeId,
                    ProductId = x.ProductId,
                    UnbalanceAmount = x.Unbalance,
                    CalculationDate = x.CalculationDate,
                });

            return newUnbalances;
        }

        private async Task<IEnumerable<UnbalanceComment>> ProcessUnbalancesAsync(
            BalanceInputInfo balanceInputInfo, Ticket ticket)
        {
            var unbalances = new List<UnbalanceComment>();
            foreach (var node in balanceInputInfo.Nodes)
            {
                unbalances.AddRange(await this.unbalanceCalculator.CalculateAsync(BuildCalculationInput(balanceInputInfo, node, ticket)).ConfigureAwait(false));
            }

            return unbalances;
        }
    }
}
