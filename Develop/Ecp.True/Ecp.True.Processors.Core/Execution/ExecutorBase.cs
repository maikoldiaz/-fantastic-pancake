// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecutorBase.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Execution
{
    using System.Threading.Tasks;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// The class ExecutorBase.
    /// </summary>
    public abstract class ExecutorBase : IExecutor
    {
        /// <summary>
        /// The next executor in chain.
        /// </summary>
        private IExecutor nextExecutor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutorBase"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        protected ExecutorBase(ITrueLogger logger)
        {
            this.Logger = logger;
        }

        /// <inheritdoc/>
        public abstract int Order { get; }

        /// <summary>
        /// Gets the type of the process.
        /// </summary>
        /// <value>
        /// The type of the process.
        /// </value>
        public abstract ProcessType ProcessType { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the current request should be executed.
        /// </summary>
        /// <value>
        ///     <c>true</c> if next executor can execute; otherwise, <c>false</c>.
        /// </value>
        public bool ShouldContinue { get; set; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        protected ITrueLogger Logger { get; }

        /// <inheritdoc/>
        public void SetNext(IExecutor nextExecutor)
        {
            this.nextExecutor = nextExecutor;
        }

        /// <inheritdoc/>
        public abstract Task ExecuteAsync(object input);

        /// <summary>
        /// Invoke the next executor.
        /// </summary>
        /// <param name="input">The input object.</param>
        /// <returns>The task.</returns>
        public async Task ExecuteNextAsync(object input)
        {
            if (this.ShouldContinue && this.nextExecutor != null)
            {
                this.Logger.LogInformation($"Executor {this.nextExecutor.GetType().Name} with order {this.nextExecutor.Order} started processing.");
                await this.nextExecutor.ExecuteAsync(input).ConfigureAwait(false);
            }
        }
    }
}
