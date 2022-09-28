// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositeValidator.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core.Entities;
    using Ecp.True.Processors.Registration.Interfaces;

    /// <summary>
    /// The Composite Validator.
    /// </summary>
    /// <typeparam name="T">The Type Param.</typeparam>
    /// <seealso cref="Ecp.True.Processors.Api.Inventory.Validator.ICompositeValidator{T}" />
    public class CompositeValidator<T> : Validator<T>, ICompositeValidator<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidator{T}"/> class.
        /// </summary>
        /// <param name="children">The children.</param>
        public CompositeValidator(IValidator<T>[] children)
        {
            this.Children = children;
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        public IEnumerable<IValidator<T>> Children { get; }

        /// <summary>
        /// Validates the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The Task.</returns>
        public override async Task<ValidationResult> ValidateAsync(T entity)
        {
            var dataAnnoationResult = await this.RunDataAnnotationValidatorAsync(entity).ConfigureAwait(false);
            if (!dataAnnoationResult.IsSuccess)
            {
                return dataAnnoationResult;
            }

            var otherValidators = this.Children.Where(x => x.GetType() != typeof(DataAnnotationValidator<T>));
            foreach (var validator in otherValidators)
            {
                var validatorResult = await validator.ValidateAsync(entity).ConfigureAwait(false);

                if (!validatorResult.IsSuccess)
                {
                    return validatorResult;
                }
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Runs the data annotation validator.
        /// </summary>
        /// <param name="entity">The entity.</param>
        private Task<ValidationResult> RunDataAnnotationValidatorAsync(T entity)
        {
            var dataAnnotationValidator = this.Children.Single(x => x.GetType() == typeof(DataAnnotationValidator<T>));
            return dataAnnotationValidator.ValidateAsync(entity);
        }
    }
}
