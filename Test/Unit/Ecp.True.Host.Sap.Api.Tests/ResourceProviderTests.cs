// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceProviderTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Sap.Api.Tests
{
    using Ecp.True.Host.Sap.Api.Resources;
    using Microsoft.Extensions.Localization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The resource provider tests.
    /// </summary>
    [TestClass]
    public class ResourceProviderTests
    {
        /// <summary>
        /// The resource provider.
        /// </summary>
        private ResourceProvider resourceProvider;

        /// <summary>
        /// The mock localizer factory.
        /// </summary>
        private Mock<IStringLocalizerFactory> mockLocalizerFactory;

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
            this.mockLocalizerFactory = new Mock<IStringLocalizerFactory>();
            this.mockLocalizer = new Mock<IStringLocalizer>();

            this.mockLocalizer.Setup(m => m[It.IsAny<string>()]).Returns<string>(s => new LocalizedString(s, s));
            this.mockLocalizerFactory.Setup(m => m.Create("SharedResource", "Ecp.True.Host.Sap.Api")).Returns(this.mockLocalizer.Object);

            this.resourceProvider = new ResourceProvider(this.mockLocalizerFactory.Object);
        }

        /// <summary>
        /// Get resource should invoke string localizer when getting resource value.
        /// </summary>
        [TestMethod]
        public void GetResource_ShouldInvokeStringLocalizer_WhenGettingResourceValue()
        {
            // Act
            var value = this.resourceProvider.GetResource("SomeKey");
            Assert.AreEqual("SomeKey", value);

            this.mockLocalizer.VerifyAll();
            this.mockLocalizerFactory.VerifyAll();
        }
    }
}
