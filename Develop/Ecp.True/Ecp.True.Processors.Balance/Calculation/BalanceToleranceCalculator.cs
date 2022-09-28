// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BalanceToleranceCalculator.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Balance.Calculation.Input;
    using Ecp.True.Processors.Balance.Calculation.Interfaces;
    using Ecp.True.Processors.Balance.Calculation.Output;

    /// <summary>
    /// Balance tolerance Calculator.
    /// </summary>
    public class BalanceToleranceCalculator : IBalanceToleranceCalculator
    {
        /// <summary>
        /// The unbalance calculator.
        /// </summary>
        private readonly IUnbalanceCalculator unbalanceCalculator;

        /// <summary>
        /// The unbalance calculator.
        /// </summary>
        private readonly ITrueLogger<BalanceToleranceCalculator> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BalanceToleranceCalculator" /> class.
        /// </summary>
        /// <param name="unbalanceCalculator">The unbalance calculator.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <param name="logger">The logger.</param>
        public BalanceToleranceCalculator(IUnbalanceCalculator unbalanceCalculator, ITrueLogger<BalanceToleranceCalculator> logger)
        {
            this.unbalanceCalculator = unbalanceCalculator;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IOutput>> CalculateAsync(CalculationInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            this.logger.LogInformation($"Balance tolerance calculation triggered for input: {input.TicketId}");
            var unbalances = await this.unbalanceCalculator.CalculateAsync(input).ConfigureAwait(false);
            var balanceIntolerancePercentage = CalculateTolerance(unbalances, input);

            var tolerances = unbalances.Select(x =>
            {
                // Multiply the sum of the inputs by the balance tolerance percentage.
                var standardUncertainty = x.Inputs * balanceIntolerancePercentage;

                // Multiply the standard uncertainty by the node control limit.
                var controlLimit = x.ControlLimit;
                var expandedUncertainty = standardUncertainty * controlLimit;

                // For each movement and inventory, the "Control Limit" attribute value used to calculate the expanded uncertainty must be recorded.
                return new BalanceTolerance
                {
                    NodeId = x.NodeId,
                    ProductId = x.ProductId,
                    InitialInventory = x.InitialInventory,
                    FinalInventory = x.FinalInventory,
                    IdentifiedLosses = x.IdentifiedLosses,
                    Unbalance = x.Unbalance,
                    Inputs = x.Inputs,
                    Outputs = x.Outputs,
                    Tolerance = Math.Abs(x.Unbalance) <= expandedUncertainty ? x.Unbalance : Math.Round(expandedUncertainty, 2),
                    StandardUncertainty = standardUncertainty,
                };
            });

            return tolerances;
        }

        /// <summary>
        /// Calculates the inventories and movements in tolerance asynchronous.
        /// </summary>
        /// <param name="unbalances">The unbalances.</param>
        /// <param name="input">The input.</param>
        /// <returns>The balanceTolerancePercentage.</returns>
        private static decimal CalculateTolerance(IEnumerable<UnbalanceComment> unbalances, CalculationInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            var inventoryUncertaintySquared = input.InitialInventories.Concat(input.FinalInventories)
                    .Select(x => x.UncertaintyPercentage.GetValueOrDefault() * x.UncertaintyPercentage.GetValueOrDefault()
                    * x.ProductVolume.GetValueOrDefault() * x.ProductVolume.GetValueOrDefault() / 10000).Sum();

            var movementUncertaintySquared = input.Movements
                .Select(x => x.UncertaintyPercentage.GetValueOrDefault() * x.UncertaintyPercentage.GetValueOrDefault()
                * x.NetStandardVolume.GetValueOrDefault() * x.NetStandardVolume.GetValueOrDefault() / 10000).Sum();

            var balanceTolerance = Math.Sqrt(Convert.ToDouble(inventoryUncertaintySquared + movementUncertaintySquared, CultureInfo.InvariantCulture));
            var totalInputs = unbalances.Sum(x => x.Inputs);
            var balanceTolerancePercentage = totalInputs == 0
                ? 0 : Convert.ToDecimal(balanceTolerance) / totalInputs;

            return balanceTolerancePercentage;
        }
    }
}
