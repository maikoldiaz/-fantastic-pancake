// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementRegistrationService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Processors.Core
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// The official registration service.
    /// </summary>
    public class MovementRegistrationService : IMovementRegistrationService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<MovementRegistrationService> logger;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementRegistrationService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        public MovementRegistrationService(
            ITrueLogger<MovementRegistrationService> logger,
            IAzureClientFactory azureClientFactory)
        {
            this.logger = logger;
            this.azureClientFactory = azureClientFactory;
        }

        /// <inheritdoc/>
        public async Task<int> RegisterMovementAsync(Movement movementToRegister, IUnitOfWork unitOfWork, bool validateTransferPoint)
        {
            ArgumentValidators.ThrowIfNull(movementToRegister, nameof(movementToRegister));
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            int movementTransactionId = 0;
            int? prevMovementTransactionId = null;
            Guid? previousBlockchainMovementTransactionId = null;
            bool isTransferPoint = false;
            var existingMovement = await GetMovementAsync(movementToRegister.MovementId, unitOfWork).ConfigureAwait(false);
            if (validateTransferPoint)
            {
                this.logger.LogInformation($"Validating transfer point movement, MovementId: {movementToRegister.MovementId}");
                isTransferPoint = await ValidateTransferPointAsync(movementToRegister, unitOfWork).ConfigureAwait(false);
            }

            if (movementToRegister.EventType.EqualsIgnoreCase(EventType.Update.ToString("G")) || movementToRegister.EventType.EqualsIgnoreCase(EventType.Delete.ToString("G")))
            {
                (movementTransactionId, previousBlockchainMovementTransactionId) = await NegateMovementAsync(
                    existingMovement,
                    movementToRegister.FileRegistrationTransactionId,
                    movementToRegister.EventType,
                    false,
                    unitOfWork).ConfigureAwait(false);
                prevMovementTransactionId = movementTransactionId;
                this.logger.LogInformation($"Inserted negated movement, MovementTransactionId: {movementTransactionId}");
            }

            if (movementToRegister.EventType.EqualsIgnoreCase(EventType.Update.ToString("G")) || movementToRegister.EventType.EqualsIgnoreCase(EventType.Insert.ToString("G")))
            {
                UpdateSapTracking(movementToRegister, isTransferPoint);
                this.logger.LogInformation($"Updated SAP tracking for movement, MovementId: {movementToRegister.MovementId}");

                //// For insert and update scenario inserting positive movement
                movementToRegister.BlockchainMovementTransactionId = Guid.NewGuid();
                movementToRegister.PreviousBlockchainMovementTransactionId = previousBlockchainMovementTransactionId;

                //// Get existing movement created date..
                var dateTime = existingMovement?.CreatedDate;

                movementTransactionId = await WriteMovementOffChainDbAsync(movementToRegister, true, unitOfWork, movementToRegister.EventType, dateTime).ConfigureAwait(false);
                this.logger.LogInformation($"Inserted movement, MovementTransactionId: {movementTransactionId}");
            }

            return movementTransactionId;
        }

        /// <inheritdoc/>
        public async Task UpdateMovementOffChainDbAsync(Movement movementToUpdate, Movement movementReceived, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(movementToUpdate, nameof(movementToUpdate));
            ArgumentValidators.ThrowIfNull(movementReceived, nameof(movementReceived));
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));

            var movementRepository = unitOfWork.CreateRepository<Movement>();

            movementToUpdate.IsOfficial = movementReceived.IsOfficial;
            movementToUpdate.BackupMovementId = movementReceived.BackupMovementId;
            movementToUpdate.GlobalMovementId = movementReceived.GlobalMovementId;
            movementRepository.Update(movementToUpdate);

            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Movement>> GetMovementsWithSapTrackingAsync(string movementId, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            var movementRepository = unitOfWork.CreateRepository<Movement>();
            return await movementRepository.GetAllAsync(
                 f =>
                f.MovementId == movementId,
                 "SapTracking").ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the movement asynchronous.
        /// </summary>
        /// <param name="movementId">The movement identifier.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>
        /// The movement.
        /// </returns>
        private static async Task<Movement> GetMovementAsync(string movementId, IUnitOfWork unitOfWork)
        {
            var movementRepository = unitOfWork.CreateRepository<Movement>();
            var allMovements = await movementRepository.GetAllAsync(
                f =>
                f.MovementId == movementId && f.NetStandardVolume >= 0,
                "MovementSource",
                "MovementDestination",
                "SapTracking",
                "Period").ConfigureAwait(false);
            var movement = allMovements.OrderByDescending(a => a.CreatedDate).FirstOrDefault();

            if (movement != null)
            {
                var tasks = new List<Task>();

                // Fetch owners per movement
                tasks.Add(GetOwnersAsync(unitOfWork, movement));

                // Fetch attributes per movement
                tasks.Add(GetAttributesAsync(unitOfWork, movement));
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }

            return movement;
        }

        private static async Task GetAttributesAsync(IUnitOfWork unitOfWork, Movement movement)
        {
            await unitOfWork.CreateRepository<AttributeEntity>().GetAllAsync(x => x.MovementTransactionId == movement.MovementTransactionId).ConfigureAwait(false);
        }

        private static async Task GetOwnersAsync(IUnitOfWork unitOfWork, Movement movement)
        {
            await unitOfWork.CreateRepository<Owner>().GetAllAsync(x => x.MovementTransactionId == movement.MovementTransactionId).ConfigureAwait(false);
        }

        /// <summary>
        /// Negates the movement asynchronous.
        /// </summary>
        /// <param name="movementObject">The movement object.</param>
        /// <param name="fileRegistrationTransactionId">The file registration transaction identifier.</param>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="isOwner">The isOwner.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>
        /// The task.
        /// </returns>
        private static async Task<(int, Guid)> NegateMovementAsync(
            Movement movementObject,
            int? fileRegistrationTransactionId,
            string eventType,
            bool isOwner,
            IUnitOfWork unitOfWork)
        {
            var negatedmovement = new Movement
            {
                MovementSource = movementObject.MovementSource != null ? new MovementSource() : null,
                MovementDestination = movementObject.MovementDestination != null ? new Entities.Registration.MovementDestination() : null,
                Period = movementObject.Period != null ? new MovementPeriod() : null,
            };

            negatedmovement.CopyFrom(movementObject);
            if (movementObject.MovementSource != null)
            {
                negatedmovement.MovementSource.CopyFrom(movementObject.MovementSource);
            }

            if (movementObject.MovementDestination != null)
            {
                negatedmovement.MovementDestination.CopyFrom(movementObject.MovementDestination);
            }

            if (movementObject.Period != null)
            {
                negatedmovement.Period.CopyFrom(movementObject.Period);
            }

            movementObject.Owners.ForEach(own =>
            {
                var owner = new Owner();
                owner.CopyFrom(own);
                owner.BlockchainStatus = StatusType.PROCESSED;
                owner.OwnershipValue = owner.OwnershipValueUnit == Constants.OwnershipPercentageUnit ? Convert.ToDecimal(owner.OwnershipValue.ToString(), CultureInfo.InvariantCulture)
                                       : Convert.ToDecimal(owner.OwnershipValue.ToString(), CultureInfo.InvariantCulture) * -1;
                negatedmovement.Owners.Add(owner);
            });

            movementObject.Attributes.ForEach(attr =>
            {
                var attribute = new AttributeEntity();
                attribute.CopyFrom(attr);

                negatedmovement.Attributes.Add(attribute);
            });

            if (!eventType.EqualsIgnoreCase(EventType.Delete.ToString("G")))
            {
                movementObject.SapTracking.ForEach(sap =>
                {
                    negatedmovement.SapTracking.Add(new SapTracking
                    {
                        StatusTypeId = StatusType.PROCESSING,
                        OperationalDate = DateTime.UtcNow.ToTrue(),
                    });
                });
            }

            var previousBlockchainMovementTransactionId = Guid.NewGuid();
            negatedmovement.FileRegistrationTransactionId = fileRegistrationTransactionId;
            negatedmovement.EventType = eventType;

            negatedmovement.NetStandardVolume = Convert.ToDecimal(movementObject.NetStandardVolume.ToString(), CultureInfo.InvariantCulture) * -1;
            negatedmovement.IsDeleted = true;

            if (movementObject.GrossStandardVolume.HasValue)
            {
                negatedmovement.GrossStandardVolume = Convert.ToDecimal(movementObject.GrossStandardVolume.ToString(), CultureInfo.InvariantCulture) * -1;
            }

            negatedmovement.BlockchainMovementTransactionId = previousBlockchainMovementTransactionId;

            var movementTransactionId = await WriteMovementOffChainDbAsync(negatedmovement, isOwner, unitOfWork, Constants.UpdateNegate, movementObject.CreatedDate).ConfigureAwait(false);
            return (movementTransactionId, previousBlockchainMovementTransactionId);
        }

        /// <summary>
        /// Writes the movement off chain database asynchronous.
        /// </summary>
        /// <param name="movement">The movement.</param>
        /// <param name="isOwner">if set to <c>true</c> [is owner].</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="eventType">The eventType.</param>
        /// <returns>
        /// The movementTransactionidentifier.
        /// </returns>
        private static async Task<int> WriteMovementOffChainDbAsync(Movement movement, bool isOwner, IUnitOfWork unitOfWork, string eventType, DateTime? dateTime)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));

            var movementRepository = unitOfWork.CreateRepository<Movement>();

            //// Register Movement.
            if (isOwner)
            {
                foreach (var item in movement.Owners)
                {
                    item.BlockchainMovementTransactionId = movement.BlockchainMovementTransactionId;
                    item.BlockchainStatus = StatusType.PROCESSING;
                }
            }

            movement.OperationalDate = movement.OperationalDate.Date;
            movement.BlockchainStatus = StatusType.PROCESSING;
            movementRepository.Insert(movement);

            //// Code to insert in index table to validate the concurrency scenario.
            movement.InsertInInventoryMovementIndex(unitOfWork, movement.MovementId, eventType, dateTime);

            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);

            return movement.MovementTransactionId;
        }

        /// <summary>
        /// Validates the transfer point.
        /// Validate if both source node and destination node are there.
        /// Validate if belongs to the operative scenario.
        /// Validate from database if source node and destination node belong to different segments.
        /// Validate from database if belongs to one of the SON segments.
        /// </summary>
        /// <param name="movementToRegister">The movement to register.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>The task.</returns>
        private static async Task<bool> ValidateTransferPointAsync(Movement movementToRegister, IUnitOfWork unitOfWork)
        {
            if (movementToRegister.ScenarioId != ScenarioType.OPERATIONAL
                || movementToRegister?.MovementSource?.SourceNodeId == null
                || movementToRegister?.MovementDestination?.DestinationNodeId == null)
            {
                return false;
            }

            try
            {
                var movementRepository = unitOfWork.CreateRepository<Movement>();
                var parameters = new Dictionary<string, object>
                {
                    { "@SegmentId", movementToRegister.SegmentId },
                    { "@SourceNodeId", movementToRegister.MovementSource.SourceNodeId },
                    { "@DestinationNodeId", movementToRegister.MovementDestination.DestinationNodeId },
                    { "@OperationalDate", movementToRegister.OperationalDate.Date },
                };

                //// SP will throw exception if its a transfer point
                await movementRepository.ExecuteAsync(Repositories.Constants.ValidateTransferPoint, parameters).ConfigureAwait(false);
                return false;
            }
            catch (SqlException ex)
            {
                if (ex.Message == Constants.MovemenTransferPoint)
                {
                    return true;
                }

                throw;
            }
        }

        /// <summary>
        /// Updates the sap tracking.
        /// </summary>
        /// <param name="movement">The movement.</param>
        /// <param name="isTransferPoint">if set to <c>true</c> [is transfer point].</param>
        private static void UpdateSapTracking(Movement movement, bool isTransferPoint)
        {
            if (isTransferPoint)
            {
                movement.IsTransferPoint = isTransferPoint;
                movement.SapTracking.Add(new SapTracking
                {
                    StatusTypeId = StatusType.PROCESSING,
                    OperationalDate = DateTime.UtcNow.ToTrue(),
                });
            }
        }

        /// <summary>
        /// Sends to sap queue asynchronous.
        /// </summary>
        /// <param name="movementTransactionId">The movement transaction identifier.</param>
        /// <param name="prevMovementTransactionId">The previous movement transaction identifier.</param>
        /// <param name="sessionId">The session identifier.</param>
        [Obsolete("This Method is Deprecated", false)]
        private async Task SendToSapQueueAsync(int movementTransactionId, int? prevMovementTransactionId, string sessionId)
        {
            var sapRequest = new SapQueueMessage
            {
                MessageId = movementTransactionId,
                PreviousMovementTransactionId = prevMovementTransactionId,
                RequestType = SapRequestType.Movement,
            };
            var client = this.azureClientFactory.GetQueueClient(QueueConstants.SapQueue);
            await client.QueueSessionMessageAsync(sapRequest, sessionId).ConfigureAwait(false);
        }
    }
}
