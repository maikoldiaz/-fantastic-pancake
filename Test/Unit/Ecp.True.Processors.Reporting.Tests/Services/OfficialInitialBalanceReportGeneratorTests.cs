// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialInitialBalanceReportGeneratorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Reporting.Tests.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Reporting.Services;
    using Ecp.True.Processors.Reporting.Tests.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The OfficialInitialBalanceReportGeneratorTests.
    /// </summary>
    [TestClass]
    public class OfficialInitialBalanceReportGeneratorTests : ReportGeneratorTests
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private OfficialInitialBalanceReportGenerator processor;

        /// <summary>
        /// The token.
        /// </summary>
        private CancellationToken token;

        /// <summary>
        /// The unit of work mock.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The logistics node repo mock.
        /// </summary>
        private Mock<IRepository<ReportExecution>> repositoryMock;

        /// <summary>
        /// The logistics node repo mock.
        /// </summary>
        private Mock<IRepository<InventoryMovementIndex>> inventoryMovementIndexrepositoryMock;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<OfficialInitialBalanceReportGenerator>> mockLogger;

        /// <summary>
        /// The mock configuration.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfiguration;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockLogger = new Mock<ITrueLogger<OfficialInitialBalanceReportGenerator>>();
            this.token = new CancellationToken(false);
            this.repositoryMock = new Mock<IRepository<ReportExecution>>();
            this.inventoryMovementIndexrepositoryMock = new Mock<IRepository<InventoryMovementIndex>>();
            this.mockConfiguration = new Mock<IConfigurationHandler>();

            this.repositoryMock.Setup(m => m.Update(It.IsAny<ReportExecution>()));
            this.repositoryMock.Setup(m => m.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()));
            this.inventoryMovementIndexrepositoryMock.Setup(m => m.ExecuteAsync(It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()));
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<ReportExecution>()).Returns(this.repositoryMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<InventoryMovementIndex>()).Returns(this.inventoryMovementIndexrepositoryMock.Object);

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(this.token));
            this.processor = new OfficialInitialBalanceReportGenerator(this.mockUnitOfWorkFactory.Object, this.mockConfiguration.Object, this.mockLogger.Object);
        }

        /// <summary>
        /// Generates the report asynchronous should generate report and update status when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GenerateReportAsync_ShouldGenerateReportAndUpdateStatus_WhenInvokedAsync()
        {
            var executionId = 1;
            var entity = new ReportExecution
            {
                ExecutionId = executionId,
                CategoryId = 10,
                Category = new Category
                {
                    CategoryId = 10,
                    Name = "Segmento",
                },
            };

            this.SetUpRepositoryMock(this.repositoryMock, entity);

            await this.processor.GenerateAsync(1).ConfigureAwait(false);

            this.VerifyRepositoryMock(this.repositoryMock, StatusType.PROCESSED);
        }

        /// <summary>
        /// Purges the should purge records based on configuration settings.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Purge_ShouldPurgeRecords_BasedOnConfigurationSettingsAsync()
        {
            this.mockConfiguration.Setup(m => m.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings)).ReturnsAsync(new SystemSettings { ReportsCleanupDurationInHours = 12 });

            await this.processor.PurgeReportHistoryAsync().ConfigureAwait(false);

            this.mockConfiguration.Verify(m => m.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings), Times.Once);
        }

        /// <summary>
        /// Officials the type of the node balance report generator should return the report.
        /// </summary>
        [TestMethod]
        public void Generator_ShouldReturnTheReportType()
        {
            Assert.AreEqual(ReportType.OfficialInitialBalance, this.processor.Type);
        }
    }
}
