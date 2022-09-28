// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataGenerationManager.cs" company="Microsoft">
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
    using System.Threading.Tasks;

    /// <summary>
    /// The IDataGenerationManager.
    /// </summary>
    public interface IDataGenerationManager
    {
        /// <summary>
        /// Generates the delta data.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task GenerateDeltaDataAsync(string[] args);

        /// <summary>
        /// Generates the consolidation delta data asynchronous.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The Task.</returns>
        Task GenerateConsolidationDeltaDataAsync(string[] args);

        /// <summary>
        /// Generates the official delta data.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task GenerateOfficialDeltaDataAsync(string[] args);

        /// <summary>
        /// Generates the official logistics data asynchronous.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>Generate Official Logistic sData.</returns>
        Task GenerateOfficialLogisticsDataAsync(string[] args);

        /// <summary>
        /// Generates the cut off data asynchronous.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task GenerateCutOffDataAsync(string[] args);
    }
}
