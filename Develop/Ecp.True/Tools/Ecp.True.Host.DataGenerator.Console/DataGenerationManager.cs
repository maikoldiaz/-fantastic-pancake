// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGenerationManager.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.DataGenerator.Console
{
    using System.Threading.Tasks;
    using Ecp.True.Host.DataGenerator.Console.Interfaces;

    /// <summary>
    /// The DataGenerationManager.
    /// </summary>
    public class DataGenerationManager : IDataGenerationManager
    {
        /// <summary>
        /// The data generator strategy factory.
        /// </summary>
        private readonly IDataGeneratorStrategyFactory dataGeneratorStrategyFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGenerationManager" /> class.
        /// </summary>
        /// <param name="dataGeneratorStrategyFactory">The data generator strategy factory.</param>
        public DataGenerationManager(IDataGeneratorStrategyFactory dataGeneratorStrategyFactory)
        {
            this.dataGeneratorStrategyFactory = dataGeneratorStrategyFactory;
        }

        /// <summary>
        /// Generates the delta data.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The task.</returns>
        public async Task GenerateDeltaDataAsync(string[] args)
        {
            var deltaDataGeneratorStrategy = this.dataGeneratorStrategyFactory.DeltaDataGeneratorStrategy;
            deltaDataGeneratorStrategy.Initialize(args);
            await deltaDataGeneratorStrategy.GenerateAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Generates the consolidation delta data asynchronous.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The task.</returns>
        public async Task GenerateConsolidationDeltaDataAsync(string[] args)
        {
            var consolidationDataGeneratorStrategy = this.dataGeneratorStrategyFactory.ConsolidationDataGeneratorStrategy;
            consolidationDataGeneratorStrategy.Initialize(args);
            await consolidationDataGeneratorStrategy.GenerateAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Generates the official delta data.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The task.</returns>
        public async Task GenerateOfficialDeltaDataAsync(string[] args)
        {
            var consolidationDataGeneratorStrategy = this.dataGeneratorStrategyFactory.ConsolidationDataGeneratorStrategy;
            consolidationDataGeneratorStrategy.Initialize(args);
            await consolidationDataGeneratorStrategy.GenerateAsync(true).ConfigureAwait(false);
            var officialDeltaDataGeneratorStrategy = this.dataGeneratorStrategyFactory.OfficialDeltaDataGeneratorStrategy;
            officialDeltaDataGeneratorStrategy.Initialize(args, consolidationDataGeneratorStrategy.Config);
            await officialDeltaDataGeneratorStrategy.GenerateAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Generates the official logistics data asynchronous.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The task.</returns>
        public async Task GenerateOfficialLogisticsDataAsync(string[] args)
        {
            var officiallogisticsGeneratorStrategy = this.dataGeneratorStrategyFactory.OfficialLogisticsGeneratorStrategy;
            officiallogisticsGeneratorStrategy.Initialize(args);
            await officiallogisticsGeneratorStrategy.GenerateAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Generates the cut off data asynchronous.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The task.</returns>
        public async Task GenerateCutOffDataAsync(string[] args)
        {
            var cutOffDataGeneratorStrategy = this.dataGeneratorStrategyFactory.CutOffDataGeneratorStrategy;
            cutOffDataGeneratorStrategy.Initialize(args);
            await cutOffDataGeneratorStrategy.GenerateAsync().ConfigureAwait(false);
        }
    }
}
