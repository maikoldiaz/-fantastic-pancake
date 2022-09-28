// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumberValidatorAttribute.cs" company="Microsoft">
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
    /// The must not be empty attribute.
    /// </summary>
    /// <seealso cref="System.ComponentModel.DataAnnotations.ValidationAttribute" />
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NumberValidatorAttribute : ValidationAttribute
    {
        /// <summary>
        /// The required property.
        /// </summary>
        private readonly bool isRequired;

        /// <summary>
        /// The minimum value for range.
        /// </summary>
        private readonly decimal minValue;

        /// <summary>
        /// The maximum value for range.
        /// </summary>
        private readonly decimal maxValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberValidatorAttribute"/> class.
        /// </summary>
        public NumberValidatorAttribute()
        {
            this.isRequired = false;
            this.minValue = 0;
            this.maxValue = 99999999999999999.99M;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberValidatorAttribute"/> class.
        /// </summary>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public NumberValidatorAttribute(object minValue, object maxValue)
        {
            this.isRequired = false;
            this.minValue = Convert.ToDecimal(minValue, CultureInfo.InvariantCulture);
            this.maxValue = Convert.ToDecimal(maxValue, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberValidatorAttribute"/> class.
        /// </summary>
        /// <param name="isRequired">The is required.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public NumberValidatorAttribute(object isRequired, object minValue, object maxValue)
        {
            this.isRequired = Convert.ToBoolean(isRequired, CultureInfo.InvariantCulture);
            this.minValue = Convert.ToDecimal(minValue, CultureInfo.InvariantCulture);
            this.maxValue = Convert.ToDecimal(maxValue, CultureInfo.InvariantCulture);
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

            // check if this value is actually required and validate it
            if (this.isRequired && value == null)
            {
                return new ValidationResult(this.ErrorMessage, new[] { validationContext.DisplayName });
            }
            else
            {
                decimal result = 0;
                if (value != null && !decimal.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), NumberStyles.Float, CultureInfo.InvariantCulture, out result))
                {
                    return new ValidationResult(this.ErrorMessage, new[] { validationContext.DisplayName });
                }

                if (result >= this.minValue && result <= this.maxValue)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(this.ErrorMessage, new[] { validationContext.DisplayName });
            }
        }
    }
}
