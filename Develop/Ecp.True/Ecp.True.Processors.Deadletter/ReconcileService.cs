// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReconcileService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Deadletter
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Core.Dto;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processors.Deadletter.Interfaces;

    /// <summary>
    /// The Reconciler.
    /// </summary>
    public class ReconcileService : IReconcileService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReconcileService"/> class.
        /// </summary>
        /// <param name="reconcilers">The reconcilers.</param>
        public ReconcileService(IEnumerable<IReconciler> reconcilers)
        {
            this.Children = reconcilers;
        }

        /// <inheritdoc />
        public ServiceType Type => ServiceType.All;

        /// <inheritdoc/>
        public IEnumerable<IReconciler> Children { get; private set; }

        /// <inheritdoc/>
        public async Task<IEnumerable<FailedRecord>> GetFailuresAsync(bool isCritical, int? takeRecords)
        {
            var records = new ConcurrentBag<FailedRecord>();
            foreach (var reconciler in this.Children)
            {
                await DoGetFailedAsync(reconciler, isCritical, takeRecords, records).ConfigureAwait(false);
            }

            return records;
        }

        /// <inheritdoc/>
        public async Task ReconcileAsync()
        {
            foreach (var reconciler in this.Children)
            {
                await reconciler.ReconcileAsync().ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public Task ReconcileAsync(OffchainMessage message)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            var reconciler = this.Children.SingleOrDefault(m => m.Type == message.Type);
            return reconciler != null ? reconciler.ReconcileAsync(message) : Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task ResetAsync(BlockchainFailures info)
        {
            foreach (var item in this.Children)
            {
                await item.ResetAsync(info).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task ReconcileFailureAsync()
        {
            var failureReconcilers = this.Children.Where(
                x => x.Type == ServiceType.Movement || x.Type == ServiceType.InventoryProduct || x.Type == ServiceType.Ownership || x.Type == ServiceType.Owner || x.Type == ServiceType.Unbalance);
            foreach (var reconciler in failureReconcilers)
            {
                await reconciler.ReconcileFailureAsync().ConfigureAwait(false);
            }
        }

        private static async Task DoGetFailedAsync(IReconciler reconciler, bool isCritical, int? takeRecords, ConcurrentBag<FailedRecord> output)
        {
            var result = await reconciler.GetFailuresAsync(isCritical, takeRecords).ConfigureAwait(false);
            result.ForEach(output.Add);
        }
    }
}