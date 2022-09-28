// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipResultService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Castle.Core.Internal;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.Response;

    /// <summary>
    /// The OwnershipResultService.
    /// </summary>
    public class OwnershipResultService : IOwnershipResultService
    {
        /// <inheritdoc/>
        public async Task<IEnumerable<Movement>> BuildOwnershipMovementResultsAsync(
            IEnumerable<CommercialMovementsResult> commercialMovementsResults,
            IEnumerable<NewMovement> newMovementsList,
            IEnumerable<CancellationMovementDetail> cancellationMovements,
            int ticketId,
            IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(ticketId, nameof(ticketId));
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            var ticketRepository = unitOfWork.CreateRepository<Ticket>();
            var ticket = await ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);

            var newMovements = new List<Movement>();
            if (!commercialMovementsResults.IsNullOrEmpty())
            {
                var result = await BuildCommercialMovementOwnershipAsync(commercialMovementsResults, ticket, unitOfWork).ConfigureAwait(false);
                newMovements.AddRange(result);
            }

            if (!newMovementsList.IsNullOrEmpty())
            {
                var result = await BuildNewMovementsOwnershipAsync(newMovementsList, ticket, unitOfWork).ConfigureAwait(false);
                newMovements.AddRange(result);
            }

            if (!cancellationMovements.IsNullOrEmpty())
            {
                var result = BuildCancellationMovementsOwnership(cancellationMovements, ticket);
                newMovements.AddRange(result);
            }

            return newMovements;
        }

        private static async Task<IEnumerable<Movement>> BuildCommercialMovementOwnershipAsync(
            IEnumerable<CommercialMovementsResult> commercialMovementsResults,
            Ticket ticket,
            IUnitOfWork unitOfWork)
        {
            var contractRepository = unitOfWork.CreateRepository<Ecp.True.Entities.Registration.Contract>();
            var movementRepository = unitOfWork.CreateRepository<Movement>();
            var newMovements = new List<Movement>();

            var contracts = await contractRepository.GetAllAsync(x => commercialMovementsResults.Select(y => y.ContractId).Distinct().Contains(x.ContractId)).ConfigureAwait(false);

            List<Movement> movements = new List<Movement>();
            foreach (var mid in commercialMovementsResults.Select(y => y.MovementId).Distinct())
            {
                var movementsPerId = await movementRepository.GetAllAsync(x => x.MovementTransactionId == mid, Constants.MovementDestination, Constants.MovementSource).ConfigureAwait(true);
                movements.AddRange(movementsPerId);
            }

            foreach (var commercialMovementsResult in commercialMovementsResults)
            {
                var contract = contracts.FirstOrDefault(x => x.ContractId == commercialMovementsResult.ContractId);
                var newMovementContract = contract.ToMovementContractForPurchaseAndSales(commercialMovementsResult, ticket.TicketId);
                var movement = movements.FirstOrDefault(x => x.MovementTransactionId == commercialMovementsResult.MovementId);
                var newMovement = movement.ToMovementsForPurchaseAndSales(commercialMovementsResult, ticket, newMovementContract);
                newMovement.SourceSystemId = (int)SourceSystem.FICO;
                newMovement.SystemId = (int)SourceSystem.FICO;
                newMovement.Period = new MovementPeriod();
                newMovement.Period.StartTime = newMovement.OperationalDate;
                newMovement.Period.EndTime = newMovement.OperationalDate;
                newMovements.Add(newMovement);
            }

            return newMovements;
        }

        private static async Task<IEnumerable<Movement>> BuildNewMovementsOwnershipAsync(
            IEnumerable<NewMovement> newMovementsList,
            Ticket ticket,
            IUnitOfWork unitOfWork)
        {
            var newMovements = new List<Movement>();
            var collaborationMovements = newMovementsList.Where(a => a.AgreementType == Constants.Collaboration);
            newMovements.AddRange(await BuildCollaborationMovementsAsync(collaborationMovements, ticket, unitOfWork).ConfigureAwait(false));

            var evacuationMovements = newMovementsList.Where(a => a.AgreementType == Constants.Evacuation);
            newMovements.AddRange(BuildEvacuationMovements(evacuationMovements, ticket));

            return newMovements;
        }

        private static async Task<IEnumerable<Movement>> BuildCollaborationMovementsAsync(
            IEnumerable<NewMovement> newMovementsList,
            Ticket ticket,
            IUnitOfWork unitOfWork)
        {
            var newMovementsEventsIdsList = newMovementsList.Select(x => x.EventId).Distinct();
            var newMovementIdsList = newMovementsList.Select(x => x.MovementId).Distinct();
            var eventRepository = unitOfWork.CreateRepository<Ecp.True.Entities.Registration.Event>();
            var movementRepository = unitOfWork.CreateRepository<Movement>();
            var events = await eventRepository.GetAllAsync(x => newMovementsEventsIdsList.Contains(x.EventId)).ConfigureAwait(false);
            var movements = await movementRepository.GetAllAsync(
                x => newMovementIdsList.Contains(x.MovementTransactionId),
                Constants.MovementDestination,
                Constants.MovementSource).ConfigureAwait(false);
            var newMovements = new List<Movement>();

            foreach (var existingmovement in newMovementsList)
            {
                var existingEvent = events.FirstOrDefault(x => x.EventId == existingmovement.EventId);
                var inputMovement = existingEvent.ToEventForCollaborationAgreements(existingmovement, ticket.TicketId);
                var outputMovement = existingEvent.ToEventForCollaborationAgreements(existingmovement, ticket.TicketId);
                var movement = movements.FirstOrDefault(x => x.MovementTransactionId == existingmovement.MovementId);
                var result = movement.ToMovementsForCollaborationAgreements(existingmovement, ticket, inputMovement, outputMovement);
                newMovements.AddRange(result);
            }

            return newMovements;
        }

        private static IEnumerable<Movement> BuildEvacuationMovements(IEnumerable<NewMovement> newMovementsList, Ticket ticket)
        {
            var newMovements = new List<Movement>();

            foreach (var existingmovement in newMovementsList)
            {
                var result = existingmovement.ToMovementsForLoansPayments(ticket);
                newMovements.AddRange(result);
            }

            return newMovements;
        }

        private static IEnumerable<Movement> BuildCancellationMovementsOwnership(
            IEnumerable<CancellationMovementDetail> cancellationMovements,
            Ticket ticket)
        {
            return cancellationMovements.Select(cancellationMovement => cancellationMovement.ToCancellationMovementWithOwnership(ticket));
        }
    }
}
