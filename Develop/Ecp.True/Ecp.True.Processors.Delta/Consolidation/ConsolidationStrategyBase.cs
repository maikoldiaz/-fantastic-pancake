// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidationStrategyBase.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Consolidation
{
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Interfaces;

    /// <summary>
    /// The ConsolidationStrategyBase.
    /// </summary>
    public abstract class ConsolidationStrategyBase : IConsolidationStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsolidationStrategyBase"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        protected ConsolidationStrategyBase(ITrueLogger logger)
        {
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
        /// Consolidates the asynchronous.
        /// </summary>
        /// <param name="batchInfo">The batch info.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        public abstract Task ConsolidateAsync(ConsolidationBatch batchInfo, IUnitOfWork unitOfWork);
    }
}
