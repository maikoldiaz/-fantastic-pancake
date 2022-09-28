// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistrationStrategyBase.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// The registration strategy base.
    /// </summary>
    public abstract class RegistrationStrategyBase : IRegistrationStrategy
    {
        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationStrategyBase" /> class.
        /// </summary>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="logger">The logger.</param>
        protected RegistrationStrategyBase(
            IAzureClientFactory azureClientFactory,
            ITrueLogger logger)
        {
            this.azureClientFactory = azureClientFactory;
            this.Logger = logger;
        }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public ITrueLogger Logger { get; set; }

        /// <summary>
        /// Registers asynchronously.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="unitOfWork">The unitOfWork.</param>
        public abstract void Insert(IEnumerable<object> entities, IUnitOfWork unitOfWork);

        /// <summary>
        /// Registers asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="unitOfWork">The unitOfWork.</param>
        /// <returns>The bool.</returns>
        public abstract Task RegisterAsync(object entity, IUnitOfWork unitOfWork);

        /// <summary>
        /// Sends to blockchain asynchronous.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="queueName">The queue name.</param>
        /// <returns>The task.</returns>
        protected async Task SendToBlockchainAsync(object data, string queueName)
        {
            ArgumentValidators.ThrowIfNull(data, nameof(data));

            try
            {
                var queueClient = this.azureClientFactory.GetQueueClient(queueName);
                await queueClient.QueueSessionMessageAsync(data, data.ToString()).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Failed to send the message to Blockchain", data.ToString());
            }
        }
    }
}
