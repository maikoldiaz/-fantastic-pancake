// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableRepository.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.ConfigurationManager.Console.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.ConfigurationManager.Console.Entities;
    using Ecp.True.ConfigurationManager.Console.Repositories.Interface;
    using Ecp.True.ConfigurationManager.Console.Settings.Interface;
    using Ecp.True.ConfigurationManager.Entities;
    using Ecp.True.ConfigurationManager.Services.Interface;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The table repository.
    /// </summary>
    public class TableRepository : ITableRepository
    {
        /// <summary>
        /// The settings manager.
        /// </summary>
        private readonly ISettingsManager settingsManager;

        /// <summary>
        /// The connection.
        /// </summary>
        private readonly IConnection connection;

        /// <summary>
        /// The settings.
        /// </summary>
        private readonly IEnumerable<ISettings> allSettings;

        /// <summary>
        /// The cloud table.
        /// </summary>
        private CloudTable table;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableRepository"/> class.
        /// The table repository.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="allSettings">The settings.</param>
        /// <param name="settingsManager">The settings manager.</param>
        public TableRepository(IConnection connection, IEnumerable<ISettings> allSettings, ISettingsManager settingsManager)
        {
            this.connection = connection;
            this.allSettings = allSettings;
            this.settingsManager = settingsManager;
        }

        /// <inheritdoc/>
        public Task InitializeAsync()
        {
            var storageAccount = CloudStorageAccount.Parse(this.connection.ConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            this.table = tableClient.GetTableReference(typeof(ConfigurationSetting).Name);
            return this.table.CreateIfNotExistsAsync();
        }

        /// <inheritdoc/>
        public async Task UpsertConfigSettingsAsync(string ignorables, string forceUpdates)
        {
            ArgumentValidators.ThrowIfNull(ignorables, nameof(ignorables));

            await this.TryRemoveSettingsAsync().ConfigureAwait(false);
            if (Debugger.IsAttached)
            {
                foreach (var setting in this.allSettings)
                {
                    await this.DoUpsertConfigSettingsAsync(setting, forceUpdates, ignorables).ConfigureAwait(false);
                }

                return;
            }

            await Task.WhenAll(this.allSettings.Select(x => this.DoUpsertConfigSettingsAsync(x, forceUpdates, ignorables))).ConfigureAwait(false);
        }

        private static CopyInput BuildCopyInput(string forceUpdate, JToken existing)
        {
            var ignoreAll = !string.IsNullOrWhiteSpace(forceUpdate) && forceUpdate.Equals("*", StringComparison.OrdinalIgnoreCase);
            var input = new CopyInput(existing, ignoreAll);
            if (ignoreAll || string.IsNullOrWhiteSpace(forceUpdate))
            {
                return input;
            }

            var token = JObject.Parse(forceUpdate);
            var mapping = token.ToObject<IDictionary<string, string[]>>();

            foreach (var pair in mapping)
            {
                input.Ignorables.Add(pair);
            }

            return input;
        }

        private async Task DoUpsertConfigSettingsAsync(ISettings settings, string forceUpdates, string ignorables)
        {
            if (ignorables.Contains(settings.Key, System.StringComparison.InvariantCulture))
            {
                return;
            }

            // Fetch existing setting from store
            var existing = await this.GetExistingValueAsync(settings?.Key).ConfigureAwait(false);

            // Merge new settings with existing
            // This will copy the value from storage if present
            // Unless force updates has this setting as key and properties as one of the values
            // To force update and not merge entire settings, send force updates key value as [*]
            this.settingsManager.Transform(settings, BuildCopyInput(forceUpdates, existing));

            var configurationSetting = new ConfigurationSetting
            {
                Key = settings?.Key,
                Value = settings?.Value,
            };
            var tableEntity = new EcpTableEntity
            {
                RowKey = configurationSetting.Key,
                PartitionKey = "v1",
                Value = JsonConvert.SerializeObject(configurationSetting, Formatting.None),
            };

            Console.WriteLine($"Creating {settings.Key} with value {settings.Value}");

            var insertOperation = TableOperation.InsertOrReplace(tableEntity);
            await this.table.ExecuteAsync(insertOperation).ConfigureAwait(false);
        }

        /// <summary>
        /// Tries to remove settings asynchronous.
        /// </summary>
        private async Task TryRemoveSettingsAsync()
        {
            var currentNames = this.allSettings.Select(s => s.Key).ToList();

            //// Add partition key specific to the config manager(v1_cm)
            //// This way we can ignore keys from other config managers
            //// Rather than harcoding like below
            currentNames.Add("Blockchain.Settings");

            var existingNames = await this.GetExistingSettingsKeysAsync().ConfigureAwait(false);

            var removables = existingNames.Except(currentNames).ToArray();
            foreach (var setting in removables)
            {
                var operation = TableOperation.Retrieve<TrueTableEntity>("v1", setting);
                var result = await this.table.ExecuteAsync(operation).ConfigureAwait(false);

                if (result != null)
                {
                    var entity = (TrueTableEntity)result.Result;
                    await this.table.ExecuteAsync(TableOperation.Delete(entity)).ConfigureAwait(false);

                    Console.WriteLine($"Removed {setting}");
                }
            }
        }

        private async Task<JObject> GetExistingValueAsync(string key)
        {
            var operation = TableOperation.Retrieve<TrueTableEntity>("v1", key);
            var result = await this.table.ExecuteAsync(operation).ConfigureAwait(false);

            if (result.Result == null)
            {
                return null;
            }

            var entity = (TrueTableEntity)result.Result;
            return JObject.Parse(entity.Value);
        }

        private async Task<IEnumerable<string>> GetExistingSettingsKeysAsync()
        {
            TableContinuationToken token = null;
            var entities = new List<TrueTableEntity>();

            do
            {
                var queryResult = await this.table.ExecuteQuerySegmentedAsync(new TableQuery<TrueTableEntity>(), token).ConfigureAwait(false);
                entities.AddRange(queryResult.Results);

                token = queryResult.ContinuationToken;
            }
            while (token != null);

            return entities.Select(e => e.RowKey);
        }
    }
}
