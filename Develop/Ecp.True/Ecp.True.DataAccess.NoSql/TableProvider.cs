// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableProvider.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.NoSql
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Ecp.True.Core.Attributes;
    using Ecp.True.DataAccess.Interfaces;
    using Microsoft.Azure.Cosmos.Table;

    using Newtonsoft.Json;

    /// <summary>
    /// The table provider.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    [IoCRegistration(IoCLifetime.ContainerControlled)]
    [ExcludeFromCodeCoverage]
    public class TableProvider : ITableProvider
    {
        /// <summary>
        /// The connection factory.
        /// </summary>
        private readonly IConnectionFactory connectionFactory;

        /// <summary>
        /// The table.
        /// </summary>
        private CloudTable table;

        /// <summary>
        /// The initialized.
        /// </summary>
        private bool initialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableProvider"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        public TableProvider(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TEntity>> ExecuteQueryAsync<TEntity>(TableQuery<TEntity> tableQueryFilter)
            where TEntity : ITableEntity, new()
        {
            if (!this.initialized)
            {
                await this.InitializeAsync<TEntity>().ConfigureAwait(false);
            }

            return this.table.ExecuteQuery(tableQueryFilter);
        }

        /// <inheritdoc />
        public async Task<TEntity> GetByRowKeyAndPartitionKeyAsync<TEntity>(string rowKey, string partitionKey)
            where TEntity : class
        {
            if (!this.initialized)
            {
                await this.InitializeAsync<TEntity>().ConfigureAwait(false);
            }

            var retrieveOperation = TableOperation.Retrieve<TrueTableEntity>(partitionKey, rowKey);
            var tableEntity = await this.table.ExecuteAsync(retrieveOperation).ConfigureAwait(false);

            var trueTableEntity = (TrueTableEntity)tableEntity.Result;
            return JsonConvert.DeserializeObject<TEntity>(trueTableEntity.Value);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns>Status indicating if the table was initialized.</returns>
        private async Task<bool> InitializeAsync<TEntity>()
        {
            if (this.table == null)
            {
                var storageAccount = CloudStorageAccount.Parse(this.connectionFactory.NoSqlConnectionString);

                var tableClient = storageAccount.CreateCloudTableClient();
                this.table = tableClient.GetTableReference(typeof(TEntity).Name);

                this.initialized = await this.table.CreateIfNotExistsAsync().ConfigureAwait(false);
            }

            return this.initialized;
        }
    }
}