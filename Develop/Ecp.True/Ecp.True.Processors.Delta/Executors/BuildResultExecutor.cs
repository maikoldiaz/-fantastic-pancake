// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildResultExecutor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Executors
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Interfaces;
    using EfCore.Models;

    /// <summary>
    /// Build Result Executor.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.Execution.ExecutorBase" />
    public class BuildResultExecutor : ExecutorBase
    {
        /// <summary>
        /// delta strategy factory.
        /// </summary>
        private readonly IDeltaStrategyFactory deltaProcessorStrategyFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildResultExecutor"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="deltaProcessorStrategyFactory">The delta processor strategy.</param>
        public BuildResultExecutor(ITrueLogger<BuildResultExecutor> logger, IDeltaStrategyFactory deltaProcessorStrategyFactory)
                   : base(logger)
        {
            this.deltaProcessorStrategyFactory = deltaProcessorStrategyFactory;
        }

        /// <summary>
        /// Gets the Order.
        /// </summary>
        public override int Order => 6;

        /// <summary>
        /// Gets the type of the process.
        /// </summary>
        /// <value>
        /// The type of the process.
        /// </value>
        public override ProcessType ProcessType => ProcessType.Delta;

        /// <summary>
        /// The ExecuteAsync.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The Task.</returns>
        public override async Task ExecuteAsync(object input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            var deltaData = (DeltaData)input;

            // add service call for transformation
            await this.BuildAsync(deltaData).ConfigureAwait(false);
            this.ShouldContinue = true;

            await this.ExecuteNextAsync(deltaData).ConfigureAwait(false);
        }

        /// <summary>
        /// validate movement.
        /// </summary>
        /// <param name="movement">the movement.</param>
        /// <param name="nodeTags">the node tag.</param>
        /// <returns>boolean.</returns>
        private static bool Validate(Movement movement, IEnumerable<NodeTag> nodeTags)
        {
            if (movement.MovementSource != null && nodeTags.Any(x => x.NodeId == movement.MovementSource.SourceNodeId))
            {
                return true;
            }

            if (movement.MovementDestination != null && nodeTags.Any(x => x.NodeId == movement.MovementDestination.DestinationNodeId))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Filter movements.
        /// </summary>
        /// <param name="movements">The movements.</param>
        /// <param name="deltaData">The delta data.</param>
        /// <returns>tuple of movement and deltaerror.</returns>
        private (IEnumerable<Movement>, IEnumerable<DeltaError>) Filter(IEnumerable<Movement> movements, DeltaData deltaData)
        {
            var resultMovements = new List<Movement>();
            var deltaErrors = new List<DeltaError>();
            foreach (var movement in movements)
            {
                if (Validate(movement, deltaData.NodeTags))
                {
                    resultMovements.Add(movement);
                }
                else
                {
                    if (!deltaErrors.Any(a => a.MovementTransactionId == movement.SourceMovementTransactionId && a.InventoryProductId == movement.SourceInventoryProductId))
                    {
                        deltaErrors.Add(new DeltaError
                        {
                            MovementTransactionId = movement.SourceMovementTransactionId,
                            InventoryProductId = movement.SourceInventoryProductId,
                            TicketId = deltaData.Ticket.TicketId,
                            ErrorMessage = string.Format(CultureInfo.InvariantCulture, Constants.DeltaRegistrationFailedMessage, deltaData.NextCutOffDate),
                        });
                    }
                }
            }

            return (resultMovements, deltaErrors);
        }

        /// <summary>
        /// update movement and inventory data.
        /// </summary>
        /// <param name="deltaData">The deltaData.</param>
        /// <returns>delta data.</returns>
        private Task<DeltaData> BuildAsync(DeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));
            var deltaErrors = new List<DeltaError>();
            var generatedMovements = new List<Movement>();
            var (movementResult, errorMovement) = this.Filter(this.deltaProcessorStrategyFactory.MovementDeltaStrategy.Build(deltaData), deltaData);
            var (inventoryResult, errorInventory) = this.Filter(this.deltaProcessorStrategyFactory.InventoryDeltaStrategy.Build(deltaData), deltaData);

            generatedMovements.AddRange(movementResult);
            generatedMovements.AddRange(inventoryResult);

            deltaErrors.AddRange(errorMovement);
            deltaErrors.AddRange(errorInventory);

            deltaData.GeneratedMovements = generatedMovements;
            deltaData.DeltaErrors = deltaErrors;

            return Task.FromResult(deltaData);
        }
    }
}
