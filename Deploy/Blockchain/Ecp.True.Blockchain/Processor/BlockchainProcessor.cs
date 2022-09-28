// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainProcessor.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Blockchain.Processor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Ecp.True.Blockchain.Entities;
    using Ecp.True.Blockchain.Helpers;
    using Ecp.True.Blockchain.Interfaces;
    using Microsoft.Extensions.Options;
    using Nethereum.Hex.HexConvertors.Extensions;
    using Nethereum.Signer;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The BlockchainProcessor.
    /// </summary>
    /// <seealso cref="Ecp.True.Blockchain.Interfaces.IBlockchainProcessor" />
    public class BlockchainProcessor : ProcessorBase, IBlockchainProcessor
    {
        /// <summary>
        /// The partition key.
        /// </summary>
        private const string PartitionKey = "v1";

        /// <summary>
        /// The row key.
        /// </summary>
        private const string RowKey = "Blockchain.Settings";

        /// <summary>
        /// The master contract.
        /// </summary>
        private const string MasterContract = "ContractFactory";

        /// <summary>
        /// The block chain settings.
        /// </summary>
        private readonly JObject blockChainSettings;

        /// <summary>
        /// The azure client factory.
        /// </summary>
        private readonly IAzureClientFactory azureClientFactory;

        /// <summary>
        /// The options.
        /// </summary>
        private readonly IOptions<ConfigOptions> options;

        /// <summary>
        /// The string builder.
        /// </summary>
        private readonly StringBuilder stringBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockchainProcessor" /> class.
        /// </summary>
        /// <param name="azureClientFactory">The azure client factory.</param>
        /// <param name="options">The options.</param>
        public BlockchainProcessor(IAzureClientFactory azureClientFactory, IOptions<ConfigOptions> options)
        {
            this.azureClientFactory = azureClientFactory;
            this.options = options;
            this.stringBuilder = new StringBuilder();

            this.blockChainSettings = new JObject
            {
                { "EthereumAccountAddress", "#Ethereum.AccountAddress#" },
                { "EthereumAccountKey", "#Ethereum.AccountKey#" },
                { "RpcEndpoint", "#Ethereum.RpcEndpoint#" },
                { "ClientId", "#BlockchainClientId#" },
                { "ClientSecret", "#BlockchainClientSecret#" },
            };
        }

        /// <summary>
        /// Processes the asynchronous.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The task.</returns>
        public async Task ProcessAsync(string[] args)
        {
            this.DoInitialize(args, this.options.Value);

            if (Arguments.BlockchainAccount)
            {
                Console.WriteLine($"Initializing keyvault client...");
                this.azureClientFactory.Initialize(new AzureConfiguration(Arguments.AppId, Arguments.AppSecret));
                await this.ProcessBlockchainAccountCreationAsync().ConfigureAwait(false);
            }
            else
            {
                Console.WriteLine($"Initializing table storage client...");
                this.azureClientFactory.Initialize(new AzureConfiguration(Arguments.StorageConnectionString));

                Console.WriteLine($"Initializing keyvault client...");
                this.azureClientFactory.Initialize(new AzureConfiguration(Arguments.AppId, Arguments.AppSecret));

                Console.WriteLine($"Initializing etherum client...");
                var profile = new QuorumProfile
                {
                    Address = Arguments.EthereumAccountAddress,
                    PrivateKey = Arguments.EthereumAccountSecret,
                    PublicKey = string.Empty,
                    RpcEndpoint = Arguments.RpcEndpoint,
                };
                this.azureClientFactory.Initialize(new AzureConfiguration(profile));

                await this.ProcessBlockchainSettingsAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Processes the blockchain account creation asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        private async Task ProcessBlockchainAccountCreationAsync()
        {
            string[] secretsArray = { Arguments.EthereumAccountAddress, Arguments.EthereumAccountSecret };

            var status = await this.azureClientFactory.KeyVaultSecretClient.CheckSecretsAsync(Arguments.KeyVaultUrl, secretsArray).ConfigureAwait(false);

            if (status)
            {
                Console.WriteLine(Constants.KeyVaulePairExists);
            }
            else
            {
                var hexString = EthECKey.GenerateKey().GetPrivateKeyAsBytes().ToHex();
                var account = new Nethereum.Web3.Accounts.Account(hexString);

                await this.azureClientFactory.KeyVaultSecretClient.InsertSecretAsync(Arguments.KeyVaultUrl, Arguments.EthereumAccountAddress, account.Address).ConfigureAwait(false);
                await this.azureClientFactory.KeyVaultSecretClient.InsertSecretAsync(Arguments.KeyVaultUrl, Arguments.EthereumAccountSecret, account.PrivateKey).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Processes the blockchain settings asynchronous.
        /// </summary>
        private async Task ProcessBlockchainSettingsAsync()
        {
            var isMasterContractDeployedFirstTime = false;
            var keyValuePair = await this.GetBlockChainSettingsAsync().ConfigureAwait(false);

            // Get all compiled contracts.
            var compiledContracts = Directory.GetFiles(Arguments.CompiledContractsLocation, "*.json").Where(a => !a.Contains("Migrations", StringComparison.InvariantCulture));

            // Storing it in the local path to access it through the pipeline.
            string path = Arguments.Localpath + @"\Contracts.txt";
            Console.WriteLine($"Contract text file: {path}");

            // Get and deploy master contract if not exists.
            (bool isDeployed, string masterContractAddress, string masterContractAbi, string masterContractJsonPath, string masterContractName) =
                await this.TryDeployMasterAsync(keyValuePair, compiledContracts).ConfigureAwait(false);

            // Get master contract Address from keyvault if the contract already exists.
            isMasterContractDeployedFirstTime = !isDeployed;

            // Ignore master contracts from the list of all contracts.
            compiledContracts = compiledContracts.Except(new List<string> { masterContractJsonPath });

            foreach (var file in compiledContracts)
            {
                var json = JObject.Parse(File.ReadAllText(file));
                var newAbi = json.Root["abi"].ToString();
                var contractName = json.Root["contractName"].ToString();
                var byteCode = json.Root["bytecode"].ToString();

                newAbi = Regex.Replace(newAbi, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");

                var getParameters = new Dictionary<string, object>();
                getParameters.Add("contractName", contractName);

                var contractMetadata = await this.azureClientFactory.EthereumClient.CallSmartContractMethodAsync<ContractMetadataStruct>(
                    masterContractAbi,
                    masterContractAddress,
                    "GetLatest",
                    getParameters).ConfigureAwait(false);

                if (contractMetadata != null && !string.IsNullOrEmpty(contractMetadata.ContractAbi))
                {
                    if (!newAbi.Equals(contractMetadata.ContractAbi, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"ABI differences detected for {contractName} contract.");
                        await this.DeployAndRegisterContractAsync(newAbi, byteCode, contractName, masterContractAbi, masterContractAddress).ConfigureAwait(false);
                    }
                    else
                    {
                        await this.PopulateSettingAndRegisterContractIfNotRegisterToMasterContractAsync(
                            isMasterContractDeployedFirstTime,
                            contractName,
                            contractMetadata.ContractAbi,
                            contractMetadata.ContractAddress,
                            masterContractAbi,
                            masterContractAddress).ConfigureAwait(false);
                    }
                }
                else
                {
                    Console.WriteLine($"{contractName} contract NOT FOUND. Deploying {contractName} contract...");
                    await this.DeployAndRegisterContractAsync(newAbi, byteCode, contractName, masterContractAbi, masterContractAddress).ConfigureAwait(false);
                }
            }

            File.WriteAllText(path, this.stringBuilder.ToString());

            this.blockChainSettings.Add("TenantId", Arguments.TenantId);
            this.blockChainSettings.Add("ResourceId", Arguments.ResourceId);

            await this.UpsertBlockChainSettingToTableAsync().ConfigureAwait(false);
        }

        private async Task PopulateSettingAndRegisterContractIfNotRegisterToMasterContractAsync(
            bool isMasterContractDeployedFirstTime,
            string contractName,
            string contractAbi,
            string contractAddress,
            string masterContractAbi,
            string masterContractAddress)
        {
            // Repopulate existing blockchain settings.
            Console.WriteLine($"No ABI differences detected for {contractName}. Populating key-values for {contractName} contract...");
            if (isMasterContractDeployedFirstTime)
            {
                Console.WriteLine($"Registering {contractName} to Master contract as it deployed for the first time...");
                var parameters = new Dictionary<string, object>
                        {
                            { "contractName", contractName },
                            { "contractAbi", contractAbi },
                            { "contractCreationDate", DateTime.UtcNow.ToTrue().ToString(CultureInfo.CurrentCulture) },
                            { "contractAddress", contractAddress },
                        };

                await this.azureClientFactory.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(
                    masterContractAbi,
                    masterContractAddress,
                    "Register",
                    parameters).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Upserts the block chain setting to table asynchronous.
        /// </summary>
        /// <param name="blockChainSettings">The block chain settings.</param>
        private async Task UpsertBlockChainSettingToTableAsync()
        {
            Console.WriteLine($"Updating blockchain settings to table storage...");
            var value = JsonConvert.SerializeObject(this.blockChainSettings);

            var configurationSetting = new ConfigurationSetting
            {
                Key = RowKey,
                Value = value,
            };

            EcpTableEntity tableEntity = new EcpTableEntity
            {
                RowKey = configurationSetting.Key,
                PartitionKey = PartitionKey,
                Value = JsonConvert.SerializeObject(configurationSetting, Formatting.None)
                .Replace(@"\\r\\n", string.Empty, StringComparison.InvariantCulture)
                .Replace(@" ", string.Empty, StringComparison.InvariantCulture),
            };

            var result = await this.azureClientFactory.TableStorageClient.InsertOrReplaceAsync(tableEntity).ConfigureAwait(false);

            Console.WriteLine($"Status Message : {result.HttpStatusCode}");
        }

        private async Task DeployAndRegisterContractAsync(
           string newAbi,
           string newByteCode,
           string newContractName,
           string masterContractAbi,
           string masterContractAddress)
        {
            Console.WriteLine($"Creating {newContractName} contract...");

            var smartContractAddress = await this.azureClientFactory.EthereumClient.CreateSmartContractAsync(Arguments.EthereumAccountAddress, newAbi, newByteCode).ConfigureAwait(false);
            Console.WriteLine($"The {newContractName} contract is created at {smartContractAddress} address");

            Console.WriteLine($"Registering {newContractName} to Master contract...");
            var parameters = new Dictionary<string, object>
                        {
                            { "contractName", newContractName },
                            { "contractAbi", newAbi },
                            { "contractCreationDate", DateTime.UtcNow.ToTrue().ToString(CultureInfo.CurrentCulture) },
                            { "contractAddress", smartContractAddress },
                        };

            await this.azureClientFactory.EthereumClient.ExecuteTransactionOnSmartContractMethodAsync(masterContractAbi, masterContractAddress, "Register", parameters).ConfigureAwait(false);
        }

        private async Task<(bool isDeployed, string masterContractAddress, string masterContractAbi, string masterContractJsonPath, string masterContractName)>
            TryDeployMasterAsync(IDictionary<string, string> keyValuePair, IEnumerable<string> compiledContracts)
        {
            var masterContractJsonPath = compiledContracts.FirstOrDefault(a => a.Contains(MasterContract, StringComparison.InvariantCultureIgnoreCase));
            var masterContractJson = JObject.Parse(File.ReadAllText(masterContractJsonPath));
            var masterContractAbi = Regex.Replace(masterContractJson.Root["abi"].ToString(), "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
            var masterContractName = masterContractJson.Root["contractName"].ToString();
            var masterContractAddressSecretKey = $"{masterContractName}ContractAddress";
            var masterContractAddress = await this.azureClientFactory.KeyVaultSecretClient.GetSecretAsync(Arguments.KeyVaultUrl, masterContractAddressSecretKey).ConfigureAwait(false);

            keyValuePair.TryGetValue($"{masterContractName}Abi", out var existingAbi);

            var isDeployed = await this.IsMasterContractDeployedAsync(masterContractAddress, masterContractAbi, existingAbi).ConfigureAwait(false);

            if (!isDeployed)
            {
                Console.WriteLine($"{masterContractName} NOT FOUND. Deploying {masterContractName} contract...");
                masterContractAddress = await this.azureClientFactory.EthereumClient.CreateSmartContractAsync(
                    Arguments.EthereumAccountAddress,
                    masterContractAbi,
                    masterContractJson.Root["bytecode"].ToString()).ConfigureAwait(false);
            }
            else
            {
                Console.WriteLine($"{masterContractName} contract FOUND...");
                masterContractAbi = existingAbi;
            }

            this.blockChainSettings.Add($"{masterContractName}Abi", masterContractAbi);
            this.blockChainSettings.Add($"{masterContractName}ByteCode", string.Empty);
            this.blockChainSettings.Add($"{masterContractName}ContractAddress", $"#{masterContractName}Contract.Address#");

            this.stringBuilder.AppendLine($"{masterContractName}{Environment.NewLine}{masterContractAddress}");

            return (isDeployed, masterContractAddress, masterContractAbi, masterContractJsonPath, masterContractName);
        }

        private async Task<bool> IsMasterContractDeployedAsync(string address, string currentAbi, string existingAbi)
        {
            if (string.IsNullOrWhiteSpace(address) || currentAbi != existingAbi)
            {
                return false;
            }

            return await this.azureClientFactory.EthereumClient.HasContractAsync(address).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the block chain settings asynchronous.
        /// </summary>
        /// <returns>The disctionary.</returns>
        private async Task<Dictionary<string, string>> GetBlockChainSettingsAsync()
        {
            Console.WriteLine($"Retrieving Blockchain settings from table storage...");
            var tableResult = await this.azureClientFactory.TableStorageClient.RetrieveAsync(PartitionKey, RowKey).ConfigureAwait(false);

            if (tableResult.Result == null)
            {
                return new Dictionary<string, string>();
            }

            var entity = (EcpTableEntity)tableResult.Result;
            var configurationSetting = JsonConvert.DeserializeObject<ConfigurationSetting>(entity.Value);

            return JsonConvert.DeserializeObject<Dictionary<string, string>>(configurationSetting.Value);
        }
    }
}