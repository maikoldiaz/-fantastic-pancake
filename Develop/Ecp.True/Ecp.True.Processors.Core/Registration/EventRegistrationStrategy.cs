// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventRegistrationStrategy.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Registration
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure;
    using Newtonsoft.Json;

    /// <summary>
    /// The EventRegistrationStrategy.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.Registration.RegistrationStrategyBase" />
    public class EventRegistrationStrategy : RegistrationStrategyBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventRegistrationStrategy" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        public EventRegistrationStrategy(
                  ITrueLogger logger,
                  IAzureClientFactory azureClientFactory)
                  : base(azureClientFactory, logger)
        {
        }

        /// <summary>
        /// Registers asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="unitOfWork">The unitOfWork.</param>
        /// <returns>
        /// The bool.
        /// </returns>
        public override async Task RegisterAsync(object entity, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));

            var eventEntity = (Event)entity;

            await RegistrationExecutorAsync(eventEntity, unitOfWork).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public override void Insert(IEnumerable<object> entities, IUnitOfWork unitOfWork)
        {
            this.Logger.LogInformation(JsonConvert.SerializeObject(entities));
        }

        /// <summary>
        /// Does the get event asynchronous.
        /// </summary>
        /// <param name="eventObject">The event object.</param>
        /// <param name="repository">The repository.</param>
        /// <returns>The task.</returns>
        private static Task<Event> DoGetEventAsync(Event eventObject, IRepository<Event> repository)
        {
            return repository.SingleOrDefaultAsync(
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
        /// Register the create Event asynchronous.
        /// </summary>
        /// <param name="eventObject">The Event.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        private static void RegisterCreateEvent(Event eventObject, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(eventObject, nameof(eventObject));

            var repository = unitOfWork.CreateRepository<Event>();
            eventObject.IsDeleted = false;
            repository.Insert(eventObject);
        }

        /// <summary>
        /// Register the update Event asynchronous.
        /// </summary>
        /// <param name="eventObject">The Event.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>
        /// The task.
        /// </returns>
        private static async Task RegisterUpdateEventAsync(Event eventObject, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(eventObject, nameof(eventObject));

            var repository = unitOfWork.CreateRepository<Event>();
            var existing = await DoGetEventAsync(eventObject, repository).ConfigureAwait(false);
            existing.CopyFrom(eventObject);
            repository.Update(existing);
        }

        /// <summary>
        /// Registers the delete event asynchronous.
        /// </summary>
        /// <param name="eventObject">The event object.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>
        /// The task.
        /// </returns>
        private static async Task RegisterDeleteEventAsync(Event eventObject, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(eventObject, nameof(eventObject));

            var repository = unitOfWork.CreateRepository<Event>();
            var eventObj = await DoGetEventAsync(eventObject, repository).ConfigureAwait(false);
            eventObj.IsDeleted = true;

            repository.Update(eventObj);
        }

        /// <summary>
        /// Registrations the executor asynchronous.
        /// </summary>
        /// <param name="eventObject">The event object.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        private static async Task RegistrationExecutorAsync(Event eventObject, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(eventObject, nameof(eventObject));

            if (eventObject.ActionType.EqualsIgnoreCase(EventType.Insert.ToString("G")))
            {
                RegisterCreateEvent(eventObject, unitOfWork);
            }

            if (eventObject.ActionType.EqualsIgnoreCase(EventType.Update.ToString("G")))
            {
                await RegisterUpdateEventAsync(eventObject, unitOfWork).ConfigureAwait(false);
            }

            if (eventObject.ActionType.EqualsIgnoreCase(EventType.Delete.ToString("G")))
            {
                await RegisterDeleteEventAsync(eventObject, unitOfWork).ConfigureAwait(false);
            }

            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }
    }
}