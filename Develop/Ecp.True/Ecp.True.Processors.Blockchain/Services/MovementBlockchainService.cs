// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementBlockchainService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Blockchain.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Core.Dto;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Azure.Entities;

    /// <summary>
    /// The Blockchain Movement service.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Registration.Interfaces.IBlockchainMovementService" />
    public class MovementBlockchainService : BlockchainService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<MovementBlockchainService> logger;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementBlockchainService" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="azureclientFactory">The azureclient factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="telemetry">The telemetry.</param>
        public MovementBlockchainService(
            ITrueLogger<MovementBlockchainService> logger,
            IAzureClientFactory azureclientFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            ITelemetry telemetry)
            : base(azureclientFactory, logger, telemetry)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));

            this.logger = logger;
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        }

        /// <inheritdoc/>
        public override ServiceType Type => ServiceType.Movement;

        /// <summary>
        /// Registers the asynchronous.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns>Returns the completed task.</returns>
        public override async Task RegisterAsync(int entityId)
        {
            var repository = this.unitOfWork.CreateRepository<Movement>();
            var movement = await repository.SingleOrDefaultAsync(
                                                x => x.MovementTransactionId == entityId && x.BlockchainStatus == StatusType.PROCESSING,
                                                "MovementSource",
                                                "MovementDestination",
                                                "Period",
                                                "Owners")
                                                .ConfigureAwait(false);

            if (movement == null || !Enum.TryParse(movement.EventType, true, out EventType eventType))
            {
                this.logger.LogInformation($"No movement transaction record found or event type is invalid");
                return;
            }

            var status = await ValidateAsync(movement, repository).ConfigureAwait(false);
            if (!status)
            {
                this.logger.LogInformation($"Validation failed for movement transaction {entityId}.", $"{entityId}");
                return;
            }

            var existingMovement = await this.unitOfWork.MovementRepository.GetLatestBlockchainMovementAsync(movement.MovementId).ConfigureAwait(false);

            var result = await this.RegisterMovementAsync(movement, existingMovement).ConfigureAwait(false);
            if (result)
            {
                await this.RegisterOwnerAsync(movement.Owners, movement.MovementId, movement.BlockchainMovementTransactionId, eventType.ToString(), existingMovement).ConfigureAwait(false);
            }
        }

        private static async Task<bool> ValidateAsync(Movement movement, IRepository<Movement> movementRepository)
        {
            // If there is earlier movement which is not registered yet
            var count = await movementRepository.GetCountAsync(x =>
                                    x.MovementId == movement.MovementId &&
                                    x.MovementTransactionId < movement.MovementTransactionId &&
                                    x.BlockchainStatus != StatusType.PROCESSED &&
                                    (x.EventType != EventType.Update.ToString("G") || !x.IsDeleted)).ConfigureAwait(false);

            return count == 0;
        }

        private async Task<bool> RegisterMovementAsync(Movement movement, Movement existingMovement)
        {
            var result = await this.WriteToMovementBlockchainAsync(movement, existingMovement).ConfigureAwait(false);
            this.logger.LogInformation($"Contract call finished for movement: {movement.MovementTransactionId}, sending message to offchain queue.");
            await this.QueueAsync(result, movement.MovementTransactionId, ServiceType.Movement).ConfigureAwait(false);
            return true;
        }

        private async Task RegisterOwnerAsync(IEnumerable<Owner> owners, string movementId, Guid? blockchainMovementTransactionId, string eventType, Movement existingMovement)
        {
            var tasks = new List<Task>();
            foreach (var owner in owners)
            {
                tasks.Add(Task.Run(async () =>
                {
                    this.logger.LogInformation($"Registering Owner: {owner.OwnerId} with identifier {owner.Id} for movement {blockchainMovementTransactionId}");
                    var result = await this.WriteToMovementOwnersBlockchainAsync(owner, movementId, blockchainMovementTransactionId, eventType, existingMovement).ConfigureAwait(false);
                    this.logger.LogInformation($"Contract call finished for Owner: {owner.OwnerId} with identifier {owner.Id} for movement {blockchainMovementTransactionId}");
                    await this.QueueAsync(result, owner.Id, ServiceType.Owner).ConfigureAwait(false);
                }));
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task<OffchainMessage> WriteToMovementBlockchainAsync(Movement movement, Movement existingMovement)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));
            var eventType = Enum.Parse<EventType>(movement.EventType, true);

            this.logger.LogInformation($"Started building movement parameters {movement.MovementTransactionId}");
            var parameters = new Dictionary<string, object>
                {
                    { "movementId", movement.MovementId },
                };

            if (eventType != EventType.Delete)
            {
                parameters.Add("netStandardVolume", movement.NetStandardVolume.ToBlockChainNumber());
            }

            parameters.Add("transactionId", movement.BlockchainMovementTransactionId.ToString());
            parameters.Add("operationalDate", movement.OperationalDate.Ticks);

            if (eventType != EventType.Delete)
            {
                parameters.Add("measurementUnit", Convert.ToString(movement.MeasurementUnit, CultureInfo.InvariantCulture));
            }

            parameters.Add("metadata", movement.GetMetadata());
            var isBridge = (existingMovement != null && existingMovement.BlockNumber != null && existingMovement.BlockNumber.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                           || (movement.BlockNumber != null && movement.BlockNumber.StartsWith("0x", StringComparison.OrdinalIgnoreCase));

            var contractName = isBridge ? $"{ContractNames.MovementsFactory}Bridge" : ContractNames.MovementsFactory;
            this.logger.LogInformation($"Finished building movement parameters for {movement.MovementTransactionId}, writing to contract");

            return await this.WriteAsync(eventType.ToString(), parameters, contractName).ConfigureAwait(false);
        }
    }
}