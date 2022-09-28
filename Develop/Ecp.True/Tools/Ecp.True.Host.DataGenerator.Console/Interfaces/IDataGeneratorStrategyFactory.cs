// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataGeneratorStrategyFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.DataGenerator.Console.Interfaces
{
    /// <summary>
    /// The IDataGeneratorStrategyFactory.
    /// </summary>
    public interface IDataGeneratorStrategyFactory
    {
        /// <summary>
        /// Gets the delta data generator strategy.
        /// </summary>
        /// <value>
        /// The delta data generator strategy.
        /// </value>
        IDataGeneratorStrategy DeltaDataGeneratorStrategy { get; }

        /// <summary>
        /// Gets the consolidation data generator strategy.
        /// </summary>
        /// <value>
        /// The consolidation data generator strategy.
        /// </value>
        IDataGeneratorStrategy ConsolidationDataGeneratorStrategy { get; }

        /// <summary>
        /// Gets the official delta data generator strategy.
        /// </summary>
        /// <value>
        /// The official delta data generator strategy.
        /// </value>
        IDataGeneratorStrategy OfficialDeltaDataGeneratorStrategy { get; }

        /// <summary>
        /// Gets the officiallogistics generator strategy.
        /// </summary>
        /// <value>
        /// The officiallogistics generator strategy.
        /// </value>
        IDataGeneratorStrategy OfficialLogisticsGeneratorStrategy { get; }

        /// <summary>
        /// Gets the cut off data generator strategy.
        /// </summary>
        /// <value>
        /// The cut off data generator strategy.
        /// </value>
        IDataGeneratorStrategy CutOffDataGeneratorStrategy { get; }
    }
}
