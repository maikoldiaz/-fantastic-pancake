// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageConfigurationStore.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.DataAccess;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Ioc.Interfaces;
    using Ecp.True.Logging;
    using Microsoft.Azure.Cosmos.Table;

    /// <summary>
    /// The storage configuration store.
    /// </summary>
    [IoCRegistration(IoCLifetime.ContainerControlled)]
    public class StorageConfigurationStore : IConfigurationStore
    {
        /// <summary>
        /// The string secret replacement indicator.
        /// </summary>
        private const string StringSecretReplacementIndicator = "#";

        /// <summary>
        /// The configuration settings exception message.
        /// </summary>
        private const string ConfigurationSettingsParsingExceptionMessage = "Exception while parsing configuration value to {0}.";

        /// <summary>
        /// The binary secret replacement indicator.
        /// </summary>
        private const string BinarySecretReplacementIndicator = "##";

        /// <summary>
        /// The string secret replacement pattern.
        /// </summary>
        private readonly string stringSecretReplacementPattern = $"{StringSecretReplacementIndicator}[\\w.-_]+{StringSecretReplacementIndicator}";

        /// <summary>
        /// The binary secret replacement pattern.
        /// </summary>
        private readonly string binarySecretReplacementPattern = $"{BinarySecretReplacementIndicator}[\\w.-_]+{BinarySecretReplacementIndicator}";

        /// <summary>
        /// The configuration store factory.
        /// </summary>
        private readonly IConfigurationStoreFactory configurationStoreFactory;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly IResolver resolver;

        /// <summary>
        /// The version.
        /// </summary>
        private readonly string version;

        /// <summary>
        /// The table provider.
        /// </summary>
        private readonly ITableProvider tableProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageConfigurationStore" /> class.
        /// </summary>
        /// <param name="tableProvider">The table provider.</param>
        /// <param name="configurationStoreFactory">The configuration store factory.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="version">The version.</param>
        public StorageConfigurationStore(
            ITableProvider tableProvider,
            IConfigurationStoreFactory configurationStoreFactory,
            IResolver resolver,
            string version)
        {
            this.configurationStoreFactory = configurationStoreFactory;
            this.resolver = resolver;
            this.version = version;
            this.tableProvider = tableProvider;
        }

        private ITrueLogger<StorageConfigurationStore> Logger => this.resolver.GetInstance<ITrueLogger<StorageConfigurationStore>>();

        /// <summary>
        /// The get from store async.
        /// </summary>
        /// <typeparam name="T">The type of value to retrieve.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>
        /// The value.
        /// </returns>
        public async Task<T> GetFromStoreAsync<T>(string key)
        {
            ArgumentValidators.ThrowIfNullOrEmpty(key, nameof(key));

            var configurationKey = key.Replace(ConfigurationConstants.DataStoreConfigurationPrefix, string.Empty, StringComparison.OrdinalIgnoreCase);
            var settings = await this.tableProvider.GetByRowKeyAndPartitionKeyAsync<ConfigurationSetting>(configurationKey, this.version).ConfigureAwait(false);
            var convertedJson = await this.ReplaceSecretPlaceholderInConfigurationAsync(settings.Value).ConfigureAwait(false);
            return this.ParseType<T>(convertedJson);
        }

        /// <summary>
        /// The get from store asynchronously
        /// Considers caching depending on shouldCache parameter.
        /// </summary>
        /// <typeparam name="T">The type of value to retrieve.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="shouldCache">if set to <c>true</c> [should cache].</param>
        /// <returns>
        /// The value.
        /// </returns>
        public Task<T> GetFromStoreAsync<T>(string key, bool shouldCache)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetAllFromStoreAsync<T>()
        {
            var resultCollection = new List<T>();

            var query = new TableQuery<ConfigurationSetting>().Where(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "v1"));

            var settingCollections = await this.tableProvider.ExecuteQueryAsync(query).ConfigureAwait(false);
            foreach (var item in settingCollections)
            {
                var convertedJson = await this.ReplaceSecretPlaceholderInConfigurationAsync(item.Value).ConfigureAwait(false);
                resultCollection.Add(this.ParseType<T>(convertedJson));
            }

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
        private T ParseType<T>(string strValue)
        {
            var typedValue = default(T);
            var parseResult = strValue.ParseType(typeof(T));

            if (parseResult.Item2 != null)
            {
                this.Logger.LogError(parseResult.Item2, string.Format(CultureInfo.InvariantCulture, ConfigurationSettingsParsingExceptionMessage, typeof(T)));
                return typedValue;
            }

            if (parseResult.Item1 != null)
            {
                typedValue = (T)parseResult.Item1;
            }

            return typedValue;
        }

        /// <summary>
        /// Replaces the secret by replacement pattern.
        /// </summary>
        /// <param name="jsonString">The JSON string.</param>
        /// <param name="replacementIndicator">The replacement indicator.</param>
        /// <param name="replacementPattern">The replacement pattern.</param>
        /// <returns>
        /// The updated JSON string.
        /// </returns>
        private async Task<string> ReplaceSecretByReplacementPatternAsync(string jsonString, string replacementIndicator, string replacementPattern)
        {
            var secret = jsonString;
            var secretMatches = Regex.Matches(jsonString, replacementPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            var uniqueSecretMatches = secretMatches.OfType<Match>().Select(m => m.Value).Distinct();

            // get all matches value from key vault & replace in json
            foreach (var key in uniqueSecretMatches)
            {
                var configKey = key.Replace(replacementIndicator, string.Empty, StringComparison.OrdinalIgnoreCase);
                var newValue = await this.configurationStoreFactory.SecretStore.GetFromStoreAsync<string>(configKey).ConfigureAwait(false);
                secret = secret.Replace(key, newValue, StringComparison.OrdinalIgnoreCase);
            }

            return secret;
        }

        /// <summary>
        /// Transforms the JSON.
        /// </summary>
        /// <param name="jsonString">The JSON string.</param>
        /// <returns>JSON string after replacing all values.</returns>
        private async Task<string> ReplaceSecretPlaceholderInConfigurationAsync(string jsonString)
        {
            string secret;
            try
            {
                // Find all binary secrets with ##*## pattern in config
                secret = await this.ReplaceSecretByReplacementPatternAsync(jsonString, BinarySecretReplacementIndicator, this.binarySecretReplacementPattern).ConfigureAwait(false);

                // Find all string secrets with #*# pattern in config
                secret = await this.ReplaceSecretByReplacementPatternAsync(secret, StringSecretReplacementIndicator, this.stringSecretReplacementPattern).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, "Not Found");
                throw;
            }

            return secret;
        }
    }
}