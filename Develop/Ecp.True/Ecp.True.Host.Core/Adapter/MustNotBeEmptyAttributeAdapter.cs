// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MustNotBeEmptyAttributeAdapter.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core.Adapter
{
    using System.Diagnostics.CodeAnalysis;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Microsoft.AspNetCore.Mvc.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using Microsoft.Extensions.Localization;

    /// <summary>
    /// The custom attribute adapter.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    [ExcludeFromCodeCoverage]
    public class MustNotBeEmptyAttributeAdapter : AttributeAdapterBase<MustNotBeEmptyAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MustNotBeEmptyAttributeAdapter"/> class.
        /// </summary>
        /// <param name="attribute">The validation attribute being wrapped.</param>
        /// <param name="stringLocalizer">The string localizer to be used in error generation.</param>
        public MustNotBeEmptyAttributeAdapter(MustNotBeEmptyAttribute attribute, IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {
        }

        /// <inheritdoc/>
        public override void AddValidation(ClientModelValidationContext context)
        {
            // Do nothing?
        }

        /// <inheritdoc/>
        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            ArgumentValidators.ThrowIfNull(validationContext, nameof(validationContext));
            return this.GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName());
        }
    }
}
