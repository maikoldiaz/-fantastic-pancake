// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResultValidationExecutor.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Services;

    /// <summary>
    /// The ValidationExecutor class.
    /// </summary>
    public class ResultValidationExecutor : ExecutorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultValidationExecutor"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="ownershipValidator">The ownershipValidator.</param>
        public ResultValidationExecutor(ITrueLogger<ResultValidationExecutor> logger)
            : base(logger)
        {
        }

        /// <inheritdoc/>
        public override int Order => 6;

        /// <summary>
        /// Gets the type of the process.
        /// </summary>
        /// <value>
        /// The type of the process.
        /// </value>
        public override ProcessType ProcessType => ProcessType.Ownership;

        /// <inheritdoc/>
        public override async Task ExecuteAsync(object input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));

            var ownershipRuleData = (OwnershipRuleData)input;
            var inventoryResultList = ownershipRuleData.OwnershipRuleResponse.InventoryResults;
            var movementResultList = ownershipRuleData.OwnershipRuleResponse.MovementResults;

            if (inventoryResultList.Any() || movementResultList.Any())
            {
                ownershipRuleData.Errors = OwnershipValidator.ValidateOwnershipRuleResult(ownershipRuleData);
            }
            else
            {
                ownershipRuleData.Errors = new List<ErrorInfo> { new ErrorInfo(Constants.ValidationNoInventoryAndMovementResultFailureMessage) };
            }

            this.ShouldContinue = !ownershipRuleData.Errors.Any();

            await this.ExecuteNextAsync(ownershipRuleData).ConfigureAwait(false);
        }
    }
}