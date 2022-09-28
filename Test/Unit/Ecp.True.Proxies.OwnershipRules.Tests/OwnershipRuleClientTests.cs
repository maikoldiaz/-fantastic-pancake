// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipRuleClientTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.OwnershipRules.Tests
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.ExceptionHandling;
    using Ecp.True.ExceptionHandling.Core;
    using Ecp.True.Ioc.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.OwnershipRules.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.Retry;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;

    /// <summary>
    /// The Ownership Rule Client Tests.
    /// </summary>
    [TestClass]
    public class OwnershipRuleClientTests
    {
        /// <summary>
        /// The ownership rule client.
        /// </summary>
        private OwnershipRuleClient ownershipRuleClient;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<OwnershipRuleClient>> mockLogger;

        /// <summary>
        /// The mock HTTP client proxy.
        /// </summary>
        private Mock<IHttpClientProxy> mockHttpClientProxy;

        /// <summary>
        /// The retry policy factory.
        /// </summary>
        private IRetryPolicyFactory retryPolicyFactory;

        /// <summary>
        /// The retry handler.
        /// </summary>
        private IRetryHandler retryHandler;

        /// <summary>
        /// The telemetry mock.
        /// </summary>
        private Mock<ITelemetry> telemetryMock;

        /// <summary>
        /// Intializes the method.
        /// </summary>
        [TestInitialize]
        public void IntializeMethod()
        {
            this.mockHttpClientProxy = new Mock<IHttpClientProxy>();
            this.mockLogger = new Mock<ITrueLogger<OwnershipRuleClient>>();
            this.retryPolicyFactory = new RetryPolicyFactory();
            var retryHandlerLoggerMock = new Mock<ITrueLogger<OwnershipRuleRetryHandler>>();
            var mockResolver = new Mock<IResolver>();
            mockResolver.Setup(x => x.GetInstance<ITrueLogger<OwnershipRuleRetryHandler>>()).Returns(retryHandlerLoggerMock.Object);
            this.retryHandler = new OwnershipRuleRetryHandler(mockResolver.Object);
            this.telemetryMock = new Mock<ITelemetry>();

            this.ownershipRuleClient = new OwnershipRuleClient(this.mockHttpClientProxy.Object, this.mockLogger.Object, this.retryPolicyFactory, this.retryHandler, this.telemetryMock.Object);
            this.ownershipRuleClient.Initialize(new OwnershipRuleSettings() { BasePath = "https://mockfico.com", RegistrationPath = "https://mockfico.com" });
        }

        /// <summary>
        /// Ownerships the rule client post asynchronous should get HTTP response message asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task OwnershipRuleClient_PostAsync_Should_GetHttpResponseMessageAsync()
        {
            using (var response = new HttpResponseMessage())
            {
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Content = new StringContent(JsonConvert.SerializeObject("response"));

                this.mockHttpClientProxy.Setup(x => x.SendAsync(It.IsAny<HttpMethod>(), It.IsAny<Uri>(), It.IsAny<StringContent>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>())).ReturnsAsync(response);

                var result = await this.ownershipRuleClient.PostAsync("http://abc.com", "payload").ConfigureAwait(false);
                Assert.IsNotNull(result);
                this.mockHttpClientProxy.Verify(a => a.SendAsync(It.IsAny<HttpMethod>(), It.IsAny<Uri>(), It.IsAny<StringContent>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Exactly(2));
            }
        }
    }
}
