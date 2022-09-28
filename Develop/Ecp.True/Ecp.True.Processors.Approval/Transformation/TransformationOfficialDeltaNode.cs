// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformationOfficialDeltaNode.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Approval.Transformation
{
    using System;
    using System.Collections.Generic;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Approval.Interfaces;

    /// <summary>
    /// the TransformationOfficialDeltaNode.
    /// </summary>
    public class TransformationOfficialDeltaNode : ITransformationOfficialDeltaNode
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<TransformationOfficialDeltaNode> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformationOfficialDeltaNode"/> class.
        /// Constructor TransformationOfficialDeltaNode.
        /// </summary>
        /// <param name="logger">logger.</param>
        public TransformationOfficialDeltaNode(ITrueLogger<TransformationOfficialDeltaNode> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Apply Transformation Official Delta.
        /// </summary>
        /// <param name="movements">movements.</param>
        /// <param name="dateCutOff">date Cut off.</param>
        /// <returns>IEnumerable Movement.</returns>
        public IEnumerable<Movement> ApplyTransformationOfficialDelta(IEnumerable<OfficialDeltaNodeMovement> movements, DateTime dateCutOff)
        {
            var transformedMovemens = new List<Movement>();
            movements.ForEach(movement => transformedMovemens.Add(this.InitializationTransformedMovement(movement, dateCutOff)));

            return transformedMovemens;
        }

        /// <summary>
        /// InitializeMovement.
        /// </summary>
        /// <param name="movement">movement.</param>
        /// <param name="dateCutOff">date Cut off.</param>
        /// <returns>Movement.</returns>
        private static Movement InitializeMovement(OfficialDeltaNodeMovement movement, DateTime dateCutOff)
        {
            var finalMovement = new Movement();
            finalMovement.Period = new MovementPeriod();
            finalMovement.MessageTypeId = (int)MessageType.Movement;
            finalMovement.SystemTypeId = (int)SystemType.TRUE;
            finalMovement.EventType = EventType.Insert.ToString();
            finalMovement.IsDeleted = false;
            finalMovement.Classification = string.Empty;
            finalMovement.IsSystemGenerated = true;
            finalMovement.SegmentId = movement.SegmentId;
            finalMovement.OperationalDate = dateCutOff;
            finalMovement.BlockchainStatus = StatusType.PROCESSING;
            finalMovement.MovementId = Constants.DeltaOfficialPrefix + Guid.NewGuid();
            finalMovement.BlockchainMovementTransactionId = Guid.NewGuid();
            finalMovement.Period.StartTime = dateCutOff;
            finalMovement.Period.EndTime = dateCutOff;
            finalMovement.NetStandardVolume = movement.NetStandardVolume;
            finalMovement.GrossStandardVolume = movement.GrossStandardVolume;
            finalMovement.MeasurementUnit = movement.MeasurementUnit;
            finalMovement.ScenarioId = ScenarioType.OPERATIONAL;
            finalMovement.Version = movement.Version;
            finalMovement.SourceSystemId = (int)SourceSystem.TRUE;
            finalMovement.BackupMovementId = movement.MovementId;
            finalMovement.OriginalMovementTransactionId = movement.MovementTransactionId;

            finalMovement.MovementSource = new MovementSource
            {
                SourceNodeId = movement.DestinationNodeId,
                SourceProductId = movement.DestinationProductId,
            };
            finalMovement.MovementDestination = new MovementDestination
            {
                DestinationNodeId = movement.SourceNodeId,
                DestinationProductId = movement.SourceProductId,
            };

            finalMovement.Owners.Add(new Owner
            {
                OwnerId = movement.OwnerId,
                OwnershipValue = movement.OwnershipValue,
                OwnershipValueUnit = movement.OwnershipValueUnit,
                BlockchainStatus = StatusType.PROCESSING,
            });

            return finalMovement;
        }

        /// <summary>
        /// Initialization Transformed Movement.
        /// </summary>
        /// <param name="movement">movement.</param>
        /// <param name="dateCutOff">date Cut off.</param>
        /// <returns>movement..</returns>
        private Movement InitializationTransformedMovement(OfficialDeltaNodeMovement movement, DateTime dateCutOff)
        {
            var finalMovement = InitializeMovement(movement, dateCutOff);

            switch (movement.MovementTypeId)
            {
                case (int)MovementType.DeltaInventory:
                    finalMovement.MovementTypeId = (int)MovementType.DeltaInventory;
                    break;
                case (int)MovementType.InputEvacuation:
                    finalMovement.MovementTypeId = (int)MovementType.DeltaAnnulationEMEvacuation;
                    break;
                case (int)MovementType.OutputEvacuation:
                    finalMovement.MovementTypeId = (int)MovementType.DeltaAnnulationSMEvacuation;
                    break;
                default:
                    this.logger.LogInformation($"Type of movement not contemplated", $"{movement.MovementTransactionId}");
                    break;
            }

            return finalMovement;
        }
    }
}
