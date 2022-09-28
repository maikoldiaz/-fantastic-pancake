// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipValidator.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The Ownership Validator.
    /// </summary>
    /// <typeparam name="T">The type param.</typeparam>
    /// <seealso cref="Ecp.True.Processors.Api.Inventory.Validator.Interface.IValidator{T}" />
    public class OwnershipValidator<T> : Validator<T>
        where T : class
    {
        /// <summary>
        /// The factory.
        /// </summary>
        private readonly IRepositoryFactory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipValidator{T}"/> class.
        /// </summary>
        /// <param name="factory">factory.</param>
        public OwnershipValidator(IRepositoryFactory factory)
        {
            this.factory = factory;
        }

        /// <inheritdoc/>
        protected override async Task<ValidationResult> ValidateInventoryAsync(InventoryProduct inventoryProduct)
        {
            ArgumentValidators.ThrowIfNull(inventoryProduct, nameof(inventoryProduct));
            var segment = await this.GetSegmentAsync(inventoryProduct.SegmentId ?? 0).ConfigureAwait(false);
            return ValidateOwnerShip(OwershipValidationInfo.CreateFromInventory(inventoryProduct, segment));
        }

        /// <inheritdoc/>
        protected override async Task<ValidationResult> ValidateMovementAsync(Movement movement)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));
            var segment = await this.GetSegmentAsync(movement.SegmentId ?? 0).ConfigureAwait(false);
            return ValidateOwnerShip(OwershipValidationInfo.CreateFromMovement(movement, segment));
        }

        private static ValidationResult ValidateOwnerShip(OwershipValidationInfo validationInfo)
        {
            var errorResponse = new List<ErrorInfo>();
            if (validationInfo.Owners.Count > 0)
            {
                var units = new List<string>();
                units.AddRange(validationInfo.Owners.Select(x => x.OwnershipValueUnit));

                ErrorInfo info = AreUnitsValid(units);
                if (info != null)
                {
                    errorResponse.Add(info);
                    return new ValidationResult(errorResponse);
                }

                // When owner doesn't have a value, raise an error
                if (!validationInfo.Owners.All(o => o.OwnershipValue.HasValue))
                {
                    errorResponse.Add(new ErrorInfo(Registration.Constants.OwnershipValueFailed));
                    return new ValidationResult(errorResponse);
                }

                var sumOfOwnerShipValue = validationInfo.Owners.Sum(x => x.OwnershipValue).Value;
                var isPercentage = units.FirstOrDefault().Contains(Constants.OwnershipPercentageUnit, System.StringComparison.InvariantCulture);

                return DoValidateDeviationPercentage(sumOfOwnerShipValue, validationInfo.TotalVolume, isPercentage, validationInfo.DeviationPercentage);
            }

            if (validationInfo.ScenarioType != ScenarioType.OFFICER)
            {
                return ValidationResult.Success;
            }

            errorResponse.Add(new ErrorInfo(Registration.Constants.ErrorOwnershipRequired));

            return new ValidationResult(errorResponse);
        }

        private static ErrorInfo AreUnitsValid(List<string> units)
        {
            //// If any of the units has empty space, throw an error.
            if (units.Contains(string.Empty))
            {
                return new ErrorInfo(Registration.Constants.OwnerValueUnitEmptyValidationFailed);
            }

            //// If ownership values are not same insert into transaction repository and remove from the inventory list.
            if (units.Distinct().Count() > 1)
            {
                return new ErrorInfo(Registration.Constants.OwnerValueUnitValidationFailed);
            }

            return null;
        }

        private static ValidationResult DoValidateDeviationPercentage(decimal sumOfOwnerShipValue, decimal totalVolume, bool isPercentage, decimal deviationPercentage)
        {
            var errorResponse = new List<ErrorInfo>();
            if (totalVolume == 0)
            {
                if (sumOfOwnerShipValue > 0)
                {
                    errorResponse.Add(new ErrorInfo(Registration.Constants.PropertyDistribution));
                    return new ValidationResult(errorResponse);
                }

                return ValidationResult.Success;
            }

            if (!isPercentage)
            {
                sumOfOwnerShipValue = (sumOfOwnerShipValue * 100) / totalVolume;
            }

            decimal resultDeviation = Math.Abs(sumOfOwnerShipValue - 100.0m);
            bool isUpper = sumOfOwnerShipValue > 100;

            if (resultDeviation > deviationPercentage)
            {
                errorResponse.Add(new ErrorInfo(string.Format(
                    CultureInfo.InvariantCulture,
                    isPercentage ? Registration.Constants.ErrorDeviationPercentage : Registration.Constants.ErrorDeviationDistribution,
                    isUpper ? Registration.Constants.OverText : Registration.Constants.AboveText)));
                return new ValidationResult(errorResponse);
            }

            return ValidationResult.Success;
        }

        private async Task<CategoryElement> GetSegmentAsync(int segmentId)
        {
            var category = await this.factory.CreateRepository<CategoryElement>().GetByIdAsync(segmentId).ConfigureAwait(false);
            return category;
        }
    }
}