// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstantExpressionEvaluator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Approval
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Approval.Interfaces;
    using Ecp.True.Processors.Core;
    using Ecp.True.Proxies.Azure;

    /// <summary>
    /// The Expression Evaluator Processor.
    /// </summary>
    public class ConstantExpressionEvaluator : ProcessorBase, IExpressionEvaluator
    {
        /// <summary>
        /// The repository factory.
        /// </summary>
        private readonly IRepositoryFactory repositoryFactory;

        /// <summary>
        /// The true Expression Evaluator.
        /// </summary>
        private readonly ITrueExpressionEvaluator trueExpressionEvaluator;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        /// <summary>
        /// The approval processor.
        /// </summary>
        private readonly IApprovalProcessor approvalProcessor;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<ConstantExpressionEvaluator> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantExpressionEvaluator" /> class.
        /// </summary>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <param name="trueExpressionEvaluator">The true expression evaluator.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="approvalProcessor">The approval processor.</param>
        /// <param name="logger">The logger.</param>
        public ConstantExpressionEvaluator(
            IRepositoryFactory repositoryFactory,
            ITrueExpressionEvaluator trueExpressionEvaluator,
            IUnitOfWorkFactory unitOfWorkFactory,
            IAzureClientFactory azureClientFactory,
            IApprovalProcessor approvalProcessor,
            ITrueLogger<ConstantExpressionEvaluator> logger)
            : base(repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
            this.trueExpressionEvaluator = trueExpressionEvaluator;
            this.azureClientFactory = azureClientFactory;
            this.logger = logger;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.approvalProcessor = approvalProcessor;
        }

        /// <summary>
        /// Evaluate the expression.
        /// </summary>
        /// <param name="ownershipNodeId">The ownership node identifier.</param>
        /// <returns>The ownership nodes summary aggregate data.</returns>
        public async Task EvaluateAsync(int ownershipNodeId)
        {
            var aggregates = await this.GetAggregatesAsync(ownershipNodeId).ConfigureAwait(false);
            this.trueExpressionEvaluator.InitializeVariables(aggregates.FirstOrDefault());

            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var repository = unitOfWork.CreateRepository<ApprovalRule>();
            var rules = await repository.GetAllAsync(null).ConfigureAwait(false);

            if (this.DoEvaluate(rules?.Take(10)))
            {
                await this.ApproveOwnershipAsync(ownershipNodeId).ConfigureAwait(false);
                return;
            }

            await this.SendMessageToServiceBusQueueAsync(ownershipNodeId).ConfigureAwait(false);
        }

        private Task<IEnumerable<BalanceSummaryAggregate>> GetAggregatesAsync(int ownershipNodeId)
        {
            var parameters = new Dictionary<string, object>
            {
               { "@OwnershipNodeId", ownershipNodeId },
            };

            var repo = this.repositoryFactory.CreateRepository<BalanceSummaryAggregate>();
            return repo.ExecuteQueryAsync(Repositories.Constants.BalanceSummaryAggregate, parameters);
        }

        private bool DoEvaluate(IEnumerable<ApprovalRule> rules)
        {
            ArgumentValidators.ThrowIfNull(rules, nameof(rules));

            foreach (var rule in rules)
            {
                try
                {
                    if (this.trueExpressionEvaluator.TryEvaluate(rule.Rule))
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, ex.Message);
                }
            }

            return false;
        }

        private async Task ApproveOwnershipAsync(int ownershipNodeId)
        {
            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var repository = unitOfWork.CreateRepository<OwnershipNode>();

            var ownershipNode = await repository.SingleOrDefaultAsync(a => a.OwnershipNodeId == ownershipNodeId).ConfigureAwait(false);
            ownershipNode.OwnershipStatus = OwnershipNodeStatusType.APPROVED;
            ownershipNode.Comment = Constants.NodeAutomaticallyApproved;

            repository.Update(ownershipNode);
            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            await this.approvalProcessor.SaveOperativeMovementsAsync(ownershipNodeId).ConfigureAwait(false);
        }

        private Task SendMessageToServiceBusQueueAsync(object data)
        {
            var flowApprovalQueueClient = this.azureClientFactory.GetQueueClient(QueueConstants.FlowApprovalQueue);
            return flowApprovalQueueClient.QueueMessageAsync(data);
        }
    }
}