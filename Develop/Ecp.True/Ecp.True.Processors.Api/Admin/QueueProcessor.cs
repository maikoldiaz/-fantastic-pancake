// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueueProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Admin
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Core;
    using Ecp.True.Proxies.Azure;
    using EntitiesConstants = Ecp.True.Entities.Constants;

    /// <summary>
    /// The Ticket Processor.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Api.ProcessorBase" />
    /// <seealso cref="Ecp.True.Processors.Api.Interfaces.IQueueProcessor" />
    public class QueueProcessor : ProcessorBase, IQueueProcessor
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<TicketProcessor> logger;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueProcessor"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="azureClientFactory">The azure client.</param>
        public QueueProcessor(
            ITrueLogger<TicketProcessor> logger,
            IAzureClientFactory azureClientFactory)
        {
            this.logger = logger;
            this.azureClientFactory = azureClientFactory;
        }

        /// <inheritdoc/>
        public async Task PushQueueSessionMessageAsync(int ticketId, string queueName)
        {
            try
            {
                string sessionId = Guid.NewGuid().ToString();
                IServiceBusQueueClient client;
                client = this.azureClientFactory.GetQueueClient(
                queueName);
                await client.QueueSessionMessageAsync(ticketId, sessionId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var error = ex.InnerException ?? ex;
                this.logger.LogError(error, Constants.TechnicalExceptionErrorMessage, ticketId);
            }
        }

        /// <inheritdoc/>
        public async Task PushQueueMessageAsync(int ticketId, string queueName)
        {
            try
            {
                IServiceBusQueueClient client;
                client = this.azureClientFactory.GetQueueClient(
                queueName);
                await client.QueueMessageAsync(ticketId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var error = ex.InnerException ?? ex;
                this.logger.LogError(error, Constants.TechnicalExceptionErrorMessage, ticketId);
            }
        }
    }
}
