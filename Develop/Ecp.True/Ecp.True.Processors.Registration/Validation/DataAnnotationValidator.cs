// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataAnnotationValidator.cs" company="Microsoft">
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
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using ValidationResultEntity = Ecp.True.Core.Entities.ValidationResult;

    /// <summary>
    /// The Data Annotation Validator.
    /// </summary>
    /// <typeparam name="T">The Type Param.</typeparam>
    /// <seealso cref="Ecp.True.Processors.Api.Inventory.Validator.Interface.IValidator{T}" />
    public class DataAnnotationValidator<T> : Validator<T>
        where T : class
    {
        /// <summary>
        /// Validates the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The Validation Result.</returns>
        public override Task<ValidationResultEntity> ValidateAsync(T entity)
        {
            var validationResult = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            var context = new ValidationContext(entity);
            var isValid = Validator.TryValidateObject(entity, context, validationResult, validateAllProperties: true);

            var errors = new List<ErrorInfo>();
            validationResult.ForEach(v =>
            {
                if (v is CompositeValidationResult compositeValidationResult)
                {
                    compositeValidationResult.Results.ForEach(x => errors.Add(new ErrorInfo(x.ErrorMessage)));
                }
                else
                {
                    errors.Add(new ErrorInfo(v.ErrorMessage));
                }
            });

            var result = !isValid ? new ValidationResultEntity(errors) : ValidationResultEntity.Success;
            return Task.FromResult(result);
        }
    }
}
