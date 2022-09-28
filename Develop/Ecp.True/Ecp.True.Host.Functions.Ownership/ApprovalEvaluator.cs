// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApprovalEvaluator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Host.Functions.Approval
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Configuration.Entities;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Approval.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;

    /// <summary>
    /// The Expression Evaluator function.
    /// </summary>
    public class ApprovalEvaluator : FunctionBase
    {
        /// <summary>
        /// The failed approval.
        /// </summary>
        private readonly string failedApproval = "Approval failed for node";

        /// <summary>
        /// The balance processor.
        /// </summary>
        private readonly IExpressionEvaluator expressionEvaluator;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<ApprovalEvaluator> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApprovalEvaluator" /> class.
        /// </summary>
        /// <param name="expressionEvaluatorProcessor">The balance processor.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public ApprovalEvaluator(
            IExpressionEvaluator expressionEvaluatorProcessor,
            ITrueLogger<ApprovalEvaluator> logger,
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            this.expressionEvaluator = expressionEvaluatorProcessor;
            this.logger = logger;
        }

        /// <summary>
        /// Gets the message from the queue.
        /// </summary>
        /// <param name="ownershipNodeId">The queue message.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("ApprovalQueueTrigger")]
        public async Task ApprovalQueueTriggerAsync(
            [ServiceBusTrigger("%ApprovalQueue%", Connection = "IntegrationServiceBusConnectionString")]int ownershipNodeId,
            string label,
            string replyTo,
            ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(ownershipNodeId, nameof(ownershipNodeId));
            try
            {
                await this.TryInitializeAsync().ConfigureAwait(false);
                this.ProcessMetadata(label, FunctionNames.Approvals, replyTo);

                await this.expressionEvaluator.EvaluateAsync(ownershipNodeId).ConfigureAwait(false);
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, this.failedApproval);
            }
        }

        /// <inheritdoc/>
        protected override async Task DoInitializeAsync()
        {
            var azureClientFactory = this.Resolve<IAzureClientFactory>();
            var configurationHandler = this.Resolve<IConfigurationHandler>();

            if (azureClientFactory.IsReady)
            {
                return;
            }

            var storageSettings = await configurationHandler.GetConfigurationAsync<StorageSettings>(ConfigurationConstants.StorageSettings).ConfigureAwait(false);
            var serviceBusSettings = await configurationHandler.GetConfigurationAsync<ServiceBusSettings>(ConfigurationConstants.ServiceBusSettings).ConfigureAwait(false);

            var azureConfiguration = new AzureConfiguration(storageSettings, serviceBusSettings);
            azureClientFactory.Initialize(azureConfiguration);
        }
    }
}
