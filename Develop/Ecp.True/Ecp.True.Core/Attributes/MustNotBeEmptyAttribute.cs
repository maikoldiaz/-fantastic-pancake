// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MustNotBeEmptyAttribute.cs" company="Microsoft">
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
    using System.Collections;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The must not be empty attribute.
    /// </summary>
    /// <seealso cref="System.ComponentModel.DataAnnotations.ValidationAttribute" />
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class MustNotBeEmptyAttribute : ValidationAttribute
    {
        /// <summary>
        /// Gets a value indicating whether the attribute requires validation context.
        /// </summary>
        /// <value> True or False.</value>
        public override bool RequiresValidationContext => true;

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="System.ComponentModel.DataAnnotations.ValidationResult" /> class.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ArgumentValidators.ThrowIfNull(validationContext, nameof(validationContext));

            // check if this value is actually required and validate it
            if (value == null)
            {
                return new ValidationResult(this.ErrorMessage, new[] { validationContext.DisplayName });
            }

            if (value is ICollection list && list.Count == 0)
            {
                return new ValidationResult(this.ErrorMessage, new[] { validationContext.DisplayName });
            }

            return ValidationResult.Success;
        }
    }
}
