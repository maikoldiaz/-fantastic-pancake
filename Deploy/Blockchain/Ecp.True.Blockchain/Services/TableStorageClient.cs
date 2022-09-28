// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableStorageClient.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Blockchain.Entities;
    using Ecp.True.Blockchain.Interfaces;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// The TableStorageClient.
    /// </summary>
    /// <seealso cref="Ecp.True.Blockchain.Interfaces.ITableStorageClient" />
    public class TableStorageClient : ITableStorageClient
    {
        private CloudTableClient tableClient;

        /// <summary>
        /// Initializes the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public void Initialize(string connectionString)
        {
            if (this.tableClient == null)
            {
                var storageAccount = CloudStorageAccount.Parse(connectionString);
                this.tableClient = storageAccount.CreateCloudTableClient();
            }
        }

        /// <summary>
        /// Inserts the or replace asynchronous.
        /// </summary>
        /// <param name="ecpTableEntity">The ecp table entity.</param>
        /// <returns>
        /// The TableResult.
        /// </returns>
        public async Task<TableResult> InsertOrReplaceAsync(EcpTableEntity ecpTableEntity)
        {
            var cloudTable = await this.GetCloudTableAsync().ConfigureAwait(false);
            var insertOperation = TableOperation.InsertOrReplace(ecpTableEntity);
            return await cloudTable.ExecuteAsync(insertOperation).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves the asynchronous.
        /// </summary>
        /// <param name="partitionKey">The partition key.</param>
        /// <param name="rowKey">The row key.</param>
        /// <returns>
        /// The TableResult.
        /// </returns>
        public async Task<TableResult> RetrieveAsync(string partitionKey, string rowKey)
        {
            var cloudTable = await this.GetCloudTableAsync().ConfigureAwait(false);
            TableOperation retrieveOperation = TableOperation.Retrieve<EcpTableEntity>(partitionKey, rowKey);

            return await cloudTable.ExecuteAsync(retrieveOperation).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the cloud table asynchronous.
        /// </summary>
        /// <returns>The CloudTable.</returns>
        private async Task<CloudTable> GetCloudTableAsync()
        {
            var cloudTable = this.tableClient.GetTableReference(Constants.ConfigurationSetting);
            await cloudTable.CreateIfNotExistsAsync().ConfigureAwait(false);

            return cloudTable;
        }
    }
}
