// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InterfaceCalculator.cs" company="Microsoft">
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
    using Ecp.True.Logging;
    using Ecp.True.Processors.Balance.Calculation.Input;
    using Ecp.True.Processors.Balance.Calculation.Interfaces;
    using Ecp.True.Processors.Balance.Calculation.Output;

    /// <summary>
    /// Interface Calculator.
    /// </summary>
    public class InterfaceCalculator : IInterfaceCalculator
    {
        /// <summary>
        /// The unbalance calculator.
        /// </summary>
        private readonly IUnbalanceCalculator unbalanceCalculator;

        /// <summary>
        /// The unbalance calculator.
        /// </summary>
        private readonly ITrueLogger<InterfaceCalculator> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="InterfaceCalculator"/> class.
        /// </summary>
        /// <param name="unbalanceCalculator">The unbalance calculator.</param>
        /// <param name="logger">The logger.</param>
        public InterfaceCalculator(IUnbalanceCalculator unbalanceCalculator, ITrueLogger<InterfaceCalculator> logger)
        {
            this.unbalanceCalculator = unbalanceCalculator;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IOutput>> CalculateAsync(CalculationInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            this.logger.LogInformation($"Interface calculation triggered for input: {input.TicketId}");
            var unbalances = await this.unbalanceCalculator.CalculateAsync(input).ConfigureAwait(false);
            var totalInputs = unbalances.Sum(x => x.Inputs);
            var totalUnbalances = unbalances.Sum(x => x.Unbalance);
            var interfaces = unbalances.Select(x => new InterfaceInfo
            {
                NodeId = x.NodeId,
                ProductId = x.ProductId,
                InitialInventory = x.InitialInventory,
                Inputs = x.Inputs,
                Outputs = x.Outputs,
                FinalInventory = x.FinalInventory,
                IdentifiedLosses = x.IdentifiedLosses,
                Unbalance = x.Unbalance,
                Interface = totalInputs == 0 ? 0 : Math.Round(x.Unbalance - ((x.Inputs * totalUnbalances) / totalInputs), 2),
            });
            return interfaces;
        }
    }
}
