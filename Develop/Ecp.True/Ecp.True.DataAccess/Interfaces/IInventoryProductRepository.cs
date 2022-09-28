// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInventoryProductRepository.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Interfaces
{
    using System.Threading.Tasks;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The inventory product repository.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Interfaces.IRepository{Ecp.True.Entities.Registration.InventoryProduct}" />
    public interface IInventoryProductRepository : IRepository<InventoryProduct>
    {
        /// <summary>
        /// Gets the latest inventory product unique identifier.
        /// </summary>
        /// <param name="inventoryProductUniqueId">The inventory product unique identifier.</param>
        /// <returns>The product volume.</returns>
        Task<InventoryProduct> GetLatestInventoryProductAsync(string inventoryProductUniqueId);

        /// <summary>
        /// Gets the latest inventory product unique identifier.
        /// </summary>
        /// <param name="inventoryProductUniqueId">The inventory product unique identifier.</param>
        /// <returns>The product volume.</returns>
        Task<InventoryProduct> GetLatestBlockchainInventoryProductAsync(string inventoryProductUniqueId);
    }
}
