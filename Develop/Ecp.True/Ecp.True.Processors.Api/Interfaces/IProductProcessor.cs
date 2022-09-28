// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProductProcessor.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Interfaces
{
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;

    /// <summary>
    /// The product processor.
    /// </summary>
    public interface IProductProcessor
    {
        /// <summary>
        /// Creates a new product async.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns>The task.</returns>
        Task<ProductInfo> CreateProductAsync(Product product);

        /// <summary>
        /// Updates a new product async.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <param name="productId">The original product id.</param>
        /// <returns>The task.</returns>
        Task UpdateProductAsync(Product product, string productId);

        /// <summary>
        /// Gets a product by id.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <returns>The task.</returns>
        Task<ProductInfo> GetProductAsync(string productId);

        /// <summary>
        /// Deletes a product by id.
        /// </summary>
        /// <param name="productId">The product Id.</param>
        /// <returns>The task.</returns>
        Task DeleteProductAsync(string productId);
    }
}