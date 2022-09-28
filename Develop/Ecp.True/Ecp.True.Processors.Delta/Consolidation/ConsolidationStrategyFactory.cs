// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidationStrategyFactory.cs" company="Microsoft">
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
    using Ecp.True.Logging;
    using Ecp.True.Processors.Delta.Interfaces;

    /// <summary>
    /// The ConsolidationStrategyFactory.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Delta.Interfaces.IConsolidationStrategyFactory" />
    public class ConsolidationStrategyFactory : IConsolidationStrategyFactory
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<ConsolidationStrategyFactory> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsolidationStrategyFactory"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ConsolidationStrategyFactory(
            ITrueLogger<ConsolidationStrategyFactory> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Gets the movement consolidation strategy.
        /// </summary>
        /// <value>
        /// The movement consolidation strategy.
        /// </value>
        public IConsolidationStrategy MovementConsolidationStrategy => new MovementConsolidationStrategy(this.logger);

        /// <summary>
        /// Gets the inventory product consolidation strategy.
        /// </summary>
        /// <value>
        /// The inventory consolidation strategy.
        /// </value>
        public IConsolidationStrategy InventoryProductConsolidationStrategy => new InventoryProductConsolidationStrategy(this.logger);
    }
}
