// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITableStorageClient.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Blockchain.Entities;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// The ITableStorageClient.
    /// </summary>
    public interface ITableStorageClient
    {
        /// <summary>
        /// Initializes the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        void Initialize(string connectionString);

        /// <summary>
        /// Retrieves the asynchronous.
        /// </summary>
        /// <param name="partitionKey">The partition key.</param>
        /// <param name="rowKey">The row key.</param>
        /// <returns>The TableResult.</returns>
        Task<TableResult> RetrieveAsync(string partitionKey, string rowKey);

        /// <summary>
        /// Inserts the or replace asynchronous.
        /// </summary>
        /// <param name="ecpTableEntity">The ecp table entity.</param>
        /// <returns>The TableResult.</returns>
        Task<TableResult> InsertOrReplaceAsync(EcpTableEntity ecpTableEntity);
    }
}
