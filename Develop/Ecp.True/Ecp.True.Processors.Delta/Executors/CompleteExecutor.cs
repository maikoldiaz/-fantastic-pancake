// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompleteExecutor.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta.Entities;

    /// <summary>
    /// The Complete Executor class.
    /// </summary>
    public class CompleteExecutor : ExecutorBase
    {
        /// <summary>
        /// The registration factory.
        /// </summary>
        private readonly IRegistrationStrategyFactory registrationFactory;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        private readonly ITrueLogger<CompleteExecutor> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompleteExecutor"/> class.
        /// </summary>
        /// <param name="registrationFactory">The registration factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="logger">The logger.</param>
        public CompleteExecutor(
              IRegistrationStrategyFactory registrationFactory,
              IUnitOfWorkFactory unitOfWorkFactory,
              ITrueLogger<CompleteExecutor> logger)
            : base(logger)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            this.registrationFactory = registrationFactory;
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.logger = logger;
        }

        /// <inheritdoc/>
        public override int Order => 7;

        /// <summary>
        /// Gets the type of the process.
        /// </summary>
        /// <value>
        /// The type of the process.
        /// </value>
        public override ProcessType ProcessType => ProcessType.Delta;

        /// <inheritdoc/>
        public override async Task ExecuteAsync(object input)
        {
            True.Core.ArgumentValidators.ThrowIfNull(input, nameof(input));

            var deltaData = (DeltaData)input;
            await this.CompleteAsync(deltaData).ConfigureAwait(false);

            this.ShouldContinue = false;
            await this.ExecuteNextAsync(deltaData).ConfigureAwait(false);
        }

        /// <summary>
        /// Processes the delta asynchronous.
        /// </summary>
        /// <param name="deltaData">The ownershipRuleData.</param>
        /// <exception cref="System.NotSupportedException">exception.</exception>
        /// <returns>The object.</returns>
        private async Task CompleteAsync(DeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));
            var movementRepository = this.unitOfWork.CreateRepository<Movement>();
            var movementIds = deltaData.UpdatedMovements.Select(x => x.MovementId).Distinct();
            var movements = await movementRepository.GetAllAsync(x => movementIds.Contains(x.MovementId)).ConfigureAwait(false);
            foreach (var movement in movements)
            {
                var updatedMovement = deltaData.UpdatedMovements.Single(x => x.MovementId == movement.MovementId);
                if (movement.MovementTransactionId <= updatedMovement.MovementTransactionId)
                {
                    movement.DeltaTicketId = deltaData.Ticket.TicketId;
                }
            }

            movementRepository.UpdateAll(movements);
            var inventoryRepository = this.unitOfWork.CreateRepository<InventoryProduct>();
            var inventoryIds = deltaData.UpdatedInventories.Select(x => x.InventoryProductUniqueId).Distinct();
            var inventories = await inventoryRepository.GetAllAsync(x => inventoryIds.Contains(x.InventoryProductUniqueId)).ConfigureAwait(false);
            foreach (var inventory in inventories)
            {
                var updatedInventory = deltaData.UpdatedInventories.Single(x => x.InventoryProductUniqueId == inventory.InventoryProductUniqueId);
                if (inventory.InventoryProductId <= updatedInventory.InventoryProductId)
                {
                    inventory.DeltaTicketId = deltaData.Ticket.TicketId;
                }
            }

            inventoryRepository.UpdateAll(inventories);
            var deltaErrorRepository = this.unitOfWork.CreateRepository<DeltaError>();
            deltaErrorRepository.InsertAll(deltaData.DeltaErrors);

            this.registrationFactory.MovementRegistrationStrategy.Insert(deltaData.GeneratedMovements, this.unitOfWork);
            await this.SuccessTicketAsync(deltaData.Ticket.TicketId, this.unitOfWork).ConfigureAwait(false);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Process the ticket asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        private async Task SuccessTicketAsync(int ticketId, IUnitOfWork unitOfWork)
        {
            this.logger.LogInformation($"Success delta ticket:  {ticketId}", $"{ticketId}");
            var ticketRepository = unitOfWork.CreateRepository<Ticket>();
            var ticket = await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);
            ticket.Status = StatusType.DELTA;
            ticketRepository.Update(ticket);
        }
    }
}
