// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationInsightsResolverTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Core.Tests
{
    using Ecp.True.Host.Functions.Core.Setup;
    using Microsoft.Extensions.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The application insights resolver tests.
    /// </summary>
    [TestClass]
    public class ApplicationInsightsResolverTests
    {
        /// <summary>
        /// The resolver.
        /// </summary>
        private ApplicationInsightsResolver resolver;

        /// <summary>
        /// The mock repository.
        /// </summary>
        private MockRepository mockRepository;

        /// <summary>
        /// The mock configuration.
        /// </summary>
        private Mock<IConfiguration> mockConfiguration;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Loose);
            this.mockConfiguration = this.mockRepository.Create<IConfiguration>();

            this.resolver = new ApplicationInsightsResolver(this.mockConfiguration.Object);
        }

        /// <summary>
        /// Resolves the application insights key should get application insights key from application settings section.
        /// </summary>
        [TestMethod]
        public void ResolveApplicationInsightsKey_ShouldGetAppInsightsKey_FromAppSettingsSection()
        {
            this.mockConfiguration.Setup(m => m["InstrumentationKey"]).Returns("InstrumentationKey");

            // Act
            var result = this.resolver.ResolveApplicationInsightsKey();

            // Assert
            Assert.AreEqual("InstrumentationKey", result);
            this.mockConfiguration.Verify(m => m["InstrumentationKey"], Times.Once);
        }
    }
}
