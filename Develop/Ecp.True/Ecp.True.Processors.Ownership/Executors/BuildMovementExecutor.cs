// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildMovementExecutor.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Castle.Core.Internal;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Query;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Services.Interfaces;

    /// <summary>
    /// The RegisterOwnershipWithMovementsExecutor class.
    /// </summary>
    public class BuildMovementExecutor : ExecutorBase
    {
        /// <summary>
        /// The ownership service.
        /// </summary>
        private readonly IOwnershipService ownershipService;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildMovementExecutor"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="ownershipService">The ownershipService.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public BuildMovementExecutor(ITrueLogger<BuildMovementExecutor> logger, IOwnershipService ownershipService, IUnitOfWorkFactory unitOfWorkFactory)
            : base(logger)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));

            this.ownershipService = ownershipService;
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        }

        /// <inheritdoc/>
        public override int Order => 8;

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
            var commercialMovementsResults = ownershipRuleData.OwnershipRuleResponse.CommercialMovementsResults;
            var newMovements = ownershipRuleData.OwnershipRuleResponse.NewMovements;
            var cancellationMovements = ownershipRuleData.CancellationMovements ?? new List<CancellationMovementDetail>();

            if (!commercialMovementsResults.IsNullOrEmpty() || !newMovements.IsNullOrEmpty() || !cancellationMovements.IsNullOrEmpty())
            {
                var result = await this.ownershipService.BuildOwnershipMovementResultsAsync(
                    commercialMovementsResults,
                    newMovements,
                    cancellationMovements,
                    ownershipRuleData.TicketId,
                    this.unitOfWork).ConfigureAwait(false);
                ownershipRuleData.Movements = result;
            }

            this.ShouldContinue = true;
            await this.ExecuteNextAsync(ownershipRuleData).ConfigureAwait(false);
        }
    }
}
