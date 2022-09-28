// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateOfficialPointsAttribute.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Sap.Api.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Host.Core;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// The ADAL token acquisition exception filter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [CLSCompliant(false)]
    public sealed class ValidateOfficialPointsAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The argument name.
        /// </summary>
        private readonly string argumentName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateOfficialPointsAttribute" /> class.
        /// </summary>
        /// <param name="argumentName">Name of the argument.</param>
        public ValidateOfficialPointsAttribute(string argumentName)
        {
            this.argumentName = argumentName;
        }

        /// <inheritdoc/>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(next, nameof(next));

            var listOfMovements = context.ActionArguments[this.argumentName];
            var repositoryFactory = (IRepositoryFactory)context.HttpContext.RequestServices.GetService(typeof(IRepositoryFactory));
            var resourceProvider = (IResourceProvider)context.HttpContext.RequestServices.GetService(typeof(IResourceProvider));
            var error = await ValidateItemsAsync(listOfMovements, repositoryFactory, resourceProvider).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(error))
            {
                await base.OnActionExecutionAsync(context, next).ConfigureAwait(false);
                return;
            }

            context.Result = context.HttpContext.BuildErrorResult(error);
        }

        private static async Task<string> ValidateItemsAsync(object argument, IRepositoryFactory repositoryFactory, IResourceProvider resourceProvider)
        {
            try
            {
                var items = (IEnumerable<SapMovement>)argument;

                //// Validate no records found.
                if (items == null || !items.Any())
                {
                    return resourceProvider.GetResource(Entities.Constants.NoRecordsFound);
                }

                //// Validate only 2 records.
                if (items.Count() > 2)
                {
                    return resourceProvider.GetResource(Entities.Constants.MoreThan2RecordsFound);
                }

                //// Validate official information object available.
                if (items.Any(x => x.BackupMovement == null))
                {
                    return resourceProvider.GetResource(Entities.Constants.OfficialInformationRequired);
                }

                if (items.Count() == 1)
                {
                    return await ValidateSingleMovementAsync(items, repositoryFactory, resourceProvider).ConfigureAwait(false);
                }

                if (items.Count() == 2)
                {
                    return await ValidateMultipleMovementAsync(items, repositoryFactory, resourceProvider).ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                //// Validate invalid structure.
                return resourceProvider.GetResource(Entities.Constants.MovementInvalidDataType);
            }

            return null;
        }

        private static async Task<string> ValidateSingleMovementAsync(IEnumerable<SapMovement> items, IRepositoryFactory repositoryFactory, IResourceProvider resourceProvider)
        {
            //// Validate single movement case it should be official.
            if (!items.Any(x => x.BackupMovement.IsOfficial == true))
            {
                return resourceProvider.GetResource(Entities.Constants.SingleMovementOfficialPoint);
            }

            var movementReceived = items.FirstOrDefault(x => x.BackupMovement.IsOfficial == true);
            var result = await GetMovementsAsync(new List<string> { movementReceived.MovementId }, repositoryFactory).ConfigureAwait(false);
            if (result == null || !result.Any())
            {
                //// Validate single movement stored in true.
                return resourceProvider.GetResource(Entities.Constants.SingleMovementStored);
            }

            var sapTrackingResult = GetLastSapTrackingResult(result);

            if (sapTrackingResult == null)
            {
                //// Movement not sent to SAP.
                return resourceProvider.GetResource(Entities.Constants.SingleMovementNotSendToSap);
            }

            var movementSentToSap = result.FirstOrDefault(x => x.MovementTransactionId == sapTrackingResult.MovementTransactionId);

            if (!IsValidDataReceivedWithTrue(movementSentToSap, movementReceived))
            {
                //// Validate movement send to sap  and received movement data is not same
                return resourceProvider.GetResource(Entities.Constants.SingleMovementDataNotMatchStored);
            }

            return null;
        }

        private static async Task<string> ValidateMultipleMovementAsync(IEnumerable<SapMovement> sapMovements, IRepositoryFactory repositoryFactory, IResourceProvider resourceProvider)
        {
            //// Validate movement should be official.
            if (sapMovements.Count(x => x.BackupMovement.IsOfficial == true) != 1)
            {
                return resourceProvider.GetResource(Entities.Constants.OfficialPointError);
            }

            //// Validate BackupMovementId not null for official movement.
            if (sapMovements.Any(x => x.BackupMovement.IsOfficial == true && string.IsNullOrEmpty(x.BackupMovement.BackupMovementId)))
            {
                return resourceProvider.GetResource(Entities.Constants.BackupMovementIdRequired);
            }

            //// Validate BackupMovementId for official movement should be same for backup movement id.
            var movementId = sapMovements.FirstOrDefault(x => x.BackupMovement.IsOfficial == false).MovementId;
            if (sapMovements.Any(x => x.BackupMovement.IsOfficial == true && x.BackupMovement.BackupMovementId != movementId))
            {
                return resourceProvider.GetResource(Entities.Constants.SameBackupMovementId);
            }

            //// Validate GlobalMovementId should be same for both the movements.
            var backupGlobalMovementId = sapMovements.FirstOrDefault(x => x.BackupMovement.IsOfficial == false).BackupMovement.GlobalMovementId;
            var officialGlobalMovementId = sapMovements.FirstOrDefault(x => x.BackupMovement.IsOfficial == true).BackupMovement.GlobalMovementId;
            if (officialGlobalMovementId != backupGlobalMovementId)
            {
                return resourceProvider.GetResource(Entities.Constants.SameGlobalMovementId);
            }

            var officialMovement = sapMovements.FirstOrDefault(x => x.BackupMovement.IsOfficial == true);
            var backupMovement = sapMovements.FirstOrDefault(x => x.BackupMovement.IsOfficial == false);

            if (!IsValidDataReceived(backupMovement, officialMovement))
            {
                //// Both movement data not valid.
                return resourceProvider.GetResource(Entities.Constants.BothMovementDataNotvalid);
            }

            return await ValidateMultipleMovementDataWithTrueAsync(officialMovement.MovementId, backupMovement.MovementId, sapMovements, resourceProvider, repositoryFactory).ConfigureAwait(false);
        }

        private static async Task<string> ValidateMultipleMovementDataWithTrueAsync(
            string officialMovementId, string backupMovementMovementId, IEnumerable<SapMovement> sapMovements, IResourceProvider resourceProvider, IRepositoryFactory repositoryFactory)
        {
            var movementsFromTrue = await GetMovementsAsync(new List<string> { officialMovementId, backupMovementMovementId }, repositoryFactory).ConfigureAwait(false);
            if (movementsFromTrue == null || !movementsFromTrue.Any())
            {
                //// Validate atleast one movement is stored in true.
                return resourceProvider.GetResource(Entities.Constants.AtLeastOneMovementStored);
            }

            var sapTrackingResult = GetLastSapTrackingResult(movementsFromTrue);

            if (sapTrackingResult == null)
            {
                //// Movement not sent to SAP.
                return resourceProvider.GetResource(Entities.Constants.MultipleMovementNotSendToSap);
            }

            var movementSentToSap = movementsFromTrue.FirstOrDefault(x => x.MovementTransactionId == sapTrackingResult.MovementTransactionId);

            if (sapMovements.Any(x => IsValidDataReceivedWithTrue(movementSentToSap, x)))
            {
                return null;
            }

            //// Movement send to sap  and received movement data is not same
            return string.Format(
                            CultureInfo.InvariantCulture,
                            resourceProvider.GetResource(Entities.Constants.MultipleMovementDataNotMatchStored),
                            movementSentToSap.MovementId);
        }

        private static SapTracking GetLastSapTrackingResult(IEnumerable<Movement> result)
        {
            var sapTrackings = result.SelectMany(x => x.SapTracking);
            if (sapTrackings.Any())
            {
                return sapTrackings.Where(x => x.StatusTypeId == StatusType.PROCESSED).OrderByDescending(x => x.OperationalDate).FirstOrDefault();
            }

            return null;
        }

        private static bool IsValidDataReceivedWithTrue(Movement movementSentToSap, SapMovement movementReceived)
        {
            return ValidateMovementSource(movementSentToSap.MovementSource, movementReceived.MovementSource) &&
                   ValidateMovementDestination(movementSentToSap.MovementDestination, movementReceived.MovementDestination) &&
                   movementSentToSap.NetStandardVolume == movementReceived.NetStandardVolume;
        }

        private static bool ValidateMovementSource(MovementSource movementSource, SapMovementSource sapMovementSource)
        {
            var sourceNodeId = movementSource != null ? Convert.ToString(movementSource.SourceNodeId, CultureInfo.InvariantCulture) : null;
            return sourceNodeId == sapMovementSource?.SourceNodeId &&
                   movementSource?.SourceProductId == sapMovementSource?.SourceProductId;
        }

        private static bool ValidateMovementDestination(MovementDestination movementDestination, SapMovementDestination sapMovementdestination)
        {
            var destinationNodeId = movementDestination != null ? Convert.ToString(movementDestination.DestinationNodeId, CultureInfo.InvariantCulture) : null;
            return destinationNodeId == sapMovementdestination?.DestinationNodeId &&
                   movementDestination?.DestinationProductId == sapMovementdestination?.DestinationProductId;
        }

        private static bool IsValidDataReceived(SapMovement backupmovement, SapMovement officialmovement)
        {
            return ValidateSapMovementSource(backupmovement.MovementSource, officialmovement.MovementSource) &&
                   ValidateSapMovementDestination(backupmovement.MovementDestination, officialmovement.MovementDestination);
        }

        private static bool ValidateSapMovementSource(SapMovementSource backupMovementSource, SapMovementSource officialMovementSource)
        {
            return backupMovementSource?.SourceNodeId == officialMovementSource?.SourceNodeId &&
                   backupMovementSource?.SourceProductId == officialMovementSource?.SourceProductId;
        }

        private static bool ValidateSapMovementDestination(SapMovementDestination backupMovementDestination, SapMovementDestination officialMovementdestination)
        {
            return backupMovementDestination?.DestinationNodeId == officialMovementdestination?.DestinationNodeId &&
                   backupMovementDestination?.DestinationProductId == officialMovementdestination?.DestinationProductId;
        }

        private static async Task<IEnumerable<Movement>> GetMovementsAsync(List<string> movementIds, IRepositoryFactory repositoryFactory)
        {
            var movementRepository = repositoryFactory.CreateRepository<Movement>();
            return await movementRepository.GetAllAsync(
                 a =>
                 movementIds.Contains(a.MovementId) && a.NetStandardVolume > 0,
                 "MovementSource",
                 "MovementSource.SourceNode",
                 "MovementSource.SourceProduct",
                 "MovementDestination",
                 "MovementDestination.DestinationNode",
                 "MovementDestination.DestinationProduct",
                 "SapTracking").ConfigureAwait(false);
        }
    }
}