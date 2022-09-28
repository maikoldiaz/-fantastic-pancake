// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfoBuildExecutor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Executors
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Query;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.Request;
    using Constants = Ecp.True.Core.Constants;

    /// <summary>
    /// The InfoBuildExecutor class.
    /// </summary>
    public class InfoBuildExecutor : ExecutorBase
    {
        /// <summary>
        /// The ownership calculation service.
        /// </summary>
        private readonly IOwnershipCalculationService ownershipCalculationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="InfoBuildExecutor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="ownershipCalculationService">The ownershipCalculationService.</param>
        public InfoBuildExecutor(ITrueLogger<InfoBuildExecutor> logger, IOwnershipCalculationService ownershipCalculationService)
            : base(logger)
        {
            this.ownershipCalculationService = ownershipCalculationService;
        }

        /// <inheritdoc/>
        public override int Order => 2;

        /// <summary>
        /// Gets the type of the process.
        /// </summary>
        /// <value>
        /// The type of the process.
        /// </value>
        public override ProcessType ProcessType => ProcessType.Ownership;

        /// <inheritdoc/>
        public override async Task ExecuteAsync(object input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));

            var ownershipRuleData = (OwnershipRuleData)input;
            await this.ownershipCalculationService.PopulateOwnershipRuleRequestDataAsync(ownershipRuleData).ConfigureAwait(false);

            // Building and concating the analytics data
            if (ownershipRuleData.TransferPointMovements.Any())
            {
                ownershipRuleData.OwnershipRuleRequest.PreviousMovementsOperationalData = BuildInputMovementOperationalData(ownershipRuleData);
            }

            FormatOwnershipRuleRequest(ownershipRuleData.OwnershipRuleRequest);
            this.ShouldContinue = true;

            await this.ExecuteNextAsync(ownershipRuleData).ConfigureAwait(false);
        }

        private static IEnumerable<PreviousMovementOperationalData> BuildInputMovementOperationalData(
            OwnershipRuleData ownershipRuleData)
        {
            var transferPointMovements = ownershipRuleData.TransferPointMovements.Where(m => m.OwnershipVolume.HasValue).Select(m => new PreviousMovementOperationalData
            {
                MovementId = m.MovementTransactionId,
                OwnerId = m.OwnerId,
                OwnershipVolume = m.OwnershipVolume.ToTrueDecimal(),
                OwnershipPercentage = m.OwnershipPercentage.ToTrueDecimal(),
                AppliedRule = m.AlgorithmId,
            });

            var inputMovements = new List<PreviousMovementOperationalData>(transferPointMovements);
            inputMovements.AddRange(ownershipRuleData.OwnershipRuleRequest.PreviousMovementsOperationalData.Where(x => !transferPointMovements.Any(y => y.MovementId == x.MovementId)));

            return inputMovements;
        }

        private static void FormatOwnershipRuleRequest(OwnershipRuleRequest ownershipRuleRequest)
        {
            FormatMovementsOperationalData(ownershipRuleRequest);
            FormatInventoryOperationalData(ownershipRuleRequest);
        }

        private static void FormatMovementsOperationalData(OwnershipRuleRequest ownershipRuleRequest)
        {
            if (ownershipRuleRequest.MovementsOperationalData.Any())
            {
                foreach (var movementsOperationalData in ownershipRuleRequest.MovementsOperationalData.Where(x => !string.IsNullOrEmpty(x.OwnershipUnit)))
                {
                    movementsOperationalData.OwnershipUnit = ValidatePercentageandVolume(movementsOperationalData.OwnershipUnit);
                }
            }
        }

        private static void FormatInventoryOperationalData(OwnershipRuleRequest ownershipRuleRequest)
        {
            if (ownershipRuleRequest.InventoryOperationalData.Any())
            {
                foreach (var inventoryOperationalData in ownershipRuleRequest.InventoryOperationalData.Where(x => !string.IsNullOrEmpty(x.OwnershipUnit)))
                {
                    inventoryOperationalData.OwnershipUnit = ValidatePercentageandVolume(inventoryOperationalData.OwnershipUnit);
                }
            }
        }

        private static string ValidatePercentageandVolume(string ownershipUnit)
        {
            if (ownershipUnit == Constants.OwnershipPercentageUnit || ownershipUnit.ToUpper(System.Globalization.CultureInfo.InvariantCulture) == "% VOLUMEN")
            {
                return Constants.Percentage;
            }

            return Constants.Volume;
        }
    }
}