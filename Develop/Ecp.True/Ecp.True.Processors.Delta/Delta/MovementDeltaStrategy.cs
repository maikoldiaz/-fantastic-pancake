// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementDeltaStrategy.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Delta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Delta.Entities;

    /// <summary>
    /// the movement delta strategy.
    /// </summary>
    public class MovementDeltaStrategy : DeltaStrategyBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovementDeltaStrategy" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public MovementDeltaStrategy(
           ITrueLogger logger)
           : base(logger)
        {
        }

        /// <summary>
        /// build inventory object.
        /// </summary>
        /// <param name="deltaData">The deltaData.</param>
        /// <returns>inventory.</returns>
        public override IEnumerable<Movement> Build(DeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));
            return this.DoBuild(deltaData);
        }

        private IEnumerable<Movement> DoBuild(DeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));
            var movements = new List<Movement>();
            foreach (var movementResult in deltaData.ResultMovements)
            {
                // to build actual movement
                var updatedMovement = deltaData.Movements.First(x => x.MovementTransactionId == movementResult.MovementTransactionId);
                var originalMovementRecord = deltaData.OriginalMovements.SingleOrDefault(x => x.MovementId.Equals(movementResult.MovementId, StringComparison.Ordinal));

                Movement originalMovement = null;
                if (originalMovementRecord != null)
                {
                    originalMovement = deltaData.Movements.First(x => x.MovementTransactionId == originalMovementRecord.MovementTransactionId);
                }

                if (movementResult.Sign)
                {
                    movements.Add(this.GetPositiveMovement(updatedMovement, originalMovement, movementResult, deltaData));
                }
                else
                {
                    movements.AddRange(this.GetNegativeMovements(updatedMovement, originalMovement, movementResult, deltaData.CancellationTypes, deltaData));
                }
            }

            return movements;
        }

        private Movement GetPositiveMovement(
            Movement updatedMovement,
            Movement originalMovement,
            ResultMovement movementResult,
            DeltaData deltaData)
        {
            var movement = this.BuildMovement(updatedMovement, originalMovement, movementResult.Delta, deltaData);
            if (updatedMovement.MovementSource != null)
            {
                movement.MovementSource = new MovementSource
                {
                    SourceNodeId = updatedMovement.MovementSource.SourceNodeId,
                    SourceProductId = updatedMovement.MovementSource.SourceProductId,
                };
            }

            if (updatedMovement.MovementDestination != null)
            {
                movement.MovementDestination = new MovementDestination
                {
                    DestinationNodeId = updatedMovement.MovementDestination.DestinationNodeId,
                    DestinationProductId = updatedMovement.MovementDestination.DestinationProductId,
                };
            }

            return movement;
        }

        private IEnumerable<Movement> GetNegativeMovements(
            Movement updatedMovement,
            Movement originalMovement,
            ResultMovement movementResult,
            IEnumerable<Annulation> cancellationTypes,
            DeltaData deltaData)
        {
            var movements = new List<Movement>();
            var cancellationType = cancellationTypes.First(x => x.SourceMovementTypeId == updatedMovement.MovementTypeId).AnnulationMovementTypeId;
            if (updatedMovement.MovementDestination != null)
            {
                var destinationMovement = this.BuildMovement(updatedMovement, originalMovement, movementResult.Delta, deltaData);
                destinationMovement.OriginalMovementTransactionId = originalMovement.MovementTransactionId;

                destinationMovement.MovementSource = new MovementSource
                {
                    SourceNodeId = updatedMovement.MovementDestination.DestinationNodeId,
                    SourceProductId = updatedMovement.MovementDestination.DestinationProductId,
                };

                destinationMovement.MovementTypeId = cancellationType;
                movements.Add(destinationMovement);
            }

            // movement has only the destination node.
            if (updatedMovement.MovementSource != null)
            {
                var sourceMovement = this.BuildMovement(updatedMovement, originalMovement, movementResult.Delta, deltaData);
                sourceMovement.OriginalMovementTransactionId = originalMovement.MovementTransactionId;

                sourceMovement.MovementDestination = new MovementDestination
                {
                    DestinationNodeId = updatedMovement.MovementSource.SourceNodeId,
                    DestinationProductId = updatedMovement.MovementSource.SourceProductId,
                };
                sourceMovement.MovementTypeId = cancellationType;
                movements.Add(sourceMovement);
            }

            return movements;
        }

        /// <summary>
        /// CreateSourceDestinationMovement.
        /// </summary>
        /// <param name="updatedMovement">The updatedMovement.</param>
        /// <param  name="delta">The delta.</param>
        /// <returns>movement.</returns>
        private Movement BuildMovement(
            Movement updatedMovement,
            Movement originalMovement,
            decimal delta,
            DeltaData deltaData)
        {
            var movement = this.CreateMovement(deltaData, delta);
            movement.MovementTypeId = updatedMovement.MovementTypeId;
            movement.MeasurementUnit = updatedMovement.MeasurementUnit;
            movement.SourceMovementTransactionId = updatedMovement.MovementTransactionId;

            if (originalMovement != null)
            {
                movement.Owners.AddRange(this.CalculateOwners(originalMovement.Ownerships, delta));
            }

            return movement;
        }
    }
}
