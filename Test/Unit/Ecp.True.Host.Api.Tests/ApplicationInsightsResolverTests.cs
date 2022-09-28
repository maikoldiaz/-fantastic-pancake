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

namespace Ecp.True.Host.Api.Tests
{
    using Ecp.True.Host.Shared;
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
        /// The mock configuration section.
        /// </summary>
        private Mock<IConfigurationSection> mockConfigurationSection;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Loose);

            this.mockConfiguration = this.mockRepository.Create<IConfiguration>();
            this.mockConfigurationSection = this.mockRepository.Create<IConfigurationSection>();

            this.mockConfiguration.Setup(m => m.GetSection("ApplicationInsights")).Returns(this.mockConfigurationSection.Object);

            this.resolver = new ApplicationInsightsResolver(this.mockConfiguration.Object);
        }

        /// <summary>
        /// Resolves the application insights key should get application insights key from application settings section.
        /// </summary>
        [TestMethod]
        public void ResolveApplicationInsightsKey_ShouldGetAppInsightsKey_FromAppSettingsSection()
        {
            // Act
            this.resolver.ResolveApplicationInsightsKey();

            // Assert
            this.mockConfiguration.Verify(m => m.GetSection("ApplicationInsights"), Times.Once);
        }
    }
}
