// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FailureHandlerBase.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.HandleFailure
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// The Transform Processor.
    /// </summary>
    /// <seealso cref="IFailureHandler" />
    public abstract class FailureHandlerBase : IFailureHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FailureHandlerBase" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        protected FailureHandlerBase(
            ITrueLogger logger)
        {
            this.Logger = logger;
        }

        /// <inheritdoc/>
        public abstract TicketType TicketType { get; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        protected ITrueLogger Logger { get; }

        /// <inheritdoc/>
        public virtual async Task HandleFailureAsync(IUnitOfWork unitOfWork, FailureInfo failureInfo)
        {
            ArgumentValidators.ThrowIfNull(failureInfo, nameof(failureInfo));
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            this.Logger.LogInformation($"Handling failure for ticket:  {failureInfo.TicketId}", $"{failureInfo.TicketId}");
            var ticketRepository = unitOfWork.CreateRepository<Ticket>();
            var ticket = await ticketRepository.GetByIdAsync(failureInfo.TicketId).ConfigureAwait(false);
            ticket.Status = StatusType.FAILED;
            ticket.ErrorMessage = failureInfo.ErrorMessage;
            ticketRepository.Update(ticket);
            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }
    }
}