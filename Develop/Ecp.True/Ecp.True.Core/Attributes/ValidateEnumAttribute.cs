// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateEnumAttribute.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Core.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The Validate enum Attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ValidateEnumAttribute : ValidationAttribute
    {
        private readonly string errorMessage;

        private readonly Type enumType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateEnumAttribute"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="errorMessage">The errorMessage.</param>
        public ValidateEnumAttribute(Type type, string errorMessage)
        {
            this.enumType = type;
            this.errorMessage = errorMessage;
        }

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

            if (value == null)
            {
                return ValidationResult.Success;
            }

            string result = (string)value;
            if (!string.IsNullOrWhiteSpace(result) && !(Enum.TryParse(this.enumType, result, true, out object validObject) && Enum.IsDefined(this.enumType, validObject)))
            {
                return new ValidationResult(this.errorMessage, new[] { validationContext.DisplayName });
            }

            return ValidationResult.Success;
        }
    }
}
