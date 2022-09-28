// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementValidator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Balance.Operation;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Registration.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The Movement validator.
    /// </summary>
    /// <seealso cref="IMovementValidatorService" />
    public class MovementValidator : BaseValidator, IMovementValidator
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<MovementValidator> logger;

        /// <summary>
        /// The inventory validator service.
        /// </summary>
        private readonly IBlobOperations blobOperations;

        /// <summary>
        /// The composite validator factory.
        /// </summary>
        private readonly ICompositeValidatorFactory compositeValidatorFactory;

        /// <summary>
        /// The file registration transaction service.
        /// </summary>
        private readonly IFileRegistrationTransactionService fileRegistrationTransactionService;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The invalid message.
        /// </summary>
        private readonly string validationMessage = "Movement validation started";

        /// <summary>
        /// The validated.
        /// </summary>
        private readonly string validated = "The homologated entity is validated";

        /// <summary>
        /// The validated.
        /// </summary>
        private readonly string validatedMovement = "The movement entity is validated";

        /// <summary>
        /// The failed.
        /// </summary>
        private readonly string failed = "The homologated entity is failed";

        /// <summary>
        /// The validate create.
        /// </summary>
        private readonly string validateCreate = "The movement identifier already exists in the system";

        /// <summary>
        /// The validate update.
        /// </summary>
        private readonly string validateUpdate = "The identifier of the movement to adjust does not exist";

        /// <summary>
        /// The validate update.
        /// </summary>
        private readonly string validateDelete = "The identifier of the movement not able to delete";

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementValidator" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="blobOperations">The blobOperations.</param>
        /// <param name="compositeValidatorFactory">The composite validator factory.</param>
        /// <param name="registrationService">The registration processor.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="fileRegistrationTransactionService">The file registration transaction service.</param>
        public MovementValidator(
            ITrueLogger<MovementValidator> logger,
            IBlobOperations blobOperations,
            ICompositeValidatorFactory compositeValidatorFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            IAzureClientFactory azureClientFactory,
            IFileRegistrationTransactionService fileRegistrationTransactionService)
             : base(azureClientFactory)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            this.logger = logger;
            this.blobOperations = blobOperations;
            this.compositeValidatorFactory = compositeValidatorFactory;
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.fileRegistrationTransactionService = fileRegistrationTransactionService;
        }

        /// <summary>
        /// Validate the movement asynchronous.
        /// </summary>
        /// <param name="fileRegistrationTransaction">The file registration transaction.</param>
        /// <param name="homologatedJson">The homologated json.</param>
        /// <returns>
        /// The [True] if validation passes, [False] otherwise.
        /// </returns>
        public async Task<(bool isValid, Movement movement)> ValidateMovementAsync(FileRegistrationTransaction fileRegistrationTransaction, JToken homologatedJson)
        {
            ArgumentValidators.ThrowIfNull(fileRegistrationTransaction, nameof(fileRegistrationTransaction));
            this.logger.LogInformation(this.validationMessage, fileRegistrationTransaction.UploadId);

            var movement = this.blobOperations.GetHomologatedObject<Movement>(homologatedJson, fileRegistrationTransaction.UploadId);
            if (fileRegistrationTransaction.SkipValidation)
            {
                return (true, movement.Item1);
            }

            if (movement.Item1 != null)
            {
                var isValid = await this.DoValidateMovementAsync(fileRegistrationTransaction, movement.Item1).ConfigureAwait(false);
                return (isValid, movement.Item1);
            }
            else
            {
                var pendingTransaction = homologatedJson.ToPendingTransaction(fileRegistrationTransaction, movement.Item2);
                await this.fileRegistrationTransactionService.RegisterFailureAsync(
                    pendingTransaction,
                    fileRegistrationTransaction.FileRegistrationTransactionId,
                    movement.Item3,
                    Constants.InvalidDataType,
                    fileRegistrationTransaction.UploadId).ConfigureAwait(false);
                return (false, null);
            }
        }

        /// <summary>
        /// validate if the movement Contains Version.
        /// </summary>
        /// <param name="movement">movement.</param>
        /// <returns>bool.</returns>
        private static bool ContainsVersion(Movement movement)
        {
            if (movement.ScenarioId == ScenarioType.OFFICER)
            {
                return !string.IsNullOrEmpty(movement.Version);
            }

            return true;
        }

        private async Task<bool> DoValidateMovementAsync(FileRegistrationTransaction fileRegistrationTransaction, Movement movement)
        {
            var result = await this.compositeValidatorFactory.MovementCompositeValidator.ValidateAsync(movement).ConfigureAwait(false);
            this.logger.LogInformation(this.validated, fileRegistrationTransaction.UploadId);

            if (!result.IsSuccess)
            {
                await this.DoRegisterFailureAsync(
                movement, fileRegistrationTransaction, result.ErrorInfo.Select(r => r.Message), this.failed, fileRegistrationTransaction.UploadId).ConfigureAwait(false);
                return false;
            }

            this.logger.LogInformation(this.validatedMovement, fileRegistrationTransaction.UploadId);

            //// In case of SAP official and backup movements no need to run the movement validation.
            return !string.IsNullOrEmpty(movement.GlobalMovementId) || await this.ValidateMovementAsync(movement, fileRegistrationTransaction).ConfigureAwait(false);
        }

        /// <summary>
        /// Validates the movement asynchronous.
        /// </summary>
        /// <param name="movement">The movement.</param>
        /// <param name="fileRegistrationTransaction">The file registration transaction message.</param>
        /// <exception cref="System.InvalidOperationException">Unknown event type {movement.EventType}.</exception>
        private async Task<bool> ValidateMovementAsync(Movement movement, FileRegistrationTransaction fileRegistrationTransaction)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));
            if (movement.EventType.EqualsIgnoreCase(EventType.Insert.ToString("G")))
            {
                return await this.ValidateCreateMovementAsync(movement, fileRegistrationTransaction).ConfigureAwait(false);
            }

            if (movement.EventType.EqualsIgnoreCase(EventType.Update.ToString("G")))
            {
                return await this.ValidateUpdateMovementAsync(movement, fileRegistrationTransaction).ConfigureAwait(false);
            }

            if (movement.EventType.EqualsIgnoreCase(EventType.Delete.ToString("G")))
            {
                return await this.ValidateDeleteMovementAsync(movement, fileRegistrationTransaction).ConfigureAwait(false);
            }

            throw new InvalidOperationException($"Unknown event type {movement.EventType}");
        }

        /// <summary>
        /// Validates the create movement asynchronous.
        /// </summary>
        /// <param name="movement">The movement.</param>
        /// <param name="fileRegistrationTransaction">The file registration transaction message.</param>
        private async Task<bool> ValidateCreateMovementAsync(Movement movement, FileRegistrationTransaction fileRegistrationTransaction)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));

            var result = await this.unitOfWork.MovementRepository.GetLatestMovementAsync(movement.MovementId).ConfigureAwait(false);
            var errors = new List<string>();

            // Either no movement with this ID is present
            // OrElse the movement with the same ID is already deleted.
            if (!(result == null ||
                result.EventType.EqualsIgnoreCase(EventType.Delete.ToString("G"))))
            {
                errors.Add(Constants.MovementCreateConflict);
            }

            if (!await this.ValidateNetAndGrossStandardVolumeAsync(movement).ConfigureAwait(false))
            {
                errors.Add(Constants.ErrorNegativeValueNoAnnulation);
            }

            if (!ContainsVersion(movement))
            {
                errors.Add(Constants.VersionRequired);
            }

            if (errors.Any())
            {
                await this.DoRegisterFailureAsync(
                movement, fileRegistrationTransaction, errors, this.validateCreate, fileRegistrationTransaction.UploadId).ConfigureAwait(false);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the update movement asynchronous.
        /// </summary>
        /// <param name="movement">The movement.</param>
        /// <param name="fileRegistrationTransaction">The file registration transaction message.</param>
        private async Task<bool> ValidateUpdateMovementAsync(Movement movement, FileRegistrationTransaction fileRegistrationTransaction)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));
            var result = await this.unitOfWork.MovementRepository.GetLatestMovementAsync(movement.MovementId).ConfigureAwait(false);
            var errors = new List<string>();

            // There must be a movement with same Id, and
            // The volume must be positive, and
            // The last recorded event type is either insert or update
            if (!(result != null &&
                result.NetStandardVolume.HasValue &&
                !result.NetStandardVolume.IsNegative() &&
                !result.EventType.EqualsIgnoreCase(EventType.Delete.ToString("G"))))
            {
                errors.Add(Constants.MovementUpdateNotFound);
            }

            if (!await this.ValidateNetAndGrossStandardVolumeAsync(movement).ConfigureAwait(false))
            {
                errors.Add(Constants.ErrorNegativeValueNoAnnulation);
            }

            if (!ContainsVersion(movement))
            {
                errors.Add(Constants.VersionRequired);
            }

            if (errors.Any())
            {
                await this.DoRegisterFailureAsync(
                movement, fileRegistrationTransaction, errors, this.validateUpdate, fileRegistrationTransaction.UploadId).ConfigureAwait(false);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the delete movement asynchronous.
        /// </summary>
        /// <param name="movement">The movement.</param>
        /// <param name="fileRegistrationTransaction">The file registration transaction message.</param>
        private async Task<bool> ValidateDeleteMovementAsync(Movement movement, FileRegistrationTransaction fileRegistrationTransaction)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));
            var result = await this.unitOfWork.MovementRepository.GetLatestMovementAsync(movement.MovementId).ConfigureAwait(false);

            // There must be a movement with same Id, and
            // The volume must be positive, and
            // The last recorded event type is either insert or update
            if (result != null &&
                result.NetStandardVolume.HasValue &&
                !result.NetStandardVolume.IsNegative() &&
                !result.EventType.EqualsIgnoreCase(EventType.Delete.ToString("G")))
            {
                return true;
            }

            await this.DoRegisterFailureAsync(
            movement, fileRegistrationTransaction, new[] { Constants.MovementDeleteNotFound }, this.validateDelete, fileRegistrationTransaction.UploadId).ConfigureAwait(false);
            return false;
        }

        /// <summary>
        /// Validates the Net And Gross Standard Volume asynchronous.
        /// </summary>
        /// <param name="movement">The movement.</param>
        private async Task<bool> ValidateNetAndGrossStandardVolumeAsync(Movement movement)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));
            var repository = this.unitOfWork.CreateRepository<Annulation>();
            var annulations = await repository.GetCountAsync(x => x.AnnulationMovementTypeId == movement.MovementTypeId && x.IsActive == true).ConfigureAwait(false);
            if (movement.NetStandardVolume < 0 || movement.GrossStandardVolume < 0)
            {
                return movement.ScenarioId == ScenarioType.OFFICER &&
                movement.SourceSystemId == True.Core.Constants.ManualMovOfficial &&
                annulations > 0;
            }

            return true;
        }

        /// <summary>
        /// Does the register failure.
        /// </summary>
        /// <param name="movement">The movement.</param>
        /// <param name="fileRegistrationTransaction">The file registration  session message.</param>
        private async Task DoRegisterFailureAsync(Movement movement, FileRegistrationTransaction fileRegistrationTransaction, IEnumerable<string> errorInfos, string errorMessage, params object[] args)
        {
            var pendingTransaction = movement.ToPendingTransaction(fileRegistrationTransaction, errorInfos);
            await this.fileRegistrationTransactionService.RegisterFailureAsync(
                pendingTransaction,
                fileRegistrationTransaction.FileRegistrationTransactionId,
                null,
                errorMessage,
                args).ConfigureAwait(false);
        }
    }
}