// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrueRpcClientTests.cs" company="Microsoft">
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
    using System.Net.Http;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Proxies.Azure.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The TRUE RPC Client Tests.
    /// </summary>
    [TestClass]
    public class TrueRpcClientTests
    {
        /// <summary>
        /// The client.
        /// </summary>
        private TrueRpcClient client;

        /// <summary>
        /// The profile.
        /// </summary>
        private QuorumProfile profile;

        /// <summary>
        /// The factory mock.
        /// </summary>
        private Mock<IHttpClientFactory> mockFactory;

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
            this.profile = new QuorumProfile
            {
                RpcEndpoint = "https://abstest.azure.com:3200/12345",
                ClientId = "ClientId",
                ClientSecret = "ClientSecret",
                TenantId = "TenantId",
                ResourceId = "ResourceId",
            };

            this.mockFactory = new Mock<IHttpClientFactory>();
            this.mockTokenProvider = new Mock<ITokenProvider>();

            this.httpClient = new HttpClient();
            this.mockFactory.Setup(m => m.CreateClient(Constants.DefaultHttpClient)).Returns(this.httpClient);

            this.mockTokenProvider.Setup(m => m.GetAppTokenAsync(this.profile.TenantId, this.profile.Resource, this.profile.ClientId, this.profile.ClientSecret)).ReturnsAsync("Token");

            this.client = new TrueRpcClient(this.profile, this.mockFactory.Object, this.mockTokenProvider.Object);
        }

        /// <summary>
        /// Sends the asynchronous should send request with bearer token asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SendAsync_ShouldSendRequest_WithBearerTokenAsync()
        {
            try
            {
                await this.client.SendRequestAsync("test").ConfigureAwait(false);
            }
            catch (HttpRequestException)
            {
                Assert.AreEqual(this.profile.RpcEndpoint, this.httpClient.BaseAddress.OriginalString);
                Assert.IsNotNull(this.httpClient.DefaultRequestHeaders.Authorization);
                Assert.AreEqual(Constants.Bearer, this.httpClient.DefaultRequestHeaders.Authorization.Scheme);
                Assert.AreEqual("Token", this.httpClient.DefaultRequestHeaders.Authorization.Parameter);

                this.mockFactory.VerifyAll();
                this.mockTokenProvider.VerifyAll();
            }
        }
    }
}
