// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainReconcilerTests.cs" company="Microsoft">
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
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Core.Dto;
    using Ecp.True.Host.Functions.Deadletter;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Deadletter.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Timers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The DeadletterGeneratorTests.
    /// </summary>
    [TestClass]
    public class BlockchainReconcilerTests
    {
        /// <summary>
        /// The Deadletter Generator.
        /// </summary>
        private BlockchainReconciler reconciler;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private Mock<IServiceProvider> mockServiceProvider;

        /// <summary>
        /// The reconcile service.
        /// </summary>
        private Mock<IReconcileService> mockReconciler;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<BlockchainReconciler>> mockLogger;

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureClientFactory;

        /// <summary>
        /// The mock connection factory.
        /// </summary>
        private Mock<IConnectionFactory> mockConnectionFactory;

        /// <summary>
        /// The mock configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigHandler;

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
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<BlockchainReconciler>>();
            this.mockServiceProvider = new Mock<IServiceProvider>();
            this.mockServiceProvider.Setup(x => x.GetService(It.IsAny<Type>()));
            this.mockReconciler = new Mock<IReconcileService>();
            this.mockReconciler.Setup(m => m.ReconcileAsync(It.IsAny<OffchainMessage>()));
            this.mockReconciler.Setup(m => m.ReconcileAsync());
            this.reconciler = new BlockchainReconciler(
                this.mockServiceProvider.Object,
                this.mockReconciler.Object,
                this.mockLogger.Object);
        }

        /// <summary>
        /// Scheduleds the records reconciliation null timer information should throw exception asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ScheduledRecordsReconciliation_NullTimerInfo_ShouldThrowExceptionAsync()
        {
            this.SetupMocks(true);
            await this.reconciler.ReconcileAsync(null, new ExecutionContext()).ConfigureAwait(false);
        }

        /// <summary>
        /// Scheduleds the records reconciliation should be suceessfull asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ScheduledRecordsReconciliation_ShouldBeSuceessfullAsync()
        {
            this.SetupMocks(true);
            var timerInfo = new TimerInfo(null, new ScheduleStatus(), false);
            await this.reconciler.ReconcileAsync(timerInfo, new ExecutionContext()).ConfigureAwait(false);
            this.mockReconciler.Verify(a => a.ReconcileAsync(), Times.Once);
        }

        /// <summary>
        /// Scheduleds the records reconciliation should be suceessfull asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task RecordsReconciliation_ShouldReconcile_WhenInvokedAsync()
        {
            this.SetupMocks(true);
            var message = new OffchainMessage { Type = ServiceType.Node };

            await this.reconciler.ReconcileOffchainAsync(message, "label", new ExecutionContext()).ConfigureAwait(false);
            this.mockReconciler.Verify(a => a.ReconcileAsync(It.IsAny<OffchainMessage>()), Times.Once);
        }

        /// <summary>
        /// Setups the mocks.
        /// </summary>
        /// <param name="isReady">if set to <c>true</c> [is ready].</param>
        private void SetupMocks(bool isReady)
        {
            this.mockConnectionFactory = new Mock<IConnectionFactory>();
            this.mockConfigHandler = new Mock<IConfigurationHandler>();
            this.mockAzureClientFactory = new Mock<IAzureClientFactory>();
            this.mockSqlTokenProvider = new Mock<ISqlTokenProvider>();
            this.mockChaosManager = new Mock<IChaosManager>();

            this.mockConnectionFactory.Setup(m => m.SetupStorageConnection(It.IsAny<string>()));
            this.mockConnectionFactory.SetupGet(m => m.IsReady).Returns(isReady);

            this.mockAzureClientFactory.SetupGet(m => m.IsReady).Returns(isReady);
            this.mockAzureClientFactory.Setup(m => m.Initialize(It.IsAny<AzureConfiguration>()));

            this.mockServiceProvider.Setup(s => s.GetService(typeof(IConnectionFactory))).Returns(this.mockConnectionFactory.Object);

            this.mockServiceProvider.Setup(s => s.GetService(typeof(IConfigurationHandler))).Returns(this.mockConfigHandler.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IAzureClientFactory))).Returns(this.mockAzureClientFactory.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IReconciler))).Returns(this.mockReconciler.Object);
            this.mockServiceProvider.Setup(r => r.GetService(typeof(ISqlTokenProvider))).Returns(this.mockSqlTokenProvider.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IChaosManager))).Returns(this.mockChaosManager.Object);
        }
    }
}