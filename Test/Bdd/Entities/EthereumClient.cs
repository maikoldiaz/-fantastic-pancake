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

namespace Ecp.True.Bdd.Tests.Entities
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;

    using Nethereum.Web3;
    using Nethereum.Web3.Accounts;

    using NLog;

    [ExcludeFromCodeCoverage]
    public class EthereumClient
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Web3 web3;

        private readonly BlockchainConnectionSettings profile;

        public EthereumClient(BlockchainConnectionSettings profile)
        {
            if (this.web3 == null && profile != null)
            {
                this.profile = profile;
                var account = new Account(profile.EthereumAccountKey);
                this.web3 = new Web3(account, url: profile.RpcEndpoint);
            }
        }

        public async Task<T> GetDataAsync<T>(string abi, string address, string functionName, Dictionary<string, object> functionInput)
            where T : class, new()
        {
            var contract = this.web3.Eth.GetContract(abi, address);
            var arguments = functionInput?.Values?.ToArray();
            var function = contract.GetFunction(functionName);
            var result = await function.CallDeserializingToObjectAsync<T>(arguments).ConfigureAwait(false);
            return result;
        }
    }
}