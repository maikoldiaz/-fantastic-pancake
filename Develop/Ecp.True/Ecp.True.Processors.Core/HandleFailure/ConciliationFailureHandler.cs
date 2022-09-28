// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConciliationFailureHandler.cs" company="Microsoft">
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
    using System;
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

    /// <summary>
    /// The logistics failure handler.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.FailureHandler.FailureHandlerBase" />
    public class ConciliationFailureHandler : FailureHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConciliationFailureHandler"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ConciliationFailureHandler(
                  ITrueLogger<ConciliationFailureHandler> logger)
                  : base(logger)
        {
        }

        /// <inheritdoc/>
        public override TicketType TicketType => TicketType.Conciliation;

        /// <inheritdoc/>
        public override async Task HandleFailureAsync(IUnitOfWork unitOfWork, FailureInfo failureInfo)
        {
            ArgumentValidators.ThrowIfNull(failureInfo, nameof(failureInfo));
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            this.Logger.LogInformation($"Handling failure for ticket:  {failureInfo.TicketId}", $"{failureInfo.TicketId}");
            this.Logger.LogInformation($"Clear conciliation  data is requested for ticket: {failureInfo.TicketId}", $"{failureInfo.TicketId}");
            await UpdateStatusTicketAsync(unitOfWork, failureInfo.TicketId, StatusType.CONCILIATIONFAILED, failureInfo.ErrorMessage).ConfigureAwait(false);
            await UpdateOwnershipNodeAsync(unitOfWork, failureInfo.TicketId, StatusType.CONCILIATIONFAILED, OwnershipNodeStatusType.CONCILIATIONFAILED, failureInfo.NodeId).ConfigureAwait(false);
        }

        private static async Task UpdateStatusTicketAsync(IUnitOfWork unitOfWork, int ticketId, StatusType statusType, string error)
        {
                var ticketRepository = unitOfWork.CreateRepository<Ticket>();
                var ticket = await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);
                ticket.Status = statusType;
                ticket.ErrorMessage = string.IsNullOrEmpty(error) ? null : error;
                ticketRepository.Update(ticket);
                await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        private static async Task UpdateOwnershipNodeAsync(IUnitOfWork unitOfWork, int ticketId, StatusType status, OwnershipNodeStatusType ownershipStatus, int? nodeId)
        {
            var ownershipNodeDataRepository = unitOfWork.CreateRepository<OwnershipNodeData>();
            var ownershipNodeData = await ownershipNodeDataRepository.ExecuteViewAsync().ConfigureAwait(false);
            var ownershipNodeList = ownershipNodeData.Where(x => x.TicketId == ticketId && (nodeId == null || x.NodeId == nodeId) && x.IsTransferPoint).ToList();

            var ownershipNodeRepository = unitOfWork.CreateRepository<OwnershipNode>();
            var ownerShip = await ownershipNodeRepository.GetAllAsync(
                x => x.TicketId == ticketId &&
                ownershipNodeList.Select(node => node.NodeId).Contains(x.NodeId),
                "OwnershipNodeErrors").ConfigureAwait(false);

            foreach (var item in ownerShip)
            {
                if (item.OwnershipNodeErrors.Any())
                {
                    item.OwnershipNodeErrors.ForEach(y => y.ErrorMessage = Constants.ConciliationFailureMessage);
                }
                else
                {
                    item.OwnershipNodeErrors = SetOwnerShipNodeError(nodeId);
                }

                item.Status = status;
                item.OwnershipStatus = ownershipStatus;
            }

            ownershipNodeRepository.UpdateAll(ownerShip);
            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        private static List<OwnershipNodeError> SetOwnerShipNodeError(int? nodeId)
        {
            var nodeError = new List<OwnershipNodeError>();
            nodeError.Add(new OwnershipNodeError
            {
                CreatedDate = DateTime.Now,
                ExecutionDate = DateTime.Now,
                ErrorMessage = Constants.ConciliationFailureMessage,
                OwnershipNodeId = nodeId.GetValueOrDefault(),
            });
            return nodeError;
        }
    }
}
