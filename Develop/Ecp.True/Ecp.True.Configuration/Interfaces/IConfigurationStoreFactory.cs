// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConfigurationStoreFactory.cs" company="Microsoft">
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
    using System.Threading.Tasks;

    using Ecp.True.KeyStore.Entities;

    /// <summary>
    /// Configuration store factory interface.
    /// </summary>
    public interface IConfigurationStoreFactory
    {
        /// <summary>
        /// Gets the secret store.
        /// </summary>
        /// <value>
        /// The secret store.
        /// </value>
        IConfigurationStore SecretStore { get; }

        /// <summary>
        /// Gets the configuration store.
        /// </summary>
        /// <param name="key">The config key.</param>
        /// <returns>The <see cref="IConfigurationStore" />.</returns>
        Task<IConfigurationStore> GetConfigurationStoreAsync(string key);

        /// <summary>
        /// Gets the configuration store.
        /// </summary>
        /// <param name="regionalKeyVaultConfiguration">The regional key vault configuration.</param>
        /// <returns>
        /// The <see cref="IConfigurationStore" />.
        /// </returns>
        Task<bool> InitializeConfigurationStoresAsync(KeyVaultConfiguration regionalKeyVaultConfiguration);
    }
}