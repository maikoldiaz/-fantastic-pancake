// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateObjectAttribute.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Core.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The attribute to validate object.
    /// </summary>
    /// <seealso cref="System.ComponentModel.DataAnnotations.ValidationAttribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ValidateObjectAttribute : ValidationAttribute
    {
        /// <summary>
        /// Returns true if ... is valid.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// The validation result.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var results = new List<ValidationResult>();
            var context = new ValidationContext(value, null, null);

            Validator.TryValidateObject(value, context, results, true);

            if (results.Count != 0)
            {
                var compositeResults = new CompositeValidationResult($"Validation for {validationContext?.DisplayName} failed!");
                results.ForEach(compositeResults.AddResult);

                return compositeResults;
            }

            return ValidationResult.Success;
        }
    }
}
