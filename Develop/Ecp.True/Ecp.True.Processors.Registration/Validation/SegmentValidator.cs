// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SegmentValidator.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Registration;
    using EfCore.Models;

    /// <summary>
    /// The Segment Validator.
    /// </summary>
    /// <typeparam name="T">The type param.</typeparam>
    /// <seealso cref="Ecp.True.Processors.Api.Inventory.Validator.Interface.IValidator{T}" />
    public class SegmentValidator<T> : Validator<T>
        where T : class
    {
        /// <summary>
        /// The factory.
        /// </summary>
        private readonly IRepositoryFactory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentValidator{T}"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public SegmentValidator(IRepositoryFactory factory)
        {
            this.factory = factory;
        }

        /// <inheritdoc/>
        protected override async Task<ValidationResult> ValidateInventoryAsync(InventoryProduct inventoryProduct)
        {
            ArgumentValidators.ThrowIfNull(inventoryProduct, nameof(inventoryProduct));

            if (!inventoryProduct.SegmentId.HasValue)
            {
                return ValidationResult.Success;
            }

            var isValid = await this.ValidateSegmentAsync(inventoryProduct.NodeId, inventoryProduct.SegmentId.Value, inventoryProduct.InventoryDate.GetValueOrDefault()).ConfigureAwait(false);
            return isValid ? ValidationResult.Success : new ValidationResult(Registration.Constants.NodeSegmentInvalid);
        }

        /// <inheritdoc/>
        protected override async Task<ValidationResult> ValidateMovementAsync(Movement movement)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));

            if (!movement.SegmentId.HasValue)
            {
                return ValidationResult.Success;
            }

            bool isValid = false;
            if (movement.MovementSource != null && movement.MovementSource.SourceNodeId.HasValue)
            {
                isValid = await this.ValidateSegmentAsync(movement.MovementSource.SourceNodeId.Value, movement.SegmentId.Value, movement.OperationalDate).ConfigureAwait(false);
            }

            if (isValid)
            {
                return ValidationResult.Success;
            }

            // Even if above validation fails, we still need to validate destination node id
            // So setting this to true
            isValid = true;
            if (movement.MovementDestination != null && movement.MovementDestination.DestinationNodeId.HasValue)
            {
                isValid = await this.ValidateSegmentAsync(movement.MovementDestination.DestinationNodeId.Value, movement.SegmentId.Value, movement.OperationalDate).ConfigureAwait(false);
            }

            return isValid ? ValidationResult.Success : new ValidationResult(Registration.Constants.NodeSegmentInvalid);
        }

        private async Task<bool> ValidateSegmentAsync(int nodeId, int segmentId, DateTime current)
        {
            var repository = this.factory.CreateRepository<NodeTag>();
            var tagsCount = await repository.GetCountAsync(t => t.NodeId == nodeId && t.ElementId == segmentId
            && t.StartDate.Date <= current.Date && t.EndDate >= current.Date).ConfigureAwait(false);

            return tagsCount > 0;
        }
    }
}
