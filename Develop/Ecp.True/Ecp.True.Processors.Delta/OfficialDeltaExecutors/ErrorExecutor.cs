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

namespace Ecp.True.Processors.Delta.OfficialDeltaExecutors
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.Interfaces;

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
        /// The unit of work.
        /// </summary>
        private readonly ICompositeOfficialDeltaBuilder compositeOfficialDeltaBuilder;

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
        /// <param name="compositeOfficialDeltaBuilder">The official delta builders.</param>
        public ErrorExecutor(ITrueLogger<ErrorExecutor> logger, IUnitOfWorkFactory unitOfWorkFactory, ICompositeOfficialDeltaBuilder compositeOfficialDeltaBuilder)
            : base(logger)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.logger = logger;
            this.compositeOfficialDeltaBuilder = compositeOfficialDeltaBuilder;
        }

        /// <inheritdoc/>
        public override int Order => 3;

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
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            var deltaData = (OfficialDeltaData)input;

            this.Logger.LogInformation($"Started {nameof(ErrorExecutor)} for ticket {deltaData.Ticket.TicketId}", deltaData.Ticket.TicketId);
            deltaData.HasProcessingErrors = deltaData.MovementErrors.Any() || deltaData.InventoryErrors.Any();
            if (deltaData.HasProcessingErrors)
            {
                await this.BuildDeltaNodesAsync(deltaData).ConfigureAwait(false);
                await this.InsertDeltaNodeErrorsAsync(deltaData).ConfigureAwait(false);
            }

            this.Logger.LogInformation($"Completed {nameof(ErrorExecutor)} for ticket {deltaData.Ticket.TicketId}", deltaData.Ticket.TicketId);
            this.ShouldContinue = !deltaData.HasProcessingErrors;
            await this.ExecuteNextAsync(deltaData).ConfigureAwait(false);
        }

        /// <summary>
        /// Fails the nodes asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifiers.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        private static async Task FailDeltaNodesAsync(int ticketId, IUnitOfWork unitOfWork)
        {
            var deltaNodeRepository = unitOfWork.CreateRepository<DeltaNode>();
            var deltaNodes = await deltaNodeRepository.GetAllAsync(x => x.TicketId == ticketId).ConfigureAwait(false);
            deltaNodes.ForEach(y => y.Status = OwnershipNodeStatusType.FAILED);
            deltaNodeRepository.UpdateAll(deltaNodes);
        }

        private async Task BuildDeltaNodesAsync(OfficialDeltaData officialDeltaData)
        {
            var nodeIds = officialDeltaData.GetPendingOfficialMovementNodes();
            var deltaNodeRepository = this.unitOfWork.CreateRepository<DeltaNode>();
            var deltaNodes = await deltaNodeRepository.GetAllAsync(x => nodeIds.Contains(x.NodeId) && x.TicketId == officialDeltaData.Ticket.TicketId).ConfigureAwait(false);
            (officialDeltaData.DeltaNodes as List<DeltaNode>).AddRange(deltaNodes);
        }

        /// <summary>
        /// Inserts the delta error.
        /// </summary>
        /// <param name="deltaData">The delta data.</param>
        private async Task InsertDeltaNodeErrorsAsync(OfficialDeltaData deltaData)
        {
            var deltaErrors = await this.compositeOfficialDeltaBuilder.BuildErrorsAsync(deltaData).ConfigureAwait(false);
            var deltaNodeErrorRepository = this.unitOfWork.CreateRepository<DeltaNodeError>();
            deltaNodeErrorRepository.InsertAll(deltaErrors);

            await FailDeltaNodesAsync(deltaData.Ticket.TicketId, this.unitOfWork).ConfigureAwait(false);
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
