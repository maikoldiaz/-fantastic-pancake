// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildExecutor.cs" company="Microsoft">
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
    using Ecp.True.Processors.Ownership.Services.Interfaces;

    /// <summary>
    /// The BuildOwnershipExecutor class.
    /// </summary>
    public class BuildExecutor : ExecutorBase
    {
        /// <summary>
        /// The ownership service.
        /// </summary>
        private readonly IOwnershipService ownershipService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildExecutor"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="ownershipService">The ownershipService.</param>
        public BuildExecutor(ITrueLogger<BuildExecutor> logger, IOwnershipService ownershipService)
            : base(logger)
        {
            this.ownershipService = ownershipService;
        }

        /// <inheritdoc/>
        public override int Order => 7;

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
            var inventoryResultList = this.ownershipService.ConsolidateInventoryResults(
                            ownershipRuleData.OwnershipRuleResponse.InventoryResults,
                            ownershipRuleData.OwnershipRuleRequest.PreviousInventoryOperationalData,
                            ownershipRuleData.TicketId);

            var movementResultList = this.ownershipService.ConsolidateMovementResults(
                            ownershipRuleData.OwnershipRuleResponse.MovementResults,
                            ownershipRuleData.OwnershipRuleRequest.PreviousMovementsOperationalData,
                            ownershipRuleData.TicketId);

            ownershipRuleData.Ownerships = this.ownershipService.BuildOwnershipResults(inventoryResultList, movementResultList);

            this.ShouldContinue = true;
            await this.ExecuteNextAsync(ownershipRuleData).ConfigureAwait(false);
        }
    }
}
