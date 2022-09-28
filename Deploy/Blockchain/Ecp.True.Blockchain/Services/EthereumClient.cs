// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EthereumClient.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Blockchain.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Numerics;
    using System.Threading.Tasks;
    using Ecp.True.Blockchain.Entities;
    using Ecp.True.ExceptionHandling.Core;
    using Ecp.True.ExceptionHandling.Entities;

    using Nethereum.Hex.HexTypes;
    using Nethereum.RPC.Eth.DTOs;
    using Nethereum.Web3;
    using Nethereum.Web3.Accounts;

    using BlockchainConstants = Ecp.True.Blockchain.Constants;

    /// <summary>
    /// The Ethereum Client.
    /// </summary>
    /// <seealso cref="Ecp.True.Proxies.Azure.IEthereumClient" />
    [ExcludeFromCodeCoverage]
    public class EthereumClient : IEthereumClient
    {
        /// <summary>
        /// The function not found.
        /// </summary>
        private readonly string functionNotFound = BlockchainConstants.FunctionNotFound;

        /// <summary>
        /// The invalid arguments.
        /// </summary>
        private readonly string invalidArguments = BlockchainConstants.InvalidArguments;

        /// <summary>
        /// The argument null.
        /// </summary>
        private readonly string argumentNull = BlockchainConstants.ArgumentNull;

        /// <summary>
        /// The retry policy factory.
        /// </summary>
        private readonly IRetryPolicyFactory retryPolicyFactory;

        /// <summary>
        /// The retry handler.
        /// </summary>
        private readonly IRetryHandler retryHandler;

        /// <summary>
        /// The web3.
        /// </summary>
        private Web3 web3;

        /// <summary>
        /// The profile.
        /// </summary>
        private QuorumProfile profile;

        /// <summary>
        /// The retry policy.
        /// </summary>
        private IRetryPolicy retryPolicy;

        /// <summary>
        /// Initializes a new instance of the <see cref="EthereumClient" /> class.
        /// </summary>
        /// <param name="retryPolicyFactory">The retry policy factory.</param>
        /// <param name="retryHandler">The retry handler.</param>
        public EthereumClient(IRetryPolicyFactory retryPolicyFactory, IRetryHandler retryHandler)
        {
            this.retryPolicyFactory = retryPolicyFactory;
            this.retryHandler = retryHandler;
        }

        /// <summary>
        /// Initializes the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public void Initialize(QuorumProfile profile)
        {
            if (this.web3 == null && profile != null)
            {
                this.profile = profile;
                var account = new Account(profile.PrivateKey);
                this.web3 = new Web3(account, url: profile.RpcEndpoint);
            }

            var retrySettings = new RetrySettings
            {
                RetryCount = 3,
                RetryIntervalInSeconds = 3,
                RetryStrategy = RetryStrategy.Exponential,
            };
            this.retryPolicy = this.retryPolicyFactory.GetRetryPolicy("Blockchain", retrySettings, this.retryHandler, false);
        }

        /// <summary>
        /// Creates the smart contract asynchronous.
        /// </summary>
        /// <param name="addressFrom">The address from.</param>
        /// <param name="abi">The abi.</param>
        /// <param name="byteCode">The byte code.</param>
        /// <returns>
        /// Returns address of deployed smart contract.
        /// </returns>
        public async Task<string> CreateSmartContractAsync(string addressFrom, string abi, string byteCode)
        {
            return await this.retryPolicy.ExecuteWithRetryAsync(async () =>
            {
                var abiDeserialiser = new Nethereum.ABI.JsonDeserialisation.ABIDeserialiser();
                var contract = abiDeserialiser.DeserialiseContract(abi);
                Dictionary<string, object> inputParameters = new Dictionary<string, object>();
                if (contract?.Constructor == null)
                {
                    throw new EthereumException(BlockchainConstants.FunctionNotFound);
                }

                var arguments = ConstructInputParameters(inputParameters);

                if (arguments == null)
                {
                    throw new EthereumException(BlockchainConstants.ArgumentNull);
                }

                var transactionInput = await this.GetTransactionInputAsync(this.web3.Eth.DeployContract.GetData(byteCode, abi, arguments), this.profile.Address).ConfigureAwait(false);

                transactionInput.Nonce = await this.web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(this.profile.Address, BlockParameter.CreatePending()).ConfigureAwait(false);

                var hash = await this.web3.Eth.TransactionManager.SendTransactionAsync(transactionInput).ConfigureAwait(false);
                var receipt = await this.web3.TransactionManager.TransactionReceiptService.PollForReceiptAsync(hash).ConfigureAwait(false);

                return receipt.ContractAddress;
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes the transaction on smart contract method asynchronous.
        /// </summary>
        /// <param name="abi">The abi.</param>
        /// <param name="smartContractAddress">The smart contract address.</param>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="inputParameters">The input parameters.</param>
        /// <returns>The TransactionReceipt.</returns>
        public async Task<TransactionReceipt> ExecuteTransactionOnSmartContractMethodAsync(
            string abi,
            string smartContractAddress,
            string functionName,
            Dictionary<string, object> inputParameters)
        {
            return await this.retryPolicy.ExecuteWithRetryAsync(async () =>
            {
                var contract = this.web3.Eth.GetContract(abi, smartContractAddress);

                var function = contract.ContractBuilder.ContractABI.Functions
                    .FirstOrDefault(f => f.Name == functionName);

                if (function == null)
                {
                    throw new EthereumException(this.functionNotFound);
                }

                var functionParameters = contract.ContractBuilder.GetFunctionBuilder(functionName)?.FunctionABI.InputParameters;

                if (inputParameters == null || functionParameters?.Length != inputParameters.Count)
                {
                    throw new EthereumException(this.invalidArguments);
                }

                var arguments = ConstructInputParameters(inputParameters);

                if (arguments == null)
                {
                    throw new EthereumException(this.argumentNull);
                }

                var transactionInput = await this.GetTransactionInputAsync(contract.GetFunction(functionName).GetData(arguments), this.profile.Address, contract.Address).ConfigureAwait(false);

                return await this.web3.Eth.TransactionManager.SendTransactionAndWaitForReceiptAsync(transactionInput, null).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Calls the smart contract method asynchronous.
        /// </summary>
        /// <typeparam name="T">The [T] type of Struct.</typeparam>
        /// <param name="abi">The abi.</param>
        /// <param name="smartContractAddress">The smart contract address.</param>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="inputParameters">The input parameters.</param>
        /// <returns>The [T] type of struct.</returns>
        public async Task<T> CallSmartContractMethodAsync<T>(
            string abi,
            string smartContractAddress,
            string functionName,
            Dictionary<string, object> inputParameters)
           where T : class, new()
        {
            return await this.retryPolicy.ExecuteWithRetryAsync(async () =>
            {
                var contract = this.web3.Eth.GetContract(abi, smartContractAddress);

                var function = contract.ContractBuilder.ContractABI.Functions
                    .FirstOrDefault(f => f.Name == functionName);

                if (function == null)
                {
                    throw new EthereumException(this.functionNotFound);
                }

                var functionParameters = contract.ContractBuilder.GetFunctionBuilder(functionName)?.FunctionABI.InputParameters;

                if (inputParameters == null || functionParameters?.Length != inputParameters.Count)
                {
                    throw new EthereumException(BlockchainConstants.ParametersMismatch);
                }

                var arguments = ConstructInputParameters(inputParameters);

                if (arguments == null)
                {
                    throw new EthereumException(this.argumentNull);
                }

                var functionToCall = contract.GetFunction(functionName);
                var result = await functionToCall.CallDeserializingToObjectAsync<T>(arguments).ConfigureAwait(false);

                return result;
            }).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<bool> HasContractAsync(string address)
        {
            return await this.retryPolicy.ExecuteWithRetryAsync(async () =>
            {
                var code = await this.web3.Eth.GetCode.SendRequestAsync(address).ConfigureAwait(false);
                return code != "0x";
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Constructs the input parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Returns array of object containing arguments.</returns>
        private static object[] ConstructInputParameters(Dictionary<string, object> parameters)
        {
            var args = parameters.Values.ToArray<object>();
            return args;
        }

        /// <summary>
        /// Gets the transaction input asynchronous.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="addressFrom">The address from.</param>
        /// <param name="addressTo">The address to.</param>
        /// <param name="gas">The gas.</param>
        /// <returns>Returns Transaction input instance.</returns>
        /// <exception cref="Ecp.True.Proxies.Azure.EthereumException">
        /// Throws Ethereum exception.
        /// </exception>
        private Task<TransactionInput> GetTransactionInputAsync(string data, string addressFrom = null, string addressTo = null, HexBigInteger gas = null)
        {
            try
            {
                var input = new TransactionInput(data, gas, addressFrom)
                {
                    To = addressTo,
                };
                input.GasPrice = new HexBigInteger(new BigInteger(0));
                input.Gas = new HexBigInteger(new BigInteger(7000000));

                return Task.FromResult(input);
            }
            catch (Exception e)
            {
                throw new EthereumException(BlockchainConstants.ContractTransactionFailed, e);
            }
        }
    }
}