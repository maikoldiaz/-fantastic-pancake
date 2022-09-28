// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Arguments.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Blockchain.Entities
{
    /// <summary>
    /// The Arguments.
    /// </summary>
    public static class Arguments
    {
        /// <summary>
        /// Gets or sets a value indicating whether [blockchain account].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [blockchain account]; otherwise, <c>false</c>.
        /// </value>
        public static bool BlockchainAccount { get; set; }

        /// <summary>
        /// Gets or sets the compiled contracts location.
        /// </summary>
        /// <value>
        /// The compiled contracts location.
        /// </value>
        public static string CompiledContractsLocation { get; set; }

        /// <summary>
        /// Gets or sets the ethereum account address.
        /// </summary>
        /// <value>
        /// The ethereum account address.
        /// </value>
        public static string EthereumAccountAddress { get; set; }

        /// <summary>
        /// Gets or sets the ethereum account secret.
        /// </summary>
        /// <value>
        /// The ethereum account secret.
        /// </value>
        public static string EthereumAccountSecret { get; set; }

        /// <summary>
        /// Gets or sets the RPC endpoint.
        /// </summary>
        /// <value>
        /// The RPC endpoint.
        /// </value>
        public static string RpcEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the localpath.
        /// </summary>
        /// <value>
        /// The localpath.
        /// </value>
        public static string Localpath { get; set; }

        /// <summary>
        /// Gets or sets the storage connection string.
        /// </summary>
        /// <value>
        /// The storage connection string.
        /// </value>
        public static string StorageConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the application identifier.
        /// </summary>
        /// <value>
        /// The application identifier.
        /// </value>
        public static string AppId { get; set; }

        /// <summary>
        /// Gets or sets the application secret.
        /// </summary>
        /// <value>
        /// The application secret.
        /// </value>
        public static string AppSecret { get; set; }

        /// <summary>
        /// Gets or sets the key vault URL.
        /// </summary>
        /// <value>
        /// The key vault URL.
        /// </value>
        public static string KeyVaultUrl { get; set; }

        /// <summary>
        /// Gets or sets the tenant identifier.
        /// </summary>
        /// <value>
        /// The tenant identifier.
        /// </value>
        public static string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the resource identifier.
        /// </summary>
        /// <value>
        /// The resource identifier.
        /// </value>
        public static string ResourceId { get; set; }
    }
}
