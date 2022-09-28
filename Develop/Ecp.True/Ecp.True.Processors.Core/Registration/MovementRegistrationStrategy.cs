// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementRegistrationStrategy.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// The movement registration strategy.
    /// </summary>
    public class MovementRegistrationStrategy : RegistrationStrategyBase
    {
        /// <summary>
        /// The movement registration service.
        /// </summary>
        private readonly IMovementRegistrationService movementRegistrationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementRegistrationStrategy" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="movementRegistrationService">The movement registration service.</param>
        public MovementRegistrationStrategy(
            ITrueLogger logger,
            IAzureClientFactory azureClientFactory,
            IMovementRegistrationService movementRegistrationService)
            : base(azureClientFactory, logger)
        {
            this.movementRegistrationService = movementRegistrationService;
        }

        /// <inheritdoc/>
        public override void Insert(IEnumerable<object> entities, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(entities, nameof(entities));
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            var movementRepository = unitOfWork.CreateRepository<Movement>();
            var movements = (IEnumerable<Movement>)entities;
            var movementsToInsert = GetMovements(movements);

            this.Logger.LogInformation($"Number of new movements {movementsToInsert.Count()}");

            movementRepository.InsertAll(movementsToInsert);
        }

        /// <summary>
        /// Registers asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="unitOfWork">The unitOfWork.</param>
        /// <returns>The task.</returns>
        public override async Task RegisterAsync(object entity, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var movement = (Movement)entity;

            this.Logger.LogInformation($"Starting movement registration for MovementId: {movement.MovementId}");

            var movementTransactionId = string.IsNullOrEmpty(movement.GlobalMovementId)
                ? await this.movementRegistrationService.RegisterMovementAsync(movement, unitOfWork, true).ConfigureAwait(false)
                : await this.RegisterOfficialandBackupMovementAsync(movement, unitOfWork).ConfigureAwait(false);

            this.Logger.LogInformation($"Finished movement registration for MovementId: {movement.MovementId}");

            if (movementTransactionId > 0)
            {
                // Send Message to blockchain queue to register it in blockchain.
                await this.SendToBlockchainAsync(movementTransactionId, QueueConstants.BlockchainMovementQueue).ConfigureAwait(false);
            }
        }

        private static SapTracking GetLastSapTrackingResult(IEnumerable<Movement> result)
        {
            var sapTrackings = result.SelectMany(x => x.SapTracking);
            if (sapTrackings.Any())
            {
                return sapTrackings.Where(x => x.StatusTypeId == StatusType.PROCESSED).OrderByDescending(x => x.OperationalDate).FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// Gets the movements.
        /// </summary>
        /// <param name="movements">The movements.</param>
        /// <returns>The collection of Movement.</returns>
        private static IEnumerable<Movement> GetMovements(IEnumerable<Movement> movements)
        {
            var movementsToInsert = new List<Movement>();
            foreach (var movement in movements)
            {
                if (movement.EventType.EqualsIgnoreCase(EventType.Insert.ToString("G")))
                {
                    var blockchainMovementTransactionId = Guid.NewGuid();
                    movement.IsSystemGenerated = true;
                    movement.BlockchainStatus = StatusType.PROCESSING;
                    movement.OperationalDate = movement.OperationalDate.Date;
                    movement.BlockchainMovementTransactionId = blockchainMovementTransactionId;
                    if (movement.PendingApproval != true)
                    {
                        movement.ScenarioId = ScenarioType.OPERATIONAL;
                    }

                    foreach (var ownership in movement.Ownerships)
                    {
                        ownership.MessageTypeId = MessageType.MovementOwnership;
                        ownership.BlockchainOwnershipId = Guid.NewGuid();
                        ownership.BlockchainMovementTransactionId = blockchainMovementTransactionId;
                        ownership.EventType = EventType.Insert.ToString();
                        ownership.BlockchainStatus = StatusType.PROCESSING;
                    }

                    movementsToInsert.Add(movement);
                }
            }

            return movementsToInsert;
        }

        private async Task<int> RegisterOfficialandBackupMovementAsync(Movement movement, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            movement.IsTransferPoint = true;

            this.Logger.LogInformation($"Starting official and backup movement registration for MovementId: {movement.MovementId}");

            var movementsFromTrue = await this.movementRegistrationService.GetMovementsWithSapTrackingAsync(movement.MovementId, unitOfWork).ConfigureAwait(false);
            if (movementsFromTrue == null || !movementsFromTrue.Any())
            {
                //// Movement not available in true , register the movement in true.
                this.Logger.LogInformation($"Registering as movement not available in TRUE for MovementId: {movement.MovementId}");
                movement.EventType = EventType.Insert.ToString();
                return await this.movementRegistrationService.RegisterMovementAsync(movement, unitOfWork, false).ConfigureAwait(false);
            }

            var sapTrackingResult = GetLastSapTrackingResult(movementsFromTrue);

            //// Movement not sent to SAP.
            if (sapTrackingResult == null)
            {
                var latestMovement = movementsFromTrue.OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if (latestMovement.EventType.EqualsIgnoreCase(EventType.Delete.ToString("G")))
                {
                    movement.EventType = EventType.Insert.ToString();
                    return await this.movementRegistrationService.RegisterMovementAsync(movement, unitOfWork, false).ConfigureAwait(false);
                }
                else
                {
                    //// Event type insert and update scenario
                    if (latestMovement.NetStandardVolume != movement.NetStandardVolume)
                    {
                        //// IF movement from true and current movement volume doesn't match
                        //// Negate the exiting movement and insert the new
                        movement.EventType = EventType.Update.ToString();
                        return await this.movementRegistrationService.RegisterMovementAsync(movement, unitOfWork, false).ConfigureAwait(false);
                    }
                    else
                    {
                        //// IF movement from true and backup movement volume match , update the details for the latest movement.
                        this.Logger.LogInformation($"Updating details for latest movement as movement from TRUE and backup movement volume match for MovementId: {movement.MovementId}");
                        await this.movementRegistrationService.UpdateMovementOffChainDbAsync(latestMovement, movement, unitOfWork).ConfigureAwait(false);
                    }
                }
            }
            else
            {
                //// Update the details for the movement sent to SAP having the maximum date.
                var movementSentToSap = movementsFromTrue.FirstOrDefault(x => x.MovementTransactionId == sapTrackingResult.MovementTransactionId);
                await this.movementRegistrationService.UpdateMovementOffChainDbAsync(movementSentToSap, movement, unitOfWork).ConfigureAwait(false);
                this.Logger.LogInformation($"Updated the details for the movement sent to SAP having the maximum date for MovementId: {movement.MovementId}");
            }

            return 0;
        }
    }
}