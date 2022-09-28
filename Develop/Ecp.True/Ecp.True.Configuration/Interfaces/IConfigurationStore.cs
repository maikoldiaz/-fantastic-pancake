// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConfigurationStore.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Configuration
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// The configuration store.
    /// </summary>
    public interface IConfigurationStore
    {
        /// <summary>
        /// The get from store asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of value to retrieve.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>
        /// The value.
        /// </returns>
        Task<T> GetFromStoreAsync<T>(string key);

        /// <summary>
        /// The get from store asynchronously.
        /// Considers caching depending on shouldCache parameter.
        /// </summary>
        /// <typeparam name="T">The type of value to retrieve.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="shouldCache">if set to <c>true</c> [should cache].</param>
        /// <returns>
        /// The value.
        /// </returns>
        Task<T> GetFromStoreAsync<T>(string key, bool shouldCache);

        /// <summary>
        /// Gets all from store asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of value to retrieve.</typeparam>
        /// <returns>The collections.</returns>
        Task<IEnumerable<T>> GetAllFromStoreAsync<T>();
    }
}