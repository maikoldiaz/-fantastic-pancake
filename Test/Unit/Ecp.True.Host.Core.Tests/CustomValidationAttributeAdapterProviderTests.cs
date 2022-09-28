// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomValidationAttributeAdapterProviderTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core.Tests
{
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Host.Core.Adapter;
    using Microsoft.AspNetCore.Mvc.DataAnnotations;
    using Microsoft.Extensions.Localization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The custom validation attribute adapter provider tests.
    /// </summary>
    [TestClass]
    public class CustomValidationAttributeAdapterProviderTests
    {
        /// <summary>
        /// The adapter provider.
        /// </summary>
        private CustomValidationAttributeAdapterProvider adapterProvider;

        /// <summary>
        /// The mock repository.
        /// </summary>
        private MockRepository mockRepository;

        /// <summary>
        /// The mock localizer.
        /// </summary>
        private Mock<IStringLocalizer> mockLocalizer;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockLocalizer = this.mockRepository.Create<IStringLocalizer>();
            this.adapterProvider = new CustomValidationAttributeAdapterProvider();
        }

        /// <summary>
        /// Get attribute adaptor should return required if adapter for required if validation attribute.
        /// </summary>
        [TestMethod]
        public void GetAttributeAdaptor_ShouldReturnRequiredIfAdapter_ForRequiredIfValidationAttribute()
        {
            var adapter = this.adapterProvider.GetAttributeAdapter(new RequiredIfAttribute("P1", "P2"), this.mockLocalizer.Object);
            Assert.IsInstanceOfType(adapter, typeof(RequiredIfAttributeAdapter));
        }

        /// <summary>
        /// Get attribute adaptor should return must not be empty if attribute adapter for must not be empty if validation attribute.
        /// </summary>
        [TestMethod]
        public void GetAttributeAdaptor_ShouldReturnMustNotBeEmptyIfAttributeAdapter_ForMustNotBeEmptyIfValidationAttribute()
        {
            var adapter = this.adapterProvider.GetAttributeAdapter(new MustNotBeEmptyIfAttribute("P1", "P2"), this.mockLocalizer.Object);
            Assert.IsInstanceOfType(adapter, typeof(MustNotBeEmptyIfAttributeAdapter));
        }

        /// <summary>
        /// Get attribute adaptor should return default attribute adapter for default validation attribute.
        /// </summary>
        [TestMethod]
        public void GetAttributeAdaptor_ShouldReturnDefaultAttributeAdapter_ForDefaultValidationAttribute()
        {
            var adapter = this.adapterProvider.GetAttributeAdapter(new RequiredAttribute(), this.mockLocalizer.Object);
            Assert.IsInstanceOfType(adapter, typeof(RequiredAttributeAdapter));
        }

        /// <summary>
        /// Get attribute adaptor should return must not be empty attribute adapter for must not be empty validation attribute.
        /// </summary>
        [TestMethod]
        public void GetAttributeAdaptor_ShouldReturnMustNotBeEmptyAttributeAdapter_ForMustNotBeEmptyValidationAttribute()
        {
            var adapter = this.adapterProvider.GetAttributeAdapter(new MustNotBeEmptyAttribute(), this.mockLocalizer.Object);
            Assert.IsInstanceOfType(adapter, typeof(MustNotBeEmptyAttributeAdapter));
        }
    }
}
