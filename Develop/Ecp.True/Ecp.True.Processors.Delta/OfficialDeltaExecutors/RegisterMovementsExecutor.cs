// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterMovementsExecutor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.OfficialDeltaExecutors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;

    /// <summary>
    /// The Register Movements Executor class.
    /// </summary>
    public class RegisterMovementsExecutor : ExecutorBase
    {
        /// <summary>
        /// The business context.
        /// </summary>
        private readonly IBusinessContext businessContext;

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
        private readonly ITrueLogger<RegisterMovementsExecutor> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterMovementsExecutor" /> class.
        /// </summary>
        /// <param name="businessContext">The business context.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="logger">The logger.</param>
        public RegisterMovementsExecutor(
              IBusinessContext businessContext,
              IUnitOfWorkFactory unitOfWorkFactory,
              ITrueLogger<RegisterMovementsExecutor> logger)
            : base(logger)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            this.businessContext = businessContext;
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.logger = logger;
        }

        /// <inheritdoc/>
        public override int Order => 5;

        /// <summary>
        /// Gets the type of the process.
        /// </summary>
        /// <value>
        /// The type of the process.
        /// </value>
        public override ProcessType ProcessType => ProcessType.OfficialDelta;

        /// <inheritdoc/>
        public override async Task ExecuteAsync(object input)
        {
            True.Core.ArgumentValidators.ThrowIfNull(input, nameof(input));

            var deltaData = (OfficialDeltaData)input;
            this.Logger.LogInformation($"Started {nameof(RegisterMovementsExecutor)} for ticket {deltaData.Ticket.TicketId}", deltaData.Ticket.TicketId);
            await this.RegisterMovementsAsync(deltaData).ConfigureAwait(false);
            this.Logger.LogInformation($"Completed {nameof(RegisterMovementsExecutor)} for ticket {deltaData.Ticket.TicketId}", deltaData.Ticket.TicketId);
            this.ShouldContinue = true;
            await this.ExecuteNextAsync(deltaData).ConfigureAwait(false);
        }

        private static IEnumerable<Movement> GetMovements(IEnumerable<Movement> movements)
        {
            var movementsToInsert = new List<Movement>();
            foreach (var movement in movements)
            {
                if (movement.EventType.EqualsIgnoreCase(EventType.Insert.ToString("G")))
                {
                    var blockchainMovementTransactionId = Guid.NewGuid();
                    movement.IsSystemGenerated = true;
                    movement.OperationalDate = movement.OperationalDate.Date;
                    if (movement.PendingApproval != true)
                    {
                        movement.ScenarioId = ScenarioType.OPERATIONAL;
                        movement.BlockchainMovementTransactionId = blockchainMovementTransactionId;
                    }

                    foreach (var ownership in movement.Ownerships)
                    {
                        ownership.MessageTypeId = MessageType.MovementOwnership;
                        ownership.BlockchainOwnershipId = Guid.NewGuid();
                        ownership.BlockchainMovementTransactionId = blockchainMovementTransactionId;
                        ownership.EventType = EventType.Insert.ToString();
                    }

                    movementsToInsert.Add(movement);
                }
            }

            return movementsToInsert;
        }

        /// <summary>
        /// Process the ticket asynchronous.
        /// </summary>
        /// <param name="deltaData">The delta data.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        private async Task DeleteOfficialMovementsAsync(OfficialDeltaData deltaData, IUnitOfWork unitOfWork)
        {
            var movementRepository = unitOfWork.CreateRepository<Movement>();
            var deletedMovementsList = deltaData.MovementTransactionIds.Select(x => x.MovementTransactionId).Distinct();

            this.Logger.LogInformation($"Number of movements to delete {deletedMovementsList.Count()}", deltaData.Ticket.TicketId);

            if (deletedMovementsList.Any())
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@MovementTransactionIdList", deletedMovementsList.ToDataTable(Repositories.Constants.KeyTypeColumnName, Repositories.Constants.KeyType) },
                };

                await movementRepository.ExecuteAsync(Repositories.Constants.DeleteMovementsProcedureName, parameters).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Processes the delta asynchronous.
        /// </summary>
        /// <param name="deltaData">The ownershipRuleData.</param>
        /// <returns>The object.</returns>
        private async Task RegisterMovementsAsync(OfficialDeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));

            // Delete movements for previous attempts.
            await this.DeleteOfficialMovementsAsync(deltaData, this.unitOfWork).ConfigureAwait(false);

            // Insert new FICO-generated movements.
            var movementRepository = this.unitOfWork.CreateRepository<Movement>();
            var movementsToInsert = GetMovements(deltaData.GeneratedMovements);
            this.Logger.LogInformation($"Number of new movements {movementsToInsert.Count()}");

            var movementTempEntities = new List<MovementTempEntity>();
            var movementOwnerTempEntities = new List<MovementOwnerTempEntity>();

            var index = 1;
            foreach (var movement in movementsToInsert)
            {
                movementTempEntities.Add(new MovementTempEntity(movement, index, this.businessContext.UserId));
                movement.Owners.ForEach(o => movementOwnerTempEntities.Add(new MovementOwnerTempEntity(o, index, this.businessContext.UserId)));
                index++;
            }

            var parameters = new Dictionary<string, object>
            {
                { "@Movements", movementTempEntities.ToDataTable(Repositories.Constants.MovementType) },
                { "@MovementOwners", movementOwnerTempEntities.ToDataTable(Repositories.Constants.MovementOwnerType) },
            };

            await movementRepository.ExecuteAsync(Repositories.Constants.SaveDeltaBulkMovements, parameters).ConfigureAwait(false);

            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            this.logger.LogInformation($"Successfully registered FICO generated movements for official delta ticket: {deltaData.Ticket.TicketId}", $"{deltaData.Ticket.TicketId}");
        }
    }
}
