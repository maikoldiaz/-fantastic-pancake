// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportGeneratorTests.cs" company="Microsoft">
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
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Host.Functions.Reporting;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Reporting.Interfaces;
    using Microsoft.Azure.WebJobs;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The OwnershipGeneratorTests.
    /// </summary>
    [TestClass]
    public class ReportGeneratorTests
    {
        /// <summary>
        /// The report generator.
        /// </summary>
        private ReportGenerator reportGenerator;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<ITrueLogger<ReportGenerator>> mockLogger;

        /// <summary>
        /// The telemetry.
        /// </summary>
        private Mock<IServiceProvider> mockServiceProvider;

        /// <summary>
        /// The mock report processing factory.
        /// </summary>
        private Mock<IReportGeneratorFactory> mockReportProcessingFactory;

        /// <summary>
        /// The mock report processor.
        /// </summary>
        private Mock<IReportGenerator> mockReportProcessor;

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
            this.mockLogger = new Mock<ITrueLogger<ReportGenerator>>();
            this.mockServiceProvider = new Mock<IServiceProvider>();
            this.mockReportProcessingFactory = new Mock<IReportGeneratorFactory>();
            this.reportGenerator = new ReportGenerator(
                this.mockReportProcessingFactory.Object,
                this.mockLogger.Object,
                this.mockServiceProvider.Object);
        }

        /// <summary>
        /// Executes the report asynchronous should generate report asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExecuteReportAsync_ShouldGenerateReportAsync()
        {
            this.SetupMocks(true);

            this.mockReportProcessingFactory.Setup(a => a.GetReportGenerator(ReportType.BeforeCutOff)).Returns(this.mockReportProcessor.Object);
            this.mockReportProcessor.Setup(a => a.GenerateAsync(It.IsAny<int>()));

            await this.reportGenerator.GenerateBeforeCutOffAsync(1, null, null, new ExecutionContext() { InvocationId = Guid.NewGuid() }).ConfigureAwait(false);

            this.mockReportProcessingFactory.Verify(a => a.GetReportGenerator(ReportType.BeforeCutOff), Times.Once);
            this.mockReportProcessor.Verify(a => a.GenerateAsync(1), Times.Once);
        }

        /// <summary>
        /// Executes the report User groups And Permissions asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GenerateUserRolesAndPermissions_ExecuteReportAsync()
        {
            this.SetupMocks(true);

            this.mockReportProcessingFactory.Setup(a => a.GetReportGenerator(ReportType.UserRolesAndPermissions)).Returns(this.mockReportProcessor.Object);
            this.mockReportProcessor.Setup(a => a.GenerateAsync(It.IsAny<int>()));

            await this.reportGenerator.GenerateUserRolesAndPermissionsAsync(1, null, null, new ExecutionContext { InvocationId = Guid.NewGuid() }).ConfigureAwait(false);

            this.mockReportProcessingFactory.Verify(a => a.GetReportGenerator(ReportType.UserRolesAndPermissions), Times.Once);
            this.mockReportProcessor.Verify(a => a.GenerateAsync(1), Times.Once);
        }

        /// <summary>
        /// Executes the report asynchronous should generate report asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ExecuteReportAsync_ShouldGenerateOfficialInitialBalance_ReportAsync()
        {
            this.SetupMocks(true);

            this.mockReportProcessingFactory.Setup(a => a.GetReportGenerator(ReportType.OfficialInitialBalance)).Returns(this.mockReportProcessor.Object);
            this.mockReportProcessor.Setup(a => a.GenerateAsync(It.IsAny<int>()));

            await this.reportGenerator.GenerateOfficialInitialBalanceAsync(1, null, null, new ExecutionContext() { InvocationId = Guid.NewGuid() }).ConfigureAwait(false);

            this.mockReportProcessingFactory.Verify(a => a.GetReportGenerator(ReportType.OfficialInitialBalance), Times.Once);
            this.mockReportProcessor.Verify(a => a.GenerateAsync(1), Times.Once);
        }

        [TestMethod]
        public async Task GenerateSendToSapAsync_ShouldGenerateSapBalanceReport_ReportAsync()
        {
            this.SetupMocks(true);

            this.mockReportProcessingFactory.Setup(a => a.GetReportGenerator(ReportType.SapBalance)).Returns(this.mockReportProcessor.Object);
            this.mockReportProcessor.Setup(a => a.GenerateAsync(It.IsAny<int>()));

            await this.reportGenerator.GenerateSendToSapAsync(1, null, null, new ExecutionContext { InvocationId = Guid.NewGuid() }).ConfigureAwait(false);

            this.mockReportProcessingFactory.Verify(a => a.GetReportGenerator(ReportType.SapBalance), Times.Once);
            this.mockReportProcessor.Verify(a => a.GenerateAsync(1), Times.Once);
        }

        /// <summary>
        /// Setups the mocks.
        /// </summary>
        /// <param name="isReady">if set to <c>true</c> [is ready].</param>
        private void SetupMocks(bool isReady)
        {
            this.mockConnectionFactory = new Mock<IConnectionFactory>();
            this.mockConfigHandler = new Mock<IConfigurationHandler>();
            this.mockSqlTokenProvider = new Mock<ISqlTokenProvider>();
            this.mockChaosManager = new Mock<IChaosManager>();
            this.mockReportProcessor = new Mock<IReportGenerator>();

            this.mockConnectionFactory.Setup(m => m.SetupStorageConnection(It.IsAny<string>()));
            this.mockConnectionFactory.SetupGet(m => m.IsReady).Returns(isReady);

            this.mockServiceProvider.Setup(s => s.GetService(typeof(IConnectionFactory))).Returns(this.mockConnectionFactory.Object);

            this.mockServiceProvider.Setup(s => s.GetService(typeof(IConfigurationHandler))).Returns(this.mockConfigHandler.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IReportGeneratorFactory))).Returns(this.mockReportProcessingFactory.Object);
            this.mockServiceProvider.Setup(s => s.GetService(typeof(IReportGenerator))).Returns(this.mockReportProcessor.Object);
            this.mockServiceProvider.Setup(r => r.GetService(typeof(ISqlTokenProvider))).Returns(this.mockSqlTokenProvider.Object);
            this.mockServiceProvider.Setup(r => r.GetService(typeof(IChaosManager))).Returns(this.mockChaosManager.Object);
        }
    }
}
