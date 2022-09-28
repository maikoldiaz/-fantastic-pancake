// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Validator.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Core.Entities;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Registration.Interfaces;

    /// <summary>
    /// The Validator.
    /// </summary>
    /// <typeparam name="T">The type Param Type.</typeparam>
    public class Validator<T> : IValidator<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator{T}"/> class.
        /// </summary>
        protected Validator()
        {
        }

        /// <summary>
        /// Validates the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// bool.
        /// </returns>
        public virtual Task<ValidationResult> ValidateAsync(T entity)
        {
            if (entity is InventoryProduct)
            {
                return this.ValidateInventoryAsync(entity as InventoryProduct);
            }

            if (entity is Event)
            {
                return this.ValidateEventAsync(entity as Event);
            }

            if (entity is Contract)
            {
                return this.ValidateContractAsync(entity as Contract);
            }

            return this.ValidateMovementAsync(entity as Movement);
        }

        /// <summary>
        /// Validates the inventory product asynchronous.
        /// </summary>
        /// <param name="inventoryProduct">The inventory.</param>
        /// <returns>
        /// The task.
        /// </returns>
        protected virtual Task<ValidationResult> ValidateInventoryAsync(InventoryProduct inventoryProduct)
        {
            return Task.FromResult(ValidationResult.Success);
        }

        /// <summary>
        /// Validates the movement asynchronous.
        /// </summary>
        /// <param name="movement">The movement.</param>
        /// <returns>
        /// The task.
        /// </returns>
        protected virtual Task<ValidationResult> ValidateMovementAsync(Movement movement)
        {
            return Task.FromResult(ValidationResult.Success);
        }

        /// <summary>
        /// Validates the event asynchronous.
        /// </summary>
        /// <param name="eventObj">The event.</param>
        /// <returns>
        /// The task.
        /// </returns>
        protected virtual Task<ValidationResult> ValidateEventAsync(Event eventObj)
        {
            return Task.FromResult(ValidationResult.Success);
        }

        /// <summary>
        /// Validates the contract asynchronous.
        /// </summary>
        /// <param name="contractObj">The contract.</param>
        /// <returns>
        /// The task.
        /// </returns>
        protected virtual Task<ValidationResult> ValidateContractAsync(Contract contractObj)
        {
            return Task.FromResult(ValidationResult.Success);
        }
    }
}
