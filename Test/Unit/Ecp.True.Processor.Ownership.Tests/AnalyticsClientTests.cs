// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalyticsClientTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Azure.Tests
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Processors.Ownership.Calculation.Request;
    using Ecp.True.Processors.Ownership.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The Analytics Client Tests.
    /// </summary>
    [TestClass]
    public class AnalyticsClientTests
    {
        /// <summary>
        /// The client.
        /// </summary>
        private AnalyticsClient client;

        /// <summary>
        /// The profile.
        /// </summary>
        private AnalyticsSettings config;

        /// <summary>
        /// The factory mock.
        /// </summary>
        private Mock<IHttpClientFactory> mockFactory;

        /// <summary>
        /// The mock configuration.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfig;

        /// <summary>
        /// The mock token provider.
        /// </summary>
        private Mock<ITokenProvider> mockTokenProvider;

        /// <summary>
        /// The client.
        /// </summary>
        private HttpClient httpClient;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.config = new AnalyticsSettings
            {
                ClientId = "ClientId",
                ClientSecret = "ClientSecret",
                TenantId = "TenantId",
                Scope = "test",
            };

            this.mockFactory = new Mock<IHttpClientFactory>();
            this.mockTokenProvider = new Mock<ITokenProvider>();
            this.mockConfig = new Mock<IConfigurationHandler>();

            this.httpClient = new HttpClient();
            this.mockFactory.Setup(m => m.CreateClient(Constants.DefaultHttpClient)).Returns(this.httpClient);

            this.mockTokenProvider.Setup(m => m.GetAppTokenAsync(this.config.TenantId, this.config.Scope, this.config.ClientId, this.config.ClientSecret)).ReturnsAsync("Token");
            this.mockConfig.Setup(c => c.GetConfigurationAsync<string>(ConfigurationConstants.AnalyticsClientPath)).ReturnsAsync("Analytics");
            this.mockConfig.Setup(c => c.GetConfigurationAsync<AnalyticsSettings>(ConfigurationConstants.AnalyticsSettings)).ReturnsAsync(this.config);

            this.client = new AnalyticsClient(this.mockFactory.Object, this.mockConfig.Object, this.mockTokenProvider.Object);
        }

        /// <summary>
        /// Sends the asynchronous should send request with bearer token asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetOwnershipAnalytics_ShouldSendRequest_WithBearerTokenAsync()
        {
            this.mockConfig.Setup(c => c.GetConfigurationAsync<bool>(ConfigurationConstants.DummyAnalyticsResponse)).ReturnsAsync(true);
            var response = await this.client.GetOwnershipAnalyticsAsync(new AnalyticalServiceRequestData()).ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Count());
            Assert.AreEqual("CHICHIMENE", response.First().TransferPoint);
            Assert.AreEqual("TRANSFER", response.Last().TransferPoint);

            Assert.IsNull(this.httpClient.DefaultRequestHeaders.Authorization);

            this.mockFactory.Verify(m => m.CreateClient(Constants.DefaultHttpClient), Times.Never);
            this.mockConfig.Verify(m => m.GetConfigurationAsync<AnalyticsSettings>(ConfigurationConstants.AnalyticsSettings), Times.Never);
            this.mockTokenProvider.Verify(m => m.GetAppTokenAsync(this.config.TenantId, this.config.Scope, this.config.ClientId, this.config.ClientSecret), Times.Never);

            this.mockConfig.Verify(m => m.GetConfigurationAsync<bool>(ConfigurationConstants.DummyAnalyticsResponse), Times.Once);
            this.mockConfig.Verify(m => m.GetConfigurationAsync<string>(ConfigurationConstants.AnalyticsClientPath), Times.Once);
        }

        /// <summary>
        /// Sends the asynchronous should send request with bearer token asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetOwnershipAnalytics_ShouldReturnDummyResponse_WhenIsDummyIsTrueAsync()
        {
            try
            {
                await this.client.GetOwnershipAnalyticsAsync(new AnalyticalServiceRequestData()).ConfigureAwait(false);
            }
            catch (UriFormatException)
            {
                Assert.IsNotNull(this.httpClient.DefaultRequestHeaders.Authorization);
                Assert.AreEqual(Constants.Bearer, this.httpClient.DefaultRequestHeaders.Authorization.Scheme);
                Assert.AreEqual("Token", this.httpClient.DefaultRequestHeaders.Authorization.Parameter);

                this.mockFactory.VerifyAll();
                this.mockTokenProvider.VerifyAll();
                this.mockConfig.VerifyAll();
            }
        }
    }
}
