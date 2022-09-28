// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElementValidator.cs" company="Microsoft">
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
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The element Validator.
    /// </summary>
    /// <typeparam name="T">The type of message.</typeparam>
    /// <seealso cref="Ecp.True.Processors.Registration.Validation.Validator{T}" />
    public class ElementValidator<T> : Validator<T>
        where T : class
    {
        /// <summary>
        /// The repository factory.
        /// </summary>
        private readonly IRepositoryFactory repositoryFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementValidator{T}"/> class.
        /// </summary>
        /// <param name="repositoryFactory">The repository factory.</param>
        public ElementValidator(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        /// <inheritdoc/>
        protected override async Task<ValidationResult> ValidateInventoryAsync(InventoryProduct inventoryProduct)
        {
            ArgumentValidators.ThrowIfNull(inventoryProduct, nameof(inventoryProduct));

            // Products
            var result = await this.ValidateInventoryProductTypeAsync(inventoryProduct).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            // Operator Id
            result = await this.ValidateElementAsync(3, inventoryProduct.OperatorId, Registration.Constants.InvalidOperatorId).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            // System Id
            result = await this.ValidateElementAsync(8, inventoryProduct.SystemId, Registration.Constants.InvalidSystemId).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            // Source System Id
            result = await this.ValidateElementAsync(22, inventoryProduct.SourceSystemId, Registration.Constants.InvalidSourceSystemId).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            // SegmentId
            result = await this.ValidateElementAsync(2, inventoryProduct.SegmentId, Registration.Constants.InvalidSegmentId).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            // Units
            result = await this.ValidateUnitAsync(new[] { inventoryProduct.MeasurementUnit.GetValueOrDefault() }).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            // Owners
            result = await this.ValidateOwnersAsync(inventoryProduct.Owners.ToArray(), Registration.Constants.InvalidOwnerId).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            // Attributes
            result = await this.ValidateAttributeIdAsync(inventoryProduct.Attributes.ToArray(), Registration.Constants.InvalidAttributeId).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            // ValueAttributeUnit
            return await this.ValidateInventoryValueAttributeUnitIdAsync(inventoryProduct).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override async Task<ValidationResult> ValidateMovementAsync(Movement movement)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));

            // Movement type
            var result = await this.ValidateMovementTypeAsync(movement.MovementTypeId).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            // Product Type
            result = await this.ValidateMovementProductTypeAsync(movement).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            // Operator Id
            result = await this.ValidateElementAsync(3, movement.OperatorId, Registration.Constants.InvalidOperatorId).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            // System Id
            result = await this.ValidateElementAsync(8, movement.SystemId, Registration.Constants.InvalidSystemId).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            // Source System Id
            result = await this.ValidateElementAsync(22, movement.SourceSystemId, Registration.Constants.InvalidSourceSystemId).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            // SegmentId
            result = await this.ValidateElementAsync(2, movement.SegmentId, Registration.Constants.InvalidSegmentId).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            // Units
            result = await this.ValidateUnitAsync(new[] { movement.MeasurementUnit.GetValueOrDefault() }).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            // Owners
            result = await this.ValidateOwnersAsync(movement.Owners.ToArray(), Registration.Constants.InvalidOwnerId).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            // Attributes
            return await this.ValidateMovementAttributesAsync(movement).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override async Task<ValidationResult> ValidateEventAsync(Event eventObj)
        {
            ArgumentValidators.ThrowIfNull(eventObj, nameof(eventObj));

            // Units
            var result = await this.ValidateUnitAsync(new[] { Convert.ToInt32(eventObj.MeasurementUnit, CultureInfo.InvariantCulture) }).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            // Event Type
            result = await this.ValidateTypeAsync(12, new[] { Convert.ToString(eventObj.EventTypeId, CultureInfo.InvariantCulture) }).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            // Owners
            result = await this.ValidateOwnersAsync(
                new[] { new Owner { OwnerId = eventObj.Owner1Id } },
                Registration.Constants.Owner1Notfound).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            result = await this.ValidateOwnersAsync(
                new[] { new Owner { OwnerId = eventObj.Owner2Id } },
                Registration.Constants.Owner2Notfound).ConfigureAwait(false);
            return result.Item1 ? ValidationResult.Success : new ValidationResult(result.Item2);
        }

        /// <inheritdoc/>
        protected override async Task<ValidationResult> ValidateContractAsync(Contract contractObj)
        {
            ArgumentValidators.ThrowIfNull(contractObj, nameof(contractObj));

            // Units
            var result = await this.ValidateUnitAsync(new[] { contractObj.MeasurementUnit }).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            // Event Type
            result = await this.ValidateTypeAsync(9, new[] { Convert.ToString(contractObj.MovementTypeId, CultureInfo.InvariantCulture) }).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            return result.Item1 ? ValidationResult.Success : new ValidationResult(result.Item2);
        }

        private async Task<Tuple<bool, string>> ValidateMovementTypeAsync(int movementTypeId)
        {
            var isValid = true;
            var message = string.Empty;
            var repo = this.repositoryFactory.CreateRepository<CategoryElement>();

            if (movementTypeId > 0)
            {
                isValid = await repo.GetCountAsync(c => c.CategoryId == 9 && c.ElementId == movementTypeId).ConfigureAwait(false) > 0;
                message = !isValid ? Registration.Constants.InvalidMovementTypeId : message;
            }

            return Tuple.Create(isValid, message);
        }

        private async Task<Tuple<bool, string>> ValidateMovementProductTypeAsync(Movement movement)
        {
            var result = await this.ValidateMovementSourceProductTypeAsync(movement).ConfigureAwait(false);
            return !result.Item1 ? result : await this.ValidateMovementDestinationProductTypeAsync(movement).ConfigureAwait(false);
        }

        private async Task<Tuple<bool, string>> ValidateMovementSourceProductTypeAsync(Movement movement)
        {
            var isValid = true;
            var message = string.Empty;
            var repo = this.repositoryFactory.CreateRepository<CategoryElement>();

            var typeId = movement.MovementSource != null ? movement.MovementSource.SourceProductTypeId.GetValueOrDefault() : 0;

            if (typeId > 0)
            {
                isValid = await repo.GetCountAsync(c => c.CategoryId == 11 && c.ElementId == typeId).ConfigureAwait(false) > 0;
                message = !isValid ? Registration.Constants.InvalidSourceProductTypeId : message;
            }

            return Tuple.Create(isValid, message);
        }

        private async Task<Tuple<bool, string>> ValidateMovementDestinationProductTypeAsync(Movement movement)
        {
            var isValid = true;
            var message = string.Empty;
            var repo = this.repositoryFactory.CreateRepository<CategoryElement>();

            var typeId = movement.MovementDestination != null ? movement.MovementDestination.DestinationProductTypeId.GetValueOrDefault() : 0;

            if (typeId > 0)
            {
                isValid = await repo.GetCountAsync(c => c.CategoryId == 11 && c.ElementId == typeId).ConfigureAwait(false) > 0;
                message = !isValid ? Registration.Constants.InvalidDestinationProductTypeId : message;
            }

            return Tuple.Create(isValid, message);
        }

        private async Task<Tuple<bool, string>> ValidateInventoryProductTypeAsync(InventoryProduct inventoryProduct)
        {
            var isValid = true;
            var message = string.Empty;
            var repo = this.repositoryFactory.CreateRepository<CategoryElement>();

            var typeId = inventoryProduct.ProductType.GetValueOrDefault();

            if (typeId > 0)
            {
                isValid = await repo.GetCountAsync(c => c.CategoryId == 11 && c.ElementId == typeId).ConfigureAwait(false) > 0;
                message = !isValid ? Registration.Constants.InvalidProductTypeId : message;
            }

            return Tuple.Create(isValid, message);
        }

        private async Task<Tuple<bool, string>> ValidateElementAsync(int categoryId, int? elementId, string errorMessage)
        {
            var isValid = true;
            var message = string.Empty;
            var repo = this.repositoryFactory.CreateRepository<CategoryElement>();

            if (elementId.HasValue)
            {
                isValid = await repo.GetCountAsync(c => c.CategoryId == categoryId && c.ElementId == elementId).ConfigureAwait(false) > 0;
                message = !isValid ? errorMessage : message;
            }

            return Tuple.Create(isValid, message);
        }

        private async Task<Tuple<bool, string>> ValidateStorageLocationIdAsync(int? elementId, string errorMessage)
        {
            var isValid = true;
            var message = string.Empty;
            var repo = this.repositoryFactory.CreateRepository<NodeStorageLocation>();

            if (elementId.HasValue)
            {
                isValid = await repo.GetCountAsync(c => c.NodeStorageLocationId == elementId).ConfigureAwait(false) > 0;
                message = !isValid ? errorMessage : message;
            }

            return Tuple.Create(isValid, message);
        }

        private async Task<Tuple<bool, string>> ValidateUnitAsync(int[] units)
        {
            var isValid = true;
            var message = string.Empty;
            var i = 0;

            while (isValid && i < units.Length)
            {
                if (units[i] == 0)
                {
                    isValid = false;
                    message = Registration.Constants.ManadatoryUnitId;
                }

                if (units[i] > 0)
                {
                    var validationResult = await this.ValidateMeasurementUnitCategoryAsync(units[i], message).ConfigureAwait(false);
                    isValid = validationResult.Item1;
                    message = validationResult.Item2;
                }

                i++;
            }

            return Tuple.Create(isValid, message);
        }

        private async Task<Tuple<bool, string>> ValidateMeasurementUnitCategoryAsync(int typeId, string message)
        {
            var repo = this.repositoryFactory.CreateRepository<CategoryElement>();

            var isValid = await repo.GetCountAsync(c => c.CategoryId == 6 && c.ElementId == typeId).ConfigureAwait(false) > 0;
            message = !isValid ? Registration.Constants.InvalidUnitId : message;
            return Tuple.Create(isValid, message);
        }

        private async Task<Tuple<bool, string>> ValidateOwnersAsync(Owner[] owners, string errorMessage)
        {
            var isValid = true;
            var message = string.Empty;
            var repo = this.repositoryFactory.CreateRepository<CategoryElement>();

            var i = 0;

            while (isValid && i < owners.Length)
            {
                if (owners[i].OwnerId > 0)
                {
                    isValid = await repo.GetCountAsync(c => c.CategoryId == 7 && c.ElementId == owners[i].OwnerId).ConfigureAwait(false) > 0;
                    message = !isValid ? errorMessage : message;
                }

                i++;
            }

            return Tuple.Create(isValid, message);
        }

        private async Task<Tuple<bool, string>> ValidateAttributeIdAsync(AttributeEntity[] attributes, string errorMessage)
        {
            var isValid = true;
            var message = string.Empty;
            var repo = this.repositoryFactory.CreateRepository<CategoryElement>();

            var typeId = 0;
            var i = 0;

            while (isValid && i < attributes.Length)
            {
                if (typeId > 0)
                {
                    isValid = await repo.GetCountAsync(c => c.CategoryId == 20 && c.ElementId == typeId).ConfigureAwait(false) > 0;
                    message = !isValid ? errorMessage : message;
                }

                i++;
            }

            return Tuple.Create(isValid, message);
        }

        private async Task<Tuple<bool, string>> ValidateValueAttributeUnitIdAsync(AttributeEntity[] attributes, string errorMessage)
        {
            var isValid = true;
            var message = string.Empty;
            var repo = this.repositoryFactory.CreateRepository<CategoryElement>();

            var typeId = 0;
            var i = 0;

            while (isValid && i < attributes.Length)
            {
                if (typeId > 0)
                {
                    isValid = await repo.GetCountAsync(c => c.CategoryId == 6 && c.ElementId == typeId).ConfigureAwait(false) > 0;
                    message = !isValid ? errorMessage : message;
                }

                i++;
            }

            return Tuple.Create(isValid, message);
        }

        private async Task<ValidationResult> ValidateMovementAttributesAsync(Movement movement)
        {
            // Attributes
            var result = await this.ValidateAttributeIdAsync(movement.Attributes.ToArray(), Registration.Constants.InvalidAttributeId).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            result = await this.ValidateValueAttributeUnitIdAsync(movement.Attributes.ToArray(), Registration.Constants.InvalidValueAttributeUnitId).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            // Source Storage Location Id
            result = await this.ValidateStorageLocationIdAsync(movement.MovementSource?.SourceStorageLocationId, Registration.Constants.InvalidSourceStorageLocationId).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            // Destination Storage Location Id
            result = await this.ValidateStorageLocationIdAsync(
                movement.MovementDestination?.DestinationStorageLocationId,
                Registration.Constants.InvalidDestinationStorageLocationId).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            return result.Item1 ? ValidationResult.Success : new ValidationResult(result.Item2);
        }

        private async Task<ValidationResult> ValidateInventoryValueAttributeUnitIdAsync(InventoryProduct inventoryProduct)
        {
            // ValueAttributeUnitId
            var result = await this.ValidateValueAttributeUnitIdAsync(inventoryProduct.Attributes.ToArray(), Registration.Constants.InvalidValueAttributeUnitId).ConfigureAwait(false);
            if (!result.Item1)
            {
                return new ValidationResult(result.Item2);
            }

            return result.Item1 ? ValidationResult.Success : new ValidationResult(result.Item2);
        }

        private async Task<Tuple<bool, string>> ValidateTypeAsync(int categoryId, string[] types)
        {
            var isValid = true;
            var message = string.Empty;
            var repo = this.repositoryFactory.CreateRepository<CategoryElement>();

            int typeId = 0;
            var i = 0;

            while (isValid && i < types.Length)
            {
                if (!string.IsNullOrWhiteSpace(types[0]) && !int.TryParse(types[0], out typeId))
                {
                    isValid = false;
                    message = $"{Registration.Constants.InvalidDataType} EventType";
                }

                if (typeId > 0)
                {
                    isValid = await repo.GetCountAsync(c => c.CategoryId == categoryId && c.ElementId == typeId).ConfigureAwait(false) > 0;
                    message = !isValid ? Registration.Constants.InvalidUnitId : message;
                }

                i++;
            }

            return Tuple.Create(isValid, message);
        }
    }
}
