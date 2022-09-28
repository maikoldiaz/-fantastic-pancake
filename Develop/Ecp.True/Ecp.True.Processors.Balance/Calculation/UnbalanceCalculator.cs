// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnbalanceCalculator.cs" company="Microsoft">
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
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Balance.Calculation.Input;
    using Ecp.True.Processors.Balance.Calculation.Interfaces;

    /// <summary>
    /// Unbalance Calculator.
    /// </summary>
    public class UnbalanceCalculator : IUnbalanceCalculator
    {
        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// The ILogger.
        /// </summary>
        private readonly ITrueLogger<UnbalanceCalculator> logger;

        /// <summary>
        /// The default control limit.
        /// </summary>
        private decimal defaultControlLimit;

        /// <summary>
        /// The default acceptable balance.
        /// </summary>
        private decimal defaultAcceptableBalance;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnbalanceCalculator"/> class.
        /// </summary>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="logger">The logger.</param>
        public UnbalanceCalculator(IConfigurationHandler configurationHandler, ITrueLogger<UnbalanceCalculator> logger)
        {
            this.configurationHandler = configurationHandler;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UnbalanceComment>> CalculateAsync(CalculationInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            this.logger.LogInformation($"Unbalance calculation triggered for input: {input.TicketId}");
            var systemConfig = await this.configurationHandler.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings).ConfigureAwait(false);

            this.defaultControlLimit = systemConfig.ControlLimit.GetValueOrDefault();
            this.defaultAcceptableBalance = systemConfig.AcceptableBalancePercentage.GetValueOrDefault();

            var unbalanceComments = new List<UnbalanceComment>();
            input.ProductsInput.ForEach(product =>
            {
                var initialInventories = input.InitialInventories.Where(y => y.ProductId == product.Key);
                var finalInventories = input.FinalInventories.Where(y => y.ProductId == product.Key);

                var inputMovements = GetInputMovements(input, input.Movements, product.Key);
                var outputMovements = GetOutputMovements(input, input.Movements, product.Key);
                var lossMovements = GetLossMovements(input, input.Movements, product.Key);

                unbalanceComments.Add(this.DoCalculate(input, inputMovements, outputMovements, lossMovements, initialInventories, finalInventories, product));
            });

            return unbalanceComments;
        }

        /// <summary>
        /// Gets input movements.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="movements">The input movements.</param>
        /// <param name="productId">The product.</param>
        /// <returns>The movements.</returns>
        private static IEnumerable<MovementCalculationInput> GetInputMovements(
              CalculationInput input,
              IEnumerable<MovementCalculationInput> movements,
              string productId)
        {
            return movements.Where(x => (x.MessageTypeId == (int)MessageType.Movement || x.MessageTypeId == (int)MessageType.SpecialMovement)
                        && x.DestinationNodeId == input.Node.NodeId && x.DestinationProductId == productId);
        }

        /// <summary>
        /// Gets output movements.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="movements">The movements.</param>
        /// <param name="productId">The product.</param>
        /// <returns>The output movements.</returns>
        private static IEnumerable<MovementCalculationInput> GetOutputMovements(
              CalculationInput input,
              IEnumerable<MovementCalculationInput> movements,
              string productId)
        {
            return movements.Where(x => (x.MessageTypeId == (int)MessageType.Movement || x.MessageTypeId == (int)MessageType.SpecialMovement)
                        && x.SourceNodeId == input.Node.NodeId && x.SourceProductId == productId);
        }

        /// <summary>
        /// Gets loss movements.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="movements">The movements.</param>
        /// <param name="productId">The product.</param>
        /// <returns>The loss movements.</returns>
        private static IEnumerable<MovementCalculationInput> GetLossMovements(
              CalculationInput input,
              IEnumerable<MovementCalculationInput> movements,
              string productId)
        {
            return movements.Where(x => x.MessageTypeId == ((int)MessageType.Loss)
                        && x.SourceNodeId == input.Node.NodeId && x.SourceProductId == productId);
        }

        /// <summary>
        /// Does the calculate asynchronous.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="inputMovements">The input movements.</param>
        /// <param name="outputMovements">The output movements..</param>
        /// <param name="lossMovements">The identified loss movements.</param>
        /// <param name="initialInventories">The initial inventories.</param>
        /// <param name="finalInventories">The final inventories.</param>
        /// <param name="product">The product.</param>
        /// <returns>The UnbalanceComment.</returns>
        private UnbalanceComment DoCalculate(
              CalculationInput input,
              IEnumerable<MovementCalculationInput> inputMovements,
              IEnumerable<MovementCalculationInput> outputMovements,
              IEnumerable<MovementCalculationInput> lossMovements,
              IEnumerable<InventoryInput> initialInventories,
              IEnumerable<InventoryInput> finalInventories,
              KeyValuePair<string, string> product)
        {
            var initialInventoryValue = initialInventories.Sum(x => x.ProductVolume) ?? 0;
            var finalInventoryValue = finalInventories.Sum(x => x.ProductVolume) ?? 0;
            var inputs = inputMovements.Sum(x => x.NetStandardVolume) ?? 0;
            var outputs = outputMovements.Sum(x => x.NetStandardVolume) ?? 0;
            var identifiedLosses = lossMovements.Sum(x => x.NetStandardVolume) ?? 0;
            var unbalance = initialInventoryValue + inputs - outputs - finalInventoryValue - identifiedLosses;
            var percentage = inputs == 0 ? 0 : (Math.Abs(unbalance) / inputs) * 100;
            var percentageText = inputs == 0 ? "Error" : Math.Round(percentage, 2).ToString(CultureInfo.InvariantCulture);
            var unbalanceComment = new UnbalanceComment
            {
                NodeId = input.Node.NodeId,
                ProductId = product.Key,
                NodeName = input.Node.Name,
                ProductName = product.Value,
                Unbalance = unbalance,
                Units = "31",
                UnbalancePercentage = percentage,
                ControlLimit = input.Node.ControlLimit ?? this.defaultControlLimit,
                AcceptableBalance = input.Node.AcceptableBalancePercentage ?? this.defaultAcceptableBalance,
                InitialInventory = initialInventoryValue,
                Inputs = inputs,
                Outputs = outputs,
                FinalInventory = finalInventoryValue,
                IdentifiedLosses = identifiedLosses,
                CalculationDate = input.CalculationDate,
                UnbalancePercentageText = percentageText,
            };

            this.logger.LogInformation($"The unbalance for node {input.Node.NodeId} product {product.Key} on {input.CalculationDate}");
            return unbalanceComment;
        }
    }
}