// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConsolidationStrategy.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Interfaces
{
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Processors.Delta.Entities;

    /// <summary>
    /// The IConsolidationStrategy.
    /// </summary>
    public interface IConsolidationStrategy
    {
        /// <summary>
        /// Consolidates the asynchronous.
        /// </summary>
        /// <param name="batchInfo">The batch info.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        Task ConsolidateAsync(ConsolidationBatch batchInfo, IUnitOfWork unitOfWork);
    }
}
