// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AzureClientFactory.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Blockchain.Services
{
    using System;
    using Ecp.True.Blockchain.Entities;
    using Ecp.True.Blockchain.Interfaces;

    /// <summary>
    /// The AzureClientFactory.
    /// </summary>
    /// <seealso cref="Ecp.True.Blockchain.Interfaces.IAzureClientFactory" />
    public class AzureClientFactory : IAzureClientFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureClientFactory" /> class.
        /// </summary>
        /// <param name="ethereumClient">The ethereum client.</param>
        /// <param name="keyVaultSecretClient">The key vault secret client.</param>
        /// <param name="tableStorageClient">The table storage client.</param>
        public AzureClientFactory(IEthereumClient ethereumClient, IKeyVaultSecretClient keyVaultSecretClient, ITableStorageClient tableStorageClient)
        {
            this.EthereumClient = ethereumClient;
            this.KeyVaultSecretClient = keyVaultSecretClient;
            this.TableStorageClient = tableStorageClient;
        }

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
        public void Initialize(AzureConfiguration azureConfiguration)
        {
            if (azureConfiguration == null)
            {
                throw new ArgumentNullException(nameof(azureConfiguration));
            }

            if (!string.IsNullOrEmpty(azureConfiguration.StorageConnectionString))
            {
                this.TableStorageClient.Initialize(Arguments.StorageConnectionString);
            }

            if (!string.IsNullOrEmpty(azureConfiguration.AppId) && !string.IsNullOrEmpty(azureConfiguration.AppSecret))
            {
                this.KeyVaultSecretClient.Initialize(azureConfiguration.AppId, azureConfiguration.AppSecret);
            }

            if (azureConfiguration.QuorumProfile != null)
            {
                this.EthereumClient.Initialize(azureConfiguration.QuorumProfile);
            }
        }
    }
}
