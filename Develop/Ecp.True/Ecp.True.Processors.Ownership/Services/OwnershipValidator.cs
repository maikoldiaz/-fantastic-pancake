// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipValidator.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Services.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.Response;

    /// <summary>
    /// The ownership validator.
    /// </summary>
    /// <seealso cref="IOwnershipProcessor" />
    public class OwnershipValidator : IOwnershipValidator
    {
        /// <summary>
        /// The repository factory.
        /// </summary>
        private readonly IRepositoryFactory repositoryFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipValidator" /> class.
        /// </summary>
        /// <param name="repositoryFactory">The repository factory.</param>
        public OwnershipValidator(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        /// <summary>
        /// Validates ownership results.
        /// </summary>
        /// <param name="ownershipRuleData">Ownership rule data.</param>
        /// <returns>
        /// The error information.
        /// </returns>
        public static IEnumerable<ErrorInfo> ValidateOwnershipRuleResult(
            OwnershipRuleData ownershipRuleData)
        {
            ArgumentValidators.ThrowIfNull(ownershipRuleData, nameof(ownershipRuleData));
            var errors = new List<ErrorInfo>();
            var inventoryList = ownershipRuleData.OwnershipRuleResponse.InventoryResults;
            var movementList = ownershipRuleData.OwnershipRuleResponse.MovementResults;

            ValidateInventoryResults(ownershipRuleData, errors);
            if (!errors.Any())
            {
                ValidateMovementResults(ownershipRuleData, errors);
            }

            if (inventoryList.Any())
            {
                ValidateOwnershipPercentage(inventoryList, errors);
            }

            if (movementList.Any())
            {
                ValidateOwnershipPercentage(movementList, errors);
            }

            return errors;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ErrorInfo>> ValidateOwnershipRuleErrorAsync(IEnumerable<OwnershipErrorInventory> inventoryList, IEnumerable<OwnershipErrorMovement> movementList)
        {
            var errors = new List<ErrorInfo>();

            if (inventoryList.Any())
            {
                await this.ValidateInventoryErrorsAsync(inventoryList, errors).ConfigureAwait(false);
            }

            if (movementList.Any())
            {
                await this.ValidateMovementErrorsAsync(movementList, errors).ConfigureAwait(false);
            }

            return errors;
        }

        /// <summary>
        /// Validates ticket exists.
        /// </summary>
        /// <param name="ticketId">The ticketId.</param>
        /// <returns>
        /// Return the ticket.
        /// </returns>
        public async Task<(ICollection<ErrorInfo>, bool)> CheckTicketExistsAsync(int ticketId)
        {
            var errors = new List<ErrorInfo>();

            var ticket = await this.repositoryFactory.CreateRepository<Ticket>().FirstOrDefaultAsync(x => x.TicketId == ticketId && x.TicketTypeId == TicketType.Ownership).ConfigureAwait(false);
            if (ticket == null)
            {
                errors.Add(new ErrorInfo(OwnershipConstants.InvalidTicket));
                return (errors, false);
            }

            if (ticket.Status != StatusType.PROCESSING)
            {
                errors.Add(new ErrorInfo(OwnershipConstants.TicketAlreadyProcessed));
                return (errors, false);
            }

            return (errors, true);
        }

        /// <summary>
        /// Validates inventory Id.
        /// </summary>
        /// <param name="ownershipRuleData">Ownership rule data.</param>
        /// <param name="errors">List of errors.</param>
        private static void ValidateInventoryResults(OwnershipRuleData ownershipRuleData, ICollection<ErrorInfo> errors)
        {
            ArgumentValidators.ThrowIfNull(errors, nameof(errors));
            ArgumentValidators.ThrowIfNull(ownershipRuleData, nameof(ownershipRuleData));

            var inventoryList = ownershipRuleData.OwnershipRuleResponse.InventoryResults;

            var inventoryProductIdsList = inventoryList.Select(x => x.InventoryId).Distinct();
            var requestInventoryProductIdsList = ownershipRuleData.OwnershipRuleRequest.InventoryOperationalData.Select(x => x.InventoryId).Distinct();
            if (!requestInventoryProductIdsList.All(inventoryProductIdsList.Contains))
            {
                errors.Add(new ErrorInfo(Constants.ValidationFailureMessage));
            }
        }

        /// <summary>
        /// Validates inventory Id.
        /// </summary>
        /// <param name="ownershipRuleData">The ownership rule data.</param>
        /// <param name="errors">List of errors.</param>
        private static void ValidateMovementResults(OwnershipRuleData ownershipRuleData, ICollection<ErrorInfo> errors)
        {
            ArgumentValidators.ThrowIfNull(errors, nameof(errors));
            ArgumentValidators.ThrowIfNull(ownershipRuleData, nameof(ownershipRuleData));

            var movementList = ownershipRuleData.OwnershipRuleResponse.MovementResults;
            var movementIdsList = movementList.Select(x => x.MovementId).Distinct();

            // movement id list without cancellation movements
            var requestMovementsIdsList =
                ownershipRuleData.OwnershipRuleRequest.MovementsOperationalData.
                Where(x => x.MovementTypeId != Constants.EntryCancellationMovementType
                && x.MovementTypeId != Constants.DepartureCancellationMovementType).Select(x => x.MovementTransactionId).Distinct();

            // Input movements
            var previousMovementIds = ownershipRuleData.OwnershipRuleRequest.PreviousMovementsOperationalData.Select(x => x.MovementId).Distinct();

            var movementIdsToValidate = requestMovementsIdsList.Except(previousMovementIds).Distinct();

            if (!movementIdsToValidate.All(movementIdsList.Contains))
            {
                errors.Add(new ErrorInfo(Constants.ValidationFailureMessage));
            }
        }

        /// <summary>
        /// Validates ticket Id.
        /// </summary>
        /// <param name="inventoryList">Inventory list.</param>
        /// <param name="errors">List of errors.</param>
        private static void ValidateOwnershipPercentage(IEnumerable<OwnershipResultInventory> inventoryList, ICollection<ErrorInfo> errors)
        {
            var result = inventoryList.GroupBy(i => i.InventoryId).Select(g => g.Sum(o => o.OwnershipPercentage));
            if (result.Any(r => r != 100.00M))
            {
                errors.Add(new ErrorInfo(Constants.ValidateInventoryOwnershipPercentageFailureMessage));
            }
        }

        /// <summary>
        /// Validates ticket Id.
        /// </summary>
        /// <param name="movementList">Movement list.</param>
        /// <param name="errors">List of errors.</param>
        private static void ValidateOwnershipPercentage(IEnumerable<OwnershipResultMovement> movementList, ICollection<ErrorInfo> errors)
        {
            var result = movementList.GroupBy(i => i.MovementId).Select(g => g.Sum(o => o.OwnershipPercentage));
            if (result.Any(r => r != 100.00M))
            {
                errors.Add(new ErrorInfo(Constants.ValidateMovementOwnershipPercentageFailureMessage));
            }
        }

        /// <summary>
        /// Validates inventory Id.
        /// </summary>
        /// <param name="inventoryList">The list.</param>
        /// <param name="errors">List of errors.</param>
        /// <returns>
        /// Return inventory exists or not.
        /// </returns>
        private async Task ValidateInventoryErrorsAsync(IEnumerable<OwnershipErrorInventory> inventoryList, ICollection<ErrorInfo> errors)
        {
            ArgumentValidators.ThrowIfNull(errors, nameof(errors));
            ArgumentValidators.ThrowIfNull(inventoryList, nameof(inventoryList));

            var inventoryProductRepository = this.repositoryFactory.CreateRepository<InventoryProduct>();

            var inventoryProductIds = inventoryList.Select(x => x.InventoryId).ToList();
            var trueInventoryProducts = await inventoryProductRepository.GetAllAsync(x => inventoryProductIds.Contains(x.InventoryProductId)).ConfigureAwait(false);

            foreach (var inventory in inventoryList)
            {
                if (!trueInventoryProducts.Any(x => x.InventoryProductId == inventory.InventoryId))
                {
                    errors.Add(new ErrorInfo(string.Format(CultureInfo.InvariantCulture, Constants.ValidateInventoryProductFailureMessage, inventory.InventoryId)));
                    continue;
                }

                var trueInventoryProduct = trueInventoryProducts.
                    FirstOrDefault(x => x.InventoryProductId == inventory.InventoryId);

                if (trueInventoryProduct.NodeId != inventory.NodeId)
                {
                    errors.Add(new ErrorInfo(string.Format(CultureInfo.InvariantCulture, Constants.ValidateInvalidNodeForInventoryProductFailureMessage, inventory.InventoryId)));
                }
            }
        }

        /// <summary>
        /// Validates inventory Id.
        /// </summary>
        /// <param name="movementList">The list.</param>
        /// <param name="errors">List of errors.</param>
        /// <returns>
        /// Return inventory exists or not.
        /// </returns>
        private async Task ValidateMovementErrorsAsync(IEnumerable<OwnershipErrorMovement> movementList, ICollection<ErrorInfo> errors)
        {
            ArgumentValidators.ThrowIfNull(errors, nameof(errors));
            ArgumentValidators.ThrowIfNull(movementList, nameof(movementList));
            try
            {
                var movementsRepository = this.repositoryFactory.CreateRepository<Movement>();

                var movementIds = movementList.Select(x => x.MovementId).ToList();
                var trueMovements = await movementsRepository.GetAllAsync(x => movementIds.Contains(x.MovementTransactionId), "MovementSource", "MovementDestination").ConfigureAwait(false);

                foreach (var movement in movementList)
                {
                    var trueMovement = trueMovements.FirstOrDefault(x => x.MovementTransactionId == movement.MovementId);
                    if (trueMovement == null)
                    {
                        errors.Add(new ErrorInfo(string.Format(CultureInfo.InvariantCulture, Constants.ValidateMovementFailureMessage, movement.MovementId)));
                        continue;
                    }

                    if (movement.SourceNodeId != 0 &&
                        !(trueMovement.MovementSource?.SourceNodeId.GetValueOrDefault() == movement.SourceNodeId
                            || trueMovement.MovementDestination?.DestinationNodeId.GetValueOrDefault() == movement.SourceNodeId))
                    {
                        errors.Add(new ErrorInfo(string.Format(CultureInfo.InvariantCulture, Constants.ValidateInvalidNodeForMovementFailureMessage, movement.MovementId)));
                    }
                }
            }
            catch (System.Exception ex)
            {
                errors.Add(new ErrorInfo(string.Format(CultureInfo.InvariantCulture, Constants.ConfigurationError, ex.Message)));
            }
        }
    }
}
