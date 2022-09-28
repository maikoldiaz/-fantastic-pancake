// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipSchedulerTests.cs" company="Microsoft">
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
    using Ecp.True.Host.Functions.Ownership;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Timers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class OwnershipSchedulerTests
    {
        /// <summary>
        /// The ownership rule synchronizer.
        /// </summary>
        private Scheduler ownershipScheduler;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<Scheduler>> mockLogger;

        /// <summary>
        /// The mock ownership rule processor.
        /// </summary>
        private Mock<IOwnershipRuleProcessor> mockOwnershipRuleProcessor;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private Mock<IAzureClientFactory> mockAzureClientFactory;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private Mock<IServiceProvider> mockServiceProvider;

        /// <summary>
        /// Gets the mock SQL token provider.
        /// </summary>
        /// <value>
        /// The mock SQL token provider.
        /// </value>
        private Mock<ISqlTokenProvider> mockSqlTokenProvider;

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
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<Scheduler>>();
            this.mockServiceProvider = new Mock<IServiceProvider>();
            this.mockSqlTokenProvider = new Mock<ISqlTokenProvider>();
            this.mockServiceProvider.Setup(r => r.GetService(typeof(ISqlTokenProvider))).Returns(this.mockSqlTokenProvider.Object);
            this.mockOwnershipRuleProcessor = new Mock<IOwnershipRuleProcessor>();
            this.mockAzureClientFactory = new Mock<IAzureClientFactory>();
            this.ownershipScheduler = new Scheduler(
                this.mockLogger.Object,
                this.mockServiceProvider.Object,
                this.mockOwnershipRuleProcessor.Object,
                this.mockAzureClientFactory.Object);
        }

        [TestMethod]
        public async Task ScheduledSyncOwnershipRulesAsync_ShouldSyncOwnershipRulesAsync()
        {
            this.SetupMocks(true);
            this.mockOwnershipRuleProcessor.Setup(a => a.QueueSyncOwnershipRuleAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            var timerInfo = new TimerInfo(null, new ScheduleStatus(), false);
            await this.ownershipScheduler.ScheduledSyncOwnershipRulesAsync(timerInfo, new ExecutionContext() { InvocationId = Guid.NewGuid() }).ConfigureAwait(false);

            this.mockOwnershipRuleProcessor.Verify(a => a.QueueSyncOwnershipRuleAsync("System"), Times.Once);
        }

        [TestMethod]
        public async Task ScheduledSyncOwnershipRulesAsync_Should_Handle_ExceptionsAsync()
        {
            this.SetupMocks(true);
            this.mockOwnershipRuleProcessor.Setup(a => a.QueueSyncOwnershipRuleAsync(It.IsAny<string>())).ThrowsAsync(new Exception());

            var timerInfo = new TimerInfo(null, new ScheduleStatus(), false);
            await this.ownershipScheduler.ScheduledSyncOwnershipRulesAsync(timerInfo, new ExecutionContext() { InvocationId = Guid.NewGuid() }).ConfigureAwait(false);

            this.mockOwnershipRuleProcessor.Verify(a => a.QueueSyncOwnershipRuleAsync("System"), Times.Once);
        }

        [TestMethod]
        public async Task ScheduledSyncAuditReportsAsync_ShouldSyncAuditReportssAsync()
        {
            this.SetupMocks(true);
            this.mockAzureClientFactory.Setup(x => x.AnalysisServiceClient.RefreshAuditReportsAsync()).Returns(Task.CompletedTask);

            var timerInfo = new TimerInfo(null, new ScheduleStatus(), false);
            await this.ownershipScheduler.ScheduledSyncAuditReportsAsync(timerInfo, new ExecutionContext() { InvocationId = Guid.NewGuid() }).ConfigureAwait(false);

            this.mockAzureClientFactory.Verify(x => x.AnalysisServiceClient.RefreshAuditReportsAsync(), Times.Once);
        }

        [TestMethod]
        public async Task ScheduledSyncAuditReportsAsyncc_Should_Handle_ExceptionsAsync()
        {
            this.SetupMocks(true);
            this.mockAzureClientFactory.Setup(x => x.AnalysisServiceClient.RefreshAuditReportsAsync()).ThrowsAsync(new Exception());

            var timerInfo = new TimerInfo(null, new ScheduleStatus(), false);
            await this.ownershipScheduler.ScheduledSyncAuditReportsAsync(timerInfo, new ExecutionContext() { InvocationId = Guid.NewGuid() }).ConfigureAwait(false);

            this.mockAzureClientFactory.Verify(x => x.AnalysisServiceClient.RefreshAuditReportsAsync(), Times.Once);
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

            this.mockConnectionFactory.Setup(m => m.SetupStorageConnection(It.IsAny<string>()));
            this.mockConnectionFactory.SetupGet(m => m.IsReady).Returns(isReady);

            this.mockServiceProvider.Setup(s => s.GetService(typeof(IConnectionFactory))).Returns(this.mockConnectionFactory.Object);

            this.mockServiceProvider.Setup(s => s.GetService(typeof(ITrueLogger<Scheduler>))).Returns(this.mockLogger.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IConfigurationHandler))).Returns(this.mockConfigHandler.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IOwnershipRuleProcessor))).Returns(this.mockOwnershipRuleProcessor.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IChaosManager))).Returns(this.mockChaosManager.Object);
        }
    }
}
