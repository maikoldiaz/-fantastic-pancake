// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompleteExecutor.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Ownership.Entities;
    using Ecp.True.Processors.Ownership.Interfaces;

    /// <summary>
    /// The UpdateStatusExecutor class.
    /// </summary>
    public class CompleteExecutor : ExecutorBase
    {
        /// <summary>
        /// The ownership processor.
        /// </summary>
        private readonly IOwnershipProcessor ownershipProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompleteExecutor"/> class.
        /// </summary>
        /// <param name="ownershipProcessor">The ownership processor.</param>
        /// <param name="logger">The logger.</param>
        public CompleteExecutor(
            IOwnershipProcessor ownershipProcessor,
            ITrueLogger<CompleteExecutor> logger)
            : base(logger)
        {
            this.ownershipProcessor = ownershipProcessor;
        }

        /// <inheritdoc/>
        public override int Order => 11;

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
            await this.ownershipProcessor.CompleteAsync(ownershipRuleData).ConfigureAwait(false);

            this.ShouldContinue = false;
            await this.ExecuteNextAsync(ownershipRuleData).ConfigureAwait(false);
        }
    }
}