// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataService.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.UI.Services.Core
{
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The data Service.
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// Gets the entities asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="apiVersion">The API version.</param>
        /// <param name="uriParts">The Uri parts.</param>
        /// <returns>
        /// The entities.
        /// </returns>
        Task<IQueryable<T>> GetEntitiesAsync<T>(string apiVersion, params string[] uriParts);

        /// <summary>
        /// Gets the entity asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="apiVersion">The API version.</param>
        /// <param name="uriParts">The Uri parts.</param>
        /// <returns>
        /// The entity.
        /// </returns>
        Task<T> GetEntityAsync<T>(string apiVersion, params string[] uriParts);

        /// <summary>
        /// Queries the entities asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="apiVersion">The API version.</param>
        /// <param name="queryString">The query string.</param>
        /// <param name="uriParts">The Uri parts.</param>
        /// <returns>
        /// The entities.
        /// </returns>
        Task<IQueryable<T>> QueryEntitiesAsync<T>(string apiVersion, string queryString, params string[] uriParts);

        /// <summary>
        /// Queries the entity asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="apiVersion">The API version.</param>
        /// <param name="queryString">The query string.</param>
        /// <param name="uriParts">The Uri parts.</param>
        /// <returns>
        /// The entity.
        /// </returns>
        Task<T> QueryEntityAsync<T>(string apiVersion, string queryString, params string[] uriParts);

        /// <summary>
        /// Gets the entity asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="apiVersion">The API version.</param>
        /// <param name="uriParts">The Uri parts.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task<bool> SaveEntityAsync<T>(T entity, string apiVersion, params string[] uriParts);

        /// <summary>
        /// Saves the entity asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="apiVersion">The API version.</param>
        /// <param name="uriParts">The URI parts.</param>
        /// <returns>The task.</returns>
        Task<bool> SaveAsync(object entity, string apiVersion, params string[] uriParts);

        /// <summary>
        /// Gets the entity asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="apiVersion">The API version.</param>
        /// <param name="uriParts">The Uri parts.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task<T> SaveAndGetResultAsync<T>(T entity, string apiVersion, params string[] uriParts);

        /// <summary>
        /// Saves the and get result asynchronous.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="apiVersion">The API version.</param>
        /// <param name="uriParts">The URI parts.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task<TResult> SaveAndGetResultAsync<TResult, T>(T entity, string apiVersion, params string[] uriParts);

        /// <summary>
        /// Updates the entity asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="apiVersion">The API version.</param>
        /// <param name="uriParts">The Uri parts.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task<bool> UpdateEntityAsync<T>(T entity, string apiVersion, params string[] uriParts);

        /// <summary>
        /// Updates the entity and get result asynchronous.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="apiVersion">The API version.</param>
        /// <param name="uriParts">The URI parts.</param>
        /// <returns>The task.</returns>
        Task<TResult> UpdateEntityAndGetResultAsync<TResult, T>(T entity, string apiVersion, params string[] uriParts);

        /// <summary>
        /// Updates the entity asynchronous.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="apiVersion">The API version.</param>
        /// <param name="uriParts">The Uri parts.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task<bool> RemoveEntityAsync(string entityId, string apiVersion, params string[] uriParts);

        /// <summary>
        /// Removes the and get result asynchronous.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="apiVersion">The API version.</param>
        /// <param name="uriParts">The URI parts.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task<TResult> RemoveAndGetResultAsync<TResult, T>(T entity, string apiVersion, params string[] uriParts);
    }
}