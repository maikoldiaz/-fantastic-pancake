// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnidentifiedLossCalculationService.cs" company="Microsoft">
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
    public class UnidentifiedLossCalculationService : CalculationService
    {
        /// <summary>
        /// The unbalance calculator.
        /// </summary>
        private readonly IUnidentifiedLossCalculator unidentifiedLossCalculator;

        /// <summary>
        /// The movement generators.
        /// </summary>
        private readonly IUnidentifiedLossMovementGenerator unidentifiedLossMovementGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnidentifiedLossCalculationService"/> class.
        /// </summary>
        /// <param name="unidentifiedLossCalculator">The unidentified loss calculator.</param>
        /// <param name="unidentifiedLossMovementGenerator">The movement generators.</param>
        public UnidentifiedLossCalculationService(IUnidentifiedLossCalculator unidentifiedLossCalculator, IUnidentifiedLossMovementGenerator unidentifiedLossMovementGenerator)
        {
            this.unidentifiedLossCalculator = unidentifiedLossCalculator;
            this.unidentifiedLossMovementGenerator = unidentifiedLossMovementGenerator;
        }

        /// <inheritdoc/>
        public override MovementCalculationStep Type => MovementCalculationStep.UnidentifiedLosses;

        /// <inheritdoc/>
        public override async Task<CalculationOutput> ProcessAsync(
            BalanceInputInfo balanceInputInfo, Ticket ticket, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(balanceInputInfo, nameof(balanceInputInfo));
            var (unidentifiedLosses, movements) = await this.ProcessUnidentifiedLossesAsync(balanceInputInfo, ticket, balanceInputInfo.CalculationDate).ConfigureAwait(false);
            var unbalances = GetUnIdentifiedLossInfoInUnbalance(balanceInputInfo.CalculationDate, unidentifiedLosses);
            return new CalculationOutput(movements, unbalances);
        }

        /// <summary>
        /// Updates the un identified loss information in unbalance asynchronous.
        /// </summary>
        /// <param name="calculationDate">The calculation date.</param>
        /// <param name="unidentifiedLosses">The unidentified losses.</param>
        /// <returns>The Unbalances.</returns>
        private static IEnumerable<Unbalance> GetUnIdentifiedLossInfoInUnbalance(
                DateTime calculationDate,
                IEnumerable<UnIdentifiedLossInfo> unidentifiedLosses)
        {
            var newUnbalances = unidentifiedLosses
               .Select(x => new Unbalance
               {
                   NodeId = x.NodeId,
                   ProductId = x.ProductId,
                   UnidentifiedLosses = x.UnIdentifiedLoss,
                   CalculationDate = calculationDate,
                   UnidentifiedLossesUnbalance = x.Unbalance,
               });

            return newUnbalances;
        }

        private async Task<(IEnumerable<UnIdentifiedLossInfo> unIdentifiedLosses, IEnumerable<Movement> movements)> ProcessUnidentifiedLossesAsync(
            BalanceInputInfo balanceInputInfo, Ticket ticket, DateTime calculationDate)
        {
            var calculationInputs = balanceInputInfo.Nodes.Select(x => BuildCalculationInput(balanceInputInfo, x, ticket));
            var movements = new List<Movement>();
            var unidentifiedLosses = new List<UnIdentifiedLossInfo>();
            foreach (var calculationInput in calculationInputs)
            {
                var unidentifiedLossesList = (IEnumerable<UnIdentifiedLossInfo>)await this.unidentifiedLossCalculator.CalculateAsync(calculationInput).ConfigureAwait(false);
                var generatedMovements = await this.unidentifiedLossMovementGenerator
                .GenerateAsync(new MovementInput(unidentifiedLossesList, ticket, calculationDate)).ConfigureAwait(false);
                movements.AddRange(generatedMovements);
                unidentifiedLosses.AddRange(unidentifiedLossesList);
            }

            return (unidentifiedLosses, movements);
        }
    }
}
