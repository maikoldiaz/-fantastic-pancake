// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipRuleSynchronizer.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Ownership
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Host.Functions.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Microsoft.Azure.WebJobs;

    /// <summary>
    /// The Ownership Rule Synchronizer.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Functions.Core.FunctionBase" />
    public class OwnershipRuleSynchronizer : FunctionBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<OwnershipRuleSynchronizer> logger;

        /// <summary>
        /// The ownership rule processor.
        /// </summary>
        private readonly IOwnershipRuleProcessor ownershipRuleProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipRuleSynchronizer" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="ownershipRuleProcessor">The ownership rule processor.</param>
        public OwnershipRuleSynchronizer(
            ITrueLogger<OwnershipRuleSynchronizer> logger,
            IServiceProvider serviceProvider,
            IOwnershipRuleProcessor ownershipRuleProcessor)
            : base(serviceProvider)
        {
            this.logger = logger;
            this.ownershipRuleProcessor = ownershipRuleProcessor;
        }

        /// <summary>
        /// Synchronizes the ownership rules asynchronous.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="label">The label.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        [FunctionName("SyncOwnershipRules")]
        public async Task SyncOwnershipRulesAsync(
           [ServiceBusTrigger("%OwnershipRulesQueue%", IsSessionsEnabled = true, Connection = "IntegrationServiceBusConnectionString")]string source,
           string label,
           string replyTo,
           ExecutionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNullOrEmpty(source, nameof(source));

            this.logger.LogInformation($"The ownership rules sync is invoked by {source}", Constants.OwnershipRulesSync);

            try
            {
                await this.TryInitializeAsync().ConfigureAwait(false);
                this.ProcessMetadata(label, FunctionNames.Ownership, replyTo);

                await this.ownershipRuleProcessor.SyncOwnershipRulesAsync(source).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message, Constants.OwnershipRulesSync);
            }

            this.logger.LogInformation($"The ownership rules sync is finished at {DateTime.UtcNow.ToTrue()}", Constants.OwnershipRulesSync);
        }
    }
}