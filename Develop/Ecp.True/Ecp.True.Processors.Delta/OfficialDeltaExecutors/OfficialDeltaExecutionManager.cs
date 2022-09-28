// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaExecutionManager.cs" company="Microsoft">
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
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;

    /// <summary>
    /// The ExecutionManager.
    /// </summary>
    /// <seealso cref="IExecutionManager" />
    public class OfficialDeltaExecutionManager : IExecutionManager
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<OfficialDeltaExecutionManager> logger;

        /// <summary>
        /// The executor.
        /// </summary>
        private IExecutor executor;

        /// <summary>
        /// Initializes a new instance of the <see cref="OfficialDeltaExecutionManager"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public OfficialDeltaExecutionManager(ITrueLogger<OfficialDeltaExecutionManager> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc/>
        public TicketType Type => TicketType.OfficialDelta;

        /// <summary>
        /// Executes the chain asynchronous.
        /// </summary>
        /// <param name="executor">The executor.</param>
        public void Initialize(IExecutor executor)
        {
            this.executor = executor;
        }

        /// <summary>
        /// Executes the chain asynchronous.
        /// </summary>
        /// <param name="input">The input object.</param>
        /// <returns>
        ///   The object.
        /// </returns>
        public async Task<object> ExecuteChainAsync(object input)
        {
            var deltaData = (OfficialDeltaData)input;
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));

            this.logger.LogInformation($"Official Delta chain execution processing started at {DateTime.UtcNow.ToTrue()}", $"{deltaData.Ticket.TicketId}");
            this.logger.LogInformation($"Executor {this.executor.GetType().Name} with order {this.executor.Order} started processing.", $"{deltaData.Ticket.TicketId}");

            await this.executor.ExecuteAsync(input).ConfigureAwait(false);

            this.logger.LogInformation($"Official Delta chain execution processing finished at {DateTime.UtcNow.ToTrue()}", $"{deltaData.Ticket.TicketId}");
            return deltaData;
        }
    }
}