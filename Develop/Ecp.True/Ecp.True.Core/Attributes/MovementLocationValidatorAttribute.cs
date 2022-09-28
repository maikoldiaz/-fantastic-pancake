// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementLocationValidatorAttribute.cs" company="Microsoft">
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
    using System.Globalization;

    /// <summary>
    /// The Data Validator Attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class MovementLocationValidatorAttribute : ValidationAttribute
    {
        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ArgumentValidators.ThrowIfNull(validationContext, nameof(validationContext));

            if (validationContext.ObjectType != null)
            {
                var classificationProperty = validationContext.ObjectType.GetProperty("Classification");

                var classification = Convert.ToString(classificationProperty.GetValue(validationContext.ObjectInstance, null), CultureInfo.InvariantCulture).ToLowerCase();

                if ((classification.EqualsIgnoreCase("Movimiento") || classification.EqualsIgnoreCase("OperacionTrazable")) && value == null)
                {
                    var errorMessage = validationContext.MemberName == "MovementSource"
                        ? Constants.MovementSourceRequired
                        : Constants.MovementDestinationRequired;

                    return new ValidationResult(errorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
