// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGeneratorStrategyFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.DataGenerator.Console.Strategies
{
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Host.DataGenerator.Console.Generators;
    using Ecp.True.Host.DataGenerator.Console.Interfaces;

    /// <summary>
    /// The DataGeneratorStrategyFactory.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.DataGenerator.Console.Interfaces.IDataGeneratorStrategyFactory" />
    public class DataGeneratorStrategyFactory : IDataGeneratorStrategyFactory
    {
        /// <summary>
        /// The data generators.
        /// </summary>
        private readonly IDataGeneratorFactory dataGeneratorFactory;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGeneratorStrategyFactory" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public DataGeneratorStrategyFactory(IUnitOfWorkFactory unitOfWorkFactory)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));

            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.dataGeneratorFactory = new DataGeneratorFactory(this.unitOfWork);
        }

        /// <summary>
        /// Gets the delta data generator strategy.
        /// </summary>
        /// <value>
        /// The delta data generator strategy.
        /// </value>
        public IDataGeneratorStrategy DeltaDataGeneratorStrategy => new DeltaDataGeneratorStrategy(this.unitOfWork, this.dataGeneratorFactory);

        /// <summary>
        /// Gets the consolidation data generator strategy.
        /// </summary>
        /// <value>
        /// The consolidation data generator strategy.
        /// </value>
        public IDataGeneratorStrategy ConsolidationDataGeneratorStrategy => new ConsolidationDataGeneratorStrategy(this.unitOfWork, this.dataGeneratorFactory);

        /// <summary>
        /// Gets the official delta data generator strategy.
        /// </summary>
        /// <value>
        /// The official delta data generator strategy.
        /// </value>
        public IDataGeneratorStrategy OfficialDeltaDataGeneratorStrategy => new OfficialDeltaDataGeneratorStrategy(this.unitOfWork, this.dataGeneratorFactory);

        /// <summary>
        /// Gets the official logistics generator strategy.
        /// </summary>
        /// <value>
        /// The official logistics generator strategy.
        /// </value>
        public IDataGeneratorStrategy OfficialLogisticsGeneratorStrategy => new OfficialLogisticsGeneratorStrategy(this.unitOfWork, this.dataGeneratorFactory);

        /// <summary>
        /// Gets the cut off data generator strategy.
        /// </summary>
        /// <value>
        /// The cut off data generator strategy.
        /// </value>
        public IDataGeneratorStrategy CutOffDataGeneratorStrategy => new CutOffDataGeneratorStrategy(this.unitOfWork, this.dataGeneratorFactory);
    }
}
