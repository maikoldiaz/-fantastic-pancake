// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaFinalizer.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Deltas
{
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Core;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// Delta finalizer.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.Interfaces.IFinalizer" />
    public class DeltaFinalizer : FinalizerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaFinalizer" /> class.
        /// </summary>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public DeltaFinalizer(
            IAzureClientFactory azureClientFactory,
            IUnitOfWorkFactory unitOfWorkFactory)
            : base(azureClientFactory, unitOfWorkFactory)
        {
        }

        /// <summary>
        /// Gets the type of the ticket.
        /// </summary>
        /// <value>
        /// The type of the ticket.
        /// </value>
        public override TicketType Type => TicketType.Delta;

        /// <summary>
        /// Gets the type of the finalizer.
        /// </summary>
        /// <value>
        /// The type of the finalizer.
        /// </value>
        public override FinalizerType Finalizer => FinalizerType.Delta;

        /// <summary>
        /// Processes the asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public override async Task ProcessAsync(int ticketId)
        {
            var movements = await this.UnitOfWork.CreateRepository<Movement>().GetAllAsync(x => x.DeltaTicketId == ticketId &&
            x.IsSystemGenerated == true && x.BlockchainStatus == StatusType.PROCESSING).ConfigureAwait(false);
            var movementTransactionIds = movements.Select(x => x.MovementTransactionId).Distinct();
            await this.SendSessionMessageToQueueAsync(movementTransactionIds, QueueConstants.BlockchainMovementQueue).ConfigureAwait(false);
        }
    }
}
