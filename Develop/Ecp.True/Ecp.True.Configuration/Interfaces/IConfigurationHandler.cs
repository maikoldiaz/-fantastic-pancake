// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConfigurationHandler.cs" company="Microsoft">
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

    using Ecp.True.KeyStore.Entities;

    /// <summary>
    /// The configuration handler interface.
    /// </summary>
    public interface IConfigurationHandler
    {
        /// <summary>
        /// Gets configuration setting.
        /// </summary>
        /// <typeparam name="T">Type of setting.</typeparam>
        /// <param name="key">Configuration setting key.</param>
        /// <returns>Task of type.</returns>
        Task<T> GetConfigurationAsync<T>(string key);

        /// <summary>
        /// The get configuration.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <typeparam name="T">
        /// Type of setting.
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task" />.
        /// </returns>
        Task<T> GetConfigurationAsync<T>(string key, T defaultValue);

        /// <summary>Gets the configuration setting.</summary>
        /// <param name="key">The configuration settings key.</param>
        /// <returns>Configuration setting value.</returns>
        Task<string> GetConfigurationAsync(string key);

        /// <summary>
        /// The get configuration or default.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <typeparam name="T">
        /// Type of setting.
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task" />.
        /// </returns>
        Task<T> GetConfigurationOrDefaultAsync<T>(string key, T defaultValue);

        /// <summary>
        /// Initializes the configuration handler.
        /// </summary>
        /// <param name="regionalKeyVaultConfiguration">The regional key vault configuration.</param>
        /// <returns>
        /// Status indicating if the initialization is successful.
        /// </returns>
        Task<bool> InitializeAsync(KeyVaultConfiguration regionalKeyVaultConfiguration);

        /// <summary>
        /// Initializes the configuration handler, this method will need the calling process to implement file provider interface.
        /// </summary>
        /// <returns>
        /// Status indicating if the initialization is successful.
        /// </returns>
        Task<bool> InitializeAsync();

        /// <summary>
        /// Gets the collection configuration asynchronous.
        /// </summary>
        /// <typeparam name="T">Type of setting.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The collections.</returns>
        Task<IEnumerable<T>> GetCollectionConfigurationAsync<T>(string key);
    }
}