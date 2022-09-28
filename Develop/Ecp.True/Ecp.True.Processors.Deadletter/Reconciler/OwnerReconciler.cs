// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnerReconciler.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Deadletter.Reconciler
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Core.Dto;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Azure.Interfaces;

    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The movement reconciler.
    /// </summary>
    public class OwnerReconciler : ReconcilerBase<Owner>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OwnerReconciler" /> class.
        /// </summary>
        /// <param name="factory">The unit of work factory.</param>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        public OwnerReconciler(IUnitOfWorkFactory factory, ITelemetry telemetry, IAzureClientFactory azureClientFactory, IConfigurationHandler configurationHandler)
            : base(factory, telemetry, azureClientFactory, configurationHandler, x => x.Owners, y => y.Id, z => z.OwnerId.ToString(CultureInfo.InvariantCulture))
        {
        }

        /// <inheritdoc />
        public override ServiceType Type => ServiceType.Owner;

        /// <inheritdoc/>
        protected override EventName EventName => EventName.OwnerReconcileFailed;

        /// <inheritdoc/>
        protected override string QueueName => QueueConstants.BlockchainOwnerQueue;

        /// <inheritdoc/>
        protected override async Task<IEnumerable<Owner>> GetFailedEntitiesAsync(ReconciliationSettings settings)
        {
            ArgumentValidators.ThrowIfNull(settings, nameof(settings));
            var ownerQuery = await this.UnitOfWork.CreateRepository<Owner>().QueryAllAsync(
                                x => x.BlockchainStatus == StatusType.FAILED && x.RetryCount <= (settings.DefaultRetries + 1),
                                "MovementTransaction",
                                "InventoryProduct").ConfigureAwait(false);

            return await ownerQuery.OrderBy(x => x.Id).Take(ReconciliationSettings.FailureBatch).ToListAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected async override Task<IEvent> GetEventAsync(Owner entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            IEnumerable<IEvent> events;
            if (entity.MovementTransaction != null)
            {
                var id = $"{entity.MovementTransaction.MovementId}-{entity.OwnerId}";
                events = await this.AzureClientFactory.EthereumClient.GetEventsAsync<Blockchain.Events.MovementOwnerEvent>(id).ConfigureAwait(false);
            }
            else
            {
                var id = $"{entity.InventoryProduct.InventoryProductUniqueId}-{entity.OwnerId}";
                events = await this.AzureClientFactory.EthereumClient.GetEventsAsync<Blockchain.Events.InventoryProductOwnerEvent>(id).ConfigureAwait(false);
            }

            return events.FirstOrDefault();
        }

        /// <inheritdoc/>
        protected override OffchainMessage GetOffchainMessage(Owner entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var offchainMessage = new OffchainMessage();
            offchainMessage.EntityId = entity.Id;

            return offchainMessage;
        }
    }
}
