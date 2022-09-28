// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationHandler.cs" company="Microsoft">
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
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Caching;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.KeyStore.Entities;
    using NeoSmart.AsyncLock;

    /// <summary>
    /// The configuration handler.
    /// </summary>
    [IoCRegistration(IoCLifetime.ContainerControlled)]
    public class ConfigurationHandler : IConfigurationHandler
    {
        /// <summary>
        /// The concurrent lock.
        /// </summary>
        private static readonly ConcurrentDictionary<string, AsyncLock> ConcurrentLock = new ConcurrentDictionary<string, AsyncLock>();

        /// <summary>
        /// The configuration store factory.
        /// </summary>
        private readonly IConfigurationStoreFactory configurationStoreFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationHandler" /> class.
        /// </summary>
        /// <param name="configurationStoreFactory">The configuration store factory.</param>
        public ConfigurationHandler(IConfigurationStoreFactory configurationStoreFactory)
        {
            this.configurationStoreFactory = configurationStoreFactory;
        }

        /// <summary>
        /// Initializes the configuration handler.
        /// </summary>
        /// <param name="regionalKeyVaultConfiguration">The regional key vault configuration.</param>
        /// <returns>
        /// Status indicating if the initialization is successful.
        /// </returns>
        public Task<bool> InitializeAsync(KeyVaultConfiguration regionalKeyVaultConfiguration)
        {
            return this.configurationStoreFactory.InitializeConfigurationStoresAsync(regionalKeyVaultConfiguration);
        }

        /// <summary>Initializes the configuration handler, this method will need the calling process to implement file provider interface.</summary>
        /// <returns>Status indicating if the initialization is successful.</returns>
        public Task<bool> InitializeAsync()
        {
            // Pass null for config, the values will be fetched from File
            return this.configurationStoreFactory.InitializeConfigurationStoresAsync(null);
        }

        /// <summary>
        /// Gets demo configuration setting.
        /// </summary>
        /// <typeparam name="T">Type of setting.</typeparam>
        /// <param name="key">Configuration setting key.</param>
        /// <param name="defaultValue">default object.</param>
        /// <returns>
        /// Task of type.
        /// </returns>
        public async Task<T> GetConfigurationOrDefaultAsync<T>(string key, T defaultValue)
        {
            return await this.GetConfigurationAsync<T>(key).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets configuration setting.
        /// </summary>
        /// <typeparam name="T">Type of setting.</typeparam>
        /// <param name="key">Configuration setting key.</param>
        /// <returns>
        /// Task of type.
        /// </returns>
        public async Task<T> GetConfigurationAsync<T>(string key)
        {
            ArgumentValidators.ThrowIfNullOrEmpty(key, nameof(key));

            var config = InMemoryCacheManager.Get<T>(key);
            if (!EqualityComparer<T>.Default.Equals(config, default(T)))
            {
                return config;
            }

            // no need to jump to actual config store when multiple thread don't find it in cache for the first time
            // so, using optimized locking so that all thread do not get blocked.
            // only the config requested for a particular key will acquire lock only for that key if not found in cache
            // others can proceed in parallel since each key will use separate lock
            using (await ConcurrentLock.GetOrAdd(key, new AsyncLock()).LockAsync().ConfigureAwait(false))
            {
                // recheck if it's added into cache by other while waiting on to acquire lock
                config = InMemoryCacheManager.Get<T>(key);
                if (!EqualityComparer<T>.Default.Equals(config, default(T)))
                {
                    return config;
                }

                var configStore = await this.configurationStoreFactory.GetConfigurationStoreAsync(key).ConfigureAwait(false);
                config = await configStore.GetFromStoreAsync<T>(key).ConfigureAwait(false);

                // Keeping track of the data type of config is required to be able to update cache independently
                InMemoryCacheManager.Add(key, config);
                return config;
            }
        }

        /// <summary>
        /// The get configuration.
        /// </summary>
        /// <typeparam name="T">Type of setting.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The <see cref="Task" />.
        /// </returns>
        public async Task<T> GetConfigurationAsync<T>(string key, T defaultValue)
        {
            var config = await this.GetConfigurationAsync<T>(key).ConfigureAwait(false);
            if (EqualityComparer<T>.Default.Equals(config, default(T)))
            {
                config = defaultValue;
            }

            return config;
        }

        /// <summary>
        /// Gets the configuration setting.
        /// </summary>
        /// <param name="key">The configuration settings key.</param>
        /// <returns>
        /// Configuration setting value.
        /// </returns>
        public Task<string> GetConfigurationAsync(string key)
        {
            return this.GetConfigurationAsync<string>(key);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetCollectionConfigurationAsync<T>(string key)
        {
            ArgumentValidators.ThrowIfNullOrEmpty(key, nameof(key));
            var resultCollection = new List<T>();

            //// Removing the DataStoreConfigurationPrefix.
            var stringPattern = key.Split('_')[1];

            var configStore = await this.configurationStoreFactory.GetConfigurationStoreAsync(key).ConfigureAwait(false);
            var totalKeys = await configStore.GetAllFromStoreAsync<ConfigurationSetting>().ConfigureAwait(false);
            totalKeys.ForEach(x =>
            {
                if (x.Key.Contains(stringPattern, System.StringComparison.OrdinalIgnoreCase))
                {
                    resultCollection.Add(ParseType<T>(x.Value));
                }
            });
            return resultCollection;
        }

        /// <summary>
        /// The parse type.
        /// </summary>
        /// <param name="strValue">
        /// The string value.
        /// </param>
        /// <typeparam name="T">
        /// Type of setting.
        /// </typeparam>
        /// <returns>
        /// typed value.
        /// </returns>
        protected static T ParseType<T>(string strValue)
        {
            ArgumentValidators.ThrowIfNull(strValue, nameof(strValue));
            var typedValue = default(T);
            var parseResult = strValue.ParseType(typeof(T));

            if (parseResult.Item2 != null)
            {
                return typedValue;
            }

            if (parseResult.Item1 != null)
            {
                typedValue = (T)parseResult.Item1;
            }

            return typedValue;
        }
    }
}