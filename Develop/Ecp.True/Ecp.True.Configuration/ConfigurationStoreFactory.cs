// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationStoreFactory.cs" company="Microsoft">
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
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;

    using Ecp.True.DataAccess;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Ioc.Interfaces;
    using Ecp.True.KeyStore.Entities;
    using Ecp.True.KeyStore.Interfaces;

    /// <summary>
    /// The configuration store factory.
    /// </summary>
    [IoCRegistration(IoCLifetime.ContainerControlled)]
    [CLSCompliant(false)]
    public class ConfigurationStoreFactory : IConfigurationStoreFactory
    {
        /// <summary>
        /// The file configuration store.
        /// </summary>
        private readonly IFileConfigurationStore fileConfigStore;

        /// <summary>
        /// The resolver.
        /// </summary>
        private readonly IResolver resolver;

        /// <summary>
        /// The regional secret management provider.
        /// </summary>
        private readonly ISecretManagementProvider secretManagementProvider;

        /// <summary>
        /// The table provider.
        /// </summary>
        private readonly ITableProvider tableProvider;

        /// <summary>
        /// The regional storage store.
        /// </summary>
        private IConfigurationStore storageStore;

        /// <summary>
        /// The regional secret store.
        /// </summary>
        private SecretConfigurationStore secretStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationStoreFactory" /> class.
        /// </summary>
        /// <param name="fileConfigStore">The file configuration store.</param>
        /// <param name="secretManagementProvider">The regional secret management provider.</param>
        /// <param name="tableProvider">The table provider.</param>
        /// <param name="resolver">The resolver.</param>
        public ConfigurationStoreFactory(
            IFileConfigurationStore fileConfigStore,
            ISecretManagementProvider secretManagementProvider,
            ITableProvider tableProvider,
            IResolver resolver)
        {
            this.fileConfigStore = fileConfigStore;
            this.secretManagementProvider = secretManagementProvider;
            this.tableProvider = tableProvider;
            this.resolver = resolver;
        }

        /// <summary>
        /// Gets the regional secret store.
        /// </summary>
        /// <value>
        /// The regional secret store.
        /// </value>
        public IConfigurationStore SecretStore => this.secretStore;

        /// <summary>
        /// Gets the configuration store.
        /// </summary>
        /// <param name="key">The config key.</param>
        /// <returns>
        /// The <see cref="IConfigurationStore" />.
        /// </returns>
        public Task<IConfigurationStore> GetConfigurationStoreAsync(string key)
        {
            return this.GetConfigurationStoreBasedOnKeyNameAsync(key);
        }

        /// <summary>
        /// Gets the configuration store.
        /// </summary>
        /// <param name="regionalKeyVaultConfiguration">The regional key vault configuration.</param>
        /// <returns>
        /// The <see cref="IConfigurationStore" />.
        /// </returns>
        public async Task<bool> InitializeConfigurationStoresAsync(KeyVaultConfiguration regionalKeyVaultConfiguration)
        {
            var keyvaultConfiguration = regionalKeyVaultConfiguration;
            if (regionalKeyVaultConfiguration == null)
            {
                var vaultName = await this.fileConfigStore.GetFromStoreAsync<string>(ConfigurationConstants.KeyVaultName).ConfigureAwait(false);
                keyvaultConfiguration = new KeyVaultConfiguration
                {
                    VaultAddress = $"https://{vaultName}.vault.azure.net",
                };
            }

            this.secretManagementProvider.InitializeSettings(keyvaultConfiguration);
            this.secretStore = new SecretConfigurationStore(this.secretManagementProvider);

            return true;
        }

        /// <summary>
        /// Gets the name of the configuration store based on key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// Configuration Store based on the key name.
        /// </returns>
        /// <exception cref="System.ArgumentException">Secret Store must be initialized first.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">key.</exception>
        private async Task<IConfigurationStore> GetConfigurationStoreBasedOnKeyNameAsync(string key)
        {
            if (key.StartsWith(ConfigurationConstants.SecretConfigurationPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return this.secretStore ?? (this.secretStore =
                               new SecretConfigurationStore(this.secretManagementProvider));
            }

            if (key.StartsWith(ConfigurationConstants.FileConfigurationPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return this.fileConfigStore;
            }

            if (key.StartsWith(ConfigurationConstants.DataStoreConfigurationPrefix, StringComparison.OrdinalIgnoreCase))
            {
                ArgumentValidators.ThrowIfNull(this.secretStore, nameof(this.secretStore));

                if (this.storageStore != null)
                {
                    return this.storageStore;
                }

                await Task.CompletedTask.ConfigureAwait(false);

                this.storageStore = new StorageConfigurationStore(this.tableProvider, this, this.resolver, "v1");
                return this.storageStore;
            }

            throw new ArgumentOutOfRangeException(nameof(key), $"{key} does not start with a valid prefix");
        }
    }
}