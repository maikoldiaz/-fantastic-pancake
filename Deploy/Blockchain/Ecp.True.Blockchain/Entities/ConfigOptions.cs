// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigOptions.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Blockchain.Entities
{
    using CommandLine;

    /// <summary>
    /// The Options.
    /// </summary>
    public class ConfigOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether [blockchain account].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [blockchain account]; otherwise, <c>false</c>.
        /// </value>
        [Option('o', "blockchainaccount", Required = false, HelpText = "BlockchainAccount is required.")]
        public bool BlockchainAccount { get; set; }

        /// <summary>
        /// Gets or sets the compiled contracts location.
        /// </summary>
        /// <value>
        /// The compiled contracts location.
        /// </value>
        [Option('c', "contractspath", Required = false, HelpText = "Contract json path is required.")]
        public string CompiledContractsLocation { get; set; }

        /// <summary>
        /// Gets or sets the ethereum account address.
        /// </summary>
        /// <value>
        /// The ethereum account address.
        /// </value>
        [Option('a', "address", Required = false, HelpText = "Ethereum Account Address is required.")]
        public string EthereumAccountAddress { get; set; }

        /// <summary>
        /// Gets or sets the ethereum account secret.
        /// </summary>
        /// <value>
        /// The ethereum account secret.
        /// </value>
        [Option('p', "privatekey", Required = false, HelpText = "Ethereum Account Secret is required.")]
        public string EthereumAccountSecret { get; set; }

        /// <summary>
        /// Gets or sets the RPC endpoint.
        /// </summary>
        /// <value>
        /// The RPC endpoint.
        /// </value>
        [Option('r', "rpcendpoint", Required = false, HelpText = "RPC EndPoint is required.")]
        public string RpcEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the localpath.
        /// </summary>
        /// <value>
        /// The localpath.
        /// </value>
        [Option('l', "localpath", Required = false, HelpText = "Local Path is required.")]
        public string Localpath { get; set; }

        /// <summary>
        /// Gets or sets the storage connection string.
        /// </summary>
        /// <value>
        /// The storage connection string.
        /// </value>
        [Option('s', "storageconnstr", Required = false, HelpText = "Storage Account Connection String is required.")]
        public string StorageConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the application identifier.
        /// </summary>
        /// <value>
        /// The application identifier.
        /// </value>
        [Option('i', "appId", Required = false, HelpText = "Application Id is required.")]
        public string AppId { get; set; }

        /// <summary>
        /// Gets or sets the application secret.
        /// </summary>
        /// <value>
        /// The application secret.
        /// </value>
        [Option('q', "appsecret", Required = false, HelpText = "Application secret is required.")]
        public string AppSecret { get; set; }

        /// <summary>
        /// Gets or sets the key vault URL.
        /// </summary>
        /// <value>
        /// The key vault URL.
        /// </value>
        [Option('u', "keyvaulturl", Required = false, HelpText = "Key vault URL is required.")]
        public string KeyVaultUrl { get; set; }

        /// <summary>
        /// Gets or sets the tenant identifier.
        /// </summary>
        /// <value>
        /// The tenant identifier.
        /// </value>
        [Option('t', "tenantId", Required = false, HelpText = "Tenant Id is required.")]
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the resource identifier.
        /// </summary>
        /// <value>
        /// The resource identifier.
        /// </value>
        [Option('n', "resourceId", Required = false, HelpText = "Resource Id is required.")]
        public string ResourceId { get; set; }
    }
}
