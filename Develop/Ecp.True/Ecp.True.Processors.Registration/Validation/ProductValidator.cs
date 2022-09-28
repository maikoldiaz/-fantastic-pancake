// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductValidator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Registration.Validation
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The Product Validator.
    /// </summary>
    /// <typeparam name="T">The Type Param.</typeparam>
    /// <seealso cref="Ecp.True.Processors.Api.Inventory.Validator.Interface.IValidator{T}" />
    public class ProductValidator<T> : Validator<T>
        where T : class
    {
        /// <summary>
        /// The factory.
        /// </summary>
        private readonly IRepositoryFactory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductValidator{T}"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public ProductValidator(IRepositoryFactory factory)
        {
            this.factory = factory;
        }

        /// <inheritdoc/>
        protected override async Task<ValidationResult> ValidateInventoryAsync(InventoryProduct inventoryProduct)
        {
            ArgumentValidators.ThrowIfNull(inventoryProduct, nameof(inventoryProduct));

            var errors = new List<ErrorInfo>();
            var isProductAvailable = await this.ValidateProductAsync(inventoryProduct.ProductId).ConfigureAwait(false);
            if (!isProductAvailable)
                {
                    var errorMessage = string.Format(CultureInfo.InvariantCulture, Registration.Constants.InvalidProductForNodeStorageLocation, inventoryProduct.ProductId);
                    errors.Add(new ErrorInfo(errorMessage));

                    return new ValidationResult(errors);
                }

            return ValidationResult.Success;
        }

        /// <inheritdoc/>
        protected override async Task<ValidationResult> ValidateMovementAsync(Movement movement)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));

            var errors = new List<ErrorInfo>();
            var sourceProductId = movement.MovementSource?.SourceProductId;

            if (!string.IsNullOrWhiteSpace(sourceProductId))
            {
                var isProductAvailable = await this.ValidateProductAsync(sourceProductId).ConfigureAwait(false);
                if (!isProductAvailable)
                {
                    var errorMessage = string.Format(CultureInfo.InvariantCulture, Registration.Constants.InvalidProductForNodeStorageLocation, sourceProductId);
                    errors.Add(new ErrorInfo(errorMessage));

                    return new ValidationResult(errors);
                }
            }

            if (movement.MovementDestination != null && movement.MovementDestination.DestinationProductId == null)
            {
                errors.Add(new ErrorInfo(Constants.DestinationProductIdIsMandatory));
                return new ValidationResult(errors);
            }

            var destinationProductId = movement.MovementDestination?.DestinationProductId;
            if (!string.IsNullOrWhiteSpace(destinationProductId))
            {
                var isProductAvailable = await this.ValidateProductAsync(destinationProductId).ConfigureAwait(false);
                if (!isProductAvailable)
                {
                    var errorMessage = string.Format(CultureInfo.InvariantCulture, Registration.Constants.InvalidProductForNodeStorageLocation, destinationProductId);
                    errors.Add(new ErrorInfo(errorMessage));

                    return new ValidationResult(errors);
                }
            }

            return ValidationResult.Success;
        }

        /// <inheritdoc/>
        protected override async Task<ValidationResult> ValidateEventAsync(Event eventObj)
        {
            ArgumentValidators.ThrowIfNull(eventObj, nameof(eventObj));

            var errors = new List<ErrorInfo>();

            var isSourceProductAvailable = await this.ValidateProductAsync(eventObj.SourceProductId).ConfigureAwait(false);
            if (!isSourceProductAvailable)
            {
                var errorMessage = string.Format(CultureInfo.InvariantCulture, Registration.Constants.EventInvalidSourceProduct, eventObj.SourceProductId);
                errors.Add(new ErrorInfo(errorMessage));

                return new ValidationResult(errors);
            }

            var isDestinationProductAvailable = await this.ValidateProductAsync(eventObj.DestinationProductId).ConfigureAwait(false);
            if (!isDestinationProductAvailable)
            {
                var errorMessage = string.Format(CultureInfo.InvariantCulture, Registration.Constants.EventInvalidDestinationProduct, eventObj.DestinationProductId);
                errors.Add(new ErrorInfo(errorMessage));

                return new ValidationResult(errors);
            }

            return ValidationResult.Success;
        }

        /// <inheritdoc/>
        protected override async Task<ValidationResult> ValidateContractAsync(Contract contractObj)
        {
            ArgumentValidators.ThrowIfNull(contractObj, nameof(contractObj));

            var errors = new List<ErrorInfo>();
            var isProductAvailable = await this.ValidateProductAsync(contractObj.ProductId).ConfigureAwait(false);
            if (!isProductAvailable)
            {
                var errorMessage = string.Format(CultureInfo.InvariantCulture, Registration.Constants.EventInvalidSourceProduct, contractObj.ProductId);
                errors.Add(new ErrorInfo(errorMessage));

                return new ValidationResult(errors);
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Checks the product available asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The bool.</returns>
        private async Task<bool> ValidateProductAsync(string productId)
        {
            var storageLocationProductCount = await this.factory.CreateRepository<Product>().GetCountAsync(x => x.ProductId == productId).ConfigureAwait(true);
            return storageLocationProductCount > 0;
        }
    }
}
