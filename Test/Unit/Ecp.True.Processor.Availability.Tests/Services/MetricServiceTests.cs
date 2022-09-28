// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetricServiceTests.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Availability.Response;
    using Ecp.True.Processors.Availability.Services;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The MetricServiceTests.
    /// </summary>
    [TestClass]
    public class MetricServiceTests
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
        /// The metric service.
        /// </summary>
        private MetricService metricService;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private Mock<ITelemetry> mockTelemetry;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<MetricService>> mockLogger;

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
            this.mockTelemetry = new Mock<ITelemetry>();
            this.mockLogger = new Mock<ITrueLogger<MetricService>>();
            this.azureClientFactory = new AzureClientFactory(
                null,
                null,
                null,
                null,
                this.azureManagementApiClient.Object);
            this.metricService = new MetricService(this.mockLogger.Object, this.mockTelemetry.Object, this.azureClientFactory);
        }

        /// <summary>
        /// Metrics the service get resources asynchronous asynchronous.
        /// </summary>
        /// <returns>the task.</returns>
        [TestMethod]
        public async Task MetricService_GetResources_Async()
        {
            var collections = new List<ResourceDetails>();
            var resourceDetails = new ResourceDetails
            {
                Id = "ResourceId",
                Name = "ResourceName",
                Type = "ResourceType",
            };

            collections.Add(resourceDetails);
            var response = "{\"value\":[{\"errorCode\":\"Success\"}]}";

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

            this.azureManagementApiClient.Setup(m => m.GetAsync(It.IsAny<Uri>())).ReturnsAsync(response);
            this.mockAzureClientFactory.Setup(m => m.AzureManagementApiClient).Returns(this.azureManagementApiClient.Object);

            var result = await this.metricService.GetAvailabilityAsync(collections, availabilitySettings).ConfigureAwait(false);
            Assert.IsNotNull(result);
            Assert.AreEqual("ResourceId", result.ElementAt(0).Id);
            Assert.AreEqual("Available", result.ElementAt(0).AvailabilityState);
            Assert.AreEqual("Resource : ResourceName reported available from metric api.", result.ElementAt(0).Message);
            this.azureManagementApiClient.Verify(m => m.GetAsync(It.IsAny<Uri>()), Times.Once);
        }

        /// <summary>
        /// Metrics the service get resources asynchronous asynchronous.
        /// </summary>
        /// <returns>the task.</returns>
        [TestMethod]
        public async Task MetricService_GetNoResponse_Async()
        {
            var collections = new List<ResourceDetails>();
            var resourceDetails = new ResourceDetails
            {
                Id = "ResourceId",
                Name = "ResourceName",
                Type = "ResourceType",
            };

            collections.Add(resourceDetails);

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

            var result = await this.metricService.GetAvailabilityAsync(collections, availabilitySettings).ConfigureAwait(false);
            Assert.IsNotNull(result);
            Assert.AreEqual("ResourceId", result.ElementAt(0).Id);
            Assert.AreEqual("Unavailable", result.ElementAt(0).AvailabilityState);
        }

        /// <summary>
        /// Metrics the service report availability.
        /// </summary>
        [TestMethod]
        public void MetricService_ReportAvailability()
        {
            var resourceAvailabilityCollections = new List<ResourceAvailability>();
            var resourceAvailability = new ResourceAvailability();
            resourceAvailability.Id = "ResourceId";
            resourceAvailability.AvailabilityState = "Available";
            resourceAvailabilityCollections.Add(resourceAvailability);

            var moduleAvailabilitySettingsCollections = new List<ModuleAvailabilitySettings>();
            var moduleAvailabilitySetting = new ModuleAvailabilitySettings();
            moduleAvailabilitySetting.Name = "ModuleName";
            moduleAvailabilitySetting.Resources.Add("ResourceName");
            moduleAvailabilitySettingsCollections.Add(moduleAvailabilitySetting);

            this.metricService.ReportAvailability(resourceAvailabilityCollections, moduleAvailabilitySettingsCollections, default(TimeSpan), false);
            Assert.IsTrue(true);
            this.azureManagementApiClient.Verify(m => m.GetAsync(It.IsAny<Uri>()), Times.Never);
        }

        /// <summary>
        /// Metrics the service report un availability.
        /// </summary>
        [TestMethod]
        public void MetricService_ReportUnAvailability()
        {
            var resourceAvailabilityCollections = new List<ResourceAvailability>();
            var resourceAvailability = new ResourceAvailability();
            resourceAvailability.Id = "ResourceName";
            resourceAvailability.AvailabilityState = "UnAvailable";
            resourceAvailabilityCollections.Add(resourceAvailability);

            var moduleAvailabilitySettingsCollections = new List<ModuleAvailabilitySettings>();
            var moduleAvailabilitySetting = new ModuleAvailabilitySettings();
            moduleAvailabilitySetting.Name = "ModuleName";
            moduleAvailabilitySetting.Resources.Add("ResourceName");
            moduleAvailabilitySettingsCollections.Add(moduleAvailabilitySetting);

            this.metricService.ReportAvailability(resourceAvailabilityCollections, moduleAvailabilitySettingsCollections, default(TimeSpan), false);
            Assert.IsTrue(true);
            this.azureManagementApiClient.Verify(m => m.GetAsync(It.IsAny<Uri>()), Times.Never);
        }
    }
}
