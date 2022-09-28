// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AvailabilityGeneratorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Tests
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Configuration.Entities;
    using Ecp.True.Host.Functions.Availability;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Availability.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Timers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class AvailabilityGeneratorTests
    {
        /// <summary>
        /// The availability generator.
        /// </summary>
        private AvailabilityGenerator availabilityGenerator;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<AvailabilityGenerator>> mockLogger;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private Mock<IServiceProvider> mockServiceProvider;

        /// <summary>
        /// The mock connection factory.
        /// </summary>
        private Mock<IConnectionFactory> mockConnectionFactory;

        /// <summary>
        /// The mock configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigHandler;

        /// <summary>
        /// The mock availability processor.
        /// </summary>
        private Mock<IAvailabilityProcessor> mockAvailabilityProcessor;

        /// <summary>
        /// Gets the mock SQL token provider.
        /// </summary>
        /// <value>
        /// The mock SQL token provider.
        /// </value>
        private Mock<ISqlTokenProvider> mockSqlTokenProvider;

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureClientFactory;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private Mock<ITelemetry> mockTelemetry;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<AvailabilityGenerator>>();
            this.mockServiceProvider = new Mock<IServiceProvider>();
            this.mockSqlTokenProvider = new Mock<ISqlTokenProvider>();
            this.mockConfigHandler = new Mock<IConfigurationHandler>();
            this.mockAzureClientFactory = new Mock<IAzureClientFactory>();
            this.mockTelemetry = new Mock<ITelemetry>();
            this.mockServiceProvider.Setup(r => r.GetService(typeof(ISqlTokenProvider))).Returns(this.mockSqlTokenProvider.Object);
            this.mockAvailabilityProcessor = new Mock<IAvailabilityProcessor>();
            this.availabilityGenerator = new AvailabilityGenerator(
                this.mockLogger.Object,
                this.mockConfigHandler.Object,
                this.mockAzureClientFactory.Object,
                this.mockServiceProvider.Object,
                this.mockAvailabilityProcessor.Object,
                this.mockTelemetry.Object);
        }

        [TestMethod]
        public async Task AvailabilityGenerator_ServicesAvailabilityAsync()
        {
            this.SetupMocks(true);
            this.mockAvailabilityProcessor.Setup(a => a.CheckAndReportAvailabilityAsync(false)).Returns(Task.CompletedTask);

            var timerInfo = new TimerInfo(null, new ScheduleStatus(), false);
            await this.availabilityGenerator.ServicesAvailabilityAsync(timerInfo, new ExecutionContext() { InvocationId = Guid.NewGuid() }).ConfigureAwait(false);

            this.mockAvailabilityProcessor.Verify(a => a.CheckAndReportAvailabilityAsync(false), Times.Once);
        }

        [TestMethod]
        public async Task AvailabilityGenerator_ThrowException_ServicesAvailabilityAsync()
        {
            this.SetupMocks(false);

            var timerInfo = new TimerInfo(null, new ScheduleStatus(), false);
            await this.availabilityGenerator.ServicesAvailabilityAsync(timerInfo, new ExecutionContext() { InvocationId = Guid.NewGuid() }).ConfigureAwait(false);
            this.mockAvailabilityProcessor.Verify(a => a.CheckAndReportAvailabilityAsync(false), Times.Never);
        }

        /// <summary>
        /// Setups the mocks.
        /// </summary>
        /// <param name="isReady">if set to <c>true</c> [is ready].</param>
        private void SetupMocks(bool isReady)
        {
            this.mockConnectionFactory = new Mock<IConnectionFactory>();

            this.mockConnectionFactory.Setup(m => m.SetupStorageConnection(It.IsAny<string>()));
            this.mockConnectionFactory.SetupGet(m => m.IsReady).Returns(isReady);

            this.mockAzureClientFactory.SetupGet(m => m.IsReady).Returns(isReady);
            this.mockAzureClientFactory.Setup(m => m.Initialize(It.IsAny<AzureConfiguration>()));

            this.mockServiceProvider.Setup(s => s.GetService(typeof(IConnectionFactory))).Returns(this.mockConnectionFactory.Object);

            this.mockServiceProvider.Setup(s => s.GetService(typeof(ITrueLogger<AvailabilityGenerator>))).Returns(this.mockLogger.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IConfigurationHandler))).Returns(this.mockConfigHandler.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IAvailabilityProcessor))).Returns(this.mockAvailabilityProcessor.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IAzureClientFactory))).Returns(this.mockAzureClientFactory.Object);

            var analysisSettings = new AnalysisSettings
            {
                ClientId = "ClientID",
                ClientSecret = "ClientSecret",
                ModelName = "ModelName",
                Region = "Region",
                ServerName = "ServerName",
                TenantId = "TenantID",
            };
            this.mockConfigHandler.Setup(m => m.GetConfigurationAsync<AnalysisSettings>(ConfigurationConstants.AnalysisSettings))
                .ReturnsAsync(analysisSettings);
            var storageSettings = new StorageSettings
            {
                MaxAttempts = 5,
                DeltaBackOff = 2,
                ConnectionString = "ConnectionString",
            };
            this.mockConfigHandler.Setup(m => m.GetConfigurationAsync<StorageSettings>(ConfigurationConstants.StorageSettings))
                .ReturnsAsync(storageSettings);

            this.mockConfigHandler.Setup(m => m.GetConfigurationAsync<ServiceBusSettings>(ConfigurationConstants.ServiceBusSettings))
           .ReturnsAsync(new ServiceBusSettings());
        }
    }
}
