// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorValidationExecutor.cs" company="Microsoft">
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
    using Ecp.True.Processors.Ownership.Services.Interfaces;

    /// <summary>
    /// The ValidationExecutor class.
    /// </summary>
    public class ErrorValidationExecutor : ExecutorBase
    {
        /// <summary>
        /// The ownership validator.
        /// </summary>
        private readonly IOwnershipValidator ownershipValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorValidationExecutor"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="ownershipValidator">The ownershipValidator.</param>
        public ErrorValidationExecutor(ITrueLogger<ErrorValidationExecutor> logger, IOwnershipValidator ownershipValidator)
            : base(logger)
        {
            this.ownershipValidator = ownershipValidator;
        }

        /// <inheritdoc/>
        public override int Order => 4;

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

            ownershipRuleData.Errors = await this.ownershipValidator.ValidateOwnershipRuleErrorAsync(inventoryErrorList, movementErrorList).ConfigureAwait(false);
            this.ShouldContinue = !ownershipRuleData.Errors.Any();

            await this.ExecuteNextAsync(ownershipRuleData).ConfigureAwait(false);
        }
    }
}