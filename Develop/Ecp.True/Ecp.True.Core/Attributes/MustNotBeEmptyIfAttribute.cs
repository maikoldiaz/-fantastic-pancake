// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MustNotBeEmptyIfAttribute.cs" company="Microsoft">
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
    using System.Globalization;

    /// <summary>
    /// The must not be empty if attribute.
    /// </summary>
    /// <seealso cref="System.ComponentModel.DataAnnotations.ValidationAttribute" />
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class MustNotBeEmptyIfAttribute : ValidationAttribute
    {
        /// <summary>
        /// The other property.
        /// </summary>
        private readonly string otherProperty;

        /// <summary>
        /// The other property value.
        /// </summary>
        private readonly object otherPropertyValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="MustNotBeEmptyIfAttribute" /> class.
        /// </summary>
        /// <param name="otherProperty">The other property.</param>
        /// <param name="otherPropertyValue">The other property value.</param>
        /// <param name="errorMessage">The error message.</param>
        public MustNotBeEmptyIfAttribute(string otherProperty, object otherPropertyValue)
        {
            this.otherProperty = otherProperty;
            this.otherPropertyValue = otherPropertyValue;
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

            var otherPropertyElement = validationContext.ObjectType.GetProperty(this.otherProperty);
            if (otherPropertyElement == null)
            {
                return new ValidationResult(string.Format(CultureInfo.InvariantCulture, "Could not find a property named '{0}'.", this.otherProperty));
            }

            var otherValue = otherPropertyElement.GetValue(validationContext.ObjectInstance);

            // check if this value is actually required and validate it
            if (Equals(otherValue, this.otherPropertyValue))
            {
                if (value == null)
                {
                    return new ValidationResult(this.ErrorMessage, new[] { validationContext.DisplayName });
                }

                if (value is ICollection list && list.Count == 0)
                {
                    return new ValidationResult(this.ErrorMessage, new[] { validationContext.DisplayName });
                }
            }

            return ValidationResult.Success;
        }
    }
}
