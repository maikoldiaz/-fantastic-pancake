// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnbalanceReconciler.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Core.Dto;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Azure.Interfaces;

    /// <summary>
    /// The movement reconciler.
    /// </summary>
    public class UnbalanceReconciler : ReconcilerBase<Unbalance>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnbalanceReconciler" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        public UnbalanceReconciler(IUnitOfWorkFactory factory, ITelemetry telemetry, IAzureClientFactory azureClientFactory, IConfigurationHandler configurationHandler)
            : base(factory, telemetry, azureClientFactory, configurationHandler, x => x.Unbalances, y => y.UnbalanceId, z => z.NodeId.ToString(CultureInfo.InvariantCulture))
        {
        }

        /// <inheritdoc />
        public override ServiceType Type => ServiceType.Unbalance;

        /// <inheritdoc/>
        protected override EventName EventName => EventName.UnbalanceReconcileFailed;

        /// <inheritdoc/>
        protected override string QueueName => QueueConstants.BlockchainNodeProductCalculationQueue;

        /// <inheritdoc/>
        protected async override Task<IEvent> GetEventAsync(Unbalance entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var id = $"{entity.CalculationDate.Date.ToString("d", CultureInfo.InvariantCulture)}-{entity.NodeId}-{entity.ProductId}";
            var events = await this.AzureClientFactory.EthereumClient.GetEventsAsync<Blockchain.Events.UnbalanceEvent>(id).ConfigureAwait(false);
            return events.SingleOrDefault(x => x.TicketId == entity.TicketId);
        }

        /// <inheritdoc/>
        protected override OffchainMessage GetOffchainMessage(Unbalance entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            var offchainMessage = new OffchainMessage();
            offchainMessage.EntityId = entity.UnbalanceId;

            return offchainMessage;
        }
    }
}
