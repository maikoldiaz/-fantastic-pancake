// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapClientTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Sap.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.ExceptionHandling;
    using Ecp.True.ExceptionHandling.Core;
    using Ecp.True.Ioc.Interfaces;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Sap.Interfaces;
    using Ecp.True.Proxies.Sap.Response;
    using Ecp.True.Proxies.Sap.Retry;
    using Ecp.True.Proxies.Sap.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;

    /// <summary>
    /// The Sap Client Tests.
    /// </summary>
    [TestClass]
    public class SapClientTests
    {
        /// <summary>
        /// The sap client.
        /// </summary>
        private SapClient sapClient;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<SapClient>> mockLogger;

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
            this.mockLogger = new Mock<ITrueLogger<SapClient>>();
            this.retryPolicyFactory = new RetryPolicyFactory();
            var retryHandlerLoggerMock = new Mock<ITrueLogger<SapRetryHandler>>();
            var mockResolver = new Mock<IResolver>();
            mockResolver.Setup(x => x.GetInstance<ITrueLogger<SapRetryHandler>>()).Returns(retryHandlerLoggerMock.Object);
            this.retryHandler = new SapRetryHandler(mockResolver.Object);
            this.telemetryMock = new Mock<ITelemetry>();

            this.sapClient = new SapClient(this.mockHttpClientProxy.Object, this.mockLogger.Object, this.retryPolicyFactory, this.retryHandler, this.telemetryMock.Object);
            this.sapClient.Initialize(new SapSettings() { BasePath = "https://mockfico.com", TransferPointPath = "https://mockfico.com" });
        }

        /// <summary>
        /// Ownerships the rule client post asynchronous should get HTTP response message asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SapClient_PostAsync_Should_GetHttpResponseMessageAsync()
        {
            using (var response = new HttpResponseMessage())
            {
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Content = new StringContent(JsonConvert.SerializeObject("response"));

                this.mockHttpClientProxy.Setup(x => x.SendAsync(It.IsAny<HttpMethod>(), It.IsAny<Uri>(), It.IsAny<HttpContent>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);

                var result = await this.sapClient.PostAsync("http://abc.com", "payload").ConfigureAwait(false);
                Assert.IsNotNull(result);
                this.mockHttpClientProxy.Verify(a => a.SendAsync(It.IsAny<HttpMethod>(), It.IsAny<Uri>(), It.IsAny<HttpContent>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
            }
        }

        /// <summary>
        /// Ownerships the rule client post asynchronous should get HTTP response empty message asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SapClient_PostAsync_Should_GetHttpResponseEmptyMessageAsync()
        {
            using (var response = new HttpResponseMessage())
            {
                this.mockHttpClientProxy.Setup(x => x.SendAsync(It.IsAny<HttpMethod>(), It.IsAny<Uri>(), It.IsAny<HttpContent>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(this.GetEmptyResponse);
                var result = await this.sapClient.PostAsync("http://abc.com", "payload").ConfigureAwait(false);
                Assert.IsNotNull(result);
            }
        }

        /// <summary>
        /// SAP client gets asynchronous should get HTTP response message asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SapClient_GetAsync_Should_GetHttpResponseMessageAsync()
        {
            using (var response = new HttpResponseMessage())
            {
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Content = new StringContent(JsonConvert.SerializeObject(this.GetSapMappingResponse()));

                this.mockHttpClientProxy.Setup(x => x.SendAsync(It.IsAny<HttpMethod>(), It.IsAny<Uri>(), It.IsAny<HttpContent>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);

                var result = await this.sapClient.GetAsync("http://abc.com").ConfigureAwait(false);
                Assert.IsNotNull(result);
                this.mockHttpClientProxy.Verify(a => a.SendAsync(It.IsAny<HttpMethod>(), It.IsAny<Uri>(), It.IsAny<HttpContent>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
            }
        }

        /// <summary>
        /// Gets the empty response.
        /// </summary>
        /// <returns>Returns Http Response message.</returns>
        private HttpResponseMessage GetEmptyResponse()
        {
            var response = new HttpResponseMessage();
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Content = new StringContent(string.Empty);
            return response;
        }

        private List<SapMappingResponse> GetSapMappingResponse()
        {
            return new List<SapMappingResponse>()
            {
                new SapMappingResponse
                {
                    SourceProductId = "123",
                    DestinationMovementTypeId = 1,
                    DestinationProductId = "1",
                    DestinationSystemDestinationNodeId = 1,
                    DestinationSystemSourceNodeId = 1,
                    DestinationSystemId = 1,
                    OfficialSystem = 1,
                    SourceSystemDestinationNodeId = 1,
                    SourceMovementTypeId = 1,
                    SourceSystemId = 1,
                    SourceSystemSourceNodeId = 1,
                },
            };
        }
    }
}
