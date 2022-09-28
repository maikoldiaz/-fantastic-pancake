// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorExecutor.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Delta.Entities;

    /// <summary>
    ///  The ErrorExecutor class.
    /// </summary>
    public class ErrorExecutor : ExecutorBase
    {
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
        private readonly ITrueLogger<ErrorExecutor> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorExecutor"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="unitOfWorkFactory">The unitOfWorkFactory.</param>
        public ErrorExecutor(ITrueLogger<ErrorExecutor> logger, IUnitOfWorkFactory unitOfWorkFactory)
            : base(logger)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.logger = logger;
        }

        /// <inheritdoc/>
        public override int Order => 4;

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
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            var deltaData = (DeltaData)input;
            deltaData.HasProcessingErrors = deltaData.ErrorInventories.Any() || deltaData.ErrorMovements.Any();
            if (deltaData.HasProcessingErrors)
            {
                await this.InsertDeltaErrorAsync(deltaData).ConfigureAwait(false);
            }

            this.ShouldContinue = !deltaData.HasProcessingErrors;
            await this.ExecuteNextAsync(deltaData).ConfigureAwait(false);
        }

        /// <summary>
        /// Inserts the delta error.
        /// </summary>
        /// <param name="deltaData">The delta data.</param>
        /// <returns>
        /// the task.
        /// </returns>
        private async Task InsertDeltaErrorAsync(DeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));
            var deltaErrorRepository = this.unitOfWork.CreateRepository<DeltaError>();
            var deltaErrors = new List<DeltaError>();
            foreach (var deltaMovementError in deltaData.ErrorMovements)
            {
                var movementError = new DeltaError
                {
                    MovementTransactionId = deltaMovementError.MovementTransactionId,
                    TicketId = deltaData.Ticket.TicketId,
                    ErrorMessage = deltaMovementError.Description,
                };

                deltaErrors.Add(movementError);
            }

            foreach (var deltaInventoryError in deltaData.ErrorInventories)
            {
                var inventoryError = new DeltaError
                {
                    InventoryProductId = deltaInventoryError.InventoryProductId,
                    TicketId = deltaData.Ticket.TicketId,
                    ErrorMessage = deltaInventoryError.Description,
                };

                deltaErrors.Add(inventoryError);
            }

            deltaErrorRepository.InsertAll(deltaErrors);
            await this.FailTicketAsync(deltaData.Ticket.TicketId, this.unitOfWork).ConfigureAwait(false);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Fails the ticket asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        private async Task FailTicketAsync(int ticketId, IUnitOfWork unitOfWork)
        {
            this.logger.LogInformation($"Fail delta ticket:  {ticketId}", $"{ticketId}");
            var ticketRepository = unitOfWork.CreateRepository<Ticket>();
            var ticket = await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);
            ticket.Status = StatusType.FAILED;
            ticketRepository.Update(ticket);
        }
    }
}
