// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainSettings.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Configuration
{
    /// <summary>
    /// The Blockchain Configuration.
    /// </summary>
    public class BlockchainSettings
    {
        /// <summary>
        /// Gets or sets the RPC endpoint.
        /// </summary>
        /// <value>
        /// The RPC endpoint.
        /// </value>
        public string RpcEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the ethereum account address.
        /// </summary>
        /// <value>
        /// The ethereum account address.
        /// </value>
        public string EthereumAccountAddress { get; set; }

        /// <summary>
        /// Gets or sets the ethereum account key.
        /// </summary>
        /// <value>
        /// The ethereum account key.
        /// </value>
        public string EthereumAccountKey { get; set; }

        /// <summary>
        /// Gets or sets the contract factory abi.
        /// </summary>
        /// <value>
        /// The contract factory abi.
        /// </value>
        public string ContractFactoryAbi { get; set; }

        /// <summary>
        /// Gets or sets the contract factory contract address.
        /// </summary>
        /// <value>
        /// The contract factory contract address.
        /// </value>
        public string ContractFactoryContractAddress { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the tenant identifier.
        /// </summary>
        /// <value>
        /// The tenant identifier.
        /// </value>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the resource identifier.
        /// </summary>
        /// <value>
        /// The resource identifier.
        /// </value>
        public string ResourceId { get; set; }
    }
}
