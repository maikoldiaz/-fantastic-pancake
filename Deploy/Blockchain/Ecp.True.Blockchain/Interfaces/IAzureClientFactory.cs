// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAzureClientFactory.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Blockchain.Interfaces
{
    using Ecp.True.Blockchain.Entities;

    /// <summary>
    /// The IAzureClientFactory.
    /// </summary>
    public interface IAzureClientFactory
    {
        /// <summary>
        /// Gets the ethereum client.
        /// </summary>
        /// <value>
        /// The ethereum client.
        /// </value>
        public IEthereumClient EthereumClient { get; }

        /// <summary>
        /// Gets the key vault secret client.
        /// </summary>
        /// <value>
        /// The key vault secret client.
        /// </value>
        public IKeyVaultSecretClient KeyVaultSecretClient { get; }

        /// <summary>
        /// Gets the table storage client.
        /// </summary>
        /// <value>
        /// The table storage client.
        /// </value>
        public ITableStorageClient TableStorageClient { get; }

        /// <summary>
        /// Initializes the specified azure configuration.
        /// </summary>
        /// <param name="azureConfiguration">The azure configuration.</param>
        void Initialize(AzureConfiguration azureConfiguration);
    }
}
