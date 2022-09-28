// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AvailabilityProcessorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Availability.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Availability;
    using Ecp.True.Processors.Availability.Interfaces;
    using Ecp.True.Processors.Availability.Response;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The AvailabilityProcessorTests.
    /// </summary>
    [TestClass]
    public class AvailabilityProcessorTests
    {
        /// <summary>
        /// The mock resource service.
        /// </summary>
        private Mock<IResourceService> mockResourceService;

        /// <summary>
        /// The mock metric service.
        /// </summary>
        private Mock<IMetricService> mockMetricService;

        /// <summary>
        /// The availability processor.
        /// </summary>
        private AvailabilityProcessor availabilityProcessor;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<AvailabilityProcessor>> mockLogger;

        /// <summary>
        /// The mock configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigurationHandler;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<AvailabilityProcessor>>();
            this.mockResourceService = new Mock<IResourceService>();
            this.mockMetricService = new Mock<IMetricService>();
            this.mockConfigurationHandler = new Mock<IConfigurationHandler>();
            this.availabilityProcessor = new AvailabilityProcessor(this.mockLogger.Object, this.mockResourceService.Object, this.mockMetricService.Object, this.mockConfigurationHandler.Object);
        }

        /// <summary>
        /// Availabilities the processor check and report availability no metric API called asynchronous.
        /// </summary>
        /// <returns>the task.</returns>
        [TestMethod]
        public async Task AvailabilityProcessor_CheckAndReportAvailability_NoMetricApiCalled_Async()
        {
            var availabilitySettings = new AvailabilitySettings
            {
                ResourceId = "ResourceId",
                ClientId = "ClientId",
                ClientSecret = "ClientSecret",
                TenantId = "TenantId",
            };
            availabilitySettings.ResourceGroups.Add(new ResourceGroup { SubscriptionId = "SubscriptionId", Name = "ResourceName" });

            //// Resource detail collections
            var resourceDetailsCollections = new List<ResourceDetails>();
            var resourcedetail = new ResourceDetails
            {
                Id = "http://ResourceId1",
                Name = "ResourceId1",
            };
            resourceDetailsCollections.Add(resourcedetail);

            //// Resource availability collections
            var resourceAvailabilityCollections = new List<ResourceAvailability>();
            var resourceAvailability = new ResourceAvailability
            {
                Id = "http://ResourceId1",
                AvailabilityState = "Available",
            };
            resourceAvailabilityCollections.Add(resourceAvailability);

            //// Module availability collections
            var moduleAvailabilitySettingsCollections = new List<ModuleAvailabilitySettings>();
            var moduleAvailabilitySetting = new ModuleAvailabilitySettings
            {
                Name = "TrueAdmin",
            };
            moduleAvailabilitySetting.Resources.Add("ResourceId1");
            moduleAvailabilitySettingsCollections.Add(moduleAvailabilitySetting);

            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<AvailabilitySettings>(It.IsAny<string>())).ReturnsAsync(availabilitySettings);
            this.mockResourceService.Setup(x => x.GetResourcesAsync(It.IsAny<AvailabilitySettings>())).ReturnsAsync(resourceDetailsCollections);
            this.mockResourceService.Setup(x => x.GetAvailabilityAsync(It.IsAny<AvailabilitySettings>())).ReturnsAsync(resourceAvailabilityCollections);
            this.mockConfigurationHandler.Setup(x => x.GetCollectionConfigurationAsync<ModuleAvailabilitySettings>(It.IsAny<string>())).ReturnsAsync(moduleAvailabilitySettingsCollections);
            this.mockMetricService.Setup(x => x.ReportAvailability(It.IsAny<IEnumerable<ResourceAvailability>>(), It.IsAny<IEnumerable<ModuleAvailabilitySettings>>(), It.IsAny<TimeSpan>(), It.IsAny<bool>()));

            await this.availabilityProcessor.CheckAndReportAvailabilityAsync(false).ConfigureAwait(false);

            this.mockConfigurationHandler.Verify(x => x.GetConfigurationAsync<AvailabilitySettings>(It.IsAny<string>()), Times.Once);
            this.mockResourceService.Verify(x => x.GetResourcesAsync(It.IsAny<AvailabilitySettings>()), Times.Once);
            this.mockResourceService.Verify(x => x.GetAvailabilityAsync(It.IsAny<AvailabilitySettings>()), Times.Once);
            this.mockConfigurationHandler.Verify(x => x.GetCollectionConfigurationAsync<ModuleAvailabilitySettings>(It.IsAny<string>()), Times.Once);
            this.mockMetricService.Verify(x => x.ReportAvailability(It.IsAny<IEnumerable<ResourceAvailability>>(), It.IsAny<IEnumerable<ModuleAvailabilitySettings>>(), It.IsAny<TimeSpan>(), It.IsAny<bool>()), Times.Once);
        }

        /// <summary>
        /// Availabilities the processor check and report availability asynchronous.
        /// </summary>
        /// <returns>the task.</returns>
        [TestMethod]
        public async Task AvailabilityProcessor_CheckAndReportAvailability_Async()
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

            //// Resource detail collections
            var resourceDetailsCollections = new List<ResourceDetails>();
            var resourcedetail1 = new ResourceDetails
            {
                Id = "http://ResourceId1",
                Name = "ResourceId1",
            };
            var resourcedetail2 = new ResourceDetails
            {
                Id = "http://ResourceId1",
                Name = "ResourceId2",
            };
            resourceDetailsCollections.Add(resourcedetail1);
            resourceDetailsCollections.Add(resourcedetail2);

            //// Resource availability collections
            var resourceAvailabilityCollections1 = new List<ResourceAvailability>();
            var resourceAvailability1 = new ResourceAvailability
            {
                Id = "http://ResourceId1",
                AvailabilityState = "Available",
            };
            resourceAvailabilityCollections1.Add(resourceAvailability1);

            //// Module availability collections
            var moduleAvailabilitySettingsCollections = new List<ModuleAvailabilitySettings>();
            var moduleAvailabilitySetting = new ModuleAvailabilitySettings
            {
                Name = "TrueAdmin",
            };
            moduleAvailabilitySetting.Resources.Add("ResourceId1");
            moduleAvailabilitySetting.Resources.Add("ResourceId2");
            moduleAvailabilitySettingsCollections.Add(moduleAvailabilitySetting);

            //// Metric resource availability collections
            var resourceAvailabilityCollections2 = new List<ResourceAvailability>();
            var resourceAvailability2 = new ResourceAvailability
            {
                Id = "http://ResourceId2",
                AvailabilityState = "Available",
            };
            resourceAvailabilityCollections2.Add(resourceAvailability2);

            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<AvailabilitySettings>(It.IsAny<string>())).ReturnsAsync(availabilitySettings);
            this.mockResourceService.Setup(x => x.GetResourcesAsync(It.IsAny<AvailabilitySettings>())).ReturnsAsync(resourceDetailsCollections);
            this.mockResourceService.Setup(x => x.GetAvailabilityAsync(It.IsAny<AvailabilitySettings>())).ReturnsAsync(resourceAvailabilityCollections1);
            this.mockConfigurationHandler.Setup(x => x.GetCollectionConfigurationAsync<ModuleAvailabilitySettings>(It.IsAny<string>())).ReturnsAsync(moduleAvailabilitySettingsCollections);
            this.mockMetricService.Setup(x => x.GetAvailabilityAsync(It.IsAny<IEnumerable<ResourceDetails>>(), It.IsAny<AvailabilitySettings>())).ReturnsAsync(resourceAvailabilityCollections2);
            this.mockMetricService.Setup(x => x.ReportAvailability(It.IsAny<IEnumerable<ResourceAvailability>>(), It.IsAny<IEnumerable<ModuleAvailabilitySettings>>(), It.IsAny<TimeSpan>(), It.IsAny<bool>()));

            await this.availabilityProcessor.CheckAndReportAvailabilityAsync(false).ConfigureAwait(false);

            this.mockConfigurationHandler.Verify(x => x.GetConfigurationAsync<AvailabilitySettings>(It.IsAny<string>()), Times.Once);
            this.mockResourceService.Verify(x => x.GetResourcesAsync(It.IsAny<AvailabilitySettings>()), Times.Once);
            this.mockResourceService.Verify(x => x.GetAvailabilityAsync(It.IsAny<AvailabilitySettings>()), Times.Once);
            this.mockConfigurationHandler.Verify(x => x.GetCollectionConfigurationAsync<ModuleAvailabilitySettings>(It.IsAny<string>()), Times.Once);
            this.mockMetricService.Verify(x => x.GetAvailabilityAsync(It.IsAny<IEnumerable<ResourceDetails>>(), It.IsAny<AvailabilitySettings>()), Times.Once);
            this.mockMetricService.Verify(x => x.ReportAvailability(It.IsAny<IEnumerable<ResourceAvailability>>(), It.IsAny<IEnumerable<ModuleAvailabilitySettings>>(), It.IsAny<TimeSpan>(), It.IsAny<bool>()), Times.Once);
        }

        /// <summary>
        /// Availabilities the processor check and report availability no avaiability settings asynchronous.
        /// </summary>
        /// <returns>the task.</returns>
        [TestMethod]
        public async Task AvailabilityProcessor_CheckAndReportAvailability_NoAvaiabilitySettings_Async()
        {
            this.mockConfigurationHandler.Setup(x => x.GetConfigurationAsync<AvailabilitySettings>(It.IsAny<string>()));

            await this.availabilityProcessor.CheckAndReportAvailabilityAsync(false).ConfigureAwait(false);

            this.mockConfigurationHandler.Verify(x => x.GetConfigurationAsync<AvailabilitySettings>(It.IsAny<string>()), Times.Once);
        }
    }
}
