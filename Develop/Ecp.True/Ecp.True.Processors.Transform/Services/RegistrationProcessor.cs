// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistrationProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// -------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Services
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Registration.Interfaces;
    using Ecp.True.Processors.Transform.Services.Interfaces;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The RegistrationProcessors.
    /// </summary>
    /// <seealso cref="IRegistrationProcessor" />
    public class RegistrationProcessor : IRegistrationProcessor
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<RegistrationProcessor> logger;

        /// <summary>
        /// The movement validator.
        /// </summary>
        private readonly IMovementValidator movementValidator;

        /// <summary>
        /// The inventory validator.
        /// </summary>
        private readonly IInventoryValidator inventoryValidator;

        /// <summary>
        /// The event validator.
        /// </summary>
        private readonly IEventValidator eventValidator;

        /// <summary>
        /// The contract validator.
        /// </summary>
        private readonly IContractValidator contractValidator;

        /// <summary>
        /// The BLOB operations.
        /// </summary>
        private readonly IBlobOperations blobOperations;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Registration strategy factory.
        /// </summary>
        private readonly IRegistrationStrategyFactory registrationStrategyFactory;

        /// <summary>
        /// The file registration transaction service.
        /// </summary>
        private readonly IFileRegistrationTransactionService fileRegistrationTransactionService;

        /// <summary>
        /// The downloaded.
        /// </summary>
        private readonly string downloaded = "The homologated entities are downloaded";

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationProcessor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="movementValidator">The movement validator.</param>
        /// <param name="inventoryValidator">The inventory validator.</param>
        /// <param name="eventValidator">The event validator.</param>
        /// <param name="contractValidator">The contract validator.</param>
        /// <param name="blobOperations">The BLOB operations.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="registrationStrategyFactory">The registration strategy factory.</param>
        /// <param name="fileRegistrationTransactionService">The file registration transaction service.</param>
        public RegistrationProcessor(
            ITrueLogger<RegistrationProcessor> logger,
            IMovementValidator movementValidator,
            IInventoryValidator inventoryValidator,
            IEventValidator eventValidator,
            IContractValidator contractValidator,
            IBlobOperations blobOperations,
            IUnitOfWorkFactory unitOfWorkFactory,
            IRegistrationStrategyFactory registrationStrategyFactory,
            IFileRegistrationTransactionService fileRegistrationTransactionService)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));

            this.logger = logger;
            this.movementValidator = movementValidator;
            this.inventoryValidator = inventoryValidator;
            this.eventValidator = eventValidator;
            this.contractValidator = contractValidator;
            this.blobOperations = blobOperations;
            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.registrationStrategyFactory = registrationStrategyFactory;
            this.fileRegistrationTransactionService = fileRegistrationTransactionService;
        }

        /// <summary>
        /// Registers the movement asynchronous.
        /// </summary>
        /// <param name="message">The file registration transaction.</param>
        /// <returns>The task.</returns>
        public async Task RegisterMovementAsync(FileRegistrationTransaction message)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            Movement movement = null;

            try
            {
                var homologatedToken = await this.GetHomologatedTokenAsync(message).ConfigureAwait(false);
                if (homologatedToken.Any() && message.SystemTypeId == Entities.Dto.SystemType.SAP)
                {
                    await this.TransformMovementAsync(homologatedToken).ConfigureAwait(false);
                }

                var result = await this.movementValidator.ValidateMovementAsync(message, homologatedToken).ConfigureAwait(false);
                movement = result.movement;
                if (result.isValid)
                {
                    movement.FileRegistrationTransactionId = message.FileRegistrationTransactionId;
                    movement.SystemTypeId = (int)message.SystemTypeId;
                    movement.MessageTypeId = GetMessageTypeId(movement.Classification);

                    await this.registrationStrategyFactory.MovementRegistrationStrategy.RegisterAsync(movement, this.unitOfWork).ConfigureAwait(false);
                    await this.UpdateFileRegistrationTransactionStatusAsync(message.FileRegistrationTransactionId, StatusType.PROCESSED).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                var error = ex.InnerException ?? ex;
                var errors = new[] { Constants.RegistrationErrorMessage };
                var fileRegistration = await this.GetFileRegistrationAsync(message.UploadId).ConfigureAwait(false);
                await this.fileRegistrationTransactionService.RegisterFailureAsync(
                    movement != null ?
                    movement.ToPendingTransaction(message, errors)
                    : fileRegistration.ToPendingTransaction(errors),
                    message.FileRegistrationTransactionId,
                    error,
                    error.Message,
                    message.SessionId).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Registers the inventory asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task RegisterInventoryAsync(FileRegistrationTransaction message)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            InventoryProduct inventory = null;

            try
            {
                var homologatedToken = await this.GetHomologatedTokenAsync(message).ConfigureAwait(false);
                if (homologatedToken.Any() && message.SystemTypeId == Entities.Dto.SystemType.SAP)
                {
                    await this.TransformInventoryAsync(homologatedToken).ConfigureAwait(false);
                }

                var result = await this.inventoryValidator.ValidateInventoryAsync(message, homologatedToken).ConfigureAwait(false);
                inventory = result.inventory;

                if (result.isValid)
                {
                    inventory.FileRegistrationTransactionId = message.FileRegistrationTransactionId;
                    await this.registrationStrategyFactory.InventoryProductRegistrationStrategy.RegisterAsync(inventory, this.unitOfWork).ConfigureAwait(false);
                    await this.UpdateFileRegistrationTransactionStatusAsync(message.FileRegistrationTransactionId, StatusType.PROCESSED).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                var error = ex.InnerException ?? ex;
                var errors = new[] { Constants.RegistrationErrorMessage };
                var fileRegistration = await this.GetFileRegistrationAsync(message.UploadId).ConfigureAwait(false);
                await this.fileRegistrationTransactionService.RegisterFailureAsync(
                    inventory != null ?
                    inventory.ToPendingTransaction(message, errors)
                    : fileRegistration.ToPendingTransaction(errors),
                    message.FileRegistrationTransactionId,
                    error,
                    error.Message,
                    message.SessionId).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Registers the event asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The task.</returns>
        public async Task RegisterEventAsync(FileRegistrationTransaction message)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            Event eventObject = null;

            try
            {
                var homologatedToken = await this.GetHomologatedTokenAsync(message).ConfigureAwait(false);
                var result = await this.eventValidator.ValidateEventAsync(message, homologatedToken).ConfigureAwait(false);
                eventObject = result.eventObject;

                if (result.isValid)
                {
                    eventObject.ActionType = Convert.ToString(message.ActionType.Value, CultureInfo.InvariantCulture);
                    eventObject.FileRegistrationTransactionId = message.FileRegistrationTransactionId;

                    await this.registrationStrategyFactory.EventRegistrationStrategy.RegisterAsync(eventObject, this.unitOfWork).ConfigureAwait(false);
                    await this.UpdateFileRegistrationTransactionStatusAsync(message.FileRegistrationTransactionId, StatusType.PROCESSED).ConfigureAwait(false);

                    this.logger.LogInformation($"{message.SystemTypeId} {MessageType.Events} processed successfully.", message.UploadId);
                }
            }
            catch (Exception ex)
            {
                var error = ex.InnerException ?? ex;
                var errors = new[] { Constants.RegistrationErrorMessage };
                var fileRegistration = await this.GetFileRegistrationAsync(message.UploadId).ConfigureAwait(false);
                await this.fileRegistrationTransactionService.RegisterFailureAsync(
                    eventObject != null ?
                    eventObject.ToPendingTransaction(message, errors)
                    : fileRegistration.ToPendingTransaction(errors),
                    message.FileRegistrationTransactionId,
                    error,
                    error.Message,
                    message.SessionId).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Registers the contract asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The task.</returns>
        public async Task RegisterContractAsync(FileRegistrationTransaction message)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            Contract contractObject = null;
            try
            {
                var homologatedToken = await this.GetHomologatedTokenAsync(message).ConfigureAwait(false);

                var result = await this.contractValidator.ValidateContractAsync(message, homologatedToken).ConfigureAwait(false);
                contractObject = result.contractObject;

                if (result.isValid)
                {
                    contractObject.FileRegistrationTransactionId = message.FileRegistrationTransactionId;

                    if ((message.MessageType == MessageType.Purchase || message.MessageType == MessageType.Sale) &&
                         GetIsDeleteContract(homologatedToken))
                    {
                        contractObject.ActionType = EventType.Delete.ToString("G");
                    }

                    await this.registrationStrategyFactory.ContractRegistrationStrategy.RegisterAsync(contractObject, this.unitOfWork).ConfigureAwait(false);
                    await this.UpdateFileRegistrationTransactionStatusAsync(message.FileRegistrationTransactionId, StatusType.PROCESSED).ConfigureAwait(false);

                    this.logger.LogInformation($"{message.SystemTypeId} {MessageType.Contract} processed successfully.", message.UploadId);
                }
            }
            catch (Exception ex)
            {
                var error = ex.InnerException ?? ex;
                var errors = new[] { Constants.RegistrationErrorMessage };
                var fileRegistration = await this.GetFileRegistrationAsync(message.UploadId).ConfigureAwait(false);
                await this.fileRegistrationTransactionService.RegisterFailureAsync(
                    contractObject != null ?
                    contractObject.ToPendingTransaction(message, errors)
                    : fileRegistration.ToPendingTransaction(errors),
                    message.FileRegistrationTransactionId,
                    error,
                    error.Message,
                    message.SessionId).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Validate if action is delete.
        /// </summary>
        /// <param name="homologatedToken"> The homologate token.</param>
        /// <returns>True if action is delete.</returns>
        private static bool GetIsDeleteContract(JToken homologatedToken)
        {
            if (string.IsNullOrEmpty((string)homologatedToken["ActionType"]))
            {
                return false;
            }

            return ((string)homologatedToken["ActionType"]).EqualsIgnoreCase(EventType.Delete.ToString("G"));
        }

        private static int GetMessageTypeId(string classification)
        {
            if (classification.EqualsIgnoreCase(Constants.MovementClassification))
            {
                return (int)MessageType.Movement;
            }

            if (classification.EqualsIgnoreCase(Constants.LossClassification))
            {
                return (int)MessageType.Loss;
            }

            if (classification.EqualsIgnoreCase(Constants.SpecialMovementClassification))
            {
                return (int)MessageType.SpecialMovement;
            }

            return (int)MessageType.Movement;
        }

        /// <summary>
        /// Handles the registration failure asynchronous.
        /// </summary>
        /// <param name="uploadId">The transaction identifier.</param>
        /// <returns>The task.</returns>
        private async Task<FileRegistration> GetFileRegistrationAsync(string uploadId)
        {
            var fileRegistrationRepository = this.unitOfWork.CreateRepository<FileRegistration>();
            var fileRegistration = await fileRegistrationRepository.FirstOrDefaultAsync(x => x.UploadId == uploadId).ConfigureAwait(false);
            return fileRegistration;
        }

        /// <summary>
        /// Gets the homologated token asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The JToken.</returns>
        private async Task<JToken> GetHomologatedTokenAsync(FileRegistrationTransaction message)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            ArgumentValidators.ThrowIfNull(message.BlobPath, nameof(message.BlobPath));

            var homologatedToken = await this.blobOperations.GetHomologatedJsonAsync(message.BlobPath, message.UploadId).ConfigureAwait(false);
            this.logger.LogInformation(this.downloaded, message.UploadId);

            return homologatedToken;
        }

        /// <summary>
        /// Updates the file registration transaction status asynchronous.
        /// </summary>
        /// <param name="fileRegistrationTransactionId">The file registration transaction identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns>The task.</returns>
        private async Task UpdateFileRegistrationTransactionStatusAsync(int fileRegistrationTransactionId, StatusType status)
        {
            var fileRegistrationTransactionRepository = this.unitOfWork.CreateRepository<FileRegistrationTransaction>();

            var fileRegistrationTransaction = await fileRegistrationTransactionRepository.GetByIdAsync(fileRegistrationTransactionId).ConfigureAwait(false);

            fileRegistrationTransaction.StatusTypeId = status;
            fileRegistrationTransactionRepository.Update(fileRegistrationTransaction);
            await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Get transformation for inventories.
        /// </summary>
        /// <param name="homologatedToken">The homologateToken.</param>
        /// <returns>The homologated Token.</returns>
        private async Task TransformInventoryAsync(JToken homologatedToken)
        {
            int measurementUnit = (int)homologatedToken["MeasurementUnit"];
            int nodeId = (int)homologatedToken["NodeId"];
            string productId = (string)homologatedToken["ProductId"];

            ArgumentValidators.ThrowIfNull(measurementUnit, nameof(measurementUnit));
            ArgumentValidators.ThrowIfNull(nodeId, nameof(nodeId));
            ArgumentValidators.ThrowIfNull(productId, nameof(productId));

            var transformationRepository = this.unitOfWork.CreateRepository<Transformation>();

            var result = await transformationRepository.GetAllAsync(
                t => t.OriginSourceNodeId == nodeId && t.OriginSourceProductId == productId && t.OriginMeasurementId == measurementUnit &&
                !t.IsDeleted.Value && !t.IsMovement).ConfigureAwait(false);

            if (result.Any())
            {
                var transform = result.FirstOrDefault();
                homologatedToken["MeasurementUnit"] = transform.DestinationMeasurementId;
                homologatedToken["NodeId"] = transform.DestinationSourceNodeId;
                homologatedToken["ProductId"] = transform.DestinationSourceProductId;
            }
        }

        /// <summary>
        /// Get transformation for movements.
        /// </summary>
        /// <param name="homologatedToken">The homologateToken.</param>
        /// <returns>The homologated Token.</returns>
        private async Task TransformMovementAsync(JToken homologatedToken)
        {
            int measurementUnit = (int)homologatedToken["MeasurementUnit"];
            int nodeId = (int)homologatedToken["MovementSource"]["SourceNodeId"];
            string sourceProductId = (string)homologatedToken["MovementSource"]["SourceProductId"];
            int? destinationNodeId = (int)homologatedToken["MovementDestination"]["DestinationNodeId"];
            string destinationProductId = (string)homologatedToken["MovementDestination"]["DestinationProductId"];

            ArgumentValidators.ThrowIfNull(measurementUnit, nameof(measurementUnit));
            ArgumentValidators.ThrowIfNull(nodeId, nameof(nodeId));
            ArgumentValidators.ThrowIfNull(sourceProductId, nameof(sourceProductId));
            ArgumentValidators.ThrowIfNull(destinationNodeId, nameof(destinationNodeId));
            ArgumentValidators.ThrowIfNull(destinationProductId, nameof(destinationProductId));

            var transformationRepository = this.unitOfWork.CreateRepository<Transformation>();

            var result = await transformationRepository.GetAllAsync(
                t => t.OriginSourceNodeId == nodeId && t.OriginSourceProductId == sourceProductId && t.OriginMeasurementId == measurementUnit &&
                t.OriginDestinationNodeId == destinationNodeId && t.OriginDestinationProductId == destinationProductId &&
                !t.IsDeleted.Value && t.IsMovement).ConfigureAwait(false);

            if (result.Any())
            {
                var transform = result.FirstOrDefault();

                homologatedToken["MeasurementUnit"] = transform.DestinationMeasurementId;
                homologatedToken["MovementSource"]["SourceNodeId"] = transform.DestinationSourceNodeId;
                homologatedToken["MovementSource"]["SourceProductId"] = transform.DestinationSourceProductId;
                homologatedToken["MovementDestination"]["DestinationNodeId"] = transform.DestinationDestinationNodeId;
                homologatedToken["MovementDestination"]["DestinationProductId"] = transform.DestinationDestinationProductId;
            }
        }
    }
}