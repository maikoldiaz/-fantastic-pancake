// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApiService.cs" company="Microsoft">
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
    using System.Threading.Tasks;

    /// <summary>
    /// The API Service interface.
    /// </summary>
    public interface IApiService
    {
        /// <summary>
        /// The get async.
        /// </summary>
        /// <param name="api">
        /// The API.
        /// </param>
        /// <typeparam name="T">
        /// The type of workload.
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task" />.
        /// </returns>
        Task<T> GetAsync<T>(string api);

        /// <summary>
        /// Posts the asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of workload.</typeparam>
        /// <param name="api">The API.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// The <see cref="Task" />.
        /// </returns>
        Task<bool> PostAsync<T>(string api, T entity);

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <param name="api">The API.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// The task object.
        /// </returns>
        Task<bool> PostAsync(string api, object entity);

        /// <summary>
        /// Posts and gets the result the asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of workload.</typeparam>
        /// <param name="api">The API.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// The <see cref="Task" />.
        /// </returns>
        Task<T> PostAndGetResultAsync<T>(string api, T entity);

        /// <summary>
        /// Posts the and get result asynchronous.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="api">The API.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>The task.</returns>
        Task<TResult> PostAndGetResultAsync<TResult, T>(string api, T entity);

        /// <summary>
        /// Puts the asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of workload.</typeparam>
        /// <param name="api">The API.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// The <see cref="Task" />.
        /// </returns>
        Task<bool> PutAsync<T>(string api, T entity);

        /// <summary>
        /// Puts the and get result asynchronous.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="T">The type of workload.</typeparam>
        /// <param name="api">The API.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// The <see cref="Task" />.
        /// </returns>
        Task<TResult> PutAndGetResultAsync<TResult, T>(string api, T entity);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="api">The API.</param>
        /// <returns>The task.</returns>
        Task<bool> DeleteAsync(string api);

        /// <summary>
        /// Deletes the and get result asynchronous.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="api">The API.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>The task.</returns>
        Task<TResult> DeleteAndGetResultAsync<TResult, T>(string api, T entity);
    }
}