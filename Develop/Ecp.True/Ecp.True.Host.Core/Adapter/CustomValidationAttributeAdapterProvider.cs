// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomValidationAttributeAdapterProvider.cs" company="Microsoft">
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
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Core.Attributes;
    using Microsoft.AspNetCore.Mvc.DataAnnotations;
    using Microsoft.Extensions.Localization;

    /// <summary>
    /// The custom validation attribute adapter provider.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.DataAnnotations.IValidationAttributeAdapterProvider" />
    [IoCRegistration(IoCLifetime.ContainerControlled)]
    public class CustomValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {
        /// <summary>
        /// The base provider.
        /// </summary>
        private readonly IValidationAttributeAdapterProvider baseProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomValidationAttributeAdapterProvider"/> class.
        /// </summary>
        /// <param name="baseProvider">The base provider.</param>
        public CustomValidationAttributeAdapterProvider()
        {
            this.baseProvider = new ValidationAttributeAdapterProvider();
        }

        /// <summary>
        /// Gets the attribute adapter.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="stringLocalizer">The string localizer.</param>
        /// <returns>The attribute adapter.</returns>
        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            if (attribute is RequiredIfAttribute)
            {
                return new RequiredIfAttributeAdapter(attribute as RequiredIfAttribute, stringLocalizer);
            }

            if (attribute is MustNotBeEmptyIfAttribute)
            {
                return new MustNotBeEmptyIfAttributeAdapter(attribute as MustNotBeEmptyIfAttribute, stringLocalizer);
            }

            if (attribute is MustNotBeEmptyAttribute)
            {
                return new MustNotBeEmptyAttributeAdapter(attribute as MustNotBeEmptyAttribute, stringLocalizer);
            }

            if (attribute is NumberValidatorAttribute)
            {
                return new NumberValidatorAttributeAdapter(attribute as NumberValidatorAttribute, stringLocalizer);
            }

            if (attribute is OptionalIfAttribute)
            {
                return new OptionalIfAttributeAdapter(attribute as OptionalIfAttribute, stringLocalizer);
            }

            return this.baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
        }
    }
}
