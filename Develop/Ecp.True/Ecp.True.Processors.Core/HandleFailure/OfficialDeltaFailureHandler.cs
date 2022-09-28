// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaFailureHandler.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.HandleFailure
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
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;

    /// <summary>
    /// The OfficialDeltaFailureHandler.
    /// </summary>
    public class OfficialDeltaFailureHandler : FailureHandlerBase
    {
        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// Initializes a new instance of the <see cref="OfficialDeltaFailureHandler"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="telemetry">The telemetry.</param>
        public OfficialDeltaFailureHandler(
                  ITrueLogger<OfficialDeltaFailureHandler> logger,
                  ITelemetry telemetry)
                  : base(logger)
        {
            this.telemetry = telemetry;
        }

        /// <inheritdoc/>
        public override TicketType TicketType => TicketType.OfficialDelta;

        /// <inheritdoc/>
        public override async Task HandleFailureAsync(IUnitOfWork unitOfWork, FailureInfo failureInfo)
        {
            ArgumentValidators.ThrowIfNull(failureInfo, nameof(failureInfo));
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            this.telemetry.TrackEvent(Constants.Critical, EventName.OfficialDeltaFailureEvent.ToString("G"));
            var nodeStatusIsValid = await ValidateNodeStatusAsync(unitOfWork, failureInfo.TicketId).ConfigureAwait(false);
            if (!nodeStatusIsValid)
            {
                await UpdateTicketAndDeltaNodeAsync(unitOfWork, failureInfo).ConfigureAwait(false);
                return;
            }

            await ConsolidationDataCleanupAsync(unitOfWork, failureInfo).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the ticket errors asynchronous.
        /// </summary>
        /// <param name="unitOfWork">The unitOfWork.</param>
        /// <param name="failureInfo">The failureInfo.</param>
        /// <returns>Returns completed task.</returns>
        private static Task UpdateTicketAndDeltaNodeAsync(IUnitOfWork unitOfWork, FailureInfo failureInfo)
        {
            ArgumentValidators.ThrowIfNull(failureInfo, nameof(failureInfo));
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            UpdateTicketError(unitOfWork, failureInfo);
            UpdatedeltaNodeError(unitOfWork, failureInfo);
            unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            return Task.CompletedTask;
        }

        private static void UpdateTicketError(IUnitOfWork unitOfWork, FailureInfo failureInfo)
        {
            var repo = unitOfWork.CreateRepository<Ticket>();
            var ticketDetails = repo.GetByIdAsync(failureInfo.TicketId).ConfigureAwait(false).GetAwaiter().GetResult();
            ticketDetails.ErrorMessage = failureInfo.ErrorMessage;
            ticketDetails.Status = StatusType.ERROR;
            repo.Update(ticketDetails);
        }

        private static void UpdatedeltaNodeError(IUnitOfWork unitOfWork, FailureInfo failureInfo)
        {
            var repo = unitOfWork.CreateRepository<DeltaNode>();
            var deltaNode = repo.GetByIdAsync(failureInfo.TicketId).ConfigureAwait(false).GetAwaiter().GetResult();
            deltaNode.Status = OwnershipNodeStatusType.FAILED;
            repo.Update(deltaNode);
        }

        private static Task ConsolidationDataCleanupAsync(IUnitOfWork unitOfWork, FailureInfo failureInfo)
        {
            var ticketRepository = unitOfWork.CreateRepository<Ticket>();

            var parameters = new Dictionary<string, object>
                {
                    { "@TicketId", failureInfo.TicketId },
                    { "@ErrorMessage", failureInfo.ErrorMessage },
                    { "@MovementTransactionIdList", failureInfo.MovementTransactionIds.ToDataTable(Repositories.Constants.KeyTypeColumnName, Repositories.Constants.KeyType) },
                };

            return ticketRepository.ExecuteAsync(Repositories.Constants.ConsolidationDataCleanup, parameters);
        }

        private static async Task<bool> ValidateNodeStatusAsync(IUnitOfWork unitOfWork, int ticketId)
        {
            var parameters = new Dictionary<string, object>
                {
                    { "@TicketId", ticketId },
                };

            var consolidationNodesStatus = unitOfWork.CreateRepository<ConsolidationNodesStatus>().
                ExecuteQueryAsync(Repositories.Constants.ValidateNodesStateDifferentDeltas, parameters);
            var consolidationNodes = await consolidationNodesStatus.ConfigureAwait(false);
            return consolidationNodes.Any();
        }
    }
}
