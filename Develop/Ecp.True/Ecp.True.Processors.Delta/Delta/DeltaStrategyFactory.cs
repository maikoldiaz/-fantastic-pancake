// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaStrategyFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Delta
{
    using Ecp.True.Logging;
    using Ecp.True.Processors.Delta.Interfaces;

    /// <summary>
    /// The  delta strategy factory.
    /// </summary>
    public class DeltaStrategyFactory : IDeltaStrategyFactory
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<DeltaStrategyFactory> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaStrategyFactory"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public DeltaStrategyFactory(
            ITrueLogger<DeltaStrategyFactory> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc/>
        public IDeltaStrategy MovementDeltaStrategy => new MovementDeltaStrategy(this.logger);

        /// <inheritdoc/>
        public IDeltaStrategy InventoryDeltaStrategy => new InventoryDeltaStrategy(this.logger);
    }
}
