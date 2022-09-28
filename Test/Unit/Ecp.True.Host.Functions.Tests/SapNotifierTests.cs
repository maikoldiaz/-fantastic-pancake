// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapNotifierTests.cs" company="Microsoft">
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
    using Microsoft.Azure.WebJobs;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The sap notifier test class.
    /// </summary>
    [TestClass]
    public class SapNotifierTests
    {
        /// <summary>
        /// The logger mock.
        /// </summary>
        private readonly Mock<ITrueLogger<SapNotifier>> loggerMock = new Mock<ITrueLogger<SapNotifier>>();

        /// <summary>
        /// The service provider mock.
        /// </summary>
        private readonly Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();

        /// <summary>
        /// The sap processor.
        /// </summary>
        private readonly Mock<ISapProcessor> sapProcessor = new Mock<ISapProcessor>();

        /// <summary>
        /// The sap status processor.
        /// </summary>
        private readonly Mock<ISapStatusProcessor> sapStatusProcessor = new Mock<ISapStatusProcessor>();

        /// <summary>
        /// Gets the mock SQL token provider.
        /// </summary>
        /// <value>
        /// The mock SQL token provider.
        /// </value>
        private readonly Mock<ISqlTokenProvider> mockSqlTokenProvider = new Mock<ISqlTokenProvider>();

        /// <summary>
        /// The configuration handler mock.
        /// </summary>
        private readonly Mock<IConfigurationHandler> configurationHandlerMock = new Mock<IConfigurationHandler>();

        /// <summary>
        /// The mock chaos manager.
        /// </summary>
        private Mock<IChaosManager> mockChaosManager;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock unit of work factory.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The sap notifier.
        /// </summary>
        private SapNotifier sapNotifier;

        /// <summary>
        /// The system repository mock.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureClientFactory;

        /// <summary>
        /// The mock connection factory.
        /// </summary>
        private Mock<IConnectionFactory> mockConnectionFactory;

        /// <summary>
        /// Initialize the required set up.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            var connectionFactoryMock = new Mock<IConnectionFactory>();
            connectionFactoryMock.Setup(m => m.IsReady).Returns(true);
            this.mockChaosManager = new Mock<IChaosManager>();
            this.serviceProviderMock.Setup(m => m.GetService(typeof(IConnectionFactory))).Returns(connectionFactoryMock.Object);
            this.serviceProviderMock.Setup(m => m.GetService(typeof(ISqlTokenProvider))).Returns(this.mockSqlTokenProvider.Object);
            this.serviceProviderMock.Setup(r => r.GetService(typeof(IChaosManager))).Returns(this.mockChaosManager.Object);
            this.serviceProviderMock.Setup(s => s.GetService(typeof(ITrueLogger<SapNotifier>))).Returns(this.loggerMock.Object);
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockUnitOfWorkFactory.Setup(a => a.GetUnitOfWork()).Returns(this.mockUnitOfWork.Object);
        }

        /// <summary>
        /// Saps the notifier update transfer point success asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [Ignore("This Method is Deprecated")]
        [Obsolete("This Method is Deprecated", false)]
        public async Task SapNotifier_UpdateTransferPoint_SuccessAsync()
        {
            this.SetupMocks(true);
            var context = new ExecutionContext();
            var sapRequest = new SapQueueMessage() { MessageId = 1, RequestType = SapRequestType.Movement };
            this.sapProcessor.Setup(x => x.UpdateTransferPointAsync(It.IsAny<int>(), It.IsAny<int?>())).Verifiable();
            await this.sapNotifier.UpdateTransferPointAsync(sapRequest, string.Empty, string.Empty, context).ConfigureAwait(false);
            this.sapProcessor.Verify(x => x.UpdateTransferPointAsync(It.IsAny<int>(), It.IsAny<int?>()), Times.Once);
        }

        /// <summary>
        /// Saps the notifier update upload status success asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task SapNotifier_UpdateUploadStatus_SuccessAsync()
        {
            this.SetupMocks(true);
            this.mockAzureClientFactory.Setup(a => a.IsReady).Returns(false);
            var context = new ExecutionContext();
            var sapRequest = new SapQueueMessage(SapRequestType.Upload, "Test");
            this.sapStatusProcessor.Setup(x => x.TryUploadStatusAsync(It.IsAny<string>())).Verifiable();
            await this.sapNotifier.UpdateTransferPointAsync(sapRequest, string.Empty, string.Empty, context).ConfigureAwait(false);
            this.sapStatusProcessor.Verify(x => x.TryUploadStatusAsync(It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Setups the mocks.
        /// </summary>
        /// <param name="isReady">if set to <c>true</c> [is ready].</param>
        private void SetupMocks(bool isReady)
        {
            this.mockConnectionFactory = new Mock<IConnectionFactory>();
            this.mockAzureClientFactory = new Mock<IAzureClientFactory>();
            this.mockChaosManager = new Mock<IChaosManager>();

            this.mockConnectionFactory.Setup(m => m.SetupStorageConnection(It.IsAny<string>()));
            this.mockConnectionFactory.Setup(m => m.SetupSqlConfig(It.IsAny<SqlConnectionSettings>()));
            this.mockConnectionFactory.SetupGet(m => m.IsReady).Returns(isReady);

            this.mockAzureClientFactory.SetupGet(m => m.IsReady).Returns(isReady);
            this.mockAzureClientFactory.Setup(m => m.Initialize(It.IsAny<AzureConfiguration>()));

            this.serviceProviderMock.Setup(s => s.GetService(typeof(IConnectionFactory))).Returns(this.mockConnectionFactory.Object);
            this.serviceProviderMock.Setup(s => s.GetService(typeof(IConfigurationHandler))).Returns(this.configurationHandlerMock.Object);
            this.serviceProviderMock.Setup(s => s.GetService(typeof(IAzureClientFactory))).Returns(this.mockAzureClientFactory.Object);
            this.serviceProviderMock.Setup(r => r.GetService(typeof(ISqlTokenProvider))).Returns(this.mockSqlTokenProvider.Object);
            this.serviceProviderMock.Setup(s => s.GetService(typeof(IChaosManager))).Returns(this.mockChaosManager.Object);
            var analysisSettings = new AnalysisSettings
            {
                ClientId = "ClientID",
                ClientSecret = "ClientSecret",
                ModelName = "ModelName",
                Region = "Region",
                ServerName = "ServerName",
                TenantId = "TenantID",
            };
            this.configurationHandlerMock.Setup(m => m.GetConfigurationAsync<AnalysisSettings>(ConfigurationConstants.AnalysisSettings))
                .ReturnsAsync(analysisSettings);
            var storageSettings = new StorageSettings
            {
                MaxAttempts = 5,
                DeltaBackOff = 2,
                ConnectionString = "ConnectionString",
            };
            this.configurationHandlerMock.Setup(m => m.GetConfigurationAsync<StorageSettings>(ConfigurationConstants.StorageSettings))
                .ReturnsAsync(storageSettings);

            this.configurationHandlerMock.Setup(m => m.GetConfigurationAsync<ServiceBusSettings>(ConfigurationConstants.ServiceBusSettings))
           .ReturnsAsync(new ServiceBusSettings());
            this.sapNotifier = new SapNotifier(
                this.loggerMock.Object,
                this.serviceProviderMock.Object,
                this.sapStatusProcessor.Object);
        }
    }
}
