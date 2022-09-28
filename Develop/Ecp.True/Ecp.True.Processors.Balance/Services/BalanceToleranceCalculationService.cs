// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BalanceToleranceCalculationService.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Balance.Calculation.Input;
    using Ecp.True.Processors.Balance.Calculation.Interfaces;
    using Ecp.True.Processors.Balance.Calculation.Output;
    using Ecp.True.Processors.Balance.Operation;
    using Ecp.True.Processors.Balance.Operation.Input;
    using Ecp.True.Processors.Balance.Operation.Interfaces;

    /// <summary>
    /// Interface Calculator.
    /// </summary>
    public class BalanceToleranceCalculationService : CalculationService
    {
        /// <summary>
        /// The unbalance calculator.
        /// </summary>
        private readonly IBalanceToleranceCalculator balanceToleranceCalculator;

        /// <summary>
        /// The movement generators.
        /// </summary>
        private readonly IBalanceToleranceMovementGenerator balanceToleranceMovementGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BalanceToleranceCalculationService" /> class.
        /// </summary>
        /// <param name="balanceToleranceCalculator">The balance tolerance calculator.</param>
        /// <param name="balanceToleranceMovementGenerator">The movement generators.</param>
        public BalanceToleranceCalculationService(
            IBalanceToleranceCalculator balanceToleranceCalculator,
            IBalanceToleranceMovementGenerator balanceToleranceMovementGenerator)
        {
            this.balanceToleranceCalculator = balanceToleranceCalculator;
            this.balanceToleranceMovementGenerator = balanceToleranceMovementGenerator;
        }

        /// <inheritdoc/>
        public override MovementCalculationStep Type => MovementCalculationStep.BalanceTolerances;

        /// <inheritdoc/>
        public override async Task<CalculationOutput> ProcessAsync(
            BalanceInputInfo balanceInputInfo, Ticket ticket, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(balanceInputInfo, nameof(balanceInputInfo));
            var (balanceTolerances, movements) = await this.ProcessBalanceTolerancesAsync(balanceInputInfo, ticket, balanceInputInfo.CalculationDate).ConfigureAwait(false);
            var unbalances = GetBalanceToleranceInUnbalance(ticket, balanceInputInfo, unitOfWork, balanceTolerances);
            return new CalculationOutput(movements, unbalances);
        }

        /// <summary>
        /// Updates the unbalance asynchronous.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="balanceInputInfo">The balance input information.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="balanceTolerances">The balance tolerances.</param>
        /// <returns>The list of unbalances.</returns>
        private static IEnumerable<Unbalance> GetBalanceToleranceInUnbalance(
                Ticket ticket,
                BalanceInputInfo balanceInputInfo,
                IUnitOfWork unitOfWork,
                IEnumerable<BalanceTolerance> balanceTolerances)
        {
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));

            var newUnbalances = balanceTolerances
                .Select(x => new Unbalance
                {
                    NodeId = x.NodeId,
                    ProductId = x.ProductId,
                    TicketId = ticket.TicketId,
                    Tolerance = x.Tolerance,
                    ToleranceUnbalance = x.Unbalance,
                    StandardUncertainty = x.StandardUncertainty,
                    ToleranceIdentifiedLosses = x.IdentifiedLosses,
                    ToleranceInputs = x.Inputs,
                    ToleranceOutputs = x.Outputs,
                    ToleranceInitialInventory = x.InitialInventory,
                    ToleranceFinalInventory = x.FinalInventory,
                    InitialInventory = x.InitialInventory,
                    Inputs = x.Inputs,
                    Outputs = x.Outputs,
                    FinalInventory = x.FinalInventory,
                    IdentifiedLosses = x.IdentifiedLosses,
                    InterfaceUnbalance = 0.00M,
                    Interface = 0.00M,
                    CalculationDate = balanceInputInfo.CalculationDate,
                });

            return newUnbalances;
        }

        private async Task<(IEnumerable<BalanceTolerance> balanceTolerances, IEnumerable<Movement> movements)> ProcessBalanceTolerancesAsync(
            BalanceInputInfo balanceInputInfo, Ticket ticket, DateTime calculationDate)
        {
            var calculationInputs = balanceInputInfo.Nodes.Select(x => BuildCalculationInput(balanceInputInfo, x, ticket));
            var movements = new List<Movement>();
            var balanceTolerances = new List<BalanceTolerance>();
            foreach (var calculationInput in calculationInputs)
            {
                var tolerances = (IEnumerable<BalanceTolerance>)await this.balanceToleranceCalculator.CalculateAsync(calculationInput).ConfigureAwait(false);
                var generatedMovements = await this.balanceToleranceMovementGenerator
                .GenerateAsync(new MovementInput(tolerances, ticket, calculationDate)).ConfigureAwait(false);
                movements.AddRange(generatedMovements);
                balanceTolerances.AddRange(tolerances);
            }

            return (balanceTolerances, movements);
        }
    }
}
