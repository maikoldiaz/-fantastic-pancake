// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductProcessor.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Api.Specifications;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The Product processor.
    /// </summary>
    public class ProductProcessor : IProductProcessor
    {
        /// <summary>
        /// The product repository.
        /// </summary>
        private readonly IRepository<Product> repository;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductProcessor"/> class.
        /// </summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public ProductProcessor(IUnitOfWorkFactory unitOfWorkFactory)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));

            this.unitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.repository = this.unitOfWork.CreateRepository<Product>();
        }

        /// <inheritdoc />
        public async Task UpdateProductAsync(Product product, string productId)
        {
            ArgumentValidators.ThrowIfNull(product, nameof(product));

            var existing = await this.GetExistingAsync(productId).ConfigureAwait(false);

            if (existing is null)
            {
                throw new KeyNotFoundException(Constants.ProductDoesNotExist);
            }

            UpdateProductNameAndState(product, existing);

            this.repository.Update(existing);

            await this.unitOfWork.SaveAsync(CancellationToken.None)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<ProductInfo> CreateProductAsync(Product product)
        {
            ArgumentValidators.ThrowIfNull(product, nameof(product));
            var existing = await this.GetExistingAsync(product.ProductId).ConfigureAwait(false);

            if (existing is { })
            {
                var info = new ProductInfo(existing, false) { ErrorMessage = Constants.ProductAlreadyExists };
                return info;
            }

            this.repository.Insert(product);

            await this.unitOfWork.SaveAsync(CancellationToken.None)
                .ConfigureAwait(false);

            return new ProductInfo(product, false);
        }

        /// <inheritdoc />
        public async Task<ProductInfo> GetProductAsync(string productId)
        {
            var product = await this.GetExistingAsync(productId).ConfigureAwait(false);

            return new ProductInfo(product, await this.IsEditableAsync(product).ConfigureAwait(false));
        }

        /// <inheritdoc />
        public async Task DeleteProductAsync(string productId)
        {
            var existing = await this.GetExistingAsync(productId).ConfigureAwait(false);

            if (existing is null)
            {
                throw new KeyNotFoundException(Constants.ProductDoesNotExist);
            }

            var isEditable = await this.IsEditableAsync(existing).ConfigureAwait(false);

            if (!isEditable)
            {
                throw new KeyNotFoundException(Constants.ProductWithMovements);
            }

            try
            {
                this.repository.Delete(existing);
                await this.unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
            }
            catch (DbUpdateException)
            {
                throw new KeyNotFoundException(Constants.ProductWithConfigurations);
            }
        }

        /// <summary>
        /// Updates the existing product.
        /// </summary>
        /// <param name="product">The updated product.</param>
        /// <param name="existing">The existing product.</param>
        private static void UpdateProductNameAndState(Product product, Product existing)
        {
            existing.Name = product.Name;
            existing.IsActive = product.IsActive;
            existing.RowVersion = product.RowVersion;
        }

        /// <summary>
        /// Gets a task that queries whether a product is editable.
        /// </summary>
        /// <param name="existing">The product.</param>
        /// <returns>The task.</returns>
        private async Task<bool> IsEditableAsync(Product existing)
        {
            var movementRepo = this.unitOfWork.CreateRepository<Movement>();
            var mappingRepo = this.unitOfWork.CreateRepository<StorageLocationProductMapping>();

            var sourcesWithMovements = await movementRepo
                .QueryAllAsync(m => m.MovementSource.SourceProductId == existing.ProductId)
                .ConfigureAwait(false);

            var destinationWithMovements = await movementRepo
                .QueryAllAsync(m => m.MovementDestination.DestinationProductId == existing.ProductId)
                .ConfigureAwait(false);

            var mappings = await mappingRepo.QueryAllAsync(m => m.ProductId == existing.ProductId)
                .ConfigureAwait(false);

            return !(sourcesWithMovements.Any() || destinationWithMovements.Any() || mappings.Any());
        }

        /// <summary>
        /// Gets the existing product async.
        /// </summary>
        /// <param name="productId">The updated product.</param>
        /// <returns>The existing product.</returns>
        private async Task<Product> GetExistingAsync(string productId)
        {
            var spec = new ProductSpecification(productId);
            return await this.repository.SingleOrDefaultAsync(spec).ConfigureAwait(false);
        }
    }
}