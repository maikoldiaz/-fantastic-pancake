// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildResultExecutor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.OfficialDeltaExecutors
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.Interfaces;

    /// <summary>
    /// Build Result Executor.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.Execution.ExecutorBase" />
    public class BuildResultExecutor : ExecutorBase
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly ICompositeOfficialDeltaBuilder compositeOfficialDeltaBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildResultExecutor"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="compositeOfficialDeltaBuilder">The composite delta builder.</param>
        public BuildResultExecutor(ITrueLogger<BuildResultExecutor> logger, ICompositeOfficialDeltaBuilder compositeOfficialDeltaBuilder)
                   : base(logger)
        {
            this.compositeOfficialDeltaBuilder = compositeOfficialDeltaBuilder;
        }

        /// <summary>
        /// Gets the Order.
        /// </summary>
        public override int Order => 4;

        /// <summary>
        /// Gets the type of the process.
        /// </summary>
        /// <value>
        /// The type of the process.
        /// </value>
        public override ProcessType ProcessType => ProcessType.OfficialDelta;

        /// <summary>
        /// The ExecuteAsync.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The Task.</returns>
        public override async Task ExecuteAsync(object input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));

            var deltaData = (OfficialDeltaData)input;
            this.Logger.LogInformation($"Started {nameof(BuildResultExecutor)} for ticket {deltaData.Ticket.TicketId}", deltaData.Ticket.TicketId);

            // add service call for transformation
            await this.BuildAsync(deltaData).ConfigureAwait(false);
            this.ShouldContinue = true;
            this.Logger.LogInformation($"Completed {nameof(BuildResultExecutor)} for ticket {deltaData.Ticket.TicketId}", deltaData.Ticket.TicketId);
            await this.ExecuteNextAsync(input).ConfigureAwait(false);
        }

        /// <summary>
        /// update movement and inventory data.
        /// </summary>
        /// <param name="deltaData">The deltaData.</param>
        /// <returns>delta data.</returns>
        private async Task<OfficialDeltaData> BuildAsync(OfficialDeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));
            var generatedMovements = new List<Movement>();
            var movementResult = await this.compositeOfficialDeltaBuilder.BuildMovementsAsync(deltaData).ConfigureAwait(false);

            generatedMovements.AddRange(movementResult);
            deltaData.GeneratedMovements = generatedMovements;

            return deltaData;
        }
    }
}
