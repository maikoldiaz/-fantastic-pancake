// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestHandlerTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Services.Api.Tests
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Chaos.Interfaces;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Host.Common;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using ChaosConstants = Ecp.True.Chaos.Constants;

    /// <summary>
    /// The log handler tests.
    /// </summary>
    [TestClass]
    public class RequestHandlerTests
    {
        /// <summary>
        /// The headers.
        /// </summary>
        private readonly HeaderDictionary headers = new HeaderDictionary();

        /// <summary>
        /// The configuration mock.
        /// </summary>
        private Mock<IConfigurationHandler> configurationMock;

        /// <summary>
        /// The logger mock.
        /// </summary>
        private Mock<ITrueLogger<RequestHandler>> loggerMock;

        /// <summary>
        /// The telemetry mock.
        /// </summary>
        private Mock<ITelemetry> telemetryMock;

        /// <summary>
        /// The log handler.
        /// </summary>
        private RequestHandler logHandler;

        /// <summary>
        /// The mock HTTP context.
        /// </summary>
        private Mock<HttpContext> mockHttpContext;

        /// <summary>
        /// The mock service provider.
        /// </summary>
        private Mock<IServiceProvider> mockServiceProvider;

        /// <summary>
        /// The mock business context.
        /// </summary>
        private Mock<IBusinessContext> mockBusinessContext;

        /// <summary>
        /// Gets the mock SQL token provider.
        /// </summary>
        /// <value>
        /// The mock SQL token provider.
        /// </value>
        private Mock<ISqlTokenProvider> mockSqlTokenProvider;

        /// <summary>
        /// The mock chaos manager.
        /// </summary>
        private Mock<IChaosManager> mockChaosManager;

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureClientFactory;

        /// <summary>
        /// Setups this instance.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            this.loggerMock = new Mock<ITrueLogger<RequestHandler>>();
            var httpRequest = new Mock<HttpRequest>();
            var httpResponse = new Mock<HttpResponse>();
            this.telemetryMock = new Mock<ITelemetry>();
            this.mockBusinessContext = new Mock<IBusinessContext>();
            this.mockSqlTokenProvider = new Mock<ISqlTokenProvider>();
            this.mockChaosManager = new Mock<IChaosManager>();
            this.configurationMock = new Mock<IConfigurationHandler>();
            this.mockAzureClientFactory = new Mock<IAzureClientFactory>();

            this.mockServiceProvider = new Mock<IServiceProvider>();
            this.mockServiceProvider.Setup(r => r.GetService(typeof(ITrueLogger<RequestHandler>))).Returns(this.loggerMock.Object);
            this.mockServiceProvider.Setup(r => r.GetService(typeof(ITelemetry))).Returns(this.telemetryMock.Object);
            this.mockServiceProvider.Setup(r => r.GetService(typeof(IBusinessContext))).Returns(this.mockBusinessContext.Object);
            this.mockServiceProvider.Setup(r => r.GetService(typeof(ISqlTokenProvider))).Returns(this.mockSqlTokenProvider.Object);
            this.mockServiceProvider.Setup(r => r.GetService(typeof(IChaosManager))).Returns(this.mockChaosManager.Object);
            this.mockServiceProvider.Setup(r => r.GetService(typeof(IConfigurationHandler))).Returns(this.configurationMock.Object);
            this.mockServiceProvider.Setup(r => r.GetService(typeof(IAzureClientFactory))).Returns(this.mockAzureClientFactory.Object);

            this.mockHttpContext = new Mock<HttpContext>();
            this.mockHttpContext.SetupGet(c => c.Request).Returns(httpRequest.Object);
            this.mockHttpContext.SetupGet(c => c.Response).Returns(httpResponse.Object);
            this.mockHttpContext.SetupGet(c => c.RequestServices).Returns(this.mockServiceProvider.Object);

            httpRequest.SetupGet(r => r.Headers).Returns(this.headers);
            httpResponse.SetupGet(r => r.StatusCode).Returns(StatusCodes.Status302Found);
            this.configurationMock.Setup(c => c.GetConfigurationAsync(ConfigurationConstants.ConfigConnectionString)).ReturnsAsync("Test");
            this.configurationMock.Setup(c => c.GetConfigurationAsync<BlockchainSettings>(ConfigurationConstants.BlockchainSettings)).ReturnsAsync(new BlockchainSettings());

            this.logHandler = new RequestHandler(this.NextStubAsync);
        }

        /// <summary>
        /// Invokes the should setup chaos manager when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Invoke_ShouldSetupChaosManager_WhenInvokedAsync()
        {
            this.mockChaosManager.Setup(t => t.Initialize(It.IsAny<string>()));
            this.headers.Add(ChaosConstants.ChaosHeaderName, new StringValues("Web"));

            await this.logHandler.InvokeAsync(this.mockHttpContext.Object, this.configurationMock.Object).ConfigureAwait(false);
            this.mockChaosManager.Verify(t => t.Initialize("Web"), Times.Once);
        }

        /// <summary>
        /// Invokes the should setup chaos manager when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Invoke_ShouldSetupSqlTokenProvider_WhenInvokedAsync()
        {
            this.mockSqlTokenProvider.Setup(t => t.InitializeAsync()).Returns(Task.CompletedTask);

            await this.logHandler.InvokeAsync(this.mockHttpContext.Object, this.configurationMock.Object).ConfigureAwait(false);
            this.mockSqlTokenProvider.Verify(t => t.InitializeAsync(), Times.Once);
        }

        /// <summary>
        /// Invokes the asynchronous should set activity identifier from headers when activity identifier is passed in header.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Invoke_ShouldReturnInternalServerError_WhenExceptionOccursAsync()
        {
            var exception = new NullReferenceException();
            this.loggerMock.Setup(l => l.LogError(It.IsAny<Exception>(), exception.Message)).Verifiable();

            this.logHandler = new RequestHandler(c => Task.FromException(exception));

            var context = new DefaultHttpContext();
            var request = context.Request;
            var response = context.Response;
            this.mockHttpContext.SetupGet(c => c.Response).Returns(response);
            this.mockHttpContext.SetupGet(c => c.Request).Returns(request);

            await this.logHandler.InvokeAsync(this.mockHttpContext.Object, this.configurationMock.Object).ConfigureAwait(false);

            Assert.AreEqual(500, response.StatusCode);
            this.loggerMock.Verify();
        }

        /// <summary>
        /// Invokes the asynchronous should set activity identifier from headers when activity identifier is passed in header.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Invoke_ShouldReturnUnauthorizedStatusCode_WhenUnauthorizedExceptionOccursAsync()
        {
            var exception = new UnauthorizedAccessException();
            this.loggerMock.Setup(l => l.LogError(It.IsAny<Exception>(), exception.Message)).Verifiable();

            this.logHandler = new RequestHandler(c => Task.FromException(exception));

            var context = new DefaultHttpContext();
            var request = context.Request;
            var response = context.Response;
            this.mockHttpContext.SetupGet(c => c.Response).Returns(response);
            this.mockHttpContext.SetupGet(c => c.Request).Returns(request);

            await this.logHandler.InvokeAsync(this.mockHttpContext.Object, this.configurationMock.Object).ConfigureAwait(false);

            Assert.AreEqual(401, response.StatusCode);
            this.loggerMock.Verify();
        }

        /// <summary>
        /// Invokes the asynchronous should set activity identifier from headers when activity identifier is passed in header.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task Invoke_ShouldThrowException_WhenThrowExceptionsIsTrueAsync()
        {
            this.configurationMock.Setup(c => c.GetConfigurationAsync<bool>(ConfigurationConstants.ThrowExceptions)).ReturnsAsync(true);

            this.logHandler = new RequestHandler(c => Task.FromException(new NullReferenceException()));
            await this.logHandler.InvokeAsync(this.mockHttpContext.Object, this.configurationMock.Object).ConfigureAwait(false);
        }

        /// <summary>
        /// Invokes the asynchronous should set activity identifier from headers when activity identifier is passed in header.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Invoke_ShouldUseDefaultActivityId_WhenActivityIdIsNotPassedAndGenerateRequestIsFalseAsync()
        {
            this.configurationMock.Setup(c => c.GetConfigurationAsync<bool>(ConfigurationConstants.GenerateRequestId)).ReturnsAsync(false);
            await this.logHandler.InvokeAsync(this.mockHttpContext.Object, this.configurationMock.Object).ConfigureAwait(false);

            Assert.AreEqual(0, this.headers.Count);
        }

        /// <summary>
        /// Invokes the asynchronous should set activity identifier from headers when activity identifier is passed in header.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Invoke_ShouldUseDefaultActivityId_WhenEmptyActivityIdIsPassedAndGenerateRequestIsFalseAsync()
        {
            this.configurationMock.Setup(c => c.GetConfigurationAsync<bool>(ConfigurationConstants.GenerateRequestId)).ReturnsAsync(false);
            this.headers.Add(CorrelationInfo.OperationId, new StringValues(string.Empty));
            await this.logHandler.InvokeAsync(this.mockHttpContext.Object, this.configurationMock.Object).ConfigureAwait(false);

            Assert.AreEqual(1, this.headers.Count);
        }

        /// <summary>
        /// Invokes the asynchronous should set activity identifier from headers when activity identifier is passed in header.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Invoke_ShouldUseDefaultActivityId_WhenNullActivityIdIsPassedAndGenerateRequestIsFalseAsync()
        {
            this.configurationMock.Setup(c => c.GetConfigurationAsync<bool>(ConfigurationConstants.GenerateRequestId)).ReturnsAsync(false);
            this.headers.Add(CorrelationInfo.OperationId, new StringValues(default(string)));
            await this.logHandler.InvokeAsync(this.mockHttpContext.Object, this.configurationMock.Object).ConfigureAwait(false);

            Assert.AreEqual(1, this.headers.Count);
        }

        /// <summary>
        /// Nexts the stub.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The task.</returns>
        private Task NextStubAsync(HttpContext context)
        {
            return Task.FromResult(context.Response);
        }
    }
}