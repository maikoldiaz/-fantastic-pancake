// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContractFrequencyAttribute.cs" company="Microsoft">
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
    /// The Data Validator Attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class ContractFrequencyAttribute : ValidationAttribute
    {
        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ArgumentValidators.ThrowIfNull(validationContext, nameof(validationContext));
            ArgumentValidators.ThrowIfNull(value, nameof(value));
            var frequency = value.ToString();
            if (!(frequency.EqualsIgnoreCase("diaria") || frequency.EqualsIgnoreCase("semanal")
                || frequency.EqualsIgnoreCase("quincenal") || frequency.EqualsIgnoreCase("mensual")))
            {
                return new ValidationResult(Constants.FrequencyInvalidValue);
            }

            return ValidationResult.Success;
        }
    }
}
