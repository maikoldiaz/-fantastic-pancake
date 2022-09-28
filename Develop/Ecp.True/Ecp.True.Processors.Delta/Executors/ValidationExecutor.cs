// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationExecutor.cs" company="Microsoft">
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
    using System.Globalization;
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
    /// The ValidationExecutor.
    /// </summary>
    /// <seealso cref="ExecutorBase" />
    public class ValidationExecutor : ExecutorBase
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationExecutor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="unitOfWorkFactory">The unitOfWorkFactory.</param>
        public ValidationExecutor(ITrueLogger<ValidationExecutor> logger, IUnitOfWorkFactory unitOfWorkFactory)
                   : base(logger)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        }

        /// <summary>
        /// Gets the Order.
        /// </summary>
        public override int Order => 2;

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

            await this.ValidateAsync(deltaData).ConfigureAwait(false);

            this.ShouldContinue = !deltaData.HasProcessingErrors;

            await this.ExecuteNextAsync(deltaData).ConfigureAwait(false);
        }

        /// <summary>
        /// Validates the asynchronous.
        /// </summary>
        /// <param name="deltaData">The deltaData.</param>
        /// <returns>
        /// The collection of ErrorInfo.
        /// </returns>
        private async Task ValidateAsync(DeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));

            var invalidMovements = deltaData.UpdatedMovements.Where(a => string.IsNullOrEmpty(a.CancellationType));

            var deltaErrors = invalidMovements.Select(a =>
                new DeltaError
                {
                    MovementTransactionId = a.MovementTransactionId,
                    TicketId = deltaData.Ticket.TicketId,
                    ErrorMessage = string.Format(CultureInfo.InvariantCulture, Constants.NoAnnulationErrorMessage, a.MovementType),
                });

            if (deltaErrors.Any())
            {
                var deltaErrorRepository = this.unitOfWork.CreateRepository<DeltaError>();
                deltaErrorRepository.InsertAll(deltaErrors);

                var ticketRepository = this.unitOfWork.CreateRepository<Ticket>();
                var ticket = await ticketRepository.GetByIdAsync(deltaData.Ticket.TicketId).ConfigureAwait(false);
                ticket.Status = StatusType.FAILED;
                ticketRepository.Update(ticket);

                deltaData.HasProcessingErrors = true;

                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }
    }
}
