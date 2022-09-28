// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainDataSource.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.DataSources
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;

    using global::Bdd.Core.DataSources;
    using global::Bdd.Core.Utils;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    // https://nethereum.readthedocs.io/en/latest/nethereum-azure-quorum/
    public class BlockchainDataSource : DataSourceBase
    {
        protected static readonly ConcurrentDictionary<string, EthereumClient> Clients = new ConcurrentDictionary<string, EthereumClient>();

        private static readonly NameValueCollection Settings = ConfigurationManager.GetSection("blockchain") as NameValueCollection;

        private static Dictionary<string, ContractMetadataStruct> contracts = new Dictionary<string, ContractMetadataStruct>();

        private static List<string> contractNames = new List<string>() { "MovementsFactory", "InventoryProductsFactory", "NodeDetailsFactory", "NodeConnectionDetailsFactory", "MovementDetailOwnerFactory", "InventoryProductOwnerFactory", "MovementOwnershipFactory", "InventoryOwnershipFactory", "NodeProductCalculationsFactory" };

        private static bool isInitialised;

        public async Task<T> GetDataAsync<T>(string contractName, string methodName, Dictionary<string, object> parameters, string keyPrefix = null)
            where T : class, new()
        {
            var client = await this.GetClientAsync(keyPrefix).ConfigureAwait(false);
            var contractDetails = contracts.First(c => c.Key == contractName).Value;
            var data = await client.GetDataAsync<T>(contractDetails.ContractAbi, contractDetails.ContractAddress, methodName, parameters).ConfigureAwait(false);
            return data;
        }

        protected override Task<IEnumerable<T>> ReadAllInternalAsync<T>(string input = null, string keyPrefix = null, params object[] args)
        {
            throw new NotSupportedException();
        }

        protected override Task<T> ReadInternalAsync<T>(string input = null, string keyPrefix = null, params object[] args)
        {
            throw new NotSupportedException();
        }

        private async Task<BlockchainConnectionSettings> GetBlockchainClientSettingAsync(string keyPrefix = null)
        {
            var accountAddressSecretName = Settings.GetValue("EthereumAccountAddress", string.IsNullOrWhiteSpace(keyPrefix) ? this.DefaultKeyPrefix : keyPrefix);
            var accountKeySecretName = Settings.GetValue("EthereumAccountKey", string.IsNullOrWhiteSpace(keyPrefix) ? this.DefaultKeyPrefix : keyPrefix);
            var rpcEndPointSecretName = Settings.GetValue("RpcEndpoint", string.IsNullOrWhiteSpace(keyPrefix) ? this.DefaultKeyPrefix : keyPrefix);
            var movementContactAddressSecretName = Settings.GetValue("MovementContractAddress", string.IsNullOrWhiteSpace(keyPrefix) ? this.DefaultKeyPrefix : keyPrefix);
            var inventoryContactAddressSecretName = Settings.GetValue("InventoryContractAddress", string.IsNullOrWhiteSpace(keyPrefix) ? this.DefaultKeyPrefix : keyPrefix);

            var accountAddress = (await KeyVaultHelper.GetKeyVaultSecretAsync(accountAddressSecretName).ConfigureAwait(false)).Value;
            var accountKey = (await KeyVaultHelper.GetKeyVaultSecretAsync(accountKeySecretName).ConfigureAwait(false)).Value;
            var rpcEndPoint = (await KeyVaultHelper.GetKeyVaultSecretAsync(rpcEndPointSecretName).ConfigureAwait(false)).Value;
            var contractFactory = (await KeyVaultHelper.GetKeyVaultSecretAsync("ContractFactoryContractAddress").ConfigureAwait(false)).Value;
            var tableDataSource = new TableStorageDataSource();
#pragma warning disable VSTHRD103 // Call async methods when in an async method
            var tableData = tableDataSource.ReadAll("ConfigurationSetting");
#pragma warning restore VSTHRD103 // Call async methods when in an async method
            var blockchainSettingsRow = tableData.FirstOrDefault(e => e.PartitionKey == "v1" && e.RowKey == "Blockchain.Settings");
            var blockchainSettingsValue = JObject.Parse(blockchainSettingsRow.Properties.First().Value.StringValue).SelectToken("Value").ToString();
            return this.GetBlockchainSettings(accountAddress, accountKey, rpcEndPoint, contractFactory, blockchainSettingsValue);
        }

        private BlockchainConnectionSettings GetBlockchainSettings(string accountAddress, string accountKey, string rpcEndPoint, string contractFactoryAddress, string blockchainSettingsValue)
        {
            var blockchainConnectionSettings = JsonConvert.DeserializeObject<BlockchainConnectionSettings>(blockchainSettingsValue);
            blockchainConnectionSettings.EthereumAccountAddress = accountAddress;
            blockchainConnectionSettings.EthereumAccountKey = accountKey;
            blockchainConnectionSettings.RpcEndpoint = rpcEndPoint;
            blockchainConnectionSettings.ContractFactoryContractAddress = contractFactoryAddress;
            return blockchainConnectionSettings;
        }

        private async Task<EthereumClient> GetClientAsync(string keyPrefix = "")
        {
            BlockchainConnectionSettings blockchainSettings = null;
            var key = string.IsNullOrWhiteSpace(keyPrefix) ? this.DefaultKeyPrefix : keyPrefix;
            var client = Clients.GetOrAdd(key, k =>
            {
                blockchainSettings = this.GetBlockchainClientSettingAsync(key).GetAwaiter().GetResult();
                var ethereumClient = new EthereumClient(blockchainSettings);
                return ethereumClient;
            });

            if (!isInitialised)
            {
                foreach (var contract in contractNames)
                {
                    var parameters = new Dictionary<string, object>
                {
                    { "contractName", contract },
                };
                    var details = await client.GetDataAsync<ContractMetadataStruct>(blockchainSettings.ContractFactoryAbi, blockchainSettings.ContractFactoryContractAddress, "GetLatest", parameters).ConfigureAwait(false);
                    contracts.Add(contract, details);
                }

                isInitialised = true;
            }

            return client;
        }
    }
}