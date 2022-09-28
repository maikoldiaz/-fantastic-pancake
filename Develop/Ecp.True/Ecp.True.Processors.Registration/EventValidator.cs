// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventValidator.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
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
    /// The Event validator.
    /// </summary>
    /// <seealso cref="IEventValidatorService" />
    public class EventValidator : BaseValidator, IEventValidator
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<EventValidator> logger;

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
        private readonly string validationMessage = "Event validation started";

        /// <summary>
        /// The validated.
        /// </summary>
        private readonly string validated = "The homologated event is validated";

        /// <summary>
        /// The validated.
        /// </summary>
        private readonly string validatedEvent = "The event entity is validated";

        /// <summary>
        /// The failed.
        /// </summary>
        private readonly string failed = "The homologated event is failed";

        /// <summary>
        /// Initializes a new instance of the <see cref="EventValidator" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="blobOperations">The blobOperations.</param>
        /// <param name="compositeValidatorFactory">The composite validator factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="fileRegistrationTransactionService">The file registration transaction service.</param>
        public EventValidator(
            ITrueLogger<EventValidator> logger,
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
        /// Validate the event asynchronous.
        /// </summary>
        /// <param name="fileRegistrationTransaction">The file registration transaction.</param>
        /// <param name="homologatedJson">The homologated json.</param>
        /// <returns>
        /// The [True] if validation passes, [False] otherwise.
        /// </returns>
        public async Task<(bool isValid, Event eventObject)> ValidateEventAsync(FileRegistrationTransaction fileRegistrationTransaction, JToken homologatedJson)
        {
            ArgumentValidators.ThrowIfNull(fileRegistrationTransaction, nameof(fileRegistrationTransaction));
            this.logger.LogInformation(this.validationMessage, fileRegistrationTransaction.UploadId);

            var eventObject = this.blobOperations.GetHomologatedObject<Event>(homologatedJson, fileRegistrationTransaction.UploadId);

            if (eventObject.Item1 != null)
            {
                eventObject.Item1.ActionType = Convert.ToString(fileRegistrationTransaction.ActionType.Value, CultureInfo.InvariantCulture);
                var isValid = await this.DoValidateEventAsync(fileRegistrationTransaction, eventObject.Item1).ConfigureAwait(false);
                return (isValid, eventObject.Item1);
            }
            else
            {
                var pendingTransaction = homologatedJson.ToPendingTransaction(fileRegistrationTransaction, eventObject.Item2);
                await this.fileRegistrationTransactionService.RegisterFailureAsync(
                                    pendingTransaction,
                                    fileRegistrationTransaction.FileRegistrationTransactionId,
                                    eventObject.Item3,
                                    Constants.InvalidDataType,
                                    fileRegistrationTransaction.UploadId).ConfigureAwait(false);
                return (false, null);
            }
        }

        /// <summary>
        /// Does the validate event asynchronous.
        /// </summary>
        /// <param name="fileRegistrationTransaction">The file registration transaction.</param>
        /// <param name="eventObject">The event object.</param>
        /// <returns>The boolean.</returns>
        private async Task<bool> DoValidateEventAsync(FileRegistrationTransaction fileRegistrationTransaction, Event eventObject)
        {
            this.logger.LogInformation(this.validated, fileRegistrationTransaction.UploadId);
            var result = await this.compositeValidatorFactory.EventCompositeValidator.ValidateAsync(eventObject).ConfigureAwait(false);

            if (!result.IsSuccess)
            {
                await this.DoRegisterFailureAsync(
                eventObject, fileRegistrationTransaction, result.ErrorInfo.Select(r => r.Message), this.failed, fileRegistrationTransaction.UploadId).ConfigureAwait(false);
                return false;
            }

            this.logger.LogInformation(this.validatedEvent, fileRegistrationTransaction.UploadId);
            return await this.ValidateEventAsync(eventObject, fileRegistrationTransaction).ConfigureAwait(false);
        }

        /// <summary>
        /// Validates the event date asynchronous.
        /// </summary>
        /// <param name="eventObject">The event object.</param>
        /// <returns>The task.</returns>
        private async Task<bool> ValidateEventDateAsync(Event eventObject)
        {
            var eventRepository = this.unitOfWork.CreateRepository<Event>();
            var eventObjects = await eventRepository.GetAllAsync(
            x => x.EventTypeId == eventObject.EventTypeId &&
            x.SourceNodeId == eventObject.SourceNodeId &&
            x.DestinationNodeId == eventObject.DestinationNodeId &&
            x.SourceProductId == eventObject.SourceProductId &&
            x.DestinationProductId == eventObject.DestinationProductId &&
            x.IsDeleted == false).ConfigureAwait(false);

            return !eventObjects.Any(x => x.StartDate <= eventObject.EndDate && eventObject.StartDate <= x.EndDate);
        }

        /// <summary>
        /// Does the get event asynchronous.
        /// </summary>
        /// <param name="eventObject">The event object.</param>
        /// <returns>The task.</returns>
        private Task<Event> DoGetEventAsync(Event eventObject)
        {
            var eventRepository = this.unitOfWork.CreateRepository<Event>();
            return eventRepository.SingleOrDefaultAsync(
                            x =>
                            x.SourceNodeId == eventObject.SourceNodeId &&
                            x.DestinationNodeId == eventObject.DestinationNodeId &&
                            x.SourceProductId == eventObject.SourceProductId &&
                            x.DestinationProductId == eventObject.DestinationProductId &&
                            x.StartDate == eventObject.StartDate &&
                            x.EndDate == eventObject.EndDate &&
                            x.IsDeleted == false);
        }

        /// <summary>
        /// Validates the event asynchronous.
        /// </summary>
        /// <param name="eventObject">The event object.</param>
        /// <param name="fileRegistrationTransaction">The file registration transaction.</param>
        /// <returns>The boolean.</returns>
        private async Task<bool> ValidateEventAsync(Event eventObject, FileRegistrationTransaction fileRegistrationTransaction)
        {
            ArgumentValidators.ThrowIfNull(eventObject, nameof(eventObject));
            string actionType = Convert.ToString(fileRegistrationTransaction.ActionType.Value, CultureInfo.InvariantCulture);

            if (actionType.EqualsIgnoreCase(EventType.Insert.ToString("G")))
            {
                return await this.ValidateCreateEventAsync(eventObject, fileRegistrationTransaction).ConfigureAwait(false);
            }

            if (actionType.EqualsIgnoreCase(EventType.Update.ToString("G")) || actionType.EqualsIgnoreCase(EventType.Delete.ToString("G")))
            {
                return await this.ValidateUpdateDeleteEventAsync(eventObject, fileRegistrationTransaction).ConfigureAwait(false);
            }

            throw new InvalidOperationException($"Unknown event type {actionType}");
        }

        /// <summary>
        /// Validates the create event asynchronous.
        /// </summary>
        /// <param name="eventObject">The event object.</param>
        /// <param name="fileRegistrationTransaction">The file registration transaction.</param>
        /// <returns>The boolean.</returns>
        private async Task<bool> ValidateCreateEventAsync(Event eventObject, FileRegistrationTransaction fileRegistrationTransaction)
        {
            ArgumentValidators.ThrowIfNull(eventObject, nameof(eventObject));

            var existing = await this.DoGetEventAsync(eventObject).ConfigureAwait(false);
            if (existing != null)
            {
                await this.DoRegisterFailureAsync(
                    eventObject,
                    fileRegistrationTransaction,
                    new[] { Constants.EventCreateConflict },
                    Constants.EventCreateConflict,
                    fileRegistrationTransaction.UploadId).ConfigureAwait(false);
                return false;
            }

            var isEventValid = await this.ValidateEventDateAsync(eventObject).ConfigureAwait(false);
            if (!isEventValid)
            {
                await this.DoRegisterFailureAsync(
                    eventObject,
                    fileRegistrationTransaction,
                    new[] { Constants.EventPeriodAlreadyExists },
                    Constants.EventPeriodAlreadyExists,
                    fileRegistrationTransaction.UploadId).ConfigureAwait(false);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the update delete event asynchronous.
        /// </summary>
        /// <param name="eventObject">The event object.</param>
        /// <param name="fileRegistrationTransaction">The file registration transaction.</param>
        /// <returns>The boolean.</returns>
        private async Task<bool> ValidateUpdateDeleteEventAsync(Event eventObject, FileRegistrationTransaction fileRegistrationTransaction)
        {
            var existing = await this.DoGetEventAsync(eventObject).ConfigureAwait(false);
            if (existing != null)
            {
                return true;
            }

            await this.DoRegisterFailureAsync(
                eventObject,
                fileRegistrationTransaction,
                new[] { Constants.EventNotFound },
                Constants.EventNotFound,
                fileRegistrationTransaction.UploadId).ConfigureAwait(false);
            return false;
        }

        /// <summary>
        /// Does the register failure.
        /// </summary>
        /// <param name="eventObject">The event.</param>
        /// <param name="fileRegistrationTransaction">The file registration  session message.</param>
        private async Task DoRegisterFailureAsync(Event eventObject, FileRegistrationTransaction fileRegistrationTransaction, IEnumerable<string> errorInfos, string errorMessage, params object[] args)
        {
            var pendingTransaction = eventObject.ToPendingTransaction(fileRegistrationTransaction, errorInfos);
            await this.fileRegistrationTransactionService.RegisterFailureAsync(
                pendingTransaction,
                fileRegistrationTransaction.FileRegistrationTransactionId,
                null,
                errorMessage,
                args).ConfigureAwait(false);
        }
    }
}