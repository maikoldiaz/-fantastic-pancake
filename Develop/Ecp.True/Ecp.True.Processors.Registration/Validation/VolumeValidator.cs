// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeValidator.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Registration.Validation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The Volume Validator.
    /// </summary>
    /// <typeparam name="T">The type param.</typeparam>
    /// <seealso cref="Ecp.True.Processors.Api.Inventory.Validator.Interface.IValidator{T}" />
    public class VolumeValidator<T> : Validator<T>
        where T : class
    {
        /// <summary>
        /// The factory.
        /// </summary>
        private readonly IRepositoryFactory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeValidator{T}"/> class.
        /// </summary>
        /// <param name="factory">factory.</param>
        public VolumeValidator(IRepositoryFactory factory)
        {
            this.factory = factory;
        }

        /// <inheritdoc/>
        protected override async Task<ValidationResult> ValidateInventoryAsync(InventoryProduct inventoryProduct)
        {
            ArgumentValidators.ThrowIfNull(inventoryProduct, nameof(inventoryProduct));
            var segment = await this.GetSegmentAsync(inventoryProduct.SegmentId ?? 0).ConfigureAwait(false);
            return ValidateVolume(OwershipValidationInfo.CreateFromInventory(inventoryProduct, segment));
        }

        /// <inheritdoc/>
        protected override async Task<ValidationResult> ValidateMovementAsync(Movement movement)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));
            var segment = await this.GetSegmentAsync(movement.SegmentId ?? 0).ConfigureAwait(false);
            return ValidateVolume(OwershipValidationInfo.CreateFromMovement(movement, segment));
        }

        private static ValidationResult ValidateVolume(OwershipValidationInfo validationInfo)
        {
            var errorResponse = new List<ErrorInfo>();

            // When ownership or total volume has a value more than 2 decimal, raise an error
            if (InvalidDecimalPlaces(validationInfo))
            {
                errorResponse.Add(new ErrorInfo(Registration.Constants.OwnershipValueDecimalFailed));
                return new ValidationResult(errorResponse);
            }

            return ValidationResult.Success;
        }

        private static bool InvalidDecimalPlaces(OwershipValidationInfo validationInfo)
        {
            var isNoSonSegment = !validationInfo.Segment.IsOperationalSegment.GetValueOrDefault();
            if (isNoSonSegment)
            {
                return false;
            }

            return !validationInfo.Owners.All(v => IsTwoDecimalValue(v.OwnershipValue)) ||
                   !IsTwoDecimalValue(validationInfo.TotalVolume);
        }

        private static bool IsTwoDecimalValue(decimal? val)
        {
            if (val == null)
            {
                return true;
            }

            bool isTwoDecimal = true;
            return val.ToString().Contains('.', System.StringComparison.InvariantCulture) ? val.ToString().Split('.')[1].Length <= 2 : isTwoDecimal;
        }

        private async Task<CategoryElement> GetSegmentAsync(int segmentId)
        {
            var category = await this.factory.CreateRepository<CategoryElement>().GetByIdAsync(segmentId).ConfigureAwait(false);
            return category;
        }
    }
}