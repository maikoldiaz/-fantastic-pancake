// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessLogisticMovementTest.cs" company="Microsoft">
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
    using Ecp.True.Chaos.Interfaces;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Configuration.Entities;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Host.Functions.Sap;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Sap.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The Sap Notifier.
    /// </summary>
    [TestClass]
    public class ProcessLogisticMovementTest
    {
        /// <summary>
        /// The ownership rule synchronizer.
        /// </summary>
        private ProcessLogisticMovement processLogisticMovement;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<ProcessLogisticMovement>> mockLogger;

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
        /// The mock chaos manager.
        /// </summary>
        private Mock<IChaosManager> mockChaosManager;

        /// <summary>
        /// The mock ownership rule processor.
        /// </summary>
        private Mock<ISapProcessor> mockSapProcessor;

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
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<ProcessLogisticMovement>>();
            this.mockServiceProvider = new Mock<IServiceProvider>();
            this.mockSqlTokenProvider = new Mock<ISqlTokenProvider>();
            this.mockServiceProvider.Setup(r => r.GetService(typeof(ISqlTokenProvider))).Returns(this.mockSqlTokenProvider.Object);
            this.mockSapProcessor = new Mock<ISapProcessor>();
            this.processLogisticMovement = new ProcessLogisticMovement(
                this.mockLogger.Object,
                this.mockServiceProvider.Object,
                this.mockSapProcessor.Object);
        }

        /// <summary>
        /// The Sap Notifier Test.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ProcessLogisticMovement_ProcessLogisticMovementsAsync()
        {
            this.SetupMocks(false);
            LogisticQueueMessage logisticQueueMessage = new LogisticQueueMessage() { RequestType = SapRequestType.LogisticMovement, Process = Entities.Core.StatusType.SENT };
            this.mockSapProcessor.Setup(a => a.ProcessLogisticMovementAsync(It.IsAny<LogisticQueueMessage>())).Returns(Task.CompletedTask);
            await this.processLogisticMovement.ProcessLogisticMovementsAsync(logisticQueueMessage, null, null, new Microsoft.Azure.WebJobs.ExecutionContext() { InvocationId = Guid.NewGuid() }).ConfigureAwait(false);
            this.mockSapProcessor.Verify(a => a.ProcessLogisticMovementAsync(It.IsAny<LogisticQueueMessage>()), Times.Once);
        }

        /// <summary>
        /// Setups the mocks.
        /// </summary>
        /// <param name="isReady">if set to <c>true</c> [is ready].</param>
        private void SetupMocks(bool isReady)
        {
            this.mockConnectionFactory = new Mock<IConnectionFactory>();
            this.mockConfigHandler = new Mock<IConfigurationHandler>();
            this.mockChaosManager = new Mock<IChaosManager>();
            this.mockAzureClientFactory = new Mock<IAzureClientFactory>();
            this.mockConnectionFactory.Setup(m => m.SetupStorageConnection(It.IsAny<string>()));
            this.mockConnectionFactory.SetupGet(m => m.IsReady).Returns(isReady);
            this.mockAzureClientFactory.SetupGet(m => m.IsReady).Returns(isReady);
            this.mockAzureClientFactory.Setup(m => m.Initialize(It.IsAny<AzureConfiguration>()));
            this.mockConfigHandler.Setup(m => m.GetConfigurationAsync<SqlConnectionSettings>(ConfigurationConstants.SqlConnectionSettings)).ReturnsAsync(new SqlConnectionSettings());
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IConnectionFactory))).Returns(this.mockConnectionFactory.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(ITrueLogger<SapMappingSynchronizer>))).Returns(this.mockLogger.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IConfigurationHandler))).Returns(this.mockConfigHandler.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(ISapProcessor))).Returns(this.mockSapProcessor.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IChaosManager))).Returns(this.mockChaosManager.Object);
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
