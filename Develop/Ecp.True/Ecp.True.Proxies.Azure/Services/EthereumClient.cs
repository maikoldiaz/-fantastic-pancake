// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EthereumClient.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Azure
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net.Http;
    using System.Numerics;
    using System.Text;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.ExceptionHandling.Core;
    using Ecp.True.ExceptionHandling.Entities;
    using Ecp.True.Proxies.Azure.Interfaces;
    using Ecp.True.Proxies.Azure.Services;
    using Nethereum.Contracts;
    using Nethereum.Hex.HexTypes;
    using Nethereum.RPC.Eth.DTOs;
    using Nethereum.RPC.NonceServices;
    using Nethereum.Web3;
    using Nethereum.Web3.Accounts;

    using Org.BouncyCastle.Crypto.Digests;

    using Constants = Ecp.True.Core.Constants;

    /// <summary>
    /// The Ethereum Client.
    /// </summary>
    /// <seealso cref="Ecp.True.Proxies.Azure.IEthereumClient" />
    [ExcludeFromCodeCoverage]
    [IoCRegistration(IoCLifetime.ContainerControlled)]
    public class EthereumClient : IEthereumClient
    {
        /// <summary>
        /// The function not found.
        /// </summary>
        private readonly string functionNotFound = Constants.FunctionNotFound;

        /// <summary>
        /// The parameters mismatch.
        /// </summary>
        private readonly string parametersMismatch = Constants.ParametersMismatch;

        /// <summary>
        /// The retry policy factory.
        /// </summary>
        private readonly IRetryPolicyFactory retryPolicyFactory;

        /// <summary>
        /// The retry handler.
        /// </summary>
        private readonly IRetryHandler retryHandler;

        /// <summary>
        /// The HTTP client factory.
        /// </summary>
        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// The token provider.
        /// </summary>
        private readonly ITokenProvider tokenProvider;

        /// <summary>
        /// The web3.
        /// </summary>
        private Web3 web3;

        /// <summary>
        /// The profile.
        /// </summary>
        private QuorumProfile profile;

        /// <summary>
        /// The nonce service.
        /// </summary>
        private InMemoryNonceService nonceService;

        /// <summary>
        /// The retry policy.
        /// </summary>
        private IRetryPolicy retryPolicy;

        /// <summary>
        /// Initializes a new instance of the <see cref="EthereumClient" /> class.
        /// </summary>
        /// <param name="retryPolicyFactory">The retry policy factory.</param>
        /// <param name="retryHandler">The retry handler.</param>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        /// <param name="tokenProvider">The token provider.</param>
        public EthereumClient(IRetryPolicyFactory retryPolicyFactory, IRetryHandler retryHandler, IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            this.retryPolicyFactory = retryPolicyFactory;
            this.retryHandler = retryHandler;
            this.httpClientFactory = httpClientFactory;
            this.tokenProvider = tokenProvider;
        }

        /// <inheritdoc/>
        public void Initialize(QuorumProfile profile)
        {
            ArgumentValidators.ThrowIfNull(profile, nameof(profile));
            if (this.web3 != null)
            {
                return;
            }

            this.profile = profile;
            var account = new Account(profile.PrivateKey);

            this.web3 = new Web3(account, new TrueRpcClient(profile, this.httpClientFactory, this.tokenProvider));
            this.nonceService = new InMemoryNonceService(account.Address, this.web3.Client);

            var retrySettings = new RetrySettings
            {
                RetryCount = 3,
                RetryIntervalInSeconds = 10,
                RetryStrategy = RetryStrategy.Exponential,
            };

            this.retryPolicy = this.retryPolicyFactory.GetRetryPolicy("Blockchain", retrySettings, this.retryHandler);
        }

        /// <inheritdoc/>
        public async Task<TransactionReceipt> ExecuteTransactionOnSmartContractMethodAsync(
            string abi,
            string smartContractAddress,
            string functionName,
            IDictionary<string, object> inputParameters)
        {
            return await this.retryPolicy.ExecuteWithRetryAsync(async () =>
            {
                var function = this.GetFunction(abi, smartContractAddress, functionName, inputParameters);
                var data = function.GetData(inputParameters.Values.ToArray());

                var transactionInput = await this.GetTransactionInputAsync(data, this.profile.Address, smartContractAddress).ConfigureAwait(false);
                var receipt = await this.web3.Eth.TransactionManager.SendTransactionAndWaitForReceiptAsync(transactionInput, null).ConfigureAwait(false);

                if (receipt.Status.ToUlong() == 0)
                {
                    var message = await this.web3.Eth.GetContractTransactionErrorReason.SendRequestAsync(receipt.TransactionHash).ConfigureAwait(false);
                    throw new EthereumRequireException(message);
                }

                return receipt;
            }).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<T> CallSmartContractMethodAsync<T>(
            string abi,
            string smartContractAddress,
            string functionName,
            IDictionary<string, object> inputParameters)
            where T : class, new()
        {
            return await this.retryPolicy.ExecuteWithRetryAsync(async () =>
            {
                var function = this.GetFunction(abi, smartContractAddress, functionName, inputParameters);
                return await function.CallDeserializingToObjectAsync<T>(inputParameters.Values.ToArray()).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<T> CallMethodAsync<T>(
            string abi,
            string smartContractAddress,
            string functionName,
            IDictionary<string, object> inputParameters)
        {
            return await this.retryPolicy.ExecuteWithRetryAsync(async () =>
            {
                var function = this.GetFunction(abi, smartContractAddress, functionName, inputParameters);
                return await function.CallAsync<T>(inputParameters.Values.ToArray()).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TEvent>> GetEventsAsync<TEvent>(ulong blockNumber, string transactionHash)
            where TEvent : IEvent, new()
        {
            var tail = blockNumber;
            var eventLogs = await this.GetEventsAsync<TEvent>(blockNumber, tail).ConfigureAwait(false);
            return eventLogs.Where(e => e.TransactionHash == transactionHash);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TEvent>> GetEventsAsync<TEvent>(ulong head, ulong tail)
            where TEvent : IEvent, new()
        {
            var fromBlock = new BlockParameter(head);
            var toBlock = new BlockParameter(tail);
            var eventHandler = this.web3.Eth.GetEvent<TEvent>();
            var eventFilter = eventHandler.CreateFilterInput(fromBlock: fromBlock, toBlock: toBlock);

            var eventLogs = await eventHandler.GetAllChanges(eventFilter).ConfigureAwait(false);

            for (var i = 0; i < eventLogs.Count; i++)
            {
                var log = eventLogs[i];
                log.Event.BlockNumber = log.Log.BlockNumber.ToUlong();
                log.Event.TransactionHash = log.Log.TransactionHash;
                log.Event.Address = log.Log.Address;
            }

            return eventLogs.Select(e => e.Event);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TEvent>> GetEventsAsync<TEvent>(string transactionId)
            where TEvent : IEvent, new()
        {
            var eventHandler = this.web3.Eth.GetEvent<TEvent>();
            var eventFilter = eventHandler.CreateFilterInput<byte[]>(GenerateIndex(transactionId));
            var eventLogs = await eventHandler.GetAllChanges(eventFilter).ConfigureAwait(false);
            foreach (var eventLog in eventLogs)
            {
                eventLog.Event.BlockNumber = eventLog.Log.BlockNumber.ToUlong();
                eventLog.Event.TransactionHash = eventLog.Log.TransactionHash;
            }

            return eventLogs.Select(e => e.Event);
        }

        /// <inheritdoc/>
        public async Task<ulong> GetLatestBlockNumberAsync()
        {
            var lastBlock = await this.web3.Eth.Blocks.GetBlockNumber.SendRequestAsync().ConfigureAwait(false);
            return lastBlock.ToUlong();
        }

        /// <inheritdoc/>
        public Task<BlockWithTransactionHashes> GetBlockAsync(ulong blockNumber)
        {
            return this.web3.Eth.Blocks.GetBlockWithTransactionsHashesByNumber.SendRequestAsync(new BlockParameter(blockNumber));
        }

        private static byte[] GenerateIndex(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            var digest = new KeccakDigest(256);
            var output = new byte[digest.GetDigestSize()];
            digest.BlockUpdate(bytes, 0, bytes.Length);
            digest.DoFinal(output, 0);
            return output;
        }

        private Function GetFunction(
            string abi,
            string smartContractAddress,
            string functionName,
            IDictionary<string, object> inputParameters)
        {
            var contract = this.web3.Eth.GetContract(abi, smartContractAddress);
            var function = contract.ContractBuilder.ContractABI.Functions.FirstOrDefault(f => f.Name == functionName);

            if (function == null)
            {
                throw new EthereumException(this.functionNotFound);
            }

            var functionParameters = contract.ContractBuilder.GetFunctionBuilder(functionName)?.FunctionABI.InputParameters;

            if (inputParameters == null || functionParameters?.Length != inputParameters.Count)
            {
                throw new EthereumException(this.parametersMismatch);
            }

            return contract.GetFunction(functionName);
        }

        private async Task<TransactionInput> GetTransactionInputAsync(string data, string from, string to)
        {
            var gasPrice = new HexBigInteger(new BigInteger(0));
            var value = new HexBigInteger(new BigInteger(0));
            var gas = new HexBigInteger(new BigInteger(7000000));

            var input = new TransactionInput(data, to, from, gas, gasPrice, value);
            input.Nonce = await this.nonceService.GetNextNonceAsync().ConfigureAwait(false);

            return input;
        }
    }
}