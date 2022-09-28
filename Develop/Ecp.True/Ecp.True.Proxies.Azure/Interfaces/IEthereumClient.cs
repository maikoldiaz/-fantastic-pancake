// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEthereumClient.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Proxies.Azure.Interfaces;
    using Nethereum.RPC.Eth.DTOs;

    /// <summary>
    /// The I Ethereum Client.
    /// </summary>
    public interface IEthereumClient
    {
        /// <summary>
        /// Initializes the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        void Initialize(QuorumProfile profile);

        /// <summary>
        /// Executes the transaction on smart contract method asynchronous.
        /// </summary>
        /// <param name="abi">The abi.</param>
        /// <param name="smartContractAddress">The smart contract address.</param>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="inputParameters">The input parameters.</param>
        /// <returns>Returns transaction hash as string.</returns>
        Task<TransactionReceipt> ExecuteTransactionOnSmartContractMethodAsync(
            string abi,
            string smartContractAddress,
            string functionName,
            IDictionary<string, object> inputParameters);

        /// <summary>
        /// Calls the smart contract method asynchronous.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="abi">The abi.</param>
        /// <param name="smartContractAddress">The smart contract address.</param>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="inputParameters">The input parameters.</param>
        /// <returns>Returns deserialized type.</returns>
        Task<T> CallSmartContractMethodAsync<T>(
            string abi,
            string smartContractAddress,
            string functionName,
            IDictionary<string, object> inputParameters)
        where T : class, new();

        /// <summary>
        /// Calls the method asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of return.</typeparam>
        /// <param name="abi">The abi.</param>
        /// <param name="smartContractAddress">The smart contract address.</param>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="inputParameters">The input parameters.</param>
        /// <returns>The task.</returns>
        Task<T> CallMethodAsync<T>(
            string abi,
            string smartContractAddress,
            string functionName,
            IDictionary<string, object> inputParameters);

        /// <summary>
        /// Gets the paged events asynchronous.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="head">The head.</param>
        /// <param name="tail">The tail.</param>
        /// <returns>
        /// The events page.
        /// </returns>
        Task<IEnumerable<TEvent>> GetEventsAsync<TEvent>(ulong head, ulong tail)
            where TEvent : IEvent, new();

        /// <summary>
        /// Gets the events asynchronous.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="blockNumber">The block number.</param>
        /// <param name="transactionHash">The transaction hash.</param>
        /// <returns>
        /// The events.
        /// </returns>
        Task<IEnumerable<TEvent>> GetEventsAsync<TEvent>(ulong blockNumber, string transactionHash)
            where TEvent : IEvent, new();

        /// <summary>
        /// Gets the events asynchronous.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <returns>
        /// The events.
        /// </returns>
        Task<IEnumerable<TEvent>> GetEventsAsync<TEvent>(string transactionId)
            where TEvent : IEvent, new();

        /// <summary>
        /// Gets the last block number asynchronous.
        /// </summary>
        /// <returns>The last block number.</returns>
        Task<ulong> GetLatestBlockNumberAsync();

        /// <summary>
        /// Gets the block asynchronous.
        /// </summary>
        /// <param name="blockNumber">The block number.</param>
        /// <returns>The block.</returns>
        Task<BlockWithTransactionHashes> GetBlockAsync(ulong blockNumber);
    }
}
