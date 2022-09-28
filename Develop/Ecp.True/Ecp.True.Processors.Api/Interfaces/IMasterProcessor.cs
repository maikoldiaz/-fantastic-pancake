// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMasterProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// The IMaster Processor.
    /// </summary>
    public interface IMasterProcessor : IProcessor
    {
        /// <summary>
        /// Gets logistic centers asynchronous.
        /// </summary>
        /// <returns>
        /// The collection of SAP Logistic Centers.
        /// </returns>
        Task<IEnumerable<LogisticCenter>> GetLogisticCentersAsync();

        /// <summary>
        /// Gets storage locations asynchronous.
        /// </summary>
        /// <returns>The collection of SAP Storage Locations.</returns>
        Task<IEnumerable<StorageLocation>> GetStorageLocationsAsync();

        /// <summary>
        /// Gets products asynchronous.
        /// </summary>
        /// <returns>The collection of Products.</returns>
        Task<IEnumerable<Product>> GetProductsAsync();

        /// <summary>
        /// Gets the scenarios by roles asynchronous.
        /// </summary>
        /// <param name="roles">The roles.</param>
        /// <returns>
        /// The scenarios.
        /// </returns>
        Task<IEnumerable<Scenario>> GetScenariosByRoleAsync(IEnumerable<string> roles);

        /// <summary>
        /// Gets the algorithms asynchronous.
        /// </summary>
        /// <returns>
        /// The algorithms.
        /// </returns>
        Task<IEnumerable<Algorithm>> GetAlgorithmsAsync();

        /// <summary>
        /// Gets the system types asynchronous.
        /// </summary>
        /// <returns>The Collection of SystemTypeEntity.</returns>
        Task<IEnumerable<SystemTypeEntity>> GetSystemTypesAsync();

        /// <summary>
        /// Gets the users asynchronous.
        /// </summary>
        /// <returns>The users.</returns>
        Task<IEnumerable<User>> GetUsersAsync();

        /// <summary>
        /// Gets the variable types asynchronous.
        /// </summary>
        /// <returns>The Variable Types.</returns>
        Task<IEnumerable<VariableTypeEntity>> GetVariableTypesAsync();

        /// <summary>
        /// Gets the icons asynchronous.
        /// </summary>
        /// <returns>The Icons.</returns>
        Task<IEnumerable<Icon>> GetIconsAsync();

        /// <summary>
        /// Gets the origin types asynchronous.
        /// </summary>
        /// <returns>the origin types.</returns>
        Task<IEnumerable<OriginType>> GetOriginTypesAsync();
    }
}
