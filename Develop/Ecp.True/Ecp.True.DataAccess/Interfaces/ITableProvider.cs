// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITableProvider.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Table;

    /// <summary>
    /// The table provider interface.
    /// </summary>
    public interface ITableProvider
    {
        /// <summary>
        /// Gets the entity based on Identifier.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="rowKey">The row key.</param>
        /// <param name="partitionKey">The partition key.</param>
        /// <returns>
        /// The entity.
        /// </returns>
        Task<TEntity> GetByRowKeyAndPartitionKeyAsync<TEntity>(string rowKey, string partitionKey)
            where TEntity : class;

        /// <summary>
        /// Executes the query asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="tableQueryFilter">The table query filter.</param>
        /// <returns>
        /// The entity.
        /// </returns>
        Task<IEnumerable<TEntity>> ExecuteQueryAsync<TEntity>(TableQuery<TEntity> tableQueryFilter)
        where TEntity : ITableEntity, new();
    }
}