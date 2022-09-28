// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CutOffFailureHandler.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Logging;

    /// <summary>
    /// CutOffFailureHandler.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.FailureHandler.FailureHandlerBase" />
    public class CutOffFailureHandler : FailureHandlerBase
    {
        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// Initializes a new instance of the <see cref="CutOffFailureHandler"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="telemetry">The telemetry.</param>
        public CutOffFailureHandler(
                  ITrueLogger<CutOffFailureHandler> logger,
                  ITelemetry telemetry)
                  : base(logger)
        {
            this.telemetry = telemetry;
        }

        /// <inheritdoc/>
        public override TicketType TicketType => TicketType.Cutoff;

        /// <inheritdoc/>
        public override async Task HandleFailureAsync(IUnitOfWork unitOfWork, FailureInfo failureInfo)
        {
            ArgumentValidators.ThrowIfNull(failureInfo, nameof(failureInfo));
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            this.Logger.LogInformation($"Fail and cleaning data for ticket:  {failureInfo.TicketId}", $"{failureInfo.TicketId}");
            this.telemetry.TrackEvent(Constants.Critical, EventName.CutoffFailureEvent.ToString("G"));
            var parameters = new Dictionary<string, object>
            {
                { "@TicketId", failureInfo.TicketId },
                { "@ErrorMessage", failureInfo.ErrorMessage },
            };

            var repository = unitOfWork.CreateRepository<Unbalance>();
            await repository.ExecuteAsync(Repositories.Constants.CutoffFailAndCleanupProcedureName, parameters).ConfigureAwait(false);
        }
    }
}
