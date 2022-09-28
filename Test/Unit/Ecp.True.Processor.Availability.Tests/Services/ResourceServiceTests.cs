// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceServiceTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Availability.Tests.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Availability.Services;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The ResourceServiceTests.
    /// </summary>
    [TestClass]
    public class ResourceServiceTests
    {
        /// <summary>
        /// The client.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The azure management API client.
        /// </summary>
        private Mock<IAzureManagementApiClient> azureManagementApiClient;

        /// <summary>
        /// The resource service.
        /// </summary>
        private ResourceService resourceService;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<ResourceService>> mockLogger;

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private IAzureClientFactory azureClientFactory;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.azureManagementApiClient = new Mock<IAzureManagementApiClient>();
            this.mockLogger = new Mock<ITrueLogger<ResourceService>>();
            this.azureClientFactory = new AzureClientFactory(
                null,
                null,
                null,
                null,
                this.azureManagementApiClient.Object);
            this.resourceService = new ResourceService(this.mockLogger.Object, this.azureClientFactory);
        }

        /// <summary>
        /// Resources the service get resources asynchronous.
        /// </summary>
        /// <returns>the task.</returns>
        [TestMethod]
        public async Task ResourceService_GetResourcesAsync_Async()
        {
            var availabilitySettings = new AvailabilitySettings
            {
                ResourceId = "ResourceId",
                ClientId = "ClientId",
                ClientSecret = "ClientSecret",
                TenantId = "TenantId",
                AvailabilityPath = "https://test/{0}/resourceGroups/{1}/providers/Microsoft.ResourceHealth/availabilityStatuses?api-version=2020-05-01",
                ResourceDetailPath = "https://test/{0}/resourceGroups/{1}/resources?api-version=2019-10-01",
                MetricPath = "https://test/{0}/providers/microsoft.insights/metrics?api-version=2018-01-01",
            };
            availabilitySettings.ResourceGroups.Add(new ResourceGroup { SubscriptionId = "SubscriptionId", Name = "ResourceName" });

            var response = "{\"value\":[{\"id\":\"TestResourceId\",\"name\":\"TestResourceName\",\"type\":\"TestResourceType\"}]}";

            this.azureManagementApiClient.Setup(m => m.GetAsync(It.IsAny<Uri>())).ReturnsAsync(response);
            this.mockAzureClientFactory.Setup(m => m.AzureManagementApiClient).Returns(this.azureManagementApiClient.Object);

            var result = await this.resourceService.GetResourcesAsync(availabilitySettings).ConfigureAwait(false);
            Assert.IsNotNull(result);
            Assert.AreEqual("TestResourceId", result.ElementAt(0).Id);
            Assert.AreEqual("TestResourceName", result.ElementAt(0).Name);
            Assert.AreEqual("TestResourceType", result.ElementAt(0).Type);
            this.azureManagementApiClient.Verify(m => m.GetAsync(It.IsAny<Uri>()), Times.Once);
        }

        /// <summary>
        /// Resources the service get resources availability asynchronous.
        /// </summary>
        /// <returns>the task.</returns>
        [TestMethod]
        public async Task ResourceService_GetAvailabilityAsync_Async()
        {
            var availabilitySettings = new AvailabilitySettings
            {
                ResourceId = "ResourceId",
                ClientId = "ClientId",
                ClientSecret = "ClientSecret",
                TenantId = "TenantId",
                AvailabilityPath = "https://test/{0}/resourceGroups/{1}/providers/Microsoft.ResourceHealth/availabilityStatuses?api-version=2020-05-01",
                ResourceDetailPath = "https://test/{0}/resourceGroups/{1}/resources?api-version=2019-10-01",
                MetricPath = "https://test/{0}/providers/microsoft.insights/metrics?api-version=2018-01-01",
            };
            availabilitySettings.ResourceGroups.Add(new ResourceGroup { SubscriptionId = "SubscriptionId", Name = "ResourceName" });

            var response = "{\"value\":[{ \"id\":\"TestResourceId\",\"properties\":{\"availabilityState\":\"Available\",\"summary\":\"This server is running normally.\"} }]}";

            this.azureManagementApiClient.Setup(m => m.GetAsync(It.IsAny<Uri>())).ReturnsAsync(response);
            this.mockAzureClientFactory.Setup(m => m.AzureManagementApiClient).Returns(this.azureManagementApiClient.Object);

            var result = await this.resourceService.GetAvailabilityAsync(availabilitySettings).ConfigureAwait(false);
            Assert.IsNotNull(result);
            Assert.AreEqual("TestResourceId", result.ElementAt(0).Id);
            Assert.AreEqual("Available", result.ElementAt(0).AvailabilityState);
            Assert.AreEqual("This server is running normally.", result.ElementAt(0).Message);
            this.azureManagementApiClient.Verify(m => m.GetAsync(It.IsAny<Uri>()), Times.Once);
        }
    }
}
