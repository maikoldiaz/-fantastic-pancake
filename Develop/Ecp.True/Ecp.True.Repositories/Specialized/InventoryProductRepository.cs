// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryProductRepository.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Repositories.Specialized
{
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Registration;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The inventory product repository.
    /// </summary>
    public class InventoryProductRepository : Repository<InventoryProduct>, IInventoryProductRepository
    {
        /// <summary>
        /// The SQL data access.
        /// </summary>
        private readonly ISqlDataAccess<InventoryProduct> sqlDataAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryProductRepository"/> class.
        /// </summary>
        /// <param name="sqlDataAccess">The SQL data access.</param>
        public InventoryProductRepository(ISqlDataAccess<InventoryProduct> sqlDataAccess)
            : base(sqlDataAccess)
        {
            this.sqlDataAccess = sqlDataAccess;
        }

        /// <summary>
        /// Gets the inventory products.
        /// </summary>
        /// <value>
        /// The inventory products.
        /// </value>
        private DbSet<InventoryProduct> InventoryProducts => this.sqlDataAccess.EntitySet();

        /// <inheritdoc/>
        public async Task<InventoryProduct> GetLatestInventoryProductAsync(string inventoryProductUniqueId)
        {
            var result = await this.InventoryProducts
                                .Where(x => x.InventoryProductUniqueId == inventoryProductUniqueId)
                                .OrderByDescending(x => x.InventoryProductId)
                                .Select(x => new { x.ProductVolume, x.EventType })
                                .FirstOrDefaultAsync().ConfigureAwait(false);
            return result != null ? new InventoryProduct { ProductVolume = result.ProductVolume, EventType = result.EventType } : null;
        }

        /// <inheritdoc/>
        public async Task<InventoryProduct> GetLatestBlockchainInventoryProductAsync(string inventoryProductUniqueId)
        {
            var result = await this.InventoryProducts
                                .Where(x => x.InventoryProductUniqueId == inventoryProductUniqueId
                                 && x.BlockchainStatus == Entities.Core.StatusType.PROCESSED && x.EventType == EventType.Insert.ToString("G"))
                                .OrderBy(x => x.InventoryProductId)
                                .Select(x => new { x.ProductVolume, x.EventType, x.BlockNumber, x.TransactionHash })
                                .FirstOrDefaultAsync().ConfigureAwait(false);
            return result != null ?
                new InventoryProduct { ProductVolume = result.ProductVolume, EventType = result.EventType, BlockNumber = result.BlockNumber, TransactionHash = result.TransactionHash } : null;
        }
    }
}
