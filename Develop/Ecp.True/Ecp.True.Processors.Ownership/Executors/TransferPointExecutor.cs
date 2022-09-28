// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransferPointExecutor.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Interfaces;

    /// <summary>
    /// The TransferPointExecutor class.
    /// </summary>
    public class TransferPointExecutor : ExecutorBase
    {
        /// <summary>
        /// The analytical calculation service.
        /// </summary>
        private readonly IAnalyticalOwnershipCalculationService analyticalCalculationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferPointExecutor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="analyticalCalculationService">The analytical calculation service.</param>
        public TransferPointExecutor(ITrueLogger<TransferPointExecutor> logger, IAnalyticalOwnershipCalculationService analyticalCalculationService)
            : base(logger)
        {
            this.analyticalCalculationService = analyticalCalculationService;
        }

        /// <inheritdoc/>
        public override int Order => 1;

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
            var transferPointMovements = await this.analyticalCalculationService.GetTransferPointMovementsAsync(ownershipRuleData.TicketId).ConfigureAwait(false);
            ownershipRuleData.TransferPointMovements = await this.analyticalCalculationService.GetOwnershipAnalyticalDataAsync(transferPointMovements).ConfigureAwait(false);
            this.ShouldContinue = true;

            await this.ExecuteNextAsync(ownershipRuleData).ConfigureAwait(false);
        }
    }
}