// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapProxyTests.cs" company="Microsoft">
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
    using System.Net.Http;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.Sap.Interfaces;
    using Ecp.True.Proxies.Sap.Request;
    using Ecp.True.Proxies.Sap.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The Sap Proxy Tests.
    /// </summary>
    [TestClass]
    public class SapProxyTests
    {
        /// <summary>
        /// The sap proxy.
        /// </summary>
        private SapProxy sapProxy;

        /// <summary>
        /// The mock sap client.
        /// </summary>
        private Mock<ISapClient> mockSapClient;

        /// <summary>
        /// The mock configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigurationHandler;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<SapProxy>> mockLogger;

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureClientFactory;

        /// <summary>
        /// Intializes the method.
        /// </summary>
        [TestInitialize]
        public void IntializeMethod()
        {
            this.mockSapClient = new Mock<ISapClient>();
            this.mockConfigurationHandler = new Mock<IConfigurationHandler>();
            this.mockLogger = new Mock<ITrueLogger<SapProxy>>();
            this.mockAzureClientFactory = new Mock<IAzureClientFactory>();

            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<SapSettings>(It.IsAny<string>())).ReturnsAsync(new SapSettings() { TransferPointPath = string.Empty });

            this.sapProxy = new SapProxy(this.mockConfigurationHandler.Object, this.mockSapClient.Object, this.mockLogger.Object, this.mockAzureClientFactory.Object);
        }

        /// <summary>
        /// The Sap Proxy update transfer points.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [Obsolete("This Method is Deprecated", false)]
        public async Task SapProxy_UpdateTransferPointAsync_Should_GetActiveRulesAsync()
        {
            using (var response = this.GetSapResponse("{IsSuccessStatusCode: true}"))
            {
                var sapMovement = new SapMovementRequest() { MovementId = "1" };
                this.mockSapClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(response);
                this.mockAzureClientFactory.Setup(x => x.GetBlobStorageSaSClient(It.IsAny<string>(), It.IsAny<string>()).CreateBlobAsync(It.IsAny<string>())).Verifiable();
                var result = await this.sapProxy.UpdateMovementTransferPointAsync(sapMovement, 1).ConfigureAwait(false);
                Assert.IsNotNull(result);
                this.mockSapClient.Verify(a => a.PostAsync(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
                this.mockSapClient.Verify(a => a.Initialize(It.IsAny<SapSettings>()), Times.Once);
                this.mockConfigurationHandler.Verify(a => a.GetConfigurationAsync<SapSettings>(It.IsAny<string>()), Times.Once);
                this.mockAzureClientFactory.Verify(x => x.GetBlobStorageSaSClient(It.IsAny<string>(), It.IsAny<string>()).CreateBlobAsync(It.IsAny<string>()), Times.Once);
            }
        }

        [TestMethod]
        public async Task SapProxy_UpdateUploadStatusAsync_Should_GetActiveRulesAsync()
        {
            using (var response = this.GetSapResponse(string.Empty))
            {
                this.mockSapClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(response);
                var uploadStatusResult = await this.sapProxy.UpdateUploadStatusAsync(It.IsAny<SapUploadStatus>()).ConfigureAwait(false);
                Assert.IsNotNull(uploadStatusResult);
                var uploadStatusContractResult = await this.sapProxy.UpdateUploadStatusContractAsync(It.IsAny<SapUploadStatusContract>()).ConfigureAwait(false);
                Assert.IsNotNull(uploadStatusContractResult);
                this.mockSapClient.Verify(a => a.PostAsync(It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(2));
                this.mockSapClient.Verify(a => a.Initialize(It.IsAny<SapSettings>()), Times.Exactly(2));
                this.mockConfigurationHandler.Verify(a => a.GetConfigurationAsync<SapSettings>(It.IsAny<string>()), Times.Exactly(2));
            }
        }

        /// <summary>
        /// Gets the sap  response.
        /// </summary>
        /// <returns>Returns Http Response message.</returns>
        private HttpResponseMessage GetSapResponse(string content)
        {
            var response = new HttpResponseMessage();
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Content = new StringContent(content);
            return response;
        }
    }
}
