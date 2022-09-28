// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptionalIfAttribute.cs" company="Microsoft">
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
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    /// <summary>
    /// Provides conditional validation based on related property value.
    /// </summary>
    /// <seealso cref="System.ComponentModel.DataAnnotations.ValidationAttribute" />
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class OptionalIfAttribute : ValidationAttribute
    {
        /// <summary>
        /// The other property.
        /// </summary>
        private readonly string mandatoryProperty;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionalIfAttribute" /> class.
        /// </summary>
        /// <param name="mandatoryProperty">The mandatory property.</param>
        public OptionalIfAttribute(string mandatoryProperty)
        {
            this.mandatoryProperty = mandatoryProperty;
        }

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

            var mandatoryPropertyElement = validationContext.ObjectType.GetProperty(this.mandatoryProperty);
            if (mandatoryPropertyElement == null)
            {
                if (value == null)
                {
                    return new ValidationResult(string.Format(CultureInfo.InvariantCulture, this.ErrorMessage));
                }

                return ValidationResult.Success;
            }

            var mandatoryValue = mandatoryPropertyElement.GetValue(validationContext.ObjectInstance);

            if (mandatoryValue == null && value == null)
            {
                return new ValidationResult(string.Format(CultureInfo.InvariantCulture, this.ErrorMessage));
            }

            return ValidationResult.Success;
        }
    }
}
