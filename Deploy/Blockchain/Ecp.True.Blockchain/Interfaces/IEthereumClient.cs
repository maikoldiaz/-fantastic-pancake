// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEthereumClient.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Blockchain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Blockchain.Entities;
    using Nethereum.Hex.HexTypes;
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
        /// Creates the smart contract asynchronous.
        /// </summary>
        /// <param name="addressFrom">The address from.</param>
        /// <param name="abi">The abi.</param>
        /// <param name="byteCode">The byte code.</param>
        /// <returns>Returns address of deployed smart contract.</returns>
        Task<string> CreateSmartContractAsync(string addressFrom, string abi, string byteCode);

        /// <summary>
        /// Executes the transaction on smart contract method asynchronous.
        /// </summary>
        /// <param name="abi">The abi.</param>
        /// <param name="smartContractAddress">The smart contract address.</param>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="inputParameters">The input parameters.</param>
        /// <returns>The TransactionReceipt.</returns>
        Task<TransactionReceipt> ExecuteTransactionOnSmartContractMethodAsync(
            string abi,
            string smartContractAddress,
            string functionName,
            Dictionary<string, object> inputParameters);

        /// <summary>
        /// Calls the smart contract method asynchronous.
        /// </summary>
        /// <typeparam name="T">The [T] type of Struct.</typeparam>
        /// <param name="abi">The abi.</param>
        /// <param name="smartContractAddress">The smart contract address.</param>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="inputParameters">The input parameters.</param>
        /// <returns>The [T] type of struct.</returns>
        Task<T> CallSmartContractMethodAsync<T>(string abi, string smartContractAddress, string functionName, Dictionary<string, object> inputParameters)
              where T : class, new();

        /// <summary>
        /// Determines whether [has contract asynchronous] [the specified address].
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>The status.</returns>
        Task<bool> HasContractAsync(string address);
    }
}