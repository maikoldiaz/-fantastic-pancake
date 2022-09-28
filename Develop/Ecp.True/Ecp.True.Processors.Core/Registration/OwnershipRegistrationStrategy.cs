// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipRegistrationStrategy.cs" company="Microsoft">
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
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// The movement ownership registration strategy.
    /// </summary>
    public class OwnershipRegistrationStrategy : RegistrationStrategyBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipRegistrationStrategy" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        public OwnershipRegistrationStrategy(
            ITrueLogger logger,
            IAzureClientFactory azureClientFactory)
            : base(azureClientFactory, logger)
        {
        }

        /// <inheritdoc/>
        public override void Insert(IEnumerable<object> entities, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(entities, nameof(entities));
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            var ownerships = (IEnumerable<Ownership>)entities;
            var ownershipRepository = unitOfWork.CreateRepository<Ownership>();
            foreach (var ownership in ownerships)
            {
                ownership.BlockchainOwnershipId = Guid.NewGuid();
                ownership.EventType = EventType.Insert.ToString();
                ownership.BlockchainStatus = StatusType.PROCESSING;
                ownershipRepository.Insert(ownership);
            }
        }

        /// <inheritdoc/>
        public override async Task RegisterAsync(object entity, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            var ownershipRepository = unitOfWork.CreateRepository<Ownership>();
            var movement = entity as Movement;
            if (movement != null)
            {
                await RegisterMovementOwnershipsAsync(movement, ownershipRepository).ConfigureAwait(false);
            }

            var inventoryOwnership = entity as InventoryOwnership;
            if (inventoryOwnership != null)
            {
                await RegisterInventoryProductOwnershipsAsync(inventoryOwnership, ownershipRepository).ConfigureAwait(false);
            }
        }

        private static async Task RegisterMovementOwnershipsAsync(Movement movement, IRepository<Ownership> ownershipRepository)
        {
            var existingOwnerships = await ownershipRepository.GetAllAsync(x => x.MovementTransactionId == movement.MovementTransactionId && !x.IsDeleted).ConfigureAwait(false);
            if (movement.EventType.EqualsIgnoreCase(EventType.Update.ToString("G")))
            {
                UpdateOwnerships(existingOwnerships, movement.Ownerships, ownershipRepository, MessageType.MovementOwnership);
            }

            if (movement.EventType.EqualsIgnoreCase(EventType.Delete.ToString("G")))
            {
                UpdateOwnerships(existingOwnerships, new List<Ownership>(), ownershipRepository, MessageType.MovementOwnership);
            }
        }

        private static async Task RegisterInventoryProductOwnershipsAsync(InventoryOwnership inventoryOwnership, IRepository<Ownership> ownershipRepository)
        {
            var existingOwnerships = await ownershipRepository.GetAllAsync(x => x.InventoryProductId == inventoryOwnership.InventoryProductId && !x.IsDeleted).ConfigureAwait(false);
            UpdateOwnerships(existingOwnerships, inventoryOwnership.Ownerships, ownershipRepository, MessageType.InventoryOwnership);
        }

        private static void UpdateOwnerships(
            IEnumerable<Ownership> existingOwnerships,
            IEnumerable<Ownership> ownerships,
            IRepository<Ownership> ownershipRepository,
            MessageType messageTypeId)
        {
            foreach (var ownership in ownerships)
            {
                // Adding default properties
                ownership.RuleVersion = "1";
                ownership.ExecutionDate = ownership.ExecutionDate != default(DateTime) ? ownership.ExecutionDate : DateTime.UtcNow.ToTrue();
                ownership.MessageTypeId = messageTypeId;
                ownership.EventType = EventType.Insert.ToString();
                ownership.BlockchainStatus = StatusType.PROCESSING;
                var existingOwnership = existingOwnerships.FirstOrDefault(x => x.OwnerId == ownership.OwnerId);
                if (existingOwnership != null)
                {
                    // Update case
                    existingOwnership.IsDeleted = true;
                    ownershipRepository.Update(existingOwnership);

                    var negatedOwnership = BuildNegateOwnership(existingOwnership);
                    negatedOwnership.EventType = EventType.Update.ToString();

                    ownership.PreviousBlockchainOwnershipId = negatedOwnership.BlockchainOwnershipId;
                    ownership.EventType = EventType.Update.ToString();
                    ownershipRepository.Insert(negatedOwnership);
                }

                // Insert case
                ownership.BlockchainOwnershipId = Guid.NewGuid();
                ownershipRepository.Insert(ownership);
            }

            foreach (var existingOwnership in existingOwnerships)
            {
                if (!ownerships.Any(x => x.OwnerId == existingOwnership.OwnerId))
                {
                    // Delete case
                    existingOwnership.IsDeleted = true;
                    ownershipRepository.Update(existingOwnership);
                    var negatedOwnership = BuildNegateOwnership(existingOwnership);
                    negatedOwnership.EventType = EventType.Delete.ToString();

                    ownershipRepository.Insert(negatedOwnership);
                }
            }
        }

        private static Ownership BuildNegateOwnership(Ownership existingOwnership)
        {
            return new Ownership
            {
                TicketId = existingOwnership.TicketId,
                OwnerId = existingOwnership.OwnerId,
                OwnershipPercentage = -existingOwnership.OwnershipPercentage,
                OwnershipVolume = -existingOwnership.OwnershipVolume,
                AppliedRule = existingOwnership.AppliedRule,
                RuleVersion = existingOwnership.RuleVersion,
                ExecutionDate = existingOwnership.ExecutionDate,
                BlockchainOwnershipId = Guid.NewGuid(),
                IsDeleted = true,
                MovementTransactionId = existingOwnership.MovementTransactionId,
                InventoryProductId = existingOwnership.InventoryProductId,
                BlockchainMovementTransactionId = existingOwnership.BlockchainMovementTransactionId,
                BlockchainInventoryProductTransactionId = existingOwnership.BlockchainInventoryProductTransactionId,
                MessageTypeId = existingOwnership.MessageTypeId,
                BlockchainStatus = StatusType.PROCESSING,
            };
        }
    }
}
