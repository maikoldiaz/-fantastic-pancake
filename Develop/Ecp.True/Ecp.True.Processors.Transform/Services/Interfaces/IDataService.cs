// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Processors.Transform.Services.Interfaces
{
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The data service.
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// Saves the homologated array asynchronous.
        /// </summary>
        /// <param name="homologatedArray">The homologated array.</param>
        /// <param name="trueMessage">The TRUE message.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task SaveAsync(JArray homologatedArray, TrueMessage trueMessage);

        /// <summary>
        /// Saves the excel asynchronous.
        /// </summary>
        /// <param name="inventoryArray">The inventory array.</param>
        /// <param name="movementArray">The movement array.</param>
        /// <param name="eventArray">The event array.</param>
        /// <param name="contractArray">The contract array.</param>
        /// <param name="trueMessage">The true message.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task SaveExcelAsync(JArray inventoryArray, JArray movementArray, JArray eventArray, JArray contractArray, TrueMessage trueMessage);

        /// <summary>
        /// Saves the external source entity array asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task<string> SaveExternalSourceEntityArrayAsync(JArray entity, TrueMessage message);

        /// <summary>
        /// Saves the external source entity asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task<string> SaveExternalSourceEntityAsync(JObject entity, TrueMessage message);

        /// <summary>
        /// Saves the logistic entity asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task<string> SaveLogisticEntityAsync(JObject entity, TrueMessage message);
    }
}
