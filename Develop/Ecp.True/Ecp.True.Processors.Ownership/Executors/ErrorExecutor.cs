// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorExecutor.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Ownership.Entities;

    /// <summary>
    /// The ErrorExecutor class.
    /// </summary>
    public class ErrorExecutor : ExecutorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorExecutor"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ErrorExecutor(ITrueLogger<ErrorExecutor> logger)
            : base(logger)
        {
        }

        /// <inheritdoc/>
        public override int Order => 5;

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
            var inventoryErrorList = ownershipRuleData.OwnershipRuleResponse.InventoryErrors;
            var movementErrorList = ownershipRuleData.OwnershipRuleResponse.MovementErrors;

            ownershipRuleData.HasProcessingErrors = inventoryErrorList.Any() || movementErrorList.Any();

            this.ShouldContinue = !ownershipRuleData.HasProcessingErrors;
            await this.ExecuteNextAsync(ownershipRuleData).ConfigureAwait(false);
        }
    }
}
