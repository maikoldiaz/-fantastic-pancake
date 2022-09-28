// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaFailureHandler.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Logging;

    /// <summary>
    /// The DeltaFailureHandler.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.HandleFailure.FailureHandlerBase" />
    public class DeltaFailureHandler : FailureHandlerBase
    {
        /// <summary>
        /// The telemetry.
        /// </summary>
        private readonly ITelemetry telemetry;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaFailureHandler"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="telemetry">The telemetry.</param>
        public DeltaFailureHandler(
                 ITrueLogger<DeltaFailureHandler> logger,
                 ITelemetry telemetry)
                 : base(logger)
        {
            this.telemetry = telemetry;
        }

        /// <inheritdoc/>
        public override TicketType TicketType => TicketType.Delta;

        /// <inheritdoc/>
        public override async Task HandleFailureAsync(IUnitOfWork unitOfWork, FailureInfo failureInfo)
        {
            await base.HandleFailureAsync(unitOfWork, failureInfo).ConfigureAwait(false);
            this.telemetry.TrackEvent(Constants.Critical, EventName.OperativeDeltaFailureEvent.ToString("G"));
        }
    }
}
