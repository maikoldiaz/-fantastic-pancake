// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecutionManager.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Executors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Services.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.Response;

    /// <summary>
    /// The execution manager.
    /// </summary>
    public class ExecutionManager : IExecutionManager
    {
        /// <summary>
        /// The ownership service.
        /// </summary>
        private readonly IOwnershipService ownershipService;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<ExecutionManager> logger;

        /// <summary>
        /// The executor.
        /// </summary>
        private IExecutor executor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionManager"/> class.
        /// </summary>
        /// <param name="ownershipService">The ownership service.</param>
        /// <param name="logger">The logger.</param>
        public ExecutionManager(IOwnershipService ownershipService, ITrueLogger<ExecutionManager> logger)
        {
            this.ownershipService = ownershipService;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public TicketType Type => TicketType.Ownership;

        /// <inheritdoc/>
        public void Initialize(IExecutor executor)
        {
            this.executor = executor;
        }

        /// <inheritdoc/>
        public async Task<object> ExecuteChainAsync(object input)
        {
            var ownershipRuleData = (OwnershipRuleData)input;
            ArgumentValidators.ThrowIfNull(ownershipRuleData, nameof(ownershipRuleData));

            this.logger.LogInformation($"Ownership chain execution processing started at {DateTime.UtcNow.ToTrue()}", $"{ownershipRuleData.TicketId}");
            this.logger.LogInformation($"Executor {this.executor.GetType().Name} with order {this.executor.Order} started processing.", $"{ownershipRuleData.TicketId}");
            await this.executor.ExecuteAsync(input).ConfigureAwait(false);
            var errors = ownershipRuleData.Errors;

            if (errors.Any() || ownershipRuleData.HasProcessingErrors)
            {
                this.logger.LogInformation(
                    $"Handling ownership chain execution processing errors for ticket {ownershipRuleData.TicketId} has {errors.ToList().Count} processing errors .", $"{ownershipRuleData.TicketId}");
                var movementErrors = ownershipRuleData.OwnershipRuleResponse != null ? ownershipRuleData.OwnershipRuleResponse.MovementErrors : new List<OwnershipErrorMovement>();
                var inventoryErrors = ownershipRuleData.OwnershipRuleResponse != null ? ownershipRuleData.OwnershipRuleResponse.InventoryErrors : new List<OwnershipErrorInventory>();
                await this.ownershipService.HandleFailureAsync(
                    ownershipRuleData.TicketId,
                    errors,
                    movementErrors,
                    inventoryErrors,
                    ownershipRuleData.HasProcessingErrors).ConfigureAwait(false);
            }

            this.logger.LogInformation($"Ownership chain execution processing finished at {DateTime.UtcNow.ToTrue()}", $"{ownershipRuleData.TicketId}");
            return input;
        }
    }
}
